<template>
  <v-container>
    <h1 class="mb-4">{{ $t('dashboards.admin.title') }}</h1>

    <v-row>
      <v-col cols="12" md="4">
        <v-card class="pa-4" elevation="2">
          <h3>👥 {{ $t('dashboards.admin.users.title') }}</h3>
          <p>{{ $t('dashboards.admin.users.subtitle') }}</p>
          <v-btn color="primary" @click="$router.push('/users')">
            {{ $t('dashboards.admin.go') }}
          </v-btn>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="pa-4" elevation="2">
          <h3>🛡️ {{ $t('dashboards.admin.roles.title') }}</h3>
          <p>{{ $t('dashboards.admin.roles.subtitle') }}</p>
          <v-btn color="primary" @click="$router.push('/roles')">
            {{ $t('dashboards.admin.go') }}
          </v-btn>
        </v-card>
      </v-col>

      <v-col cols="12" md="4">
        <v-card class="pa-4" elevation="2">
          <h3>🔑 {{ $t('dashboards.admin.permissions.title') }}</h3>
          <p>{{ $t('dashboards.admin.permissions.subtitle') }}</p>
          <v-btn color="primary" @click="$router.push('/permissions')">
            {{ $t('dashboards.admin.go') }}
          </v-btn>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup lang="ts">
</script>