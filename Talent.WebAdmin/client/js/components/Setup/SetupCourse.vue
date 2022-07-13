<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="col-md-12">
            <div class="row">
                <div class="col col-md-12">
                    <!--TITLE-->
                    <h3><fa icon="folder"></fa> Setup Learning > Setup Course > <span class="talent_font_red"><span v-if="mode == 'Add'">Add</span><span v-if="mode == 'Edit'">Edit</span><span v-if="mode == 'View'">View Detail</span></span></h3>
                    <br />

                    <!--Add-->
                    <div class="row">
                        <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                            <h4>Course</h4>
                            <hr/>
                            <div class="row">
                                <div class="col-md-12">
                                    <label>Course Name<span class="talent_font_red">*</span></label>
                                    <!--<select class="form-control" v-model="courseModel" @change.prevent="changeCourse" v-if="mode == 'Add'">
                                        <option v-for="c in courses.listCourseModel" :value="c">{{c.courseName}}</option>
                                    </select>-->
                                    <div class="col-md-12" v-if="mode == 'Add'">
                                        <div class="input-group">
                                            <multiselect v-model="courseModel"
                                                        :options="courses.listCourseModel ? courses.listCourseModel : []"
                                                        label="courseName"
                                                        v-validate="'required'"
                                                        :searchable="true"
                                                        @select="changeCourse"></multiselect>
                                        </div>
                                    </div>
                                    <input class="form-control" v-model="courseModel.courseName" v-if="mode == 'Edit' || mode == 'View'" disabled>
                                </div>
                            </div>
                            <div class="alert alert-danger" v-if="isTrainingTypeEmpty() == true && (isSubmit == true && submitClickedValidateTrainingType() == false)">
                                <div v-if="validationCourseTrainingTypeMappingMinimumScore(0)">Pre Training Minimum Score is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingMinimumScore(1)">During Training Minimum Score is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingMinimumScore(2)">Post Training Minimum Score is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingMinimumScore(3)">Course Minimum Score is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingWorkload(0)">Pre Training Workload Requirement is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingWorkload(1)">During Training Workload Requirement is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingWorkload(2)">Post Training Workload Requirement is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingWorkload(3)">Course Workload Requirement is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingPercentage(0)">Pre Training Percentage is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingPercentage(1)">During Training Percentage is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingPercentage(2)">Post Training Percentage is Required</div>
                                <div v-if="validationCourseTrainingTypeMappingPercentage(3)">Course Percentage is Required</div>
                                <div v-if="courseModules.length === 0">Module / Assessment is Required</div>
                            </div>

                            <div class="row justify-content-between mt-2">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <label>Category on Web (For Trial Only)</label>
                                                    <div class="input-group">
                                                        <div class="form-check form-check-inline">
                                                            <input class="form-check-input" type="checkbox" id="inlineCheckbox1" v-model="setupCourseModel.isRecommendedForYou" :disabled="mode == 'View'">
                                                            <label class="form-check-label" for="inlineCheckbox1">Recommended for You</label>
                                                        </div>
                                                        <div class="form-check form-check-inline">
                                                            <input class="form-check-input" type="checkbox" id="inlineCheckbox2" v-model="setupCourseModel.isPopular" :disabled="mode == 'View'">
                                                            <label class="form-check-label" for="inlineCheckbox2">Popular</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                </div>

                                <div class="col-md-12">
                                    <table class="table table-bordered table-hover matrix_table_input" id="tab_logic">
                                        <thead>
                                            <tr>
                                                <th class="text-center">
                                                    <label class="talent_nomargin">Training Scheme</label>
                                                </th>
                                                <th class="text-center">
                                                    <label class="talent_nomargin">Minimum Score</label>
                                                </th>
                                                <th class="text-center">
                                                    <label class="talent_nomargin">Workload Requirement</label>
                                                </th>
                                                <th class="text-center">
                                                    <label class="talent_nomargin">Percentage</label>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td class="text-center talent_nomargin talent_nopadding">
                                                    <label class="h-100 w-100 talent_nomargin talent_nopadding">Pre</label>
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[0].minimumScore" type="number" min="0" :disabled="mode == 'View'" />
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[0].workloadRequirement" :disabled="mode == 'View'" />
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[0].percentage" type="number" min="0" :disabled="mode == 'View'" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text-center talent_nomargin talent_nopadding">
                                                    <label class="talent_nomargin talent_nopadding">During</label>
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[1].minimumScore" type="number" min="0" :disabled="mode == 'View'" />
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[1].workloadRequirement" :disabled="mode == 'View'" />
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[1].percentage" type="number" min="0" :disabled="mode == 'View'" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text-center">
                                                    <label class="talent_nomargin talent_nopadding">Post</label>
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[2].minimumScore" type="number" min="0" :disabled="mode == 'View'" />
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[2].workloadRequirement" :disabled="mode == 'View'" />
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[2].percentage" type="number" min="0" :disabled="mode == 'View'" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="text-center">
                                                    <label class="talent_nomargin talent_nopadding">Total Course Score</label>
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[3].minimumScore" type="number" min="0" :disabled="mode == 'View'" />
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[3].workloadRequirement" :disabled="mode == 'View'" />
                                                </td>
                                                <td>
                                                    <input class="form-control" v-model="courseTrainingTypeMappingModels[3].percentage" type="number" min="0" :disabled="mode == 'View'" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Prerequisite (Setup Course & Setup Module)</label>
                                            <div v-for="(p ,index) in coursePrerequisites">
                                                <div class="row">
                                                    <div class="col-md-1 text-center">
                                                        <a class="d-flex align-items-center w-100 h-100">{{index + 1}}</a>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <multiselect v-model="coursePrerequisites[index]"
                                                                     :options="prerequisites"
                                                                     :custom-label="NamePrerequisite"
                                                                     :loading="isPrerequisitesLoading"
                                                                     :disabled="mode == 'View'"
                                                                     placeholder=""
                                                                     @search-change="initPrerequisitesDropdown">
                                                        </multiselect>
                                                    </div>
                                                    <div class="col-md-1" v-if="mode !== 'View'">
                                                        <button class="button_text_no_background" @click.prevent="AddPrerequisite()" v-if="index + 1 == totalPrerequisite">
                                                            <fa icon="plus-circle"></fa>
                                                        </button>
                                                        <button class="button_text_no_background" @click.prevent="DeletePrerequisite(index)" v-else>
                                                            <fa icon="trash-alt"></fa>
                                                        </button>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>

                                        </div>
                                        <div class="col-md-6">
                                            <label>Learning Objective</label>
                                            <div v-for="(learningObjective,index) in courseLearningObjectives">
                                                <div class="row">
                                                    <div class="col-md-1 text-center">
                                                        <a class="d-flex align-items-center w-100 h-100">{{index + 1}}</a>
                                                    </div>
                                                    <div class="col-md-10">
                                                        <div class="input-group">
                                                            <input class="form-control" v-model="courseLearningObjectives[index]" :disabled="mode == 'View'" />
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1" v-if="mode !== 'View'">
                                                        <button class="button_text_no_background" @click.prevent="AddLearningObjective()" v-if="index + 1 == totalLearningObjective">
                                                            <fa icon="plus-circle"></fa>
                                                        </button>
                                                        <button class="button_text_no_background" @click.prevent="DeleteLearningObjective(index)" v-else>
                                                            <fa icon="trash-alt"></fa>
                                                        </button>
                                                    </div>
                                                </div>
                                                <br />
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                </div>

                                <div class="col-md-12">
                                    <div class="row justify-content-end">
                                        <div class="col-auto">
                                            <button class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="AddModule" v-if="mode != 'View'">Add Module</button>
                                            <button class="btn talent_bg_orange talent_roundborder talent_font_white" @click.prevent="AddModule('assessment')" v-if="mode != 'View'">Add Assessment</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <br />

                    <div class="row">
                        <div class="col-md-12" v-for="(module,index) in courseModules" :key="index">
                            <!-- module -->
                            <div class="row" v-if="Object.keys(module.assessment).length === 0">
                                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="row justify-content-between">
                                                        <div class="col-md-2">
                                                            <h4>Module</h4>
                                                        </div>
                                                        <div class=" col-md-4 ">
                                                            <div class="d-flex justify-content-end">
                                                                <!-- <button class="btn talent_bg_red talent_roundborder talent_font_white" @click.prevent="RemoveModule(index)" disabled v-if="index == 0 && totalModule == 1 ">Remove</button> -->
                                                                <button class="btn talent_bg_red talent_roundborder talent_font_white" @click.prevent="RemoveModule(index)" if="mode != 'View'">Remove</button>
                                                                <!-- <button class="btn talent_bg_blue talent_roundborder talent_font_white" @click.prevent="AddModule(index)" v-if="mode != 'View'">Add Module</button> -->
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="alert alert-danger" v-if="module.isModuleError(2) == true && (isSubmit == true && submitClickedValidateModule(module) == false)">
                                                <div v-if="module.typeOfTraining == null">Type Of Training is Required</div>
                                                <div v-if="module.minimumScore < 0 || module.minimumScore.toString() == ''">Minimum Score is Required</div>
                                                <div v-if="module.timepoint.timePointId == 0">Learning Time is Required</div>
                                                <div v-if="module.module.moduleName == ''">Module is Required</div>
                                                <div v-if="module.enumRemedialOption === 'Limit' && Number(module.remedialLimit) <= 1">Remedial Limit must higher than 1</div>
                                            </div>
                                            <div class="alert alert-danger" v-if="module.isModuleError(3) == true && (isSave == true && saveClickedValidateModule(module) == false)">
                                                <div v-if="module.typeOfTraining == null">Type Of Training is Required</div>
                                                <div v-if="module.module.moduleName == ''">Module is Required</div>
                                                <div v-if="module.timepoint.timePointId == 0">Learning Time is Required</div>
                                                <div v-if="module.enumRemedialOption === 'Limit' && Number(module.remedialLimit) <= 1">Remedial Limit must higher than 1</div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="row justify-content-between">
                                                        <div class="col-md-3">
                                                            <label>Type of Training<span class="talent_font_red">*</span></label>
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-12 float-left">
                                                                    <button v-if="module.typeOfTraining == 1" class="btn setup-module-button talent_bg_red talent_font_white">
                                                                        Pre
                                                                    </button>
                                                                    <button v-else-if="mode != 'View'" class="btn setup-module-button talent_bg_darkgrey talent_font_white" @click.prevent="ChangeTypeofTraining(index,1)">
                                                                        Pre
                                                                    </button>
                                                                    <button v-if="module.typeOfTraining == 2" class="btn setup-module-button talent_bg_orange talent_font_white">
                                                                        During
                                                                    </button>
                                                                    <button v-else-if="mode != 'View'" class="btn setup-module-button talent_bg_darkgrey talent_font_white" @click.prevent="ChangeTypeofTraining(index,2)">
                                                                        During
                                                                    </button>
                                                                    <button v-if="module.typeOfTraining == 3" class="btn setup-module-button talent_bg_green talent_font_white">
                                                                        Post
                                                                    </button>
                                                                    <button v-else-if="mode != 'View'" class="btn setup-module-button talent_bg_darkgrey talent_font_white" @click.prevent="ChangeTypeofTraining(index,3)">
                                                                        Post
                                                                    </button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <label>Minimum Score<span class="talent_font_red">*</span></label>
                                                            <div class="input-group">
                                                                <input class="form-control" v-model="module.minimumScore" type="number" min="0" :disabled="mode == 'View'" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <label>Learning Time (Minutes)<span class="talent_font_red">*</span></label>
                                                            <select class="form-control" v-model="module.timepoint" :disabled="mode == 'View'">
                                                                <option v-for="t in timepoints" :value="t">{{t.time}}</option>
                                                            </select>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <label>Points</label>
                                                            <div class="input-group">
                                                                <input class="form-control" :value="module.timepoint.points" disabled />
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>
                                            <br />

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label>Module Name<span class="talent_font_red">*</span></label>
                                                    <multiselect v-model="module.module"
                                                                 :options="getRemovableModules ? getRemovableModules: []"
                                                                 label="moduleName"
                                                                 :loading="isRemovableModulesLoading"
                                                                 :disabled="mode == 'View'"
                                                                 placeholder=""
                                                                 :allowEmpty="false"
                                                                 @search-change="initRemovableModulesDropdown"
                                                                 track-by="moduleName">
                                                    </multiselect>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="form-check form-check-inline">
                                                                <input class="form-check-input" type="checkbox" :id="index + 'needQuestion'" v-model="module.needQuestion" :disabled="mode == 'View'">
                                                                <label class="form-check-label" :for="index + 'needQuestion'">Need Question ?</label>
                                                            </div>

                                                            <!-- <div class="form-check form-check-inline">
                                                                <input class="form-check-input" type="checkbox" :id="index + 'isRemedial'" v-model="module.isRemedial" :disabled="mode == 'View'">
                                                                <label class="form-check-label" :for="index + 'isRemedial'">For Remedial ?</label>
                                                            </div> -->
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>

                                            <div class="row" v-if="module.needQuestion">
                                                <div class="col-md-12">
                                                    <label>Remedial Option<span class="talent_font_red">*</span></label>
                                                    <div class="row align-items-center">
                                                        <div class="col-md-12">
                                                            <input :id="`${index}-noneed`" class="mr-1" :name="`${index}-enumRemedialOption`" type="radio" value="No Need" v-model="module.enumRemedialOption"> 
                                                            <label :for="`${index}-noneed`" class="mr-4">No Need</label>
                                                            <input :id="`${index}-duringtrainingschedule`" class="mr-1" :name="`${index}-enumRemedialOption`" type="radio" value="During Training Schedule" v-model="module.enumRemedialOption"> 
                                                            <label :for="`${index}-duringtrainingschedule`" class="mr-4">During Training Schedule</label>
                                                            <input :id="`${index}-limit`" class="mr-1" :name="`${index}-enumRemedialOption`" type="radio" value="Limit" v-model="module.enumRemedialOption"> 
                                                            <label :for="`${index}-limit`" class="mr-4">Limit <span v-if="Number(module.remedialLimit) <= 1 && module.enumRemedialOption === 'Limit'" class="talent_font_red">*input must higher than 1</span></label>
                                                            <input class="mr-1" name="remedialLimit" type="text" :disabled="module.enumRemedialOption != 'Limit'" v-model="module.remedialLimit">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row" v-if="module.needQuestion">
                                                <div class="col-md-12">
                                                    <label>Scoring Method<span class="talent_font_red">*</span></label>
                                                    <div class="row align-items-center">
                                                        <div class="col-md-12">
                                                        <input :id="`${index}-average`" class="mr-1" :name="`${index}-numScoringMethod`" type="radio" value="Average" v-model="module.enumScoringMethod" :disabled="module.enumRemedialOption == 'No Need'"> 
                                                        <label :for="`${index}-average`" class="mr-4">Average</label>
                                                        <input :id="`${index}-highest`" class="mr-1" :name="`${index}-numScoringMethod`" type="radio" value="Highest" v-model="module.enumScoringMethod" :disabled="module.enumRemedialOption == 'No Need'"> 
                                                        <label :for="`${index}-highest`" class="mr-4">Highest</label>
                                                        <input :id="`${index}-latest`" class="mr-1" :name="`${index}-numScoringMethod`" type="radio" value="Latest" v-model="module.enumScoringMethod" :disabled="module.enumRemedialOption == 'No Need'"> 
                                                        <label :for="`${index}-latest`" class="mr-4">Latest</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="justify-content-between" v-if="module.needQuestion">
                                                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                                                    <div class="justify-content-between">
                                                        <div class="col-md-12">
                                                            <div class="row justify-content-between">
                                                                <h4>Choose Task</h4>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />

                                                    <div class="alert alert-danger" v-if="(module.isTaskError(2) == true || module.isTaskEmpty(2) == true || module.isTaskCodesAvailable == false) && (isSubmit == true && submitClickedValidateQuestion(module) == false)">
                                                        <div v-if="module.testTime.toString() == '' || module.testTime < 1">Test Time is Required</div>
                                                        <div v-if="(module.totalParticipant.toString() == '' || module.totalParticipant < 1) && module.isGrouping == true">Total Participant is Required</div>
                                                        <div v-if="(module.totalQuestion.toString() == '' || module.totalQuestion < 1) && module.isGrouping == true">Total Question is Required</div>
                                                        <div v-if="module.filterCompetency.competencyId == null && module.isGrouping == true">Competency is Required For Grouping Type</div>
                                                        <div v-if="module.filterModule.moduleId == null && module.isGrouping == true">Module is Required For Grouping Type</div>
                                                        <div v-if="module.isTaskEmpty(2) == true">All Task Code is Required</div>
                                                        <div v-if="module.isTaskCodesAvailable == false">No Task Code with that Competency and Module Found</div>
                                                    </div>

                                                    <div class="alert alert-danger" v-if="(module.isTaskError(3) == true || module.isTaskEmpty(3) == true || module.isTaskCodesAvailable == false) && (isSave == true && saveClickedValidateQuestion(module) == false)">
                                                        <div v-if="module.testTime.toString() == '' || module.testTime < 1">Test Time is Required</div>
                                                        <div v-if="module.isTaskEmpty(3) == true">All Task Code is Required</div>
                                                    </div>

                                                    <div class="row justify-content-between">
                                                        <div class="col-md-12">
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-3">
                                                                    <label>Test Time (Minutes)<span class="talent_font_red">*</span></label>
                                                                    <div class="input-group">
                                                                        <input class="form-control" type="number" placeholder="Minute" min="1" v-model="module.testTime" :disabled="mode == 'View'" />
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                        <div class="col-md-6">
                                                            <label>Type of Task<span class="talent_font_red">*</span></label>
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-6">
                                                                    <div class="form-check form-check-inline">
                                                                        <input class="form-check-input" type="radio" :name="index" v-model="module.isGrouping" :value=false :id="'inlineRadio1' + index" @change.prevent="resetTaskCodeDropdown(index)" :disabled="mode == 'View'">
                                                                        <label class="form-check-label" :for="'inlineRadio1' + index">Specific</label>
                                                                    </div>
                                                                    <div class="form-check form-check-inline">
                                                                        <input class="form-check-input" type="radio" :name="index" v-model="module.isGrouping" :value=true :id="'inlineRadio2' + index" @change.prevent="resetTaskCodeList(index)" :disabled="mode == 'View'">
                                                                        <label class="form-check-label" :for="'inlineRadio2' + index">Grouping</label>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-12">
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-6">
                                                                    <div class="row justify-content-between">
                                                                        <div class="col-md-6">
                                                                            <label>Competency<span class="talent_font_red">*</span></label>
                                                                            <div class="input-group">
                                                                                <multiselect v-model="module.filterCompetency"
                                                                                             :options="competencies"
                                                                                             :custom-label="NameCompetency"
                                                                                             :loading="isCompetenciesLoading"
                                                                                             :disabled="module.isGrouping == false || mode == 'View'"
                                                                                             placeholder=""
                                                                                             @search-change="initCompetenciesDropdown">
                                                                                </multiselect>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-6">
                                                                            <label>Module<span class="talent_font_red">*</span></label>
                                                                            <div class="input-group">
                                                                                <multiselect v-model="module.filterModule"
                                                                                             :options="modules"
                                                                                             :custom-label="NameModule"
                                                                                             :loading="isModulesLoading"
                                                                                             :disabled="module.isGrouping == false || mode == 'View'"
                                                                                             placeholder=""
                                                                                             @search-change="initModulesDropdown">
                                                                                </multiselect>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>

                                                        <div class="col-md-12">
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-9">
                                                                    <div class="row justify-content-between">
                                                                        <div class="col-md-4">
                                                                            <label>Total Participant<span class="talent_font_red">*</span></label>
                                                                            <div class="input-group">
                                                                                <input type="number" class="form-control" v-model="module.totalParticipant" :disabled="module.isGrouping == false || mode == 'View'" min="1" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <label>Total Question<span class="talent_font_red">*</span></label>
                                                                            <div class="input-group">
                                                                                <input type="number" class="form-control" v-model="module.totalQuestion" :disabled="module.isGrouping == false || mode == 'View'" min="1" />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <label>Question/Participant</label>
                                                                            <div class="input-group">
                                                                                <input class="form-control" :value="Math.floor(module.totalTask / module.totalParticipant)" disabled />
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                </div>
                                                                <div class="col-md-3" v-if="mode != 'View'">
                                                                    <label>Generate</label>
                                                                    <div>
                                                                        <button :disabled="module.isGrouping === false"
                                                                                class="btn talent_bg_blue talent_roundborder talent_font_white"
                                                                                @click.prevent="generateTaskCode(index)">
                                                                            Generate
                                                                        </button>
                                                                    </div>
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <div class="col-md-12">
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-1">
                                                                    <label>Question Number</label>
                                                                </div>
                                                                <div class="col-md-11 justify-content-between">
                                                                    <div class="row">
                                                                        <div class="col-md-8">
                                                                            <label>Task Code<span class="talent_font_red">*</span></label>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <label>Score</label>
                                                                        </div>
                                                                        <div class="col-md-2 d-flex justify-content-end align-items-center" v-if="mode != 'View'">
                                                                            <button class="btn talent_bg_blue talent_font_white" @click.prevent="AddTaskCode(index)">Add Task Code</button>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>

                                                        <div class="col-md-12">
                                                            <div class="row justify-content-between" v-for="(task, index2) in courseModules[index].tasks">
                                                                <div class="col-md-1">
                                                                    <div class="input-group">
                                                                        <input class="form-control" :placeholder="index2 + 1" disabled />
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-11">
                                                                    <div class="row justify-content-between">
                                                                        <div class="col-md-8">
                                                                            <div class="input-group">
                                                                                <multiselect v-model="module.tasks[index2]"
                                                                                             :options="module.taskCodes.filter(v => !selectedTasksInCourse.includes(v.taskCode))"
                                                                                             :custom-label="NameTaskCode"
                                                                                             :loading="module.isTaskCodesLoading"
                                                                                             :disabled="mode == 'View'"
                                                                                             placeholder=""
                                                                                             @search-change="initTaskCodesDropdown($event, index)"
                                                                                             @select="(x) => handleSelectedTask(x,index, index2)">
                                                                                </multiselect>
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <div class="input-group">
                                                                                <input class="form-control" :placeholder="[11, 7, 6].includes(module.tasks[index2].taskTypeId) ? module.tasks[index2].score : module.tasks[index2].score " disabled />
                                                                            </div>
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <button class="btn talent_bg_red talent_font_white" v-if="index2 == 0 && courseModules[index].totalTask == 1 && mode != 'View'" disabled>
                                                                                Remove
                                                                            </button>
                                                                            <button class="btn talent_bg_red talent_font_white" @click.prevent="DeleteTaskCode(index,index2)" v-else-if="mode != 'View'">
                                                                                Remove
                                                                            </button>
                                                                        </div>
                                                                    </div>
                                                                    <br />
                                                                </div>
                                                            </div>
                                                            <br />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- setting order -->
                                            <div class="row" v-if="courseModules.length > 1">
                                                <div class="col-md-12">
                                                    <hr class="w-100">
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="row justify-content-end">
                                                        <div class="col-auto">
                                                            <button class="btn talent_bg_blue talent_roundborder talent_font_white" @click.prevent="handleOrder('up', index)" v-if="index >= 1">
                                                                <fa icon="chevron-up" style="margin-left: 16px; margin-right: 16px;"></fa>
                                                            </button>

                                                            <button class="btn talent_bg_blue talent_roundborder talent_font_white" @click.prevent="handleOrder('down', index)" v-if="index < courseModules.length - 1">
                                                                <fa icon="chevron-down" style="margin-left: 16px; margin-right: 16px;"></fa>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                                <br />
                            </div>
                            <!-- assessment -->
                            <div class="row" v-else>
                                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="row justify-content-between">
                                                        <div class="col-md-2">
                                                            <h4>Assessment</h4>
                                                        </div>
                                                        <div class=" col-md-4 ">
                                                            <div class="d-flex justify-content-end">
                                                                <!-- <button class="btn talent_bg_red talent_roundborder talent_font_white" @click.prevent="RemoveModule(index)" disabled v-if="index == 0 && totalModule == 1 ">Remove</button> -->
                                                                <button class="btn talent_bg_red talent_roundborder talent_font_white" @click.prevent="RemoveModule(index)" if="mode != 'View'">Remove</button>
                                                                <!-- <button class="btn talent_bg_blue talent_roundborder talent_font_white" @click.prevent="AddModule(index)" v-if="mode != 'View'">Add Module</button> -->
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="alert alert-danger" v-if="(module.isAssessmentError() == true) && (isSubmit == true || isSave == true)">
                                                <!-- <div v-if="module.typeOfTraining == null">Type Of Training is Required</div>
                                                <div v-if="module.module.moduleName == ''">Module is Required</div> -->
                                                <div v-if="module.isAssessmentError() == true">Please fill all field skill check</div>
                                                <div v-if="module.assessment.learningTime == 0">Learning Time is Required</div>
                                                <div v-if="module.assessment.name == ''">Assessment name is Required</div>
                                                <div v-if="module.assessment.enumRemedialOption === 'Limit' && module.assessment.remedialLimit <= 1">Remedial Limit must higher than 1</div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="row justify-content-start align-items-center">
                                                        <div class="col-md-3">
                                                            <label>Type of Training<span class="talent_font_red">*</span></label>
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-12 float-left">
                                                                    <button v-if="module.assessment.enumTrainingType == 1" class="btn setup-module-button talent_bg_red talent_font_white">
                                                                        Pre
                                                                    </button>
                                                                    <button v-else-if="mode != 'View'" class="btn setup-module-button talent_bg_darkgrey talent_font_white" @click.prevent="ChangeTypeofTrainingAssessment(module,1)">
                                                                        Pre
                                                                    </button>
                                                                    <button v-if="module.assessment.enumTrainingType == 2" class="btn setup-module-button talent_bg_orange talent_font_white">
                                                                        During
                                                                    </button>
                                                                    <button v-else-if="mode != 'View'" class="btn setup-module-button talent_bg_darkgrey talent_font_white" @click.prevent="ChangeTypeofTrainingAssessment(module,2)">
                                                                        During
                                                                    </button>
                                                                    <button v-if="module.assessment.enumTrainingType == 3" class="btn setup-module-button talent_bg_green talent_font_white">
                                                                        Post
                                                                    </button>
                                                                    <button v-else-if="mode != 'View'" class="btn setup-module-button talent_bg_darkgrey talent_font_white" @click.prevent="ChangeTypeofTrainingAssessment(module,3)">
                                                                        Post
                                                                    </button>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <input :id="`${index}-totalskillcheck`" class="mr-1" :name="`${index}-isByOption`" type="radio" :value="false" v-model="module.assessment.isByOption" :checked="!module.assessment.isByOption" @change="(e) => handleChangeIsByOption(module, e)"> 
                                                            <label :for="`${index}-totalskillcheck`"> Total Skill Check</label>
                                                            <input :id="`${index}-byoption`" class="mr-1 ml-4" :name="`${index}-isByOption`" type="radio" :value="true" v-model="module.assessment.isByOption" :checked="module.assessment.isByOption" @change="(e) => handleChangeIsByOption(module, e)"> 
                                                            <label :for="`${index}-byoption`"> By Option</label>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <label>Minimum Score<span class="talent_font_red">*</span></label>
                                                            <div class="input-group">
                                                                <input class="form-control" v-model="module.minimumScore" type="number" min="0" :disabled="mode == 'View' || module.assessment.isByOption" />
                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <label>Learning Time (Minutes)<span class="talent_font_red">*</span></label>
                                                            <select class="form-control" v-model="module.assessment.learningTime" :disabled="mode == 'View'">
                                                                <option v-for="t in timepoints" :value="t">{{t.time}}</option>
                                                            </select>
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>
                                            <br />

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label>Assessment Name<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <input class="form-control" v-model="module.assessment.name" type="text" :disabled="mode == 'View'"/>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label>Remedial Option<span class="talent_font_red">*</span></label>
                                                    <div class="row align-items-center">
                                                        <div class="col-md-12">
                                                            <input :id="`${index}-noneed`" class="mr-1" :name="`${index}-enumRemedialOption`" type="radio" value="No Need" v-model="module.assessment.enumRemedialOption"> 
                                                            <label :for="`${index}-noneed`" class="mr-4">No Need</label>
                                                            <input :id="`${index}-duringtrainingschedule`" class="mr-1" :name="`${index}-enumRemedialOption`" type="radio" value="During Training Schedule" v-model="module.assessment.enumRemedialOption"> 
                                                            <label :for="`${index}-duringtrainingschedule`" class="mr-4">During Training Schedule</label>
                                                            <input :id="`${index}-limit`" class="mr-1" :name="`${index}-enumRemedialOption`" type="radio" value="Limit" v-model="module.assessment.enumRemedialOption"> 
                                                            <label :for="`${index}-limit`" class="mr-4">Limit <span class="talent_font_red">*input must higher than 1</span></label>
                                                            <input class="mr-1" name="remedialLimit" type="text" :disabled="module.assessment.enumRemedialOption != 'Limit'" v-model="module.assessment.remedialLimit">
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <label>Scoring Method<span class="talent_font_red">*</span></label>
                                                    <div class="row align-items-center">
                                                        <div class="col-md-12">
                                                        <input :id="`${index}-assessment-average`" class="mr-1" :name="`${index}-enumScoringMethod`" type="radio" value="Average" v-model="module.assessment.enumScoringMethod" :disabled="module.assessment.enumRemedialOption == 'No Need'"> 
                                                        <label :for="`${index}-assessment-average`" class="mr-4">Average</label>
                                                        <input :id="`${index}-assessment-highest`" class="mr-1" :name="`${index}-enumScoringMethod`" type="radio" value="Highest" v-model="module.assessment.enumScoringMethod" :disabled="module.assessment.enumRemedialOption == 'No Need'"> 
                                                        <label :for="`${index}-assessment-highest`" class="mr-4">Highest</label>
                                                        <input :id="`${index}-assessment-latest`" class="mr-1" :name="`${index}-enumScoringMethod`" type="radio" value="Latest" v-model="module.assessment.enumScoringMethod" :disabled="module.assessment.enumRemedialOption == 'No Need'"> 
                                                        <label :for="`${index}-assessment-latest`" class="mr-4">Latest</label>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <br />

                                            <!-- skill checks -->
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="row justify-content-end">
                                                        <div class="col-auto">
                                                            <button class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="handleAddSkillCheck(module)" v-if="mode != 'View'">Add Skill Check</button>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <div class="col-md-2">
                                                            <label>Order<span class="talent_font_red">*</span></label>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <label>Skill Check Name<span class="talent_font_red">*</span></label>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <label>Minimum Score</label>
                                                        </div>
                                                    </div>
                                                    <div class="row justify-content-between mt-2" v-for="(skillCheck, iSkillCheck) in module.assessment.assesmentSkillChecks" :key="iSkillCheck">
                                                        <div class="col-md-2">
                                                            <input class="form-control" type="number" v-model="skillCheck.order">
                                                        </div>
                                                        <div class="col-md-6">
                                                            <multiselect :ref="`${iSkillCheck}-skillcheck`" 
                                                                v-model="skillCheck.skillCheckGUID"
                                                                :options="skillChecks.filter(v => !selectedSkillChecksInCourse.includes(v.guid))"
                                                                track-by="guid"
                                                                label="title"
                                                                :disabled="mode == 'View'"
                                                                placeholder="">
                                                            </multiselect>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <input class="form-control" type="number" :value="skillCheck.skillCheckGUID ? skillCheck.skillCheckGUID.minimumScore : ''" :disabled="true">
                                                        </div>
                                                        <div class="col-md-2">
                                                            <button class="btn talent_bg_red talent_roundborder talent_font_white" @click.prevent="handleRemoveSkillCheck(module, iSkillCheck)" v-if="mode != 'View' && module.assessment.assesmentSkillChecks.length > 1">Remove</button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- setting order -->
                                            <div class="row" v-if="courseModules.length > 1">
                                                <div class="col-md-12">
                                                    <hr class="w-100">
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="row justify-content-end">
                                                        <div class="col-auto">
                                                            <button class="btn talent_bg_blue talent_roundborder talent_font_white" @click.prevent="handleOrder('up', index)" v-if="index >= 1">
                                                                <fa icon="chevron-up" style="margin-left: 16px; margin-right: 16px;"></fa>
                                                            </button>

                                                            <button class="btn talent_bg_blue talent_roundborder talent_font_white" @click.prevent="handleOrder('down', index)" v-if="index < courseModules.length - 1">
                                                                <fa icon="chevron-down" style="margin-left: 16px; margin-right: 16px;"></fa>
                                                            </button>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                </div>
                                <br />
                            </div>
                            <br />
                        </div>
                    </div>

                    <!-- summary -->
                    <div class="row">
                        <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-3">
                                            TOTAL MODULE
                                        </div>
                                        <div class="col-md-3">
                                            <input class="form-control" v-model="totalModule" disabled />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            TOTAL TIME (Minutes)
                                        </div>
                                        <div class="col-md-3">
                                            <input class="form-control" :value="totalTime()" disabled />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-3">
                                            TOTAL SCORE
                                        </div>
                                        <div class="col-md-3">
                                            <input class="form-control" :value="total('score')" disabled />
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-3">
                                            TOTAL POINTS
                                        </div>
                                        <div class="col-md-3">
                                            <input class="form-control" :value="total('points')" disabled />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!-- action -->
                    <div class="row">
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
                                            <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="saveData(3)" v-if="mode !== 'View'">
                                                Save
                                            </button>
                                            <button class="btn talent_bg_blue talent_font_white" @click.prevent="saveData(2)" v-if="mode !== 'View'">
                                                Submit
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
            </div>

        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { SetupCourseService, SetupModuleService, TaskService, ProgramTypeService, CourseModel, SetupCourseFormModel, CourseTrainingTypeMappingModel, CoursePrerequisiteViewModel, TaskCodeSetupModel, SetupTaskCodesFormModel, CourseViewModel } from '../../services/NSwagService'
    import { Sort } from '../../class/Sort';
    import { config } from '@fortawesome/fontawesome-svg-core';
    import { CompetencyJoinViewModel, CompetencyJoinModel, ICompetencyJoinModel } from '../../services/CompetencyService';
    import { ModuleForTaskModel, IModuleForTaskModel, CompetencyMappingJoinModel } from '../../services/Task';
    import { isNullOrUndefined } from 'util';
    import { AssessmentService } from '../../services/Assessment/AssesmentService';

    @Component({
        props: ['mode', 'id', 'fromOutside'],
        created: async function (this: SetupCourse) {
            this.isBusy = true;
            this.initData();
        },
    })
    export default class SetupCourse extends Vue {
        isBusy: boolean = false;
        mode: string;
        id: number;
        fromOutside: boolean;
        isSave: boolean = false;
        isSubmit: boolean = false;

        setupCourseMan: SetupCourseService = new SetupCourseService();
        setupModuleMan: SetupModuleService = new SetupModuleService();
        taskMan: TaskService = new TaskService();
        programTypeMan: ProgramTypeService = new ProgramTypeService();
        setupAssessment: AssessmentService = new AssessmentService();

        courses: CourseViewModel = {};
        prerequisites = [];
        removableModules = [];

        defaultAssesment = {
            name: '',
            enumTrainingType: 1,
            enumScoringMethod: "Latest",
            enumRemedialOption: "No Need",
            learningTime: 0,
            remedialLimit: 0,
            isByOption: false,
            assesmentSkillChecks: [
                {
                    skillCheckGUID: '',
                    order: ''
                }
            ]
        }
        
        public get getRemovableModules() : any {
            const selectedModules = this.courseModules.filter(v => Object.keys(v.assessment).length === 0)
                .map(v => v.module.moduleName)

            return this.removableModules.sort((a,b) => (a.moduleName > b.moduleName) ? 1 : ((b.moduleName > a.moduleName) ? -1 : 0))
                .filter(v => !selectedModules.includes(v.moduleName))
        }
        
        timepoints = [];
        modules = [];
        competencies = [];
        listCourses: any = [];
        skillChecks = []
        tasks = [];

        async initData() {
            const resSkillChecks = await this.setupAssessment.getAllSkillCheck({
                Query: "",
                SortBy: '',
                Page: 1,
                Limit: 500
            });
            this.skillChecks = resSkillChecks.data;
            this.courses = await this.setupCourseMan.getAllCourseNoSetup('');
            this.sortedArray();
            this.removableModules = await this.setupCourseMan.getAllModules('');
            this.timepoints = await this.setupModuleMan.getAllTimePoints();
            this.prerequisites = await this.setupCourseMan.getAllPrerequisites('a');
            this.tasks = await this.setupCourseMan.getAllTaskFiltered('', null, null);

            if (this.mode == 'Edit' || this.mode == 'View' || this.mode == 'view') {
                this.setupCourseModel = await this.setupCourseMan.get(this.id);
                this.courseModel.courseId = this.setupCourseModel.courseId;
                this.courseModel.courseName = this.setupCourseModel.courseName;
                this.programTypeName = this.setupCourseModel.programTypeName;
                for (var i = 0; i < 4; i++) {
                    this.courseTrainingTypeMappingModels[i].courseId = this.id;
                    this.courseTrainingTypeMappingModels[i].trainingTypeId = this.setupCourseModel.listCourseTrainingTypeMappings[i].trainingTypeId;
                    this.courseTrainingTypeMappingModels[i].minimumScore = this.setupCourseModel.listCourseTrainingTypeMappings[i].minimumScore;
                    this.courseTrainingTypeMappingModels[i].workloadRequirement = this.setupCourseModel.listCourseTrainingTypeMappings[i].workloadRequirement;
                    this.courseTrainingTypeMappingModels[i].percentage = this.setupCourseModel.listCourseTrainingTypeMappings[i].percentage;
                }
                if (this.setupCourseModel.listCoursePrerequisiteMappings != null) {
                    for (var i = 0; i < this.setupCourseModel.listCoursePrerequisiteMappings.length; i++) {
                        if (i > 0) {
                            this.coursePrerequisites.push(null);
                            this.totalPrerequisite += 1;
                        }
                        this.coursePrerequisites[i] = this.setupCourseModel.listCoursePrerequisiteMappings[i];
                    }
                }
                for (var i = 0; i < this.setupCourseModel.listCourseLearningObjectives.length; i++) {
                    if (i > 0) {
                        this.courseLearningObjectives.push('');
                        this.totalLearningObjective += 1;
                    }
                    this.courseLearningObjectives[i] = this.setupCourseModel.listCourseLearningObjectives[i].learningObjectiveName;
                }
                this.courseModel.courseName = this.setupCourseModel.courseName;
                this.programTypeName = this.setupCourseModel.programTypeName;

                const ordering = this.setupCourseModel.listSetupModules.sort((a,b) => a.order - b.order)

                for (var i = 0; i < ordering.length; i++) {
                    // if (i > 0) {
                        this.courseModules.push(new Module());
                        this.totalModule += 1;
                    // }

                    this.courseModules[i].enumRemedialOption = this.setupCourseModel.listSetupModules[i].enumRemedialOption;
                    this.courseModules[i].remedialLimit = this.setupCourseModel.listSetupModules[i].remedialLimit;
                    this.courseModules[i].enumScoringMethod = this.setupCourseModel.listSetupModules[i].enumScoringMethod;
                    this.courseModules[i].typeOfTraining = this.setupCourseModel.listSetupModules[i].trainingTypeId;
                    this.courseModules[i].minimumScore = this.setupCourseModel.listSetupModules[i].minimumScore;
                    this.courseModules[i].timepoint.timePointId = this.setupCourseModel.listSetupModules[i].timePointId;
                    for (let tp of this.timepoints) {
                        let found = this.timepoints.findIndex(Q => Q.timePointId == this.setupCourseModel.listSetupModules[i].timePointId);

                        if (found >= 0) {
                            this.courseModules[i].timepoint = this.timepoints[found];
                        }
                    }
                    this.courseModules[i].module.moduleId = this.setupCourseModel.listSetupModules[i].moduleId;
                    this.courseModules[i].module.moduleName = this.setupCourseModel.listSetupModules[i].moduleName;
                    this.courseModules[i].needQuestion = this.setupCourseModel.listSetupModules[i].setupTaskForm.setupTaskId == null ? false : true;
                    this.courseModules[i].isRemedial = this.setupCourseModel.listSetupModules[i].isForRemedial;

                    if (this.courseModules[i].needQuestion == true) {
                        this.courseModules[i].testTime = this.setupCourseModel.listSetupModules[i].setupTaskForm.testTime;
                        this.courseModules[i].isGrouping = this.setupCourseModel.listSetupModules[i].setupTaskForm.isGrouping;

                        if (this.setupCourseModel.listSetupModules[i].setupTaskForm.competencyId != null) {
                            this.courseModules[i].filterCompetency = {
                                competencyId: this.setupCourseModel.listSetupModules[i].setupTaskForm.competencyId,
                                prefixCode: this.setupCourseModel.listSetupModules[i].setupTaskForm.prefixCode,
                                competencyTypeName: this.setupCourseModel.listSetupModules[i].setupTaskForm.competencyTypeName,
                                init: null,
                                toJSON: null,
                            };
                            this.courseModules[i].competencyId = this.setupCourseModel.listSetupModules[i].setupTaskForm.competencyId;
                        }
                        else {
                            this.courseModules[i].filterCompetency = { competencyId: null, init: null, toJSON: null };
                        }
                        if (this.setupCourseModel.listSetupModules[i].setupTaskForm.moduleId != null) {
                            this.courseModules[i].filterModule = {
                                moduleId: this.setupCourseModel.listSetupModules[i].setupTaskForm.moduleId,
                                moduleName: this.setupCourseModel.listSetupModules[i].setupTaskForm.moduleName,
                                init: null,
                                toJSON: null,
                            };
                            this.courseModules[i].moduleId = this.setupCourseModel.listSetupModules[i].setupTaskForm.moduleId;
                        }
                        else {
                            this.courseModules[i].filterModule = { moduleId: null, init: null, toJSON: null };
                        }
                        this.courseModules[i].totalParticipant = this.setupCourseModel.listSetupModules[i].setupTaskForm.totalParticipant == null ? 1 : this.setupCourseModel.listSetupModules[i].setupTaskForm.totalParticipant;
                        this.courseModules[i].totalQuestion = this.setupCourseModel.listSetupModules[i].setupTaskForm.taskList.length;
                        this.courseModules[i].totalTask = this.setupCourseModel.listSetupModules[i].setupTaskForm.taskList.length;

                        // this.courseModules[i].taskCodes = await this.setupCourseMan.getAllTaskFiltered('', null, null);
                        this.courseModules[i].taskCodes = this.tasks;
                        this.setupCourseModel.listSetupModules[i].setupTaskForm.taskList.sort((a, b) => a.questionNumber - b.questionNumber)
                        for (var j = 0; j < this.setupCourseModel.listSetupModules[i].setupTaskForm.taskList.length; j++) {
                            this.courseModules[i].tasks[j] = this.setupCourseModel.listSetupModules[i].setupTaskForm.taskList[j]
                            // for (let tas of this.tasks) {
                            //     let found = this.tasks.findIndex(Q => Q.taskId == this.setupCourseModel.listSetupModules[i].setupTaskForm.taskList[j].taskId);

                            //     if (found >= 0) {
                            //         this.courseModules[i].tasks[j] = this.tasks[found];
                            //     }
                            // }
                        }
                        this.spliceTaskCodeDropdown(i);
                        this.courseModules[i].taskCodes = [];
                    }

                    // assessment
                    if(this.setupCourseModel.listSetupModules[i].assesmentId != null) {
                        this.courseModules[i].assessment = {
                            guid: this.setupCourseModel.listSetupModules[i].assesmentList[0].guid,
                            name: this.setupCourseModel.listSetupModules[i].moduleName,
                            enumTrainingType: Number(this.setupCourseModel.listSetupModules[i].assesmentList[0].enumTrainingType),
                            enumScoringMethod:this.setupCourseModel.listSetupModules[i].assesmentList[0].enumScoringMethod,
                            enumRemedialOption: this.setupCourseModel.listSetupModules[i].assesmentList[0].enumRemedialOption,
                            learningTime: this.timepoints.find(z => z.time == this.setupCourseModel.listSetupModules[i].assesmentList[0].learningTime),
                            remedialLimit: this.setupCourseModel.listSetupModules[i].assesmentList[0].remedialLimit,
                            isByOption: this.setupCourseModel.listSetupModules[i].assesmentList[0].isByOption,
                            assesmentSkillChecks: this.setupCourseModel.listSetupModules[i].assesmentList[0].skillCheck.sort((a,b) => a.order - b.order).map(v => ({
                                guid: v.guid,
                                skillCheckGUID: this.skillChecks.find(z => z.guid == v.skillCheckGUID),
                                order: v.order
                            }))
                            
                            // [
                            //     {
                            //         skillCheckGUID: ,
                            //         order: ''
                            //     }
                            // ]
                        }
                    }
                }
            }
            this.isBusy = false;
        }

        async handleSelectedTask(task, index, index2) {
            const res: any = await this.setupCourseMan.getTaskScorePoint(task.taskTypeId, task.taskId);
            this.courseModules[index].tasks[index2].score = res.score as number
            this.courseModules[index].tasks[index2].points = res.point as number

            console.log(this.courseModules[index].tasks[index2])
        }

        sortedArray(){
            this.listCourses = this.courses.listCourseModel;
            this.listCourses.sort((a, b) => a.courseName.localeCompare(b.courseName))            
        }

        async changeCourse(data) {
            var programType = await this.programTypeMan.getProgramTypeById(data.programTypeId);
            this.programTypeName = programType.programTypeName;
        }

        handleChangeIsByOption (module, e) {
            if (e.target.value) {
                module.minimumScore = 0
            }
        }

        get selectedTasksInCourse() {
            const selectedTasks = []
            this.courseModules.filter(v => Object.keys(v.assessment).length == 0).forEach(v => {
                v.tasks.forEach(detail => {
                    selectedTasks.push(detail.taskCode)
                })
            })

            return selectedTasks
        }

        get selectedSkillChecksInCourse() {
            const selectedSkillCheck = []
            this.courseModules.filter(v => Object.keys(v.assessment).length > 0).forEach(v => {
                v.assessment.assesmentSkillChecks.filter(x => x.skillCheckGUID).forEach(x => {
                    selectedSkillCheck.push(x.skillCheckGUID.guid)
                })
            })
            return selectedSkillCheck
        }


        // reset taskCodes list when moving task type from specific to grouping
        resetTaskCodeList(index) {
            this.courseModules[index].totalTask = 1;
            this.courseModules[index].totalQuestion = 1;
            this.courseModules[index].tasks = [{ taskId: null }];
            this.courseModules[index].taskCodes = [];
            this.courseModules[index].filterCompetency = new CompetencyMappingJoinModel({ competencyId: 0 });
            this.courseModules[index].filterModule = new ModuleForTaskModel({ moduleId: 0 });
        }

        // reset taskCodes dropdown when moving task type from grouping to specific
        async resetTaskCodeDropdown(index) {
            this.resetTaskCodeList(index);
        }

        // rerender total tasks, reset tasks & change taskCodes dropdown when generate
        async generateTaskCode(index) {
            this.courseModules[index].isTaskCodesAvailable = true;
            if (this.courseModules[index].filterCompetency.competencyId == 0)
                this.courseModules[index].filterCompetency.competencyId = null;
            if (this.courseModules[index].filterModule.moduleId == 0)
                this.courseModules[index].filterModule.moduleId = null;
            if (this.courseModules[index].filterCompetency.competencyId == null || this.courseModules[index].filterModule.moduleId == null) return;

            this.courseModules[index].taskCodes = await this.setupCourseMan.getAllTaskFiltered('', this.courseModules[index].filterCompetency.competencyId, this.courseModules[index].filterModule.moduleId);
            if (this.courseModules[index].taskCodes.length == 0) {
                this.courseModules[index].isTaskCodesAvailable = false;
                return;
            }

            this.courseModules[index].competencyId = this.courseModules[index].filterCompetency.competencyId;
            this.courseModules[index].moduleId = this.courseModules[index].filterModule.moduleId;

            this.courseModules[index].totalTask = this.courseModules[index].totalQuestion;
            if (this.courseModules[index].totalTask >= this.courseModules[index].taskCodes.length) {
                this.courseModules[index].totalTask = this.courseModules[index].taskCodes.length;
                this.courseModules[index].totalQuestion = this.courseModules[index].taskCodes.length;
                this.reRenderTask(index);
                for (var i = 0; i < this.courseModules[index].totalTask; i++) {
                    this.courseModules[index].tasks[i] = this.courseModules[index].taskCodes[i];
                }
            }
            else {
                this.reRenderTask(index);
                for (var i = 0; i < this.courseModules[index].totalTask; i++) {
                    this.courseModules[index].tasks[i] = this.courseModules[index].taskCodes[i];
                }
            }
            this.courseModules[index].taskCodes = [];
        }

        // splice taskCodes dropdown when adding & removing taskCode
        spliceTaskCodeDropdown(index) {
            for (let setupTaskCode of this.courseModules[index].tasks) {
                if (setupTaskCode == null) continue;
                let found = this.courseModules[index].taskCodes.findIndex(Q => Q.taskId === setupTaskCode.taskId);

                if (found >= 0) {
                    this.courseModules[index].taskCodes.splice(found, 1);
                }
            }
        }

        // splice removableModules dropdown when adding & removing module
        async spliceModulesDropdown() {
            for (let module of this.courseModules) {
                if (module == null) continue;
                let found = this.removableModules.findIndex(Q => Q.moduleId === module.module.moduleId);

                if (found >= 0) {
                    this.removableModules.splice(found, 1);
                }
            }
        }

        // splice removableModules dropdown when adding & removing prerequisites
        async splicePrerequisitesDropdown() {
            for (let pre of this.coursePrerequisites) {
                if (pre == null) continue;
                let found = this.prerequisites.findIndex(Q => Q.nextCourseId === pre.nextCourseId && Q.nextSetupModuleId === pre.nextSetupModuleId);

                if (found >= 0) {
                    this.prerequisites.splice(found, 1);
                }
            }
        }

        totalTime() {
            var totalTime = 0;
            for (let module of this.courseModules) {
                totalTime += module.timepoint.time == null ? 0 : Number.parseInt(module.timepoint.time);
                if (module.needQuestion == true) {
                    totalTime += module.testTime == null ? 0 : Number.parseInt(module.testTime.toString());
                }
            }
            return totalTime;
        }

        total(option) {
            var total = 0;
            for (let module of this.courseModules) {
                total += option == 'score' ? 0 : (module.timepoint.points == null ? 0 : module.timepoint.points);
                if (module.needQuestion == true) {
                    for (let task of module.tasks) {
                        if (!isNullOrUndefined(task))
                            total += option == 'score' ? (task.score == null ? 0 : task.score) : (task.points == null ? 0 : task.points);
                    }
                }
            }
            return total;
        }

        handleOrder(type, index) {
            const currentItem = this.courseModules[index]
            const targetIndex = type == 'up' ? index - 1 : index + 1
            const targetItem = this.courseModules[targetIndex]
            this.courseModules = this.courseModules.map((v, i) => {
                if(i == index) {
                    return targetItem
                } else if(i == targetIndex) {
                    return currentItem
                } else {
                    return v
                }
            })
        }

        // ---------------------------------------- save ------------------------------------------

        setupCourseModel: SetupCourseFormModel = {};
        courseModel: CourseModel = {
            programTypeId: null
        };
        courseTrainingTypeMappingModels: CourseTrainingTypeMappingModel[] = [
            { trainingTypeId: 0, workloadRequirement: null, minimumScore: 0, percentage: 0 },
            { trainingTypeId: 0, workloadRequirement: null, minimumScore: 0, percentage: 0 },
            { trainingTypeId: 0, workloadRequirement: null, minimumScore: 0, percentage: 0 },
            { trainingTypeId: 0, workloadRequirement: null, minimumScore: 0, percentage: 0 }
        ];
        programTypeName = '';

        isTrainingTypeEmpty() {
            var check = false;
            for (let trainingType of this.courseTrainingTypeMappingModels) {
                if (trainingType.workloadRequirement == ''
                    || trainingType.minimumScore < 0
                    || trainingType.minimumScore == null
                    || (trainingType.minimumScore != null && trainingType.minimumScore.toString() == '')
                    || trainingType.percentage == null
                    || trainingType.percentage < 0
                    || (trainingType.percentage != null && trainingType.percentage.toString() == ''))
                {
                    check = true;
                }
            }
            return check;
        }
        isCourseEmpty() {
            if (this.courseModel.courseId == 0) {
                return true;
            }
            return false;
        }

        validationCourseTrainingTypeMappingMinimumScore(index) {
            if (this.courseTrainingTypeMappingModels[index].minimumScore == null
                || this.courseTrainingTypeMappingModels[index].minimumScore < 0
                || (this.courseTrainingTypeMappingModels[index].minimumScore != null && this.courseTrainingTypeMappingModels[index].minimumScore.toString() == ''))
                return true;
            return false;
        }

        validationCourseTrainingTypeMappingWorkload(index) {
            if (this.courseTrainingTypeMappingModels[index].workloadRequirement == '')
                return true;
            return false;
        }

        validationCourseTrainingTypeMappingPercentage(index) {
            if (this.courseTrainingTypeMappingModels[index].percentage == null
                || this.courseTrainingTypeMappingModels[index].percentage < 0
                || (this.courseTrainingTypeMappingModels[index].percentage != null && this.courseTrainingTypeMappingModels[index].percentage.toString() == ''))
                return true;
            return false;
        }

        saveClickedValidateQuestion(module) {

            if (module.testTime.toString() == '' || module.testTime < 1) {
                return false;
            }

            if (module.isTaskEmpty(3)) {
                return false;
            }

            return true;
        }

        saveClickedValidateModule(module) {

            if (module.typeOfTraining == null) {
                return false;
            }

            if (module.module.moduleName == '') {
                return false;
            }

            if (module.timepoint.timePointId == 0) {
                return false;
            }

            return true;
        }

        submitClickedValidateQuestion(module) {

            if (module.testTime.toString() == '' || module.testTime < 1) {
                return false;
            }

            if ((module.totalParticipant.toString() == '' || module.totalParticipant < 1) && module.isGrouping == true) {
                return false;
            }

            if ((module.totalQuestion.toString() == '' || module.totalQuestion < 1) && module.isGrouping == true) {
                return false;
            }

            if (module.filterCompetency.competencyId == null && module.isGrouping == true) {
                return false;
            }

            if (module.filterModule.moduleId == null && module.isGrouping == true) {
                return false;
            }

            if (module.isTaskEmpty(2) == true) {
                return false;
            }

            if (module.isTaskCodesAvailable == false) {
                return false;
            }

            return true;
        }

        submitClickedValidateModule(module) {

            if (module.typeOfTraining == null) {
                return false;
            }

            if (module.minimumScore < 0 || module.minimumScore.toString() == '') {
                return false;
            }

            if (module.timepoint.timePointId == 0) {
                return false;
            }

            if (module.module.moduleName == '') {
                return false;
            }

            return true;
        }

        submitClickedValidateTrainingType() {

            if (this.courseTrainingTypeMappingModels[0].minimumScore == null
                || this.courseTrainingTypeMappingModels[0].minimumScore < 0
                || (this.courseTrainingTypeMappingModels[0].minimumScore != null && this.courseTrainingTypeMappingModels[0].minimumScore.toString() == ''))
            {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[1].minimumScore == null
                || this.courseTrainingTypeMappingModels[1].minimumScore < 0
                || (this.courseTrainingTypeMappingModels[1].minimumScore != null && this.courseTrainingTypeMappingModels[1].minimumScore.toString() == ''))
            {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[2].minimumScore == null
                || this.courseTrainingTypeMappingModels[2].minimumScore < 0
                || (this.courseTrainingTypeMappingModels[2].minimumScore != null && this.courseTrainingTypeMappingModels[2].minimumScore.toString() == ''))
            {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[3].minimumScore == null
                || this.courseTrainingTypeMappingModels[3].minimumScore < 0
                || (this.courseTrainingTypeMappingModels[3].minimumScore != null && this.courseTrainingTypeMappingModels[3].minimumScore.toString() == ''))
            {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[0].workloadRequirement == '') {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[1].workloadRequirement == '') {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[2].workloadRequirement == '') {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[3].workloadRequirement == '') {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[0].percentage == null
                || this.courseTrainingTypeMappingModels[0].percentage < 0
                || (this.courseTrainingTypeMappingModels[0].percentage != null && this.courseTrainingTypeMappingModels[0].percentage.toString() == ''))
            {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[1].percentage == null
                || this.courseTrainingTypeMappingModels[1].percentage < 0
                || (this.courseTrainingTypeMappingModels[1].percentage != null && this.courseTrainingTypeMappingModels[1].percentage.toString() == ''))
            {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[2].percentage == null
                || this.courseTrainingTypeMappingModels[2].percentage < 0
                || (this.courseTrainingTypeMappingModels[2].percentage != null && this.courseTrainingTypeMappingModels[2].percentage.toString() == ''))
            {
                return false;
            }

            if (this.courseTrainingTypeMappingModels[3].percentage == null
                || this.courseTrainingTypeMappingModels[3].percentage < 0
                || (this.courseTrainingTypeMappingModels[3].percentage != null && this.courseTrainingTypeMappingModels[3].percentage.toString() == ''))
            {
                return false;
            }

            return true;
        }

        async saveData(approvalId) {
            this.isBusy = true;
            this.isSave = false;
            this.isSubmit = false;

            if (approvalId == 3) this.isSave = true;
            else if (approvalId == 2) this.isSubmit = true;

            for (let trainingType of this.courseTrainingTypeMappingModels) {
                if (trainingType.workloadRequirement == null) {
                    trainingType.workloadRequirement = '';
                }
            }

            if (this.courseModel.courseId == null) {
                this.courseModel.courseId = 0;
            }

            if(this.courseModules.length === 0) {
                this.isBusy = false
                return;
            }

            for (var i = 0; i < this.courseModules.length; i++) {
                if (this.courseModules[i].typeOfTraining == 0)
                    this.courseModules[i].typeOfTraining = null;
                if (this.courseModules[i].timepoint.timePointId == null)
                    this.courseModules[i].timepoint.timePointId = 0;
                if (this.courseModules[i].module.moduleName == null)
                    this.courseModules[i].module.moduleName = '';
                if (this.courseModules[i].filterCompetency.competencyId == 0)
                    this.courseModules[i].filterCompetency.competencyId = null;
                if (this.courseModules[i].filterModule.moduleId == 0)
                    this.courseModules[i].filterModule.moduleId = null;
                for (var j = 0; j < this.courseModules[i].tasks.length; j++) {
                    if (this.courseModules[i].tasks[j].taskId == null)
                        this.courseModules[i].tasks[j].taskId = 0;
                }
            }

            if (this.isCourseEmpty() == true) {
                this.isBusy = false;
                return;
            }

            if (approvalId == 2) {
                if (this.isTrainingTypeEmpty() == true) {
                    this.isBusy = false;
                    return;
                }

                if (this.submitClickedValidateTrainingType() == false) {
                    this.isBusy = false;
                    return;
                }
            }

            for (var i = 0; i < this.courseModules.length; i++) {
                if(Object.keys(this.courseModules[i].assessment).length == 0) {
                    if (this.courseModules[i].isModuleError(approvalId) == true || (this.courseModules[i].needQuestion && this.courseModules[i].isTaskError(approvalId) == true) || (this.courseModules[i].isTaskEmpty(approvalId) == true && this.courseModules[i].needQuestion)) {
                        this.isBusy = false;
                        return;
                    }

                    if (approvalId == 3) {
                        if (this.saveClickedValidateModule(this.courseModules[i]) == false) {
                            this.isBusy = false;
                            return;
                        }

                        if (this.courseModules[i].needQuestion && this.saveClickedValidateQuestion(this.courseModules[i]) == false) {
                            this.isBusy = false;
                            return;
                        }
                    }
                    else if (approvalId == 2) {
                        if (this.submitClickedValidateModule(this.courseModules[i]) == false) {
                            this.isBusy = false;
                            return;
                        }

                        if (this.courseModules[i].needQuestion && this.submitClickedValidateQuestion(this.courseModules[i]) == false) {
                            this.isBusy = false;
                            return;
                        }
                    }
                } else {
                    if(this.courseModules[i].isAssessmentError() == true) {
                        this.isBusy = false
                        return;
                    }
                }
            }

            this.setupCourseModel.courseId = this.courseModel.courseId;
            this.setupCourseModel.courseName = this.courseModel.courseName;
            this.setupCourseModel.programTypeName = this.programTypeName;
            this.setupCourseModel.approvalId = approvalId;
            this.setupCourseModel.listCourseTrainingTypeMappings = [];
            for (var i = 0; i < 4; i++) {
                this.setupCourseModel.listCourseTrainingTypeMappings.push({
                    courseId: this.courseModel.courseId,
                    trainingTypeId: i + 1,
                    minimumScore: this.courseTrainingTypeMappingModels[i].minimumScore,
                    workloadRequirement: this.courseTrainingTypeMappingModels[i].workloadRequirement,
                    percentage: this.courseTrainingTypeMappingModels[i].percentage
                });
            }
            this.setupCourseModel.listCoursePrerequisiteMappings = [];
            for (var i = 0; i < this.totalPrerequisite; i++) {
                if (this.coursePrerequisites[i] == null) continue;
                this.setupCourseModel.listCoursePrerequisiteMappings.push({
                    nextCourseId: this.coursePrerequisites[i].nextCourseId,
                    nextSetupModuleId: this.coursePrerequisites[i].nextSetupModuleId
                });
            }
            this.setupCourseModel.listCourseLearningObjectives = [];
            for (var i = 0; i < this.totalLearningObjective; i++) {
                if (this.courseLearningObjectives[i] == null || this.courseLearningObjectives[i] == '') continue;
                this.setupCourseModel.listCourseLearningObjectives.push({
                    courseId: this.courseModel.courseId,
                    learningObjectiveName: this.courseLearningObjectives[i]
                });
            }
            this.setupCourseModel.listSetupModules = [];
            // for (var i = 0; i < this.courseModules.length; i++) {
            //     if(Object.keys(this.courseModules[i].assessment).length == 0) {
            //         this.setupCourseModel.listSetupModules.push({
            //             courseId: this.courseModel.courseId,
            //             moduleId: this.courseModules[i].module.moduleId == null ? 0 : this.courseModules[i].module.moduleId,
            //             moduleName: this.courseModules[i].module.moduleName,
            //             trainingTypeId: this.courseModules[i].typeOfTraining,
            //             timePointId: this.courseModules[i].timepoint.timePointId,
            //             isRecommendedForYou: false,
            //             isPopular: false,
            //             minimumScore: this.courseModules[i].minimumScore,
            //             isForRemedial: this.courseModules[i].isRemedial
            //         });

            //         if (this.courseModules[i].needQuestion == true) {
            //             this.setupCourseModel.listSetupModules[i].setupTaskForm = {
            //                 testTime: this.courseModules[i].testTime,
            //                 isGrouping: this.courseModules[i].isGrouping,
            //                 competencyId: this.courseModules[i].isGrouping == true ? this.courseModules[i].competencyId : null,
            //                 moduleId: this.courseModules[i].isGrouping == true ? this.courseModules[i].moduleId : null,
            //                 totalParticipant: this.courseModules[i].isGrouping == true ? this.courseModules[i].totalParticipant : null,
            //                 totalQuestion: this.courseModules[i].isGrouping == true ? this.courseModules[i].totalTask : null,
            //                 questionPerParticipant: this.courseModules[i].isGrouping == true ? Math.floor(this.courseModules[i].totalTask / this.courseModules[i].totalParticipant) : null
            //             }

            //             this.setupCourseModel.listSetupModules[i].setupTaskForm.taskList = [];

            //             if (this.courseModules[i].tasks.length > 0 && this.courseModules[i].tasks[0].taskId != 0 &&!isNullOrUndefined(this.courseModules[i].tasks[0].taskId))
            //             {
            //                 for (var j = 0; j < this.courseModules[i].tasks.length; j++) {
            //                     this.setupCourseModel.listSetupModules[i].setupTaskForm.taskList.push({ taskId: this.courseModules[i].tasks[j].taskId });
            //                 }
            //             }
            //         }
            //     } else {
            //         this.setupCourseModel.listAssessment.push({
            //             guid: "",
            //             name: "",
            //             enumTrainingType: this.courseModules[i].assessment.enumTrainingType,
            //             enumScoringMethod: this.courseModules[i].assessment.enumScoringMethod,
            //             enumRemedialOption: this.courseModules[i].assessment.enumRemedialOption,
            //             learningTime: Number(this.courseModules[i].assessment.learningTime.time),
            //             remedialLimit: Number(this.courseModules[i].assessment.remedialLimit),
            //             isByOption: this.courseModules[i].assessment.isByOption,
            //             assesmentSkillChecks: this.courseModules[i].assessment.assesmentSkillChecks.filter(v => v.skillCheckGUID).map(v => ({
            //                 order: Number(v.order),
            //                 skillCheckGUID: v.skillCheckGUID.guid
            //             }))
            //         });
            //     }
            // }


            // Upload sequence assessment
            for (const [index, moduled] of this.courseModules.entries()) {
                if(Object.keys(moduled.assessment).length == 0) {
                    this.setupCourseModel.listSetupModules.push({
                        courseId: this.courseModel.courseId,
                        moduleId: moduled.module.moduleId == null ? null : moduled.module.moduleId,
                        moduleName: moduled.module.moduleName,
                        trainingTypeId: moduled.typeOfTraining,
                        timePointId: moduled.timepoint.timePointId,
                        isRecommendedForYou: false,
                        isPopular: false,
                        minimumScore: moduled.minimumScore,
                        isForRemedial: moduled.isRemedial,
                        order: index + 1,
                        enumRemedialOption: moduled.enumRemedialOption,
                        remedialLimit: Number(moduled.remedialLimit),
                        enumScoringMethod: moduled.enumScoringMethod,
                        isByOption: false,
                        enumCategoryPreDuringPost: moduled.typeOfTraining == 1 ? 'Pre' : moduled.typeOfTraining == 2 ? 'During' : 'Post',
                    });

                    if (moduled.needQuestion == true) {
                        this.setupCourseModel.listSetupModules[index].setupTaskForm = {
                            testTime: moduled.testTime,
                            isGrouping: moduled.isGrouping,
                            competencyId: moduled.isGrouping == true ? moduled.competencyId : null,
                            moduleId: moduled.isGrouping == true ? moduled.moduleId : null,
                            totalParticipant: moduled.isGrouping == true ? moduled.totalParticipant : null,
                            totalQuestion: moduled.isGrouping == true ? moduled.totalTask : null,
                            questionPerParticipant: moduled.isGrouping == true ? Math.floor(moduled.totalTask / moduled.totalParticipant) : null
                        }

                        this.setupCourseModel.listSetupModules[index].setupTaskForm.taskList = [];

                        if (moduled.tasks.length > 0 && moduled.tasks[0].taskId != 0 &&!isNullOrUndefined(moduled.tasks[0].taskId))
                        {
                            for (var j = 0; j < moduled.tasks.length; j++) {
                                this.setupCourseModel.listSetupModules[index].setupTaskForm.taskList.push({ taskId: moduled.tasks[j].taskId });
                            }
                        }
                    }
                } else {
                    if(this.mode == 'Edit' && moduled.assessment.guid && moduled.assessment.guid != null) {
                        await this.setupCourseMan.updateAssessment({
                            guid: moduled.assessment.guid ? moduled.assessment.guid : null,
                            name: moduled.assessment.name,
                            enumTrainingType: moduled.assessment.enumTrainingType,
                            enumScoringMethod: moduled.assessment.enumScoringMethod,
                            enumRemedialOption: moduled.assessment.enumRemedialOption,
                            learningTime: Number(moduled.assessment.learningTime.time),
                            remedialLimit: Number(moduled.assessment.remedialLimit),
                            isByOption: moduled.assessment.isByOption,
                            assesmentSkillChecks: moduled.assessment.assesmentSkillChecks.filter(v => v.skillCheckGUID).map(v => ({
                                guid: v.guid ? v.guid : null,
                                order: Number(v.order),
                                skillCheckGUID: v.skillCheckGUID.guid
                            }))
                        })
                        this.setupCourseModel.listSetupModules.push({
                            courseId: this.courseModel.courseId,
                            moduleId: moduled.module.moduleId == null ? null : moduled.module.moduleId,
                            moduleName: '',
                            trainingTypeId: moduled.assessment.enumTrainingType,
                            timePointId: moduled.assessment.learningTime.timePointId,
                            isRecommendedForYou: false,
                            isPopular: false,
                            minimumScore: moduled.minimumScore,
                            isForRemedial: moduled.isRemedial,
                            assesmentId: moduled.assessment.guid,
                            order: index + 1,
                            enumRemedialOption: moduled.assessment.enumRemedialOption,
                            remedialLimit: Number(moduled.assessment.remedialLimit),
                            enumScoringMethod: moduled.assessment.enumScoringMethod,
                            isByOption: moduled.assessment.isByOption,
                            enumCategoryPreDuringPost: moduled.assessment.enumTrainingType == 1 ? 'Pre' : moduled.assessment.enumTrainingType == 2 ? 'During' : 'Post',
                        });
                    } else {
                        const res = await this.setupCourseMan.createAssessment({
                            name: moduled.assessment.name,
                            enumTrainingType: moduled.assessment.enumTrainingType,
                            enumScoringMethod: moduled.assessment.enumScoringMethod,
                            enumRemedialOption: moduled.assessment.enumRemedialOption,
                            learningTime: Number(moduled.assessment.learningTime.time),
                            remedialLimit: Number(moduled.assessment.remedialLimit),
                            isByOption: moduled.assessment.isByOption,
                            assesmentSkillChecks: moduled.assessment.assesmentSkillChecks.filter(v => v.skillCheckGUID).map(v => ({
                                order: Number(v.order),
                                skillCheckGUID: v.skillCheckGUID.guid
                            }))
                        })
                        this.setupCourseModel.listSetupModules.push({
                            courseId: this.courseModel.courseId,
                            moduleId: moduled.module.moduleId == null ? null : moduled.module.moduleId,
                            moduleName: '',
                            trainingTypeId: moduled.assessment.enumTrainingType,
                            timePointId: moduled.assessment.learningTime.timePointId,
                            isRecommendedForYou: false,
                            isPopular: false,
                            minimumScore: moduled.minimumScore,
                            isForRemedial: moduled.isRemedial,
                            assesmentId: res.data.guid,
                            order: index + 1,
                            enumRemedialOption: moduled.assessment.enumRemedialOption,
                            remedialLimit: Number(moduled.assessment.remedialLimit),
                            enumScoringMethod: moduled.assessment.enumScoringMethod,
                            isByOption: moduled.assessment.isByOption,
                            enumCategoryPreDuringPost: moduled.assessment.enumTrainingType == 1 ? 'Pre' : moduled.assessment.enumTrainingType == 2 ? 'During' : 'Post',
                        });
                    }
                }
            }

            console.log(this.setupCourseModel)

            await this.setupCourseMan.create(this.setupCourseModel);
            this.closeClicked();
            this.isBusy = false
        }

        closeClicked() {
            window.location.href = '/Setup/SetupLearning';
        }

        coursePrerequisites: CoursePrerequisiteViewModel[] = [null];
        courseLearningObjectives: string[] = [null];
        courseModules: Module[] = [];

        totalPrerequisite: number = 1;
        totalLearningObjective: number = 1;
        totalModule: number = 0;

        AddPrerequisite() {
            this.coursePrerequisites.push(null);
            this.totalPrerequisite++;
        }

        DeletePrerequisite(indexToDelete: number) {
            this.coursePrerequisites.splice(indexToDelete, 1);
            this.totalPrerequisite--;
        }

        AddLearningObjective() {
            this.courseLearningObjectives.push(null);
            this.totalLearningObjective++;
        }

        DeleteLearningObjective(indexToDelete: number) {
            this.courseLearningObjectives.splice(indexToDelete, 1);
            this.totalLearningObjective--;
        }

        async AddModule(type) {
            if(type == 'assessment') {
                const newAssessment = new Module();
                newAssessment.assessment = {...this.defaultAssesment}
                newAssessment.assessment.assesmentSkillChecks = [
                    {
                        skillCheckGUID: '',
                        order: ''
                    }
                ]
                this.courseModules.push(newAssessment);
            } else {
                this.courseModules.push(new Module());
                this.totalModule++;
                this.courseModules[this.totalModule - 1].taskCodes = await this.setupCourseMan.getAllTaskFiltered(null, null, null);
            }
        }

        handleAddSkillCheck(module) {
            module.assessment
                .assesmentSkillChecks.push(
                    {
                        skillCheckGUID: '',
                        order: 0
                    })
        }

        handleRemoveSkillCheck(module, indexToDelete) {
            module.assessment.assesmentSkillChecks.splice(indexToDelete, 1);
        }

        RemoveModule(indexToDelete: number) {
            this.courseModules.splice(indexToDelete, 1);
            this.totalModule--;
        }

        AddTaskCode(indexModule: number) {
            //console.log(indexModule);
            this.courseModules[indexModule].tasks.push({ taskId: null });
            this.courseModules[indexModule].totalTask++;
            this.courseModules[indexModule].totalQuestion++;
        }

        DeleteTaskCode(indexModule: number, indexToDelete: number) {
            //console.log(indexModule, indexToDelete);
            this.courseModules[indexModule].tasks.splice(indexToDelete, 1);
            this.courseModules[indexModule].totalTask--;
            this.courseModules[indexModule].totalQuestion--;

            this.spliceTaskCodeDropdown(indexModule);
        }

        ChangeTypeofTraining(indexToChange: number, buttonSelected: number) {
            //console.log("running", buttonSelected);
            this.courseModules[indexToChange].typeOfTraining = buttonSelected;
        }

        ChangeTypeofTrainingAssessment(module, selected) {
            module.assessment.enumTrainingType = selected
        }

        reRenderTask(index: number) {
            if (this.courseModules[index].totalTask < 1 || isNaN(this.courseModules[index].totalTask) == true) {
                return;
            }

            if (this.courseModules[index].tasks.length > this.courseModules[index].totalTask) {
                //console.log("lebih kecil");
                var toPop = this.courseModules[index].tasks.length - this.courseModules[index].totalTask;
                for (var i = 0; i < toPop; i++) {
                    this.courseModules[index].tasks.pop();
                }
            }
            else if (this.courseModules[index].tasks.length < this.courseModules[index].totalTask) {
                //console.log("lebih besar");
                var totalToAdd = this.courseModules[index].totalTask - this.courseModules[index].tasks.length;
                for (var i = 0; i < totalToAdd; i++) {
                    this.courseModules[index].tasks.push({ taskId: null });
                }
            }
            this.$forceUpdate();
        }

        // ---------------------------------------- Multiselect Naming ------------------------------------------

        NamePrerequisite({ name }) {
            if (name == null) {
                return null;
            }

            return name;
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

        NameTaskCode({ taskCode }): string {
            if (taskCode == null) {
                return null;
            }

            return taskCode;
        }

        isPrerequisitesLoading = false;
        async initPrerequisitesDropdown(query) {
            if (query == null || query === '') {
                this.prerequisites = [];
                return;
            }

            this.isPrerequisitesLoading = true;
            this.prerequisites = await this.setupCourseMan.getAllPrerequisites(query);
            this.splicePrerequisitesDropdown();
            this.isPrerequisitesLoading = false;
        }

        isRemovableModulesLoading = false;
        async initRemovableModulesDropdown(query) {
            if (query == null || query === '') {
                this.removableModules = [];
                return;
            }

            this.isRemovableModulesLoading = true;
            this.removableModules = await this.setupCourseMan.getAllModules(query);
            // console.log(this.removableModules);
            this.spliceModulesDropdown();
            this.isRemovableModulesLoading = false;
        }

        isCompetenciesLoading = false;
        async initCompetenciesDropdown(query) {
            if (query == null || query === '') {
                this.competencies = [];
                return;
            }

            this.isCompetenciesLoading = true;
            this.competencies = await this.setupCourseMan.getAllTaskCompetencies(query);
            this.isCompetenciesLoading = false;
        }

        isModulesLoading = false;
        async initModulesDropdown(query) {
            if (query == null || query === '') {
                this.modules = [];
                return;
            }

            this.isModulesLoading = true;
            this.modules = await this.setupCourseMan.getAllTaskModules(query);
            this.isModulesLoading = false;
        }

        async initTaskCodesDropdown(query, index) {
            if (query == null || query === '') {
                this.courseModules[index].taskCodes = [];
                return;
            }

            this.courseModules[index].isTaskCodesLoading = true;
            var competency = this.courseModules[index].filterCompetency.competencyId == 0 ? null : this.courseModules[index].filterCompetency.competencyId;
            var module = this.courseModules[index].filterModule.moduleId == 0 ? null : this.courseModules[index].filterModule.moduleId;
            this.courseModules[index].competencyId = competency;
            this.courseModules[index].moduleId = module;
            this.courseModules[index].taskCodes = await this.setupCourseMan.getAllTaskFiltered(query, competency, module);
            this.spliceTaskCodeDropdown(index);
            this.courseModules[index].isTaskCodesLoading = false;
        }

        goBackPage() {
            window.history.back();
        }
    }

    class Module {
        typeOfTraining: number = 0;
        module = { moduleId: null, moduleName: null };
        minimumScore: number = 0;
        timepoint = { timePointId: null, score: null, time: null, points: null };
        needQuestion: boolean = false;
        isRemedial: boolean = false;

        testTime = 1;
        isGrouping: boolean = false;
        filterCompetency = new CompetencyMappingJoinModel({ competencyId: 0 });
        competencyId = null;
        filterModule = new ModuleForTaskModel({ moduleId: 0 });
        moduleId = null;
        totalParticipant: number = 1;
        totalQuestion: number = 1;

        tasks: SetupTaskCodesFormModel[] = [{ taskId: null }];
        totalTask: number = 1;

        taskCodes = [];

        enumRemedialOption = 'No Need';
        remedialLimit: any = '';
        enumScoringMethod = 'Latest';

        assessment: any = {};

        isModuleError(approvalId) {
            var check = false;

            if (approvalId == 2) {
                if (this.typeOfTraining == null)
                    check = true;
                if (this.minimumScore < 0 || this.minimumScore.toString() == '')
                    check = true;
                if (this.timepoint.timePointId == 0)
                    check = true;
                if (this.module.moduleName == '')
                    check = true;
            } else if (approvalId == 3) {
                if (this.typeOfTraining == null)
                    check = true;
                if (this.module.moduleName == '')
                    check = true;
                if (this.timepoint.timePointId == 0)
                    check = true;
            }
            if(this.enumRemedialOption === 'Limit' && Number(this.remedialLimit) <= 1)
                check = true; 
            
            return check;
        }

        isTaskError(approvalId) {
            var check = false;

            if (approvalId == 2) {
                if (this.testTime.toString() == '' || this.testTime < 1)
                    check = true;
                if (this.totalParticipant != null && (this.totalParticipant.toString() == '' || this.totalParticipant < 1) && this.isGrouping == true)
                    check = true;
                if (this.totalQuestion != null && (this.totalQuestion.toString() == '' || this.totalQuestion < 1) && this.isGrouping == true)
                    check = true;
                if (this.filterCompetency.competencyId == null && this.isGrouping == true)
                    check = true;
                if (this.filterModule.moduleId == null && this.isGrouping == true)
                    check = true;
            }
            else if (approvalId == 3) {
                if (this.testTime.toString() == '' || this.testTime < 1)
                    check = true;
            }

            return check;
        }

        isTaskEmpty(approvalId) {

            if (approvalId == 2) {
                for (var i = 0; i < this.tasks.length; i++) {
                    if (this.tasks[i] != null && this.tasks[i].taskId == 0)
                        return true;
                    else if (this.tasks[i] == null) return true;
                }
            }
            else if (approvalId == 3) {
                if (this.tasks.length > 1 || (!isNullOrUndefined(this.tasks[0].taskId) && this.tasks[0].taskId != 0)) {
                    for (var i = 0; i < this.tasks.length; i++) {
                        if (this.tasks[i] != null && this.tasks[i].taskId == 0)
                            return true;

                        else if (this.tasks[i].taskId == null) return true;
                    }
                }
            }

            return false;
        }

        isAssessmentError() {
            var check = false;

            if (this.assessment.assesmentSkillChecks.filter(v => v.order && v.skillCheckGUID).length == 0)
                check = true
            if (this.assessment.learningTime == 0) 
                check = true
            if (this.assessment.name == '') 
                check = true
            if(this.assessment.enumRemedialOption === 'Limit' && Number(this.assessment.remedialLimit) <= 1)
                check = true; 

            return check;
        }

        isTaskCodesLoading = false;
        isTaskCodesAvailable = true;
    }

</script>