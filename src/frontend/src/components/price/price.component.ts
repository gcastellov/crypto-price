import Vue from 'vue';
import Component from 'vue-class-component';
import Rate from '@/models/rate';
@Component({
  props: {
    rate: Rate,
  },
})
export default class PriceComponent extends Vue {

  public created() {
    Vue.prototype.$priceHub.connectionOpenned(this.$props.rate.crypto, this.$props.rate.currency);
  }

  public destroyed() {
    Vue.prototype.$priceHub.connectionClosed(this.$props.rate.crypto, this.$props.rate.currency);
  }

  public kill() {
    Vue.prototype.$priceHub.$emit('price-killed', this.$props.rate);
  }
}
