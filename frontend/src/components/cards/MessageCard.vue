<template>
    <div v-if="message" class="w-full py-2">
        <div class="d-flex align-items-center" :class="message.Self ? 'justify-content-end' : 'justify-content-start'">
            <div class="message_box d-flex flex-column">
                <div class="px-2 py-2 d-flex flex-column" :class="message.Self ? 'message_self' : 'message_user'">
                    <div v-if="message.Type == MessageType.Picture" class="flex-center mb-2 rounded-1 overflow-hidden">
                        <img src="@/assets/images/profile3.webp" alt="">
                    </div>
                    <span :class="message.Self ? 'description-color' : 'description-color'" class="fs-medium fw-normal p-1">
                        {{ message.Message }}
                    </span>
                </div>
                <div class="d-flex align-items-center mt-1 justify-content-end">
                    <span class="description-color fs-secondary-small fw-normal ">22:43 PM</span>
                    <div class="flex-center ml-2">
                        <div class="rounded-circle seen-circle bg-theme"></div>
                        <div class="rounded-circle seen-circle bg-theme"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</template>

<style scoped>
.message_self {
    border-radius: 10px 10px 0px 10px;
    background: rgb(var(--bg-card-color));
}

.message_user {
    border-radius: 10px 10px 10px 0px;
    background: rgb(var(--bg-card-color));
}

.message_self,
.message_user {
    max-width: 300px;
}

.seen-circle {
    width: 5px;
    height: 5px;
}
</style>


<script setup lang="ts">
import MessageModel from '@/models/messageModel';
import MessageType from '@/models/messageType';

interface Props {
    message?: MessageModel
}
const props = withDefaults(defineProps<Props>(), {
});
</script>
