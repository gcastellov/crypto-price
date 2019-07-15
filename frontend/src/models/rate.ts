import { AxiosInstance } from 'axios';
import Vue from 'vue';

export default class Rate {

    public currency: string = '';
    public crypto: string = '';
    public price: number = 0;
    public at: string = '';
    private http: AxiosInstance = Vue.prototype.$http;

    constructor(currency: string, crypto: string) {
        this.currency = currency;
        this.crypto = crypto;
    }

    public getPrice() {
        this.http.get(`/api/price?crypto=${this.crypto}&currency=${this.currency}`).then((res) => {
            this.price = res.data.price;
            this.at = res.data.at;
        });
    }
}
