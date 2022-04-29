# TimeTaker

Das Projekt TimeTaker besteht aus drei Komponenten

-   API/Server
    -   Bildet die Grundlage für die Datenverarbeitung und Speicherung
-   Webclient
    -   Kann von überall über den Browser aufgerufen werden.
    -   Wird verwendet, um
        -   Benutzer anzulegen
        -   Rollen festzulegen
        -   Daten zwischen DRK-Server und TimeTake auzutauschen
        -   Efasste Buchungen zu bearbeiten und frei zu geben
-   Terminal
    -   Ist die vor Ort installierte Einhaeit, die zum Erfassen von Buchungen verwendet wird.
    -   Hier scannen die Benutzer ihren RFID-Chip
    -   [Dokumentation](https://github.com/ThomasTebbe93/TimeTaker/blob/main/Terminal/README.md)

## 1 Develpment

### 1.1 Voraussetzungen:

#### 1.1.1 .Net 6

Um das Repo auf deiner Maschine bauen und laufen zu können, benötigen Sie die SDK von .Net 6.

-   https://dotnet.microsoft.com/en-us/download/dotnet/6.0

#### 1.1.2 Node JS

Zum Bauen der Weboberfläche müssen Pakete über npm installiert werden. Außerdem brauchen Sie zum lokalen Ausführen der Weboberfläche einen Node JS Server. Dafür sollte Node JS auf Ihrem Rechner installiert sein.

-   https://nodejs.org/en/

#### 1.1.3 PostgreSQL

Das Projekt nutzt eine PostgreSQL-Datenbank. Daher müssen Sie auch diese installieren, damit das Projekt lokal laufen kann. Bitte achten Sie darauf mindestens PostgreSQL 10 oder höher zu installieren.

-   https://www.postgresql.org/download/

#### 1.1.4 pgAdmin

pgAdmin ist ein PostgreSQL Tool, welches sowohl für das Aufsetzen der Datenbank als auch für das Updaten sinnvoll ist.

-   https://www.pgadmin.org/download/

### 1.2 Projekt lokal laufen lassen

-   Clonen Sie dieses Reop mit git.

#### 1.2.1 Datanbank vorbereiten

-   Installieren SIE PostgreSQL.
-   Erstellen Sie einen neuen Benutzer.
-   Erstellen Sie eine neue Datenbank für dieses Projekt.
-   Erstellen Sie die Tabellen:
    -   Führen Sie dafür alle Scripte im Ordner "./timeTaker/Database/setup" in Reihenfolge ihrer Nummerierung aus.
    -   Ist dies geschaft müssen ggf. noch Updates durchgeführt werden. Dazu öffnen Sie die jeweiligen VersionsOrdner unter "./timeTaker/Database" in Reihenfolge ihrer Nummerierung und führen die darin enthaltenen Scripte ebenfalls in Reihenfolge ihrer Nummerierung aus.
-   Passen Sie nun die "DbConnection" in "./timeTaker/API/appsettings.json" an.

#### 1.2.2 Run API

-   Gehen Sie in den Ordner "./timeTaker/API" und öffnen das Projekt in Ihrer IDE.
-   Lassen Sie alle verwendeten NuGet-Pakete installieren.
-   Passen Sie die appsettings.json in "./timeTaker/API/appsettings.json" an.
-   Um das Project lokal laufen zu lassen, können Sie die IDE nutzen oder in dem Ordner "./timeTaker/API" folgenden Befehl ausführen:
    ```
    dotnet run
    ```
-   Die API ist dann unter den in der appsettings.json festgelegten "Urls" erreichbar.

#### 1.2.3 Run UI

-   Öffnen Sie den Ordner "./timeTaker/ui" in einer IDE Ihrer Wahl
-   Installieren Sie die nötigen Node-Modules
    ```
    npm i
    ```
-   Starten Sie den integrierten Develpmentserver.
    ```
    npm run build
    ```
-   Die Weboberfläche ist nun unter http://localhost:3000 erreichbar.

## 2 Deploy

### 2.1 Voraussetzungen:

#### 2.1.1 .Net 6

Um das Repo auf Ihrer Maschine bauen zu können benötigen Sie die SDK von .Net 6, für das Veröffentlichen und Installieren auf einer Maschine Ihrer Wahl muss dort die ASP.NET Core 6 Runtime installiert sein.

-   https://dotnet.microsoft.com/en-us/download/dotnet/6.0

#### 2.1.2 Node JS

Zum Bauen der Weboberfläche müssen Pakete über npm installiert werden. Dafür sollte Node JS auf Ihrem Rechner installiert sein.

-   https://nodejs.org/en/

#### 2.1.3 FTP-Client

Damit Sie die gebauten Dateien auf den Server verschieben Können ist es sinnvoll einen FTP-Client wie FileZilla zu installieren.

-   https://filezilla-project.org/download.php?type=client

#### 2.1.3 ssh-Client

Damit Sie den Server vorbereiten können ist es sinnvoll einen SSH-Client wie PuTTY zu installieren.

-   https://www.chiark.greenend.org.uk/~sgtatham/putty/latest.html

#### 2.1.4 pgAdmin

pgAdmin ist ein PostgreSQL-Tool, welches sowohl für das Aufsetzen der Datenbank als auch für das Updaten sinnvoll ist.

-   https://www.pgadmin.org/download/

### 2.2 Projekt bauen

-   Clonen Sie dieses Reop mit git.

#### 2.2.1 Build API

-   Gehen Sie in den Ordner "./timeTaker/API" und öffnen das Projekt in Ihrer IDE.
-   Lassen Sie alle verwendeten NuGet-Pakete installieren.
-   Bauen Sie das Projekt als .dll für Linux.
    ```
    dotnet publish API.csproj --configuration Release --framework net6.0 --output ./deploy --self-contained false --runtime linux-x64 --verbosity quiet
    ```
-   Für andere Systeme muss der oben aufgführte Befehl ggf. angepasst werden.
-   Der Output landet im Ordner "./timeTaker/API/deploy".

#### 2.2.2 Build UI

-   Öffnen Sie ein Terminal und navigieren in den Ordner "./timeTaker/ui"
-   Dort führen Sie folgende Befehle aus:
    ```
    npm i
    npm run build
    ```
-   Der Output liegt im Ordner "./timeTaker/ui/build"

#### 2.2.2 Build UI-Server

-   Öffnen Sie ein Terminal und navigieren in den Ordner "./timeTaker/uiServer"
-   Dort führen Sie folgende Befehle aus
    ```
    npm i
    ```

### 2.3 Server vorbereiten (Ubuntu)

-   Sellen Sie sicher, das ssh auf dem Server aktiviert ist
-   Verbinden Sie sich mit Ihrem SSH-Client mit dem Server

#### 2.3.1 PostgreSQL

-   Insallieren Sie PostgreSQL.
    ```
    sudo apt update
    sudo apt install postgresql postgresql-contrib
    ```
-   Legen Sie eien Benutzer für die Anwendung an.
    ```
    sudo -u postgres createuser --interactive
    ```
    Folgender Output erscheint
    ```
    Output
    Enter name of role to add: <NAME_DER_ANWENDUNG>
    Shall the new role be a superuser? (y/n) y
    ```
-   Erstellen Sie die Datenbank für die Anwendung.

    ```
    sudo -u postgres createdb <NAME_DER_ANWENDUNG>
    ```

https://www.digitalocean.com/community/tutorials/how-to-install-postgresql-on-ubuntu-20-04-quickstart-de

#### 2.3.2 nginx

Führen Sie auf dem Server folgende Befehle aus:

```
sudo apt update
sudo apt install nginx
```

Konfigrieren Sie nginx

-   Navigieren Sie in "../etc/nginx/sites-available".
-   Öffnen Sie die Konfiguration "default" mit einem Editor.
    ```
    vim default
    ```
-   Bearbeiten Sie eienen forhandenen Server oder fügen Sie einen neuen hinzu.

    -   In diesem Beispiel ist der Server über "<DOMAIN_OR_SUBDOMAIN>" erreichbar, die API läuft auf Ihrem Server unter "http://localhost:<API_PORT>" und der Server für die Weboberfläche ist unter "http://localhost:<UI_SERVER_PORT>" erreichbar.
    -   Die Ports sind in der "./timeTaker/API/appsettings.json" und der "./timeTaker/uiServer/uiServer.js" definiert.

    ```
    server {
            client_max_body_size    2000m;
            server_name <DOMAIN_OR_SUBDOMAIN>;

            location /api {
                    rewrite                 /api/?(.*) /$1 break;
                    proxy_pass              http://localhost:<API_PORT>;
                    proxy_http_version      1.1;
                    proxy_set_header        Upgrade $http_upgrade;
                    proxy_set_header        Connection keep-alive;
                    proxy_set_header        Host $host;
                    proxy_cache_bypass      $http_upgrade;
                    proxy_set_header        X-Forwarded-For $proxy_add_x_forwarded_for;
                    proxy_set_header        X-Forwarded-Proto $scheme;
            }

            location / {
                    proxy_pass              http://localhost:<UI_SERVER_PORT>;
                    proxy_http_version      1.1;
                    proxy_set_header        Upgrade $http_upgrade;
                    proxy_set_header        Connection keep-alive;
                    proxy_cache_bypass      $http_upgrade;
                    proxy_set_header        X-Forwarded-For $proxy_add_x_forwarded_for;
                    proxy_set_header        X-Forwarded-Proto $scheme;
            }
    }
    ```

-   Im Anschluss ppeichern Sie die geänderte Konfiguration und starten ngingx neu, damit die Änderungen übernommen werden.
    ```
    sudo nginx -s reload
    sudo service nginx start
    ```

#### 2.3.3 Certbot

Certbot ist eine Anwendung, die sich nach einmaliger Konfiguration um die Installation und Aktualisierung von SSL-Zertifikaten kümmert.
Diese sind zwingend notwendig, damit Ihre Seite und die erfassten persönlichen Daten sicher ist. Außerdem werden Webseiten ohne SSL-Zertifikat von modernen Browsern blockiert.

Zur Installation führen Sie folgende Befehle aus:

```
sudo apt install snapd
sudo snap install core; sudo snap refresh core
sudo snap install --classic certbot
sudo ln -s /snap/bin/certbot /usr/bin/certbot
```

Nun können Sie automatisch die ersten Zertifikate generieren lasssen. Führen Sie dazu folgenden Befehl aus und folgen den Anweisungen in der Konsole:

```
sudo certbot --nginx
```

https://www.inmotionhosting.com/support/website/ssl/lets-encrypt-ssl-ubuntu-with-certbot/

#### 2.3.4 Linux Screen

Führen Sie auf dem Server folgende Befehle aus:

```
sudo apt update
sudo apt install screen
```

#### 2.3.5 Datenbank einrichten

Nun ist es an der zeit mit Hilfe von pgAdmin sich auf den PostgreSQL-Datenbak-Server zu verbinden.

-   ggf. muss dazu noch die eigene IP zugelassen werden.
    Die eigene IP finden Sie einfach über die Webseite https://www.wieistmeineip.de/ heraus.
    Haben Sie diese, so muss diese in die Konfiguration des PostgreSQL-Servers hinterlegt werden.
    Gehen Sie dazu in den Ordner "../etc/postgresql/10/main/" und passen Sie die Datei "pg_hba.conf" mit eienm Editor an.

    ```
    cd ../etc/postgresql/10/main/
    vim pg_hba.conf
    ```

    Fügen Sie Folgenden Eintrag hinzu

    ```
    host    <DATABASE>   <USER>   <YOUR_IP> 255.255.255.255 trust
    ```

    Starten Sie danach den PostgreSQL-Server neu

    ```
    cd ../etc/init.d
    ./postgresql restart
    ```

-   Erstellen Sie die notwendigen Tabellen:
    -   Führen Sie dafür alle Scripte im Ordner "./timeTaker/Database/setup" in Reihenfolge ihrer Nummerierung aus
    -   Ist dies geschaft müssen ggf. noch Updates durchgeführt werden. Dazu öffnen Sie die jeweiligen VersionsOrdner unter "./timeTaker/Database" in Reihenfolge ihrer Nummerierung und führen die darin enthaltenen Scripte ebenfalls in Reihenfolge ihrer Nummerierung aus.

#### 2.3.6 Anwendung installieren

Nun ist der Server soweit vorbereitet, dass die Pakete für die Anwendung übertragen werden können.

-   Erstellen Sie mit Hilfe der Konsole oder des FTP-Clients folgende Ordnerstruktur
    ```
    Anwendungsordner
    ├── api
    ├── ui
    └── uiServer
    ```
-   Kopieren Sie nach dem Bauen der einzelnen Teile der Anwendung (siehe 2.2) den Inhalt der Ordner in den passenden Ordner auf dem Server.
    ```
    Inhalt von "./timetaker/API/deploy" => "./Anwendungsordner/api"
    Inhalt von "./timetaker/ui/buuild" => "./Anwendungsordner/ui"
    Inhalt von "./timetaker/uiServer" => "./Anwendungsordner/uiServer"
    ```

#### 2.3.7 Anwendung Konfigurieren

-   Passen Sie unter "./Anwendungsordner/api" die appsettings.json an.
-   Passen Sie unter "./Anwendungsordner/uiServer" den Port inder uiServer.js an.

### 2.4 Anwendung starten

#### 2.4.1 API

Führen Sie in "./Anwendungsordner/api" folgenden Befehl aus:

```
sudo screen -S timeTakerApi dotnet API.dll
```

#### 2.4.2 UI-Server

Führen Sie in "./Anwendungsordner/uiServer" folgenden Befehl aus:

```
sudo screen -S timeTakerUi node uiServer.js
```

### 2.5 Anwendung beenden

#### 2.5.1 API

Führen Sie im Terminal Ihres Servers folgenden Befehl aus:

```
sudo screen -r timeTakerApi
```

Sind Sie im richtigen screen, so können Sie über

-   ctrl + c oder
-   ctrl + a, dann k und wenn gefragt wird, ob Sie die Session wirklich beenden wollen y

den Servie beenden

#### 2.5.2 UI-Server

Führen Sie im Terminal Ihres Servers folgenden Befehl aus:

```
sudo screen -r timeTakerUi
```

Sind Sie im richtigen screen, so können Sie über

-   ctrl + c oder
-   ctrl + a, dann k und wenn gefragt wird, ob Sie die Session wirklich beenden wollen y

den Servie beenden
