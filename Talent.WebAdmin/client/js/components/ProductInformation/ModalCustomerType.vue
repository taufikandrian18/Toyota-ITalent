<template>
    <div class="modal fade" :id="name" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 d-flex">
                        Add Customer Type
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
                        <label>Customer Type Name<span class="talent_font_red"> *</span></label>
                        <div class="input-group">
                            <input name="model_name" type="text" class="form-control" v-model="categoryName"/>
                        </div>
                      </div>
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
                                <input class="form-control" placeholder="Search By Name" v-model="params.ProductCustomerTypeName"/>
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
                            <tr v-for="dictionary in categories" :key="dictionary.productCustomerTypeId">
                                <td>
                                    {{dictionary.productCustomerTypeName}}
                                </td>
                                <td class="talent_nopadding_important">
                                    <button class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(dictionary)">Edit</button>
                                </td>
                                <td class="talent_nopadding_important">
                                    <button type="button" class="btn btn-block talent_bg_red talent_font_white" 
                                            data-toggle="modal"
                                            data-target="#modalDeleteCategory" @click="selectedData = dictionary">
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
          name="modalDeleteCategory"
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
            ProductCustomerTypeName: '',
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
      this.fetchCategories()
      const vm = this
      $('#modalCustomerType').on('hidden.bs.modal', function (e) {
        vm.categoryName = ''
        vm.selectedEditData = null
      })
    },
    methods: {
      async fetchCategories() {
        try {
            const res = await this.service.getAllCustomerType(this.params)
            this.categories = res.productCustomerTypes
            this.totalData = res.totalProductCustomerTypes
        } catch (err) {
            console.error(err)
        }
      },
      async handleSave() {
        this.errored = []
        if(!this.categoryName) {
          this.errored.push({
            key: 'category name',
            value: 'Please fill category name field'
          })
        }
        if(this.errored.length > 0) {
          return
        }
        this.isBusy = true
        try {
          if(this.selectedEditData) {
            await this.service.updateCustomerType({
              productCustomerTypeId: this.selectedEditData.productCustomerTypeId,
              "productCustomerTypeName": this.categoryName
            })

          } else {
            await this.service.createCustomerType({
              "productCustomerTypeName": this.categoryName
            })
          }
          this.categoryName = ''
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
          await this.service.deleteCustomerType(this.selectedData.productCustomerTypeId)
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
        this.categoryName = category.productCustomerTypeName
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