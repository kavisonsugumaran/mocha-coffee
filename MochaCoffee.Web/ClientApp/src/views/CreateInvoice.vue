<template>
  <div>
    <h1 id="invoiceTitle">
      Create Invoice
    </h1>
    <hr>

    <div class="invoice-step" v-if="invoiceStep === 1">
      <h2>Step 1: Select Customer</h2>
      <div v-if="customers.length" class="invoice-step-detail">
        <label for="customer">Customer:</label>
        <select v-model="selectedCustomerId" class="invoice-customers" id="customer" >
          <option disabled value="">Please select a Customer</option>
          <option v-for="c in customers" :value="c.id" :key="c.id">
            {{ c.firstName + " " + c.lastName }}
          </option>
        </select>
      </div>
    </div>

    <div class="invoice-step" v-if="invoiceStep === 2">
      <h2>Step 2: Create Order</h2>

      <div v-if="inventory.length" class="invoice-step-detail">
        <label for="product">Product:</label>
        <select v-model="newItem.product" class="invoiceLineItem" id="product">
          <option disabled value="">Please select a Product</option>
          <option v-for="i in inventory" :value="i.product" :key="i.product.id">
            {{ i.product.name }}
          </option>
        </select>
        <label for="quantity">Quantity:</label>
        <input v-model="newItem.quantity" id="quantity" type="number" min="0" />
      </div>

      <div class="invoice-item-actions">
        <mocha-button :disabled="!newItem.product || !newItem.quantity" @click-event="addLineItem">
          Add Line Item
        </mocha-button>
        <mocha-button :disabled="!lineItems.length" @click-event="finalizeOrder">
          Finalize Order
        </mocha-button>
      </div>

      <div class="invoice-order-list" v-if="lineItems.length">
        <div class="runningTotal">
          <h3>Running Total:</h3>
          {{ runningTotal  }}
        </div>
        <hr />
        <table class="table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Description</th>
              <th>Qty.</th>
              <th>Price</th>
              <th>Subtotal</th>
            </tr>
          </thead>
          <tr v-for="lineItem in lineItems" :key="`index_${lineItem.product.id}`">
            <td>{{ lineItem.product.name }}</td>
            <td>{{ lineItem.product.description }}</td>
            <td>{{ lineItem.quantity }}</td>
            <td>{{ lineItem.product.price }}</td>
            <td>{{ (lineItem.product.price * lineItem.quantity)  }}</td>
          </tr>
        </table>
      </div>
    </div>

    <div class="invoice-step" v-if="invoiceStep === 3">
      <h2>Step 3: Review and Submit</h2>
      <mocha-button @click-event="submitInvoice">Submit Invoice</mocha-button>
      <hr />

      <div class="invoice-step-detail" id="invoice" ref="invoice">
        <div class="invoice-logo">
          <img id="imgLogo" alt="Mocha Coffee logo" src="../assets/images/mocha_coffee_logo.png" />
          <h3>13 Coffee Lane</h3>
          <h3>Someplace, Colombo 06</h3>
          <h3>SL</h3>

          <div class="invoice-order-list" v-if="lineItems.length">
            <div class="invoice-header">
              <h3>Invoice: {{ new Date() }}</h3>
              <h3> 
                Customer: {{ selectedCustomer.firstName + " " + selectedCustomer.lastName }}
              </h3>
              <h3>
                Address: {{ selectedCustomer.primaryAddress.addressLine1 }}
              </h3>
              <h3 v-if="selectedCustomer.primaryAddress.addressLine2">
                {{ selectedCustomer.primaryAddress.addressLine2 }}
              </h3>
              <h3>
                {{ selectedCustomer.primaryAddress.city }},
                {{ selectedCustomer.primaryAddress.state }},
                {{ selectedCustomer.primaryAddress.postalCode }}
              </h3>
              <h3>
                {{ selectedCustomer.primaryAddress.country }}
              </h3>
            </div>
            <table class="table">
              <thead>
                <tr>
                  <th>Product</th>
                  <th>Description</th>
                  <th>Qty.</th>
                  <th>Price</th>
                  <th>Subtotal</th>
                </tr>
              </thead>
              <tr v-for="lineItem in lineItems" :key="`index_${lineItem.product.id}`">
                <td>{{ lineItem.product.name }}</td>
                <td>{{ lineItem.product.description }}</td>
                <td>{{ lineItem.quantity }}</td>
                <td>{{ lineItem.product.price }}</td>
                <td>
                  {{ (lineItem.product.price * lineItem.quantity) }}
                </td>
              </tr>
              <tr>
                <th colspan="4"></th>
                <th>Grand Total</th>
              </tr>
              <tfoot>
                <tr>
                  <td colspan="4" class="due">Balance due upon receipt:</td>
                  <td class="price-final">{{ runningTotal }}</td>
                </tr>
              </tfoot>
            </table>
          </div>
        </div>
      </div>
    </div>
    <hr>

    <div class="invoice-steps-actions">
      <mocha-button @click-event="prev" :disabled="!canGoPrev">
        Previous
      </mocha-button>
      <mocha-button @click-event="next" :disabled="!canGoNext">
        Next
      </mocha-button>
      <mocha-button @click-event="startOver">
        Start Over
      </mocha-button>
    </div>
  </div>
</template>

<script>
import MochaButton from '../components/MochaButton.vue';
import axios from 'axios';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';

export default {
  name: 'CreateInvoice',
  components: { MochaButton },
  data() {
    return {
      invoiceStep: 1,
      invoice: {
        createdOn: new Date(),
        customerId: 0,
        lineItems: [],
        updatedOn: new Date(),
      },
      customers: [],
      selectedCustomerId: 0,
      inventory: [],
      lineItems: [],
      newItem: {
        product: '',
        quantity: 0,
      },
    }
  },
  async created() {
    this.API_URL = process.env.VUE_APP_API_URL;
    await this.initialize();
  },
  computed: {
    canGoNext() {
      if (this.invoiceStep === 1) {
        return this.selectedCustomerId !== 0;
      }

      if (this.invoiceStep === 2) {
        return this.lineItems.length;
      }

      if (this.invoiceStep === 3) {
        return false;
      }
      return false;
    },
    canGoPrev() {
      return this.invoiceStep !== 1;
    },
    runningTotal() {
      return this.lineItems.reduce((a, b) => a + (b["product"]["price"] * b["quantity"]), 0)
    },
    selectedCustomer() {
      return this.customers.find(c => c.id == this.selectedCustomerId);
    },
  },
  methods: {
    async initialize() {
      await axios.get(`${this.API_URL}/customer`).then((resp) => {
        this.customers = resp.data;
      });
      await axios.get(`${this.API_URL}/inventory`).then((resp) => {
        this.inventory = resp.data;
      });
    },
    prev() {
      if (this.invoiceStep === 1) {
        return;
      }
      this.invoiceStep -= 1;
    },
    next() {
      if (this.invoiceStep === 3) {
        return;
      }
      this.invoiceStep += 1;
    },
    startOver() {
      this.invoice = {
        customerId: 0,
        lineItems: []
      };
      this.invoiceStep = 1;
    },
    addLineItem() {
      let newItem = {
        product : this.newItem.product,
        quantity : Number(this.newItem.quantity)
      };

      let existingItems = this.lineItems.map(item => item.product.id);

      if (existingItems.includes(newItem.product.id)) {
        let lineItem = this.lineItems.find(item => item.product.id === newItem.product.id);

        let currentQuantity = Number(lineItem.quantity);
        let updatedQuantity = (currentQuantity += newItem.quantity);
        lineItem.quantity = updatedQuantity;
      } else {
        this.lineItems.push(this.newItem);
      }

      this.newItem = {
        product : '',
        quantity : 0
      };
    },
    finalizeOrder() {
      this.invoiceStep = 3;
    },
    downloadPdf() {
      let pdf = new jsPDF("p", "pt", "a4", true);
      let invoice = document.getElementById("invoice");
      let width = this.$refs.invoice.clientWidth;
      let height = this.$refs.invoice.clientHeight;

      html2canvas(invoice).then(canvas => {
        let image = canvas.toDataURL("image/png");
        pdf.addImage(image, "PNG", 0, 0, width * 0.55, height * 0.55);
        pdf.save("invoice");
      });
    },
    async submitInvoice() {
      this.invoice = {
        customerId: this.selectedCustomerId,
        lineItems: this.lineItems,
        createdOn: new Date(),
        updatedOn: new Date()
      }
      await axios.post(`${this.API_URL}/invoice`, this.invoice).then((resp) => {
        console.log(resp);
      })
      this.downloadPdf();
      await this.$router.push("/orders");
    }
  }
}
</script>

<style scoped lang="scss">
@import "@/scss/global.scss";

.invoice-steps-actions {
  display: flex;
  width: 100%;
}

.invoice-step {

}
.invoice-step-detail {
  margin: 1.2rem;
}

.invoice-order-list {
  margin-top: 1.2rem;
  padding: 0.8rem;

  .line-item {
    display: flex;
    border-bottom: 1px dashed #ccc;
    padding: 0.8rem;
  }

  .item-col {
    flex-grow: 1;
  }
}

.invoice-item-actions {
  display: flex;
}

.price-pre-tax {
  font-weight: bold;
}

.price-final {
  font-weight: bold;
  color: $mocha-green;
}

.due {
  font-weight: bold;
}

.invoice-header {
  width: 100%;
  margin-bottom: 1.2rem;
}

.invoice-logo {
  padding-top: 1.4rem;
  text-align: center;

  img {
    width: 280px;
  }
}
</style>