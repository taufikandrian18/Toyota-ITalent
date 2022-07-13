<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <!--ADD-->
        <div class="row" v-if="mode == 'Add'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Tebak Gambar > <span class="talent_font_red">Add</span></h3>
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
                            <input v-model="type.question" type="text" maxlength="2000" class="form-control" />
                            <div class="mb-4"></div>

                            <!--ForEach-->
                            <div v-for="(image, index) in AnswerImage">
                                <div class=" h-100">
                                    <div class="row">
                                        <div class="col-md-1 align-self-center">
                                            {{index + 1}}
                                        </div>
                                        <div class="col-md-10 mb-4">
                                            <div>
                                                <img class="h-100 w-100" :src="AnswerImage[index].imageData ? AnswerImage[index].imageData : '/upload-image2.png'" />
                                                <div class="custom-file">
                                                    <input type="file" class="custom-file-input" id="customFile" @change.prevent="handler(index, $event)" />
                                                    <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{image.fileName.length ? image.fileName : 'Choose File'}}</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-1 align-self-center">
                                            <div v-if="index != AnswerImage.length - 1" class="text-center" @click.prevent="RemovePicture(index)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                            <div v-if="index == AnswerImage.length - 1" class="text-center" @click.prevent="AddPicture()"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <!--<select v-model="ANSWERINT" class="form-control" @change.prevent="getNumber()">-->
                                    <select v-model="ANSWERINT" class="form-control">
                                        <option v-for="a in ANSWER" :value="a + 1">{{a + 1}}</option>
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
                                                Total Scores
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
        <!--EDIT-->
        <div class="row" v-if="mode == 'Edit'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Tebak Gambar > <span class="talent_font_red">Edit</span></h3>
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
                            <input v-model="type.question" type="text" maxlength="2000" class="form-control" />
                            <div class="mb-4"></div>

                            <!--ForEach-->
                            <div v-for="(image, index) in AnswerImage">
                                <div class=" h-100">
                                    <div class="row">
                                        <div class="col-md-1 align-self-center">
                                            {{index + 1}}
                                        </div>
                                        <div class="col-md-10 mb-4">
                                            <div>
                                                <img class="h-100 w-100" :src="AnswerImage[index].imageData ? AnswerImage[index].imageData : '/upload-image2.png'" />
                                                <div class="custom-file">
                                                    <input type="file" class="custom-file-input" id="customFile" @change.prevent="handler(index, $event)" />
                                                    <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{image.fileName.length ? image.fileName : 'Choose File'}}</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-1 align-self-center">
                                            <div v-if="index != AnswerImage.length - 1" class="text-center" @click.prevent="RemovePicture(index)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                            <div v-if="index == AnswerImage.length - 1" class="text-center" @click.prevent="AddPicture()"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="mb-4"></div>
                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <!--<select v-model="ANSWERINT" class="form-control" @change.prevent="getNumber()">-->
                                    <select v-model="ANSWERINT" class="form-control">
                                        <option v-for="a in ANSWER" :value="a + 1">{{a + 1}}</option>
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
                                                Total Scores
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
        <!--VIEW DETAIL-->
        <div class="row" v-if="mode == 'View'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > Task > Tebak Gambar > <span class="talent_font_red">View Detail</span></h3>
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
                                            <upload-file :updatedImage="fileImageStoryLineHorizontalGet" :updatedName="fileNameStoryLineHorizontalGet" layout="TaskHorizontal" :formData.sync="formDataStoryLineHorizontal" :fileName.sync="fileNameStoryLineHorizontal" :isStoryLineType="isStoryLineType" :layoutType="layoutType" :disable="true"></upload-file>
                                        </div>

                                    </div>
                                    <div class="mb-4"></div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea v-model="textAreaHorizontal" maxlength="5000" class="form-control talent_textarea" disabled></textarea>
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
                                                    <upload-file layout="TaskVertical" :formData.sync="formDataStoryLineVertical" :fileName.sync="fileNameStoryLineVertical" :isStoryLineType="isStoryLineType" :layoutType="layoutType" :updatedImage="fileImageStoryLineVerticalGet" :updatedName="fileNameStoryLineVerticalGet" :disable="true"></upload-file>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5 ">
                                            <textarea v-model="textAreaVertical" maxlength="5000" class="form-control h-100 talent_overflowy" disabled></textarea>
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
                            <input disabled v-model="type.question" type="text" maxlength="2000" class="form-control" />
                            <div class="mb-4"></div>

                            <!--ForEach-->
                            <!--<div v-for="(image, index) in picture">
                                <div class="talent_marginbottom h-100">
                                    <div class="row">
                                        <div class="col-md-1 align-self-center">
                                            {{index + 1}}
                                        </div>
                                        <div class="col-md-10">
                                            <upload-file layout="TaskHorizontal" :formData.sync="formDataAnswer[index]" :fileName.sync="fileNameAnswer[index]" :updatedImage="fileImageAnswerGet[index]" :updatedName="fileNameAnswerGet[index]" :isStoryLineType="renderInitHelper" :layoutType="layoutType" :disable="true"></upload-file>
                                        </div>
                                        <div class="col-md-1 align-self-center">
                                            <div v-if="index != picture.length - 1" class="text-center" @click.prevent="RemovePicture(index)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                            <div v-if="index == picture.length - 1" class="text-center" @click.prevent="AddPicture()"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                        </div>
                                    </div>
                                </div>
                            </div>-->
                            <div v-for="(image, index) in AnswerImage">
                                <div class="h-100">
                                    <div class="row">
                                        <div class="col-md-1 align-self-center">
                                            {{index + 1}}
                                        </div>
                                        <div class="col-md-11 mb-4">
                                            <div>
                                                <img class="h-100 w-100" :src="AnswerImage[index].imageData ? AnswerImage[index].imageData : '/upload-image2.png'" />
                                                <div class="custom-file">
                                                    <input disabled type="file" class="custom-file-input" id="customFile" @change.prevent="handler(index, $event)" />
                                                    <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{image.fileName.length ? image.fileName : 'Choose File'}}</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="mb-4"></div>
                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <select disabled v-model="ANSWERINT" class="form-control" @change.prevent="getNumber()">
                                        <option v-for="(a, index) in ANSWER" :value="a + 1">{{a + 1}}</option>
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
                                                Total Scores
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
    import {
        TaskService, SetupTimePointService, TebakGambarService, CompetencyMappingJoinModel, TimePointTaskModel, ModuleForTaskModel,
        TaskInsertModel, TaskTebakGambarPicturesModel, TaskTebakGambarTypesModel, TaskTebakGambarModel, FileContent
    } from '../../../services/NSwagService'
    import { faFileContract } from '@fortawesome/free-solid-svg-icons';

    @Component({
        props: ['mode', 'taskId', 'fromOutside'],
        created: async function (this: TebakGambar) {
            await this.initialize();
            if (this.mode == 'View' || this.mode == 'Edit') {
                await this.InitialGetData()
            }
            if (this.mode == 'Add') {
                this.AnswerImage.push({ fileName: '', formData: new FormData, blobId: null, imageData: '', number: null, taskTebakGambarId: null, taskId: 0, imageUpload: null })
                //this.picture.push({ blobId: null, taskId: 0, taskTebakGambarId: null, number: null });
                this.ANSWER.push(0);

            }
        },
    })
    export default class TebakGambar extends Vue {


        //API
        TebakGambarAPI: TebakGambarService = new TebakGambarService;
        TimePointAPI: SetupTimePointService = new SetupTimePointService;
        TaskApi: TaskService = new TaskService;

        //LOADING OVERLAY
        isBusy: boolean = false;

        //VARIABLE INSERT
        task: TaskInsertModel = {
            blobId: null, competencyId: null, evaluationTypeId: null, layoutType: null,
            moduleId: null, questionTypeId: null, storyLineDescription: null, taskId: 0, taskNumber: null
        }
        type: TaskTebakGambarTypesModel = { taskId: 0, answer: null, question: null, score: null }
        picture: TaskTebakGambarPicturesModel[] = []; //Variable yang dipke untuk parameter API
        insert: TaskTebakGambarModel = { task: this.task, type: this.type, picture: this.picture }
        //Variable Pembantu insert picture
        insertPictureHelper: TaskTebakGambarPicturesModel[] = []; //Variable yang di pake untuk tampung data get berdasarkan TaskID

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
        formDataStoryLineHorizontal: FileContent = { base64: "", contentType: "", fileName: "" }
        fileNameStoryLineHorizontal: string = null;
        formDataStoryLineVertical: FileContent = { base64: "", contentType: "", fileName: "" }
        fileNameStoryLineVertical: string = null;

        //Variable untuk File Upload Manual
        AnswerImage: FileUploadImage[] = [] //Variable yang di pake buat atur UI image

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
        MODULE: ModuleForTaskModel = { moduleId: null, moduleName: null }

        //VARIABLE TYPE ADDITIONAL
        SCORE: TimePointTaskModel = { points: null, score: null, timePointId: null }
        ANSWER: number[] = [];
        ANSWERINT: number = null;

        //VARIABLE ALERT
        errorMessageShow: boolean = false;
        errorMessage: string = "";

        //INITIALIZE
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

        async addTask() {
            //VALIDASI TASK
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

            //VALIDASI PICTURE
            for (let f of this.AnswerImage) {
                if (f.fileName == null || f.fileName == '') {
                    this.errorMessage = "Picture must be filled!"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }
            }

            //VALIDASI TYPE
            //VALIDASI ANSWER

            if (this.ANSWERINT == null) {
                this.errorMessage = "Answer must be filled!";
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
                        this.isBusy = false
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
                        this.isBusy = false
                    }
                }
                this.task.storyLineDescription = this.textAreaVertical;
                this.task.layoutType = 2
                if (this.fileNameStoryLineVertical != null) {
                    this.task.fileContent = this.formDataStoryLineVertical;
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
            for (let p of this.AnswerImage) {
                this.picture.push({ blobId: null, taskId: this.task.taskId, number: null });
                if (this.AnswerImage[index].blobId == null) {
                    //await this.UploadFile(this.AnswerImage[index].formData).then((onResult) => {
                    //    this.picture[index].blobId = onResult
                    //})

                    this.picture[index].imageUpload = this.AnswerImage[index].imageUpload;

                } else {
                    this.picture[index].blobId = p.blobId
                }
                this.picture[index].number = index + 1;
                this.picture[index].taskTebakGambarId = index;
                index++;
            }

            //TOSTRING SEMENTARA
            this.type.answer = this.ANSWERINT

            //this.insert.task.blobId = this.task.blobId;
            this.insert.task.fileContent = this.task.fileContent;
            this.insert.task.competencyId = this.task.competencyId;
            this.insert.task.evaluationTypeId = this.task.evaluationTypeId;
            this.insert.task.layoutType = this.task.layoutType;
            this.insert.task.moduleId = this.task.moduleId;
            this.insert.task.questionTypeId = this.task.questionTypeId;
            this.insert.task.storyLineDescription = this.task.storyLineDescription;
            this.insert.task.taskId = this.task.taskId;
            this.insert.task.taskNumber = this.task.taskNumber;

            this.insert.type.taskId = this.type.taskId;
            this.insert.type.answer = this.type.answer;
            this.insert.type.question = this.type.question;
            this.insert.type.score = this.type.score;


            const tempcount = this.AnswerImage.length;
            for (var i = 0; i < tempcount; i++) {
                this.insertPictureHelper.push({
                    blobId: this.AnswerImage[i].blobId,
                    number: this.AnswerImage[i].number,
                    taskId: this.AnswerImage[i].taskId,
                    taskTebakGambarId: this.AnswerImage[i].taskTebakGambarId,
                    imageUpload: this.AnswerImage[i].imageUpload
                });
            }

            //INSERT TO DB
            await this.TebakGambarAPI.insertTaskTebakGambar(this.insert);
            this.Close()
            this.isBusy = false
        }

        //SOURCE OF TRUTH
        sourceCompetencyId: number = null;
        sourceEvaluationTypeId: number = null;
        sourceModuleId: number = null;
        sourceTaskNumber: number = null;

        //Score
        taskScoreGet: number[] = []

        async InitialGetData() {
            this.isBusy = true
            this.task = await this.TaskApi.getTaskById(this.taskId);
            this.type = await this.TebakGambarAPI.getTaskTebakGambarTypeById(this.taskId);

            //Score
            this.taskScoreGet.push(this.type.score);
            this.allPoints = await this.TaskApi.getAllTimePointsForTask();

            //VARIABLE UNTUK VALIDASI
            this.sourceCompetencyId = this.task.competencyId;
            this.sourceEvaluationTypeId = this.task.evaluationTypeId;
            this.sourceModuleId = this.task.moduleId;
            this.sourceTaskNumber = this.task.taskNumber;

            this.insertPictureHelper = await this.TebakGambarAPI.getTaskTebakGambarPictureById(this.taskId);
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

            index = 0
            //Generate Image URL
            for (let p of this.insertPictureHelper) {
                this.ANSWER.push(index);
                this.AnswerImage.push({ fileName: '', formData: new FormData, blobId: null, imageData: '', number: 0, taskTebakGambarId: 0, taskId: 0, imageUpload: null })
                var x = await BlobService.getBlobById(p.blobId);
                var getImg = await BlobService.getImageUrl(x.blobId, x.mime)
                this.AnswerImage[index].imageData = getImg
                this.AnswerImage[index].fileName = x.name
                this.AnswerImage[index].blobId = p.blobId

                index++
            }

            //SCORE
            this.SCORE = await this.TimePointAPI.getSetupTimePointByScoreAsync(this.type.score);
            //ANSWER
            this.ANSWERINT = this.type.answer;
            this.isBusy = false
        }

        async updateTask() {
            //VALIDASI TASK
            this.isBusy = true
            //VALIDASI TASK CODE*
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

            //VALIDASI PICTURE
            //masih masalh
            for (var i = 0; i < this.AnswerImage.length; i++) {
                if (this.AnswerImage[i].fileName == null || this.AnswerImage[i].fileName == '') {
                    this.errorMessage = "Picture must be filled!"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }
            }

            //VALIDASI LAYOUT 1
            if (this.isStoryLineType == true && this.layoutType == 1) {
                console.log("INI 1");

                if ((this.textAreaHorizontal == null || this.textAreaHorizontal == '') && (this.formDataStoryLineHorizontal.fileName == null || this.formDataStoryLineHorizontal.fileName == '')) {
                    this.errorMessage = "Storyline or Image must be filled"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }

                if (this.textAreaHorizontal != null) {
                    if (this.textAreaHorizontal.length > 5000) {
                        this.errorMessage = "Storyline description must be less than 5000 characters"
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
            if (this.isStoryLineType == true && this.layoutType == 2) {
                console.log("INI 2");

                if ((this.textAreaVertical == null || this.textAreaVertical == '') && (this.formDataStoryLineVertical.fileName == null || this.formDataStoryLineVertical.fileName == '')) {
                    this.errorMessage = "Storyline or Image must be filled"
                    this.errorMessageShow = true;
                    this.isBusy = false
                    return
                }

                if (this.textAreaVertical != null) {
                    if (this.textAreaVertical.length > 5000) {
                        this.errorMessage = "Storyline description must be less than 5000 characters"
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
                console.log("INI 3");

                this.task.storyLineDescription = null;
                this.task.layoutType = 0;
                this.task.blobId = null;
            }

            //masih msalah aama value
            if (this.ANSWERINT == null) {
                this.errorMessage = "Answer must be filled!";
                this.errorMessageShow = true;
                return
            }

            //VALIDASI SCORE
            if (this.SCORE.score == null) {
                this.errorMessage = "Score must be selected!";
                this.errorMessageShow = true;
            }

            //INSERT TASK
            this.task.taskId = this.taskId; //SEMENTARA AJA
            this.task.questionTypeId = 4;


            this.task.competencyId = this.TASKCODE.competencyId;
            this.task.evaluationTypeId = this.TASKCODE.evaluationTypeId;
            this.task.moduleId = this.MODULE.moduleId;


            //INSERT TYPE
            this.type.taskId = this.taskId;
            //this.type.score = this.SCORE.score;
            this.type.score = this.SCORE.timePointId;

            //INSERT PICTURE
            //masih sedikit maslah
            let index = 0;
            for (let p of this.AnswerImage) {
                this.picture.push({ blobId: null, taskId: this.taskId, number: null });
                if (this.AnswerImage[index].blobId == null) {
                    //await this.UploadFile(this.AnswerImage[index].formData).then((onResult) => {
                    //    this.picture[index].blobId = onResult
                    //})

                    this.picture[index].imageUpload = this.AnswerImage[index].imageUpload;
                    this.picture[index].blobId = p.blobId
                } else {
                    this.picture[index].blobId = p.blobId
                }
                this.picture[index].number = index + 1;
                this.picture[index].taskTebakGambarId = index;
                index++;
            }

            //TOSTRING SEMENTARA
            this.type.answer = this.ANSWERINT

            this.insert.task.blobId = this.task.blobId;
            this.insert.task.fileContent = this.task.fileContent;
            this.insert.task.competencyId = this.task.competencyId;
            this.insert.task.evaluationTypeId = this.task.evaluationTypeId;
            this.insert.task.layoutType = this.task.layoutType;
            this.insert.task.moduleId = this.task.moduleId;
            this.insert.task.questionTypeId = this.task.questionTypeId;
            this.insert.task.storyLineDescription = this.task.storyLineDescription;
            this.insert.task.taskId = this.task.taskId;
            this.insert.task.taskNumber = this.task.taskNumber;

            this.insert.type.taskId = this.type.taskId;
            this.insert.type.answer = this.type.answer;
            this.insert.type.question = this.type.question;
            this.insert.type.score = this.type.score;

            //this.insert.picture = this.picture;

            //INSERT TO DB
            await this.TebakGambarAPI.updateTaskTebakGambar(this.taskId, this.insert);
            this.Close();
            this.isBusy = false
        }

        //ADD ANSWER ARRAY
        AddPicture() {
            if (this.mode == 'add') {
                this.picture.push({ blobId: null, taskId: 0, number: null });
            }

            this.AnswerImage.push({ blobId: null, fileName: '', formData: new FormData, imageData: '', number: 0, taskTebakGambarId: 0, taskId: 0, imageUpload: null })
            this.ANSWER.push(this.ANSWER.length)
        }
        //REMOVE ANSWER ARRAY
        RemovePicture(index: number) {
            if (this.AnswerImage.length <= 1) {
                return
            }
            this.picture.splice(index, 1);
            this.AnswerImage.splice(index, 1);
            this.ANSWER.splice(this.ANSWER.length - 1, 1);
            if (this.ANSWERINT > this.ANSWER.length - 1) {
                this.ANSWERINT = null;
            }
        }

        //CLOSE
        Close() {
            window.location.href = "/Setup/Tasks"
        }
        //RESET ERROR MESSAGE
        ResetErrorMessage() {
            this.errorMessageShow = false;
        }

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

        handler(index: number, $event) {
            if ($event.target.files.length === 0) {
                return;
            }
            this.loadFile($event, index);
            //this.fileChange($event, index);
        }

        async loadFile($event, index: number) {
            var reader = new FileReader();
            reader.onload = (e: Event) => {
                this.AnswerImage[index].imageData = (<FileReader>e.target).result;
            }
            reader.readAsDataURL($event.target.files[0]);
            this.AnswerImage[index].fileName = $event.target.files[0].name;
            this.AnswerImage[index].formData.set($event.target.files, $event.target.files[0], $event.target.files[0].name);
            this.AnswerImage[index].blobId = null;

            let inputFile = (<HTMLInputElement>$event.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = $event.target.files[0];

            let array = file.name.split(".");

            this.AnswerImage[index].imageUpload = {
                base64: base64String,
                fileName: file.name,
                contentType: array.pop()
            };
        }

        backPage() {
            window.history.back();
        }
    }

    class FileUploadImage {
        imageData: string | ArrayBuffer;
        formData: FormData;
        fileName: string;
        blobId: string;
        taskTebakGambarId: number;
        number: number;
        taskId: number;
        imageUpload: FileContent;
    }


</script>
