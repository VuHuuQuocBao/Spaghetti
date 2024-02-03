using StackExchange.Redis;




ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost:6379");
var db = redis.GetDatabase();
var endpoints = redis.GetEndPoints();
var server = redis.GetServer(endpoints.First());
var keys = server.Keys().ToList();
var keyCount = keys.Count();



var a = 1;