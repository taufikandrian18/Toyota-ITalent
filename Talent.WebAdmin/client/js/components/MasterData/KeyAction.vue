<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="alert alert-success alert-dismissible fade show" role="alert" v-if="success">
            {{successMessage}}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close" @clicked="alertClose()">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>


        <div class="alert alert-danger" v-show="errors.items.length > 0 || errorMessage.length > 0">
            <button type="button" class="close" aria-label="Close" @click="$validator.errors.clear()">
                <span aria-hidden="true">&times;</span>
            </button>
            <div v-for="error in errors.all()">{{error}}</div>
            <div>{{errorMessage}}</div>
        </div>

        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > <span class="talent_font_red">Key Action</span></h3>
                <br />
                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Key Action</h4>
                    <br />
                    <!--2 Column Menu-->
                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group talent_front">
                                        <v-date-picker v-model="keyActionFilter.DateFilter" :masks="{ input: 'DD/MM/YYYY' }" class="v-date-style-report" mode="range" :firstDayOfWeek="2" :value="null" :input-props="props"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Key Action Name</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="keyActionFilter.KeyActionName" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Key Action Code</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="keyActionFilter.KeyActionCode" />
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
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="advanceSearch()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="resetButton()">
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
                    <h4>Key Action List</h4>
                    <br />
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{getPageData()}} of {{getTotalData()}} Entry(s)</a>
                            <button class="btn talent_bg_cyan talent_roundborder talent_font_white" v-if="crud.create" @click.prevent="addClicked">Add Key Action</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortKeyActionCode()">Key Action Code <fa icon="sort" v-if="keyActionCodeSort.sort == true"></fa><fa icon="sort-up" v-if="keyActionCodeSort.sortup == true"></fa><fa icon="sort-down" v-if="keyActionCodeSort.sortdown == true"></fa></a>
                                    </th>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortKeyActionName()">Key Action Name <fa icon="sort" v-if="keyActionName.sort == true"></fa><fa icon="sort-up" v-if="keyActionName.sortup == true"></fa><fa icon="sort-down" v-if="keyActionName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortDescription()">Description <fa icon="sort" v-if="description.sort == true"></fa><fa icon="sort-up" v-if="description.sortup == true"></fa><fa icon="sort-down" v-if="description.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="3" v-if="crud.read || crud.delete || crud.update">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(keyAction,index) in keyActionList.listActionModel">
                                    <td>
                                        {{keyAction.keyActionCode}}
                                    </td>
                                    <td>
                                        {{keyAction.keyActionName}}
                                    <td>
                                        {{keyAction.keyActionDescription}}
                                    </td>
                                    <td>
                                        {{keyAction.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{keyAction.updatedAt | dateFormat}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="viewDetailClicked(index)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="editClicked(index)">Edit </button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click.prevent="DeleteClicked(index)">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <br />
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate :currentPage.sync="currentPage" :total-data=keyActionList.totalItem :limit-data=pageSize @update:currentPage="fetch()"></paginate>
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

        <!--Add-->
        <div class="row" v-if="add === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Key Action > <span class="talent_font_red">Add Key Action</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Key Action Information</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label for="keyActionCodeString">Key Action Code<span class="talent_font_red">*</span></label>
                                            <input name="Key Action Code" key="keyActionCodeString" v-validate="'required|max:16'" v-model="keyActionModelForm.keyActionCode" class="form-control" />
                                        </div>
                                        <div class="col-md-12">
                                            <label for="keyActionNameString">Key Action Name<span class="talent_font_red">*</span></label>
                                            <input class="form-control" v-model="keyActionModelForm.keyActionName" v-validate="'required|max:255'" key="keyActionNameString" name="Key Action Name" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <label for="keyActionDescription">Key Action Description</label>
                                    <textarea name="Key Action Description" key="keyActionDescription" v-validate="'max:1024'" class="form-control talent_textarea" v-model="keyActionModelForm.keyActionDescription" maxlength="1024"></textarea>
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
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">
                                        Close
                                    </button>
                                    <button type="submit" @click.prevent="addKeyAction()" class="btn talent_bg_lightgreen talent_font_white">
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

                <h3><fa icon="database"></fa> Master Data > Key Action > <span class="talent_font_red">Edit Key Action</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Key Action Information</h4>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label for="keyActionCodeStringUpdate">Key Action Code<span class="talent_font_red">*</span></label>
                                            <input class="form-control" key="keyActionCodeStringUpdate" v-validate="'required|max:16'" v-model="keyActionModelForm.keyActionCode" name="Key Action Code" />
                                        </div>
                                        <div class="col-md-12">
                                            <label for="keyActionNameStringUpdate">Key Action Name<span class="talent_font_red">*</span></label>
                                            <input class="form-control" key="keyActionNameStringUpdate" v-validate="'required|max:255'" v-model="keyActionModelForm.keyActionName" name="Key Action Name" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label for="keyActioDescriptionUpdate">Description</label>
                                    <textarea name="Key Action Description" key="keyActioDescriptionUpdate" v-validate="'max:1024'" class="form-control talent_textarea" v-model="keyActionModelForm.keyActionDescription" maxlength="1024"></textarea>
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
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="updateKeyAction">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" v-if="detail === true">
            <div class="col col-md-12">

                <h3><fa icon="database"></fa> Master Data > Key Action > <span class="talent_font_red">Detail Key Action</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Key Action Information</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label>Key Action Code<span class="talent_font_red">*</span></label>
                                            <input class="form-control" disabled v-model="keyActionList.listActionModel[indexDetail].keyActionCode" />
                                        </div>
                                        <div class="col-md-12">
                                            <label>Key Action Name<span class="talent_font_red">*</span></label>
                                            <input class="form-control" disabled v-model="keyActionList.listActionModel[indexDetail].keyActionName" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Description<span class="talent_font_red">*</span></label>
                                    <textarea class="form-control talent_textarea" disabled v-model="keyActionList.listActionModel[indexDetail].keyActionDescription"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                </div>
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
    import { KeyActionService, KeyActionViewModel, KeyActionModel, KeyActionFormModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService'
    import { faAd } from '@fortawesome/free-solid-svg-icons';
    import { PageEnums } from '../../enum/PageEnums';

    @Component({
        created: async function (this: KeyAction) {
            this.isBusy = true;
            await this.getAccess();
            await this.initialize();
            this.isBusy = false;
        }
    })
    export default class KeyAction extends Vue {
        currentPage: number = 1;
        pageSize: number = 10;

        //Page navigation
        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;
        success: boolean = false;
        error: boolean = false;
        isBusy: boolean = false;

        successMessage: string = '';
        errorMessage: string = '';

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        //testing Variable
        keyActionNameString: string = '';
        keyActionCodeString: string = '';
        keyActionList: KeyActionViewModel = {
            listActionModel: [],
            totalItem: null
        };

        keyActionFilter: IKeyActionFilter = {
            DateFilter: {
                end: null,
                start: null
            },
            KeyActionCode: '',
            KeyActionName: '',
            PageNumber: 1,
            SortBy: ''
        }

        indexDetail: number;
        indexUpdate: number;
        indexDelete: number;

        keyActionModelForm: KeyActionFormModel = { createdAt: null, keyActionCode: '', keyActionDescription: '', keyActionId: null, keyActionName: '', updatedAt: null };

        Service: KeyActionService = new KeyActionService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async initialize() {
            await this.fetch();
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.KeyAction);
            this.crud = data;
        }

        async fetch() {
            this.keyActionList = await this.Service.gets(this.keyActionFilter.DateFilter.start, this.keyActionFilter.DateFilter.end, this.keyActionFilter.KeyActionCode, this.keyActionFilter.KeyActionName, this.keyActionFilter.SortBy, this.currentPage);
        }

        async advanceSearch() {
            this.keyActionList = await this.Service.gets(this.keyActionFilter.DateFilter.start, this.keyActionFilter.DateFilter.end, this.keyActionFilter.KeyActionCode, this.keyActionFilter.KeyActionName, this.keyActionFilter.SortBy, 1);
        }

        getPageData() {
            return this.keyActionList.listActionModel.length;
        }

        getTotalData() {
            return this.keyActionList.totalItem;
        }

        //Function to insert Key Action
        async addKeyAction() {
            this.errorMessage = '';
            if (!this.crud.create) {
                return;
            }

            let valid = await this.$validator.validateAll(['Key Action Code', 'Key Action Name', 'Key Action Description']);

            if (valid == false) {
                return;
            }

            //if (await this.Service.checkUnique(this.keyActionModelForm.keyActionCode) == false) {
            //    this.errorMessage = "Key Action Code must be unique!"
            //    this.error = true;
            //    return;
            //}

            if (this.keyActionModelForm != null) {
                try {
                    this.isBusy = true;
                    let result = await this.Service.postKeyAction(this.keyActionModelForm);

                    this.clearForm();
                    this.resetButton();
                    this.fetch();

                    this.closeClicked();

                    this.successMessage = 'Success to Add Data!';
                    this.success = true;
                    this.error = false;
                }
                catch{
                    this.errorMessage = 'Duplicated Data, Failed to Insert';
                }
                this.isBusy = false;
            }
        }

        //Fucntin to call Upadate Key Action
        async updateKeyAction() {

            if (!this.crud.update) {
                return;
            }

            let valid = await this.$validator.validate();

            if (valid == false) {
                return;
            }

            this.isBusy = true;
            try {
                let isSuccess = await this.Service.updateKeyAction(this.keyActionModelForm);

                if (isSuccess === false) {
                    this.error = true;
                    this.errorMessage = 'Must contain a valid Key Action Code, Key Action maybe duplicated';
                    return;
                }
                else {
                    this.closeClicked();

                    this.successMessage = 'Success to Edit Data!';
                    this.success = true;
                    this.error = false;
                    this.errorMessage = '';
                    this.keyActionModelForm = { createdAt: null, keyActionCode: '', keyActionDescription: '', keyActionId: null, keyActionName: '', updatedAt: null };
                    this.resetButton();
                    this.fetch();
                }
            }
            catch{
                this.errorMessage = 'Must contain a valid Key Action Code, Key Action maybe duplicated';
            }

            this.isBusy = false;

        }

        //Props
        looping: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
        props: any = {
            placeholder: "",
            readonly: true
        };

        //Variable untuk sorting
        keyActionCodeSort = new Sort();
        keyActionName = new Sort();
        description = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //Function to call delete API after Delete Confirmation Pressed
        async deleteConfirmed() {

            if (!this.crud.delete) {
                return;
            }

            this.isBusy = true;
            try {
                let deleteKeyAction = await this.Service.deleteKeyAction(this.keyActionList.listActionModel[this.indexDelete].keyActionId);

                this.successMessage = 'Success to Remove Data!';
                this.success = true;
                this.fetch();
            }
            catch{
                this.errorMessage = 'Failed to Remove Data! Data Key Action In Used!'
            }
            this.isBusy = false;
        }

        addClicked() {
            this.alertClose();

            if (!this.crud.create) {
                return;
            }

            this.$validator.resume();

            this.add = true;
            this.edit = false;
            this.detail = false;
        }

        //Setup for edit component
        editClicked(index: number) {

            if (!this.crud.update) {
                return;
            }

            this.$validator.resume();

            this.$validator.errors.clear();

            this.alertClose();

            this.indexUpdate = index;
            this.keyActionModelForm.keyActionCode = this.keyActionList.listActionModel[this.indexUpdate].keyActionCode;
            this.keyActionModelForm.createdAt = this.keyActionList.listActionModel[this.indexUpdate].createdAt;
            this.keyActionModelForm.updatedAt = this.keyActionList.listActionModel[this.indexUpdate].updatedAt;
            this.keyActionModelForm.keyActionDescription = this.keyActionList.listActionModel[this.indexUpdate].keyActionDescription;
            this.keyActionModelForm.keyActionName = this.keyActionList.listActionModel[this.indexUpdate].keyActionName;
            this.keyActionModelForm.keyActionId = this.keyActionList.listActionModel[this.indexUpdate].keyActionId;

            this.edit = true;
            this.add = false;
            this.detail = false;
        }

        //Setup for view detail component
        viewDetailClicked(index: number) {

            if (!this.crud.read) {
                return;
            }

            this.indexDetail = index;
            this.alertClose();


            this.edit = false;
            this.add = false;
            this.detail = true;
        }

        DeleteClicked(index: number) {
            this.alertClose();

            if (!this.crud.delete) {
                return;
            }

            this.indexDelete = index;
        }

        async resetButton() {
            this.keyActionFilter.KeyActionCode = '';

            this.keyActionFilter.DateFilter = {
                end: null,
                start: null
            };
            this.keyActionFilter.KeyActionName = '';
            this.currentPage = 1;
            this.ResetSort('');
            this.keyActionList = await this.Service.gets(null, null, '', '', '', 1);
        }

        alertClose() {
            this.success = false;
            this.successMessage = '';
            this.error = false;
            this.errorMessage = '';
            this.$validator.errors.clear();

        }

        async closeClicked() {
            this.clearForm();

            this.$validator.pause();
            this.add = false;
            this.edit = false;
            this.detail = false;
            this.success = false;
            this.successMessage = '';
            this.errorMessage = '';
            this.$validator.errors.clear();
        }

        clearForm() {
            this.keyActionModelForm = { createdAt: null, keyActionCode: '', keyActionDescription: '', keyActionId: null, keyActionName: '', updatedAt: null };
        }

        async FetchSorting() {
            this.keyActionList = this.keyActionList = await this.Service.gets(this.keyActionFilter.DateFilter.start, this.keyActionFilter.DateFilter.end, this.keyActionFilter.KeyActionCode, this.keyActionFilter.KeyActionName, this.keyActionFilter.SortBy, this.currentPage);
        }

        //ClickSortKeyActionCode
        async ClickSortKeyActionCode() {
            this.ResetSort('keyActionCodeSort');
            //Sorting
            this.keyActionCodeSort.sorting();
            //Function Sorting
            if (this.keyActionCodeSort.sortup == true) {
                this.keyActionFilter.SortBy = 'keyActionCode';
            }
            else if (this.keyActionCodeSort.sortdown == true) {
                this.keyActionFilter.SortBy = 'keyActionCodeDesc';
            }
            else {
                this.keyActionFilter.SortBy = '';
            }
            this.FetchSorting();

            return;
        }

        //ClickSortKeyActionName
        async ClickSortKeyActionName() {
            this.ResetSort('keyActionName');
            //Sorting
            this.keyActionName.sorting();
            //Function Sorting
            if (this.keyActionName.sortup == true) {
                this.keyActionFilter.SortBy = 'keyActionName';
            }
            else if (this.keyActionName.sortdown == true) {
                this.keyActionFilter.SortBy = 'keyActionNameDesc';
            }
            else {
                this.keyActionFilter.SortBy = '';
            }
            this.FetchSorting();

            return;
        }

        //ClickSortDescription
        async ClickSortDescription() {
            this.ResetSort('description');
            //Sorting
            this.description.sorting();
            if (this.description.sortup == true) {
                this.keyActionFilter.SortBy = 'keyActionDescription';
            }
            else if (this.description.sortdown == true) {
                this.keyActionFilter.SortBy = 'keyActionDescriptionDesc';
            }
            else {
                this.keyActionFilter.SortBy = '';
            }
            this.FetchSorting();

            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting
            if (this.createdDate.sortup == true) {
                this.keyActionFilter.SortBy = 'createdDate';
            }
            else if (this.createdDate.sortdown == true) {
                this.keyActionFilter.SortBy = 'createdDateDesc';
            }
            else {
                this.keyActionFilter.SortBy = '';
            }
            this.FetchSorting();
            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting
            if (this.updatedDate.sortup == true) {
                this.keyActionFilter.SortBy = 'updatedDate';
            }
            else if (this.updatedDate.sortdown == true) {
                this.keyActionFilter.SortBy = 'updateDateDesc';
            }
            else {
                this.keyActionFilter.SortBy = '';
            }
            this.FetchSorting();

            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'keyActionCodeSort':
                    this.description.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    this.keyActionName.reset();
                    return;
                case 'description':
                    this.keyActionCodeSort.reset();
                    this.createdDate.reset();
                    this.keyActionName.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.keyActionCodeSort.reset();
                    this.description.reset();
                    this.updatedDate.reset();
                    this.keyActionName.reset();
                    return;
                case 'updatedDate':
                    this.keyActionCodeSort.reset();
                    this.keyActionName.reset();
                    this.description.reset();
                    this.createdDate.reset();
                    return;
                case 'keyActionName':
                    this.keyActionCodeSort.reset();
                    this.updatedDate.reset();
                    this.description.reset();
                    this.createdDate.reset();
                    return;
                default:
                    this.keyActionFilter.SortBy = '';
                    this.keyActionCodeSort.reset();
                    this.updatedDate.reset();
                    this.description.reset();
                    this.createdDate.reset();
                    this.keyActionName.reset();
                    return;
            }
        }
    }
</script>
