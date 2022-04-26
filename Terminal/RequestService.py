import string
import requests
import json
import configparser
from datetime import datetime


class RequestService:
    def __init__(self, config: configparser):
        self.baseurl = config["API"]["BaseURL"]
        self.login = config["Terminal.User.Credentials"]["Login"]
        self.password = config["Terminal.User.Credentials"]["Password"]
        self.autToken = self.getToken()

    def getToken(self) -> string:
        url = "%s/authentication/authenticate" % (self.baseurl)
        payload = {"UserName": self.login, "Password": self.password}
        headers = {
            "content-type": "application/json; charset=UTF-8",
        }
        with requests.post(url, data=json.dumps(payload), headers=headers) as r:
            if r.status_code == 200:
                result = r.json()
                if result["token"]:
                    return result["token"]
                else:
                    print("error trying to authenticate")

    def signIn(self, userIdent: string) -> json:
        currentTime = datetime.now().isoformat()
        url = "%s/dutyHoursBooking/signInByChip" % (self.baseurl)
        payload = {
            "userIdent": userIdent,
            "bookingTime": currentTime,
        }
        headers = {
            "content-type": "application/json; charset=UTF-8",
            "Authorization": "Bearer %s" % (self.autToken),
        }
        with requests.post(url, data=json.dumps(payload), headers=headers) as r:
            if r.status_code == 200:
                return True
            else:
                print("error trying to sign in")
                return False

    def signOut(self, userIdent: string) -> json:
        currentTime = datetime.now().isoformat()
        url = "%s/dutyHoursBooking/signOutByChip" % (self.baseurl)
        payload = {
            "UserIdent": userIdent,
            "BookingTime": currentTime,
        }
        headers = {
            "content-type": "application/json; charset=UTF-8",
            "Authorization": "Bearer %s" % (self.autToken),
        }
        with requests.post(url, data=json.dumps(payload), headers=headers) as r:
            if r.status_code == 200:
                return True
            else:
                print("error trying to sign out")
                return False


    def getUserInfoByChip(self, uid: string) -> json:
        url = "%s/dutyHoursBooking/getPersonAndStateByChipId/%s" % (self.baseurl, uid)
        headers = {
            "content-type": "application/json; charset=UTF-8",
            "Authorization": "Bearer %s" % (self.autToken),
        }
        with requests.get(url, headers=headers) as r:
            if r.status_code == 200 and r.text is not None:
                result = json.loads(r.text)
                value = result['value']
                try:
                    lastBooking = value['lastBooking']
                    if(lastBooking is None):
                        return {
                            "UserName": value['user']['firstName'] + ' ' + value['user']['lastName'], 
                            "Ident": value['user']['ident']['ident'], 
                            "SignedIn": False}
                    return {
                            "UserName": value['user']['firstName'] + ' ' + value['user']['lastName'], 
                            "Ident": value['user']['ident']['ident'], 
                            "SignedIn": lastBooking["isSignedIn"]}
                    
                except KeyError as ke:
                    print("error finding user for chip")
                    return None
            else:
                print("error while fetching info")
                return None
