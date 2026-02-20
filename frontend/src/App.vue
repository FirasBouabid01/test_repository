<template>
  <v-app :dir="isRtl ? 'rtl' : 'ltr'">
    <Sidebar v-if="auth.isAuthenticated" />

    <v-app-bar color="primary" density="compact" elevation="2">
      <template v-slot:prepend>
        <v-icon color="white" class="ml-2">mdi-application</v-icon>
      </template>
      <v-app-bar-title class="text-white font-weight-bold" style="cursor: pointer" @click="router.push('/')">
        {{ $t('common.home') }}
      </v-app-bar-title>
      
      <v-spacer></v-spacer>
      
      <!-- Language Switcher -->
      <v-menu location="bottom end">
        <template v-slot:activator="{ props }">
          <v-btn icon="mdi-translate" color="white" v-bind="props" class="mr-2"></v-btn>
        </template>
        <v-list>
          <v-list-item
            v-for="lang in languages"
            :key="lang.code"
            @click="setLanguage(lang.code)"
            :active="currentLocale === lang.code"
          >
            <v-list-item-title>{{ lang.name }}</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>

      <div v-if="!auth.isAuthenticated" class="mr-4">
        <v-btn color="white" to="/login" prepend-icon="mdi-login">{{ $t('common.login') }}</v-btn>
        <v-btn color="white" variant="outlined" to="/register" class="ml-2">{{ $t('common.register') }}</v-btn>
      </div>
      <div v-else class="mr-4">
        <v-btn color="white" icon="mdi-logout" @click="logout"></v-btn>
      </div>
    </v-app-bar>

    <v-main>
      <v-container fluid>
        <router-view />
      </v-container>
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'
import { useLocale } from 'vuetify'
import { auth } from './store/auth'
import Sidebar from './components/Sidebar.vue'

const router = useRouter()
const { locale } = useI18n()
const { current: vuetifyLocale } = useLocale()

const languages = [
  { code: 'en', name: 'English' },
  { code: 'fr', name: 'Français' },
  { code: 'ar', name: 'العربية' }
]

const currentLocale = computed(() => locale.value)
const isRtl = computed(() => locale.value === 'ar')

const setLanguage = (code: string) => {
  locale.value = code
  vuetifyLocale.value = code
  localStorage.setItem('locale', code)
  document.documentElement.lang = code
}

// Initialize on load
setLanguage(localStorage.getItem('locale') || 'en')

const logout = () => {
  auth.logout()
  router.push('/')
}
</script>

<style scoped>
</style>
