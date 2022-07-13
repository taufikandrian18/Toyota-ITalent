<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="row">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Matching > <span class="talent_font_red" v-if="mode == 'Add'">Add</span><span class="talent_font_red" v-if="mode == 'Edit'">Edit</span><span class="talent_font_red" v-if="mode == 'View'">View Detail</span></h3>
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
                                                <textarea class="form-control talent_textarea" maxlength="5000" v-model="verticalStorylineDescription" :disabled="stringLayoutType != '1' || mode == 'View' || storyLine == false"></textarea>
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
                                            <div class="col-md-5">
                                                <textarea class="form-control h-100 talent_overflowy" maxlength="5000" v-model="horizontalStorylineDescription" :disabled="stringLayoutType != '2' || mode == 'View' || storyLine == false"></textarea>
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
                        <div v-if="addModel.question!=null && addModel.question.length > 2000">Question must be less than 2000 characters</div>
                        <div v-if="(this.verticalStorylineDescription.length >5000 || this.horizontalStorylineDescription.length > 5000)">Storyline description must be less than 5000 characters</div>
                        <div v-if="this.storyLine && 
                        (((this.stringLayoutType == '1' &&this.verticalStorylineDescription=='') 
                        || (this.stringLayoutType == '2' &&this.horizontalStorylineDescription=='')) 
                        && ((this.stringLayoutType == '1' && this.imageNameLeft == '') 
                        || (this.stringLayoutType == '2' && this.imageNameRight == '')))">Image or Storyline Description is Required if Using Story Line</div>
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
                            <input class="form-control" v-model="addModel.question" maxlength="2000" :disabled="mode == 'View'" />
                            <br />

                            <div v-for="(image, index) in matching.uploadLeft" class="" :key="index">
                                <div class="row ">
                                    <div :class="mode != 'View' ? 'col-md-11' : 'col-md-12'">
                                        <div class="row">
                                            <div class="col-md-6 talent_borderright ">
                                                <div class="talent_marginbottom h-100">
                                                    <div class="input-group mb-3 talent_nomargin">
                                                        <div class="input-group-prepend ">
                                                            <span class="input-group-text talent_noroundborder_bottom">{{index+1}}</span>
                                                        </div>
                                                        <select class="form-control talent_noroundborder_bottom" v-model="matching.uploadLeft[index].type" :disabled="mode == 'View'">
                                                            <option value="image">Image</option>
                                                            <option value="text">Text</option>
                                                        </select>
                                                    </div>
                                                    <div v-if="matching.uploadLeft[index].type === 'image'">
                                                        <img class="h-100 w-100" :src="matching.uploadLeft[index].image ? matching.uploadLeft[index].image : '/upload-image2.png'" />
                                                        <div class="custom-file">
                                                            <input type="file" class="custom-file-input talent_noroundborder_top" accept="image/*" @change.prevent="loadFileChoiceLeft($event, index)" :disabled="mode == 'View'">
                                                            <label class="custom-file-label talent_textshorten talent_noroundborder_top">{{matching.uploadLeft[index].name ? matching.uploadLeft[index].name : 'Choose File'}}</label>
                                                        </div>
                                                    </div>
                                                    <div v-if="matching.uploadLeft[index].type === 'text'" class="h-100">
                                                        <!--<input class="form-control" v-model="matching.uploadLeft[index].text" />-->
                                                        <textarea class="form-control talent_textarea talent_noroundborder_top" v-model="matching.uploadLeft[index].text" style="height: calc(100% - 52px)!important;" maxlength="2000" :disabled="mode == 'View'"></textarea>

                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-6 ">
                                                <div class="talent_marginbottom h-100">
                                                    <div class="input-group mb-3 talent_nomargin">
                                                        <div class="input-group-prepend">
                                                            <span class="input-group-text talent_noroundborder_bottom">{{index+1}}</span>
                                                        </div>
                                                        <select class="form-control talent_noroundborder_bottom" v-model="matching.uploadRight[index].type" :disabled="mode == 'View'">
                                                            <option value="image">Image</option>
                                                            <option value="text">Text</option>
                                                        </select>
                                                    </div>
                                                    <div v-if="matching.uploadRight[index].type === 'image'">
                                                        <img class="h-100 w-100" :src="matching.uploadRight[index].image ? matching.uploadRight[index].image : '/upload-image2.png'" :disabled="mode == 'View'" />
                                                        <div class="custom-file">
                                                            <input type="file" class="custom-file-input talent_noroundborder_top" accept="image/*" @change.prevent="loadFileChoiceRight($event, index)" :disabled="mode == 'View'">
                                                            <label class="custom-file-label talent_textshorten talent_noroundborder_top">{{matching.uploadRight[index].name ? matching.uploadRight[index].name : 'Choose File'}}</label>
                                                        </div>
                                                    </div>
                                                    <div v-if="matching.uploadRight[index].type === 'text'" class="h-100">
                                                        <textarea class="form-control talent_textarea talent_noroundborder_top" v-model="matching.uploadRight[index].text" style="height: calc(100% - 52px)!important;" maxlength="2000" :disabled="mode == 'View'"></textarea>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1 d-flex align-items-center " v-if="mode != 'View'">
                                        <div v-if="index != matching.uploadLeft.length - 1" class="text-center" @click.prevent="remove(index)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                        <div v-if="index == matching.uploadLeft.length - 1" class="text-center" @click.prevent="add()"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                    </div>
                                </div>
                            </div>

                            <div class="alert alert-danger" v-if="emptyChoiceErrorMsg.length > 0">{{emptyChoiceErrorMsg}}</div>

                            <br />
                            <!--Point-->
                            <div class="row">
                                <div class="col-md-12">
                                    <h5 v-model="matching.answer">Answer<span class="talent_font_red">*</span></h5>
                                </div>
                            </div>

                            <div v-for="(image, index) in matching.uploadLeft">
                                <div class="row talent_marginbottom">
                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-6">
                                                <input class="form-control" :value="index+1" disabled />
                                            </div>
                                            <div class="col-md-6">
                                                <select class="form-control" v-model="matching.answerList[index].answerRight" :disabled="mode == 'View'">
                                                    <option v-for="(image, index) in matching.uploadLeft" :value="index+1">{{index+1}}</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="alert alert-danger" v-if="emptyAnswerErrorMsg.length > 0">{{emptyAnswerErrorMsg}}</div>

                            <br />

                            <div class="row">
                                <div class="col-md-6">
                                    <h5 v-model="matching.answer">Score<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-6">
                                    <h5 v-model="matching.answer">Points</h5>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6">
                                    <select class="form-control" v-model="score" :disabled="mode == 'View'">
                                        <option v-for="(s) in scores" :value="s">{{s.score}}</option>
                                    </select>
                                </div>
                                <div class="col-md-6">
                                    <input class="form-control" v-model="score.points" disabled />
                                </div>
                            </div>

                            <br />

                            <div class="alert alert-danger" v-if="emptyScoreErrorMsg.length > 0">{{emptyScoreErrorMsg}}</div>
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
                                                Total Score
                                            </h5>
                                        </div>
                                        <div class="col-md-8">
                                            <input class="form-control" v-model="score.score" disabled />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row ">
                                        <div class="col-md-4 align-self-center">
                                            <h5 class="talent_nomargin">
                                                Total Points
                                            </h5>
                                        </div>
                                        <div class="col-md-8">
                                            <input class="form-control" v-model="score.points" disabled />
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
                                    <button v-if="mode != 'View'" class="btn talent_bg_lightgreen talent_font_white" @click.prevent="onClickButtonSave">
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

    import { TaskService, TaskMatchingTypeFormModel, TaskMatchingChoiceFormModel, MatchingService, SetupTimePointService, TimePointTaskModel, FileContent } from '../../../services/NSwagService';

    @Component({
        props: ['mode', 'taskId', 'fromOutside'],
        created: async function (this: Matching) {
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
            fetch(this: Matching) {
            }
        }
    })
    export default class Matching extends Vue {
        isBusy: boolean = false;
        mode: string;
        taskId: number;
        fromOutside: boolean;

        taskCodes = {};
        modules = {};
        scores = [];
        score: TimePointTaskModel = { score: null, points: null, timePointId: null, time: null };

        taskMatchingMan: MatchingService = new MatchingService();
        taskMan: TaskService = new TaskService();
        setupTimePointMan: SetupTimePointService = new SetupTimePointService();
        verticalStorylineDescription : string = '';
        horizontalStorylineDescription : string = '';

        async initData() {
            this.taskCodes = await this.taskMan.getAllCompetencies();
            this.modules = await this.taskMan.getAllModulesForTask();
            this.scores = await this.taskMan.getAllTimePointsForTask();
            this.add();

            if (this.mode == 'Edit' || this.mode == 'View') {
                var model = await this.taskMatchingMan.getTaskMatchingTypeById(this.taskId);
                this.taskCode.competencyId = model.competencyId;
                this.taskCode.evaluationTypeId = model.evaluationTypeId;
                this.moduleId = model.moduleId;
                this.addModel.taskNumber = model.taskNumber;
                this.addModel.question = model.question;
                this.addModel.blobId = model.blobId;
                var answers = [];
                var answer = '';
                var found = this.scores.findIndex(Q => Q.timePointId == model.score);
                if (found > -1) {
                    this.score = this.scores[found];
                }
                for (var i = 0; i < model.answer.length; i++) {
                    if (model.answer[i] != '-')
                        answer += model.answer[i];
                    if (model.answer[i] == '-' || i == model.answer.length - 1) {
                        answers.push(answer);
                        answer = '';
                    }
                }
                for (var i = 0; i < answers.length; i++) {
                    if (i > 1) {
                        this.add();
                    }
                    this.matching.answerList[i].answerRight = answers[i].toString();
                }

                // storyline
                this.storyLine = model.layoutType!=0;
                if(model.layoutType == 1){
                    this.verticalStorylineDescription = model.storyLineDescription || '';
                }
                if(model.layoutType == 2){
                    this.horizontalStorylineDescription = model.storyLineDescription || '';
                }
                this.stringLayoutType = model.layoutType.toString();
                if (this.stringLayoutType == '1') {
                    this.imageDataLeft = model.blobId == null ? null : await BlobService.getImageUrl(model.blobId, model.mime);
                    this.imageNameLeft = model.blobId == null ? '' : model.fileName;
                }
                if (this.stringLayoutType == '2') {
                    this.imageDataRight = model.blobId == null ? null : await BlobService.getImageUrl(model.blobId, model.mime);
                    this.imageNameRight = model.blobId == null ? '' : model.fileName;
                }
                // choices
                for (var i = 0; i < this.matching.uploadLeft.length; i++) {
                    this.matching.uploadLeft[i].type = model.taskMatchingChoices[(i * 2) + 0].text == null ? 'image' : 'text';
                    if (this.matching.uploadLeft[i].type == 'text') {
                        this.matching.uploadLeft[i].text = model.taskMatchingChoices[(i * 2) + 0].text;
                    } else {
                        this.matching.uploadLeft[i].name = model.taskMatchingChoices[(i * 2) + 0].fileName;
                        this.matching.uploadLeft[i].blobId = model.taskMatchingChoices[(i * 2) + 0].blobId;
                        this.matching.uploadLeft[i].image = await BlobService.getImageUrl(this.matching.uploadLeft[i].blobId, model.taskMatchingChoices[(i * 2) + 0].mime);
                    }
                    this.matching.uploadRight[i].type = model.taskMatchingChoices[(i * 2) + 1].text == null ? 'image' : 'text';
                    if (this.matching.uploadRight[i].type == 'text') {
                        this.matching.uploadRight[i].text = model.taskMatchingChoices[(i * 2) + 1].text;
                    } else {
                        this.matching.uploadRight[i].name = model.taskMatchingChoices[(i * 2) + 1].fileName;
                        this.matching.uploadRight[i].blobId = model.taskMatchingChoices[(i * 2) + 1].blobId;
                        this.matching.uploadRight[i].image = await BlobService.getImageUrl(this.matching.uploadRight[i].blobId, model.taskMatchingChoices[(i * 2) + 1].mime);
                    }
                }
            }

            this.isBusy = false;
        }

        async getTaskNumber() {
            if (this.mode == 'Add') {
                this.addModel.taskNumber = await this.taskMan.getNumber(this.taskCode.competencyId, this.moduleId, this.taskCode.evaluationTypeId);
            } else if (this.mode == 'Edit') {
                var model = await this.taskMatchingMan.getTaskMatchingTypeById(this.taskId);
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
        addModel: TaskMatchingTypeFormModel = {
            questionTypeId: 2,
            competencyId: null,
            evaluationTypeId: null,
            moduleId: null,
            taskNumber: null,
            layoutType: 0,
            storyLineDescription: null,
            question: null,
            answer: null,
            createdBy: null,
            imageUpload: null
        };
        matchingChoice: TaskMatchingChoiceFormModel = {
            text: null,
            taskId: null,
            taskMatchingCode: null
        };
        storyLine: boolean = false;
        stringLayoutType = '1';
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

        imageUploadLeft: FileContent = null;
        imageUploadRight: FileContent = null;

        formImageUploadLeft = [{
            index: -1,
            file: null
        }];
        formImageUploadRight = [{
            index: -1,
            file: null
        }];

        fileTypeErrorMsg = '';
        emptyChoiceErrorMsg = '';
        emptyAnswerErrorMsg = '';
        emptyScoreErrorMsg = '';

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

            this.fileTypeErrorMsg = "";
            var extension = fileInput[0].name.substring(fileInput[0].name.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpg' && extension != '.png' && extension != '.jpeg') {
                this.fileTypeErrorMsg = "Please upload .jpg or .png image";
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

            this.fileTypeErrorMsg = "";
            var extension = fileInput[0].name.substring(fileInput[0].name.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpg' && extension != '.png' && extension != '.jpeg') {
                this.fileTypeErrorMsg = "Please upload .jpg or .png image";
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
            let isVerticalDescEmpty = Number.parseInt(this.stringLayoutType)===1 && this.verticalStorylineDescription === '';
            let isHorizontalDescEmpty = Number.parseInt(this.stringLayoutType)===2 && this.horizontalStorylineDescription === '';
            let isVerticalImageEmpty = Number.parseInt(this.stringLayoutType)===1 && this.imageNameLeft == '';
            let isHorizontalImageEmpty = Number.parseInt(this.stringLayoutType)===2 && this.imageNameRight == '';

            if (this.taskCode.competencyId == 0 || this.moduleId == 0 || this.addModel.question == '' ||
                (this.storyLine && (isVerticalDescEmpty || isHorizontalDescEmpty) && (isVerticalImageEmpty || isHorizontalImageEmpty))) {
                return false;
            }
            let storylineDesc = this.addModel.storyLineDescription || '';
            let question = this.addModel.question || '';
            if (storylineDesc.length > 5000) return false;
            if (question.length > 2000) return false;
                
            return true;
        }

        async onClickButtonSave() {
            this.isBusy = true;

            this.addValidation = true;
            this.emptyChoiceErrorMsg = '';
            this.emptyAnswerErrorMsg = '';
            this.emptyScoreErrorMsg = '';
            this.addModel.storyLineDescription = null;
            this.addModel.imageUpload = null;
            this.addModel.layoutType=0;
            
            // check choices
            var emptyLeftOptions = [];
            var emptyRightOptions = [];
            for (var i = 0; i < this.matching.uploadLeft.length; i++) {
                if (this.matching.uploadLeft[i].type == 'text' && (this.matching.uploadLeft[i].text == null || this.matching.uploadLeft[i].text == '') ||
                    this.matching.uploadLeft[i].type == 'image' && this.matching.uploadLeft[i].image == null) {
                    emptyLeftOptions.push(i + 1);
                }
                if (this.matching.uploadRight[i].type == 'text' && (this.matching.uploadRight[i].text == null || this.matching.uploadLeft[i].text == '') ||
                    this.matching.uploadRight[i].type == 'image' && this.matching.uploadRight[i].image == null) {
                    emptyRightOptions.push(i + 1);
                }
            }
            if (emptyLeftOptions.length > 0 || emptyRightOptions.length > 0) {
                this.emptyChoiceErrorMsg = 'Please fill number: ' + (emptyLeftOptions.length > 0 ? emptyLeftOptions.toString() + ' (left)' : '') + ' ' + (emptyRightOptions.length > 0 ? emptyRightOptions.toString() + ' (right)' : '');
            }

            // check answers
            var answersDistinct = [];
            var unique = {};
            for (var i = 0; i < this.matching.answerList.length; i++) {
                if (typeof (unique[this.matching.answerList[i].answerRight]) == "undefined") {
                    answersDistinct.push(this.matching.answerList[i].answerRight);
                }
                unique[this.matching.answerList[i].answerRight] = 0;
            }
            if (answersDistinct.length != this.matching.answerList.length) {
                this.emptyAnswerErrorMsg = 'Please fill the answers with different values';
            }
            for (var i = 0; i < this.matching.answerList.length; i++) {
                if (this.matching.answerList[i].answerRight == null)
                    this.emptyAnswerErrorMsg = 'Please fill all the answers';
            }

            // check score
            if (isNullOrUndefined(this.score.score)) {
                this.emptyScoreErrorMsg = 'Score is Required';
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
    
            if (this.validateAdd() === false) {
                this.addValidation = false;
                this.isBusy =false;
                return;
            }
            if(this.storyLine){
                let storylineType = Number.parseInt(this.stringLayoutType);
                this.addModel.layoutType = storylineType;

                if(storylineType==1 && this.verticalStorylineDescription){
                    this.addModel.storyLineDescription = this.verticalStorylineDescription;
                }
                if(storylineType==2 && this.horizontalStorylineDescription){
                    this.addModel.storyLineDescription = this.horizontalStorylineDescription;
                }
                if (this.stringLayoutType == '1') {
                    // Note: 
                    // kalau image lama, maka imageDataRight/Left dimulai dengan http (url) dan addModel.blobId != null
                    // Jika image baru, maka imageDataRight/Left dimulai dengan data: (local source saat input image)
                    // dan dari adanya addModel.blobId atau tidak.
                    // -Aldrian, ngubah erlina punya
                    let isImgSrcFromLocal = this.imageDataLeft!=null&&this.imageDataLeft.toString().includes("data:",0)
                    if (isImgSrcFromLocal) {
                        this.addModel.imageUpload = this.imageUploadLeft;
                        this.addModel.blobId = null;
                    }
                    let isNewImageExist = isImgSrcFromLocal && this.addModel.blobId
                    if((this.imageUploadRight==null && this.imageDataLeft==null)||isNewImageExist){
                        this.addModel.blobId = null;
                    }
                }
                if (this.stringLayoutType == '2') {
                    let isImgSrcFromLocal = this.imageDataRight!=null&&this.imageDataRight.toString().includes("data:",0)
                    if (isImgSrcFromLocal) {
                        this.addModel.imageUpload = this.imageUploadRight;
                        this.addModel.blobId = null;
                    }
                    let isNewImageExist = isImgSrcFromLocal && this.addModel.blobId
                    if((this.imageUploadRight==null && this.imageDataLeft==null)||isNewImageExist){
                        this.addModel.blobId = null;
                    }
                }
            }else{
                this.addModel.blobId=null;
            }
            if (this.addValidation === true && this.emptyChoiceErrorMsg === '' && this.emptyAnswerErrorMsg === '' && this.emptyScoreErrorMsg === '' && this.mode == 'Add') {
                var answer = '';
                for (var i = 0; i < this.matching.uploadLeft.length; i++) {
                    answer += this.matching.answerList[i].answerRight;
                    if (i == this.matching.uploadLeft.length - 1) break;
                    answer += '-';
                }
                this.addModel.competencyId = this.taskCode.competencyId;
                this.addModel.evaluationTypeId = this.taskCode.evaluationTypeId;
                this.addModel.moduleId = this.moduleId;
                this.addModel.createdBy = 'Admin';
                this.addModel.answer = answer;
                this.addModel.score = this.score.timePointId;
                
                this.addModel.taskMatchingChoices = [];
                for (var i = 0; i < this.matching.uploadLeft.length; i++) {
                    if (this.matching.uploadLeft[i].type == 'text') {
                        this.addModel.taskMatchingChoices.push({
                            'taskId': this.taskId,
                            'taskMatchingCode': i + 1 > 9 ? (i + 1) + 'A' : '0' + (i + 1) + 'A',
                            'text': this.matching.uploadLeft[i].text,
                            'blobId': null,
                            'imageUpload': null
                        });
                    } else {

                        let index = this.formImageUploadLeft.findIndex(Q => Q.index == i);

                        let theFile = this.formImageUploadLeft[index].file;

                        this.addModel.taskMatchingChoices.push({
                            'taskId': this.taskId,
                            'taskMatchingCode': i + 1 > 9 ? (i + 1) + 'A' : '0' + (i + 1) + 'A',
                            'text': null,
                            'blobId': null,
                            'imageUpload': theFile
                        });
                    }
                    if (this.matching.uploadRight[i].type == 'text') {
                        this.addModel.taskMatchingChoices.push({
                            'taskId': this.taskId,
                            'taskMatchingCode': i + 1 > 9 ? (i + 1) + 'B' : '0' + (i + 1) + 'B',
                            'text': this.matching.uploadRight[i].text,
                            'blobId': null,
                            'imageUpload': null
                        });
                    } else {

                        let index = this.formImageUploadRight.findIndex(Q => Q.index == i);

                        let theFile = this.formImageUploadRight[index].file;

                        this.addModel.taskMatchingChoices.push({
                            'taskId': this.taskId,
                            'taskMatchingCode': i + 1 > 9 ? (i + 1) + 'B' : '0' + (i + 1) + 'B',
                            'text': null,
                            'blobId': null,
                            'imageUpload': theFile
                        });
                    }
                }
                var taskId = await this.taskMatchingMan.create(this.addModel);
                this.close();
                this.isBusy =false;
            }
            else if (this.addValidation === true && this.emptyChoiceErrorMsg === '' && this.emptyAnswerErrorMsg === '' && this.emptyScoreErrorMsg === '' && this.mode == 'Edit') {
                var answer = '';
                for (var i = 0; i < this.matching.uploadLeft.length; i++) {
                    answer += this.matching.answerList[i].answerRight;
                    if (i == this.matching.uploadLeft.length - 1) break;
                    answer += '-';
                }
                this.addModel.taskId = this.taskId;
                this.addModel.competencyId = this.taskCode.competencyId;
                this.addModel.evaluationTypeId = this.taskCode.evaluationTypeId;
                this.addModel.moduleId = this.moduleId;
                this.addModel.createdBy = 'Admin';
                this.addModel.answer = answer;
                this.addModel.score = this.score.timePointId;
            
                this.addModel.taskMatchingChoices = [];
                for (var i = 0; i < this.matching.uploadLeft.length; i++) {
                    if (this.matching.uploadLeft[i].type == 'text') {
                        this.addModel.taskMatchingChoices.push({
                            'taskId': this.taskId,
                            'taskMatchingCode': i + 1 > 9 ? (i + 1) + 'A' : '0' + (i + 1) + 'A',
                            'text': this.matching.uploadLeft[i].text,
                            'blobId': null,
                            'imageUpload': null
                        });
                    } else {

                        let theFile = null;

                        if (this.matching.uploadLeft[i].formData !== null) {
                            let index = this.formImageUploadLeft.findIndex(Q => Q.index == i);
                            theFile = this.formImageUploadLeft[index].file;
                        }

                        this.addModel.taskMatchingChoices.push({
                            'taskId': this.taskId,
                            'taskMatchingCode': i + 1 > 9 ? (i + 1) + 'A' : '0' + (i + 1) + 'A',
                            'text': null,
                            'blobId': this.matching.uploadLeft[i].blobId,
                            'imageUpload': theFile
                        });
                    }
                    if (this.matching.uploadRight[i].type == 'text') {
                        this.addModel.taskMatchingChoices.push({
                            'taskId': this.taskId,
                            'taskMatchingCode': i + 1 > 9 ? (i + 1) + 'B' : '0' + (i + 1) + 'B',
                            'text': this.matching.uploadRight[i].text,
                            'blobId': null,
                            'imageUpload': null
                        });
                    } else {

                        let theFile = null;

                        if (this.matching.uploadRight[i].formData !== null) {
                            let index = this.formImageUploadRight.findIndex(Q => Q.index == i);
                            theFile = this.formImageUploadRight[index].file;
                        }

                        this.addModel.taskMatchingChoices.push({
                            'taskId': this.taskId,
                            'taskMatchingCode': i + 1 > 9 ? (i + 1) + 'B' : '0' + (i + 1) + 'B',
                            'text': null,
                            'blobId': this.matching.uploadRight[i].blobId,
                            'imageUpload': theFile
                        });
                    }
                }
                await this.taskMatchingMan.update(this.addModel);
                this.close();
                this.isBusy =false;
            }
            else {
                this.isBusy = false;
            }
        }

        matching: IMatchingContainer = { answerList: [{ answerLeft: null, answerRight: null, point: 0, score: 0 }], uploadLeft: [{ name: null, image: null, blobId: null, formData: null, type: "image", text: null }], uploadRight: [{ name: null, image: null, blobId: null, formData: null, type: "image", text: null }] }

        async loadFileChoiceLeft(e, index: number) {
            var fileInput = e.target.files || e.dataTransfer.files;
            if (!fileInput.length) return;

            this.fileTypeErrorMsg = "";
            var extension = fileInput[0].name.substring(fileInput[0].name.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpg' && extension != '.png' && extension != '.jpeg') {
                this.fileTypeErrorMsg = "Please upload .jpg or .png image";
                return;
            }

            var reader = new FileReader();
            reader.onload = (e) => {
                this.matching.uploadLeft[index].image = reader.result;
            }
            reader.readAsDataURL(e.target.files[0]);
            this.matching.uploadLeft[index].name = e.target.files[0].name;

            this.matching.uploadLeft[index].formData = new FormData();
            Array
                .from(Array(e.target.files.length).keys())
                .map(x => {
                    this.matching.uploadLeft[index].formData.append(e.target.files, e.target.files[x], e.target.files[x].name);
                });

            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");

            let theFile = {
                base64: base64String,
                fileName: file.name,
                contentType: array.pop()
            };

            this.formImageUploadLeft.push({
                index: index,
                file: theFile 
            });
        }

        async loadFileChoiceRight(e, index: number) {
            var fileInput = e.target.files || e.dataTransfer.files;
            if (!fileInput.length) return;

            this.fileTypeErrorMsg = "";
            var extension = fileInput[0].name.substring(fileInput[0].name.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpg' && extension != '.png' && extension != '.jpeg') {
                this.fileTypeErrorMsg = "Please upload .jpg or .png image";
                return;
            }

            var reader = new FileReader();
            reader.onload = (e) => {
                this.matching.uploadRight[index].image = reader.result;
            }
            reader.readAsDataURL(e.target.files[0]);
            this.matching.uploadRight[index].name = e.target.files[0].name;

            this.matching.uploadRight[index].formData = new FormData();
            Array
                .from(Array(e.target.files.length).keys())
                .map(x => {
                    this.matching.uploadRight[index].formData.append(e.target.files, e.target.files[x], e.target.files[x].name);
                });

            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");

            let theFile = {
                base64: base64String,
                fileName: file.name,
                contentType: array.pop()
            };

            this.formImageUploadRight.push({
                index: index,
                file: theFile 
            });
        }

        add() {
            this.matching.uploadLeft.push({ name: null, image: null, blobId: null, formData: null, type: "image", text: null });
            this.matching.uploadRight.push({ name: null, image: null, blobId: null, formData: null, type: "image", text: null });
            this.matching.answerList.push({ answerLeft: null, answerRight: null, point: 0, score: 0 })
        }

        remove(index: number) {
            if (this.matching.uploadLeft.length <= 2) {
                alert('There should be at least 2 matching pairs')
                return
            }
            this.matching.uploadLeft.splice(index, 1);
            this.matching.uploadRight.splice(index, 1);

            this.matching.answerList.pop();
        }

        generatePoint(index: number) {
            this.matching.answerList[index].point = Math.round(this.matching.answerList[index].score / 10);
        }

        backPage() {
            window.history.back();
        }
    }

    interface IMatchingUpload {
        name: string;
        image: string | ArrayBuffer;
        blobId: string;
        formData: FormData;
        type: string;
        text: string;
    }
    interface IMatchingContainer {
        uploadLeft: IMatchingUpload[];
        uploadRight: IMatchingUpload[];
        answerList: IMatchingAnswer[];
    }
    interface IMatchingAnswer {
        answerLeft: string;
        answerRight: string;
        score: number;
        point: number;
    }

</script>
