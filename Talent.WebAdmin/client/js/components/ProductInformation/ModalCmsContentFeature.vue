<template>
    <div class="modal fade" :id="name" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 d-flex">
                        Add
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
                        <div class="input-group talent_front">
                          <label for="" class="w-100">Media (.jpg, .png, dll)</label>
                          <div class="d-flex align-items-center w-100">
                            <div class="container-upload">
                              <img :src="selectedFile ? selectedFile : `/upload-image2.png`" alt="" style="width: 100%">
                              <!-- <div class="btn-upload"> -->
                                <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                                  <label :for="`input-manfaat-baru-${this.name}`" style="margin: 0px">Browse</label>
                                </button>
                              <!-- </div> -->
                                <input type="file" :name="`input-manfaat-baru-${this.name}`" :id="`input-manfaat-baru-${this.name}`" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg">
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class="col-md-12 mt-4">
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
                      </div>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="row">
                      <div class="col-md-12">
                        <div class="input-group talent_front">
                          <label for="" class="w-100">Upload File (.pdf, .doc, dll)</label>
                          <div class="d-flex align-items-center w-100">
                            <!-- <div class="container-upload"> -->
                              <button class="btn btn-primary mr-4">
                                <label :for="`input-doc-${this.name}`" style="margin: 0px">Browse File</label>
                              </button>
                                {{ imageUpload.fileName }}
                              <input type="file" :name="`input-doc-${this.name}`" :id="`input-doc-${this.name}`" style="display: none;" @change="handleChangeFileUpload" accept=".pdf,.doc">
                            <!-- </div> -->
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <div class="modal-footer">
                    <div class="col-md-12 d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-around">
                            <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" @click="submitModel">Save</button>
                            <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { ProductInformationService } from '../../services/ProductInformation/ProductInformationService';
import $ from 'jquery'
import ModalCategory from './ModalCategory.vue';
export default {
  components: { ModalCategory },
    props: {
        name: {
            type: String,
            default: 'modalModel'
        },
    },
    mounted() {
      Promise.all([
        this.fetchCategories(),
        this.fetchSegments(),
      ])
      const vm = this
      $('#modalCategory').on('hidden.bs.modal', function (e) {
        vm.fetchCategories()
      })
    },
    data() {
      return {
        errored: [],
        selectedFile: null,
        selectedFileUpload: null,
        categories: [],
        segments: [],
        service: new ProductInformationService(),
        query: {
          ProductCategoryName: "",
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
        },
        selectedCategory: null,
        selectedSegment: null,
        modelName: '',
        isCompetitor: false,
        image: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        imageUpload: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        description: ''
      }
    },
    methods: {
      handleAddSub() {
        $('#modalModel').modal('hide')
        $('#modalCategory').modal('show')
      },
      async fetchCategories() {
        try {
          const res = await this.service.getAllCategories(this.query)
          this.categories = res.productCategories
        } catch(err) {
          console.error(err)
        }
      },
      async fetchSegments() {
        try {
          const res = await this.service.getAllSegments(this.query)
          this.segments = res
        } catch(err) {
          console.error(err)
        }
      },
      async submitModel(e) {
        if(this.handleValidation()) {
          return
        }
        try {
          this.$emit('callback', {
            image: this.image,
            imageUpload: this.imageUpload,
            description: this.description
          })
          $(`#${this.name}`).modal('hide');
        } catch (err) {
          console.error(err)
        }
      },
      handleValidation () {
        this.errored = []
        if(!this.description) {
          this.errored.push({
            key: 'description',
            value: 'Please fill description field'
          })
        }
        if(!this.selectedFile) {
          this.errored.push({
            key: 'media',
            value: 'Please fill media field'
          })
        }
        if(!this.selectedFileUpload) {
          this.errored.push({
            key: 'file upload',
            value: 'Please fill file upload field'
          })
        }
        return this.errored.length > 0
      },
      async handleChangeFile(e) {
        try {
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
        } catch (err) {
          console.log(err)
        }
      },
      async handleChangeFileUpload(e) {
        const selectedImage = e.target.files[0]
        // if(selectedImage.size > 104857600) {
        //     this.imageUpload.base64 = ''
        //     this.imageUpload.contentType = ''
        //     this.imageUpload.fileName = ''
        //     return
        // }
        let array = selectedImage.name.split(".");
        let extension = array.pop();
        const res = await this.getBase64Upload(selectedImage)
        this.imageUpload.base64 = res
        this.imageUpload.fileName = selectedImage.name
        this.imageUpload.contentType = extension
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
      },
      getBase64Upload(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
            let encoded = reader.result.toString().replace(/^data:(.*,)?/, '');
            if ((encoded.length % 4) > 0) {
                encoded += '='.repeat(4 - (encoded.length % 4));
            }
            this.selectedFileUpload = reader.result
            resolve(encoded);
            };
            reader.onerror = error => reject(error);
        });
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