<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="row">
            <div class="col-md-12">
                <!--TITLE-->
                <h3 v-if="(viewCourse == false && viewCoach == false) && url.length == 0">
                    <fa icon="chart-bar"></fa> Report >
                    <span class="talent_font_red">Report NPS</span>
                </h3>
                <h3 v-if="(viewCourse == true && viewCoach == false) && url.length > 0">
                    <fa icon="chart-bar"></fa> Report > Report NPS >
                    <span class="talent_font_red">View Course</span>
                </h3>
                <h3 v-if="(viewCourse == false && viewCoach == true) && url.length > 0">
                    <fa icon="chart-bar"></fa> Report > Report NPS >
                    <span class="talent_font_red">View Coach</span>
                </h3>
                <br />
            </div>
        </div>

        <!--View-->
        <div class="row" v-if="viewCourse == false && viewCoach == false">
            <div class="col col-md-12">
                <!--ADVANCE SEARCH-->
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Search Training NPS Report</h4>

                    <br />
                    <!--4 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker
                                            class="v-date-style-report"
                                            mode="range"
                                            :firstDayOfWeek="2"
                                            v-model="filterDate"
                                            :masks="{ input: 'DD/MM/YYYY' }"
                                        ></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text">
                                                <fa icon="calendar-alt"></fa>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Program Type</span>
                                </div>
                                <div class="col-md-8">
                                    <select class="form-control" v-model="filterProgramTypeId">
                                        <option
                                            v-for="p in programTypes.listProgramTypeModel"
                                            v-bind:key="p.programTypeId"
                                            :value="p.programTypeName"
                                        >{{p.programTypeName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Status</span>
                                </div>
                                <div class="col-md-8">
                                    <select class="form-control" v-model="filterStatus">
                                        <option
                                            v-for="status in statuses"
                                            v-bind:key="status"
                                            :value="status"
                                        >{{status}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--4 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Course Name</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="filterCourseName"
                                            type="text"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Batch</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input
                                            min="1"
                                            class="form-control"
                                            v-model="filterBatch"
                                            type="number"
                                        />
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
                                        <button
                                            class="btn talent_bg_blue talent_font_white"
                                            @click.prevent="fetch"
                                        >Search</button>
                                        <button
                                            class="btn talent_bg_red talent_font_white"
                                            @click.prevent="reset"
                                        >Reset</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <br />

                <!--Table-->
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Training NPS Report List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{npsReports.reportNPSItems == null ? 0 : npsReports.reportNPSItems.length}} of {{npsReports.totalData}} Entry(s)</a>
                        </div>
                    </div>
                    <div class="talent_overflowx">
                        <table
                            class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter"
                        >
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="ClickSortCourseName()"
                                        >
                                            Course Name
                                            <fa icon="sort" v-if="courseName.sort == true"></fa>
                                            <fa icon="sort-up" v-if="courseName.sortup == true"></fa>
                                            <fa icon="sort-down" v-if="courseName.sortdown == true"></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="ClickSortProgramType()"
                                        >
                                            Program Type
                                            <fa icon="sort" v-if="programType.sort == true"></fa>
                                            <fa icon="sort-up" v-if="programType.sortup == true"></fa>
                                            <fa
                                                icon="sort-down"
                                                v-if="programType.sortdown == true"
                                            ></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="ClickSortBatch()"
                                        >
                                            Batch
                                            <fa icon="sort" v-if="batchSort.sort == true"></fa>
                                            <fa icon="sort-up" v-if="batchSort.sortup == true"></fa>
                                            <fa icon="sort-down" v-if="batchSort.sortdown == true"></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="ClickSortStatus()"
                                        >
                                            Status
                                            <fa icon="sort" v-if="statusSort.sort == true"></fa>
                                            <fa icon="sort-up" v-if="statusSort.sortup == true"></fa>
                                            <fa icon="sort-down" v-if="statusSort.sortdown == true"></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="ClickSortStartDate()"
                                        >
                                            Start Date
                                            <fa icon="sort" v-if="startDateSort.sort == true"></fa>
                                            <fa icon="sort-up" v-if="startDateSort.sortup == true"></fa>
                                            <fa
                                                icon="sort-down"
                                                v-if="startDateSort.sortdown == true"
                                            ></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="ClickSortEndDate()"
                                        >
                                            End Date
                                            <fa icon="sort" v-if="endDateSort.sort == true"></fa>
                                            <fa icon="sort-up" v-if="endDateSort.sortup == true"></fa>
                                            <fa
                                                icon="sort-down"
                                                v-if="endDateSort.sortdown == true"
                                            ></fa>
                                        </a>
                                    </th>
                                    <th colspan="3">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr
                                    v-for="(report)  in npsReports.reportNPSItems"
                                    v-bind:key="report.trainingId"
                                >
                                    <td>{{report.courseName}}</td>
                                    <td>{{report.programTypeName}}</td>
                                    <td>{{report.batch}}</td>
                                    <td>{{report.status}}</td>
                                    <td>{{convertDateTime(report.startDate)}}</td>
                                    <td>{{convertDateTime(report.endDate)}}</td>
                                    <td class="talent_nopadding_important d-flex justify-content-center">
                                        <button
                                            class="btn talent_bg_lightgreen talent_font_white mx-2"
                                            @click.prevent="onClickViewCourse(report)"
                                        >View Course</button>
                                        <button
                                            class="btn talent_bg_cyan talent_font_white mx-2"
                                            @click.prevent="onClickViewCoach(report)"
                                        >View Coach</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--Pagination-->
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate
                            :currentPage.sync="currentPage"
                            :total-data="npsReports.totalData"
                            :limit-data="itemPerPage"
                            @update:currentPage="fetch()"
                        ></paginate>
                    </div>
                </div>

                <div
                    class="modal fade"
                    id="exampleModalCenter"
                    tabindex="-1"
                    role="dialog"
                    aria-labelledby="exampleModalCenterTitle"
                    aria-hidden="true"
                >
                    <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                        <div class="modal-content">
                            <div class="modal-header">
                                <div class="col-md-12 justify-content-center d-flex">
                                    <h5>Are you sure want to delete?</h5>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <div class="col-md-12 d-flex justify-content-center">
                                    <div class="col-md-12 d-flex justify-content-around">
                                        <button
                                            class="btn talent_bg_red talent_font_white talent_roundborder"
                                            type="button"
                                            data-dismiss="modal"
                                            @click.prevent="deleteData()"
                                        >Delete</button>
                                        <button
                                            class="btn talent_bg_blue talent_font_white talent_roundborder"
                                            type="button"
                                            data-dismiss="modal"
                                        >Close</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--IFrame-->
        <div
            v-if="(url.length > 0) && ( viewCoach==true || viewCourse==true )"
            class="row"
            style="height:100vh"
        >
            <iframe width="100%" height="100%" :src="url" frameborder="0" allowfullscreen="true"></iframe>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { Sort } from '../../class/Sort';
import {
    ProgramTypeService,
    ProgramTypeViewModel,
    UserPrivilegeSettingsService,
    UserAccessCRUD,
    ReportNPSViewModel,
    ReportNpsService,
    ReportNPSItemModel
} from '../../services/NSwagService';

@Component({
    created: async function(this: ReportNPS) {
        this.isBusy = true;
        await this.getAccess();
        await this.initDropdownData();
        await this.fetch();
    }
})
export default class ReportNPS extends Vue {
    url: string = '';
    privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
    async getAccess() {
        var data = await this.privilegeApi.crudAccessPage('RPT');
        this.crud = data;
    }
    crud: UserAccessCRUD = {
        create: false,
        delete: false,
        read: false,
        update: false
    };

    isBusy: boolean = false;
    itemPerPage: number = 10;
    fromOutside: boolean;

    // ---------------------------------------- FETCH -----------------------------------------

    currentPage: number = 1;
    filterDate = {
        start: null,
        end: null
    };
    filterCourseName: string = '';
    filterProgramTypeId: number = null;
    filterBatch: number = null;
    filterStatus: string = '';
    sortBy: string = '';
    reportMan: ReportNpsService = new ReportNpsService();
    programTypeMan: ProgramTypeService = new ProgramTypeService();
    npsReports: ReportNPSViewModel = {
        reportNPSItems: null,
        totalData: null
    };
    programTypes: ProgramTypeViewModel = {};
    statuses = ['On Going', 'Done'];
    pricingTypes = ['Pay', 'Free'];

    async fetch() {
        this.isBusy = true;
        this.npsReports = await this.reportMan.getAllReportNps(
            this.itemPerPage,
            this.filterDate.start,
            this.filterDate.end,
            this.filterCourseName,
            this.filterProgramTypeId,
            this.filterStatus,
            this.filterBatch,
            this.sortBy,
            this.currentPage
        );
        this.isBusy = false;
    }

    async initDropdownData() {
        this.programTypes = await this.programTypeMan.getAllProgramTypes();
    }
    reset() {
        this.filterDate = {
            start: null,
            end: null
        };
        this.currentPage = 1;
        this.filterCourseName = '';
        this.filterProgramTypeId = null;
        this.filterBatch = null;
        this.filterStatus = '';
        this.fetch();
    }

    convertDateTime(stringdate) {
        var date = new Date(stringdate);
        function pad(n) {
            return n < 10 ? '0' + n : n;
        }
        return (
            pad(date.getDate()) +
            '/' +
            pad(date.getMonth() + 1) +
            '/' +
            date.getFullYear()
        );
    }

    viewCourse: boolean = false;
    viewCoach: boolean = false;

    async onClickViewCourse(report: ReportNPSItemModel) {
        this.viewCourse = true;
        this.viewCoach = false;
        this.url = report.viewCourseUrl;
    }
    async onClickViewCoach(report: ReportNPSItemModel) {
        this.viewCourse = false;
        this.viewCoach = true;
        this.url = report.viewCoachUrl;
    }

    closeClicked() {
        this.viewCourse = false;
        this.viewCoach = false;
        this.url = '';
        //this.detail = false;

        //this.fileType = true;
        //this.fileSize = true;
        //this.imageSize = true;
        //this.imageUpload = null;
    }

    backPage() {
        window.history.back();
    }

    // ---------------------------------------- Sorting ------------------------------------------

    //Variable untuk sorting
    courseName = new Sort();
    programType = new Sort();
    batchSort = new Sort();
    statusSort = new Sort();
    startDateSort = new Sort();
    endDateSort = new Sort();

    //ClickSortCourseName
    async ClickSortCourseName() {
        this.ResetSort('courseName');
        //Sorting
        this.courseName.sorting();
        //Function Sorting
        if (this.courseName.sortup == true) {
            this.sortBy = 'courseName';
        } else if (this.courseName.sortdown == true) {
            this.sortBy = 'courseNameDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortProgramType
    async ClickSortProgramType() {
        this.ResetSort('programType');
        //Sorting
        this.programType.sorting();
        //Function Sorting

        if (this.programType.sortup == true) {
            this.sortBy = 'programType';
        } else if (this.programType.sortdown == true) {
            this.sortBy = 'programTypeDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortLearningType
    async ClickSortBatch() {
        this.ResetSort('batch');
        //Sorting
        this.batchSort.sorting();
        //Function Sorting

        if (this.batchSort.sortup == true) {
            this.sortBy = 'batch';
        } else if (this.batchSort.sortdown == true) {
            this.sortBy = 'batchDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortLevel
    async ClickSortStatus() {
        this.ResetSort('status');
        //Sorting
        this.statusSort.sorting();
        //Function Sorting

        if (this.statusSort.sortup == true) {
            this.sortBy = 'status';
        } else if (this.statusSort.sortdown == true) {
            this.sortBy = 'statusDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortPricing
    async ClickSortStartDate() {
        this.ResetSort('startDate');
        //Sorting
        this.startDateSort.sorting();
        //Function Sorting

        if (this.startDateSort.sortup == true) {
            this.sortBy = 'startDate';
        } else if (this.startDateSort.sortdown == true) {
            this.sortBy = 'startDateDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortApprovalStatus
    async ClickSortEndDate() {
        this.ResetSort('endDate');
        //Sorting
        this.endDateSort.sorting();
        //Function Sorting

        if (this.endDateSort.sortup == true) {
            this.sortBy = 'endDate';
        } else if (this.endDateSort.sortdown == true) {
            this.sortBy = 'endDateDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //Reset Sorting
    async ResetSort(parameter: string) {
        switch (parameter) {
            case 'courseName':
                this.programType.reset();
                this.statusSort.reset();
                this.startDateSort.reset();
                this.batchSort.reset();
                this.endDateSort.reset();
                return;
            case 'programType':
                this.courseName.reset();
                this.statusSort.reset();
                this.startDateSort.reset();
                this.batchSort.reset();
                this.endDateSort.reset();
                return;
            case 'batch':
                this.courseName.reset();
                this.programType.reset();
                this.statusSort.reset();
                this.startDateSort.reset();
                this.endDateSort.reset();
                return;
            case 'status':
                this.courseName.reset();
                this.programType.reset();
                this.startDateSort.reset();
                this.batchSort.reset();
                this.endDateSort.reset();
                return;
            case 'startDate':
                this.courseName.reset();
                this.programType.reset();
                this.statusSort.reset();
                this.batchSort.reset();
                this.endDateSort.reset();
                return;
            case 'endDate':
                this.courseName.reset();
                this.programType.reset();
                this.statusSort.reset();
                this.startDateSort.reset();
                this.batchSort.reset();
                return;
        }
    }
}
</script>
