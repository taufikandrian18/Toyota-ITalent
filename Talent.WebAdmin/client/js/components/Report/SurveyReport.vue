<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <!-- View -->
        <div class="row">            
            <div class="col col-md-12" v-if="view === true">
                <h3><fa icon="chart-bar"></fa> Report > <span class="talent_font_red">Survey Report</span></h3>
                <br />

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Survey Report</h4>

                    <br />
                    <!--3 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group talent_front">
                                        <v-date-picker class="v-date-style-report" mode="range" :firstDayOfWeek="2" v-model="filterDate" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Respondent</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input type="number" class="form-control" v-model="filterRespondent" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Status</span>
                                </div>
                                <div class="col-md-9">
                                    <select class="form-control" v-model="filterStatus">
                                        <option v-for="data in statusDropdown" :value="data">{{data}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--2 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Survey Title</span>
                                </div>
                                <div class="col-md-9">
                                    <input class="form-control" v-model="filterSurveyTitle" />
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Respondent Rate</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" type="number" v-model="filterRespondentRate" />
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
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="fetch">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="reset">
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
                    <h4>Survey Report List</h4>
                    <br />
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{surveyReport.surveyReportList == null ? 0 : surveyReport.surveyReportList.length }} of {{surveyReport.totalData}} Entry(s)</a>
                        </div>
                    </div>
                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortSurveyTitle()">Survey Title<fa icon="sort" v-if="surveyTitle.sort == true"></fa><fa icon="sort-up" v-if="surveyTitle.sortup == true"></fa><fa icon="sort-down" v-if="surveyTitle.sortdown"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortRespondent()">Respondent<fa icon="sort" v-if="respondent.sort == true"></fa><fa icon="sort-up" v-if="respondent.sortup == true"></fa><fa icon="sort-down" v-if="respondent.sortdown"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortRespondentRate()">Respondent Rate<fa icon="sort" v-if="respondentRate.sort == true"></fa><fa icon="sort-up" v-if="respondentRate.sortup == true"></fa><fa icon="sort-down" v-if="respondentRate.sortdown"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortStatus()">Status<fa icon="sort" v-if="status.sort == true"></fa><fa icon="sort-up" v-if="status.sortup == true"></fa><fa icon="sort-down" v-if="status.sortdown"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortStartDate()">Start Date<fa icon="sort" v-if="startDate.sort == true"></fa><fa icon="sort-up" v-if="startDate.sortup == true"></fa><fa icon="sort-down" v-if="startDate.sortdown"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortEndDate()">End Date<fa icon="sort" v-if="endDate.sort == true"></fa><fa icon="sort-up" v-if="endDate.sortup == true"></fa><fa icon="sort-down" v-if="endDate.sortdown"></fa></a>
                                    </th>
                                    <th colspan="2">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item,index) in surveyReport.surveyReportList">
                                    <td>
                                        {{item.surveyTitle}}
                                    </td>
                                    <td>
                                        {{item.respondent}}
                                    </td>
                                    <td>
                                        {{item.respondentRate.toPrecision(3)}}%
                                    </td>
                                    <td>
                                        {{item.status}}
                                    </td>
                                    <td>
                                        {{convertDateTime(item.startDate)}}
                                    </td>
                                    <td>
                                        {{convertDateTime(item.endDate)}}
                                    </td>
                                    <td class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click="detailClicked(index)">View Detail</button>
                                    </td>
                                    <td class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click="downloadExcel(item.surveyId)">Download</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="row justify-content-center">
                        <paginate :currentPage.sync="currentPage"
                                  :total-data=surveyReport.totalData
                                  :limit-data=10
                                  @update:currentPage="fetch()"></paginate>
                    </div>
                </div>
                <br />

            </div>

            <div class="col col-md-12" v-if="viewDetail === true">
                <h3 class="mb-3">
                    <fa icon="chart-bar"></fa>Report > Survey Report
                    <span class="talent_font_red">View Detail</span>
                </h3>

                <div v-if="url.length > 0" class="row mb-3" style="height:100vh">
                    <iframe width="100%"
                            height="100%"
                            :src="url"
                            frameborder="0"
                            allowfullscreen="true"></iframe>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { Sort } from '../../class/Sort';
    import { FileReportService } from '../../services/FileReportService';
    import { SurveyReportService, SurveyReportViewModel, SurveyReportModel } from '../../services/NSwagService';
    @Component({
        created: async function (this: SurveyReport) {
            this.isBusy = true;
            await this.fetch();
            //await this.initDropdownData();
        }
    })
    export default class SurveyReport extends Vue {
        isBusy: boolean = false;
        view: boolean = true;
        viewDetail: boolean = false;


        // ---------------------------------------- FETCH -----------------------------------------

        surveyReportMan: SurveyReportService = new SurveyReportService();

        statusDropdown: string[] = ["On Going", "Done"];

        surveyReport: SurveyReportViewModel = {
            surveyReportList: null,
            totalData: null
        };

        currentPage: number = 1;
        filterDate = {
            start: null,
            end: null
        };
        filterSurveyTitle: string = '';
        filterRespondent: number = null;
        filterRespondentRate: number = null;
        filterStatus: string = '';
        sortBy: string = '';

        async fetch() {
            this.isBusy = true;
            this.surveyReport = await this.surveyReportMan.getAllSurveyReport(this.filterSurveyTitle, this.filterRespondent, this.filterRespondentRate, this.filterStatus, this.filterDate.start, this.filterDate.end, this.sortBy, this.currentPage);
            this.isBusy = false;
        };

        convertDateTime(stringdate) {
            if (stringdate === null) {
                return "-"
            }
            var date = new Date(stringdate);
            function pad(n) { return n < 10 ? "0" + n : n; }
            return pad(date.getDate()) + "/" + pad(date.getMonth() + 1) + "/" + date.getFullYear();
        }

        // ---------------------------------------- Sorting ------------------------------------------

        //Variable untuk sorting
        surveyTitle = new Sort();
        respondent = new Sort();
        respondentRate = new Sort();
        status = new Sort();
        startDate = new Sort();
        endDate = new Sort();

        //Reset
        reset() {
            this.filterDate = {
                start: null,
                end: null
            };
            this.filterSurveyTitle = '';
            this.filterRespondent = null;
            this.filterRespondentRate = null;
            this.filterStatus = '';
            this.sortBy = '';
            this.fetch();
        }

        //ClickSortSurveyTitle
        async ClickSortSurveyTitle() {
            this.ResetSort('surveyTitle');
            //Sorting
            this.surveyTitle.sorting();
            //Function Sorting
            if (this.surveyTitle.sortup == true) {
                this.sortBy = 'surveyTitle';
            }
            else if (this.surveyTitle.sortdown == true) {
                this.sortBy = 'surveyTitleDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortRespondent
        async ClickSortRespondent() {
            this.ResetSort('respondent');
            //Sorting
            this.respondent.sorting();
            //Function Sorting

            if (this.respondent.sortup == true) {
                this.sortBy = 'respondent';
            }
            else if (this.respondent.sortdown == true) {
                this.sortBy = 'respondentDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortRespondentRate
        async ClickSortRespondentRate() {
            this.ResetSort('respondentRate');
            //Sorting
            this.respondentRate.sorting();
            //Function Sorting

            if (this.respondentRate.sortup == true) {
                this.sortBy = 'respondentRate';
            }
            else if (this.respondentRate.sortdown == true) {
                this.sortBy = 'respondentRateDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortStatus
        async ClickSortStatus() {
            this.ResetSort('status');
            //Sorting
            this.status.sorting();
            //Function Sorting

            if (this.status.sortup == true) {
                this.sortBy = 'status';
            }
            else if (this.status.sortdown == true) {
                this.sortBy = 'statusDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortStartDate
        async ClickSortStartDate() {
            this.ResetSort('startDate');
            //Sorting
            this.startDate.sorting();
            //Function Sorting

            if (this.startDate.sortup == true) {
                this.sortBy = 'startDate';
            }
            else if (this.startDate.sortdown == true) {
                this.sortBy = 'startDateDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortCreatedDate
        async ClickSortEndDate() {
            this.ResetSort('endDate');
            //Sorting
            this.endDate.sorting();
            //Function Sorting

            if (this.endDate.sortup == true) {
                this.sortBy = 'endDate';
            }
            else if (this.endDate.sortdown == true) {
                this.sortBy = 'endDateDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'surveyTitle':
                    this.respondent.reset();
                    this.respondentRate.reset();
                    this.status.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'respondent':
                    this.surveyTitle.reset();
                    this.respondentRate.reset();
                    this.status.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'respondentRate':
                    this.surveyTitle.reset();
                    this.respondent.reset();
                    this.status.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'status':
                    this.surveyTitle.reset();
                    this.respondent.reset();
                    this.respondentRate.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'startDate':
                    this.surveyTitle.reset();
                    this.respondent.reset();
                    this.respondentRate.reset();
                    this.status.reset();
                    this.endDate.reset();
                    return;
                case 'endDate':
                    this.surveyTitle.reset();
                    this.respondent.reset();
                    this.respondentRate.reset();
                    this.status.reset();
                    this.startDate.reset();
                    return;
            }
        }

        //download excel
        downloadExcel(surveyId) {
            this.isBusy = true;
            //var response = await this.surveyReportMan.generateExcel(surveyId);
            FileReportService.surveyReportDownloadExcelService(surveyId);
            this.isBusy = false;
        }

        //view detail
        url: string = '';

        detailClicked(index: number) {
            this.viewDetail = true;
            this.view = false;

            this.url = this.surveyReport.surveyReportList[index].urlDetail;
        }

        closeClicked() {
            this.view = true;
            this.viewDetail = false;
            this.url = '';
        }


    }
</script>
