<template>
  <div class="row mx-0">
    <ul class="nav nav-tabs nav-justified">
      <li v-for="menu in menus" :key="menu.key" class="nav-item">
        <a :class="`nav-link ${page === menu.key ? menu.isSub ? 'active-sub' : 'active' : ''} menu-item h-100`" aria-current="page" @click="handleRoute(menu)">{{ menu.value }}</a>
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
          key: 'BasicInformation',
          value: 'Basic Information',
          isSub: false
        },
        {
          key: 'ProductType',
          value: 'Product Type',
          isSub: false
        },
        {
          key: 'PhotoGallery',
          value: 'Color Gallery',
          isSub: true
        },
        {
          key: 'ContributePhotos',
          value: 'Contribute Photos',
          isSub: true
        },
        {
          key: 'Photo360',
          value: 'Photo 360',
          isSub: true
        },
        {
          key: 'CustomerProfile',
          value: 'Customer Profile',
          isSub: true
        },
        {
          key: 'ProductFeatures',
          value: 'Product Features',
          isSub: true
        },
        {
          key: 'ProductCompetitor',
          value: 'Product Competitor',
          isSub: true
        },
        {
          key: 'SPWATestDrive',
          value: 'SPWA & Test Drive',
          isSub: true
        },
        {
          key: 'NonProductProgram',
          value: 'Non Product & Program',
          isSub: true
        },
        {
          key: 'Tips',
          value: 'Tips',
          isSub: true
        },
        {
          key: 'FAQ',
          value: 'FAQ',
          isSub: true
        }
      ]
    }
  },
  methods: {
    handleRoute(menu) {
      window.location.href = `/MyTools/ProductInformationDetail?id=${this.id}&page=${menu.key}`
    }
  },
  computed: {
    page() {
      let uri = window.location.search.substring(1); 
      let params = new URLSearchParams(uri);
      return params.get("page")
    },
    id() {
      let uri = window.location.search.substring(1); 
      let params = new URLSearchParams(uri);
      return params.get("id")
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
}
.active {
  color: #0085CA !important;
  border-top: 3px solid #0085CA !important;
}
.active-sub {
  color: #60D408 !important;
  border-top: 3px solid #60D408 !important;
  background: white;
}
</style>