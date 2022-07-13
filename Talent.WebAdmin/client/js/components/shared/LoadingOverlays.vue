<template>

    <div class="loading" v-if="loading">
       <fa icon="spinner" size="5x" spin></fa>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';

    @Component({
        props:['loading'],
        created: function (this: LoadingOverlays) {
            //this.initial();
            console.log(this.loading);
        }
        
    })
    export default class LoadingOverlays extends Vue {
        loading: boolean;
    }
</script>