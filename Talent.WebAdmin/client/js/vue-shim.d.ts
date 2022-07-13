declare module "*.vue" {
    import Vue from "vue";
    import VueRouter, { Route } from 'vue-router'
    interface Vue {
        $route: VueRouter
    }
}
