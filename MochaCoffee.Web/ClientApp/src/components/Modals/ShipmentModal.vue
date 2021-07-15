<template>
  <mocha-modal>
    <template v-slot:header>
      Receive Shipment
    </template>
    <template v-slot:body>
      <label for="product">Product Received</label>

      <select v-model="selectedProduct" class="shipmentItems" id="product">
        <option disabled value="">Please select one</option>
        <option v-for="item in inventory" :value="item" :key="item.product.id">
          {{ item.product.name }}
        </option>
      </select>

      <label for="qtyReceived">Quantity Received</label>
      <input type="number" id="qtyReceived" v-model="qtyReceived">
    </template>
    <template v-slot:footer>
      <mocha-button type="button" @click-event="save" aria-label="save new shipment">
        Save Received Shipment
      </mocha-button>
      <mocha-button type="button" @click-event="close" aria-label="Close modal">
        Close
      </mocha-button>
    </template>
  </mocha-modal>
</template>

<script>
import MochaButton from '../MochaButton.vue';
import MochaModal from '../Modals/MochaModal.vue';


export default {
  name: "ShipmentModal",
  components: { MochaButton, MochaModal },
  props: ['inventory'],
  data() {
    return {
      selectedProduct: {
        createdOn: new Date(),
        updatedOn: new Date(),
        id: 0,
        description: "",
        isTaxable: false,
        name: "",
        price: 0,
        isarchived: false
      },
      qtyReceived: 0,
    };
  },
  methods: {
    close() {
      this.$emit("close");
    },
    save() {
      let shipment = {
        productId: this.selectedProduct.id,
        adjustment: this.qtyReceived,
      };

      this.$emit("save-shipment", shipment);
    }
  }
}
</script>

<style>

</style>