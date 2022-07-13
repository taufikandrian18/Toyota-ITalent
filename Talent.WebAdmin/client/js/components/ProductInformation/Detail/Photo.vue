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
      <h5>Photo Product Gallery</h5>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <div class="input-group talent_front">
            <label for="" class="w-100">Media<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100 h-full">
              <div class="container-upload  h-full">
                <img v-if="!['mp4', 'pdf'].includes(image.contentType)" class="h-100 w-100" :src="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                <video v-if="image.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFile ? selectedFile : `/upload-image2.png`"></video>
                <!-- <div class="btn-upload"> -->
                  <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                    <label for="input-manfaat" style="margin: 0px">Browse</label>
                  </button>
                <!-- </div> -->
                  <input type="file" name="input-manfaat" id="input-manfaat" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg,.gif,.mp4,.pdf">
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <div class="col-md-6">
        <div class="col-md-12">
          <checkbox class="talent-checkbox-lineheight"
                        id="is_special"
                        v-model="is360Photo">Is 360</checkbox>
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
                            Media
                          </a>
                      </th>
                      <th scope="col" class="text-center">
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
                  <tr v-for="dictionary in productTypes" :key="dictionary.productTypeId">
                      <td class="text-center">
                          <img v-if="!['mp4', 'pdf'].includes(dictionary.blob.mime)" :src="dictionary.imageUrl" alt="" style="width: 100px">
                          <fa v-if="['mp4'].includes(dictionary.blob.mime)" icon="video" alt="" style="width: 64px;height:64px"></fa>
                          <fa v-if="['pdf'].includes(dictionary.blob.mime)" icon="file-alt" alt="" style="width: 64px;height:64px"></fa>
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
        image: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        success: false,
        selectedData: null,
        selectedEditData: null,
        is360Photo: true
      }
    },
    methods: {
      reset() {
        this.success = false
        this.selectedFile = null
        this.image = {
          base64: '',
          fileName: '',
          contentType: ''
        }
        this.selectedEditData = null
        this.is360Photo = true
        window.location.href = `/MyTools/ProductInformation`
      },
      async fetchFAQ() {
        try {
          this.params.ProductId = this.id
          const res = await this.service.getAllProductPhotos(this.params)
          this.productTypes = res.productPhotos
          this.totalData = res.totalProductPhotos
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
            productFileContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            },
            approvedAt: e ?moment().format('YYYY-MM-DD')  : null,
            is360Photo: this.is360Photo
          }
          if(!this.selectedEditData) {
            await this.service.createProductPhoto(body)
          } else {
            if(!this.image.base64) {
              delete body.productFileContent
            }
            body.productPhotoId = this.selectedEditData.productPhotoId
            await this.service.updateProductPhoto(body)
          }
          this.params.PageIndex = 1
          this.fetchFAQ()
          this.success = true
          this.selectedFile = null
          this.image = {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.selectedEditData = null
          this.is360Photo = true
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleValidation () {
        this.errored = []
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
          await this.service.deleteProductPhoto(this.selectedData.productPhotoId)
          this.fetchFAQ()
          this.success = true
          this.selectedCategory = null
          this.selectedFile = null
          this.image = {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.selectedEditData = null
          this.is360Photo = true
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(data) {
        this.selectedEditData = data
        this.selectedFile = data.imageUrl
        this.image.contentType = data.blob.mime
        this.is360Photo = data.is360Photos
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateProductPhoto({
          productPhotoId: data.productPhotoId,
          productId: this.id,
          is360Photo: data.is360Photos,
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