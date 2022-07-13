<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <!--ADD-->
        <div class="row" v-if="mode === pageMode.ADD">
            <div class="col col-md-12">
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup Learning > Setup Pop Quiz > <span class="talent_font_red">Add</span></h3>

                <!--Add-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4>Setup Quiz</h4>

                    <div class="alert alert-danger" v-show="errors.items.length > 0">
                        <button type="button" class="close" aria-label="Close" @click="$validator.errors.clear()">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <div v-for="error in errors.all()">{{ error }}</div>
                    </div>

                    <div class="row justify-content-between">
                        <div class="col-md-12">
                            <div class="row justify-content-between mb-md-4">
                                <div class="col-md-12">
                                    <label>Quiz Title<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control"
                                               name="Quiz Title"
                                               v-validate="'required'"
                                               v-model="popQuizForm.quizTitle" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div class="col-md-12 mb-md-4">
                            <div class="row">
                                <h4>Choose Your Question</h4>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-3">
                                        <label>Test Time<span class="talent_font_red">*</span></label>
                                        <div class="input-group">
                                            <input type="number"
                                                   min="0"
                                                   class="form-control"
                                                   placeholder="minute"
                                                   v-model.number="popQuizForm.testTime"
                                                   name="Test Time"
                                                   v-validate="'required'" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-6">
                                <label>Type Task<span class="talent_font_red">*</span></label>
                                <div class="row justify-content-between">
                                    <div class="col-md-6">
                                        <div class="form-check form-check-inline">
                                            <input class="form-check-input" type="radio" name="typeOfBanner" v-model="popQuizForm.isGrouping"
                                                   :value="false" id="inlineRadio1" @change="ResetTaskCodeForm()">
                                            <label class="form-check-label" for="inlineRadio1">Specific</label>
                                        </div>
                                        <div class="form-check form-check-inline">
                                            <input class="form-check-input" type="radio" name="typeOfBanner" v-model="popQuizForm.isGrouping"
                                                   :value="true" id="inlineRadio2" @change="ResetTaskCodeForm()">
                                            <label class="form-check-label" for="inlineRadio2">Grouping</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-6">
                                        <div class="row justify-content-between">
                                            <div class="col-md-6">
                                                <label>Competency<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <multiselect v-model="competencyForm"
                                                                 name="Competency"
                                                                 v-validate="'required-multiselect'"
                                                                 :options="competencyOption"
                                                                 :searchable="true"
                                                                 :custom-label="NameCompetency"
                                                                 placeholder="Pick Competency"
                                                                 :disabled="popQuizForm.isGrouping === false"
                                                                 :allow-empty="false"
                                                                 deselect-label="Can't remove this value"
                                                                 @search-change="CompetencyAutoComplete"
                                                                 @open="resetCompetencyOption">

                                                    </multiselect>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label>Module<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <multiselect v-model="moduleForm"
                                                                 name="Module"
                                                                 v-validate="'required-multiselect'"
                                                                 :options="moduleOption"
                                                                 :searchable="true"
                                                                 :custom-label="NameModule"
                                                                 placeholder="Pick Module"
                                                                 :disabled="popQuizForm.isGrouping === false"
                                                                 :allow-empty="false"
                                                                 deselect-label="Can't remove this value"
                                                                 @search-change="ModuleAutoComplete"
                                                                 @open="resetModuleOption">

                                                    </multiselect>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-9">
                                        <div class="row justify-content-between">
                                            <div class="col-md-4">
                                                <label>Total Participant<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <input type="number"
                                                           name="Total Participant"
                                                           v-validate="'required'"
                                                           min="0"
                                                           class="form-control"
                                                           v-model.number="popQuizForm.totalParticipant"
                                                           :disabled="popQuizForm.isGrouping === false"
                                                           @change="generateQuestionPerParticipant" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <label>Total Question<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <input type="number"
                                                           name="Total Question"
                                                           v-validate="'required|min_value:1'"
                                                           min="0"
                                                           class="form-control"
                                                           v-model.number="popQuizForm.totalQuestion"
                                                           :disabled="popQuizForm.isGrouping === false"
                                                           @change="generateQuestionPerParticipant" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <label>Question/Participant<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <input type="number"
                                                           class="form-control"
                                                           v-model.number="popQuizForm.questionPerParticipant" disabled />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <label>Generate</label>

                                        <div>
                                            <button :disabled="popQuizForm.isGrouping === false"
                                                    class="btn talent_bg_blue talent_roundborder talent_font_white"
                                                    @click.prevent="GenerateTaskForm()">
                                                Generate
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between">
                            <div class="col-md-1">
                                <label>Question Number</label>
                            </div>
                            <div class="col-md-9">
                                <label>Task Code<span class="talent_font_red">*</span></label>
                            </div>
                            <div class="col-md-2 d-flex justify-content-end align-items-center">
                                <button class="btn talent_bg_blue talent_roundborder talent_font_white"
                                        @click.prevent="AddTaskCode()">
                                    Add Task Code
                                </button>
                            </div>
                        </div>

                        <div class="row justify-content-between">
                            <div class="col-md-12" v-for="(loop, index) in taskCodeForm">
                                <div class="row justify-content-between mb-md-4">
                                    <div class="col-md-1">
                                        <div class="input-group">
                                            <input class="form-control" :placeholder="index + 1" disabled />
                                        </div>
                                    </div>
                                    <div class="col-md-10">
                                        <div class="input-group">
                                            <multiselect v-model="taskCodeForm[index]"
                                                         :name="'Task Code No.' + (index + 1)"
                                                         v-validate="'required-multiselect'"
                                                         :options="taskCodeOption"
                                                         :searchable="true"
                                                         :custom-label="NameTaskCode"
                                                         placeholder="Pick Task Code"
                                                         :allow-empty="false"
                                                         :loading="isLoadingOption"
                                                         :showNoResults="true"
                                                         deselect-label="Can't remove this value"
                                                         @search-change="AutoComplete"
                                                         >

                                                <span slot="noResult"><i>Not Found!</i></span>
                                            </multiselect>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="DeleteTaskCode(index)" :disabled="taskCodeForm.length === 1">
                                            Remove
                                        </button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="gotoSetupLearning">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="savePopQuiz">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_blue talent_font_white" @click="submitPopQuiz">
                                        Submit
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <!--EDIT-->
        <div class="row" v-if="mode === pageMode.EDIT">
            <div class="col col-md-12">
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup Learning > Setup Pop Quiz > <span class="talent_font_red">Edit</span></h3>

                <!--Add-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4>Setup Quiz</h4>

                    <div class="alert alert-danger" v-show="errors.items.length > 0">
                        <button type="button" class="close" aria-label="Close" @click="$validator.errors.clear()">
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <div v-for="error in errors.all()">{{ error }}</div>
                    </div>

                    <div class="row justify-content-between">
                        <div class="col-md-12">
                            <div class="row justify-content-between mb-md-4">
                                <div class="col-md-12">
                                    <label>Quiz Title<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control"
                                               name="Quiz Title"
                                               v-validate="'required'"
                                               v-model="popQuizForm.quizTitle" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div class="col-md-12 mb-md-4">
                            <div class="row">
                                <h4>Choose Your Question</h4>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-3">
                                        <label>Test Time<span class="talent_font_red">*</span></label>
                                        <div class="input-group">
                                            <input type="number"
                                                   min="0"
                                                   class="form-control"
                                                   placeholder="minute"
                                                   v-model.number="popQuizForm.testTime"
                                                   name="Test Time"
                                                   v-validate="'required'" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-6">
                                <label>Type Task<span class="talent_font_red">*</span></label>
                                <div class="row justify-content-between">
                                    <div class="col-md-6">
                                        <div class="form-check form-check-inline">
                                            <input class="form-check-input" type="radio" name="typeOfBanner" v-model="popQuizForm.isGrouping"
                                                   :value="false" id="inlineRadio1" @change="ResetTaskCodeForm()">
                                            <label class="form-check-label" for="inlineRadio1">Specific</label>
                                        </div>
                                        <div class="form-check form-check-inline">
                                            <input class="form-check-input" type="radio" name="typeOfBanner" v-model="popQuizForm.isGrouping"
                                                   :value="true" id="inlineRadio2" @change="ResetTaskCodeForm()">
                                            <label class="form-check-label" for="inlineRadio2">Grouping</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-6">
                                        <div class="row justify-content-between">
                                            <div class="col-md-6">
                                                <label>Competency<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <multiselect v-model="competencyForm"
                                                                 name="Competency"
                                                                 v-validate="'required-multiselect'"
                                                                 :options="competencyOption"
                                                                 :searchable="true"
                                                                 :custom-label="NameCompetency"
                                                                 placeholder="Pick Competency"
                                                                 :disabled="popQuizForm.isGrouping === false"
                                                                 :allow-empty="false"
                                                                 deselect-label="Can't remove this value"
                                                                 @search-change="CompetencyAutoComplete"
                                                                 @open="resetCompetencyOption">

                                                    </multiselect>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label>Module<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <multiselect v-model="moduleForm"
                                                                 name="Module"
                                                                 v-validate="'required-multiselect'"
                                                                 :options="moduleOption"
                                                                 :searchable="true"
                                                                 :custom-label="NameModule"
                                                                 placeholder="Pick Module"
                                                                 :disabled="popQuizForm.isGrouping === false"
                                                                 :allow-empty="false"
                                                                 deselect-label="Can't remove this value"
                                                                 @search-change="ModuleAutoComplete"
                                                                 @open="resetModuleOption">

                                                    </multiselect>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-9">
                                        <div class="row justify-content-between">
                                            <div class="col-md-4">
                                                <label>Total Participant<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <input type="number"
                                                           name="Total Participant"
                                                           v-validate="'required'"
                                                           min="0"
                                                           class="form-control"
                                                           v-model.number="popQuizForm.totalParticipant"
                                                           :disabled="popQuizForm.isGrouping === false"
                                                           @change="generateQuestionPerParticipant" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <label>Total Question<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <input type="number"
                                                           name="Total Question"
                                                           v-validate="'required|min_value:1'"
                                                           min="0"
                                                           class="form-control"
                                                           v-model.number="popQuizForm.totalQuestion"
                                                           :disabled="popQuizForm.isGrouping === false"
                                                           @change="generateQuestionPerParticipant" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <label>Question/Participant<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <input type="number"
                                                           class="form-control"
                                                           v-model.number="popQuizForm.questionPerParticipant" disabled />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-md-3">
                                        <label>Generate</label>

                                        <div>
                                            <button :disabled="popQuizForm.isGrouping === false"
                                                    class="btn talent_bg_blue talent_roundborder talent_font_white"
                                                    @click.prevent="GenerateTaskForm()">
                                                Generate
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between">
                            <div class="col-md-1">
                                <label>Question Number</label>
                            </div>
                            <div class="col-md-9">
                                <label>Task Code<span class="talent_font_red">*</span></label>
                            </div>
                            <div class="col-md-2 d-flex justify-content-end align-items-center">
                                <button class="btn talent_bg_blue talent_roundborder talent_font_white"
                                        @click.prevent="AddTaskCode()">
                                    Add Task Code
                                </button>
                            </div>
                        </div>

                        <div class="row justify-content-between">
                            <div class="col-md-12" v-for="(loop, index) in taskCodeForm">
                                <div class="row justify-content-between mb-md-4">
                                    <div class="col-md-1">
                                        <div class="input-group">
                                            <input class="form-control" :placeholder="index + 1" disabled />
                                        </div>
                                    </div>
                                    <div class="col-md-10">
                                        <div class="input-group">
                                            <multiselect v-model="taskCodeForm[index]"
                                                         :name="'Task Code No.' + (index + 1)"
                                                         v-validate="'required-multiselect'"
                                                         :options="taskCodeOption"
                                                         :searchable="true"
                                                         :custom-label="NameTaskCode"
                                                         placeholder="Pick Task Code"
                                                         :allow-empty="false"
                                                         :loading="isLoadingOption"
                                                         :showNoResults="true"
                                                         deselect-label="Can't remove this value"
                                                         @search-change="AutoComplete"
                                                         >

                                                <span slot="noResult"><i>Not Found!</i></span>
                                            </multiselect>
                                        </div>
                                    </div>
                                    <div class="col-md-1">
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="DeleteTaskCode(index)" :disabled="taskCodeForm.length === 1">
                                            Remove
                                        </button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="gotoSetupLearning">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="savePopQuiz">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_blue talent_font_white" @click="submitPopQuiz">
                                        Submit
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <!--VIEW DETAIL-->
        <div class="row" v-if="mode === pageMode.DETAIL">
            <div class="col col-md-12">
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup Learning > Setup Pop Quiz > <span class="talent_font_red">View Detail</span></h3>

                <!--Add-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4>Setup Quiz</h4>

                    <div class="row justify-content-between">
                        <div class="col-md-12">
                            <div class="row justify-content-between mb-md-4">
                                <div class="col-md-12">
                                    <label>Quiz Title<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="popQuizForm.quizTitle" disabled />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div class="col-md-12 mb-md-4">
                            <div class="row">
                                <h4>Choose Your Question</h4>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-3">
                                        <label>Test Time<span class="talent_font_red">*</span></label>
                                        <div class="input-group">
                                            <input type="number" class="form-control" placeholder="minute" v-model.number="popQuizForm.testTime" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-6">
                                <label>Type Task<span class="talent_font_red">*</span></label>
                                <div class="row justify-content-between">
                                    <div class="col-md-6">
                                        <div class="form-check form-check-inline">
                                            <input class="form-check-input" type="radio" name="typeOfBanner" v-model="popQuizForm.isGrouping"
                                                   :value="false" id="inlineRadio1" @change="ResetTaskCodeForm()" disabled>
                                            <label class="form-check-label" for="inlineRadio1">Specific</label>
                                        </div>
                                        <div class="form-check form-check-inline">
                                            <input class="form-check-input" type="radio" name="typeOfBanner" v-model="popQuizForm.isGrouping"
                                                   :value="true" id="inlineRadio2" @change="ResetTaskCodeForm()" disabled>
                                            <label class="form-check-label" for="inlineRadio2">Grouping</label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-6">
                                        <div class="row justify-content-between">
                                            <div class="col-md-6">
                                                <label>Competency<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <multiselect v-model="competencyForm"
                                                                 :options="competencyOption"
                                                                 :searchable="true"
                                                                 :custom-label="NameCompetency"
                                                                 placeholder="Pick Competency"
                                                                 :disabled="true"
                                                                 :allow-empty="false"
                                                                 deselect-label="Can't remove this value">

                                                    </multiselect>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <label>Module<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <multiselect v-model="moduleForm"
                                                                 :options="moduleOption"
                                                                 :searchable="true"
                                                                 :custom-label="NameModule"
                                                                 placeholder="Pick Module"
                                                                 :disabled="true"
                                                                 :allow-empty="false"
                                                                 deselect-label="Can't remove this value">

                                                    </multiselect>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between mb-md-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-9">
                                        <div class="row justify-content-between">
                                            <div class="col-md-4">
                                                <label>Total Participant<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <input type="number"
                                                           class="form-control"
                                                           v-model.number="popQuizForm.totalParticipant"
                                                           @change="generateQuestionPerParticipant"
                                                           disabled />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <label>Total Question<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <input type="number"
                                                           class="form-control"
                                                           v-model.number="popQuizForm.totalQuestion"
                                                           @change="generateQuestionPerParticipant"
                                                           disabled />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <label>Question/Participant<span class="talent_font_red">*</span></label>
                                                <div class="input-group">
                                                    <input type="number"
                                                           class="form-control"
                                                           v-model.number="popQuizForm.questionPerParticipant"
                                                           disabled />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="row justify-content-between">
                            <div class="col-md-1">
                                <label>Question Number</label>
                            </div>
                            <div class="col-md-9">
                                <label>Task Code<span class="talent_font_red">*</span></label>
                            </div>
                        </div>

                        <div class="row justify-content-between">
                            <div class="col-md-12" v-for="(loop, index) in taskCodeForm">
                                <div class="row justify-content-between mb-md-4">
                                    <div class="col-md-1">
                                        <div class="input-group">
                                            <input class="form-control" :placeholder="index + 1" disabled />
                                        </div>
                                    </div>
                                    <div class="col-md-10">
                                        <div class="input-group">
                                            <multiselect v-model="taskCodeForm[index]"
                                                         :options="taskCodeOption"
                                                         :searchable="true"
                                                         :custom-label="NameTaskCode"
                                                         placeholder="Pick Task Code"
                                                         :allow-empty="false"
                                                         :loading="isLoadingOption"
                                                         :showNoResults="true"
                                                         :disabled="true"
                                                         deselect-label="Can't remove this value"
                                                         @search-change="AutoComplete"
                                                         >

                                                <span slot="noResult"><i>Not Found!</i></span>
                                            </multiselect>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white" @click="goBackPage()">
                                        Close
                                    </button>
                                    <button v-else class="btn talent_bg_red talent_font_white" @click="gotoSetupLearning()">
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
    import { PageModeEnum } from '../../enum/PageModeEnum';
    import { ApprovalStatusEnum } from '../../enum/ApprovalStatusEnum';
    import { SetupPopQuizService, CompetencySetupModel, ModuleSetupModel, TaskCodeSetupModel, SetupPopUpQuizCreateModel, SetupPopQuizUpdateModel } from '../../services/NSwagService';
    import { isNullOrUndefined } from 'util';

    @Component({
        created: async function (this: SetupPopUpQuiz) {
            if (this.mode === this.pageMode.DETAIL || this.mode === this.pageMode.EDIT) {
                await this.GetPopQuiz();
            }
            this.taskCodeOption = await this.Service.getTaskCode(this.competencyForm.competencyId, this.moduleForm.moduleId, 'a');
            
        },

        props: ['mode', 'popQuizId', 'fromOutside']
    })

    export default class SetupPopUpQuiz extends Vue {
        Service: SetupPopQuizService = new SetupPopQuizService();

        mode: string;
        popQuizId: number;
        fromOutside: boolean;

        isLoading: boolean = false;
        isLoadingOption: boolean = false;
        timeout: any;

        pageMode = PageModeEnum;

        popQuizForm: ISetupPopQuizFormModel = {
            quizTitle: '',
            testTime: null,
            isGrouping: false,
            competencyId: null,
            competencyTypeId: null,
            moduleId: null,
            totalParticipant: null,
            totalQuestion: 1,
            questionPerParticipant: null,
            taskCodeId: []
        };

        competencyOption: CompetencySetupModel[] = [];
        competencyForm: CompetencySetupModel = {};

        moduleOption: ModuleSetupModel[] = [];
        moduleForm: ModuleSetupModel = {};

        taskCodeAllOption: TaskCodeSetupModel[] = [];
        taskCodeOption: TaskCodeSetupModel[] = [];

        taskCodeForm: TaskCodeSetupModel[] = [{}];

        popQuizInsertForm: SetupPopUpQuizCreateModel = {
            quizTitle: '',
            testTime: 0,
            approvalId: 0,
            isGrouping: false,
            competencyId: null,
            moduleId: null,
            totalParticipant: null,
            totalQuestion: null,
            questionPerParticipant: null,
            taskIds: []
        };

        popQuizUpdateForm: SetupPopQuizUpdateModel = {
            popQuizId: 0,
            quizTitle: '',
            testTime: 0,
            approvalId: 0,
            isGrouping: false,
            competencyId: null,
            moduleId: null,
            totalParticipant: null,
            totalQuestion: null,
            questionPerParticipant: null,
            taskIds: []
        }

        validateGenerate: string[] = ['Competency', 'Module', 'Total Question'];
        validateSpecificFormSubmit: string[] = ['Quiz Title', 'Test Time', 'Total Participant', 'Total Question'];
        validateGroupingFormSubmit: string[] = ['Quiz Title', 'Test Time', 'Competency', 'Module', 'Total Participant', 'Total Question'];

        validateFormSave: string[] = ['Quiz Title', 'Test Time'];
        validateFormSaveBackUp: string[] = ['Quiz Title', 'Test Time'];

        error: string = '';

        async GetPopQuiz() {
            this.isLoading = true;
            let popQuizDetail = await this.Service.getPopQuizDetail(this.popQuizId);
            this.popQuizForm.quizTitle = popQuizDetail.quizTitle;
            this.popQuizForm.testTime = popQuizDetail.testTime;
            this.popQuizForm.isGrouping = popQuizDetail.isGrouping;

            this.competencyOption = await this.Service.getCompetency('');
            this.moduleOption = await this.Service.getModule('');

            if (popQuizDetail.isGrouping === true) {
                let competencyIndex = this.competencyOption.findIndex(Q => Q.competencyId === popQuizDetail.competencyId);
                isNullOrUndefined(this.competencyOption[competencyIndex]) == true ? this.competencyForm : this.competencyForm = this.competencyOption[competencyIndex];

                let moduleIndex = this.moduleOption.findIndex(Q => Q.moduleId === popQuizDetail.moduleId);
                isNullOrUndefined(this.moduleOption[moduleIndex]) == true ? this.moduleForm : this.moduleForm = this.moduleOption[moduleIndex];
            }

            this.popQuizForm.totalParticipant = popQuizDetail.totalParticipant;
            this.popQuizForm.totalQuestion = popQuizDetail.totalQuestion;
            this.popQuizForm.questionPerParticipant = popQuizDetail.questionPerParticipant;

            if (popQuizDetail.taskIds.length !== 0) {
                this.taskCodeForm = popQuizDetail.taskIds;
            }

            
            this.isLoading = false;
        }

        AddTaskCode() {
            this.taskCodeForm.push({});

            this.popQuizForm.totalQuestion = this.taskCodeForm.length;
        }

        DeleteTaskCode(indexToDelete: number) {
            this.taskCodeForm.splice(indexToDelete, 1);
            this.popQuizForm.totalQuestion = this.taskCodeForm.length;
            this.reloadTaskCodeOption();
        }

        generateQuestionPerParticipant() {
            if (this.popQuizForm.totalParticipant > this.popQuizForm.totalQuestion) {
                this.popQuizForm.totalParticipant = this.popQuizForm.totalQuestion;
            }

            if (this.popQuizForm.totalQuestion == null || this.popQuizForm.totalParticipant == null) {
                return;
            }

            this.popQuizForm.questionPerParticipant = Math.floor(this.popQuizForm.totalQuestion / this.popQuizForm.totalParticipant);
        }

        async AutoComplete(query) {
            this.isLoadingOption = true;
            if (query == null || query === '') {
                this.resetOption();
                this.isLoadingOption = false;
                return;
            }

            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetTaskCode(query);
                }, 500
            );

        }

        async CompetencyAutoComplete(query) {
            this.isLoadingOption = true;
            if (query == null || query === '') {
                this.resetCompetencyOption();
                this.isLoadingOption = false;
                return;
            }

            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetCompetency(query);
                }, 500
            );
        }

        async ModuleAutoComplete(query) {
            this.isLoadingOption = true;
            if (query == null || query === '') {
                this.resetModuleOption();
                this.isLoadingOption = false;
                return;
            }

            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetModule(query);
                }, 500
            );
        }

        async GetTaskCode(query) {
            if (query === '' || query == null) {
                this.resetOption();
                return;
            }

            this.taskCodeAllOption = await this.Service.getTaskCode(this.competencyForm.competencyId, this.moduleForm.moduleId, query);
            this.taskCodeOption = this.taskCodeAllOption;

            for (let index of this.taskCodeForm) {
                let found = this.taskCodeOption.findIndex(Q => Q.taskId === index.taskId);

                if (found >= 0) {
                    this.taskCodeOption.splice(found, 1);
                }
            }
            this.isLoadingOption = false;
        }

        async GetCompetency(query) {
            if (query === '' || query == null) {
                this.resetCompetencyOption();
                return;
            }

            this.competencyOption = await this.Service.getCompetency(query);
            this.isLoadingOption = false;
        }

        async GetModule(query) {
            if (query === '' || query == null) {
                this.resetModuleOption();
                return;
            }

            this.moduleOption = await this.Service.getModule(query);
            this.isLoadingOption = false;
        }

        resetOption() {
            this.taskCodeOption = [];
        }

        resetCompetencyOption() {
            this.competencyOption = [];
        }

        resetModuleOption() {
            this.moduleOption = [];
        }

        async ResetTaskCodeForm() {
            this.taskCodeForm = [{}]
            this.popQuizForm.totalQuestion = 1;
            this.$validator.reset();

            if (this.popQuizForm.isGrouping === false) {
                this.competencyForm = {};
                this.moduleForm = {};

                this.popQuizForm.totalParticipant = null;
                this.popQuizForm.questionPerParticipant = null;
            }
        }

        async GenerateTaskForm() {
            this.isLoading = true;
            let valid = await this.$validator.validateAll(this.validateGenerate);
            if (!valid) {
                this.isLoading = false;
                return;
            }

            this.$validator.reset();

            this.taskCodeAllOption = await this.Service.getTaskCode(this.competencyForm.competencyId, this.moduleForm.moduleId, '');

            if (this.taskCodeAllOption.length === 0) {
                this.popQuizForm.totalQuestion = 1;

                this.$validator.errors.add({
                    field: 'Quiz Title',
                    msg: 'There is no task code for this competency and module',
                });

                this.isLoading = false;
                return;
            }

            if (Number(this.popQuizForm.totalQuestion) > this.taskCodeAllOption.length) {
                this.popQuizForm.totalQuestion = this.taskCodeAllOption.length;
                this.generateQuestionPerParticipant();
            }

            this.taskCodeForm = [];
            for (let i = 0; i < this.popQuizForm.totalQuestion; i++) {
                this.taskCodeForm.push(this.taskCodeAllOption[i]);
            }

            this.reloadTaskCodeOption();
            this.isLoading = false;
        }

        reloadTaskCodeOption() {
            this.taskCodeOption = this.taskCodeAllOption.filter(Q => !this.taskCodeForm.includes(Q));
        }

        async savePopQuiz() {
            this.isLoading = true;

            let validateForm = this.validateFormSave;

            if (this.taskCodeForm.length > 1 || !isNullOrUndefined(this.taskCodeForm[0].taskId)) {
                let tempIndex = 1;
                for (let taskCode in this.taskCodeForm) {
                    validateForm.push('Task Code No.' + tempIndex);
                    tempIndex++;
                }
            } else {
                validateForm = this.validateFormSaveBackUp;
            }

            let valid = await this.$validator.validateAll(validateForm);
            if (!valid) {
                this.isLoading = false;
                return;
            }

            if (this.mode === 'Add') {
                this.popQuizInsertForm.approvalId = ApprovalStatusEnum.DRAFT;
                await this.insertPopQuiz();
            }

            else {
                this.popQuizUpdateForm.approvalId = ApprovalStatusEnum.DRAFT;
                await this.updatePopQuiz();
            }
        }

        async submitPopQuiz() {
            this.isLoading = true;

            let validateForm = this.validateSpecificFormSubmit;

            if (this.popQuizForm.isGrouping === true) {
                validateForm = this.validateGroupingFormSubmit;
            }

            let tempIndex = 1;
            for (let taskCode in this.taskCodeForm) {
                validateForm.push('Task Code No.' + tempIndex);
                tempIndex++;
            }

            let valid = await this.$validator.validateAll(validateForm);
            if (!valid) {
                this.isLoading = false;
                return;
            }

            if (this.mode === 'Add') {
                this.popQuizInsertForm.approvalId = ApprovalStatusEnum.WAITING;
                await this.insertPopQuiz();
            }

            else {
                this.popQuizUpdateForm.approvalId = ApprovalStatusEnum.WAITING;
                await this.updatePopQuiz();
            }
        }

        async insertPopQuiz() {
            this.$validator.errors.clear();

            this.popQuizInsertForm.quizTitle = this.popQuizForm.quizTitle;
            this.popQuizInsertForm.testTime = this.popQuizForm.testTime;
            this.popQuizInsertForm.isGrouping = this.popQuizForm.isGrouping;

            if (this.popQuizForm.isGrouping == true) {
                this.popQuizInsertForm.competencyId = this.competencyForm.competencyId;
                this.popQuizInsertForm.moduleId = this.moduleForm.moduleId;
            }

            this.popQuizInsertForm.totalParticipant = this.popQuizForm.totalParticipant;
            this.popQuizInsertForm.totalQuestion = this.popQuizForm.totalQuestion;
            this.popQuizInsertForm.questionPerParticipant = this.popQuizForm.questionPerParticipant;

            this.popQuizInsertForm.taskIds = [];
            if (this.taskCodeForm.length > 0 && !isNullOrUndefined(this.taskCodeForm[0].taskId)) {
                for (let taskCode of this.taskCodeForm) {

                    this.popQuizInsertForm.taskIds.push(taskCode.taskId);
                }
            }

            let success = await this.Service.insertPopQuiz(this.popQuizInsertForm);
            if (!success) {
                this.isLoading = false;
                this.$validator.errors.add({
                    field: 'Quiz Title',
                    msg: 'There is an existing quiz title, please use another title',
                })
                return;
            }

            this.popQuizInsertForm = {};

            this.isLoading = false;
            this.gotoSetupLearning();
        }

        async updatePopQuiz() {
            this.$validator.errors.clear();

            this.popQuizUpdateForm.popQuizId = this.popQuizId;
            this.popQuizUpdateForm.quizTitle = this.popQuizForm.quizTitle;
            this.popQuizUpdateForm.testTime = this.popQuizForm.testTime;
            this.popQuizUpdateForm.isGrouping = this.popQuizForm.isGrouping;

            if (this.popQuizForm.isGrouping == true) {
                if (!isNullOrUndefined(this.competencyForm)) this.popQuizUpdateForm.competencyId = this.competencyForm.competencyId;
                if (!isNullOrUndefined(this.moduleForm)) this.popQuizUpdateForm.moduleId = this.moduleForm.moduleId;
            }

            this.popQuizUpdateForm.totalParticipant = this.popQuizForm.totalParticipant;
            this.popQuizUpdateForm.totalQuestion = this.popQuizForm.totalQuestion;
            this.popQuizUpdateForm.questionPerParticipant = this.popQuizForm.questionPerParticipant;


            this.popQuizUpdateForm.taskIds = [];
            if (this.taskCodeForm.length > 0 && !isNullOrUndefined(this.taskCodeForm[0].taskId)) {
                for (let taskCode of this.taskCodeForm) {
                    this.popQuizUpdateForm.taskIds.push(taskCode.taskId);
                }
            }

            let success = await this.Service.editPopQuiz(this.popQuizUpdateForm);
            if (!success) {
                this.isLoading = false;
                this.$validator.errors.add({
                    field: 'Quiz Title',
                    msg: 'There is an existing quiz title, please use another title',
                })
                return;
            }

            this.popQuizUpdateForm = {};

            this.isLoading = false;
            this.gotoSetupLearning();
        }

        NameTaskCode({ competencyTypeName, prefixCode, evaluationTypeName, taskNumber, moduleName }): string {
            if (competencyTypeName == null || prefixCode == null || evaluationTypeName == null || taskNumber === 0 || moduleName == null) {
                return null;
            }

            let taskCodeName = competencyTypeName.charAt(0) + '-' + prefixCode + '-' + evaluationTypeName + '-' + taskNumber + '-' + moduleName;

            return taskCodeName;
        }

        NameCompetency({ competencyTypeName, prefixCode }) {
            if (competencyTypeName == null || prefixCode == null) {
                return null;
            }

            let competencyName = competencyTypeName.charAt(0) + '-' + prefixCode;

            return competencyName;
        }

        NameModule({ moduleName }) {
            if (moduleName == null) {
                return null;
            }

            let module = moduleName;

            return module;
        }

        gotoSetupLearning() {
            window.location.href = "/Setup/SetupLearning";
        }

        goBackPage() {
            window.history.back();
        }
    }
</script>
