import { createRouter, createWebHistory } from "vue-router";
import { getUserRole, isAuthenticated } from "../utils/auth";

const routes = [
  {
    path: "/",
    component: () => import("../views/HomeView.vue"),
  },
  {
    path: "/login",
    component: () => import("../views/LoginView.vue"),
  },
  {
  path: "/register",
  name: "Register",
  component: () => import("../views/RegisterView.vue"),
 },
  {
    path: "/dashboard",
    component: () => import("../views/DashboardView.vue"),
    meta: { requiresAuth: true },
  },
  {
    path: "/admin/dashboard",
    component: () => import("../views/AdminDashboard.vue"),
    meta: { requiresAuth: true, requiresAdmin: true },
  },
  {
  path: "/profile",
  name: "Profile",
  component: () => import("../views/ProfileView.vue"),
  meta: { requiresAuth: true },
 },
];

export const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach((to, from, next) => {
  const role = getUserRole();
  const authenticated = isAuthenticated();

  // 🔒 يحتاج تسجيل دخول
  if (to.meta.requiresAuth && !authenticated) {
    return next("/login");
  }

  // 👑 يحتاج Admin
  if (to.meta.requiresAdmin && role !== "Admin") {
    return next("/dashboard");
  }

  next();
});