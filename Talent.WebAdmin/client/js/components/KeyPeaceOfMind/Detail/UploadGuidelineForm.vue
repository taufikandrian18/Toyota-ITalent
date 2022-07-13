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
    <div class="col-md-12 p-0">
      <div class="row w-100">
        <div class="col-md-12 mb-3">
          <h5>Upload Guideline</h5>
        </div>
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12 mt-4">
              <label>Title</label>
              <div class="input-group">
                  <input name="name" type="text" class="form-control" v-model="name"/>
              </div>
            </div>
            <div class="col-md-12 mt-4">
              <label>Comment</label>
              <div class="input-group">
                <textarea name="model_name" type="text" class="form-control" v-model="comment"/>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <div class="input-group talent_front">
                <label for="" class="w-100">Upload File (pdf/jpg/mp4) maks 100MB</label>
                <div class="d-flex align-items-center w-100">
                  <div class="container-upload">
                    <img v-if="!['mp4', 'pdf'].includes(image.contentType)" class="h-100 w-100" :src="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                    <video v-if="image.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFile ? selectedFile : `/upload-image2.png`"></video>
                    <!-- <div class="btn-upload"> -->
                      <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                        <label for="input-manfaat" style="margin: 0px">Browse</label>
                      </button>
                    <!-- </div> -->
                      <input type="file" name="input-manfaat" id="input-manfaat" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg,.pdf,.mp4">
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <object v-if="image.contentType === 'pdf'" class="h-100 w-100 min-heigth-full" type="application/pdf" width="100%" height="100%" :data="selectedFile ? selectedFile : `/upload-image2.png`" for="customFileFile" style="height: 500px"/>
        </div>
        <div class="col-md-12 d-flex justify-content-end mt-4">
          <button class="btn btn-secondary mr-2" @click="reset" v-if="selectedEditData">
              Cancel
          </button>
          <button class="btn btn-success mr-2" @click="submit(null)">
              Save
          </button>
          <!-- <button class="btn btn-primary" @click="submit(1)">
              Submit
          </button> -->
        </div>
        <div class="col-md-12 py-2">
          <hr>
        </div>
      </div>
    </div>
    <!-- table data -->
    <div class="col-md-12">
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
                      <input class="form-control" placeholder="Search By Title" v-model="params.hoGuidelineTitle" @keyup.enter="handleFilter"/>
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
                            Title
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Media
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="width: 150px;">
                          <a href="#" class="talent_sort" style="color: white;">
                            Comment
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Created At
                          </a>
                      </th>
                      <!-- <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Published
                          </a>
                      </th> -->
                      <th scope="col" class="text-center" colspan="1">
                          <a href="#" class="talent_sort" style="color: white;">
                            Action
                          </a>
                      </th>
                      <th scope="col" class="text-center" colspan="1">
                          <a href="#" class="talent_sort" style="color: white;">
                            Status
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
                          {{dictionary.hoGuidelineTitle}}
                      </td>
                      <td class="text-center">
                          <img v-if="!['mp4', 'pdf'].includes(dictionary.blobFileType)" :src="dictionary.blobUrl" alt="" style="width: 100px">
                          <fa v-if="['mp4'].includes(dictionary.blobFileType)" icon="video" alt="" style="width: 64px;height:64px"></fa>
                          <fa v-if="['pdf'].includes(dictionary.blobFileType)" icon="file-alt" alt="" style="width: 64px;height:64px"></fa>
                      </td>
                      <td style="width: 150px;max-width: 150px; height: 100px; max-height: 100px">
                          {{dictionary.hoGuidelineComment}}
                      </td>
                      <td style="max-width: 150px;white-space: normal">
                          {{moment(dictionary.createdAt).format('DD/MM/YYYY')}}
                      </td>
                        <!-- <td>
                            <button
                              :class="`btn btn-block ${dictionary.approvedAt ? 'talent_bg_green talent_font_white' : 'talent_bg_red talent_font_white'}`" 
                              @click="handleEditStatus(dictionary)">
                                <fa :icon="dictionary.approvedAt ? 'eye' : 'eye-slash'"></fa>
                            </button>
                        </td> -->
                      <td class="talent_nopadding_important">
                          <button v-if="dictionary.hoGuidelineStatus != 'Approved'" class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(dictionary)">Edit</button>

                        <button type="button" class="btn btn-block talent_bg_red talent_font_white" 
                                  data-toggle="modal"
                                  data-target="#modalDelete" @click="selectedData = dictionary">
                          Remove
                        </button>
                      </td>
                      <td>
                        <div :class="getClass(dictionary.hoGuidelineStatus)">
                          {{dictionary.hoGuidelineStatus}}
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
        await this.fetchProductTypes()
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
        totalData: 0,
        service: new ProductInformationService(),
        params: {
          hoGuidelineTitle: "",
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
        },
        name: '',
        comment: '',
        salesTalk: '',
        image: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        success: false,
        selectedData: null,
        selectedEditData: null,
        isAdd: false
      }
    },
    methods: {
      getClass(status) {
        switch (status) {
          case 'Approved':
            return 'approved'
          case 'Awaiting Approval':
            return 'awaiting-approval'
          case 'Rejected':
            return 'rejected'
            default: 
            return 'awaiting-approval'
        }
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
        this.selectedEditData = null,
        this.isAdd = false
      },
      async fetchProductTypes() {
        this.isBusy = true
        try {
          const res = await this.service.getAllHOUploadGuideline(this.params)
          this.productTypes = res.hoGuidelineUploads
          this.totalData = res.totalHOGuidelineUploads
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
            hoGuidelineTitle: this.name,
            hoGuidelineFileContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            },
            approvedAt: e ? moment().format('YYYY-MM-DD') : null
          }
          if(!this.selectedEditData) {
            await this.service.createHOUploadGuideline(body)
          } else {
            if(!this.image.base64) {
              delete body.hoGuidelineFileContent
            }
            body.hoGuidelineId = this.selectedEditData.hoGuidelineId
            await this.service.updateHOUploadGuideline(body)

            await this.service.updateCommentHOUploadGuideline({
              hoGuidelineId: this.selectedEditData.hoGuidelineId,
              "hoGuidelineComment": this.comment,
              approvedAt: this.selectedEditData.approvedAt,
            })
          }
          this.params.PageIndex = 1
          this.fetchProductTypes()
          this.success = true
          this.isAdd = false
          this.name = ''
          this.comment = ''
          this.salesTalk = ''
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
          this.comment = ''
          this.salesTalk = ''
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
        this.success = false
        this.selectedEditData = data
        this.name = data.hoGuidelineTitle
        this.comment = data.hoGuidelineComment
        this.salesTalk = data.productTypeSalesTalk
        this.image.contentType = data.blobFileType
        this.selectedFile = data.blobUrl
        this.isAdd = true
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateHOUploadGuideline({
            hoGuidelineId: data.hoGuidelineId,
            hoGuidelineTitle: data.hoGuidelineTitle,
            hoGuidelineFileContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            }
        })
        this.fetchProductTypes()
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
  min-height: 100px;
  border: 1px solid #000000;
  background-color: rgb(146, 145, 145);
  position: relative;
  display: flex;
}
.btn-upload {
  position: absolute;
  top: 0;
  display: flex;
  justify-content: end;
  width: 100%;
  padding: 16px 16px;
}
.approved {
  background: #60D408 0% 0% no-repeat padding-box;
  border-radius: 27px;
  color: white;
  padding: 6px 24px;
  text-align: center;
}
.awaiting-approval {
  background: #EEA944 0% 0% no-repeat padding-box;
  border-radius: 27px;
  color: white;
  padding: 6px 24px;
  text-align: center;
}
.rejected {
  background: #EC182E 0% 0% no-repeat padding-box;
  border-radius: 27px;
  color: white;
  padding: 6px 24px;
  text-align: center;
}
</style>