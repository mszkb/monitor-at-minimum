import pkg from 'json-server';
const { create, router: _router, defaults, bodyParser } = pkg;
import { usePingTask } from './tasks/ping/ping.js'
import { $http } from './helper/axios.js'
import cron from 'node-cron'
const server = create()
const router = _router('db.json')
// const swaggerUi = require('swagger-ui-express')
// const swaggerFile = require('./swagger_output.json')
const middlewares = defaults()
 
const port = process.env.PORT || 3001

// Set default middlewares (logger, static, cors and no-cache)
server.use(middlewares)

// Add custom routes before JSON Server router
server.get('/echo', (req, res) => {
  res.jsonp(req.query)
})

server.get('/list', (_, res) => {
    const list = router.db.get('sites')
    res.json(list)
})

server.get('/meta', (_, res) => {
    const meta = router.db.get('meta')
    res.json(meta)
});

server.delete('/delete/:id', (req, res) => {
    if (!req.params.id) {
        return res.status(400).json({ message: 'Missing id' })
    }

    const id = parseInt(req.params.id)

    if (isNaN(id)) {
        return res.status(400).json({ message: 'Invalid id' })
    }

    const list = router.db.get('sites').value()
    const updatedList = list.filter(item => item.id !== id)
    router.db.set('sites', updatedList).write()

    if (list.length === updatedList.length) {
        return res.status(404).json({ message: 'Site not found' })
    }

    res.status(200).json({ message: 'Site deleted successfully' })
})

server.post('/update', async (_, res) => {
    const list = await doWork()
    updateList(list);
    res.json(list)
})

function updateList(list) {
  router.db.set('sites', list).write()

  const meta = router.db.get('meta').value()
  meta.total = list.length
  meta.checkEnded = Date.now()
  router.db.set('meta', meta).write()
}

async function doWork() {
  const list = router.db.get('sites').value()
  const meta = router.db.get('meta').value()

  meta.checkStarted = Date.now()

  const pingTask = usePingTask($http);
  
  const updatedList = list.map(async item => {
      console.log(`Checking ${item.url}...`)
      
      const url = item.url
      const response = await pingTask.start(url)

      const isReachable = response.status === 200
  
      item.lastChecked = Date.now()
      item.status = isReachable ? 'up' : 'down'
      item.ping = response.duration

      return item
  })

  const d = await Promise.all(updatedList)
  
  meta.checkEnded = Date.now()
  router.db.set('meta', meta).write();
  return d;
}

function stopInverval() {
  const meta = router.db.get('meta').value()
  meta.intervalActive = false
  router.db.set('meta', meta).write()

  if (backgroundTasks) {
    backgroundTasks.stop()
    console.log('Interval stopped')   
  }
}

function enableInterval() {
  const meta = router.db.get('meta').value()
  meta.intervalActive = true
  router.db.set('meta', meta).write()

  let running = false;
  backgroundTasks = cron.schedule('* * * * * *', async () => {
    console.log("Running background task")
      if (running) {
        console.log('Task already running...')
        return;
      }

      running = true;
      try {
        console.log('Running ping task...')
        const results = await doWork();
        updateList(results);
        console.log('Finished ping task...')
      } catch (e) {
        console.error(e)
        stopInverval();
      }

      running = false;
  })
}

let backgroundTasks = null;
server.post('/ping', (_, res) => {
  const meta = router.db.get('meta').value()
  if (meta.intervalActive) {
    stopInverval();
    res.json({ message: 'Interval disabled' })
    return;
  }

  

  enableInterval();

  // console.log('Interval enabled')
  res.json({ message: 'Interval enabled' })
})

// To handle POST, PUT and PATCH you need to use a body-parser
// You can use the one used by JSON Server
server.use(bodyParser)
server.use((req, res, next) => {
  if (req.method === 'POST') {
    req.body.createdAt = Date.now()
  }
  // Continue to JSON Server router
  next()
})

// server.use('/doc', swaggerUi.serve, swaggerUi.setup(swaggerFile))

// Use default router
server.use(router)
server.listen(port, () => {
  console.log(`JSON Server is running on port ${port}`)
})