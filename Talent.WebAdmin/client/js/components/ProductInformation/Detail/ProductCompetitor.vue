<template>
  <div class="row px-4 py-4">
    <loading-overlay :loading="isBusy"></loading-overlay>
    <div class="col-md-12 p-0 mb-4"  v-if="errored.length > 0">
      <div class="alert alert-danger alert-dismissible fade show" role="alert" v-if="errored.length > 0">
          <ul>
            <li v-for="error in errored" :key="error.key">{{error.value}}</li>
          </ul>
      </div>
    </div>
    <div class="col-md-12 p-0 mb-4"  v-if="success">
      <div class="alert alert-success alert-dismissible fade show" role="alert">
        Success
      </div>
    </div>
    <div class="col-md-12 mb-3">
      <h5>Product Competitor ( Default Tampilan Perbandingan Spesifikasi)</h5>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <div class="input-group talent_front">
            <label for="" class="w-100">Product Type<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="ProductType"
                :options="categories"
                label="productTypeName"
                track-by="productTypeId"
                v-model="selectedCategory">
              </multiselect>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Product Competitor<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="ProductCompetitor"
                :options="productCompetitor"
                label="productName"
                track-by="productId"
                v-model="selectedProductCompetitor">
              </multiselect>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Product Competitor Type<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="ProductCompetitorType"
                :options="productCompetitorType"
                label="productTypeName"
                track-by="productTypeId"
                v-model="selectedProductCompetitorType">
              </multiselect>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="col-md-12 d-flex justify-content-end mt-4">
      <button class="btn btn-secondary mr-2" @click="reset">
          Close
      </button>
      <button class="btn btn-success mr-2" @click="submit(null)">
          {{!selectedEditData ? 'Save' : 'Save'}}
      </button>
      <button class="btn btn-primary" @click="submit(1)">
          Submit
      </button>
    </div>

    <!-- divider -->
    <div class="col-md-12 py-2">
      <hr>
    </div>

    <!-- table data -->
    <div class="col-md-12">
      <!-- filter -->
      <div class="col-md-12 talent_magin_small mt-3">
          <div class="row align-items-center justify-content-between">
              <div class="col d-flex py-0 pl-0 pr-4">
                  <div class="dropdown">
                    <button class="btn btn-info mr-2" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-expanded="false" @click="handleFilter">
                      Filter
                    </button>
                  </div>
                  <div class="input-group">
                      <input class="form-control" placeholder="Search By Product Competitor Name" v-model="params.ProductCompetitorName"/>
                  </div>
              </div>
              <div class="col-8 row d-flex justify-content-end">
              </div>
          </div>
      </div>
      <!-- table -->
      <div class="w-100 talent_overflowx mt-3">
          <table class="table table-bordered table-responsive-md talent_table_padding">
              <thead>
                  <tr>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Product Type
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Product Competitor
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Product Competitor Type
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Updated At
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Published
                          </a>
                      </th>
                      <th scope="col" class="text-center" colspan="2">
                          <a href="#" class="talent_sort" style="color: white;">
                            Action
                          </a>
                      </th>
                  </tr>
              </thead>
              <tbody>
                  <tr v-for="dictionary in productTypes" :key="dictionary.productTypeId">
                      <td>
                          {{dictionary.productTypeName}}
                      </td>
                      <td>
                          {{dictionary.productCompetitorName}}
                      </td>
                      <td>
                          {{dictionary.productTypeCompetitorName}}
                      </td>
                      <td style="max-width: 150px;white-space: normal">
                          {{moment(dictionary.updatedAt).format('DD/MM/YYYY')}}
                      </td>
                      <td>
                          <button
                            :class="`btn btn-block ${dictionary.approvedAt ? 'talent_bg_green talent_font_white' : 'talent_bg_red talent_font_white'}`" 
                            @click="handleEditStatus(dictionary)">
                              <fa :icon="dictionary.approvedAt ? 'eye' : 'eye-slash'"></fa>
                          </button>
                      </td>
                      <td class="talent_nopadding_important">
                          <button class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(dictionary)">Edit</button>
                      </td>
                      <td class="talent_nopadding_important">
                        <button type="button" class="btn btn-block talent_bg_red talent_font_white" 
                                  data-toggle="modal"
                                  data-target="#modalDelete" @click="selectedData = dictionary">
                          Remove
                        </button>
                      </td>
                  </tr>
              </tbody>
          </table>
      </div>

      <!--Pagination-->
      <div class="col-md-12 d-flex justify-content-center">
          <paginate :currentPage.sync="params.PageIndex" :total-data="totalData" :limit-data="10" @update:currentPage="fetchFAQ()"></paginate>
      </div>
    </div>


    <modal-delete
      name="modalDelete"
      @delete="handleDelete"
    />
  </div>
</template>


<script>
import { ProductInformationService } from '../../../services/ProductInformation/ProductInformationService';
import $ from 'jquery'
import ModalDelete from '../../shared/ModalDelete.vue';
import moment from 'moment'

export default {
  components: { ModalDelete },
    async mounted() {
      this.isBusy = true
      try {
        await this.fetchCategories()
        await this.fetchProductCompetitor()
        await this.fetchFAQ()
      } catch(err) {
        console.error(err)
      }
      this.isBusy = false
    },
    watch: {
      async selectedProductCompetitor() {
        this.isBusy = true
        try {
          await this.fetchCategoriesCompetitor({
            ProductId: this.selectedProductCompetitor.productId,
            SortBy: '',
            PageIndex: 1,
            PageSize: 100
          })
        } catch(err) {
          console.error(err)
        }
        this.isBusy = false
      }
    },
    data() {
      return {
        moment: moment,
        isBusy: false,
        errored: [],
        selectedFile: null,
        productTypes: [],
        categories: [],
        totalData: 0,
        selectedCategory: null,
        service: new ProductInformationService(),
        params: {
          ProductId: '',
          ProductCompetitorName: '',
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
        },
        paramsCategories: {
          ProductId: "",
          SortBy: '',
          PageIndex: 1,
          PageSize: 100
        },
        name: '',
        salesTalk: '',
        image: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        success: false,
        selectedData: null,
        selectedEditData: null,
        productCompetitor: [],
        productCompetitorType: [],
        selectedProductCompetitor: null,
        selectedProductCompetitorType: null
      }
    },
    methods: {
      reset() {
        this.success = false
        this.selectedCategory = null
        this.selectedProductCompetitor = null
        this.selectedProductCompetitorType = null
        this.selectedEditData = null
        window.location.href = `/MyTools/ProductInformation`
      },
      async fetchProductCompetitor() {
        try {
          const res = await this.service.getAllProductCompetitor({})
          this.productCompetitor = res
        } catch(err) {
          console.error(err)
        }
      },
      async fetchCategories() {
        this.paramsCategories.ProductId = this.id
        try {
          const res = await this.service.getAllProductTypes(this.paramsCategories)
          this.categories = res.productTypes
        } catch(err) {
          console.error(err)
        }
      },
      async fetchCategoriesCompetitor(query) {
        try {
          const res = await this.service.getAllProductTypes(query)
          this.productCompetitorType = res.productTypes
          if(!this.productCompetitorType.map(v => v.productTypeId).includes(this.selectedProductCompetitorType.productTypeId)) {
            this.selectedProductCompetitorType = null
          }
        } catch(err) {
          console.error(err)
        }
      },
      async fetchFAQ() {
        try {
          this.params.ProductId = this.id
          const res = await this.service.getAllProductCompetitorMapping(this.params)
          this.productTypes = res.productCompetitorMappings
          this.totalData = res.totalProductCompetitorMapping
        } catch(err) {
          console.error(err)
        }
      },
      async submit(e) {
        if(this.handleValidation()) {
          return
        }
        this.isBusy = true
        this.success = false
        try {
          const body = {
            productId: this.id,
            "productTypeId": this.selectedCategory.productTypeId,
            "productCompetitorId": this.selectedProductCompetitor.productId,
            "productCompetitorTypeId": this.selectedProductCompetitorType.productTypeId,
            approvedAt: e ? moment().format('YYYY-MM-DD')  : null
          }
          if(!this.selectedEditData) {
            await this.service.createProductCompetitorMapping(body)
          } else {
            body.productCompetitorMappingId = this.selectedEditData.productCompetitorMappingId
            await this.service.updateProductCompetitorMapping(body)
          }
          this.params.PageIndex = 1
          this.fetchFAQ()
          this.success = true
          this.selectedCategory = null
          this.selectedProductCompetitor = null
          this.selectedProductCompetitorType = null
          this.selectedEditData = null
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleValidation () {
        this.errored = []
        if(!this.selectedCategory) {
          this.errored.push({
            key: 'product type',
            value: 'Please fill product type field'
          })
        }
        if(!this.selectedProductCompetitor) {
          this.errored.push({
            key: 'product competitor',
            value: 'Please fill product competitor field'
          })
        }
        if(!this.selectedProductCompetitorType) {
          this.errored.push({
            key: 'product competitor Type',
            value: 'Please fill product competitor Type field'
          })
        }
        return this.errored.length > 0
      },
      handleFilter() {
        this.params.PageIndex = 1
        this.fetchFAQ()
      },
      async handleDelete() {
        this.isBusy = true
        try {
          await this.service.deleteProductCompetitorMapping(this.selectedData.productCompetitorMappingId)
          this.fetchFAQ()
          this.success = true
          this.selectedCategory = null
          this.selectedProductCompetitor = null
          this.selectedProductCompetitorType = null
          this.selectedEditData = null
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(data) {
        this.selectedEditData = data
        this.selectedProductCompetitor = {
          productId: data.productCompetitorId,
          productName: data.productCompetitorName
        }
        this.selectedProductCompetitorType = {
          productTypeId: data.productCompetitorTypeId,
          productTypeName: data.productTypeCompetitorName
        }
        this.selectedCategory = {
          productTypeId: data.productTypeId,
          productTypeName: data.productTypeName
        }
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateProductCompetitorMapping({
            productCompetitorMappingId: data.productCompetitorMappingId,
            productId: data.productId,
            "productTypeId": data.productTypeId,
            "productCompetitorId": data.productCompetitorId,
            "productCompetitorTypeId": data.productCompetitorTypeId,
            approvedAt: data.approvedAt ? null : moment().format('YYYY-MM-DD')
        })
        this.fetchFAQ()
        this.isBusy = false
      },
      async handleChangeFile(e) {
        const selectedImage = e.target.files[0]
        // if(selectedImage.size > 104857600) {
        //     this.image.base64 = ''
        //     this.image.contentType = ''
        //     this.image.fileName = ''
        //     return
        // }
        let array = selectedImage.name.split(".");
        let extension = array.pop();
        const res = await this.getBase64(selectedImage)
        this.image.base64 = res
        this.image.fileName = selectedImage.name
        this.image.contentType = extension
      },
      getBase64(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
            let encoded = reader.result.toString().replace(/^data:(.*,)?/, '');
            if ((encoded.length % 4) > 0) {
                encoded += '='.repeat(4 - (encoded.length % 4));
            }
            this.selectedFile = reader.result
            resolve(encoded);
            };
            reader.onerror = error => reject(error);
        });
      }
    },
    computed: {
      id() {
        let uri = window.location.search.substring(1); 
        let params = new URLSearchParams(uri);
        return params.get("id")
      }
    }
}
</script>

<style scoped>
.container-upload {
  width: 100%;
  border: 1px solid #000000;
  background-color: rgb(146, 145, 145);
  position: relative;
  display: flex;
}
.btn-upload {
  position: absolute;
  bottom: 0;
  display: flex;
  justify-content: center;
  width: 100%;
  padding: 16px 0px;
}
</style>