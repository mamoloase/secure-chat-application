import RsaCryptography from "@/packages/cryptography/rsa";
import AesCryptography from "@/packages/cryptography/aes";
import ConnectionMessage from "./types/connectionMessage";

export interface AnswerHandler {
    id: string,
    callback: (request: ConnectionMessage) => void,
}

export default class Transport {
    private webSocket: WebSocket;
    private handlers: Array<(request: ConnectionMessage) => undefined> = [];
    private handlersAnswer: Array<AnswerHandler> = [];

    private rsa: RsaCryptography = new RsaCryptography();
    private aes: AesCryptography = new AesCryptography();

    constructor(path: string) {

        const url = new URL(path);

        url.searchParams.set("pk", this.rsa.getPublicKeyBase64());

        this.webSocket = new WebSocket(
            url.toString().replace(/\+/g, "%2B"))

        this.webSocket.onmessage = async (event: MessageEvent) => {
            const payload = await event.data.text();

            if (this.messageEncrypted(payload)) {
                const requestMessage = this.parseRequest(this.aes.decrypt(payload));

                if (requestMessage.AnswerId)
                    this.handlersAnswer.find(x => x.id == requestMessage.AnswerId)?.callback(requestMessage)

                this.pushToHandlers(requestMessage);
            }
            else {
                const request = this.parseRequest(payload);

                switch (request.Method.toLowerCase()) {
                    case "exception":
                        return;

                    case "initialize":
                        const signitureParameter = request.Parameters.find(
                            x => x.Key == "signiture");

                        if (signitureParameter) {
                            const signitureDecrypted = this.rsa.decrypt(signitureParameter.Value.toString());

                            if (signitureDecrypted)
                                this.aes.loadSigniture(signitureDecrypted);
                        }
                        break;

                    default: break;
                }
            }
        }
        this.webSocket.onclose = (event: CloseEvent) => {

        }
    }
    public on_message(callback = (request: ConnectionMessage) => undefined) {
        this.handlers.push(callback);
    }


    pushToHandlers = (message: ConnectionMessage) => {
        this.handlers.forEach(handler => handler(message));
    }

    messageEncrypted = (str: string): boolean => {
        try {
            JSON.parse(str);
        } catch (e) {
            return true;
        }
        return false;
    }
    parseRequest = (str: string): ConnectionMessage => {
        return JSON.parse(str, (key, value) => {
            return (key.toLowerCase(), value);
        });
    }
    generateId = () => {
        var d = new Date().getTime();
        var d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;//Time in microseconds since page-load or 0 if unsupported
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16;//random number between 0 and 16
            if (d > 0) {
                r = (d + r) % 16 | 0;
                d = Math.floor(d / 16);
            } else {
                r = (d2 + r) % 16 | 0;
                d2 = Math.floor(d2 / 16);
            }
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
        });
    }

    send = async (response: ConnectionMessage, waitAnswer: boolean = false, timeout: number = 3000): Promise<ConnectionMessage> => {
        return new Promise<ConnectionMessage>((resolve, reject) => {
            if (!response.Id) response.Id = this.generateId();

            if (waitAnswer) this.handlersAnswer.push({
                id: response.Id,
                callback: (request: ConnectionMessage) => {
                    resolve(request);
                    const findAnswer = this.handlersAnswer.findIndex(x => x.id == response.Id);
                    if (findAnswer) this.handlersAnswer.slice(findAnswer);
                }
            });
            else return reject();

            const json = JSON.stringify(response);
            // encrypt data with aes key
            const encrypted = this.aes.encrypt(json);
            try {
                this.webSocket.send(encrypted);

                setTimeout(() => {
                    if (!this.handlersAnswer.find(x => x.id == response.Id))
                        reject(undefined)
                }, timeout);
            } catch (exception) {
                reject(exception)
            }

        });
    }
}