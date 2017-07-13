from whatarethose.items import WhatarethoseItem
import scrapy
import sqlite3

class NikeSpider(scrapy.Spider):
	name = 'nike-men'

	start_urls = ['http://store.nike.com/us/en_us/pw/mens-shoes/7puZoi3?ipp=120']

	def parse(self,response):
		conn = sqlite3.connect('WhatAreThose.db')
		c = conn.cursor()
		for images in response.css(".grid-item-image"):
			yield{
				'images': response.css('img').xpath('@src').extract()
				#'links': response.css('a').xpath('@href').extract()
			}
