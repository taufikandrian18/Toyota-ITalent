<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <!--Add-->
        <div class="row" v-if="mode == 'Add'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Multiple Choice > <span class="talent_font_red">Add</span></h3>
                <div class="mb-4"></div>


                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="justify-content-between align-items-center">
                            <checkbox v-model="isStoryLineType" class="talent_checkbox" style="line-height: 50%;" color="#0085CA" :size="16"> Type of Story Line</checkbox>
                        </div>
                    </div>
                    <div class="mb-4"></div>

                    <!--STORYLINE-->
                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="layoutType" type="radio" name="layout type" id="vertical" :disabled="isStoryLineType != true" value="1" v-validate="isStoryLineType == true ? 'required' : ''" /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="layoutType" type="radio" name="layout type" id="horizontal" :disabled="isStoryLineType != true" value="2" /> <label for="horizontal"><b>Type 2</b></label>
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
                                            <upload-file layout="TaskHorizontal" :formData.sync="formDataStoryLineHorizontal" :fileName.sync="fileNameStoryLineHorizontal" :isStoryLineType="isStoryLineType" name="picture" :layoutType="layoutType" v-validate="isStoryLineType == true && layoutType == 1 && ((textAreaHorizontal === null || textAreaHorizontal === '') && fileNameStoryLineHorizontal == null) ? 'required' : ''"></upload-file>
                                        </div>

                                    </div>
                                    <div class="mb-4"></div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea v-model="textAreaHorizontal" maxlength="5000" class="form-control talent_textarea" :disabled="isStoryLineType != true || layoutType != 1" name="description" v-validate="isStoryLineType == true &&  layoutType == 1 && (fileNameStoryLineHorizontal === null || fileNameStoryLineHorizontal === '') ? 'required' : ''"></textarea>
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
                                                    <upload-file layout="TaskVertical" :formData.sync="formDataStoryLineVertical" :fileName.sync="fileNameStoryLineVertical" :isStoryLineType="isStoryLineType" name="picture" :layoutType="layoutType" v-validate="isStoryLineType == true && layoutType == 2 && ((textAreaVertical === null || textAreaVertical === '') && fileNameStoryLineVertical == null) ? 'required' : ''"></upload-file>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5 ">
                                            <textarea v-model="textAreaVertical" maxlength="5000" class="form-control h-100 talent_overflowy" :disabled="isStoryLineType != true || layoutType != 2" name="description" v-validate="isStoryLineType == true &&  layoutType == 2 && (fileNameStoryLineVertical === null || fileNameStoryLineVertical === '') ? 'required' : ''"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="mb-4"></div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding">
                    <!--<div v-if="errorMessageShow == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                        {{errorMessage}}
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>-->
                    <div v-if="errorMessageShow == true && errors.items.length > 0" class="alert alert-danger alert-dismissible fade show" role="alert">

                        <div v-for="error in errors.items">
                            {{error.msg}}
                        </div>

                        <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <select v-model="TASKCODEFORVALIDATION" class="form-control" name="task code" @change.prevent="GenerateTaskCodeAndNumber()" v-validate="'required'">
                                        <option v-for="(competencyMapping,index) in allTaskCode"
                                                :value="competencyMapping.competencyTypeId + '-' + competencyMapping.competencyId + '-' + competencyMapping.evaluationTypeId
                                                ">
                                            {{competencyMapping.competencyTypeName.charAt(0)}}-{{competencyMapping.prefixCode}}-{{competencyMapping.evaluationTypeName}}
                                        </option>
                                    </select>
                                </div>
                                <div class="col-md-6">
                                    <h5>Number</h5>
                                    <input v-model="task.taskNumber" class="form-control" disabled />
                                </div>
                            </div>
                            <div class="mb-4"></div>
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>Module<span class="talent_font_red">*</span></h5>
                                    <select v-model="MODULE.moduleId" name="module" class="form-control" @change.prevent="getNumber()" v-validate="'required'">
                                        <option v-for="module in allModules" :value="module.moduleId">{{module.moduleName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-12 ">
                                    <h5>Question<span class="talent_font_red">*</span></h5>
                                </div>
                            </div>

                            <div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <input name="question" v-model="type.question" maxlength="2000" class="form-control" v-validate="'required'" />
                                    </div>
                                </div>
                            </div>
                            <div class="mb-4"></div>
                            <div class="row">
                                <div class="col-md-12 ">
                                    <h5>Options<span class="talent_font_red">*</span></h5>
                                </div>
                            </div>

                            <!--ForEach-->
                            <div v-for="(seq, index) in choice" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 text-center">
                                        {{alphabet[index]}}
                                    </div>
                                    <div class="col-md-11">
                                        <input maxlength="2000" :name="'options ' + (index + 1)" class="form-control form-control-sm" v-model="choice[index].text" v-validate="'required'" />
                                    </div>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <select v-model="ANSWERINT" class="form-control" name="answer" v-validate="'required'">
                                        <option :value="1">A</option>
                                        <option :value="2">B</option>
                                        <option :value="3">C</option>
                                        <option :value="4">D</option>
                                        <option :value="5">E</option>
                                    </select>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 text-center">
                                            <h5>Score<span class="talent_font_red">*</span></h5>
                                            <select class="form-control" v-model="SCOREFORVALIDATION" @change.prevent="GenerateScore()" name="score" v-validate="'required'">
                                                <option v-for="point in allPoints" :value="point.score">{{point.score}}</option>
                                            </select>
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h5>Point</h5>
                                            <input v-model="SCORE.points" class="form-control" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mb-4"></div>

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
                                            <input v-model="SCORE.score" class="form-control" disabled />
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
                                            <input v-model="SCORE.points" class="form-control" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="mb-4"></div>
                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="Close()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="addTask()">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Update-->
        <div class="row" v-if="mode == 'Edit'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Multiple Choice > <span class="talent_font_red">Edit</span></h3>
                <div class="mb-4"></div>


                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="justify-content-between align-items-center">
                            <checkbox v-model="isStoryLineType" class="talent_checkbox" style="line-height: 50%;" color="#0085CA" :size="16"> Type of Story Line</checkbox>
                        </div>
                    </div>
                    <div class="mb-4"></div>

                    <!--STORYLINE-->
                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="layoutType" type="radio" name="typeofstoryline" id="vertical" :disabled="isStoryLineType != true" value="1" /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="layoutType" type="radio" name="typeofstoryline" id="horizontal" :disabled="isStoryLineType != true" value="2" /> <label for="horizontal"><b>Type 2</b></label>
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
                                            <upload-file :updatedImage.sync="fileImageStoryLineHorizontalGet" :updatedName="fileNameStoryLineHorizontalGet" layout="TaskHorizontal" :formData.sync="formDataStoryLineHorizontal" :fileName.sync="fileNameStoryLineHorizontal" :isStoryLineType="isStoryLineType" :layoutType="layoutType"></upload-file>
                                            <!--<upload-file layout="TaskHorizontal" :formData.sync="formDataStoryLineHorizontal" :fileName.sync="fileNameStoryLineHorizontal" :isStoryLineType="isStoryLineType" :layoutType="layoutType"></upload-file>-->
                                        </div>

                                    </div>
                                    <div class="mb-4"></div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea v-model="textAreaHorizontal" maxlength="5000" class="form-control talent_textarea" :disabled="isStoryLineType != true || layoutType != 1"></textarea>
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
                                                    <upload-file layout="TaskVertical" :formData.sync="formDataStoryLineVertical" :fileName.sync="fileNameStoryLineVertical" :isStoryLineType="isStoryLineType" :layoutType="layoutType" :updatedImage.sync="fileImageStoryLineVerticalGet" :updatedName="fileNameStoryLineVerticalGet"></upload-file>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5 ">
                                            <textarea v-model="textAreaVertical" maxlength="5000" class="form-control h-100 talent_overflowy" :disabled="isStoryLineType != true || layoutType != 2"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="mb-4"></div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding">
                    <div v-if="errorMessageShow == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                        {{errorMessage}}
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <select v-model="TASKCODE" class="form-control" name="CompetencyMapping" @change.prevent="getNumber()">
                                        <option v-for="(competencyMapping,index) in allTaskCode" :value="competencyMapping">{{competencyMapping.competencyTypeName.charAt(0)}}-{{competencyMapping.prefixCode}}-{{competencyMapping.evaluationTypeName}}</option>
                                    </select>
                                </div>
                                <div class="col-md-6">
                                    <h5>Number</h5>
                                    <input v-model="task.taskNumber" class="form-control" disabled />
                                </div>
                            </div>
                            <div class="mb-4"></div>
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>Module<span class="talent_font_red">*</span></h5>
                                    <select v-model="MODULE.moduleId" class="form-control" @change.prevent="getNumber()">
                                        <option v-for="module in allModules" :value="module.moduleId">{{module.moduleName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-12 ">
                                    <h5>Question<span class="talent_font_red">*</span></h5>
                                </div>
                            </div>

                            <div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <input v-model="type.question" maxlength="2000" class="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="mb-4"></div>
                            <div class="row">
                                <div class="col-md-12 ">
                                    <h5>Options<span class="talent_font_red">*</span></h5>
                                </div>
                            </div>

                            <!--ForEach-->
                            <div v-for="(seq, index) in choice" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 text-center">
                                        {{alphabet[index]}}
                                    </div>
                                    <div class="col-md-11">
                                        <input maxlength="2000" class="form-control form-control-sm" v-model="choice[index].text" />
                                    </div>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <select v-model="ANSWERINT" class="form-control">
                                        <option :value="1">A</option>
                                        <option :value="2">B</option>
                                        <option :value="3">C</option>
                                        <option :value="4">D</option>
                                        <option :value="5">E</option>
                                    </select>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 text-center">
                                            <h5>Score<span class="talent_font_red">*</span></h5>
                                            <select class="form-control" v-model="SCORE">
                                                <option v-for="point in allPoints" :value="point">{{point.score}}</option>
                                            </select>
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h5>Point</h5>
                                            <input v-model="SCORE.points" class="form-control" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mb-4"></div>

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
                                            <input v-model="SCORE.score" class="form-control" disabled />
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
                                            <input v-model="SCORE.points" class="form-control" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="mb-4"></div>
                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="Close()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="updateTask()">
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
        <div class="row" v-if="mode == 'View'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Multiple Choice > <span class="talent_font_red">View Detail</span></h3>
                <div class="mb-4"></div>


                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="justify-content-between align-items-center">
                            <checkbox disabled v-model="isStoryLineType" class="talent_checkbox" style="line-height: 50%;" color="#0085CA" :size="16"> Type of Story Line</checkbox>
                        </div>
                    </div>
                    <div class="mb-4"></div>

                    <!--STORYLINE-->
                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="layoutType" type="radio" name="typeofstoryline" id="vertical" disabled value="1" /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="layoutType" type="radio" name="typeofstoryline" id="horizontal" disabled value="2" /> <label for="horizontal"><b>Type 2</b></label>
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
                                            <upload-file :updatedImage.sync="fileImageStoryLineHorizontalGet" :updatedName="fileNameStoryLineHorizontalGet" layout="TaskHorizontal" :formData.sync="formDataStoryLineHorizontal" :fileName.sync="fileNameStoryLineHorizontal" :isStoryLineType="isStoryLineType" :layoutType="layoutType" :disable="true"></upload-file>
                                            <!--<upload-file layout="TaskHorizontal" :formData.sync="formDataStoryLineHorizontal" :fileName.sync="fileNameStoryLineHorizontal" :isStoryLineType="isStoryLineType" :layoutType="layoutType"></upload-file>-->
                                        </div>

                                    </div>
                                    <div class="mb-4"></div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea v-model="textAreaHorizontal" class="form-control talent_textarea" disabled></textarea>
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
                                                    <upload-file layout="TaskVertical" :formData.sync="formDataStoryLineVertical" :fileName.sync="fileNameStoryLineVertical" :isStoryLineType="isStoryLineType" :layoutType="layoutType" :updatedImage.sync="fileImageStoryLineVerticalGet" :updatedName="fileNameStoryLineVerticalGet" :disable="true"></upload-file>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5 ">
                                            <textarea v-model="textAreaVertical" class="form-control h-100 talent_overflowy" disabled></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="mb-4"></div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding">
                    <div v-if="errorMessageShow == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                        {{errorMessage}}
                        <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <select disabled v-model="TASKCODE" class="form-control" name="CompetencyMapping" @change.prevent="getNumber()">
                                        <option v-for="(competencyMapping,index) in allTaskCode" :value="competencyMapping">{{competencyMapping.competencyTypeName.charAt(0)}}-{{competencyMapping.prefixCode}}-{{competencyMapping.evaluationTypeName}}</option>
                                    </select>
                                </div>
                                <div class="col-md-6">
                                    <h5>Number</h5>
                                    <input v-model="task.taskNumber" class="form-control" disabled />
                                </div>
                            </div>
                            <div class="mb-4"></div>
                            <div class="row">
                                <div class="col-md-12">
                                    <h5>Module<span class="talent_font_red">*</span></h5>
                                    <select disabled v-model="MODULE.moduleId" class="form-control" @change.prevent="getNumber()">
                                        <option v-for="module in allModules" :value="module.moduleId">{{module.moduleName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-12 ">
                                    <h5>Question<span class="talent_font_red">*</span></h5>
                                </div>
                            </div>

                            <div>
                                <div class="row">
                                    <div class="col-md-12">
                                        <input disabled v-model="type.question" class="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="mb-4"></div>
                            <div class="row">
                                <div class="col-md-12 ">
                                    <h5>Options<span class="talent_font_red">*</span></h5>
                                </div>
                            </div>

                            <!--ForEach-->
                            <div v-for="(seq, index) in choice" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 text-center">
                                        {{alphabet[index]}}
                                    </div>
                                    <div class="col-md-11">
                                        <input disabled maxlength="2000" class="form-control form-control-sm" v-model="choice[index].text" />
                                    </div>
                                </div>
                            </div>


                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <select disabled v-model="ANSWERINT" class="form-control">
                                        <option :value="1">A</option>
                                        <option :value="2">B</option>
                                        <option :value="3">C</option>
                                        <option :value="4">D</option>
                                        <option :value="5">E</option>
                                    </select>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 text-center">
                                            <h5>Score<span class="talent_font_red">*</span></h5>
                                            <select disabled class="form-control" v-model="SCORE">
                                                <option v-for="point in allPoints" :value="point">{{point.score}}</option>
                                            </select>
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h5>Point</h5>
                                            <input v-model="SCORE.points" class="form-control" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="mb-4"></div>

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
                                            <input v-model="SCORE.score" class="form-control" disabled />
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
                                            <input v-model="SCORE.points" class="form-control" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="mb-4"></div>
                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white" @click.prevent="backPage()">
                                        Close
                                    </button>
                                    <button v-else class="btn talent_bg_red talent_font_white" @click.prevent="Close()">
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
    import { BlobService } from '../../../services/BlobService';
    //import { TaskMan, CompetencyMappingJoinModel, TimePointTaskModel, ModuleForTaskModel } from '../../../services/TaskService'
    //import { SetupTimePointMan } from '../../../services/SetupTimePointService'
    //import { TaskMultipleChoiceMan, TaskInsertModel, TaskMultipleChoiceTypeModel, TaskMultipleChoiceChoiceModel, TaskMultipleChoiceModel } from '../../../services/TaskMultipleChoiceService'
    import {
        TaskService, SetupTimePointService, MultipleChoiceService, CompetencyMappingJoinModel, TimePointTaskModel, ModuleForTaskModel,
        TaskInsertModel, TaskMultipleChoiceTypeModel, TaskMultipleChoiceChoiceModel, TaskMultipleChoiceModel
    } from '../../../services/NSwagService'
    import VeeValidate from 'vee-validate'
    import IFileContent from '../../../models/IFileContent';
    import { faAd } from '@fortawesome/free-solid-svg-icons';

    Vue.use(VeeValidate);

    @Component({
        props: ['mode', 'taskId', 'fromOutside'],
        created: async function (this: MultipleChoice) {
            await this.initialize();
            //if (this.mode == 'Add') {
            //    this.allPoints = await this.TaskApi.getAllTimePointsForTask();
            //}
            if (this.mode == 'Edit' || this.mode == 'View') {
                this.isBusy = true
                await this.InitialGetData();
            }
        }
    })
    export default class MultipleChoice extends Vue {
        //API
        MultipleChoiceAPI: MultipleChoiceService = new MultipleChoiceService;
        TimePointAPI: SetupTimePointService = new SetupTimePointService;
        TaskApi: TaskService = new TaskService;

        //LOADING OVERLAY
        isBusy: boolean = false;

        //VARIABLE INSERT
        task: TaskInsertModel = {
            blobId: null, competencyId: null, evaluationTypeId: null, layoutType: null,
            moduleId: null, questionTypeId: null, storyLineDescription: null, taskId: 0, taskNumber: null, fileContent: null
        }
        type: TaskMultipleChoiceTypeModel = {
            taskId: 0, question: null, score: null, answerId: 0

        }
        choice: TaskMultipleChoiceChoiceModel[] = [
            { number: 1, taskId: 0, taskMultipleChoiceChoiceId: 0, text: null },
            { number: 2, taskId: 0, taskMultipleChoiceChoiceId: 0, text: null },
            { number: 3, taskId: 0, taskMultipleChoiceChoiceId: 0, text: null },
            { number: 4, taskId: 0, taskMultipleChoiceChoiceId: 0, text: null },
            { number: 5, taskId: 0, taskMultipleChoiceChoiceId: 0, text: null }
        ]; //Variable yang dipke untuk parameter API
        choiceHelper: TaskMultipleChoiceChoiceModel[] = []; //Variable Pembantu
        insert: TaskMultipleChoiceModel = { task: this.task, type: this.type, choice: this.choice }

        //VARIABLE MODE
        mode: string;
        taskId: number;
        fromOutside: boolean;

        //VARIABLE INITIALIZE
        allTaskCode: Array<CompetencyMappingJoinModel> = [];
        allModules: Array<ModuleForTaskModel> = [];
        allPoints: Array<TimePointTaskModel> = [];

        //VARIABLE STORYLINE
        isStoryLineType: boolean = false;
        layoutType: number = null;
        textAreaHorizontal: string = null;
        textAreaVertical: string = null;

        //VARIABLE UPLOAD IMAGE
        //formDataStoryLineHorizontal: FormData = new FormData();
        formDataStoryLineHorizontal: IFileContent = {
            base64: "",
            contentType: "",
            fileName: ""
        };
        fileNameStoryLineHorizontal: string = null;
        //formDataStoryLineVertical: FormData = new FormData();
        formDataStoryLineVertical: IFileContent = {
            base64: "",
            contentType: "",
            fileName: ""
        };
        fileNameStoryLineVertical: string = null;

        //VARIABLE UPLOAD IMAGE STORYLINE
        fileImageStoryLineHorizontalGet: string = '';
        fileNameStoryLineHorizontalGet: string = '';
        fileImageStoryLineVerticalGet: string = '';
        fileNameStoryLineVerticalGet: string = '';

        //VARIABLE TASK ADDITIONAL
        TASKCODE: CompetencyMappingJoinModel = {
            competencyId: null, competencyTypeId: null, prefixCode: null,
            competencyTypeName: null, evaluationTypeId: null, evaluationTypeName: null, competencyNameMapping: null
        }
        TASKCODEFORVALIDATION: string = null;

        //MENCARI TASKCODE
        async GenerateTaskCode() {
            var code = this.TASKCODEFORVALIDATION.split("-", 3);
            for (var i of this.allTaskCode) {
                if (i.competencyTypeId == parseInt(code[0]) && i.competencyId == parseInt(code[1]) && i.evaluationTypeId == parseInt(code[2])) {
                    this.TASKCODE = i;
                    return
                }
            }
        }

        //2FUNCTION
        async GenerateTaskCodeAndNumber() {
            await this.GenerateTaskCode().then(() => {
                this.getNumber()
            })

        }


        MODULE: ModuleForTaskModel = { moduleId: null, moduleName: null }

        //VARIABLE TYPE ADDITIONAL
        ANSWERINT: number = null;
        SCORE: TimePointTaskModel = { points: null, score: null, timePointId: null }
        SCOREFORVALIDATION: number = null;

        //MENCARI SCORE
        GenerateScore() {
            for (var p of this.allPoints) {
                if (p.score === this.SCOREFORVALIDATION) {
                    this.SCORE = p
                    return
                }
            }
        }

        //VARIABLE ALERT
        errorMessageShow: boolean = false;
        errorMessage: string = "";

        //VARIABLE TOTAL
        totalScore: number = 0;
        totalPoint: number = 0;

        //SOURCE OF TRUTH
        sourceCompetencyId: number = null;
        sourceEvaluationTypeId: number = null;
        sourceModuleId: number = null;
        sourceTaskNumber: number = null;

        async initialize() {
            this.isBusy = true
            this.allTaskCode = await this.TaskApi.getAllCompetencies();
            this.allModules = await this.TaskApi.getAllModulesForTask();
            this.allPoints = await this.TaskApi.getAllTimePointsForTask();
            this.isBusy = false
        }

        //GETTASKNUMBER
        async getNumber() {

            if (this.mode == "update") {
                //VALIDASI JIKA UPDATE DAN TASKCODE & MODULE TIDAK DIUBAH
                if (this.TASKCODE.competencyId == this.sourceCompetencyId && this.TASKCODE.evaluationTypeId == this.sourceEvaluationTypeId && this.MODULE.moduleId == this.sourceModuleId
                ) {
                    this.task.taskNumber = this.sourceTaskNumber;
                    return
                }
            }
            this.task.competencyId = this.TASKCODE.competencyId;
            this.task.evaluationTypeId = this.TASKCODE.evaluationTypeId;
            this.task.taskNumber = await this.TaskApi.getNumber(this.TASKCODE.competencyId, this.MODULE.moduleId, this.TASKCODE.evaluationTypeId);

        }

        //UPLOADIMAGE
        UploadFile(formData: FormData) {
            return BlobService.uploadService(formData);
        }

        //RESET ERROR MESSAGE
        ResetErrorMessage() {
            this.errorMessageShow = false;
        }

        async addTask() {
            this.errorMessage = '';
            this.errorMessageShow = false;
            this.isBusy = true
            let valid = await this.$validator.validateScopes();
            if (!valid) {
                this.errorMessageShow = true;
                this.isBusy = false
                return;
            }
            this.$validator.reset;

            //VALIDASI LAYOUT 1
            if (this.isStoryLineType == true && this, this.layoutType == 1) {

                if ((this.textAreaHorizontal == null || this.textAreaHorizontal == '') && (this.formDataStoryLineHorizontal.fileName == null || this.formDataStoryLineHorizontal.fileName == '')) {
                    this.errorMessage = "Storyline or Image must be filled"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }

                if (this.textAreaHorizontal != null) {
                    if (this.textAreaHorizontal.length > 5000) {
                        this.errorMessage = "Storyline description must be less than 5000 characters";
                        this.errorMessageShow = true;
                        this.isBusy = false
                        return
                    }
                }
                this.task.storyLineDescription = this.textAreaHorizontal;
                this.task.layoutType = 1
                if (this.fileNameStoryLineHorizontal != null) {
                    this.task.fileContent = this.formDataStoryLineHorizontal;
                }
            }
            //VALIDASI LAYOUT 2
            if (this.isStoryLineType == true && this, this.layoutType == 2) {

                if ((this.textAreaVertical == null || this.textAreaVertical == '') && (this.formDataStoryLineVertical.fileName == null || this.formDataStoryLineVertical.fileName == '')) {
                    this.errorMessage = "Storyline or Image must be filled"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }

                if (this.textAreaVertical != null) {
                    if (this.textAreaVertical.length > 5000) {
                        this.errorMessage = "Storyline description must be less than 5000 characters";
                        this.errorMessageShow = true;
                        this.isBusy = false
                        return
                    }
                }
                this.task.storyLineDescription = this.textAreaVertical;
                this.task.layoutType = 2
                if (this.fileNameStoryLineVertical != null) {
                    this.task.fileContent = this.formDataStoryLineVertical
                }
            }
            //VALIDASI LAYOUT 0
            if (this.isStoryLineType == false) {
                this.task.storyLineDescription = null;
                this.task.layoutType = 0;
                this.task.blobId = null;
            }

            //INSERT TASK
            this.task.taskId = 0; //SEMENTARA AJA
            this.task.questionTypeId = 4;


            this.task.competencyId = this.TASKCODE.competencyId;
            this.task.evaluationTypeId = this.TASKCODE.evaluationTypeId;
            this.task.moduleId = this.MODULE.moduleId;


            //INSERT TYPE
            //this.type.score = this.SCORE.score;
            this.type.score = this.SCORE.timePointId;

            //INSERT PICTURE
            let index = 0;

            this.insert.task.blobId = this.task.blobId;
            this.insert.task.competencyId = this.task.competencyId;
            this.insert.task.evaluationTypeId = this.task.evaluationTypeId;
            this.insert.task.layoutType = this.task.layoutType;
            this.insert.task.moduleId = this.task.moduleId;
            this.insert.task.questionTypeId = this.task.questionTypeId;
            this.insert.task.storyLineDescription = this.task.storyLineDescription;
            this.insert.task.taskId = this.task.taskId;
            this.insert.task.taskNumber = this.task.taskNumber;
            this.insert.task.fileContent = this.task.fileContent

            this.insert.type.taskId = this.type.taskId;
            this.insert.type.answerId = this.ANSWERINT;
            this.insert.type.question = this.type.question;
            //this.insert.type.score = this.SCORE.score;
            this.insert.type.score = this.SCORE.timePointId;

            //INSERT TO DB
            await this.MultipleChoiceAPI.insertTaskMultipleChoice(this.insert);
            window.location.href = "/Setup/Tasks"
            this.isBusy = false
        }


        //Score
        taskScoreGet: number[] = []

        async InitialGetData() {
            this.isBusy = true
            this.task = await this.TaskApi.getTaskById(this.taskId);
            this.type = await this.MultipleChoiceAPI.getTaskMultipleChoiceTypeById(this.taskId);


            //Score
            this.taskScoreGet.push(this.type.score);
            this.allPoints = await this.TaskApi.getAllTimePointsForTaskUpdate(this.taskScoreGet);

            //this.type = tempType

            //VARIABLE UNTUK VALIDASI
            this.sourceCompetencyId = this.task.competencyId;
            this.sourceEvaluationTypeId = this.task.evaluationTypeId;
            this.sourceModuleId = this.task.moduleId;
            this.sourceTaskNumber = this.task.taskNumber;

            this.choiceHelper = await this.MultipleChoiceAPI.getTaskMultipleChoiceChoiceById(this.taskId);
            //variable pembantu untuk generate taskcode
            var taskCodeHelper = await this.TaskApi.getTaskCodeByTaskId(this.taskId);
            //Variable Pembantu Untuk Generate ImageStoryLine
            var storyLineImageHelper = await BlobService.getBlobById(this.task.blobId);

            //Generate Task Code
            this.TASKCODE.competencyId = taskCodeHelper.competencyId
            this.TASKCODE.prefixCode = taskCodeHelper.prefixCode
            this.TASKCODE.evaluationTypeName = taskCodeHelper.evaluationTypeName
            this.TASKCODE.evaluationTypeId = taskCodeHelper.evaluationTypeId
            this.TASKCODE.competencyTypeId = taskCodeHelper.competencyTypeId
            this.TASKCODE.competencyTypeName = taskCodeHelper.competencyTypeName
            this.TASKCODE.competencyNameMapping = null

            //Generate Module
            this.MODULE.moduleId = this.task.moduleId;

            //Generate StoryLine
            if (this.task.layoutType != 0) {

                this.layoutType = this.task.layoutType;
                if (this.task.layoutType == 1) {
                    this.textAreaHorizontal = this.task.storyLineDescription;
                    if (storyLineImageHelper.blobId != null) {
                        this.fileImageStoryLineHorizontalGet = await BlobService.getImageUrl(storyLineImageHelper.blobId, storyLineImageHelper.mime)
                        this.fileNameStoryLineHorizontalGet = storyLineImageHelper.name;
                    }
                    this.isStoryLineType = true;
                }
                if (this.task.layoutType == 2) {
                    this.textAreaVertical = this.task.storyLineDescription;
                    if (storyLineImageHelper.blobId != null) {
                        this.fileImageStoryLineVerticalGet = await BlobService.getImageUrl(storyLineImageHelper.blobId, storyLineImageHelper.mime)
                        this.fileNameStoryLineVerticalGet = storyLineImageHelper.name;
                    }
                    this.isStoryLineType = true;
                }
            } else {
                this.isStoryLineType = false;
            }
            var index = 0;
            //Generate Image URL
            for (let p of this.choiceHelper) {
                this.choice[index].taskId = p.taskId
                this.choice[index].number = p.number
                this.choice[index].taskMultipleChoiceChoiceId = p.taskMultipleChoiceChoiceId
                this.choice[index].text = p.text
                index++;
            }

            //SCORE
            this.SCORE = await this.TimePointAPI.getSetupTimePointByScoreAsync(this.type.score);
            //ANSWER
            this.ANSWERINT = Number(this.type.answerId);
            this.isBusy = false
        }

        async updateTask() {
            //VALIDASI TASK
            this.errorMessage = '';
            this.errorMessageShow = false;
            this.isBusy = true

            //VALIDASI TASK CODE
            if (this.TASKCODE.competencyId == null || this.TASKCODE.evaluationTypeId == null || this.TASKCODE.competencyTypeId == null) {
                this.errorMessage = "Task Code must be selected!";
                this.errorMessageShow = true;
                this.isBusy = false
                return
            }
            if (this.MODULE.moduleId == null) {
                this.errorMessage = "Module must be selected!";
                this.errorMessageShow = true;
                this.isBusy = false
                return
            }

            //VALIDASI TYPE
            //VALIDASI QUESTION
            if (this.type.question == null || (this.type.question).toString() == '') {
                this.errorMessage = "Question must be filled!";
                this.errorMessageShow = true;
                this.isBusy = false
                return
            }

            if (this.type.question.length > 2000) {
                this.errorMessage = "Question must be less than 2000 characters";
                this.errorMessageShow = true;
                this.isBusy = false
                return
            }

            //VALIDASI SCORE
            if (this.SCORE.score == null) {
                this.errorMessage = "Score must be selected!";
                this.errorMessageShow = true;
                this.isBusy = false
                return
            }

            //VALIDASI CHOICE
            for (let f of this.choice) {
                if (f.text == null || f.text == '') {
                    this.errorMessage = "Options must be filled!"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }
            }

            //VALIDASI LAYOUT 1
            if (this.isStoryLineType == true && this, this.layoutType == 1) {

                if ((this.textAreaHorizontal == null || this.textAreaHorizontal == '') && (this.formDataStoryLineHorizontal.fileName == null || this.formDataStoryLineHorizontal.fileName == '')) {
                    this.errorMessage = "Storyline or Image must be filled"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }


                if (this.textAreaHorizontal != null) {
                    if (this.textAreaHorizontal.length > 5000) {
                        this.errorMessage = "Storyline description must be less than 5000 characters";
                        this.errorMessageShow = true;
                        this.isBusy = false
                        return
                    }
                }
                this.task.storyLineDescription = this.textAreaHorizontal;
                this.task.layoutType = 1
                if (this.fileNameStoryLineHorizontal != null && (this.formDataStoryLineHorizontal.fileName != null || this.formDataStoryLineHorizontal.fileName != '')) {
                    this.task.fileContent = this.formDataStoryLineHorizontal;
                }
                else if (this.fileImageStoryLineHorizontalGet != '') {
                    this.task.blobId = this.task.blobId;
                }
                else {
                    this.task.blobId = null;
                }

            }
            //VALIDASI LAYOUT 2
            if (this.isStoryLineType == true && this, this.layoutType == 2) {

                if ((this.textAreaVertical == null || this.textAreaVertical == '') && (this.formDataStoryLineVertical.fileName == null || this.formDataStoryLineVertical.fileName == '')) {
                    this.errorMessage = "Storyline or Image must be filled"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }
                if (this.textAreaVertical != null) {
                    if (this.textAreaVertical.length > 5000) {
                        this.errorMessage = "Storyline description must be less than 5000 characters";
                        this.errorMessageShow = true;
                        this.isBusy = false
                        return
                    }
                }
                this.task.storyLineDescription = this.textAreaVertical;
                this.task.layoutType = 2
                if (this.fileNameStoryLineVertical != null && (this.formDataStoryLineVertical.fileName != null || this.formDataStoryLineVertical.fileName != '')) {
                    this.task.fileContent = this.formDataStoryLineVertical;
                }
                else if (this.fileImageStoryLineVerticalGet != '') {
                    this.task.blobId = this.task.blobId;
                }
                else {
                    this.task.blobId = null;
                }
            }

            //VALIDASI LAYOUT 0
            if (this.isStoryLineType == false) {
                this.task.storyLineDescription = null;
                this.task.layoutType = 0;
                this.task.blobId = null;
            }
            //INSERT TASK
            this.task.taskId = 0; //SEMENTARA AJA
            this.task.questionTypeId = 9;
            this.task.competencyId = this.TASKCODE.competencyId;
            this.task.evaluationTypeId = this.TASKCODE.evaluationTypeId;
            this.task.moduleId = this.MODULE.moduleId;

            //INSERT TYPE
            //this.type.score = this.SCORE.score;
            this.type.score = this.SCORE.timePointId;



            //TASK
            this.insert.task.blobId = this.task.blobId;
            this.insert.task.competencyId = this.task.competencyId;
            this.insert.task.evaluationTypeId = this.task.evaluationTypeId;
            this.insert.task.layoutType = this.task.layoutType;
            this.insert.task.moduleId = this.task.moduleId;
            this.insert.task.questionTypeId = this.task.questionTypeId;
            this.insert.task.storyLineDescription = this.task.storyLineDescription;
            this.insert.task.taskId = this.task.taskId
            this.insert.task.taskNumber = this.task.taskNumber;
            this.insert.task.fileContent = this.task.fileContent;

            this.insert.type.question = this.type.question;
            //this.insert.type.score = this.SCORE.score;
            this.insert.type.score = this.SCORE.timePointId;
            this.insert.type.answerId = this.ANSWERINT;

            var index = 0;
            for (let f of this.choice) {
                f.number = index + 1;
                index++;
            }

            await this.MultipleChoiceAPI.updateTaskMultipleChoice(this.taskId, this.insert);
            window.location.href = "/Setup/Tasks"
            this.isBusy = false
        }

        //CLOSE
        Close() {
            window.location.href = "/Setup/Tasks"
        }

        backPage() {
            window.history.back();
        }

        alphabet: Array<string> = ["A", "B", "C", "D", "E"]


    }
</script>
