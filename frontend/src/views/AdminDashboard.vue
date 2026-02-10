<template>
  <v-container>
    <h1 class="mb-4">Admin Dashboard</h1>

    <v-row>
      <v-col cols="12" md="4">
        <v-card class="pa-4" elevation="2">
          <h3>👥 Users</h3>
          <p>Manage users and assign roles</p>
          <v-btn color="primary" @click="$router.push('/users')">
            Go
          </v-btn>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="pa-4" elevation="2">
          <h3>🛡️ Roles</h3>
          <p>Create roles and manage permissions</p>
          <v-btn color="primary" @click="$router.push('/roles')">
            Go
          </v-btn>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="pa-4" elevation="2">
          <h3>🔑 Permissions</h3>
          <p>Assign permissions to roles</p>
          <v-btn color="primary" @click="$router.push('/permissions')">
            Go
          </v-btn>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
</script>