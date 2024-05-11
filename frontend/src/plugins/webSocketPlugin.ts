import Transport from '@/packages/transport/transport';
import { App, Plugin } from 'vue';

export const WebSocketPlugin: Plugin = {
    install(app: App, options: { url: string }) {
        app.config.globalProperties.$socket = new Transport(options.url);
    }
}