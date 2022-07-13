<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="row" v-if="mode === pageMode.ADD">


            <div class="col col-md-12">
                <!--TITLE-->
                <div class="alert alert-danger" v-model="errorMessage" v-if="errorMessage.length > 0">{{errorMessage}}</div>
                <div class="alert alert-success" v-model="successMessage" v-if="successMessage.length > 0">{{successMessage}}</div>

                <h3><fa icon="folder"></fa> Setup Learning > Setup Module > <span class="talent_font_red">Add</span></h3>
                <br />

                <!--Add-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Setup Module</h4>
                    <br />

                    <div class="row justify-content-between">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Category on Web (For Trial Only)</label>
                                            <div class="input-group">
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" type="checkbox" id="inlineCheckbox1" value="recommended" v-model="categoryOnWeb">
                                                    <label class="form-check-label" for="inlineCheckbox1">Recommended for You</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" type="checkbox" id="inlineCheckbox2" value="popular" v-model="categoryOnWeb">
                                                    <label class="form-check-label" for="inlineCheckbox2">Popular</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Learning Time<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <select class="form-control" v-model="selectedTimePoints">
                                                    <option v-for="(point,index) in timePoints" :value="point">{{point.time}}</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Points</label>
                                            <div class="input-group">
                                                <input class="form-control" v-model="selectedTimePoints.points" disabled />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="col-md-12">
                            <h4>Module</h4>
                        </div>
                        <br />

                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Module Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <multiselect v-model="moduleChoosen"
                                                     id="ajax"
                                                     track-by="moduleName"
                                                     placeholder="Insert Module Name"
                                                     label="moduleName"
                                                     :options="listModule"
                                                     :searchable="true"
                                                     :close-on-select="true"
                                                     :clear-on-select="true"
                                                     :loading="isLoading"
                                                     :hide-selected="true"
                                                     :showNoResults="true"
                                                     @search-change="AutoComplete"
                                                     @open="reset">

                                            <span slot="noResult"><i>Not Found!</i></span>
                                        </multiselect>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Module Description</label>
                                    <textarea class="form-control talent_textarea" :placeholder="moduleChoosen ? moduleChoosen.moduleDescription : emptyString" disabled></textarea>
                                </div>
                            </div>
                            <br />
                        </div>

                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input" type="checkbox" id="inlineCheckbox3" v-model="needQuestion" value="option3">
                                        <label class="form-check-label" for="inlineCheckbox3">Need Question?</label>
                                    </div>
                                </div>
                            </div>
                            <br />
                        </div>
                    </div>

                    <div class="talent_roundborder talent_bg_white talent_padding talent_bg_shadow" v-if="needQuestion == true">
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <h4>Choose Your Question</h4>
                                </div>
                            </div>

                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-3">
                                            <label>Test Time<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <input class="form-control" type="number" placeholder="Minute" min="0" v-model="testTime" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label>Type of Task<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input" type="radio" name="typeOfBanner" v-model="isGrouping" @change="changeTypeGrouping" :value="false" id="inlineRadio1">
                                                <label class="form-check-label" for="inlineRadio1">Specific</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input" type="radio" name="typeOfBanner" v-model="isGrouping" @change="changeTypeGrouping" :value="true" id="inlineRadio2">
                                                <label class="form-check-label" for="inlineRadio2">Grouping</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <label>Competency<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <multiselect v-model="selectedCompetency"
                                                                     id="ajax"
                                                                     track-by="competencyId"
                                                                     placeholder="Insert Competency Name"
                                                                     label="competencyNameMapping"
                                                                     :options="listCompetencies"
                                                                     :searchable="true"
                                                                     :close-on-select="true"
                                                                     :clear-on-select="true"
                                                                     :loading="isLoading"
                                                                     :hide-selected="true"
                                                                     :showNoResults="true"
                                                                     @search-change="AutoCompleteCompetency"
                                                                     @open="resetCompetency"
                                                                     :disabled="isGrouping === false">

                                                            <span slot="noResult"><i>Not Found!</i></span>
                                                        </multiselect>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <label>Module<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <multiselect v-model="moduleChoosenTypeOfTask"
                                                                     id="ajax"
                                                                     track-by="moduleName"
                                                                     placeholder="Insert Module Name"
                                                                     label="moduleName"
                                                                     :options="listModule"
                                                                     :searchable="true"
                                                                     :close-on-select="true"
                                                                     :clear-on-select="true"
                                                                     :loading="isLoading"
                                                                     :hide-selected="true"
                                                                     :showNoResults="true"
                                                                     @search-change="AutoCompleteModuleTask"
                                                                     @open="resetSelectModule"
                                                                     :disabled="isGrouping === false">

                                                            <span slot="noResult"><i>Not Found!</i></span>
                                                        </multiselect>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-9">
                                            <div class="row justify-content-between">
                                                <div class="col-md-4">
                                                    <label>Total Participant<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" type="number" min="0" :disabled="isGrouping === false" v-model="totalParticipant" @change="setQuestionPerParticipant" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <label>Total Question<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" v-model="totalData" type="number" :disabled="isGrouping === false" @change="setQuestionPerParticipant" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <label>Question/Participant<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" type="number" min="0" disabled v-model="questionPerParticipant" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label>Generate</label>
                                                    <div class="input-group">
                                                        <button class="btn talent_bg_blue talent_font_white" :disabled="isGrouping === false" @click.prevent="generateTaskCode">Generate</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-1">
                                            <label>Question Number</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>Task Code<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-2 d-flex justify-content-end align-items-center">
                                            <button class="btn talent_bg_blue talent_font_white" @click.prevent="AddTaskCode()">Add Task Code</button>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12" v-for="(task, index) in selectedTask">
                                    <div class="row justify-content-between">
                                        <div class="col-md-1">
                                            <div class="input-group">
                                                <input class="form-control" :placeholder="index + 1" disabled />
                                            </div>
                                        </div>
                                        <div class="col-md-10">
                                            <div class="input-group">
                                                <multiselect v-model="selectedTask[index]"
                                                             id="ajax"
                                                             track-by="taskCode"
                                                             placeholder="Insert Task Code"
                                                             label="taskCode"
                                                             :options="listTask"
                                                             :searchable="true"
                                                             :close-on-select="true"
                                                             :clear-on-select="true"
                                                             :loading="isLoading"
                                                             :hide-selected="true"
                                                             :showNoResults="true"
                                                             @search-change="AutoCompleteTask"
                                                             @open="resetTask(index)">

                                                    <span slot="noResult"><i>Not Found!</i></span>
                                                </multiselect>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <button class="btn talent_bg_red talent_font_white" @click.prevent="DeleteTaskCode(index)" :disabled="index == 0 && totalData == 1">
                                                Remove
                                            </button>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="addSetupModule(1)">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_blue talent_font_white" @click.prevent="addSetupModule(2)">
                                        Submit
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
            </div>
        </div>

        <div class="row" v-if="mode === pageMode.DETAIL">
            <div class="col col-md-12">
                <!--TITLE-->

                <h3><fa icon="folder"></fa>Setup Learning > Setup Module > <span class="talent_font_red">View Detail</span></h3>
                <br />

                <!--View-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Setup Module</h4>
                    <br />

                    <div class="row justify-content-between">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Category on Web (For Trial Only)</label>
                                            <div class="input-group">
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" type="checkbox" id="inlineCheckbox1" value="recommended" v-model="categoryOnWeb" disabled>
                                                    <label class="form-check-label" for="inlineCheckbox1">Recommended for You</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" type="checkbox" id="inlineCheckbox2" value="popular" v-model="categoryOnWeb" disabled>
                                                    <label class="form-check-label" for="inlineCheckbox2">Popular</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Learning Time<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <select class="form-control" v-model="selectedTimePoints" disabled>
                                                    <option v-for="(point,index) in timePoints" :value="point">{{point.time}}</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Points</label>
                                            <div class="input-group">
                                                <input class="form-control" v-model="selectedTimePoints.points" disabled />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="col-md-12">
                            <h4>Module</h4>
                        </div>
                        <br />

                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Module Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <multiselect v-model="moduleChoosen"
                                                     id="ajax"
                                                     track-by="moduleName"
                                                     placeholder="Insert Module Name"
                                                     label="moduleName"
                                                     :options="listModule"
                                                     :searchable="true"
                                                     :close-on-select="true"
                                                     :clear-on-select="true"
                                                     :loading="isLoading"
                                                     :hide-selected="true"
                                                     :showNoResults="true"
                                                     @search-change="AutoComplete"
                                                     @open="reset"
                                                     disabled>

                                            <span slot="noResult"><i>Not Found!</i></span>
                                        </multiselect>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Module Description</label>
                                    <textarea class="form-control talent_textarea" v-if="moduleChoosen.moduleDescription !== null" v-model="moduleChoosen.moduleDescription" disabled></textarea>
                                </div>
                            </div>
                            <br />
                        </div>

                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input" type="checkbox" id="inlineCheckbox3" v-model="needQuestion" value="option3" disabled>
                                        <label class="form-check-label" for="inlineCheckbox3">Need Question?</label>
                                    </div>
                                </div>
                            </div>
                            <br />
                        </div>
                    </div>

                    <div class="talent_roundborder talent_bg_white talent_padding talent_bg_shadow" v-if="needQuestion == true">
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <h4>Choose Your Question</h4>
                                </div>
                            </div>

                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-3">
                                            <label>Test Time<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <input class="form-control" type="number" placeholder="Minute" v-model="testTime" disabled />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label>Type of Task<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input" type="radio" name="typeOfBanner" v-model="isGrouping" @change="changeTypeGrouping" :value="false" id="inlineRadio1" disabled>
                                                <label class="form-check-label" for="inlineRadio1">Specific</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input" type="radio" name="typeOfBanner" v-model="isGrouping" @change="changeTypeGrouping" :value="true" id="inlineRadio2" disabled>
                                                <label class="form-check-label" for="inlineRadio2">Grouping</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <label>Competency<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <multiselect v-model="selectedCompetency"
                                                                     id="ajax"
                                                                     track-by="competencyId"
                                                                     placeholder="Insert Competency Name"
                                                                     label="competencyNameMapping"
                                                                     :options="listCompetencies"
                                                                     :searchable="true"
                                                                     :close-on-select="true"
                                                                     :clear-on-select="true"
                                                                     :loading="isLoading"
                                                                     :hide-selected="true"
                                                                     :showNoResults="true"
                                                                     @search-change="AutoCompleteCompetency"
                                                                     @open="resetCompetency"
                                                                     disabled>

                                                            <span slot="noResult"><i>Not Found!</i></span>
                                                        </multiselect>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <label>Module<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <multiselect v-model="moduleChoosenTypeOfTask"
                                                                     id="ajax"
                                                                     track-by="moduleName"
                                                                     placeholder="Insert Module Name"
                                                                     label="moduleName"
                                                                     :options="listModule"
                                                                     :searchable="true"
                                                                     :close-on-select="true"
                                                                     :clear-on-select="true"
                                                                     :loading="isLoading"
                                                                     :hide-selected="true"
                                                                     :showNoResults="true"
                                                                     @search-change="AutoComplete"
                                                                     @open="resetSelectModule"
                                                                     disabled>

                                                            <span slot="noResult"><i>Not Found!</i></span>
                                                        </multiselect>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-9">
                                            <div class="row justify-content-between">
                                                <div class="col-md-4">
                                                    <label>Total Participant<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" type="number" disabled v-model="totalParticipant" @change="setQuestionPerParticipant" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <label>Total Question<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" v-model="totalData" type="number" @change="reRenderTask(totalData)" disabled />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <label>Question/Participant<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" type="number" disabled v-model="questionPerParticipant" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label>Generate</label>
                                                    <div class="input-group">
                                                        <button class="btn talent_bg_blue talent_font_white" disabled @click.prevent="generateTaskCode">Generate</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-1">
                                            <label>Question Number</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>Task Code<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-2 d-flex justify-content-end align-items-center">
                                            <button class="btn talent_bg_blue talent_font_white" disabled @click.prevent="AddTaskCode()">Add Task Code</button>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12" v-for="(task, index) in selectedTask">
                                    <div class="row justify-content-between">
                                        <div class="col-md-1">
                                            <div class="input-group">
                                                <input class="form-control" :placeholder="index + 1" disabled />
                                            </div>
                                        </div>
                                        <div class="col-md-10">
                                            <div class="input-group">
                                                <multiselect v-model="selectedTask[index]"
                                                             id="ajax"
                                                             track-by="taskCode"
                                                             placeholder="Insert Task Code"
                                                             label="taskCode"
                                                             :options="listTask"
                                                             :searchable="true"
                                                             :close-on-select="true"
                                                             :clear-on-select="true"
                                                             :loading="isLoading"
                                                             :hide-selected="true"
                                                             :showNoResults="true"
                                                             @search-change="AutoCompleteTask"
                                                             @open="resetTask(index)"
                                                             disabled>

                                                    <span slot="noResult"><i>Not Found!</i></span>
                                                </multiselect>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <button class="btn talent_bg_red talent_font_white" @click.prevent="DeleteTaskCode(index)" disabled>
                                                Remove
                                            </button>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white" @click.prevent="goBackPage()">
                                        Close
                                    </button>
                                    <button v-else class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked()">
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
            </div>
        </div>

        <div class="row" v-if="mode === pageMode.EDIT">
            <div class="col col-md-12">
                <!--TITLE-->
                <div class="alert alert-danger" v-model="errorMessage" v-if="errorMessage.length > 0">{{errorMessage}}</div>

                <h3><fa icon="folder"></fa> Setup Learning > Setup Module > <span class="talent_font_red">Edit</span></h3>
                <br />

                <!--Edit-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Setup Module</h4>
                    <br />

                    <div class="row justify-content-between">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Category on Web (For Trial Only)</label>
                                            <div class="input-group">
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" type="checkbox" id="inlineCheckbox1" value="recommended" v-model="categoryOnWeb">
                                                    <label class="form-check-label" for="inlineCheckbox1">Recommended for You</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" type="checkbox" id="inlineCheckbox2" value="popular" v-model="categoryOnWeb">
                                                    <label class="form-check-label" for="inlineCheckbox2">Popular</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Learning Time<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <select class="form-control" v-model="selectedTimePoints">
                                                    <option v-for="(point,index) in timePoints" :value="point">{{point.time}}</option>
                                                </select>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Points</label>
                                            <div class="input-group">
                                                <input class="form-control" v-model="selectedTimePoints.points" disabled />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />
                        <div class="col-md-12">
                            <h4>Module</h4>
                        </div>
                        <br />

                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Module Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <multiselect v-model="moduleChoosen"
                                                     id="ajax"
                                                     track-by="moduleName"
                                                     placeholder="Insert Module Name"
                                                     label="moduleName"
                                                     :options="listModule"
                                                     :searchable="true"
                                                     :close-on-select="true"
                                                     :clear-on-select="true"
                                                     :loading="isLoading"
                                                     :hide-selected="true"
                                                     :showNoResults="true"
                                                     @search-change="AutoCompleteUpdate"
                                                     @open="reset">

                                            <span slot="noResult"><i>Not Found!</i></span>
                                        </multiselect>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Module Description</label>
                                    <textarea class="form-control talent_textarea" :placeholder="moduleChoosen ? moduleChoosen.moduleDescription : emptyString " disabled></textarea>
                                </div>
                            </div>
                            <br />
                        </div>

                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input" type="checkbox" id="inlineCheckbox3" v-model="needQuestion" value="option3">
                                        <label class="form-check-label" for="inlineCheckbox3">Need Question?</label>
                                    </div>
                                </div>
                            </div>
                            <br />
                        </div>
                    </div>

                    <div class="talent_roundborder talent_bg_white talent_padding talent_bg_shadow" v-if="needQuestion == true">
                        <div class="col-md-12">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <h4>Choose Your Question</h4>
                                </div>
                            </div>

                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-3">
                                            <label>Test Time<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <input class="form-control" type="number" placeholder="Minute" min="0" v-model="testTime" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label>Type of Task<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input" type="radio" name="typeOfBanner" v-model="isGrouping" @change="changeTypeGrouping" :value="false" id="inlineRadio1">
                                                <label class="form-check-label" for="inlineRadio1">Specific</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input" type="radio" name="typeOfBanner" v-model="isGrouping" @change="changeTypeGrouping" :value="true" id="inlineRadio2">
                                                <label class="form-check-label" for="inlineRadio2">Grouping</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <label>Competency<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <multiselect v-model="selectedCompetency"
                                                                     id="ajax"
                                                                     track-by="competencyId"
                                                                     placeholder="Insert Competency Name"
                                                                     label="competencyNameMapping"
                                                                     :options="listCompetencies"
                                                                     :searchable="true"
                                                                     :close-on-select="true"
                                                                     :clear-on-select="true"
                                                                     :loading="isLoading"
                                                                     :hide-selected="true"
                                                                     :showNoResults="true"
                                                                     @search-change="AutoCompleteCompetency"
                                                                     @open="resetCompetency"
                                                                     :disabled="isGrouping === false">

                                                            <span slot="noResult"><i>Not Found!</i></span>
                                                        </multiselect>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <label>Module<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <multiselect v-model="moduleChoosenTypeOfTask"
                                                                     id="ajax"
                                                                     track-by="moduleName"
                                                                     placeholder="Insert Module Name"
                                                                     label="moduleName"
                                                                     :options="listModule"
                                                                     :searchable="true"
                                                                     :close-on-select="true"
                                                                     :clear-on-select="true"
                                                                     :loading="isLoading"
                                                                     :hide-selected="true"
                                                                     :showNoResults="true"
                                                                     @search-change="AutoCompleteModuleTask"
                                                                     @open="resetSelectModule"
                                                                     :disabled="isGrouping === false">

                                                            <span slot="noResult"><i>Not Found!</i></span>
                                                        </multiselect>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-9">
                                            <div class="row justify-content-between">
                                                <div class="col-md-4">
                                                    <label>Total Participant<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" type="number" min="0" :disabled="isGrouping === false" v-model="totalParticipant" @change="setQuestionPerParticipant" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <label>Total Question<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" v-model="totalData" type="number" :disabled="isGrouping === false" @change="setQuestionPerParticipant" />
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <label>Question/Participant<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" type="number" disabled v-model="questionPerParticipant" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label>Generate</label>
                                                    <div class="input-group">
                                                        <button class="btn talent_bg_blue talent_font_white" :disabled="isGrouping === false" @click.prevent="generateTaskCode">Generate</button>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-1">
                                            <label>Question Number</label>
                                        </div>
                                        <div class="col-md-9">
                                            <label>Task Code<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-2 d-flex justify-content-end align-items-center">
                                            <button class="btn talent_bg_blue talent_font_white" @click.prevent="AddTaskCode()">Add Task Code</button>
                                        </div>

                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12" v-for="(task, index) in selectedTask">
                                    <div class="row justify-content-between">
                                        <div class="col-md-1">
                                            <div class="input-group">
                                                <input class="form-control" :placeholder="index + 1" disabled />
                                            </div>
                                        </div>
                                        <div class="col-md-10">
                                            <div class="input-group">
                                                <multiselect v-model="selectedTask[index]"
                                                             id="ajax"
                                                             track-by="taskCode"
                                                             placeholder="Insert Task Code"
                                                             label="taskCode"
                                                             :options="listTask"
                                                             :searchable="true"
                                                             :close-on-select="true"
                                                             :clear-on-select="true"
                                                             :loading="isLoading"
                                                             :hide-selected="true"
                                                             :showNoResults="true"
                                                             @search-change="AutoCompleteTask"
                                                             @open="resetTask(index)">

                                                    <span slot="noResult"><i>Not Found!</i></span>
                                                </multiselect>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <button class="btn talent_bg_red talent_font_white" @click.prevent="DeleteTaskCode(index)" :disabled="index == 0 && totalData == 1">
                                                Remove
                                            </button>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="updateSetupModule(1)">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_blue talent_font_white" @click.prevent="updateSetupModule(2)">
                                        Submit
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
            </div>
        </div>

    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { TimePointTaskModel, CompetencyMappingJoinModel, TaskForSetupModuleModel, SetupModuleFormModel, SetupModuleModuleViewModel, SetupTaskFormModel, SetupModuleService, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService';
    import Message from '../../class/Message';
    import { PageModeEnum } from '../../enum/PageModeEnum';
    @Component({
        props: ['mode', 'setupmoduleid', 'fromOutside'],
        created: async function (this: SetupModule) {
            await this.getAccess();
            this.listModule = await this.Service.getAllModule('');
            console.log(this.listModule);
            if (this.mode === this.pageMode.ADD) {
                this.initialize();
            }
            else if (this.mode === this.pageMode.DETAIL || this.mode === this.pageMode.EDIT) {
                this.initializeData();
            }
        }
    })
    export default class SetupModule extends Vue {

        props: any = {
            placeholder: "",
            readonly: true
        };

        pageMode = PageModeEnum;

        mode: string;
        setupmoduleid: number;
        fromOutside: boolean;

        needQuestion: boolean = true;

        isBusy: boolean = false;

        totalData: number = 1;

        errorMessage: string = '';
        successMessage: string = '';

        formModel: SetupModuleFormModel = {
            courseId: null,
            isForRemedial: null,
            isPopular: null,
            isRecommendedForYou: null,
            minimumScore: null,
            moduleDescription: '',
            moduleId: null,
            moduleName: null,
            points: null,
            score: null,
            setupModuleId: null,
            setupTaskForm: {
                competencyId: null,
                competencyNameMapping: null,
                competencyTypeId: null,
                competencyTypeName: null,
                evaluationTypeId: null,
                evaluationTypeName: null,
                isGrouping: null,
                moduleDescription: '',
                moduleId: null,
                moduleName: null,
                popQuizId: null,
                prefixCode: null,
                questionPerParticipant: null,
                setupModuleId: null,
                setupTaskId: null,
                taskList: [],
                testTime: null,
                totalParticipant: null,
                totalQuestion: null
            },
            timePointId: null,
            trainingTypeId: null,
        };

        Service: SetupModuleService = new SetupModuleService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        timePoints: TimePointTaskModel[] = [];

        emptyString: string = '';

        selectedTimePoints: TimePointTaskModel = {
            points: null,
            score: null,
            timePointId: null
        };
        selectedCompetency: CompetencyMappingJoinModel = {
            competencyId: null,
            competencyNameMapping: null,
            competencyTypeId: null,
            competencyTypeName: null,
            evaluationTypeId: null,
            evaluationTypeName: null,
            prefixCode: null
        };
        selectedTask: TaskForSetupModuleModel[] = [{
            taskCode: null,
            taskId: null
        }];

        listModule: SetupModuleModuleViewModel[] = [];
        listSortedModule: any = [];
        listTask: TaskForSetupModuleModel[] = [];
        listCompetencies: CompetencyMappingJoinModel[] = [];

        listAllTask: TaskForSetupModuleModel[] = [];

        moduleChoosen: SetupModuleModuleViewModel = {
            moduleDescription: '',
            moduleId: null,
            moduleName: null
        };
        moduleChoosenTypeOfTask: SetupModuleModuleViewModel = {
            moduleDescription: '',
            moduleId: null,
            moduleName: null
        };
        moduleChoosenGroup: SetupModuleModuleViewModel = {
            moduleDescription: '',
            moduleId: null,
            moduleName: null
        };

        categoryOnWeb: string[] = [];

        isGrouping: boolean = false;
        testTime: number = 0;
        totalParticipant: number = 0;
        questionPerParticipant: number = 0;

        isLoading: boolean = false;
        timeout: any = null;

        setQuestionPerParticipant() {
            if (this.totalParticipant <= 0) {
                this.questionPerParticipant = 0;
            }
            else {
                this.questionPerParticipant = Math.floor(this.totalData / this.totalParticipant);
            }
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("STM");
            this.crud = data
        }

        async addSetupModule(insertType: number) {

            if (!this.crud.create) {
                return
            }

            this.isBusy = true;
            this.errorMessage = '';

            if (this.validateAndAssign() === false) {
                this.isBusy = false;
                return;
            }

            try {
                this.formModel.inputType = insertType;
                var result = await this.Service.insertSetupModule(this.formModel);

                if (result === true) {
                    this.successMessage = "Success to insert";
                    this.isBusy = false;
                    this.closeClicked();
                }
            }
            catch {
                this.errorMessage = "Failed to Setup Module";
                this.isBusy = false;
            }
        }

        validateAndAssign() {
            if (this.selectedTimePoints.timePointId === null || !this.selectedTimePoints.timePointId) {
                this.errorMessage = "Learning Time must be filled!";
                return false;
            }
            else {
                this.formModel.timePointId = this.selectedTimePoints.timePointId;
            }

            if (this.moduleChoosen.moduleId === null || !this.moduleChoosen.moduleId) {
                this.errorMessage = "Module must be filled";
                return false;
            }
            else {
                this.formModel.moduleId = this.moduleChoosen.moduleId;
                this.formModel.moduleName = this.moduleChoosen.moduleName;
                this.formModel.moduleDescription = this.moduleChoosen.moduleDescription;
            }


            if (this.needQuestion === true) {

                if (this.testTime <= 0 || this.testTime === null) {
                    this.errorMessage = "Test Time must be filled and must be bigger than 0";
                    return false;
                }
                else {
                    this.formModel.setupTaskForm.testTime = this.testTime;
                }

                if (this.isGrouping === null) {
                    this.errorMessage = "Type of Task must be choosen";
                    return false;
                }
                else {
                    this.formModel.setupTaskForm.isGrouping = this.isGrouping;
                }

                if (this.isGrouping === true) {
                    if (!this.selectedCompetency.competencyId || this.selectedCompetency.competencyId === null || Object.keys(this.selectedCompetency).length === 0) {
                        this.errorMessage = "Competency must be filled";
                        return false;
                    }
                    else {
                        this.formModel.setupTaskForm.competencyId = this.selectedCompetency.competencyId;
                    }

                    if (Object.keys(this.moduleChoosenTypeOfTask).length === 0) {
                        this.errorMessage = "Module Task must be filled";
                        return false;
                    }
                    else {
                        this.formModel.setupTaskForm.moduleId = this.moduleChoosenTypeOfTask.moduleId;
                    }

                    if (this.totalParticipant === null || this.totalParticipant <= 0) {
                        this.errorMessage = "Total Participant must be filled and must be greater than 0";
                        return false;
                    }
                    else {
                        this.formModel.setupTaskForm.totalParticipant = this.totalParticipant;
                    }

                    if (this.totalData === null) {
                        this.errorMessage = "Total Question must be filled";
                        return false;
                    }
                    else {
                        this.formModel.setupTaskForm.totalQuestion = this.totalData;
                    }

                    if (this.questionPerParticipant === null) {
                        this.errorMessage = "Question/Participant must be filled";
                        return false;
                    }
                    else {
                        this.formModel.setupTaskForm.questionPerParticipant = this.questionPerParticipant;
                    }

                    if (this.totalParticipant > this.totalData) {
                        this.errorMessage = "Total participant can't be greater than Total Question";
                        return false;
                    }
                }
                else {
                    this.formModel.setupTaskForm.competencyId = null;
                    this.formModel.setupTaskForm.moduleId = null;
                }

                if (this.selectedTask.length <= 0) {
                    this.errorMessage = "Task can't be empty";
                    return false;
                }

                this.formModel.setupTaskForm.taskList = [];

                for (let task of this.selectedTask) {
                    if (!task || !task.taskId || task.taskId === null) {
                        this.errorMessage = "Task can't be empty";
                        return false;
                    }
                    else {
                        console.log(task);
                        this.formModel.setupTaskForm.taskList.push(task);
                    }
                }

                if (this.totalData !== this.formModel.setupTaskForm.taskList.length) {
                    this.errorMessage = "Total Task must be same with Total Question";
                    return false;
                }
            }
            else if (this.needQuestion === false) {
                this.formModel.setupTaskForm.taskList = null;
            }

            if (this.categoryOnWeb.includes("recommended") === true) {
                this.formModel.isRecommendedForYou = true;
            }
            else {
                this.formModel.isRecommendedForYou = false;
            }

            if (this.categoryOnWeb.includes("popular") === true) {
                this.formModel.isPopular = true;
            }
            else {
                this.formModel.isPopular = false;
            }

            return true;
        }

        async updateSetupModule(insertType: number) {
            this.errorMessage = '';

            if (!this.crud.update) {
                return
            }

            if (this.validateAndAssign() === false) {
                return;
            }

            this.isBusy = true;

            try {
                this.formModel.inputType = insertType;
                var result = await this.Service.updateSetupModule(this.formModel);
                if (result === true) {
                    this.successMessage = Message.SuccessEditMessage;
                    this.isBusy = false;
                    this.closeClicked();
                }
            }
            catch {
                this.errorMessage = "Data already exist.";
                this.isBusy = false;
            }
        }


        async initialize() {

            if (!this.crud.create) {
                return;
            }

            this.timePoints = await this.Service.getAllTimePoints();
            this.moduleChoosen = {
                moduleDescription: '',
                moduleId: null,
                moduleName: null
            };
            this.selectedCompetency = {
                competencyId: null,
                competencyNameMapping: null,
                competencyTypeId: null,
                competencyTypeName: null,
                evaluationTypeId: null,
                evaluationTypeName: null,
                prefixCode: null
            };
            this.formModel = {
                courseId: null,
                isForRemedial: null,
                isPopular: null,
                isRecommendedForYou: null,
                minimumScore: null,
                moduleDescription: '',
                moduleId: null,
                moduleName: null,
                points: null,
                score: null,
                setupModuleId: null,
                setupTaskForm: null,
                timePointId: null,
                trainingTypeId: null
            };
            this.formModel.setupTaskForm = {
                competencyId: null,
                competencyNameMapping: null,
                competencyTypeId: null,
                competencyTypeName: null,
                evaluationTypeId: null,
                evaluationTypeName: null,
                isGrouping: null,
                moduleDescription: '',
                moduleId: null,
                moduleName: null,
                popQuizId: null,
                prefixCode: null,
                questionPerParticipant: null,
                setupModuleId: null,
                setupTaskId: null,
                taskList: null,
                testTime: null,
                totalParticipant: null,
                totalQuestion: null
            };
            this.formModel.setupTaskForm = {
                competencyId: null,
                competencyNameMapping: null,
                competencyTypeId: null,
                competencyTypeName: null,
                evaluationTypeId: null,
                evaluationTypeName: null,
                isGrouping: null,
                moduleDescription: '',
                moduleId: null,
                moduleName: null,
                popQuizId: null,
                prefixCode: null,
                questionPerParticipant: null,
                setupModuleId: null,
                setupTaskId: null,
                taskList: null,
                testTime: null,
                totalParticipant: null,
                totalQuestion: null
            };
            this.setQuestionPerParticipant();
            this.sortedArray();
        }

        async initializeData() {

            if (this.pageMode.DETAIL) {
                if (!this.crud.read) {
                    return;
                }
            } else if (this.pageMode.EDIT) {
                if (!this.crud.update) {
                    return;
                }
            }
    
            this.formModel = await this.Service.getSetupModuleUpdate(this.setupmoduleid);
            this.timePoints = await this.Service.getAllTimePoints();

            if (this.formModel.isPopular === true) {
                this.categoryOnWeb.push("popular")
            }

            if (this.formModel.isRecommendedForYou === true) {
                this.categoryOnWeb.push("recommended");
            }

            this.selectedTimePoints = this.timePoints.find(Q => Q.timePointId === this.formModel.timePointId);

            var selectedModule: SetupModuleModuleViewModel = {
                moduleDescription: '',
                moduleId: null,
                moduleName: null
            };
            selectedModule.moduleDescription = this.formModel.moduleDescription;
            selectedModule.moduleId = this.formModel.moduleId;
            selectedModule.moduleName = this.formModel.moduleName;
            this.moduleChoosen = selectedModule;
            if (this.formModel.setupTaskForm.taskList) {

                this.needQuestion = true;

                this.testTime = this.formModel.setupTaskForm.testTime;
                this.isGrouping = this.formModel.setupTaskForm.isGrouping;

                var competency: CompetencyMappingJoinModel = {
                    competencyId: null,
                    competencyNameMapping: null,
                    competencyTypeId: null,
                    competencyTypeName: null,
                    evaluationTypeId: null,
                    evaluationTypeName: null,
                    prefixCode: null
                };
                competency.competencyId = this.formModel.setupTaskForm.competencyId;
                competency.competencyNameMapping = this.formModel.setupTaskForm.competencyNameMapping;
                competency.competencyTypeId = this.formModel.setupTaskForm.competencyTypeId;
                competency.competencyTypeName = this.formModel.setupTaskForm.competencyTypeName;
                competency.evaluationTypeId = this.formModel.setupTaskForm.evaluationTypeId;
                competency.evaluationTypeName = this.formModel.setupTaskForm.evaluationTypeName;
                this.selectedCompetency = competency;

                var module: SetupModuleModuleViewModel = {
                    moduleDescription: '',
                    moduleId: null,
                    moduleName: null
                };
                module.moduleDescription = this.formModel.setupTaskForm.moduleDescription;
                module.moduleId = this.formModel.setupTaskForm.moduleId;
                module.moduleName = this.formModel.setupTaskForm.moduleName;
                this.moduleChoosenTypeOfTask = module;

                this.selectedTask = this.formModel.setupTaskForm.taskList;
                this.totalData = this.selectedTask.length;

                this.totalParticipant = this.formModel.setupTaskForm.totalParticipant;
                this.questionPerParticipant = this.formModel.setupTaskForm.questionPerParticipant;
            }
            else {
                this.needQuestion = false;
            }
            this.sortedArray();

        }

        sortedArray(){
            this.listSortedModule = this.listModule;
            this.listSortedModule.sort((a, b) => a.moduleName.localeCompare(b.moduleName))            
        }

        closeClicked() {
            window.location.href = '/Setup/SetupLearning';
        }

        changeTypeGrouping() {
            this.selectedTask = [{
                taskCode: null,
                taskId: null
            }];
            this.totalData = 1;
            this.questionPerParticipant = 0;
            this.totalParticipant = 0;
            this.selectedCompetency = {
                competencyId: null,
                competencyNameMapping: null,
                competencyTypeId: null,
                competencyTypeName: null,
                evaluationTypeId: null,
                evaluationTypeName: null,
                prefixCode: null
            };
            this.moduleChoosenTypeOfTask = {
                moduleDescription: '',
                moduleId: null,
                moduleName: null
            };
        }

        async AutoComplete(query) {
            if (query == null || query === '') {
                this.listModule = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetModule(query);
                }, 500
            );
        }

        async AutoCompleteUpdate(query) {
            if (query == null || query === '') {
                this.listModule = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetModuleUpdate(query);
                }, 500
            );
        }

        async AutoCompleteModuleTask(query) {
            if (query == null || query === '') {
                this.listModule = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetModuleTask(query);
                }, 500
            );
        }

        async AutoCompleteCompetency(query) {
            if (query == null || query === '') {
                this.listCompetencies = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetCompetencies(query);
                }, 500
            );
        }

        async AutoCompleteTask(query) {
            if (query == null || query === '') {
                this.listTask = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetTaskList(query);
                }, 500
            );
        }


        reset() {
            this.moduleChoosen = {
                moduleDescription: '',
                moduleId: null,
                moduleName: null
            };
        }

        resetSelectModule() {
            this.moduleChoosenGroup = {
                moduleDescription: '',
                moduleId: null,
                moduleName: null
            };
        }

        resetTask(index: number) {
            this.selectedTask[index] = {
                taskCode: null,
                taskId: null
            };
        }

        resetCompetency(index: number) {
            this.selectedCompetency = {
                competencyId: null,
                competencyNameMapping: null,
                competencyTypeId: null,
                competencyTypeName: null,
                evaluationTypeId: null,
                evaluationTypeName: null,
                prefixCode: null
            };
        }

        async GetModule(query) {
            if (query === '' || query == null) {
                this.listModule = [];
                return;
            }

            let a = await this.Service.getAllModule(query);
            this.listModule = a;

            this.isLoading = false;
        }

        async GetModuleUpdate(query) {
            if (query === '' || query == null) {
                this.listModule = [];
                return;
            }

            let a = await this.Service.getAllModuleForUpdate(query);
            this.listModule = a;

            this.isLoading = false;
        }

        async GetModuleTask(query) {
            if (query === '' || query == null) {
                this.listModule = [];
                return;
            }

            let a = await this.Service.getAllModuleFromTask(query);
            this.listModule = a;

            this.isLoading = false;
        }

        async GetCompetencies(query) {
            if (query === '' || query == null) {
                this.listCompetencies = [];
                return;
            }

            let a = await this.Service.getAllCompetencyFiltered(query);
            this.listCompetencies = a;

            this.isLoading = false;
        }

        async GetTaskList(query) {
            this.errorMessage = '';
            if (query === '' || query == null) {
                this.listTask = [];
                return;
            }

            let result;
            if (this.isGrouping === false) {
                result = await this.Service.getAllTaskCodeFiltered(query, null, null);
            }
            else if (this.isGrouping === true) {

                if (this.selectedCompetency.competencyId == null || !this.selectedCompetency.competencyId) {
                    this.errorMessage = "Competency can't be empty!";
                    return;
                }
                if (this.moduleChoosenTypeOfTask.moduleId == null || !this.moduleChoosenTypeOfTask.moduleId) {
                    this.errorMessage = "Module can't be empty!";
                    return;
                }

                result = await this.Service.getAllTaskCodeFiltered(query, this.selectedCompetency.competencyId, this.moduleChoosenTypeOfTask.moduleId);
            }

            this.listTask = result;
            console.log(this.listTask);

            for (var a = 0; a < this.selectedTask.length; a++) {
                if (this.selectedTask[a] !== undefined) {
                    let found = this.listTask.findIndex(Q => Q.taskId === this.selectedTask[a].taskId);
                    if (found !== -1) {
                        this.listTask.splice(found, 1);
                    }
                }
            }

            this.isLoading = false;
        }

        async AddTaskCode() {
            this.selectedTask.push({
                taskCode: null,
                taskId: null
            });
            this.totalData++;
            this.setQuestionPerParticipant();
        }

        async DeleteTaskCode(indexToDelete: number) {
            this.selectedTask.splice(indexToDelete, 1);
            this.totalData--;
            this.setQuestionPerParticipant();
        }

        reloadListOption() {
            console.log("Reload", this.selectedTask);
            this.listTask = this.listTask.filter(Q => this.selectedTask.includes(Q));
        }

        reRenderTask(totalQuestion: number) {

            if (this.selectedCompetency == null) {
                this.errorMessage = "Competency can't be empty!";
                return;
            }

            if (this.moduleChoosenTypeOfTask == null) {
                this.errorMessage = "Module can't be empty!";
                return;
            }

            if (totalQuestion < 1 || isNaN(totalQuestion) == true) {
                totalQuestion = 1;
                this.totalData = 1;
            }
            if (this.selectedTask.length > totalQuestion) {
                console.log("lebih kecil");
                var toPop = this.selectedTask.length - totalQuestion;
                for (var i = 0; i < toPop; i++) {
                    this.selectedTask.pop();
                }
            }
            else if (this.selectedTask.length < totalQuestion) {
                console.log("lebih besar");
                var totalToAdd = totalQuestion - this.selectedTask.length;
                for (var i = 0; i < totalToAdd; i++) {
                    this.selectedTask.push({
                        taskCode: null,
                        taskId: null
                    });
                }
            }
            this.setQuestionPerParticipant();
            this.$forceUpdate();
        }

        async generateTaskCode() {
            this.errorMessage = '';
            if (this.selectedCompetency.competencyId == null || !this.selectedCompetency.competencyId) {
                this.errorMessage = "Competency can't be empty!";
                return;
            }
            if (this.moduleChoosenTypeOfTask.moduleId == null || !this.moduleChoosenTypeOfTask.moduleId) {
                this.errorMessage = "Module can't be empty!";
                return;
            }
            console.log(this.totalData, '', this.selectedCompetency.competencyId, this.moduleChoosenTypeOfTask.moduleId);
            let result = await this.Service.generetaTask(this.totalData, '', this.selectedCompetency.competencyId, this.moduleChoosenTypeOfTask.moduleId);

            console.log("result", result);
            this.selectedTask = [];
            for (var a = 0; a < result.length; a++) {
                this.selectedTask.push(result[a]);
                console.log("index result", this.selectedTask[a]);
            }
            this.totalData = this.selectedTask.length;
            this.setQuestionPerParticipant();
            this.$forceUpdate();
        }

        goBackPage() {
            window.history.back();
        }

    }
</script>
