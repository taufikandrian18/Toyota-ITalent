﻿
<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <!--MAIN PAGE-->
        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-md-4"><fa icon="database"></fa> Master Data > <span class="talent_font_red">Hobbies</span></h3>

                <div class="alert alert-success" v-if="successMessage !== ''">
                    {{successMessage}}
                    <button type="button" class="close" aria-label="Close" @click="successMessage = ''">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <!--SEARCH-->
                <div class="d-flex align-items-end flex-column mb-md-4">
                    <div class="col-md-3">
                        <div class="row justify-content-between">
                            <div class="col-md-8 talent_nopadding">
                                <input class="form-control" v-model="filter.hobbyName" />
                            </div>
                            <div class="col-md-3 talent_nopadding d-flex justify-content-center">
                                <button class="btn btn-block talent_bg_blue talent_font_white" @click.prevent="fetch()">Search</button>
                            </div>
                        </div>
                    </div>
                </div>

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Hobby List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{hobbyList.length}} of {{hobbyGrid.totalFilterData}} Entry(s)</a>

                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" @click="addClicked">Add Hobby</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortHobbyName()">Hobby Name <fa icon="sort" v-if="hobbyName.sort == true"></fa><fa icon="sort-up" v-if="hobbyName.sortup == true"></fa><fa icon="sort-down" v-if="hobbyName.sortdown == true"></fa></a>
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
                                    <th v-if="crud.read || crud.update || crud.delete" colspan="3">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="hobby in hobbyList">
                                    <td>
                                        {{hobby.hobbyName}}
                                    </td>
                                    <td>
                                        {{hobby.hobbyDescription}}
                                    </td>
                                    <td>
                                        {{hobby.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{hobby.updatedAt | dateFormat}}
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.read">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click="viewClicked(hobby.hobbyId)">View Detail</button>
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.update">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click="editClicked(hobby.hobbyId)">Edit</button>
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.delete">
                                        <button type="button" class="btn btn-block talent_bg_red talent_font_white"
                                                data-backdrop="static" data-keyboard="false"
                                                data-toggle="modal" data-target="#deleteConfirmation"
                                                @click="setDelete(hobby.hobbyId, hobby.hobbyName)">
                                            Remove
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="row justify-content-center">
                        <paginate :currentPage.sync="filter.pageIndex"
                                  :total-data="hobbyGrid.totalFilterData"
                                  :limit-data="filter.pageSize"
                                  @update:currentPage="fetch()"></paginate>
                    </div>
                </div>

            </div>
        </div>

        <!--ADD-->
        <div class="row" v-if="add === true">
            <div class="col-md-12">
                <h3><fa icon="database"></fa> Master Data > Hobbies > <span class="talent_font_red">Add Hobbies</span></h3>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4>Hobby Information</h4>
                    <div class="alert alert-danger pb-0" v-show="errors.items.length > 0">
                        <button type="button" class="close" aria-label="Close" @click="$validator.errors.clear()">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <div v-for="error in errors.all()">{{error}}</div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Hobby Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control"
                                               name="Hobby Name"
                                               v-model="hobbyForm.hobbyName"
                                               maxlength="255"
                                               v-validate="'required'" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Hobby Description</label>
                                    <textarea class="form-control talent_overflowy hobby-description-height"
                                              name="Hobby Description"
                                              v-model="hobbyForm.hobbyDescription" maxlength="1024"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="insertHobby">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <!--EDIT-->
        <div class="row" v-if="edit === true">
            <div class="col-md-12">
                <h3><fa icon="database"></fa> Master Data > Hobbies > <span class="talent_font_red">Edit Hobbies</span></h3>
                <div class="alert alert-danger pb-0" v-show="errors.items.length > 0">
                    <button type="button" class="close" aria-label="Close" @click="$validator.errors.clear()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <div v-for="error in errors.all()">{{error}}</div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4>Hobby Information</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Hobby Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control"
                                               name="Hobby Name"
                                               v-model="hobbyForm.hobbyName"
                                               maxlength="255"
                                               v-validate="'required'" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Hobby Description</label>
                                    <textarea class="form-control talent_overflowy hobby-description-height"
                                              name="Hobby Description"
                                              v-model="hobbyForm.hobbyDescription" maxlength="1024"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="updateHobby">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <!--VIEW DETAIL-->
        <div class="row" v-if="detail === true">
            <div class="col-md-12">
                <h3><fa icon="database"></fa> Master Data > Hobbies > <span class="talent_font_red">View Detail Hobbies</span></h3>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4>Hobby Information</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Hobby Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="hobbyForm.hobbyName" disabled />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Hobby Description</label>
                                    <textarea class="form-control talent_overflowy hobby-description-height" v-model="hobbyForm.hobbyDescription" disabled></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

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

        <!--DELETE CONFIRMATION-->
        <div class="modal fade" id="deleteConfirmation" tabindex="-1" role="dialog" aria-labelledby="deleteConfirmLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="col-md-12 justify-content-center d-flex">
                            <h5>Are you sure want to delete?</h5>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="col-md-12 d-flex justify-content-around">
                                <button type="button" class="btn talent_bg_red talent_font_white talent_roundborder" data-dismiss="modal"
                                        @click="deleteHobby(toBeDeletedHobby.hobbyId)">
                                    Delete
                                </button>
                                <button type="button" class="btn talent_bg_blue talent_font_white talent_roundborder" data-dismiss="modal" @click="emptyDelete">Close</button>
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
    import { HobbyService, HobbyCreateModel, HobbyGridModel, HobbyGridViewModel, HobbyUpdateModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService'
    import Message from '../../class/Message';
    import { PageEnums } from '../../enum/PageEnums';

    @Component({
        created: async function (this: Hobby) {
            await this.getAccess();
            await this.fetch();
        }
    })

    export default class Hobby extends Vue {
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Hobby);
            this.crud = data
        }

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        Service: HobbyService = new HobbyService();
        isLoading: boolean = null;

        successMessage: string = '';
        message = Message;

        hobbyGrid: HobbyGridModel = {
            grid: [],
            totalData: 0,
            totalFilterData: 0
        };

        hobbyList: HobbyGridViewModel[] = [];

        filter: IHobbyFilter = {
            sortBy: '',
            pageIndex: 1,
            pageSize: 10,
            hobbyName: ''
        };

        hobbyCreateForm: HobbyCreateModel = {
            hobbyName: '',
            hobbyDescription: ''
        };

        hobbyUpdateForm: HobbyUpdateModel = {
            hobbyId: 0,
            hobbyName: null,
            hobbyDescription: null
        };

        hobbyForm: IHobbyForm = {
            hobbyName: null,
            hobbyDescription: null
        };

        toBeDeletedHobby: { hobbyId: number, hobbyName: string } = {
            hobbyId: 0,
            hobbyName: ''
        };

        async fetch() {
            this.isLoading = true;
            this.hobbyGrid = await this.Service.getAllHobbies(this.filter.hobbyName, this.filter.sortBy, this.filter.pageIndex, this.filter.pageSize);
            this.hobbyList = this.hobbyGrid.grid;
            this.isLoading = false;
        }

        async insertHobby() {
            this.isLoading = true;
            this.$validator.resume();
            let valid = await this.$validator.validateScopes();
            if (!valid) {
                this.isLoading = false;
                return;
            }

            this.hobbyCreateForm.hobbyName = this.hobbyForm.hobbyName;
            this.hobbyCreateForm.hobbyDescription = this.hobbyForm.hobbyDescription;

            let success = await this.Service.insertHobby(this.hobbyCreateForm);
            if (!success) {
                this.$validator.errors.add({
                    field: "Hobby Name",
                    msg: "Hobby Name already exist, please use another name"
                });
                this.isLoading = false;
                return;
            }

            this.$validator.reset();
            this.successMessage = this.message.SuccessInsertMessage;
            await this.fetch();
            this.closeClicked();
        }

        async updateHobby() {
            this.isLoading = true;
            this.$validator.resume();
            let valid = await this.$validator.validateScopes();
            if (!valid) {
                this.isLoading = false;
                return;
            }

            this.hobbyUpdateForm.hobbyName = this.hobbyForm.hobbyName;
            this.hobbyUpdateForm.hobbyDescription = this.hobbyForm.hobbyDescription;

            let success = await this.Service.updateHobby(this.hobbyUpdateForm);
            if (!success) {
                this.$validator.errors.add({
                    field: "Hobby Name",
                    msg: "Hobby Name already exist, please use another name"
                });
                this.isLoading = false;
                return;
            }

            this.$validator.reset();
            this.successMessage = this.message.SuccessEditMessage;
            await this.fetch();
            this.closeClicked();
        }

        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;

        resetForm() {
            this.$validator.pause();

            this.hobbyForm = {
                hobbyName: null,
                hobbyDescription: null
            };
        }

        addClicked() {
            this.add = true;
            this.edit = false;
            this.detail = false;
        }

        async editClicked(hobbyId: number) {
            this.isLoading = true;
            let hobbyDetail = await this.Service.getHobbyById(hobbyId);
            this.hobbyForm = {
                hobbyName: hobbyDetail.hobbyName,
                hobbyDescription: hobbyDetail.hobbyDescription
            };

            this.hobbyUpdateForm.hobbyId = hobbyId;

            this.add = false;
            this.edit = true;
            this.detail = false;
            this.isLoading = false;
        }

        async viewClicked(hobbyId: number) {
            this.isLoading = true;
            let hobbyDetail = await this.Service.getHobbyById(hobbyId);
            this.hobbyForm = {
                hobbyName: hobbyDetail.hobbyName,
                hobbyDescription: hobbyDetail.hobbyDescription
            };

            this.add = false;
            this.edit = false;
            this.detail = true;
            this.isLoading = false;
        }

        closeClicked() {
            this.resetForm();

            this.add = false;
            this.edit = false;
            this.detail = false;
        }

        setDelete(hobbyId: number, hobbyName: string) {
            this.toBeDeletedHobby = {
                hobbyId: hobbyId,
                hobbyName: hobbyName
            };
        }

        emptyDelete() {
            this.toBeDeletedHobby = {
                hobbyId: 0,
                hobbyName: ''
            };
        }

        async deleteHobby(hobbyId: number) {
            this.isLoading = true;
            await this.Service.deleteHobby(hobbyId);
            this.successMessage = this.message.RemoveMessage;
            await this.fetch();
        }

        //Variable untuk sorting
        hobbyName = new Sort();
        description = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortHobbyName
        async ClickSortHobbyName() {
            this.ResetSort('hobbyName');
            //Sorting
            this.hobbyName.sorting();
            if (this.hobbyName.sortup === true) {
                this.filter.sortBy = 'name';
            }

            else if (this.hobbyName.sortdown === true) {
                this.filter.sortBy = 'nameDesc';
            }

            else {
                this.filter.sortBy = '';
            }
            this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortDescription
        async ClickSortDescription() {
            this.ResetSort('description');
            //Sorting
            this.description.sorting();
            if (this.description.sortup === true) {
                this.filter.sortBy = 'description';
            }

            else if (this.description.sortdown === true) {
                this.filter.sortBy = 'descriptionDesc';
            }

            else {
                this.filter.sortBy = '';
            }
            this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            if (this.createdDate.sortup === true) {
                this.filter.sortBy = 'createdAt';
            }

            else if (this.createdDate.sortdown === true) {
                this.filter.sortBy = 'createdAtDesc';
            }

            else {
                this.filter.sortBy = '';
            }
            this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            if (this.updatedDate.sortup === true) {
                this.filter.sortBy = 'updatedAt';
            }

            else if (this.updatedDate.sortdown === true) {
                this.filter.sortBy = 'updatedAtDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            this.fetch();
            //Function Sorting
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'hobbyName':
                    this.description.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'description':
                    this.hobbyName.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.hobbyName.reset();
                    this.description.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.hobbyName.reset();
                    this.description.reset();
                    this.createdDate.reset();
                    return;
            }
        }
    }
</script>
