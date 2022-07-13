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
      <h5>FAQ</h5>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Category<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Category"
                :options="categories"
                label="productFaqCategoryName"
                track-by="productFaqCategoryId"
                v-model="selectedCategory">
              </multiselect>
              <button class="btn_add_rounded" @click="handleAddSub">
                +
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <label for="" class="w-100">Media<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <div class="container-upload">
                <img v-if="!['mp4', 'pdf'].includes(image.contentType)" class="h-100 w-100" :src="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                <video v-if="image.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFile ? selectedFile : `/upload-image2.png`"></video>
                
                <!-- <div class="btn-upload"> -->
                  <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                    <label for="input-manfaat" style="margin: 0px">Browse</label>
                  </button>
                <!-- </div> -->
                  <input type="file" name="input-manfaat" id="input-manfaat" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg,.pdf,.gif,.mp4">
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <label>Sequence</label>
          <div class="input-group">
              <input name="sequence" type="number" class="form-control" v-model="sequence"/>
          </div>
        </div>
      </div>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <label>Topic<span class="talent_font_red"> *</span></label>
          <div class="input-group">
              <input name="topic" type="text" class="form-control" v-model="topic"/>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <label for="" class="w-100">Description<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <wysiwyg 
                  class="w-100" 
                  v-model="description" 
                  :options="{
                      hideModules: {image: true, table: true, removeFormat: true, code: true }
                  }"/>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="col-md-12 mt-4">
      <object v-if="image.contentType === 'pdf'" class="h-100 w-100 min-heigth-full" type="application/pdf" width="100%" height="100%" :data="selectedFile ? selectedFile : `/upload-image2.png`" for="customFileFile" />
    </div>
    <div class="col-md-12 d-flex justify-content-end mt-4">
      <button class="btn btn-secondary mr-2" @click="reset">
          Close
      </button>
      <button class="btn btn-success mr-2" @click="submit(null)">
          {{!selectedEditData? 'Save' : 'Save' }}
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
                      <input class="form-control" placeholder="Search By Name" v-model="params.ProductFaqTitle"/>
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
                            User Fullname
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Category
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Topic
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Description
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Created At
                          </a>
                      </th>
                      <th scope="col" class="text-center">
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
                          {{dictionary.createdBy}}
                      </td>
                      <td>
                          {{dictionary.productFaqCategoryName}}
                      </td>
                      <td>
                          {{dictionary.productFaqTitle}}
                      </td>
                      <td style="max-width: 300px;white-space: normal" v-html="dictionary.productFaqDescription">
                      </td>
                      <td style="max-width: 150px;white-space: normal">
                          {{moment(dictionary.createdAt).format('DD/MM/YYYY')}}
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

    <!-- divider -->
    <div class="col-md-12 py-2">
      <hr>
    </div>


    <div class="col-md-12 mb-3">
      <h5>Question From User</h5>
    </div>

    <!-- table data -->
    <div class="col-md-12">
      <!-- filter -->
      <!-- <div class="col-md-12 talent_magin_small mt-3">
          <div class="row align-items-center justify-content-between">
              <div class="col d-flex py-0 pl-0 pr-4">
                  <div class="dropdown">
                    <button class="btn btn-info mr-2" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-expanded="false" @click="handleFilter">
                      Filter
                    </button>
                  </div>
                  <div class="input-group">
                      <input class="form-control" placeholder="Search By Name" v-model="params.ProductTypeName"/>
                  </div>
              </div>
              <div class="col-8 row d-flex justify-content-end">
              </div>
          </div>
      </div> -->
      <!-- table -->
      <div class="w-100 talent_overflowx mt-3">
          <table class="table table-bordered table-responsive-md talent_table_padding">
              <thead>
                  <tr>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            User Fullname
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Outlet
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Position
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Category
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Question
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Created At
                          </a>
                      </th>
                  </tr>
              </thead>
              <tbody>
                  <tr v-for="dictionary in faqUser" :key="dictionary.productFAQUserId">
                      <td>
                          {{dictionary.createdBy}}
                      </td>
                      <td>
                          {{dictionary.outletName}}
                      </td>
                      <td>
                          {{dictionary.positionName}}
                      </td>
                      <td style="">
                          {{dictionary.productFaqCategoryName}}
                      </td>
                      <td style="max-width: 300px;white-space: normal !important; text-overflow: initial !important;" v-html="dictionary.productFAQUserQuestion">
                      </td>
                      <td style="max-width: 150px;white-space: normal">
                          {{moment(dictionary.createdAt).format('DD/MM/YYYY')}}
                      </td>
                  </tr>
              </tbody>
          </table>
      </div>

      <!--Pagination-->
      <div class="col-md-12 d-flex justify-content-center">
          <paginate :currentPage.sync="params.PageIndex" :total-data="totalDataUser" :limit-data="10" @update:currentPage="fetchFAQ()"></paginate>
      </div>
    </div>


    <modal-delete
      name="modalDelete"
      @delete="handleDelete"
    />

    <modal-faq-category name="modalCategoryFaq"/>
  </div>
</template>


<script>
import { ProductInformationService } from '../../../services/ProductInformation/ProductInformationService';
import $ from 'jquery'
import ModalDelete from '../../shared/ModalDelete.vue';
import moment from 'moment'
import ModalFaqCategory from '../ModalFaqCategory.vue';

export default {
  components: { ModalDelete, ModalFaqCategory },
    async mounted() {
      this.isBusy = true
      try {
        await this.fetchCategories()
        await this.fetchFAQ()
        await this.fetchFAQUser()
      } catch(err) {
        console.error(err)
      }
      this.isBusy = false

      const vm = this
      $('#modalCategoryFaq').on('hidden.bs.modal', function (e) {
        vm.fetchCategories()
      })
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
        totalDataUser: 0,
        selectedCategory: null,
        service: new ProductInformationService(),
        params: {
          ProductFaqTitle: '',
          ProductId: '',
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
        },
        paramsUser: {
          ProductId: '',
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
        },
        paramsCategories: {
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
        sequence: 1,
        topic : '',
        description: '' ,
        faqUser: []
      }
    },
    methods: {
      handleAddSub() {
        $('#modalCategoryFaq').modal('show')
      },
      reset() {
        this.success = false
        this.topic = ''
        this.description = ''
        this.sequence = 1
        this.selectedFile = null
        this.image = {
          base64: '',
          fileName: '',
          contentType: ''
        }
        this.selectedEditData = null
        window.location.href = `/MyTools/ProductInformation`
      },
      async fetchCategories() {
        try {
          const res = await this.service.getAllProductFAQCategories( {
          ProductId: this.id,
          SortBy: '',
          PageIndex: 1,
          PageSize: 100
        })
          this.categories = res.productFAQCategories
        } catch(err) {
          console.error(err)
        }
      },
      async fetchFAQ() {
        try {
          this.params.ProductId = this.id
          const res = await this.service.getAllProductFAQ(this.params)
          this.productTypes = res.productFAQs
          this.totalData = res.totalProductFAQs
        } catch(err) {
          console.error(err)
        }
      },
      async fetchFAQUser() {
        try {
          this.paramsUser.ProductId = this.id
          const res = await this.service.getAllProductFAQUser(this.paramsUser)
          this.faqUser = res.productFAQUsers
          this.totalDataUser = res.totalProductFAQUsers
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
            productFaqCategoryId: this.selectedCategory.productFaqCategoryId,
            productGalleryFileContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            },
            "productFaqSequence": this.sequence,
            "productFaqTitle": this.topic,
            "productFaqDescription": this.description,
            approvedAt: e ?moment().format('YYYY-MM-DD')  : null
          }
          if(!this.selectedEditData) {
            await this.service.createProductFAQ(body)
          } else {
            if(!this.image.base64) {
              delete body.productGalleryFileContent
            }
            body.productFaqId = this.selectedEditData.productFaqId
            await this.service.updateProductFAQ(body)
          }
          this.params.PageIndex = 1
          this.fetchFAQ()
          this.success = true
          this.topic = ''
          this.description = ''
          this.sequence = 1
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
        if(!this.topic) {
          this.errored.push({
            key: 'topic',
            value: 'Please fill topic field'
          })
        }
        if(!this.sequence) {
          this.errored.push({
            key: 'sequence',
            value: 'Please fill sequence field'
          })
        }
        if(!this.description) {
          this.errored.push({
            key: 'description',
            value: 'Please fill description field'
          })
        }
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
          await this.service.deleteProductFAQ(this.selectedData.productFaqId)
          this.fetchFAQ()
          this.success = true
          this.sequence = 1
          this.topic = ''
          this.description = ''
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
        this.topic = data.productFaqTitle
        this.description = data.productFaqDescription
        this.sequence = data.productFaqSequence
        this.selectedCategory = {
          productFaqCategoryId: data.productFaqCategoryId,
          productFaqCategoryName: data.productFaqCategoryName
        }
        this.selectedFile = data.imageUrl
        this.image.contentType = data.blob.mime
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateProductFAQ({
          productFaqId: data.productFaqId,
          productId: data.productId,
          productFaqCategoryId: data.productFaqCategoryId,
          "productFaqSequence": data.productFaqSequence,
          "productFaqTitle": data.productFaqTitle,
          "productFaqDescription": data.productFaqDescription,
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
        this.selectedFile = URL.createObjectURL(selectedImage)
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
            // this.selectedFile = reader.result
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
  min-height: 150px;
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