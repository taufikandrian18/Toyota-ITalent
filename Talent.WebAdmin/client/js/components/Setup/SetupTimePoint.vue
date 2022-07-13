<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="row" v-if="add == false && edit == false && viewDetail == false">
            <div class="col-md-12">
                <h3 class="mb-4" v-if="add == false && edit == false"><fa icon="folder"></fa> Setup > <span class="talent_font_red">Setup Time Point</span></h3>
                <div class="mb-4"></div>

                <div v-if="successMessageShow == true" class="alert alert-success alert-dismissible fade show" role="alert">
                    {{successMessage}}
                    <button type="button" class="close" data-dismiss="alert" @click.prevent="ResetSuccessMessage()" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <div v-if="errorMessageShow == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                    {{errorMessage}}
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Time Point</h4>

                    <div class="mb-4"></div>
                    <!--3 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker v-model="filterTimePoints.Date" class="v-date-style-report" mode="range" :firstDayOfWeek="2" :value="null" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                    <span>Time (Minutes)</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input type="number" v-model="filterTimePoints.Time" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Points</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input type="number" v-model="filterTimePoints.Points" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mb-4"></div>

                    <!--3 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Type of Point</span>
                                </div>
                                <div class="col-md-8">
                                    <select v-model="filterTimePoints.TypeofPoints" class="form-control">
                                        <option v-for="point in pointTypes" :value="point.pointTypeName">{{point.pointTypeName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Score</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input type="number" v-model="filterTimePoints.Score" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mb-4"></div>

                    <!--Search Button-->
                    <div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="d-flex align-items-end flex-column">
                                    <div>
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="FetchData()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="reset()">
                                            Reset
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <div class="mb-4"></div>

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Time Point List</h4>

                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a v-if="(SetupTimePoints.totalData - ((currentPage - 1) * pageSize) ) >= pageSize">Showing {{pageSize}} of {{SetupTimePoints.totalData}} Entry(s)</a>
                            <a v-else>Showing {{(SetupTimePoints.totalData) - ((currentPage - 1) * pageSize)}} of {{SetupTimePoints.totalData}} Entry(s)</a>
                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="AddClicked()">Add Time Point</button>
                        </div>
                    </div>


                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortProgramType()">Type of Points <fa icon="sort" v-if="typeofPoint .sort == true"></fa><fa icon="sort-up" v-if="typeofPoint .sortup == true"></fa><fa icon="sort-down" v-if="typeofPoint.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortLearningType()">Time (Minutes) <fa icon="sort" v-if="time .sort == true"></fa><fa icon="sort-up" v-if="time .sortup == true"></fa><fa icon="sort-down" v-if="time.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortLevel()">Score <fa icon="sort" v-if="score.sort == true"></fa><fa icon="sort-up" v-if="score.sortup == true"></fa><fa icon="sort-down" v-if="score.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortPricing()">Points <fa icon="sort" v-if="points.sort == true"></fa><fa icon="sort-up" v-if="points.sortup == true"></fa><fa icon="sort-down" v-if="points.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="3" v-if="crud.delete || crud.read || crud.update">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="TimePoint in SetupTimePoints.data">
                                    <td>
                                        {{TimePoint.pointTypeName}}
                                    </td>
                                    <td>
                                        {{TimePoint.time}}
                                    </td>
                                    <td>
                                        {{TimePoint.score}}
                                    </td>
                                    <td>
                                        {{TimePoint.points}}
                                    </td>
                                    <td>
                                        {{TimePoint.createdDate | dateFormat}}
                                    </td>
                                    <td>
                                        {{TimePoint.updatedDate | dateFormat}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="ViewDetailInitial(TimePoint.timePointId)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="EditInitial(TimePoint.timePointId)">Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#modalDelete" @click.prevent="GetDelete(TimePoint.timePointId)">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <div class="col-md-12 d-flex justify-content-center mt-3">
                        <paginate :currentPage.sync="currentPage" :total-data=SetupTimePoints.totalData :limit-data=pageSize @update:currentPage="FetchData()"></paginate>
                    </div>

                </div>

                <div class="mb-4"></div>
            </div>
        </div>

        <!--Add-->
        <div class="row" v-if="add == true">

            <div class="col-md-12">
                <h3><fa icon="folder"></fa> Setup > Setup Time Point > <span class="talent_font_red">Add Time Point</span></h3>
                <div class="mb-4"></div>

                <div v-if="errorMessageShow == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                    {{errorMessage}}
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Time Point Information</h4>
                    <div class="mb-4"></div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-2">
                                    <label>Type of Points<span class="talent_font_red">*</span></label>
                                </div>
                                <div v-if="addTimePoints.pointTypeId != null && (addTimePoints.pointTypeId == 1 || addTimePoints.pointTypeId == 2 || addTimePoints.pointTypeId == 3)" class="col-md-10">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>Time (Minutes)<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>Points<span class="talent_font_red">*</span></label>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-10" v-if="addTimePoints.pointTypeId != null && (addTimePoints.pointTypeId != 1 && addTimePoints.pointTypeId != 2 && addTimePoints.pointTypeId != 3)">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>Score<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>Points<span class="talent_font_red">*</span></label>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-2">
                                    <select v-model="addTimePoints.pointTypeId" class="form-control">
                                        <option value="" hidden disabled>-</option>
                                        <option v-for="point in pointTypes" :value="point.pointTypeId">{{point.pointTypeName}}</option>
                                    </select>
                                </div>
                                <div class="col-md-10" v-if="addTimePoints.pointTypeId != null && (addTimePoints.pointTypeId == 1 || addTimePoints.pointTypeId == 2 || addTimePoints.pointTypeId == 3)">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <input type="number" min="0" v-model="addTimePoints.time" class="form-control" />
                                        </div>
                                        <div class="col-md-9">
                                            <input type="number" min="0" v-model="addTimePoints.points" class="form-control" placeholder="Point" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-10" v-if="addTimePoints.pointTypeId != null && (addTimePoints.pointTypeId != 1 && addTimePoints.pointTypeId != 2 && addTimePoints.pointTypeId != 3)">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <input type="number" min="0" v-model="addTimePoints.score" class="form-control" placeholder="Score" />
                                        </div>
                                        <div class="col-md-9">
                                            <input type="number" min="0" v-model="addTimePoints.points" class="form-control" placeholder="Point" />
                                        </div>
                                    </div>
                                </div>

                            </div>


                        </div>
                    </div>
                </div>
                <div class="mb-4"></div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="Close()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="Add()">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mb-4"></div>
            </div>
        </div>
        <!--Edit-->
        <div class="row" v-if="edit == true">

            <div class="col-md-12">
                <h3><fa icon="folder"></fa> Setup > Setup Time Point > <span class="talent_font_red">Edit Time Point</span></h3>
                <div class="mb-4"></div>

                <div v-if="errorMessageShow == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                    {{errorMessage}}
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Time Point Information</h4>
                    <div class="mb-4"></div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-2">
                                    <label>Type of Points<span class="talent_font_red">*</span></label>
                                </div>
                                <div v-if="editTimePoints.pointTypeId != null && (editTimePoints.pointTypeId == 1 || editTimePoints.pointTypeId == 2 || editTimePoints.pointTypeId == 3)" class="col-md-10">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>Time (Minutes)<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>Points<span class="talent_font_red">*</span></label>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-10" v-if="editTimePoints.pointTypeId != null && (editTimePoints.pointTypeId != 1 && editTimePoints.pointTypeId != 2 && editTimePoints.pointTypeId != 3)">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>Score<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>Points<span class="talent_font_red">*</span></label>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-2">
                                    <select disabled v-model="editTimePoints.pointTypeId" class="form-control">
                                        <option value="" hidden disabled>-</option>
                                        <option v-for="point in pointTypes" :value="point.pointTypeId">{{point.pointTypeName}}</option>
                                    </select>
                                </div>
                                <div class="col-md-10" v-if="editTimePoints.pointTypeId != null && (editTimePoints.pointTypeId == 1 || editTimePoints.pointTypeId == 2 || editTimePoints.pointTypeId == 3)">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <input type="number" min="0" v-model="editTimePoints.time" class="form-control" />
                                        </div>
                                        <div class="col-md-9">
                                            <input type="number" min="0" v-model="editTimePoints.points" class="form-control" placeholder="Point" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-10" v-if="editTimePoints.pointTypeId != null && (editTimePoints.pointTypeId != 1 && editTimePoints.pointTypeId != 2 && editTimePoints.pointTypeId != 3)">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <input type="number" min="0" v-model="editTimePoints.score" class="form-control" placeholder="Score" />
                                        </div>
                                        <div class="col-md-9">
                                            <input type="number" min="0" v-model="editTimePoints.points" class="form-control" placeholder="Point" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mb-4"></div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="Close()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="Edit()">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mb-4"></div>
            </div>


        </div>
        <!--View Detail-->
        <div class="row" v-if="viewDetail == true">

            <div class="col-md-12">
                <h3><fa icon="folder"></fa> Setup > Setup Time Point > <span class="talent_font_red">View Detail Time Point</span></h3>
                <div class="mb-4"></div>
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Time Point Information</h4>
                    <div class="mb-4"></div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-2">
                                    <label>Type of Points<span class="talent_font_red">*</span></label>
                                </div>
                                <div v-if="viewTimePoints.pointTypeId != null && (viewTimePoints.pointTypeId == 1 || viewTimePoints.pointTypeId == 2 || viewTimePoints.pointTypeId == 3)" class="col-md-10">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>Time (Minutes)<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>Points<span class="talent_font_red">*</span></label>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-10" v-if="viewTimePoints.pointTypeId != null && (viewTimePoints.pointTypeId != 1 && viewTimePoints.pointTypeId != 2 && viewTimePoints.pointTypeId != 3)">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <label>Score<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>Points<span class="talent_font_red">*</span></label>
                                        </div>
                                    </div>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-2">
                                    <select disabled v-model="viewTimePoints.pointTypeId" class="form-control">
                                        <option value="" hidden disabled>-</option>
                                        <option v-for="point in pointTypes" :value="point.pointTypeId">{{point.pointTypeName}}</option>
                                    </select>
                                </div>
                                <div class="col-md-10" v-if="viewTimePoints.pointTypeId != null && (viewTimePoints.pointTypeId == 1 || viewTimePoints.pointTypeId == 2 || viewTimePoints.pointTypeId == 3)">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <input disabled type="number" min="0" v-model="viewTimePoints.time" class="form-control" />
                                        </div>
                                        <div class="col-md-9">
                                            <input disabled type="number" min="0" v-model="viewTimePoints.points" class="form-control" placeholder="point" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-10" v-if="viewTimePoints.pointTypeId != null && (viewTimePoints.pointTypeId != 1 && viewTimePoints.pointTypeId != 2 && viewTimePoints.pointTypeId != 3)">
                                    <div class="row">
                                        <div class="col-md-3">
                                            <input disabled type="number" min="0" v-model="viewTimePoints.score" class="form-control" placeholder="score" />
                                        </div>
                                        <div class="col-md-9">
                                            <input disabled type="number" min="0" v-model="viewTimePoints.points" class="form-control" placeholder="point" />
                                        </div>
                                    </div>
                                </div>

                            </div>


                        </div>
                    </div>
                </div>
                <div class="mb-4"></div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="Close()">
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mb-4"></div>
            </div>
        </div>
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
    import { PointTypeService, SetupTimePointService, SetupTimePointPaginate, SetupTimePointInsertModel, PointTypeViewModel, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService'
    import Message from '../../class/Message'

    @Component({
        props: ['framework', 'compiler'],
        components: {

        },
        created: async function (this: SetupTimePoint) {
            this.isBusy = true;
            await this.getAccess();
            await this.GetAllPointType();
            await this.FetchData();
        }
    })
    export default class SetupTimePoint extends Vue {

        //API
        privilegeSettingsAPI: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async getAccess() {
            var data = await this.privilegeSettingsAPI.crudAccessPage("STPT");
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        isBusy: boolean = false;

        props: any = {
            placeholder: "",
            readonly: true
        };

        //API
        SetupTimePointApi: SetupTimePointService = new SetupTimePointService;
        PointTypeApi: PointTypeService = new PointTypeService;

        currentPage: number = 1;
        pageSize: number = 10;

        SetupTimePoints: SetupTimePointPaginate = { data: null, totalData: null }

        pointTypes: Array<PointTypeViewModel> = [];



        //Display
        async GetAllPointType() {
            let data = await this.PointTypeApi.getAllPointTypeAsync();
            this.pointTypes = data
        }

        filterTimePoints: ISetupTimePointModels = {
            Date: { start: null, end: null }, Points: null, Score: null, Time: null, TypeofPoints: "", PageIndex: this.currentPage, PageSize: this.pageSize, SortBy: "", PointTypeId: null
        };

        async FetchData() {
            this.isBusy = true;
            this.ResetErrorMessage();
            let data = await this.SetupTimePointApi.getAllSetupTimePointPaginateAsync(this.filterTimePoints.Date.start, this.filterTimePoints.Date.end, this.filterTimePoints.Time, this.filterTimePoints.Points, this.filterTimePoints.TypeofPoints, this.filterTimePoints.Score, this.filterTimePoints.SortBy, this.currentPage, this.filterTimePoints.PageSize)
            this.SetupTimePoints.data = data.data;
            this.SetupTimePoints.totalData = data.totalData;
            this.isBusy = false;
        }

        async reset() {
            this.ResetErrorMessage();
            this.filterTimePoints = {
                Date: { end: null, start: null }, Points: null, Score: null, Time: null, TypeofPoints: "", PageIndex: 1, PageSize: this.pageSize, SortBy: "", PointTypeId: null
            };

            this.typeofPoint.reset();
            this.time.reset();
            this.score.reset();
            this.points.reset();
            this.createdDate.reset();
            this.updatedDate.reset();

            await this.FetchData();
        }

        //Variable untuk sorting
        typeofPoint = new Sort();
        time = new Sort();
        score = new Sort();
        points = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortProgramType
        async ClickSortProgramType() {
            this.ResetSort('typeofPoint');
            //Sorting
            this.typeofPoint.sorting();
            //Function Sorting
            if (this.typeofPoint.sortup == true) {
                this.filterTimePoints.SortBy = "TypeofPointAsc";
            } else if (this.typeofPoint.sortdown == true) {
                this.filterTimePoints.SortBy = "TypeofPointDesc"
            }
            else {
                this.filterTimePoints.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortLearningType
        async ClickSortLearningType() {
            this.ResetSort('time');
            //Sorting
            this.time.sorting();
            //Function Sorting
            if (this.time.sortup == true) {
                this.filterTimePoints.SortBy = "TimeAsc";
            } else if (this.time.sortdown == true) {
                this.filterTimePoints.SortBy = "TimeDesc"
            }
            else {
                this.filterTimePoints.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortLevel
        async ClickSortLevel() {
            this.ResetSort('score');
            //Sorting
            this.score.sorting();
            //Function Sorting
            if (this.score.sortup == true) {
                this.filterTimePoints.SortBy = "ScoreAsc";
            } else if (this.score.sortdown == true) {
                this.filterTimePoints.SortBy = "ScoreDesc"
            }
            else {
                this.filterTimePoints.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortPricing
        async ClickSortPricing() {
            this.ResetSort('points');
            //Sorting
            this.points.sorting();
            //Function Sorting
            if (this.points.sortup == true) {
                this.filterTimePoints.SortBy = "PointAsc";
            } else if (this.points.sortdown == true) {
                this.filterTimePoints.SortBy = "PointDesc"
            }
            else {
                this.filterTimePoints.SortBy = "";
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
                this.filterTimePoints.SortBy = "CreatedDateAsc";
            } else if (this.createdDate.sortdown == true) {
                this.filterTimePoints.SortBy = "CreatedDateDesc"
            }
            else {
                this.filterTimePoints.SortBy = "";
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
                this.filterTimePoints.SortBy = "UpdatedDateAsc";
            } else if (this.updatedDate.sortdown == true) {
                this.filterTimePoints.SortBy = "UpdatedDateDesc"
            }
            else {
                this.filterTimePoints.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'typeofPoint':
                    this.score.reset();
                    this.time.reset();
                    this.points.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'time':
                    this.typeofPoint.reset();
                    this.score.reset();
                    this.points.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'score':
                    this.typeofPoint.reset();
                    this.time.reset();
                    this.points.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'points':
                    this.typeofPoint.reset();
                    this.score.reset();
                    this.time.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.typeofPoint.reset();
                    this.score.reset();
                    this.time.reset();
                    this.points.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.typeofPoint.reset();
                    this.score.reset();
                    this.time.reset();
                    this.points.reset();
                    this.createdDate.reset();
                    return;
            }
        }

        //Variable untuk control page
        add: boolean = false;
        edit: boolean = false;
        viewDetail: boolean = false;

        //Error Message
        errorMessageShow: boolean = false;
        errorMessage: string = "";

        successMessageShow: boolean = false;
        successMessage: string = "";

        ResetSuccessMessage() {
            this.successMessageShow = false;
            this.successMessage = "";
        }

        ResetErrorMessage() {
            this.errorMessageShow = false;

        }

        //Add
        addTimePoints: SetupTimePointInsertModel = {
            pointTypeId: null, points: null, score: null, time: null
        }

        AddClicked() {
            this.add = true;
            this.ResetErrorMessage();
        }

        async Add() {
            this.isBusy = true;
            if (this.addTimePoints.pointTypeId == null) {
                this.errorMessage = "Type of Points must be selected!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if ((this.addTimePoints.pointTypeId == 1 || this.addTimePoints.pointTypeId == 2 || this.addTimePoints.pointTypeId == 3) && ((this.addTimePoints.time == null || (this.addTimePoints.time).toString() == ""))) {
                this.errorMessage = "Time must be filled!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if ((this.addTimePoints.pointTypeId != 1 && this.addTimePoints.pointTypeId != 2 && this.addTimePoints.pointTypeId != 3) && ((this.addTimePoints.score == null || (this.addTimePoints.score).toString() == ""))) {
                this.errorMessage = "Score must be filled!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if (this.addTimePoints.points == null) {
                this.errorMessage = "Points must be filled!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if ((this.addTimePoints.pointTypeId == 1 || this.addTimePoints.pointTypeId == 2 || this.addTimePoints.pointTypeId == 3)) {

                this.addTimePoints.score = 0;
                var add = await this.SetupTimePointApi.insertTimePointAsync(this.addTimePoints);
            }

            if ((this.addTimePoints.pointTypeId != 1 && this.addTimePoints.pointTypeId != 2 && this.addTimePoints.pointTypeId != 3)) {

                this.addTimePoints.time = 0;
                var add = await this.SetupTimePointApi.insertTimePointAsync(this.addTimePoints);
            }


            if (add == false) {
                this.errorMessage = "Time Point already exist or Data already used!";
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }
            this.reset();
            this.isBusy = false;
            this.FetchData();


            this.add = false;
            this.errorMessageShow = false;
            this.addTimePoints = {
                pointTypeId: null, points: null, score: null, time: null
            }

            this.successMessage = Message.SuccessInsertMessage;
            this.successMessageShow = true;
        }

        async Close() {
            this.add = false;
            this.edit = false;
            this.viewDetail = false;
            this.errorMessageShow = false;
            this.successMessageShow = false;
            this.ResetErrorMessage();
            this.addTimePoints = {
                pointTypeId: null, points: null, score: null, time: null
            }
            this.viewTimePoints = {
                pointTypeId: null, points: null, score: null, time: null
            }
            this.editTimePoints = {
                pointTypeId: null, points: null, score: null, time: null
            }

        }

        //initial edit
        editTimePoints: SetupTimePointInsertModel = {
            pointTypeId: null, points: null, score: null, time: null
        }
        timePointId: number = 0;
        async EditInitial(id: number) {
            this.isBusy = true;
            this.ResetErrorMessage();
            this.edit = true;
            this.timePointId = id;
            this.editTimePoints = await this.SetupTimePointApi.getSetupTimePointGetIdAsync(id);
            this.isBusy = false;
        }

        async Edit() {
            this.isBusy = true;
            if (this.editTimePoints.pointTypeId == null) {
                this.errorMessage = "Type of Points must be selected!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if ((this.editTimePoints.pointTypeId == 1 || this.editTimePoints.pointTypeId == 2 || this.editTimePoints.pointTypeId == 3) && ((this.editTimePoints.time == null || (this.editTimePoints.time).toString() == ""))) {
                this.errorMessage = "Time must be filled!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if ((this.editTimePoints.pointTypeId != 1 && this.editTimePoints.pointTypeId != 2 && this.editTimePoints.pointTypeId != 3) && ((this.editTimePoints.score == null || (this.editTimePoints.score).toString() == ""))) {
                this.errorMessage = "Score must be filled!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if (this.editTimePoints.points == null || this.editTimePoints.points.toString() == "") {
                this.errorMessage = "Points must be filled!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if ((this.editTimePoints.pointTypeId == 1 || this.editTimePoints.pointTypeId == 2 || this.editTimePoints.pointTypeId == 3)) {

                this.editTimePoints.score = 0;
                try {
                    var edit = await this.SetupTimePointApi.updateTimePointAsync(this.timePointId, this.editTimePoints);
                }
                catch{
                    this.errorMessage = "Can't edit this Timepoint because there are Task(s) with this Timepoint."
                    this.errorMessageShow = true;
                    this.isBusy = false;
                    return
                }
            }

            if ((this.editTimePoints.pointTypeId != 1 && this.editTimePoints.pointTypeId != 2 && this.editTimePoints.pointTypeId != 3)) {

                this.editTimePoints.time = 0;
                try {
                    var edit = await this.SetupTimePointApi.updateTimePointAsync(this.timePointId, this.editTimePoints);
                }
                catch{
                    this.errorMessage = "Can't edit this Timepoint because there are Task(s) with this Timepoint."
                    this.errorMessageShow = true
                    this.isBusy = false;
                    return
                }
            }
            this.reset();
            this.FetchData();
            this.isBusy = false;

            this.edit = false;
            this.errorMessageShow = false;
            this.editTimePoints = {
                pointTypeId: null, points: null, score: null, time: null
            }

            this.successMessage = Message.SuccessEditMessage;
            this.successMessageShow = true;
        }

        GetDelete(id: number) {
            this.timePointId = id;
        }

        async Delete() {
            this.isBusy = true;
            try {
                await this.SetupTimePointApi.deleteTimePointAsync(this.timePointId);
                this.successMessage = Message.RemoveMessage;
                this.successMessageShow = true;
                this.FetchData();
            }
            catch{
                this.errorMessage = "Can't delete this Timepoint because there are Task(s) with this Timepoint.";
                this.errorMessageShow = true;
            }
            this.isBusy = false;
        }

        //View Detail
        viewTimePoints: SetupTimePointInsertModel = {
            pointTypeId: null, points: null, score: null, time: null
        }
        async ViewDetailInitial(id: number) {
            this.isBusy = true;
            this.ResetErrorMessage();
            this.viewDetail = true;
            this.timePointId = id;
            this.viewTimePoints = await this.SetupTimePointApi.getSetupTimePointGetIdAsync(id);
            this.isBusy = false;

            //Function get by Id
        }
    }


</script>


