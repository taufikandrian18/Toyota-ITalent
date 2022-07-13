<template>
  <div class="row px-4 py-4 w-100">
    <loading-overlay :loading="isBusy || isBusyData"></loading-overlay>
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
      <h5>Product Features</h5>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Type<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Category"
                :options="productFeatureTypes"
                label="productTypeName"
                track-by="productTypeId"
                v-model="selectedProductFeatureType">
              </multiselect>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Category<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Category"
                :options="productFeatureCategories"
                label="productFeatureCategoryName"
                track-by="productFeatureCategoryId"
                v-model="selectedProductFeatureCategory">
              </multiselect>
              <button class="btn_add_rounded" @click="handleAddSubFeatureCategory">
                +
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <label for="" class="w-100">Choose Feature<span class="talent_font_red"> *</span></label>
            <div class="d-flex align-items-center w-100">
              <multiselect
                class="w-100 mr-2"
                name="Category"
                :options="productFeatureFeatures"
                label="productFeatureName"
                track-by="productFeatureId"
                v-model="selectedProductFeatureFeature">
              </multiselect>
              <button class="btn_add_rounded" @click="handleAddSubFeature">
                +
              </button>
            </div>
          </div>
        </div>
        <!-- <div class="col-md-12 mt-4">
          <checkbox class="talent-checkbox-lineheight"
                        id="is_special"
                        v-model="isSpecial">Is Special</checkbox>
        </div> -->
      </div>
    </div>
    <div class="col-md-6">
      <div class="row">
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <div class="d-flex align-items-center w-100">
              <span for="" class="w-auto mr-4">Add a Content<span class="talent_font_red"> *</span></span>
              <button :class="`${cmsContent.image ? 'btn_edit_rounded' : 'btn_add_rounded'}`" @click="handleAddSubContent">
                <span v-if="cmsContent.image"><fa icon="edit"></fa></span>
                <span v-else>+</span>
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <div class="d-flex align-items-center w-100">
              <span for="" class="w-auto mr-4">Add a Benefits, Facts, Meanings</span>
              <button :class="`${cmsFmb.image ? 'btn_edit_rounded' : 'btn_add_rounded'}`" @click="handleAddSubFmb">
                <span v-if="cmsFmb.image"><fa icon="edit"></fa></span>
                <span v-else>+</span>
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <div class="d-flex align-items-center w-100">
              <span for="" class="w-auto mr-4">Add a Operation</span>
              <button :class="`${cmsOperation.image ? 'btn_edit_rounded' : 'btn_add_rounded'}`" @click="handleAddSubOperation">
                <span v-if="cmsOperation.image"><fa icon="edit"></fa></span>
                <span v-else>+</span>
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <div class="d-flex align-items-center w-100">
              <span for="" class="w-auto mr-4">Add a Work Principle</span>
              <button :class="`${cmsWorkPrincipal.image ? 'btn_edit_rounded' : 'btn_add_rounded'}`" @click="handleAddSubWorkPrincipal">
                <span v-if="cmsWorkPrincipal.image"><fa icon="edit"></fa></span>
                <span v-else>+</span>
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="input-group talent_front">
            <div class="d-flex align-items-center w-100">
              <span for="" class="w-auto mr-4">Add a Setting</span>
              <button :class="`${cmsSetting.image ? 'btn_edit_rounded' : 'btn_add_rounded'}`" @click="handleAddSubSetting">
                <span v-if="cmsSetting.image"><fa icon="edit"></fa></span>
                <span v-else>+</span>
              </button>
            </div>
          </div>
        </div>
      </div>
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
      <div class="col-md-12 talent_magin_small mt-3">
          <div class="row align-items-center justify-content-between">
              <div class="col d-flex py-0 pl-0 pr-4">
                  <div class="dropdown">
                        <button class="btn btn-info mr-2" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-expanded="false">
                          Filter
                        </button>
                        <div class="dropdown-menu p-3" aria-labelledby="dropdownMenuButton" style="min-width: 500px" @click="handlePreventMenuClose">
                          <b>Filter</b>
                          <div class="row">
                            <div class="col">
                              <div class="input-group talent_front">
                                <label for="">Status</label>
                                <div class="w-100 d-flex flex-wrap">
                                  <div :class="`${selectedFilterStatus == data.value ? 'filter-status-active' : 'filter-status'} mx-2 my-2`" v-for="data in filterStatus" :key="data.value" @click="handleSelectedFilterStatus(data)">
                                    {{ data.label }}
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div class="row d-flex flex-wrap">
                            <hr class="w-100">
                            <div class="w-100 mt-1 d-flex justify-content-end px-3">
                              <button data-toggle="dropdown" class="btn btn-info mr-3" @click="handleFilter">Apply</button>
                              <button data-toggle="dropdown" class="btn btn-info mr-3" @click="handleResetFilter">Reset</button>
                              <button data-toggle="dropdown" class="btn btn-danger" @click="hideDropdown">Cancel</button>
                            </div>
                          </div>
                        </div>
                  </div>
                  <div class="input-group">
                      <input class="form-control" placeholder="Search By Title" v-model="params.Query" @keyup.enter="handleFilter"/>
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
                            Category
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Features
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Type
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Content
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Facts
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Operation
                          </a>
                      </th>
                      <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Setting
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
                  <tr v-for="dictionary in productTypes" :key="dictionary.productFeatureMappingId">
                      <td>
                          {{dictionary.productFeatureCategoryName}}
                      </td>
                      <td>
                          {{dictionary.productFeatureName}}
                      </td>
                      <td>
                          {{dictionary.productTypeName}}
                      </td>
                      <td style="max-width: 150px;white-space: normal" v-html="dictionary.cms_ContentDescription">
                      </td>
                      <td style="max-width: 150px;white-space: normal" v-html="dictionary.cms_FmbDescription">
                      </td>
                      <td style="max-width: 150px;white-space: normal" v-html="dictionary.cms_OperationDescription">
                      </td>
                      <td style="max-width: 150px;white-space: normal" v-html="dictionary.cms_SettingDescription">
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
                      <td class="talent_nopadding_important text-center">
                        <div class="d-flex align-items-center w-100">
                          <!-- <div class="custom-control custom-switch w-100">
                            <input type="checkbox" class="custom-control-input" :id="`customSwitch-${dictionary.productTipId}`" :checked="dictionary.isApproved != 'Draft'" @change="handleChangeStatus(dictionary)">
                            <label class="custom-control-label" :for="`customSwitch-${dictionary.productTipId}`"></label>
                          </div> -->
                          <button type="button" class="btn btn-block talent_bg_orange" v-if="dictionary.productFeatureMappingApprovalStatus == 'Draft'" @click="handleChangeStatus(dictionary)">
                            Approve
                          </button>
                        </div>
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
    <modal-category-tip 
      name="modalCategoryTip"
    />
    <modal-product-feature 
      name="modalProductFeature"/>
      
    <modal-feature-category
      name="modalProductFeatureCategory"/>

    <modal-cms-content
      :data="cmsContent"
      name="modalCmsContent"
      title="Content"
      @callback="handleCmsContent"/>

    <modal-cms-content-another
      :data="cmsOperation"
      name="modalCmsOperation"
      title="Operation"
      @callback="handleCmsOperation"/>
    <modal-cms-content-another
      :data="cmsSetting"
      name="modalCmsSetting"
      title="Setting"
      @callback="handleCmsSetting"/>
    <modal-cms-content-another
      :data="cmsFmb"
      name="modalCmsFmb"
      title="Benefits, Facts, Meanings"
      @callback="handleCmsFmb"/>
    <modal-cms-content-another
      :data="cmsWorkPrincipal"
      title="Work Principle"
      name="modalCmsWorkPrincipal"
      @callback="handleCmsWorkPrincipal"/>
  </div>
</template>


<script>
import { ProductInformationService } from '../../../services/ProductInformation/ProductInformationService';
import $ from 'jquery'
import ModalDelete from '../../shared/ModalDelete.vue';
import moment from 'moment'
import ModalCategoryTip from '../ModalCategoryTip.vue';
import ModalProductFeature from '../ModalProductFeature.vue';
import ModalCmsContent from '../ModalCmsContent.vue';
import ModalCmsSubContent from '../ModalCmsContentFeature.vue';
import ModalFeatureCategory from '../ModalFeatureCategory.vue';
import ModalCmsContentAnother from '../ModalCmsContentAnother.vue';

export default {
  components: { ModalDelete, ModalCategoryTip, ModalProductFeature, ModalCmsContent, ModalCmsSubContent, ModalFeatureCategory, ModalCmsContentAnother },
    async mounted() {
      this.isBusy = true
      try {
        await this.fetchCategories()
        await this.fetchFAQ()
        await this.fetchProductFeatureCategories()
        await this.fetchProductFeatureTypes()
        await this.fetchProductFeatureFeatures()
      } catch(err) {
        console.error(err)
      }
      this.isBusy = false

      const vm = this
      $('#modalProductFeature').on('hidden.bs.modal', function (e) {
        vm.fetchProductFeatureFeatures()
      })
      $('#modalProductFeatureCategory').on('hidden.bs.modal', function (e) {
        vm.fetchProductFeatureCategories()
      })
    },
    data() {
      return {
        moment: moment,
        isBusy: false,
        isBusyData: false,
        errored: [],
        selectedFile: null,
        productTypes: [],
        categories: [],
        totalData: 0,
        selectedCategory: null,
        service: new ProductInformationService(),
        params: {
          Query: '',
          ProductId: '',
          SortBy: '',
          PageIndex: 1,
          PageSize: 10
        },
        paramsCategories: {
          SortBy: '',
          PageIndex: 1,
          PageSize: 100
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
        sequence: 1,
        topic : '',
        description: '',

        productFeatureCategories: [],
        selectedProductFeatureCategory: null,
        productFeatureTypes: [],
        selectedProductFeatureType: [],
        productFeatureFeatures: [],
        selectedProductFeatureFeature: [],

        cmsContent: {},
        cmsOperation: {},
        cmsSetting: {},
        cmsFmb: {},
        cmsWorkPrincipal: {},

        isSpecial: false,

        selectedFilterStatus: null,
        filterStatus: [
          {
            value: true,
            label: 'Published',
            isSelected: false
          },
          {
            value: false,
            label: 'Draft',
            isSelected: false
          }
        ]
      }
    },
    methods: {
      handleAddSub() {
        $('#modalCategoryTip').modal('show')
      },
      handleAddSubFeature() {
        $('#modalProductFeature').modal('show')
      },
      handleAddSubFeatureCategory() {
        $('#modalProductFeatureCategory').modal('show')
      },
      handleAddSubContent() {
        $('#modalCmsContent').modal('show')
      },
      handleAddSubOperation() {
        $('#modalCmsOperation').modal('show')
      },
      handleAddSubSetting() {
        $('#modalCmsSetting').modal('show')
      },
      handleAddSubFmb() {
        $('#modalCmsFmb').modal('show')
      },
      handleAddSubWorkPrincipal() {
        $('#modalCmsWorkPrincipal').modal('show')
      },
      handleCmsContent(data) {
        this.cmsContent = data
      },
      handleCmsOperation(data) {
        this.cmsOperation = data
      },
      handleCmsSetting(data) {
        this.cmsSetting = data
      },
      handleCmsFmb(data) {
        this.cmsFmb = data
      },
      handleCmsWorkPrincipal(data) {
        this.cmsWorkPrincipal = data
      },
      async fetchProductFeatureCategories() {
        try {
          const res = await this.service.getAllProductFeatureCategories({
            PageIndex: 1,
            PageSize: 100,
          })
          this.productFeatureCategories = res.productFeatureCategories
        } catch(err) {
          console.error(err)
        }
      },
      async fetchProductFeatureTypes() {
        try {
          const res = await this.service.getAllProductTypes({
            ProductId: this.id,
            PageIndex: 1,
            PageSize: 100
          })
          this.productFeatureTypes = res.productTypes
        } catch(err) {
          console.error(err)
        }
      },
      async fetchProductFeatureFeatures() {
        try {
          const res = await this.service.getAllProductFeature({
            ProductId: this.id,
            PageIndex: 1,
            PageSize: 100
          })
          this.productFeatureFeatures = res.productFeatures
        } catch(err) {
          console.error(err)
        }
      },
      reset() {
          this.success = false
          this.cmsContent = {}
          this.cmsOperation = {}
          this.cmsSetting = {}
          this.cmsFmb = {}
          this.cmsWorkPrincipal = {}
          this.selectedProductFeatureCategory = null
          this.selectedProductFeatureType = null
          this.selectedProductFeatureFeature = null
          this.selectedEditData = null
          this.isSpecial = false
          window.location.href = `/MyTools/ProductInformation`
      },
      async fetchCategories() {
        try {
          const res = await this.service.getAllTipCategory(this.paramsCategories)
          this.categories = res.productTipCategories
        } catch(err) {
          console.error(err)
        }
      },
      async fetchFAQ() {
        this.isBusyData = true
        try {
          this.params.ProductId = this.id
          this.params.ApprovedAt = this.selectedFilterStatus
          const res = await this.service.getAllProductFeatureMapping(this.params)
          this.productTypes = res.productFeatureMappings
          this.totalData = res.totalProductFeatureMapping
        } catch(err) {
          console.error(err)
        }
        this.isBusyData = false
      },
      async submit(e) {
        if(this.handleValidation()) {
          return
        }
        this.isBusy = true
        try {
          const body = {
            productId: this.id,
            productTypeId: this.selectedProductFeatureType.productTypeId,
            "productFeatureId": this.selectedProductFeatureFeature.productFeatureId,
            "cms_FmbId": null,
            "cms_WorkPrincipalId": null,
            "cms_ContentId": null,
            "cms_OperationId": null,
            "cms_SettingId": null,
            "isSpecial": this.isSpecial,
            "productFeatureCategoryId": this.selectedProductFeatureCategory.productFeatureCategoryId,
            approvedAt: e ?moment().format('YYYY-MM-DD')  : null
          }
          if(!this.selectedEditData) {
            if(this.cmsFmb.image) {
              const fmbId = await this.service.createCMSFmb({
                "cmsFmbPicContent": {
                  "base64": this.cmsFmb.image.contentType ? this.cmsFmb.image.base64 : '',
                  "fileName": this.cmsFmb.image.contentType ? this.cmsFmb.image.fileName : '',
                  "contentType": this.cmsFmb.image.contentType ? this.cmsFmb.image.contentType : ''
                },
                "cmsFmbFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": '',
                },
                "cms_FmbDescription": this.cmsFmb.description,
              })
              body.cms_FmbId = fmbId
            }
            if(this.cmsWorkPrincipal.image) {
              const workPricipalId = await this.service.createCMSWorkPrincipal({
                "cmsWorkPrincipalPicContent": {
                  "base64": this.cmsWorkPrincipal.image.contentType ? this.cmsWorkPrincipal.image.base64 : '',
                  "fileName": this.cmsWorkPrincipal.image.contentType ? this.cmsWorkPrincipal.image.fileName : '',
                  "contentType": this.cmsWorkPrincipal.image.contentType ? this.cmsWorkPrincipal.image.contentType : ''
                },
                "cmsWorkPrincipalFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": '',
                },
                "cms_WorkPrincipalDescription": this.cmsWorkPrincipal.description,
              })
              body.cms_WorkPrincipalId = workPricipalId
            }

            if(this.cmsOperation.image) {
              const operationId = await this.service.createCMSOperation({
                "cmsOperationPicContent": {
                  "base64": this.cmsOperation.image.contentType ? this.cmsOperation.image.base64 : '',
                  "fileName": this.cmsOperation.image.contentType ? this.cmsOperation.image.fileName : '',
                  "contentType": this.cmsOperation.image.contentType ? this.cmsOperation.image.contentType : ''
                },
                "cmsOperationFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": '',
                },
                "cms_OperationDescription": this.cmsOperation.description,
              })
              body.cms_OperationId = operationId
            }

            if(this.cmsSetting.image) {
              const settingId = await this.service.createCMSSetting({
                "cmsSettingPicContent": {
                  "base64": this.cmsSetting.image.contentType ? this.cmsSetting.image.base64 : '',
                  "fileName": this.cmsSetting.image.contentType ? this.cmsSetting.image.fileName : '',
                  "contentType": this.cmsSetting.image.contentType ? this.cmsSetting.image.contentType : ''
                },
                "cmsSettingFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": '',
                },
                "cms_SettingDescription": this.cmsSetting.description,
              })
              body.cms_SettingId = settingId
            }
            
            const contentId = await this.service.createCMSContent({
              "cmsContentPicContent": {
                "base64": this.cmsContent.image.contentType ? this.cmsContent.image.base64 : '',
                "fileName": this.cmsContent.image.contentType ? this.cmsContent.image.fileName : '',
                "contentType": this.cmsContent.image.contentType ? this.cmsContent.image.contentType : ''
              },
              "cmsContentFileContent": {
                "base64": this.cmsContent.imageUpload.base64,
                "fileName": this.cmsContent.imageUpload.fileName,
                "contentType": this.cmsContent.imageUpload.contentType,
              },
              "cmsContentVidContent": {
                "base64": '',
                "fileName": '',
                "contentType": '',
              },
              "cms_ContentDescription": this.cmsContent.description,
              "cms_ContentName": 'content',
              "cms_ContentCategory": 'Feature',
              "cms_ContentSequence": 0,
              "cms_ContentIsSequence": false
            })
            body.cms_ContentId = contentId
            
            await this.service.createProductFeatureMapping(body)
          } else {
            if(!this.image.base64) {
              delete body.productGalleryFileContent
            }
            body.productFeatureMappingId = this.selectedEditData.productFeatureMappingId
            body.cms_FmbId = this.selectedEditData.cms_FmbId
            body.cms_WorkPrincipalId = this.selectedEditData.cms_WorkPrincipalId
            body.cms_ContentId = this.selectedEditData.cms_ContentId
            body.cms_OperationId = this.selectedEditData.cms_OperationId
            body.cms_SettingId = this.selectedEditData.cms_SettingId

            if(this.cmsFmb.image && this.selectedEditData.cms_FmbId == null) {
              const fmbId = await this.service.createCMSFmb({
                "cmsFmbPicContent": {
                  "base64": this.cmsFmb.image.contentType ? this.cmsFmb.image.base64 : '',
                  "fileName": this.cmsFmb.image.contentType ? this.cmsFmb.image.fileName : '',
                  "contentType": this.cmsFmb.image.contentType ? this.cmsFmb.image.contentType : ''
                },
                "cmsFmbFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": '',
                },
                "cms_FmbDescription": this.cmsFmb.description,
              })
              body.cms_FmbId = fmbId
            } else if(this.selectedEditData.cms_FmbId) {
              const fmbId = await this.service.updateCMSFmb({
                "cms_FmbId": this.selectedEditData.cms_FmbId,
                "cmsFmbPicContent": {
                  "base64": this.cmsFmb.image.contentType ? this.cmsFmb.image.base64 : '',
                  "fileName": this.cmsFmb.image.contentType ? this.cmsFmb.image.fileName : '',
                  "contentType": this.cmsFmb.image.contentType ? this.cmsFmb.image.contentType : ''
                },
                "cmsFmbFileContent": {
                  "base64": '',
                  "fileName":  '',
                  "contentType": '',
                },
                "cms_FmbDescription": this.cmsFmb.description,
                isDeleteFile: true,
                isDeletePic: false
              })
            }

            if(this.cmsWorkPrincipal.image && this.selectedEditData.cms_WorkPrincipalId == null) {
              const workPricipalId = await this.service.createCMSWorkPrincipal({
                "cmsWorkPrincipalPicContent": {
                  "base64": this.cmsWorkPrincipal.image.contentType ? this.cmsWorkPrincipal.image.base64 : '',
                  "fileName": this.cmsWorkPrincipal.image.contentType ? this.cmsWorkPrincipal.image.fileName : '',
                  "contentType": this.cmsWorkPrincipal.image.contentType ? this.cmsWorkPrincipal.image.contentType : ''
                },
                "cmsWorkPrincipalFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": ''
                },
                "cms_WorkPrincipalDescription": this.cmsWorkPrincipal.description,
              })
              body.cms_WorkPrincipalId = workPricipalId
            } else if(this.selectedEditData.cms_WorkPrincipalId) {
              const workPricipalId = await this.service.updateCMSWorkPrincipal({
                "cms_WorkPrincipalId": this.selectedEditData.cms_WorkPrincipalId,
                "cmsWorkPrincipalPicContent": {
                  "base64": this.cmsWorkPrincipal.image.contentType? this.cmsWorkPrincipal.image.base64 : '',
                  "fileName": this.cmsWorkPrincipal.image.contentType? this.cmsWorkPrincipal.image.fileName : '',
                  "contentType": this.cmsWorkPrincipal.image.contentType? this.cmsWorkPrincipal.image.contentType : ''
                },
                "cmsWorkPrincipalFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": ''
                },
                "cms_WorkPrincipalDescription": this.cmsWorkPrincipal.description,
                isDeleteFile: true,
                isDeletePic: false,
              })
            }

            const contentId = await this.service.updateCMSContent({
              "cms_ContentId": this.selectedEditData.cms_ContentId,
              "cmsContentPicContent": {
                "base64": this.cmsContent.image.contentType ? this.cmsContent.image.base64 : '',
                "fileName": this.cmsContent.image.contentType ? this.cmsContent.image.fileName : '',
                "contentType": this.cmsContent.image.contentType ? this.cmsContent.image.contentType : ''
              },
              "cmsContentFileContent": {
                "base64": this.cmsContent.imageUpload.base64,
                "fileName": this.cmsContent.imageUpload.fileName,
                "contentType": this.cmsContent.imageUpload.contentType,
              },
              "cmsContentVidContent": {
                "base64": '',
                "fileName": '',
                "contentType": '',
              },
              "cms_ContentDescription": this.cmsContent.description,
              "cms_ContentName": 'content',
              "cms_ContentCategory": 'Feature',
              "cms_ContentSequence": 0,
              "cms_ContentIsSequence": false,
              isDeleteFile: this.cmsContent.imageUpload.base64 ? false : this.cmsContent.imageUpload.isDeleteFile,
              isDeleteVid: true,
              isDeletePic: false
            })
            
            if(this.cmsOperation.image && this.selectedEditData.cms_OperationId == null) {
              const operationId = await this.service.createCMSOperation({
                "cmsOperationPicContent": {
                  "base64": this.cmsOperation.image.contentType ? this.cmsOperation.image.base64 : '',
                  "fileName": this.cmsOperation.image.contentType ? this.cmsOperation.image.fileName : '',
                  "contentType": this.cmsOperation.image.contentType ? this.cmsOperation.image.contentType : ''
                },
                "cmsOperationFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": '',
                },
                "cms_OperationDescription": this.cmsOperation.description,
              })
              body.cms_OperationId = operationId
            } else if(this.selectedEditData.cms_OperationId) {
              const operationId = await this.service.updateCMSOperation({
                "cms_OperationId": this.selectedEditData.cms_OperationId,
                "cmsOperationPicContent": {
                  "base64": this.cmsOperation.image.contentType ? this.cmsOperation.image.base64 : '',
                  "fileName": this.cmsOperation.image.contentType ? this.cmsOperation.image.fileName : '',
                  "contentType": this.cmsOperation.image.contentType ? this.cmsOperation.image.contentType : ''
                },
                "cmsOperationFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": '',
                },
                "cms_OperationDescription": this.cmsOperation.description,
                isDeleteFile: true,
                isDeletePic: false,
              })
            }
            
            if(this.cmsSetting.image && this.selectedEditData.cms_SettingId == null) {
              const settingId = await this.service.createCMSSetting({
                "cmsSettingPicContent": {
                  "base64": this.cmsOperation.image.contentType ? this.cmsSetting.image.base64 : '',
                  "fileName": this.cmsOperation.image.contentType ? this.cmsSetting.image.fileName : '',
                  "contentType": this.cmsOperation.image.contentType ? this.cmsSetting.image.contentType : ''
                },
                "cmsSettingFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": '',
                },
                "cms_SettingDescription": this.cmsSetting.description,
              })
              body.cms_SettingId = settingId
            } else if(this.selectedEditData.cms_SettingId) {
              const settingId = await this.service.updateCMSSetting({
                "cms_SettingId": this.selectedEditData.cms_SettingId,
                "cmsSettingPicContent": {
                  "base64": this.cmsSetting.image.contentType ? this.cmsSetting.image.base64 : '',
                  "fileName": this.cmsSetting.image.contentType ? this.cmsSetting.image.fileName : '',
                  "contentType": this.cmsSetting.image.contentType ? this.cmsSetting.image.contentType : ''
                },
                "cmsSettingFileContent": {
                  "base64": '',
                  "fileName": '',
                  "contentType": '',
                },
                "cms_SettingDescription": this.cmsSetting.description,
                isDeleteFile: true,
                isDeletePic: false
              })
            }
            
            await this.service.updateProductFeatureMapping(body)
          }
          this.params.PageIndex = 1
          this.fetchFAQ()
          this.success = true
          this.cmsContent = {}
          this.cmsOperation = {}
          this.cmsSetting = {}
          this.cmsFmb = {}
          this.cmsWorkPrincipal = {}
          this.selectedProductFeatureCategory = null
          this.selectedProductFeatureType = null
          this.selectedProductFeatureFeature = null
          this.selectedEditData = null
          this.isSpecial = false
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleValidation () {
        this.errored = []
        this.success = false
        if(!this.selectedProductFeatureCategory) {
          this.errored.push({
            key: 'category',
            value: 'Please fill category field'
          })
        }
        if(!this.selectedProductFeatureFeature) {
          this.errored.push({
            key: 'feature',
            value: 'Please fill feature field'
          })
        }
        if(!this.selectedProductFeatureType) {
          this.errored.push({
            key: 'type',
            value: 'Please fill type field'
          })
        }
        if(!this.cmsContent.image) {
          this.errored.push({
            key: 'content',
            value: 'Please fill content field'
          })
        }
        return this.errored.length > 0
      },
      handleFilter() {
        this.params.PageIndex = 1
        this.fetchFAQ()
      },
      handleResetFilter() {
        this.selectedFilterStatus = null
        this.params.Query = ''
        this.fetchFAQ()
      },
      hideDropdown () {
        $('#dropdownMenuButton').dropdown('hide')
      },
      async handleDelete() {
        this.isBusy = true
        try {
          await this.service.deleteProductFeatureMapping(this.selectedData.productFeatureMappingId)
          this.fetchFAQ()
          this.success = true
          this.cmsContent = {}
          this.cmsOperation = {}
          this.cmsSetting = {}
          this.cmsFmb = {}
          this.cmsWorkPrincipal = {}
          this.selectedProductFeatureCategory = null
          this.selectedProductFeatureType = null
          this.selectedProductFeatureFeature = null
          this.selectedEditData = null
          this.isSpecial = false
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(data) {
        this.selectedEditData = data
        this.isSpecial = data.isSpecial
        this.selectedProductFeatureCategory = {
          productFeatureCategoryId: data.productFeatureCategoryId,
          productFeatureCategoryName: data.productFeatureCategoryName
        }
        this.selectedProductFeatureType = {
          productTypeId: data.productTypeId,
          productTypeName: data.productTypeName
        }
        this.selectedProductFeatureFeature = {
          productFeatureId: data.productFeatureId,
          productFeatureName: data.productFeatureName
        }
        if(data.cms_FmbId) {
          this.cmsFmb = {
            selectedFile: data.cms_Fmb.blobUrl ? data.cms_Fmb.blobUrl : data.cms_Fmb.fileBlobUrl,
            selectedFileUpload: data.cms_Fmb.fileBlobUrl,
            image: {
              base64: null,
              fileName: data.cms_Fmb.blobUrl ? data.cms_Fmb.blob.name : data.cms_Fmb.fileBlob.name,
              contentType: data.cms_Fmb.blobUrl ? data.cms_Fmb.blob.mime : data.cms_Fmb.fileBlob.mime,
            },
            imageUpload: {
              base64: null,
              fileName: data.cms_Fmb.fileBlob.name,
              contentType: data.cms_Fmb.fileBlob.mime,
            },
            description: data.cms_Fmb.cms_FmbDescription
          }
        }
        if(data.cms_WorkPrincipalId) {
          this.cmsWorkPrincipal = {
            selectedFile: data.cms_WorkPrincipal.blobUrl ? data.cms_WorkPrincipal.blobUrl : data.cms_WorkPrincipal.fileBlobUrl,
            selectedFileUpload: data.cms_WorkPrincipal.fileBlobUrl,
            image: {
              base64: null,
              fileName: data.cms_WorkPrincipal.blobUrl ? data.cms_WorkPrincipal.blob.name : data.cms_WorkPrincipal.fileBlob.name,
              contentType: data.cms_WorkPrincipal.blobUrl ? data.cms_WorkPrincipal.blob.mime : data.cms_WorkPrincipal.fileBlob.mime,
            },
            imageUpload: {
              base64: null,
              fileName: data.cms_WorkPrincipal.fileBlob.name,
              contentType: data.cms_WorkPrincipal.fileBlob.mime,
            },
            description: data.cms_WorkPrincipal.cms_WorkPrincipalDescription
          }
        }
        this.cmsContent = {
          selectedFile: data.cms_Content.blobUrl ? data.cms_Content.blobUrl : data.cms_Content.videoBlobUrl,
          selectedFileUpload: data.cms_Content.fileBlobUrl,
          image: {
            base64: null,
            fileName: data.cms_Content.blobId ? data.cms_Content.blob.name : data.cms_Content.videoBlob.name,
            contentType:  data.cms_Content.blobId ? data.cms_Content.blob.mime : data.cms_Content.videoBlob.mime,
          },
          imageUpload: {
            base64: null,
            fileName: data.cms_Content.fileBlob.name,
            contentType: data.cms_Content.fileBlob.mime,
          },
          description: data.cms_Content.cms_ContentDescription
        }
        if(data.cms_OperationId) {
          this.cmsOperation = {
            selectedFile: data.cms_Operation.blobUrl ? data.cms_Operation.blobUrl : data.cms_Operation.fileBlobUrl,
            selectedFileUpload: data.cms_Operation.fileBlobUrl,
            image: {
              base64: null,
              fileName: data.cms_Operation.blobUrl ? data.cms_Operation.blob.name : data.cms_Operation.fileBlob.name,
              contentType: data.cms_Operation.blobUrl ? data.cms_Operation.blob.mime : data.cms_Operation.fileBlob.mime,
            },
            imageUpload: {
              base64: null,
              fileName: data.cms_Operation.fileBlob.name,
              contentType: data.cms_Operation.fileBlob.mime,
            },
            description: data.cms_Operation.cms_OperationDescription
          }
        }
        if(data.cms_SettingId) {
          this.cmsSetting = {
            selectedFile: data.cms_Setting.blobUrl ? data.cms_Setting.blobUrl : data.cms_Setting.fileBlobUrl,
            selectedFileUpload: data.cms_Setting.fileBlobUrl,
            image: {
              base64: null,
              fileName: data.cms_Setting.blobUrl ? data.cms_Setting.blob.name : data.cms_Setting.fileBlob.name,
              contentType: data.cms_Setting.blobUrl ? data.cms_Setting.blob.mime : data.cms_Setting.fileBlob.mime,
            },
            imageUpload: {
              base64: null,
              fileName: data.cms_Setting.fileBlob.name,
              contentType: data.cms_Setting.fileBlob.mime,
            },
            description: data.cms_Setting.cms_SettingDescription
          }
        }
      },
      async handleEditStatus(data) {
        this.isBusy = true
        console.log(data)
        await this.service.updateProductFeatureMapping({
          productFeatureMappingId: data.productFeatureMappingId,
          productId: this.id,
          productTypeId: data.productTypeId,
          "productFeatureId": data.productFeatureId,
          "cms_FmbId": data.cms_FmbId,
          "cms_WorkPrincipalId": data.cms_WorkPrincipalId,
          "cms_ContentId": data.cms_ContentId,
          "cms_OperationId": data.cms_OperationId,
          "cms_SettingId": data.cms_SettingId,
          "isSpecial": data.isSpecial,
          "productFeatureCategoryId": data.productFeatureCategoryId,
          approvedAt: data.approvedAt ? null : moment().format('YYYY-MM-DD')
        })
        this.fetchFAQ()
        this.isBusy = false
      },
      async handleChangeStatus(data) {
        this.isBusy = true
        await this.service.updateApprovalProductFeatureMapping({
          "productFeatureMappingId": data.productFeatureMappingId,
          "isUpdateProductFeatureMappingApproval": true
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
      },
      handlePreventMenuClose(e) {
        e.stopPropagation();
      },
      handleSelectedFilterStatus(data) {
        const value = data.value == this.selectedFilterStatus ? null : data.value
        this.selectedFilterStatus = value
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