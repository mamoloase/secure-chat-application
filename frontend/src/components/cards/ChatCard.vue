<template>
    <div v-if="user" class="mt-2 w-full d-flex align-items-center justify-content-between">
        <div class="flex-center">
            <ProfileImageCard />

            <div class="d-flex flex-column">
                <span class="text-color fs-small fw-normal">{{ user.Name }}</span>
                <span class="description-color fs-small fw-normal mt-2">Hello How Are you?</span>
            </div>
        </div>
        <div class="flex-center">
            <span class="description-color fs-small fw-normal">23:12</span>
        </div>
    </div>
</template>

<script setup lang="ts">
import UserModel from '@/models/userModel';
import ProfileImageCard from './ProfileImageCard.vue';

interface Props {
    user?: UserModel,
}
const props = withDefaults(defineProps<Props>(), {
});
</script>
