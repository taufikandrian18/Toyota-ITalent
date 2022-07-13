<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <!--MAIN PAGE-->
        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-md-4"><fa icon="database"></fa> Master Data > <span class="talent_font_red">Assessment</span></h3>
                <div class="alert alert-success" v-if="success !== ''">
                    {{success}}
                    <button type="button" class="close" aria-label="Close" @click="success = ''">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4 class="mb-md-4">Search Assessment</h4>

                    <!--4 Column Menu-->
                    <div class="row mb-md-4">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group talent_front">
                                        <v-date-picker class="v-date-style-report" mode="range" :firstDayOfWeek="2"
                                                       v-model="filter.DateFilter" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                    <span>Outlet</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.outletName" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Assessment Name</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.assessmentName" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--4 Column Menu-->
                    <div class="row mb-md-4">

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Position</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.positionName" />
                                    </div>
                                </div>
                            </div>
                        </div>


                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Employee Name</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.employeeName" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Publisher</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.publisher" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--Search Button-->
                    <div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="d-flex align-items-end flex-column">
                                    <div>
                                        <button class="btn talent_bg_blue talent_font_white"
                                                @click="fetch()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white"
                                                @click="resetFilter()">
                                            Reset
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4>Assessment List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{grid.assessments.length}} of {{grid.totalFilterData}} Entry(s)</a>
                            <button class="btn talent_bg_cyan talent_roundborder talent_font_white" @click="addClicked" v-if="crud.create">Add Assessment</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortAsssessment()">Assessment Name <fa icon="sort" v-if="assessmentName.sort == true"></fa><fa icon="sort-up" v-if="assessmentName.sortup == true"></fa><fa icon="sort-down" v-if="assessmentName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickEmployeeName()">Employee Name <fa icon="sort" v-if="employeeName.sort == true"></fa><fa icon="sort-up" v-if="employeeName.sortup == true"></fa><fa icon="sort-down" v-if="employeeName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortPosition()">Position <fa icon="sort" v-if="position.sort == true"></fa><fa icon="sort-up" v-if="position.sortup == true"></fa><fa icon="sort-down" v-if="position.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortOutlet()">Outlet <fa icon="sort" v-if="outlet.sort == true"></fa><fa icon="sort-up" v-if="outlet.sortup == true"></fa><fa icon="sort-down" v-if="outlet.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortPublisher()">Publisher <fa icon="sort" v-if="publisher.sort == true"></fa><fa icon="sort-up" v-if="publisher.sortup == true"></fa><fa icon="sort-down" v-if="publisher.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                    </th>
                                    <th v-if="crud.read || crud.update || crud.delete" colspan="4">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="assessment in grid.assessments">
                                    <td>
                                        {{assessment.assessmentName}}
                                    </td>
                                    <td>
                                        {{assessment.employeeName}}
                                    </td>
                                    <td>
                                        {{assessment.positionName}}
                                    </td>
                                    <td>
                                        {{assessment.outletName}}
                                    </td>
                                    <td>
                                        {{assessment.publisherName}}
                                    </td>
                                    <td>
                                        {{assessment.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{assessment.updatedAt | dateFormat}}
                                    </td>

                                    <td class="talent_nopadding_important" v-if="crud.read">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click="detailClicked(assessment.assessmentId)">View Detail</button>
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.update">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click="editClicked(assessment.assessmentId)">Edit</button>
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.delete">
                                        <button type="button" class="btn btn-block talent_bg_red talent_font_white"
                                                data-backdrop="static" data-keyboard="false"
                                                data-toggle="modal" data-target="#deleteConfirmation"
                                                @click="setDelete(assessment.assessmentId, assessment.assessmentName)">
                                            Remove
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--Pagination-->
                    <div class="row justify-content-center">
                        <paginate :currentPage.sync="filter.pageIndex"
                                  :total-data="grid.totalFilterData"
                                  :limit-data="filter.pageSize"
                                  @update:currentPage="fetch()"></paginate>
                    </div>
                </div>
            </div>
        </div>

        <!--ADD, UPDATE DAN DETAIL-->
        <div class="row" v-else>
            <div class="col-md-12">
                <h3 v-show="add === true"><fa icon="database"></fa> Master Data > Assessment > <span class="talent_font_red">Add Assessment</span></h3>
                <h3 v-show="edit === true"><fa icon="database"></fa> Master Data > Assessment > <span class="talent_font_red">Edit Assessment</span></h3>
                <h3 v-show="detail === true"><fa icon="database"></fa> Master Data > Assessment > <span class="talent_font_red">View Detail Assessment</span></h3>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4>Assessment Information</h4>
                    <div class="alert alert-danger" v-show="errors.items.length > 0">
                        <button type="button" class="close" aria-label="Close" @click="$validator.errors.clear()">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <div v-for="error in errors.all()">{{error}}</div>
                    </div>

                    <div class="row mb-md-4">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Dealer<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="assessmentForm.dealer"
                                                 name="Dealer"
                                                 v-validate="'required-multiselect'"
                                                 :options="dealerOption"
                                                 :allow-empty="false"
                                                 label="dealerName"
                                                 deselect-label="Selected"
                                                 @input="fillOutletOption"
                                                 :disabled="detail === true">
                                    </multiselect>
                                </div>

                                <div class="col-md-6">
                                    <label>Outlet<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="assessmentForm.outlet"
                                                 name="Outlet"
                                                 v-validate="'required-multiselect'"
                                                 :options="outletOption"
                                                 :allow-empty="false"
                                                 label="outletName"
                                                 deselect-label="Selected"
                                                 @input="fillPositionOption"
                                                 :disabled="detail === true || Object.keys(assessmentForm.dealer).length === 0">
                                    </multiselect>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-md-4">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Position<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="assessmentForm.position"
                                                 name="Position"
                                                 v-validate="'required-multiselect'"
                                                 :options="positionOption"
                                                 :allow-empty="false"
                                                 label="positionName"
                                                 deselect-label="Selected"
                                                 @input="fillEmployeeOption"
                                                 :disabled="detail === true || Object.keys(assessmentForm.outlet).length === 0">
                                    </multiselect>
                                </div>

                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-4">
                                            <label>Employee ID<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <input class="form-control" v-model="assessmentForm.employee.employeeId" disabled />
                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <label>Employee Name<span class="talent_font_red">*</span></label>
                                            <multiselect v-model="assessmentForm.employee"
                                                         name="Employee"
                                                         v-validate="'required-multiselect'"
                                                         :options="employeeOption"
                                                         :allow-empty="false"
                                                         label="employeeName"
                                                         track-by="employeeName"
                                                         deselect-label="Selected"
                                                         :disabled="detail === true || Object.keys(assessmentForm.position).length === 0">
                                            </multiselect>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Assessment Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group mb-md-4">
                                        <input class="form-control" :disabled="detail === true" v-model="assessmentForm.assessmentName" name="Assessment Name" v-validate="'required'" maxlength="255"/>
                                    </div>
                                    <label>Publisher<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" :disabled="detail === true" v-model="assessmentForm.publisher" name="Publisher" v-validate="'required'" maxlength="255"/>
                                    </div>
                                </div>

                                <div class="col-md-6">
                                    <label>Description</label>
                                    <textarea class="form-control talent_overflowy description-height" :disabled="detail === true" v-model="assessmentForm.description" maxlength="1024"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row mb-md-4">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Upload File<span class="talent_font_red">*</span></label>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <div class="custom-file">
                                                        <input type="file"
                                                               class="custom-file-input"
                                                               id="customFile"
                                                               name="Upload File"
                                                               @change="handler"
                                                               :disabled="detail === true"
                                                               v-validate="'required'">
                                                        <label class="custom-file-label overflow-hidden" for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    *Recommended File: PDF
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <img :src="imageData.length ? imageData : '/upload-image2.png'" alt="Alternate Text" class="col-md-12" />
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="insert" v-if="add === true">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="update" v-if="edit === true">
                                        Save
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
                                        @click="deleteAssessment(toBeDeletedAssessment.id)">
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
    import VeeValidate from 'vee-validate';

    import { Sort } from '../../class/Sort';
    import { DealerDropdownModel, DropdownService, OutletDropdownModel, PositionDropdownModel, EmployeeDropdownModel, AssessmentCreateModel, AssessmentService, AssessmentGridModel, AssessmentUpdateModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService';
    import { IAssessmentFormModel, IAssessmentFilterModel } from '../../models/IAssessmentModel'
    import { BlobService } from '../../services/BlobService';
    import Message from '../../class/Message';
    import { PageEnums } from '../../enum/PageEnums';

    Vue.use(VeeValidate, { events: '' });

    @Component({
        created: async function (this: Assessment) {
            await this.getAccess();
            await this.fetch();
            await this.fillDealerOption();
        }
    })

    export default class Assessment extends Vue {
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Assessment);
            this.crud = data
        }

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        dropMan: DropdownService = new DropdownService();
        assMan: AssessmentService = new AssessmentService();

        props: any = {
            placeholder: "",
            readonly: true
        };

        isLoading: boolean = false;

        framework: string;
        compiler: string;

        success: string = '';

        grid: AssessmentGridModel = {
            assessments: [],
            totalFilterData: 0
        };

        filter: IAssessmentFilterModel = {
            sortBy: '',
            pageIndex: 1,
            pageSize: 10,
            DateFilter: {
                start: null,
                end: null
            },
            assessmentName: '',
            employeeName: '',
            outletName: '',
            positionName: '',
            publisher: ''
        }

        dealerOption: DealerDropdownModel[] = [];
        outletOption: OutletDropdownModel[] = [];
        positionOption: PositionDropdownModel[] = [];
        employeeOption: EmployeeDropdownModel[] = [];

        formData: FormData = new FormData();
        imageData: any = [];
        imageName: string = '';
        isImageChanged: boolean = false;

        message = Message;

        assessmentForm: IAssessmentFormModel = {
            dealer: {},
            outlet: {},
            position: {},
            employee: {},
            assessmentName: '',
            description: '',
            publisher: '',
            fileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        assessmentInsertForm: AssessmentCreateModel = {
            employeeId: '',
            assessmentName: '',
            description: '',
            publisher: '',
            fileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        assessmentUpdateForm: AssessmentUpdateModel = {
            assessmentId: 0,
            employeeId: '',
            assessmentName: '',
            description: '',
            publisher: '',
            blobId: '',
            fileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        toBeDeletedAssessment: { id: number, name: string } = {
            id: 0,
            name: ''
        }

        async fetch() {
            this.isLoading = true;
            this.grid = await this.assMan.getAllAssessment(this.filter.DateFilter.start
                , this.filter.DateFilter.end
                , this.filter.outletName
                , this.filter.assessmentName
                , this.filter.positionName
                , this.filter.employeeName
                , this.filter.publisher
                , this.filter.sortBy
                , this.filter.pageIndex
                , this.filter.pageSize);
            this.isLoading = false;
        }

        async resetFilter() {
            this.filter.DateFilter = {
                start: null,
                end: null
            };

            this.filter.assessmentName = '';
            this.filter.employeeName = '';
            this.filter.outletName = '';
            this.filter.positionName = '';
            this.filter.publisher = '';

            this.ResetSort('');

            await this.fetch();
        }

        async fillDealerOption() {
            this.isLoading = true;
            this.dealerOption = await this.dropMan.getDealerDropdown();
            this.isLoading = false;
        }

        async fillOutletOption() {
            this.isLoading = true;
            this.outletOption = await this.dropMan.getOutletDropdown(this.assessmentForm.dealer.dealerId);
            this.assessmentForm.outlet = {};
            this.assessmentForm.position = {};
            this.assessmentForm.employee = {};
            this.isLoading = false;
        }

        async fillPositionOption() {
            this.isLoading = true;
            this.positionOption = await this.dropMan.getPositionDropdown(this.assessmentForm.outlet.outletId);
            this.assessmentForm.position = {};
            this.assessmentForm.employee = {};
            this.isLoading = false;
        }

        async fillEmployeeOption() {
            this.isLoading = true;
            this.employeeOption = await this.dropMan.getEmployeeDropdown(this.assessmentForm.position.positionId, this.assessmentForm.outlet.outletId);
            this.assessmentForm.employee = {};
            this.isLoading = false;
        }

        async fillAllOption() {
            this.dealerOption = await this.dropMan.getDealerDropdown();
            this.outletOption = await this.dropMan.getOutletDropdown(this.assessmentForm.dealer.dealerId);
            this.positionOption = await this.dropMan.getPositionDropdown(this.assessmentForm.outlet.outletId);
            this.employeeOption = await this.dropMan.getEmployeeDropdown(this.assessmentForm.position.positionId, this.assessmentForm.outlet.outletId);
        }

        async insert() {
            this.$validator.resume();
            this.isLoading = true;
            let valid = await this.$validator.validateScopes();

            if (!valid) {
                this.isLoading = false;
                return;
            }

            this.$validator.reset();

            this.assessmentInsertForm.employeeId = this.assessmentForm.employee.employeeId;
            this.assessmentInsertForm.assessmentName = this.assessmentForm.assessmentName;
            this.assessmentInsertForm.publisher = this.assessmentForm.publisher;
            this.assessmentInsertForm.description = this.assessmentForm.description;
            this.assessmentInsertForm.fileContent = this.assessmentForm.fileContent;

            await this.assMan.insertAssessment(this.assessmentInsertForm);

            this.success = this.message.SuccessInsertMessage;
            await this.resetFilter();
            this.closeClicked();
        }

        async update() {
            this.$validator.resume();
            this.isLoading = true;
            let valid = await this.$validator.validateAll(['Assessment Name', 'Publisher']);

            if (!valid) {
                this.isLoading = false;
                return;
            }

            this.$validator.reset();

            this.assessmentUpdateForm.employeeId = this.assessmentForm.employee.employeeId;
            this.assessmentUpdateForm.assessmentName = this.assessmentForm.assessmentName;
            this.assessmentUpdateForm.publisher = this.assessmentForm.publisher;
            this.assessmentUpdateForm.description = this.assessmentForm.description;
            this.assessmentUpdateForm.fileContent = this.assessmentForm.fileContent;

            await this.assMan.updateAssessment(this.assessmentUpdateForm);

            this.success = this.message.SuccessEditMessage;
            await this.resetFilter();
            this.closeClicked();
        }

        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;

        addClicked() {
            this.add = true;
            this.edit = false;
            this.detail = false;
        }

        async editClicked(assessmentId: number) {
            this.isLoading = true;
            let data = await this.assMan.getAssessmentById(assessmentId);
            this.assessmentForm = {
                dealer: data.dealer,
                outlet: data.outlet,
                position: data.position,
                employee: data.employee,
                assessmentName: data.assessmentName,
                publisher: data.publisherName,
                description: data.description,
                fileContent: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                }
            };

            await this.fillAllOption();

            this.imageData = await BlobService.getImageUrl(data.imageBlobId, data.imageBlobMIME);
            this.imageName = data.imageBlobName;

            this.assessmentUpdateForm.assessmentId = assessmentId;
            this.assessmentUpdateForm.blobId = data.imageBlobId;

            this.add = false;
            this.edit = true;
            this.detail = false;
            this.isLoading = false;
        }

        async detailClicked(assessmentId: number) {
            this.isLoading = true;
            let data = await this.assMan.getAssessmentById(assessmentId);
            this.assessmentForm = {
                dealer: data.dealer,
                outlet: data.outlet,
                position: data.position,
                employee: data.employee,
                assessmentName: data.assessmentName,
                publisher: data.publisherName,
                description: data.description,
                fileContent: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                }
            };

            this.imageData = await BlobService.getImageUrl(data.imageBlobId, data.imageBlobMIME);
            this.imageName = data.imageBlobName;

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

        resetForm() {
            this.$validator.pause();

            this.assessmentForm = {
                dealer: {},
                outlet: {},
                position: {},
                employee: {},
                assessmentName: '',
                description: '',
                publisher: '',
                fileContent: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                }
            }

            this.imageData = [];
            this.imageName = '';
        }

        setDelete(assessmentId: number, assessmentName: string) {
            this.toBeDeletedAssessment = {
                id: assessmentId,
                name: assessmentName
            };
        }

        emptyDelete() {
            this.toBeDeletedAssessment = {
                id: 0,
                name: ''
            };
        }

        async deleteAssessment(assessmentId: number) {
            this.isLoading = true;
            await this.assMan.deleteAssessment(assessmentId);
            this.success = this.message.RemoveMessage;
            await this.fetch();
        }

        //Variable untuk sorting
        assessmentName = new Sort();
        employeeName = new Sort();
        position = new Sort();
        publisher = new Sort();
        outlet = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortAsssessment
        async ClickSortAsssessment() {
            this.ResetSort('assessmentName');
            //Sorting
            this.assessmentName.sorting();

            if (this.assessmentName.sortup === true) {
                this.filter.sortBy = 'name';
            }

            else if (this.assessmentName.sortdown === true) {
                this.filter.sortBy = 'nameDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //ClickEmployeeName
        async ClickEmployeeName() {
            this.ResetSort('employeeName');
            //Sorting
            this.employeeName.sorting();

            if (this.employeeName.sortup === true) {
                this.filter.sortBy = 'employee';
            }

            else if (this.employeeName.sortdown === true) {
                this.filter.sortBy = 'employeeDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortPosition
        async ClickSortPosition() {
            this.ResetSort('position');
            //Sorting
            this.position.sorting();

            if (this.position.sortup === true) {
                this.filter.sortBy = 'position';
            }

            else if (this.position.sortdown === true) {
                this.filter.sortBy = 'positionDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortPublisher
        async ClickSortPublisher() {
            this.ResetSort('publisher');

            //Sorting
            this.publisher.sorting();

            if (this.publisher.sortup === true) {
                this.filter.sortBy = 'publisher';
            }

            else if (this.publisher.sortdown === true) {
                this.filter.sortBy = 'publisherDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortOutlet
        async ClickSortOutlet() {
            this.ResetSort('outlet');
            //Sorting
            this.outlet.sorting();

            if (this.outlet.sortup === true) {
                this.filter.sortBy = 'outlet';
            }

            else if (this.outlet.sortdown === true) {
                this.filter.sortBy = 'outletDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();

            if (this.createdDate.sortup === true) {
                this.filter.sortBy = 'CreatedAt';
            }

            else if (this.createdDate.sortdown === true) {
                this.filter.sortBy = 'CreatedAtDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();

            if (this.updatedDate.sortup === true) {
                this.filter.sortBy = 'UpdatedAt';
            }

            else if (this.updatedDate.sortdown === true) {
                this.filter.sortBy = 'UpdatedAtDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'assessmentName':
                    this.employeeName.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.publisher.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'employeeName':
                    this.assessmentName.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.publisher.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'position':
                    this.assessmentName.reset();
                    this.employeeName.reset();
                    this.publisher.reset();
                    this.outlet.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'outlet':
                    this.assessmentName.reset();
                    this.employeeName.reset();
                    this.position.reset();
                    this.publisher.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'publisher':
                    this.assessmentName.reset();
                    this.employeeName.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.assessmentName.reset();
                    this.employeeName.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.publisher.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.assessmentName.reset();
                    this.employeeName.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.publisher.reset();
                    this.createdDate.reset();
                    return;
                default:
                    this.filter.sortBy = '';
                    this.updatedDate.reset();
                    this.assessmentName.reset();
                    this.employeeName.reset();
                    this.position.reset();
                    this.outlet.reset();
                    this.publisher.reset();
                    this.createdDate.reset();
                    return;
            }
        }

        async handler($event) {
            if ($event.target.files.length === 0) {
                return;
            }

            this.isImageChanged = true;
            this.loadFile($event);
            await this.fileChange($event);
        }

        async fileChange(e) {
            let baseFileInput = (<HTMLInputElement>e.target).files[0];
            let base64String = await this.convertToBase64(baseFileInput);
            let extension = baseFileInput.name.split(".").pop();

            this.assessmentForm.fileContent.base64 = base64String;
            this.assessmentForm.fileContent.contentType = extension;
            this.assessmentForm.fileContent.fileName = baseFileInput.name;
        }

        loadFile($event) {
            var reader = new FileReader();
            reader.onload = (e: Event) => {
                this.imageData = (<FileReader>e.target).result;
            }
            reader.readAsDataURL($event.target.files[0]);
            this.imageName = $event.target.files[0].name;
        }

        convertToBase64(file: File): Promise<string> {
            let promise = new Promise<string>((resolve, reject) => {
                let reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => {
                    resolve(reader.result.toString().split(',')[1]);
                }
                reader.onerror = error => {
                    reject(error);
                }
            });

            return promise;
        }
    }
</script>