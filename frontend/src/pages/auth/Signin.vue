<template>
    <transition name="fade">
        <div v-if="verification" class="h-screen w-full container flex-center flex-column">
            <div class="form w-full d-flex flex-column">
                <div class="d-flex flex-column">
                    <h1 class="fs-large fw-bold text-color">Verify</h1>
                    <p class="fs-small fw-normal description-color mt-2">
                        please enter verification code
                    </p>
                </div>
                <div class="flex-center flex-column mt-4">
                    <div class="w-full d-flex flex-column">
                        <div class="p-3 box-shadow  rounded-1 w-full form-input bg-card flex-center">
                            <EnvelopeIcon width="20px" height="20px" class="description-color mr-2" />

                            <input v-model="verificationCode" class="w-full fs-medium fw-normal text-color"
                                placeholder="Verification Code" type="text">
                        </div>
                    </div>
                </div>

                <div class="flex-center flex-column mt-4">
                    <div class="w-full d-flex flex-column">
                        <LoaderButton @click="requestVerifyCode" class="p-3 box-shadow rounded-1 bg-theme"
                            :loading="loadingVerifyCode">
                            <span class="fs-medium fw-normal text-color">Signin</span>
                        </LoaderButton>

                    </div>
                </div>
            </div>
        </div>
        <div v-else class="h-screen w-full container flex-center flex-column">
            <div class="form w-full d-flex flex-column">
                <div class="d-flex flex-column">
                    <h1 class="fs-large fw-bold text-color">Signin</h1>
                    <p class="fs-small fw-normal description-color mt-2">
                        please enter a valid phone number
                    </p>
                </div>
                <div class="flex-center flex-column mt-4">
                    <div class="w-full d-flex flex-column">
                        <div class="p-3 box-shadow  rounded-1 w-full form-input bg-card flex-center">
                            <EnvelopeIcon width="20px" height="20px" class="description-color mr-2" />
                            <input v-model="phoneNumber" class="w-full fs-medium fw-normal text-color"
                                placeholder="989938170183" type="text">
                        </div>
                    </div>
                </div>

                <div class="flex-center flex-column mt-4">
                    <div class="w-full d-flex flex-column">
                        <LoaderButton @click="requestSendCode" class="p-3 box-shadow rounded-1 bg-theme"
                            :loading="loadingReceiveCode">
                            <span class="fs-medium fw-normal text-color">Get Code</span>
                        </LoaderButton>
                    </div>
                </div>
            </div>
        </div>

    </transition>

</template>

<script lang="ts" setup>

import LoaderButton from '@/components/buttons/LoaderButton.vue';
import EnvelopeIcon from '@/components/icons/EnvelopeIcon.vue';
</script>

<script lang="ts">
import { ref } from 'vue';
import router from '@/routers';
import { defineComponent } from "vue";
import ConnectionMessage from '@/packages/transport/types/connectionMessage';

export default defineComponent({
    data: () => {
        return {
            phoneNumber: ref<string>(""),
            verificationCode: ref<string>(""),
            verification: ref<boolean>(false),
            loadingVerifyCode: ref<boolean>(false),
            loadingReceiveCode: ref<boolean>(false),
        }
    },
    created() {
        this.$socket.on_message((request: ConnectionMessage): any => {
            console.log(request);
        })

        this.$socket.send({
                Handler: "Chat",
                Method: "Create",
                Parameters: [
                    { Key: "request", Value: { name: "Mehrab Channel" } }
                ]

            }, true)
                .then((message: ConnectionMessage) => {
                    console.log(message)
                }).catch((except: any) => {
                    console.log(except)
                })
    },
    methods: {
        validationPhoneNumber(phone: string): boolean {
            return true;
        },
        requestSendCode() {
            if (!this.validationPhoneNumber(this.phoneNumber))
                return;

            this.loadingReceiveCode = true;

            this.$socket.send({
                Handler: "Authentication",
                Method: "SendVerificationCode",
                Parameters: [
                    { Key: "request", Value: { phone: this.phoneNumber } }
                ]
            }, true)
                .then((message: ConnectionMessage) => {
                    this.verification = true;
                    this.loadingReceiveCode = false;
                }).catch((except: any) => {

                })

        },

        requestVerifyCode() {
            this.loadingVerifyCode = true;

            this.$socket.send({
                Handler: "Authentication",
                Method: "VerificationCode",
                Parameters: [
                    { Key: "request", Value: { phone: this.phoneNumber, Code: this.verificationCode } }
                ]
            }, true)
                .then((message: ConnectionMessage) => {
                    const token = message.Parameters.find(x => x.Key.toLowerCase() == "token");

                    this.loadingVerifyCode = false;

                    if (token) {
                        localStorage.setItem("token", String(token.Value));
                        router.push({ name: "index" })
                    }
                }).catch((except: any) => {

                })
        }
    },

});
</script>

<style scoped>
.form {
    max-width: 500px;
}

.fade {
    opacity: 0;
}
</style>