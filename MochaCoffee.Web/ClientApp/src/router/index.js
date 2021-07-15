import { createWebHistory, createRouter } from "vue-router";
import Inventory from "@/views/Inventory.vue"

const routes = [
    {
        path: "/",
        name: "Home",
        component: Inventory,
    },
    {
        path: "/inventory",
        name: "Inventory",
        component: Inventory,
    },
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;