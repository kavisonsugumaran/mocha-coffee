<template>
  <div class="side-menu-container">
    <router-link to="/">
      <img id="imgLogo" src="../assets/images/mocha_coffee_logo.png" alt="Mocha Coffee Logo">
    </router-link>
    <h1>Management Portal</h1>
    <mocha-button id="menuInventory" @click-event="goToRoute('/inventory')" :isFullWidth="true">
      Inventory
    </mocha-button>
    <mocha-button id="menuCustomers" @click-event="goToRoute('/customers')" :isFullWidth="true">
      Manage Customers
    </mocha-button>
    <mocha-button id="menuInvoice" @click-event="goToRoute('/invoice/new')" :isFullWidth="true">
      New Invoice
    </mocha-button>
    <mocha-button id="menuOrders" @click-event="goToRoute('/orders')" :isFullWidth="true">
      Orders
    </mocha-button>
  </div>
</template>

<script>
import MochaButton from "./MochaButton.vue";
export default {
  name: "SideMenu",
  components: {
    MochaButton,
  },
  methods: {
    goToRoute(route) {
      this.$router.push(route);
    }
  }

}
</script>

<style lang="scss" scoped>
@import "@/scss/global.scss";

.side-menu-container {
  background-color: #fcfcfc;
  height: 100vh;
  width: $menu-width;
  display: flex;
  flex-direction: column;
  padding: 0.8rem;
  box-shadow: 0 1px 1px rgba(0, 0, 0, 0.12), 0 1px 1px rgba(0, 0, 0, 0.24);
  box-sizing: border-box;
}
 #imgLogo {
   width: 100%;
 }

 h1 {
   font-size: 1.2rem;
   margin: 1rem 0;
 }
</style>>
