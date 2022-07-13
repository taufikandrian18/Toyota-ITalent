<template>
  <div class="row px-4 py-4 w-100">
    <loading-overlay :loading="isBusy || isBusyData"></loading-overlay>
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
    <!-- <div class="col-md-12 mb-3">
      <h5>SPWA & Test Drive Guidance</h5>
    </div> -->
    <div class="col-md-12" v-if="isAdd">
      <div class="row">
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <div class="input-group talent_front">
                <label for="" class="w-100">Choose Menu</label>
                <div class="d-flex align-items-center w-100">
                  <multiselect
                    class="w-100 mr-2"
                    name="MenuKPOM"
                    :options="kodawariMenus"
                    label="keyPieceOfMindMenuName"
                    track-by="keyPieceOfMindMenuId"
                    v-model="selectedKodawariMenu">
                  </multiselect>
                  <button class="btn_add_rounded" @click="handleAddSubKodawariMenu">
                    +
                  </button>
                </div>
              </div>
            </div>
            <div class="col-md-12 mt-4">
              <div class="input-group talent_front">
                <label for="" class="w-100">Choose Category</label>
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
                <label for="" class="w-100">Choose Type</label>
                <div class="d-flex align-items-center w-100">
                  <multiselect
                    class="w-100 mr-2"
                    name="Category"
                    :options="categories"
                    label="keyPieceOfMindTypeName"
                    track-by="keyPieceOfMindTypeId"
                    v-model="selectedCategory">
                  </multiselect>
                  <button class="btn_add_rounded" @click="handleAddKodawariType">
                    +
                  </button>
                </div>
              </div>
            </div>
            <div class="col-md-12 mt-4">
              <div class="input-group talent_front">
                <label for="" class="w-100">Choose Content</label>
                <div class="d-flex align-items-center w-100">
                  <multiselect
                    class="w-100 mr-2"
                    name="Content"
                    :options="cmsContents"
                    label="cms_ContentName"
                    track-by="cms_ContentId"
                    v-model="selectedCmsContent">
                  </multiselect>
                  <button class="btn_add_rounded" @click="handleAddSubContent">
                    +
                  </button>
                </div>
              </div>
            </div>
            <div class="col-md-12 mt-4">
              <label>Sequence</label>
              <div class="input-group">
                  <input name="sequence" type="number" class="form-control" v-model="sequence"/>
              </div>
            </div>
            <!-- <div class="col-md-12 mt-4">
              <div class="input-group talent_front">
                <label for="" class="w-100">Choose Sub Content</label>
                <div class="d-flex align-items-center w-100">
                  <multiselect
                    class="w-100 mr-2"
                    name="Content"
                    :options="cmsSubContents"
                    label="cms_SubContentName"
                    track-by="cms_SubContentId"
                    v-model="selectedSubCmsContent">
                  </multiselect>
                  <button class="btn_add_rounded" @click="handleAddSubSubContent">
                    +
                  </button>
                </div>
              </div>
            </div> -->
          </div>
        </div>
        <div class="col-md-6">
          <div class="row">
          </div>
        </div>
        <div class="col-md-12 d-flex justify-content-end mt-4">
          <button class="btn btn-secondary mr-2" @click="reset">
              Back
          </button>
          <button class="btn btn-success mr-2" @click="submit(null)">
              {{!selectedEditData? 'Save' : 'Save' }}
          </button>
          <button class="btn btn-primary" @click="submit(1)">
              Submit
          </button>
        </div>
      </div>
    </div>


    <!-- table data -->
    <div class="col-md-12" v-else>
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
                      <input class="form-control" placeholder="Search By Category" v-model="params.KeyPieceOfMindMenuName" @keyup.enter="handleFilter"/>
                  </div>
              </div>
              <div class="col-8 row d-flex justify-content-end">
                <button class="btn btn-success" @click="() => {isAdd = true; success=false}">
                    Add New
                </button>
              </div>
          </div>
      </div>
      <!-- table  -->
      <div class="w-100 talent_overflowx mt-3">
          <table class="table table-bordered table-responsive-md talent_table_padding">
              <thead>
                  <tr>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Main Menu
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Category
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Type
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Content
                          </a>
                      </th>
                      <!-- <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Sub Content
                          </a>
                      </th> -->
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
                          {{dictionary.keyPieceOfMindMenuName}}
                      </td>
                      <td>
                          {{dictionary.cms_MenuName}}
                      </td>
                      <td>
                          {{dictionary.keyPieceOfMindTypeName}}
                      </td>
                      <td>
                          {{dictionary.cms_ContentName}}
                      </td>
                      <!-- <td>
                          {{dictionary.cms_SubContentName}}
                      </td> -->
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
      type="KPOM"/>

    <key-peace-of-mind-menu
      name="modalKodawariMenu"
      type="KPOM"/>

    <modal-key-peace-of-mind-type
      name="modalKodawariType"/>

    <modal-cms-content-full-kpom
      name="modalCmsContentFull"
      type="KPOM"/>


    <!-- <modal-cms-sub-content
      name="modalCmsSubContent"
      type="KPOM"/> -->
  </div>
</template>


<script>
import { ProductInformationService } from '../../../services/ProductInformation/ProductInformationService';
import $ from 'jquery'
import ModalDelete from '../../shared/ModalDelete.vue';
import moment from 'moment'
import ModalCmsMenu from '../../ProductInformation/ModalCmsMenu.vue';
import ModalSpwaCategory from '../../ProductInformation/ModalSpwaCategory.vue';
import ModalCmsSubContent from '../../ProductInformation/ModalCmsSubContent.vue';
import ModalKodawariMenu from '../../ProductInformation/ModalKodawariMenu.vue';
import ModalKodawariType from '../../ProductInformation/ModalKodawariType.vue';
import ModalKeyPeaceOfMindType from '../../ProductInformation/ModalKeyPeaceOfMindType.vue';
import KeyPeaceOfMindMenu from '../../ProductInformation/KeyPeaceOfMindMenu.vue';
import ModalCmsContentFullKpom from '../../ProductInformation/ModalCmsContentFullKpom.vue';

export default {
  components: { ModalDelete, ModalCmsMenu, ModalSpwaCategory, ModalCmsSubContent, ModalKodawariMenu, ModalKodawariType, ModalKeyPeaceOfMindType, KeyPeaceOfMindMenu, ModalCmsContentFullKpom },
    async mounted() {
    this.isBusy = true
      try {
        await this.fetchCategories()
        await this.fetchCmsMenu()
        await this.fetchKodawariMenu()
        await this.fetchCmsContent()
        await this.fetchFAQ()
        // await this.fetchCmsSubContent()
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
      $('#modalSpwaCategory').on('hidden.bs.modal', function (e) {
        vm.fetchCategories()
      })
      // $('#modalCmsSubContent').on('hidden.bs.modal', function (e) {
      //   vm.fetchCmsSubContent()
      // })
      $('#modalKodawariMenu').on('hidden.bs.modal', function (e) {
        vm.fetchKodawariMenu()
      })
      $('#modalKodawariType').on('hidden.bs.modal', function (e) {
        vm.fetchCategories()
      })
    },
    data() {
      return {
        isAdd: false,
        moment: moment,
        isBusy: false,
        isBusyData: false,
        errored: [],
        selectedFile: null,
        productTypes: [],
        categories: [],
        totalData: 0,
        selectedCategory: null,
        service: new ProductInformationService(),
        params: {
          KeyPieceOfMindMenuName: '',
          SortBy: '',
          PageIndex: 1,
          PageSize: 100
        },
        paramsCategories: {
          Cms_MenuCategory: "KPOM",
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
        selectedCmsContent: null,
        cmsSubContents: [],
        selectedSubCmsContent: null,
        kodawariMenus: [],
        selectedKodawariMenu: null,
      }
    },
    methods: {
      handleAddSubMenu() {
        $('#modalCmsMenu').modal('show')
      },
      handleAddSubContent() {
        $('#modalCmsContentFull').modal('show')
      },
      handleAddSubSubContent() {
        $('#modalCmsSubContent').modal('show')
      },
      handleAddSubCategory() {
        $('#modalSpwaCategory').modal('show')
      },
      handleAddSubKodawariMenu() {
        $('#modalKodawariMenu').modal('show')
      },
      handleAddKodawariType() {
        $('#modalKodawariType').modal('show')
      },
      async fetchKodawariMenu() {
        try {
          const res = await this.service.getAllKPOMMenu({
            PageIndex: 1,
            PageSize: 100
          })
          this.kodawariMenus = res.keyPieceOfMindMenus
        } catch(err) {
          console.error(err)
        }
      },
      async fetchCmsMenu() {
        try {
          const res = await this.service.getAllCMSMenu({
            Cms_menuCategory: 'KPOM',
            PageIndex: 1,
            PageSize: 100
          })
          this.cmsMenus = res.cmsMenus
        } catch(err) {
          console.error(err)
        }
      },
      async fetchCmsContent() {
        try {
          const res = await this.service.getAllCMSContent({
            Cms_ContentCategory: 'KPOM',
            PageIndex: 1,
            PageSize: 100
          })
          this.cmsContents = res.cmsContents
        } catch(err) {
          console.error(err)
        }
      },
      async fetchCmsSubContent() {
        try {
          const res = await this.service.getAllCMSSubContent({
            Cms_SubContentCategory: 'KPOM',
            PageIndex: 1,
            PageSize: 100
          })
          this.cmsSubContents = res.cmsSubContents
        } catch(err) {
          console.error(err)
        }
      },
      async fetchCategories() {
        try {
          const res = await this.service.getAllKeyPeaceOfMindType(this.paramsCategories)
          this.categories = res.keyPieceOfMindTypes
        } catch(err) {
          console.error(err)
        }
      },
      async fetchFAQ() {
        this.isBusyData = true;
        try {
          const res = await this.service.getAllKeyPeaceOfMind(this.params)
          this.productTypes = res.keyPieceOfMinds
          this.totalData = res.totalKeyPieceOfMinds
        } catch(err) {
          console.error(err)
        }
        this.isBusyData = false;
      },
      reset() {
        this.isAdd = false
        this.success = false
        this.selectedCmsContent = null
        this.selectedCmsMenu = null
        this.selectedCategory = null
        this.sequence = 1
        this.selectedSubCmsContent = null
        this.selectedKodawariMenu = null
        this.selectedEditData = null
      },
      async submit(e) {
        if(this.handleValidation()) {
          return
        }
        this.isBusy = true
        this.success = false
        try {
          const body = {
            "keyPieceOfMindTypeId": this.selectedCategory.keyPieceOfMindTypeId,
            "keyPieceOfMindMenuId": this.selectedKodawariMenu.keyPieceOfMindMenuId,
            "isSequence": this.sequence,
            "cms_ContentId": this.selectedCmsContent.cms_ContentId,
            "cms_MenuId": this.selectedCmsMenu.cms_MenuId,
            "cms_SubContentId": null,
            approvedAt: e ?moment().format('YYYY-MM-DD')  : null
          }
          if(!this.selectedEditData) {
            await this.service.createKeyPeaceOfMind(body)
          } else {
            body.keyPieceOfMindId = this.selectedEditData.keyPieceOfMindId
            await this.service.updateKeyPeaceOfMind(body)
          }
          this.params.PageIndex = 1
          this.fetchFAQ()
          this.success = true
          this.selectedCmsContent = null
          this.selectedCmsMenu = null
          this.selectedCategory = null
          this.selectedSubCmsContent = null
          this.selectedKodawariMenu = null
          this.sequence = 1
          this.selectedEditData = null
        } catch (err) {
          console.error(err)
        }
        this.isAdd = false
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
            key: 'type',
            value: 'Please fill type field'
          })
        }
        if(!this.selectedCmsMenu) {
          this.errored.push({
            key: 'category',
            value: 'Please fill category field'
          })
        }
        if(!this.selectedCmsContent) {
          this.errored.push({
            key: 'content',
            value: 'Please fill content field'
          })
        }
        if(!this.selectedKodawariMenu) {
          this.errored.push({
            key: 'content',
            value: 'Please fill menu field'
          })
        }
        // if(!this.selectedSubCmsContent) {
        //   this.errored.push({
        //     key: 'sub content',
        //     value: 'Please fill sub content field'
        //   })
        // }
        return this.errored.length > 0
      },
      handleFilter() {
        this.params.PageIndex = 1
        this.fetchFAQ()
      },
      async handleDelete() {
        this.isBusy = true
        try {
          await this.service.deleteKeyPeaceOfMind(this.selectedData.keyPieceOfMindId)
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
          this.selectedSubCmsContent = null
          this.selectedKodawariMenu = null
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(data) {
        this.selectedEditData = data
        this.sequence = data.isSequence
        this.selectedCategory = {
          keyPieceOfMindTypeId: data.keyPieceOfMindTypeId,
          keyPieceOfMindTypeName: data.keyPieceOfMindTypeName
        }
        this.selectedCmsMenu = {
          cms_MenuId: data.cms_MenuId,
          cms_MenuName: data.cms_MenuName
        }
        this.selectedCmsContent = {
          cms_ContentId: data.cms_ContentId,
          cms_ContentName: data.cms_ContentName
        }
        this.selectedKodawariMenu = {
          keyPieceOfMindMenuId: data.keyPieceOfMindMenuId,
          keyPieceOfMindMenuName: data.keyPieceOfMindMenuName
        }
        // this.selectedSubCmsContent = {
        //   cms_SubContentId: data.cms_SubContentId,
        //   cms_SubContentName: data.cms_SubContentName
        // }
        this.isAdd = true
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateKeyPeaceOfMind({
          keyPieceOfMindId: data.keyPieceOfMindId,
          "keyPieceOfMindMenuId": data.keyPieceOfMindMenuId,
          "keyPieceOfMindTypeId": data.keyPieceOfMindTypeId,
          "isSequence": data.isSequence,
          "cms_ContentId": data.cms_ContentId,
          "cms_MenuId": data.cms_MenuId,
          "cms_SubContentId": data.cms_SubContentId,
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