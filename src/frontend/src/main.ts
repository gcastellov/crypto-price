import Vue from 'vue';
import App from './App.vue';
import axios from 'axios';
import BootstrapVue from 'bootstrap-vue';
import PriceHub from './plugins/price.hub';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap-vue/dist/bootstrap-vue.css';

Vue.config.productionTip = false;
axios.defaults.baseURL = 'http://localhost:65385';
Vue.prototype.$http = axios;
const priceHub = new Vue();
Vue.prototype.$priceHub = priceHub;

Vue.use(BootstrapVue);
Vue.use(PriceHub);
 Vue.use(require('vue-moment'));

new Vue({
  render: (h) => h(App),
}).$mount('#app');
