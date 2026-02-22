<template>
  <div>
    <v-container>
      <v-row>
        <v-col>
          <h1>{{ $t('roles.title') }}</h1>
        </v-col>
        <v-col class="d-flex justify-end">
          <v-btn color="primary" @click="showCreateDialog = true">
            {{ $t('roles.create') }}
          </v-btn>
        </v-col>
      </v-row>

      <!-- Search -->
      <v-row class="mb-4">
        <v-col cols="12" md="6">
          <v-text-field
            v-model="searchTerm"
            :label="$t('roles.search')"
            prepend-icon="mdi-magnify"
            @input="searchRoles"
          />
        </v-col>
      </v-row>

      <!-- Roles Table -->
      <v-data-table
        :headers="headers"
        :items="roles"
        :loading="loading"
        :items-per-page="pageSize"
        :page="pageNumber"
        :server-items-length="totalCount"
        @update:options="updateTable"
      >
        <template #item.permissions="{ item }">
          <v-chip v-for="perm in item.permissions" :key="perm" size="small" class="mr-1">
            {{ perm }}
          </v-chip>
        </template>
        <template #item.actions="{ item }">
          <v-btn size="small" @click="editRole(item)">
            {{ $t('roles.edit') }}
          </v-btn>
          <v-btn size="small" color="error" @click="deleteRole(item)" :disabled="item.userCount > 0">
            {{ $t('roles.delete') }}
          </v-btn>
        </template>
      </v-data-table>

      <!-- Create/Edit Dialog -->
      <v-dialog v-model="showCreateDialog" max-width="500">
        <v-card>
          <v-card-title>
            {{ editingRole ? $t('roles.edit') : $t('roles.create') }}
          </v-card-title>
          <v-card-text>
            <v-form ref="form">
              <v-text-field
                v-model="formData.name"
                :label="$t('roles.name')"
                required
              />
              <v-select
                v-model="formData.permissionIds"
                :items="availablePermissions"
                :label="$t('roles.permissions')"
                multiple
                chips
              />
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-btn @click="showCreateDialog = false">
              {{ $t('common.cancel') }}
            </v-btn>
            <v-btn color="primary" @click="saveRole">
              {{ $t('common.save') }}
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-dialog>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { RoleService, RoleDto } from '@/services/roleService'
import { PermissionService, PermissionDto } from '@/services/permissionService'

const { t } = useI18n()

const roles = ref<RoleDto[]>([])
const loading = ref(false)
const pageNumber = ref(1)
const pageSize = ref(10)
const totalCount = ref(0)
const searchTerm = ref('')
const showCreateDialog = ref(false)
const editingRole = ref<RoleDto | null>(null)
const availablePermissions = ref<PermissionDto[]>([])

const headers = [
  { title: t('roles.name'), key: 'name' },
  { title: t('roles.permissions'), key: 'permissions' },
  { title: t('roles.userCount'), key: 'userCount' },
  { title: t('roles.actions'), key: 'actions', sortable: false },
]

const formData = ref({
  name: '',
  permissionIds: [] as string[],
})

async function fetchRoles() {
  loading.value = true
  try {
    const response = await RoleService.getRoles(
      pageNumber.value,
      pageSize.value,
      searchTerm.value
    )
    roles.value = response.data
    totalCount.value = response.totalCount
  } catch (error) {
    console.error('Error fetching roles:', error)
  } finally {
    loading.value = false
  }
}

async function fetchPermissions() {
  try {
    const response = await PermissionService.getPermissions(1, 1000)
    availablePermissions.value = response.data
  } catch (error) {
    console.error('Error fetching permissions:', error)
  }
}

function searchRoles() {
  pageNumber.value = 1
  fetchRoles()
}

function editRole(role: RoleDto) {
  editingRole.value = role
  formData.value = {
    name: role.name,
    permissionIds: [...role.permissions],
  }
  showCreateDialog.value = true
}

async function saveRole() {
  if (editingRole.value) {
    try {
      await RoleService.updateRole(editingRole.value.id, formData.value)
      showCreateDialog.value = false
      editingRole.value = null
      fetchRoles()
    } catch (error) {
      console.error('Error updating role:', error)
    }
  } else {
    try {
      await RoleService.createRole(formData.value)
      showCreateDialog.value = false
      fetchRoles()
    } catch (error) {
      console.error('Error creating role:', error)
    }
  }
}

async function deleteRole(role: RoleDto) {
  if (role.userCount > 0) {
    alert(t('roles.cannotDelete'))
    return
  }
  if (confirm(t('common.confirm'))) {
    try {
      await RoleService.deleteRole(role.id)
      fetchRoles()
    } catch (error) {
      console.error('Error deleting role:', error)
    }
  }
}

function updateTable(options: any) {
  pageNumber.value = options.page
  pageSize.value = options.itemsPerPage
  fetchRoles()
}

onMounted(() => {
  fetchRoles()
  fetchPermissions()
})
</script>
