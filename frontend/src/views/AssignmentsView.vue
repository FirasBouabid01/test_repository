<template>
  <div>
    <v-container>
      <v-row>
        <v-col>
          <h1>{{ $t('assignments.title') }}</h1>
        </v-col>
      </v-row>

      <!-- Assign Role to User -->
      <v-card class="mb-4">
        <v-card-title>{{ $t('assignments.assignRole') }}</v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="12" md="6">
              <v-select
                v-model="assignRoleData.userId"
                :items="users"
                item-title="email"
                item-value="id"
                :label="$t('assignments.selectUser')"
              />
            </v-col>
            <v-col cols="12" md="6">
              <v-select
                v-model="assignRoleData.roleId"
                :items="roles"
                item-title="name"
                item-value="id"
                :label="$t('assignments.selectRole')"
              />
            </v-col>
            <v-col cols="12">
              <v-btn
                color="primary"
                @click="assignRoleToUser"
                :disabled="!assignRoleData.userId || !assignRoleData.roleId"
              >
                {{ $t('assignments.assign') }}
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>

      <!-- Assign Permission to User -->
      <v-card class="mb-4">
        <v-card-title>{{ $t('assignments.assignPermission') }}</v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="12" md="6">
              <v-select
                v-model="assignPermissionData.userId"
                :items="users"
                item-title="email"
                item-value="id"
                :label="$t('assignments.selectUser')"
              />
            </v-col>
            <v-col cols="12" md="6">
              <v-select
                v-model="assignPermissionData.permissionId"
                :items="permissions"
                item-title="name"
                item-value="id"
                :label="$t('assignments.selectPermission')"
              />
            </v-col>
            <v-col cols="12">
              <v-btn
                color="primary"
                @click="assignPermissionToUser"
                :disabled="!assignPermissionData.userId || !assignPermissionData.permissionId"
              >
                {{ $t('assignments.assign') }}
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>

      <!-- Assign Permission to Role -->
      <v-card class="mb-4">
        <v-card-title>{{ $t('assignments.assignPermission') }} to Role</v-card-title>
        <v-card-text>
          <v-row>
            <v-col cols="12" md="6">
              <v-select
                v-model="assignPermissionToRoleData.roleId"
                :items="roles"
                item-title="name"
                item-value="id"
                :label="$t('assignments.selectRole')"
              />
            </v-col>
            <v-col cols="12" md="6">
              <v-select
                v-model="assignPermissionToRoleData.permissionId"
                :items="permissions"
                item-title="name"
                item-value="id"
                :label="$t('assignments.selectPermission')"
              />
            </v-col>
            <v-col cols="12">
              <v-btn
                color="primary"
                @click="assignPermissionToRole"
                :disabled="!assignPermissionToRoleData.roleId || !assignPermissionToRoleData.permissionId"
              >
                {{ $t('assignments.assign') }}
              </v-btn>
            </v-col>
          </v-row>
        </v-card-text>
      </v-card>
    </v-container>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useI18n } from 'vue-i18n'
import { UserService, type UserDto } from '../services/userService'
import { RoleService, type RoleDto } from '../services/roleService'
import { PermissionService, type PermissionDto } from '../services/permissionService'

const { t } = useI18n()

const users = ref<UserDto[]>([])
const roles = ref<RoleDto[]>([])
const permissions = ref<PermissionDto[]>([])

const assignRoleData = ref({
  userId: '',
  roleId: '',
})

const assignPermissionData = ref({
  userId: '',
  permissionId: '',
})

const assignPermissionToRoleData = ref({
  roleId: '',
  permissionId: '',
})

async function fetchData() {
  try {
    const [usersRes, rolesRes, permsRes] = await Promise.all([
      UserService.getUsers(1, 1000),
      RoleService.getRoles(1, 1000),
      PermissionService.getPermissions(1, 1000),
    ])
    users.value = usersRes.data
    roles.value = rolesRes.data
    permissions.value = permsRes.data
  } catch (error) {
    console.error('Error fetching data:', error)
  }
}

async function assignRoleToUser() {
  try {
    await PermissionService.assignRoleToUser(
      assignRoleData.value.userId,
      assignRoleData.value.roleId
    )
    alert(t('assignments.success'))
    assignRoleData.value = { userId: '', roleId: '' }
  } catch (error) {
    console.error('Error assigning role:', error)
    alert(t('assignments.error'))
  }
}

async function assignPermissionToUser() {
  try {
    await PermissionService.assignPermissionToUser(
      assignPermissionData.value.userId,
      assignPermissionData.value.permissionId
    )
    alert(t('assignments.success'))
    assignPermissionData.value = { userId: '', permissionId: '' }
  } catch (error) {
    console.error('Error assigning permission:', error)
    alert(t('assignments.error'))
  }
}

async function assignPermissionToRole() {
  try {
    await PermissionService.assignPermissionToRole(
      assignPermissionToRoleData.value.roleId,
      assignPermissionToRoleData.value.permissionId
    )
    alert(t('assignments.success'))
    assignPermissionToRoleData.value = { roleId: '', permissionId: '' }
  } catch (error) {
    console.error('Error assigning permission to role:', error)
    alert(t('assignments.error'))
  }
}

onMounted(() => {
  fetchData()
})
</script>
