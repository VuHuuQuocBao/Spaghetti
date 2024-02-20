# Spaghetti

![image](https://github.com/VuHuuQuocBao/Spaghetti/assets/96562872/415cd112-9d0f-4f9f-867e-5fe5153183bd)


- Kafka, redis, and ES are running as a single cluster with just 1 node, have to deploy cluster with multiple nodes and simulate the parallel processing in the future
- May need to add MongoDB as persistent layer. Deploy as multiple nodes cluster and need to verify bucket hashing mechanism of MongoDB
- support for media file, search metadata of media file and host to S3


- Current:
    - wrap fetch, push and delete keys in redis in a transaction
    - deploy multi nodes redis cluster and distribute write using hashSlot
    - multi process redis 


- Struggle:
    - Don't understand why trying to connect to the cluster using StackExchange by specifying all nodes in the cluster, try to set or get a key, it's like we have to connect to that specific node that holds the key to perform the opeartion, or else will encounter a timeout. Really don't know how to overcome this issue ?
