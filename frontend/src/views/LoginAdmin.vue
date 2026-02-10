<template>
  <v-container class="fill-height" fluid>
    <v-row align="center" justify="center">
      <v-col cols="12" sm="8" md="4">
        <v-card elevation="4">
          <v-card-title class="text-center">
            <span class="text-h6">Admin Login</span>
          </v-card-title>

          <v-card-text>
            <v-text-field
              label="Email"
              v-model="email"
              type="email"
              prepend-icon="mdi-email"
              required
            />

            <v-text-field
              label="Password"
              v-model="password"
              type="password"
              prepend-icon="mdi-lock"
              required
            />

            <v-alert
              v-if="error"
              type="error"
              variant="outlined"
              class="mt-2"
            >
              {{ error }}
            </v-alert>
          </v-card-text>

          <v-card-actions>
            <v-btn
              color="primary"
              block
              @click="login"
            >
              LOGIN
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";

const email = ref("admin@test.com");
const password = ref("");
const error = ref("");
const router = useRouter();

const login = async () => {
  error.value = "";

  try {
    const res = await axios.post(
      "http://localhost:5112/api/Users/login",
      {
        email: email.value,
        password: password.value,
      }
    );

    localStorage.setItem("token", res.data.token);
    router.push("/dashboard");
  } catch {
    error.value = "Invalid email or password";
  }
};
</script>