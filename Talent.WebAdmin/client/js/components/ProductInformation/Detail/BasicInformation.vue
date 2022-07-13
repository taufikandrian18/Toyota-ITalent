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
        Success Edit Basic Information Product
      </div>
    </div>
    <div class="col-md-12 mb-3">
      <h5>Basic Information</h5>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Category<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Category"
                :options="categories"
                label="productCategoryName"
                track-by="productCategoryId"
                v-model="selectedCategory">
              </multiselect>
              <button class="btn_add_rounded" @click="handleAddSub">
                +
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <label for="" class="w-100">Segment<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Segment"
                :options="segments"
                label="name"
                track-by="name"
                v-model="selectedSegment">
              </multiselect>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <label>Model Name<span class="talent_font_red"> *</span></label>
          <div class="input-group">
              <input name="model_name" type="text" class="form-control" v-model="modelName"/>
          </div>
        </div>
        <div class="col-md-12">
          <checkbox class="talent-checkbox-lineheight"
                        id="is_competitor"
                        v-model="isCompetitor">Is Competitor</checkbox>
        </div>
      </div>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <div class="input-group talent_front">
            <label for="" class="w-100">Thumbnail<span class="talent_font_red"> *</span></label>
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
      <button class="btn btn-secondary mr-2" @click="reset">
          Close
      </button>
      <button class="btn btn-success mr-2" @click="submitModel(null)">
          Save
      </button>
      <button class="btn btn-primary" @click="submitModel(1)">
          Submit
      </button>
    </div>
    <modal-category name="modalCategory"/>
  </div>
</template>


<script>
import { ProductInformationService } from '../../../services/ProductInformation/ProductInformationService';
import $ from 'jquery'
import ModalCategory from '../ModalCategory.vue';
import moment from 'moment';

export default {
  components: { ModalCategory },
    async mounted() {
      this.isBusy = true
      try {
        await this.fetchCategories()
        await this.fetchSegments()
        await this.fetchDetail()
      } catch(err) {
        console.error(err)
      }
      this.isBusy = false
      const vm = this
      $('#modalCategory').on('hidden.bs.modal', function (e) {
        vm.fetchCategories()
      })
    },
    data() {
      return {
        isBusy: false,
        errored: [],
        selectedFile: null,
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
        product: {},
        success: false
      }
    },
    methods: {
      reset() {
        window.location.href = `/MyTools/ProductInformation`
      },
      getSegmentName(id) {
        switch (id) {
          case 1:
            return 'Low'
        
          case 2:
            return 'Medium'
        
          case 3:
            return 'High'
        
          case 4:
            return 'Luxury'
        
          default:
            return 'Low'
        }
      },
      async fetchDetail() {
        try {
          const res = await this.service.getProductById({
            ProductId: this.id
          })
          this.product = res
          this.modelName = res.productName
          this.selectedFile = res.imageUrl
          this.isCompetitor = res.isCompetitor
          this.selectedCategory = {
            productCategoryId: res.productCategoryId,
            productCategoryName: res.productCategoryName
          }
          this.selectedSegment = {
            id: res.productSegment,
            name: this.getSegmentName(res.productSegment)
          }
        } catch (err) {
          console.error(err)
        }
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
        this.isBusy = true
        this.success = false
        try {
          const body = {
            productId: this.id,
            productName: this.modelName,
            productCategoryId: this.selectedCategory.productCategoryId,
            productSegment: this.selectedSegment.id,
            productFileContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            },
            isCompetitor: this.isCompetitor,
            approvedAt: e ? moment().format('YYYY-MM-DD') : null
          }
          if(!this.image.base64) {
            delete body.productFileContent
          }
          const res = await this.service.updateProduct(body)
          this.success = true
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleValidation () {
        this.errored = []
        if(!this.modelName) {
          this.errored.push({
            key: 'model name',
            value: 'Please fill model name field'
          })
        }
        if(!this.selectedCategory) {
          this.errored.push({
            key: 'category',
            value: 'Please fill category field'
          })
        }
        if(!this.selectedSegment) {
          this.errored.push({
            key: 'segment',
            value: 'Please fill segment field'
          })
        }
        if(!this.selectedFile) {
          this.errored.push({
            key: 'thumbnail',
            value: 'Please fill thumbnail field'
          })
        }
        return this.errored.length > 0
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
      },
      handleAddSub() {
        $('#modalCategory').modal('show')
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