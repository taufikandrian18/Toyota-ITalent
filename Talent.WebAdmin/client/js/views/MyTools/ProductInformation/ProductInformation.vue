<template>
  <div>
    <loading-overlay :loading="isBusy"></loading-overlay>

    <div class="row">
      <div class="col">
        <h3>
            <fa icon="chart-pie"></fa> My Tools >
            <span>Product Information > </span> 
            <span class="talent_font_red">Tipe & Model</span>
        </h3>
      </div>

      <div class="col-md-12 mt-3">
        <div class="row mb-3 mx-0 px-0">
          <div class="col">
            <filter-bar :filters="filters" @callback="handleFilterCategory"/>
          </div>
          <div class="col-auto mx-0 px-0">
            <button class="btn talent_bg_cyan talent_font_white" 
              data-toggle="modal"
              data-target="#modalModel">Add New Model</button>
          </div>
        </div>
        <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
          <!-- <h5>Product Information</h5>
          <hr /> -->
          <div class="col-md-12 talent_magin_small mt-3">
              <div class="row align-items-center justify-content-between">
                  <div class="col-md-6 d-flex py-0 pl-0 pr-4">
                      <!-- <button class="btn btn-info mr-2" >
                          Filter
                      </button> -->

                      <div class="dropdown">
                        <button class="btn btn-info mr-2" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-expanded="false" @click="handleFilter">
                          Filter
                        </button>
                        <!-- <div class="dropdown-menu p-3" aria-labelledby="dropdownMenuButton" style="min-width: 500px" @click="handlePreventMenuClose">
                          <b>Filter</b>
                          <div class="row">
                          </div>
                          <div class="row d-flex flex-wrap">
                            <hr class="w-100">
                            <div class="w-100 mt-1 d-flex justify-content-end px-3">
                              <button data-toggle="dropdown" class="btn btn-info mr-3" @click="handleFilter">Apply</button>
                              <button data-toggle="dropdown" class="btn btn-info mr-3" @click="handleResetFilter">Reset</button>
                              <button data-toggle="dropdown" class="btn btn-danger" @click="hideDropdown">Cancel</button>
                            </div>
                          </div>
                        </div> -->
                      </div>
                      <div class="input-group">
                          <input class="form-control" placeholder="Search By Name" v-model="params.ProductName" @keyup.enter="handleFilter"/>
                      </div>
                  </div>
                  <div class="col-md-6">
                    <div class="row justify-content-end">
                      <button 
                      :class="`btn talent_bg_green talent_font_white text-center mr-2`">
                          <fa :icon="'edit'"></fa> : Edit
                      </button>
                      <button 
                      :class="`btn talent_bg_green talent_font_white text-center mr-2`">
                          <fa :icon="'eye'"></fa> : Published
                      </button>
                      <button 
                      :class="`btn talent_bg_red talent_font_white text-center`" >
                          <fa :icon="'eye-slash'"></fa> : Hide
                      </button>
                    </div>
                  </div>
                  <div class="col-8 row d-flex justify-content-end">
                  </div>
              </div>
          </div>

          <div class="w-100 mt-3">
              <div class="row">
                <div v-for="product in data" :key="product.productId" class="col-md-4 p-2">
                  <div class="product-item">
                    <div class="row m-0">
                      <div class="col-md-12 p-0">
                        <div class="product-image-container">
                          <img :src="product.imageUrl" alt="" class="product-image">
                          <div style="bottom: 6px; right: 6px; position: absolute">
                            <button class="btn btn-success text-center" @click="handleEdit(product.productId)">
                                <fa icon="edit" />
                            </button>
                            <button 
                            :class="`btn ${product.approvedAt ? 'talent_bg_green talent_font_white' : 'talent_bg_red talent_font_white'} text-center`" 
                            @click="handleEditStatus(product)">
                                <fa :icon="product.approvedAt ? 'eye' : 'eye-slash'"></fa>
                            </button>
                          </div>
                        </div>
                      </div>
                      <div class="col-md-12 p-3" style="border-top: 1px solid grey">
                        <span class="px-4">
                          {{ product.productName }}
                        </span>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
          </div>

          <!--Pagination-->
          <div class="col-md-12 d-flex justify-content-center">
              <paginate :currentPage.sync="params.PageIndex" :total-data="totalData" :limit-data="10" @update:currentPage="fetchDatas()"></paginate>
          </div>
      </div>
      </div>
    </div>

    <modal-delete
      name="modalDelete"
      @delete="handleDelete"
    />

    <modal-category name="modalCategory"/>
    <modal-model name="modalModel" />

  </div>
</template>

<script lang="ts">
  import { IParamDictionaries, IAllDictionaries, DictionaryModel } from '../../../services/Dictionary/DictionaryModel';
  import Vue from 'vue';
  import Component from 'vue-class-component';
  import { BlobService } from '../../../services/NSwagService';
  import ModalDelete from '../../../components/shared/ModalDelete.vue';
  import FilterBar from '../../../components/ProductInformation/FilterBar.vue';
  import ModalCategory from '../../../components/ProductInformation/ModalCategory.vue';
  import ModalModel from '../../../components/ProductInformation/ModalModel.vue';
  import { ProductInformationService } from '../../../services/ProductInformation/ProductInformationService';
  import $ from 'jquery'
import moment from 'moment';
    
  @Component({
  components: { ModalDelete, FilterBar, ModalCategory, ModalModel },
    props: ['claims'],
    created: async function(this: ProductInformationPage) {
      this.fetchDatas()
      this.fetchCategories()
      const vm = this
      $('#modalCategory').on('hidden.bs.modal', function (e) {
        vm.fetchCategories()
      })
      $('#modalModel').on('hidden.bs.modal', function (e) {
        vm.fetchDatas()
      })
    }
  })
  export default class ProductInformationPage extends Vue {
    isBusy: boolean = false
    blobService: BlobService = new BlobService()
    dictionaryService: ProductInformationService = new ProductInformationService()
    params: any = {
      StartDate: null,
      EndData: null,
      ProductName: '',
      ProductCategoryName: '',
      PageIndex: 1,
      PageSize: 10,
      SortBy: null
    }
    paramsCategories: any = {
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

    async fetchCategories () {
      this.isBusy = true
      try {
        const res : any = await this.dictionaryService.getAllCategories(this.paramsCategories)
        this.filters = [
          {
            key: 'ALL',
            value: ''
          },
          ...res.productCategories.map(v => ({
            key: v.productCategoryName,
            value: v.productCategoryName
          }))
        ]
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
    handleFilterCategory(filter) {
      this.params.PageIndex = 1
      this.params.ProductCategoryName=filter.value
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
      window.location.href = `/MyTools/ProductInformationDetail?id=${id}&page=BasicInformation`
    }

    async handleEditStatus(product) {
      this.isBusy = true
      try {
        await this.dictionaryService.updateProduct({
            productId: product.productId,
            productName: product.productName,
            productCategoryId: product.productCategoryId,
            productSegment: product.productSegment,
            isCompetitor: product.isCompetitor,
            approvedAt: product.approvedAt ? null : moment().format('YYYY-MM-DD')
        })
        this.fetchDatas()
      } catch (err) {
        console.error(err)
      }
      this.isBusy = false
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