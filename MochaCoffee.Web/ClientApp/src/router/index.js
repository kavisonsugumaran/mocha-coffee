import { createWebHistory, createRouter } from "vue-router";
import Home from "@/views/Home.vue";
import Inventory from "@/views/Inventory.vue"
import Customers from "@/views/Customers.vue";
import Orders from "@/views/Orders.vue";
import CreateInvoice from "@/views/CreateInvoice.vue";

const routes = [
  {
    path: "/",
    name: "home",
    component: Home,
  },
  {
    path: "/inventory",
    name: "inventory",
    component: Inventory,
  },
  {
    path: "/customers",
    name: "customers",
    component: Customers,
  },
  {
    path: "/orders",
    name: "order",
    component: Orders,
  },
  {
    path: "/invoice/new",
    name: "create-invoice",
    component: CreateInvoice,
  },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;