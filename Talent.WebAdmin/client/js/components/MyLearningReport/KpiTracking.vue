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
    <div class="col-md-12 p-0">
      <div class="row w-100">
        <div class="col-md-12">
          <h5>Search Learning KPI Report</h5>
        </div>

        <div class="col-md-12 mt-4">
          <div class="row">
              <div class="col-md-6 mt-4">
                  <div class="row align-items-center">
                      <div class="col-md-12">
                          <span>Program Type <span class="talent_font_red">*</span></span>
                      </div>
                      <div class="col-md-12 mt-2">
                        <div class="input-group">
                            <multiselect 
                              v-model="selectedProgramTypes"
                              :options="programTypes"
                              label="programTypeName"
                              v-validate="'required'"
                              :searchable="true"
                              @select="handleChangeProgramType"
                            />
                        </div>
                      </div>
                  </div>
              </div>
              <div class="col-md-6 mt-4">
                  <div class="row align-items-center">
                      <div class="col-md-12">
                          <span>Course <span class="talent_font_red">*</span> </span>
                      </div>
                      <div class="col-md-12 mt-2">
                        <div class="input-group">
                            <multiselect 
                              v-model="selectedTraining"
                              :options="trainings"
                              label="courseName"
                              v-validate="'required'"
                              :searchable="true"
                              @search-change="trainingSearch"
                              :disabled="!selectedProgramTypes"
                            />
                        </div>
                      </div>
                  </div>
              </div>
              <div class="col-md-6 mt-4">
                  <div class="row align-items-center">
                      <div class="col-md-12">
                          <span>Area</span>
                      </div>
                      <div class="col-md-12 mt-2">
                        <div class="input-group">
                            <multiselect 
                              v-model="selectedArea"
                              :options="areas"
                              label="name"
                              v-validate="'required'"
                              :searchable="true"
                              @select="handleChangeArea"
                              @remove="handleRemoveArea"
                              :disabled="claims.DealerId"
                            />
                        </div>
                      </div>
                  </div>
              </div>
              <div class="col-md-6 mt-4">
                  <div class="row align-items-center">
                      <div class="col-md-12">
                          <span>Area Specialist</span>
                      </div>
                      <div class="col-md-12 mt-2">
                        <div class="input-group">
                            <multiselect 
                              v-model="selectedAreaSpecialist"
                              :options="areaSpecialists"
                              label="areaSpecialistName"
                              v-validate="'required'"
                              :searchable="true"
                              :disabled="claims.DealerId"
                            />
                        </div>
                      </div>
                  </div>
              </div>
              <div class="col-md-6 mt-4">
                  <div class="row align-items-center">
                      <div class="col-md-12">
                          <span>Dealer</span>
                      </div>
                      <div class="col-md-12 mt-2">
                        <div class="input-group">
                            <multiselect 
                              v-model="selectedDealer"
                              :options="filteredDealers"
                              label="dealerName"
                              v-validate="'required'"
                              :searchable="true"
                              @select="handleChangeDealer"
                              @remove="handleRemoveDealer"
                              :disabled="claims.DealerId"
                            />
                        </div>
                      </div>
                  </div>
              </div>
              <div class="col-md-6 mt-4">
                  <div class="row align-items-center">
                      <div class="col-md-12">
                          <span>Outlet</span>
                      </div>
                      <div class="col-md-12 mt-2">
                        <div class="input-group">
                            <multiselect 
                              v-model="selectedOutlet"
                              :options="filteredOutlets"
                              label="name"
                              v-validate="'required'"
                              :searchable="true"
                            />
                        </div>
                      </div>
                  </div>
              </div>
              <!-- <div class="col-md-6 mt-4">
                  <div class="row align-items-center">
                    <div class="col-md-12">
                        <span>Username SSO</span>
                    </div>
                    <div class="col-md-12 mt-2">
                        <div class="input-group">
                            <multiselect 
                              v-model="selectedEmployee"
                              :options="employees"
                              label="name"
                              v-validate="'required'"
                              :searchable="true"
                              @search-change="employeeSearch"
                            />
                        </div>
                    </div>
                  </div>
              </div>
              <div class="col-md-6 mt-4">
                  <div class="row align-items-center">
                    <div class="col-md-12">
                        <span>Employee Name</span>
                    </div>
                    <div class="col-md-12 mt-2">
                      <div class="input-group">
                        <input name="name" type="text" class="form-control" v-model="name"/>
                      </div>
                    </div>
                  </div>
              </div> -->
              <!-- <div class="col-md-12 mt-4">
                  <div class="row align-items-center">
                    <div class="col-md-12">
                        <span>Employee</span>
                    </div>
                    <div class="col-md-12 mt-2">
                      <div class="input-group">
                          <multiselect 
                            v-model="selectedEmployee"
                            :options="employees"
                            label="name"
                            v-validate="'required'"
                            :searchable="true"
                            @search-change="employeeSearch"
                          />
                      </div>
                    </div>
                  </div>
              </div>
              <div class="col-md-12 mt-4">
                  <div class="row align-items-center">
                    <div class="col-md-12">
                        <span>Employee Name</span>
                    </div>
                    <div class="col-md-12 mt-2">
                      <div class="input-group">
                        <input name="name" type="text" class="form-control" v-model="name"/>
                      </div>
                    </div>
                  </div>
              </div> -->
              <div class="col-md-12 mt-4">
                  <div class="row justify-content-end">
                      <div class="col-auto">
                          <button class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="_downloadReport">Download</button>
                      </div>
                  </div>
              </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
import moment from 'moment'
import { ReportService } from '../../services/MyLearning/ReportService'
import { DashboardGuestAccountService, ReleaseTrainingService, ProgramTypeService } from '../../services/NSwagService'

export default {
  props: ['claims'],
    async mounted() {
      this.trainings = await (await this.service.getReleaseTrainingReport('', '', '', '', '', '', '', '', 1, '', `${this.claims.DealerId ? this.claims.DealerId : ''}`)).listTraining.map(v => ({...v, courseName: `${v.courseName} (Batch-${v.batch})`}));
      this.programTypes = await (await this.serviceProgram.getAllProgramTypes()).listProgramTypeModel;
      this.areas = await this.service.getAllArea();
      this.dealers = await this.service.getAllDealer();
      this.filteredDealers = this.dealers
      this.outlets = await this.service.getAllOutlet();
      this.filteredOutlets = this.outlets
      // this.employees = await (await this.serviceEmployee.getAccounts({
      //   ...this.paramsEmployee,
      //   SearchQuery: ''
      // })).data
          if(this.claims.DealerId) {
              this.selectedDealer = [...this.dealers.filter(v => v.dealerId === this.claims.DealerId)]
              this.handleChangeDealer({dealerId: this.claims.DealerId})
          }
          console.log(this.claims)
    },
    data() {
      return {
        isBusy: false,
        errored: [],
        trainings: [],
        dealers: [],
        outlets: [],
        filteredOutlets: [],
        filteredDealers: [],
        employees: [],
        programTypes: [],
        areas: [],
        isTrainingsLoading: false,
        reportService: new ReportService(),
        service: new ReleaseTrainingService(),
        serviceEmployee: new DashboardGuestAccountService(),
        serviceProgram: new ProgramTypeService(),
        params: {
          TrainingId: '',
          DealerId: '',
          OutletID: '',
          EmployeeID: ''
        },
        selectedTraining: null,
        selectedDealer: null,
        selectedOutlet: null,
        selectedEmployee: null,
        selectedArea: null,
        selectedProgramTypes: null,
        selectedAreaSpecialist: null,
        areaSpecialists: [],
        paramsEmployee: {
          Page: 1,
          Limit: 100
        },
        name: ''
      }
    },
    methods: {
      async _downloadReport() {
        this.errored = []
        
        if(!this.selectedTraining) {
          this.errored.push({
            key: 'validation',
            value: 'Please fill Training field'
          })
        }
        if(!this.selectedProgramTypes) {
          this.errored.push({
            key: 'program type',
            value: 'Please fill Program Type field'
          })
        }

        if(this.errored.length > 0) {
          return
        }

        this.isBusy = true
        try {
          const res = await this.reportService.getKPITrackingReport({
            TrainingId: this.selectedTraining.trainingId,
            OutletId: this.selectedOutlet ? this.selectedOutlet.outletId : '',
            DealerId: this.selectedDealer ? this.selectedDealer.dealerId : '',
            AreaId: this.selectedOutlet ? this.selectedArea.areaId : '',
          })
        } catch (err) {
          this.errored.push({
            key: 'no data',
            value: 'No report'
          })
        }
        this.isBusy = false
      },
      async trainingSearch(query) {
          if (query == null || query === '') {
            this.isTrainingsLoading = true;
            this.trainings = await (await this.service.getReleaseTrainingReport('', '', '', '', '', '', '', '', 1, this.selectedProgramTypes.programTypeId, `${this.claims.DealerId ? this.claims.DealerId : ''}`)).listTraining.map(v => ({...v, courseName: `${v.courseName} (Batch-${v.batch})`}));
            this.isTrainingsLoading = false;
            return;
          }

          this.isTrainingsLoading = true;
          this.trainings = await (await this.service.getReleaseTrainingReport(query, '', '', '', '', '', '', '', 1, this.selectedProgramTypes.programTypeId, `${this.claims.DealerId ? this.claims.DealerId : ''}`)).listTraining.map(v => ({...v, courseName: `${v.courseName} (Batch-${v.batch})`}));
          this.isTrainingsLoading = false;
      },
      async employeeSearch(query) {
          if (query == null || query === '') {
            this.isTrainingsLoading = true;
              this.employees = await (await this.serviceEmployee.getAccounts({
                ...this.paramsEmployee,
                SearchQuery: query
              })).data;
              this.isTrainingsLoading = false;
              return;
          }

          this.isTrainingsLoading = true;
          this.employees = await (await this.serviceEmployee.getAccounts({
            ...this.paramsEmployee,
            SearchQuery: query
          })).data;
          this.isTrainingsLoading = false;
      },
      async handleChangeProgramType(a) {
          this.trainings = await (
              await this.service.getReleaseTrainingReport(
                  '',
                  '',
                  '',
                  '',
                  '',
                  '',
                  '',
                  '',
                  1,
                  a.programTypeId,
                  `${this.claims.DealerId ? this.claims.DealerId : ''}`
              )
          ).listTraining.map(v => ({
              ...v,
              courseName: `${v.courseName} (Batch-${v.batch})`
          }));
        },
        handleChangeArea(area) {
            // reset
            this.selectedDealer = null
            this.selectedOutlet = null

            const findedOutlets = this.outlets.filter(v => v.areaId === area.areaId)
            const dealerIds = findedOutlets.map(v => v.dealerId)
            const findedDealers = this.dealers.filter(v => dealerIds.includes(v.dealerId))

            this.filteredOutlets = findedOutlets
            this.filteredDealers = findedDealers
        },
        handleRemoveArea(area) {
            // reset
            this.selectedDealer = null
            this.selectedOutlet = null

            this.filteredOutlets = this.outlets
            this.filteredDealers = this.dealers
        },
        handleChangeDealer(dealer) {
            // reset
            this.selectedOutlet = null

            const findedOutlets = this.outlets.filter(v => v.dealerId === dealer.dealerId)

            this.filteredOutlets = findedOutlets
        },
        handleRemoveDealer(dealer) {
            // reset
            this.selectedOutlet = null

            this.filteredOutlets = this.outlets
        }
    }
}
</script>

<style scoped>
</style>