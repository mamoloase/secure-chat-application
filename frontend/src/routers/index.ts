import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
    history: createWebHistory(),
    routes: [
        {
            path: '/',
            meta: {
                requiresAuth: true
            },
            children: [
                {
                    path: '',
                    name: "index",
                    component: () => import('@/pages/Index.vue')
                },
            ]

        },
        {
            path: '/auth',
            meta: {
                requiresAuth: false
            },
            children: [
                {
                    path: 'signin',
                    name: "signin",
                    component: () => import('@/pages/auth/Signin.vue')
                },
            ]

        },
    ]
})
router.beforeEach((to: any, from: any, next: any) => {
    next()
});
router.afterEach(() => {


});
export default router