# time-terminal

## hardware

Raspberry pi 4B
sd-card
RFID-Module with chips (https://www.amazon.de/gp/product/B00QFDRPZY?ie=UTF8&linkCode=as2&camp=1634&creative=6738&tag=754-21&creativeASIN=B00QFDRPZY)
Display (https://www.amazon.de/gp/product/B08CH24YYD/ref=ppx_yo_dt_b_asin_title_o00_s00?ie=UTF8&psc=1)
some jumpercables

## Connect to raspberry pi

SDA -> Pin 24 / GPIO8 (CE0)
SCK -> Pin 23 / GPIO11 (SCKL)
MOSI -> Pin 19 / GPIO10 (MOSI)
MISO -> Pin 21 / GPIO9 (MISO)
IRQ —
GND -> Pin6 (GND)
RST -> Pin22 / GPIO25
3.3V -> Pin 1 (3V3)

## Prepare raspbery pi

### Configure boot config

sudo nano /boot/config.txt
add
device_tree_param=spi=on
dtoverlay=spi-bcm2708
save and quit (STRG+O, STRG+X)

### Aktivate SPI

sudo raspi-config
„Advanced Options“ > „SPI“ activate
reboot pi

### Install packages

#### Update raspberry pi

sudo apt update

#### Insatll python

sudo apt install python3-dev python3-pip

#### Install git

sudo apt-get install git

#### Install lib for gui

sudo apt-get install python3-tk

#### Install SPI Module

sudo pip3 install spidev

#### Insatll lib for RFID

sudo pip3 install mfrc522

#### Install lib for http-requests

sudo pip3 install requests

### Edit config.ini

### Run application

python ./main.py
