#! /usr/bin/env python
from wimd2 import WIMD2
from pandas import Timestamp
from datetime import datetime, timedelta
import pygal
from pygal.style import NeonStyle
from pygal import Config

try:
    w = WIMD2("lora@nomail.com", "lorademo", True)
except Exception as error:
    print("Login or network error - " + str(error))
    exit()

hours_behind = 0
hours_in_chart = 24
p = '%Y-%m-%d %H:%M:%S'
p2 = '%Y-%m-%d'
p3 = '%Y-%m-%d %H:%M'
ed = datetime.now() - timedelta(hours=hours_behind)
sd = ed - timedelta(hours=hours_in_chart)
period = 'hour'
function = 'min'

series = []
places = w.readPlaces()
if not places.success:
    print("Error reading places")
    exit()
for place in places.data:
    things = w.readThings(place['id'])
    if not things.success:
        print("Error reading things")
        exit()
    for thing in things.data:
        sensors = w.listSensors(thing['id'])
        if not sensors.success:
            print("Error reading sensor")
            exit()
        for sensor in sensors.data:
            if sensor['usage'].lower().find("comfort") >= 0:
                series.append({"name":sensor['name'], "id":sensor['id']})

early_sample = datetime(2000,1,1)
if len(series) > 0:
    config=Config()
    config.style = NeonStyle
    config.show_dots = False
    config.interpolate='cubic'
    chart = pygal.Radar(config)
    x_lables = []
    first_serie = 1
    series_id = ""
    for s in series:
        if series_id != "":
            series_id += ","
        series_id += s['id']
    h = w.readCleanMultipleData(series_id, sd, ed, function, period)
    if h.success:
        idx = 0
        for serie in h.data:
            x = []
            y = []
            for dp in serie['values']:
                ts = Timestamp(dp[0])
                if ts > early_sample:
                    early_sample = ts
                x.append(ts)
                y.append(dp[1])
                if first_serie:
                    x_lables.append(ts)
            chart.add(series[idx]['name'], y)
            idx += 1
            first_serie = 0
    chart.x_labels = map(lambda d: d.strftime('%H:%M'),x_lables)
    chart.title = "Comfort Index - Generated at: " + datetime.now().strftime(p3) + "\nLast Acquisition: " + early_sample.strftime(p3)
    #chart.render_to_file('/var/wimd/comfort.svg')
    chart.render_to_file('/tmp/comfort.svg')
    
