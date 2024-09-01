#include <WiFi.h>
#include <Wire.h>
#include <Adafruit_MPU6050.h>
#include <Adafruit_Sensor.h>
#include <WiFiClient.h>

const char* ssid = "Virus";
const char* password = "ali@1818231042";
const char* serverIP = "192.168.158.168";
const uint16_t serverPort = 1234;

WiFiClient client;
Adafruit_MPU6050 mpu;

void setup() {
  Serial.begin(115200);
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(1000);
    Serial.println("Connecting to WiFi...");
  }
  Serial.println("Connected to WiFi");

while (1) {
  if (!mpu.begin()) {
    Serial.println("Failed to find MPU6050 chip");
    delay(10);
  }
  break;
}
  mpu.setAccelerometerRange(MPU6050_RANGE_2_G);
  mpu.setGyroRange(MPU6050_RANGE_250_DEG);
  mpu.setFilterBandwidth(MPU6050_BAND_21_HZ);

  if (!client.connect(serverIP, serverPort)) {
    Serial.println("Connection to server failed");
  } else {
    Serial.println("Connected to server");
   }
}

void loop() {
  sensors_event_t a, g, temp;
  mpu.getEvent(&a, &g, &temp);

  if (client.connected()) {
    client.println("Acceleration X:" + String(a.acceleration.x) + 
               ", Y:" + String(a.acceleration.y) + 
               ", Z:" + String(a.acceleration.z) + 
               " | Gyro X:" + String(g.gyro.x) + 
               ", Y:" + String(g.gyro.y) + 
               ", Z:" + String(g.gyro.z));
  } else {
    Serial.println("Disconnected from server");
    while (1) {
        if (!client.connect(serverIP, serverPort)) {
            Serial.println("Connection to server failed");
            delay(1000);
        } else {
            Serial.println("Connected to server");
            break; 
        }
    }
  }
  delay(10);
}
