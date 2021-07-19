<template>
  <div>
    <h1 id="customersTitle">
      Manage Customers
    </h1>
    <hr>
    <div class="customer-actions">
      <mocha-button @click="showNewCustomerModal">
        Add Customer
      </mocha-button>
    </div>
    <table id="customers" class="table">
      <tr>
        <th>Customer</th>
        <th>Address</th>
        <th>State</th>
        <th>Since</th>
        <th>Delete</th>
      </tr>
      <tr v-for="customer in customers" :key="customer.id">
        <td>
          {{ customer.firstName + " " + customer.lastName }}
        </td>
        <td>
          {{ customer.primaryAddress.addressLine1 +
              " " +customer.primaryAddress.addressLine2 }}
        </td>
        <td>
          {{ customer.primaryAddress.state }}
        </td>
        <td>
          {{ moment(customer.createdOn).format("ddd MMM DD, YYYY") }}
        </td>
        <td>
          <div class="lni lni-cross-circle customer-delete" @click="deleteCustomer(customer.id)"> 
          </div>
        </td>
      </tr>
    </table>
    <new-customer-modal v-if="isCustomerModalVisible" @save-customer="saveNewCustomer" @close="closeModal"/>
  </div>
</template>

<script>
import MochaButton from '../components/MochaButton.vue';
import NewCustomerModal from '../components/Modals/NewCustomerModal.vue';
import axios from 'axios';
import moment from 'moment';

export default {
  name: "Customers",
  components: { MochaButton, NewCustomerModal },
  data() {
    return {
      customers: [],
      isCustomerModalVisible: false,
    }
  },
  methods: {
    showNewCustomerModal() {
      this.isCustomerModalVisible = true;
    },
    closeModal() {
      this.isCustomerModalVisible = false;
    },
    async initialize() {
      await axios.get(`${this.API_URL}/customer`).then((resp) => {
        this.customers = resp.data;
      });
    },
    async saveNewCustomer(newCustomer) {
      await axios.post(`${this.API_URL}/customer`, newCustomer).then((resp) => {
        console.log(resp);
      })
      this.isCustomerModalVisible = false;
      await this.initialize();
    },
    async deleteCustomer(customerId) {
      await axios.delete(`${this.API_URL}/customer/${customerId}`).then((resp) => {
        console.log(resp);
      })
      await this.initialize();
    },
  },
  async created() {
    this.moment = moment;
    this.API_URL = process.env.VUE_APP_API_URL;
    await this.initialize();
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.customer-actions {
  display: flex;
  margin-bottom: 0.8rem;

  div {
    margin-right: 0.8rem;
  }
}

.customer-delete {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $mocha-red;
}
</style>