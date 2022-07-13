<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="alert alert-success alert-dismissible fade show" role="alert" v-if="success">
            {{successMessage}}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close" @clicked="alertClose()">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > <span class="talent_font_red">Competency</span></h3>
                <br />
                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Competency</h4>
                    <br />
                    <!--2 Column Menu-->
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker class="v-date-style-report" mode="range" :firstDayOfWeek="2" v-model="filterDate" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Competency Name</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filterCompetencyName" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--2 Column Menu-->
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Prefix Code</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filterPrefixCode" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Key Action Code</span>
                                </div>
                                <div class="col-md-8">
                                    <select class="form-control" v-model="filterKeyActionCode">
                                        <option v-for="k in keyActions" :value="k.keyActionCode">{{k.keyActionCode}}</option>
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
                    <h4>Competency List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{competencies.listCompetencyJoinModel == null ? 0 : competencies.listCompetencyJoinModel.length}} of {{competencies.totalItem}} Entry(s)</a>
                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="addClicked">Add Competency</button>
                        </div>
                    </div>
                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortPrefixCode()">Prefix Code <fa icon="sort" v-if="prefixCode.sort == true"></fa><fa icon="sort-up" v-if="prefixCode.sortup == true"></fa><fa icon="sort-down" v-if="prefixCode.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickCompetencyName()">Competency Name <fa icon="sort" v-if="competencyName.sort == true"></fa><fa icon="sort-up" v-if="competencyName.sortup == true"></fa><fa icon="sort-down" v-if="competencyName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortKeyActionCode()">Key Action Code <fa icon="sort" v-if="keyActionCode.sort == true"></fa><fa icon="sort-up" v-if="keyActionCode.sortup == true"></fa><fa icon="sort-down" v-if="keyActionCode.sortdown == true"></fa></a>
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
                                <tr v-for="(c, index) in competencies.listCompetencyJoinModel">
                                    <td>
                                        {{c.competencyTypeName.substring(0, 1)}}-{{c.prefixCode}}
                                    </td>
                                    <td>
                                        {{c.competencyName}}
                                    </td>
                                    <td>
                                        {{c.keyActionCode}}
                                    </td>
                                    <td>
                                        {{c.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{c.updatedAt | dateFormat}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="detailClicked(c.competencyId)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="editClicked(c.competencyId)">Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click.prevent="deleteClicked(c.competencyId, index)">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate :currentPage.sync="currentPage" :total-data=competencies.totalItem :limit-data=itemPerPage @update:currentPage="fetch()"></paginate>
                    </div>
                </div>
                <br />
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
        </div>

        <!--Add-->
        <div class="row" v-else-if="add === true">

            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Competency > <span class="talent_font_red">Add Competency</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Competency Information</h4>

                    <div v-if="validateAdd() === false || addKeyActionError == true || addNameError == true || addPrefixError == true" class="alert alert-danger">
                        <div v-if="addCompetencyType.competencyTypeId === 0">Type of Competency is Required</div>
                        <div v-if="addModel.prefixCode === '' ">Prefix Code is Required</div>
                        <div v-if="addModel.prefixCode !== null && addModel.prefixCode !== addModel.prefixCode.toUpperCase()">Prefix Code must be Upper Case</div>
                        <div v-if="addModel.competencyName === ''">Competency Name is Required</div>
                        <div v-if="addKeyActionError == true">Key Action Code is Required</div>
                        <div v-if="addNameError == true">Competency Name already exist</div>
                        <div v-if="addPrefixError == true">Prefix Code already exist</div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Prefix Code<span class="talent_font_red">*</span></label>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <input class="form-control" disabled v-model="addCompetencyType.competencyTypeName.substring(0, 1)" />
                                                        </div>

                                                        <div class="col-md-8">
                                                            <input class="form-control" v-model="addModel.prefixCode" maxlength="16" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-8">
                                                    <label>Competency Name<span class="talent_font_red">*</span></label>
                                                    <input class="form-control" v-model="addModel.competencyName" maxlength="255" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 my-4">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Type of Competency<span class="talent_font_red">*</span></label>
                                                    <select class="form-control" v-model="addCompetencyType">
                                                        <option v-for="(ct) in competencyTypes.listCompetencyTypeModel" :value="ct">{{ct.competencyTypeName}}</option>
                                                    </select>
                                                </div>

                                                <div class="col-md-8">
                                                    <label>Key Action Code<span class="talent_font_red">*</span></label>
                                                    <multiselect v-model="addKeyActions"
                                                                 track-by="keyActionId"
                                                                 label="keyActionCode"
                                                                 :options="keyActions"
                                                                 :multiple="true"
                                                                 :searchable="true"
                                                                 placeholder="">
                                                    </multiselect>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <label>Competency Description</label>
                                    <textarea class="form-control" style="height:130px;overflow-y:scroll;" v-model="addModel.competencyDescription" maxlength="1024"></textarea>
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
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="addData">
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
        <div class="row" v-else-if="edit === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Competency > <span class="talent_font_red">Edit Competency</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Competency Information</h4>

                    <div v-if="validateEdit() === false || editKeyActionError == true || editNameError == true || editPrefixError == true" class="alert alert-danger">
                        <div v-if="editModel.prefixCode === ''">Prefix Code is Required</div>
                        <div v-if="editModel.prefixCode !== editModel.prefixCode.toUpperCase()">Prefix Code must be Upper Case</div>
                        <div v-if="editModel.competencyName === ''">Competency Name is Required</div>
                        <div v-if="editKeyActionError == true">Key Action Code is Required</div>
                        <div v-if="editNameError == true">Competency Name already exist</div>
                        <div v-if="editPrefixError == true">Prefix Code already exist</div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Prefix Code<span class="talent_font_red">*</span></label>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <input class="form-control" disabled v-model="editCompetencyType.competencyTypeName.substring(0, 1)" />
                                                        </div>

                                                        <div class="col-md-8">
                                                            <input class="form-control" v-model="editModel.prefixCode" maxlength="16" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-8">
                                                    <label>Competency Name<span class="talent_font_red">*</span></label>
                                                    <input class="form-control" v-model="editModel.competencyName" maxlength="255" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 my-4">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Type of Competency<span class="talent_font_red">*</span></label>
                                                    <select class="form-control" v-model="editCompetencyType">
                                                        <option v-for="(ct) in competencyTypes.listCompetencyTypeModel" :value="ct">{{ct.competencyTypeName}}</option>
                                                    </select>
                                                </div>

                                                <div class="col-md-8">
                                                    <label>Key Action Code<span class="talent_font_red">*</span></label>
                                                    <multiselect v-model="editKeyActions"
                                                                 track-by="keyActionId"
                                                                 label="keyActionCode"
                                                                 :options="keyActions"
                                                                 :multiple="true"
                                                                 :searchable="true"
                                                                 placeholder="">
                                                    </multiselect>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <label>Competency Description</label>
                                    <textarea class="form-control" style="height:130px;overflow-y:scroll;" v-model="editModel.competencyDescription" maxlength="1024"></textarea>
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
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="editData">
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
        <div class="row" v-else-if="detail === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Competency > <span class="talent_font_red">Detail Competency</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Competency Information</h4>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Prefix Code<span class="talent_font_red">*</span></label>
                                                    <div class="row">
                                                        <div class="col-md-4">
                                                            <input class="form-control" disabled :value="detailCompetencyTypeCode" />
                                                        </div>

                                                        <div class="col-md-8">
                                                            <input class="form-control" disabled :value="detailPrefixCode" />
                                                        </div>
                                                    </div>
                                                </div>

                                                <div class="col-md-8">
                                                    <label>Competency Name<span class="talent_font_red">*</span></label>
                                                    <input class="form-control" disabled :value="detailCompetencyName" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-12 my-4">
                                            <div class="row">
                                                <div class="col-md-4">
                                                    <label>Type of Competency<span class="talent_font_red">*</span></label>
                                                    <input class="form-control" disabled :value="detailCompetencyTypeName" />
                                                </div>

                                                <div class="col-md-8">
                                                    <label>Key Action Code<span class="talent_font_red">*</span></label>
                                                    <multiselect v-model="detailKeyActions"
                                                                 track-by="keyActionId"
                                                                 label="keyActionCode"
                                                                 :options="keyActions"
                                                                 :multiple="true"
                                                                 :searchable="true"
                                                                 placeholder=""
                                                                 disabled>
                                                    </multiselect>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <label>Competency Description</label>
                                    <textarea class="form-control" style="height:130px;overflow-y:scroll;" disabled :value="detailCompetencyDescription"></textarea>
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
    import { isNullOrUndefined } from 'util';
    import { CompetencyService, CompetencyJoinViewModel, CompetencyFormModel, CompetencyTypeService, KeyActionService, CompetencyKeyActionMappingModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService'
    import { DateTime } from 'luxon';

    @Component({
        created: async function (this: Competency) {
            this.isBusy = true;
            await this.getAccess();
            this.fetch();
            this.initDropdownData();
        },
        //watch: {
        //    currentPage(val, oldVal) {
        //        this.fetch();
        //    }
        //},
        //methods: {
        //    fetch(this: Competency) {
        //    }
        //}
    })
    export default class Competency extends Vue {
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("COMP");
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }


        isBusy: boolean = false;
        itemPerPage: number = 10;
        success: boolean = false;
        successMessage: string = '';

        alertClose() {
            this.success = false;
            this.successMessage = '';
        }

        clearInput(){
            this.addModel = {
                prefixCode:null,
                competencyName:null,
                competencyId:null,
                competencyTypeId:null,
                competencyDescription:null,
                createdAt:null,
                updatedAt:null,
                competencyKeyActionMappings:null
            };

            this.editModel = {
                prefixCode:null,
                competencyName:null,
                competencyId:null,
                competencyTypeId:null,
                competencyDescription:null,
                createdAt:null,
                updatedAt:null,
                competencyKeyActionMappings:null
            }

            this.editCompetencyType = {
                competencyTypeId: null,
                competencyTypeName: ''
            };
            this.addCompetencyType = {
                competencyTypeId: null,
                competencyTypeName: ''
            };
            
            this.addKeyActions = [];
            this.editKeyActions = [];
        }

        // ---------------------------------------- FETCH -----------------------------------------

        currentPage: number = 1;
        filterDate = {
            start: null,
            end: null
        };
        filterCompetencyName: string = '';
        filterPrefixCode: string = '';
        filterKeyActionCode: string = '';
        sortBy: string = '';

        competencyMan: CompetencyService = new CompetencyService();
        keyActionMan: KeyActionService = new KeyActionService();
        competencyTypeMan: CompetencyTypeService = new CompetencyTypeService();
        competencies: CompetencyJoinViewModel = {
            listCompetencyJoinModel: null,
            totalItem: null
        };
        competencyTypes = {};
        keyActions = [];

        async fetch() {
            this.isBusy = true;
            this.competencies = await this.competencyMan.getAllJoinCompetencies(this.filterDate.start, this.filterDate.end, this.filterCompetencyName, this.filterPrefixCode, this.filterKeyActionCode, this.sortBy, this.currentPage);
            this.isBusy = false;
        }

        async initDropdownData() {
            this.keyActions = await this.keyActionMan.getAllKeyActions();
            this.competencyTypes = await this.competencyTypeMan.getAllCompetencyTypes();
        }

        reset() {
            this.filterDate = {
                start: null,
                end: null
            };
            this.filterCompetencyName = '';
            this.filterPrefixCode = '';
            this.filterKeyActionCode = '';
            this.ResetSort('');
            this.fetch();
        }

        // ----------------------------------------- CRUD ---------------------------------------------

        //Navigation
        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;

        //Create
        addValidation = false;
        addModel: CompetencyFormModel = {
            prefixCode: null,
            competencyName: null,
        };
        addCompetencyType = {
            competencyTypeId: null,
            competencyTypeName: ''
        };
        addKeyActions = [];
        addKeyActionError = false;
        addNameError = false;
        addPrefixError = false;

        addClicked() {
            if (!this.crud.create) {
                return
            }
            this.alertClose();
            this.add = true;
        }

        validateAdd() {
            if (this.addCompetencyType.competencyTypeId === 0 || this.addModel.prefixCode === '' || this.addModel.competencyName === '') {
                return false;
            }
            if (this.addModel.prefixCode !== null && this.addModel.prefixCode !== this.addModel.prefixCode.toUpperCase()) {
                return false;
            }
            return true;
        }

        async addData() {
            this.isBusy = true;

            if (!this.crud.create) {
                this.isBusy = false;
                return
            }
            this.addValidation = true;
            this.addKeyActionError = false;
            this.addNameError = false;
            this.addPrefixError = false;
            if (isNullOrUndefined(this.addCompetencyType.competencyTypeId)) {
                this.addCompetencyType.competencyTypeId = 0;
            }
            if (this.addModel.prefixCode == null) {
                this.addModel.prefixCode = '';
            }
            if (this.addModel.competencyName == null) {
                this.addModel.competencyName = '';
            }
            if (this.validateAdd() === false) {
                this.addValidation = false;
            }
            if (this.addKeyActions.length === 0) {
                this.addKeyActionError = true;
                this.addValidation = false;
            }
            var competencies = await this.competencyMan.getAllCompetenciesAsync();
            var found = competencies.findIndex(Q => Q.competencyName == this.addModel.competencyName);
            if (found > -1) {
                this.addNameError = true;
                this.addValidation = false;
            }
            var found2 = -1;
            if (this.addCompetencyType.competencyTypeId == 1)
                found2 = competencies.findIndex(Q => Q.prefixCode == 'H-' + this.addModel.prefixCode);
            else if (this.addCompetencyType.competencyTypeId == 2)
                found2 = competencies.findIndex(Q => Q.prefixCode == 'S-' + this.addModel.prefixCode);
            if (found2 > -1) {
                this.addPrefixError = true;
                this.addValidation = false;
            }
            if (this.addValidation === true) {
                this.addModel.competencyTypeId = this.addCompetencyType.competencyTypeId;
                this.addModel.competencyKeyActionMappings = [];
                for (var i = 0; i < this.addKeyActions.length; i++) {
                    this.addModel.competencyKeyActionMappings.push({
                        keyActionId: this.addKeyActions[i].keyActionId
                    })
                }
                await this.competencyMan.create(this.addModel);
                this.reset();
                this.fetch();
                this.addModel.competencyTypeId = null;
                this.addModel.prefixCode = null;
                this.addModel.competencyName = null;
                this.addModel.competencyDescription = null;
                this.addCompetencyType.competencyTypeId = null;
                this.addCompetencyType.competencyTypeName = '';
                this.addKeyActions = [];
                this.initDropdownData();
                this.closeClicked();
                this.successMessage = 'Success to Add Data!';
                this.success = true;
            }
            this.isBusy = false;
        }

        //Edit
        editValidation = false;
        editModel: CompetencyFormModel = {
            prefixCode: null,
            competencyName: null
        };
        editCompetencyType = {
            competencyTypeId: null,
            competencyTypeName: null
        };
        editKeyActions = [];
        editKeyActionError = false;
        editNameError = false;
        editPrefixError = false;

        async editClicked(competencyId: number) {
            if (!this.crud.update) {
                return
            }
            this.alertClose();
            var data = await this.competencyMan.getJoinCompetencyById(competencyId);

            this.editKeyActionError = false;
            this.editNameError = false;
            this.editPrefixError = false;
            this.editModel.competencyId = data.competencyId;
            this.editModel.prefixCode = data.prefixCode;
            this.editModel.competencyTypeId = data.competencyTypeId;
            this.editModel.competencyName = data.competencyName;
            this.editModel.competencyDescription = data.competencyDescription;
            this.editCompetencyType.competencyTypeId = data.competencyTypeId;
            this.editCompetencyType.competencyTypeName = data.competencyTypeName;
            this.editKeyActions = [];
            for (var i = 0; i < data.competencyKeyActionMappings.length; i++) {
                var found = this.keyActions.findIndex(Q => Q.keyActionId == data.competencyKeyActionMappings[i].keyActionId);

                if (found > -1) {
                    this.editKeyActions.push(this.keyActions[found]);
                }
            }

            this.edit = true;
        }

        validateEdit() {
            if (this.editModel.prefixCode === '' || this.editModel.competencyName === '' || this.editModel.prefixCode !== this.editModel.prefixCode.toUpperCase()) {
                return false;
            } else {
                return true;
            }
        }

        async editData() {
            this.isBusy = true;

            this.editValidation = true;
            this.editKeyActionError = false;
            this.editNameError = false;
            this.editPrefixError = false;
            if (this.validateEdit() === false) {
                this.editValidation = false;
            }
            if (this.editKeyActions.length === 0) {
                this.editKeyActionError = true;
                this.editValidation = false;
            }

            var competencies = await this.competencyMan.getAllCompetenciesAsync();
            var competency = competencies.findIndex(Q => Q.competencyId == this.editModel.competencyId);
            competencies.splice(competency, 1);

            var found = competencies.findIndex(Q => Q.competencyName == this.editModel.competencyName);
            if (found > -1) {
                this.editNameError = true;
                this.editValidation = false;
            }

            var found2 = -1;
            if (this.editCompetencyType.competencyTypeId == 1)
                found2 = competencies.findIndex(Q => Q.prefixCode == 'H-' + this.editModel.prefixCode);
            else if (this.editCompetencyType.competencyTypeId == 2)
                found2 = competencies.findIndex(Q => Q.prefixCode == 'S-' + this.editModel.prefixCode);
            if (found2 > -1) {
                this.editPrefixError = true;
                this.editValidation = false;
            }

            if (this.editValidation === true) {
                this.editModel.competencyTypeId = this.editCompetencyType.competencyTypeId;
                this.editModel.competencyKeyActionMappings = [];
                for (var i = 0; i < this.editKeyActions.length; i++) {
                    this.editModel.competencyKeyActionMappings.push({
                        competencyId: this.editModel.competencyId,
                        keyActionId: this.editKeyActions[i].keyActionId
                    })
                }
                await this.competencyMan.update(this.editModel);
                this.initDropdownData();
                this.reset();
                this.fetch();
                this.closeClicked();
                this.successMessage = 'Success to Edit Data!';
                this.success = true;
            }
            this.isBusy = false;
        }

        //Delete
        deleteCompetencyId;
        deleteIndex

        async deleteClicked(competencyId: number, index: number) {
            if (!this.crud.delete) {
                return
            }
            this.alertClose(); 
            this.deleteCompetencyId = competencyId;
            this.deleteIndex = index;
        }

        async deleteData() {
            if (!this.crud.delete) {
                return
            }
            this.isBusy = true;
            await this.competencyMan.delete(this.deleteCompetencyId);
            this.fetch();
            this.isBusy = false;
            //this.competencies.listCompetencyJoinModel.splice(this.deleteIndex, 1);
            this.successMessage = 'Success to Remove Data!';
            this.success = true;
        }

        //Detail
        detailCompetencyTypeCode;
        detailPrefixCode;
        detailCompetencyTypeName;
        detailCompetencyName;
        detailKeyActions;
        detailCompetencyDescription;

        async detailClicked(competencyId: number) {
            if (!this.crud.read) {
                return
            }
            this.alertClose();
            var data = await this.competencyMan.getJoinCompetencyById(competencyId);

            this.detailCompetencyTypeCode = data.competencyTypeName.substring(0, 1);
            this.detailPrefixCode = data.prefixCode;
            this.detailCompetencyTypeName = data.competencyTypeName;
            this.detailCompetencyName = data.competencyName;
            this.detailCompetencyDescription = data.competencyDescription;
            this.detailKeyActions = [];
            for (var i = 0; i < data.competencyKeyActionMappings.length; i++) {
                var found = this.keyActions.findIndex(Q => Q.keyActionId == data.competencyKeyActionMappings[i].keyActionId);

                if (found > -1) {
                    this.detailKeyActions.push(this.keyActions[found]);
                }
            }

            this.detail = true;
        }

        closeClicked() {
            this.add = false;
            this.edit = false;
            this.detail = false;

            this.clearInput();
        }

        // ---------------------------------------- Sorting ------------------------------------------

        //Variable untuk sorting
        prefixCode = new Sort();
        competencyName = new Sort();
        keyActionCode = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortPrefixCode
        async ClickSortPrefixCode() {
            this.ResetSort('prefixCode');
            //Sorting
            this.prefixCode.sorting();
            //Function Sorting
            if (this.prefixCode.sortup == true) {
                this.sortBy = 'prefixCode';
            }
            else if (this.prefixCode.sortdown == true) {
                this.sortBy = 'prefixCodeDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickCompetencyName
        async ClickCompetencyName() {
            this.ResetSort('competencyName');
            //Sorting
            this.competencyName.sorting();
            //Function Sorting
            if (this.competencyName.sortup == true) {
                this.sortBy = 'competencyName';
            }
            else if (this.competencyName.sortdown == true) {
                this.sortBy = 'competencyNameDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortKeyActionCode
        async ClickSortKeyActionCode() {
            this.ResetSort('keyActionCode');
            //Sorting
            this.keyActionCode.sorting();
            //Function Sorting
            if (this.keyActionCode.sortup == true) {
                this.sortBy = 'keyActionCode';
            }
            else if (this.keyActionCode.sortdown == true) {
                this.sortBy = 'keyActionCodeDesc';
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
                case 'prefixCode':
                    this.competencyName.reset();
                    this.keyActionCode.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'competencyName':
                    this.prefixCode.reset();
                    this.keyActionCode.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'keyActionCode':
                    this.prefixCode.reset();
                    this.competencyName.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.prefixCode.reset();
                    this.competencyName.reset();
                    this.keyActionCode.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.prefixCode.reset();
                    this.competencyName.reset();
                    this.keyActionCode.reset();
                    this.createdDate.reset();
                    return;
                default:
                    this.sortBy = '';
                    this.prefixCode.reset();
                    this.competencyName.reset();
                    this.keyActionCode.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
            }
        }
    }
</script>
