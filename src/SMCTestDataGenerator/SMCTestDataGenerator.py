import time
from random import randint
import paho.mqtt.client as mqtt
import argparse
from uuid import uuid4
import sys
import math

n_sensors = 0
n_messages_published = 0
topic = ""
mqtt_client = mqtt.Client(str(uuid4()))
is_connected = False
degrees_in_radians = 0.0
degrees_per_tick = 3.0
circlesize = 5.0
oldpoint = [0.0, 1.0]

def generateDataEntry(sensorID):
    degrees_in_radians += 2.0 * math.pi * degrees_per_tick / 360.0
    newpoint = [circlesize * math.sin(degrees_in_radians) , circlesize * math.cos(degrees_in_radians)]
    diff = [newpoint[0]-oldpoint[0], newpoint[1]-oldpoint[1]]
    oldpoint = newpoint
    return f"""[{{
    "version": "2.0",
    "tagId": "{sensorID}",
    "timestamp": {time.time()},
    "data": {{
        "coordinates": {{
            "x": {oldpoint[0]},
            "y": {oldpoint[1]},
            "z": {0}
        }},
        "velocity": {{
            "x": {diff[0]},
            "y": {diff[1]},
            "z": {0}
        }},
        "acceleration": {{
            "x": {- math.sin(degrees_in_radians)},
            "y": {- math.cos(degrees_in_radians)},
            "z": {0}
        }},

        "tagData": {{
            "blinkIndex": 226,
            "accelerometer": [
                [{- math.sin(degrees_in_radians)}, {- math.cos(degrees_in_radians)}, {0}]
            ]
        }},
    }},
    "success": true
    }}]
    """

def main():
    parser = argparse.ArgumentParser(description='Generate test data.')
    parser.add_argument('-s', "--server", help="MQTT Server", required=True)
    parser.add_argument('-u', "--username", help="MQTT Username", type=str, default="")
    parser.add_argument('-P', "--password", help="MQTT Username", type=str, default="")
    parser.add_argument('-t', "--topic", help="MQTT topic", type=str, default="tags")
    parser.add_argument('-n', "--sensors", help="Amount of sensors to generate data for", type=int, default=4)
    parser.add_argument('-p', "--port", help="Amount of sensors to generate data for", type=int, default=8883)
    parser.add_argument("--datarate", help="Rate of data (in Hz)", type=float, default=1)
    
    args = parser.parse_args(sys.argv[1:])

    global n_sensors
    n_sensors = args.sensors
    global topic
    topic = args.topic

    global mqtt_client
    mqtt_client.username_pw_set(args.username, args.password)
    mqtt_client.connect(args.server, args.port)
    while True:
        for i in range(n_sensors):
            print(f"publishing {i}")
            mqtt_client.publish(topic, generateDataEntry(i))
            mqtt_client.loop()
        time.sleep(1 / args.datarate)

if __name__ == '__main__':
    main()
