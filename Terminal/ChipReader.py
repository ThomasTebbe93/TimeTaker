import string
import MFRC522 as mfrc


class ChipReader(object):
    def uidToString(self, uid: list) -> string:
        result = ""
        for x in uid:
            result= result + (self.intToHex(x).zfill(2))
        return result

    def intToHex(self, value: int) -> string:
        return f"{value:x}"

    def read(self) -> string:
        MIFAREReader = mfrc.MFRC522()
        (status) = MIFAREReader.MFRC522_Request(MIFAREReader.PICC_REQIDL)

        if status == MIFAREReader.MI_OK:
            print("Card detected")

        (status, uid) = MIFAREReader.MFRC522_Anticoll()

        if status == MIFAREReader.MI_OK:
            result = self.uidToString(list(uid))
            print("Card read UID: %s" % (result))
            return result
