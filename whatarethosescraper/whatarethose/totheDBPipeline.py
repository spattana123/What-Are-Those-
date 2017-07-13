import sqlite3

from scrapy.conf import settings
from scrapy.exceptions import DropItem

class TotheDBPipline(object):
   
   def open_spider(self,spider):
       self.file = sqlite3.connect('Whatarethose.db')
       
       
   def proccess_item(self,item,spider):
       conn = sqlite3.connect('Whatarethose.db')
       c = conn.cursor() 
       c.execute("CREATE images IF NOT EXISTS(id REAL,image BLOB, url TEXT)")
       c.execute("INSERT INTO images VALUES('%s,%s')" % (item['files_urls'],item['files']))
       conn.commit()
       conn.close()
       