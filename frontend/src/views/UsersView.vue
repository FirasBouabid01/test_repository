<template>
  <v-container>

    <!-- Header -->
    <v-row>
      <v-col>
        <h1>{{ $t('users.title') }}</h1>
      </v-col>
      <v-col class="d-flex justify-end">
        <v-btn color="primary" @click="showCreateDialog = true">
          {{ $t('users.create') }}
        </v-btn>
      </v-col>
    </v-row>

    <!-- Filters -->
    <v-row class="mb-4">
      <v-col cols="12" md="6">
        <v-text-field
          v-model="searchTerm"
          :label="$t('users.search')"
          prepend-icon="mdi-magnify"
          @input="searchUsers"
        />
      </v-col>

      <v-col cols="12" md="3">
        <v-select
          v-model="filterIsAdmin"
          :items="adminFilterOptions"
          :label="$t('users.isAdmin')"
          clearable
          @update:modelValue="searchUsers"
        />
      </v-col>

      <v-col cols="12" md="3">
        <v-select
          v-model="sortBy"
          :items="sortOptions"
          :label="$t('common.sort')"
          @update:modelValue="searchUsers"
        />
      </v-col>
    </v-row>

    <!-- Users Table -->
    <v-data-table
      :headers="headers"
      :items="users"
      :loading="loading"
      :items-per-page="pageSize"
      :page="pageNumber"
      :server-items-length="totalCount"
      @update:options="updateTable"
    >

      <!-- IsAdmin -->
      <template #item.isAdmin="{ item }">
        <v-chip
          :color="item.isAdmin ? 'red' : 'grey'"
          size="small"
        >
          {{ item.isAdmin ? 'Admin' : 'User' }}
        </v-chip>
      </template>

      <!-- Roles -->
      <template #item.roles="{ item }">
        <v-chip
          v-for="role in item.roles"
          :key="role"
          class="ma-1"
          color="primary"
          size="small"
        >
          {{ role }}
        </v-chip>
      </template>

      <!-- Permissions -->
      <template #item.permissions="{ item }">
        <v-chip
          v-for="perm in item.permissions"
          :key="perm"
          class="ma-1"
          color="green"
          size="small"
        >
          {{ perm }}
        </v-chip>
      </template>

      <!-- Actions -->
      <template #item.actions="{ item }">
        <v-btn size="small" @click="editUser(item)">
          {{ $t('users.edit') }}
        </v-btn>
        <v-btn size="small" color="error" @click="deleteUser(item)">
          {{ $t('users.delete') }}
        </v-btn>
      </template>

    </v-data-table>

  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { UserService, UserDto } from '@/services/userService'

const { t } = useI18n()

const users = ref<UserDto[]>([])
const loading = ref(false)
const pageNumber = ref(1)
const pageSize = ref(10)
const totalCount = ref(0)
const searchTerm = ref('')
const filterIsAdmin = ref<boolean | null>(null)
const sortBy = ref('username')
const showCreateDialog = ref(false)

const adminFilterOptions = [
  { title: t('common.all'), value: null },
  { title: t('common.yes'), value: true },
  { title: t('common.no'), value: false },
]

const sortOptions = [
  { title: t('users.username'), value: 'username' },
  { title: t('users.email'), value: 'email' },
  { title: t('users.firstName'), value: 'firstname' },
  { title: t('users.lastName'), value: 'lastname' },
]

const headers = [
  { title: t('users.username'), key: 'username' },
  { title: t('users.email'), key: 'email' },
  { title: t('users.firstName'), key: 'firstName' },
  { title: t('users.lastName'), key: 'lastName' },
  { title: t('users.isAdmin'), key: 'isAdmin' },
  { title: 'Roles', key: 'roles', sortable: false },
  { title: 'Permissions', key: 'permissions', sortable: false },
  { title: t('users.actions'), key: 'actions', sortable: false },
]

async function fetchUsers() {
  loading.value = true
  try {
    const response = await UserService.getUsers(
      pageNumber.value,
      pageSize.value,
      searchTerm.value,
      filterIsAdmin.value ?? undefined,
      sortBy.value
    )

    users.value = response.data
    totalCount.value = response.totalCount
  } catch (error) {
    console.error(error)
  } finally {
    loading.value = false
  }
}

function searchUsers() {
  pageNumber.value = 1
  fetchUsers()
}

function updateTable(options: any) {
  pageNumber.value = options.page
  pageSize.value = options.itemsPerPage
  fetchUsers()
}

function editUser(user: UserDto) {
  console.log('Edit', user)
}

async function deleteUser(user: UserDto) {
  if (confirm(t('common.confirm'))) {
    await UserService.deleteUser(user.id)
    fetchUsers()
  }
}

onMounted(fetchUsers)
</script>