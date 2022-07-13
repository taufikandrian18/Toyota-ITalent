<template>
    <div class="modal fade" :id="name" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 d-flex">
                        Add Guidance Type
                    </div>
                </div>
                <div class="row px-4 py-4">
                  <div class="col-md-12 p-0 mb-4">
                    <div class="alert alert-danger alert-dismissible fade show" role="alert" v-if="errored.length > 0">
                        <ul>
                          <li v-for="error in errored" :key="error.key">{{error.value}}</li>
                        </ul>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="row">
                      <div class="col-md-12">
                        <label>Guidance Type Name</label>
                        <div class="input-group">
                            <input name="model_name" type="text" class="form-control" v-model="categoryName"/>
                        </div>
                      </div>
                      <div class="col-md-12 mt-4">
                        <!-- <div class="input-group talent_front">
                          <label for="" class="w-100">Media</label>
                          <div class="d-flex align-items-center w-100">
                            <div class="container-upload">
                              <img v-if="!['mp4', 'pdf'].includes(image.contentType)" class="h-100 w-100" :src="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                              <video v-if="image.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFile ? selectedFile : `/upload-image2.png`"></video>
                              <object v-if="image.contentType === 'pdf'" class="h-100 w-100 min-heigth-full" type="application/pdf" width="100%" height="100%" :data="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                                <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                                  <label for="input-manfaat" style="margin: 0px">Browse</label>
                                </button>
                                <input type="file" name="input-manfaat" id="input-manfaat" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg,.pdf,.gif,.mp4">
                            </div>
                          </div>
                        </div> -->
                      </div>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="row">
                      <!-- <div class="col-md-12">
                        <div class="input-group talent_front">
                          <label for="" class="w-100">Description</label>
                          <div class="d-flex align-items-center w-100">
                            <wysiwyg 
                                class="w-100" 
                                v-model="description" 
                                :options="{
                                    hideModules: {image: true, table: true, removeFormat: true, code: true }
                                }"
                                @change="(e) => description = e"/>
                          </div>
                        </div>
                      </div> -->
                    </div>
                  </div>
                </div>

                <div class="col-md-12 d-flex justify-content-end mt-4">
                  <button class="btn btn-secondary mr-2" data-dismiss="modal">
                      Cancel
                  </button>
                  <button class="btn btn-primary" @click="handleSave">
                      {{ selectedEditData ? 'Save' : 'Save'}}
                  </button>
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
                                <input class="form-control" placeholder="Search By Name" v-model="params.KodawariTypeName"/>
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
                                        Name
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
                            <tr v-for="dictionary in categories" :key="dictionary.kodawariTypeId">
                                <td>
                                    {{dictionary.kodawariTypeName}}
                                </td>
                                <td class="talent_nopadding_important">
                                    <button class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(dictionary)">Edit</button>
                                </td>
                                <td class="talent_nopadding_important">
                                    <button type="button" class="btn btn-block talent_bg_red talent_font_white" 
                                            data-toggle="modal"
                                            data-target="#modalDeleteKodawariType" @click="selectedData = dictionary">
                                    Remove
                                    </button>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!--Pagination-->
                <div class="col-md-12 d-flex justify-content-center">
                    <paginate :currentPage.sync="params.PageIndex" :total-data="totalData" :limit-data="10" @update:currentPage="fetchCategories()"></paginate>
                </div>
                </div>
            </div>
        </div>
        <modal-delete 
          name="modalDeleteKodawariType"
          @delete="handleDelete"
        />
    </div>
</template>

<script>
import { ProductInformationService } from '../../services/ProductInformation/ProductInformationService'
import ModalDelete from '../shared/ModalDelete.vue'
import $ from 'jquery'
export default {
  components: { ModalDelete },
    props: {
        name: {
            type: String,
            default: 'modalCategory'
        },
    },
    data() {
      return {
        categories: [],
        service: new ProductInformationService(),
        params: {
          ProductId: '',
          KodawariTypeName: '',
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
        },
        totalData: 1,
        isBusy: false,
        selectedData: null,
        errored: [],
        categoryName: '',
        selectedEditData: null,
        image: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        selectedFile: null,
        description: ''
      }
    },
    mounted() {
      this.fetchCategories()
      const vm = this
      $(`#${this.name}`).on('hidden.bs.modal', function (e) {
        vm.categoryName = ''
        vm.image= {
          base64: '',
          fileName: '',
          contentType: ''
        }
        vm.selectedFile= null
        vm.selectedEditData = null
        vm.description = null
      })
    },
    methods: {
      async fetchCategories() {
        try {
            const res = await this.service.getAllKodawariType(this.params)
            this.categories = res.kodawariTypes
            this.totalData = res.totalKodawariTypes
        } catch (err) {
            console.error(err)
        }
      },
      async handleSave() {
        this.errored = []
        if(!this.categoryName) {
          this.errored.push({
            key: 'feature name',
            value: 'Please fill feature name field'
          })
        }
        // if(!this.description) {
        //   this.errored.push({
        //     key: 'description',
        //     value: 'Please fill description field'
        //   })
        // }
        // if(!this.selectedFile) {
        //   this.errored.push({
        //     key: 'Media',
        //     value: 'Please fill Media field'
        //   })
        // }
        if(this.errored.length > 0) {
          return
        }
        this.isBusy = true
        try {
          if(this.selectedEditData) {
            const body = {
              kodawariTypeId: this.selectedEditData.kodawariTypeId,
              "kodawariTypeName": this.categoryName,
              "kodawariTypeDescription": this.description,
              kodawariTypeFileContent : {
                base64: this.image.base64,
                fileName: this.image.fileName,
                contentType: this.image.contentType
              }
            }
            await this.service.updateKodawariType(body)

          } else {
            await this.service.createKodawariType({
              "kodawariTypeName": this.categoryName,
              "kodawariTypeDescription": this.description,
              kodawariTypeFileContent: {
                base64: this.image.base64,
                fileName: this.image.fileName,
                contentType: this.image.contentType
              },
            })
          }
          this.categoryName = ''
          this.image= {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.selectedFile= null
          this.selectedEditData = null
          this.fetchCategories()
        } catch (err) {

        }
        this.isBusy = false
      },
      handleFilter() {
        this.params.PageIndex = 1
        this.fetchCategories()
      },
      async handleDelete() {
        this.isBusy = true
        try {
          await this.service.deleteProductProgramCategory(this.selectedData.kodawariTypeId)
          this.fetchCategories()
          this.success = true
          this.categoryName = ''
          vm.image= {
            base64: '',
            fileName: '',
            contentType: ''
          }
          vm.selectedFile= null
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(category) {
        this.selectedEditData = category
        this.categoryName = category.kodawariTypeName
        this.selectedFile = category.imageUrl
        this.image.contentType = category.fileTypeBlob
        this.description = category.kodawariTypeDescription
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