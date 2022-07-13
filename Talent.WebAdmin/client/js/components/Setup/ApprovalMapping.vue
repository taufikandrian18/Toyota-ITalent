<template>
    <div>
        <div v-if="successMessage" class="alert alert-success alert-dismissible fade show" role="alert">
            {{successMessage}}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <loading-overlay :loading="isBusy"></loading-overlay>

        <div v-if="this.errors.items.length != 0 || errorMessage.length > 0" class="alert alert-danger">
            <div>{{errorMessage}}</div>--------
            <div>{{errors.first('Page')}}</div>
            <div>{{errors.first('Position')}}</div>

            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <h3>
                    <fa icon="folder"></fa> Setup > <span class="talent_font_red">Approval Mapping</span>
                </h3>

                <br />

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">

                    <div class="mb-md-4">
                        <h4>Search Approval Mapping</h4>
                    </div>

                    <!--2 Column Menu-->
                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker class="v-date-style-report"
                                                       mode="range"
                                                       :firstDayOfWeek="2"
                                                       :value="null"
                                                       v-model="filterModel.DateFilter"
                                                       :input-props="props"
                                                       :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text">
                                                <fa icon="calendar-alt"></fa>
                                            </span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Position Name</span>
                                </div>
                                <div class="col-md-8">
                                    <select class="form-control" v-model="filterModel.PositionName">
                                        <option v-for="level in listApprovalLevel" :value="level.name">{{level.name}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--2 Column Menu-->
                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Menu Page</span>
                                </div>
                                <div class="col-md-8">
                                    <select class="form-control" v-model="filterModel.PageName">
                                        <option v-for="page in listPage" :value="page.name">{{page.name}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Type</span>
                                </div>
                                <div class="col-md-8">
                                    <select class="form-control" v-model="filterModel.Type">
                                        <option v-for="level in listApprovalLevel" :value="level.approvalLevelId">Level {{level.approvalLevelId}}</option>
                                    </select>
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
                                        <button class="btn talent_bg_blue talent_font_white" @click="fetch">Search</button>
                                        <button class="btn talent_bg_red talent_font_white" @click="reset">Reset</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <br />

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Approval Mapping List</h4>
                    <br />
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{approvalMapping.listApprovalMapping.length}} of {{approvalMapping.totalData}} Entry(s)</a>
                            <button class="btn talent_bg_cyan talent_roundborder talent_font_white" v-if="crud.create" @click="addClicked">
                                Add Approval Mapping
                            </button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#"
                                           class="talent_sort"
                                           @click.prevent="ClickSortMenuPage()">
                                            Menu Page
                                            <fa icon="sort" v-if="menuPage.sort == true"></fa>
                                            <fa icon="sort-up" v-if="menuPage.sortup == true"></fa>
                                            <fa icon="sort-down" v-if="menuPage.sortdown == true"></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a href="#"
                                           class="talent_sort"
                                           @click.prevent="ClickSortPosition()">
                                            Position Name
                                            <fa icon="sort" v-if="position.sort == true"></fa>
                                            <fa icon="sort-up" v-if="position.sortup == true"></fa>
                                            <fa icon="sort-down" v-if="position.sortdown == true"></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a href="#"
                                           class="talent_sort"
                                           @click.prevent="ClickSortType()">
                                            Type
                                            <fa icon="sort" v-if="type.sort == true"></fa>
                                            <fa icon="sort-up" v-if="type.sortup == true"></fa>
                                            <fa icon="sort-down" v-if="type.sortdown == true"></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a href="#"
                                           class="talent_sort"
                                           @click.prevent="ClickSortCreatedDate()">
                                            Created Date
                                            <fa icon="sort" v-if="createdDate.sort == true"></fa>
                                            <fa icon="sort-up" v-if="createdDate.sortup == true"></fa>
                                            <fa icon="sort-down" v-if="createdDate.sortdown == true"></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a href="#"
                                           class="talent_sort"
                                           @click.prevent="ClickSortUpdatedDate()">
                                            Updated Date
                                            <fa icon="sort" v-if="updatedDate.sort == true"></fa>
                                            <fa icon="sort-up" v-if="updatedDate.sortup == true"></fa>
                                            <fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa>
                                        </a>
                                    </th>
                                    <th colspan="3" v-if="crud.read || crud.update || crud.delete">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(approval,index) in approvalMapping.listApprovalMapping">
                                    <td>{{approval.pageName}}</td>
                                    <td>{{approval.positionName}}</td>
                                    <td>Level {{approval.approvalLevelId}}</td>
                                    <td>{{approval.createdAt | dateFormat}}</td>
                                    <td>{{approval.updatedAt | dateFormat}}</td>

                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click="detailClicked(index)">
                                            View Detail
                                        </button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click="editClicked(index)">
                                            Edit
                                        </button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click="removeClicked(index)">
                                            Remove
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>

                    </div>

                    <div class="col-md-12 d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center">
                            <paginate :currentPage.sync="currentPage" :total-data=approvalMapping.totalData :limit-data=pageSize @update:currentPage="fetch()"></paginate>
                        </div>
                    </div>

                </div>
                <br />
            </div>
        </div>

        <!--Add-->
        <div class="row" v-if="add === true">
            <div class="col col-md-12">
                <h3>
                    <fa icon="folder"></fa> Setup > Approval Mapping > <span class="talent_font_red">New Approval Mapping</span>
                </h3>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Mapping Information</h4>
                    <div class="row">
                        <div class="col-md-12 mb-md-3">
                            <label>
                                Menu Page
                                <span class="talent_font_red">*</span>
                            </label>
                            <div class="input-group">
                                <select class="form-control" v-model="formModel.pageId" v-validate="'required'" name="Page">
                                    <option v-for="page in listAddPage" :value="page.pageId">{{page.name}}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12 mb-md-3">
                            <label>
                                Position Name
                                <span class="talent_font_red">*</span>
                            </label>
                            <div class="input-group">
                                <select class="form-control" v-model="formModel.approvalLevelId" v-validate="'required'" name="Level" @change="changeLevel()">
                                    <option v-for="(level,index) in listApprovalLevel" :value="level.approvalLevelId">{{level.name}}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12 mb-md-3">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>
                                        Type
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <div class="input-group">
                                        <select class="form-control" v-model="choosenLevel" disabled>
                                            <option v-for="level in listApprovalLevel" :value="level">Level {{level.approvalLevelId}}</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        Description
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <textarea class="form-control"
                                              style="height:150px;overflow-y:scroll;"
                                              v-model="choosenLevel.description"
                                              disabled></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white"
                                            @click.prevent="closeClicked()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="insertApprovalMapping">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Edit-->
        <div class="row" v-if="edit === true">
            <div class="col col-md-12">
                <h3>
                    <fa icon="folder"></fa> Setup > Approval Mapping > <span class="talent_font_red">Edit Approval Mapping</span>
                </h3>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Mapping Information</h4>
                    <div class="row">
                        <div class="col-md-12 mb-md-3">
                            <label>
                                Menu Page
                                <span class="talent_font_red">*</span>
                            </label>
                            <div class="input-group">
                                <select class="form-control" v-model="formModel.pageId" v-validate="'required'" name="Page" disabled>
                                    <option v-for="page in listPage" :value="page.pageId">{{page.name}}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12 mb-md-3">
                            <label>
                                Position Name
                                <span class="talent_font_red">*</span>
                            </label>
                            <div class="input-group">
                                <select class="form-control" v-model="formModel.approvalLevelId" v-validate="'required'" name="Position" @change="changeLevel()">
                                    <option v-for="(level,index) in listApprovalLevel" :value="level.approvalLevelId">{{level.name}}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12 mb-md-3">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>
                                        Type
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <div class="input-group">
                                        <select class="form-control" v-model="choosenLevel" disabled>
                                            <option v-for="level in listApprovalLevel" :value="level">Level {{level.approvalLevelId}}</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        Description
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <textarea class="form-control"
                                              style="height:150px;overflow-y:scroll;"
                                              v-model="choosenLevel.description"
                                              disabled></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white"
                                            @click.prevent="closeClicked()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="editApprovalMapping">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Detail-->
        <div class="row" v-if="detail === true">
            <div class="col col-md-12">
                <h3>
                    <fa icon="folder"></fa>Setup > Approval Mapping > <span class="talent_font_red">Approval Mapping Detail</span>
                </h3>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Mapping Information</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <label>
                                Menu Page
                                <span class="talent_font_red">*</span>
                            </label>
                            <div class="input-group">
                                <select class="form-control" v-model="approvalMapping.listApprovalMapping[this.viewIndex].pageName" disabled>
                                    <option :value="approvalMapping.listApprovalMapping[this.viewIndex].pageName">{{approvalMapping.listApprovalMapping[this.viewIndex].pageName}}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <label>
                                Position Name
                                <span class="talent_font_red">*</span>
                            </label>
                            <div class="input-group">
                                <select class="form-control" v-model="approvalMapping.listApprovalMapping[this.viewIndex].positionName" disabled>
                                    <option :value="approvalMapping.listApprovalMapping[this.viewIndex].positionName">{{approvalMapping.listApprovalMapping[this.viewIndex].positionName}}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>
                                        Type
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <div class="input-group">
                                        <select class="form-control" v-model="approvalMapping.listApprovalMapping[this.viewIndex].level" disabled>
                                            <option :value="approvalMapping.listApprovalMapping[this.viewIndex].level">Level {{approvalMapping.listApprovalMapping[this.viewIndex].approvalLevelId}}</option>
                                        </select>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        Description
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <textarea class="form-control"
                                              style="height:150px;overflow-y:scroll;"
                                              v-model="approvalMapping.listApprovalMapping[this.viewIndex].description"
                                              disabled></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white"
                                            @click.prevent="closeClicked()">
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
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
                                <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="deleteConfirmed()">Delete</button>
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
    import { ApprovalMappingService, PagesModel, ApprovalMappingViewModel, ApprovalMappingFormModel, UserPrivilegeSettingsService, UserAccessCRUD, ApprovalLevelModel } from '../../services/NSwagService';
    import { ucs2 } from 'punycode';
    import { PageEnums } from '../../enum/PageEnums';

    @Component({
        created: async function (this: ApprovalMapping) {
            await this.getAccess();
            await this.initialize();
        }
    })
    export default class ApprovalMapping extends Vue {
        framework: string;
        compiler: string;
        looping: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        currentPage: number = 1;
        pageSize: number = 10;

        props: any = {
            placeholder: '',
            readonly: true
        };

        isBusy: boolean = false;

        //Navigation
        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;
        close: boolean = false;

        //Variable untuk sorting
        menuPage = new Sort();
        position = new Sort();
        type = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        Service: ApprovalMappingService = new ApprovalMappingService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        errorMessage: string = '';
        successMessage: string = '';

        listPage: PagesModel[] = [];
        listApprovalLevel: ApprovalLevelModel[] = [];

        listAddPage: PagesModel[] = [];

        viewIndex: number;
        deleteIndex: number;

        choosenPage: PagesModel = {
            name: '',
            needApproval: null,
            pageId: null
        }

        formModel: ApprovalMappingFormModel = {
            approvalMappingid: null,
            pageId: null,
            approvalLevelId: null,
        }

        choosenLevel: ApprovalLevelModel = {
            approvalLevelId: null,
            description: '',
            name: ''
        };

        approvalMapping: ApprovalMappingViewModel = {
            listApprovalMapping: [],
            totalData: null
        }

        filterModel: IApprovalMapping = {
            DateFilter: {
                end: null,
                start: null
            },
            PageName: '',
            PositionName: '',
            SortBy: '',
            Type: null,
            PageId: ''
        };

        async validateInput() {
            this.clearMessage();
            let validate = await this.$validator.validateAll();

            if (validate === false) {
                return false;
            }

            return true;
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.ApprovalMapping);
            this.crud = data;
        }

        async insertApprovalMapping() {

            if (!this.crud.create) {
                return
            }

            let resultValidate = await this.validateInput();

            if (resultValidate === false) {
                return;
            }

            this.isBusy = true;
            try {
                var result = await this.Service.insertApprovalMappings(this.formModel);
                this.closeClicked();
                this.successMessage = "Success to Add Data!";
            }
            catch{
                this.errorMessage = 'Data already exist.';
            }
            this.reset();
            this.isBusy = false;
        }

        async editApprovalMapping() {

            if (!this.crud.update) {
                return
            }

            this.isBusy = true;
            let resultValidate = await this.validateInput();

            if (resultValidate === false) {
                return;
            }

            try {
                let result = await this.Service.updateApprovalMappings(this.formModel);
                if (result === true) {
                    this.closeClicked();
                    this.successMessage = "Success to Edit Data!";
                }
            }
            catch{
                this.errorMessage = "Data already exist.";
            }
            this.reset();
            this.isBusy = false;

        }

        async fetch() {
            this.isBusy = true;
            this.approvalMapping = await this.Service.getApprovalMappingFiltered(this.filterModel.DateFilter.start, this.filterModel.DateFilter.end, this.filterModel.Type, this.filterModel.PageName, this.filterModel.PositionName, this.filterModel.SortBy, this.currentPage);
            this.isBusy = false;
        }

        async initialize() {
            this.fetch();
            this.listPage = await this.Service.getAllPages();
            this.listApprovalLevel = await this.Service.getAllApprovalLevel();
        }

        clearMessage() {
            this.errorMessage = '';
            this.successMessage = '';
        }

        async deleteConfirmed() {

            if (!this.crud.delete) {
                return
            }

            this.isBusy = true;
            try {
                let result = await this.Service.deleteApprovalMappings(this.approvalMapping.listApprovalMapping[this.deleteIndex].approvalMappingid);
                this.successMessage = "Success to Remove Data!";
                this.fetch();
            }
            catch{
                this.errorMessage = "Failed to Delete Data";
            }
            this.isBusy = false;
        }

        //Navigation function
        async addClicked() {

            if (!this.crud.create) {
                return
            }

            this.add = true;
            this.edit = false;
            this.detail = false;
            this.clearMessage();

            this.formModel = {
                approvalMappingid: null,
                pageId: null,
                approvalLevelId: null,
            }

            this.choosenLevel = {
                approvalLevelId: null,
                description: '',
                name: ''
            }

            this.listAddPage = await this.Service.getUninsertedPage();

        }

        closeClicked() {
            this.add = false;
            this.edit = false;
            this.detail = false;

            this.clearMessage();
            this.fetch();
        }

        editClicked(index: number) {

            if (!this.crud.update) {
                return
            }

            this.clearMessage();

            this.add = false;
            this.edit = true;
            this.detail = false;

            this.formModel.pageId = this.approvalMapping.listApprovalMapping[index].pageId;
            this.formModel.approvalLevelId = this.approvalMapping.listApprovalMapping[index].approvalLevelId;
            this.formModel.approvalMappingid = this.approvalMapping.listApprovalMapping[index].approvalMappingid;
            this.changeLevel();


        }

        detailClicked(index: number) {

            if (!this.crud.read) {
                return
            }
            this.clearMessage();

            this.add = false;
            this.edit = false;
            this.detail = true;

            this.viewIndex = index;
        }

        removeClicked(index: number) {

            if (!this.crud.delete) {
                return
            }

            this.deleteIndex = index;
        }

        changeLevel() {
            this.choosenLevel = this.listApprovalLevel.find(Q => Q.approvalLevelId === this.formModel.approvalLevelId);
        }

        reset() {
            this.filterModel = {
                DateFilter: {
                    end: null,
                    start: null
                },
                PageName: '',
                PositionName: '',
                SortBy: '',
                Type: null,
                PageId: ''
            }
            this.currentPage = 1;
            this.ResetSort('resetAll');

            this.fetch();
        }

        //ClickSortMenuPage
        async ClickSortMenuPage() {
            this.ResetSort('menuPage');
            //Sorting
            this.menuPage.sorting();
            //Function Sorting
            if (this.menuPage.sortup == true) {
                this.filterModel.SortBy = 'menuPage'
            }
            else if (this.menuPage.sortdown == true) {
                this.filterModel.SortBy = 'menuPageDesc'
            }
            else {
                this.filterModel.SortBy = '';
            }
            this.fetch();
            return;
        }

        //ClickSortPosition
        async ClickSortPosition() {
            this.ResetSort('position');
            //Sorting
            this.position.sorting();
            //Function Sorting
            if (this.position.sortup == true) {
                this.filterModel.SortBy = 'position'
            }
            else if (this.position.sortdown == true) {
                this.filterModel.SortBy = 'positionDesc'
            }
            else {
                this.filterModel.SortBy = '';
            }
            this.fetch();
            return;
        }

        //ClickSortType
        async ClickSortType() {
            this.ResetSort('type');
            //Sorting
            this.type.sorting();
            //Function Sorting
            if (this.type.sortup == true) {
                this.filterModel.SortBy = 'type'
            }
            else if (this.type.sortdown == true) {
                this.filterModel.SortBy = 'typeDesc'
            }
            else {
                this.filterModel.SortBy = '';
            }
            this.fetch();
            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting

            if (this.createdDate.sortup == true) {
                this.filterModel.SortBy = 'createdDate'
            }
            else if (this.createdDate.sortdown == true) {
                this.filterModel.SortBy = 'createdDateDesc'
            }
            else {
                this.filterModel.SortBy = '';
            }
            this.fetch();
            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting

            if (this.updatedDate.sortup == true) {
                this.filterModel.SortBy = 'updatedDate'
            }
            else if (this.updatedDate.sortdown == true) {
                this.filterModel.SortBy = 'updatedDateDesc'
            }
            else {
                this.filterModel.SortBy = '';
            }
            this.fetch();
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'menuPage':
                    this.position.reset();
                    this.type.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'position':
                    this.menuPage.reset();
                    this.type.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'type':
                    this.menuPage.reset();
                    this.position.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.menuPage.reset();
                    this.position.reset();
                    this.type.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.menuPage.reset();
                    this.position.reset();
                    this.type.reset();
                    this.createdDate.reset();
                    return;
                case 'resetAll':
                    this.menuPage.reset();
                    this.position.reset();
                    this.type.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
            }
        }
    }
</script>
