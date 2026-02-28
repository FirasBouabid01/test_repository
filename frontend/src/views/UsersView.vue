<template>
  <v-container fluid>
    <v-card class="elevation-2 rounded-lg">
      <v-card-title class="d-flex align-center pa-4 bg-primary text-white">
        <v-icon start icon="mdi-account-group" class="mr-2"></v-icon>
        {{ $t('users.title') }}
        <v-spacer></v-spacer>
        <v-text-field
          v-model="search"
          prepend-inner-icon="mdi-magnify"
          label="Search"
          single-line
          hide-details
          density="compact"
          class="max-width-300"
          variant="solo-filled"
          flat
        ></v-text-field>
      </v-card-title>

      <v-data-table
        :headers="headers"
        :items="users"
        :search="search"
        :loading="loading"
        hover
        class="pa-2"
      >
        <template v-slot:item.fullName="{ item }">
          {{ item.firstName }} {{ item.lastName }}
        </template>

        <template v-slot:item.roles="{ item }">
          <v-chip
            v-for="role in item.roles"
            :key="role"
            size="small"
            color="primary"
            variant="tonal"
            class="mr-1"
          >
            {{ role }}
          </v-chip>
        </template>

        <template v-slot:item.permissions="{ item }">
          <v-chip
            v-for="permission in item.permissions"
            :key="permission"
            size="small"
            color="secondary"
            variant="tonal"
            class="mr-1"
          >
            {{ permission }}
          </v-chip>
        </template>

        <template v-slot:item.actions="{ item }">
          <v-btn
            icon="mdi-pencil"
            size="small"
            color="primary"
            variant="text"
            @click="editUser(item)"
          ></v-btn>
        </template>
      </v-data-table>
    </v-card>

    <!-- Edit Dialog -->
    <v-dialog v-model="dialog" max-width="600px">
      <v-card v-if="selectedUser">
        <v-card-title class="pa-4 bg-primary text-white">
          {{ $t('users.edit.title') }}: {{ selectedUser.username }}
        </v-card-title>
        <v-card-text class="pt-4">
          <v-select
            v-model="editForm.roles"
            :items="availableRoles"
            item-title="name"
            item-value="name"
            label="Roles"
            multiple
            chips
            variant="outlined"
            class="mb-4"
          ></v-select>

          <v-select
            v-model="editForm.permissions"
            :items="availablePermissions"
            item-title="name"
            item-value="name"
            label="Direct Permissions"
            multiple
            chips
            variant="outlined"
          ></v-select>
        </v-card-text>
        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn color="grey" variant="text" @click="dialog = false">{{ $t('common.cancel') }}</v-btn>
          <v-btn color="primary" @click="saveUser" :loading="saving">{{ $t('users.edit.save') }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="snackbar" :color="snackbarColor" timeout="3000">
      {{ snackbarText }}
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const users = ref<any[]>([])
const loading = ref(false)
const search = ref('')
const dialog = ref(false)
const saving = ref(false)
const selectedUser = ref<any>(null)

const editForm = ref({
  roles: [] as string[],
  permissions: [] as string[]
})

const availableRoles = ref<any[]>([])
const availablePermissions = ref<any[]>([])

const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

const headers = computed(() => [
  { title: t('users.table.username'), key: 'username', align: 'start' as const },
  { title: t('users.table.email'), key: 'email' },
  { title: t('users.table.fullName'), key: 'fullName' },
  { title: t('users.table.roles'), key: 'roles', sortable: false },
  { title: t('users.table.permissions'), key: 'permissions', sortable: false },
  { title: t('users.table.actions'), key: 'actions', sortable: false, align: 'center' as const }
])

const fetchData = async () => {
  loading.value = true
  const token = localStorage.getItem('token')
  try {
    const [usersRes, rolesRes, permsRes] = await Promise.all([
      fetch('http://localhost:5112/api/users', { headers: { 'Authorization': `Bearer ${token}` } }),
      fetch('http://localhost:5112/api/roles', { headers: { 'Authorization': `Bearer ${token}` } }),
      fetch('http://localhost:5112/api/permissions', { headers: { 'Authorization': `Bearer ${token}` } })
    ])

    if (usersRes.ok) users.value = await usersRes.json()
    if (rolesRes.ok) availableRoles.value = await rolesRes.json()
    if (permsRes.ok) availablePermissions.value = await permsRes.json()
  } catch (error) {
    console.error('Failed to fetch user data:', error)
  } finally {
    loading.value = false
  }
}

const editUser = (user: any) => {
  selectedUser.value = user
  editForm.value = {
    roles: [...user.roles],
    permissions: [...user.permissions]
  }
  dialog.value = true
}

const saveUser = async () => {
  if (!selectedUser.value) return
  saving.value = true
  const token = localStorage.getItem('token')
  try {
    const response = await fetch(`http://localhost:5112/api/users/${selectedUser.value.id}/roles-permissions`, {
      method: 'PUT',
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(editForm.value)
    })

    if (response.ok) {
      snackbarText.value = t('users.edit.success')
      snackbarColor.value = 'success'
      snackbar.value = true
      dialog.value = false
      await fetchData()
    } else {
      throw new Error('Failed to update user access')
    }
  } catch (error) {
    snackbarText.value = error instanceof Error ? error.message : t('common.error')
    snackbarColor.value = 'error'
    snackbar.value = true
  } finally {
    saving.value = false
  }
}

onMounted(() => {
  fetchData()
})
</script>

<style scoped>
.max-width-300 {
  max-width: 300px;
}
</style>
