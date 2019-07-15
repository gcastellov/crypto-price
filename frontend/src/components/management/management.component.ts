import Vue from 'vue';
import Component from 'vue-class-component';
import PriceComponent from '../../components/price/price.component';
import { AxiosInstance } from 'axios';
import Rate from '@/models/rate';

@Component({
  components: {
    PriceComponent,
  },
})
export default class ManagementComponent extends Vue {

  public currency: string = '';
  public crypto: string = '';
  public availableCurrencies: string[] = [];
  public availableCryptoCurrencies: string[] = [];
  public rates: Rate[] = [];
  private http: AxiosInstance = Vue.prototype.$http;

  constructor() {
    super();
  }

  public created() {
    Vue.prototype.$priceHub.$on('price-changed', this.onPriceChanged);
    Vue.prototype.$priceHub.$on('price-killed', this.onPriceKilled);
    this.getAvailableCurrencies();
    this.getAvailableCryptoCurrencies();
  }

  public getPrice() {
    if (this.currency !== '' && this.crypto !== '') {
      const rate = new Rate(this.currency, this.crypto);
      rate.getPrice();
      this.rates.push(rate);
    }
  }

  private getAvailableCurrencies() {
    this.http.get('/api/currency/currencies').then((res) => {
      this.availableCurrencies = res.data;
    });
  }

  private getAvailableCryptoCurrencies() {
    this.http.get('/api/currency/crypto-currencies').then((res) => {
      this.availableCryptoCurrencies = res.data;
    });
  }

  private onPriceChanged(priceDto: any) {
    const response: any = priceDto.response;
    this.rates.filter((rate: Rate) => {
      if (rate.currency === response.currency && rate.crypto === response.crypto) {
        rate.price = response.price;
        rate.at = response.at;
      }
    });
  }

  private onPriceKilled(rate: Rate) {
    const index = this.rates.indexOf(rate);
    this.rates.splice(index, 1);
  }
}
