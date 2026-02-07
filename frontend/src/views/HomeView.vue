<template>
  <v-container fluid class="fill-height bg-gradient">
    <v-row align="center" justify="center" class="text-center">
      <v-col cols="12" md="8" lg="6">
        <v-icon size="100" color="white" class="mb-6">mdi-rocket-launch</v-icon>
        <h1 class="text-h2 font-weight-bold text-white mb-4">
          Welcome to Our Platform
        </h1>
        <p class="text-h5 text-white mb-8 opacity-90">
          Experience the next generation of web applications. Secure, fast, and reliable.
        </p>
        
        <div class="d-flex justify-center gap-4">
          <v-btn
            size="x-large"
            color="white"
            variant="flat"
            class="text-primary font-weight-bold px-8 mr-4"
            to="/register"
            prepend-icon="mdi-account-plus"
          >
            Get Started
          </v-btn>
          
          <v-btn
            size="x-large"
            color="white"
            variant="outlined"
            class="font-weight-bold px-8"
            to="/login"
            prepend-icon="mdi-login"
          >
            Login
          </v-btn>
        </div>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
// No script needed for simple landing page
</script>

<style scoped>
.bg-gradient {
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  min-height: 100vh;
}
.gap-4 {
  gap: 16px;
}
</style>
