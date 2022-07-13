<template>
  <div class="row mx-0">
    <ul class="nav nav-tabs nav-justified">
      <li v-for="menu in menus" :key="menu.key" class="nav-item">
        <a :class="`nav-link ${page === menu.key ? 'active' : ''} menu-item h-100`" aria-current="page" @click="handleRoute(menu)">{{ menu.value }}</a>
      </li>
    </ul>
  </div>
</template>

<script>
export default {
  data() {
    return {
      menus: [
        {
          key: 'Banner',
          value: 'Banner'
        },
        {
          key: 'MainMenu',
          value: 'Main Menu'
        },
        {
          key: 'Downloads',
          value: 'Downloads'
        },
        {
          key: 'DealerHOContributionFiles',
          value: 'Dealer HO Contribution Files'
        },
      ]
    }
  },
  methods: {
    handleRoute(menu) {
      window.location.href = `/MyTools/Kodawari?page=${menu.key}`
    }
  },
  computed: {
    page() {
      let uri = window.location.search.substring(1); 
      let params = new URLSearchParams(uri);
      return params.get("page")
    }
  }
}
</script>

<style scoped>
.menu-item {
  font-size: 14px !important;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 0px;
  cursor: pointer;
  background: #DADADA;
  border-radius: 0px;
  color: #9D9D9D;
  white-space: nowrap;
}
.active {
  color: #0085CA !important;
  border-top: 3px solid #0085CA !important;
}
</style>