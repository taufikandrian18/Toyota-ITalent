<template>
    <div class="modal fade" :id="name" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <loading-overlay :loading="isBusy || isBusyData"></loading-overlay>
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 d-flex">
                        Add Content
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
                        <label>Content Name<span class="talent_font_red"> *</span></label>
                        <div class="input-group">
                            <input name="model_name" type="text" class="form-control" v-model="categoryName"/>
                        </div>
                      </div>
                      <div class="col-md-12 mt-4">
                        <div class="input-group talent_front">
                          <label for="" class="w-100">Media (jpeg/png/mp4) maks 100MB<span class="talent_font_red"> *</span></label>
                          <div class="d-flex align-items-center w-100">
                            <div class="">
                              <img v-if="!['mp4', 'pdf'].includes(image.contentType)" class="h-100 w-100" :src="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                              <video v-if="image.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFile ? selectedFile : `/upload-image2.png`"></video>
                              <!-- <div class="btn-upload"> -->
                                <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                                  <label :for="`input-manfaat-${this.name}`" style="margin: 0px">Browse</label>
                                </button>
                              <!-- </div> -->
                                <input type="file" :name="`input-manfaat-${this.name}`" :id="`input-manfaat-${this.name}`" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg,.mp4">
                            </div>
                          </div>
                        </div>
                      </div>

                      <div class="col-md-12 mt-4">
                        <checkbox class="talent-checkbox-lineheight"
                                      id="is_competitor"
                                      v-model="isSequence">Is Sequence</checkbox>
                      </div>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="row">
                      <div class="col-md-12">
                        <div class="input-group talent_front">
                          <label for="" class="w-100">Upload File (pdf)</label>
                          <div class="d-flex align-items-center w-100">
                              <button class="btn btn-primary mr-4">
                                <label :for="`input-doc-${this.name}`" style="margin: 0px">Browse File</label>
                              </button>
                              <button class="btn btn-danger text-center mr-4" @click="handleDeleteFile" v-if="imageFile.fileName">
                                  <fa icon="trash" />
                              </button>
                                {{ imageFile.fileName }}
                              <input type="file" :name="`input-doc-${this.name}`" :id="`input-doc-${this.name}`" style="display: none;" @change="handleChangeFileFile" accept=".pdf">
                          </div>
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
                                }"
                                @change="(e) => description = e"/>
                          </div>
                        </div>
                      </div>
                      <!-- <div class="col-md-12 mt-4">
                        <div class="input-group talent_front">
                          <label for="" class="w-100">Video</label>
                          <div class="d-flex align-items-center w-100">
                            <div class="">
                              <img v-if="!['mp4', 'pdf'].includes(imageVideo.contentType)" class="h-100 w-100" :src="selectedFileVideo ? selectedFileVideo : `/upload-image2.png`" for="customFile" />
                              <video v-if="imageVideo.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFileVideo ? selectedFileVideo : `/upload-image2.png`"></video>
                              <object v-if="imageVideo.contentType === 'pdf'" class="h-100 w-100 min-heigth-full" type="application/pdf" width="100%" height="100%" :data="selectedFileVideo ? selectedFileVideo : `/upload-image2.png`" for="customFile" />
                              <div class="btn-upload">
                                <button class="btn btn-primary">
                                  <label for="input-manfaat" style="margin: 0px">Browse</label>
                                </button>
                              </div>
                                <input type="file" name="input-manfaat" id="input-manfaat" style="display: none;" @change="handleChangeFileVideo" accept=".pdf">
                            </div>
                          </div>
                        </div>
                      </div> -->
                      <div class="col-md-12 mt-4">
                        <label>Sequence</label>
                        <div class="input-group">
                            <input name="sequence" type="number" class="form-control" v-model="sequence"/>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="col-md-12">
                  <div v-if="imageFile.contentType === 'pdf'" style="height: 500px;">
                    <object v-if="imageFile.contentType === 'pdf'" class="w-100" type="application/pdf" width="100%" height="500px" :data="selectedFileFile ? selectedFileFile : `/upload-image2.png`" for="customFileFile" style="height: 100%"/>
                  </div>
                </div>

                <div class="col-md-12 d-flex justify-content-end mt-4" style="z-index: 100">
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
                                <input class="form-control" placeholder="Search By Name" v-model="params.cms_ContentName"/>
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
                                        Content Name
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a href="#" class="talent_sort" style="color: white;">
                                        Description Name
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
                            <tr v-for="dictionary in categories" :key="dictionary.cms_ContentId">
                                <td>
                                    {{dictionary.cms_ContentName}}
                                </td>
                                <td class="talent_nopadding_important">
                                    <button class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(dictionary)">Edit</button>
                                </td>
                                <td class="talent_nopadding_important">
                                    <button type="button" class="btn btn-block talent_bg_red talent_font_white" 
                                            data-toggle="modal"
                                            data-target="#modalDeleteCmsContentFull" @click="selectedData = dictionary">
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
          name="modalDeleteCmsContentFull"
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
        type: {
          type: String,
          default: 'SPWA'
        }
    },
    data() {
      return {
        categories: [],
        service: new ProductInformationService(),
        params: {
          Cms_ContentCategory: '',
          cms_ContentName: '',
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
        },
        totalData: 1,
        isBusy: false,
        isBusyData: false,
        selectedData: null,
        errored: [],
        categoryName: '',
        selectedEditData: null,
        image: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        imageFile: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        imageVideo: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        selectedFile: null,
        selectedFileFile: null,
        selectedFileVideo: null,
        isSequence: false,
        sequence: 1,
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
        vm.imageFile= {
          base64: '',
          fileName: '',
          contentType: ''
        }
        vm.imageVideo= {
          base64: '',
          fileName: '',
          contentType: ''
        }
        vm.selectedFile= null
        vm.selectedFileFile= null
        vm.selectedFileVideo= null
        vm.selectedEditData = null
        vm.isSequence= false
        vm.sequence= 1
        vm.description = ''
        vm.params.cms_ContentName = ''
        vm.fetchCategories()
      })
    },
    methods: {
      async fetchCategories() {
        this.isBusyData = true
        this.params.Cms_ContentCategory = this.type
        try {
            const res = await this.service.getAllCMSContent(this.params)
            this.categories = res.cmsContents
            this.totalData = res.totalCmsContents
        } catch (err) {
            console.error(err)
        }
        this.isBusyData = false
      },
      async handleSave() {
        console.log('called')
        this.errored = []
        if(!this.categoryName) {
          this.errored.push({
            key: 'Category Name',
            value: 'Please fill Category Name field'
          })
        }
        if(!this.selectedFile) {
          this.errored.push({
            key: 'image',
            value: 'Please fill media field'
          })
        }
        if(!this.description) {
          this.errored.push({
            key: 'description',
            value: 'Please fill description field'
          })
        }
        if(this.errored.length > 0) {
          return
        }
        this.isBusy = true
        try {
          if(this.selectedEditData) {
            const body = {
              cms_ContentId: this.selectedEditData.cms_ContentId,
              "cmsContentPicContent": {
                "base64": this.image.contentType != 'mp4' ? this.image.base64 : '',
                "fileName": this.image.contentType != 'mp4' ? this.image.fileName : '',
                "contentType": this.image.contentType != 'mp4' ? this.image.contentType : ''
              },
              "cmsContentFileContent": {
                "base64": this.imageFile.base64,
                "fileName": this.imageFile.fileName,
                "contentType": this.imageFile.contentType
              },
              "cmsContentVidContent": {
                "base64": this.image.contentType == 'mp4' ? this.image.base64 : '',
                "fileName": this.image.contentType == 'mp4' ? this.image.fileName : '',
                "contentType": this.image.contentType == 'mp4' ? this.image.contentType : ''
              },
              "cms_ContentDescription": this.description,
              "cms_ContentName": this.categoryName,
              "cms_ContentCategory": this.type,
              "cms_ContentSequence": Number(this.sequence),
              "cms_ContentIsSequence": this.isSequence,
              isDeleteFile: this.imageFile.base64 ? false : this.imageFile.isDeleteFile,
              isDeleteVid: this.image.contentType != 'mp4',
              isDeletePic: this.image.contentType == 'mp4'
            }
            await this.service.updateCMSContent(body)

          } else {
            await this.service.createCMSContent({
              "cmsContentPicContent": {
                "base64": this.image.contentType != 'mp4' ? this.image.base64 : '',
                "fileName": this.image.contentType != 'mp4' ? this.image.fileName : '',
                "contentType": this.image.contentType != 'mp4' ? this.image.contentType : ''
              },
              "cmsContentFileContent": {
                "base64": this.imageFile.base64,
                "fileName": this.imageFile.fileName,
                "contentType": this.imageFile.contentType
              },
              "cmsContentVidContent": {
                "base64": this.image.contentType == 'mp4' ? this.image.base64 : '',
                "fileName": this.image.contentType == 'mp4' ? this.image.fileName : '',
                "contentType": this.image.contentType = 'mp4' ? this.image.contentType : ''
              },
              "cms_ContentDescription": this.description,
              "cms_ContentName": this.categoryName,
              "cms_ContentCategory": this.type,
              "cms_ContentSequence": Number(this.sequence),
              "cms_ContentIsSequence": this.isSequence
            })
          }
          this.categoryName = ''
          this.image= {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.imageFile= {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.imageVideo= {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.selectedFile= null
          this.selectedFileFile= null
          this.selectedFileVideo= null
          this.selectedEditData = null
          this.isSequence= false
          this.sequence= 1
          this.description = ''
          this.fetchCategories()
        } catch (err) {
          console.log(err)
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
          await this.service.deleteCMSContent(this.selectedData.cms_ContentId)
          this.fetchCategories()
          this.success = true
          this.image= {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.imageFile= {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.imageVideo= {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.selectedFile= null
          this.selectedFileFile= null
          this.selectedFileVideo= null
          this.selectedEditData = null
          this.isSequence= false
          this.sequence= 1
          this.description = ''
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(category) {
        this.selectedEditData = category
        this.categoryName = category.cms_ContentName
        this.sequence = category.cms_ContentSequence
        this.isSequence = category.cms_ContentIsSequence
        this.description = category.cms_ContentDescription

        this.selectedFile = category.blobUrl ? category.blobUrl : category.videoBlobUrl
        this.selectedFileFile = category.fileBlobUrl
        this.selectedFileVideo = category.videoBlobUrl
        this.image = {
          base64: null,
          fileName: category.blobId ? category.blob.name : category.videoBlob.name,
          contentType: category.blobId ? category.blob.mime : category.videoBlob.mime,
        }
        this.imageFile = {
          base64: null,
          fileName: category.fileBlob.name,
          contentType: category.fileBlob.mime,
        }
        this.imageVideo = {
          base64: null,
          fileName: category.videoBlob.name,
          contentType: category.videoBlob.mime,
        }
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
      },
      async handleChangeFileFile(e) {
        const selectedImage = e.target.files[0]
        // if(selectedImage.size > 104857600) {
        //     this.imageFile.base64 = ''
        //     this.imageFile.contentType = ''
        //     this.imageFile.fileName = ''
        //     return
        // }
        let array = selectedImage.name.split(".");
        let extension = array.pop();
        this.selectedFileFile = URL.createObjectURL(selectedImage)
        const res = await this.getBase64File(selectedImage)
        this.imageFile.base64 = res
        this.imageFile.fileName = selectedImage.name
        this.imageFile.contentType = extension
        this.imageFile.isDeleteFile = false
      },
      getBase64File(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
            let encoded = reader.result.toString().replace(/^data:(.*,)?/, '');
            if ((encoded.length % 4) > 0) {
                encoded += '='.repeat(4 - (encoded.length % 4));
            }
            // this.selectedFileFile = reader.result
            resolve(encoded);
            };
            reader.onerror = error => reject(error);
        });
      },
      async handleChangeFileVideo(e) {
        const selectedImage = e.target.files[0]
        // if(selectedImage.size > 104857600) {
        //     this.imageFile.base64 = ''
        //     this.imageFile.contentType = ''
        //     this.imageFile.fileName = ''
        //     return
        // }
        let array = selectedImage.name.split(".");
        let extension = array.pop();
        this.selectedFileVideo = URL.createObjectURL(selectedImage)
        const res = await this.getBase64Video(selectedImage)
        this.imageFile.base64 = res
        this.imageFile.fileName = selectedImage.name
        this.imageFile.contentType = extension
      },
      getBase64Video(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
            let encoded = reader.result.toString().replace(/^data:(.*,)?/, '');
            if ((encoded.length % 4) > 0) {
                encoded += '='.repeat(4 - (encoded.length % 4));
            }
            // this.selectedFileVideo = reader.result
            resolve(encoded);
            };
            reader.onerror = error => reject(error);
        });
      },
      handleDeleteFile() {
        this.selectedFileFile = null
        this.imageFile = {
          base64: null,
          fileName: '',
          contentType: '',
          isDeleteFile: true
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
.container-upload{
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