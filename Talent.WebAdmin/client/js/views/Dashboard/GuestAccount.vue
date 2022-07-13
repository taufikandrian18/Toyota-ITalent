<template>
  <div>
    <loading-overlay :loading="isBusy"></loading-overlay>
    
    <dashboard-view v-if="data.layout == 'view'" @navigation="handleNavigation" :data="data" :claims="claims"/>

    <detail-account v-if="['detail', 'edit'].includes(data.layout)" @navigation="handleNavigation" :data="data" :claims="claims"/>

  </div>
</template>

<script lang="ts">
  import Vue from 'vue';
  import Component from 'vue-class-component';
  import DashboardView from '../../components/Dashboard/GuestAccount/DashboardView.vue';
  import DetailAccount from '../../components/Dashboard/GuestAccount/DetailAccount.vue';
    
  @Component({
    props: ['claims'],
    components: { DashboardView, DetailAccount },
    created: async function(this: GuestAccount) {

    }
  })
  export default class GuestAccount extends Vue {

    isBusy: boolean = false

    data: any = {
      layout : 'view', // view, edit, detail
      selectedAccount: {}
    }

    handleNavigation(data) {
      this.data = data
    }

  }
</script>