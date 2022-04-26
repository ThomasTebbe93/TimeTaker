from distutils.command.config import config
import string
import tkinter as tk
from tkinter import Button
from ChipReader import ChipReader
from RequestService import RequestService
from loopExecuter import LoopExecuter
import configparser

global appConfig
appConfig = configparser.ConfigParser()
appConfig.read("config.ini")
appConfig.sections()

global currentUserIdent
currentUserIdent = None

class ABC(tk.Frame):
    def __init__(self, parent=None):
        tk.Frame.__init__(self, parent)
        self.parent = parent
        self.pack()
        global appConfig
        self.config = appConfig
        self.make_widgets()

    def make_widgets(self):
        def checkForScan():
            chipId = ChipReader().read()
            if chipId is not None:

                requestService = RequestService(self.config)
                userInfo = requestService.getUserInfoByChip(chipId)

                if userInfo is not None:
                    global currentUserIdent
                    currentUserIdent = userInfo["Ident"]
                    showUser(userInfo["UserName"], userInfo["SignedIn"])
                if userInfo is None:
                    removeUser()
                    showError("Unbekannter Chip %s" % (chipId))
                    currentUserIdent = None

        def onSignIn():
            global currentUserIdent
            if currentUserIdent is not None and currentUserIdent != "":
                requestService = RequestService(self.config)
                result = requestService.signIn(currentUserIdent)
                if result == True:
                    removeUser()
                    currentUserIdent = None
                else:
                    showError("Fehler beim Anmelden")
                    currentUserIdent = None

        def onSignOut():
            global currentUserIdent
            if currentUserIdent is not None and currentUserIdent != "":
                requestService = RequestService(self.config)
                result = requestService.signOut(currentUserIdent)
                if result == True:
                    removeUser()
                    currentUserIdent = None
                else:
                    showError("Fehler beim Abmelden")
                    currentUserIdent = None

        def showUser(username: string, signedIn):
            userLable.config(text="Wilkommen {}".format(username))
            if signedIn == True:
                signIn.pack_forget()
                signOut.pack()
            if signedIn != True:
                signOut.pack_forget()
                signIn.pack()

        def removeUser():
            userLable.config(text="")
            signIn.pack_forget()
            signOut.pack_forget()

        def showError(meaasge: string):
            userLable.config(text=meaasge)

        def notAssignedChip():
            userLable.config(text="Unbekannter / nicht zugewiesener Chip")
            signIn.pack_forget()
            signOut.pack_forget()

        self.winfo_toplevel().title("ChipScanner")

        titleFrame = tk.Frame(master=self.winfo_toplevel(), height=100, bg="#eeeeee")
        titleFrame.pack(fill=tk.X, side=tk.TOP)

        title = tk.Label(master=titleFrame, text="ChipScanner", bg="#eeeeee")
        title.config(font=("Courier", 44))
        title.pack()

        body = tk.Frame(
            master=self.winfo_toplevel(), width=100, height=100, bg="#eeeeee"
        )
        body.pack(fill=tk.BOTH, side=tk.LEFT, expand=True)

        userInfo = tk.Frame(body, height=100, bg="#eeeeee")
        userInfo.pack(fill=tk.X, side=tk.TOP)
        userLable = tk.Label(master=userInfo, bg="#eeeeee")
        userLable.config(font=("Courier", 22))
        userLable.pack()

        footer = tk.Frame(master=body, height=50, bg="#eeeeee")
        footer.pack(side=tk.BOTTOM)

        actions = tk.Frame(body, height=100, width=200, bg="#eeeeee")
        actions.pack(side=tk.BOTTOM)

        signIn = tk.Frame(master=actions, width=100, height=100, bg="#aaffaa")
        signInButton = Button(
            signIn,
            text="Kommen",
            bg="#aaffaa",
            width=10,
            height=5,
            command= onSignIn,
        )
        signInButton.config(font=("Courier", 22))
        signInButton.pack()

        signOut = tk.Frame(master=actions, width=100, height=100, bg="#ffaaaa")
        signOutButton = Button(
            signOut,
            text="Gehen",
            bg="#ffaaaa",
            width=10,
            height=5,
            command=onSignOut,
        )
        signOutButton.config(font=("Courier", 22))
        signOutButton.pack()

        spacer = tk.Frame(master=body, height=50, bg="#eeeeee")
        spacer.pack(side=tk.BOTTOM)

        LoopExecuter(0.25, checkForScan)


def toggleFullScreen(self, event):
    self.window.attributes("-fullscreen", True)


def quitFullScreen(self, event):
    self.window.attributes("-fullscreen", False)


root = tk.Tk()
root.geometry("800x480")
# root.attributes("-fullscreen", True)
# root.bind("<F11>", toggleFullScreen)
# root.bind("<Escape>", quitFullScreen)
abc = ABC(root)
root.mainloop()
