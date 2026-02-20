<template>
  <v-container fluid class="fill-height bg-grey-lighten-4">
    <v-row justify="center">
      <v-col cols="12" md="10" lg="8">
        <v-card class="elevation-2 rounded-lg pa-6">
          <v-card-title class="text-h4 mb-4">
            {{ $t('dashboards.user.title') }}
          </v-card-title>
          
          <v-card-text>
            <p class="text-h6 mb-4">
              {{ $t('dashboards.user.welcome') }}
            </p>
            
            <v-row>
              <v-col cols="12" md="6">
                <v-card variant="outlined" class="pa-4 h-100" @click="$router.push('/profile')" hover>
                  <div class="d-flex align-center">
                    <v-icon size="48" color="primary" class="mr-4">mdi-account-circle</v-icon>
                    <div>
                      <div class="text-h6">{{ $t('dashboards.user.profile.title') }}</div>
                      <div class="text-body-2 text-grey-darken-1">{{ $t('dashboards.user.profile.subtitle') }}</div>
                    </div>
                  </div>
                </v-card>
              </v-col>
              
              <v-col cols="12" md="6">
                <v-card variant="outlined" class="pa-4 h-100">
                  <div class="d-flex align-center">
                    <v-icon size="48" color="success" class="mr-4">mdi-shield-check</v-icon>
                    <div>
                      <div class="text-h6">{{ $t('dashboards.user.status.title') }}</div>
                      <div class="text-body-2 text-grey-darken-1">{{ $t('dashboards.user.status.subtitle') }}</div>
                    </div>
                  </div>
                </v-card>
              </v-col>
            </v-row>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()

onMounted(() => {
  const token = localStorage.getItem('token')
  if (!token) {
    router.push('/')
  }
})
</script>
