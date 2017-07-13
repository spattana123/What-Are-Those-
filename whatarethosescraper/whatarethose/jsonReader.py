import json
import sqlite3
#open a connection to the data base
connection = sqlite3.connect('WhatAreThose.db')
cur = connection.cursor()
#open the json file to start reading it

#execute the insert in to my reading the images and links keys and storing there values
with open("justLinks.json") as json_file:
	d = json.load(json_file)
	for key in d:
		for k,v in key.iteritems():
			for link in v:
				cur.execute("""INSERT INTO justlnk VALUES(?)""",(link,))

connection.commit()
connection.close()

