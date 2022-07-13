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
      <h5>SPWA & Test Drive Guidance</h5>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Menu<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Menu"
                :options="cmsMenus"
                label="cms_MenuName"
                track-by="cms_MenuCategory"
                v-model="selectedCmsMenu">
              </multiselect>
              <button class="btn_add_rounded" @click="handleAddSubMenu">
                +
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Category<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Category"
                :options="categories"
                label="productSPWACategoryName"
                track-by="productSPWACategoryId"
                v-model="selectedCategory">
              </multiselect>
              <button class="btn_add_rounded" @click="handleAddSubCategory">
                +
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Content<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Content"
                :options="cmsContents"
                label="cms_ContentName"
                track-by="cms_ContentId"
                v-model="selectedCmsContent">
              </multiselect>
              <button v-if="selectedCmsMenu != null && selectedCmsMenu.cms_MenuCategory == 'SPWA'" class="btn_add_rounded" @click="handleAddSubContent">
                +
              </button>
              <button v-if="selectedCmsMenu != null && selectedCmsMenu.cms_MenuCategory == 'TEST DRIVE'" class="btn_add_rounded" @click="handleAddSubContentTestDrive">
                +
              </button>
              <!-- {{ JSON.stringify(selectedCmsMenu) }} -->
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
      </div>
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
                      <input class="form-control" placeholder="Search By Category or Content" v-model="params.Query" @keyup.enter="handleFilter"/>
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
                            Menu
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Category
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Content
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Sequence
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Created At
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
                          {{dictionary.cms_MenuName}}
                      </td>
                      <td>
                          {{dictionary.productSPWACategoryName}}
                      </td>
                      <td>
                          {{dictionary.cms_ContentName}}
                      </td>
                      <td style="max-width: 150px;white-space: normal">
                          {{dictionary.isSequence}}
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


    <modal-delete
      name="modalDelete"
      @delete="handleDelete"
    />

    <modal-cms-menu 
      name="modalCmsMenu"
      type="SPWA"/>

    <modal-spwa-category 
      name="modalSpwaCategory"/>

    <modal-cms-content-full-spwa
      name="modalCmsContentFull"
      type="SPWA"/>

    <modal-cms-content-full-test-drive
      name="modalCmsContentFullTestDrive"
      type="TEST DRIVE"/>
  </div>
</template>


<script>
import { ProductInformationService } from '../../../services/ProductInformation/ProductInformationService';
import $ from 'jquery'
import ModalDelete from '../../shared/ModalDelete.vue';
import moment from 'moment'
import ModalCmsMenu from '../ModalCmsMenu.vue';
import ModalSpwaCategory from '../ModalSpwaCategory.vue';
import ModalCmsContentFullSpwa from '../ModalCmsContentFullSpwa.vue';
import ModalCmsContentFullTestDrive from '../ModalCmsContentFullTestDrive.vue';

export default {
  components: { ModalDelete, ModalCmsMenu, ModalSpwaCategory, ModalCmsContentFullSpwa, ModalCmsContentFullTestDrive },
    async mounted() {
      this.isBusy = true
      try {
        await this.fetchCategories()
        await this.fetchCmsMenu()
        await this.fetchCmsContent()
        await this.fetchFAQ()
      } catch(err) {
        console.error(err)
      }
      this.isBusy = false

      const vm = this
      $('#modalCmsMenu').on('hidden.bs.modal', function (e) {
        vm.fetchCmsMenu()
      })
      $('#modalCmsContentFull').on('hidden.bs.modal', function (e) {
        vm.fetchCmsContent()
      })
      $('#modalCmsContentFullTestDrive').on('hidden.bs.modal', function (e) {
        vm.fetchCmsContent()
      })
      $('#modalSpwaCategory').on('hidden.bs.modal', function (e) {
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
        selectedCategory: null,
        service: new ProductInformationService(),
        params: {
          Query: '',
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

        cmsMenus: [],
        selectedCmsMenu: null,
        cmsContents: [],
        selectedCmsContent: null
      }
    },
    methods: {
      handleAddSubMenu() {
        $('#modalCmsMenu').modal('show')
      },
      handleAddSubContent() {
        $('#modalCmsContentFull').modal('show')
      },
      handleAddSubContentTestDrive() {
        $('#modalCmsContentFullTestDrive').modal('show')
      },
      handleAddSubCategory() {
        $('#modalSpwaCategory').modal('show')
      },
      async fetchCmsMenu() {
        try {
          const res = await this.service.getAllCMSMenu({
            cms_MenuCategory: 'SPWA',
            PageIndex: 1,
            PageSize: 100
          })
          this.cmsMenus = res.cmsMenus
        } catch(err) {
          console.error(err)
        }
      },
      async fetchCmsContent() {
        if(this.selectedCmsMenu != null) {
          try {
            const res = await this.service.getAllCMSContent({
              cms_ContentCategory: this.selectedCmsMenu.cms_MenuCategory,
              PageIndex: 1,
              PageSize: 100,
              ProductId: this.id
            })
            this.cmsContents = res.cmsContents
          } catch(err) {
            console.error(err)
          }
        }
      },
      async fetchCategories() {
        try {
          const res = await this.service.getAllSPWACategory({
          ProductId: this.id,
          SortBy: '',
          PageIndex: 1,
          PageSize: 100
        })
          this.categories = res.productSPWACategories
        } catch(err) {
          console.error(err)
        }
      },
      async fetchFAQ() {
        try {
          this.params.ProductId = this.id
          const res = await this.service.getAllSPWAMapping(this.params)
          this.productTypes = res.productSPWAMappings
          this.totalData = res.totalProductSPWAMapping
        } catch(err) {
          console.error(err)
        }
      },
      reset() {
        this.success = false
        this.selectedCmsContent = null
        this.selectedCmsMenu = null
        this.selectedCategory = null
        this.sequence = 1
        this.selectedEditData = null
        window.location.href = `/MyTools/ProductInformation`
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
            "productSPWACategoryId": this.selectedCategory.productSPWACategoryId,
            "isSequence": this.sequence,
            "cms_ContentId": this.selectedCmsContent.cms_ContentId,
            "cms_MenuId": this.selectedCmsMenu.cms_MenuId,
            approvedAt: e ?moment().format('YYYY-MM-DD')  : null
          }
          if(!this.selectedEditData) {
            await this.service.createSPWAMapping(body)
          } else {
            body.productSPWAMappingId = this.selectedEditData.productSPWAMappingId
            await this.service.updateSPWAMapping(body)
          }
          this.params.PageIndex = 1
          this.fetchFAQ()
          this.success = true
          this.selectedCmsContent = null
          this.selectedCmsMenu = null
          this.selectedCategory = null
          this.sequence = 1
          this.selectedEditData = null
        } catch (err) {
          this.errored.push({
            key: 'duplicate',
            value: 'Failed to insert data. Duplicate data!'
          })
        }
        this.isBusy = false
      },
      handleValidation () {
        this.success = false
        this.errored = []
        if(!this.sequence) {
          this.errored.push({
            key: 'sequence',
            value: 'Please fill sequence field'
          })
        }
        if(!this.selectedCategory) {
          this.errored.push({
            key: 'category',
            value: 'Please fill category field'
          })
        }
        if(!this.selectedCmsMenu) {
          this.errored.push({
            key: 'menu',
            value: 'Please fill menu field'
          })
        }
        if(!this.selectedCmsContent) {
          this.errored.push({
            key: 'content',
            value: 'Please fill content field'
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
          await this.service.deleteSPWAMapping(this.selectedData.productSPWAMappingId)
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
        this.sequence = data.isSequence
        this.selectedCategory = {
          productSPWACategoryId: data.productSPWACategoryId,
          productSPWACategoryName: data.productSPWACategoryName
        }
        this.selectedCmsMenu = {
          cms_MenuId: data.cms_MenuId,
          cms_MenuName: data.cms_MenuName,
          cms_MenuCategory: data.cms_MenuCategory
        }
        this.selectedCmsContent = {
          cms_ContentId: data.cms_ContentId,
          cms_ContentName: data.cms_ContentName
        }
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateSPWAMapping({
          productSPWAMappingId: data.productSPWAMappingId,
          productId: this.id,
          "productSPWACategoryId": data.productSPWACategoryId,
          "isSequence": data.isSequence,
          "cms_ContentId": data.cms_ContentId,
          "cms_MenuId": data.cms_MenuId,
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
    watch: {
      async selectedCmsMenu() {
        if(this.selectedCmsMenu != null) {
          this.isBusy = true
          await this.fetchCmsContent()
          this.isBusy = false
        }
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