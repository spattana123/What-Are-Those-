/*
this is our web server. It uses express.js to handle the get and 
post request. The client will send a json object of an image and I will 
take that image and send it to the cloudinary api.
*/
var http = require('http');
var express = require('express');
var net = require('net');
var app = express();
var bodyParser = require('body-parser');
var cloud = require('cloudinary');
var fs = require('fs');
var busboy = require('connect-busboy');
//cloudinary acournt 
cloud.config({
	cloud_name:'dtludbb6q',
	api_key:'148366515357314',
	api_secret: '_XoXFClQ8v6nRIDekP85spE3U-A'

});

app.use(busboy);

var client = new net.Socket();
app.get('/',function(res,req){
	req.send("<h1>Hello World!</h1>");
});

app.post('/',function(res,req){
//	console.log(req);
  var stream = cloud.uploader.upload_stream(function(result) { console.log(result) });
var file_reader = fs.createReadStream(req.body).pipe(stream)

});
app.listen(process.env.PORT,function(){
	console.log("Server is working!");
});

client.connect(process.env.PORT,'10.105.42.230',function(){
	console.log("client has connected");
});