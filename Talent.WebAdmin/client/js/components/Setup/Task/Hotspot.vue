<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <!--ADD-->
        <div class="row" v-if="mode == 'Add'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Hotspot > <span class="talent_font_red">Add</span></h3>
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
                                            <upload-file layout="TaskHorizontal" :formData.sync="formDataStoryLineHorizontal" :fileName.sync="fileNameStoryLineHorizontal" :isStoryLineType="isStoryLineType" :layoutType="layoutType"></upload-file>
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
                                                    <upload-file layout="TaskVertical" :formData.sync="formDataStoryLineVertical" :fileName.sync="fileNameStoryLineVertical" :isStoryLineType="isStoryLineType" :layoutType="layoutType"></upload-file>
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
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <input v-model="type.question" maxlength="2000" class="form-control" />
                            <div class="mb-4"></div>
                            <upload-file layout="TaskHorizontal" :formData.sync="formDataQuestion" :fileName.sync="fileNameQuestion" :isStoryLineType="true" :layoutType="1"></upload-file>
                            <div class="mb-4"></div>

                            <div class="row">
                                <div class="col-md-1">

                                </div>
                                <div class="col-md-6">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 text-center">
                                            <h5>Score<span class="talent_font_red">*</span></h5>
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h5>Point</h5>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-1"></div>
                            </div>

                            <!--ForEach-->
                            <div v-for="(option, index) in choice" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 text-center">
                                        {{index + 1}}
                                    </div>
                                    <div class="col-md-6">
                                        <input v-model="choice[index].answer" class="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-6 text-center">
                                                <select class="form-control" v-model="SCORE[index]" @change.prevent="SumScore(), SumPoint()">
                                                    <option v-for="point in allPoints" :value="point">{{point.score}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-6 text-center">
                                                <input v-model="SCORE[index].points" class="form-control" disabled />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <div v-if="index != choice.length - 1" class="text-center" @click.prevent="remove(index)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                        <div v-if="index == choice.length - 1" class="text-center" @click.prevent="add()"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                    </div>
                                </div>
                            </div>
                            <div class="mb-4"></div>


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
                                            <input v-model="totalScore" class="form-control" disabled />
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
                                            <input v-model="totalPoint" class="form-control" disabled />
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
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="CloseTask()">
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

        <!--EDIT-->
        <div class="row" v-if="mode == 'Edit'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Hotspot > <span class="talent_font_red">Edit</span></h3>
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
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <input v-model="type.question" maxlength="2000" class="form-control" />
                            <div class="mb-4"></div>
                            <upload-file layout="TaskHorizontal" :formData.sync="formDataQuestion" :fileName.sync="fileNameQuestion" :isStoryLineType="imageQuestionShow" :layoutType="1" :updatedImage.sync="fileImageQuestionGet" :updatedName="fileNameQuestionGet"></upload-file>
                            <div class="mb-4"></div>

                            <div class="row">
                                <div class="col-md-1">

                                </div>
                                <div class="col-md-6">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 text-center">
                                            <h5>Score<span class="talent_font_red">*</span></h5>
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h5>Point</h5>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-1"></div>
                            </div>

                            <!--ForEach-->
                            <div v-for="(option, index) in choice" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 text-center">
                                        {{index + 1}}
                                    </div>
                                    <div class="col-md-6">
                                        <input v-model="choice[index].answer" class="form-control" />
                                    </div>
                                    <div class="col-md-4">
                                        <div class="row">
                                            <div class="col-md-6 text-center">
                                                <select class="form-control" v-model="SCORE[index]" @change.prevent="SumScore(), SumPoint()">
                                                    <option v-for="point in allPoints" :value="point">{{point.score}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-6 text-center">
                                                <input v-model="SCORE[index].points" class="form-control" disabled />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <div v-if="index != choice.length - 1" class="text-center" @click.prevent="remove(index)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                        <div v-if="index == choice.length - 1" class="text-center" @click.prevent="add()"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                    </div>
                                </div>
                            </div>
                            <div class="mb-4"></div>


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
                                            <input v-model="totalScore" class="form-control" disabled />
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
                                            <input v-model="totalPoint" class="form-control" disabled />
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
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="CloseTask()">
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

        <!--VIEW-->
        <div class="row" v-if="mode == 'View'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Hotspot > <span class="talent_font_red">View Detail</span></h3>
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
                                            <upload-file :disable="true" :updatedImage.sync="fileImageStoryLineHorizontalGet" :updatedName="fileNameStoryLineHorizontalGet" layout="TaskHorizontal" :formData.sync="formDataStoryLineHorizontal" :fileName.sync="fileNameStoryLineHorizontal" :isStoryLineType="isStoryLineType" :layoutType="layoutType"></upload-file>
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
                                                    <upload-file :disable="true" layout="TaskVertical" :formData.sync="formDataStoryLineVertical" :fileName.sync="fileNameStoryLineVertical" :isStoryLineType="isStoryLineType" :layoutType="layoutType" :updatedImage.sync="fileImageStoryLineVerticalGet" :updatedName="fileNameStoryLineVerticalGet"></upload-file>
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
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <input disabled v-model="type.question" maxlength="2000" class="form-control" />
                            <div class="mb-4"></div>
                            <upload-file layout="TaskHorizontal" :formData.sync="formDataQuestion" :fileName.sync="fileNameQuestion" :isStoryLineType="imageQuestionShow" :layoutType="1" :updatedImage.sync="fileImageQuestionGet" :updatedName="fileNameQuestionGet" :disable="true"></upload-file>
                            <div class="mb-4"></div>

                            <div class="row">
                                <div class="col-md-1">

                                </div>
                                <div class="col-md-6">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-5">
                                    <div class="row">
                                        <div class="col-md-6 text-center">
                                            <h5>Score<span class="talent_font_red">*</span></h5>
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h5>Point</h5>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!--ForEach-->
                            <div v-for="(option, index) in choice" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 text-center">
                                        {{index + 1}}
                                    </div>
                                    <div class="col-md-6">
                                        <input v-model="choice[index].answer" disabled class="form-control" />
                                    </div>
                                    <div class="col-md-5">
                                        <div class="row">
                                            <div class="col-md-6 text-center">
                                                <select disabled class="form-control" v-model="SCORE[index]" @change.prevent="SumScore(), SumPoint()">
                                                    <option disabled v-for="point in allPoints" :value="point">{{point.score}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-6 text-center">
                                                <input v-model="SCORE[index].points" class="form-control" disabled />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="mb-4"></div>


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
                                            <input v-model="totalScore" class="form-control" disabled />
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
                                            <input v-model="totalPoint" class="form-control" disabled />
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
                                    <button v-else class="btn talent_bg_red talent_font_white" @click.prevent="CloseTask()">
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
    import {
        TaskService, SetupTimePointService, HotSpotService, CompetencyMappingJoinModel, TimePointTaskModel,
        ModuleForTaskModel, TaskInsertModel, TaskHotSpotModel, TaskHotSpotTypeModel, TaskHotSpotAnswerModel
    } from '../../../services/NSwagService';
    import Message from '../../../class/Message';
    import IFileContent from '../../../models/IFileContent';

    @Component({
        created: async function (this: Hotspot) {
            await this.initialize();
            if (this.mode == 'Add') {
                this.SCORE.push({ points: null, score: null, timePointId: null });
                this.choice.push({ answer: null, score: null, taskHotSpotAnswerId: 0, taskId: 0, number: null });
            }
            if (this.mode == 'Edit' || this.mode == 'View') {
                await this.InitialGetData();
                this.SumScore();
                this.SumPoint();
            }
        },
        props: ['mode', 'taskId', 'fromOutside']
    })
    export default class Hotspot extends Vue {
        //LOADING OVERLAY
        isBusy: boolean = false;

        //API
        HotSpotAPI: HotSpotService = new HotSpotService;
        TimePointAPI: SetupTimePointService = new SetupTimePointService;
        TaskApi: TaskService = new TaskService;

        //VARIABLE INSERT
        task: TaskInsertModel = {
            blobId: null, competencyId: null, evaluationTypeId: null, layoutType: null,
            moduleId: null, questionTypeId: null, storyLineDescription: null, taskId: 0, taskNumber: null
        }
        type: TaskHotSpotTypeModel = {
            taskId: 0, question: null, blobId: null, fileContent: null
        }
        choice: TaskHotSpotAnswerModel[] = []; //Variable yang dipke untuk parameter API
        choiceHelper: TaskHotSpotAnswerModel[] = []; //Variable Pembantu
        insert: TaskHotSpotModel = { task: this.task, type: this.type, choice: this.choice }

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
        layoutType: number = 0;
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
        //formDataQuestion: FormData = new FormData();
        formDataQuestion: IFileContent = {
            base64: "",
            contentType: "",
            fileName: ""
        };
        fileNameQuestion: string = null;

        //VARIABLE UPLOAD IMAGE STORYLINE
        fileImageStoryLineHorizontalGet: string = '';
        fileNameStoryLineHorizontalGet: string = '';
        fileImageStoryLineVerticalGet: string = '';
        fileNameStoryLineVerticalGet: string = '';
        fileImageQuestionGet: string = '';
        fileNameQuestionGet: string = '';
        imageQuestionShow: boolean = false;

        //VARIABLE TASK ADDITIONAL
        TASKCODE: CompetencyMappingJoinModel = {
            competencyId: null, competencyTypeId: null, prefixCode: null,
            competencyTypeName: null, evaluationTypeId: null, evaluationTypeName: null, competencyNameMapping: null
        }
        MODULE: ModuleForTaskModel = { moduleId: null, moduleName: null }

        //VARIABLE CHOICE ADDITIONAL
        SCORE: TimePointTaskModel[] = []

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

        //ADD
        async addTask() {
            this.isBusy = true
            //VALIDASI TASK
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

            if (this.fileNameQuestion == null) {
                this.errorMessage = "Question Image must be Uploaded!";
                this.errorMessageShow = true;
                this.isBusy = false
                return
            }

            //VALIDASI CHOICE
            for (let f of this.choice) {
                //if (f.answer == null || f.answer == '') {
                if (f.answer == null) {
                    this.errorMessage = "Answer must be filled!"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }
            }

            for (let f of this.SCORE) {
                //if (f.answer == null || f.answer == '') {
                if (f.score == null) {
                    this.errorMessage = "Score must be filled!"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }
            }

            //VALIDASI LAYOUT 1
            if (this.isStoryLineType == true && this.layoutType == 1) {

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
                        this.isBusy = false;
                    }
                }
                this.task.storyLineDescription = this.textAreaHorizontal;
                this.task.layoutType = 1
                if (this.fileNameStoryLineHorizontal != null) {
                    this.task.fileContent = this.formDataStoryLineHorizontal;
                }
            }
            //VALIDASI LAYOUT 2
            if (this.isStoryLineType == true && this.layoutType == 2) {

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
                        this.isBusy = false;
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
            this.task.questionTypeId = 5;
            this.task.competencyId = this.TASKCODE.competencyId;
            this.task.evaluationTypeId = this.TASKCODE.evaluationTypeId;
            this.task.moduleId = this.MODULE.moduleId;

            //TYPE UPLOAD FILE
            //await this.UploadFile(this.formDataQuestion).then((onResult) => {
            //    this.type.blobId = onResult
            //});
            this.type.fileContent = this.formDataQuestion;


            var index = 0;
            for (let f of this.SCORE) {
                //this.insert.choice[index].score = f.score
                this.insert.choice[index].score = f.timePointId
                index++;
            }

            //NNTI DI BUANG BUAT BIAR MASUK AJA KE DB SOALNYA ANSWER DI DB ITU INT HARUNYS VARCHAR
            index = 0;
            for (let f of this.choice) {
                this.insert.choice[index].answer = f.answer;
                this.insert.choice[index].number = index + 1;
                index++;
            }

            await this.HotSpotAPI.insertTaskHotspot(this.insert);
            window.location.href = "/Setup/Tasks";
            this.isBusy = false
        }

        //Score
        taskScoreGet: number[] = []

        //INITAL GET DATA
        async InitialGetData() {
            this.isBusy = true;
            this.task = await this.TaskApi.getTaskById(this.taskId);
            this.type = await this.HotSpotAPI.getTaskHotspotTypeById(this.taskId);



            //VARIABLE UNTUK VALIDASI
            this.sourceCompetencyId = this.task.competencyId;
            this.sourceEvaluationTypeId = this.task.evaluationTypeId;
            this.sourceModuleId = this.task.moduleId;
            this.sourceTaskNumber = this.task.taskNumber;

            this.choiceHelper = await this.HotSpotAPI.getTaskHotspotChoiceById(this.taskId);
            //variable pembantu untuk generate taskcode
            var taskCodeHelper = await this.TaskApi.getTaskCodeByTaskId(this.taskId);
            //Variable Pembantu Untuk Generate ImageStoryLine
            var storyLineImageHelper = await BlobService.getBlobById(this.task.blobId);
            //Variable Pembantu Untuk Generate QuestionImage
            var questionImageHelper = await BlobService.getBlobById(this.type.blobId);


            //Generate Task Code
            this.TASKCODE.competencyId = taskCodeHelper.competencyId
            this.TASKCODE.prefixCode = taskCodeHelper.prefixCode
            this.TASKCODE.evaluationTypeName = taskCodeHelper.evaluationTypeName
            this.TASKCODE.evaluationTypeId = taskCodeHelper.evaluationTypeId
            this.TASKCODE.competencyTypeId = taskCodeHelper.competencyTypeId
            this.TASKCODE.competencyTypeName = taskCodeHelper.competencyTypeName

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

            this.fileImageQuestionGet = await BlobService.getImageUrl(questionImageHelper.blobId, questionImageHelper.mime)
            this.fileNameQuestionGet = questionImageHelper.name;
            this.imageQuestionShow = true;

            var index = 0;
            //Generate Image URL
            for (let p of this.choiceHelper) {
                this.choice.push({ answer: null, score: null, taskHotSpotAnswerId: 0, taskId: 0, number: null })
                this.SCORE.push({ points: null, score: null, timePointId: 0 })
                this.choice[index].taskId = p.taskId
                this.choice[index].answer = p.answer
                this.choice[index].number = index
                this.choice[index].taskHotSpotAnswerId = p.taskHotSpotAnswerId
                this.SCORE[index] = await this.TimePointAPI.getSetupTimePointByScoreAsync(p.score)
                this.choice[index].score = this.SCORE[index].score
                index++;
                this.taskScoreGet.push(p.score);
            }

            //Score
            this.allPoints = await this.TaskApi.getAllTimePointsForTask();

            this.isBusy = false;
        }

        //UPDATE
        async updateTask() {
            this.isBusy = true;
            //VALIDASI TASK
            //VALIDASI TASK CODE
            if (this.TASKCODE.competencyId == null || this.TASKCODE.evaluationTypeId == null || this.TASKCODE.competencyTypeId == null) {
                this.errorMessage = "Task Code must be selected!";
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }
            if (this.MODULE.moduleId == null) {
                this.errorMessage = "Module must be selected!";
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            //VALIDASI TYPE
            //VALIDASI QUESTION
            if (this.type.question == null || (this.type.question).toString() == '') {
                this.errorMessage = "Question must be filled!";
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }

            if (this.type.question.length > 2000) {
                this.errorMessage = "Question must be less than 2000 characters";
                this.errorMessageShow = true;
                this.isBusy = false;
                return
            }


            //VALIDASI CHOICE
            for (let f of this.choice) {
                //if (f.answer == null || f.answer == '') {
                if (f.answer == null) {
                    this.errorMessage = "Answer must be filled!"
                    this.isBusy = false;
                    return
                }
            }

            for (let f of this.SCORE) {
                //if (f.answer == null || f.answer == '') {
                if (f.score == null) {
                    this.errorMessage = "Score must be filled!"
                    this.isBusy = false;
                    return
                }
            }

            //VALIDASI LAYOUT 1
            if (this.isStoryLineType == true && this.layoutType == 1) {

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
                        this.isBusy = false;
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
            if (this.isStoryLineType == true && this.layoutType == 2) {

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
                        this.isBusy = false;
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
            this.task.questionTypeId = 5;
            this.task.competencyId = this.TASKCODE.competencyId;
            this.task.evaluationTypeId = this.TASKCODE.evaluationTypeId;
            this.task.moduleId = this.MODULE.moduleId;

            //INSERT TYPE
            if (this.fileNameQuestion != null) {
                //await this.UploadFile(this.formDataQuestion).then((onResult) => {
                //    this.type.blobId = onResult
                //});
                this.type.fileContent = this.formDataQuestion
            }

            //TASK
            this.insert.task.blobId = this.task.blobId;
            this.insert.task.competencyId = this.task.competencyId;
            this.insert.task.evaluationTypeId = this.task.evaluationTypeId;
            this.insert.task.layoutType = this.task.layoutType;
            this.insert.task.moduleId = this.task.moduleId;
            this.insert.task.questionTypeId = this.task.questionTypeId;
            this.insert.task.storyLineDescription = this.task.storyLineDescription;
            this.insert.task.taskId = this.task.taskId;
            this.insert.task.taskNumber = this.task.taskNumber;
            this.insert.task.fileContent = this.task.fileContent;

            this.insert.type.question = this.type.question;
            this.insert.type.blobId = this.type.blobId;
            this.insert.type.fileContent = this.type.fileContent;

            var index = 0;
            for (let f of this.SCORE) {
                //this.insert.choice[index].score = f.score
                this.insert.choice[index].score = f.timePointId
                index++;
            }

            index = 0;
            for (let f of this.choice) {
                this.insert.choice[index].answer = f.answer;
                this.insert.choice[index].number = index + 1;
                index++;
            }

            await this.HotSpotAPI.updateTaskHotspot(this.taskId, this.insert);
            this.CloseTask();
            this.isBusy = false;
        }

        SumScore() {
            this.totalScore = 0;
            for (var c of this.SCORE) {
                this.totalScore = this.totalScore + c.score
            }
        }

        //Get Sum Point
        SumPoint() {
            this.totalPoint = 0;
            for (var c of this.SCORE) {
                this.totalPoint = this.totalPoint + c.points
            }
        }

        //GetNumber
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

        async add() {
            this.choice.push({ answer: null, score: null, taskHotSpotAnswerId: 0, taskId: 0, number: null })
            this.SCORE.push({ points: null, score: null, timePointId: null })
        }

        async remove(index: number) {
            if (this.choice.length <= 1) {
                return
            }
            this.choice.splice(index, 1);
            this.SCORE.splice(index, 1);
        }
        ResetErrorMessage() {
            this.errorMessageShow = false;
        }

        CloseTask() {
            window.location.href = "/Setup/Tasks";
        }

        backPage() {
            window.history.back();
        }
    }

    //jngn lupa nanti pas masukin db ada kolom index
</script>
