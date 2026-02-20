import { createRouter, createWebHistory } from "vue-router";
import { isAdmin } from "../utils/auth";

const routes = [
  {
    path: "/",
    name: "Home",
    component: () => import("../views/HomeView.vue"),
  },

  // 👤 LOGIN (واحد للجميع)
  {
    path: "/login",
    name: "Login",
    component: () => import("../views/LoginView.vue"),
  },

  {
    path: "/register",
    name: "Register",
    component: () => import("../views/RegisterView.vue"),
  },

  // 👤 USER DASHBOARD
  {
    path: "/dashboard",
    name: "UserDashboard",
    component: () => import("../views/DashboardView.vue"),
    meta: { requiresAuth: true },
  },

  // 👑 ADMIN DASHBOARD
  {
    path: "/admin/dashboard",
    name: "AdminDashboard",
    component: () => import("../views/AdminDashboard.vue"),
    meta: { requiresAuth: true, requiresAdmin: true },
  },
  {
    path: "/roles",
    name: "RoleManagement",
    component: () => import("../views/RolesView.vue"),
    meta: { requiresAuth: true, requiresAdmin: true },
  },
  {
    path: "/permissions",
    name: "PermissionManagement",
    component: () => import("../views/PermissionsView.vue"),
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

// 🔐 GLOBAL AUTH + ROLE GUARD
router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("token");

  // 🔒 requires login
  if (to.meta.requiresAuth && !token) {
    return next("/login");
  }

  // 👑 admin always goes to admin dashboard
  if (isAdmin() && to.name === "UserDashboard") {
    return next("/admin/dashboard");
  }

  // ❌ non-admins cannot access admin dashboard
  if (to.meta.requiresAdmin && !isAdmin()) {
    return next("/dashboard");
  }

  next();
});