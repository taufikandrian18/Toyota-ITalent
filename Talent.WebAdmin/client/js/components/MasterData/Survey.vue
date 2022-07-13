<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div v-if="add == false && edit == false && view == false" class="row">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="database"></fa> Master Data > <span class="talent_font_red">Survey</span></h3>
                <br />
                <div v-if="successMessageShow == true" class="alert alert-success alert-dismissible fade show" role="alert">
                    {{successMessage}}
                    <button type="button" class="close" data-dismiss="alert" @click.prevent="ResetSuccessMessage()" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Survey</h4>

                    <br />

                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker v-model="filterSurvey.Date" class="v-date-style-report" mode="range" :firstDayOfWeek="2" :value="null" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Survey Title</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input v-model="filterSurvey.Title" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Status</span>
                                </div>
                                <div class="col-md-8">
                                    <select v-model="filterSurvey.ApprovalId" class="form-control">
                                        <option v-for="s in searchApproval" :value="s.approvalId">{{s.approvalName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <!--<div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Position</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input v-model="filterSurvey.Position" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>-->
                    </div>

                    <br />

                    <!--<div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Outlet</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input v-model="filterSurvey.Outlet" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Status</span>
                                </div>
                                <div class="col-md-8">
                                    <select v-model="filterSurvey.ApprovalId" class="form-control">
                                        <option v-for="s in searchApproval" :value="s.approvalId">{{s.approvalName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>-->
                    <!--<br />-->
                    <!--Search Button-->
                    <div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="d-flex align-items-end flex-column">
                                    <div>
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="FetchData()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="Reset()">
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
                    <h4>Survey List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a v-if="(survey.totalData - ((currentPage - 1) * pageSize) ) >= pageSize">Showing {{pageSize}} of {{survey.totalData}} Entry(s)</a>
                            <a v-else>Showing {{(survey.totalData) - ((currentPage - 1) * pageSize)}} of {{survey.totalData}} Entry(s)</a>

                            <button v-if="crud.create" @click.prevent="addClicked()" class="btn talent_bg_cyan talent_roundborder talent_font_white">Add Survey</button>
                        </div>
                    </div>


                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortSurveyTitle()">Survey Title <fa icon="sort" v-if="surveyTitle.sort == true"></fa><fa icon="sort-up" v-if="surveyTitle.sortup == true"></fa><fa icon="sort-down" v-if="surveyTitle.sortdown == true"></fa></a>
                                    </th>
                                    <!--<th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortPosition()">Position <fa icon="sort" v-if="position.sort == true"></fa><fa icon="sort-up" v-if="position.sortup == true"></fa><fa icon="sort-down" v-if="position.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortOutlet()">Outlet <fa icon="sort" v-if="outlet.sort == true"></fa><fa icon="sort-up" v-if="outlet.sortup == true"></fa><fa icon="sort-down" v-if="outlet.sortdown == true"></fa></a>
                                    </th>-->
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortStatus()">Status <fa icon="sort" v-if="status.sort == true"></fa><fa icon="sort-up" v-if="status.sortup == true"></fa><fa icon="sort-down" v-if="status.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="3">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="s in survey.data">
                                    <td>
                                        {{s.title}}
                                    </td>
                                    <!--<td>
                                        {{s.position}}
                                    </td>
                                    <td>
                                        {{s.outlet}}
                                    </td>-->
                                    <td>
                                        {{s.status}}
                                    </td>
                                    <td>
                                        {{s.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{s.updatedAt | dateFormat}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="GetSurveyIdViewData(s.surveyId)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" :disabled="s.status == 'Waiting for Approval'? true: false" @click.prevent="GetSurveyId(s.surveyId)">Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" :disabled="s.status == 'Waiting for Approval'? true: false" data-toggle="modal" data-target="#modalDelete" @click.prevent="GetDelete(s.surveyId)">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="col-md-12 d-flex justify-content-center mt-3">
                        <paginate :currentPage.sync="currentPage" :total-data=survey.totalData :limit-data=pageSize @update:currentPage="FetchData()"></paginate>
                    </div>
                </div>
                <br />
            </div>
        </div>

        <!--ADD-->
        <div v-if="add == true" class="row">
            <div class="col col-md-12">
                <!--Add User Privilege-->
                <SurveyAdd :add.sync="add" :success-message-show.sync="successMessageShow" :success-message.sync="successMessage" @update:add="FetchData()"></SurveyAdd>
            </div>
            <br />
        </div>

        <!--Edit-->
        <div v-if="edit == true" class="row">
            <div class="col col-md-12">
                <!--Edit User Privilege-->
                <SurveyEdit :edit.sync="edit" :survey-id="editId" :success-message-show.sync="successMessageShow" :success-message.sync="successMessage" @update:edit="FetchData()"></SurveyEdit>
            </div>
            <br />
        </div>

        <!--View Detail-->
        <div v-if="view == true" class="row">
            <div class="col col-md-12">
                <!--View User Privilege-->
                <SurveyView :view.sync="view" :type="type" :survey-id="viewId" :success-message-show.sync="successMessageShow" :success-message.sync="successMessage" @update:view="FetchData()"></SurveyView>
            </div>
            <br />
        </div>

        <!-- Delete Popup-->
        <div class="modal fade" id="modalDelete" tabindex="-1" role="dialog">
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
                                <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="Delete()">Delete</button>
                                <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { Sort } from '../../class/Sort';
    import { SurveysService, SurveysPaginate, ApprovalStatusModel, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService'
    import SurveyAdd from '../MasterData/SurveyAdd.vue'
    import SurveyEdit from '../MasterData/SurveyEdit.vue'
    import SurveyView from '../MasterData/SurveyViewDetail.vue'
    import Message from '../../class/Message';

    @Component({
        props: ['type', 'id'],
        components: {
            SurveyAdd,
            SurveyEdit,
            SurveyView
        },
        created: async function (this: Survey) {
            this.isBusy = true
            await this.getAccess()
            await this.FetchData()
            await this.GetAllApproval()
        }
    })
    export default class Survey extends Vue {
        isBusy: boolean = false;

        type: string;
        id: number;

        //API
        APISurvey: SurveysService = new SurveysService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        //Pagination
        currentPage: number = 1;
        pageSize: number = 10;

        //Control
        add: boolean = false;
        edit: boolean = false;
        view: boolean = false;

        //Success Message
        successMessageShow: boolean = false;
        successMessage: string = ""

        ResetSuccessMessage() {
            this.successMessageShow = false;
            this.successMessage = "";
        }

        addClicked() {
            this.add = true;
            this.resetSortingAndAdvanceSearch();
        }

        //DatePicker
        props: any = {
            placeholder: "",
            readonly: true
        };

        looping: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        //Survey
        survey: SurveysPaginate = { data: null, totalData: null }
        //Filter
        filterSurvey: ISurveyFilter = {
            ApprovalId: null, Date: { start: null, end: null }, Outlet: '', Position: '', Title: '', PageIndex: this.currentPage, PageSize: this.pageSize, SortBy: null
        }
        searchApproval: ApprovalStatusModel[] = []

        //privilege
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("SVY");
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        //GetAllSurvey
        async FetchData() {
            if (this.type != undefined) {
                if (this.type.toLowerCase() == "fromoutside") {
                    this.GetSurveyIdViewData(this.id);
                }
            }

            this.isBusy = true
            var data = await this.APISurvey.getAllSurveyPaginateAsync(this.filterSurvey.Date.start, this.filterSurvey.Date.end, this.filterSurvey.Title, this.filterSurvey.Position, this.filterSurvey.Outlet, this.filterSurvey.ApprovalId, this.filterSurvey.SortBy, this.currentPage, this.pageSize)
            this.survey.data = data.data
            this.survey.totalData = data.totalData
            this.isBusy = false
        }

        //Edit
        editId: number = null
        GetSurveyId(id: number) {
            //console.log(id)
            this.editId = id
            this.edit = true;

            this.resetSortingAndAdvanceSearch();

        }

        //View Detail
        viewId: number = null
        GetSurveyIdViewData(id: number) {
            this.viewId = id
            this.view = true;

            this.Reset();
        }

        //Get Delete Id
        deleteId: number = null
        GetDelete(id: number) {
            this.deleteId = id;
        }

        async Reset() {
            this.resetSortingAndAdvanceSearch();

            await this.FetchData();
        }

        resetSortingAndAdvanceSearch() {
            this.filterSurvey = {
                ApprovalId: null, Date: { start: null, end: null }, Outlet: '', Position: '', Title: '', PageIndex: this.currentPage, PageSize: this.pageSize, SortBy: null
            }

            this.ResetSort('');
        }

        async GetAllApproval() {
            this.isBusy = true
            var data = await this.APISurvey.getAllApprovalStatus()
            this.searchApproval = data
            this.isBusy = false
        }

        //SORTING
        //Variable untuk sorting
        surveyTitle = new Sort();
        position = new Sort();
        outlet = new Sort();
        status = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortSurveyTitle
        async ClickSortSurveyTitle() {
            this.ResetSort('surveyTitle');
            //Sorting
            this.surveyTitle.sorting();
            //Function Sorting
            if (this.surveyTitle.sortup == true) {
                this.filterSurvey.SortBy = "TitleAsc";
            } else if (this.surveyTitle.sortdown == true) {
                this.filterSurvey.SortBy = "TitleDesc"
            }
            else {
                this.filterSurvey.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortPosition
        async ClickSortPosition() {
            this.ResetSort('position');
            //Sorting
            this.position.sorting();
            //Function Sorting
            if (this.position.sortup == true) {
                this.filterSurvey.SortBy = "PositionAsc";
            } else if (this.position.sortdown == true) {
                this.filterSurvey.SortBy = "PositionDesc"
            }
            else {
                this.filterSurvey.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortOutlet
        async ClickSortOutlet() {
            this.ResetSort('outlet');
            //Sorting
            this.outlet.sorting();
            //Function Sorting
            if (this.outlet.sortup == true) {
                this.filterSurvey.SortBy = "OutletAsc";
            } else if (this.outlet.sortdown == true) {
                this.filterSurvey.SortBy = "OutletDesc"
            }
            else {
                this.filterSurvey.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortStatus
        async ClickSortStatus() {
            this.ResetSort('status');
            //Sorting
            this.status.sorting();
            //Function Sorting
            if (this.status.sortup == true) {
                this.filterSurvey.SortBy = "StatusAsc";
            } else if (this.status.sortdown == true) {
                this.filterSurvey.SortBy = "StatusDesc"
            }
            else {
                this.filterSurvey.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting
            if (this.createdDate.sortup == true) {
                this.filterSurvey.SortBy = "CreatedDateAsc";
            } else if (this.createdDate.sortdown == true) {
                this.filterSurvey.SortBy = "CreatedDateDesc"
            }
            else {
                this.filterSurvey.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting
            if (this.updatedDate.sortup == true) {
                this.filterSurvey.SortBy = "UpdatedDateAsc";
            } else if (this.updatedDate.sortdown == true) {
                this.filterSurvey.SortBy = "UpdatedDateDesc"
            }
            else {
                this.filterSurvey.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'surveyTitle':
                    this.position.reset();
                    this.outlet.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'position':
                    this.surveyTitle.reset();
                    this.outlet.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'outlet':
                    this.surveyTitle.reset();
                    this.position.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset()
                    return;
                case 'status':
                    this.surveyTitle.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.surveyTitle.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.status.reset();
                    this.updatedDate.reset()
                    return;
                case 'updatedDate':
                    this.surveyTitle.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    return;
                default:
                    this.filterSurvey.SortBy = "";
                    this.updatedDate.reset();
                    this.surveyTitle.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    return;
            }
        }

        //delete survey
        async Delete() {
            if (!this.crud.delete) {
                return
            }
            this.isBusy = true;
            await this.APISurvey.deleteSurvey(this.deleteId);
            this.isBusy = false;
            this.FetchData();
            this.successMessage = Message.RemoveMessage;
            this.successMessageShow = true;
        }

    }
</script>
