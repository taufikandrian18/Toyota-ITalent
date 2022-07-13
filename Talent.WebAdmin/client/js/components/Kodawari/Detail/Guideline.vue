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
    <div class="col-md-12 p-0" v-if="isAdd">
      <div class="row w-100">
        <!-- <div class="col-md-12 mb-3">
          <h5>Banner</h5>
        </div> -->
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <label>Title</label>
              <div class="input-group">
                  <input name="name" type="text" class="form-control" v-model="name" disabled/>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <label>Comment</label>
              <div class="input-group">
                  <textarea name="model_name" type="text" class="form-control" v-model="comment"/>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <label for="" class="w-100">File</label>
        </div>
        <div class="col-md-12" v-if="image.contentType !== 'pdf'">
          <div class="input-group talent_front">
            <div class="d-flex align-items-center w-100">
              <div class="container-upload">
                <img v-if="!['mp4', 'pdf'].includes(image.contentType)" class="h-100 w-100" :src="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                <video v-if="image.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFile ? selectedFile : `/upload-image2.png`"></video>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-12" v-if="image.contentType === 'pdf'">
          <object v-if="image.contentType === 'pdf'" class="h-100 w-100 min-heigth-full" type="application/pdf" width="100%" height="100%" :data="selectedFile ? selectedFile : `/upload-image2.png`" for="customFileFile" />
        </div>
        <div class="col-md-12 d-flex justify-content-end mt-4">
          <button class="btn btn-secondary mr-2" @click="reset" v-if="selectedEditData">
              Close
          </button>
          <!-- <button class="btn btn-success mr-2" @click="submit(null)">
              {{!selectedEditData ? 'Save' : 'update'}}
          </button> -->
          <button class="btn btn-success" @click="handleSave(null)">
              Save
          </button>
        </div>
        <div class="col-md-12 py-2">
          <hr>
        </div>
      </div>
    </div>
    <!-- table data -->
    <div class="col-md-12" v-if="!selectedEditData">
      <!-- filter -->
      <div class="col-md-12 talent_magin_small mt-3">
          <div class="row w-100 align-items-center justify-content-between">
              <div class="col d-flex py-0 pl-0 pr-4">
                  <div class="dropdown">
                    <button class="btn btn-info mr-2" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-expanded="false" @click="handleFilter">
                      Filter
                    </button>
                  </div>
                  <div class="input-group">
                      <input class="form-control" placeholder="Search By Title" v-model="params.HOGuidelineTitle" @keyup.enter="handleFilter"/>
                  </div>
              </div>
              <div class="col-8 row d-flex justify-content-end p-0">
                <!-- <button class="btn btn-success" @click="() => {isAdd = true; success=false}">
                    Add New
                </button> -->
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
                            Uploader Name
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Dealer Name
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Title
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Media
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Comment
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Created At
                          </a>
                      </th>
                      <!-- <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Published
                          </a>
                      </th> -->
                      <th scope="col" class="text-center" colspan="3">
                          <a href="#" class="talent_sort" style="color: white;">
                            Action
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Approve
                          </a>
                      </th>
                  </tr>
              </thead>
              <tbody>
                  <tr v-for="dictionary in productTypes" :key="dictionary.hoGuidelineId">
                      <td>
                          {{dictionary.createdBy}}
                      </td>
                      <td>
                          {{dictionary.outletName}}
                      </td>
                      <td>
                          {{dictionary.hoGuidelineTitle}}
                      </td>
                      <td class="text-center">
                          <img v-if="!['mp4', 'pdf'].includes(dictionary.blobFileType)" :src="dictionary.blobUrl" alt="" style="width: 100px">
                          <fa v-if="['mp4'].includes(dictionary.blobFileType)" icon="video" alt="" style="width: 64px;height:64px"></fa>
                          <fa v-if="['pdf'].includes(dictionary.blobFileType)" icon="file-alt" alt="" style="width: 64px;height:64px"></fa>
                      </td>
                      <td style="max-width: 150px; max-height: 100px">
                          {{dictionary.hoGuidelineComment}}
                      </td>
                      <td>
                          {{moment(dictionary.createdAt).format('DD/MM/YYYY')}}
                      </td>
                      <!-- <td>
                          <button
                          v-if="dictionary.isApproved"
                            :class="`btn btn-block ${dictionary.approvedAt ? 'talent_bg_green talent_font_white' : 'talent_bg_red talent_font_white'}`" 
                            @click="handleEditStatus(dictionary)">
                              <fa :icon="dictionary.approvedAt ? 'eye' : 'eye-slash'"></fa>
                          </button>
                      </td> -->
                      <td class="talent_nopadding_important">
                          <button class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(dictionary)">Edit</button>
                      </td>
                      <!-- <td class="talent_nopadding_important">
                          <button class="btn btn-block talent_bg_cyan talent_font_white" @click="handleComment(dictionary)">Comment</button>
                      </td> -->
                      <td class="talent_nopadding_important">
                        <button type="button" class="btn btn-block talent_bg_red talent_font_white" 
                                  data-toggle="modal"
                                  data-target="#modalDelete" @click="selectedData = dictionary">
                          Remove
                        </button>
                      </td>
                      <td class="talent_nopadding_important text-center">
                        <div class="d-flex align-items-center w-100">
                          <div class="custom-control custom-switch w-100">
                            <input type="checkbox" class="custom-control-input" :id="`customSwitch-${dictionary.hoGuidelineId}`" :checked="dictionary.isApproved" @change="handleChangeStatus(dictionary)">
                            <label class="custom-control-label" :for="`customSwitch-${dictionary.hoGuidelineId}`"></label>
                          </div>
                        </div>
                      </td>
                  </tr>
              </tbody>
          </table>
      </div>

      <!--Pagination-->
      <div class="col-md-12 d-flex justify-content-center">
          <paginate :currentPage.sync="params.PageIndex" :total-data="totalData" :limit-data="10" @update:currentPage="fetchProductTypes()"></paginate>
      </div>
    </div>


    <modal-delete
      name="modalDelete"
      @delete="handleDelete"
    />

    <modal-kodawari-menu
      name="modalKodawariMenu"
      type="Kodawari"/>

    <modal-comment
      name="modalComment"
      :data="selectedCommentData"/>
  </div>
</template>


<script>
import { ProductInformationService } from '../../../services/ProductInformation/ProductInformationService';
import $ from 'jquery'
import ModalDelete from '../../shared/ModalDelete.vue';
import moment from 'moment'
import ModalKodawariMenu from '../../ProductInformation/ModalKodawariMenu.vue';
import ModalComment from '../../ProductInformation/ModalComment.vue';

export default {
  components: { ModalDelete, ModalKodawariMenu, ModalComment },
    async mounted() {
      this.isBusy = true
      try {
        await this.fetchProductTypes()
        await this.fetchKodawariMenu()
      } catch(err) {
        console.error(err)
      }
      this.isBusy = false
      const vm = this

      $('#modalKodawariMenu').on('hidden.bs.modal', function (e) {
        vm.fetchKodawariMenu()
      })
      $('#modalComment').on('hidden.bs.modal', function (e) {
        vm.selectedCommentData = {}
        vm.fetchProductTypes()
      })
    },
    data() {
      return {
        moment: moment,
        isBusy: false,
        errored: [],
        selectedFile: null,
        productTypes: [],
        totalData: 0,
        service: new ProductInformationService(),
        params: {
          HOGuidelineTitle: "",
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
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
        isAdd: false,
        isDownloadable: false,
        categories: [],
        selectedCategory: null,
        selectedCommentData: {},
        comment: ''      }
    },
    methods: {
      handleAddSubKodawariMenu() {
        $('#modalKodawariMenu').modal('show')
      },
      reset() {
        this.success = false
        this.name = ''
        this.salesTalk = ''
        this.selectedFile = null
        this.image = {
          base64: '',
          fileName: '',
          contentType: ''
        }
        this.comment = ''
        this.selectedEditData = null
        this.isAdd = false
        this.selectedCategory = null
      },
      async fetchKodawariMenu() {
        try {
          const res = await this.service.getAllKodawariMenu({
            PageIndex: 1,
            PageSize: 100
          })
          this.categories = res.kodawariMenus
        } catch(err) {
          console.error(err)
        }
      },
      async fetchProductTypes() {
        this.isBusy = true
        try {
          const res = await this.service.getAllHOGuideline(this.params)
          this.productTypes = res.hoGuidelines
          this.totalData = res.totalHOGuidelines
        } catch(err) {
          console.error(err)
        }
        this.isBusy = false
      },
      async handleSave() {
        this.errored = []
        if(!this.comment) {
          this.errored.push({
            key: 'comment',
            value: 'Please fill comment field'
          })
        }
        if(this.errored.length > 0) {
          return
        }
        this.isBusy = true
        try {
          await this.service.updateCommentHOUploadGuideline({
            hoGuidelineId: this.selectedEditData.hoGuidelineId,
            "hoGuidelineComment": this.comment,
            approvedAt: this.selectedEditData.approvedAt,
          })
          this.params.PageIndex = 1
          this.success = true
          this.isAdd = false
          this.name = ''
          this.salesTalk = ''
          this.selectedCategory = null
          this.selectedFile = null
          this.image = {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.selectedEditData = null
          this.comment = ''
          this.fetchProductTypes()
        } catch (err) {

        }
        this.isBusy = false
      },
      // async submit(e) {
      //   if(this.handleValidation()) {
      //     return
      //   }
      //   this.isBusy = true
      //   this.success = false
      //   try {
      //     const body = {
      //       kodawariDownloadFileContent: {
      //         base64: this.image.base64,
      //         fileName: this.image.fileName,
      //         contentType: this.image.contentType
      //       },
      //       approvedAt: e ? moment().format('YYYY-MM-DD') : null,
      //       "kodawariMenuId": this.selectedCategory.kodawariMenuId,
      //       "hoGuidelineTitle": this.name,
      //       "isDownloadable": this.isDownloadable,
      //     }
      //     if(!this.selectedEditData) {
      //       await this.service.createKodawariDownload(body)
      //     } else {
      //       body.hoGuidelineId = this.selectedEditData.hoGuidelineId
      //       await this.service.updateKodawariDownload(body)
      //     }
      //     this.params.PageIndex = 1
      //     this.fetchProductTypes()
      //     this.success = true
      //     this.isAdd = false
      //     this.name = ''
      //     this.salesTalk = ''
      //     this.selectedCategory = null
      //     this.selectedFile = null
      //     this.image = {
      //       base64: '',
      //       fileName: '',
      //       contentType: ''
      //     }
      //     this.selectedEditData = null
      //   } catch (err) {
      //     console.error(err)
      //   }
      //   this.isBusy = false
      // },
      handleValidation () {
        this.success= false
        this.errored = []
        if(!this.name) {
          this.errored.push({
            key: 'title',
            value: 'Please fill title field'
          })
        }
        if(!this.selectedFile) {
          this.errored.push({
            key: 'media',
            value: 'Please fill media field'
          })
        }
        if(!this.selectedCategory) {
          this.errored.push({
            key: 'menu',
            value: 'Please fill menu field'
          })
        }
        return this.errored.length > 0
      },
      handleFilter() {
        this.params.PageIndex = 1
        this.fetchProductTypes()
      },
      async handleDelete() {
        this.isBusy = true
        try {
          await this.service.deleteHOUploadGuideline(this.selectedData.hoGuidelineId)
          this.fetchProductTypes()
          this.success = true
          this.name = ''
          this.salesTalk = ''
          this.selectedFile = null
          this.selectedCategory = null
          this.image = {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.selectedEditData = null
          this.comment = ''
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(data) {
        this.success = false
        this.selectedEditData = data
        this.name = data.hoGuidelineTitle
        this.selectedFile = data.blobUrl
        this.isAdd = true
        this.image = {
          base64: '',
          fileName: '',
          contentType: data.blobFileType
        }
        this.comment = data.hoGuidelineComment
      },
      handleComment(data) {
        this.selectedCommentData = data

        $('#modalComment').modal('show')
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateApprovalHOUploadGuideline({
            hoGuidelineId: data.hoGuidelineId,
            "isApproved": data.isApproved,
            approvedAt: data.approvedAt ? null : moment().format('YYYY-MM-DD')
        })
        this.fetchProductTypes()
        this.isBusy = false
      },
      async handleChangeStatus(data) {
        this.isBusy = true
        await this.service.updateApprovalHOUploadGuideline({
            hoGuidelineId: data.hoGuidelineId,
            "isApproved": !data.isApproved,
            approvedAt: data.approvedAt
        })
        this.fetchProductTypes()
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