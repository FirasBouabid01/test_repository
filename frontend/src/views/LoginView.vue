<template>
  <v-container fluid class="fill-height bg-gradient">
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-card class="elevation-12 rounded-xl">
          <v-card-title class="text-h4 font-weight-bold text-center pa-6 bg-primary">
            <span class="text-white">Welcome Back</span>
          </v-card-title>
          
          <v-card-text class="pa-8">
            <v-form ref="form" v-model="valid" @submit.prevent="handleLogin">
              <v-text-field
                v-model="email"
                label="Email"
                prepend-inner-icon="mdi-email"
                :rules="[rules.required, rules.email]"
                variant="outlined"
                class="mb-2"
              />

              <v-text-field
                v-model="password"
                label="Password"
                prepend-inner-icon="mdi-lock"
                :type="showPassword ? 'text' : 'password'"
                :append-inner-icon="showPassword ? 'mdi-eye' : 'mdi-eye-off'"
                @click:append-inner="showPassword = !showPassword"
                :rules="[rules.required]"
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

              <v-btn
                type="submit"
                color="primary"
                size="large"
                block
                :loading="loading"
                :disabled="!valid"
                class="mb-4"
              >
                Sign In
              </v-btn>

              <v-divider class="my-4" />

              <div class="text-center">
                <span class="text-body-2">Don't have an account?</span>
                <v-btn
                  variant="text"
                  color="primary"
                  to="/register"
                  class="ml-2"
                >
                  Register Now
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
import { auth } from '../store/auth'

const router = useRouter()
const form = ref()
const valid = ref(false)
const loading = ref(false)
const error = ref('')
const showPassword = ref(false)

const email = ref('')
const password = ref('')

const rules = {
  required: (value: string) => !!value || 'This field is required',
  email: (value: string) => {
    const pattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/
    return pattern.test(value) || 'Invalid email address'
  }
}

const handleLogin = async () => {
  error.value = ''
  
  if (!form.value) return
  
  const { valid: isValid } = await form.value.validate()
  if (!isValid) return

  loading.value = true

  try {
    const response = await fetch('http://localhost:5112/api/users/login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        email: email.value,
        password: password.value,
      }),
    })

    if (!response.ok) {
      const errorData = await response.json()
      throw new Error(errorData.error || 'Login failed')
    }

    const data = await response.json()
    
    // Store token and update state
    auth.login(data.token)
    
    // Redirect to dashboard
    router.push('/dashboard')
    
  } catch (err) {
    error.value = err instanceof Error ? err.message : 'An error occurred during login'
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
