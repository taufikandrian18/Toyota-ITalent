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
            <div class="col-md-12 mt-4">
              <label>Title</label>
              <div class="input-group">
                  <input name="name" type="text" class="form-control" v-model="name"/>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <div class="input-group talent_front">
                <label for="" class="w-100">Thumbnail</label>
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
        <div class="col-md-12 d-flex justify-content-end mt-4">
          <button class="btn btn-secondary mr-2" @click="reset" v-if="selectedEditData">
              Cancel
          </button>
          <button class="btn btn-success mr-2" @click="submit(null)">
              {{!selectedEditData ? 'Save' : 'update'}}
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
                      <input class="form-control" placeholder="Search By Title" v-model="params.KodawariBannerTitle" @keyup.enter="handleFilter"/>
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
                            Title
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Media
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
                  <tr v-for="dictionary in productTypes" :key="dictionary.kodawariBannerId">
                      <td>
                          {{dictionary.kodawariBannerTitle}}
                      </td>
                      <td class="text-center">
                          <img :src="dictionary.imageUrl" alt="" width="150px">
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
          KodawariBannerTitle: "",
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
        isAdd: false
      }
    },
    methods: {
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
      },
      async fetchProductTypes() {
        this.isBusy = true
        try {
          const res = await this.service.getAllKodawariBanner(this.params)
          this.productTypes = res.kodawariBanners
          this.totalData = res.totalKodawariBanners
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
            kodawariBannerTitle: this.name,
            kodawariBannerFileContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            },
            approvedAt: e ? moment().format('YYYY-MM-DD') : null
          }
          if(!this.selectedEditData) {
            await this.service.createKodawariBanner(body)
          } else {
            if(!this.image.base64) {
              delete body.kodawariBannerFileContent
            }
            body.kodawariBannerId = this.selectedEditData.kodawariBannerId
            await this.service.updateKodawariBanner(body)
          }
          this.params.PageIndex = 1
          this.fetchProductTypes()
          this.success = true
          this.isAdd = false
          this.name = ''
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
          await this.service.deleteKodawariBanner(this.selectedData.kodawariBannerId)
          this.fetchProductTypes()
          this.success = true
          this.name = ''
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
        this.name = data.kodawariBannerTitle
        this.salesTalk = data.productTypeSalesTalk
        this.selectedFile = data.imageUrl
        this.isAdd = true
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateKodawariBanner({
            kodawariBannerId: data.kodawariBannerId,
            kodawariBannerTitle: data.kodawariBannerTitle,
            productTypeSalesTalk: data.productTypeSalesTalk,
            approvedAt: data.approvedAt ? null : moment().format('YYYY-MM-DD')
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