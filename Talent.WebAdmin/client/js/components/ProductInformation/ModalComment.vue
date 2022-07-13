<template>
    <div class="modal fade" :id="name" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 d-flex">
                        Add Comment
                    </div>
                </div>
                <div class="row px-4 py-4">
                  <div class="col-md-12 p-0">
                    <div class="alert alert-danger alert-dismissible fade show" role="alert" v-if="errored.length > 0">
                        <ul>
                          <li v-for="error in errored" :key="error.key">{{error.value}}</li>
                        </ul>
                    </div>
                  </div>
                  <div class="col-md-6">
                    <div class="row">
                      <div class="col-md-12">
                        <label>Comment</label>
                        <div class="input-group">
                            <textarea name="model_name" type="text" class="form-control" v-model="categoryName"/>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>

                <div class="col-md-12 d-flex justify-content-end mt-4 mb-4">
                  <button class="btn btn-secondary mr-2" data-dismiss="modal">
                      Cancel
                  </button>
                  <button class="btn btn-primary" @click="handleSave">
                      {{ selectedEditData ? 'Save' : 'Save'}}
                  </button>
                </div>
            </div>
        </div>
        <modal-delete 
          name="modalDeleteKodawariMenu"
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
        },
        data: {
          type: Object,
          default: () => {}
        }
    },
    data() {
      return {
        categories: [],
        service: new ProductInformationService(),
        params: {
            Cms_MenuCategory: this.type,
            hoGuidelineComment: '',
            SortBy: '',
            PageIndex: 1,
            PageSize: 10
        },
        totalData: 1,
        isBusy: false,
        selectedData: null,
        errored: [],
        categoryName: '',
        selectedEditData: null
      }
    },
    mounted() {
        this.params.Cms_MenuCategory = this.type
      // this.fetchCategories()
      const vm = this
      $(`#${this.name}`).on('hidden.bs.modal', function (e) {
        vm.categoryName = ''
        vm.selectedEditData = null
      })
    },
    watch: {
      data() {
        this.categoryName = this.data.hoGuidelineComment
      }
    },
    methods: {
      async fetchCategories() {
        this.params.Cms_MenuCategory = this.type
        try {
            const res = await this.service.getAllKodawariMenu(this.params)
            this.categories = res.kodawariMenus
            this.totalData = res.totalKodawariMenus
        } catch (err) {
            console.error(err)
        }
      },
      async handleSave() {
        this.errored = []
        if(!this.categoryName) {
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
            hoGuidelineId: this.data.hoGuidelineId,
            "hoGuidelineComment": this.categoryName,
            approvedAt: this.data.approvedAt,
          })
          this.categoryName = ''
          this.selectedEditData = null
          this.fetchCategories()
        } catch (err) {

        }
        this.isBusy = false
        $('#modalComment').modal('hide')
      },
      handleFilter() {
        this.params.PageIndex = 1
        this.fetchCategories()
      },
      async handleDelete() {
        this.isBusy = true
        try {
          await this.service.deleteKodawariMenu(this.selectedData.hoGuidelineId)
          this.fetchCategories()
          this.success = true
          this.categoryName = ''
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(category) {
        this.selectedEditData = category
        this.categoryName = category.hoGuidelineComment
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