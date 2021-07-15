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
        <td>
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
        <td>
          X
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


export default {
  name: "Inventory",
  components: { MochaButton, ShipmentModal, NewProductModal },
  data() {
    return {
      inventory:[
        {
          id: 1,
          product: {
            id: 1,
            name: "Some Product",
            description: "Good Stuff",
            price: 100,
            createdOn: new Date(),
            updatedOn: new Date(),
            isTaxable: true,
            isArchived: false,
          },
          quantityOnHand: 100,
          idealQuantity: 100
        },
        {
          id: 2,
          product: {
            id: 1,
            name: "Another Product",
            description: "Good Stuff",
            price: 100,
            createdOn: new Date(),
            updatedOn: new Date(),
            isTaxable: false,
            isArchived: false,
          },
          quantityOnHand: 40,
          idealQuantity: 20
        },
      ],
      isNewProductVisible: false,
      isShipmentVisible: false,
    };
  },
  methods: {
    closeModals() {
      this.isNewProductVisible = false;
      this.isShipmentVisible = false;
    },
    saveNewProduct(newProduct) {
      console.log(newProduct);
    },
    saveNewShipment(shipment) {
      console.log(shipment);
    },
    showNewProductModal() {
      this.isNewProductVisible = true;
    },
    showShipmentModal() {
      this.isShipmentVisible = true;
    },
  },
}
</script>

<style lang="scss" scoped>
@import "@/scss/global.scss";
</style>