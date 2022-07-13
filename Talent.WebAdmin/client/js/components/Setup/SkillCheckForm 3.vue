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
          <h5>Add New Skill Check</h5>
        </div>
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <label>Skill Check Name</label>
              <div class="input-group">
                  <input name="name" type="text" class="form-control" v-model="name"/>
              </div>
            </div>
            <div class="col-md-12 mt-4">
              <div class="input-group talent_front">
                <label for="" class="w-100">Media</label>
                <div class="d-flex align-items-center w-100">
                  <div class="container-upload">
                    <img v-if="!['mp4', 'pdf'].includes(image.contentType)" class="h-100 w-100" :src="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                    <video v-if="image.contentType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedFile ? selectedFile : `/upload-image2.png`"></video>
                    <object v-if="image.contentType === 'pdf'" class="h-100 w-100 min-heigth-full" type="application/pdf" width="100%" height="100%" :data="selectedFile ? selectedFile : `/upload-image2.png`" for="customFile" />
                    <!-- <div class="btn-upload"> -->
                      <button class="btn btn-primary" style="position: absolute; right: 50%; left: 50%; transform: translate(-50%, 0); bottom: 16px; height: auto">
                        <label for="input-manfaat" style="margin: 0px">Browse</label>
                      </button>
                    <!-- </div> -->
                      <input type="file" name="input-manfaat" id="input-manfaat" style="display: none;" @change="handleChangeFile" accept=".png,.jpg,.jpeg,.pdf,.mp4">
                  </div>
                </div>
              </div>
            </div>
            <!-- <div class="col-md-12">
              <checkbox 
                class="talent-checkbox-lineheight"
                id="is_competitor"
                v-model="isQuestion">Question not included</checkbox>
            </div> -->
          </div>
        </div>
        <div class="col-md-6">
          <div class="row">
            <div class="col-md-12">
              <label>Minimum Score</label>
              <div class="input-group">
                  <input name="name" type="number" class="form-control" v-model="minimumScore"/>
              </div>
            </div>
            <div class="col-md-12 mt-4">
              <label>Question Description</label>
              <div class="input-group">
                  <textarea name="name" class="form-control" v-model="description"></textarea>
              </div>
            </div>
          </div>
        </div>
        <!-- <div class="col-md-12 mt-4">
          <div class="row justify-content-end">
            <button class="btn talent_bg_cyan talent_font_white" @click="submit(null)">
                Add Assessment Code
            </button>
          </div>
          <div class="row align-items-end">
            <div class="col-md-2">
              <label>Question Number</label>
              <div class="input-group">
                  <input name="name" type="number" class="form-control" v-model="minimumScore"/>
              </div>
            </div>
            <div class="col-md-10">
              <div class="row align-items-end">
                <div class="col">
                  <label>Assessment Code</label>
                  <div class="input-group">
                      <input name="name" type="text" class="form-control" v-model="minimumScore"/>
                  </div>
                </div>
                <div>
                  <button class="btn talent_bg_red talent_font_white" @click="submit(null)">
                      Remove
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div> -->
        <div class="col-md-12 mt-4">
          <div class="row ml-2">
            Scoring Method
          </div>
          <div class="row align-items-end ml-2 mt-2">
            <div class="col-md-4 d-flex align-items-center">
              <div class="input-group mr-4" v-for="method in scoringMethod" :key="method.key">
                  <input :id="`scoring-method-${method.key}`" name="scoring-method" type="radio" class="form-check-input mr-1" v-model="selectedScoringMethod" :value="method.key"/>
                  <label :for="`scoring-method-${method.key}`">{{ method.value }}</label>
              </div>
            </div>
          </div>
        </div>
        <div class="col-md-12 mt-4">
          <div class="row align-items-end mt-2" v-for="(config, index) in scoringConfig" :key="index">
            <div class="col-md-2">
              <label>Title</label>
              <div class="input-group">
                  <input name="name" type="text" class="form-control" v-model="config.title"/>
              </div>
            </div>
            <div class="col-md-3">
              <div class="row align-items-end">
                <div class="col">
                  <label>Value</label>
                  <div class="input-group">
                      <input name="name" type="text" class="form-control" v-model="config.value"/>
                  </div>
                </div>
                <div>
                  <button class="btn" @click="removeConfig(index)" v-if="!(index === 0 && scoringConfig.length == 1)">
                      <fa icon="trash"></fa>
                  </button>
                </div>
              </div>
            </div>
          </div>
          <div class="row mt-2">
            <div class="col-md-5 d-flex justify-content-end" style="padding-right: 0px">
              <button class="btn" @click="addMoreConfig">
                  <fa icon="plus"></fa> Add More
              </button>
            </div>
          </div>
        </div>
        <div class="col-md-12 d-flex justify-content-end mt-4">
          <button class="btn btn-danger mr-2" @click="reset">
              Close
          </button>
          <button class="btn btn-success mr-2" @click="submit(null)">
              {{!selectedEditData ? 'Save' : 'update'}}
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
                      <input class="form-control" placeholder="Search By Title" v-model="params.Query"/>
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
                            Name
                          </a>
                      </th>
                      <!-- <th scope="col" class="text-center">
                          <a href="#" class="talent_sort" style="color: white;">
                            Created By
                          </a>
                      </th> -->
                      <!-- <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Created At
                          </a>
                      </th>
                      <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Updated At
                          </a>
                      </th> -->
                      <!-- <th scope="col" class="text-center" style="max-width: 150px">
                          <a href="#" class="talent_sort" style="color: white;">
                            Published
                          </a>
                      </th> -->
                      <th scope="col" class="text-center" colspan="2">
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
                          {{dictionary.title}}
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
                          <button  class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(dictionary)">Edit</button>
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
          <paginate :currentPage.sync="params.Page" :total-data="totalData" :limit-data="10" @update:currentPage="fetchProductTypes()"></paginate>
      </div>
    </div>


    <modal-delete
      name="modalDelete"
      @delete="handleDelete"
    />
  </div>
</template>


<script>
import { AssessmentService } from '../../services/Assessment/AssesmentService';
import $ from 'jquery'
import moment from 'moment'
import ModalDelete from '../shared/ModalDelete.vue';

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
        service: new AssessmentService(),
        params: {
          Query: "",
          SortBy: '',
          Page: 1,
          Limit: 10
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

        questions: [
          {
            questionNumber: '',
            assesmentCode: ''
          }
        ],
        scoringMethod: [
          {
            key: 'dropdown',
            value: 'Dropdown'
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
            value: ''
          }
        ],
        defaultConfig: {
          title: '',
          value: ''
        }
      }
    },
    methods: {
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
      reset() {
        this.success = false
        this.isAdd = false
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
        this.scoringConfig = [...this.defaultConfig]
        this.isAdd = false
      },
      async fetchProductTypes() {
        this.isBusy = true
        try {
          const res = await this.service.getAllSkillCheck(this.params)
          this.productTypes = res.data
          this.totalData = res.count
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
            mediaContent: {
              base64: this.image.base64,
              fileName: this.image.fileName,
              contentType: this.image.contentType
            },
            "guid": "",
            "title": this.name,
            "name": this.name,
            "isQuestion": false,
            "minimumScore": Number(this.minimumScore),
            "enumFeedbackScore": this.selectedScoringMethod,
            "description": this.description,
            "mediaBlobId": "",
            "scoringConfig": [
              ...this.scoringConfig.map(v => (
              {
                "guid": "",
                "skillCheckGUID": "",
                "text": v.title,
                "point": Number(v.value),
                "score": Number(v.value)
              }))
            ]
          }
          if(!this.selectedEditData) {
            await this.service.createSkillCheck(body)
          } else {
            body.guid = this.selectedEditData.guid
            body.scoringConfig = this.scoringConfig.map(v => ({
              ...v,
              "guid": v.guid ? v.guid : null,
              "text": v.title,
              "point": Number(v.value),
              "score": Number(v.value)
            }))
            body.mediaBlobId = this.selectedEditData.mediaBlobId
            await this.service.updateSkillCheck(body)
          }
          this.params.Page = 1
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
          this.selectedScoringMethod = 'dropdown'
          this.minimumScore = ''
          this.description = ''
          this.scoringConfig = [...this.defaultConfig]
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
            value: 'Please fill Skill Check Name field'
          })
        }
        if(!this.minimumScore) {
          this.errored.push({
            key: 'minimum',
            value: 'Please fill Minimum Score field'
          })
        }
        if(!this.description) {
          this.errored.push({
            key: 'description',
            value: 'Please fill Question Description field'
          })
        }
        let isAllConfigFilled = true
        try {
          this.scoringConfig.forEach(v => {
            if(!v.title || !v.value) {
              console.log('called')
              isAllConfigFilled = false;
              throw BreakException
            }
          })
        } catch(err) {
          isAllConfigFilled = false;
          if(!isAllConfigFilled) {
            this.errored.push({
              key: 'config',
              value: 'Please fill all Scoring Config field'
            })
          }
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
        this.params.Page = 1
        this.fetchProductTypes()
      },
      async handleDelete() {
        this.isBusy = true
        try {
          await this.service.deleteSkillCheck(this.selectedData.guid)
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
          this.selectedScoringMethod = 'dropdown'
          this.minimumScore = ''
          this.description = ''
          this.scoringConfig = [...this.defaultConfig]
        } catch (err) {
          console.error(err)
        }
        this.isBusy = false
      },
      handleEdit(data) {
        this.success = false
        this.selectedEditData = data
        this.name = data.title
        this.description = data.description
        this.minimumScore = data.minimumScore
        this.selectedScoringMethod = data.enumFeedbackScore
        this.scoringConfig = data.scoringConfig.map(v => ({
          ...v,
          title: v.text,
          value: v.point
        }))
        this.image.contentType = data.mediaBlobData.mime
        this.selectedFile = data.mediaUrl
        this.isAdd = true
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