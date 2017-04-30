/*
this is our web server. It uses express.js to handle the get and 
post request. The client will send a json object of an image and I will 
take that image and send it to the cloudinary api.
*/
var http = require('http');
var express = require('express');
var net = require('net');
var app = express();

var client = new net.Socket();
app.get('/',function(res,req){
	req.send("<h1>Hello World!</h1>");
});

app.post('/',function(res,req){
	console.log(req.body);
	var content = JSON.stringify(req);
	//loops through the json string
	var pos = content.search(":");
	var string = content.substring(pos+1,content.length);
	console.log(string);
});
app.listen(process.env.PORT,function(){
	console.log("Server is working!");
});

client.connect(process.env.PORT,'127.0.0.1',function(){
	console.log("client has connected");
});