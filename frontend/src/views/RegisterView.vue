<template>
  <v-container fluid class="fill-height bg-gradient">
    <v-row align="center" justify="center">
      <v-col cols="12" sm="10" md="8" lg="6">
        <v-card class="elevation-12 rounded-xl">
          <v-card-title class="text-h4 font-weight-bold text-center pa-6 bg-primary">
            <span class="text-white">Create Account</span>
          </v-card-title>
          
          <v-card-text class="pa-8">
            <v-form ref="form" v-model="valid" @submit.prevent="handleRegister">
              <!-- Username -->
              <v-text-field
                v-model="formData.username"
                label="Username *"
                prepend-inner-icon="mdi-account"
                :rules="[rules.required]"
                variant="outlined"
                class="mb-2"
              />

              <!-- Email -->
              <v-text-field
                v-model="formData.email"
                label="Email *"
                prepend-inner-icon="mdi-email"
                :rules="[rules.required, rules.email]"
                variant="outlined"
                class="mb-2"
              />

              <!-- First Name and Last Name -->
              <v-row>
                <v-col cols="6">
                  <v-text-field
                    v-model="formData.firstName"
                    label="First Name *"
                    prepend-inner-icon="mdi-account-outline"
                    :rules="[rules.required]"
                    variant="outlined"
                  />
                </v-col>
                <v-col cols="6">
                  <v-text-field
                    v-model="formData.lastName"
                    label="Last Name *"
                    :rules="[rules.required]"
                    variant="outlined"
                  />
                </v-col>
              </v-row>

              <!-- Date of Birth -->
              <v-text-field
                v-model="formData.dateOfBirth"
                label="Date of Birth *"
                prepend-inner-icon="mdi-calendar"
                type="date"
                :rules="[rules.required, rules.dateOfBirth]"
                variant="outlined"
                class="mb-2"
              />

              <!-- Phone Number -->
              <v-text-field
                v-model="formData.phoneNumber"
                label="Phone Number *"
                prepend-inner-icon="mdi-phone"
                :rules="[rules.required, rules.phone]"
                variant="outlined"
                class="mb-2"
              />

              <!-- Address -->
              <v-text-field
                v-model="formData.address"
                label="Address *"
                prepend-inner-icon="mdi-map-marker"
                :rules="[rules.required]"
                variant="outlined"
                class="mb-2"
              />

              <!-- Password -->
              <v-text-field
                v-model="formData.password"
                label="Password *"
                prepend-inner-icon="mdi-lock"
                :type="showPassword ? 'text' : 'password'"
                :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                @click:append-inner="showPassword = !showPassword"
                :rules="[rules.required, rules.minLength]"
                variant="outlined"
                class="mb-2"
              />

              <!-- Confirm Password -->
              <v-text-field
                v-model="formData.confirmPassword"
                label="Confirm Password *"
                prepend-inner-icon="mdi-lock-check"
                :type="showConfirmPassword ? 'text' : 'password'"
                :append-inner-icon="showConfirmPassword ? 'mdi-eye' : 'mdi-eye-off'"
                @click:append-inner="showConfirmPassword = !showConfirmPassword"
                :rules="[rules.required, rules.passwordMatch]"
                variant="outlined"
                class="mb-4"
              />

              <v-alert
                v-if="error"
                type="error"
                variant="tonal"
                class="mb-4"
                closable
                @click:close="error = ''"
              >
                {{ error }}
              </v-alert>

              <v-alert
                v-if="success"
                type="success"
                variant="tonal"
                class="mb-4"
              >
                {{ success }}
              </v-alert>

              <v-btn
                type="submit"
                color="primary"
                size="large"
                block
                :loading="loading"
                :disabled="!valid"
                class="mb-4"
              >
                Register
              </v-btn>

              <v-divider class="my-4" />

              <div class="text-center">
                <span class="text-body-2">Already have an account?</span>
                <v-btn
                  variant="text"
                  color="primary"
                  @click="$router.push('/')"
                  class="ml-2"
                >
                  Sign In
                </v-btn>
              </div>
            </v-form>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const router = useRouter()
const form = ref()
const valid = ref(false)
const loading = ref(false)
const error = ref('')
const success = ref('')
const showPassword = ref(false)
const showConfirmPassword = ref(false)

const formData = ref({
  username: '',
  email: '',
  firstName: '',
  lastName: '',
  dateOfBirth: '',
  phoneNumber: '',
  address: '',
  password: '',
  confirmPassword: ''
})

const rules = {
  required: (value: string) => !!value || t('validation.required'),
  email: (value: string) => {
    const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return pattern.test(value) || t('validation.email')
  },
  phone: (value: string) => {
    const pattern = /^[\d\s\-+()]+$/
    return (value.length >= 10 && pattern.test(value)) || t('validation.phone')
  },
  dateOfBirth: (value: string) => {
    if (!value) return t('validation.required')
    const date = new Date(value)
    const today = new Date()
    const age = today.getFullYear() - date.getFullYear()
    return age >= 13 || t('register.ageError')
  },
  minLength: (value: string) => value.length >= 6 || t('validation.minLength'),
  passwordMatch: (value: string) => value === formData.value.password || t('validation.passwordMatch')
}

const handleRegister = async () => {
  error.value = ''
  success.value = ''
  
  if (!form.value) return
  
  const { valid: isValid } = await form.value.validate()
  if (!isValid) return

  loading.value = true

  try {
    const response = await fetch('http://localhost:5112/api/users', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        username: formData.value.username,
        email: formData.value.email,
        firstName: formData.value.firstName,
        lastName: formData.value.lastName,
        password: formData.value.password,
        dateOfBirth: formData.value.dateOfBirth,
        phoneNumber: formData.value.phoneNumber,
        address: formData.value.address,
      }),
    })

    if (!response.ok) {
      const errorData = await response.json()
      throw new Error(errorData.error || t('common.error'))
    }

    success.value = t('register.success')
    
    setTimeout(() => {
      router.push('/')
    }, 2000)
  } catch (err) {
    error.value = err instanceof Error ? err.message : t('common.error')
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.bg-gradient {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  min-height: 100vh;
}
</style>
