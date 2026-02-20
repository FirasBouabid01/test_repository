<template>
  <v-container>
    <v-row class="mb-4" align="center">
      <v-col>
        <h1 class="text-h4 font-weight-bold">Permission Management</h1>
      </v-col>
      <v-col cols="auto">
        <v-btn color="primary" prepend-icon="mdi-plus" @click="openDialog()">
          Add Permission
        </v-btn>
      </v-col>
    </v-row>

    <v-card elevation="2" class="rounded-lg">
      <v-data-table
        :headers="headers"
        :items="permissions"
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

    <!-- Permission Dialog (Add/Edit) -->
    <v-dialog v-model="dialog" max-width="500px">
      <v-card class="rounded-xl">
        <v-card-title class="bg-primary text-white pa-4">
          <span class="text-h5">{{ editedItem.id ? 'Edit Permission' : 'New Permission' }}</span>
        </v-card-title>

        <v-card-text class="pa-6">
          <v-form ref="form" v-model="valid">
            <v-text-field
              v-model="editedItem.name"
              label="Permission Name"
              variant="outlined"
              hint="e.g., User.Create, Role.Delete"
              persistent-hint
              :rules="[v => !!v || 'Name is required']"
              required
            ></v-text-field>
          </v-form>
        </v-card-text>

        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn variant="text" @click="closeDialog">Cancel</v-btn>
          <v-btn color="primary" :loading="saving" :disabled="!valid" @click="savePermission">
            Save
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Delete Confirmation -->
    <v-dialog v-model="deleteDialog" max-width="400px">
      <v-card class="rounded-xl">
        <v-card-title class="text-h5 pa-4">Confirm Delete</v-card-title>
        <v-card-text class="pa-4 pt-0">
          Are you sure you want to delete the permission <b>{{ editedItem.name }}</b>?
          <v-alert v-if="deleteError" type="error" variant="tonal" class="mt-4" closable>
            {{ deleteError }}
          </v-alert>
        </v-card-text>
        <v-card-actions class="pa-4">
          <v-spacer></v-spacer>
          <v-btn variant="text" @click="deleteDialog = false">Cancel</v-btn>
          <v-btn color="error" :loading="deleting" @click="deletePermission">Delete</v-btn>
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
import { ref, onMounted } from 'vue'

interface Permission {
  id?: string
  name: string
}

const permissions = ref<Permission[]>([])
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

const headers = [
  { title: 'Name', key: 'name' },
  { title: 'Actions', key: 'actions', sortable: false, align: 'end' },
]

const defaultItem: Permission = { name: '' }
const editedItem = ref<Permission>({ ...defaultItem })

const fetchPermissions = async () => {
  loading.value = true
  try {
    const response = await fetch('http://localhost:5112/api/permissions', {
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    })
    if (!response.ok) throw new Error('Failed to fetch permissions')
    permissions.value = await response.json()
  } catch (err) {
    showSnackbar('Error loading permissions', 'error')
  } finally {
    loading.value = false
  }
}

const openDialog = (item?: Permission) => {
  editedItem.value = item ? { ...item } : { ...defaultItem }
  dialog.value = true
}

const closeDialog = () => {
  dialog.value = false
  editedItem.value = { ...defaultItem }
}

const savePermission = async () => {
  saving.value = true
  try {
    const isEdit = !!editedItem.value.id
    const url = isEdit 
      ? `http://localhost:5112/api/permissions/${editedItem.value.id}` 
      : 'http://localhost:5112/api/permissions'
    
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
      throw new Error(data.message || data.error || 'Failed to save permission')
    }

    showSnackbar(`Permission ${isEdit ? 'updated' : 'created'} successfully`)
    closeDialog()
    fetchPermissions()
  } catch (err: any) {
    showSnackbar(err.message, 'error')
  } finally {
    saving.value = false
  }
}

const confirmDelete = (item: Permission) => {
  editedItem.value = { ...item }
  deleteError.value = ''
  deleteDialog.value = true
}

const deletePermission = async () => {
  deleting.value = true
  deleteError.value = ''
  try {
    const response = await fetch(`http://localhost:5112/api/permissions/${editedItem.value.id}`, {
      method: 'DELETE',
      headers: { 'Authorization': `Bearer ${localStorage.getItem('token')}` }
    })

    if (!response.ok) {
      const data = await response.json().catch(() => ({}))
      throw new Error(data.message || data.error || 'Failed to delete permission')
    }

    showSnackbar('Permission deleted successfully')
    deleteDialog.value = false
    fetchPermissions()
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

onMounted(fetchPermissions)
</script>
