<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="alert alert-danger" v-if="this.errorMessage">
            {{this.errorMessage}}
        </div>

        <div class="alert alert-success" v-if="this.successMessage">
            {{this.successMessage}}
        </div>

        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > <span class="talent_font_red">Digital Signature</span></h3>
                <br />
                <div>
                    <!--SEARCH-->
                    <div class="d-flex align-items-end flex-column">
                        <div class="col-md-3">
                            <div class="row justify-content-between">
                                <div class="col-md-8 talent_nopadding">
                                    <input class="form-control" v-model="digitalSignatureFilter.EmployeeName" />
                                </div>
                                <div class="col-md-3 talent_nopadding d-flex justify-content-center">
                                    <button class="btn btn-block talent_bg_blue talent_font_white" @click="fetch">Search</button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <!--Table-->
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <h4>Digital Signature List</h4>
                        <div class="col-md-12 talent_magin_small">
                            <div class="row align-items-center row justify-content-between">
                                <a>Showing {{listDS.listDigitalSignature.length}} of {{listDS.totalItem}} Entry(s)</a>
                                <button class="btn talent_bg_cyan talent_roundborder talent_font_white" v-if="crud.create" @click="addClicked">Upload Signature</button>
                            </div>
                        </div>

                        <div class="talent_overflowx">
                            <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                                <thead>
                                    <tr>
                                        <th>
                                            <a href="#" class="talent_sort" @click.prevent="ClickEmployeeName()">Employee Name <fa icon="sort" v-if="employeeName.sort == true"></fa><fa icon="sort-up" v-if="employeeName.sortup == true"></fa><fa icon="sort-down" v-if="employeeName.sortdown == true"></fa></a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" @click.prevent="ClickSortUploadedFile()">Uploaded File <fa icon="sort" v-if="uploadedFile.sort == true"></fa><fa icon="sort-up" v-if="uploadedFile.sortup == true"></fa><fa icon="sort-down" v-if="uploadedFile.sortdown == true"></fa></a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                        </th>
                                        <th v-if="crud.read || crud.delete || crud.update" colspan="3">
                                            Action
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(ds,index) in listDS.listDigitalSignature" v-bind:key="ds.digitalSignatureId">
                                        <td>
                                            {{ds.employeeName}}
                                        </td>
                                        <td>
                                            {{ds.blobName}}
                                        </td>
                                        <td>
                                            {{ds.createdAt|dateFormat}}
                                        </td>
                                        <td>
                                            {{ds.updatedAt|dateFormat}}
                                        </td>
                                        <td v-if="crud.read" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_orange talent_font_white" @click="detailClicked(index)">View Detail</button>
                                        </td>
                                        <td v-if="crud.update" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_cyan talent_font_white" @click="editClicked(index)">Edit</button>
                                        </td>
                                        <td v-if="crud.delete" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click="removeClicked(index)">Remove</button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>

                        </div>

                        <div class="col-md-12 d-flex justify-content-center">
                            <paginate :currentPage.sync="currentPage" :total-data=listDS.totalItem :limit-data=pageSize @update:currentPage="fetch()"></paginate>
                        </div>

                        <div class="col-md-12 d-flex justify-content-center">
                            <paginate v-model="currentPage" total-data=35 limit-data=5></paginate>
                        </div>
                    </div>
                    <br />
                </div>
                <br />


            </div>
        </div>

        <!--Add-->
        <div class="row" v-if="add === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Digital Signature > <span class="talent_font_red">Upload Digital Signature</span></h3>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Digital Signature Information</h4>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Employee Name<span class="talent_font_red">*</span></label>

                                    <multiselect v-model="employeeChoosen"
                                                 id="ajax"
                                                 track-by="employeeName"
                                                 placeholder="Select Employee Name"
                                                 label="employeeName"
                                                 :options="listName"
                                                 :searchable="true"
                                                 :close-on-select="true"
                                                 :clear-on-select="true"
                                                 :loading="isLoading"
                                                 :hide-selected="true"
                                                 :showNoResults="true"
                                                 @search-change="AutoComplete"
                                                 @open="reset">

                                        <span slot="noResult"><i>Not Found!</i></span>
                                    </multiselect>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Upload File<span class="talent_font_red">*</span></label>
                                    <div class="custom-file">
                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)">
                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageName ? imageName : 'Choose File'}}</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <!--Image-->
                    <div class="h-100">
                        <img :src="imageData" alt="Alternate Text" class="img-fluid w-100">
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
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="insertDigitalSignature">
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
                <h3><fa icon="database"></fa> Master Data > Digital Signature > <span class="talent_font_red">Edit Digital Signature</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Digital Signature Information</h4>

                    <br />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Employee Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <multiselect v-model="employeeChoosen"
                                                     id="ajax"
                                                     track-by="employeeName"
                                                     placeholder="Select Employee Name"
                                                     label="employeeName"
                                                     :options="listName"
                                                     :searchable="true"
                                                     :close-on-select="true"
                                                     :clear-on-select="true"
                                                     :loading="isLoading"
                                                     :hide-selected="true"
                                                     :showNoResults="true"
                                                     @search-change="AutoCompleteUpdate"
                                                     :disabled="true"
                                                     @open="reset">

                                            <span slot="noResult"><i>Not Found!</i></span>
                                        </multiselect>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Upload File<span class="talent_font_red">*</span></label>
                                    <div class="custom-file">
                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)">
                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageName ? imageName : 'Choose File'}}</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <!--Image-->
                    <div class="h-100">
                        <img :src="imageData" alt="Alternate Text" class="img-fluid w-100">
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
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="updateDigitalSignature">
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
                <h3><fa icon="database"></fa> Master Data > Digital Signature > <span class="talent_font_red">Detail Digital Signature</span></h3>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Digital Signature Information</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Assessment Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" disabled v-model="listDS.listDigitalSignature[this.indexViewDetail].employeeName">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Upload File<span class="talent_font_red">*</span></label>
                                    <div class="custom-file">
                                        <input type="file" class="custom-file-input" id="customFile" disabled>
                                        <label class="custom-file-label" for="customFile">Choose file</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <!--Image-->
                    <div class="h-100">
                        <img :src="imageViewDetailUrl" alt="Alternate Text" class="img-fluid w-100">
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
    //import { EmployeeService, EmployeeViewModel, EmployeeListViewModel, EmployeeFormModel } from '../../services/employeeService';
    import { clearTimeout, setTimeout } from 'timers';
    //import { DigitalService, DigitalSignatureViewModel, DigitalSignatureFormModel } from '../../services/DigitalSignature';
    import { BlobService } from '../../services/BlobService';
    import { EmployeeService, DigitalSignatureService, EmployeeViewModel, DigitalSignatureFormModel, DigitalSignatureViewModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService'
    import { DateTime } from 'luxon';
    import Message from '../../class/Message';
    import { PageEnums } from '../../enum/PageEnums';

    @Component({
        created: async function (this: DigitalSignature) {
            await this.getAccess();
            await this.initialize();
        }
    })
    export default class DigitalSignature extends Vue {

        //Paging
        currentPage: number = 1;
        pageSize: number = 10;

        //Image Needs
        formData: FormData = new FormData();
        imageData: string | ArrayBuffer = '/upload-image2.png';
        imageName: string = '';
        imageViewDetailUrl: string = '';
        tempName: string = '';


        //Service used
        Service: EmployeeService = new EmployeeService();
        ServiceDS: DigitalSignatureService = new DigitalSignatureService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.DigitalSignature);
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        //Message
        errorMessage: string = "";
        successMessage: string = '';

        listName: EmployeeViewModel[] = [];
        listNameForUpdate: EmployeeViewModel[] = [];

        indexToDelete: number;
        indexViewDetail: number;
        indexToUpdate: number;

        timeout: any = null;

        digitalSignatureFilter: IDigitalSignatureFilter = {
            EmployeeName: '',
            PageNumber: 1,
            SortBy: '',
        }

        blobId: string = "";

        employeeChoosen: EmployeeViewModel = null;

        digitalSignatureForm: DigitalSignatureFormModel = {
            blobId: null, createdAt: null, updatedAt: null, digitalSignatureId: null, employeeId: '', isDeleted: false, fileContent: {base64: '', contentType: '', fileName: ''} };

        listDS: DigitalSignatureViewModel = {
            listDigitalSignature: [],
            totalItem: null
        };

        isLoading: boolean = false;
        isBusy: boolean = false;

        async initialize() {
            await this.fetch();
        }

        async fetch() {
            this.isBusy = true;
            try {
                this.listDS = await this.ServiceDS.getAllDigitalSignature(this.digitalSignatureFilter.EmployeeName, this.digitalSignatureFilter.SortBy, this.currentPage);
            }
            catch{
                this.errorMessage = "Failed to get Data!"
            }
            this.isBusy = false;
        }

        async FetchSorting() {
            this.listDS = await this.ServiceDS.getAllDigitalSignature(this.digitalSignatureFilter.EmployeeName, this.digitalSignatureFilter.SortBy, this.currentPage);
        }

        //Auto Complete for Add
        async AutoComplete(query) {
            if (query == null || query === '') {
                this.listName = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetEmployeeName(query);
                }, 500
            );
        }

        //Auto Complete for Update
        async AutoCompleteUpdate(query) {
            if (query == null || query === '') {
                this.listName = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetEmployeeNameUpdate(query);
                }, 500
            );
        }

        //Get All Employee Name
        async GetEmployeeNameUpdate(query) {
            if (query === '' || query == null) {
                this.listName = [];
                return;
            }

            let a = await this.Service.getEmployeeName(query);
            this.listName = a.employeeList;

            console.log("This is list name", this.listName);

            this.isLoading = false;
        }

        //Get Employee who are not a coach
        async GetEmployeeName(query) {
            if (query === '' || query == null) {
                this.listName = [];
                return;
            }

            let a = await this.Service.getEmployeeNameForDigitalSignature(query);
            this.listName = a.employeeList;

            console.log("This is list name", this.listName);

            this.isLoading = false;
        }

        //Update Click to Call the Update API
        async updateDigitalSignature() {

            if (!this.crud.update) {
                return
            }

            if (this.employeeChoosen == null) {
                this.errorMessage = 'Employee field must be filled'
                return;
            }

            if (this.imageName == '' || this.imageName == null) {
                this.errorMessage = 'Image must be choosen';
                return;
            }

            this.digitalSignatureForm.employeeId = this.employeeChoosen.employeeId;
            this.digitalSignatureForm.digitalSignatureId = this.listDS.listDigitalSignature[this.indexToUpdate].digitalSignatureId;

            this.isBusy = true;

            let valid = await this.ServiceDS.validateUpdate(this.digitalSignatureForm.digitalSignatureId, this.digitalSignatureForm.employeeId);

            if (valid === false) {
                this.errorMessage = "Failed to Update Task! Employee ID is duplicated or Blob ID not found";
            }

            //var blobId;

            //if (this.tempName === this.imageName) {
            //    blobId = this.listDS.listDigitalSignature[this.indexToUpdate].blobId
            //}
            //else {
            //    try {
            //        blobId = await BlobService.uploadService(this.formData);
            //    }
            //    catch{
            //        this.errorMessage = "Failed to Upload Image!";
            //    }
            //}

            //this.digitalSignatureForm.blobId = blobId;

            try {
                let response = await this.ServiceDS.updateDigitalSignature(this.digitalSignatureForm);

                if (response === true) {
                    await this.fetch();
                    this.closeClicked();

                    this.successMessage = Message.SuccessEditMessage;
                }
                else {
                    this.errorMessage = "Failed to Update!";
                }
            }
            catch{
                this.errorMessage = "Failed to Update!";
            }

            this.isBusy = false;

        }

        //Function to get the image
        async getImage() {
            let result = await BlobService.getImageUrl(this.listDS.listDigitalSignature[this.indexViewDetail].blobId, this.listDS.listDigitalSignature[this.indexViewDetail].mime);
            this.imageViewDetailUrl = result;
            console.log(result);
            return result;
        }

        async insertDigitalSignature() {

            if (!this.crud.create) {
                return
            }

            if (this.employeeChoosen == null) {
                this.errorMessage = "Need to choose employee";
                return;
            }

            if (this.imageName == '' || this.imageName == null) {
                this.errorMessage = "Need to choose image";
                return;
            }

            this.isBusy = true;

            //await this.uploadedFileToMiniO();

            this.digitalSignatureForm.employeeId = this.employeeChoosen.employeeId;
            this.digitalSignatureForm.blobId = this.blobId;
            try {
                let id = await this.ServiceDS.insertDigitalSignature(this.digitalSignatureForm);
                this.reset();
                this.closeClicked();
                this.successMessage = Message.SuccessInsertMessage;
                this.formData = new FormData();
                this.imageData = '/upload-image2.png';
                await this.fetch();
            }
            catch{
                this.errorMessage = "Failed to Insert!";
            }

            this.isBusy = false;
        }


        reset() {
            this.employeeChoosen = null;
        }

        //Navigation variables
        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;

        //Variable untuk sorting
        employeeName = new Sort();
        uploadedFile = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //Navigation function

        clearAlert() {
            this.errorMessage = '';
            this.successMessage = '';
            this.imageData = '/upload-image2.png';
            this.imageName = '';
            this.employeeChoosen = null;
            this.imageViewDetailUrl = '';
        }

        addClicked() {

            if (!this.crud.create) {
                return
            }

            this.add = true;
            this.edit = false;
            this.detail = false;
            this.clearAlert();
        }

        async editClicked(index: number) {

            if (!this.crud.update) {
                return
            }

            this.add = false;
            this.edit = true;
            this.detail = false;

            this.indexToUpdate = index;
            this.clearAlert();
            let data = this.listDS.listDigitalSignature[index].blobId;

            this.employeeChoosen = {
                createdAt: null,
                employeeId: this.listDS.listDigitalSignature[index].employeeId,
                employeeName: this.listDS.listDigitalSignature[index].employeeName
            }
            this.listName = [];
            this.listName.push(this.employeeChoosen);

            let result = await BlobService.getImageUrl(data, this.listDS.listDigitalSignature[index].mime);
            this.imageName = this.listDS.listDigitalSignature[index].blobName;
            this.tempName = this.imageName;

            this.imageData = result;

        }

        removeClicked(index: number) {

            if (!this.crud.delete) {
                return
            }

            this.indexToDelete = index;
            this.clearAlert();
        }

        detailClicked(index: number) {

            if (!this.crud.read) {
                return
            }

            this.add = false;
            this.edit = false;
            this.detail = true;
            this.clearAlert();

            this.indexViewDetail = index;
            this.getImage();
        }

        closeClicked() {
            this.add = false;
            this.edit = false;
            this.detail = false;
            this.listName = [];
            this.clearAlert();
        }

        async deleteConfirmed() {
            if (!this.crud.delete) {
                return
            }

            await this.ServiceDS.deleteDigitalSignature(this.listDS.listDigitalSignature[this.indexToDelete].digitalSignatureId);
            await this.fetch();
            this.closeClicked();

            this.successMessage = Message.RemoveMessage;
        }

        //ClickEmployeeName
        async ClickEmployeeName() {
            this.ResetSort('employeeName');
            //Sorting
            this.employeeName.sorting();
            //Function Sorting
            if (this.employeeName.sortup == true) {
                this.digitalSignatureFilter.SortBy = 'employeeName';
            }
            else if (this.employeeName.sortdown == true) {
                this.digitalSignatureFilter.SortBy = 'employeeNameDesc';
            }
            else {
                this.digitalSignatureFilter.SortBy = '';
            }
            this.FetchSorting();
            return;
        }

        //ClickSortUploadedFile
        async ClickSortUploadedFile() {
            this.ResetSort('uploadedFile');
            //Sorting
            this.uploadedFile.sorting();
            //Function Sorting
            if (this.uploadedFile.sortup == true) {
                this.digitalSignatureFilter.SortBy = 'uploadFile';
            }
            else if (this.uploadedFile.sortdown == true) {
                this.digitalSignatureFilter.SortBy = 'uploadFileDesc';
            }
            else {
                this.digitalSignatureFilter.SortBy = '';
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
                this.digitalSignatureFilter.SortBy = 'createdDate';
            }
            else if (this.createdDate.sortdown == true) {
                this.digitalSignatureFilter.SortBy = 'createdDateDesc';
            }
            else {
                this.digitalSignatureFilter.SortBy = '';
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
                this.digitalSignatureFilter.SortBy = 'updateDate';
            }
            else if (this.updatedDate.sortdown == true) {
                this.digitalSignatureFilter.SortBy = 'updateDateDesc';
            }
            else {
                this.digitalSignatureFilter.SortBy = '';
            }
            this.FetchSorting();
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'employeeName':
                    this.uploadedFile.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'uploadedFile':
                    this.employeeName.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.employeeName.reset();
                    this.uploadedFile.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.employeeName.reset();
                    this.uploadedFile.reset();
                    this.createdDate.reset();
                    return;
            }
        }

        //Function for Upload the Image to MiniO
        async uploadedFileToMiniO() {
            try {
                let response = await BlobService.uploadService(this.formData);
                this.blobId = response;
            }
            catch{
                this.errorMessage = "Failed to Upload Image!";
            }
        }

        //Function for preview the Image
        async previewImage(event) {
            var input = event.target;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.readAsDataURL(event.target.files[0]);
                let name = event.target.files[0].name;
                reader.onload = (e: Event) => {
                    let image = (<FileReader>e.target).result;
                    this.imageData = image;
                }
                this.imageName = name;
            }

            let baseFileInput = (<HTMLInputElement>event.target).files[0];
            let base64String = await this.convertToBase64(baseFileInput);
            let extension = baseFileInput.name.split(".").pop();

            this.digitalSignatureForm.fileContent.base64 = base64String;
            this.digitalSignatureForm.fileContent.contentType = extension;
            this.digitalSignatureForm.fileContent.fileName = baseFileInput.name;

            //this.formData = new FormData();

            //Array.from(Array(event.target.files.length).keys())
            //    .map(x => {
            //        this.formData.append(event.target.files, event.target.files[x], event.target.files[x].name);
            //    });
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
