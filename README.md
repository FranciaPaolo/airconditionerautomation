# Air Conditioner Automation

The purspose: remote control of the Mitsubishi Electric Air Conditioner.
* Access a WebApplication made in Angular, AspentCore, Sqlite
  * See the environmental data (temperature, humidity)
  * Send command to the air conditioner
  * Turn on setting temperature and mode (Cool or Hot)
  * Turn off

![screenshot](imgs/webapp_preview.png)

Component diagram:
![screenshot](imgs/diagram_components.png)

## WebApplication
See the code in the WebApplication folder for details

## Esp32 device
Use the Arduino Ide to compile, add the following libraries:
* DHT22
* DHT22
* IRemoteESP8266
* Adafruit Unified Sensor

The library "IRemoteESP8266" is extremily important because it has predefined Infrared commands for a lot of devices.

Things to setup:
* Connect to your home wifi setting "ssid" and "password" in the "sketch_temp_sensor.ino"
* Change your data pin in "sketch_temp_sensor.ino"

Esp32 webserver routes, the they are all Http Get:
| Method  | Description | Request example | Response example |
| ------------- | ------------- | ------------- | ------------- |
| /  | Html page that show the status and explain the possible http commands.  |  |  |
| /dht22  | Show temperature and humidity like a rest api in Json | No parameter required  | {"temperature":20.2, "humidity":30.2} |
| /airconditioning | Return the status of the air conditioner. Mode: 1 heat, 3 cool.  | No parameter required.| {"power": 1, "mode": 3, "temperature": 20.0} |
| /climateon | Turn on the air conditioner. The api returns the status of the air conditioner after the command. | /climateon?temperature=20&mode=3 | {"power": 1, "mode": 3, "temperature": 20.0} |
| /climateoff | Turn off the air conditioner. The api returns the status of the air conditioner after the command. | No parameter required | {"power": 0, "mode": 3, "temperature": 20.0} |

See the code in the Esp32 folder for details

## Ops (putting everything together)

