<template>
  <div>
    <loading-overlay :loading="isBusy"></loading-overlay>

    <div class="row mx-0">
      <div class="col">
        <h3>
            <fa icon="folder-open"></fa> My Tools >
            <span>Service Tips</span>
        </h3>
      </div>

      <div class="col-md-12 mt-3 p-0">
        <!-- <kodawari-menus /> -->
        <div class="row talent_bg_white talent_padding talent_bg_shadow m-0">
          <service-tips-form/>
        </div>
      </div>
    </div>

  </div>
</template>

<script lang="ts">
  import { IParamDictionaries, IAllDictionaries, DictionaryModel } from '../../services/Dictionary/DictionaryModel';
  import Vue from 'vue';
  import Component from 'vue-class-component';
  import { BlobService } from '../../services/NSwagService';
  import ModalDelete from '../../components/shared/ModalDelete.vue';
  import FilterBar from '../../components/ProductInformation/FilterBar.vue';
  import ModalCategory from '../../components/ProductInformation/ModalCategory.vue';
  import ModalModel from '../../components/ProductInformation/ModalModel.vue';
  import { ProductInformationService } from '../../services/ProductInformation/ProductInformationService';
  import ProductMenus from '../../components/ProductInformation/ProductMenus.vue';
  import BasicInformation from '../../components/ProductInformation/Detail/BasicInformation.vue';
  import ProductType from '../../components/ProductInformation/Detail/ProductType.vue';
  import Faq from '../../components/ProductInformation/Detail/Faq.vue';
  import PhotoGallery from '../../components/ProductInformation/Detail/PhotoGallery.vue';
  import Photo from '../../components/ProductInformation/Detail/Photo.vue';
  import Customer from '../../components/ProductInformation/Detail/Customer.vue';
  import ProductCompetitor from '../../components/ProductInformation/Detail/ProductCompetitor.vue';
  import Tips from '../../components/ProductInformation/Detail/Tips.vue';
  import Spwa from '../../components/ProductInformation/Detail/Spwa.vue';
import KodawariMenus from '../../components/Kodawari/KodawariMenus.vue';
import Banner from '../../components/Kodawari/Detail/Banner.vue';
import MainMenu from '../../components/Kodawari/Detail/MainMenu.vue';
import Downloads from '../../components/Kodawari/Detail/Downloads.vue';
import KeyPeaceOfMindForm from '../../components/KeyPeaceOfMind/Detail/KeyPeaceOfMindForm.vue';
import ServiceTipsForm from '../../components/KeyPeaceOfMind/Detail/ServiceTipsForm.vue';
    
  @Component({
  components: { ModalDelete, FilterBar, ModalCategory, ModalModel, ProductMenus, BasicInformation, ProductType, Faq, PhotoGallery, Photo, Customer, ProductCompetitor, Tips, Spwa, KodawariMenus, Banner, MainMenu, Downloads, KeyPeaceOfMindForm, ServiceTipsForm },
    props: ['claims'],
    created: async function(this: ProductInformationPage) {
    }
  })
  export default class ProductInformationPage extends Vue {
    isBusy: boolean = false
    blobService: BlobService = new BlobService()
    dictionaryService: ProductInformationService = new ProductInformationService()
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
    selectedData: DictionaryModel = null

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

    async fetchDatas () {
      this.isBusy = true
      try {
        const res : any = await this.dictionaryService.getAllProducts(this.params)
        this.data = res.products
        this.totalData = res.totalProducts
      } catch (err) {
        console.error(err)
      }
      this.isBusy = false
    }

    async getFile(id) {
      const file = await this.blobService.getImageDetail(id)
      return file.fileUrl
    }

    // filter 
    handleFilter() {
      this.params.PageIndex = 1
      this.fetchDatas()
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
        this.fetchDatas()
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