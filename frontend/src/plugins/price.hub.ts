import { HubConnectionBuilder, LogLevel } from '@aspnet/signalr';
import { VueConstructor } from 'vue';

export default {
  install(Vue: VueConstructor) {
    const connection = new HubConnectionBuilder()
      .withUrl(`${Vue.prototype.$http.defaults.baseURL}/price-hub`)
      .configureLogging(LogLevel.Information)
      .build();

    connection.start();

    connection.on('SendPrice', (response) => {
      Vue.prototype.$priceHub.$emit('price-changed', { response });
    });

    Vue.prototype.$priceHub.connectionOpenned = (crypo: string, currency: string) => {
      return connection.invoke('JoinGroup', crypo, currency);
    };

    Vue.prototype.$priceHub.connectionClosed = (crypo: string, currency: string) => {
      return connection.invoke('LeaveGroup', crypo, currency);
    };
  },

};
