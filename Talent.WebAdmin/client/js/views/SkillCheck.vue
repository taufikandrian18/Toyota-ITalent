<template>
  <div>
    <loading-overlay :loading="isBusy"></loading-overlay>

    <div class="row mx-0">
      <div class="col">
        <h3>
            <fa icon="folder-open"></fa> Master Data >
            <span>Setup Skill Check</span>
        </h3>
      </div>
      <br>
      <skill-check-form />
    </div>

  </div>
</template>

<script lang="ts">
  import Vue from 'vue';
  import Component from 'vue-class-component';
  import SkillCheckForm from '../components/Setup/SkillCheckForm.vue';
    
  @Component({
  components: {SkillCheckForm },
    props: ['claims'],
    created: async function(this: ProductInformationPage) {
    }
  })
  export default class ProductInformationPage extends Vue {
    isBusy: boolean = false
    // service: ProductInformationService = new ProductInformationService()
    params: any = {
      StartDate: null,
      EndData: null,
      ProductCategoryName: '',
      PageIndex: 1,
      PageSize: 10,
      SortBy: null
    }
    totalData: Number = 0
    data: any = []
    filters: Array<any> = [
      {
        key: 'ALL',
        value: ''
      }
    ]

    get page() {
      let uri = window.location.search.substring(1); 
      let params = new URLSearchParams(uri);
      return params.get("page")
    }
    // async fetchDatas () {
    //   this.isBusy = true
    //   try {
    //     const res : any = await this.dictionaryService.getAllProducts(this.params)
    //     this.data = res.products
    //     this.totalData = res.totalProducts
    //   } catch (err) {
    //     console.error(err)
    //   }
    //   this.isBusy = false
    // }
    // filter 
    handleFilter() {
      this.params.PageIndex = 1
      // this.fetchDatas()
    }
    handleResetFilter() {
    }
    hideDropdown () {
    }
    handleSearch() {
    }
    // end filter
    async handleDelete() {
      this.isBusy = true
      try {
        // await this.dictionaryService.deleteDictionary(this.selectedData.dictionaryId)
        // this.fetchDatas()
      } catch (err) {
        console.error(err)
      }
      this.isBusy = true
    }
    handleEdit(id) {
      window.location.href = `/MyTools/ProductInformationDetail?id=${id}`
    }
    handlePreventMenuClose(e) {
      e.stopPropagation();
    }

    fetchdata() {

    }

    resetsearch() {

    }

  }
</script>

<style scoped>
.product-item {
  border: 1px solid grey;
  border-radius: 10px;
  overflow: hidden;
}
.product-image-container {
  position: relative;
  padding-top: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}
.product-image {
  position: absolute;
  width: 100%;
  top: 0;
  left: 0;
  bottom: 0;
  right: 0;
  object-fit: cover;
}
</style>