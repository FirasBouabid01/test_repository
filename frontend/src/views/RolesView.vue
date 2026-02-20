<template>
  <v-container>
    <v-row class="mb-4" align="center">
      <v-col>
        <h1 class="text-h4 font-weight-bold">{{ $t('roles.title') }}</h1>
      </v-col>
      <v-col cols="auto">
        <v-btn color="primary" prepend-icon="mdi-plus" @click="openDialog()">
          {{ $t('roles.add') }}
        </v-btn>
      </v-col>
    </v-row>

    <v-card elevation="2" class="rounded-lg">
      <v-data-table
        :headers="translatedHeaders"
        :items="roles"
        :loading="loading"
        class="elevation-0"
      >
        <template v-slot:item.actions="{ item }">
          <v-icon size="small" class="mr-2" color="blue" @click="openDialog(item)">
            mdi-pencil
          </v-icon>
          <v-icon size="small" color="error" @click="confirmDelete(item)">
            mdi-delete
          </v-icon>
        </template>
      </v-data-table>
    </v-card>

    <!-- Role Dialog (Add/Edit) -->
    <v-dialog v-model="dialog" max-width="600px">
      <v-card class="rounded-xl">
        <v-card-title class="bg-primary text-white pa-4">
          <span class="text-h5">{{ editedItem.id ? $t('roles.edit') : $t('roles.new') }}</span>
        </v-card-title>

        <v-card-text class="pa-6">
          <v-form ref="form" v-model="valid">
            <v-text-field
              v-model="editedItem.name"
              :label="$t('roles.name')"
              variant="outlined"
              :rules="[v => !!v || $t('roles.nameRequired')]"
              required
              class="mb-4"
            ></v-text-field>

            <div class="text-subtitle-1 mb-2 font-weight-bold">{{ $t('roles.permissions') }}</div>
            <v-autocomplete
              v-model="selectedPermissions"
              :items="allPermissions"
              item-title="name"
              item-value="id"
              :label="$t('roles.select')"
              multiple
              chips
              closable-chips
              variant="outlined"
              :hint="$t('roles.search')"
              persistent-hint
            ></v-autocomplete>
          </v-form>
        </v-card-text>

        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn variant="text" @click="closeDialog">{{ $t('common.cancel') }}</v-btn>
          <v-btn color="primary" :loading="saving" :disabled="!valid" @click="saveRole">
            {{ $t('common.save') }}
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Delete Confirmation -->
    <v-dialog v-model="deleteDialog" max-width="400px">
      <v-card class="rounded-xl">
        <v-card-title class="text-h5 pa-4">{{ $t('common.confirmDelete') }}</v-card-title>
        <v-card-text class="pa-4 pt-0">
          {{ $t('common.deleteConfirmText', { name: editedItem.name }) }}
          <v-alert v-if="deleteError" type="error" variant="tonal" class="mt-4" closable>
            {{ deleteError }}
          </v-alert>
        </v-card-text>
        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn variant="text" @click="deleteDialog = false">{{ $t('common.cancel') }}</v-btn>
          <v-btn color="error" :loading="deleting" @click="deleteRole">{{ $t('common.delete') }}</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Toast/Snackbar -->
    <v-snackbar v-model="snackbar" :color="snackbarColor" timeout="3000">
      {{ snackbarText }}
    </v-snackbar>
  </v-container>
</template>

<script setup lang="ts">
import { ref, onMounted, computed } from 'vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

interface Permission {
  id: string
  name: string
}

interface RolePermission {
  roleId: string
  permissionId: string
  permission: Permission
}

interface Role {
  id?: string
  name: string
  rolePermissions?: RolePermission[]
}

const roles = ref<Role[]>([])
const allPermissions = ref<Permission[]>([])
const selectedPermissions = ref<string[]>([])
const loading = ref(false)
const dialog = ref(false)
const deleteDialog = ref(false)
const valid = ref(false)
const saving = ref(false)
const deleting = ref(false)
const deleteError = ref('')
const snackbar = ref(false)
const snackbarText = ref('')
const snackbarColor = ref('success')

const translatedHeaders = computed(() => [
  { title: t('common.name'), key: 'name' },
  { title: t('common.actions'), key: 'actions', sortable: false, align: 'end' },
])

const defaultItem: Role = { name: '' }
const editedItem = ref<Role>({ ...defaultItem })

const fetchRoles = async () => {
  loading.value = true
  try {
    const response = await fetch('http://localhost:5112/api/roles', {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    })
    if (!response.ok) throw new Error(t('roles.fetchError'))
    roles.value = await response.json()
  } catch (err) {
    showSnackbar(t('roles.fetchError'), 'error')
  } finally {
    loading.value = false
  }
}

const fetchAllPermissions = async () => {
  try {
    const response = await fetch('http://localhost:5112/api/permissions', {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    })
    if (!response.ok) throw new Error(t('permissions.fetchError'))
    allPermissions.value = await response.json()
  } catch (err) {
    console.error('Error fetching permissions:', err)
  }
}

const openDialog = async (item?: Role) => {
  if (item && item.id) {
    // Fetch role details with permissions
    try {
      const response = await fetch(`http://localhost:5112/api/roles/${item.id}`, {
        headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
      })
      if (response.ok) {
        const fullRole = await response.json()
        editedItem.value = { ...fullRole }
        selectedPermissions.value = fullRole.rolePermissions?.map((rp: any) => rp.permissionId) || []
      } else {
        editedItem.value = { ...item }
        selectedPermissions.value = []
      }
    } catch (err) {
      editedItem.value = { ...item }
      selectedPermissions.value = []
    }
  } else {
    editedItem.value = { ...defaultItem }
    selectedPermissions.value = []
  }
  dialog.value = true
}

const closeDialog = () => {
  dialog.value = false
  editedItem.value = { ...defaultItem }
  selectedPermissions.value = []
}

const saveRole = async () => {
  saving.value = true
  try {
    const isEdit = !!editedItem.value.id
    const url = isEdit 
      ? `http://localhost:5112/api/roles/${editedItem.value.id}` 
      : 'http://localhost:5112/api/roles'
    
    const response = await fetch(url, {
      method: isEdit ? 'PUT' : 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
      body: JSON.stringify(editedItem.value)
    })

    if (!response.ok) {
      const data = await response.json().catch(() => ({}))
      throw new Error(data.message || data.error || t('roles.saveError'))
    }

    let roleId = editedItem.value.id
    if (!isEdit) {
      const data = await response.json()
      roleId = data.id
    }

    // Assign permissions
    if (roleId) {
      const permResponse = await fetch(`http://localhost:5112/api/roles/${roleId}/permissions`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${localStorage.getItem('token')}`
        },
        body: JSON.stringify(selectedPermissions.value)
      })

      if (!permResponse.ok) {
        showSnackbar(t('roles.permUpdateError'), 'warning')
      }
    }

    showSnackbar(isEdit ? t('roles.updated') : t('roles.created'))
    closeDialog()
    fetchRoles()
  } catch (err: any) {
    showSnackbar(err.message, 'error')
  } finally {
    saving.value = false
  }
}

const confirmDelete = (item: Role) => {
  editedItem.value = { ...item }
  deleteError.value = ''
  deleteDialog.value = true
}

const deleteRole = async () => {
  deleting.value = true
  deleteError.value = ''
  try {
    const response = await fetch(`http://localhost:5112/api/roles/${editedItem.value.id}`, {
      method: 'DELETE',
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    })

    if (!response.ok) {
      const data = await response.json().catch(() => ({}))
      throw new Error(data.message || data.error || t('roles.deleteError'))
    }

    showSnackbar(t('roles.deleted'))
    deleteDialog.value = false
    fetchRoles()
  } catch (err: any) {
    deleteError.value = err.message
  } finally {
    deleting.value = false
  }
}

const showSnackbar = (text: string, color = 'success') => {
  snackbarText.value = text
  snackbarColor.value = color
  snackbar.value = true
}

onMounted(() => {
  fetchRoles()
  fetchAllPermissions()
})
</script>
