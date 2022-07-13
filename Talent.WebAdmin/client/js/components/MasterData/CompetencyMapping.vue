<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="row" v-if="add == false && edit == false && viewDetail == false">
            <div class="col col-md-12">
                <!--TITLE-->
                <!--<h2><fa icon="cog"></fa> <fa icon="user"></fa> <fa icon="database"></fa> <fa icon="folder"></fa> Menu > <span class="talent_font_red">Page</span></h2>-->
                <h3><fa icon="database"></fa> Master Data > <span class="talent_font_red">Competency Mapping</span></h3>

                <div class="mb-4"></div>
                <!--<div v-if="successMessageShow == true" class="alert alert-success">{{successMessage}}</div>-->

                <div v-if="successMessageShow == true" class="alert alert-success alert-dismissible fade show" role="alert">
                    {{successMessage}}
                    <button type="button" class="close" data-dismiss="alert" @click.prevent="ResetSuccessMessage()" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Competency Mapping</h4>
                    <div class="mb-4"></div>

                    <!--2 Column Menu-->
                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker v-model="advancedSearch.Date" class="v-date-style-report" mode="range" :firstDayOfWeek="2" :value="null" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                        <input v-model="advancedSearch.CompetencyName" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mb-4"></div>

                    <!--2 Column Menu-->
                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Mapping Code</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <div class="input-group">
                                            <input v-model="advancedSearch.CompetencyMappingCode" class="form-control" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Type of Evaluation</span>
                                </div>
                                <div class="col-md-8">
                                    <select v-model="advancedSearch.TypeofEvalutaion" class="form-control">
                                        <option v-for="evaluationType in evaluationTypes" :value="evaluationType.evaluationTypeName">{{evaluationType.evaluationTypeName}}</option>
                                    </select>
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
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="AdvanceSearch()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="ResetAdvanceSearch()">
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
                    <h4>Competency Mapping List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a v-if="(competencyMappings.totalData - ((currentPage - 1) * pageSize) ) >= pageSize">Showing {{pageSize}} of {{competencyMappings.totalData}} Entry(s)</a>
                            <a v-else>Showing {{(competencyMappings.totalData) - ((currentPage - 1) * pageSize)}} of {{competencyMappings.totalData}} Entry(s)</a>
                            <button class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="add = true" if="crud.create">Add Mapping</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCompetencyMappingCode()">Competency Mapping Code<fa icon="sort" v-if="competencyMappingCode.sort == true"></fa><fa icon="sort-up" v-if="competencyMappingCode.sortup == true"></fa><fa icon="sort-down" v-if="competencyMappingCode.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickCompetencyName()">Competency Name <fa icon="sort" v-if="competencyName.sort == true"></fa><fa icon="sort-up" v-if="competencyName.sortup == true"></fa><fa icon="sort-down" v-if="competencyName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortTypeofEvalutaion()">Type of Evaluation <fa icon="sort" v-if="typeofEvaluation.sort == true"></fa><fa icon="sort-up" v-if="typeofEvaluation.sortup == true"></fa><fa icon="sort-down" v-if="typeofEvaluation.sortdown == true"></fa></a>
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
                                <tr v-for="c in competencyMappings.data">
                                    <td>
                                        {{c.competencyMappingCode}}
                                    </td>
                                    <td>
                                        {{c.competencyName}}
                                    </td>
                                    <td>
                                        {{c.typeofEvaluation}}
                                    </td>
                                    <td>
                                        {{c.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{c.updatedAt | dateFormat}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="ViewDetailCompetencyMappingInitial(c.competencyId, c.evaluationTypeId)" >View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="EditCompetencyMappingInitial(c.competencyId, c.evaluationTypeId)" >Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#modalDelete" @click.prevent="GetDelete(c.competencyId, c.evaluationTypeId)">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="col-md-12 d-flex justify-content-center mt-3">
                        <paginate :currentPage.sync="currentPage" :total-data=competencyMappings.totalData :limit-data=pageSize @update:currentPage="FetchData()"></paginate>
                    </div>
                </div>
                <div class="mb-4"></div>

            </div>
        </div>

        <!--Add-->
        <div class="row" v-if="add == true">
            <div class="col-md-12">
                <h3><fa icon="database"></fa> Master Data > Competency Mapping > <span class="talent_font_red">Add Competency Mapping</span></h3>
                <div class="mb-4"></div>
                <!--<div v-if="errorMessageShow == true" class="alert alert-danger">{{errorMessage}}</div>-->
                <div v-if="errorMessageShow == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                    {{errorMessage}}
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <!--Add-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Competency Mapping Information</h4>
                    <div class="mb-4"></div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Competency Name<span class="talent_font_red">*</span></label>
                                    <select v-model="competencyMappingsAdd.competencyId" class="form-control" @change.prevent="GetCompetencyPrefixCodeById(competencyMappingsAdd.competencyId)">
                                        <option hidden disabled value="">-</option>
                                        <option v-for="competency in competencies" :value="competency.competencyId">{{competency.competencyName}}</option>
                                    </select>
                                </div>

                                <div class="col-md-6">
                                    <label>Type of Evaluation<span class="talent_font_red">*</span></label>
                                    <div>
                                        <div class="form-check form-check-inline" v-for="(evaluationType, index) in evaluationTypes">
                                            <input v-model="competencyMappingsAdd.evaluationTypeId" class="form-check-input" type="radio" name="TypeofEvaluationAdd" :id="'TypeofEvaluationAdd' + index" :value="evaluationType.evaluationTypeId">
                                            <label class="form-check-label" :for="'TypeofEvaluationAdd' + index">{{evaluationType.evaluationTypeName}}</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mb-4"></div>

                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Prefix Code<span class="talent_font_red">*</span></label>
                                    <input class="form-control" disabled v-model="prefixCode" />
                                </div>

                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Bonus Score < 50 </label>
                                            <div class="input-group">
                                                <input type="number" min="0" v-model="competencyMappingsAdd.bonusScoreLt50" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Bonus Score >= 50 </label>
                                            <div class="input-group">
                                                <input type="number" min="0" v-model="competencyMappingsAdd.bonusScoreMte50" class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mb-4"></div>
                </div>
                <div class="mb-4"></div>
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="AddCompetencyMappingClose">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="AddCompetencyMapping()">
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
                <h3><fa icon="database"></fa> Master Data > Competency Mapping > <span class="talent_font_red">Edit Competency Mapping</span></h3>
                <div class="mb-4"></div>
                <!--<div v-if="errorMessageShow == true" class="alert alert-danger">{{errorMessage}}</div>-->
                <div v-if="errorMessageShow == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                    {{errorMessage}}
                    <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <!--Edit-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Competency Mapping Information</h4>
                    <div class="mb-4"></div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Competency Name<span class="talent_font_red">*</span></label>
                                    <select v-model="competencyMappingsEdit.competencyId" class="form-control" @change.prevent="GetCompetencyPrefixCodeById(competencyMappingsEdit.competencyId)">
                                        <option hidden disabled value="">-</option>
                                        <option v-for="competency in competencies" :value="competency.competencyId">{{competency.competencyName}}</option>
                                    </select>
                                </div>

                                <div class="col-md-6">
                                    <label>Type of Evaluation<span class="talent_font_red">*</span></label>
                                    <div>
                                        <div class="form-check form-check-inline" v-for="(evaluationType, index) in evaluationTypes">
                                            <input v-model="competencyMappingsEdit.evaluationTypeId" class="form-check-input" type="radio" name="TypeofEvaluationEdit" :id="'TypeofEvaluationAdd' + index" :value="evaluationType.evaluationTypeId">
                                            <label class="form-check-label" :for="'TypeofEvaluationEdit' + index">{{evaluationType.evaluationTypeName}}</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mb-4"></div>

                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Prefix Code<span class="talent_font_red">*</span></label>
                                    <input class="form-control" disabled v-model="prefixCode" />
                                </div>

                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Bonus Score < 50 </label>
                                            <div class="input-group">
                                                <input type="number" min="0" v-model="competencyMappingsEdit.bonusScoreLt50" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Bonus Score >= 50 </label>
                                            <div class="input-group">
                                                <input type="number" min="0" v-model="competencyMappingsEdit.bonusScoreMte50" class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mb-4"></div>
                </div>
                <div class="mb-4"></div>
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="EditCompetencyMappingClose">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="EditCompetencyMapping">
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
                <h3><fa icon="database"></fa> Master Data > Competency Mapping > <span class="talent_font_red">View Detail Competency Mapping</span></h3>
                <div class="mb-4"></div>
                <!--View Detail-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Competency Mapping Information</h4>
                    <div class="mb-4"></div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Competency Name<span class="talent_font_red">*</span></label>
                                    <select disabled v-model="competencyMappingsViewDetail.competencyId" class="form-control" @change.prevent="GetCompetencyPrefixCodeById(competencyMappingsEdit.competencyId)">
                                        <option hidden disabled value="">-</option>
                                        <option v-for="competency in competencies" :value="competency.competencyId">{{competency.competencyName}}</option>
                                    </select>
                                </div>

                                <div class="col-md-6">
                                    <label>Type of Evaluation<span class="talent_font_red">*</span></label>
                                    <div>
                                        <div class="form-check form-check-inline" v-for="(evaluationType, index) in evaluationTypes">
                                            <input v-model="competencyMappingsViewDetail.evaluationTypeId" class="form-check-input" type="radio" name="TypeofEvaluationViewDetail" :id="'TypeofEvaluationAdd' + index" :value="evaluationType.evaluationTypeId" disabled>
                                            <label class="form-check-label" :for="'TypeofEvaluationViewDetail' + index">{{evaluationType.evaluationTypeName}}</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mb-4"></div>

                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Prefix Code<span class="talent_font_red">*</span></label>
                                    <input class="form-control" disabled v-model="prefixCode" />
                                </div>

                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Bonus Score < 50 </label>
                                            <div class="input-group">
                                                <input v-model="competencyMappingsViewDetail.bonusScoreLt50" class="form-control" disabled />
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Bonus Score >= 50 </label>
                                            <div class="input-group">
                                                <input v-model="competencyMappingsViewDetail.bonusScoreMte50" class="form-control" disabled />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="mb-4"></div>
                </div>
                <div class="mb-4"></div>
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="ViewDetailCompetencyMappingClose">
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
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
                                <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="DeleteCompetencyMapping()">Delete</button>
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
    import { CompetencyMappingService, EvaluationTypesService, CompetencyService, CompetencyMappingPaginate, EvaluationTypesViewModel, CompetencyMappingInsertModel, CompetencyModel, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService'    
    @Component({
        props: [],
        created: async function (this: CompetencyMapping2) {
            //this.isBusy = true;
            await this.getAccess();
            await this.GetAllEvaluationTypes();
            await this.GetAllCompetencies();
            await this.Initialize();
        }
    })
    export default class CompetencyMapping2 extends Vue {
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("COMPM");
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
        //Controller
        add: boolean = false;
        edit: boolean = false;
        viewDetail: boolean = false;

        //initial data
        currentPage: number = 1;
        pageSize: number = 10;
        competencyMappings: CompetencyMappingPaginate = { data: null, totalData: null };
        evaluationTypes: Array<EvaluationTypesViewModel> = [];
        competencies: Array<CompetencyModel> = []; 
        //api: Client = new Client;

        CompetencyMappingApi: CompetencyMappingService = new CompetencyMappingService;
        EvaluationTypeApi: EvaluationTypesService = new EvaluationTypesService;
        CompetencyApi: CompetencyService = new CompetencyService;

        //CompetencyMappingApi: Client = new Client;
        //EvaluationTypeApi: Client = new Client;
        //CompetencyApi: Client = new Client;

        //ReadOnly
        advancedSearch: ICompetencyMappingModels = {
            CompetencyMappingCode: "", CompetencyName: "", Date: { start: null, end: null }, PageIndex: this.currentPage, PageSize: this.pageSize, SortBy: "", TypeofEvalutaion: ""
        };

        async GetAllEvaluationTypes() {
            var data = await this.EvaluationTypeApi.getAllEvaluationTypesAsync();
            this.evaluationTypes = data;
            console.log(this.evaluationTypes)
        }

        async GetAllCompetencies() {
            var data = await this.CompetencyApi.getAllCompetenciesAsync();
            this.competencies = data;
        }

        async Initialize() {
            await this.FetchData();
        }
        
        NumberShowData: number = 0;

        async FetchData() {
            this.isBusy = true;
            let data = await this.CompetencyMappingApi.getAllCompetencyMappingPaginateAsync(
                this.advancedSearch.Date.start,
                this.advancedSearch.Date.end,
                this.advancedSearch.CompetencyName,
                this.advancedSearch.CompetencyMappingCode,
                this.advancedSearch.TypeofEvalutaion,
                this.advancedSearch.SortBy,
                this.currentPage,
                this.advancedSearch.PageSize
            );
            this.competencyMappings.data = data.data;
            this.competencyMappings.totalData = data.totalData;
            this.isBusy = false;
        }

        //convertDateTime(rawDateTime: string) {
        //    if (rawDateTime === "") {
        //        rawDateTime = DateTime.local().toISO();
        //    }
        //    return DateTime.fromISO(rawDateTime).toFormat("dd/LL/yyyy");
        //}

        async AdvanceSearch() {
            this.isBusy = true;
            let data = await this.CompetencyMappingApi.getAllCompetencyMappingPaginateAsync(
                this.advancedSearch.Date.start,
                this.advancedSearch.Date.end,
                this.advancedSearch.CompetencyName,
                this.advancedSearch.CompetencyMappingCode,
                this.advancedSearch.TypeofEvalutaion,
                this.advancedSearch.SortBy,
                1,
                this.advancedSearch.PageSize
            );
            this.competencyMappings.data = data.data;
            this.competencyMappings.totalData = data.totalData;
            this.isBusy = false;
        }

        ResetAdvanceSearch() {
            this.currentPage = 1;
            this.advancedSearch = {
                CompetencyMappingCode: "", CompetencyName: "", Date: { start: null, end: null }, PageIndex: this.currentPage, PageSize: this.pageSize, SortBy: "", TypeofEvalutaion: ""
            };
            this.ResetSort('');
            this.FetchData();
        }

        //Variable untuk sorting
        competencyMappingCode = new Sort();
        competencyName = new Sort();
        typeofEvaluation = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortCompetencyMappingCode
        async ClickSortCompetencyMappingCode() {
            this.ResetSort('competencyMappingCode');
            //Sorting
            this.competencyMappingCode.sorting();
            if (this.competencyMappingCode.sortup == true) {
                this.advancedSearch.SortBy = "CompetencyMappingCodeAsc"
            } else if (this.competencyMappingCode.sortdown == true) {
                this.advancedSearch.SortBy = "CompetencyMappingCodeDesc"
            }
            else {
                this.advancedSearch.SortBy = "";
            }
            this.FetchData();
            return;
        }

        //ClickCompetencyName
        async ClickCompetencyName() {
            this.ResetSort('competencyName');
            //Sorting
            this.competencyName.sorting();
            if (this.competencyName.sortup == true) {
                this.advancedSearch.SortBy = "CompetencyNameAsc"
            } else if (this.competencyName.sortdown == true) {
                this.advancedSearch.SortBy = "CompetencyNameDesc"
            }
            else {
                this.advancedSearch.SortBy = "";
            }
            this.FetchData();
            return;
        }

        //ClickSortTypeofEvalutaion
        async ClickSortTypeofEvalutaion() {
            this.ResetSort('typeofEvaluation');
            //Sorting
            this.typeofEvaluation.sorting();
            //Function Sorting
            if (this.typeofEvaluation.sortup == true) {
                this.advancedSearch.SortBy = "TypeofEvaluationAsc"
            } else if (this.typeofEvaluation.sortdown == true) {
                this.advancedSearch.SortBy = "TypeofEvaluationDesc"
            }
            else {
                this.advancedSearch.SortBy = "";
            }
            this.FetchData();
            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting
            if (this.createdDate.sortup == true) {
                this.advancedSearch.SortBy = "CreatedDateAsc"
            } else if (this.createdDate.sortdown == true) {
                this.advancedSearch.SortBy = "CreatedDateDesc"
            }
            else {
                this.advancedSearch.SortBy = "";
            }
            this.FetchData();
            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting
            if (this.updatedDate.sortup == true) {
                this.advancedSearch.SortBy = "UpdatedDateAsc"
            } else if (this.updatedDate.sortdown == true) {
                this.advancedSearch.SortBy = "UpdatedDateDesc"
            }
            else {
                this.advancedSearch.SortBy = "";
            }
            this.FetchData();
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'competencyMappingCode':
                    this.competencyName.reset();
                    this.typeofEvaluation.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'competencyName':
                    this.competencyMappingCode.reset();
                    this.typeofEvaluation.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'typeofEvaluation':
                    this.competencyMappingCode.reset();
                    this.competencyName.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.competencyMappingCode.reset();
                    this.competencyName.reset();
                    this.typeofEvaluation.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.competencyMappingCode.reset();
                    this.competencyName.reset();
                    this.typeofEvaluation.reset();
                    this.createdDate.reset();
                    return;
                default:
                    this.competencyMappingCode.reset();
                    this.competencyName.reset();
                    this.typeofEvaluation.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
            }
        }

        //-----------ADD
        competencyMappingsAdd: CompetencyMappingInsertModel = { competencyId: null, evaluationTypeId: null, bonusScoreLt50: null, bonusScoreMte50: null}
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

        async AddCompetencyMapping() {
            if (!this.crud.create) {
                return
            }

            this.isBusy = true;
            if (this.competencyMappingsAdd.competencyId == null || this.competencyMappingsAdd.evaluationTypeId == null) {
                this.errorMessage= "Competency Name & Type of Evaluation must be filled!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if (this.competencyMappingsAdd.bonusScoreLt50 < 0 || this.competencyMappingsAdd.bonusScoreMte50 < 0) {
                this.errorMessage = "Bonus Score Must Be Greater Than 0!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if (this.competencyMappingsAdd.bonusScoreLt50 == null || (this.competencyMappingsAdd.bonusScoreLt50).toString() == "") {
                this.competencyMappingsAdd.bonusScoreLt50 = 0;
            }

            if (this.competencyMappingsAdd.bonusScoreMte50 == null || (this.competencyMappingsAdd.bonusScoreMte50).toString() == "") {
                this.competencyMappingsAdd.bonusScoreMte50 = 0;
            }
            
            var add = await this.CompetencyMappingApi.insertCompetencyMappingAsync(this.competencyMappingsAdd);
            this.isBusy = false;
            
            if (add == false) {
                this.errorMessage = "Competency Mapping already exist!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }
            this.isBusy = false;
            this.ResetAdvanceSearch();
            this.FetchData();
            this.add = false;
            this.errorMessageShow = false;
            this.competencyMappingsAdd = { competencyId: null, evaluationTypeId: null, bonusScoreLt50: null, bonusScoreMte50: null }
            this.prefixCode = null;
            this.successMessage = "Success to add data!";
            this.successMessageShow = true;

        }

        AddCompetencyMappingClose() {
            this.add = false;
            this.competencyMappingsAdd = { competencyId: null, evaluationTypeId: null, bonusScoreLt50: null, bonusScoreMte50: null}
            this.prefixCode = null;
            this.errorMessageShow = false;
            this.successMessageShow = false;
        }


        prefixCode: string = null
        async GetCompetencyPrefixCodeById(id: number) {
            var data = await this.CompetencyApi.getCompetenciesByIdAsync(id);
            this.prefixCode = data.prefixCode;
        }

        //----------EDIT
        competencyMappingsEdit: CompetencyMappingInsertModel = { competencyId: null, evaluationTypeId: null, bonusScoreLt50: null, bonusScoreMte50: null }

        competencyId: number = null;
        evaluationTypeId: number = null;
        async EditCompetencyMappingInitial(competencyId: number, evaluationTypeId: number) {
            if (!this.crud.update) {
                return
            }

            this.isBusy = true;
            this.edit = true;
            this.competencyId = competencyId;
            this.evaluationTypeId = evaluationTypeId;
            var data = await this.CompetencyMappingApi.getAllCompetencyMappingByCompetencyIdAndEvaluationIdPaginateAsync(competencyId, evaluationTypeId);
            this.competencyMappingsEdit = data;
            var data2 = await this.CompetencyApi.getCompetenciesByIdAsync(this.competencyId);
            this.prefixCode = data2.prefixCode;
            this.isBusy = false;
        }

        async EditCompetencyMapping() {
            if (!this.crud.update) {
                return
            }

            this.isBusy = true;
            if (this.competencyMappingsEdit.competencyId == null || this.competencyMappingsEdit.evaluationTypeId == null) {
                this.errorMessage = "Competency Name & Type of Evaluation must be filled!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if (this.competencyMappingsEdit.bonusScoreLt50 < 0 || this.competencyMappingsEdit.bonusScoreMte50 < 0) {
                this.errorMessage = "Bonus Score Must Be Greater Than 0!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if (this.competencyMappingsEdit.bonusScoreLt50 == null || (this.competencyMappingsEdit.bonusScoreLt50).toString() == "") {
                this.competencyMappingsEdit.bonusScoreLt50 = 0;
            }

            if (this.competencyMappingsEdit.bonusScoreMte50 == null || (this.competencyMappingsEdit.bonusScoreMte50).toString() == "") {
                this.competencyMappingsEdit.bonusScoreMte50 = 0;
            }

            var data = await this.CompetencyMappingApi.updateCompetencyMappingAsync(this.competencyId, this.evaluationTypeId, this.competencyMappingsEdit);
            
            if (data == false) {
                this.errorMessage = "Competency Mapping already exist!"
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }
            this.isBusy = false;
            this.ResetAdvanceSearch();
            this.FetchData();
            this.edit = false;
            this.competencyMappingsEdit = { competencyId: null, evaluationTypeId: null, bonusScoreLt50: null, bonusScoreMte50: null}
            this.prefixCode = null;

            this.successMessage = "Success to edit data!";
            this.successMessageShow = true;
            this.errorMessageShow = false;
        }

        EditCompetencyMappingClose() {
            this.edit = false;
            this.competencyMappingsEdit = { competencyId: null, evaluationTypeId: null, bonusScoreLt50: null, bonusScoreMte50: null}
            this.prefixCode = null;
            this.errorMessageShow = false;
            this.successMessageShow = false;
        }

        //-----------View Detail
        competencyMappingsViewDetail: CompetencyMappingInsertModel = { competencyId: null, evaluationTypeId: null, bonusScoreLt50: null, bonusScoreMte50: null, }

        async ViewDetailCompetencyMappingInitial(competencyId: number, evaluationTypeId: number) {
            if (!this.crud.read) {
                return
            }

            this.isBusy = true;
            this.viewDetail = true;
            this.competencyId = competencyId;
            this.evaluationTypeId = evaluationTypeId;
            var data = await this.CompetencyMappingApi.getAllCompetencyMappingByCompetencyIdAndEvaluationIdPaginateAsync(competencyId, evaluationTypeId);
            this.competencyMappingsViewDetail = data;
            var data2 = await this.CompetencyApi.getCompetenciesByIdAsync(this.competencyId);
            this.prefixCode = data2.prefixCode;
            this.isBusy = false;
        }

        ViewDetailCompetencyMappingClose() {
            this.viewDetail = false;
            this.competencyMappingsViewDetail = { competencyId: null, evaluationTypeId: null, bonusScoreLt50: null, bonusScoreMte50: null}
            this.prefixCode = null;
            this.errorMessageShow = false;
            this.successMessageShow = false;
        }

        //-----------Remove
        //competencyId: number = null;
        //evaluationTypeId: number = null;
        GetDelete(competencyId: number, evaluationTypeId: number) {
            if (!this.crud.delete) {
                return
            }

            this.competencyId = competencyId;
            this.evaluationTypeId = evaluationTypeId;
        }

        async DeleteCompetencyMapping() {
            if (!this.crud.delete) {
                return
            }

            await this.CompetencyMappingApi.deleteCompetencyMappingAsync(this.competencyId, this.evaluationTypeId);
            this.FetchData();
            this.successMessage = "Success to delete data!";
            this.successMessageShow = true;
        }
    }
</script>
