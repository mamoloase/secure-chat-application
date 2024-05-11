<template>
    <svg :width="props.width" :height="props.height" :fill="props.fill" viewBox="0 0 16 16">
        <path
            d="M9.5 13a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0m0-5a1.5 1.5 0 1 1-3 0 1.5 1.5 0 0 1 3 0" />
    </svg>
</template>

<script setup lang="ts">
interface Props {
    width?: string,
    height?: string,
    fill?: string,
}
const props = withDefaults(defineProps<Props>(), {
    width: "20px",
    height: "20px",
    fill: "currentColor"
});
</script>