<template>
    <div>

        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="alert alert-danger pb-0" v-show="errors.items.length > 0 || errorMessage.length > 0">
            <ul>
                <li v-if="errorMessage.length > 0">{{errorMessage}}</li>
                <li v-for="error in errors.all()">{{error}}</li>
            </ul>
        </div>

        <div v-if="successMessage" class="alert alert-success alert-dismissible fade show" role="alert">
            {{successMessage}}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <div class="row" v-if="add === false && edit === false && view === false">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="folder"></fa> Setup > <span class="talent_font_red">Release Training</span></h3>

                <br />

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Training</h4>

                    <br />

                    <!--3 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" :masks="{ input: 'DD/MM/YYYY' }" v-model="filter.dateFilter" mode="range" :firstDayOfWeek="2" :value="null" :input-props="props"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Training Start Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" :masks="{ input: 'DD/MM/YYYY' }" v-model="filter.enrollmentStartDate" mode="single" :firstDayOfWeek="2" :value="null" :input-props="props"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Training End Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" mode="single" v-model="filter.enrollmentEndDate" :firstDayOfWeek="2" :value="null" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <!--3 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Course Name</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.courseName" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Batch</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.batch" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Approval Status</span>
                                </div>
                                <div class="col-md-9">
                                    <select class="form-control" v-model="filter.approvalStatusId">
                                        <option v-for="status in listApprovalStatus" :value="status.approvalId">{{status.approvalName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--Search Button-->
                    <div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="d-flex align-items-end flex-column">
                                    <div>
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="fetch">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="resetFilter">
                                            Reset
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <br />

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Training List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{listReleaseTraining.listTraining.length}} of {{listReleaseTraining.totalData}} Entry(s)</a>
                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" @click="addClicked">Release Training</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCourseName()">Course Name <fa icon="sort" v-if="courseName.sort == true"></fa><fa icon="sort-up" v-if="courseName.sortup == true"></fa><fa icon="sort-down" v-if="courseName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortBatch()">Batch <fa icon="sort" v-if="batch.sort == true"></fa><fa icon="sort-up" v-if="batch.sortup == true"></fa><fa icon="sort-down" v-if="batch.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortStartDate()">Training Start Date <fa icon="sort" v-if="startDate.sort == true"></fa><fa icon="sort-up" v-if="startDate.sortup == true"></fa><fa icon="sort-down" v-if="startDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortEndDate()">Training End Date <fa icon="sort" v-if="endDate.sort == true"></fa><fa icon="sort-up" v-if="endDate.sortup == true"></fa><fa icon="sort-down" v-if="endDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortApprovalStatus()">Approval Status <fa icon="sort" v-if="approvalStatus.sort == true"></fa><fa icon="sort-up" v-if="approvalStatus.sortup == true"></fa><fa icon="sort-down" v-if="approvalStatus.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="3" v-if="crud.read || crud.update || crud.delete">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(training, index) in listReleaseTraining.listTraining">
                                    <td>
                                        {{training.courseName}}
                                    </td>
                                    <td>
                                        {{training.batch}}
                                    </td>
                                    <td>
                                        {{getStringDate(training.startDate)}}
                                    </td>
                                    <td>
                                        {{getStringDate(training.endDate)}}
                                    </td>
                                    <td>
                                        {{training.approvalStatus}}
                                    </td>
                                    <td>
                                        {{training.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{training.updatedAt | dateFormat}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click="detailClicked(index)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" :disabled="training.approvalStatus === waitingForApproval" @click="editClicked(index)">Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" :disabled="training.approvalStatus === waitingForApproval" @click.prevent="removeClicked(index)" data-toggle="modal" data-target="#exampleModalCenter" data-backdrop="static">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <!--Pagination-->
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate :currentPage.sync="currentPage" :total-data="listReleaseTraining.totalData" :limit-data=pageSize @update:currentPage="fetch()"></paginate>
                    </div>
                </div>

                <br />


            </div>
        </div>

        <!--Add-->
        <div class="col-md-12" v-if="add === true && view === false && edit === false">
            <h3 class="mb-md-3"><fa icon="folder"></fa> Setup > Release Training > <span class="talent_font_red">Add Release Training</span></h3>
            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <h4>Release Training</h4>
                    <h4>Course</h4>
                    <div class="row">
                        <div class="col my-2">
                            <label>Course Name<span class="talent_font_red">*</span></label>
                            <multiselect v-model="formModel.course"
                                         id="ajax"
                                         track-by="courseName"
                                         name="Course"
                                         placeholder="Select Course Name"
                                         label="courseName"
                                         :options="getListCourse"
                                         :searchable="true"
                                         :close-on-select="true"
                                         :clear-on-select="true"
                                         :loading="isLoading"
                                         :hide-selected="true"
                                         :showNoResults="true"
                                         @search-change="AutoCompleteCourse"
                                         @input="findSetup"
                                         @select="reset">

                                <span slot="noResult"><i>Not Found!</i></span>
                            </multiselect>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col my-2">
                            <label>Program Type</label>
                            <div class="input-group">
                                <input class="form-control" :placeholder="formModel.course ? formModel.course.programTypeName : ''" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>Batch</label>
                            <div class="input-group">
                                <input class="form-control" v-model="formModel.batch" disabled />
                            </div>
                        </div>
                        <div class="col-md-10 my-2">
                            <label>Training Date</label>
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" name="StartDate" mode="single" v-model="formModel.startDate" :firstDayOfWeek="2" :value="null" :max-date="formModel.endDate" :input-props='{placeholder: "Start Date"}' :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <button class="button_text_no_background" disabled>
                                        <fa style="font-size:30px" icon="arrow-right"></fa>
                                    </button>
                                </div>
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" name="EndDate" mode="single" v-model="formModel.endDate" :firstDayOfWeek="2" :min-date="formModel.startDate" :value="null" :input-props='{placeholder: "End Date"}' :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>Quota</label>
                            <div class="input-group">
                                <input class="form-control" type="number" min="0" name="Quota" v-model="formModel.quota" v-validate="formModel.course === null || formModel.course.learningTypeId !== 2 ? '' : 'required' " :disabled="formModel.course === null || formModel.course.learningTypeId === 1" />
                            </div>
                        </div>
                        <div class="col-md-8 my-2">
                            <label>Training Location</label>
                            <div class="input-group">
                                <input maxlength="255" class="form-control" name="Location" v-model="formModel.location" v-validate="formModel.course === null || formModel.course.learningTypeId !== 2 ? '' : 'required' " :disabled="formModel.course === null || formModel.course.learningTypeId === 1" />
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Accomodation<span class="talent_font_red">*</span></label>
                            <div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="radio" name="Accomodation" id="accomodationYes" :value="true" v-model="formModel.isAccommodate" v-validate="'required'">
                                    <label class="form-check-label" for="accomodationYes">Yes</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="radio" name="Accomodation" id="accomodationNo" :value="false" v-model="formModel.isAccommodate">
                                    <label class="form-check-label" for="accomodationNo">No</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-between" v-for="(module,index) in formModel.listSetupModule">
                        <div class="col-md-1 my-2">
                            <label>Phase</label>
                            <div class="input-group">
                                <input class="form-control" v-model="module.trainingTypesName" disabled />
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Module Name</label>
                            <div class="input-group">
                                <input class="form-control" v-model="module.moduleName" disabled />
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Training Date</label>
                            <div v-if="formModel.course === null || formModel.course.learningTypeId === 1" class="input-group">
                                <input class="form-control" type="text" disabled :placeholder="module.trainingStart" />
                                <div class="input-group-append">
                                    <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                </div>
                            </div>

                            <div v-else class="input-group">
                                <v-date-picker class="v-date-style-report" :max-date="formModel.endDate" :min-date="formModel.startDate" mode="single" v-model="module.trainingStart" :firstDayOfWeek="2" :value="null" name="TrainingStart" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>

                                <div class="input-group-append">
                                    <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Time</label>
                            <div class="row justify-content-between">
                                <div class="px-1">
                                    <vue-timepicker input-width="5em" v-model="startDateForm[index]" format="HH:mm" :disabled="formModel.course === null || formModel.course.learningTypeId === 1">
                                    </vue-timepicker>
                                </div>
                                <div class="px-1">
                                    <button class="button_text_no_background" disabled>
                                        <fa style="font-size:1rem" icon="arrow-right"></fa>
                                    </button>
                                </div>
                                <div class="px-1">
                                    <vue-timepicker input-width="5em" v-model="endDateForm[index]" format="HH:mm" :disabled="formModel.course === null || formModel.course.learningTypeId === 1">
                                    </vue-timepicker>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Coach Name</label>
                            <multiselect v-model="module.coach"
                                         id="ajax"
                                         track-by="coachId"
                                         placeholder="Select Coach Name"
                                         label="employeeName"
                                         :disabled="formModel.course === null || formModel.course.learningTypeId === 1"
                                         :options="listCoach"
                                         :searchable="true"
                                         :close-on-select="true"
                                         :clear-on-select="true"
                                         :loading="isLoading"
                                         name="Coach"
                                         :hide-selected="true"
                                         :showNoResults="true"
                                         @search-change="AutoCompleteCoach"
                                         @open="resetCoach">

                                <span slot="noResult"><i>Not Found!</i></span>
                            </multiselect>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Teaching Time (Minutes)</label>
                            <div class="input-group">
                                <select class="form-control" v-model="module.teachingTimePoint" :disabled="formModel.course === null || formModel.course.learningTypeId === 1">
                                    <option v-for="teachingPoint in listTeachingPoints" name="TeachingPoint" :value="teachingPoint">{{teachingPoint.time}}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-1 my-2">
                            <label>Points</label>
                            <div class="input-group">
                                <input class="form-control" :placeholder="module.teachingTimePoint ? module.teachingTimePoint.points : '' " disabled />
                            </div>
                        </div>
                    </div>
                    <hr />
                </div>

            </div>

            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <h4>Participant</h4>
                    <div class="row">
                        <div class="col my-2">
                            <label>Position Status<span class="talent_font_red">*</span></label>
                            <div>
                                <div>
                                    <input type="checkbox" id="permanentStatus" name="PositionStatus" :value="true" v-model="formModel.isParticipantPermanent">
                                    <label for="permanentStatus">Permanent (Permanent/TONES)</label>
                                    <input type="checkbox" id="traineeStatus" name="PositionStatus" :value="false" v-model="formModel.isParticipantTrainee">
                                    <label for="traineeStatus">Trainee (verified guest account)</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col my-2">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Area</label>
                                    <div class="resized">
                                    <multiselect v-model="selectedArea"
                                                 tag-placeholder="Add Area"
                                                 placeholder="Add Area"
                                                 label="name"
                                                 track-by="areaId"
                                                 :options="listAreaGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="areaChanged"
                                                 @remove="areaRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    </div>
                                    <br />
                                    <label>Dealer</label>
                                    <div class="resized">
                                    <multiselect v-model="selectedDealer"
                                                 tag-placeholder="Add Dealer"
                                                 placeholder="Add Dealer"
                                                 label="dealerName"
                                                 track-by="dealerId"
                                                 :options="listDealerGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="dealerChanged"
                                                 @remove="dealerRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    </div>
                                    <br />
                                    <label>Province</label>
                                    <div class="resized">
                                    <multiselect v-model="selectedProvince"
                                                 tag-placeholder="Add Province"
                                                 placeholder="Add Province"
                                                 label="provinceName"
                                                 track-by="provinceId"
                                                 :options="listProvinceGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="provinceChanged"
                                                 @remove="provinceRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    </div>
                                    <br />
                                    <label>City</label>
                                    <div class="resized">
                                    <multiselect v-model="selectedCity"
                                                 tag-placeholder="Add City"
                                                 placeholder="Add City"
                                                 label="cityName"
                                                 track-by="cityId"
                                                 :options="listCityGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="cityChanged"
                                                 @remove="cityRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    </div>
                                    <br />
                                    <label>Outlet<span class="talent_font_red">*</span></label>
                                    <div class="resized">
                                    <multiselect v-model="selectedOutletCompleted"
                                                 class="oneline-scrollable-multiselect"
                                                 tag-placeholder="Add Outlet"
                                                 placeholder="Add Outlet"
                                                 label="name"
                                                 name="Outlet"
                                                 track-by="outletId"
                                                 :options="listOutletCompletedGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :show-no-results="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Position<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                            <multiselect v-model="formModel.listPosition"
                                                        tag-placeholder="Add Position"
                                                        placeholder="Add Position"
                                                        label="positionName"
                                                        track-by="positionId"
                                                        name="Position"
                                                        :options="listPositionGroup"
                                                        group-values="ListOption"
                                                        group-label="GroupName"
                                                        :group-select="true"
                                                        :clear-on-select="true"
                                                        :multiple="true"
                                                        class="oneline-scrollable-multiselect"
                                                        :show-no-results="true"
                                                        selectLabel="Press enter to select">
                                            </multiselect>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4">
                                            <label>View only to<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                            <multiselect v-model="formModel.listPositionOnlyView"
                                                        tag-placeholder="Add Position"
                                                        placeholder="Add Position"
                                                        label="positionName"
                                                        track-by="positionId"
                                                        name="Position"
                                                        :options="listPositionGroup"
                                                        group-values="ListOption"
                                                        group-label="GroupName"
                                                        :group-select="true"
                                                        :clear-on-select="true"
                                                        :multiple="true"
                                                        class="oneline-scrollable-multiselect"
                                                        :show-no-results="true"
                                                        selectLabel="Press enter to select">
                                            </multiselect>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4">
                                            <label>Certification<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input id="withcertificate" class="mr-1" type="radio" name="isCertificate" :value="true" v-model="formModel.isCertificate" :checked="formModel.isCertificate">
                                                <label class="mr-4" for="withcertificate">With Certificate</label>
                                                <input id="nocertificate" class="mr-1" type="radio" name="isCertificate" :value="false" v-model="formModel.isCertificate" :checked="formModel.isCertificate">
                                                <label class="mr-4" for="nocertificate">No Certificate</label>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate">
                                            <label>Certificate Header<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input id="default" class="mr-1" type="radio" name="certificateHeader" :checked="formModel.enumCertificate == 'Default'" @change="handleChangeCertificateHeader('Default')">
                                                <label class="mr-4" for="default">Default</label>
                                                <input id="customname" class="mr-1" type="radio" name="certificateHeader" :checked="formModel.enumCertificate != 'Default'" @change="handleChangeCertificateHeader('')">
                                                <label class="mr-4" for="customname">Custom Name</label>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate && formModel.enumCertificate != 'Default'">
                                            <label>Certificate Name<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input class="form-control" type="text" v-model="formModel.enumCertificate">
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate">
                                            <label>Certification Trigger<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input class="mr-1" type="radio" name="certificateTrigger" id="personal" value="Personal" v-model="formModel.enumCertificationTtrigger" @change="handleChangeCertificationTtrigger('personal')">
                                                <label class="mr-4" for="personal">Personal</label>
                                                <input class="mr-1" type="radio" name="certificateTrigger" id="hirarki" value="Hirarki" v-model="formModel.enumCertificationTtrigger" @change="handleChangeCertificationTtrigger('hirarki')">
                                                <label class="mr-4" for="hirarki">Hierarchy</label>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate && formModel.enumCertificationTtrigger == 'Hirarki'">
                                            <label>Course Name<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <multiselect
                                                            ref="addNew"
                                                            id="addNew"
                                                            track-by="trainingId"
                                                            name="Course"
                                                            placeholder="Select Course Name"
                                                            label="courseName"
                                                            v-model="formModel.listCertifications[0].courseId"
                                                            :options="trainings"
                                                            :searchable="true"
                                                            :close-on-select="true"
                                                            :clear-on-select="true"
                                                            :loading="isLoading"
                                                            :hide-selected="true"
                                                            :showNoResults="true"
                                                            @search-change="AutoCompleteCourseTraining">

                                                    <span slot="noResult"><i>Not Found!</i></span>
                                                </multiselect>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>TOTAL MODULE<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalModule" />
                            </div>
                        </div>
                        <div class="col my-2">
                            <label>TOTAL SCORE<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalScore" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>TOTAL TIME<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalTime" />
                            </div>
                        </div>
                        <div class="col my-2">
                            <label>TOTAL POINTS<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalPoints" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="d-flex align-items-end flex-column">
                            <div>
                                <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                    Close
                                </button>
                                <button class="btn talent_bg_lightgreen talent_font_white" @click="insertTraining(1)">
                                    Save
                                </button>
                                <button class="btn talent_bg_blue talent_font_white" @click="insertTraining(2)">
                                    Submit
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <!--Edit-->
        <div class="col-md-12" v-if="add === false && view === false && edit === true">
            <h3 class="mb-md-3"><fa icon="folder"></fa> Setup > Release Training > <span class="talent_font_red">Edit Release Training</span></h3>
            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <h4>Release Training</h4>
                    <h4>Course</h4>
                    <div class="row">
                        <div class="col my-2">
                            <label>Course Name<span class="talent_font_red">*</span></label>
                            <multiselect v-model="formModel.course"
                                         id="ajax"
                                         track-by="courseName"
                                         name="Course"
                                         placeholder="Select Course Name"
                                         label="courseName"
                                         :options="getListCourse"
                                         :searchable="true"
                                         :close-on-select="true"
                                         :clear-on-select="true"
                                         :loading="isLoading"
                                         :hide-selected="true"
                                         :showNoResults="true"
                                         @search-change="AutoCompleteCourse"
                                         @input="findSetup"
                                         @select="reset">

                                <span slot="noResult"><i>Not Found!</i></span>
                            </multiselect>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col my-2">
                            <label>Program Type</label>
                            <div class="input-group">
                                <input class="form-control" :placeholder="formModel.course ? formModel.course.programTypeName : ''" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>Batch</label>
                            <div class="input-group">
                                <input class="form-control" v-model="formModel.batch" disabled />
                            </div>
                        </div>
                        <div class="col-md-10 my-2">
                            <label>Training Date</label>
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" name="StartDate" mode="single" v-model="formModel.startDate" :firstDayOfWeek="2" :value="null" :max-date="formModel.endDate" :input-props='{placeholder: "Start Date"}' :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <button class="button_text_no_background" disabled>
                                        <fa style="font-size:30px" icon="arrow-right"></fa>
                                    </button>
                                </div>
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" name="EndDate" mode="single" v-model="formModel.endDate" :firstDayOfWeek="2" :min-date="formModel.startDate" :value="null" :input-props='{placeholder: "End Date"}' :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>Quota</label>
                            <div class="input-group">
                                <input class="form-control" type="number" min="0" name="Quota" v-model="formModel.quota" v-validate="formModel.course === null || formModel.course.learningTypeId !== 2 ? '' : 'required' " :disabled="formModel.course === null || formModel.course.learningTypeId === 1" />
                            </div>
                        </div>
                        <div class="col-md-8 my-2">
                            <label>Training Location</label>
                            <div class="input-group">
                                <input maxlength="255" class="form-control" name="Location" v-model="formModel.location" v-validate="formModel.course === null || formModel.course.learningTypeId !== 2 ? '' : 'required' " :disabled="formModel.course === null || formModel.course.learningTypeId === 1" />
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Accomodation<span class="talent_font_red">*</span></label>
                            <div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="radio" name="Accomodation" id="accomodationYes" :value="true" v-model="formModel.isAccommodate" v-validate="'required'">
                                    <label class="form-check-label" for="accomodationYes">Yes</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="radio" name="Accomodation" id="accomodationNo" :value="false" v-model="formModel.isAccommodate">
                                    <label class="form-check-label" for="accomodationNo">No</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-between" v-for="(module,index) in formModel.listSetupModule">
                        <div class="col-md-1 my-2">
                            <label>Phase</label>
                            <div class="input-group">
                                <input class="form-control" v-model="module.trainingTypesName" disabled />
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Module Name</label>
                            <div class="input-group">
                                <input class="form-control" v-model="module.moduleName" disabled />
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Training Date</label>
                            <div v-if="formModel.course === null || formModel.course.learningTypeId === 1" class="input-group">
                                <input class="form-control" type="text" disabled :placeholder="module.trainingStart" />
                                <div class="input-group-append">
                                    <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                </div>
                            </div>

                            <div v-else class="input-group">
                                <v-date-picker class="v-date-style-report" :max-date="formModel.endDate" :min-date="formModel.startDate" mode="single" v-model="module.trainingStart" :firstDayOfWeek="2" :value="null" name="TrainingStart" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>

                                <div class="input-group-append">
                                    <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Time</label>
                            <div class="row justify-content-between">
                                <div class="px-1">
                                    <vue-timepicker input-width="5em" v-model="startDateForm[index]" format="HH:mm" :disabled="formModel.course === null || formModel.course.learningTypeId === 1">
                                    </vue-timepicker>
                                </div>
                                <div class="px-1">
                                    <button class="button_text_no_background" disabled>
                                        <fa style="font-size:1rem" icon="arrow-right"></fa>
                                    </button>
                                </div>
                                <div class="px-1">
                                    <vue-timepicker input-width="5em" v-model="endDateForm[index]" format="HH:mm" :disabled="formModel.course === null || formModel.course.learningTypeId === 1">
                                    </vue-timepicker>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Coach Name</label>
                            <multiselect v-model="module.coach"
                                         id="ajax"
                                         track-by="coachId"
                                         placeholder="Select Coach Name"
                                         label="employeeName"
                                         :disabled="formModel.course === null || formModel.course.learningTypeId === 1"
                                         :options="listCoach"
                                         :searchable="true"
                                         :close-on-select="true"
                                         :clear-on-select="true"
                                         :loading="isLoading"
                                         name="Coach"
                                         class="oneline-scrollable-multiselect"
                                         :showNoResults="true"
                                         @search-change="AutoCompleteCoach"
                                         @open="resetCoach">

                                <span slot="noResult"><i>Not Found!</i></span>
                            </multiselect>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Teaching Time (Minutes)</label>
                            <div class="input-group">
                                <select class="form-control" v-model="module.teachingTimePoint" :disabled="formModel.course === null || formModel.course.learningTypeId === 1">
                                    <option v-for="teachingPoint in listTeachingPoints" name="TeachingPoint" :value="teachingPoint">{{teachingPoint.time}}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-1 my-2">
                            <label>Points</label>
                            <div class="input-group">
                                <input class="form-control" :placeholder="module.teachingTimePoint ? module.teachingTimePoint.points : '' " disabled />
                            </div>
                        </div>
                    </div>
                    <hr />
                </div>

            </div>

            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <h4>Participant</h4>
                    <div class="row">
                        <div class="col my-2">
                            <label>Position Status<span class="talent_font_red">*</span></label>
                            <div>
                                <div>
                                    <input type="checkbox" id="permanentStatus" name="PositionStatus" :value="true" v-model="formModel.isParticipantPermanent">
                                    <label for="permanentStatus">Permanent</label>
                                    <input type="checkbox" id="traineeStatus" name="PositionStatus" :value="false" v-model="formModel.isParticipantTrainee">
                                    <label for="traineeStatus">Trainee</label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col my-2">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Area</label>
                                    <div class="resized">
                                    <multiselect v-model="selectedArea"
                                                 tag-placeholder="Add Area"
                                                 placeholder="Add Area"
                                                 label="name"
                                                 track-by="areaId"
                                                 :options="listAreaGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="areaChanged"
                                                 @remove="areaRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    </div>
                                    <br />
                                    <label>Dealer</label>
                                    <div class="resized">
                                    <multiselect v-model="selectedDealer"
                                                 tag-placeholder="Add Dealer"
                                                 placeholder="Add Dealer"
                                                 label="dealerName"
                                                 track-by="dealerId"
                                                 :options="listDealerGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="dealerChanged"
                                                 @remove="dealerRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    </div>
                                    <br />
                                    <label>Province</label>
                                    <div class="resized">
                                    <multiselect v-model="selectedProvince"
                                                 tag-placeholder="Add Province"
                                                 placeholder="Add Province"
                                                 label="provinceName"
                                                 track-by="provinceId"
                                                 :options="listProvinceGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="provinceChanged"
                                                 @remove="provinceRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    </div>
                                    <br />
                                    <label>City</label>
                                    <div class="resized">
                                    <multiselect v-model="selectedCity"
                                                 tag-placeholder="Add City"
                                                 placeholder="Add City"
                                                 label="cityName"
                                                 track-by="cityId"
                                                 :options="listCityGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="cityChanged"
                                                 @remove="cityRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    </div>
                                    <br />
                                    <label>Outlet<span class="talent_font_red">*</span></label>
                                    <div class="resized">
                                    <multiselect v-model="selectedOutletCompleted"
                                                 tag-placeholder="Add Outlet"
                                                 placeholder="Add Outlet"
                                                 label="name"
                                                 name="Outlet"
                                                 track-by="outletId"
                                                 :options="listOutletCompletedGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Position<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                            <multiselect v-model="formModel.listPosition"
                                                        tag-placeholder="Add Position"
                                                        placeholder="Add Position"
                                                        label="positionName"
                                                        track-by="positionId"
                                                        name="Position"
                                                        :options="listPositionGroup"
                                                        group-values="ListOption"
                                                        group-label="GroupName"
                                                        :group-select="true"
                                                        :clear-on-select="true"
                                                        :multiple="true"
                                                        class="oneline-scrollable-multiselect"
                                                        :show-no-results="true"
                                                        selectLabel="Press enter to select">
                                            </multiselect>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4">
                                            <label>View only to<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                            <multiselect v-model="formModel.listPositionOnlyView"
                                                        tag-placeholder="Add Position"
                                                        placeholder="Add Position"
                                                        label="positionName"
                                                        track-by="positionId"
                                                        name="Position"
                                                        :options="listPositionGroup"
                                                        group-values="ListOption"
                                                        group-label="GroupName"
                                                        :group-select="true"
                                                        :clear-on-select="true"
                                                        :multiple="true"
                                                        class="oneline-scrollable-multiselect"
                                                        :show-no-results="true"
                                                        selectLabel="Press enter to select">
                                            </multiselect>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4">
                                                    <label>Certification<span class="talent_font_red">*</span></label>
                                                    <div class="resized">
                                                        <input id="withcertificate" class="mr-1" type="radio" name="isCertificate" :value="true" v-model="formModel.isCertificate" :checked="formModel.isCertificate">
                                                        <label class="mr-4" for="withcertificate">With Certificate</label>
                                                        <input id="nocertificate" class="mr-1" type="radio" name="isCertificate" :value="false" v-model="formModel.isCertificate" :checked="formModel.isCertificate">
                                                        <label class="mr-4" for="nocertificate">No Certificate</label>
                                                    </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate">
                                            <label>Certificate Header<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input id="default" class="mr-1" type="radio" name="certificateHeader" :checked="formModel.enumCertificate == 'Default'" @change="handleChangeCertificateHeader('Default')">
                                                <label class="mr-4" for="default">Default</label>
                                                <input id="customname" class="mr-1" type="radio" name="certificateHeader" :checked="formModel.enumCertificate != 'Default'" @change="handleChangeCertificateHeader('')">
                                                <label class="mr-4" for="customname">Custom Name</label>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate && formModel.enumCertificate != 'Default'">
                                            <label>Certificate Name<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input class="form-control" type="text" v-model="formModel.enumCertificate">
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate">
                                            <label>Certification Trigger<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input class="mr-1" type="radio" name="certificateTrigger" id="personal" value="Personal" v-model="formModel.enumCertificationTtrigger" @change="handleChangeCertificationTtrigger('personal')">
                                                <label class="mr-4" for="personal">Personal</label>
                                                <input class="mr-1" type="radio" name="certificateTrigger" id="hirarki" value="Hirarki" v-model="formModel.enumCertificationTtrigger" @change="handleChangeCertificationTtrigger('hirarki')" :disabled="false">
                                                <label class="mr-4" for="hirarki">Hierarchy</label>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate && formModel.enumCertificationTtrigger == 'Hirarki'">
                                            <label>Course Name<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <multiselect
                                                    ref="addEdit"
                                                    id="addEdit"
                                                    track-by="trainingId"
                                                    name="Course"
                                                    placeholder="Select Course Name"
                                                    label="courseName"
                                                    v-model="formModel.listCertifications[0].courseId"
                                                    :options="trainings"
                                                    :searchable="true"
                                                    :close-on-select="true"
                                                    :clear-on-select="true"
                                                    :loading="isLoading"
                                                    :hide-selected="true"
                                                    :showNoResults="true"
                                                    @search-change="AutoCompleteCourseTraining">
                                                    <span slot="noResult"><i>Not Found!</i></span>
                                                </multiselect>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>TOTAL MODULE<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalModule" />
                            </div>
                        </div>
                        <div class="col my-2">
                            <label>TOTAL SCORE<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalScore" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>TOTAL TIME<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalTime" />
                            </div>
                        </div>
                        <div class="col my-2">
                            <label>TOTAL POINTS<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalPoints" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <div class="col-md-12">
                        <div class="d-flex align-items-end flex-column">
                            <div>
                                <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                    Close
                                </button>
                                <button class="btn talent_bg_lightgreen talent_font_white" @click="updateTraining(1)">
                                    Save
                                </button>
                                <button class="btn talent_bg_blue talent_font_white" @click="updateTraining(2)">
                                    Submit
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <!--View Detail-->
        <div class="col-md-12" v-if="add === false && view === true && edit === false">
            <h3 class="mb-md-3"><fa icon="folder"></fa> Setup > Release Training > <span class="talent_font_red">View Detail Release Training</span></h3>
            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <h4>Release Training</h4>
                    <h4>Course</h4>
                    <div class="row">
                        <div class="col my-2">
                            <label>Course Name<span class="talent_font_red">*</span></label>
                            <multiselect v-model="formModel.course"
                                         id="ajax"
                                         track-by="courseName"
                                         placeholder="Select Course Name"
                                         label="courseName"
                                         :options="getListCourse"
                                         :searchable="true"
                                         :close-on-select="true"
                                         :clear-on-select="true"
                                         :loading="isLoading"
                                         :hide-selected="true"
                                         :showNoResults="true"
                                         disabled
                                         @search-change="AutoCompleteCourse"
                                         @input="findSetup"
                                         @select="reset">

                                <span slot="noResult"><i>Not Found!</i></span>
                            </multiselect>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col my-2">
                            <label>Program Type</label>
                            <div class="input-group">
                                <input class="form-control" :placeholder="formModel.course ? formModel.course.programTypeName : ''" disabled />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>Batch</label>
                            <div class="input-group">
                                <input class="form-control" v-model="formModel.batch" disabled />
                            </div>
                        </div>
                        <div class="col-md-10 my-2">
                            <label>Training Date</label>
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <input class="form-control" type="text" disabled :placeholder="getStringDate(formModel.startDate)" />
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <button class="button_text_no_background" disabled>
                                        <fa style="font-size:30px" icon="arrow-right"></fa>
                                    </button>
                                </div>
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <input class="form-control" type="text" disabled :placeholder="getStringDate(formModel.endDate)" />
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>Quota</label>
                            <div class="input-group">
                                <input class="form-control" type="number" min="0" v-model="formModel.quota" disabled />
                            </div>
                        </div>
                        <div class="col-md-8 my-2">
                            <label>Training Location</label>
                            <div class="input-group">
                                <input class="form-control" v-model="formModel.location" disabled />
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Accomodation</label>
                            <div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="radio" name="Accomodation" id="accomodationYes" :value="true" v-model="formModel.isAccommodate" disabled>
                                    <label class="form-check-label" for="accomodationYes">Yes</label>
                                </div>
                                <div class="form-check form-check-inline">
                                    <input class="form-check-input" type="radio" name="Accomodation" id="accomodationNo" :value="false" v-model="formModel.isAccommodate" disabled>
                                    <label class="form-check-label" for="accomodationNo">No</label>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row justify-content-between" v-for="(module,index) in formModel.listSetupModule">
                        <div class="col-md-1 my-2">
                            <label>Phase</label>
                            <div class="input-group">
                                <input class="form-control" v-model="module.trainingTypesName" disabled />
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Module Name</label>
                            <div class="input-group">
                                <input class="form-control" v-model="module.moduleName" disabled />
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Training Date</label>
                            <div class="input-group">
                                <input class="form-control" type="text" disabled :placeholder="getStringDate(module.trainingStart)" />
                                <div class="input-group-append">
                                    <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Time</label>
                            <div class="row justify-content-between">
                                <div class="px-1">
                                    <vue-timepicker input-width="5em" v-model="startDateForm[index]" format="HH:mm" disabled>
                                    </vue-timepicker>
                                </div>
                                <div class="px-1">
                                    <button class="button_text_no_background" disabled>
                                        <fa style="font-size:1rem" icon="arrow-right"></fa>
                                    </button>
                                </div>
                                <div class="px-1">
                                    <vue-timepicker input-width="5em" v-model="endDateForm[index]" format="HH:mm" disabled>
                                    </vue-timepicker>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Coach Name</label>
                            <multiselect v-model="module.coach"
                                         id="ajax"
                                         track-by="coachId"
                                         placeholder="Select Coach Name"
                                         label="employeeName"
                                         :options="listCoach"
                                         :searchable="true"
                                         :close-on-select="true"
                                         :clear-on-select="true"
                                         :loading="isLoading"
                                         :hide-selected="true"
                                         :showNoResults="true"
                                         disabled
                                         @search-change="AutoCompleteCoach"
                                         @open="resetCoach">

                                <span slot="noResult"><i>Not Found!</i></span>
                            </multiselect>
                        </div>
                        <div class="col-md-2 my-2">
                            <label>Teaching Time (Minutes)</label>
                            <div class="input-group">
                                <select class="form-control" v-model="module.teachingTimePoint" disabled>
                                    <option v-for="teachingPoint in listTeachingPoints" :value="teachingPoint">{{teachingPoint.time}}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-1 my-2">
                            <label>Points</label>
                            <div class="input-group">
                                <input class="form-control" :placeholder="module.teachingTimePoint ? module.teachingTimePoint.points : '' " disabled />
                            </div>
                        </div>
                    </div>
                    <hr />
                </div>

            </div>

            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <h4>Participant</h4>
                    <div class="row">
                        <div class="col my-2">
                            <label>Position Status<span class="talent_font_red">*</span></label>
                            <div>
                                <input type="checkbox" id="permanentStatus" :value="true" v-model="formModel.isParticipantPermanent" disabled>
                                <label for="permanentStatus">Permanent</label>
                                <input type="checkbox" id="traineeStatus" :value="false" v-model="formModel.isParticipantTrainee" disabled>
                                <label for="traineeStatus">Trainee</label>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col my-2">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Area</label>
                                    <multiselect v-model="selectedArea"
                                                 tag-placeholder="Add Area"
                                                 placeholder="Add Area"
                                                 label="name"
                                                 track-by="areaId"
                                                 :options="listAreaGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 :disabled="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>Dealer</label>
                                    <multiselect v-model="selectedDealer"
                                                 tag-placeholder="Add Dealer"
                                                 placeholder="Add Dealer"
                                                 label="dealerName"
                                                 track-by="dealerId"
                                                 :options="listDealerGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 :disabled="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>Province</label>
                                    <multiselect v-model="selectedProvince"
                                                 tag-placeholder="Add Province"
                                                 placeholder="Add Province"
                                                 label="provinceName"
                                                 track-by="provinceId"
                                                 :options="listProvinceGroup"
                                                 :disabled="true"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>City</label>
                                    <multiselect v-model="selectedCity"
                                                 tag-placeholder="Add City"
                                                 placeholder="Add City"
                                                 label="cityName"
                                                 track-by="cityId"
                                                 :options="listCityGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 :disabled="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>Outlet<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="formModel.listOutlet"
                                                 tag-placeholder="Add Outlet"
                                                 placeholder="Add Outlet"
                                                 label="name"
                                                 name="Outlet"
                                                 track-by="outletId"
                                                 :options="listOutletGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :disabled="true"
                                                 :show-no-results="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Position<span class="talent_font_red">*</span></label>
                                            <multiselect v-model="formModel.listPosition"
                                                        tag-placeholder="Add Position"
                                                        placeholder="Add Position"
                                                        label="positionName"
                                                        track-by="positionId"
                                                        name="Position"
                                                        :options="listPositionGroup"
                                                        group-values="ListOption"
                                                        group-label="GroupName"
                                                        :group-select="true"
                                                        :clear-on-select="true"
                                                        :multiple="true"
                                                        :hide-selected="true"
                                                        :disabled="true"
                                                        :show-no-results="true"
                                                        selectLabel="Press enter to select">
                                            </multiselect>
                                        </div>

                                        <div class="col-md-12 mt-4">
                                            <label>View only to<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                            <multiselect v-model="formModel.listPositionOnlyView"
                                                        tag-placeholder="Add Position"
                                                        placeholder="Add Position"
                                                        label="positionName"
                                                        track-by="positionId"
                                                        name="Position"
                                                        :options="listPositionGroup"
                                                        group-values="ListOption"
                                                        group-label="GroupName"
                                                        :group-select="true"
                                                        :clear-on-select="true"
                                                        :multiple="true"
                                                        class="oneline-scrollable-multiselect"
                                                        :show-no-results="true"
                                                        :disabled="true"
                                                        selectLabel="Press enter to select">
                                            </multiselect>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4">
                                                    <label>Certification<span class="talent_font_red">*</span></label>
                                                    <div class="resized">
                                                        <input id="withcertificate" class="mr-1" type="radio" name="isCertificate" :value="true" v-model="formModel.isCertificate" :checked="formModel.isCertificate" :disabled="true">
                                                        <label class="mr-4" for="withcertificate">With Certificate</label>
                                                        <input id="nocertificate" class="mr-1" type="radio" name="isCertificate" :value="false" v-model="formModel.isCertificate" :checked="formModel.isCertificate" :disabled="true">
                                                        <label class="mr-4" for="nocertificate">No Certificate</label>
                                                    </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate">
                                            <label>Certificate Header<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input id="default" class="mr-1" type="radio" name="certificateHeader" :checked="formModel.enumCertificate == 'Default'" @change="handleChangeCertificateHeader('Default')" :disabled="true">
                                                <label class="mr-4" for="default">Default</label>
                                                <input id="customname" class="mr-1" type="radio" name="certificateHeader" :checked="formModel.enumCertificate != 'Default'" @change="handleChangeCertificateHeader('')" :disabled="true">
                                                <label class="mr-4" for="customname">Custom Name</label>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate && formModel.enumCertificate != 'Default'">
                                            <label>Certificate Name<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input class="form-control" type="text" v-model="formModel.enumCertificate" :disabled="true">
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate">
                                            <label>Certification Trigger<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <input class="mr-1" type="radio" name="certificateTrigger" id="personal" value="Personal" v-model="formModel.enumCertificationTtrigger" :disabled="true">
                                                <label class="mr-4" for="personal">Personal</label>
                                                <input class="mr-1" type="radio" name="certificateTrigger" id="hirarki" value="Hirarki" v-model="formModel.enumCertificationTtrigger" :disabled="true">
                                                <label class="mr-4" for="hirarki">Hierarchy</label>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-4" v-if="formModel.isCertificate && formModel.enumCertificationTtrigger == 'Hirarki'">
                                            <label>Course Name<span class="talent_font_red">*</span></label>
                                            <div class="resized">
                                                <multiselect
                                                    ref="addView"
                                                    id="addView"
                                                    track-by="trainingId"
                                                    name="Course"
                                                    placeholder="Select Course Name"
                                                    label="courseName"
                                                    v-model="formModel.listCertifications[0].courseId"
                                                    :options="trainings"
                                                    :searchable="true"
                                                    :close-on-select="true"
                                                    :clear-on-select="true"
                                                    :loading="isLoading"
                                                    :hide-selected="true"
                                                    :showNoResults="true"
                                                    @search-change="AutoCompleteCourseTraining"
                                                    :disabled="true">

                                                    <span slot="noResult"><i>Not Found!</i></span>
                                                </multiselect>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>TOTAL MODULE<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalModule" />
                            </div>
                        </div>
                        <div class="col my-2">
                            <label>TOTAL SCORE<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalScore" />
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-2 my-2">
                            <label>TOTAL TIME<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalTime" />
                            </div>
                        </div>
                        <div class="col my-2">
                            <label>TOTAL POINTS<span class="talent_font_red">*</span></label>
                        </div>
                        <div class="col-md-4 my-2">
                            <div class="input-group">
                                <input class="form-control" disabled :placeholder="totalDetail.totalPoints" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                <div class="col-md-12">
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

        <!--Delete-->
        <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="col-md-12 justify-content-center d-flex">
                            <h5>Are you sure want to delete?</h5>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="col-md-12 d-flex justify-content-around">
                                <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="deleteConfirmed()">Delete</button>
                                <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" data-dismiss="modal">Close</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style>
.resized .oneline-scrollable-multiselect .multiselect__tags {
    min-height: 45px;
    max-height: 71vh !important;
    overflow: scroll;
    resize: vertical;
}
</style>
<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { Sort } from '../../class/Sort';
    import { ReleaseTrainingService, CourseReleaseTrainingModel, ReleaseTrainingFormModel, TrainingModuleFormModel, ReleaseTrainingSetupModuleModel, CoachForReleaseTraining, TeachingTimepPointsModel, AreaViewModel, PositionViewModel, DealerViewModel, ProvinceViewModel, CityViewModel, OutletViewModel, ReleaseTrainingViewModel, ApprovalStatusViewModels, TotalCourseDetail, UserPrivilegeSettingsService, UserAccessCRUD, OutletCompleteViewModel, TrainingOutletFormModel } from '../../services/NSwagService';
    import Message from '../../class/Message';
    import { PageEnums } from '../../enum/PageEnums';
    import { ApprovalStatusEnum } from '../../enum/ApprovalStatusEnum';

    @Component({
        created: async function (this: ReleaseTrainings) {
            await this.getAccess();
            await this.initialize();
            if (this.fromOutside === true) {
                await this.detailClickedFromOutside(this.trainingId);
            }
        },

        props: ['trainingId', 'fromOutside']
    })
    export default class ReleaseTrainings extends Vue {
        currentPage: number = 1;
        pageSize: number = 10;

        trainingId: number;
        fromOutside: boolean;

        add: boolean = false;
        edit: boolean = false;
        view: boolean = false;

        Service: ReleaseTrainingService = new ReleaseTrainingService();

        timeout: any = null;

        isLoading: boolean = false;
        isBusy: boolean = false;

        listReleaseTraining: ReleaseTrainingViewModel = {
            listTraining: [],
            totalData: null
        };

        listOutletFormModel: TrainingOutletFormModel[] = [];

        waitingForApproval = ApprovalStatusEnum.WAITINGNAME;

        startDateForm: string[] = [];
        endDateForm: string[] = [];

        errorMessage: string = '';
        successMessage: string = '';

        listCourse: CourseReleaseTrainingModel[] = [];
        
        public get getListCourse() : any {
            return this.listCourse.sort((a,b) => (a.courseName > b.courseName) ? 1 : ((b.courseName > a.courseName) ? -1 : 0))
        }
        
        listSetupModule: ReleaseTrainingSetupModuleModel[] = [];
        listCoach: CoachForReleaseTraining[] = [];
        listTeachingPoints: TeachingTimepPointsModel[] = [];
        listApprovalStatus: ApprovalStatusViewModels[] = [];

        minDate: Date = new Date('0001-01-01T00:00:00');

        listArea: AreaViewModel[] = [];
        listDealer: DealerViewModel[] = [];
        listProvince: ProvinceViewModel[] = [];
        listCity: CityViewModel[] = [];
        listOutlet: OutletViewModel[] = [];
        listCompletedOutlet: OutletCompleteViewModel[] = [];
        trainings: any = [];

        listAreaGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listPositionGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listDealerGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listProvinceGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listCityGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listOutletGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listOutletCompletedGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];

        selectedArea: AreaViewModel[] = [];
        selectedPosition: PositionViewModel[] = [];
        selectedDealer: DealerViewModel[] = [];
        selectedProvince: ProvinceViewModel[] = [];
        selectedCity: CityViewModel[] = [];
        selectedOutletCompleted: OutletCompleteViewModel[] = [];

        moduleTemplate: TrainingModuleFormModel = {};

        selectedCoach: CoachForReleaseTraining = {};

        formModel: ReleaseTrainingFormModel = {
            listOutlet: [],
            listPosition: [],
            listPositionOnlyView: [],
            listSetupModule: [],
            batch: 0,
            location: '',
            isParticipantPermanent: false,
            isParticipantTrainee: false,
            course: null,
            isAccommodate: null,
            isCertificate: false,
            enumCertificate: 'Default',
            enumCertificationTtrigger: 'Personal',
            listCertifications: []
        };

        filter: IReleaseTrainingFilter = {
            dateFilter: {
                end: null,
                start: null
            }
        };

        totalDetail: TotalCourseDetail = {
            totalModule: 0,
            totalPoints: 0,
            totalScore: 0,
            totalTime: 0
        }

        indexToDelete: number;

        props: any = {
            placeholder: "",
            readonly: true
        };

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        handleChangeCertificationTtrigger(status) {
            if(status == 'personal') {
                this.formModel.listCertifications = []
            } else {
                this.formModel.listCertifications = [{
                    courseId: null
                }]
            }
        }

        handleChangeCertificateHeader (data) {
            this.formModel.enumCertificate = data
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.ReleaseTraining);
            this.crud = data
        }

        async addClicked() {

            if (!this.crud.create) {
                return;
            }

            this.$validator.pause();

            this.clearMessage();

            this.formModel = {
                listOutlet: [],
                listPosition: [],
                listPositionOnlyView: [],
                listSetupModule: [],
                batch: 0,
                location: '',
                isParticipantPermanent: false,
                isParticipantTrainee: false,
                course: null,
                isCertificate: false,
                enumCertificate: 'Default',
                enumCertificationTtrigger: 'Personal',
                listCertifications: []
            };

            this.add = true;
            this.edit = false;
            this.view = false;

            this.resetDate();

            this.listArea = await this.Service.getAllArea();
            this.listDealer = await this.Service.getAllDealer();
            this.listProvince = await this.Service.getAllProvince();
            this.listCity = await this.Service.getAllCity();
            this.listCompletedOutlet = await this.Service.getAllOutlet();

            this.listAreaGroup[0].ListOption = this.listArea;
            this.listDealerGroup[0].ListOption = this.listDealer;
            this.listProvinceGroup[0].ListOption = this.listProvince;
            this.listCityGroup[0].ListOption = this.listCity;
            this.listOutletCompletedGroup[0].ListOption = this.listCompletedOutlet;
            this.listPositionGroup[0].ListOption = await this.Service.getAllPosition();
        }

        async initialize() {
            await this.fetch();
            this.listCourse = await this.Service.getCourseSetupLearning('');
            this.listApprovalStatus = await this.Service.getApprovalStatus();
            this.trainings = await (await this.Service.getRelatedReleaseTraining(null, null, null, null, null, null, null, null, 1)).listTraining.map(v => ({...v, courseName: `${v.courseName} (Batch-${v.batch})`}));
        }

        resetDate() {
            this.startDateForm = [];
            this.endDateForm = [];
        }

        async editClicked(index: number) {

            if (!this.crud.update) {
                return;
            }

            this.$validator.pause();

            this.isBusy = true;
            this.clearMessage();
            this.resetDate();

            let result = await this.Service.getReleaseTrainingDetail(this.listReleaseTraining.listTraining[index].trainingId);

            this.listArea = await this.Service.getAllArea();
            this.listDealer = await this.Service.getAllDealer();
            this.listProvince = await this.Service.getAllProvince();
            this.listCity = await this.Service.getAllCity();
            this.listCompletedOutlet = await this.Service.getAllOutlet();

            this.listPositionGroup[0].ListOption = await this.Service.getAllPosition();

            this.listAreaGroup[0].ListOption = this.listArea;
            this.listDealerGroup[0].ListOption = this.listDealer;
            this.listProvinceGroup[0].ListOption = this.listProvince;
            this.listCityGroup[0].ListOption = this.listCity;

            this.formModel = {
                batch: result.batch,
                course: result.course,
                endDate: new Date(result.endDate),
                isAccommodate: result.isAccommodate,
                isParticipantTrainee: result.isParticipantTrainee,
                isParticipantPermanent: result.isParticipantPermanent,
                listPosition: result.listPosition,
                listPositionOnlyView: result.listPositionOnlyView,
                listSetupModule: result.listSetupModule,
                location: result.location,
                quota: result.quota,
                startDate: new Date(result.startDate),
                trainingId: result.trainingId,
                isCertificate: result.isCertificate,
                enumCertificate: result.enumCertificate,
                enumCertificationTtrigger: result.enumCertificationTrigger,
                listCertifications: result.trainingCertificateView.map(v => ({
                    courseId: {
                        trainingId: v.relatedTrainingId,
                        courseName: v.relatedTrainingName
                    }
                }))
            }

            if (this.formModel.startDate.getUTCFullYear() === this.minDate.getUTCFullYear()) {
                this.formModel.startDate = null;
            }

            if (this.formModel.endDate.getUTCFullYear() === this.minDate.getUTCFullYear()) {
                this.formModel.endDate = null;
            }

            for (var indexTime = 0; indexTime < this.formModel.listSetupModule.length; indexTime++) {
                let startDate = this.formModel.listSetupModule[indexTime].trainingStart != null ? new Date(this.formModel.listSetupModule[indexTime].trainingStart) : null;
                let endDate = this.formModel.listSetupModule[indexTime].trainingEnd != null ? new Date(this.formModel.listSetupModule[indexTime].trainingEnd) : null;

                let startTime;
                let endTime;

                if (endDate == null || (endDate.getDate() === this.minDate.getDate() && endDate.getMonth() === this.minDate.getMonth() && endDate.getFullYear() === this.minDate.getFullYear())) {
                    endDate = null;
                    endTime = '';
                }
                else {
                    endTime = (endDate.getHours() < 10 ? "0" + endDate.getHours() : endDate.getHours()) + ":" + (endDate.getMinutes() < 10 ? "0" + endDate.getMinutes() : endDate.getMinutes());
                }

                if (startDate == null || (startDate.getDate() === this.minDate.getDate() && startDate.getMonth() === this.minDate.getMonth() && startDate.getFullYear() === this.minDate.getFullYear())) {
                    startDate = null;
                    startTime = '';
                }
                else {
                    startTime = (startDate.getHours() < 10 ? "0" + startDate.getHours() : startDate.getHours()) + ":" + (startDate.getMinutes() < 10 ? "0" + startDate.getMinutes() : startDate.getMinutes());
                }

                this.endDateForm.push(endTime);
                this.startDateForm.push(startTime);

                this.formModel.listSetupModule[indexTime].trainingEnd = endDate;
                this.formModel.listSetupModule[indexTime].trainingStart = startDate;

            }

            //this.selectedArea = result.listArea;
            //this.selectedDealer = result.listDealer;
            //this.selectedProvince = result.listProvince;
            //this.selectedCity = result.listCity;
            //this.selectedPosition = result.listPosition;

            var areaIdList = result.listArea.map(Q => Q.areaId);
            this.selectedArea = this.listAreaGroup[0].ListOption.filter(Q => areaIdList.includes(Q.areaId)).map(x => x);

            var dealerIdList = result.listDealer.map(Q => Q.dealerId);
            this.selectedDealer = this.listDealerGroup[0].ListOption.filter(Q => dealerIdList.includes(Q.dealerId)).map(x => x);

            var provinceIdList = result.listProvince.map(Q => Q.provinceId);
            this.selectedProvince = this.listProvinceGroup[0].ListOption.filter(Q => provinceIdList.includes(Q.provinceId)).map(x => x);

            var cityIdList = result.listCity.map(Q => Q.cityId);
            this.selectedCity = this.listCityGroup[0].ListOption.filter(Q => cityIdList.includes(Q.cityId)).map(x => x);

            var positionIdList = result.listPosition.map(Q => Q.positionId);
            this.formModel.listPosition = this.listPositionGroup[0].ListOption.filter(Q => positionIdList.includes(Q.positionId)).map(x => x);

            var positionIdList = result.listPositionOnlyView.map(Q => Q.positionId);
            this.formModel.listPositionOnlyView = this.listPositionGroup[0].ListOption.filter(Q => positionIdList.includes(Q.positionId)).map(x => x);

            var currentSelectedOutletIds = result.listOutlet.map(Q => Q.outletId);
            this.editListGroup();

            var selected = this.listOutletCompletedGroup[0].ListOption.filter(Q => currentSelectedOutletIds.includes(Q.outletId)).map(x => x);
            this.selectedOutletCompleted = selected.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            this.totalDetail = await this.Service.getTotalDetailCourse(this.formModel.course.courseId);

            this.add = false;
            this.edit = true;
            this.view = false;

            this.isBusy = false;
        }

        getStringDate(date: Date) {

            if (!date) {
                return '-';
            }

            if (new Date(date).getUTCFullYear() === this.minDate.getUTCFullYear()) {
                return '-';
            }

            let newDate = new Date(date);

            var ddDate = newDate.getDate();
            var mmDate = newDate.getMonth() + 1;
            var yyyyDate = newDate.getFullYear();
            var dd = '' + ddDate;
            var mm = '' + mmDate;
            var yyyy = yyyyDate;
            if (ddDate < 10) {
                dd = '0' + ddDate;
            }
            if (mmDate < 10) {
                mm = '0' + mmDate;
            }
            var today = dd + '/' + mm + '/' + yyyy;

            return today;
        }

        async closeClicked() {
            this.add = false;
            this.edit = false;
            this.view = false;

            this.$validator.errors.clear();

            this.$validator.pause();

            await this.fetch();

            this.resetFilter();
            this.clearForm();

        }

        clearForm() {
            this.formModel = {
                listOutlet: [],
                listPosition: [],
                listPositionOnlyView: [],
                listSetupModule: [],
                batch: 0,
                location: '',
                listCertifications: []
            };

            this.selectedArea = [];
            this.selectedCity = [];
            this.selectedPosition = [];
            this.selectedDealer = [];
            this.selectedOutletCompleted = [];
            this.selectedPosition = [];
            this.selectedProvince = [];

            this.resetDate();

        }

        checkCourse(): boolean {
            if (!this.formModel.course) {
                this.$validator.errors.add({
                    field: "Course",
                    msg: "Course can't be empty"
                });
                return false;
            }
            return true;
        }

        checkPosition(inputType: number): boolean {
            if (inputType == 1) {
                return true;
            }
            else {
                if (this.formModel.listPosition.length < 1) {
                    this.$validator.errors.add({
                        field: "Position",
                        msg: "Position can't be empty"
                    });
                    return false;
                }
            }
            return true;
        }

        checkOutlet(inputType: number): boolean {
            if (inputType == 1) {
                return true;
            }
            else {
                if (this.selectedOutletCompleted.length < 1) {
                    this.$validator.errors.add({
                        field: "Outlet",
                        msg: "Outlet can't be empty"
                    });
                    return false;
                }
            }
            return true;
        }

        checkListModule(inputType: number) {
            if (this.formModel.listSetupModule.length > 0) {
                for (let index = 0; index < this.formModel.listSetupModule.length; index++) {
                    //Kalau Date di isi harus di isi smua
                    if (inputType == 1) {
                        if (this.formModel.listSetupModule[index].trainingStart) {
                            let tempStartDate = this.formModel.listSetupModule[index].trainingStart;
                            if (this.formModel.listSetupModule[index].trainingStart) {
                                try {
                                    if (this.formModel.listSetupModule[index].trainingStart !== undefined || this.formModel.listSetupModule[index].trainingStart !== null) {
                                        let endDate = new Date(this.formModel.listSetupModule[index].trainingStart);

                                        var endHour = parseInt(this.endDateForm[index].substring(0, 2));
                                        var endMinute = parseInt(this.endDateForm[index].substring(3, 5));
                                        var startHour = parseInt(this.startDateForm[index].substring(0, 2));
                                        var startMinute = parseInt(this.startDateForm[index].substring(3, 5));

                                        if (isNaN(endHour) || isNaN(endMinute) || isNaN(startHour) || isNaN(startMinute)) {
                                            this.$validator.errors.add({
                                                field: "TrainingStart",
                                                msg: "Format Date is not valid"
                                            });
                                        }

                                        this.formModel.listSetupModule[index].trainingStart.setHours(startHour, startMinute);
                                        endDate.setHours(endHour, endMinute);
                                        this.formModel.listSetupModule[index].trainingEnd = endDate;

                                        if (this.formModel.listSetupModule[index].trainingEnd < this.formModel.listSetupModule[index].trainingStart) {
                                            console.log("Masuk");
                                            this.$validator.errors.add({
                                                field: "TrainingStart",
                                                msg: "Time End must be bigger than Time Start"
                                            });
                                        }
                                    }
                                }
                                catch{
                                    this.formModel.listSetupModule[index].trainingStart = tempStartDate;
                                    this.$validator.errors.add({
                                        field: "TrainingStart",
                                        msg: "Format Date is not valid"
                                    });
                                }
                            }
                        }
                    }
                    else if (inputType == 2) {
                        if (!this.formModel.listSetupModule[index].trainingStart) {
                            this.$validator.errors.add({
                                field: "TrainingStart",
                                msg: "Date must be filled"
                            });
                        }

                        if (this.formModel.listSetupModule[index].trainingStart) {
                            let tempStartDate = this.formModel.listSetupModule[index].trainingStart;

                            try {
                                if (this.formModel.listSetupModule[index].trainingStart !== undefined || this.formModel.listSetupModule[index].trainingStart !== null) {
                                    let endDate = new Date(this.formModel.listSetupModule[index].trainingStart);

                                    var endHour = parseInt(this.endDateForm[index].substring(0, 2));
                                    var endMinute = parseInt(this.endDateForm[index].substring(3, 5));
                                    var startHour = parseInt(this.startDateForm[index].substring(0, 2));
                                    var startMinute = parseInt(this.startDateForm[index].substring(3, 5));

                                    if (isNaN(endHour) || isNaN(endMinute) || isNaN(startHour) || isNaN(startMinute)) {
                                        this.$validator.errors.add({
                                            field: "TrainingStart",
                                            msg: "Format Date is not valid"
                                        });
                                    }

                                    this.formModel.listSetupModule[index].trainingStart.setHours(startHour, startMinute);
                                    endDate.setHours(endHour, endMinute);
                                    this.formModel.listSetupModule[index].trainingEnd = endDate;

                                    if (this.formModel.listSetupModule[index].trainingEnd < this.formModel.listSetupModule[index].trainingStart) {
                                        this.$validator.errors.add({
                                            field: "TrainingStart",
                                            msg: "Time End must be bigger than Time Start"
                                        });
                                    }
                                }
                            }
                            catch{
                                this.formModel.listSetupModule[index].trainingStart = tempStartDate;

                                this.$validator.errors.add({
                                    field: "TrainingStart",
                                    msg: "Format Date is not valid"
                                });
                            }
                        }
                    }

                    if (inputType == 2) {

                        if (this.formModel.listSetupModule[index].coach === null || this.formModel.listSetupModule[index].coach === undefined) {
                            this.$validator.errors.add({
                                field: "Coach",
                                msg: "Coach must be filled"
                            });
                        }

                        if (this.formModel.listSetupModule[index].teachingTimePoint === null || this.formModel.listSetupModule[index].teachingTimePoint === undefined) {
                            this.$validator.errors.add({
                                field: "TeachingPoint",
                                msg: "Teaching point must be filled"
                            });
                        }
                    }
                }
            }
        }

        checkListModuleForOfflineOnline(inputType: number) {
            if (this.formModel.listSetupModule.length > 0) {
                for (let index = 0; index < this.formModel.listSetupModule.length; index++) {
                    if (inputType == 1) {
                        if (this.formModel.listSetupModule[index].trainingStart) {
                            try {
                                if (this.formModel.listSetupModule[index].trainingStart !== undefined || this.formModel.listSetupModule[index].trainingStart !== null) {
                                    let endDate = new Date(this.formModel.listSetupModule[index].trainingStart);

                                    var endHour = parseInt(this.endDateForm[index].substring(0, 2));
                                    var endMinute = parseInt(this.endDateForm[index].substring(3, 5));
                                    var startHour = parseInt(this.startDateForm[index].substring(0, 2));
                                    var startMinute = parseInt(this.startDateForm[index].substring(3, 5));

                                    if (isNaN(endHour) || isNaN(endMinute) || isNaN(startHour) || isNaN(startMinute)) {
                                        this.$validator.errors.add({
                                            field: "TrainingStart",
                                            msg: "Format Date is not valid"
                                        });
                                    }

                                    this.formModel.listSetupModule[index].trainingStart.setHours(startHour, startMinute);
                                    endDate.setHours(endHour, endMinute);
                                    this.formModel.listSetupModule[index].trainingEnd = endDate;

                                    if (this.formModel.listSetupModule[index].trainingEnd < this.formModel.listSetupModule[index].trainingStart) {

                                        this.$validator.errors.add({
                                            field: "TrainingStart",
                                            msg: "Time End must be bigger than Time Start"
                                        });
                                    }
                                }
                            }
                            catch{

                                this.$validator.errors.add({
                                    field: "TrainingStart",
                                    msg: "Format Date is not valid"
                                });
                            }
                        }
                    }
                    else {
                        if (!this.formModel.listSetupModule[index].trainingStart) {
                            this.$validator.errors.add({
                                field: "TrainingStart",
                                msg: "Date must be filled"
                            });
                        }

                        if (this.formModel.listSetupModule[index].trainingStart) {
                            try {
                                if (this.formModel.listSetupModule[index].trainingStart !== undefined || this.formModel.listSetupModule[index].trainingStart !== null) {
                                    let endDate = new Date(this.formModel.listSetupModule[index].trainingStart);

                                    var endHour = parseInt(this.endDateForm[index].substring(0, 2));
                                    var endMinute = parseInt(this.endDateForm[index].substring(3, 5));
                                    var startHour = parseInt(this.startDateForm[index].substring(0, 2));
                                    var startMinute = parseInt(this.startDateForm[index].substring(3, 5));

                                    if (isNaN(endHour) || isNaN(endMinute) || isNaN(startHour) || isNaN(startMinute)) {
                                        this.$validator.errors.add({
                                            field: "TrainingStart",
                                            msg: "Format Date is not valid"
                                        });
                                    }

                                    this.formModel.listSetupModule[index].trainingStart.setHours(startHour, startMinute);
                                    endDate.setHours(endHour, endMinute);
                                    this.formModel.listSetupModule[index].trainingEnd = endDate;

                                    if (this.formModel.listSetupModule[index].trainingEnd < this.formModel.listSetupModule[index].trainingStart) {

                                        this.$validator.errors.add({
                                            field: "TrainingStart",
                                            msg: "Time End must be bigger than Time Start"
                                        });
                                    }
                                }
                            }
                            catch{

                                this.$validator.errors.add({
                                    field: "TrainingStart",
                                    msg: "Format Date is not valid"
                                });
                            }
                        }

                    }

                    if (inputType == 2) {
                        if (this.formModel.listSetupModule[index].teachingTimePoint) {
                            if (this.formModel.listSetupModule[index].coach === null || this.formModel.listSetupModule[index].coach === undefined) {
                                this.$validator.errors.add({
                                    field: "Coach",
                                    msg: "Coach must be filled"
                                });
                            }
                        }

                        if (this.formModel.listSetupModule[index].teachingTimePoint === null || this.formModel.listSetupModule[index].teachingTimePoint === undefined) {
                            this.$validator.errors.add({
                                field: "TeachingPoint",
                                msg: "Teaching point must be filled"
                            });
                        }
                    }

                }
            }
        }

        checkPositionStatus(inputType: number): boolean {
            if (inputType == 1) {
                return true;
            }
            else {
                if (this.formModel.isParticipantPermanent == false && this.formModel.isParticipantTrainee == false) {
                    this.$validator.errors.add({
                        field: "PositionStatus",
                        msg: "Position Status must be selected at least one"
                    });
                    return false;
                }
                return true;
            }
        }

        checkDate(): boolean {
            if (this.formModel.endDate !== null && this.formModel.startDate === null) {
                this.$validator.errors.add({
                    field: "StartDate",
                    msg: "Start Date must be filled!"
                });
            }
            else if (this.formModel.endDate === null && this.formModel.startDate !== null) {
                this.$validator.errors.add({
                    field: "EndDate",
                    msg: "End Date Date must be filled!"
                });
            }
            return true;
        }

        async insertTraining(inputType: number) {

            if (!this.crud.create) {
                return;
            }

            this.formModel.inputType = inputType;
            this.$validator.resume();
            this.$validator.errors.clear();
            let result = true;

            if (inputType == 1) {
                result = true;
            }
            else {
                result = await this.$validator.validateAll();
            }

            let check = this.checkCourse();
            this.checkPosition(inputType);
            this.checkOutlet(inputType);
            this.checkPositionStatus(inputType);
            this.checkDate();

            if (check === true) {
                if (this.formModel.course.learningTypeId === 1) {
                    for (let module of this.formModel.listSetupModule) {
                        module.trainingStart = module.trainingEnd = null;
                    }
                }
                else if (this.formModel.course.learningTypeId === 2) {
                    this.checkListModule(inputType);
                }
                else {
                    this.checkListModuleForOfflineOnline(inputType);
                }
            }

            if (this.$validator.errors.any() || result === false) {
                return;
            }

            this.formModel.listOutlet = [];

            this.listOutletFormModel = [];

            for (var a = 0; a < this.selectedOutletCompleted.length; a++) {
                var outlet = <TrainingOutletFormModel>{
                    outletId: this.selectedOutletCompleted[a].outletId
                };

                this.listOutletFormModel.push(outlet);
            }

            this.formModel.listOutlet = this.listOutletFormModel;

            this.isBusy = true;

            if(this.formModel.listCertifications.length > 0 && this.formModel.listCertifications[0].courseId.trainingId) {
                this.formModel.listCertifications = [{
                    courseId: this.formModel.listCertifications[0].courseId.trainingId
                }]
            } else {
                this.formModel.listCertifications = []
            }

            try {
                let result = await this.Service.insertRelaseTraining(this.formModel);
                if (result === true) {
                    this.successMessage = Message.SuccessInsertMessage;
                    await this.closeClicked();
                }
            }
            catch{
                this.errorMessage = "Failed to Insert Data";
            }

            this.isBusy = false;

        }

        async updateTraining(inputType: number) {

            if (!this.crud.update) {
                return;
            }

            this.formModel.inputType = inputType;
            this.$validator.resume();
            this.$validator.errors.clear();

            let result = true;

            if (inputType == 1) {
                result = true;
            }
            else {
                result = await this.$validator.validateAll();
            }

            let check = this.checkCourse();
            this.checkPosition(inputType);
            this.checkOutlet(inputType);
            this.checkPositionStatus(inputType);
            this.checkDate();

            if (check === true) {
                if (this.formModel.course.learningTypeId === 1) {
                    for (let module of this.formModel.listSetupModule) {
                        module.trainingStart = module.trainingEnd = null;
                    }
                }
                else if (this.formModel.course.learningTypeId === 2) {
                    this.checkListModule(inputType);
                }
                else {
                    this.checkListModuleForOfflineOnline(inputType);
                }
            }

            if (this.$validator.errors.any() || result === false) {
                return;
            }

            this.isBusy = true;

            this.formModel.listOutlet = [];
            this.listOutletFormModel = [];

            for (var a = 0; a < this.selectedOutletCompleted.length; a++) {
                var outlet = <TrainingOutletFormModel>{
                    outletId: this.selectedOutletCompleted[a].outletId
                };

                this.listOutletFormModel.push(outlet);
            }

            this.formModel.listOutlet = this.listOutletFormModel;

            if(this.formModel.listCertifications.length > 0 && this.formModel.listCertifications[0].courseId.trainingId) {
                this.formModel.listCertifications = [{
                    courseId: this.formModel.listCertifications[0].courseId.trainingId
                }]
            } else {
                this.formModel.listCertifications = []
            }

            try {
                let result = await this.Service.updateReleaseTraining(this.formModel);
                this.successMessage = Message.SuccessEditMessage;
                await this.closeClicked();
                this.isBusy = false;
            }
            catch{
                this.isBusy = false;
                this.errorMessage = "Failed to Update Data";
            }
            this.isBusy = false;
        }

        locationRequired(): boolean {
            if (this.formModel.course === null) {
                return false;
            }
            else {
                if (this.formModel.course.learningTypeId === 1) {
                    return false;
                }
                else if (this.formModel.course.learningTypeId === 2) {
                    return true;
                }
                else if (this.formModel.course.learningTypeId === 3) {
                    return false;
                }
            }
            return false;
        }

        async detailClickedFromOutside(id: number) {

            if (!this.crud.read) {
                return;
            }

            this.isBusy = true;
            this.clearMessage();
            this.add = false;
            this.edit = false;
            this.view = true;

            let result = await this.Service.getReleaseTrainingDetail(id);

            this.formModel = {
                batch: result.batch,
                course: result.course,
                endDate: new Date(result.endDate),
                isAccommodate: result.isAccommodate,
                isParticipantTrainee: result.isParticipantTrainee,
                isParticipantPermanent: result.isParticipantPermanent,
                listOutlet: result.listOutlet,
                listPosition: result.listPosition,
                listPositionOnlyView: result.listPositionOnlyView,
                listSetupModule: result.listSetupModule,
                location: result.location,
                quota: result.quota,
                startDate: new Date(result.startDate),
                trainingId: result.trainingId,
                isCertificate: result.isCertificate,
                enumCertificate: result.enumCertificate,
                enumCertificationTtrigger: result.enumCertificationTrigger,
                listCertifications: result.trainingCertificateView.map(v => ({
                    courseId: {
                        trainingId: v.relatedTrainingId,
                        courseName: v.relatedTrainingName
                    }
                }))
            }

            this.totalDetail = await this.Service.getTotalDetailCourse(this.formModel.course.courseId);

            this.selectedArea = result.listArea;
            this.listDealer = this.selectedDealer = result.listDealer;
            this.listProvince = this.selectedProvince = result.listProvince;
            this.listCity = this.selectedCity = result.listCity;

            this.selectedPosition = result.listPosition;

            this.resetDate();

            for (var indexTime = 0; indexTime < this.formModel.listSetupModule.length; indexTime++) {
                let startDate = this.formModel.listSetupModule[indexTime].trainingStart != null ? new Date(this.formModel.listSetupModule[indexTime].trainingStart) : null;
                let endDate = this.formModel.listSetupModule[indexTime].trainingEnd != null ? new Date(this.formModel.listSetupModule[indexTime].trainingEnd) : null;

                this.formModel.listSetupModule[indexTime].trainingEnd = endDate;
                this.formModel.listSetupModule[indexTime].trainingStart = startDate;

                let checkStartDate = this.getStringDate(startDate);
                let checkEndDate = this.getStringDate(endDate);
                if (checkStartDate === '-') {

                    this.startDateForm.push('');
                }
                else {
                    let startTime = (startDate.getHours() < 10 ? "0" + startDate.getHours() : startDate.getHours()) + ":" + (startDate.getMinutes() < 10 ? "0" + startDate.getMinutes() : startDate.getMinutes());
                    this.startDateForm.push(startTime);
                }

                if (checkEndDate === '-') {
                    this.startDateForm.push('');
                }
                else {
                    let endTime = (endDate.getHours() < 10 ? "0" + endDate.getHours() : endDate.getHours()) + ":" + (endDate.getMinutes() < 10 ? "0" + endDate.getMinutes() : endDate.getMinutes());
                    this.endDateForm.push(endTime);
                }

            }

            this.isBusy = false;
        }

        async detailClicked(index: number) {

            if (!this.crud.read) {
                return;
            }

            this.isBusy = true;
            this.clearMessage();

            this.add = false;
            this.edit = false;
            this.view = true;


            let result = await this.Service.getReleaseTrainingDetail(this.listReleaseTraining.listTraining[index].trainingId);

            this.formModel = {
                batch: result.batch,
                course: result.course,
                endDate: new Date(result.endDate),
                isAccommodate: result.isAccommodate,
                isParticipantTrainee: result.isParticipantTrainee,
                isParticipantPermanent: result.isParticipantPermanent,
                listOutlet: result.listOutlet,
                listPosition: result.listPosition,
                listPositionOnlyView: result.listPositionOnlyView,
                listSetupModule: result.listSetupModule,
                location: result.location,
                quota: result.quota,
                startDate: new Date(result.startDate),
                trainingId: result.trainingId,
                isCertificate: result.isCertificate,
                enumCertificate: result.enumCertificate,
                enumCertificationTtrigger: result.enumCertificationTrigger,
                listCertifications: result.trainingCertificateView.map(v => ({
                    courseId: {
                        trainingId: v.relatedTrainingId,
                        courseName: v.relatedTrainingName
                    }
                }))
            }

            this.totalDetail = await this.Service.getTotalDetailCourse(this.formModel.course.courseId);

            this.selectedArea = result.listArea;
            this.listDealer = this.selectedDealer = result.listDealer;
            this.listProvince = this.selectedProvince = result.listProvince;
            this.listCity = this.selectedCity = result.listCity;

            this.selectedPosition = result.listPosition;

            this.resetDate();

            for (var indexTime = 0; indexTime < this.formModel.listSetupModule.length; indexTime++) {
                let startDate = this.formModel.listSetupModule[indexTime].trainingStart != null ? new Date(this.formModel.listSetupModule[indexTime].trainingStart) : null;
                let endDate = this.formModel.listSetupModule[indexTime].trainingEnd != null ? new Date(this.formModel.listSetupModule[indexTime].trainingEnd) : null;

                this.formModel.listSetupModule[indexTime].trainingEnd = endDate;
                this.formModel.listSetupModule[indexTime].trainingStart = startDate;

                let checkStartDate = this.getStringDate(startDate);
                let checkEndDate = this.getStringDate(endDate);

                if (checkStartDate === '-') {
                    this.startDateForm.push('');
                }
                else {
                    let startTime = (startDate.getHours() < 10 ? "0" + startDate.getHours() : startDate.getHours()) + ":" + (startDate.getMinutes() < 10 ? "0" + startDate.getMinutes() : startDate.getMinutes());
                    this.startDateForm.push(startTime);
                }

                if (checkEndDate === '-') {
                    this.startDateForm.push('');
                }
                else {
                    let endTime = (endDate.getHours() < 10 ? "0" + endDate.getHours() : endDate.getHours()) + ":" + (endDate.getMinutes() < 10 ? "0" + endDate.getMinutes() : endDate.getMinutes());
                    this.endDateForm.push(endTime);
                }
            }

            this.isBusy = false;
        }

        async fetch() {
            this.isBusy = true;

            this.listReleaseTraining = await this.Service.getReleaseTraining(this.filter.courseName, this.filter.batch, this.filter.approvalStatusId, this.filter.dateFilter.start, this.filter.dateFilter.end, this.filter.enrollmentStartDate, this.filter.enrollmentEndDate, this.filter.sortBy, this.currentPage);

            this.listTeachingPoints = await this.Service.getTeachingTimePoints();

            this.isBusy = false;

        }

        async resetFilter() {
            this.filter = {
                dateFilter: {
                    end: null,
                    start: null
                },
                approvalStatusId: null,
                batch: null,
                courseName: '',
                enrollmentEndDate: null,
                enrollmentStartDate: null,
                sortBy: ''
            };

            this.courseName.reset();
            this.batch.reset();
            this.startDate.reset();
            this.endDate.reset();
            this.approvalStatus.reset();
            this.createdDate.reset();
            this.updatedDate.reset();

            this.fetch();
        }

        onlyUnique(value, index, self) {
            return self.indexOf(value) === index;
        }

        async editListGroup() {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();
            var provinceIds = this.getSelectedProvince();
            var cityIds = this.getSelectedCity();

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
                var findDealerIds = this.findDealer(findableOutlet);
                var distinctDealerIds = findDealerIds.filter(this.onlyUnique);
                var showDealer = this.listDealer.filter(Q => distinctDealerIds.includes(Q.dealerId));
                this.listDealerGroup[0].ListOption = showDealer;
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
                var findProvinceIds = this.findProvince(findableOutlet);
                var distinctProvinceIds = findProvinceIds.filter(this.onlyUnique);
                var showProvince = this.listProvince.filter(Q => distinctProvinceIds.includes(Q.provinceId));
                this.listProvinceGroup[0].ListOption = showProvince;
            }

            if (provinceIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => provinceIds.includes(Q.provinceId));
                var findCityIds = this.findCities(findableOutlet);
                var distinctCityIds = findCityIds.filter(this.onlyUnique);
                var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
                this.listCityGroup[0].ListOption = showCity;
            }

            if (cityIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => cityIds.includes(Q.cityId));
            }

            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
        }


        getSelectedArea() {
            var areaIds = this.selectedArea.map(function (Q) {
                return Q.areaId;
            });

            return areaIds;
        }

        getSelectedDealer() {
            var dealerIds = this.selectedDealer.map(function (Q) {
                return Q.dealerId;
            });

            return dealerIds;
        }

        getSelectedProvince() {
            var provinceIds = this.selectedProvince.map(function (Q) {
                return Q.provinceId;
            });

            return provinceIds;
        }

        getSelectedCity() {
            var cityIds = this.selectedCity.map(function (Q) {
                return Q.cityId
            });

            return cityIds;
        }

        findDealer(findableOutlet) {
            var dealerIds = findableOutlet.map(function (Q) {
                return Q.dealerId;
            });

            return dealerIds
        }

        findProvince(findableOutlet) {
            var provinceIds = findableOutlet.map(function (Q) {
                return Q.provinceId;
            });

            return provinceIds;
        }

        findCities(findableOutlet) {
            var cityIds = findableOutlet.map(function (Q) {
                return Q.cityId;
            });

            return cityIds;
        }

        async areaChanged(value) {
            var areaIds = this.getSelectedArea();
            var inserted;
            if (value != null) {
                if (value.areaId) {
                    inserted = value.areaId;
                    areaIds.push(inserted);
                }
                else {
                    value.forEach(Q => areaIds.push(Q.areaId));
                }
            }
            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = this.listCompletedOutlet.filter(Q => areaIds.includes(Q.areaId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var dealerIds = this.findDealer(findableOutlet);
            var distinctDealerIds = dealerIds.filter(this.onlyUnique);
            var showDealer = this.listDealer.filter(Q => distinctDealerIds.includes(Q.dealerId));
            this.listDealerGroup[0].ListOption = showDealer;

            var provinceIds = this.findProvince(findableOutlet);
            var distinctProvinceIds = provinceIds.filter(this.onlyUnique);
            var showProvince = this.listProvince.filter(Q => distinctProvinceIds.includes(Q.provinceId));
            this.listProvinceGroup[0].ListOption = showProvince;

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
        }

        async areaRemoved(value) {
            var areaIds = this.getSelectedArea();

            var index = areaIds.findIndex(Q => Q === value.areaId);
            if (index == -1) {
                areaIds = [];
            }
            else {
                areaIds.splice(index, 1);
            }

            var findableOutlet = this.listCompletedOutlet;

            if (areaIds.length == 0) {
                this.listOutletCompletedGroup[0].ListOption = this.listCompletedOutlet;
                this.selectedOutletCompleted = [];
                this.listDealerGroup[0].ListOption = this.listDealer;
                this.selectedDealer = [];
                this.listProvinceGroup[0].ListOption = this.listProvince;
                this.selectedProvince = [];
                this.listCityGroup[0].ListOption = this.listCity;
                this.selectedCity = [];
            }
            else {
                if (areaIds.length != 0) {
                    findableOutlet = this.listCompletedOutlet.filter(Q => areaIds.includes(Q.areaId));
                }

                this.listOutletCompletedGroup[0].ListOption = findableOutlet;
                this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

                var dealerIds = this.findDealer(findableOutlet);
                var distinctDealerIds = dealerIds.filter(this.onlyUnique);
                var showDealer = this.listDealer.filter(Q => distinctDealerIds.includes(Q.dealerId));
                this.listDealerGroup[0].ListOption = showDealer;
                this.selectedDealer = this.selectedDealer.filter((el) => this.listDealerGroup[0].ListOption.includes(el));

                var provinceIds = this.findProvince(findableOutlet);
                var distinctProvinceIds = provinceIds.filter(this.onlyUnique);
                var showProvince = this.listProvince.filter(Q => distinctProvinceIds.includes(Q.provinceId));
                this.listProvinceGroup[0].ListOption = showProvince;
                this.selectedProvince = this.selectedProvince.filter((el) => this.listProvinceGroup[0].ListOption.includes(el));

                var cityIds = this.findCities(findableOutlet);
                var distinctCityIds = cityIds.filter(this.onlyUnique);
                var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
                this.listCityGroup[0].ListOption = showCity;
                this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
            }
        }

        async dealerChanged(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();

            if (value.dealerId) {
                dealerIds.push(value.dealerId);
            }
            else {
                value.forEach(Q => dealerIds.push(Q.dealerId));
            }
            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var provinceIds = this.findProvince(findableOutlet);
            var distinctProvinceIds = provinceIds.filter(this.onlyUnique);
            var showProvince = this.listProvince.filter(Q => distinctProvinceIds.includes(Q.provinceId));
            this.listProvinceGroup[0].ListOption = showProvince;
            this.selectedProvince = this.selectedProvince.filter((el) => this.listProvinceGroup[0].ListOption.includes(el));

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
            this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
        }

        async dealerRemoved(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();

            var index = dealerIds.findIndex(Q => Q === value.dealerId);
            if (index == -1) {
                dealerIds = [];
            }
            else {
                dealerIds.splice(index, 1);
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var provinceIds = this.findProvince(findableOutlet);
            var distinctProvinceIds = provinceIds.filter(this.onlyUnique);
            var showProvince = this.listProvince.filter(Q => distinctProvinceIds.includes(Q.provinceId));
            this.listProvinceGroup[0].ListOption = showProvince;
            this.selectedProvince = this.selectedProvince.filter((el) => this.listProvinceGroup[0].ListOption.includes(el));

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
            this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
        }

        async provinceChanged(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();
            var provinceIds = this.getSelectedProvince();

            if (value.provinceId) {
                provinceIds.push(value.provinceId);
            }
            else {
                value.forEach(Q => provinceIds.push(Q.provinceId));
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }
            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }
            if (provinceIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => provinceIds.includes(Q.provinceId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
            this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
        }

        async provinceRemoved(value) {
            var areaIds = this.getSelectedArea();

            var dealerIds = this.getSelectedDealer();

            var provinceIds = this.getSelectedProvince();

            var index = provinceIds.findIndex(Q => Q === value.provinceId);
            if (index == -1) {
                provinceIds = [];
            }
            else {
                provinceIds.splice(index, 1);
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }

            if (provinceIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => provinceIds.includes(Q.provinceId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
            this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
        }

        async cityChanged(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();
            var provinceIds = this.getSelectedProvince();
            var cityIds = this.getSelectedCity();

            if (value.cityId) {
                cityIds.push(value.cityId);
            }
            else {
                value.forEach(Q => cityIds.push(Q.cityId));
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }

            if (provinceIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => provinceIds.includes(Q.provinceId));
            }

            if (cityIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => cityIds.includes(Q.cityId));
            }

            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));
        }

        async cityRemoved(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();
            var provinceIds = this.getSelectedProvince();
            var cityIds = this.getSelectedCity();

            var index = cityIds.findIndex(Q => Q === value.cityId);
            if (index == -1) {
                cityIds = [];
            }
            else {
                cityIds.splice(index, 1);
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }

            if (provinceIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => provinceIds.includes(Q.provinceId));
            }

            if (cityIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => cityIds.includes(Q.cityId));
            }

            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));
        }

        async findSetup() {
            this.resetDate();
            if (this.formModel.course) {
                this.formModel.batch = await this.Service.getBatch(this.formModel.course.courseId);
                this.listSetupModule = await this.Service.getSetupModuleRelase(this.formModel.course.courseId);

                this.formModel.listSetupModule = [];

                for (let module of this.listSetupModule) {
                    this.moduleTemplate = {
                        setupModuleId: module.setupModuleId,
                        moduleName: module.moduleName,
                        trainingTypesName: module.trainingTypesName,
                    }
                    this.formModel.listSetupModule.push(this.moduleTemplate);
                    this.startDateForm.push('');
                    this.endDateForm.push('');
                }

                this.totalDetail = await this.Service.getTotalDetailCourse(this.formModel.course.courseId);
            }
        }

        removeClicked(index: number) {

            if (!this.crud.delete) {
                return;
            }

            this.clearMessage();
            this.indexToDelete = index;
        }

        clearMessage() {
            this.errorMessage = '';
            this.successMessage = '';
        }

        async deleteConfirmed() {
            if (!this.crud.delete) {
                return;
            }

            this.isBusy = true;

            try {
                let result = await this.Service.deleteReleaseTraining(this.listReleaseTraining.listTraining[this.indexToDelete].trainingId);

                if (result === true) {
                    this.successMessage = Message.RemoveMessage;
                    this.fetch();
                }
            }
            catch{
                this.errorMessage = "Failed to Remove Data!";
            }

            this.isBusy = false;
        }

        async AutoCompleteCourse(query) {
            if (query == null || query === '') {
                // this.listCourse = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetCourse(query);
                }, 500
            );
        }


        async AutoCompleteCourseTraining(query) {
            if (query == null || query === '') {
                // this.listCourse = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                async () => {
                    this.trainings = await (await this.Service.getRelatedReleaseTraining(query, undefined, undefined, undefined, undefined, undefined, undefined, undefined, 1)).listTraining.map(v => ({...v, courseName: `${v.courseName} (Batch-${v.batch})`}));
                }, 500
            );
        }

        async GetCourse(query) {
            if (query === '' || query == null) {
                // this.listCourse = [];
                return;
            }

            this.listCourse = await this.Service.getCourseSetupLearning(query);
            this.isLoading = false;
        }

        async AutoCompleteCoach(query) {
            if (query == null || query === '') {
                this.listCoach = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetCoach(query);
                }, 500
            );
        }

        async GetCoach(query) {
            if (query === '' || query == null) {
                this.listCoach = [];
                return;
            }

            this.listCoach = await this.Service.getCoachForReleaseTraining(query);

            this.isLoading = false;
        }


        reset() {
            // this.listCourse = [];
            this.formModel.course = null;
            this.formModel.listSetupModule = [];
            this.formModel.location = null;
            this.formModel.quota = null;
            this.$validator.errors.clear();
        }

        resetCoach() {
            this.listCoach = [];
        }

        backPage() {
            window.history.back();
        }

        //Variable untuk sorting
        courseName = new Sort();
        batch = new Sort();
        startDate = new Sort();
        endDate = new Sort();
        approvalStatus = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortAsssessment
        async ClickSortCourseName() {
            this.ResetSort('courseName');
            //Sorting
            this.courseName.sorting();
            //Function Sorting
            if (this.courseName.sortup === true) {
                this.filter.sortBy = "courseName";
            }
            else if (this.courseName.sortdown === true) {
                this.filter.sortBy = "courseNameDesc";
            }
            else {
                this.filter.sortBy = "";
            }

            this.fetch();

            return;
        }

        //ClickEmployeeName
        async ClickSortBatch() {
            this.ResetSort('batch');
            //Sorting
            this.batch.sorting();
            //Function Sorting

            if (this.batch.sortup === true) {
                this.filter.sortBy = "batch";
            }
            else if (this.batch.sortdown === true) {
                this.filter.sortBy = "batchDesc";
            }
            else {
                this.filter.sortBy = "";
            }

            this.fetch();

            return;
        }

        //ClickSortPosition
        async ClickSortStartDate() {
            this.ResetSort('startDate');
            //Sorting
            this.startDate.sorting();
            //Function Sorting
            if (this.startDate.sortup === true) {
                this.filter.sortBy = "startDate";
            }
            else if (this.startDate.sortdown === true) {
                this.filter.sortBy = "startDateDesc";
            }
            else {
                this.filter.sortBy = "";
            }

            this.fetch();
            return;
        }

        //ClickSortDealer
        async ClickSortEndDate() {
            this.ResetSort('endDate');
            //Sorting
            this.endDate.sorting();
            //Function Sorting

            if (this.endDate.sortup === true) {
                this.filter.sortBy = "endDate";
            }
            else if (this.endDate.sortdown === true) {
                this.filter.sortBy = "endDateDesc";
            }
            else {
                this.filter.sortBy = "";
            }

            this.fetch();
            return;
        }

        //ClickSortOutlert
        async ClickSortApprovalStatus() {
            this.ResetSort('approvalStatus');
            //Sorting
            this.approvalStatus.sorting();
            //Function Sorting
            if (this.approvalStatus.sortup === true) {
                this.filter.sortBy = "approvalStatus";
            }
            else if (this.approvalStatus.sortdown === true) {
                this.filter.sortBy = "approvalStatusDesc";
            }
            else {
                this.filter.sortBy = "";
            }

            this.fetch();

            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting
            if (this.createdDate.sortup === true) {
                this.filter.sortBy = "createdDate";
            }
            else if (this.createdDate.sortdown === true) {
                this.filter.sortBy = "createdDateDesc";
            }
            else {
                this.filter.sortBy = "";
            }

            this.fetch();
            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting

            if (this.updatedDate.sortup === true) {
                this.filter.sortBy = "updatedDate";
            }
            else if (this.updatedDate.sortdown === true) {
                this.filter.sortBy = "updatedDateDesc";
            }
            else {
                this.filter.sortBy = "";
            }

            this.fetch();

            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'courseName':
                    this.batch.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'batch':
                    this.courseName.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'startDate':
                    this.courseName.reset();
                    this.batch.reset();
                    this.endDate.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'endDate':
                    this.courseName.reset();
                    this.batch.reset();
                    this.startDate.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'approvalStatus':
                    this.courseName.reset();
                    this.batch.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.courseName.reset();
                    this.batch.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.approvalStatus.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.courseName.reset();
                    this.batch.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    return;
            }
        }
    }

    interface OutletFilterModel {
        Area: AreaViewModel[],
        Province: ProvinceViewModel[],
        City: CityViewModel[],
        Dealer: DealerViewModel[]
    }

    interface ListGrouping {
        ListOption: any[],
        GroupName: string
    }

</script>