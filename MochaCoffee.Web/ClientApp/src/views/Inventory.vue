<template>
  <div class="inventory-container">
    <h1 class="inventory-title">
      Inventory Dashboard
    </h1>
    <hr/>

    <div class="inventory-actions">
      <mocha-button id="addNewBtn" @click="showNewProductModal">
        Add New Item
      </mocha-button>
      <mocha-button id="receiveShipmentBtn" @click="showShipmentModal">
        Receive Shipment
      </mocha-button>
    </div>

    <table id="inventoryTable" class="table">
      <tr>
        <th>Item</th>
        <th>Quantity On-Hand</th>
        <th>Unit Price</th>
        <th>Taxable</th>
        <th>Delete</th>
      </tr>

      <tr v-for="item in inventory" :key="item.id">
        <td>
          {{ item.product.name }}
        </td>
        <td :class="`${applyColor(item.quantityOnHand, item.idealQuantity)}`">
          {{ item.quantityOnHand }}
        </td>
        <td>
          LKR {{ (item.product.price).toFixed(2) }}
        </td>
        <td>
          <span v-if="item.product.isTaxable">
            Yes
          </span>
          <span v-else>
            No
          </span>
        </td>
        <td class="lni lni-cross-circle product-archive" @click="archiveProduct(item.product.id)">
        </td>
      </tr>
    </table>

    <new-product-modal @save-product="saveNewProduct" v-if="isNewProductVisible" @close="closeModals" />
    <shipment-modal @save-shipment="saveNewShipment" v-if="isShipmentVisible" :inventory="inventory" @close="closeModals" />
  </div>
</template>

<script>
import MochaButton from '../components/MochaButton.vue';
import ShipmentModal from '../components/Modals/ShipmentModal.vue';
import NewProductModal from '../components/Modals/NewProductModal.vue';
import axios from 'axios';


export default {
  name: "Inventory",
  components: { MochaButton, ShipmentModal, NewProductModal },
  data() {
    return {
      inventory:[],
      isNewProductVisible: false,
      isShipmentVisible: false,
      API_URL: "",
    };
  },
  methods: {
    applyColor(current, target) {
      if (current <= 0) {
        return "red";
      } else if (Math.abs(target-current) > 8) {
        return "yellow";
      } else return "green";
    },
    closeModals() {
      this.isNewProductVisible = false;
      this.isShipmentVisible = false;
    },
    async saveNewShipment(shipment) {
      await axios.patch(`${this.API_URL}/inventory`, shipment).then((resp) => {
        console.log(resp);
      })
      this.isShipmentVisible = false;
      await this.initialize();
    },
    showNewProductModal() {
      this.isNewProductVisible = true;
    },
    showShipmentModal() {
      this.isShipmentVisible = true;
    },
    async initialize() {
      await axios.get(`${this.API_URL}/inventory`).then((resp) => {
        this.inventory = resp.data;
        console.log(resp);
      });
    },
    async archiveProduct(productId) {
      await axios.patch(`${this.API_URL}/product/${productId}`).then((resp) => {
        console.log(resp);
      })
      await this.initialize();
    },
    async saveNewProduct(newProduct) {
      await axios.post(`${this.API_URL}/product`, newProduct).then((resp) => {
        console.log(resp);
      })
      this.isNewProductVisible = false;
      await this.initialize();
    },
  },
  async created() {
    this.API_URL = process.env.VUE_APP_API_URL;
    await this.initialize();
  }
}
</script>

<style lang="scss" scoped>
@import "@/scss/global.scss";

.green {
  font-weight: bold;
  color: $mocha-green;
}

.yellow {
  font-weight: bold;
  color: $mocha-yellow;
}

.red {
  font-weight: bold;
  color: $mocha-red;
}

.inventory-actions {
  display: flex;
  margin-bottom: 0.8rem;
}

.product-archive {
  cursor: pointer;
  font-weight: bold;
  font-size: 1.2rem;
  color: $mocha-red;
}
</style>