import time
from random import randint
import paho.mqtt.client as mqtt
import argparse
from uuid import uuid4
import sys

n_sensors = 0
n_messages_published = 0
topic = ""
mqtt_client = mqtt.Client(str(uuid4()))
is_connected = False

def generateDataEntry(sensorID):
    return f"""[{{
    "version": "2.0",
    "tagId": "{sensorID}",
    "timestamp": {time.time()},
    "data": {{
        "coordinates": {{
            "x": {randint(0, 100)},
            "y": {randint(0, 100)},
            "z": {randint(0, 100)}
        }},
        "velocity": {{
            "x": {randint(0, 100)},
            "y": {randint(0, 100)},
            "z": {randint(0, 100)}
        }},
        "acceleration": {{
            "x": {randint(0, 100)},
            "y": {randint(0, 100)},
            "z": {randint(0, 100)}
        }},

        "tagData": {{
            "blinkIndex": 226,
            "accelerometer": [
                [{randint(0,1000)}, {randint(0,1000)}, {randint(0,1000)}]
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
