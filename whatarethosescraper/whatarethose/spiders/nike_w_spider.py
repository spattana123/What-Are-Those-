from whatarethose.items import WhatarethoseItem
import scrapy

class NikeWSPider(scrapy.Spider):
	name = "nike-women"
	start_urls = ['https://store.nike.com/us/en_us/pw/womens-shoes/7ptZoi3']

	def parse(self,response):
		for item in response.css('.grid-item-image'):
			yield{
				#'images': response.css('img').xpath('@src').extract()
				'links': response.css('img').xpath('@src').extract()
			}

