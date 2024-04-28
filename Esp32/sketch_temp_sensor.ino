#include <DHT.h>
#include <WiFi.h>
#include <WebServer.h>
#include <IRremoteESP8266.h>
#include <IRsend.h>
#include <IRrecv.h>
#include <IRutils.h>
#include <ir_Mitsubishi.h>
#include "TempHum.h"
#include "AirConditioning.h"

// GPIO **************************************************************
#define dataPin_dth 21           // connected to DHT22 sensor
#define dataPin_irController 17  // connected to Infrared Controller

// Temp Humidity Sensor setup **************************************************************
#define DHTType DHT22
DHT dht = DHT(dataPin_dth, DHTType);
TempHum tempHum;  // Custom class to for Temperarure and Humidity value

// Wifi Network setup **************************************************************
const char* ssid = "";
const char* password = "";
// WiFiServer server(80);  // Set web server port number to 80
WebServer server(80);
String header;
unsigned long currentTime = millis();  // Current time
unsigned long previousTime = 0;        // Previous time
const long timeoutTime = 2000;         // Define timeout time in milliseconds (example: 2000ms = 2s)

// Infrared **************************************************************
decode_results ir_rec_message;
int ir_send_bool = 0;
int climate_command_status = 0;           // 0 off, 1 turn-on requested
IRMitsubishiAC ac(dataPin_irController);  // Set the GPIO used for sending messages.
AirConditioning ac_wrapper(&ac);          // wrapper to simplify ac calls

void setup() {
  Serial.begin(9600);

  ac.begin();
  // Set up what we want to send. See ir_Mitsubishi.cpp for all the options.
  delay(200);
  ac_wrapper.serialPrint();
  ac_wrapper.setDefault();
  ac_wrapper.serialPrint();

  // start temp, humidity sensor
  dht.begin();

  // Connect to Wi-Fi network with SSID and password
  Serial.print("Connecting to ");
  Serial.println(ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  // Print local IP address and start web server
  Serial.println("");
  Serial.println("WiFi connected.");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
  //server.begin();

  server.on("/", web_handle_OnConnect);
  server.on("/dht22", web_handle_get_temphum);
  server.on("/airconditioning", web_handle_get_airconditioning);
  server.on("/climateon", web_handle_set_climateon);
  server.on("/climateoff", web_handle_set_climateoff);
  server.onNotFound(web_handle_NotFound);

  server.begin();
}

void loop() {

  delay(3000);

  // read temperature and humidity
  tempHum.setTemperatureHumidity(dht.readTemperature(), dht.readHumidity());
  tempHum.serialPrint();

  server.handleClient();
}

void web_handle_OnConnect() {
  server.send(200, "text/html", ac_wrapper.html_home(tempHum.getTemperature(), tempHum.getHumidity()));
}

void web_handle_get_temphum() {
  server.send(200, "application/json", "{ \"temperature\":" + String(tempHum.getTemperature()) + ", \"humidity\":" + String(tempHum.getHumidity()) + " }");
}

void web_handle_get_airconditioning() {
  server.send(200, "application/json", ac_wrapper.json_airconditioning());
}

void web_handle_set_climateon() {

  if (server.hasArg("temperature")) {
    ac_wrapper.setTemperature(server.arg("temperature"));
  }

  if (server.hasArg("mode")) {
    ac_wrapper.setMode(server.arg("mode"));
  }

  ac_wrapper.on();
  ac_wrapper.send();

  server.send(200, "application/json", ac_wrapper.json_airconditioning());
}

void web_handle_set_climateoff() {
  ac_wrapper.off();
  ac_wrapper.send();
  server.send(200, "application/json", ac_wrapper.json_airconditioning());
}

void web_handle_NotFound() {
  server.send(404, "text/html", "<html><body><h1>Not found</h1></body></html>");
}

