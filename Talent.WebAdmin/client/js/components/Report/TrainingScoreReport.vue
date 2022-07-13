<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="row">

            <div class="col col-md-12" v-if="view === true">
                <h3>
                    <fa icon="chart-bar"></fa> Report >
                    <span class="talent_font_red">Training Score Reports</span>
                </h3>

                <br />
                <!-- error message -->
                <div v-if="isShowErrorMessage == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                    {{theShowErrorMessage}}
                    <button type="button" class="close" data-dismiss="alert" @click.prevent="ResetErrorMessage()" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <br />

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Training Score Report</h4>

                    <br />
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group talent_front">
                                        <v-date-picker class="v-date-style-report" mode="range" v-model="filter.dateFilter" :firstDayOfWeek="2" :value="null" :masks="{ input: 'DD/MM/YYYY' }" :input-props="props"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Program Type</span>
                                </div>
                                <div class="col-md-9">
                                    <select class="form-control" v-model="filter.programTypeId">
                                        <option v-for="data in programTypeDropdown" :value="data.programTypeId">{{data.programTypeName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Participant</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input type="number" class="form-control" v-model="filter.participant" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Status</span>
                                </div>
                                <div class="col-md-9">
                                    <select class="form-control" v-model="filter.status">
                                        <option v-for="data in statusDropdown" :value="data">{{data}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <div class="row">
                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Course Name</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input type="text" class="form-control" v-model="filter.courseName" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Batch</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input type="number" class="form-control" v-model="filter.batch" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Participant Rate</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input type="number" class="form-control" v-model="filter.participantRate" />
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
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="fetch()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="resetFilter()">
                                            Reset
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <br />

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Training Score Report List</h4>
                    <br />
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{reportData.listReportTrainingScore.length}} of {{reportData.totalData}} Entry(s)</a>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCourseName()">Course Name <fa icon="sort" v-if="courseName.sort == true"></fa><fa icon="sort-up" v-if="courseName.sortup == true"></fa><fa icon="sort-down" v-if="courseName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortProgramType()">Program Type <fa icon="sort" v-if="programType.sort == true"></fa><fa icon="sort-up" v-if="programType.sortup == true"></fa><fa icon="sort-down" v-if="programType.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortBatch()">Batch <fa icon="sort" v-if="batch.sort == true"></fa><fa icon="sort-up" v-if="batch.sortup == true"></fa><fa icon="sort-down" v-if="batch.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortParticipant()">Participant <fa icon="sort" v-if="participant.sort == true"></fa><fa icon="sort-up" v-if="participant.sortup == true"></fa><fa icon="sort-down" v-if="participant.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortParticipantRate()">Participant Rate <fa icon="sort" v-if="participantRate.sort == true"></fa><fa icon="sort-up" v-if="participantRate.sortup == true"></fa><fa icon="sort-down" v-if="participantRate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortStatus()">Status <fa icon="sort" v-if="status.sort == true"></fa><fa icon="sort-up" v-if="status.sortup == true"></fa><fa icon="sort-down" v-if="status.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortStartDate()">Start Date <fa icon="sort" v-if="startDate.sort == true"></fa><fa icon="sort-up" v-if="startDate.sortup == true"></fa><fa icon="sort-down" v-if="startDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortEndDate()">End Date <fa icon="sort" v-if="endDate.sort == true"></fa><fa icon="sort-up" v-if="endDate.sortup == true"></fa><fa icon="sort-down" v-if="endDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="2">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(data,index) in reportData.listReportTrainingScore" :key="index">
                                    <td>
                                        {{data.courseName}}
                                    </td>
                                    <td>
                                        {{data.programType}}
                                    </td>
                                    <td>
                                        {{data.batch}}
                                    </td>
                                    <td>
                                        {{data.participant}}
                                    </td>
                                    <td>
                                        {{data.participantRate}}
                                        <span v-show="data.participantRate != null">
                                            %
                                        </span>
                                    </td>
                                    <td>
                                        {{data.status}}
                                    </td>
                                    <td>
                                        {{getStringDate(data.startTime)}}
                                    </td>
                                    <td>
                                        {{getStringDate(data.endTime)}}
                                    </td>
                                    <td class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click="detailClicked(index)">View Detail</button>
                                    </td>
                                    <td class="talent_nopadding_important">
                                        <button type="button" button class="btn btn-block talent_bg_green talent_font_white" @click.prevent="downloadExcel(data.trainingId)">Download</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!-- paginate -->
                    <div class="col-md-12 d-flex justify-content-center mt-3">
                        <paginate :currentPage.sync="currentPage" :total-data=reportData.totalData :limit-data=currentItemPage @update:currentPage="fetch()"></paginate>
                    </div>

                </div>
            </div>

            <div class="col col-md-12" v-if="viewDetail === true">
                <h3 class="mb-3">
                    <fa icon="chart-bar"></fa> Report > Training Score Report
                    <span class="talent_font_red">View Detail</span>
                </h3>

                <div v-if="url.length > 0" class="row mb-3" style="height:100vh">
                    <iframe width="100%"
                            height="100%"
                            :src="url"
                            frameborder="0"
                            allowfullscreen="true"></iframe>
                </div>

                <!--<div class="col-md-12 mb-md-3">
                <div class="d-flex align-items-end flex-column">
                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                        Close
                    </button>
                </div>
            </div>-->

            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { Sort } from '../../class/Sort';
    import { ContentService, DropdownService, ResponseApprovalContentModel, ApprovalContentViewModel, ApprovalContentFilterModel, UserAccessCRUD, UserPrivilegeSettingsService, TrainingScoreReportService, ReportTrainingScoreViewModel, ProgramTypeModel } from '../../services/NSwagService';
    import { faAd } from '@fortawesome/free-solid-svg-icons';
    import { FileReportService } from '../../services/FileReportService';

    @Component({
        created: async function (this: TrainingScoreReport) {
            await this.getAccess();
            await this.initialize();
        }
    })
    export default class TrainingScoreReport extends Vue {

        isLoading: boolean = false;
        isBusy: boolean = true;

        view: boolean = true;
        viewDetail: boolean = false;

        isShowErrorMessage: boolean = false;
        theShowErrorMessage: string;

        currentPage: number = 1;
        currentItemPage: number = 10;
        totalData: number = 0;
        userLevel: number = 1;

        url: string = '';

        reportData: ReportTrainingScoreViewModel = {
            listReportTrainingScore: [],
            totalData: 1
        }

        props: any = {
            placeholder: "",
            readonly: true
        };

        //Variable Sorting
        filter: ITrainingScoreReport = {
            dateFilter: {
                end: null,
                start: null
            }
        };

        programTypeDropdown: ProgramTypeModel[] = [];

        statusDropdown: string[] = ["On Going", "Done"];

        //Service
        dropdownServiceMan: DropdownService = new DropdownService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        trainingReportMan: TrainingScoreReportService = new TrainingScoreReportService();

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        minDate: Date = new Date('0001-01-01T00:00:00');

        //Variable untuk sorting
        courseName = new Sort();
        programType = new Sort();
        batch = new Sort();
        participant = new Sort();
        participantRate = new Sort();
        status = new Sort();
        endDate = new Sort();
        startDate = new Sort();

        async initialize() {
            this.programTypeDropdown = await this.trainingReportMan.getProgramTypes();
            await this.fetch();
            this.isBusy = false;
        }

        async fetch() {
            this.isBusy = true;
            try {
                let result = await this.trainingReportMan.getTrainingScoreReport(this.filter.dateFilter.start, this.filter.dateFilter.end, this.filter.programTypeId, this.filter.participant, this.filter.status, this.filter.courseName, this.filter.batch, this.filter.participantRate, this.filter.sortBy, this.currentPage);

                this.reportData = result;
            }
            catch {
                this.isShowErrorMessage = true;
                this.theShowErrorMessage = "Failed to Get Data";
            }
            this.isBusy = false;
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("RPT");
            this.crud = data
        }

        async resetFilter() {

            this.currentPage = 1;

            this.filter = {};
            this.filter = {
                dateFilter: {
                    end: null,
                    start: null
                },
            };
            this.fetch();
        }

        getStringDate(date: Date) {

            if (!date) {
                return '-';
            }

            if (new Date(date).getUTCFullYear() === this.minDate.getUTCFullYear()) {
                return '-';
            }

            let newDate = new Date(date);

            var ddDate = newDate.getDate();
            var mmDate = newDate.getMonth() + 1;
            var yyyyDate = newDate.getFullYear();
            var dd = '' + ddDate;
            var mm = '' + mmDate;
            var yyyy = yyyyDate;
            if (ddDate < 10) {
                dd = '0' + ddDate;
            }
            if (mmDate < 10) {
                mm = '0' + mmDate;
            }
            var today = dd + '/' + mm + '/' + yyyy;

            return today;
        }

        closeClicked() {
            this.view = true;
            this.viewDetail = false;
            this.url = '';
        }

        detailClicked(index: number) {
            this.viewDetail = true;
            this.view = false;

            this.url = this.reportData.listReportTrainingScore[index].urlDetail;
        }

         async downloadExcel(trainingId) {
             this.isBusy = true;
             await FileReportService.trainingScoreReportDownload(trainingId);
            this.isBusy = false;
        }

        // --------------- service / main function --------------------

        ResetErrorMessage() {
            this.isShowErrorMessage = false;
            this.theShowErrorMessage = null;
        }

        // ----------------- sorting handler dan function -------------------
        //ClickSortCourseName
        async ClickSortCourseName() {
            this.ResetSort('courseName');
            //Sorting
            this.courseName.sorting();
            //Function Sorting
            if (this.courseName.sortup == true && this.courseName.sortdown == false) {
                this.filter.sortBy = 'courseName';
            }
            else if (this.courseName.sortup == false && this.courseName.sortdown == true) {
                this.filter.sortBy = 'courseNameDesc';
            } else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            return;
        }

        //ClickSortProgramType
        async ClickSortProgramType() {
            this.ResetSort('programType');
            //Sorting
            this.programType.sorting();
            //Function Sorting
            if (this.programType.sortup == true && this.programType.sortdown == false) {
                this.filter.sortBy = 'programType';
            }
            else if (this.programType.sortup == false && this.programType.sortdown == true) {
                this.filter.sortBy = 'programTypeDesc';
            } else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            return;
        }

        //ClickSortBatch
        async ClickSortBatch() {
            this.ResetSort('batch');
            //Sorting
            this.batch.sorting();
            //Function Sorting
            if (this.batch.sortup == true && this.batch.sortdown == false) {
                this.filter.sortBy = 'batch';
            }
            else if (this.batch.sortup == false && this.batch.sortdown == true) {
                this.filter.sortBy = 'batchDesc';
            } else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            return;
        }

        //ClickSortParticipant
        async ClickSortParticipant() {
            this.ResetSort('participant');
            //Sorting
            this.participant.sorting();
            //Function Sorting
            if (this.participant.sortup == true && this.participant.sortdown == false) {
                this.filter.sortBy = 'participant';
            }
            else if (this.participant.sortup == false && this.participant.sortdown == true) {
                this.filter.sortBy = 'participantDesc';
            } else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            return;
        }

        //ClickSortStatus
        async ClickSortStatus() {
            this.ResetSort('status');
            //Sorting
            this.status.sorting();
            //Function Sorting
            if (this.status.sortup == true && this.status.sortdown == false) {
                this.filter.sortBy = 'status';
            }
            else if (this.status.sortup == false && this.status.sortdown == true) {
                this.filter.sortBy = 'statusDesc';
            }
            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            return;
        }

        //ClickSortEndDate
        async ClickSortEndDate() {
            this.ResetSort('endDate');
            //Sorting
            this.endDate.sorting();
            //Function Sorting
            if (this.endDate.sortup == true && this.endDate.sortdown == false) {
                this.filter.sortBy = 'endDate';
            }
            else if (this.endDate.sortup == false && this.endDate.sortdown == true) {
                this.filter.sortBy = 'endDateDesc';
            } else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            return;
        }

        //ClickSortEndDate
        async ClickSortParticipantRate() {
            this.ResetSort('participantRate');
            //Sorting
            this.participantRate.sorting();
            //Function Sorting
            if (this.participantRate.sortup == true && this.participantRate.sortdown == false) {
                this.filter.sortBy = 'participantRate';
            }
            else if (this.participantRate.sortup == false && this.participantRate.sortdown == true) {
                this.filter.sortBy = 'participantRateDesc';
            } else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            return;
        }

        //ClickSortEndDate
        async ClickSortStartDate() {
            this.ResetSort('startDate');
            //Sorting
            this.startDate.sorting();
            //Function Sorting
            if (this.startDate.sortup == true && this.startDate.sortdown == false) {
                this.filter.sortBy = 'startDate';
            }
            else if (this.startDate.sortup == false && this.startDate.sortdown == true) {
                this.filter.sortBy = 'startDateDesc';
            } else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'courseName':
                    this.programType.reset();
                    this.batch.reset();
                    this.participant.reset();
                    this.status.reset();
                    this.endDate.reset();
                    this.startDate.reset();
                    this.participantRate.reset();
                    return;
                case 'programType':
                    this.courseName.reset();
                    this.batch.reset();
                    this.participant.reset();
                    this.status.reset();
                    this.participantRate.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'batch':
                    this.courseName.reset();
                    this.programType.reset();
                    this.participant.reset();
                    this.status.reset();
                    this.participantRate.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'participant':
                    this.courseName.reset();
                    this.programType.reset();
                    this.batch.reset();
                    this.status.reset();
                    this.participantRate.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'participantRate':
                    this.courseName.reset();
                    this.programType.reset();
                    this.batch.reset();
                    this.participant.reset();
                    this.status.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'status':
                    this.courseName.reset();
                    this.programType.reset();
                    this.batch.reset();
                    this.participant.reset();
                    this.participantRate.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'endDate':
                    this.courseName.reset();
                    this.programType.reset();
                    this.batch.reset();
                    this.participant.reset();
                    this.participantRate.reset();
                    this.startDate.reset();
                    this.status.reset();
                    return;
                case 'startDate':
                    this.courseName.reset();
                    this.programType.reset();
                    this.batch.reset();
                    this.participant.reset();
                    this.participantRate.reset();
                    this.status.reset();
                    this.endDate.reset();
                    return;
            }
        }
    }

    interface ITrainingScoreReport {
        dateFilter?: {
            start?: Date,
            end?: Date
        }
        programTypeId?: number,
        participant?: number,
        status?: string,
        courseName?: string,
        batch?: number,
        participantRate?: number,
        sortBy?: string,
        pageNumber?: number
    }
</script>
