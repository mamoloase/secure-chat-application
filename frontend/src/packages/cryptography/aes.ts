import CryptoJS from 'crypto-js';

class AesCryptography {
    private iv: CryptoJS.lib.WordArray = CryptoJS.enc.Utf8.parse("");
    private signiture: CryptoJS.lib.WordArray = CryptoJS.enc.Utf8.parse("");

    private options = { mode: CryptoJS.mode.ECB, keySize: 256 };

    loadSigniture = (key: string) => {
        this.signiture = CryptoJS.enc.Base64.parse(key)
    }
    loadIV = (iv: string) => {
        this.iv = CryptoJS.enc.Base64.parse(iv)
    }
    encrypt = (str: string): string => {
        return CryptoJS.AES.encrypt(str, this.signiture, this.options).toString();
    }
    decrypt = (str: string): string => {
        return CryptoJS.AES.decrypt(str, this.signiture, this.options)
            .toString(CryptoJS.enc.Utf8);
    }

}
export default AesCryptography