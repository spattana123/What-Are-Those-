import scrapy
from scrapy import Selector
class FightScraper(scrapy.Spider):
	name = "fightme"
	start_urls = "https://www.flightclub.com/footwear"

	def parse(self,response):
		for images in response.css('.category-products'):
			yield{
				'images': response.css('img').xpath('@src').extract()
			}
			

