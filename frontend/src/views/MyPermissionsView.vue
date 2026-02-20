<template>
  <v-container>
    <v-row class="mb-4" align="center">
      <v-col>
        <h1 class="text-h4 font-weight-bold">{{ $t('myPermissions.title') }}</h1>
        <p class="text-subtitle-1 text-medium-emphasis">{{ $t('myPermissions.subtitle') }}</p>
      </v-col>
    </v-row>

    <v-row>
      <!-- Roles Card -->
      <v-col cols="12" md="6">
        <v-card elevation="2" class="rounded-xl h-100">
          <v-card-title class="bg-primary text-white pa-4 d-flex align-center">
            <v-icon start icon="mdi-account-group" class="mr-2"></v-icon>
            {{ $t('myPermissions.roles') }}
          </v-card-title>
          <v-card-text class="pa-6">
            <div v-if="loading" class="text-center pa-4">
              <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </div>
            <v-chip-group v-else-if="roles.length > 0">
              <v-chip
                v-for="role in roles"
                :key="role"
                color="primary"
                variant="flat"
                class="ma-1"
                size="large"
              >
                {{ role }}
              </v-chip>
            </v-chip-group>
            <v-alert
              v-else
              type="info"
              variant="tonal"
              :text="$t('myPermissions.noRoles')"
              icon="mdi-information"
            ></v-alert>
          </v-card-text>
        </v-card>
      </v-col>

      <!-- Permissions Card -->
      <v-col cols="12" md="6">
        <v-card elevation="2" class="rounded-xl h-100">
          <v-card-title class="bg-secondary text-white pa-4 d-flex align-center">
            <v-icon start icon="mdi-shield-check" class="mr-2"></v-icon>
            {{ $t('myPermissions.permissions') }}
          </v-card-title>
          <v-card-text class="pa-6">
            <div v-if="loading" class="text-center pa-4">
              <v-progress-circular indeterminate color="secondary"></v-progress-circular>
            </div>
            <v-list v-else-if="permissions.length > 0" lines="one" class="bg-transparent">
              <v-list-item
                v-for="perm in permissions"
                :key="perm"
                prepend-icon="mdi-check-circle-outline"
                class="mb-1 rounded-lg border"
                :title="perm"
              >
              </v-list-item>
            </v-list>
            <v-alert
              v-else
              type="info"
              variant="tonal"
              :text="$t('myPermissions.noPermissions')"
              icon="mdi-information"
            ></v-alert>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Error Alert -->
    <v-snackbar v-model="error" color="error" timeout="5000">
      {{ errorMessage }}
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

const roles = ref<string[]>([])
const permissions = ref<string[]>([])
const loading = ref(true)
const error = ref(false)
const errorMessage = ref('')

const fetchData = async () => {
  loading.value = true
  error.value = false
  try {
    const response = await fetch('http://localhost:5112/api/users/me/permissions', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    })
    
    if (!response.ok) {
      throw new Error(t('permissions.fetchError'))
    }

    const data = await response.json()
    roles.value = data.roles
    permissions.value = data.permissions
  } catch (err: any) {
    error.value = true
    errorMessage.value = err.message
  } finally {
    loading.value = false
  }
}

onMounted(fetchData)
</script>

<style scoped>
.v-card-title {
  font-size: 1.25rem;
  letter-spacing: 0.0125em;
}
.border {
  border: 1px solid rgba(0, 0, 0, 0.12) !important;
}
</style>
