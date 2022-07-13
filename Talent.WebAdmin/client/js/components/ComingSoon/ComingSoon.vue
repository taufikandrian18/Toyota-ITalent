<template>
    <div>
        <loading-overlay :loading="false"></loading-overlay>

        <div class="row">
            <div class="col-md-12">
                <h3><fa icon="home"></fa> Coming Soon</h3>
                <br />

                <div class="col-md-12">
                    <div
                        class="row p-0 talent_bg_shadow talent_bg_white talent_paddingsmall talent_roundborder"
                    >
                        <!-- Coming Soon Image -->
                        <img
                            class="col-12 p-0"
                            src="/images/coming_soon.png"
                            alt="Welcome to Dashboard"
                        />
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
export default class ComingSoon extends Vue {}
</script>

<style scoped>
.version {
    position: absolute;
    bottom: 0;
    margin-bottom: 1rem;
}
</style>
