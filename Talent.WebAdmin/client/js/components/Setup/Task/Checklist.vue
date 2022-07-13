<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <!--ADD-->
        <div class="row" v-if="mode === 'Add'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup > Task > Checklist > <span class="talent_font_red">Add</span></h3>

                <!--STORYLINE-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding mb-md-4">
                    <div class="row d-flex justify-content-center mb-md-4">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox talent-checkbox-lineheight"
                                          v-model="storyLine" color="#0085CA" :size="16"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="storyLineLayoutType"
                                                   type="radio"
                                                   name="typeofstoryline"
                                                   id="vertical"
                                                   :value="1"
                                                   :disabled="storyLine === false" /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="storyLineLayoutType"
                                                   type="radio"
                                                   name="typeofstoryline"
                                                   id="horizontal"
                                                   :value="2"
                                                   :disabled="storyLine === false" /> <label for="horizontal"><b>Type 2</b></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row mb-md-4">
                                        <div class="col-md-12 mb-md-5">
                                            <img class="h-100 w-100" :src="ImageData1.length ? ImageData1 : '/upload-image2.png'" />
                                            <div class="custom-file">
                                                <input type="file"
                                                       class="custom-file-input"
                                                       id="customFile"
                                                       @change="handler"
                                                       :disabled="storyLineLayoutType != '1' || storyLine != true" />
                                                <label class="custom-file-label talent_textshorten talent_noroundborder_top"
                                                       for="customFile">{{ImageName1.length ? ImageName1 : 'Choose File'}}</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea class="form-control talent_textarea"
                                                      maxlength="5006"
                                                      v-model="storyLineDescription1"
                                                      :disabled="storyLineLayoutType != '1' || storyLine != true"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row h-100">
                                        <div class="col-md-7">
                                            <div class="h-75">
                                                <img class="h-100 w-100" :src="ImageData2.length ? ImageData2 : '/upload-image2.png'" />
                                                <div class="custom-file">
                                                    <input type="file"
                                                           class="custom-file-input"
                                                           id="customFile"
                                                           @change="handler"
                                                           :disabled="storyLineLayoutType != '2' || storyLine != true" />
                                                    <label class="custom-file-label talent_textshorten talent_noroundborder_top"
                                                           for="customFile">{{ImageName2.length ? ImageName2 : 'Choose File'}}</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5 ">
                                            <textarea class="form-control h-100 talent_overflowy"
                                                      maxlength="5000"
                                                      v-model="storyLineDescription2"
                                                      :disabled="storyLineLayoutType != '2' || storyLine != true"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding mb-md-4">

                    <div v-if="error !== ''" class="alert alert-danger">
                        {{error}}
                        <button type="button" class="close" aria-label="Close" @click="error = ''">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <select class="form-control" v-model="taskCodeForm" @change="getNumber()">
                                        <option v-for="taskCode in taskCodes" :value="taskCode">
                                            {{taskCode.competencyTypeName.charAt(0)}}-{{taskCode.prefixCode}}-{{taskCode.evaluationTypeName}}
                                        </option>
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
                                    <div class="input-group">
                                        <multiselect
                                            v-model="moduleForm"
                                            track-by="moduleId"
                                            :options="modules"
                                            label="moduleName"
                                            v-validate="'required'"
                                            :searchable="true"
                                        ></multiselect>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <h5>Question<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-2 text-center">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 talent_nopaddingleft text-center"><h5>Score<span class="talent_font_red">*</span></h5></div>
                                        <div class="col-md-4 talent_nopaddingleft text-center"><h5>Points</h5></div>
                                    </div>
                                </div>
                            </div>

                            <div class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-6 talent_nopaddingright">
                                        <input class="form-control" v-model="taskQuestion" maxlength="2004"/>
                                    </div>
                                </div>
                            </div>


                            <!--ForEach-->
                            <div v-for="(option, index) in options" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 ">
                                        <label>{{index + 1}}</label>
                                    </div>
                                    <div class="col-md-5 talent_nopaddingleft">
                                        <div class="row">
                                            <div class="col-md-12 talent_nopaddingright">
                                                <input class="form-control form-control-sm" v-model="options[index].text" maxlength="2000"/>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2 d-flex justify-content-center">
                                        <div>
                                            <checkbox class="talent_checkboxnolabel talent-checkbox-lineheight"
                                                      v-model="options[index].check"
                                                      color="#0085CA"
                                                      @change="changeScore(index)"
                                                      :size="31"></checkbox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 ">
                                        <div class="row">
                                            <div class="col-md-6 talent_nopaddingleft">
                                                <select class="form-control" v-model="options[index].timePointId" @change="generatePoint(index)" :disabled="!options[index].check">
                                                    <option v-for="point in points" :value="point.timePointId">{{point.score}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-4 talent_nopaddingleft">
                                                <input v-model="options[index].point" class="form-control form-control-sm " disabled />
                                            </div>
                                            <div v-if="options.length !== 2" class="col-md-1 talent_nopadding text-center" @click.prevent="remove(index)">
                                                <a class="h-100 w-100 d-flex align-items-center talent_cursorpoint justify-content-center"><fa icon="trash-alt"></fa></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="talent_nopadding text-center">
                                <a class="talent_cursorpoint" @click.prevent="add()"><fa icon="plus-circle"></fa> Add New Choice</a>
                            </div>

                        </div>
                    </div>
                </div>

                <!--Total-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
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
                                            <input class="form-control" v-model="totalScore" disabled />
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
                                            <input class="form-control" v-model="totalPoints" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white"
                                            @click="close()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="insert()">
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
        <div class="row" v-if="mode === 'Edit'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup > Task > Checklist > <span class="talent_font_red">Edit</span></h3>

                <!--STORYLINE-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding mb-md-4">
                    <div class="row d-flex justify-content-center mb-md-4">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox talent-checkbox-lineheight"
                                          v-model="storyLine" color="#0085CA" :size="16"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="storyLineLayoutType"
                                                   type="radio"
                                                   name="typeofstoryline"
                                                   id="vertical"
                                                   :value="1"
                                                   :disabled="storyLine === false" /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="storyLineLayoutType"
                                                   type="radio"
                                                   name="typeofstoryline"
                                                   id="horizontal"
                                                   :value="2"
                                                   :disabled="storyLine === false" /> <label for="horizontal"><b>Type 2</b></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row mb-md-4">
                                        <div class="col-md-12 mb-md-5">
                                            <img class="h-100 w-100" :src="ImageData1.length ? ImageData1 : '/upload-image2.png'" />
                                            <div class="custom-file">
                                                <input type="file"
                                                       class="custom-file-input"
                                                       id="customFile"
                                                       @change="handler"
                                                       :disabled="storyLineLayoutType != '1' || storyLine != true" />
                                                <label class="custom-file-label talent_textshorten talent_noroundborder_top"
                                                       for="customFile">{{ImageName1.length ? ImageName1 : 'Choose File'}}</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea class="form-control talent_textarea"
                                                      maxlength="5000"
                                                      v-model="storyLineDescription1"
                                                      :disabled="storyLineLayoutType != '1' || storyLine != true"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row h-100">
                                        <div class="col-md-7">
                                            <div class="h-75">
                                                <img class="h-100 w-100" :src="ImageData2.length ? ImageData2 : '/upload-image2.png'" />
                                                <div class="custom-file">
                                                    <input type="file"
                                                           class="custom-file-input"
                                                           id="customFile"
                                                           @change="handler"
                                                           :disabled="storyLineLayoutType != '2' || storyLine != true" />
                                                    <label class="custom-file-label talent_textshorten talent_noroundborder_top"
                                                           for="customFile">{{ImageName2.length ? ImageName2 : 'Choose File'}}</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5 ">
                                            <textarea class="form-control h-100 talent_overflowy"
                                                      maxlength="5000"
                                                      v-model="storyLineDescription2"
                                                      :disabled="storyLineLayoutType != '2' || storyLine != true"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding mb-md-4">

                    <div v-if="error !== ''" class="alert alert-danger">
                        {{error}}
                        <button type="button" class="close" aria-label="Close" @click="error = ''">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <select class="form-control" v-model="taskCodeForm" @change="getNumber()">
                                        <option v-for="taskCode in taskCodes" :value="taskCode">
                                            {{taskCode.competencyTypeName.charAt(0)}}-{{taskCode.prefixCode}}-{{taskCode.evaluationTypeName}}
                                        </option>
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
                                    <select class="form-control" v-model="moduleForm" @change="getNumber()">
                                        <option v-for="module in modules" :value="module">{{module.moduleName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <h5>Question<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-2 text-center">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 talent_nopaddingleft text-center"><h5>Score<span class="talent_font_red">*</span></h5></div>
                                        <div class="col-md-4 talent_nopaddingleft text-center"><h5>Points</h5></div>
                                    </div>
                                </div>
                            </div>

                            <div class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-6 talent_nopaddingright">
                                        <input class="form-control" v-model="taskQuestion" maxlength="2000"/>
                                    </div>
                                </div>
                            </div>


                            <!--ForEach-->
                            <div v-for="(option, index) in options" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 ">
                                        <label>{{index + 1}}</label>
                                    </div>
                                    <div class="col-md-5 talent_nopaddingleft">
                                        <div class="row">
                                            <div class="col-md-12 talent_nopaddingright">
                                                <input class="form-control form-control-sm" v-model="options[index].text" maxlength="2000"/>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2 d-flex justify-content-center">
                                        <div>
                                            <checkbox class="talent_checkboxnolabel talent-checkbox-lineheight"
                                                      v-model="options[index].check"
                                                      color="#0085CA"
                                                      @change="changeScore(index)"
                                                      :size="31"></checkbox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 ">
                                        <div class="row">
                                            <div class="col-md-6 talent_nopaddingleft">
                                                <select class="form-control" v-model="options[index].timePointId" @change="generatePoint(index)" :disabled="!options[index].check">
                                                    <option v-for="point in points" :value="point.timePointId">{{point.score}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-4 talent_nopaddingleft">
                                                <input v-model="options[index].point" class="form-control form-control-sm " disabled />
                                            </div>
                                            <div v-if="options.length !== 2" class="col-md-1 talent_nopadding text-center" @click.prevent="remove(index)">
                                                <a class="h-100 w-100 d-flex align-items-center talent_cursorpoint justify-content-center"><fa icon="trash-alt"></fa></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="talent_nopadding text-center">
                                <a class="talent_cursorpoint" @click.prevent="add()"><fa icon="plus-circle"></fa> Add New Choice</a>
                            </div>

                        </div>
                    </div>
                </div>

                <!--Total-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
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
                                            <input class="form-control" v-model="totalScore" disabled />
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
                                            <input class="form-control" v-model="totalPoints" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white"
                                            @click="close()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="update()">
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
        <div class="row" v-if="mode === 'View'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup > Task > Checklist > <span class="talent_font_red">View Detail</span></h3>

                <!--STORYLINE-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding mb-md-4">
                    <div class="row d-flex justify-content-center mb-md-4">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox talent-checkbox-lineheight"
                                          v-model="storyLine"
                                          color="#0085CA" :size="16" :disabled="true"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="storyLineLayoutType"
                                                   type="radio"
                                                   name="typeofstoryline"
                                                   id="vertical"
                                                   :value="1"
                                                   disabled /> <label for="vertical"><b>Type 1</b></label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <div class="row d-flex justify-content-center">
                                        <div class="justify-content-between align-items-center">
                                            <input v-model="storyLineLayoutType"
                                                   type="radio"
                                                   name="typeofstoryline"
                                                   id="horizontal"
                                                   :value="2"
                                                   disabled /> <label for="horizontal"><b>Type 2</b></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row mb-md-4">
                                        <div class="col-md-12 mb-md-5">
                                            <img class="h-100 w-100" :src="ImageData1.length ? ImageData1 : '/upload-image2.png'" />
                                            <div class="custom-file">
                                                <input type="file"
                                                       class="custom-file-input"
                                                       id="customFile"
                                                       @change="handler"
                                                       disabled />
                                                <label class="custom-file-label talent_textshorten talent_noroundborder_top"
                                                       for="customFile">{{ImageName1.length ? ImageName1 : 'Choose File'}}</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea class="form-control talent_textarea"
                                                      v-model="storyLineDescription1"
                                                      disabled></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row h-100">
                                        <div class="col-md-7">
                                            <div class="h-75">
                                                <img class="h-100 w-100" :src="ImageData2.length ? ImageData2 : '/upload-image2.png'" />
                                                <div class="custom-file">
                                                    <input type="file"
                                                           class="custom-file-input"
                                                           id="customFile"
                                                           @change="handler"
                                                           disabled />
                                                    <label class="custom-file-label talent_textshorten talent_noroundborder_top"
                                                           for="customFile">{{ImageName2.length ? ImageName2 : 'Choose File'}}</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5 ">
                                            <textarea class="form-control h-100 talent_overflowy"
                                                      v-model="storyLineDescription2"
                                                      disabled></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding mb-md-4">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6">
                                    <h5>Task Code<span class="talent_font_red">*</span></h5>
                                    <select class="form-control" v-model="taskCodeForm" disabled>
                                        <option v-for="taskCode in taskCodes" :value="taskCode">
                                            {{taskCode.competencyTypeName.charAt(0)}}-{{taskCode.prefixCode}}-{{taskCode.evaluationTypeName}}
                                        </option>
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
                                    <select class="form-control" v-model="moduleForm" disabled>
                                        <option v-for="module in modules" :value="module">{{module.moduleName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row">
                                <div class="col-md-6 ">
                                    <h5>Question<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-2 text-center">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 talent_nopaddingleft text-center"><h5>Score<span class="talent_font_red">*</span></h5></div>
                                        <div class="col-md-4 talent_nopaddingleft text-center"><h5>Points</h5></div>
                                    </div>
                                </div>
                            </div>

                            <div class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-6 talent_nopaddingright">
                                        <input class="form-control" v-model="taskQuestion" disabled />
                                    </div>
                                </div>
                            </div>


                            <!--ForEach-->
                            <div v-for="(option, index) in options" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 ">
                                        <label>{{index + 1}}</label>
                                    </div>
                                    <div class="col-md-5 talent_nopaddingleft">
                                        <div class="row">
                                            <div class="col-md-12 talent_nopaddingright">
                                                <input class="form-control form-control-sm" v-model="options[index].text" disabled />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-2 d-flex justify-content-center">
                                        <div>
                                            <checkbox class="talent_checkboxnolabel talent-checkbox-lineheight"
                                                      v-model="options[index].check"
                                                      color="#0085CA"
                                                      :size="31"
                                                      :disabled="true"></checkbox>
                                        </div>
                                    </div>
                                    <div class="col-md-4 ">
                                        <div class="row">
                                            <div class="col-md-6 talent_nopaddingleft">
                                                <select class="form-control" v-model="options[index].timePointId" disabled>
                                                    <option v-for="point in points" :value="point.timePointId">{{point.score}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-4 talent_nopaddingleft">
                                                <input v-model="options[index].point" class="form-control form-control-sm " disabled />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--Total-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
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
                                            <input class="form-control" v-model="totalScore" disabled />
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
                                            <input class="form-control" v-model="totalPoints" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!--SUBMIT-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white"
                                            @click="backPage()">
                                        Close
                                    </button>
                                    <button v-else class="btn talent_bg_red talent_font_white"
                                            @click="close()">
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
    import { ChecklistForm } from '../../../models/IChecklistContainer';
    import { BlobService } from '../../../services/BlobService';
    import { ChecklistService, CompetencyMappingJoinModel, ModuleForTaskModel, TimePointTaskModel, ChecklistCreateModel, ChecklistViewDetailModel, ChecklistUpdateModel, FileContent, TaskService, } from '../../../services/NSwagService';

    @Component({
        created: async function (this: Checklist) {
            await this.FillOption();
            if (this.mode === 'View' || this.mode === 'Edit') {
                await this.getTask();
            }
        },

        props: ['mode', 'taskId', 'fromOutside']
    })

    export default class Checklist extends Vue {
        Service: ChecklistService = new ChecklistService();

        mode: string;
        taskId: number;
        fromOutside: boolean;

        isLoading: boolean = false;

        taskCodes = {};
        modules = {};

        taskEssayMan: ChecklistService = new ChecklistService();
        taskMan: TaskService = new TaskService();

        /*
         * container for all storyline form
         * */
        storyLine: boolean = false;
        storyLineLayoutType: number = 0;

        storyLineDescription1: string = null;
        storyLineDescription2: string = null;

        /*
         * container for image upload file
         * */
        ImageData1: any = [];
        ImageData2: any = [];

        ImageName1: string = '';
        ImageName2: string = '';

        //storyLineFormData1: FormData = new FormData();
        //storyLineFormData2: FormData = new FormData();

        fileStoryLine1: FileContent = {
            base64: '',
            contentType: '',
            fileName: ''
        };
        fileStoryLine2: FileContent = {
            base64: '',
            contentType: '',
            fileName: ''
        };

        isChangeImage: boolean = false;

        /*
         * to contain total number of point and score,
         * computed in getNumber()
         * */
        totalPoints: number = 0;
        totalScore: number = 0;

        // taskCodes: CompetencyMappingJoinModel[] = [];
        taskCodeForm: CompetencyMappingJoinModel = {
            competencyId: 0,
            competencyTypeId: 0,
            evaluationTypeId: 0,
            prefixCode: '',
            competencyTypeName: '',
            evaluationTypeName: ''
        };

        // modules: ModuleForTaskModel[] = [];
        moduleForm: ModuleForTaskModel = {
            moduleId: 0,
            moduleName: ''
        };

        points: TimePointTaskModel[] = [];
        taskNumber: number = 0;

        checkboxValue: Array<string> = [];

        taskQuestion: string = '';

        options: ChecklistForm[] = [{ text: "", check: false, point: 0, score: 0, choiceNumber: 0, timePointId: 0 },
        { text: "", check: false, point: 0, score: 0, choiceNumber: 0, timePointId: 0  },
        { text: "", check: false, point: 0, score: 0, choiceNumber: 0, timePointId: 0  },
        { text: "", check: false, point: 0, score: 0, choiceNumber: 0, timePointId: 0  }];

        error: string = '';

        checklistInsertForm: ChecklistCreateModel = {
            task: {},
            question: '',
            choices: [],
            fileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        checklistDetail: ChecklistViewDetailModel = {};
        checklistUpdateForm: ChecklistUpdateModel = {
            task: {},
            question: '',
            competencyTypeId: 0,
            choices: [],
            fileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        };

        async FillOption() {
            this.isLoading = true;
            this.taskCodes = await this.Service.getTaskCode();
            this.modules = await this.Service.getModule();
            this.points = await this.Service.getTimePoint();
            
            this.options.forEach((v, index) => {
                this.changeScore(index)
            })
            this.isLoading = false;
        }

        add() {
            this.options.push(new ChecklistForm);
            this.changeScore(this.options.length - 1)
        }

        remove(index: number) {
            if (this.options.length <= 1) {
                return
            }

            this.options.splice(index, 1);
        }

        changeScore(index: number){
            if(this.options[index].check == false){
                var zeroScoreTimePoint = this.points.find(Q => Q.score == 0);
                if(zeroScoreTimePoint == undefined){
                    zeroScoreTimePoint = this.points[0];
                }

                this.options[index].timePointId = zeroScoreTimePoint.timePointId;
                this.generatePoint(index);
            }
        }

        generatePoint(index: number) {
            let indexTimePoint = this.points.findIndex(Q => Q.timePointId === this.options[index].timePointId);
            this.options[index].point = this.points[indexTimePoint].points;
            this.options[index].score = this.points[indexTimePoint].score;

            this.totalScore = 0;
            this.totalPoints = 0;

            for (let option of this.options) {
                this.totalScore += option.score;
                this.totalPoints += option.point;
            }
        }

        async getNumber() {
            if (this.taskCodeForm.competencyId === 0 || this.moduleForm.moduleId === 0) {
                return;
            }

            if (this.mode === 'edit' &&
                (this.taskCodeForm.competencyId === this.checklistDetail.task.competencyId &&
                    this.taskCodeForm.competencyTypeId === this.checklistDetail.competencyTypeId &&
                    this.taskCodeForm.evaluationTypeId === this.checklistDetail.evaluationTypeId &&
                    this.moduleForm.moduleId === this.checklistDetail.task.moduleId)) {
                this.taskNumber = this.checklistDetail.task.taskNumber;
                return;
            }

            this.taskNumber = await this.Service.getNumber(this.taskCodeForm.competencyId, this.moduleForm.moduleId, this.taskCodeForm.evaluationTypeId);
        }

        check(): boolean {
            if (this.storyLine === true && this.storyLineLayoutType === 0) {
                this.error = 'Layout type must be choose';
                return false;
            }

            if ((this.storyLine === true && this.storyLineLayoutType === 1 && this.storyLineDescription1 == null && this.ImageName1 === '' )||
                (this.storyLine === true && this.storyLineLayoutType === 2 && this.storyLineDescription2 == null && this.ImageName2 === '')) {
                this.error = 'Either story line image or description must be filled';
                return false;
            }

            if (this.storyLine && this.storyLineDescription1 != null) {
                if (this.storyLineLayoutType == 1 && this.storyLineDescription1.length > 5000) {
                    this.error = 'Storyline description must be less than 5000 characters';
                    return false;
                }
            }

            if (this.storyLine && this.storyLineDescription2 != null) {
                if (this.storyLineLayoutType == 2 && this.storyLineDescription2.length > 5000) {
                    this.error = 'Storyline description must be less than 5000 characters';
                    return false;
                }
            }

            if (this.taskCodeForm.competencyId === 0) {
                this.error = 'Task code must be filled';
                return false;
            }

            if (this.moduleForm.moduleId === 0) {
                this.error = 'Module must be filled';
                return false;
            }

            if (this.taskQuestion === '') {
                this.error = 'Question must be filled';
                return false;
            }

            if (this.taskQuestion.length > 2000) {
                this.error = 'Question must be less than 2000 characters';
                return false;
            }

            let hasCheckAnswer = false;

            for (let choice of this.options) {
                if (choice.text === '') {
                    this.error = 'Choice must be filled';
                    return false;
                }

                if (choice.score < 0) {
                    this.error = 'Score must be filled';
                    return false;
                }

                if (choice.check === true) {
                    hasCheckAnswer = true;
                }
            }

            if (hasCheckAnswer === false) {
                this.error = 'At leat 1 choice must be checked';
                return false;
            }

            return true;
        }

        async insert() {
            let isTrue = this.check();

            if (isTrue === false) {
                return;
            }

            this.isLoading = true;

            if (this.storyLine === true && this.storyLineLayoutType === 1 && this.ImageName1 !== '') {
                this.checklistInsertForm.fileContent = this.fileStoryLine1;
            }

            if (this.storyLine === true && this.storyLineLayoutType === 2 && this.ImageName2 !== '') {
                this.checklistInsertForm.fileContent = this.fileStoryLine2;
            }

            this.checklistInsertForm.task.competencyId = this.taskCodeForm.competencyId;
            this.checklistInsertForm.task.moduleId = this.moduleForm.moduleId;
            this.checklistInsertForm.task.evaluationTypeId = this.taskCodeForm.evaluationTypeId;
            this.checklistInsertForm.task.taskNumber = this.taskNumber;
            this.checklistInsertForm.task.layoutType = this.storyLine === true ? this.storyLineLayoutType : 0;

            if (this.storyLineLayoutType === 1 && this.storyLine === true) {
                this.checklistInsertForm.task.storyLineDescription = this.storyLineDescription1;
            }

            else if (this.storyLineLayoutType === 2 && this.storyLine === true) {
                this.checklistInsertForm.task.storyLineDescription = this.storyLineDescription2;
            }

            this.checklistInsertForm.question = this.taskQuestion;

            let choiceNumber: number = 1;

            for (let choice of this.options) {
                this.checklistInsertForm.choices.push({
                    text: choice.text,
                    number: choiceNumber,
                    score: choice.timePointId,
                    isAnswer: choice.check
                })

                choiceNumber++;
            }

            await this.Service.insertChecklistTask(this.checklistInsertForm);
            this.isLoading = false;
            this.close();
        }

        async getTask() {
            this.isLoading = true;
            this.checklistDetail = await this.Service.getTaskChecklist(this.taskId);

            this.checklistUpdateForm.task.blobId = this.checklistDetail.task.blobId;

            if (this.checklistDetail.task.layoutType < 1 &&
                (this.checklistDetail.task.storyLineDescription === '' || this.checklistDetail.task.storyLineDescription === null)) {
                this.storyLine = false;
                this.storyLineLayoutType = 0;
            }

            else {
                this.storyLine = true;
                this.storyLineLayoutType = this.checklistDetail.task.layoutType;

                if (this.storyLineLayoutType === 1) {

                    if (this.checklistDetail.task.blobId !== null) {
                        this.ImageData1 = await BlobService.getImageUrl(this.checklistDetail.task.blobId, this.checklistDetail.blobImageFileType);
                        this.ImageName1 = this.checklistDetail.blobImageName;
                    }

                    this.storyLineDescription1 = this.checklistDetail.task.storyLineDescription;
                }

                else {

                    if (this.checklistDetail.task.blobId !== null) {
                        this.ImageData2 = await BlobService.getImageUrl(this.checklistDetail.task.blobId, this.checklistDetail.blobImageFileType);
                        this.ImageName2 = this.checklistDetail.blobImageName;
                    }

                    this.storyLineDescription2 = this.checklistDetail.task.storyLineDescription;
                }
            }

            this.taskCodeForm = {
                competencyId: this.checklistDetail.task.competencyId,
                competencyTypeId: this.checklistDetail.competencyTypeId,
                evaluationTypeId: this.checklistDetail.evaluationTypeId,
                prefixCode: this.checklistDetail.prefixCode,
                competencyTypeName: this.checklistDetail.competencyTypeName,
                evaluationTypeName: this.checklistDetail.evaluationTypeName,
                competencyNameMapping: null
            }

            this.moduleForm = {
                moduleId: this.checklistDetail.task.moduleId,
                moduleName: this.checklistDetail.moduleName
            }

            this.taskNumber = this.checklistDetail.task.taskNumber;

            this.taskQuestion = this.checklistDetail.question;

            this.options = [];
            let tempPoints = await this.Service.getTimePoint();

            for (let choice of this.checklistDetail.choices) {
                this.options.push({
                    text: choice.text,
                    check: choice.isAnswer,
                    score: choice.score,
                    point: tempPoints.find(Q => Q.score == choice.score).points,
                    choiceNumber: choice.number,
                    timePointId: tempPoints.find(Q => Q.score == choice.score).timePointId
                })
            }

            this.options.sort((a, b) => a.choiceNumber - b.choiceNumber);

            this.totalScore = this.checklistDetail.totalScore;
            this.totalPoints = this.checklistDetail.totalPoint;
            this.isLoading = false;
        }

        async update() {
            let isTrue = this.check();

            if (isTrue === false) {
                return;
            }

            this.isLoading = true;

            if (this.storyLine === false) {
                this.storyLine = false;
                this.storyLineLayoutType = 0;
            }

            if (this.storyLine === true && this.storyLineLayoutType === 1 && this.ImageName1 != null && this.isChangeImage === true) {
                this.checklistUpdateForm.fileContent = this.fileStoryLine1;
            }

            if (this.storyLine === true && this.storyLineLayoutType === 2 && this.ImageName2 != null && this.isChangeImage === true) {
                this.checklistUpdateForm.fileContent = this.fileStoryLine2;
            }

            this.checklistUpdateForm.task.competencyId = this.taskCodeForm.competencyId;
            this.checklistUpdateForm.task.moduleId = this.moduleForm.moduleId;
            this.checklistUpdateForm.task.evaluationTypeId = this.taskCodeForm.evaluationTypeId;
            this.checklistUpdateForm.task.blobId = this.checklistDetail.task.blobId;
            this.checklistUpdateForm.task.taskNumber = this.taskNumber;
            this.checklistUpdateForm.task.layoutType = this.storyLine === true ? this.storyLineLayoutType : 0;

            if (this.storyLineLayoutType === 1 && this.storyLine === true) {
                this.checklistUpdateForm.task.storyLineDescription = this.storyLineDescription1;
            }

            else if (this.storyLineLayoutType === 2 && this.storyLine === true) {
                this.checklistUpdateForm.task.storyLineDescription = this.storyLineDescription2;
            }

            this.checklistUpdateForm.task.createdAt = this.checklistDetail.task.createdAt;

            this.checklistUpdateForm.question = this.checklistDetail.question;
            this.checklistUpdateForm.competencyTypeId = this.checklistDetail.competencyTypeId;

            let choiceNumber: number = 1;

            for (let choice of this.options) {
                this.checklistUpdateForm.choices.push({
                    number: choiceNumber,
                    text: choice.text,
                    isAnswer: choice.check,
                    score: choice.timePointId
                })

                choiceNumber++;
            }

            await this.Service.updateChecklistTask(this.checklistUpdateForm, this.taskId);
            this.isChangeImage = false;
            this.isLoading = false;
            this.close();
        }

        close() {
            window.location.href = "/Setup/Tasks";
        }

        async handler($event) {
            if ($event.target.files.length === 0) {
                return;
            }

            this.loadFile($event);
            await this.fileChange($event);
        }

        async fileChange(e) {
            this.isChangeImage = true;

            let baseFileInput = (<HTMLInputElement>e.target).files[0];
            let base64String = await this.convertToBase64(baseFileInput);
            let extension = baseFileInput.name.split(".").pop();

            if (this.storyLineLayoutType === 1) {
                //this.storyLineFormData1.set(e.target.files, e.target.files[0], e.target.files[0].name);
                this.fileStoryLine1.base64 = base64String;
                this.fileStoryLine1.contentType = extension;
                this.fileStoryLine1.fileName = baseFileInput.name;
            }

            else {
                //this.storyLineFormData2.set(e.target.files, e.target.files[0], e.target.files[0].name);
                this.fileStoryLine2.base64 = base64String;
                this.fileStoryLine2.contentType = extension;
                this.fileStoryLine2.fileName = baseFileInput.name;
            }
        }

        loadFile($event) {
            var reader = new FileReader();
            reader.onload = (e: Event) => {
                if (this.storyLineLayoutType === 1) {
                    this.ImageData1 = (<FileReader>e.target).result;
                }

                else {
                    this.ImageData2 = (<FileReader>e.target).result;
                }
            }

            reader.readAsDataURL($event.target.files[0]);
            if (this.storyLineLayoutType === 1) {
                this.ImageName1 = $event.target.files[0].name;
            }

            else {
                this.ImageName2 = $event.target.files[0].name;
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

        backPage() {
            window.history.back();
        }

    }

</script>
