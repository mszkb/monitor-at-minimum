# Monitor at minimum

Like Prometheus but small and easy to use.

The backend is small python code with sqlite. The main goal is having this run smoothly on low performance IoT devices.

## Running tasks

You can add tasks to your device with a specific interval. That tasks should always call back to the shard.

Minimum interval for tasks is 1min - due to crontab limitations.
You want to have less then 1min, then some workaround is needed.

## Webhooks

Webhooks are special tasks that do not run with an interval. They are just listeners for outside events.
Means you have to setup your script/task you want to monitor to send HTTP requests with predefined properties.

## Roadmap

- Horizontal scaling for better business use cases (managing devices in your space accross multiple shards - see wiki)