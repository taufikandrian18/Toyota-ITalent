<template>
    <div>
        <loading-overlay :loading="false"></loading-overlay>

        <div class="row">
            <div class="col-md-12">
                <h3><fa icon="home"></fa> Home</h3>
                <br />

                <div class="col-md-12">
                    <div class="row p-0">
                        <!-- image welcome -->
                        <img
                            class="col-12 p-0"
                            src="/images/bg_home.png"
                            alt="Welcome to Dashboard"
                        />
                        <!-- version -->
                        <div class="col-12 text-center version text-sm">
                            <label for="version" class="talent_font_grey"
                                >Version 2.12 (16 Juli 2021)</label
                            >
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';

@Component({})
export default class Home extends Vue {}
</script>

<style scoped>
.version {
    position: absolute;
    bottom: 0;
    margin-bottom: 1rem;
}
</style>
