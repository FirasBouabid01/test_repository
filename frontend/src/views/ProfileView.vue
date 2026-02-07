<template>
  <v-container fluid class="fill-height bg-grey-lighten-4">
    <v-row justify="center">
      <v-col cols="12" md="8" lg="6">
        <v-card class="elevation-2 rounded-lg" :loading="loading">
          <v-card-title class="text-h5 pa-4 bg-primary text-white">
            My Profile
          </v-card-title>

          <v-card-text class="pa-6">
            <v-alert
              v-if="error"
              type="error"
              variant="tonal"
              class="mb-4"
            >
              {{ error }}
            </v-alert>

            <div v-if="user">
              <div class="d-flex align-center mb-6">
                <v-avatar color="primary" size="80" class="mr-4">
                  <span class="text-h4 text-white">{{ user.firstName.charAt(0) }}{{ user.lastName.charAt(0) }}</span>
                </v-avatar>
                <div>
                  <h2 class="text-h5">{{ user.firstName }} {{ user.lastName }}</h2>
                  <p class="text-subtitle-1 text-grey-darken-1">@{{ user.username }}</p>
                </div>
              </div>

              <v-divider class="mb-4"></v-divider>

              <div class="text-subtitle-2 font-weight-bold mb-2 text-uppercase text-grey">Contact Information</div>
              
              <v-list density="compact">
                <v-list-item prepend-icon="mdi-email">
                  <v-list-item-title>{{ user.email }}</v-list-item-title>
                  <v-list-item-subtitle>Email</v-list-item-subtitle>
                </v-list-item>

                <v-list-item prepend-icon="mdi-phone">
                  <v-list-item-title>{{ user.phoneNumber }}</v-list-item-title>
                  <v-list-item-subtitle>Phone</v-list-item-subtitle>
                </v-list-item>

                <v-list-item prepend-icon="mdi-map-marker">
                  <v-list-item-title>{{ user.address }}</v-list-item-title>
                  <v-list-item-subtitle>Address</v-list-item-subtitle>
                </v-list-item>

                <v-list-item prepend-icon="mdi-cake-variant">
                  <v-list-item-title>{{ formatDate(user.dateOfBirth) }}</v-list-item-title>
                  <v-list-item-subtitle>Date of Birth</v-list-item-subtitle>
                </v-list-item>
              </v-list>
            </div>
            <div v-else-if="!loading" class="text-center pa-4"></div>
            <v-btn
              color="primary"
              variant="outlined"
              prepend-icon="mdi-lock-reset"
              block
              class="mt-6"
              @click="showPasswordDialog = true"
            >
              Change Password
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Change Password Dialog -->
    <v-dialog v-model="showPasswordDialog" max-width="500">
      <v-card>
        <v-card-title class="bg-primary text-white">Change Password</v-card-title>
        <v-card-text class="pt-4">
          <v-form ref="passwordForm" v-model="passwordValid" @submit.prevent="handleChangePassword">
            <v-text-field
              v-model="currentPassword"
              label="Current Password"
              type="password"
              variant="outlined"
              :rules="[rules.required]"
            ></v-text-field>

            <v-text-field
              v-model="newPassword"
              label="New Password"
              type="password"
              variant="outlined"
              :rules="[rules.required, rules.minLength]"
            ></v-text-field>

            <v-text-field
              v-model="confirmNewPassword"
              label="Confirm New Password"
              type="password"
              variant="outlined"
              :rules="[rules.required, rules.passwordMatch]"
            ></v-text-field>

            <v-alert v-if="passwordError" type="error" variant="tonal" class="mb-4" closable>
              {{ passwordError }}
            </v-alert>

            <v-alert v-if="passwordSuccess" type="success" variant="tonal" class="mb-4" closable>
              {{ passwordSuccess }}
            </v-alert>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="closePasswordDialog">Cancel</v-btn>
          <v-btn color="primary" :loading="passwordLoading" :disabled="!passwordValid" @click="handleChangePassword">
            Update Password
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'

const router = useRouter()
const user = ref<any>(null)
const loading = ref(false)
const error = ref('')

// Password Change State
const showPasswordDialog = ref(false)
const passwordForm = ref()
const passwordValid = ref(false)
const passwordLoading = ref(false)
const passwordError = ref('')
const passwordSuccess = ref('')
const currentPassword = ref('')
const newPassword = ref('')
const confirmNewPassword = ref('')

const rules = {
  required: (v: string) => !!v || 'Field is required',
  minLength: (v: string) => v.length >= 6 || 'Password must be at least 6 characters',
  passwordMatch: (v: string) => v === newPassword.value || 'Passwords do not match'
}

const fetchProfile = async () => {
  const token = localStorage.getItem('token')
  if (!token) {
    router.push('/')
    return
  }

  loading.value = true
  try {
    const response = await fetch('http://localhost:5112/api/users/profile', {
      headers: {
        'Authorization': `Bearer ${token}`
      }
    })

    if (!response.ok) {
      if (response.status === 401) {
        localStorage.removeItem('token')
        router.push('/')
        return
      }
      throw new Error('Failed to fetch profile')
    }

    user.value = await response.json()
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'An error occurred'
  } finally {
    loading.value = false
  }
}

const handleChangePassword = async () => {
  passwordError.value = ''
  passwordSuccess.value = ''
  
  const { valid } = await passwordForm.value.validate()
  if (!valid) return

  passwordLoading.value = true
  const token = localStorage.getItem('token')

  try {
    const response = await fetch('http://localhost:5112/api/users/change-password', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
      },
      body: JSON.stringify({
        currentPassword: currentPassword.value,
        newPassword: newPassword.value,
        confirmNewPassword: confirmNewPassword.value
      })
    })

    if (!response.ok) {
      const data = await response.json()
      throw new Error(data.error || 'Failed to change password')
    }

    passwordSuccess.value = 'Password changed successfully!'
    setTimeout(() => {
      closePasswordDialog()
    }, 1500)
    
  } catch (err) {
    passwordError.value = err instanceof Error ? err.message : 'An error occurred'
  } finally {
    passwordLoading.value = false
  }
}

const closePasswordDialog = () => {
  showPasswordDialog.value = false
  currentPassword.value = ''
  newPassword.value = ''
  confirmNewPassword.value = ''
  passwordError.value = ''
  passwordSuccess.value = ''
  if (passwordForm.value) passwordForm.value.resetValidation()
}

const formatDate = (dateString: string) => {
  if (!dateString) return ''
  return new Date(dateString).toLocaleDateString(undefined, {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

onMounted(() => {
  fetchProfile()
})
</script>
