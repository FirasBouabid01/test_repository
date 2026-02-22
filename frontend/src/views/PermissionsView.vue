<template>
  <div>
    <v-container>
      <v-row>
        <v-col>
          <h1>{{ $t('permissions.title') }}</h1>
          <p>{{ $t('permissions.readonly') }}</p>
        </v-col>
      </v-row>

      <!-- Search -->
      <v-row class="mb-4">
        <v-col cols="12" md="6">
          <v-text-field
            v-model="searchTerm"
            :label="$t('permissions.search')"
            prepend-icon="mdi-magnify"
            @input="searchPermissions"
          />
        </v-col>
      </v-row>

      <!-- Permissions Table (Read-only) -->
      <v-data-table
        :headers="headers"
        :items="permissions"
        :loading="loading"
        :items-per-page="pageSize"
        :page="pageNumber"
        :server-items-length="totalCount"
        @update:options="updateTable"
      />
    </v-container>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'

// Local PermissionDto type and fetch-based loader (avoid missing service import)
export interface PermissionDto {
  id: number
  name: string
  roleCount: number
  userCount: number
}

const { t } = useI18n()

const permissions = ref<PermissionDto[]>([])
const loading = ref(false)
const pageNumber = ref(1)
const pageSize = ref(10)
const totalCount = ref(0)
const searchTerm = ref('')

const headers = [
  { title: t('permissions.name'), key: 'name' },
  { title: t('permissions.roleCount'), key: 'roleCount' },
  { title: t('permissions.userCount'), key: 'userCount' },
]

async function fetchPermissions() {
  loading.value = true
  try {
    const params = new URLSearchParams({
      page: String(pageNumber.value),
      pageSize: String(pageSize.value),
      search: searchTerm.value || '',
    })
    const res = await fetch(`/api/permissions?${params.toString()}`)
    if (!res.ok) throw new Error(await res.text())
    const json = await res.json()
    // Expecting { data: PermissionDto[], totalCount: number }
    permissions.value = json.data || []
    totalCount.value = json.totalCount || 0
  } catch (error) {
    console.error('Error fetching permissions:', error)
  } finally {
    loading.value = false
  }
}

function searchPermissions() {
  pageNumber.value = 1
  fetchPermissions()
}

function updateTable(options: any) {
  pageNumber.value = options.page
  pageSize.value = options.itemsPerPage
  fetchPermissions()
}

onMounted(() => {
  fetchPermissions()
})
</script>
