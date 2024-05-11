import { createApp } from 'vue'

import App from './App.vue'
import router from './routers';
import { WebSocketPlugin } from './plugins/webSocketPlugin';

const app = createApp(App)

app.use(WebSocketPlugin,
    { url: "http://localhost:5244/socket" });

app.use(router)

app.mount('#app')