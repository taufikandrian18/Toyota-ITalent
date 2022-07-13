<template>
    <div>
        <div class="row">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Essay > <span class="talent_font_red" v-if="mode == 'Add'">Add</span><span class="talent_font_red" v-if="mode == 'Edit'">Edit</span><span class="talent_font_red" v-if="mode == 'View'">View Detail</span></h3>
                <br />

                <!--Story line option-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox talent_nomargin" style="line-height: 50%;" v-model="storyLine" color="#0085CA" :size="16" :disabled="mode == 'View'"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>
                    <!--STORYLINE-->
                    <div>
                        <br />
                        <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-5">
                                        <div class="row d-flex justify-content-center">
                                            <div class="justify-content-between align-items-center">
                                                <input type="radio" v-model="stringLayoutType" value="1" name="layoutType" id="vertical" :disabled="mode == 'View' || storyLine == false" /> <label for="vertical"><b>Type 1</b></label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-5">
                                        <div class="row d-flex justify-content-center">
                                            <div class="justify-content-between align-items-center">
                                                <input type="radio" v-model="stringLayoutType" value="2" name="layoutType" id="horizontal" :disabled="mode == 'View' || storyLine == false" /> <label for="horizontal"><b>Type 2</b></label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="talent_bg_grey talent_padding h-100">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div>
                                                    <img class="h-100 w-100" :src="imageDataLeft != null ? imageDataLeft : '/upload-image2.png'" for="customFile" />
                                                    <div class="custom-file">
                                                        <input type="file" class="custom-file-input" id="customFile" accept="image/*" @change="loadFileLeft" :disabled="stringLayoutType != '1' || mode == 'View' || storyLine == false" />
                                                        <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameLeft.length && stringLayoutType == '1' ? imageNameLeft : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="row">
                                            <div class="col-md-12">
                                                <textarea class="form-control talent_textarea" v-model="storyLineDescription1" :disabled="stringLayoutType != '1' || mode == 'View' || storyLine == false" maxlength="5000"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="talent_bg_grey talent_padding h-100">
                                        <div class="row h-100">
                                            <div class="col-md-7">
                                                <div>
                                                    <img class="h-100 w-100" :src="imageDataRight != null ? imageDataRight : '/upload-image1.png'" for="customFile" />
                                                    <div class="custom-file">
                                                        <input type="file" class="custom-file-input" id="customFile" accept="image/*" @change="loadFileRight" :disabled="stringLayoutType != '2' || mode == 'View' || storyLine == false" />
                                                        <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameRight.length && stringLayoutType == '2' ? imageNameRight : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-5 ">
                                                <textarea class="form-control h-100 talent_overflowy" v-model="storyLineDescription2" :disabled="stringLayoutType != '2' || mode == 'View' || storyLine == false" maxlength="5000"></textarea>
                                            </div>
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
                    <div v-if="validateAdd() === false" class="alert alert-danger">
                        <div v-if="taskCode.competencyId === 0">Task Code is Required</div>
                        <div v-if="moduleId === 0">Module is Required</div>
                        <div v-if="addModel.question === ''">Question is Required</div>
                        <div v-if="addModel.question != null"><div v-if="addModel.question.length > 2000">Question must be less than 2000 characters</div></div>
                        <div v-if="this.storyLine && ((this.stringLayoutType == '1' && this.imageNameLeft == '' && (this.storyLineDescription1 == null || this.storyLineDescription1 == '')) || (this.stringLayoutType == '2' && this.imageNameRight == '' && (this.storyLineDescription2 == null || this.storyLineDescription2 == '')))">Image or Storyline Description is Required if Using Story Line</div><div v-if="storyLineDescription1 != null"><div v-if="this.stringLayoutType == '1' && storyLineDescription1.length > 5000">Storyline description must be less than 5000 characters</div></div>
                        <div v-if="this.storyLine && this.stringLayoutType == '0'">Layout Type Required</div>
                        <div v-if="storyLineDescription2 != null"><div v-if="this.stringLayoutType == '2' && storyLineDescription2.length > 5000">Storyline description must be less than 5000 characters</div></div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <select class="form-control" v-model="taskCode" :disabled="mode == 'View'">
                                        <option v-for="t in taskCodes" :value="{'competencyId': t.competencyId, 'evaluationTypeId': t.evaluationTypeId}">{{t.competencyTypeName.substring(0, 1) + '-' + t.prefixCode + '-' + t.evaluationTypeName}}</option>
                                    </select>
                                </div>
                                <div class="col-md-6">
                                    <h5>Number</h5>
                                    <input class="form-control" :value="addModel.taskNumber" disabled />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>Module<span class="talent_font_red">*</span></h5>
                                    <div class="input-group">
                                        <multiselect
                                            v-model="moduleId"
                                            track-by="moduleId"
                                            :options="modules"
                                            label="moduleName"
                                            v-validate="'required'"
                                            :searchable="true"
                                        ></multiselect>
                                    </div>
                                </div>
                                <!-- <div class="col-md-6">
                                    <div class="input-group">
                                        <multiselect
                                            v-model="moduleId"
                                            track-by="moduleId"
                                            :options="modules"
                                            label="moduleName"
                                            v-validate="'required'"
                                            :searchable="true"
                                        ></multiselect>
                                    </div>
                                </div> -->
                            </div>
                        </div>
                        <div class="col-md-6">
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <textarea class="form-control talent_textarea" v-model="addModel.question" :disabled="mode == 'View'" maxlength="2000"></textarea>
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
                                    <button v-else class="btn talent_bg_red talent_font_white" @click.prevent="close">
                                        Close
                                    </button>
                                    <button v-if="mode != 'View'" class="btn talent_bg_lightgreen talent_font_white" @click.prevent="addData">
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
    import { isNullOrUndefined } from 'util';
    import { EssayService, TaskService, TaskEssayTypeFormModel } from '../../../services/NSwagService'
    import IFileContent from '../../../models/IFileContent';

    @Component({
        props: ['mode', 'taskId', 'fromOutside'],
        created: async function (this: EssayNew) {
            this.isBusy = true;
            this.initData();
        },
        watch: {
            taskCode(val, oldVal) {
                this.getTaskNumber();
            },
            moduleId(val, oldVal) {
                this.getTaskNumber();
            }
        },
        methods: {
            fetch(this: EssayNew) {
            }
        }
    })
    export default class EssayNew extends Vue {
        isBusy: boolean = false;
        mode: string;
        taskId: number;
        fromOutside: boolean;
        tempStringCheckingUpdateImage: string = '';

        taskCodes = {};
        modules = {};

        taskEssayMan: EssayService = new EssayService();
        taskMan: TaskService = new TaskService();
        moduleTask = []
        async initData() {
            this.taskCodes = await this.taskMan.getAllCompetencies();
            this.modules = await this.taskMan.getAllModulesForTask();
            console.log(this.modules)
            if (this.mode == 'Edit' || this.mode == 'View') {
                var model = await this.taskEssayMan.getTaskEssayTypeById(this.taskId);
                this.blobId = model.blobId;
                this.taskCode.competencyId = model.competencyId;
                this.taskCode.evaluationTypeId = model.evaluationTypeId;
                this.moduleId = model.moduleId;
                this.addModel.taskNumber = model.taskNumber;
                this.addModel.question = model.question;
                this.storyLine = (model.layoutType === 0) ? false : true;
                this.addModel.storyLineDescription = model.storyLineDescription;
                this.stringLayoutType = model.layoutType.toString();
                if (this.stringLayoutType == '1') {
                    this.storyLine = true;
                    this.imageDataLeft = model.blobId == null ? null : await BlobService.getImageUrl(model.blobId, model.mime);
                    this.imageNameLeft = model.blobId == null ? '' : model.fileName;
                    this.tempStringCheckingUpdateImage = model.fileName;
                    this.storyLineDescription1 = model.storyLineDescription;
                }
                else if (this.stringLayoutType == '2') {
                    this.storyLine = true;
                    this.imageDataRight = model.blobId == null ? null : await BlobService.getImageUrl(model.blobId, model.mime);
                    this.imageNameRight = model.blobId == null ? '' : model.fileName;
                    this.tempStringCheckingUpdateImage = model.fileName;
                    this.storyLineDescription2 = model.storyLineDescription;
                }
            }

            this.isBusy = false;
        }

        async getTaskNumber() {
            if (this.mode == 'Add') {
                this.addModel.taskNumber = await this.taskMan.getNumber(this.taskCode.competencyId, this.moduleId, this.taskCode.evaluationTypeId);
            } else if (this.mode == 'Edit') {
                var model = await this.taskEssayMan.getTaskEssayTypeById(this.taskId);
                if (model.competencyId !== this.taskCode.competencyId || model.evaluationTypeId !== this.taskCode.evaluationTypeId || model.moduleId !== this.moduleId) {
                    this.addModel.taskNumber = await this.taskMan.getNumber(this.taskCode.competencyId, this.moduleId, this.taskCode.evaluationTypeId);
                } else {
                    this.addModel.taskNumber = model.taskNumber;
                }
            }
        }

        close() {
            window.location.href = '/Setup/Tasks';
        }

        //Create
        addValidation = false;
        addModel: TaskEssayTypeFormModel = {
            questionTypeId: 7,
            competencyId: null,
            evaluationTypeId: null,
            moduleId: null,
            taskNumber: null,
            layoutType: 0,
            storyLineDescription: null,
            question: null,
            createdBy: null,
            fileContent: null,
        }
        storyLineDescription1: string = null;
        storyLineDescription2: string = null;
        storyLine: boolean = false;
        stringLayoutType = '0';
        taskCode = {
            competencyId: null,
            evaluationTypeId: null
        };
        blobId = null;
        moduleId = null;
        imageDataLeft = null;
        imageNameLeft = '';
        imageDataRight = null;
        imageNameRight = '';
        //addFormDataLeft = null;
        //addFormDataRight = null;
        addFormDataLeft: IFileContent = {
            base64: "",
            contentType: "",
            fileName: ""
        };
        addFormDataRight: IFileContent = {
            base64: "",
            contentType: "",
            fileName: ""
        };

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

        async fileChangeLeft(e) {
            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");

            this.addFormDataLeft.base64 = base64String;
            this.addFormDataLeft.fileName = e.target.files[0].name;
            this.addFormDataLeft.contentType = array.pop();

        }

        async fileChangeRight(e) {
            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");

            this.addFormDataRight.base64 = base64String;
            this.addFormDataRight.fileName = e.target.files[0].name;
            this.addFormDataRight.contentType = array.pop();

        }

        loadFileLeft(e) {
            var fileInput = e.target.files || e.dataTransfer.files;
            if (!fileInput.length) return;

            var extension = fileInput[0].name.substring(fileInput[0].name.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpg' && extension != '.png' && extension != '.jpeg') {
                return;
            }

            var reader = new FileReader();
            reader.onload = (e) => {
                this.imageDataLeft = reader.result;
            }
            reader.readAsDataURL(e.target.files[0]);
            this.imageNameLeft = e.target.files[0].name;

            this.fileChangeLeft(e);

            //this.addFormDataLeft = new FormData();
            //Array
            //    .from(Array(e.target.files.length).keys())
            //    .map(x => {
            //        this.addFormDataLeft.append(e.target.files, e.target.files[x], e.target.files[x].name);
            //    });
        }

        loadFileRight(e) {
            var fileInput = e.target.files || e.dataTransfer.files;
            if (!fileInput.length) return;

            var extension = fileInput[0].name.substring(fileInput[0].name.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpg' && extension != '.png' && extension != '.jpeg') {
                return;
            }

            var reader = new FileReader();
            reader.onload = (e) => {
                this.imageDataRight = reader.result;
            }
            reader.readAsDataURL(e.target.files[0]);
            this.imageNameRight = e.target.files[0].name;

            this.fileChangeRight(e);
        }

        validateAdd() {
            if (this.taskCode.competencyId == 0 || this.moduleId == 0 || this.addModel.question == '') {
                return false;
            }

            if (this.storyLine) {

                if (this.stringLayoutType == '0') {
                    return false;
                }

                if (this.stringLayoutType == '1') {
                    if (this.imageNameLeft == '' && (this.storyLineDescription1 == null || this.storyLineDescription1 == '')) {
                        return false;
                    }
                }
                else if (this.stringLayoutType == '2') {
                    if (this.imageNameRight == '' && (this.storyLineDescription2 == null || this.storyLineDescription2 == '')) {
                        return false;
                    }
                }
            }

            if (this.storyLine && this.storyLineDescription1 != null) {
                if (this.stringLayoutType == '1' && this.storyLineDescription1.length > 5000) {
                    return false;
                }
            }

            if (this.storyLine && this.storyLineDescription2 != null) {
                if (this.stringLayoutType == '2' && this.storyLineDescription2.length > 5000) {
                    return false;
                }
            }

            if (this.addModel.question != null) {
                if (this.addModel.question.length > 2000) {
                    return false;
                }
            }

            return true;
        }

        async addData() {
            this.isBusy = true;

            this.addValidation = true;
            if (this.addModel.question == null) {
                this.addModel.question = '';
            }
            if (isNullOrUndefined(this.taskCode.competencyId)) {
                this.taskCode.competencyId = 0;
            }
            if (isNullOrUndefined(this.moduleId)) {
                this.moduleId = 0;
            }
            if (this.addModel.storyLineDescription == null) {
                this.addModel.storyLineDescription = '';
            }
            if (this.validateAdd() === false) {
                this.addValidation = false;
            }
            if (this.addValidation === true && this.mode == 'Add') {

                this.addModel.competencyId = this.taskCode.competencyId;
                this.addModel.evaluationTypeId = this.taskCode.evaluationTypeId;
                this.addModel.moduleId = this.moduleId.moduleId;

                if (this.storyLine == false) {
                    this.stringLayoutType = '0';
                }

                this.addModel.layoutType = Number.parseInt(this.stringLayoutType);
                this.addModel.createdBy = 'Admin';

                if (this.storyLine == false) {
                    this.stringLayoutType = '0';
                    this.addModel.blobId = null;
                    this.addModel.fileContent = null;
                    this.addModel.layoutType = 0;
                }
                else {
                    if (this.stringLayoutType == '1') {
                        this.addModel.fileContent = this.addFormDataLeft;
                        this.addModel.storyLineDescription = this.storyLine ? this.storyLineDescription1 : '';
                    } else if (this.stringLayoutType == '2') {
                        this.addModel.fileContent = this.addFormDataRight;
                        this.addModel.storyLineDescription = this.storyLine ? this.storyLineDescription2 : '';
                    }
                }



                var taskId = await this.taskEssayMan.create(this.addModel);
                this.close();
            }
            else if (this.addValidation === true && this.mode == 'Edit') {

                this.addModel.taskId = this.taskId;
                this.addModel.competencyId = this.taskCode.competencyId;
                this.addModel.evaluationTypeId = this.taskCode.evaluationTypeId;
                this.addModel.moduleId = this.moduleId;

                this.addModel.createdBy = 'Admin';

                if (this.storyLine == false) {
                    this.stringLayoutType = '0';
                    this.addModel.blobId = null;
                    this.addModel.fileContent = null;
                    this.addModel.layoutType = 0;
                }
                else {
                    this.addModel.layoutType = Number.parseInt(this.stringLayoutType);

                    if (this.stringLayoutType == '1' && (this.imageDataLeft !== null || this.storyLineDescription1 != null || this.storyLineDescription1 != "")) {
                        this.addModel.storyLineDescription = this.storyLine ? this.storyLineDescription1 : '';
                        if (this.tempStringCheckingUpdateImage != this.imageNameLeft && this.imageDataLeft != null) {
                            this.addModel.fileContent = this.addFormDataLeft;
                        } else {
                            if (this.imageDataLeft != null) {
                                this.addModel.blobId = this.blobId;
                            }
                            else {
                                this.addModel.blobId = null;
                            }
                        }
                    } else if (this.stringLayoutType == '2' && (this.imageNameRight !== null || this.storyLineDescription2 != null || this.storyLineDescription2 != "")) {
                        this.addModel.storyLineDescription = this.storyLine ? this.storyLineDescription2 : '';
                        if (this.tempStringCheckingUpdateImage != this.imageNameRight && this.imageDataRight != null) {
                            this.addModel.fileContent = this.addFormDataRight;
                        } else {
                            if (this.imageDataRight != null) {
                                this.addModel.blobId = this.blobId;
                            }
                            else {
                                this.addModel.blobId = null;
                            }
                        }
                    }
                }


                await this.taskEssayMan.update(this.addModel);
                this.close();
            }
            else {
                this.isBusy = false;
            }
        }

        backPage() {
            window.history.back();
        }
    }
</script>
