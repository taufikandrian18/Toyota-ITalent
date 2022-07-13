<template>
  <div class="row px-4 py-4 w-100">
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
      <h5>Color Gallery</h5>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Type<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Category"
                :options="categories"
                label="productTypeName"
                track-by="productTypeId"
                v-model="selectedCategory">
              </multiselect>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <label for="" class="w-100">Media<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <div class="container-upload">
                <img :src="selectedFile ? selectedFile : `/upload-image2.png`" alt="" style="width: 100%">
                <!-- <div class="btn-upload"> -->
                  <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                    <label for="input-manfaat" style="margin: 0px">Browse</label>
                  </button>
                <!-- </div> -->
                  <input type="file" name="input-manfaat" id="input-manfaat" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg">
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <checkbox class="talent-checkbox-lineheight"
                        id="is_color"
                        v-model="isColor">Color Related Photo</checkbox>
        </div>
        <div class="col-md-12 mt-4">
          <label>Color Code</label>
          <div class="input-group">
            <div class="input-group-prepend">
              <span class="input-group-text" id="basic-addon1">
                <verte v-model="colorCode" model="hex"></verte>
              </span>
            </div>
            <input name="color_code" type="text" class="form-control" v-model="colorCode"/>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <label>Color Name</label>
          <div class="input-group">
              <input name="color_name" type="text" class="form-control" v-model="colorName"/>
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
    <div class="col-md-12" v-if="!selectedEditData">
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
                      <input class="form-control" placeholder="Search By Color Name" v-model="params.ProductGalleryColorName" @keyup.enter="handleFilter"/>
                  </div>
              </div>
              <div class="col-8 row d-flex justify-content-end">
              </div>
          </div>
      </div>
      <!-- table -->
      <div class="w-100 talent_overflowx mt-3" v-if="!selectedEditData">
          <table class="table table-bordered table-responsive-md talent_table_padding">
              <thead>
                  <tr>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Type
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Media
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Color Code
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Color Name
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Contributor
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Published
                          </a>
                      </th>
                      <th scope="col" class="text-center" colspan="3">
                          <a href="#" class="talent_sort" style="color: white;">
                            Action
                          </a>
                      </th>
                  </tr>
              </thead>
              <tbody>
                  <tr v-for="dictionary in productTypes" :key="dictionary.productGalleryId">
                      <td>
                          {{dictionary.productTypeName}}
                      </td>
                      <td style="white-space: normal">
                        <img :src="dictionary.imageUrl" alt="" srcset="" style="width: 100px">
                      </td>
                      <td>
                          {{dictionary.productGalleryColorCode}}
                      </td>
                      <td style="max-width: 150px;white-space: normal">
                          {{dictionary.productGalleryColorName}}
                      </td>
                      <td style="max-width: 150px;white-space: normal">
                          {{dictionary.createdBy}}
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
                      <!-- <td class="talent_nopadding_important text-center">
                        <div class="d-flex align-items-center w-100">
                          <div class="custom-control custom-switch w-100">
                            <input type="checkbox" class="custom-control-input" :id="`customSwitch-${dictionary.productGalleryId}`" :checked="dictionary.isApproved != 'Draft'" @change="handleChangeStatus(dictionary)">
                            <label class="custom-control-label" :for="`customSwitch-${dictionary.productGalleryId}`"></label>
                          </div>
                        </div>
                      </td> -->
                  </tr>
              </tbody>
          </table>
      </div>

      <!--Pagination-->
      <div class="col-md-12 d-flex justify-content-center" v-if="!selectedEditData">
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
import Verte from 'verte';

export default {
  components: { ModalDelete, Verte },
    async mounted() {
      this.isBusy = true
      try {
        await this.fetchCategories()
        await this.fetchFAQ()
      } catch(err) {
        console.error(err)
      }
      this.isBusy = false
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
          ProductGalleryColorName: '',
          ProductId: '',
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
        },
        paramsCategories: {
          ProductId: '',
          ProductFaqCategoryName: "",
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
        colorCode: '#000000',
        colorName: '',
        isColor: false
      }
    },
    methods: {
      reset() {
          this.success = false
          this.isColor = false
          this.colorName = ''
          this.colorCode = '#000000'
          this.selectedFile = null
          this.image = {
            base64: '',
            fileName: '',
            contentType: ''
          }
          if(this.selectedEditData) {
            this.selectedEditData = null
          } else {
            window.location.href = `/MyTools/ProductInformation`
          }
      },
      async fetchCategories() {
        try {
          this.paramsCategories.ProductId = this.id
          const res = await this.service.getAllProductTypes(this.paramsCategories)
          this.categories = res.productTypes
        } catch(err) {
          console.error(err)
        }
      },
      async fetchFAQ() {
        try {
          this.params.ProductId = this.id
          const res = await this.service.getAllProductGallery(this.params)
          this.productTypes = res.productGalleries
          this.totalData = res.totalProductGalleries
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
            productGalleryFileContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            },
            "productTypeId": this.selectedCategory.productTypeId,
            "productGalleryColorCode": this.colorCode,
            "productGalleryColorName": this.colorName,
            "isColor": this.isColor,
            approvedAt: e ? moment().format('YYYY-MM-DD') : null
          }
          if(!this.selectedEditData) {
            await this.service.createProductGallery(body)
          } else {
            if(!this.image.base64) {
              delete body.productGalleryFileContent
            }
            body.productGalleryId = this.selectedEditData.productGalleryId
            await this.service.updateProductGallery(body)
          }
          this.params.PageIndex = 1
          this.fetchFAQ()
          this.success = true
          this.isColor = false
          this.colorName = ''
          this.colorCode = '#000000'
          this.selectedFile = null
          this.image = {
            base64: '',
            fileName: '',
            contentType: ''
          }
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
            key: 'category',
            value: 'Please fill category field'
          })
        }
        if(!this.selectedFile) {
          this.errored.push({
            key: 'media',
            value: 'Please fill media field'
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
          await this.service.deleteProductGallery(this.selectedData.productGalleryId)
          this.fetchFAQ()
          this.success = true
          this.isColor = false
          this.colorName = ''
          this.colorCode = '#000000'
          this.selectedCategory = null
          this.selectedFile = null
          this.image = {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.selectedEditData = null
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(data) {
        this.selectedEditData = data
        this.colorCode = data.productGalleryColorCode
        this.colorName = data.productGalleryColorName
        this.isColor = data.isColor
        this.selectedCategory = {
          productTypeId: data.productTypeId,
          productTypeName: data.productTypeName
        }
        this.selectedFile = data.imageUrl
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateProductGallery({
          productGalleryId: data.productGalleryId,
          productId: data.productId,
          "productTypeId": data.productTypeId,
          "productGalleryColorCode": data.productGalleryColorCode,
          "productGalleryColorName": data.productGalleryColorName,
          "isColor": data.isColor,
          approvedAt: data.approvedAt ? null : moment().format('YYYY-MM-DD')
        })
        this.fetchFAQ()
        this.isBusy = false
      },
      async handleChangeStatus(data) {
        this.isBusy = true
        await this.service.approvalProductGallery({
          "productGalleryId": data.productGalleryId,
          "isApproved": data.isApproved === 'Draft' ? "Published" : 'Draft',
          "isUpdateProductGalleryApproval": true
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