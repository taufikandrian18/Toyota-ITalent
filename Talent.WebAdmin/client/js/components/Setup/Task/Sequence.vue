<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <!--ADD-->
        <div class="row" v-if="mode === 'Add'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup > Task > Sequence > <span class="talent_font_red">Add</span></h3>

                <!--Story line option-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding mb-md-4">
                    <div class="row d-flex justify-content-center mb-md-4">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox talent_nomargin"
                                          style="line-height: 50%;" v-model="storyLine"
                                          color="#0085CA" :size="16"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>

                    <!--STORYLINE-->
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
                                                   :disabled="storyLine != true" /> <label for="vertical"><b>Type 1</b></label>
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
                                                   :disabled="storyLine != true" /> <label for="horizontal"><b>Type 2</b></label>
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
                                            <textarea class="form-control talent_textarea" maxlength="5000" v-model="storyLineDescription1" :disabled="storyLineLayoutType != '1' || storyLine != true"></textarea>
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
                                            <textarea class="form-control h-100 talent_overflowy" maxlength="5000" v-model="storyLineDescription2" :disabled="storyLineLayoutType != '2' || storyLine != true"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding mb-md-4">

                    <div v-if="error !=''" class="alert alert-danger">
                        {{error}}
                        <button type="button" class="close" aria-label="Close" @click="error = ''">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="row mb-md-4">
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
                                <div class="col-md-7 ">
                                    <h5>Question<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-5 talent_nopaddingleft">
                                    <div class="row">
                                        <div class="col-md-5 talent_nopaddingleft text-center"><h5>Score<span class="talent_font_red">*</span></h5></div>
                                        <div class="col-md-3 talent_nopaddingleft text-center"><h5>Points</h5></div>
                                        <div class="col-md-4"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-7">
                                        <input class="form-control" maxlength="2000" v-model="sequence.question" />
                                    </div>
                                </div>
                            </div>


                            <!--ForEach-->
                            <div v-for="(choice, index) in sequence.choice" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 ">
                                        {{index + 1}}
                                    </div>
                                    <div class="col-md-6 talent_nopaddingleft">
                                        <input class="form-control form-control-sm" v-model="sequence.choice[index].text" placeholder="New Choice" maxlength="2000" />
                                    </div>
                                    <div class="col-md-5 ">
                                        <div class="row">
                                            <div class="col-md-5 talent_nopaddingleft">
                                                <select class="form-control" v-model="sequence.choice[index].timePointId" @change="generatePoint(index)">
                                                    <option v-for="point in points" :value="point.timePointId">{{point.score}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-3 talent_nopaddingleft">
                                                <input class="form-control form-control-sm" disabled v-model="sequence.choice[index].point" />
                                            </div>
                                            <div v-if="sequence.choice.length > 2" class="col-md-2 talent_nopaddingleft text-center" @click.prevent="remove(index)">
                                                <a class="h-100 w-100 d-flex align-items-center talent_cursorpoint"><fa icon="trash-alt"></fa></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-7">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <input class="form-control" v-model="sequence.answer" placeholder="1-2-3-4-5" />
                                </div>
                                <div class="col-md-5 row">
                                    <a class="talent_cursorpoint" id="addButton" @click.prevent="add()">
                                        <fa icon="plus-circle"></fa>
                                        <label class="p-0 talent_cursorpoint" for="addButton">Buat Pilihan Baru</label>
                                    </a>
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
                                    <button class="btn talent_bg_red talent_font_white"
                                            @click="close()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white"
                                            @click="insert()">
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
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup > Task > Sequence > <span class="talent_font_red">Edit</span></h3>

                <!--Story line option-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding mb-md-4">
                    <div class="row d-flex justify-content-center mb-md-4">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox talent_nomargin"
                                          style="line-height: 50%;" v-model="storyLine"
                                          color="#0085CA" :size="16"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>

                    <!--STORYLINE-->
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
                                                   :disabled="storyLine != true" /> <label for="vertical"><b>Type 1</b></label>
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
                                                   :disabled="storyLine != true" /> <label for="horizontal"><b>Type 2</b></label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="talent_bg_grey talent_padding h-100">
                                    <div class="row">
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
                                            <textarea class="form-control talent_textarea" maxlength="5000" v-model="storyLineDescription1" :disabled="storyLineLayoutType != '1' || storyLine != true"></textarea>
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
                                            <textarea class="form-control h-100 talent_overflowy" maxlength="5000" v-model="storyLineDescription2" :disabled="storyLineLayoutType != '2' || storyLine != true"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <!--FORM-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_bg_shadow talent_padding mb-md-4">

                    <div v-if="error !=''" class="alert alert-danger">
                        {{error}}
                        <button type="button" class="close" aria-label="Close" @click="error = ''">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>

                    <div class="row">
                        <div class="col-md-6">
                            <div class="row mb-md-4">
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
                                <div class="col-md-7 ">
                                    <h5>Question<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-5 talent_nopaddingleft">
                                    <div class="row">
                                        <div class="col-md-5 talent_nopaddingleft text-center"><h5>Score<span class="talent_font_red">*</span></h5></div>
                                        <div class="col-md-3 talent_nopaddingleft text-center"><h5>Points</h5></div>
                                        <div class="col-md-4"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-7">
                                        <input class="form-control" maxlength="2000" v-model="sequence.question" />
                                    </div>
                                </div>
                            </div>


                            <!--ForEach-->
                            <div v-for="(choice, index) in sequence.choice" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 ">
                                        {{index + 1}}
                                    </div>
                                    <div class="col-md-6 talent_nopaddingleft">
                                        <input class="form-control form-control-sm" v-model="sequence.choice[index].text" placeholder="New Choice" maxlength="2000" />
                                    </div>
                                    <div class="col-md-5 ">
                                        <div class="row">
                                            <div class="col-md-5 talent_nopaddingleft">
                                                <select class="form-control" v-model="sequence.choice[index].timePointId" @change="generatePoint(index)">
                                                    <option v-for="point in points" :value="point.timePointId">{{point.score}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-3 talent_nopaddingleft">
                                                <input class="form-control form-control-sm" disabled v-model="sequence.choice[index].point" />
                                            </div>
                                            <div v-if="sequence.choice.length > 2" class="col-md-2 talent_nopaddingleft text-center" @click.prevent="remove(index)">
                                                <a class="h-100 w-100 d-flex align-items-center talent_cursorpoint"><fa icon="trash-alt"></fa></a>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-7">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <input class="form-control" v-model="sequence.answer" placeholder="1-2-3-4-5" />
                                </div>
                                <div class="col-md-5">
                                    <a class="talent_cursorpoint" id="addButton" @click.prevent="add()">
                                        <fa icon="plus-circle"></fa>
                                        <label class="p-0 talent_cursorpoint" for="addButton">Buat Pilihan Baru</label>
                                    </a>
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
                                    <button class="btn talent_bg_red talent_font_white"
                                            @click="close()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white"
                                            @click="update()">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <!--DETAIL-->
        <div class="row" v-if="mode === 'View'">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup > Task > Sequence > <span class="talent_font_red">View Detail</span></h3>

                <!--Story line option-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow talent_padding mb-md-4">
                    <div class="row d-flex justify-content-center mb-md-4">
                        <div class="col-md-12 d-flex justify-content-center"><h3>Use Story Line?</h3></div>
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="justify-content-between align-items-center">
                                <checkbox class="talent_checkbox talent_nomargin"
                                          style="line-height: 50%;" v-model="storyLine"
                                          color="#0085CA" :size="16" :disabled="true"> Type of Story Line</checkbox>
                            </div>
                        </div>
                    </div>

                    <!--STORYLINE-->
                    <div class="col-md-12 talent_bg_darkgrey talent_roundborder talent_bg_shadow talent_padding mb-md-4">
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
                                                       id="customFile" disabled />
                                                <label class="custom-file-label talent_textshorten talent_noroundborder_top"
                                                       for="customFile">{{ImageName1.length ? ImageName1 : 'Choose File'}}</label>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <textarea class="form-control talent_textarea" v-model="storyLineDescription1" disabled></textarea>
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
                                                           id="customFile" disabled />
                                                    <label class="custom-file-label talent_textshorten talent_noroundborder_top"
                                                           for="customFile">{{ImageName2.length ? ImageName2 : 'Choose File'}}</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5 ">
                                            <textarea class="form-control h-100 talent_overflowy" v-model="storyLineDescription2" disabled></textarea>
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
                            <div class="row mb-md-4">
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
                                <div class="col-md-7 ">
                                    <h5>Question<span class="talent_font_red">*</span></h5>
                                </div>
                                <div class="col-md-5 talent_nopaddingleft">
                                    <div class="row">
                                        <div class="col-md-5 talent_nopaddingleft text-center"><h5>Score<span class="talent_font_red">*</span></h5></div>
                                        <div class="col-md-3 talent_nopaddingleft text-center"><h5>Points</h5></div>
                                        <div class="col-md-4"></div>
                                    </div>
                                </div>
                            </div>

                            <div class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-7">
                                        <input class="form-control" v-model="sequence.question" disabled />
                                    </div>
                                </div>
                            </div>


                            <!--ForEach-->
                            <div v-for="(choice, index) in sequence.choice" class="talent_marginbottom">
                                <div class="row">
                                    <div class="col-md-1 ">
                                        {{index + 1}}
                                    </div>
                                    <div class="col-md-6 talent_nopaddingleft">
                                        <input class="form-control form-control-sm" v-model="sequence.choice[index].text" placeholder="New Choice" disabled />
                                    </div>
                                    <div class="col-md-5 ">
                                        <div class="row">
                                            <div class="col-md-6 talent_nopaddingleft">
                                                <!--<input type="text" class="form-control" v-model="sequence.choice[index].score" disabled />-->
                                                <select class="form-control" v-model="sequence.choice[index].timePointId" @change="generatePoint(index)" disabled>
                                                    <option v-for="point in points" :value="point.timePointId">{{point.score}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-4 talent_nopaddingleft">
                                                <input class="form-control form-control-sm" disabled v-model="sequence.choice[index].point" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-7">
                                    <h5>Answer<span class="talent_font_red">*</span></h5>
                                    <input class="form-control" v-model="sequence.answer" placeholder="1-2-3-4-5" disabled />
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
    import { BlobService } from '../../../services/BlobService';
    import { CompetencyMappingJoinModel, SequenceService, ModuleForTaskModel, SequenceCreateModel, TaskModel, SequenceChoiceModel, TimePointTaskModel, SequenceViewDetailModel, SequenceUpdateModel, FileContent, TaskService } from '../../../services/NSwagService'


    @Component({
        created: async function (this: Sequence) {
            await this.fillOption();
            if (this.mode === 'View' || this.mode === 'Edit') {
                await this.getTask();
            }
        },

        props: ['mode', 'taskId', 'fromOutside']
    })

    export default class Sequence extends Vue {
        mode: string;
        taskId: number;
        fromOutside: boolean;
        taskService: TaskService = new TaskService();
        isLoading: boolean = false;

        Service: SequenceService = new SequenceService();

        taskCodes: CompetencyMappingJoinModel[] = [];
        taskCodeForm: CompetencyMappingJoinModel = {
            competencyId: 0,
            competencyTypeId: 0,
            evaluationTypeId: 0,
            prefixCode: '',
            competencyTypeName: '',
            evaluationTypeName: ''
        };

        modules: ModuleForTaskModel[] = [];
        moduleForm: ModuleForTaskModel = {
            moduleId: 0,
            moduleName: ''
        };

        taskNumber: number = 0;

        /*
         * untuk nampung semua story line
         * */
        storyLine: boolean = false;
        storyLineLayoutType: number = 0;

        storyLineDescription1: string = '';
        storyLineDescription2: string = '';

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

        storyLineImageFileType1: string = '';
        storyLineImageFileType2: string = '';

        /*
         *
         * */
        ImageData1: any = [];
        ImageName1: string = '';

        ImageData2: any = [];
        ImageName2: string = '';

        /*
         * untuk masukin form
         * */
        sequenceInsertForm: SequenceCreateModel = {
            task: {},
            question: '',
            answer: '',
            choices: [],
            fileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        points: TimePointTaskModel[] = [];
        totalScore: number = 0;
        totalPoints: number = 0;

        taskForm: TaskModel = {
            taskId: 0,
            competencyId: 0,
            questionTypeId: 0,
            moduleId: 0,
            evaluationTypeId: 0,
            blobId: '',
            taskNumber: 0,
            layoutType: 0,
            storyLineDescription: null,
            createdBy: ''
        }

        defaultImage: string = '/upload-image2.png';

        sequenceChoice: SequenceChoiceModel[] = [];

        sequence: ISequenceContainer = {
            question: '',
            answer: '',
            choice: [{ text: '', point: null, score: null, timePointId: null },
            { text: '', point: null, score: null, timePointId: null }]
        }

        sequenceDetail: SequenceViewDetailModel = {};
        sequenceUpdateForm: SequenceUpdateModel = {
            task: {},
            answer: '',
            choices: [],
            question: '',
            fileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        isChangeImage: boolean = false;

        error: string = '';
        checkAnswerNumber: string[] = ['1', '2'];

        async fillOption() {
            this.taskCodes = await this.Service.getTaskCode();
            this.modules = await this.Service.getModuleOption();
            this.points = await this.taskService.getAllTimePointsForTask();
        }

        async getNumber() {
            if (this.taskCodeForm.competencyId === 0 || this.moduleForm.moduleId === 0) {
                return;
            }

            if (this.mode === 'edit' &&
                (this.taskCodeForm.competencyId === this.sequenceDetail.task.competencyId &&
                    this.taskCodeForm.competencyTypeId === this.sequenceDetail.competencyTypeId &&
                    this.taskCodeForm.evaluationTypeId === this.sequenceDetail.evaluationTypeId &&
                    this.moduleForm.moduleId === this.sequenceDetail.task.moduleId)) {
                this.taskNumber = this.sequenceDetail.task.taskNumber;
                return;
            }

            this.taskNumber = await this.Service.getTaskNumber(this.taskCodeForm.competencyId, this.moduleForm.moduleId, this.taskCodeForm.evaluationTypeId);
        }

        add() {
            this.sequence.choice.push({ text: '', point: 0, score: 0, timePointId: 0 });
            let nextNumber = String(this.checkAnswerNumber.length + 1);
            this.checkAnswerNumber.push(nextNumber);
        }

        remove(index: number) {
            if (this.sequence.choice.length <= 1) {
                return;
            }

            this.sequence.choice.splice(index, 1);
            this.checkAnswerNumber.pop();

            this.totalScore = 0;
            this.totalPoints = 0;

            for (let choice of this.sequence.choice) {
                this.totalScore += choice.score;
                this.totalPoints += choice.point;
            }
        }

        generatePoint(index: number) {
            let indexTimePoint = this.points.findIndex(Q => Q.timePointId === this.sequence.choice[index].timePointId);
            this.sequence.choice[index].point = this.points[indexTimePoint].points;
            this.sequence.choice[index].score = this.points[indexTimePoint].score;

            this.totalScore = 0;
            this.totalPoints = 0;

            for (let choice of this.sequence.choice) {
                this.totalScore += choice.score;
                this.totalPoints += choice.point;
            }
        }

        check(): boolean {
            if (this.storyLine === true && this.storyLineLayoutType === 0) {
                this.error = 'Layout type must be choose';
                return false;
            }

            if (this.storyLine === true && this.storyLineLayoutType === 1 && this.storyLineDescription1 == '' && this.ImageName1 === '' ||
                this.storyLine === true && this.storyLineLayoutType === 2 && this.storyLineDescription2 == '' && this.ImageName2 === '') {
                this.error = 'Either story line image or description must be filled';
                return false;
            }

            if (this.taskCodeForm.competencyId === 0) {
                this.error = 'Task code must be filled';
                return false;
            }

            if (this.moduleForm.moduleId === 0) {
                this.error = 'Module must be filled';
                return false;
            }

            if (this.sequence.question === '') {
                this.error = 'Question must be filled';
                return false;
            }

            for (let choice of this.sequence.choice) {
                if (choice.text === '') {
                    this.error = 'Choice must be filled';
                    return false;
                }

                if (choice.score === null) {
                    this.error = 'Score must be filled';
                    return false;
                }
            }

            if (this.sequence.answer === '') {
                this.error = 'Answer must be filled';
                return false;
            }

            let totalAnswer = this.sequence.answer.split('-');
            if (this.sequence.choice.length !== totalAnswer.length) {
                this.error = 'Answer and choice are not the same';
                return false;
            }

            let filterAnswer = totalAnswer.filter((v, i) => totalAnswer.indexOf(v) === i);
            if (this.sequence.choice.length !== filterAnswer.length) {
                this.error = 'There is a duplicate answer';
                return false;
            }

            let checker = (arr, target) => target.every(v => arr.includes(v));
            let isTrue = checker(this.checkAnswerNumber, filterAnswer);
            if (isTrue === false) {
                this.error = 'There is a missing number';
                return false;
            }

            let string = this.storyLineDescription1;
            if (this.storyLineLayoutType == 2) {
                string = this.storyLineDescription2;
            }
            if (string.length > 5000) {
                this.error = "Storyline description must be less than 5000 characters";
                return false;
            }


            if (this.sequence.question.length > 2000) {
                this.error = "Question must be less than 2000 characters";
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
                this.sequenceInsertForm.fileContent = this.fileStoryLine1;
            }

            if (this.storyLine === true && this.storyLineLayoutType === 2 && this.ImageName2 !== '') {
                this.sequenceInsertForm.fileContent = this.fileStoryLine2;
            }

            this.sequenceInsertForm.task.competencyId = this.taskCodeForm.competencyId;
            this.sequenceInsertForm.task.moduleId = this.moduleForm.moduleId;
            this.sequenceInsertForm.task.evaluationTypeId = this.taskCodeForm.evaluationTypeId;
            this.sequenceInsertForm.task.taskNumber = this.taskNumber;
            this.sequenceInsertForm.task.layoutType = this.storyLine === true ? this.storyLineLayoutType : 0;

            if (this.storyLineLayoutType === 1 && this.storyLine === true) {
                this.sequenceInsertForm.task.storyLineDescription = this.storyLineDescription1;
            }

            else if (this.storyLineLayoutType === 2 && this.storyLine === true) {
                this.sequenceInsertForm.task.storyLineDescription = this.storyLineDescription2;
            }

            this.sequenceInsertForm.question = this.sequence.question;
            this.sequenceInsertForm.answer = this.sequence.answer;

            let choiceNumber: number = 1;

            for (let choice of this.sequence.choice) {
                this.sequenceChoice.push({
                    choiceText: choice.text,
                    choiceNumber: choiceNumber,
                    score: choice.timePointId
                })

                choiceNumber++;
            }

            this.sequenceInsertForm.choices = this.sequenceChoice;

            await this.Service.insertSequenceTask(this.sequenceInsertForm);

            this.isLoading = false;
            window.location.href = "/Setup/Tasks";
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

            if (this.storyLine === true) {
                if (this.storyLineLayoutType === 1) {
                    if (this.ImageName1 != null && this.isChangeImage === true) {
                        this.sequenceUpdateForm.fileContent = this.fileStoryLine1;
                    }
                    else if (this.ImageData1 == '') {
                        this.sequenceUpdateForm.task.blobId = null;
                    }
                    else {
                        this.sequenceUpdateForm.task.blobId = this.sequenceDetail.task.blobId;
                    }
                }
                else {
                    if (this.ImageName2 != null && this.isChangeImage === true) {
                        this.sequenceUpdateForm.fileContent = this.fileStoryLine2;
                    }
                    else if (this.ImageName2 == '') {
                        this.sequenceUpdateForm.task.blobId = null;
                    }
                    else {
                        this.sequenceUpdateForm.task.blobId = this.sequenceDetail.task.blobId;
                    }
                }
            }
            this.sequenceUpdateForm.task.competencyId = this.taskCodeForm.competencyId;
            this.sequenceUpdateForm.task.moduleId = this.moduleForm.moduleId;
            this.sequenceUpdateForm.task.evaluationTypeId = this.taskCodeForm.evaluationTypeId;
            this.sequenceUpdateForm.task.taskNumber = this.taskNumber;
            this.sequenceUpdateForm.task.layoutType = this.storyLine === true ? this.storyLineLayoutType : 0;


            if (this.storyLineLayoutType === 1 && this.storyLine === true) {
                this.sequenceUpdateForm.task.storyLineDescription = this.storyLineDescription1;
            }
            else if (this.storyLineLayoutType === 2 && this.storyLine === true) {
                this.sequenceUpdateForm.task.storyLineDescription = this.storyLineDescription2;
            }

            this.sequenceUpdateForm.task.createdAt = this.sequenceDetail.task.createdAt;

            this.sequenceUpdateForm.question = this.sequence.question;
            this.sequenceUpdateForm.answer = this.sequence.answer;

            this.sequenceUpdateForm.competencyTypeId = this.sequenceDetail.competencyTypeId;

            let choiceNumber: number = 1;

            for (let choice of this.sequence.choice) {
                this.sequenceChoice.push({
                    choiceText: choice.text,
                    choiceNumber: choiceNumber,
                    score: choice.timePointId
                })

                choiceNumber++;
            }

            this.sequenceUpdateForm.choices = this.sequenceChoice;

            await this.Service.updateTaskSequence(this.sequenceUpdateForm, this.taskId);
            this.isChangeImage = false;

            this.isLoading = false;

            window.location.href = "/Setup/Tasks";
        }

        async getTask() {
            this.isLoading = true;
            this.sequenceDetail = await this.Service.getTaskSequence(this.taskId);

            if (this.sequenceDetail.task.layoutType < 1 &&
                (this.sequenceDetail.task.storyLineDescription === '' || this.sequenceDetail.task.storyLineDescription === null)) {
                this.storyLine = false;
                this.storyLineLayoutType = 0;
            }

            else {
                this.storyLine = true;
                this.storyLineLayoutType = this.sequenceDetail.task.layoutType;

                if (this.storyLineLayoutType === 1) {

                    if (this.sequenceDetail.task.blobId !== null) {
                        this.ImageData1 = await BlobService.getImageUrl(this.sequenceDetail.task.blobId, this.sequenceDetail.blobImageFileType);
                        this.ImageName1 = this.sequenceDetail.blobImageName;
                    }

                    this.storyLineDescription1 = this.sequenceDetail.task.storyLineDescription;
                }

                else {

                    if (this.sequenceDetail.task.blobId !== null) {
                        this.ImageData2 = await BlobService.getImageUrl(this.sequenceDetail.task.blobId, this.sequenceDetail.blobImageFileType);
                        this.ImageName2 = this.sequenceDetail.blobImageName;
                    }

                    this.storyLineDescription2 = this.sequenceDetail.task.storyLineDescription;
                }
            }

            this.taskCodeForm = {
                competencyId: this.sequenceDetail.task.competencyId,
                competencyTypeId: this.sequenceDetail.competencyTypeId,
                evaluationTypeId: this.sequenceDetail.evaluationTypeId,
                prefixCode: this.sequenceDetail.prefixCode,
                competencyTypeName: this.sequenceDetail.competencyTypeName,
                evaluationTypeName: this.sequenceDetail.evaluationTypeName,
                competencyNameMapping: null
            }

            this.moduleForm = {
                moduleId: this.sequenceDetail.task.moduleId,
                moduleName: this.sequenceDetail.moduleName
            }

            this.taskNumber = this.sequenceDetail.task.taskNumber;

            this.sequence.question = this.sequenceDetail.question;
            this.sequence.answer = this.sequenceDetail.answer;

            this.sequence.choice = [];
            this.checkAnswerNumber = [];
            let indexForCheckNumber: number = 1;
            let tempPoints = await this.Service.getTimePoint();

            for (let choice of this.sequenceDetail.choices) {
                this.sequence.choice.push({
                    text: choice.choiceText,
                    score: choice.score,
                    point: tempPoints.find(Q => Q.score == choice.score).points,
                    timePointId: tempPoints.find(Q => Q.score == choice.score).timePointId
                })

                this.checkAnswerNumber.push(String(indexForCheckNumber));
                indexForCheckNumber++;
            }

            this.totalScore = this.sequenceDetail.totalScore;
            this.totalPoints = this.sequenceDetail.totalPoint;

            this.isLoading = false;
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
                this.fileStoryLine1.base64 = base64String;
                this.fileStoryLine1.contentType = extension;
                this.fileStoryLine1.fileName = baseFileInput.name;
            }

            else {
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
