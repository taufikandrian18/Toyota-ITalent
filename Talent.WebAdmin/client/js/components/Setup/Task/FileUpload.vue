<template>
    <div>
        <!--Add-->
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="row" v-if="mode === 'Add'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > File Upload > <span class="talent_font_red">Add</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox" @change="checkBoxCheck" style="line-height: 50%;" v-model="isStoryLineType" color="#0085CA" :size="16"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--STORYLINE-->
                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input type="radio" name="typeofstoryline" id="vertical" :disabled="isStoryLineType != true" v-model="formModel.layoutType" value="1" /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input type="radio" name="typeofstoryline" id="horizontal" :disabled="isStoryLineType != true" v-model="formModel.layoutType" value="2" /> <label for="horizontal"><b>Type 2</b></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <img :src="imageDataVertical" alt="Alternate Text" class="img-fluid w-100 h-100" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="custom-file">
                                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)" :disabled="formModel.layoutType === '2' || !formModel.layoutType">
                                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameVertical ? imageNameVertical : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea class="form-control talent_textarea" maxlength="5000" :disabled="formModel.layoutType === '2' || !formModel.layoutType" name="verticalTextField" v-model="verticalTextField"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row h-100">
                                        <div class="col-md-7">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <img :src="imageDataHorizontal" alt="Alternate Text" class="img-fluid w-100 h-100" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="custom-file">
                                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)" :disabled="formModel.layoutType === '1' || !formModel.layoutType">
                                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameHorizontal ? imageNameHorizontal : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <br />

                                        <div class="col-md-5 ">
                                            <textarea class="form-control h-100 talent_overflowy" name="horizontalTextField" maxlength="5000" :disabled="formModel.layoutType === '1' || !formModel.layoutType" v-model="horizontalTextField"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="alert alert-success" v-if="this.successMessage">
                    {{this.successMessage}}
                </div>
                <div class="alert alert-danger" v-if="this.errorMessage">
                    {{this.errorMessage}}
                </div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <select class="form-control" name="CompetencyMappingInsert" v-model="selectedCompetency" @change="getNumber()">
                                        <option v-for="(competencyMapping,index) in competenciesMappingList" :value="competencyMapping">{{competencyMapping.competencyTypeName.charAt(0)}}-{{competencyMapping.prefixCode}}-{{competencyMapping.evaluationTypeName}}</option>
                                    </select>
                                </div>
                                <div class="col-md-6">
                                    <h5>Number</h5>
                                    <input class="form-control" v-model="taskNumber" disabled />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>Module<span class="talent_font_red">*</span></h5>
                                    <select class="form-control" name="module" v-model="selectedModule" @change="getNumber()">
                                        <option v-for="(module,index) in allModulesForTask" :value="module">{{module.moduleName}}</option>
                                    </select>
                                </div>
                            </div>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <textarea class="form-control talent_textarea" v-model="formModel.question"></textarea>
                        </div>
                    </div>
                </div>

                <br />
                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="insertShortTask" v-if="crud.create">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--View-->
        <div class="row" v-if="mode === 'View'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > File Upload > <span class="talent_font_red">View Detail</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox" disabled style="line-height: 50%;" v-model="isStoryLineType" color="#0085CA" :size="16"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--STORYLINE-->
                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input type="radio" name="typeofstoryline" id="vertical" disabled v-model="viewDetailModel.layoutType" value="1" /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input type="radio" name="typeofstoryline" id="horizontal" disabled v-model="viewDetailModel.layoutType" value="2" /> <label for="horizontal"><b>Type 2</b></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <img :src="imageDataVertical" alt="Alternate Text" class="img-fluid w-100 h-100" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="custom-file">
                                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)" disabled>
                                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameVertical ? imageNameVertical : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea class="form-control talent_textarea" disabled name="verticalTextField" v-model="verticalTextField"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row h-100">
                                        <div class="col-md-7">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <img :src="imageDataHorizontal" alt="Alternate Text" class="img-fluid w-100 h-100" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="custom-file">
                                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)" disabled>
                                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameHorizontal ? imageNameHorizontal : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <br />

                                        <div class="col-md-5 ">
                                            <textarea class="form-control h-100 talent_overflowy" name="horizontalTextField" disabled v-model="horizontalTextField"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <input class="form-control" v-model="taskCodeViewDetail" disabled />
                                </div>
                                <div class="col-md-6">
                                    <h5>Number</h5>
                                    <input class="form-control" :placeholder="viewDetailModel.taskNumber" disabled />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>Module<span class="talent_font_red">*</span></h5>
                                    <input class="form-control" :placeholder="viewDetailModel.moduleName" disabled />
                                </div>
                            </div>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <textarea class="form-control talent_textarea" placeholder="Question" disabled v-model="viewDetailModel.question" name="question"></textarea>
                        </div>
                    </div>
                </div>

                <br />
                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white" @click.prevent="backPage">
                                        Close
                                    </button>
                                    <button v-else class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Update-->
        <div class="row" v-if="mode === 'Edit'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > File Upload > <span class="talent_font_red">Edit</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox" @change="checkBoxCheck" style="line-height: 50%;" v-model="isStoryLineType" color="#0085CA" :size="16"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--STORYLINE-->
                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input type="radio" name="typeofstoryline" id="vertical" :disabled="isStoryLineType != true" v-model="formUpdateModel.layoutType" value="1" @change="forceUpdate" /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input type="radio" name="typeofstoryline" id="horizontal" :disabled="isStoryLineType != true" v-model="formUpdateModel.layoutType" value="2" @change="forceUpdate" /> <label for="horizontal"><b>Type 2</b></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <img :src="imageDataVertical" alt="Alternate Text" class="img-fluid w-100 h-100" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="custom-file">
                                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)" :disabled="formUpdateModel.layoutType === 2 || formUpdateModel.layoutType === '2' || isStoryLineType != true || !formUpdateModel.layoutType">
                                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameVertical ? imageNameVertical : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea class="form-control talent_textarea" maxlength="5000" :disabled="formUpdateModel.layoutType === 2 || formUpdateModel.layoutType === '2' || isStoryLineType != true || !formUpdateModel.layoutType" name="verticalTextField" v-model="verticalTextField"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row h-100">
                                        <div class="col-md-7">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <img :src="imageDataHorizontal" alt="Alternate Text" class="img-fluid w-100 h-100" />
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="custom-file">
                                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)" :disabled="formUpdateModel.layoutType === 1 || formUpdateModel.layoutType == '1' || isStoryLineType != true || !formUpdateModel.layoutType">
                                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameHorizontal ? imageNameHorizontal : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <br />

                                        <div class="col-md-5 ">
                                            <textarea class="form-control h-100 talent_overflowy" name="horizontalTextField" maxlength="5000" :disabled="formUpdateModel.layoutType == '1' || isStoryLineType != true || !formUpdateModel.layoutType" v-model="horizontalTextField"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="alert alert-success" v-if="this.successMessage">
                    {{this.successMessage}}
                </div>
                <div class="alert alert-danger" v-if="this.errorMessage">
                    {{this.errorMessage}}
                </div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <select class="form-control" name="CompetencyMappingInsert" v-model="selectedCompetency" @change="getNumber()">
                                        <option v-for="(competencyMapping,index) in competenciesMappingList" :value="competencyMapping">{{competencyMapping.competencyTypeName.charAt(0)}}-{{competencyMapping.prefixCode}}-{{competencyMapping.evaluationTypeName}}</option>
                                    </select>
                                </div>
                                <div class="col-md-6">
                                    <h5>Number</h5>
                                    <input class="form-control" v-model="taskNumber" disabled />
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>Module<span class="talent_font_red">*</span></h5>
                                    <select class="form-control" name="module" v-model="selectedModule" @change="getNumber()">
                                        <option v-for="(module,index) in allModulesForTask" :value="module">{{module.moduleName}}</option>
                                    </select>
                                </div>
                            </div>
                            <br />
                        </div>
                        <div class="col-md-6">
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <textarea maxlength="2000" class="form-control talent_textarea" v-model="formUpdateModel.question"></textarea>
                        </div>
                    </div>
                </div>

                <br />
                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="update" v-if="crud.update">
                                        Save
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
    import { BlobService } from '../../../services/BlobService';
    import { TaskService, CompetencyMappingJoinModel, ModuleForTaskModel, TimePointTaskModel, FileUploadService, TaskFileUploadFormModel, TaskFileUploadViewDetail, UserPrivilegeSettingsService, UserAccessCRUD } from '../../../services/NSwagService'
    import { faAd } from '@fortawesome/free-solid-svg-icons';
    import IFileContent from '../../../models/IFileContent';

    @Component({
        props: ['mode', 'taskid', 'fromOutside'],
        created: async function (this: FileUpload) {
            await this.getAccess();
            if (this.mode == "Add") {
                if (!this.crud.create) {
                    return
                }
                this.initilizeAdd();
            }
            else if (this.mode == "View") {
                if (!this.crud.read) {
                    return
                }
                this.initilizeView();
            }
            else if (this.mode == "Edit") {
                if (!this.crud.update) {
                    return
                }
                this.initilizeUpdate();
            }
        }
    })
    export default class FileUpload extends Vue {
        storyLine: boolean = false;

        mode: string;
        taskid: number;
        fromOutside: boolean;

        ServiceTask: TaskService = new TaskService();
        ServiceFileUpload: FileUploadService = new FileUploadService();

        formModel: TaskFileUploadFormModel = { blobId: '', competencyId: null, createdAt: null, createdBy: null, evaluationTypeId: null, isDeleted: null, layoutType: null, moduleId: null, question: '', questionTypeId: null, storyLineDescription: '', taskId: null, taskNumber: null, updatedAt: null }
        formUpdateModel: TaskFileUploadFormModel = { blobId: '', competencyId: null, createdAt: null, createdBy: null, evaluationTypeId: null, isDeleted: null, layoutType: null, moduleId: null, question: '', questionTypeId: null, storyLineDescription: '', taskId: null, taskNumber: null, updatedAt: null }

        viewDetailModel: TaskFileUploadViewDetail = { blobId: '', competencyId: null, createdAt: null, createdBy: null, evaluationTypeId: null, isDeleted: null, layoutType: null, moduleId: null, question: '', questionTypeId: null, storyLineDescription: '', taskId: null, taskNumber: null, updatedAt: null, competencyTypeId: null, competencyTypeName: '', evaluationTypeName: '', mime: '', moduleName: '', name: '', prefixCode: '' }

        competenciesMappingList: CompetencyMappingJoinModel[] = [];
        allModulesForTask: ModuleForTaskModel[] = [];
        allPoints: TimePointTaskModel[] = [];

        selectedCompetency: CompetencyMappingJoinModel = {
            competencyId: null, competencyTypeId: null, prefixCode: null,
            competencyTypeName: null, evaluationTypeId: null, evaluationTypeName: null, competencyNameMapping: null
        }
        selectedModule: ModuleForTaskModel = { moduleId: null, moduleName: null }

        successMessage: string = '';
        errorMessage: string = '';

        tempStringCheckingUpdateImage: string = '';

        formDataVertical: IFileContent = {
            base64: "",
            contentType: "",
            fileName: ""
        };
        formDataHorizontal: IFileContent = {
            base64: "",
            contentType: "",
            fileName: ""
        };

        verticalTextField: string = '';
        horizontalTextField: string = '';
        imageDataVertical: string | ArrayBuffer = '/upload-image2.png';
        imageDataHorizontal: string | ArrayBuffer = '/upload-image2.png';
        defaultImage: string = '/upload-image2.png';
        imageNameVertical: string = '';
        imageNameHorizontal: string = '';

        taskCodeViewDetail: string = '';
        isBusy: boolean = false;

        verticalBlobId: string = null;
        horizontalBlobId: string = null;

        isStoryLineType: boolean = null;

        taskNumber: number = 0;

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("CRS");
            this.crud = data
        }

        async initilizeAdd() {
            this.competenciesMappingList = await this.ServiceTask.getAllCompetencies();
            this.allModulesForTask = await this.ServiceTask.getAllModulesForTask();
            this.allPoints = await this.ServiceTask.getAllTimePointsForTask();
            this.getNumber();
        }

        async initilizeView() {
            this.viewDetailModel = await this.ServiceFileUpload.getFileUploadDetails(this.taskid);
            this.taskCodeViewDetail = this.viewDetailModel.competencyTypeName.charAt(0) + '-' + this.viewDetailModel.prefixCode + '-' + this.viewDetailModel.evaluationTypeName;

            if (this.viewDetailModel.layoutType != 0) {
                this.isStoryLineType = true;
                if (this.viewDetailModel.layoutType == 1) {
                    this.verticalTextField = this.viewDetailModel.storyLineDescription;
                    if (this.viewDetailModel.blobId != null) {
                        this.imageNameVertical = this.viewDetailModel.name;
                        let stringSrc = await BlobService.getImageUrl(this.viewDetailModel.blobId, this.viewDetailModel.mime);
                        this.imageDataVertical = stringSrc;
                    }
                }
                else {
                    this.horizontalTextField = this.viewDetailModel.storyLineDescription;
                    if (this.viewDetailModel.blobId != null) {
                        this.imageNameHorizontal = this.viewDetailModel.name;
                        let stringSrc = await BlobService.getImageUrl(this.viewDetailModel.blobId, this.viewDetailModel.mime);
                        this.imageDataHorizontal = stringSrc;
                    }
                }
            }
        }

        async initilizeUpdate() {
            this.viewDetailModel = await this.ServiceFileUpload.getFileUploadDetails(this.taskid);
            this.formUpdateModel.question = this.viewDetailModel.question;
            this.formUpdateModel.taskId = this.taskid;

            this.taskNumber = this.viewDetailModel.taskNumber;

            if (this.viewDetailModel.layoutType != 0) {
                let stringSrc;

                if (this.viewDetailModel.blobId != null) {
                    stringSrc = await BlobService.getImageUrl(this.viewDetailModel.blobId, this.viewDetailModel.mime);
                }

                this.isStoryLineType = true;
                if (this.viewDetailModel.layoutType == 1) {
                    this.formUpdateModel.layoutType = 1;
                    this.imageNameVertical = this.viewDetailModel.name || '';
                    this.verticalTextField = this.viewDetailModel.storyLineDescription;
                    this.imageDataVertical = stringSrc || this.defaultImage;
                    this.verticalBlobId = this.viewDetailModel.blobId;
                }
                else {
                    this.formUpdateModel.layoutType = 2;
                    this.imageNameHorizontal = this.viewDetailModel.name || '';
                    this.imageDataHorizontal = stringSrc || this.defaultImage;
                    this.horizontalTextField = this.viewDetailModel.storyLineDescription;
                    this.horizontalBlobId = this.viewDetailModel.blobId;
                }
            }

            this.allModulesForTask = await this.ServiceTask.getAllModulesForTask();
            this.allPoints = await this.ServiceTask.getAllTimePointsForTask();
            this.getNumber();

            this.selectedModule.moduleId = this.viewDetailModel.moduleId;
            this.selectedModule.moduleName = this.viewDetailModel.moduleName;

            this.selectedCompetency.competencyId = this.viewDetailModel.competencyId;
            this.selectedCompetency.competencyTypeId = this.viewDetailModel.competencyTypeId;
            this.selectedCompetency.competencyTypeName = this.viewDetailModel.competencyTypeName;
            this.selectedCompetency.evaluationTypeId = this.viewDetailModel.evaluationTypeId;
            this.selectedCompetency.evaluationTypeName = this.viewDetailModel.evaluationTypeName;
            this.selectedCompetency.prefixCode = this.viewDetailModel.prefixCode;

            this.competenciesMappingList = await this.ServiceTask.getAllCompetencies();

            this.allModulesForTask = await this.ServiceTask.getAllModulesForTask();
        }

        async insertShortTask() {
            this.errorMessage = '';
            this.successMessage = '';

            if (!this.crud.create) {
                return
            }

            if (Object.keys(this.selectedCompetency).length === 0) {
                this.errorMessage = "Task Code Required";
                return;
            }
            else if (!this.selectedModule.moduleId) {
                this.errorMessage = "Module is Required";
                return;
            }
            else if (this.formModel.question == null || this.formModel.question.length <= 0) {
                this.errorMessage = "Question must be filled";
                return;
            }
            else if (this.isStoryLineType == true) {
                if (this.formModel.layoutType == 1) {
                    if (this.verticalTextField.length <= 0 && this.imageNameVertical.length <= 0) {
                        this.errorMessage = "Image or Storyline are Required";
                        return;
                    }
                }
                else {
                    if (this.horizontalTextField.length <= 0 && this.imageNameHorizontal.length <= 0) {
                        this.errorMessage = "Image or Storyline are Required";
                        return;
                    }
                }
            }

            this.formModel.competencyId = this.selectedCompetency.competencyId;
            this.formModel.moduleId = this.selectedModule.moduleId;
            this.formModel.evaluationTypeId = this.selectedCompetency.evaluationTypeId;
            this.formModel.createdBy = 'Admin';


            if (this.formModel.layoutType) {
                if (this.formModel.layoutType == 1 && this.imageNameVertical.length > 0) {
                    this.formModel.fileContent = this.formDataVertical;
                }
                else if (this.formModel.layoutType == 2 && this.imageNameHorizontal.length > 0) {
                    this.formModel.fileContent = this.formDataHorizontal;
                }

                if (this.formModel.layoutType == 1) {
                    this.formModel.storyLineDescription = this.verticalTextField;
                }
                else if (this.formModel.layoutType == 2) {
                    this.formModel.storyLineDescription = this.horizontalTextField;
                }
                if (this.formModel.storyLineDescription.length > 5000) {
                    this.errorMessage = "Storyline description must be less than 5000 characters";
                    return;
                }
            }
            if (this.formModel.question.length > 2000) {
                this.errorMessage = "Question must be less than 2000 characters";
                return;
            }
            this.isBusy = true;
            try {
                let isSuccess = await this.ServiceFileUpload.insertFileUpload(this.formModel);
                this.successMessage = "Success to Insert Task!";
                this.isBusy = false;
                this.closeClicked();
            }
            catch{
                this.errorMessage = "Failed to Insert Task!";
                this.isBusy = false;
            }
        }

        async update() {
            this.errorMessage = '';
            this.successMessage = '';

            if (!this.crud.update) {
                return
            }

            if (this.formUpdateModel.question.length > 2000) {
                this.errorMessage = "Question must be less than 2000 characters";
                return;
            }
            if (this.selectedCompetency == null) {
                this.errorMessage = "Task Code can't be Empty";
                return;
            }

            if (this.selectedModule == null) {
                this.errorMessage = "Module can't be Empty";
                return;
            }

            if (this.formUpdateModel.question.length <= 0 || this.formUpdateModel.question == null) {
                this.errorMessage = "Question can't be Empty";
                return;
            }

            if (this.isStoryLineType == false || !this.isStoryLineType) {
                this.formUpdateModel.layoutType = 0;
                this.formUpdateModel.blobId = null;
            }
            else {
                if (this.formUpdateModel.layoutType == 1) {
                    let verticalStorylineDesc = this.verticalTextField || '';
                    let verticalImageName = this.imageNameVertical || '';
                    if (verticalStorylineDesc.length <= 0 && verticalImageName.length <= 0) {
                        this.errorMessage = "Story Line or Image Description can't be Empty";
                        return;
                    }
                    else {
                        if (verticalImageName.length > 0 && this.imageDataVertical != this.defaultImage) {
                            let imageDataVertical = this.imageDataVertical || '';
                            let isImgSrcFromLocal = imageDataVertical.toString().includes("data:", 0)
                            if (isImgSrcFromLocal && imageDataVertical != this.defaultImage) {
                                this.formUpdateModel.fileContent = this.formDataVertical;
                            }
                            else {
                                this.formUpdateModel.blobId = this.verticalBlobId;
                            }
                        }
                        this.formUpdateModel.storyLineDescription = verticalStorylineDesc;
                    }
                }
                else {
                    let horizontalStorylineDesc = this.horizontalTextField || '';
                    let horizontalImageName = this.imageNameHorizontal || '';
                    if (horizontalStorylineDesc.length <= 0 && horizontalImageName.length <= 0) {
                        this.errorMessage = "Story Line or Image Description can't be Empty";
                        return;
                    }
                    else {
                        if (horizontalImageName.length > 0 && this.imageDataHorizontal != this.defaultImage) {
                            let imageDataHorizontal = this.imageDataHorizontal || '';
                            let isImgSrcFromLocal = imageDataHorizontal.toString().includes("data:", 0)
                            if (isImgSrcFromLocal && imageDataHorizontal != this.defaultImage) {
                                this.formUpdateModel.fileContent = this.formDataHorizontal;
                            }
                            else {
                                this.formUpdateModel.blobId = this.horizontalBlobId;
                            }
                        }
                        this.formUpdateModel.storyLineDescription = horizontalStorylineDesc;
                    }
                }
                if (this.formUpdateModel != null && this.formUpdateModel.storyLineDescription.length > 5000) {
                    this.errorMessage = "Storyline description must be less than 5000 characters";
                    return;
                }
            }

            this.formUpdateModel.createdBy = "Admin";
            this.formUpdateModel.moduleId = this.selectedModule.moduleId;
            this.formUpdateModel.competencyId = this.selectedCompetency.competencyId;
            this.formUpdateModel.evaluationTypeId = this.selectedCompetency.evaluationTypeId;

            this.isBusy = true;
            try {
                let result = await this.ServiceFileUpload.updateFileUpload(this.formUpdateModel);
                this.successMessage = "Success to Update Task!";
                this.isBusy = false;
                this.closeClicked();
            }
            catch{
                this.errorMessage = "Failed to Update Task!";
                this.isBusy = false;
                this.closeClicked();
            }


        }

        async getNumber() {
            if (this.mode === 'Edit') {
                if (this.selectedCompetency.competencyId) {

                    if (this.selectedModule.moduleId == this.viewDetailModel.moduleId && this.selectedCompetency.competencyId == this.viewDetailModel.competencyId && this.selectedCompetency.evaluationTypeId == this.viewDetailModel.evaluationTypeId) {
                        this.taskNumber = this.viewDetailModel.taskNumber;
                    }
                    else {
                        this.taskNumber = await this.ServiceTask.getNumber(this.selectedCompetency.competencyId, this.selectedModule.moduleId, this.selectedCompetency.evaluationTypeId);
                    }
                }
                else {
                    this.taskNumber = this.viewDetailModel.taskNumber;
                }

            }
            else {
                this.taskNumber = await this.ServiceTask.getNumber(this.selectedCompetency.competencyId, this.selectedModule.moduleId, this.selectedCompetency.evaluationTypeId);
            }

        }

        closeClicked() {
            window.location.href = '/Setup/Tasks';
        }

        forceUpdate() {
            this.$forceUpdate();
        }

        checkBoxCheck() {
            if (this.isStoryLineType == false) {
                if (this.mode == "Add") {
                    this.formModel.layoutType = 0;
                }
                else if (this.mode == "Edit") {
                    this.formUpdateModel.layoutType = 0;
                }
            }
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

        async fileChange(e) {
            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");

            if (this.formModel.layoutType == 1 || this.formUpdateModel.layoutType == 1) {
                this.formDataVertical.base64 = base64String;
                this.formDataVertical.fileName = e.target.files[0].name;
                this.formDataVertical.contentType = array.pop();
            }
            else {
                this.formDataHorizontal.base64 = base64String;
                this.formDataHorizontal.fileName = e.target.files[0].name;
                this.formDataHorizontal.contentType = array.pop();
            }
        }

        previewImage(event) {
            var input = event.target;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.readAsDataURL(event.target.files[0]);
                let name = event.target.files[0].name;
                reader.onload = (e: Event) => {
                    let image = (<FileReader>e.target).result;
                    if (this.mode == 'Add') {
                        if (this.formModel.layoutType == 1) {
                            this.imageDataVertical = image;
                            this.imageNameVertical = name;
                        }
                        else {
                            this.imageDataHorizontal = image;
                            this.imageNameHorizontal = name;
                        }
                    }
                    else if (this.mode == 'Edit') {
                        if (this.formUpdateModel.layoutType == 1) {
                            this.imageDataVertical = image;
                            this.imageNameVertical = name;
                        }
                        else {
                            this.imageDataHorizontal = image;
                            this.imageNameHorizontal = name;
                        }
                    }
                }
            }

            this.fileChange(event);
        }

        backPage() {
            window.history.back();
        }

    }
</script>
