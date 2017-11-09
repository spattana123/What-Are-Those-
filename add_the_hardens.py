import sqlite3

db = sqlite3.connect("WhatAreThose.db")
cur = db.cursor()

cur.execute("""INSERT INTO justimgTest('images') VALUES('https://images.duckduckgo.com/iu/?u=https%3A%2F%2Fcdn5.kicksonfire.com%2Fwp-content%2Fuploads%2F2017%2F05%2FNike-Air-VaporMax-College-Navy-5-1.png%3Fx77385&f=1')""")

cur.execute("""INSERT INTO justlnk('link') VALUES('https://cdn5.kicksonfire.com/wp-content/uploads/2017/05/Nike-Air-VaporMax-College-Navy-5-1.png?x77385')""")
db.commit()
db.close()

