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
        <div class="col-md-12 mb-3">
          <h5>Banner</h5>
        </div>
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <div class="input-group talent_front">
                <label for="" class="w-100">Choose Menu</label>
                <div class="d-flex align-items-center w-100">
                  <multiselect
                    class="w-100 mr-2"
                    name="Category"
                    :options="categories"
                    label="kodawariMenuName"
                    track-by="kodawariMenuId"
                    v-model="selectedCategory">
                  </multiselect>
                  <button class="btn_add_rounded" @click="handleAddSubKodawariMenu">
                    +
                  </button>
                </div>
              </div>
            </div>
            <div class="col-md-12 mt-4">
              <label for="" class="w-100">Is Downloadable</label>
              <div class="custom-control custom-switch w-100">
                <input type="checkbox" class="custom-control-input" :id="`customSwitch-1`" v-model="isDownloadable">
                <label class="custom-control-label" :for="`customSwitch-1`"></label>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <label>Title</label>
              <div class="input-group">
                  <input name="name" type="text" class="form-control" v-model="name"/>
              </div>
            </div>
            <div class="col-md-12 mt-4">
              <div class="input-group talent_front">
                <label for="" class="w-100">Upload File (jpeg, png, mp4, pdf)</label>
                <div class="d-flex align-items-center w-100">
                  <div class="container-upload">
                    <img v-if="!['mp4', 'pdf'].includes(image.contentType)" class="h-100 w-100" :src="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                    <video v-if="image.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFile ? selectedFile : `/upload-image2.png`"></video>
                    <!-- <div class="btn-upload"> -->
                      <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                        <label for="input-manfaat" style="margin: 0px">Browse</label>
                      </button>
                    <!-- </div> -->
                      <input type="file" name="input-manfaat" id="input-manfaat" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg.,.pdf,.mp4">
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <object v-if="image.contentType === 'pdf'" class="h-100 w-100 min-heigth-full" type="application/pdf" width="100%" height="100%" :data="selectedFile ? selectedFile : `/upload-image2.png`" for="customFileFile" />
        </div>
        <div class="col-md-12 d-flex justify-content-end mt-4">
          <button class="btn btn-secondary mr-2" @click="reset" v-if="selectedEditData">
              Cancel
          </button>
          <button class="btn btn-success mr-2" @click="submit(null)">
              Save
          </button>
          <button class="btn btn-primary" @click="submit(1)">
              Submit
          </button>
        </div>
        <div class="col-md-12 py-2">
          <hr>
        </div>
      </div>
    </div>
    <!-- table data -->
    <div class="col-md-12" v-else>
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
                      <input class="form-control" placeholder="Search By Title" v-model="params.KodawariDownloadTitle" @keyup.enter="handleFilter"/>
                  </div>
              </div>
              <div class="col-8 row d-flex justify-content-end p-0">
                <button class="btn btn-success" @click="() => {isAdd = true; success=false}">
                    Add New
                </button>
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
                            File
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Is Downloadable
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Title
                          </a>
                      </th>
                      <!-- <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Published
                          </a>
                      </th> -->
                      <th scope="col" class="text-center" colspan="2">
                          <a href="#" class="talent_sort" style="color: white;">
                            Action
                          </a>
                      </th>
                  </tr>
              </thead>
              <tbody>
                  <tr v-for="dictionary in productTypes" :key="dictionary.kodawariDownloadId">
                      <td>
                          {{dictionary.kodawariMenuName}}
                      </td>
                      <td class="text-center">
                          <img v-if="!['mp4', 'pdf'].includes(dictionary.fileTypeBlob)" :src="dictionary.imageUrl" alt="" style="width: 100px">
                          <fa v-if="['mp4'].includes(dictionary.fileTypeBlob)" icon="video" alt="" style="width: 64px;height:64px"></fa>
                          <fa v-if="['pdf'].includes(dictionary.fileTypeBlob)" icon="file-alt" alt="" style="width: 64px;height:64px"></fa>
                      </td>
                        <td class="talent_nopadding_important text-center">
                          <div class="d-flex align-items-center w-100">
                            <div class="custom-control custom-switch w-100">
                              <input type="checkbox" class="custom-control-input" :id="`customSwitch-${dictionary.kodawariDownloadId}`" :checked="dictionary.isDownloadable" @change="handleChangeStatus(dictionary)">
                              <label class="custom-control-label" :for="`customSwitch-${dictionary.kodawariDownloadId}`"></label>
                            </div>
                          </div>
                        </td>
                      <td>
                          {{dictionary.kodawariDownloadTitle}}
                      </td>
                      <!-- <td>
                          <button
                            :class="`btn btn-block ${dictionary.approvedAt ? 'talent_bg_green talent_font_white' : 'talent_bg_red talent_font_white'}`" 
                            @click="handleEditStatus(dictionary)">
                              <fa :icon="dictionary.approvedAt ? 'eye' : 'eye-slash'"></fa>
                          </button>
                      </td> -->
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
  </div>
</template>


<script>
import { ProductInformationService } from '../../../services/ProductInformation/ProductInformationService';
import $ from 'jquery'
import ModalDelete from '../../shared/ModalDelete.vue';
import moment from 'moment'
import ModalKodawariMenu from '../../ProductInformation/ModalKodawariMenu.vue';

export default {
  components: { ModalDelete, ModalKodawariMenu },
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
          KodawariDownloadTitle: "",
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
        selectedCategory: null
      }
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
        this.selectedEditData = null,
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
          const res = await this.service.getAllKodawariDownload(this.params)
          this.productTypes = res.kodawariDownloads
          this.totalData = res.totalKodawariDownloads
        } catch(err) {
          console.error(err)
        }
        this.isBusy = false
      },
      async submit(e) {
        if(this.handleValidation()) {
          return
        }
        this.isBusy = true
        this.success = false
        try {
          const body = {
            kodawariDownloadFileContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            },
            approvedAt: e ? moment().format('YYYY-MM-DD') : null,
            "kodawariMenuId": this.selectedCategory.kodawariMenuId,
            "kodawariDownloadTitle": this.name,
            "isDownloadable": this.isDownloadable,
          }
          if(!this.selectedEditData) {
            await this.service.createKodawariDownload(body)
          } else {
            body.kodawariDownloadId = this.selectedEditData.kodawariDownloadId
            await this.service.updateKodawariDownload(body)
          }
          this.params.PageIndex = 1
          this.fetchProductTypes()
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
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
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
          await this.service.deleteKodawariDownload(this.selectedData.kodawariDownloadId)
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
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(data) {
        this.success = false
        this.selectedEditData = data
        this.name = data.kodawariDownloadTitle
        this.salesTalk = data.productTypeSalesTalk
        this.selectedFile = data.imageUrl
        this.isAdd = true
        this.isDownloadable = data.isDownloadable
        this.selectedCategory = {
          kodawariMenuId: data.kodawariMenuId,
          kodawariMenuName: data.kodawariMenuName
        }
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateKodawariDownload({
            kodawariDownloadId: data.kodawariDownloadId,
            kodawariDownloadFileContent: {
              base64: '',
              fileName: '',
              contentType: ''
            },
            "kodawariMenuId": data.kodawariMenuId,
            "kodawariDownloadTitle": data.kodawariDownloadTitle,
            "isDownloadable": data.isDownloadable,
            approvedAt: data.approvedAt ? null : moment().format('YYYY-MM-DD')
        })
        this.fetchProductTypes()
        this.isBusy = false
      },
      async handleChangeStatus(data) {
        this.isBusy = true
        await this.service.updateKodawariDownload({
            kodawariDownloadId: data.kodawariDownloadId,
            kodawariDownloadFileContent: {
              base64: '',
              fileName: '',
              contentType: ''
            },
            "kodawariMenuId": data.kodawariMenuId,
            "kodawariDownloadTitle": data.kodawariDownloadTitle,
            "isDownloadable": !data.isDownloadable,
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
  min-height: 200px;
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