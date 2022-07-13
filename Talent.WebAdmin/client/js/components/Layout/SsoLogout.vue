<template>
    <div class="d-inline-flex">
        <a class="nav-link talent_font_darkblue" @click="logout()" style="cursor:pointer">
            Log out
        </a>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';

    @Component({
        props: ['logoutUrl'],
        created: function (this: SsoLogout) {

        }
    })
    export default class SsoLogout extends Vue {
        logoutUrl: string

        logout() {
            let width = 640;
            let height = 480;
            let left = screen.width / 2 - width / 2;
            let top = screen.height / 2 - height / 2;
            let params = 'width=' + width + ',height=' + height + ',left=' + left + ',top=' + top;

            let popup = window.open(this.logoutUrl, '_blank', params);
            setTimeout(() => {
                popup.close();
                window.location.href = '/auth/logout'
            }, 2000);
        }
    }
</script>