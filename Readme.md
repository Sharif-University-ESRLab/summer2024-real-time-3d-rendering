![Logo](https://via.placeholder.com/600x150?text=Your+Logo+Here+600x150)


# Project Title

A brief description of what this project does and who it's for comes here.


This project involves designing and building a system that uses a gyroscope sensor to measure the movement and angle of a device, and then transmits this data wirelessly to a computer using an ESP32 module. The data is used to update a real-time 3D model on the computer, which can be developed using software like Unity or Blender. The 3D model should accurately reflect the device's movements without delay, and the system must be tested and calibrated to ensure precision. This project is intended for users needing real-time visualization of device movements, such as in simulations or interactive applications.


## Tools
In this section, you should mention the hardware or simulators utilized in your project.
- MPU6050
- Unity
- ESP32
- Powerbank
- Airplane Mag


## Implementation Details

This project is divided into two main parts, the implementation of the 3D model and the TCP protocol, which we will explain below.

# TCP Protocol

This protocol had to be written in two parts, client and server, and it is in this way that the client part is written with the ESP module and it works in such a way that every 10 milliseconds data is sent from the client side to the server side on the specified IP address and port. The server also receives the information and parses it according to the sent format and gives it to the 3D modeling department to process the data.

# 3D Modeling

This part also works in the way that we used the airplane model for simulation. After the data is received, we get its acceleration and angular velocity and use it to move the plane.

## How to Run

In this part, you should provide instructions on how to run your project. Also if your project requires any prerequisites, mention them. 


#### Run Client
In this part, we should set ssid and password of this ssid to connect to the WiFi. After that we should set server IP and its port to connect to server. After doing above, we should upload the code to the ESP32. 

#### Run server
Your text comes here
```bash
  pyhton server.py -p 8080
```

| Parameter | Type     | Description                |
| :-------- | :------- | :------------------------- |
| `-p` | `int` | **Required**. Server port |



## Results
Finally, the 3D model works well and by moving the plane, its movements are repeated in the simulator. You can see a view of it in the photo below.

![photo_2024-09-01_14-35-14](https://github.com/user-attachments/assets/94b1ee92-e2ef-437d-bd15-ad5ef1979108)

## Related Links
Some links related to your project come here.
 - [ESP32 Pinout](https://randomnerdtutorials.com/esp32-pinout-reference-gpios/)
 - [Unity Doc](https://docs.unity.com/)


## Authors
Authors and their github link come here.
- [@MohammadMow](https://github.com/MohammadMow)
- [@Author2](https://github.com/Sharif-University-ESRLab)

