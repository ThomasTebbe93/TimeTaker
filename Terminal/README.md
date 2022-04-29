# time-terminal

Prototyp
![Prototyp](https://github.com/ThomasTebbe93/TimeTaker/blob/main/Terminal/case/case.PNG?raw=true)

## hardware

Raspberry pi 4 B

sd-card

RFID-Module with chips (https://www.amazon.de/gp/product/B00QFDRPZY?ie=UTF8&linkCode=as2&camp=1634&creative=6738&tag=754-21&creativeASIN=B00QFDRPZY)

Display (https://www.amazon.de/gp/product/B08CH24YYD/ref=ppx_yo_dt_b_asin_title_o00_s00?ie=UTF8&psc=1)

some jumpercables

### Case

Als Case können viele der gängigen Cases für das entsprechende Display verwendet werden.
Ich damit alles (Sendor, Display und Pi) schön in einem passenden Case enthalten ist habe ich selbst einst erstellt und gedruckt.
Die Dateien dazu sind unter "./TimeTaker/Terminal/case" zu finden.
Die dort enthaltenen STL lassen sich mit UltimakerCura 3.1 und höher gut druken. Wenn kein eigener 3D-Drucker vorhanden ist gibt es im Internet verschiedene Abieter für den Druck. Ggf. muss die Ausspahruing für den Stromanschluss etwas nachgearbeiet werden.

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

```
sudo nano /boot/config.txt
```

add:

```
device_tree_param=spi=on
dtoverlay=spi-bcm2708
```

save and quit (STRG+O, STRG+X)

### Aktivate SPI

```
sudo raspi-config
```

„Advanced Options“ > „SPI“ activate

reboot pi

### Install packages

#### Update raspberry pi

```
sudo apt update
```

#### Insatll python

```
sudo apt install python3-dev python3-pip
```

#### Install git

```
sudo apt-get install git
```

#### Install lib for gui

```
sudo apt-get install python3-tk
```

#### Install SPI Module

```
sudo pip3 install spidev
```

#### Insatll lib for RFID

```
sudo pip3 install mfrc522
```

#### Install lib for http-requests

```
sudo pip3 install requests
```

### Edit config.ini

Hinterlegen Sie die Benutzerdaten (Login und Passwort) fürs Termianl

### Run application

```
python ./main.py
```

## Create Role For Terminal-User

Damit das terminal buchen kann muss der hinterlegte Benutzer eine Rolle mit folgenden rechten haben:

- Dienststunden anzeige
  - erstellen
  - Chip scannen
