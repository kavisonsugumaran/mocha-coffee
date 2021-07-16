<template>
  <div>
    <h1 id="ordersTitle">
      Sales Orders
    </h1>
    <hr />
    <table id="sales-orders" class="table" v-if="orders.length">
      <thead>
        <tr>
          <th>CustomerId</th>
          <th>OrderId</th>
          <th>Order Total</th>
          <th>Order Status</th>
          <th>Mark Complete</th>
        </tr>
        <tr v-for="order in orders" :key="order.id">
          <td>
            {{ order.customer.id }}
          </td>
          <td>
            {{ order.id }}
          </td>
          <td>
            {{ getTotal(order) }}
          </td>
          <td :class="{ green: order.isPaid }">
            {{ getStatus(order.isPaid) }}
          </td>
          <td>
            <div v-if="!order.isPaid" class="lni lni-check-mark-circle order-complete" @click="markComplete(order.id)">
            </div>
          </td>
        </tr>
      </thead>
    </table>
  </div>
</template>

<script>
import axios from 'axios';

export default {
  name: 'Orders',
  data() {
    return {
      orders: [],
    }
  },
  methods: {
    async initialize() {
      await axios.get(`${this.API_URL}/order`).then((resp) => {
        this.orders = resp.data;
      });
    },
    getTotal(order) {
      return order.salesOrderItems.reduce((a, b) => a + (b['product']['price'] * b['quantity']), 0 )
    },
    getStatus(isPaid) {
      return isPaid ? "Paid in Full" : "Unpaid";
    },
    async markComplete(orderId) {
      await axios.patch(`${this.API_URL}/order/complete/${orderId}`).then((resp) => {
        console.log(resp.data);
      });
      await this.initialize();
    },
  },
  async created() {
    this.API_URL = process.env.VUE_APP_API_URL;
    await this.initialize();
  },


}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.green {
  font-weight: bold;
  color: $mocha-green;
}

.order-complete {
  cursor: pointer;
  text-align: center;
}
</style>