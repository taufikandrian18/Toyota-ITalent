<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="alert alert-success alert-dismissible fade show" role="alert" v-if="success">
            {{successMessage}}
            <button type="button" class="close" aria-label="Close" @click.prevent="alertClose()">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="alert alert-danger alert-dismissible fade show" role="alert" v-if="failed">
            {{errorMessage}}
            <button type="button" class="close" aria-label="Close" @click.prevent="alertClose()">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <div class="row">
            <div class="col col-md-12">
                <!--TITLE-->
                <!--<h2><fa icon="cog"></fa> <fa icon="user"></fa> <fa icon="database"></fa> <fa icon="folder"></fa> Menu > <span class="talent_font_red">Page</span></h2>-->
                <h3 v-if="add === false && edit === false && detail === false"><fa icon="database"></fa> Master Data > <span class="talent_font_red">Event Category</span></h3>
                <h3 v-if="add === true && edit === false && detail === false"><fa icon="database"></fa> Master Data > Event Category > <span class="talent_font_red">Add Event Category</span></h3>
                <h3 v-if="add === false && edit === true && detail === false"><fa icon="database"></fa> Master Data > Event Category > <span class="talent_font_red">Edit Event Category</span></h3>
                <h3 v-if="add === false && edit === false && detail === true"><fa icon="database"></fa> Master Data > Event Category > <span class="talent_font_red">View Detail</span></h3>
                <br />

                <!--SEARCH-->
                <div v-if="add === false && edit === false && detail === false">
                    <div class="d-flex align-items-end flex-column">
                        <div class="col-md-3">
                            <div class="row justify-content-between">
                                <div class="col-md-8 talent_nopadding">
                                    <input class="form-control" v-model="filterEventCategoryName" />
                                </div>
                                <div class="col-md-3 talent_nopadding d-flex justify-content-center">
                                    <button class="btn btn-block talent_bg_blue talent_font_white" @click.prevent="fetch">Search</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow" v-if="add === false && edit === false && detail === false">
                    <h4>Event Category List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{eventCategories.listEventCategoryModel == null ? 0 : eventCategories.listEventCategoryModel.length}} of {{eventCategories.totalItem}} Entry(s)</a>
                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="addClicked">Add Event Category</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortName()">Event Category Name <fa icon="sort" v-if="name.sort == true"></fa><fa icon="sort-up" v-if="name.sortup == true"></fa><fa icon="sort-down" v-if="name.sortdown == true"></fa></a>
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
                                    <th colspan="3">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(ec, index) in eventCategories.listEventCategoryModel">
                                    <td>
                                        {{ec.eventCategoryName}}
                                    </td>
                                    <td>
                                        {{ec.eventCategoryDescription}}
                                    </td>
                                    <td>
                                        {{convertDateTime(ec.createdAt)}}
                                    </td>
                                    <td>
                                        {{convertDateTime(ec.updatedAt)}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="detailClicked(ec.eventCategoryId)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="editClicked(ec.eventCategoryId)">Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click.prevent="deleteClicked(ec.eventCategoryId, index)">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--Pagination-->
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate :currentPage.sync="currentPage" :total-data=eventCategories.totalItem :limit-data=itemPerPage @update:currentPage="fetch()"></paginate>
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
                                        <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="deleteData()">Delete</button>
                                        <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" data-dismiss="modal">Close</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--Add-->
                <div v-if="add === true && edit === false && detail === false">
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <h4>Event Category Information</h4>
                        <div v-if="validateAdd() === false" class="alert alert-danger">
                            <div v-if="addModel.eventCategoryName === ''">Event Category Name is Required</div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-6">
                                        <label>Event Category Name<span class="talent_font_red">*</span></label>
                                        <div class="input-group">
                                            <input class="form-control" v-model="addModel.eventCategoryName" maxlength="255" />
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <label>Event Category Description</label>
                                        <textarea class="form-control" v-model="addModel.eventCategoryDescription" style="height:150px;overflow-y:scroll;" maxlength="1024"></textarea>
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
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked()">
                                            Close
                                        </button>
                                        <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="addData()">
                                            Save
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--Edit-->
                <div v-if="add === false && edit === true && detail === false">
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div v-if="validateEdit() === false" class="alert alert-danger">
                            <div v-if="editModel.eventCategoryName === ''">Event Category Name is Required</div>
                        </div>
                        <h4>Event Category Information</h4>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-6">
                                        <label>Event Category Name<span class="talent_font_red">*</span></label>
                                        <div class="input-group">
                                            <input class="form-control" v-model="editModel.eventCategoryName" maxlength="255" />
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <label>Event Category Description</label>
                                        <textarea class="form-control" v-model="editModel.eventCategoryDescription" style="height:150px;overflow-y:scroll;" maxlength="1024"></textarea>
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
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked()">
                                            Close
                                        </button>
                                        <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="editData()">
                                            Save
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--View Detail-->
                <div v-if="add === false && edit === false && detail === true">
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <h4>Event Category Information</h4>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-6">
                                        <label>Event Category Name<span class="talent_font_red">*</span></label>
                                        <div class="input-group">
                                            <input class="form-control" v-model="detailModel.eventCategoryName" disabled />
                                        </div>
                                    </div>

                                    <div class="col-md-6">
                                        <label>Event Category Description</label>
                                        <textarea class="form-control" v-model="detailModel.eventCategoryDescription" style="height:150px;overflow-y:scroll;" disabled></textarea>
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
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked()">
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
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { Sort } from '../../class/Sort';
    import { EventCategoryService, EventCategoryViewModel, EventCategoryFormModel, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService'
    import { isNullOrUndefined } from 'util';
import { PageEnums } from '../../enum/PageEnums';

    @Component({
        created: async function (this: EventCategory) {
            this.isBusy = true;
            await this.getAccess();
            await this.fetch();
        },
    })
    export default class EventCategory extends Vue {
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.EventCategory);
            this.crud = data;
        }

         crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        isBusy: boolean = false;
        itemPerPage: number = 10;
        success: boolean = false;
        failed: boolean = false;
        successMessage: string = '';
        errorMessage: string = '';

        alertClose() {
            this.success = false;
            this.successMessage = '';

            this.errorMessage = '';
            this.successMessage = '';
            this.success = this.failed = false
        }

        // ---------------------------------------- FETCH -----------------------------------------

        currentPage: number = 1;
        filterEventCategoryName: string = '';
        sortBy: string = '';

        eventCategoryMan: EventCategoryService = new EventCategoryService();
        eventCategories: EventCategoryViewModel = {};

        async fetch() {
            this.isBusy = true;
            this.eventCategories = await this.eventCategoryMan.getAllEventCategories(this.filterEventCategoryName, this.sortBy, this.currentPage);
            this.isBusy = false;
        }

        convertDateTime(stringdate) {
            var date = new Date(stringdate);
            function pad(n) { return n < 10 ? "0" + n : n; }
            return pad(date.getDate()) + "/" + pad(date.getMonth() + 1) + "/" + date.getFullYear();
        }

        // ----------------------------------------- CRUD ---------------------------------------------

        //Navigation
        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;

        //Add
        addValidation = false;
        addModel: EventCategoryFormModel = { eventCategoryName: null };

        addClicked() {
            if(this.crud.create == false){
                return;
            }
            this.alertClose();
            this.add = true;
        }

        validateAdd() {
            if (this.addModel.eventCategoryName === '') {
                return false;
            } else {
                return true;
            }
        }

        async addData() {
            this.isBusy = true;
            this.alertClose();
            this.addValidation = true;
            if (isNullOrUndefined(this.addModel.eventCategoryName)) {
                this.addModel.eventCategoryName = '';
            }
            if (this.validateAdd() === false) {
                this.addValidation = false;
            }
            if (this.addValidation === true) {
                try{
                    await this.eventCategoryMan.create(this.addModel);
                    this.fetch();
                    this.addModel.eventCategoryName = null;
                    this.addModel.eventCategoryDescription = null;

                    this.successMessage = 'Success to Add Data!';
                    this.success = true;
                    this.closeClicked();
                }
                catch(e){
                    this.errorMessage = 'Event Category Name already exist!';
                    this.failed = true;
                }
               
            }
            this.isBusy = false;
        }

        //Edit
        editValidation = false;
        editModel: EventCategoryFormModel = { eventCategoryName: null };

        async editClicked(guideId: number) {
            this.alertClose();

            if(this.crud.update == false){
                return;
            }

            var data = await this.eventCategoryMan.getEventCategoryById(guideId);
            this.editModel.eventCategoryId = data.eventCategoryId;
            this.editModel.eventCategoryName = data.eventCategoryName;
            this.editModel.eventCategoryDescription = data.eventCategoryDescription;

            this.edit = true;
        }

        validateEdit() {
            if (this.editModel.eventCategoryName === '') {
                return false;
            } else {
                return true;
            }
        }

        async editData() {
            this.isBusy = true;
            this.alertClose();
            this.editValidation = true;
            if (this.validateEdit() === false) {
                this.editValidation = false;
            }
            if (this.editValidation === true) {
                try{
                    await this.eventCategoryMan.update(this.editModel);
                    this.fetch();
                    this.successMessage = 'Success to Edit Data!';
                    this.success = true;
                    this.closeClicked();
                }
                catch{
                    this.errorMessage = "Failed to Update Data, Event Category Name already exist!";
                    this.failed = true;
                }
            }
            this.isBusy = false;
        }

        //View Detail
        detailModel = {};

        async detailClicked(guideId: number) {
            this.alertClose();

            if(this.crud.read == false)
            {
                return;
            }

            this.detailModel = await this.eventCategoryMan.getEventCategoryById(guideId);

            this.detail = true;
        }

        //Delete
        deleteEventCategoryId;
        deleteIndex;

        async deleteClicked(guideId: number, index: number) {
            this.alertClose();
            this.deleteEventCategoryId = guideId;
            this.deleteIndex = index;
        }

        async deleteData() {

            if(this.crud.delete == false){
                return;
            }

            this.isBusy = true;
            var check = await this.eventCategoryMan.delete(this.deleteEventCategoryId);
            if (check == false) {
                this.errorMessage = "Can't delete this category because there are event(s) with this category. Please change the category of those event(s) or delete the event(s).";
                this.failed = true;
            } else {
                this.fetch();
                this.successMessage = 'Success to Remove Data!';
                this.success = true;
            }

            this.isBusy = false;
        }

        closeClicked() {
            this.add = false;
            this.edit = false;
            this.detail = false;
        }

        // ---------------------------------------- Sorting ------------------------------------------

        //Variable untuk sorting
        name = new Sort();
        description = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortName
        async ClickSortName() {
            this.ResetSort('name');
            //Sorting
            this.name.sorting();
            //Function Sorting
            if (this.name.sortup == true) {
                this.sortBy = 'name';
            }
            else if (this.name.sortdown == true) {
                this.sortBy = 'nameDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortDescription
        async ClickSortDescription() {
            this.ResetSort('description');
            //Sorting
            this.description.sorting();
            //Function Sorting
            if (this.description.sortup == true) {
                this.sortBy = 'description';
            }
            else if (this.description.sortdown == true) {
                this.sortBy = 'descriptionDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting
            if (this.createdDate.sortup == true) {
                this.sortBy = 'createdDate';
            }
            else if (this.createdDate.sortdown == true) {
                this.sortBy = 'createdDateDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting
            if (this.updatedDate.sortup == true) {
                this.sortBy = 'updatedDate';
            }
            else if (this.updatedDate.sortdown == true) {
                this.sortBy = 'updatedDateDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'name':
                    this.description.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'description':
                    this.name.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.name.reset();
                    this.description.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.name.reset();
                    this.description.reset();
                    this.createdDate.reset();
                    return;
            }
        }
    }
</script>