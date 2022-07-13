<template>
    <div>
        <div class="row">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Matrix > <span class="talent_font_red" v-if="mode == 'Add'">Add</span><span class="talent_font_red" v-if="mode == 'Edit'">Edit</span><span class="talent_font_red" v-if="mode == 'View'">View Detail</span></h3>
                <br />

                <!--Story line option-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox talent_nomargin" style="line-height: 50%;" v-model="storyLine" color="#0085CA" :size="16" :disabled="mode == 'View'" @change="uncheckStory"> Type of Story Line</checkbox>
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
                        <div v-if="addModel.question === ''">Question is required</div>
                        <div v-if="addModel.question != null"><div v-if="addModel.question.length > 2000">Question must be less than 2000 characters</div></div>
                        <div v-if="this.storyLine && ((this.stringLayoutType == '1' && this.imageNameLeft == '' && (this.storyLineDescription1 == null || this.storyLineDescription1 == '')) || (this.stringLayoutType == '2' && this.imageNameRight == '' && (this.storyLineDescription2 == null || this.storyLineDescription2 == '')))">Image or Storyline Description is Required if Using Story Line</div>
                        <div v-if="this.storyLine == true && this.stringLayoutType == '0'">Story Type must be chosen</div>
                        <div v-if="storyLineDescription1 != null"><div v-if="this.stringLayoutType == '1' && storyLineDescription1.length > 5000">Storyline description must be less than 5000 characters</div></div>
                        <div v-if="storyLineDescription2 != null"><div v-if="this.stringLayoutType == '2' && storyLineDescription2.length > 5000">Storyline description must be less than 5000 characters</div></div>
                        <div v-if="matrix.measurement != null"><div v-for="(option, index) in matrix.measurement"><div v-if="matrix.measurement[index] != null && matrix.measurement[index].length > 64">Insert Measurement {{index+1}} must be less than 65 characters</div></div></div>
                        <div v-if="matrix.option != null"><div v-for="(option, index) in matrix.option"><div v-if="matrix.option[index] != null && matrix.option[index].length > 64">Insert Option {{index+1}} must be less than 65 characters</div></div></div>
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
                                    <select class="form-control" v-model="moduleId" :disabled="mode == 'View'">
                                        <option v-for="m in modules" :value="m.moduleId">{{m.moduleName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <textarea rows="5" class="form-control" v-model="addModel.question" maxlength="2000" :disabled="mode == 'View'"></textarea>
                        </div>
                    </div>
                    <br />

                    <div class="alert alert-danger" v-if="emptyOptionErrorMsg.length > 0 || emptyMeasurementErrorMsg.length > 0">
                        <div v-if="emptyOptionErrorMsg.length > 0">{{emptyOptionErrorMsg}}</div>
                        <div v-if="emptyMeasurementErrorMsg.length > 0">{{emptyMeasurementErrorMsg}}</div>
                        <div v-if="emptyScoreErrorMsg.length > 0">{{emptyScoreErrorMsg}}</div>
                    </div>

                    <!--Matrix-->
                    <div class="row">
                        <div class="col-md-12">
                            <table class="talent_matrix talent_nopadding">
                                <thead>
                                    <tr>
                                        <th class="talent_noborder">
                                            <div class="col-md-12">
                                                <div class="row">
                                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                                </div>
                                            </div>
                                        </th>
                                        <th v-for="(m, index) in matrix.measurement" class="talent_noborder">
                                            <div class="col-md-12">
                                                <div class="row">
                                                    <div class="col-md-4 talent_nopaddingright">
                                                        Score
                                                    </div>
                                                    <div class="col-md-8 talent_nopadding">
                                                        <select class="form-control form-control-sm talent_marginbottom" v-model="score[index]" :disabled="mode == 'View'">
                                                            <option v-for="(s) in scores" :value="s">{{s.score}}</option>
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="row">
                                                    <div class="col-md-4 talent_nopaddingright">
                                                        Point
                                                    </div>
                                                    <div class="col-md-8 talent_nopadding">
                                                        <input class="form-control form-control-sm talent_marginbottom" disabled v-model="matrix.score[index].points" />
                                                    </div>
                                                </div>
                                            </div>
                                        </th>
                                        <th class="talent_noborder " style="padding: 0px 10px"></th>
                                    </tr>
                                    <tr>
                                        <th>

                                        </th>
                                        <th v-for="(m, index) in matrix.measurement">
                                            <textarea class="form-control" :placeholder="'Insert Measurement ' + (index + 1)" v-model="matrix.measurement[index]" :disabled="mode == 'View'" maxlength="64"></textarea>
                                        </th>
                                        <th class="talent_noborder " style="padding: 0px 10px">

                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(option, index) in matrix.option">
                                        <td><textarea class="form-control" :placeholder="'Insert Option ' + (index + 1)" v-model="matrix.option[index]" :disabled="mode == 'View'" maxlength="64"></textarea></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td class="talent_noborder" style="padding: 0px 0px 0px 10px" v-if="mode != 'View'">
                                            <a class="h-100 w-100 talent_cursorpoint text-center" v-if="index != matrix.option.length - 1" @click.prevent="remove(index)"><fa icon="trash-alt"></fa></a>
                                            <a class="h-100 w-100 talent_cursorpoint text-center" v-if="index == matrix.option.length - 1" @click.prevent="add()"><fa icon="plus-circle"></fa></a>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <br />
                <!--Total-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex justify-content-between">
                                <div class="col-md-5">
                                    <div class="row ">
                                        <div class="col-md-4 align-self-center">
                                            <h5 class="talent_nomargin">
                                                Max Score
                                            </h5>
                                        </div>
                                        <div class="col-md-8">
                                            <input class="form-control" disabled :value="Math.max(matrix.score[0].score, matrix.score[1].score, matrix.score[2].score, matrix.score[3].score, matrix.score[4].score) * matrix.option.length" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row ">
                                        <div class="col-md-4 align-self-center">
                                            <h5 class="talent_nomargin">
                                                Max Points
                                            </h5>
                                        </div>
                                        <div class="col-md-8">
                                            <input class="form-control" disabled :value="Math.max(matrix.score[0].points, matrix.score[1].points, matrix.score[2].points, matrix.score[3].points, matrix.score[4].points) * matrix.option.length" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <br />
                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div v-if="fromOutside === true">
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="backPage">
                                        Close
                                    </button>
                                </div>
                                <div v-else>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="close">
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
    import { MatrixService, TaskService, SetupTimePointService, TaskMatrixTypeFormModel, TaskMatrixChoiceFormModel, FileContent } from '../../../services/NSwagService'

    @Component({
        props: ['mode', 'taskId', 'fromOutside'],
        created: async function (this: Matrixs) {
            this.isBusy = true;
            this.initData();
        },
        watch: {
            taskCode(val, oldVal) {
                this.getTaskNumber();
            },
            moduleId(val, oldVal) {
                this.getTaskNumber();
            },
            score(val, oldVal) {
                this.matrix.score[0].score = val[0] == null ? null : val[0].score;
                this.matrix.score[1].score = val[1] == null ? null : val[1].score;
                this.matrix.score[2].score = val[2] == null ? null : val[2].score;
                this.matrix.score[3].score = val[3] == null ? null : val[3].score;
                this.matrix.score[4].score = val[4] == null ? null : val[4].score;
                this.matrix.score[0].points = val[0] == null ? null : val[0].points;
                this.matrix.score[1].points = val[1] == null ? null : val[1].points;
                this.matrix.score[2].points = val[2] == null ? null : val[2].points;
                this.matrix.score[3].points = val[3] == null ? null : val[3].points;
                this.matrix.score[4].points = val[4] == null ? null : val[4].points;
            }
        },
        methods: {
            fetch(this: Matrixs) {
            }
        }
    })
    export default class Matrixs extends Vue {
        isBusy: boolean = false;
        mode: string;
        taskId: number;
        fromOutside: boolean;

        taskCodes = {};
        modules = {};
        scores = [];
        score = [null, null, null, null, null];

        taskMatrixMan: MatrixService = new MatrixService();
        taskMan: TaskService = new TaskService();
        setupTimePointMan: SetupTimePointService = new SetupTimePointService();

        async initData() {
            this.taskCodes = await this.taskMan.getAllCompetencies();
            this.modules = await this.taskMan.getAllModulesForTask();
            this.scores = await this.taskMan.getAllTimePointsForTask();

            if (this.mode == 'Edit' || this.mode == 'View') {
                var model = await this.taskMatrixMan.getTaskMatrixTypeById(this.taskId);
                this.taskCode.competencyId = model.competencyId;
                this.taskCode.evaluationTypeId = model.evaluationTypeId;
                this.moduleId = model.moduleId;
                this.addModel.taskNumber = model.taskNumber;
                this.addModel.question = model.question;
                var found1 = this.scores.findIndex(Q => Q.timePointId == model.scoreColumn1);
                if (found1 > -1) {
                    this.score[0] = this.scores[found1];
                }
                var found2 = this.scores.findIndex(Q => Q.timePointId == model.scoreColumn2);
                if (found2 > -1) {
                    this.score[1] = this.scores[found2];
                }
                var found3 = this.scores.findIndex(Q => Q.timePointId == model.scoreColumn3);
                if (found3 > -1) {
                    this.score[2] = this.scores[found3];
                }
                var found4 = this.scores.findIndex(Q => Q.timePointId == model.scoreColumn4);
                if (found4 > -1) {
                    this.score[3] = this.scores[found4];
                }
                var found5 = this.scores.findIndex(Q => Q.timePointId == model.scoreColumn5);
                if (found5 > -1) {
                    this.score[4] = this.scores[found5];
                }
                for (var i = 0; i < 5; i++) {
                    this.matrix.score[i].score = this.score[i].score;
                    this.matrix.score[i].points = this.score[i].points;
                }

                // storyline
                this.addModel.storyLineDescription = model.storyLineDescription;
                this.stringLayoutType = model.layoutType.toString();
                this.storyLine = (this.stringLayoutType == '0') ? false : true;
                if (this.stringLayoutType == '1') {
                    this.imageDataLeft = model.blobId == null ? null : await BlobService.getImageUrl(model.blobId, model.mime);
                    this.imageNameLeft = model.blobId == null ? '' : model.fileName;
                    this.storyLineDescription1 = model.storyLineDescription;
                    this.imageLeftBlobId = model.blobId;
                }
                else if (this.stringLayoutType == '2') {
                    this.imageDataRight = model.blobId == null ? null : await BlobService.getImageUrl(model.blobId, model.mime);
                    this.imageNameRight = model.blobId == null ? '' : model.fileName;
                    this.storyLineDescription2 = model.storyLineDescription;
                    this.imageRightBlobId = model.blobId;
                }

                // choices
                for (var i = 0; i < 5; i++) {
                    this.matrix.measurement[i] = model.taskMatrixChoices[i].text;
                }
                for (var i = 0; i < model.taskMatrixQuestions.length; i++) {
                    if (i > 0) {
                        this.add();
                    }
                    this.matrix.option[i] = model.taskMatrixQuestions[i].question;
                }
            }

            this.isBusy = false;
        }

        async getTaskNumber() {
            if (this.mode == 'Add') {
                this.addModel.taskNumber = await this.taskMan.getNumber(this.taskCode.competencyId, this.moduleId, this.taskCode.evaluationTypeId);
            } else if (this.mode == 'Edit') {
                var model = await this.taskMatrixMan.getTaskMatrixTypeById(this.taskId);
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
        addModel: TaskMatrixTypeFormModel = {
            questionTypeId: 12,
            competencyId: null,
            evaluationTypeId: null,
            moduleId: null,
            taskNumber: null,
            layoutType: 0,
            storyLineDescription: null,
            question: null,
            createdBy: null,
            imageUpload: null,
        };
        matrixChoice: TaskMatrixChoiceFormModel = {
            text: null,
            taskId: null,
        };
        storyLineDescription1: string = null;
        storyLineDescription2: string = null;
        storyLine: boolean = false;
        stringLayoutType = '0';
        taskCode = {
            competencyId: null,
            evaluationTypeId: null
        };
        moduleId = null;
        imageDataLeft = null;
        imageNameLeft = '';
        imageDataRight = null;
        imageNameRight = '';
        addFormDataLeft = null;
        addFormDataRight = null;
        emptyOptionErrorMsg = '';
        emptyMeasurementErrorMsg = '';
        emptyScoreErrorMsg = '';

        imageUploadLeft: FileContent = null;
        imageUploadRight: FileContent = null;
        imageLeftBlobId: string = null;
        imageRightBlobId: string = null;

        async convertToBase64(file: File): Promise<string> {
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

        async loadFileLeft(e) {
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

            this.addFormDataLeft = new FormData();
            Array
                .from(Array(e.target.files.length).keys())
                .map(x => {
                    this.addFormDataLeft.append(e.target.files, e.target.files[x], e.target.files[x].name);
                });

            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");

            this.imageUploadLeft = {
                base64: base64String,
                fileName: file.name,
                contentType: array.pop()
            };

            this.imageUploadRight = null;
        }

        async loadFileRight(e) {
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

            this.addFormDataRight = new FormData();
            Array
                .from(Array(e.target.files.length).keys())
                .map(x => {
                    this.addFormDataRight.append(e.target.files, e.target.files[x], e.target.files[x].name);
                });

            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");

            this.imageUploadRight = {
                base64: base64String,
                fileName: file.name,
                contentType: array.pop()
            };

            this.imageUploadLeft = null;
        }

        validateAdd() {
            if (this.taskCode.competencyId == 0 || this.moduleId == 0 || this.addModel.question == '') {
                return false;
            }

            if (this.storyLine) {
                if (this.stringLayoutType == '1') {
                    if (this.imageNameLeft == '' && (this.storyLineDescription1 == null || this.storyLineDescription1 == '')) {
                        return false;
                    }
                }
                else if (this.stringLayoutType == '2') {
                    if (this.imageNameRight == '' && (this.storyLineDescription2 == null || this.storyLineDescription2 == '')) {
                        return false;
                    }
                } else if (this.stringLayoutType == '0') {
                    return false;
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

            if (this.matrix.measurement != null) {
                for (var i = 0; i < this.matrix.measurement.length; i++) {
                    if (this.matrix.measurement[i] != null && this.matrix.measurement[i].length > 64) {
                        return false;
                    }
                }
            }

            if (this.matrix.option != null) {
                for (var i = 0; i < this.matrix.option.length; i++) {
                    if (this.matrix.option[i] != null && this.matrix.option[i].length > 64) {
                        return false;
                    }
                }
            }

            return true;
        }

        uncheckStory() {
            this.stringLayoutType = '0';
        }

        async addData() {
            this.isBusy = true;

            this.addValidation = true;
            this.emptyOptionErrorMsg = '';
            this.emptyMeasurementErrorMsg = '';
            this.emptyScoreErrorMsg = '';

            // check choices
            var emptyOptions = [];
            for (var i = 0; i < this.matrix.option.length; i++) {
                if (this.matrix.option[i] == null || this.matrix.option[i] == '') {
                    emptyOptions.push(i + 1);
                }
            }
            if (emptyOptions.length > 0) {
                this.emptyOptionErrorMsg = 'Please fill option ' + emptyOptions.toString();
            }

            // check measurements
            var emptyMeasurements = [];
            for (var i = 0; i < this.matrix.measurement.length; i++) {
                if (this.matrix.measurement[i] == null || this.matrix.measurement[i] == '') {
                    emptyMeasurements.push(i + 1);
                }
            }
            if (emptyMeasurements.length > 0) {
                this.emptyMeasurementErrorMsg = 'Please fill measurement ' + emptyMeasurements.toString();
            }

            // check score
            var emptyScores = [];
            for (var i = 0; i < 5; i++) {
                if (this.score[i] == null) {
                    emptyScores.push(i + 1);
                }
            }
            if (emptyScores.length > 0) {
                this.emptyScoreErrorMsg = 'Please fill score ' + emptyScores.toString();
            }

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
            this.addModel.scoreColumn1 = Number.parseInt(this.score[0].timePointId);
            this.addModel.scoreColumn2 = Number.parseInt(this.score[1].timePointId);
            this.addModel.scoreColumn3 = Number.parseInt(this.score[2].timePointId);
            this.addModel.scoreColumn4 = Number.parseInt(this.score[3].timePointId);
            this.addModel.scoreColumn5 = Number.parseInt(this.score[4].timePointId);
            if (this.addValidation === true && this.emptyOptionErrorMsg === '' && this.emptyMeasurementErrorMsg === '' && this.emptyScoreErrorMsg === '' && this.mode === 'Add') {
                this.addModel.competencyId = this.taskCode.competencyId;
                this.addModel.evaluationTypeId = this.taskCode.evaluationTypeId;
                this.addModel.moduleId = this.moduleId;
                //this.addModel.layoutType = Number.parseInt(this.stringLayoutType);
                this.addModel.createdBy = 'Admin';
                if (this.storyLine == true && this.stringLayoutType == '1') {
                    //this.addModel.blobId = this.addFormDataLeft !== null ? await BlobService.uploadService(this.addFormDataLeft) : null;
                    if (this.imageUploadLeft != null) {
                        this.addModel.imageUpload = this.imageUploadLeft;
                    }
                    this.addModel.storyLineDescription = this.storyLine ? this.storyLineDescription1 : '';
                    this.addModel.layoutType = 1;

                } else if (this.storyLine == true && this.stringLayoutType == '2') {
                    //this.addModel.blobId = this.addFormDataRight !== null ? await BlobService.uploadService(this.addFormDataRight) : null;
                    if (this.imageUploadRight != null) {
                        this.addModel.imageUpload = this.imageUploadRight;
                    }
                    this.addModel.storyLineDescription = this.storyLine ? this.storyLineDescription2 : '';
                    this.addModel.layoutType = 2;

                } else {
                    this.addModel.layoutType = 0;
                }
                //this.addModel.storyLineDescription = this.storyLine ? this.addModel.storyLineDescription : '';
                this.addModel.taskMatrixChoices = [];
                for (var i = 0; i < this.matrix.measurement.length; i++) {
                    this.addModel.taskMatrixChoices.push({ 'text': this.matrix.measurement[i], 'taskId': this.taskId, number: i + 1 });
                }
                this.addModel.taskMatrixQuestions = [];
                for (var i = 0; i < this.matrix.option.length; i++) {
                    this.addModel.taskMatrixQuestions.push({ question: this.matrix.option[i], 'taskId': this.taskId, number: i + 1 });
                }
                var taskId = await this.taskMatrixMan.create(this.addModel);
                this.close();
            }
            else if (this.addValidation === true && this.emptyOptionErrorMsg === '' && this.emptyMeasurementErrorMsg === '' && this.emptyScoreErrorMsg === '' && this.mode === 'Edit') {
                this.addModel.taskId = this.taskId;
                this.addModel.competencyId = this.taskCode.competencyId;
                this.addModel.evaluationTypeId = this.taskCode.evaluationTypeId;
                this.addModel.moduleId = this.moduleId;
                //this.addModel.layoutType = Number.parseInt(this.stringLayoutType);
                this.addModel.createdBy = 'Admin';
                if (this.storyLine == true && this.stringLayoutType == '1') {
                    if (this.imageUploadLeft != null) {
                        this.addModel.imageUpload = this.imageUploadLeft;
                        this.addModel.blobId = null;
                    }
                    else {
                        this.addModel.blobId = this.imageLeftBlobId;
                    }
                    this.addModel.storyLineDescription = this.storyLine ? this.storyLineDescription1 : '';
                    this.addModel.layoutType = 1;

                } else if (this.storyLine == true && this.stringLayoutType == '2') {
                    if (this.imageUploadRight != null) {
                        this.addModel.imageUpload = this.imageUploadRight;
                        this.addModel.blobId = null;
                    }
                    else {
                        this.addModel.blobId = this.imageRightBlobId;
                    }
                    this.addModel.storyLineDescription = this.storyLine ? this.storyLineDescription2 : '';
                    this.addModel.layoutType = 2;
                } else {
                    this.addModel.layoutType = 0;
                }
                this.addModel.taskMatrixChoices = [];
                for (var i = 0; i < this.matrix.measurement.length; i++) {
                    this.addModel.taskMatrixChoices.push({ 'text': this.matrix.measurement[i], 'taskId': this.taskId, number: i + 1 });
                }
                this.addModel.taskMatrixQuestions = [];
                for (var i = 0; i < this.matrix.option.length; i++) {
                    this.addModel.taskMatrixQuestions.push({ question: this.matrix.option[i], 'taskId': this.taskId, number: i + 1 });
                }
                await this.taskMatrixMan.update(this.addModel);
                this.close();
            }
            else {
                this.isBusy = false;
            }
        }

        matrix: IMatrixContainer = { question: "question", measurement: [null, null, null, null, null], option: [null], score: [{ score: 0, points: 0 }, { score: 0, points: 0 }, { score: 0, points: 0 }, { score: 0, points: 0 }, { score: 0, points: 0 }] }

        add() {
            this.matrix.option.push(null);
        }

        remove(index: number) {
            if (this.matrix.option.length <= 1) {
                return
            }
            this.matrix.option.splice(index, 1);
        }

        backPage() {
            window.history.back();
        }
    }

    interface IMatrixContainer {
        question: string;
        measurement: string[];
        option: string[];
        score: {};
    }
</script>
