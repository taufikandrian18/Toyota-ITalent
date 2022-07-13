<template>
  <div class="col-md-12 p-0 w-100">
    <!--ADVANCE SEARCH-->
    <div v-if="isAdd == false" class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
        <h4>Search Area Specialist</h4>
        <br />
        <!--3 Column Menu-->
        <div class="row">
            <div class="col-md-6">
                <div class="row align-items-center">
                    <div class="col-md-3">
                        <span>Name</span>
                    </div>
                    <div class="col-md-9">
                        <div class="input-group">
                            <input v-model="params.AreaSpecialistName" class="form-control" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <br />

        <!--Search Button-->
        <div>
            <div class="row">
                <div class="col-md-12">
                    <div class="d-flex align-items-end flex-column">
                        <div>
                            <button class="btn talent_bg_blue talent_font_white" @click.prevent="fetchProductTypes()">
                                Search
                            </button>
                            <button class="btn talent_bg_red talent_font_white" @click.prevent="handleResetFilter()">
                                Reset
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <br>

    <div class="col-md-12 mt-3 p-0">
      <!-- <kodawari-menus /> -->
      <div class="row talent_bg_white talent_padding talent_bg_shadow m-0">
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
                <h5>Add New Area Specialist</h5>
              </div>
              <div class="col-md-6">
                <div class="row">
                  <div class="col-md-12">
                    <label>Area Specialist Name<span class="talent_font_red"> *</span></label>
                    <div class="input-group">
                        <input name="name" type="text" class="form-control" v-model="name" :disabled="selectedEditData && !isEdit"/>
                    </div>
                  </div>
                  <div class="col-md-12 mt-4">
                    <div class="row align-items-center">
                        <div class="col-md-12">
                            <span>Outlet</span>
                        </div>
                        <div class="col-md-12 mt-2">
                          <div class="input-group">
                              <multiselect 
                                v-model="selectedOutlet"
                                :options="outlets"
                                label="name"
                                v-validate="'required'"
                                track-by="outletId"
                                :searchable="true"
                                :multiple="true"
                                :disabled="selectedEditData && !isEdit"
                              />
                          </div>
                        </div>
                    </div>
                  </div>
                </div>
              </div>
              <div class="col-md-12 d-flex justify-content-end mt-4">
                <button class="btn btn-danger mr-2" @click="reset">
                    Close
                </button>
                <button class="btn btn-success mr-2" @click="submit(null)" v-if="(selectedEditData && isEdit) || (!selectedEditData && !isEdit)">
                    {{!selectedEditData ? 'Save' : 'Save'}}
                </button>
                <!-- <button class="btn btn-primary" @click="submit(1)">
                    Submit
                </button> -->
              </div>
              <div class="col-md-12 py-2">
                <hr>
              </div>
            </div>
          </div>
          <!-- table data -->
          <div class="col-md-12" v-else>

            <h4>Area Specialist</h4>
            <!-- filter -->
            <div class="col-md-12 talent_magin_small mt-3">
                <div class="row w-100 align-items-center justify-content-between">
                    <div class="col d-flex py-0 pl-0 pr-4">
                        <div class="col-md-12 talent_magin_small">
                            <div class="row align-items-center row justify-content-between">
                                <a v-if="productTypes">Showing {{productTypes.length}} of {{totalData}} Entry(s)</a>
                            </div>
                        </div>
                        <!-- <div class="dropdown">
                          <button class="btn btn-info mr-2" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-expanded="false">
                            Filter
                          </button>

                          <div class="dropdown-menu p-3" aria-labelledby="dropdownMenuButton" style="min-width: 500px" @click="handlePreventMenuClose">
                            <b>Filter</b>
                            <div class="row">
                              <div class="col mt-4">
                                <div class="input-group talent_front">
                                  <label for="">Created Date</label>
                                    <v-date-picker class="v-date-style-report"
                                                    v-model="params.CreatedAt"
                                                    :firstDayOfWeek="2"
                                                    :value="null"
                                                    :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                    <div class="input-group-append">
                                        <span class="input-group-text">
                                            <fa icon="calendar-alt"></fa>
                                        </span>
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
                            <input class="form-control" placeholder="Search By Title" v-model="params.Query"/>
                        </div> -->
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
                                  Name
                                </a>
                            </th>
                            <th scope="col" class="text-center">
                                <a href="#" class="talent_sort" style="color: white;">
                                  Created By
                                </a>
                            </th>
                            <th scope="col" class="text-center">
                                <a href="#" class="talent_sort" style="color: white;">
                                  Created At
                                </a>
                            </th>
                            <th scope="col" class="text-center" style="max-width: 150px">
                                <a href="#" class="talent_sort" style="color: white;">
                                  Updated At
                                </a>
                            </th>
                            <th scope="col" class="text-center" colspan="3">
                                <a href="#" class="talent_sort" style="color: white;">
                                  Action
                                </a>
                            </th>
                            <!-- <th scope="col" class="text-center" colspan="1">
                                <a href="#" class="talent_sort" style="color: white;">
                                  Status
                                </a>
                            </th> -->
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="dictionary in productTypes" :key="dictionary.hoGuidelineId">
                            <td>
                                {{dictionary.areaSpecialistName}}
                            </td>
                            <td>
                                {{dictionary.createdBy}}
                            </td>
                            <td>
                                {{moment(dictionary.createdAt).format('DD/MM/YYYY')}}
                            </td>
                            <td>
                                {{moment(dictionary.updatedAt).format('DD/MM/YYYY')}}
                            </td>
                            <!-- <td style="max-width: 150px;white-space: normal">
                                {{moment(dictionary.createdAt).format('DD/MM/YYYY')}}
                            </td> -->
                              <!-- <td>
                                  <button
                                    :class="`btn btn-block ${dictionary.approvedAt ? 'talent_bg_green talent_font_white' : 'talent_bg_red talent_font_white'}`" 
                                    @click="handleEditStatus(dictionary)">
                                      <fa :icon="dictionary.approvedAt ? 'eye' : 'eye-slash'"></fa>
                                  </button>
                              </td> -->
                            <td class="talent_nopadding_important">
                                <button  class="btn btn-block talent_bg_orange talent_font_white" @click="handleEdit(dictionary, false)">View</button>
                            </td>
                            <td class="talent_nopadding_important">
                                <button  class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(dictionary, true)">Edit</button>
                            </td>
                            <!-- <td>
                              <div :class="getClass(dictionary.hoGuidelineStatus)">
                                {{dictionary.hoGuidelineStatus}}
                              </div>
                            </td> -->
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
      </div>
    </div>
  </div>
</template>


<script>
import { AssessmentService } from '../../services/Assessment/AssesmentService';
import { ReleaseTrainingService } from '../../services/NSwagService'
import $ from 'jquery'
import moment from 'moment'
import ModalDelete from '../shared/ModalDelete.vue';

export default {
  components: { ModalDelete },
    async mounted() {
      $('.editr--toolbar .dashboard label').click(function (event) {$(event.target).focus()});
      this.isBusy = true
        try {
          await this.fetchProductTypes()
          const res = await this.swagService.getAllOutlet();
          this.outlets = res.filter(v => v.areaSpecialistId === null)
        } catch(err) {
          console.error(err)
        }
        this.isBusy = false
    },
    data() {
      return {
        swagService: new ReleaseTrainingService(),
        outlets: [],
        selectedOutlet: null,
        moment: moment,
        isBusy: false,
        errored: [],
        selectedFile: null,
        productTypes: [],
        totalData: 0,
        service: new AssessmentService(),
        params: {
          Query: "",
          SortKey: '',
          SortType: '',
          PageIndex: 1,
          PageSize: 10,
          CreatedAt: null,
          ScoringMethod: null,
          CreatedBy: "",
          DateFilter: {
              end: null,
              start: null,
          },
          AreaSpecialistName: ''
        },
        name: '',
        minimumScore: '',
        description: '',
        isQuestion: false,
        salesTalk: '',
        image: {
          base64: '',
          fileName: '',
          contentType: ''
        },
        success: false,
        selectedData: null,
        selectedEditData: null,
        isAdd: false,
        isEdit: false,

        questions: [
          {
            questionNumber: '',
            assesmentCode: ''
          }
        ],
        scoringMethod: [
          {
            key: 'dropdown',
            value: 'Button'
          },
          {
            key: 'slider',
            value: 'Slider'
          },
          {
            key: 'textfield',
            value: 'Textfield'
          },
        ],
        selectedScoringMethod: 'dropdown',
        scoringConfig: [
          {
            title: '',
            value: '',
            description: ''
          }
        ],
        defaultConfig: {
          title: '',
          value: '',
          description: '',
        }
      }
    },
    methods: {
      handlePreventMenuClose(e) {
        e.stopPropagation();
      },
      getClass(status) {
        switch (status) {
          case 'Approved':
            return 'approved'
          case 'Awaiting Approval':
            return 'awaiting-approval'
          case 'Rejected':
            return 'rejected'
            default: 
            return 'awaiting-approval'
        }
      },
      // config
      addMoreConfig() {
        this.scoringConfig.push({...this.defaultConfig})
      },
      removeConfig(index) {
        this.scoringConfig.splice(index, 1)
      },
      //
      resetsearch() {
      },
      reset() {
        this.success = false
        this.isAdd = false
        this.isEdit = false
        this.name = ''
        this.salesTalk = ''
        this.selectedFile = null
        this.image = {
          base64: '',
          fileName: '',
          contentType: ''
        }
        this.selectedEditData = null,
        this.selectedScoringMethod = 'dropdown'
        this.minimumScore = ''
        this.description = ''
        this.scoringConfig = [{
          title: '',
          value: '',
          description: '',
        }]
        this.isAdd = false
        this.selectedOutlet = []
      },
      async fetchProductTypes() {
        this.isBusy = true
        try {
          const res = await this.service.getAllAreaSpecialist({
            ...this.params
          })
          this.productTypes = res.areaSpecialists
          this.totalData = res.totalAreaSpecialists
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
            "areaSpecialistName": this.name,
            "outletId": this.selectedOutlet.map(v => v.outletId)
          }
          if(!this.selectedEditData) {
            await this.service.createAreaSpecialist(body)
          } else {
            body.areaSpecialistId = this.selectedEditData.areaSpecialistId
            await this.service.updateAreaSpecialist(body)
          }
          this.selectedOutlet = []
          this.params.PageIndex = 1
          this.fetchProductTypes()
          const res = await this.swagService.getAllOutlet();
          this.outlets = res.filter(v => v.areaSpecialistId === null)
          this.success = true
          this.isAdd = false
          this.isEdit = false
          this.name = ''
          this.salesTalk = ''
          this.selectedFile = null
          this.image = {
            base64: '',
            fileName: '',
            contentType: ''
          }
          this.selectedEditData = null
          this.selectedScoringMethod = 'dropdown'
          this.minimumScore = ''
          this.description = ''
          this.scoringConfig = [{
          title: '',
          value: '',
          description: '',
        }]
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
            key: 'name',
            value: 'Please fill Area Specialist Name'
          })
        }
        return this.errored.length > 0
      },
      handleFilter() {
        this.params.PageIndex = 1
        this.params.CreatedAt = this.params.CreatedAt != null ? moment(this.params.CreatedAt).format('YYYY-MM-DD') : null
        this.fetchProductTypes()
        this.hideDropdown()
      },
      handleResetFilter() {
        this.params = {
          Query: "",
          SortKey: '',
          SortType: '',
          PageIndex: 1,
          PageSize: 10,
          CreatedAt: null,
          DateFilter: {
            start: null,
            end: null,
          },
          CreatedAt: null,
          ScoringMethod: null,
          CreatedBy: "",
        }
        this.fetchProductTypes()
        this.hideDropdown()
      },
      hideDropdown () {
        $('#dropdownMenuButton').dropdown('hide')
      },
      async handleDelete() {
        this.isBusy = true
        try {
          await this.service.deleteAreaSpecialist(this.selectedData.areaSpecialistId)
          this.fetchProductTypes()
          const res = await this.swagService.getAllOutlet();
          this.outlets = res.filter(v => v.areaSpecialistId === null)
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
          this.selectedScoringMethod = 'dropdown'
          this.minimumScore = ''
          this.description = ''
          this.scoringConfig = [{
          title: '',
          value: '',
          description: '',
        }]
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(data, isEdit) {
        this.success = false
        this.selectedEditData = data
        this.name = data.areaSpecialistName
        this.selectedOutlet = data.outlets
        // this.description = data.description
        // this.minimumScore = data.minimumScore
        // this.selectedScoringMethod = data.enumFeedbackScore
        // this.scoringConfig = data.scoringConfig.map(v => ({
        //   ...v,
        //   title: v.text,
        //   value: v.point
        // }))
        // this.image.contentType = data.mediaBlobData.mime
        // this.selectedFile = data.mediaUrl
        this.isAdd = true
        this.isEdit = isEdit
      },
      async handleEditStatus(data) {
        this.isBusy = true
        await this.service.updateHOUploadGuideline({
            hoGuidelineId: data.hoGuidelineId,
            hoGuidelineTitle: data.hoGuidelineTitle,
            mediaContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            }
        })
        this.fetchProductTypes()
      },
      async handleChangeFile(e) {
        const selectedImage = e.target.files[0]
        if(selectedImage.size > 104857600) {
            this.image.base64 = ''
            this.image.contentType = ''
            this.image.fileName = ''
            return
        }
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
  top: 0;
  display: flex;
  justify-content: end;
  width: 100%;
  padding: 16px 16px;
}
.approved {
  background: #60D408 0% 0% no-repeat padding-box;
  border-radius: 27px;
  color: white;
  padding: 6px 24px;
  text-align: center;
}
.awaiting-approval {
  background: #EEA944 0% 0% no-repeat padding-box;
  border-radius: 27px;
  color: white;
  padding: 6px 24px;
  text-align: center;
}
.rejected {
  background: #EC182E 0% 0% no-repeat padding-box;
  border-radius: 27px;
  color: white;
  padding: 6px 24px;
  text-align: center;
}
</style>