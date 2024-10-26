# Wiki

## Device

The thing were your monitoring is running.

The device can be used to execute tasks (like scripts) or receiving events (calls from the outside).

## Space

A collection of all your devices. Like a broker that collects and distributes the data accross the shards, so everyone is up to date.
The main responsibility of the space is making shards aware of changes.

A space holds no information about the devices, just about the shards. The space is like a website with all available tracker from the bittorrent protocol.

## Shard / Hub

A group of devices sharing work. If you do not want to have more than one group that your shard is 
as big as the space.
The shard is a hub for a group of devices that reports back about their work. It contain all the information that the user wants to see from the devices.

Having multiple shards is completly optional, but it makes horizontal scaling possible and more managable. You can have multiple servers running different shards, that syncs via the space. Think about like a tracker in the Bittorrent protocol.

## Main hub

