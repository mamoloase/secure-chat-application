import JSEncrypt from "jsencrypt"

class RsaCryptography {
    private crypto: JSEncrypt;

    constructor() {
        this.crypto = new JSEncrypt({ default_key_size: '1024' })
    }

    encrypt = (str: string) => {
        var encrypted = this.crypto.encrypt(str);

        return encrypted ? encrypted : "";
    }
    decrypt = (str: string): string => {

        var decrypted = this.crypto.decrypt(str);

        return decrypted ? decrypted.replace(/[^A-Za-z0-9+/=]/g, '') : "";
    }
    getPublicKeyBase64 = () => {
        return this.crypto.getPublicKeyB64();
    }
    getPrivateKeyBase64 = () => {
        return this.crypto.getPrivateKeyB64();
    }
}
export default RsaCryptography
