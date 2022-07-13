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
          <h5>Search Learning Progress Report</h5>
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
                            :options="filteredAreaSpecialists"
                            label="areaSpecialistName"
                            v-validate="'required'"
                            :searchable="true"
                              @select="handleChangeSpecial"
                              @remove="handleRemoveSpecial"
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
                                track-by="dealerId"
                              @select="handleChangeDealer"
                              @remove="handleRemoveDealer"
                              :multiple="true"
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
              <div class="col-md-6 mt-4">
                  <div class="row align-items-center">
                    <div class="col-md-12">
                        <span>Employee ID</span>
                    </div>
                    <div class="col-md-12 mt-2">
                      <div class="input-group">
                          <!-- <multiselect 
                            v-model="selectedEmployee"
                            :options="employees"
                            label="name"
                            v-validate="'required'"
                            :searchable="true"
                            @search-change="employeeSearch"
                          /> -->
                        <input name="name" type="text" class="form-control" v-model="selectedEmployee"/>
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
              </div>
              <!-- <div class="col-md-12 mt-4">
                  <div class="row justify-content-end">
                      <div class="col-auto">
                          <button class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="_downloadReport">Download</button>
                      </div>
                  </div>
              </div> -->
              <div class="col-md-12 mt-4">
                  <div class="row justify-content-end">
                      <div class="col-auto">
                          <button
                              class="btn talent_bg_cyan talent_roundborder talent_font_white"
                              @click.prevent="_handleSearch"
                          >
                              Search
                          </button>
                      </div>
                  </div>
              </div>
          </div>
        </div>
      </div>


      <!-- table -->
      <div class="row w-100 mt-4">
          <div class="col-md-12 mt-3">
              <div class="row justify-content-between">
                <div class="col-auto">
                    <div class="dropdown">
                        <button
                            class="btn btn-info dropdown-toggle mr-2"
                            type="button"
                            id="dropdownMenuButton"
                            data-toggle="dropdown"
                            aria-expanded="false"
                        >
                            Filter
                        </button>
                        <div
                            class="dropdown-menu p-3"
                            aria-labelledby="dropdownMenuButton"
                            style="min-width: 500px"
                            @click="handlePreventMenuClose"
                        >
                            <b>Filter</b>
                            <div class="row">
                                <div class="col">
                                    <div class="input-group talent_front mt-2">
                                        <label for="">Position</label>
                                        <multiselect v-model="selectedPositions"
                                                    name="Dealer"
                                                    :options="positions"
                                                    :allow-empty="true"
                                                    multiple
                                                    label="positionName"
                                                    track-by="positionName">
                                        </multiselect>
                                    </div>
                                </div>
                                <div class="col mt-4">
                                    <div
                                        class="input-group talent_front"
                                    >
                                        <label for="">Status</label>
                                        <div
                                            class="w-100 d-flex flex-wrap"
                                        >
                                            <div
                                                :class="
                                                    `${
                                                        data.isSelected
                                                            ? 'filter-status-active'
                                                            : 'filter-status'
                                                    } mx-2 my-2`
                                                "
                                                v-for="data in filterStatus"
                                                :key="data.value"
                                                @click="
                                                    data.isSelected = !data.isSelected
                                                "
                                            >
                                                {{ data.label }}
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row d-flex flex-wrap">
                                <hr class="w-100" />
                                <div
                                    class="w-100 mt-1 d-flex justify-content-end px-3"
                                >
                                    <button
                                        data-toggle="dropdown"
                                        class="btn btn-info mr-3"
                                        @click="handleFilter"
                                    >
                                        Apply
                                    </button>
                                    <button
                                        data-toggle="dropdown"
                                        class="btn btn-info mr-3"
                                        @click="handleResetFilter"
                                    >
                                        Reset
                                    </button>
                                    <button
                                        data-toggle="dropdown"
                                        class="btn btn-danger"
                                        @click="hideDropdown"
                                    >
                                        Cancel
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                  <div class="col-auto">
                      <button
                          class="btn talent_bg_green talent_roundborder talent_font_white"
                          @click.prevent="_downloadReport"
                      >
                          Download
                      </button>
                  </div>
              </div>
          </div>
          <div class="w-100 talent_overflowx mt-3">
              <table
                  class="table table-bordered table-responsive-md talent_table_padding"
              >
                  <thead>
                      <tr>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Program
                              </a>
                          </th>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Course
                              </a>
                          </th>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Dealer
                              </a>
                          </th>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Outlet
                              </a>
                          </th>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Area
                              </a>
                          </th>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Area Specialist
                              </a>
                          </th>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Position
                              </a>
                          </th>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Employee ID
                              </a>
                          </th>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Employee Name
                              </a>
                          </th>
                          <th scope="col" class="text-center" v-if="datas.length > 0 && datas[0].pointsJSON.length > 0" :colspan="datas[0].pointsJSON.length">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Pre
                              </a>
                          </th>
                          <th scope="col" class="text-center" v-if="datas.length > 0 && datas[0].pointsDuringJSON.length > 0" :colspan="datas[0].pointsDuringJSON.length">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  During
                              </a>
                          </th>
                          <th scope="col" class="text-center" v-if="datas.length > 0 && datas[0].pointsPostJSON.length > 0" :colspan="datas[0].pointsPostJSON.length">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Post
                              </a>
                          </th>
                          <th scope="col" class="text-center" rowspan="2">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  Completeness
                              </a>
                          </th>
                      </tr>
                      <tr v-if="datas.length > 0">
                          <th scope="col" class="text-center" v-for="pre in datas[0].pointsJSON">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  {{ pre.moduleName }}
                              </a>
                          </th>
                          <th scope="col" class="text-center" v-for="during in datas[0].pointsDuringJSON">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  {{ during.moduleName }}
                              </a>
                          </th>
                          <th scope="col" class="text-center" v-for="post in datas[0].pointsPostJSON">
                              <a
                                  href="#"
                                  class="talent_sort"
                                  style="color: white;"
                              >
                                  {{ post.moduleName }}
                              </a>
                          </th>
                      </tr>
                  </thead>
                  <tbody>
                      <tr v-for="(data, index) in datas" :key="index">
                          <td class="text-center">
                              {{ data.program }}
                          </td>
                          <td class="text-center">
                              {{ data.course }}
                          </td>
                          <td class="text-center">
                              {{ data.dealer }}
                          </td>
                          <td class="text-center">
                              {{ data.outlet }}
                          </td>
                          <td class="text-center">
                              {{ data.area }}
                          </td>
                          <td class="text-center">
                              {{ data.areaSpecialist }}
                          </td>
                          <td class="text-center">
                              {{ data.position }}
                          </td>
                          <td class="text-center">
                              {{ data.employeeID }}
                          </td>
                          <td class="text-center">
                              {{ data.employeeName }}
                          </td>
                          <td class="text-center" v-for="pre in data.pointsJSON">
                              {{ pre.point }}
                          </td>
                          <td class="text-center" v-for="during in data.pointsDuringJSON">
                              {{ during.point }}
                          </td>
                          <td class="text-center" v-for="post in data.pointsPostJSON">
                              {{ post.point }}
                          </td>
                          <td class="text-center">
                              {{ data.video }}
                          </td>
                      </tr>
                  </tbody>
              </table>
          </div>

          <!--Pagination-->
          <div class="col-md-12 d-flex justify-content-center">
              <paginate
                  :currentPage.sync="table.Page"
                  :total-data="totalData"
                  :limit-data="10"
                  @update:currentPage="_fetchReport()"
              ></paginate>
          </div>
      </div>
    </div>
  </div>
</template>


<script>
import moment from 'moment'
import { ReportService } from '../../services/MyLearning/ReportService'
import { AssessmentService } from '../../services/Assessment/AssesmentService';
import { DashboardGuestAccountService, ReleaseTrainingService, ProgramTypeService, DropdownService } from '../../services/NSwagService'
import $ from 'jquery';

export default {
    props: ['claims'],
    async mounted() {
      this.trainings = await (await this.service.getReleaseTrainingReport('', '', '', '', '', '', '', '', 1, '', `${this.claims.DealerId ? this.claims.DealerId : ''}`)).listTraining.map(v => ({...v, courseName: `${v.courseName} (Batch-${v.batch})`}));
      this.programTypes = await (await this.serviceProgram.getAllProgramTypes()).listProgramTypeModel;
    this.positions = await this.service.getAllPosition();
      this.dealers = await this.service.getAllDealer();
      this.filteredDealers = this.dealers.sort((a,b) => {
                if ( a.dealerName < b.dealerName ){
                    return -1;
                }
                if ( a.dealerName > b.dealerName ){
                    return 1;
                }
                return 0;
            })
      this.outlets = await this.service.getAllOutlet();
      this.filteredOutlets = this.outlets
      this.areas = await this.service.getAllArea();
      this.areaSpecialists = await (await this.areaService.getAllAreaSpecialist({
          PageIndex: 1,
          PageSize: 500,
      })).areaSpecialists;
      this.filteredAreaSpecialists = this.areaSpecialists
    //   this.employees = await (await this.serviceEmployee.getAccounts({
    //     ...this.paramsEmployee,
    //     SearchQuery: ''
    //   })).data

    
          if(this.claims.DealerId) {
              this.selectedDealer = [...this.dealers.filter(v => v.dealerId === this.claims.DealerId)]
              this.handleChangeDealer({dealerId: this.claims.DealerId})
          }
    },
    data() {
      return {
        filterStatus: [
            {
                label: 'PASSED',
                value: 'LULUS',
                isSelected: false
            },
            {
                label: 'NOT PASSED',
                value: 'TIDAK LULUS',
                isSelected: false
            },
            {
                label: 'NOT FINISHED',
                value: 'BELUM',
                isSelected: false
            },
        ],
        datas: [],
        table: {
            Page: 1,
            Limit: 10
        },
        totalData: 0,
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
        areaSpecialists: [],
        positions: [],
        filteredAreaSpecialists: [],
        isTrainingsLoading: false,
        reportService: new ReportService(),
        service: new ReleaseTrainingService(),
        serviceEmployee: new DashboardGuestAccountService(),
        serviceProgram: new ProgramTypeService(),
        areaService: new AssessmentService(),
        dropdownService: new DropdownService(),
        params: {
          TrainingId: '',
          DealerId: '',
          OutletID: '',
          EmployeeID: ''
        },
        selectedProgramTypes: null,
        selectedTraining: null,
        selectedDealer: null,
        selectedOutlet: null,
        selectedEmployee: null,
        selectedArea: null,
        selectedAreaSpecialist: null,
        selectedStatus: null,
        selectedPositions: null,
        paramsEmployee: {
          Page: 1,
          Limit: 100
        },
        name: ''
      }
    },
    methods: {
        handleFilter() {
            this.table = {
                Page: 1,
                Limit: 10
            };
            this._fetchReport();
            this.hideDropdown();
        },
        hideDropdown() {
            $('#dropdownMenuButton').dropdown('hide');
        },
        handlePreventMenuClose(e) {
            e.stopPropagation();
        },
        handleResetFilter() {
            this.filterStatus = this.filterStatus.map(v => ({
                ...v,
                isSelected: false
            }));
            this.table = {
                Page: 1,
                Limit: 10
            };
            this._fetchReport();
            this.hideDropdown();
        },
        async _fetchReport() {
            this.datas = [];
            this.errored = [];
            this.isBusy = true;
            try {
                const res = await this.reportService.getLearningProgressReportJson(
                    {
                        ProgramTypeId: this.selectedProgramTypes
                            ? this.selectedProgramTypes.programTypeId
                            : null,
                        TrainingId: this.selectedTraining
                            ? this.selectedTraining.trainingId
                            : null,
                        DealerId: this.selectedDealer && this.selectedDealer.length > 0
                            ? this.selectedDealer.map(v=>v.dealerId).join(',')
                            : '*',
                        OutletID: this.selectedOutlet
                            ? this.selectedOutlet.outletId
                            : null,
                        AreaId: this.selectedArea
                            ? this.selectedArea.areaId
                            : null,
                        EmployeeID: this.selectedEmployee
                            ? this.selectedEmployee
                            : '',
                        AreaSpecialistId: this.selectedAreaSpecialist ? this.selectedAreaSpecialist.areaSpecialistId : null,
                        EmployeeName: this.name,
                        Page: this.table.Page,
                        Limit: this.table.Limit,
                        Positions: this.selectedPositions && this.selectedPositions.length > 0
                            ? this.selectedPositions.map(v=>v.positionId).join(',')
                            : '*',
                        Status: 
                            this.filterStatus.filter(v => v.isSelected).length > 0 ? this.filterStatus.filter(v => v.isSelected).map(v => v.value).join(',') : '*'
                    }
                );
                this.datas = res.data
                this.totalData = res.count;
                console.log(res);
            } catch (err) {
                this.errored.push({
                    key: 'error',
                    value: JSON.parse(err.response).data.en
                });
            }
            this.isBusy = false;
        },
        async _handleSearch() {
            this.errored = [];
            if (this.selectedProgramTypes == null) {
                this.errored.push({
                    key: 'programtype',
                    value: 'Please fill program type'
                });
            }
            if (this.errored.length > 0) return;

            this.table = {
                Page: 1,
                Limit: 10
            };
            this._fetchReport();
            this.hideDropdown();
        },
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
          const res = await this.reportService.getCompetencyTrackingReport({
            TrainingId: this.selectedTraining.trainingId,
            AreaId: this.selectedArea ? this.selectedArea.areaId : '',
            DealerId: this.selectedDealer && this.selectedDealer.length > 0 ? this.selectedDealer.map(v=>v.dealerId).join(',') : '*',
            OutletID: this.selectedOutlet ? this.selectedOutlet.outletId : '',
            EmployeeID: this.selectedEmployee ? this.selectedEmployee : '',
            AreaSpecialistId: this.selectedAreaSpecialist ? this.selectedAreaSpecialist.areaSpecialistId : null,
            EmployeeName: this.name,
                        Positions: this.selectedPositions && this.selectedPositions.length > 0
                            ? this.selectedPositions.map(v=>v.positionId).join(',')
                            : '*',
                        Status: 
                            this.filterStatus.filter(v => v.isSelected).length > 0 ? this.filterStatus.filter(v => v.isSelected).map(v => v.value).join(',') : '*'
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
          this.selectedAreaSpecialist = null

          const findedOutlets = this.outlets.filter(v => v.areaId === area.areaId)
          const dealerIds = findedOutlets.map(v => v.dealerId)
          const areaSpecialistsIds = findedOutlets.map(v => v.areaSpecialistId)

          const findedDealers = this.dealers.filter(v => dealerIds.includes(v.dealerId))
          const findedAreaSpecialists = this.areaSpecialists.outlets.filter(v => areaSpecialistsIds.includes(v.areaSpecialistId))

          this.filteredOutlets = findedOutlets
          this.filteredDealers = findedDealers.sort((a,b) => {
                if ( a.dealerName < b.dealerName ){
                    return -1;
                }
                if ( a.dealerName > b.dealerName ){
                    return 1;
                }
                return 0;
            })
          this.filteredAreaSpecialists = findedAreaSpecialists
      },
      handleRemoveArea(area) {
          // reset
          this.selectedDealer = null
          this.selectedOutlet = null
          this.selectedAreaSpecialist = null

          this.filteredOutlets = this.outlets
          this.filteredDealers = this.dealers.sort((a,b) => {
                if ( a.dealerName < b.dealerName ){
                    return -1;
                }
                if ( a.dealerName > b.dealerName ){
                    return 1;
                }
                return 0;
            })
          this.filteredAreaSpecialist = this.areaSpecialists
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
      },
      handleChangeSpecial(special) {
          // reset
          this.selectedOutlet = null
          this.selectedDealer = null

          const outlets = special.outlets.map(v => v.outletId)

          const findedOutlets = this.outlets.filter(v => outlets.includes(v.outletId))
          const dealerIds = findedOutlets.map(v => v.dealerId)
          const findedDealers = this.dealers.filter(v => dealerIds.includes(v.dealerId))

          this.filteredDealers = findedDealers.sort((a,b) => {
                if ( a.dealerName < b.dealerName ){
                    return -1;
                }
                if ( a.dealerName > b.dealerName ){
                    return 1;
                }
                return 0;
            })
          this.filteredOutlets = findedOutlets
      },
      handleRemoveSpecial(special) {
          // reset
          this.selectedOutlet = null
          this.selectedDealer = null

          this.filteredOutlets = this.outlets
          this.filteredDealer = this.dealers
      }
    }
}
</script>

<style scoped>
.filter-status {
    border-radius: 9999px;
    color: #9e9e9e;
    border: 1px solid #9e9e9e;
    padding: 4px 6px;
    cursor: pointer;
}
.filter-status-active {
    border-radius: 9999px;
    color: white;
    border: 1px solid #007bff;
    background-color: #007bff;
    padding: 4px 6px;
    cursor: pointer;
}
</style>