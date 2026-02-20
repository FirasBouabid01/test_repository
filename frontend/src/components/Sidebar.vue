<template>
  <v-navigation-drawer
    v-if="auth.isAuthenticated"
    v-model="drawer"
    app
    permanent
    elevation="4"
    class="sidebar-bg"
  >
    <v-list density="comfortable" nav>
      <v-list-item
        prepend-icon="mdi-home"
        :title="$t('common.home')"
        to="/"
        value="home"
      ></v-list-item>
      
      <v-list-item
        prepend-icon="mdi-view-dashboard"
        :title="$t('common.dashboard')"
        to="/dashboard"
        value="dashboard"
      ></v-list-item>

      <!-- 👑 Admin Only -->
      <v-list-item
        v-if="isAdmin()"
        prepend-icon="mdi-shield-account"
        :title="$t('common.roles')"
        to="/roles"
        value="roles"
      ></v-list-item>

      <v-list-item
        v-if="isAdmin()"
        prepend-icon="mdi-key"
        :title="$t('common.permissions')"
        to="/permissions"
        value="permissions"
      ></v-list-item>

      <v-list-item
        v-if="isAdmin()"
        prepend-icon="mdi-account-group"
        :title="$t('users.title')"
        to="/users"
        value="users"
      ></v-list-item>

      <v-list-item
        prepend-icon="mdi-shield-check"
        :title="$t('myPermissions.title')"
        to="/my-permissions"
        value="my-permissions"
      ></v-list-item>

      <v-list-item
        prepend-icon="mdi-account"
        :title="$t('common.profile')"
        to="/profile"
        value="profile"
      ></v-list-item>
    </v-list>

    <template v-slot:append>
      <div class="pa-2">
        <v-btn block color="error" prepend-icon="mdi-logout" @click="handleLogout">
          {{ $t('common.logout') }}
        </v-btn>
      </div>
    </template>
  </v-navigation-drawer>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { auth } from '../store/auth'
import { isAdmin } from '../utils/auth'
import { useRouter } from 'vue-router'

const drawer = ref(true)
const router = useRouter()

const handleLogout = () => {
  auth.logout()
  router.push('/login')
}
</script>

<style scoped>
.sidebar-bg {
  border-right: 1px solid rgba(0, 0, 0, 0.12);
}
</style>
