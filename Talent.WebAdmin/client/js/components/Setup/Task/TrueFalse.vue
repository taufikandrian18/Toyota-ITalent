<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <!--Add-->
        <div class="row" v-if="mode === 'Add'">
            <div class="col col-md-12">
                <!--TITLE-->

                <h3><fa icon="folder"></fa> Setup > Task > True/False > <span class="talent_font_red">Add</span></h3>
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
                                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)" :disabled="formModel.layoutType === '2' || !formModel.layoutType || isStoryLineType != true">
                                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameVertical ? imageNameVertical : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea class="form-control talent_textarea" maxlength="5000" :disabled="formModel.layoutType === '2' || !formModel.layoutType || isStoryLineType != true" name="verticalTextField" v-model="verticalTextField"></textarea>
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
                                                        <input type="file" name="File" class="custom-file-input" id="customFile" accept="image/*" @change="previewImage($event)" :disabled="formModel.layoutType === '1' || !formModel.layoutType || isStoryLineType != true">
                                                        <label class="custom-file-label  talent_textshorten talent_noroundborder_top" for="customFile">{{imageNameHorizontal ? imageNameHorizontal : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                        <br />

                                        <div class="col-md-5 ">
                                            <textarea class="form-control h-100 talent_overflowy" name="horizontalTextField" maxlength="5000" :disabled="formModel.layoutType === '1' || !formModel.layoutType || isStoryLineType != true" v-model="horizontalTextField"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <!--FORM-->

                <div class="alert alert-success" v-if="this.successMessage">
                    {{this.successMessage}}
                </div>
                <div class="alert alert-danger" v-if="this.errorMessage">
                    {{this.errorMessage}}
                </div>

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
                                    <input class="form-control" :placeholder="taskNumber" disabled />
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
                        </div>
                        <div class="col-md-6">
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <textarea class="form-control talent_textarea" maxlength="2000" placeholder="Question" v-model="formModel.question" name="question"></textarea>
                            <br />
                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <input type="radio" name="truefalse" id="true" :value="true" v-model="formModel.answer" /><label for="true">TRUE</label>
                                        </div>
                                        <div class="col-md-6">
                                            <input type="radio" name="truefalse" id="false" :value="false" v-model="formModel.answer" /><label for="false">FALSE</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 text-center">
                                            <h5>Score<span class="talent_font_red">*</span></h5>
                                            <select class="form-control" v-model="selectedPoints" name="score">
                                                <option v-for="(points,index) in allPoints" :value="points">{{points.score}}</option>
                                            </select>
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h5>Point</h5>
                                            <input class="form-control" disabled v-model="selectedPoints.points" />
                                        </div>
                                    </div>
                                </div>
                            </div>
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
                                            <input class="form-control" disabled v-model="selectedPoints.score" />
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
                                            <input class="form-control" disabled v-model="selectedPoints.points" />
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
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="saveClicked" v-if="crud.create">
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

                <h3><fa icon="folder"></fa> Setup > Task > True/False > <span class="talent_font_red">View Detail</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding">
                    <div class="row d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox" style="line-height: 50%;" disabled v-model="isStoryLineType" color="#0085CA" :size="16"> Type of Story Line</checkbox>
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
                                            <textarea class="form-control talent_textarea" :disabled="formModel.layoutType === '2' || !formModel.layoutType" name="verticalTextField" v-model="verticalTextField"></textarea>
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
                                            <textarea class="form-control h-100 talent_overflowy" name="horizontalTextField" :disabled="formModel.layoutType === '1' || !formModel.layoutType" v-model="horizontalTextField"></textarea>
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
                        </div>
                        <div class="col-md-6">
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <textarea class="form-control talent_textarea" placeholder="Question" disabled v-model="viewDetailModel.question" name="question"></textarea>
                            <br />
                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <input type="radio" name="truefalse" id="true" disabled :value="true" v-model="viewDetailModel.answer" /><label for="true">TRUE</label>
                                        </div>
                                        <div class="col-md-6">
                                            <input type="radio" name="truefalse" id="false" disabled :value="false" v-model="viewDetailModel.answer" /><label for="false">FALSE</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 text-center">
                                            <h5>Score<span class="talent_font_red">*</span></h5>
                                            <input class="form-control" disabled v-model="viewDetailModel.timePoint.score" />
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h5>Point</h5>
                                            <input class="form-control" disabled v-model="viewDetailModel.timePoint.points" />
                                        </div>
                                    </div>
                                </div>
                            </div>
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
                                            <input class="form-control" disabled v-model="viewDetailModel.timePoint.score" />
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
                                            <input class="form-control" disabled v-model="viewDetailModel.timePoint.points" />
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
                                <div>
                                    <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white" @click="backPage">
                                        Close
                                    </button>
                                    <button v-else class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Edit-->
        <div class="row" v-if="mode === 'Edit'">
            <div class="col col-md-12">
                <!--TITLE-->

                <h3><fa icon="folder"></fa> Setup > Task > True/False > <span class="talent_font_red">Edit</span></h3>
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
                                            <input type="radio" name="typeofstoryline" id="vertical" :disabled="isStoryLineType != true" v-model="formUpdateModel.layoutType" @change="updatedLayout()" value="1" /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input type="radio" name="typeofstoryline" id="horizontal" :disabled="isStoryLineType != true" v-model="formUpdateModel.layoutType" value="2" @change="updatedLayout()" /> <label for="horizontal"><b>Type 2</b></label>
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
                                            <textarea class="form-control talent_textarea" maxlength="5000" :disabled="formUpdateModel.layoutType == '2' || isStoryLineType != true || !formUpdateModel.layoutType" name="verticalTextField" v-model="verticalTextField"></textarea>
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
                                            <textarea class="form-control h-100 talent_overflowy" maxlength="5000" name="horizontalTextField" :disabled="formUpdateModel.layoutType == '1' || isStoryLineType != true || !formUpdateModel.layoutType" v-model="horizontalTextField"></textarea>
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
                                    <input class="form-control" :placeholder="taskNumber" disabled />
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
                        </div>
                        <div class="col-md-6">
                            <h5>Question<span class="talent_font_red">*</span></h5>
                            <textarea class="form-control talent_textarea" maxlength="2000" placeholder="Question" v-model="formUpdateModel.question" name="question"></textarea>
                            <br />
                            <div class="row">
                                <div class="col-md-8">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <input type="radio" name="truefalse" id="true" :value="true" v-model="formUpdateModel.answer" /><label for="true">TRUE</label>
                                        </div>
                                        <div class="col-md-6">
                                            <input type="radio" name="truefalse" id="false" :value="false" v-model="formUpdateModel.answer" /><label for="false">FALSE</label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 text-center">
                                            <h5>Score<span class="talent_font_red">*</span></h5>
                                            <select class="form-control" v-model="selectedPoints" name="score">
                                                <option v-for="(points,index) in allPoints" :value="points">{{points.score}}</option>
                                            </select>
                                        </div>
                                        <div class="col-md-6 text-center">
                                            <h5>Point</h5>
                                            <input class="form-control" disabled v-model="selectedPoints.points" />
                                        </div>
                                    </div>
                                </div>
                            </div>
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
                                            <input class="form-control" disabled v-model="selectedPoints.score" />
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
                                            <input class="form-control" disabled v-model="selectedPoints.points" />
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
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="updateClicked" v-if="crud.update">
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
    import { TrueFalseService, TaskService, TaskTrueFalseFormModel, TrueFalseTypeViewDetails, CompetencyMappingJoinModel, ModuleForTaskModel, TimePointTaskModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../../services/NSwagService'
    import { PageEnums } from '../../../enum/PageEnums';
    import IFileContent from '../../../models/IFileContent';

    @Component({
        props: ['mode', 'taskid', 'fromOutside'],
        created: async function (this: TrueFalse) {
            await this.getAccess()
            if (this.mode == 'Add') {
                if (!this.crud.create) {
                    return;
                }
                this.initialize();
            }
            else if (this.mode == 'View') {
                if (!this.crud.read) {
                    return;
                }
                this.initializeView();
            }
            else if (this.mode == 'Edit') {
                if (!this.crud.update) {
                    return;
                }
                this.initializeUpdate();
            }
        }
    })
    export default class TrueFalse extends Vue {

        competenciesMappingList: CompetencyMappingJoinModel[] = [];
        allModulesForTask: ModuleForTaskModel[] = [];
        allPoints: TimePointTaskModel[] = [];

        viewDetailModel: TrueFalseTypeViewDetails = { blobId: '', answer: null, competencyId: null, competencyTypeId: null, competencyTypeName: '', evaluationTypeId: null, evaluationTypeName: '', layoutType: null, mime: '', moduleId: null, moduleName: '', name: '', timePoint: null, prefixCode: '', question: '', storyLineDescription: '', taskNumber: null };

        mode: string;
        taskid: number;
        fromOutside: boolean;

        tempStringCheckingUpdateImage: string = '';
        //TOLONG DI CHECK BOOLNYA BOLEH NULL APA KGK
        formModel: TaskTrueFalseFormModel = { blobId: '', answer: null, competencyId: null, evaluationTypeId: null, layoutType: null, moduleId: null, question: '', timePoint: null, storyLineDescription: '', taskNumber: null, questionTypeId: null, taskId: null, fileContent: null };

        formUpdateModel: TaskTrueFalseFormModel = { blobId: '', answer: null, competencyId: null, evaluationTypeId: null, layoutType: null, moduleId: null, question: '', timePoint: null, storyLineDescription: '', taskNumber: null, questionTypeId: null, taskId: null, fileContent: null };

        successMessage: string = '';
        errorMessage: string = '';

        verticalTextField: string = '';
        horizontalTextField: string = '';
        isBusy: boolean = false;

        imageDataVertical: string | ArrayBuffer = '/upload-image2.png';
        imageDataHorizontal: string | ArrayBuffer = '/upload-image2.png';
        defaultImage: string = '/upload-image2.png';
        imageNameVertical: string = '';
        imageNameHorizontal: string = '';

        verticalBlobId: string = null;
        horizontalBlobId: string = null;

        taskCodeViewDetail: string = '';

        isStoryLineType: boolean = null;


        taskNumber: number = 0;

        selectedCompetency: CompetencyMappingJoinModel = { competencyId: null, competencyNameMapping: null, competencyTypeId: null, competencyTypeName: '', evaluationTypeId: null, evaluationTypeName: '', prefixCode: '' }

        ServiceTrueFalseTask: TrueFalseService = new TrueFalseService();
        ServiceTask: TaskService = new TaskService();

        selectedPoints: TimePointTaskModel = { points: null, score: null, time: null, timePointId: null }

        selectedModuleIndex: number = 0;
        points: number = 0;
        selecetedComptencyIndex: number = 0;

        //formDataVertical: FormData = new FormData();
        //formDataHorizontal: FormData = new FormData();

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
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        selectedModule: ModuleForTaskModel = { moduleId: null, moduleName: '' }

        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Task);
            this.crud = data
        }


        async saveClicked() {
            this.errorMessage = '';
            this.successMessage = '';

            if (!this.crud.create) {
                return;
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
            else if (this.formModel.answer == null) {
                this.errorMessage = "Answer is Required";
                return;
            }
            else if (!this.selectedPoints.timePointId) {
                this.errorMessage = "Score is Required";
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

            this.formModel.timePoint = this.selectedPoints;
            this.formModel.competencyId = this.selectedCompetency.competencyId;
            this.formModel.moduleId = this.selectedModule.moduleId;
            this.formModel.evaluationTypeId = this.selectedCompetency.evaluationTypeId;

            if (this.formModel.layoutType) {

                //let blobId;

                if (this.formModel.layoutType == 1 && this.imageNameVertical.length > 0) {
                    //blobId = await BlobService.uploadService(this.formDataVertical);
                    this.formModel.fileContent = this.formDataVertical
                }
                else if (this.formModel.layoutType == 2 && this.imageNameHorizontal.length > 0) {
                    //blobId = await BlobService.uploadService(this.formDataHorizontal);
                    this.formModel.fileContent = this.formDataHorizontal
                }
                //else {
                //    blobId = null;
                //}

                //this.formModel.blobId = blobId;
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
                let isSuccess = await this.ServiceTrueFalseTask.insertTrueFalse(this.formModel);
                this.isBusy = false;
                this.successMessage = "Success to Input Task!";
                this.closeClicked();
            }
            catch{
                this.errorMessage = "Failed to Insert!";
                this.isBusy = false;
            }
        }

        async initialize() {
            this.competenciesMappingList = await this.ServiceTask.getAllCompetencies();
            this.allModulesForTask = await this.ServiceTask.getAllModulesForTask();
            this.allPoints = await this.ServiceTask.getAllTimePointsForTask();
            this.getNumber();
        }

        async initializeView() {
            this.viewDetailModel = await this.ServiceTrueFalseTask.getTaskTrue(this.taskid);
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

        updatedLayout(type: number) {
            this.$forceUpdate();
        }


        async initializeUpdate() {
            this.viewDetailModel = await this.ServiceTrueFalseTask.getTaskTrue(this.taskid);
            this.formUpdateModel.answer = this.viewDetailModel.answer;
            this.formUpdateModel.question = this.viewDetailModel.question;
            this.formUpdateModel.timePoint = this.viewDetailModel.timePoint;
            this.formUpdateModel.taskId = this.taskid;


            this.taskNumber = this.viewDetailModel.taskNumber;

            if (this.viewDetailModel.layoutType != 0) {
                let stringSrc;

                if (this.viewDetailModel.blobId != null) {
                    console.log("get-image");
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

            let currentPointList: number[] = [];
            currentPointList.push(this.viewDetailModel.timePoint.score);
            this.allModulesForTask = await this.ServiceTask.getAllModulesForTask();
            this.allPoints = await this.ServiceTask.getAllTimePointsForTask();
            this.getNumber();

            this.selectedModule.moduleId = this.viewDetailModel.moduleId;
            this.selectedModule.moduleName = this.viewDetailModel.moduleName;

            this.selectedPoints.timePointId = this.viewDetailModel.timePoint.timePointId;
            this.selectedPoints.points = this.viewDetailModel.timePoint.points;
            this.selectedPoints.score = this.viewDetailModel.timePoint.score;

            this.selectedCompetency.competencyId = this.viewDetailModel.competencyId;
            this.selectedCompetency.competencyTypeId = this.viewDetailModel.competencyTypeId;
            this.selectedCompetency.competencyTypeName = this.viewDetailModel.competencyTypeName;
            this.selectedCompetency.evaluationTypeId = this.viewDetailModel.evaluationTypeId;
            this.selectedCompetency.evaluationTypeName = this.viewDetailModel.evaluationTypeName;
            this.selectedCompetency.prefixCode = this.viewDetailModel.prefixCode;

            this.competenciesMappingList = await this.ServiceTask.getAllCompetencies();

            this.allModulesForTask = await this.ServiceTask.getAllModulesForTask();

        }

        checkBoxCheck() {
            if (this.isStoryLineType == false) {
                if (this.mode == 'Add') {
                    this.formModel.layoutType = 0;
                }
                else if (this.mode == 'Edit') {
                    this.formUpdateModel.layoutType = 0;
                }
            }
        }

        closeClicked() {
            window.location.href = '/Setup/Tasks';
        }

        async updateClicked() {
            this.errorMessage = '';
            this.successMessage = '';

            if (!this.crud.update) {
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

            if (this.formUpdateModel.answer == null) {
                this.errorMessage = "Answer can't be Empty";
                return;
            }

            if (this.selectedPoints == null) {
                this.errorMessage = "Score can't be Empty";
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
                            if (isImgSrcFromLocal) {
                                this.formUpdateModel.fileContent = this.formDataHorizontal;
                            }
                            else {
                                this.formUpdateModel.blobId = this.horizontalBlobId;
                            }
                        }
                        this.formUpdateModel.storyLineDescription = horizontalStorylineDesc;
                    }
                }
            }
            if (this.formUpdateModel.storyLineDescription.length > 5000) {
                this.errorMessage = "Storyline description must be less than 5000 characters";
                return;
            }
            if (this.formUpdateModel.question.length > 2000) {
                this.errorMessage = "Question must be less than 2000 characters";
                return;
            }

            this.formUpdateModel.moduleId = this.selectedModule.moduleId;
            this.formUpdateModel.timePoint = this.selectedPoints;
            this.formUpdateModel.competencyId = this.selectedCompetency.competencyId;
            this.formUpdateModel.evaluationTypeId = this.selectedCompetency.evaluationTypeId;

            this.isBusy = true;
            try {
                let result = await this.ServiceTrueFalseTask.updateTrueFalse(this.formUpdateModel);
                this.successMessage = "Success to Update Date!";
                this.isBusy = false;

                this.closeClicked();
            }
            catch{
                this.errorMessage = "Failed to Update";
                this.isBusy = false;
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
                    //console.log(this.formUpdateModel.layoutType);
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

            //this.formDataHorizontal = new FormData();
            //this.formDataVertical = new FormData();

            //Array.from(Array(event.target.files.length).keys())
            //    .map(x => {
            //        if (this.formModel.layoutType == 1 || this.formUpdateModel.layoutType == 1) {
            //            this.formDataVertical.append(event.target.files, event.target.files[x], event.target.files[x].name);
            //        }
            //        else {
            //            this.formDataHorizontal.append(event.target.files, event.target.files[x], event.target.files[x].name);
            //        }
            //    });
        }

        backPage() {
            window.history.back();
        }

    }
</script>
