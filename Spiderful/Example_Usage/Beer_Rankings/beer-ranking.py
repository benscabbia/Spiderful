#import urllib.request
import requests
import collections
import sys
from collections import OrderedDict

fobj = open("beer-list.txt", "r")

beers = []

#get beers from file
for line in fobj:
    beers.append(line.rstrip())
fobj.close()

baseUrlStart = "http://localhost:61363/api/sitemap?url=https://untappd.com/search?q="
baseUrlEnd = "&selector=//span[@class='num']"
beersAndRatings = {}
#generate url and make request for beer rating
for beer in beers:
    fullUrl = baseUrlStart + beer + baseUrlEnd
    r = requests.get(fullUrl)
    beersAndRatings[beer] = r.json()[1:-1]
    #print(rating) 
od = OrderedDict(sorted(beersAndRatings.items(), reverse=True, key=lambda t: t[1]))

#create a file with ordered beer ratings
file = open("MarketBeers.txt",'w')  
for key in od:
    #if od[key] == "": continue #uncomment to skip unrated beers
    file.write(od[key]+ " : " + key + "\n")
file.close()
print("Completed.")


