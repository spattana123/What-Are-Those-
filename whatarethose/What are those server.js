var http = require('http');
var express = require('express');
var net = require('net');
var app = express();

var client = new net.Socket();
app.get('/',function(res,req){
	req.send("<h1>Hello World!</h1>");
});

app.post('/',function(res,req){
	console.log("json sent");
	req.send("json sent");
});
app.listen(3000,function(){
	console.log("Server is working!");
});

client.connect(3000,'127.0.0.1',function(){
	console.log("client has connected");
});