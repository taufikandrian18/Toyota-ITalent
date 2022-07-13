<template>
  <div>
    <loading-overlay :loading="isBusy"></loading-overlay>

    <div class="row">
      <div class="col mb-2">
        <h3>
            <fa icon="chart-pie"></fa> My Tools >
            <span>Dictionary > </span> 
            <span class="talent_font_red">{{id ? 'Edit' : 'Add'}} Dictionary</span>
        </h3>
      </div>

      <div class="col-md-12">
        <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
          <div class="col-md-12">
            <div class="row">
              <div class="col-md-6 mb-4">
                <div class="input-group talent_front">
                  <label for="" class="w-100">Name</label>
                  <div class="d-flex align-items-center w-100">
                    <div class="input-group">
                        <input
                            class="form-control"
                            v-model="form.name"
                        />
                    </div>
                  </div>
                </div>
              </div>

              <!-- <div class="col-md-6 mb-4">
                <div class="input-group talent_front">
                  <label for="" class="w-100">Status</label>
                  <div class="d-flex align-items-center w-100">
                    <div class="custom-control custom-switch">
                      <input type="checkbox" class="custom-control-input" :id="`customSwitch`" :checked="form.status" @change="handleChangeStatus">
                      <label class="custom-control-label" :for="`customSwitch`"></label>
                    </div>
                  </div>
                </div>
              </div> -->

              <div class="col-md-6 mb-4">
                <div class="input-group talent_front">
                  <label for="" class="w-100">Media (jpeg, png, mp4, pdf)</label>
                  <div class="d-flex align-items-center w-100">
                    <div class="container-upload">
                      <!-- <img :src="selectedFile ? selectedFile : `/upload-image2.png`" alt="" style="width: 100%"> -->
                      <img v-if="!['mp4', 'pdf'].includes(image.contentType)" class="h-100 w-100" :src="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                      <video v-if="image.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFile ? selectedFile : `/upload-image2.png`"></video>
                      <!-- <div class="btn-upload"> -->
                        <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                          <label for="input-manfaat" style="margin: 0px">Browse</label>
                        </button>
                      <!-- </div> -->
                        <input type="file" name="input-manfaat" id="input-manfaat" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg,.mp4,.pdf,.gif">
                    </div>
                  </div>
                </div>
              </div>

              <div class="col-md-6 mb-4">
                <label>Manfaat</label>
                <div class="d-flex align-items-center w-100">
                  <wysiwyg 
                      class="w-100" 
                      v-model="form.manfaat" 
                      :options="{
                          hideModules: {image: true, table: true, removeFormat: true, code: true }
                      }"
                      @change="(e) => handleChangeEditor('manfaat', e)"/>
                </div>
              </div>

              <div class="col-md-6 mb-4">
                <div class="input-group talent_front">
                  <label for="" class="w-100">Arti</label>
                  <div class="d-flex align-items-center w-100">
                    <wysiwyg 
                        class="w-100" 
                        v-model="form.arti" 
                        :options="{
                            hideModules: {image: true, table: true, removeFormat: true, code: true }
                        }"
                        @change="(e) => handleChangeEditor('arti', e)"/>
                  </div>
                </div>
              </div>

              <div class="col-md-6 mb-4">
                <label>Fakta</label>
                  <div class="d-flex align-items-center w-100">
                    <wysiwyg 
                        class="w-100" 
                        v-model="form.fakta" 
                        :options="{
                            hideModules: {image: true, table: true, removeFormat: true, code: true }
                        }"
                        @change="(e) => handleChangeEditor('fakta', e)"/>
                  </div>
              </div>

              <div class="col-md-6 mb-4">
                <div class="input-group talent_front">
                  <label for="" class="w-100">Basic Operation</label>
                  <div class="d-flex align-items-center w-100">
                    <wysiwyg 
                        class="w-100" 
                        v-model="form.basicOperation" 
                        :options="{
                            hideModules: {image: true, table: true, removeFormat: true, code: true }
                        }"
                        @change="(e) => handleChangeEditor('basicOperation', e)"/>
                  </div>
                </div>
              </div>


              <div class="col-md-12 p-0 mb-4">
                  <div class="alert alert-danger alert-dismissible fade show" role="alert" v-if="errored.length > 0">
                      <ul>
                        <li v-for="error in errored" :key="error.key">{{error.value}}</li>
                      </ul>
                      <!-- <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click="handleMessageClose">
                          <span aria-hidden="true">&times;</span>
                      </button> -->
                  </div>
              </div>

              <div class="col-md-12 mt-4">
                <object v-if="image.contentType === 'pdf'" class="h-100 w-100 min-heigth-full" type="application/pdf" width="100%" height="100%" :data="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
              </div>

              <div class="col-md-12 d-flex justify-content-end">
                <button class="btn btn-success mr-2" @click="handleSave(null)">
                    Save
                </button>
                <button class="btn btn-primary" @click="handleSave(moment().format('YYYY-MM-DD'))">
                    Submit
                </button>
              </div>
            </div>
          </div>
      </div>
      </div>
    </div>

  </div>
</template>

<script lang="ts">
  import Vue from 'vue';
  import Component from 'vue-class-component';
  import { CreateDictionaryModel } from '../../../services/Dictionary/DictionaryModel';
  import { DictionaryService } from '../../../services/Dictionary/DictionaryService';
  import moment from 'moment';
    
  @Component({
    props: ['claims'],
    created: async function(this: AddDictionary) {
      let uri = window.location.search.substring(1); 
      let params = new URLSearchParams(uri);
      this.id = params.get('id')
      this.fetchDetail(this.id)
    }
  })
  export default class AddDictionary extends Vue {

    isBusy: boolean = false
    dictionaryService : DictionaryService = new DictionaryService()
    moment = moment
    
    id: string = null
    form: any = {
      name: '',
      status: false,
      media: '',
      manfaat: '',
      arti: '',
      fakta: '',
      basicOperation: ''
    }  

    errored: any = []

    selectedFile = null
    fileUploadType: string = 'image'
    image: any = {
      base64: '',
      fileName: '',
      contentType: 'png'
    }

    async fetchDetail(id) {
      this.isBusy = true
      try {
        const res = await this.dictionaryService.getDictionary(id)
        this.form.name = res.dictionaryName
        this.form.status = res.dictionaryStatus
        this.form.manfaat = res.dictionaryManfaat
        this.form.arti = res.dictionaryArti
        this.form.fakta = res.dictionaryFakta
        this.form.basicOperation = res.dictionaryBasicOperation
        this.selectedFile = res.imageUrl
        this.image.contentType = res.blob.mime
      } catch (err) {
        console.log(err)
      }
      this.isBusy = false
    }

    handleChangeEditor(field, value) {
      this.form[field] = value
    }

    handleChangeStatus(e) {
      this.form.status = !this.form.status
    }

    async handleSave(approvedAt) {
      if(this.handleValidation()) {
        return
      }
      const body : CreateDictionaryModel = {
        dictionaryName: this.form.name,
        dictionaryArti: this.form.arti,
        dictionaryManfaat: this.form.manfaat,
        dictionaryFakta: this.form.fakta,
        dictionaryStatus: this.form.status,
        dictionaryBasicOperation: this.form.basicOperation,
        productFileContent: {
          base64: this.image.base64,
          fileName: this.image.fileName,
          contentType: this.image.contentType
        },
        approvedAt: approvedAt
      }
      
      this.isBusy = true
      try {
        if(this.id) {
          body.dictionaryId = this.id
          if(!this.selectedFile) {
            body.productFileContent = null
          }
          await this.dictionaryService.updateDictionary(body)
        } else {
          await this.dictionaryService.createDictionary(body)
        }
        // this.$router.push({path: '/MyTools/Dictionary'})
        window.location.href = "/MyTools/Dictionary";
      } catch (err) {
        console.log(err)
      }
      this.isBusy = false
    }

    handlePreventMenuClose(e) {
      e.stopPropagation();
    }

    // logic
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
    }

    getBase64(file) {
      return new Promise((resolve, reject) => {
          const reader = new FileReader();
          // reader.readAsDataURL(file);
          // reader.onload = () => {
          //   let encoded = reader.result.toString().replace(/^data:(.*,)?/, '');
          //   if ((encoded.length % 4) > 0) {
          //       encoded += '='.repeat(4 - (encoded.length % 4));
          //   }
          //   this.selectedFile = reader.result
          //   resolve(encoded);
          // };
          // reader.onerror = error => reject(error);

          reader.readAsDataURL(file);
          reader.onload = () => {
            let encoded = reader.result.toString().split(',')[1];
            // this.selectedFile = reader.result
            resolve(encoded);
          }
          reader.onerror = error => {
              reject(error);
          }
      });
    }

    handleValidation () {
      this.errored = []
      if(!this.form.name) {
        this.errored.push({
          key: 'name',
          value: 'Please fill name field'
        })
      }
      if(!this.form.manfaat) {
        this.errored.push({
          key: 'manfaat',
          value: 'Please fill manfaat field'
        })
      }
      if(!this.form.arti) {
        this.errored.push({
          key: 'arti',
          value: 'Please fill arti field'
        })
      }
      if(!this.form.fakta) {
        this.errored.push({
          key: 'fakta',
          value: 'Please fill fakta field'
        })
      }
      // if(!this.form.basicOperation) {
      //   this.errored.push({
      //     key: 'basicOperation',
      //     value: 'Please fill basic operation field'
      //   })
      // }
      if(!this.selectedFile && !this.id) {
        this.errored.push({
          key: 'media',
          value: 'Please fill media field'
        })
      }
      return this.errored.length > 0
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
  top: 0;
  display: flex;
  justify-content: end;
  width: 100%;
  padding: 8px 9px;
}
</style>