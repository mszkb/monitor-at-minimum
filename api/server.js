const jsonServer = require('json-server')
const server = jsonServer.create()
const router = jsonServer.router('db.json')
// const swaggerUi = require('swagger-ui-express')
// const swaggerFile = require('./swagger_output.json')
const middlewares = jsonServer.defaults()

const axios = require('axios')
axios.interceptors.request.use(function (config) {
 
  config.metadata = { startTime: new Date()}
  return config;
}, function (error) {
  return Promise.reject(error);
});
axios.interceptors.response.use(function (response) { 
  response.config.metadata.endTime = new Date()
  response.duration = response.config.metadata.endTime - response.config.metadata.startTime
  return response;
}, function (error) {
  error.config.metadata.endTime = new Date();
  error.duration = error.config.metadata.endTime - error.config.metadata.startTime;
  return Promise.reject(error);
});
 
const port = process.env.PORT || 3001

// Set default middlewares (logger, static, cors and no-cache)
server.use(middlewares)

// Add custom routes before JSON Server router
server.get('/echo', (req, res) => {
  res.jsonp(req.query)
})

server.get('/list', (req, res) => {
    const list = router.db.get('sites')
    res.json(list)
})

server.post('/update', (req, res) => {
    const list = router.db.get('sites').value()

    const meta = router.db.get('meta').value()
    meta.checkStarted = Date.now()
        
    const updatedList = list.map(async item => {
        console.log(`Checking ${item.url}...`)
        
        const url = item.url
        const response = await axios.get(url)

        const isReachable = response.status === 200
    
        item.lastChecked = Date.now()
        item.status = isReachable ? 'up' : 'down'
        item.ping = response.duration

        return item
    })

    Promise.all(updatedList).then(() => {
        router.db.set('sites', list).write()
        meta.total = list.length
        meta.checkEnded = Date.now()
        res.json(list)
    })
})

// To handle POST, PUT and PATCH you need to use a body-parser
// You can use the one used by JSON Server
server.use(jsonServer.bodyParser)
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