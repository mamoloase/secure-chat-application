<template>
    <div class="h-screen w-full d-flex">
        <div class="h-full w-full bg-card box-shadow overflow-scroll chat_list_menu p-3">
            <div class="d-flex flex-column">
                <div class="d-flex flex-column">
                    <span class="text-color fs-medium fw-bold">Stories</span>
                    <div class="d-flex align-items-center mt-3 overflow-scroll story-list">
                        <StoryCard />
                        <StoryCard />
                        <StoryCard />
                        <StoryCard />
                        <StoryCard />
                        <StoryCard />
                        <StoryCard />
                    </div>
                </div>
                <div class="d-flex flex-column mt-4">
                    <span class="text-color fs-medium fw-bold">Messages</span>
                    <div class="flex-center flex-column mt-3">
                        <ChatCard v-for="(user, index) in users" :key="index" :user="user"
                            @click="showUserMessages(user)" />
                    </div>
                </div>
            </div>
        </div>
        <div v-if="userSelected" class="message_list_menu position-relative h-full w-full d-flex flex-column">
            <div class="w-full bg-card box-shadow p-3">
                <div class="d-flex align-items-center justify-content-between">
                    <div class="flex-center">
                        <ProfileImageCard />

                        <div class="d-flex flex-column">
                            <span class="text-color fs-medium fw-normal">{{ userSelected.Name }}</span>
                            <span class="theme-color fs-small fw-normal mt-2">Online Now</span>
                        </div>
                    </div>
                    <div class="flex-center">
                        <DotMenuIcon width="20px" height="20px" class="description-color" />
                    </div>
                </div>
            </div>
            <div class="overflow-scroll d-flex flex-column-reverse h-screen p-3">
                <MessageCard v-for="(message, index) in messages" :key="index" :message="message" />
            </div>
            <div class="p-2">
                <div class="p-2 chat_box bg-card box-shadow p-2 rounded-2">
                    <div class="flex-center">
                        <EmojiSmileIcon class="description-color" />
                        <div class="flex-center px-1 w-full">
                            <input placeholder="write a message ..." class="w-full description-color fs-small fw-normal"
                                type="text">
                        </div>
                        <div class="send_message flex-center" @click="sendMessage">
                            <SendIcon width="22px" height="22px" class="text-color" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.chat_list_menu {
    max-width: 300px;
}

.user_story {
    width: 50px;
    height: 50px;
    padding: 1px;
    min-width: 50px;
    border: 2px solid hsl(var(--theme-color));
}

@media (max-width : 650px) {
    .message_list_menu {
        display: none !important;
    }

    .chat_list_menu {
        max-width: 100% !important;
    }
}
</style>

<script setup lang="ts">
import { ref } from 'vue';

import ChatCard from '@/components/cards/ChatCard.vue';
import StoryCard from '@/components/cards/StoryCard.vue';
import MessageCard from '@/components/cards/MessageCard.vue';
import ProfileImageCard from '@/components/cards/ProfileImageCard.vue';

import MessageType from '@/models/messageType';

import SendIcon from '@/components/icons/SendIcon.vue';
import DotMenuIcon from '@/components/icons/DotMenuIcon.vue';
import EmojiSmileIcon from '@/components/icons/EmojiSmileIcon.vue';
import UserModel from '@/models/userModel';
import MessageModel from '@/models/messageModel';

const typedMessage = ref<string>('hello');

const userSelected = ref<UserModel>();

const messages = ref<Array<MessageModel>>(
    [
        { Self: false, Name: 'Mehrab', Message: 'please send a image yourself', Type: MessageType.Message },
        { Self: true, Name: 'Mehrab', Message: 'This image for before 18 years', Type: MessageType.Picture },
        { Self: false, Name: 'Mehrab', Message: 'Are you biutiful bro ,thats nice .', Type: MessageType.Message },
        { Self: true, Name: 'Mehrab', Message: 'thank you bro me too', Type: MessageType.Message },
    ].reverse()
);
const users = ref<Array<UserModel>>(
    [
        { Name: 'Mehrab' },
        { Name: 'Ali' },
        { Name: 'Reza' },
        { Name: 'Zahra' },
        { Name: 'Younes' },
        { Name: 'Amir' },
    ]
);
const showUserMessages = (user: UserModel) => {
    userSelected.value = user;
}
const sendMessage = () => {
    messages.value.push({ Message: typedMessage.value, Name: 'mehrab', Self: true, Type: MessageType.Message })
}

</script>