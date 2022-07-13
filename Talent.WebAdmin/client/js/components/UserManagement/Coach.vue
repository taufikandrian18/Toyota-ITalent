<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="alert alert-danger" v-if="errorMessage">
            {{errorMessage}}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <div class="alert alert-success" v-if="successMessage">
            {{successMessage}}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3><fa icon="user"></fa> User Management > <span class="talent_font_red">Coach</span></h3>
                <br />

                <div v-if="add == false && edit == false">
                    <!--ADVANCE SEARCH-->
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <h4>Search Coach</h4>
                        <br />

                        <!--3 Column Menu-->
                        <div class="row">
                            <div class="col-md-4">
                                <div class="row align-items-center">
                                    <div class="col-md-3">
                                        <span>Date</span>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="input-group talent_front">
                                            <v-date-picker class="v-date-style-report" v-model="coachFilter.DateFilter" mode="range" :firstDayOfWeek="2" :value="null" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                        <span>Coach Schedule</span>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="input-group talent_front">
                                            <v-date-picker class="v-date-style-report" v-model="coachFilter.CoachSchedule" mode="range" :firstDayOfWeek="2" :value="null" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                        <span>Topic</span>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="input-group">
                                            <input class="form-control" v-model="coachFilter.TopicName" />
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
                                        <span>Coach Name</span>
                                    </div>
                                    <div class="col-md-9">
                                        <div class="input-group">
                                            <input class="form-control" v-model="coachFilter.CoachName" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="row align-items-center">
                                    <div class="col-md-3">
                                        <span>E-Badge</span>
                                    </div>
                                    <div class="col-md-9">
                                        <select class="form-control" v-model="coachFilter.EbadgeName">
                                            <option value="Bronze">Bronze</option>
                                            <option value="Silver">Silver</option>
                                            <option value="Gold">Gold</option>
                                        </select>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="row align-items-center">
                                    <div class="col-md-3">
                                        <span>Status</span>
                                    </div>
                                    <div class="col-md-9">
                                        <select class="form-control" v-model="coachFilter.CoachIsActive">
                                            <option value="true">Active</option>
                                            <option value="false">Inactive</option>
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
                        <h4>Coach List</h4>
                        <div class="col-md-12 talent_magin_small">
                            <div class="row align-items-center row justify-content-between">
                                <div v-if="listCoach.listCoaches !== undefined">
                                    <a>Showing {{listCoach.listCoaches.length}} of {{listCoach.totalData}} Entry(s)</a>
                                </div>
                                <button class="btn talent_bg_cyan talent_roundborder talent_font_white" v-if="crud.create" @click.prevent="addClicked()">Add Coach</button>
                            </div>
                        </div>
                        <div class="talent_overflowx">

                            <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                                <thead>
                                    <tr>
                                        <th scope="col">
                                            <a href="#" class="talent_sort" @click.prevent="ClickSortCoachName()">Coach Name <fa icon="sort" v-if="coachName.sort == true"></fa><fa icon="sort-up" v-if="coachName.sortup == true"></fa><fa icon="sort-down" v-if="coachName.sortdown == true"></fa></a>
                                        </th>
                                        <th scope="col">
                                            <a href="#" class="talent_sort">Category</a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" @click.prevent="ClickSortCoachSchedule()">Coach Schedule <fa icon="sort" v-if="coachSchedule.sort == true"></fa><fa icon="sort-up" v-if="coachSchedule.sortup == true"></fa><fa icon="sort-down" v-if="coachSchedule.sortdown == true"></fa></a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" @click.prevent="ClickSortEBadge()">E-badge <fa icon="sort" v-if="eBadge.sort == true"></fa><fa icon="sort-up" v-if="eBadge.sortup == true"></fa><fa icon="sort-down" v-if="eBadge.sortdown == true"></fa></a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" @click.prevent="ClickSortTopic()">Topic <fa icon="sort" v-if="topic.sort == true"></fa><fa icon="sort-up" v-if="topic.sortup == true"></fa><fa icon="sort-down" v-if="topic.sortdown == true"></fa></a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" @click.prevent="ClickSortStatus()">Status <fa icon="sort" v-if="status.sort == true"></fa><fa icon="sort-up" v-if="status.sortup == true"></fa><fa icon="sort-down" v-if="status.sortdown == true"></fa></a>
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
                                    <tr v-for="(coach,index) in listCoach.listCoaches">
                                        <td>
                                            {{coach.coachName}}
                                        </td>
                                        <td>
                                            {{coach.category}}
                                        </td>
                                        <td>
                                            {{coach.startDateTime}} - {{coach.endDateTime}}
                                        </td>
                                        <td>
                                            {{coach.ebadgeName}}
                                        </td>
                                        <td>
                                            {{coach.topicName}}
                                        </td>
                                        <td v-if="coach.coachIsActive">
                                            Active
                                        </td>
                                        <td v-else>
                                            Inactive
                                        </td>
                                        <td>
                                            {{coach.createdAt}}
                                        </td>
                                        <td>
                                            {{coach.updatedAt}}
                                        </td>
                                        <td v-if="crud.read" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_orange talent_font_white" @click="detailClicked(index)">View Detail</button>
                                        </td>
                                        <td v-if="crud.update" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_cyan talent_font_white" @click="editClicked(index)">Edit</button>
                                        </td>
                                        <td v-if="crud.delete" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_red talent_font_white" @click="RemoveClicked(index)" data-toggle="modal" data-target="#exampleModalCenter" data-backdrop="static">Remove</button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <div class="col-md-12 d-flex justify-content-center">
                            <paginate :currentPage.sync="currentPage" :total-data=listCoach.totalData :limit-data=pageSize @update:currentPage="fetch()"></paginate>
                        </div>

                    </div>
                    <br />
                </div>
            </div>
        </div>

        <!--Add-->
        <div class="row" v-if="add === true">
            <div class="col col-md-12">
                <h3><fa icon="user"></fa> User Management > Coach > <span class="talent_font_red">Add</span></h3>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h5>Coach Information</h5>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Category<span class="talent_font_red">*</span></label>
                                            <div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachForm.category" type="radio" id="tam-add" name="category-add" value="tam">
                                                    <label class="form-check-label" for="tam-add">TAM</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachForm.category" type="radio" id="dealer-add" name="category-add" value="dealer">
                                                    <label class="form-check-label" for="dealer-add">Dealer</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Coach Name<span class="talent_font_red">*</span></label>

                                            <multiselect v-model="employeeChoosen"
                                                         id="ajax"
                                                         track-by="employeeName"
                                                         placeholder="Select Employee Name"
                                                         label="employeeName"
                                                         :options="listName"
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
                                    <br />
                                    <div class="row" v-if="coachForm.category != 'tam'">
                                        <div class="col-md-12">
                                            <label>Category Detail<span class="talent_font_red">*</span></label>
                                            <div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachForm.categoryDetail" type="radio" id="dealer-detail-add" name="category-detail-add" value="dealer">
                                                    <label class="form-check-label" for="dealer-detail-add">Dealer</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachForm.categoryDetail" type="radio" id="outlet-detail-add" name="category-detail-add" value="outlet">
                                                    <label class="form-check-label" for="outlet-detail-add">Outlet</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Status<span class="talent_font_red">*</span></label>
                                            <div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachForm.coachIsActive" type="radio" id="active" name="test" value="true">
                                                    <label class="form-check-label" for="active">Active</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachForm.coachIsActive" type="radio" id="inactive" name="test" value="false">
                                                    <label class="form-check-label" for="inactive">Inactive</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 ">
                                    <div class="row justify-content-between">
                                        <div class="col-md-8">
                                            <label>Topic<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-3">
                                            <label>E-Badge<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-1">

                                        </div>
                                    </div>
                                    <div v-for="(topic,index) in topics">
                                        <div class="row justify-content-between">
                                            <div class="col-md-8">
                                                <select class="form-control" v-model="topic.selectedValue">
                                                    <option v-for="(topicEbadge,index2) in listTopicsAndEbadge" :value="topicEbadge">{{topicEbadge.topicName}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <input class="form-control" disabled :placeholder="topics[index].selectedValue.ebadgeName" />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <button v-if="totalTopics == index + 1" class="button_text_no_background" @click.prevent="AddCoach(index)">
                                                    <fa style="font-size:30px" icon="plus-circle"></fa>
                                                </button>
                                                <button v-else class="button_text_no_background" @click.prevent="DeleteTopic(index)">
                                                    <fa style="font-size:30px" icon="trash-alt"></fa>
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
                            <h5>Schedule</h5>
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-4">
                                            <label>Date</label>
                                        </div>
                                        <div class="col-md-3">
                                            <label>Time</label>
                                        </div>
                                        <div class="col-md-1">

                                        </div>
                                        <div class="col-md-3">

                                        </div>
                                        <div class="col-md-1">

                                        </div>
                                    </div>
                                    <div v-for="(schedule ,index) in schedules">
                                        <div class="row justify-content-between">
                                            <div class="col-md-4">
                                                <div class="input-group talent_front">
                                                    <v-date-picker class="v-date-style-report" :masks="{ input: 'DD/MM/YYYY' }" v-model="schedule.selectedDate.endDateTimeDate" :disabled-dates='listExcludeDate' @input="excludeDate(schedule.selectedDate.endDateTimeDate,index)" mode="single" :firstDayOfWeek="2" :value="null" :input-props="props"></v-date-picker>
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <!--<div class="input-group">
                                                    <input class="form-control" placeholder="FROM" />
                                                </div>-->
                                                <vue-timepicker input-width="6em" v-model="schedule.startHour" format="HH:mm" hide-clear-button close-on-complete>
                                                </vue-timepicker>
                                            </div>
                                            <div class="col-md-1">
                                                <button class="button_text_no_background" disabled>
                                                    <fa style="font-size:30px" icon="arrow-right"></fa>
                                                </button>
                                            </div>
                                            <div class="col-md-3">
                                                <vue-timepicker input-width="6em" v-model="schedule.endHour" format="HH:mm" hide-clear-button close-on-complete>
                                                </vue-timepicker>
                                            </div>
                                            <div class="col-md-1">
                                                <button v-if="index + 1 == totalSchedule" class="button_text_no_background" @click.prevent="AddSchedule()">
                                                    <fa style="font-size:30px" icon="plus-circle"></fa>
                                                </button>
                                                <button v-else class="button_text_no_background" @click.prevent="DeleteSchedule(index)">
                                                    <fa style="font-size:30px" icon="trash-alt"></fa>
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
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="insertCoach">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
        </div>

        <!--Edit-->
        <div class="row" v-if="edit === true">
            <h3><fa icon="user"></fa> User Management > Coach > <span class="talent_font_red">Edit</span></h3>

            <div class="col col-md-12">
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h5>Coach Information</h5>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Category Detail<span class="talent_font_red">*</span></label>
                                            <div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachFormUpdate.category" type="radio" id="tam" name="category" value="tam">
                                                    <label class="form-check-label" for="tam">TAM</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachFormUpdate.category" type="radio" id="delaer" name="category" value="dealer">
                                                    <label class="form-check-label" for="delaer">Dealer</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Coach Name<span class="talent_font_red">*</span></label>

                                            <multiselect v-model="employeeChoosen"
                                                         id="ajax"
                                                         track-by="employeeName"
                                                         placeholder="Select Employee Name"
                                                         label="employeeName"
                                                         :options="listNameUpdate"
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
                                    <br />
                                    <div class="row" v-if="coachFormUpdate.category != 'tam'">
                                        <div class="col-md-12">
                                            <label>Category Detail<span class="talent_font_red">*</span></label>
                                            <div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachFormUpdate.categoryDetail" type="radio" id="delaer-detail" name="categoryDetail" value="dealer">
                                                    <label class="form-check-label" for="delaer-detail">Dealer</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachFormUpdate.categoryDetail" type="radio" id="outlet-detail" name="categoryDetail" value="outlet">
                                                    <label class="form-check-label" for="outlet-detail">Outlet</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Status<span class="talent_font_red">*</span></label>
                                            <div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachFormUpdate.coachIsActive" type="radio" id="active" name="test" :value="true">
                                                    <label class="form-check-label" for="active">Active</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="coachFormUpdate.coachIsActive" type="radio" id="inactive" name="test" :value="false">
                                                    <label class="form-check-label" for="inactive">Inactive</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 ">
                                    <div class="row justify-content-between">
                                        <div class="col-md-8">
                                            <label>Topic<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-3">
                                            <label>E-Badge<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-1">

                                        </div>
                                    </div>
                                    <div v-for="(topic,index) in topics">
                                        <div class="row justify-content-between">
                                            <div class="col-md-8">
                                                <select class="form-control" v-model="topic.selectedValue">
                                                    <option v-for="(topicEbadge,index2) in listTopicsAndEbadge" :value="topicEbadge">{{topicEbadge.topicName}}</option>
                                                </select>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <input class="form-control" disabled :placeholder="topics[index].selectedValue.ebadgeName" />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <button v-if="totalTopics == index + 1" class="button_text_no_background" @click.prevent="AddCoach(index)">
                                                    <fa style="font-size:30px" icon="plus-circle"></fa>
                                                </button>
                                                <button v-else class="button_text_no_background" @click.prevent="DeleteTopic(index)">
                                                    <fa style="font-size:30px" icon="trash-alt"></fa>
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
                            <h5>Schedule</h5>
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-4">
                                            <label>Date</label>
                                        </div>
                                        <div class="col-md-3">
                                            <label>Time</label>
                                        </div>
                                        <div class="col-md-1">

                                        </div>
                                        <div class="col-md-3">

                                        </div>
                                        <div class="col-md-1">

                                        </div>
                                    </div>
                                    <div v-for="(schedule ,index) in schedules">
                                        <div class="row justify-content-between">
                                            <div class="col-md-4">
                                                <div class="input-group talent_front">
                                                    <v-date-picker class="v-date-style-report" :masks="{ input: 'DD/MM/YYYY' }" v-model="schedule.selectedDate.endDateTimeDate" :disabled-dates='listExcludeDate' @input="excludeDate(schedule.selectedDate.endDateTimeDate,index)" mode="single" :firstDayOfWeek="2" :value="null" :input-props="props"></v-date-picker>
                                                    <div class="input-group-append">
                                                        <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <vue-timepicker input-width="6em" v-model="schedule.startHour" format="HH:mm" hide-clear-button close-on-complete>
                                                </vue-timepicker>
                                            </div>
                                            <div class="col-md-1">
                                                <button class="button_text_no_background" disabled>
                                                    <fa style="font-size:30px" icon="arrow-right"></fa>
                                                </button>
                                            </div>
                                            <div class="col-md-3">
                                                <vue-timepicker input-width="6em" v-model="schedule.endHour" format="HH:mm" hide-clear-button close-on-complete>
                                                </vue-timepicker>
                                            </div>
                                            <div class="col-md-1">
                                                <button v-if="index + 1 == totalSchedule" class="button_text_no_background" @click.prevent="AddSchedule()">
                                                    <fa style="font-size:30px" icon="plus-circle"></fa>
                                                </button>
                                                <button v-else class="button_text_no_background" @click.prevent="DeleteSchedule(index)">
                                                    <fa style="font-size:30px" icon="trash-alt"></fa>
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
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="updateClicked()">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
        </div>

        <!--Detail-->
        <div class="row" v-if="detail === true">
            <h3 v-if="add == false && edit == true"><fa icon="user"></fa> User Management > Coach > <span class="talent_font_red">Detail</span></h3>

            <div class="col col-md-12">
                <h3><fa icon="user"></fa>User Management > Coach > <span class="talent_font_red">View Detail</span></h3>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h5>Coach Information</h5>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Coach Name<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <input class="form-control" disabled v-model="listCoach.listCoaches[detailIndex].coachName">
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Category<span class="talent_font_red">*</span></label>
                                            <div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="listCoach.listCoaches[detailIndex].category" type="radio" id="tam-detail" name="category-detail" value="tam" disabled>
                                                    <label class="form-check-label" for="tam-detail">TAM</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="listCoach.listCoaches[detailIndex].category" type="radio" id="dealer-detail" name="category-detail" value="dealer" disabled>
                                                    <label class="form-check-label" for="dealer-detail">Dealer</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="listCoach.listCoaches[detailIndex].category" type="radio" id="outlet-detail" name="category-detail" value="outlet" disabled>
                                                    <label class="form-check-label" for="outlet-detail">Outlet</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Status<span class="talent_font_red">*</span></label>
                                            <div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="listCoach.listCoaches[detailIndex].coachIsActive" type="radio" id="active" name="test" value="true" disabled>
                                                    <label class="form-check-label" for="active">Active</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" v-model="listCoach.listCoaches[detailIndex].coachIsActive" type="radio" id="inactive" name="test" value="false" disabled>
                                                    <label class="form-check-label" for="inactive">Inactive</label>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 ">
                                    <div class="row justify-content-between">
                                        <div class="col-md-8">
                                            <label>Topic<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-3">
                                            <label>E-Badge<span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-1">

                                        </div>
                                    </div>
                                    <div v-for="(topic,index) in coachViewDetail.listTopicEbadge">
                                        <div class="row justify-content-between">
                                            <div class="col-md-8">
                                                <div class="input-group">
                                                    <input class="form-control" disabled :placeholder="topic.topicName" />
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <input class="form-control" disabled :placeholder="topic.ebadgeName" />
                                                </div>
                                            </div>
                                            <div class="col-md-1">

                                            </div>
                                        </div>
                                        <br />
                                    </div>

                                </div>
                            </div>
                            <br />
                        </div>

                        <div class="col-md-12">
                            <h5>Schedule</h5>
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-4">
                                            <label>Date</label>
                                        </div>
                                        <div class="col-md-8">
                                            <label>Time</label>
                                        </div>
                                    </div>
                                    <div v-for="(schedule ,index) in coachViewDetail.listCoachSchedule">
                                        <div class="row justify-content-between">
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <input class="form-control" :placeholder="schedule.startDate" disabled />
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <input class="form-control" :placeholder="schedule.startDateTime" disabled />
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <button class="button_text_no_background" disabled>
                                                    <fa style="font-size:30px" icon="arrow-right"></fa>
                                                </button>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <input class="form-control" :placeholder="schedule.endDateTime" disabled />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <!--<button v-if="index + 1 == totalSchedule" class="button_text_no_background" @click.prevent="AddSchedule()">
                                                    <fa style="font-size:30px" icon="plus-circle"></fa>
                                                </button>
                                                <button v-else class="button_text_no_background" @click.prevent="DeleteSchedule()">
                                                    <fa style="font-size:30px" icon="trash-alt"></fa>
                                                </button>-->
                                            </div>
                                        </div>
                                        <br />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
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
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
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

                    <div class="modal-body">
                        <div>
                            <h4>Coach</h4>
                            <checkbox v-model="deleteAllCoachMapping" @change="checkboxDeleteAll()">{{coachToBeDeleted.coachName}}</checkbox>
                        </div>
                        <div>
                            <h4>Topic</h4>
                            <checkbox v-for="topic in coachViewDetail.listTopicEbadge" :key="topic.topicId" :id="topic.topicId.toString()" :value="topic.topicId" v-model="toBeDeletedTopicIds" @change="CheckUncheckTopic">{{topic.topicName}}</checkbox>
                        </div>
                        <div>
                            <h4>Coach Schedule</h4>
                            <checkbox v-for="schedule in coachViewDetail.listCoachSchedule" :key="schedule.coachScheduleId" :id="schedule.coachScheduleId.toString()" :value="schedule.coachScheduleId" v-model="toBeDeletedScheduleIds" @change="CheckUncheckSchedule">{{schedule.startDate}} - {{schedule.startDateTime}} - {{schedule.endDateTime}}</checkbox>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="col-md-12 d-flex justify-content-around">
                                <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="DeleteConfirmed()">Delete</button>
                                <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="ClearDeleteSelect()">Close</button>
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
    import { Sort } from '../../class/Sort';
    import { CoachService, CoachesViewModel, TopicsEbadgeJoinModelForCoach, ListEmployeeForCoach, EmployeeForCoachModel, CoachesModel, CoachFormModel, CoachSchedulesFormModel, CoachViewDetail, CoachDeleteFormModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService';
    import { PageEnums } from '../../enum/PageEnums';
    import { Watch } from 'vue-property-decorator';


    @Component({
        props: ['framework', 'compiler'],
        created: async function (this: Coach) {
            await this.getAccess();
            await this.initialize();
        }
    })
    export default class Coach extends Vue {

        currentPage: number = 1;

        deleteAllCoachMapping: boolean = false;
        toBeDeletedTopicIds: number[] = [];
        toBeDeletedScheduleIds: number[] = [];

        listExcludeDate: Date[] = [];

        listName: EmployeeForCoachModel[] = [];
        listNameUpdate: EmployeeForCoachModel[] = [];
        employeeChoosen: EmployeeForCoachModel = {
            employeeId: null,
            employeeName: null
        };

        isLoading: boolean = false;
        isBusy: boolean = false;

        timeout: any = null;

        Service: CoachService = new CoachService();

        framework: string;
        compiler: string;
        looping: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        props: any = {
            placeholder: "",
            readonly: true
        };

        coachViewDetail: CoachViewDetail = {
            listCoachSchedule: [],
            listTopicEbadge: []
        }

        listCoach: CoachesViewModel = {
            listCoaches: [],
            totalData: null
        };

        listTopicsAndEbadge: TopicsEbadgeJoinModelForCoach[] = [];

        coachToBeDeleted: CoachesModel = {
            coachId: null,
            coachIsActive: null,
            coachName: null,
            coachScheduleId: null,
            createdAt: null,
            createdAtDate: null,
            ebadgeId: null,
            ebadgeName: null,
            employeeId: null,
            endDateTime: null,
            endDateTimeDate: null,
            startDateTime: null,
            startDateTimeDate: null,
            topicId: null,
            topicName: null,
            updatedAt: null,
            updatedAtDate: null
        };
        coachFormDeleted: CoachDeleteFormModel = {
            coachId: null,
            deleteAll: null,
            scheduleId: null,
            topicId: null
        };

        detailIndex: number;
        updateIndex: number;
        deleteIndex: number;

        coachForm: CoachFormModel = {
            coachId: null,
            coachIsActive: null,
            coachName: null,
            coachSchedule: null,
            createdAt: null,
            employeeId: null,
            topicId: null,
            updatedAt: null,
            category: 'tam',
            categoryDetail: 'tam'
        };
        coachFormUpdate: CoachFormModel = {
            coachId: null,
            coachIsActive: null,
            coachName: null,
            coachSchedule: null,
            createdAt: null,
            employeeId: null,
            topicId: null,
            updatedAt: null,
            category: 'tam',
            categoryDetail: 'tam'
        };

        coachFilter: ICoachesFilter = {
            CoachIsActive: null,
            CoachName: '',
            CoachSchedule: {
                end: null,
                start: null
            },
            DateFilter: {
                start: null,
                end: null
            },
            EbadgeName: '',
            PageNumber: 1,
            SortBy: '',
            TopicName: ''
        }


        topics: ISelectedTopic[] = [{
            index: 0,
            selectedValue: {
                ebadgeName: null
            }
        }];

        totalTopics: number = 1;

        pageSize: number = 10;

        schedules: ISchedule[] = [{
            index: 0,
            selectedDate: {
                endDateTime: null,
                startDateTime: null,
            },
            endHour: '',
            startHour: ''
        }];
        totalSchedule: number = 1;


        @Watch('coachForm.category')
        watchAdd(value: string, oldValue: string) {
            this.coachForm.categoryDetail = this.coachForm.category
            this.employeeChoosen = null;
        }

        @Watch('coachFormUpdate.category')
        watchEdit(value: string, oldValue: string) {
            this.coachFormUpdate.categoryDetail = this.coachFormUpdate.category
            this.employeeChoosen = null;
            console.log('called')
        }

        reset() {
            this.employeeChoosen = null;
        }

        async initialize() {
            this.coachForm = {
                coachId: null,
                coachIsActive: null,
                coachName: null,
                coachSchedule: null,
                createdAt: null,
                employeeId: null,
                topicId: null,
                updatedAt: null,
                category: 'tam',
                categoryDetail: 'tam'
            };
            this.listCoach = {
                listCoaches: [],
                totalData: null
            };
            await this.fetch();
        }

        async resetFilter() {
            this.coachFilter.CoachIsActive = null;
            this.coachFilter.CoachName = '';
            this.coachFilter.CoachSchedule = {
                end: null,
                start: null
            };
            this.coachFilter.DateFilter = {
                end: null,
                start: null
            };
            this.coachFilter.EbadgeName = '';
            this.coachFilter.SortBy = '';
            this.coachFilter.TopicName = '';
            this.currentPage = 1;

            this.coachName.reset();
            this.coachSchedule.reset();
            this.eBadge.reset();
            this.topic.reset();
            this.status.reset();
            this.createdDate.reset();
            this.updatedDate.reset();

            await this.fetch();
        }

        async fetch() {
            this.isBusy = true;

            this.listCoach = await this.Service.getAllCoachesFilter(this.coachFilter.CoachSchedule.start, this.coachFilter.CoachSchedule.end, this.coachFilter.DateFilter.start, this.coachFilter.DateFilter.end, this.coachFilter.CoachName, this.coachFilter.TopicName, this.coachFilter.CoachIsActive, this.coachFilter.EbadgeName, this.coachFilter.SortBy, this.currentPage);

            this.isBusy = false;
        }

        successMessage: string = '';
        errorMessage: string = '';

        //Variable untuk sorting
        coachName = new Sort();
        coachSchedule = new Sort();
        eBadge = new Sort();
        topic = new Sort();
        status = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //Navigation
        detail: boolean = false;
        add: boolean = false;
        edit: boolean = false;

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Coach);
            this.crud = data
        }

        excludeDate(date: Date, index: number) {
            if (date === undefined) {
                this.listExcludeDate.splice(index, 1)
            }
            else {

                if (this.listExcludeDate[index]) {
                    this.listExcludeDate[index] = date;

                }
                else {
                    this.listExcludeDate.push(date);
                }
            }
        }

        get getTotalData() {
            return this.listCoach.listCoaches.length;
        }

        getPageData() {
            return this.listCoach.listCoaches.length;
        }

        checkboxDeleteAll() {
            if (this.deleteAllCoachMapping === true) {
                for (let topic of this.coachViewDetail.listTopicEbadge) {
                    var hasExist = this.toBeDeletedTopicIds.includes(topic.topicId);

                    if (hasExist === false) {
                        this.toBeDeletedTopicIds.push(topic.topicId);
                    }
                }

                for (let schedule of this.coachViewDetail.listCoachSchedule) {
                    var hasExist = this.toBeDeletedScheduleIds.includes(schedule.coachScheduleId);

                    if (hasExist === false) {
                        this.toBeDeletedScheduleIds.push(schedule.coachScheduleId);
                    }
                }
            }
            else {
                this.toBeDeletedTopicIds = [];
                this.toBeDeletedScheduleIds = [];
            }
        }

        CheckUncheckTopic() {
            if (this.toBeDeletedTopicIds.length < this.coachViewDetail.listTopicEbadge.length) {
                this.deleteAllCoachMapping = false;
                return;
            }
            this.deleteAllCoachMapping = true;
            for (let schedule of this.coachViewDetail.listCoachSchedule) {
                var hasExist = this.toBeDeletedScheduleIds.includes(schedule.coachScheduleId);

                if (hasExist === false) {
                    this.toBeDeletedScheduleIds.push(schedule.coachScheduleId);
                }
            }
        }

        CheckUncheckSchedule() {
            if (this.deleteAllCoachMapping == true || this.toBeDeletedTopicIds.length == this.coachViewDetail.listTopicEbadge.length) {
                for (let schedule of this.coachViewDetail.listCoachSchedule) {
                    var hasExist = this.toBeDeletedScheduleIds.includes(schedule.coachScheduleId);

                    if (hasExist === false) {
                        this.toBeDeletedScheduleIds.push(schedule.coachScheduleId);
                    }
                }
            }
        }

        ClearDeleteSelect() {
            this.deleteAllCoachMapping = false;
            this.toBeDeletedScheduleIds = [];
            this.toBeDeletedTopicIds = [];
        }

        async insertCoach() {
            this.errorMessage = '';

            if (!this.crud.create) {
                return;
            }

            if (this.employeeChoosen == null) {
                this.errorMessage = 'Coach Name must be filled!';
                return;
            }
            if (this.coachForm.coachIsActive == null) {
                this.errorMessage = 'Coach Status must be choosen!'
                return;
            }
            if (!this.topics[0].selectedValue.topicId) {
                this.errorMessage = 'Topic must be choosen!';
                return;
            }

            this.coachForm.coachName = this.employeeChoosen.employeeName;
            this.coachForm.employeeId = this.employeeChoosen.employeeId;
            this.coachForm.topicId = [];

            for (var a of this.topics) {
                if (a.selectedValue.topicId) {
                    this.coachForm.topicId.push(a.selectedValue.topicId);
                }
                else {
                    this.errorMessage = "Topic can't be Empty";
                    return;
                }
            }

            var valueArr = this.coachForm.topicId.map(function (item) {
                return item;
            });

            var isDuplicate = valueArr.some(function (item, idx) {
                return valueArr.indexOf(item) != idx
            });

            if (isDuplicate == true) {
                this.errorMessage = "Topic can't be duplicate";
                return;
            }

            this.coachForm.coachSchedule = [];
            for (var b of this.schedules) {
                if (b.selectedDate.endDateTimeDate) {
                    if ((b.startHour !== null && b.startHour !== '') && (b.endHour !== null && b.endHour !== '')) {
                        let startDate = b.selectedDate.endDateTimeDate;
                        startDate = new Date(b.selectedDate.endDateTimeDate);

                        var endHour = parseInt(b.endHour.substring(0, 2));
                        var endMinute = parseInt(b.endHour.substring(3, 5));
                        var startHour = parseInt(b.startHour.substring(0, 2));
                        var startMinute = parseInt(b.startHour.substring(3, 5));

                        b.selectedDate.endDateTimeDate.setHours(endHour, endMinute);
                        startDate.setHours(startHour, startMinute);

                        let newDate: CoachSchedulesFormModel = {
                            coachScheduleId: null,
                            endDateTime: null,
                            endDateTimeDate: null,
                            startDate: null,
                            startDateTime: null,
                            startDateTimeDate: null
                        }

                        newDate.endDateTimeDate = b.selectedDate.endDateTimeDate;
                        newDate.startDateTimeDate = startDate;

                        if (newDate.endDateTimeDate < newDate.startDateTimeDate) {
                            this.errorMessage = "End Time must be bigger than Start Time";
                            return;
                        }

                        this.coachForm.coachSchedule.push(newDate);
                    }
                    else {
                        this.errorMessage = "Time must be filled";
                        return;
                    }
                }
                else if (this.totalSchedule == 1 && !b.selectedDate.endDateTimeDate) {

                }
                else {
                    this.errorMessage = "Date must be filled!";
                    return;
                }
            }

            this.isBusy = true;
            this.resetFilter();
            try {
                let result = await this.Service.insertMasterCoach({
                    ...this.coachForm,
                    category: this.coachForm.categoryDetail
                });
                if (result === false) {
                    this.errorMessage = "Failed to Insert Data! Topic can't be duplicated! or Coach already exist!";
                    return;
                }

                if (result === true) {
                    await this.fetch();
                    this.coachForm = {
                        coachId: null,
                        coachIsActive: null,
                        coachName: null,
                        coachSchedule: null,
                        createdAt: null,
                        employeeId: null,
                        topicId: null,
                        updatedAt: null,
                        category: 'tam',
                        categoryDetail: 'tam'
                    };
                    this.closeClicked();
                    this.successMessage = 'Success to Add Coach';
                }
                this.isBusy = false;
            }
            catch{
                this.errorMessage = "Failed to Insert";
                this.isBusy = false;
            }
        }


        async updateClicked() {
            this.errorMessage = '';

            if (!this.crud.update) {
                return;
            }

            if (this.employeeChoosen == null) {
                this.errorMessage = 'Coach Name must be filled!';
                return;
            }

            if (this.coachFormUpdate.coachIsActive == null) {
                this.errorMessage = 'Coach Status must be choosen!'
                return;
            }
            if (!this.topics[0].selectedValue.topicId) {
                this.errorMessage = 'Topic must be choosen!';
                return;
            }

            this.coachFormUpdate.coachName = this.employeeChoosen.employeeName;
            this.coachFormUpdate.employeeId = this.employeeChoosen.employeeId;

            this.coachFormUpdate.topicId = [];
            for (var a of this.topics) {
                if (a.selectedValue.topicId) {
                    this.coachFormUpdate.topicId.push(a.selectedValue.topicId);
                }
                else {
                    this.errorMessage = "Topic can't be Empty";
                    return;
                }
            }
            var valueArr = this.coachFormUpdate.topicId.map(function (item) {
                return item;
            });

            var isDuplicate = valueArr.some(function (item, idx) {
                return valueArr.indexOf(item) != idx
            });

            if (isDuplicate == true) {
                this.errorMessage = "Topic can't be Duplicate";
                return;
            }

            this.coachFormUpdate.coachSchedule = [];
            for (var b of this.schedules) {
                if (b.selectedDate.endDateTimeDate) {

                    if ((b.startHour !== null && b.startHour !== '') && (b.endHour !== null && b.endHour !== '')) {
                        let startDate = b.selectedDate.endDateTimeDate;
                        startDate = new Date(b.selectedDate.endDateTimeDate);

                        var endHour = parseInt(b.endHour.substring(0, 2));
                        var endMinute = parseInt(b.endHour.substring(3, 5));
                        var startHour = parseInt(b.startHour.substring(0, 2));
                        var startMinute = parseInt(b.startHour.substring(3, 5));

                        b.selectedDate.endDateTimeDate.setHours(endHour, endMinute);
                        startDate.setHours(startHour, startMinute);

                        let newDate: CoachSchedulesFormModel = {
                            coachScheduleId: b.coachScheduleId,
                            endDateTime: null,
                            endDateTimeDate: null,
                            startDate: null,
                            startDateTime: null,
                            startDateTimeDate: null
                        };

                        newDate.endDateTimeDate = b.selectedDate.endDateTimeDate;
                        newDate.startDateTimeDate = startDate;

                        if (newDate.endDateTimeDate < newDate.startDateTimeDate) {
                            this.errorMessage = "End Time must be bigger than Start Time";
                            return;
                        }

                        this.coachFormUpdate.coachSchedule.push(newDate);
                    }
                    else {
                        this.errorMessage = "Time must be filled";
                        return;
                    }
                }
                else if (this.totalSchedule == 1 && !b.selectedDate.endDateTimeDate) {

                }
                else {
                    this.errorMessage = "Date must be filled!";
                    return;
                }

            }

            let isValid = await this.Service.validateUpdateCoach(this.coachFormUpdate.coachId, this.coachFormUpdate.employeeId);

            if (isValid == false) {
                this.errorMessage = "Employee can't be duplicate";
                return;
            }
            this.isBusy = true;
            this.resetFilter();

            try {
                let result = await this.Service.updateCoachDetail({...this.coachFormUpdate, category: this.coachFormUpdate.categoryDetail});
                if (result != true) {
                    this.errorMessage = "Failed to Update";
                    return;
                }

                await this.fetch();
                this.coachFormUpdate = null;
                this.closeClicked();
                this.successMessage = 'Success to Edit Data!';
                this.isBusy = false;
            }

            catch{
                this.errorMessage = "Failed to Edit Data!";
                this.isBusy = false;
            }
        }

        getShowing() {
            return this.listCoach.listCoaches.length;
        }


        clearForm() {
            this.totalTopics = 1;
            this.totalSchedule = 1;
            this.successMessage = '';
            this.errorMessage = '';
            this.coachForm = {
                coachId: null,
                coachIsActive: null,
                coachName: null,
                coachSchedule: null,
                createdAt: null,
                employeeId: null,
                topicId: null,
                updatedAt: null,
                category: 'tam',
                categoryDetail: 'tam'
            };
            this.coachFormUpdate = {
                coachId: null,
                coachIsActive: null,
                coachName: null,
                coachSchedule: null,
                createdAt: null,
                employeeId: null,
                topicId: null,
                updatedAt: null,
                category: 'tam',
                categoryDetail: 'tam'
            };
            this.employeeChoosen = null;
            this.topics = [
                {
                    index: 0,
                    selectedValue: {
                        ebadgeName: null
                    }
                }
            ];
            this.schedules = [{
                index: 0,
                selectedDate: {

                },
                endHour: '',
                startHour: ''
            }];

            this.listName = [];
            this.listNameUpdate = [];

            this.listExcludeDate = [];
        }

        async AutoComplete(query) {
            if (query == null || query === '') {
                this.listName = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetEmployeeName(query);
                }, 500
            );
        }

        async AutoCompleteUpdate(query) {
            if (query == null || query === '') {
                this.listName = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetEmployeeNameUpdate(query);
                }, 500
            );
        }

        async GetEmployeeNameUpdate(query) {
            if (query === '' || query == null) {
                this.listNameUpdate = [];
                return;
            }

            let a = await this.Service.getAllEmployee(query, this.coachForm.categoryDetail == 'tam' ? 'true' : '', this.coachForm.categoryDetail != 'tam' ? 'true' : '');
            this.listNameUpdate = a.listEmployee;

            this.isLoading = false;
        }

        async GetEmployeeName(query) {
            if (query === '' || query == null) {
                this.listName = [];
                return;
            }

            let result = await this.Service.getAllEmployeeForAdd(query, this.coachForm.categoryDetail == 'tam' ? 'true' : '', this.coachForm.categoryDetail != 'tam' ? 'true' : '');
            this.listName = result.listEmployee;

            this.isLoading = false;
        }

        async addClicked() {

            if (!this.crud.create) {
                return;
            }

            this.clearForm();
            this.listTopicsAndEbadge = await this.Service.getAllTopics();

            this.add = true;
            this.edit = false;
            this.detail = false;
        }

        async editClicked(index: number) {

            if (!this.crud.update) {
                return;
            }

            this.clearForm();

            this.add = false;
            this.edit = true;
            this.detail = false;

            this.coachFormUpdate.coachIsActive = this.listCoach.listCoaches[index].coachIsActive;
            this.coachFormUpdate.coachId = this.listCoach.listCoaches[index].coachId;
            this.coachFormUpdate.coachName = this.listCoach.listCoaches[index].coachName;
            this.coachFormUpdate.category = this.listCoach.listCoaches[index].category == 'tam' ? 'tam' : 'dealer';
            this.listTopicsAndEbadge = await this.Service.getAllTopics();

            this.updateIndex = index;
            this.coachViewDetail = await this.Service.getCoachDetail(this.listCoach.listCoaches[index].coachId);

            this.employeeChoosen = {
                employeeId: this.listCoach.listCoaches[index].employeeId,
                employeeName: this.listCoach.listCoaches[index].coachName
            }
            
            this.coachFormUpdate.categoryDetail = this.listCoach.listCoaches[index].category;


            this.listNameUpdate.push(this.employeeChoosen);

            this.topics = [];
            for (var a of this.coachViewDetail.listTopicEbadge) {
                this.topics.push({
                    index: this.topics.length,
                    selectedValue: {
                        topicId: a.topicId,
                        ebadgeId: a.ebadgeId,
                        ebadgeName: a.ebadgeName,
                        topicName: a.topicName
                    }
                })
            }

            this.totalTopics = this.topics.length;

            this.schedules = [];

            if (this.coachViewDetail.listCoachSchedule.length == 0) {
                this.schedules = [{
                    index: 0,
                    selectedDate: {
                        endDateTime: null,
                        startDateTime: null
                    },
                    startHour: '',
                    endHour: ''
                }];
            }
            else {
                for (var b of this.coachViewDetail.listCoachSchedule) {
                    this.schedules.push({
                        index: this.schedules.length,
                        selectedDate: {
                            endDateTimeDate: new Date(b.endDateTimeDate),
                            startDateTimeDate: new Date(b.startDateTimeDate),
                            endDateTime: b.endDateTime,
                            startDateTime: b.startDateTime
                        },
                        startHour: b.startDateTime,
                        endHour: b.endDateTime,
                        coachScheduleId: b.coachScheduleId
                    })
                }
                this.totalSchedule = this.schedules.length;
            }
        }

        async RemoveClicked(index: number) {

            if (!this.crud.delete) {
                return;
            }

            this.clearForm();
            this.deleteIndex = index;
            this.coachToBeDeleted = this.listCoach.listCoaches[index];
            this.coachViewDetail = await this.Service.getCoachDetail(this.listCoach.listCoaches[index].coachId);
        }

        async detailClicked(index: number) {

            if (!this.crud.read) {
                return;
            }

            this.clearForm();

            this.add = false;
            this.edit = false;
            this.detail = true;
            this.detailIndex = index;
            this.coachViewDetail = await this.Service.getCoachDetail(this.listCoach.listCoaches[index].coachId);
        }

        closeClicked() {
            this.add = false;
            this.edit = false;
            this.detail = false;

            this.clearForm();
        }

        async AddSchedule() {
            this.schedules.push({
                index: this.schedules.length,
                selectedDate: {

                },
                endHour: '',
                startHour: ''
            })
            this.totalSchedule++;
        }

        async DeleteSchedule(indexToDelete: number) {
            this.schedules.splice(indexToDelete, 1);
            this.totalSchedule--;

            this.schedules.forEach(function (value) {
                if (value.endHour == "" || value.endHour == null) {
                    value.endHour = 'HH:mm';
                }

                if (value.startHour == "" || value.startHour == null) {
                    value.startHour = 'HH:mm';
                }
            });
        }

        async AddCoach() {
            this.topics.push({
                index: this.topics.length + 1,
                selectedValue: {

                }
            });
            this.totalTopics++;
        }

        async DeleteTopic(indexToDelete: number) {
            this.topics.splice(indexToDelete, 1);
            this.totalTopics--;
        }

        async DeleteConfirmed() {

            if (!this.crud.delete) {
                return;
            }

            this.coachFormDeleted.coachId = this.coachToBeDeleted.coachId;
            this.coachFormDeleted.deleteAll = this.deleteAllCoachMapping;
            this.coachFormDeleted.topicId = this.toBeDeletedTopicIds;
            this.coachFormDeleted.scheduleId = this.toBeDeletedScheduleIds;

            try {
                let result = await this.Service.deleteCoach(this.coachFormDeleted);

                if (result === true) {
                    this.successMessage = "Success to Remove Data!";
                }

                this.fetch();

                this.ClearDeleteSelect();
            }
            catch{
                this.errorMessage = "Failed to Delete Coach!";
            }

        }

        //ClickSortCoachName
        async ClickSortCoachName() {
            this.ResetSort('coachName');
            //Sorting
            this.coachName.sorting();
            //Function Sorting
            if (this.coachName.sortup == true) {
                this.coachFilter.SortBy = 'coachName';
            }
            else if (this.coachName.sortdown == true) {
                this.coachFilter.SortBy = 'coachNameDesc';
            }
            else {
                this.coachFilter.SortBy = '';
            }

            this.fetch();

            return;
        }

        //ClickSortCoachSchedule
        async ClickSortCoachSchedule() {
            this.ResetSort('coachSchedule');
            //Sorting
            this.coachSchedule.sorting();
            //Function Sorting

            if (this.coachSchedule.sortup == true) {
                this.coachFilter.SortBy = 'coachSchedule';
            }
            else if (this.coachSchedule.sortdown == true) {
                this.coachFilter.SortBy = 'coachScheduleDesc';
            }
            else {
                this.coachFilter.SortBy = '';
            }

            this.fetch();

            return;
        }

        //ClickSortEBadge
        async ClickSortEBadge() {
            this.ResetSort('eBadge');
            //Sorting
            this.eBadge.sorting();
            //Function Sorting

            if (this.eBadge.sortup == true) {
                this.coachFilter.SortBy = 'ebadgeName';
            }
            else if (this.eBadge.sortdown == true) {
                this.coachFilter.SortBy = 'ebadgeNameDesc';
            }
            else {
                this.coachFilter.SortBy = '';
            }

            this.fetch();

            return;
        }

        //ClickSortTopic
        async ClickSortTopic() {
            this.ResetSort('topic');
            //Sorting
            this.topic.sorting();
            //Function Sorting

            if (this.topic.sortup == true) {
                this.coachFilter.SortBy = 'topicName';
            }
            else if (this.topic.sortdown == true) {
                this.coachFilter.SortBy = 'topicNameDesc';
            }
            else {
                this.coachFilter.SortBy = '';
            }

            this.fetch();

            return;
        }

        //ClickSortStatus
        async ClickSortStatus() {
            this.ResetSort('status');
            //Sorting
            this.status.sorting();
            //Function Sorting

            if (this.status.sortup == true) {
                this.coachFilter.SortBy = 'coachIsActive';
            }
            else if (this.status.sortdown == true) {
                this.coachFilter.SortBy = 'coachIsActiveDesc';
            }
            else {
                this.coachFilter.SortBy = '';
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

            if (this.createdDate.sortup == true) {
                this.coachFilter.SortBy = 'createDate';
            }
            else if (this.createdDate.sortdown == true) {
                this.coachFilter.SortBy = 'createDateDesc';
            }
            else {
                this.coachFilter.SortBy = '';
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

            if (this.updatedDate.sortup == true) {
                this.coachFilter.SortBy = 'updatedDate';
            }
            else if (this.updatedDate.sortdown == true) {
                this.coachFilter.SortBy = 'updatedDateDesc';
            }
            else {
                this.coachFilter.SortBy = '';
            }

            this.fetch();

            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'coachName':
                    this.coachSchedule.reset();
                    this.eBadge.reset();
                    this.topic.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'coachSchedule':
                    this.coachName.reset();
                    this.eBadge.reset();
                    this.topic.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'eBadge':
                    this.coachName.reset();
                    this.coachSchedule.reset();
                    this.topic.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'topic':
                    this.coachName.reset();
                    this.coachSchedule.reset();
                    this.eBadge.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return
                case 'status':
                    this.coachName.reset();
                    this.coachSchedule.reset();
                    this.eBadge.reset();
                    this.topic.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.coachName.reset();
                    this.coachSchedule.reset();
                    this.eBadge.reset();
                    this.topic.reset();
                    this.status.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.coachName.reset();
                    this.coachSchedule.reset();
                    this.eBadge.reset();
                    this.topic.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    return;
            }

        }

        async Add() {
            this.add = true;
            this.edit = false;
        }

        async Close() {
            this.add = false;
            this.edit = false;
        }

        async Edit(/*Parameter id*/) {
            this.edit = true;
            this.add = false;
            //Function get by Id
        }

        async SaveAdd() {
            //Function Save
        }

        async SaveEdit() {
            //Function Edit
        }
    }

    interface ISelectedTopic {
        index: number;
        selectedValue?: TopicsEbadgeJoinModelForCoach;
    }

    interface ISchedule {
        index: number;
        selectedDate?: CoachSchedulesFormModel;
        startHour: string;
        endHour: string;
        coachScheduleId?: number;
    }

</script>
