<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="alert alert-success alert-dismissible fade show" role="alert" v-if="success">
            {{successMessage}}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close" @clicked="alertClose()">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > <span class="talent_font_red">Event</span></h3>
                <br />

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Event</h4>
                    <br />
                    <!--4 Column Menu-->
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" mode="range" :firstDayOfWeek="2" v-model="filterDate" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Start Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" :firstDayOfWeek="2" v-model="filterFromEventDate" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>End Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <v-date-picker class="v-date-style-report" :firstDayOfWeek="2" v-model="filterToEventDate" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Approval Status</span>
                                </div>
                                <div class="col-md-9">
                                    <select class="form-control" v-model="filterApprovalStatus">
                                        <option v-for="a in approvalStatuses.listApprovalStatusModel" :value="a.approvalName">{{a.approvalName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <!--4 Column Menu-->
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Event Title</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filterEventName" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Event Category</span>
                                </div>
                                <div class="col-md-9">
                                    <select class="form-control" v-model="filterEventCategoryName">
                                        <option v-for="ec in eventCategories">{{ec.eventCategoryName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Host</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filterHostName" />
                                    </div>
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
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="reset">
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
                    <h4>Event List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{events.listEventJoinModel == null ? 0 : events.listEventJoinModel.length}} of {{events.totalItem}} Entry(s)</a>
                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="addClicked()">Add Event</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortEventTitle()">Event Title <fa icon="sort" v-if="eventTitle.sort == true"></fa><fa icon="sort-up" v-if="eventTitle.sortup == true"></fa><fa icon="sort-down" v-if="eventTitle.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortEventCategory()">Event Category <fa icon="sort" v-if="eventCategory.sort == true"></fa><fa icon="sort-up" v-if="eventCategory.sortup == true"></fa><fa icon="sort-down" v-if="eventCategory.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortStartDate()">Start Date <fa icon="sort" v-if="startDate.sort == true"></fa><fa icon="sort-up" v-if="startDate.sortup == true"></fa><fa icon="sort-down" v-if="startDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortEndDate()">End Date <fa icon="sort" v-if="endDate.sort == true"></fa><fa icon="sort-up" v-if="endDate.sortup == true"></fa><fa icon="sort-down" v-if="endDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortHost()">Host <fa icon="sort" v-if="host.sort == true"></fa><fa icon="sort-up" v-if="host.sortup == true"></fa><fa icon="sort-down" v-if="host.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortApprovalStatus()">Approval <fa icon="sort" v-if="approvalStatus.sort == true"></fa><fa icon="sort-up" v-if="approvalStatus.sortup == true"></fa><fa icon="sort-down" v-if="approvalStatus.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="3">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(e, index) in events.listEventJoinModel">
                                    <td>
                                        {{e.eventName}}
                                    </td>
                                    <td>
                                        {{e.eventCategoryName}}
                                    </td>
                                    <td>
                                        {{convertDateTime(e.startDateTime)}}
                                    </td>
                                    <td>
                                        {{convertDateTime(e.endDateTime)}}
                                    </td>
                                    <td>
                                        {{e.eventHostName}}
                                    </td>
                                    <td>
                                        {{e.approvalStatus}}
                                    </td>
                                    <td>
                                        {{convertDateTime(e.createdAt)}}
                                    </td>
                                    <td>
                                        {{convertDateTime(e.updatedAt)}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="detailClicked(e.eventId)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="editClicked(e.eventId)" :disabled="e.approvalStatus == 'Waiting for Approval'">Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click.prevent="deleteClicked(e.eventId, index)" :disabled="e.approvalStatus == 'Waiting for Approval'">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--Pagination-->
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate :currentPage.sync="currentPage" :total-data=events.totalItem :limit-data=itemPerPage @update:currentPage="fetch()"></paginate>
                    </div>

                </div>
                <br />

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
                                        <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="deleteData()">Delete</button>
                                        <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" data-dismiss="modal">Close</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Add-->
        <div class="row" v-if="add === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Event > <span class="talent_font_red">Add Event</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Event Information</h4>

                    <div v-if="fileType === false || fileSize === false" class="alert alert-danger">
                        <div v-if="fileSize === false">Maximum File Size is 5 MB</div>
                        <div v-if="fileType === false">Please input a jpg/png/jpeg file</div>
                    </div>

                    <div v-if="validateAdd() === false" class="alert alert-danger">
                        <div v-if="addFileName === ''">File is Required</div>
                        <div v-if="addModel.eventName === ''">Event Title is Required</div>
                        <div v-if="addEventCategory.eventCategoryId == 0">Event Category is Required</div>
                        <div v-if="addStartDate === null">Start date is Required</div>
                        <div v-if="addEndDate === null">End date is Required</div>
                        <div v-if="addModel.eventHostName === ''">Host is Required</div>
                        <div v-if="addModel.location === ''">Location is Required</div>
                        <div v-if="selectedOutletCompleted === null">Outlet is Required</div>
                        <div v-if="listPosition === null">Position is Required</div>
                        <div v-if="addStartDate != '' && addEndDate != '' && addStartDate != null && addEndDate != null && addEndDate < addStartDate">End Date can't be before Start Date</div>
                    </div>

                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <label>Event Title<span class="talent_font_red">*</span></label>
                            <div class="input-group">
                                <input class="form-control" v-model="addModel.eventName" maxlength="255" />
                            </div>
                            <br />
                            <label>Event Category<span class="talent_font_red">*</span></label>
                            <select class="form-control" v-model="addEventCategory">
                                <option v-for="ec in eventCategories" :value="ec">{{ec.eventCategoryName}}</option>
                            </select>
                        </div>
                        <div class="col-md-6">
                            <label>Description</label>
                            <textarea class="form-control" style="height:150px;overflow-y:scroll;" v-model="addModel.eventDescription" maxlength="1024"></textarea>
                        </div>
                    </div>
                    <br />

                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Period<span class="talent_font_red">*</span></label>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="input-group">
                                                <v-date-picker class="v-date-style-report" :firstDayOfWeek="2" :value="null" :masks="{ input: 'DD/MM/YYYY' }" v-model="addStartDate" :max-date="addEndDate" :input-props='{placeholder: "Start Date"}'></v-date-picker>
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
                                                <v-date-picker class="v-date-style-report" :firstDayOfWeek="2" :value="null" v-model="addEndDate" :min-date="addStartDate" :masks="{ input: 'DD/MM/YYYY' }" :input-props='{placeholder: "End Date"}'></v-date-picker>
                                                <div class="input-group-append">
                                                    <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Host<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="addModel.eventHostName"maxlength="255"  />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Location<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="addModel.location" maxlength="255" />
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
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="areaChanged"
                                                 @remove="areaRemoved"
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
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="dealerChanged"
                                                 @remove="dealerRemoved"
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
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="cityChanged"
                                                 @remove="cityRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>Outlet<span class="talent_font_red">*</span></label>
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
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <label>Position<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="listPosition"
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
                        </div>
                    </div>
                    <br />

                    <h4>File Upload</h4>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Upload Event Picture<span class="talent_font_red">*</span></label>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <div class="custom-file">
                                                        <input type="file" class="custom-file-input" id="customFile" @change="onAddFileChange" />
                                                        <label class="custom-file-label talent_textshorten " for="customFile">{{addFileName == null ? addFileName : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 font_size_12">
                                                    *Max File Size 5MB
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="h-100">
                        <img :src="addImageData" for="customFile" class="h-100 w-100" />
                    </div>
                </div>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="addData(3)">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_blue talent_font_white" @click.prevent="addData(2)">
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

        <!--Edit-->
        <div class="row" v-if="edit === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Event > <span class="talent_font_red">Edit Event</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Event Information</h4>

                    <div v-if="fileType === false || fileSize === false" class="alert alert-danger">
                        <div v-if="fileSize === false">Maximum File Size is 5 MB</div>
                        <div v-if="fileType === false">Please input a jpg/png/jpeg file</div>
                    </div>

                    <div v-if="validateEdit() === false" class="alert alert-danger">
                        <div v-if="editModel.eventName === ''">Event Title is Required</div>
                        <div v-if="editStartDate === ''">Start date is Required</div>
                        <div v-if="editEndDate === ''">End date is Required</div>
                        <div v-if="editModel.eventHostName === ''">Host is Required</div>
                        <div v-if="editModel.location === ''">Location is Required</div>
                        <div v-if="selectedOutletCompleted === null">Outlet is Required</div>
                        <div v-if="listPosition === null">Position is Required</div>
                        <div v-if="editStartDate != '' && editEndDate != '' && editStartDate != null && editEndDate != null && editEndDate < editStartDate">End Date can't be before Start Date</div>
                    </div>

                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <label>Event Title<span class="talent_font_red">*</span></label>
                            <div class="input-group">
                                <input class="form-control" v-model="editModel.eventName" maxlength="255" />
                            </div>
                            <br />
                            <label>Event Category<span class="talent_font_red">*</span></label>
                            <select class="form-control" v-model="editEventCategory">
                                <option v-for="ec in eventCategories" :value="ec">{{ec.eventCategoryName}}</option>
                            </select>
                        </div>
                        <div class="col-md-6">
                            <label>Description</label>
                            <textarea class="form-control" style="height:150px;overflow-y:scroll;" v-model="editModel.eventDescription" maxlength="1024"></textarea>
                        </div>
                    </div>
                    <br />
                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Period<span class="talent_font_red">*</span></label>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="input-group">
                                                <v-date-picker class="v-date-style-report" :firstDayOfWeek="2" v-model="editStartDate" :masks="{ input: 'DD/MM/YYYY' }" :max-date="editEndDate" :input-props='{placeholder: "Start Date"}'></v-date-picker>
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
                                                <v-date-picker class="v-date-style-report" :firstDayOfWeek="2" v-model="editEndDate" :min-date="editStartDate" :masks="{ input: 'DD/MM/YYYY' }" :input-props='{placeholder: "End Date"}'></v-date-picker>
                                                <div class="input-group-append">
                                                    <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Host<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="editModel.eventHostName" maxlength="255" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Location<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="editModel.location" maxlength="255" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
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
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="areaChanged"
                                                 @remove="areaRemoved"
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
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="dealerChanged"
                                                 @remove="dealerRemoved"
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
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="cityChanged"
                                                 @remove="cityRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>Outlet<span class="talent_font_red">*</span></label>
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
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <label>Position<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="listPosition"
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
                    <h4>File Upload</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Upload Event Picture<span class="talent_font_red">*</span></label>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <div class="custom-file">
                                                        <input type="file" class="custom-file-input" id="customFile" @change="onEditFileChange" />
                                                        <label class="custom-file-label talent_textshorten " for="customFile">{{editFileName !== null ? editFileName : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 font_size_12">
                                                    *Max File Size 5MB
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="h-100">
                        <img :src="editImageData" for="customFile" class="w-100 h-100" />
                    </div>
                </div>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="editData(3)">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_blue talent_font_white" @click.prevent="editData(2)">
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

        <!--View Detail-->
        <div class="row" v-if="detail === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Event > <span class="talent_font_red">Detail Event</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Event Information</h4>
                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <label>Event Title<span class="talent_font_red">*</span></label>
                            <div class="input-group">
                                <input class="form-control" v-model="detailModel.eventName" disabled />
                            </div>
                            <br />
                            <label>Event Category<span class="talent_font_red">*</span></label>
                            <input class="form-control" v-model="detailModel.eventCategoryName" disabled />
                        </div>
                        <div class="col-md-6">
                            <label>Description</label>
                            <textarea class="form-control" style="height:150px;overflow-y:scroll;" v-model="detailModel.eventDescription" disabled></textarea>
                        </div>
                    </div>
                    <br />

                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Period<span class="talent_font_red">*</span></label>
                                    <div class="row">
                                        <div class="col-md-5">
                                            <div class="input-group">
                                                <input class="form-control" :value="convertDateTime(detailModel.startDateTime)" disabled />
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
                                                <input class="form-control" :value="convertDateTime(detailModel.endDateTime)" disabled />
                                                <div class="input-group-append">
                                                    <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Host<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="detailModel.eventHostName" disabled />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Location<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="detailModel.location" disabled />
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
                                                 @select="areaChanged"
                                                 @remove="areaRemoved"
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
                                                 @select="dealerChanged"
                                                 @remove="dealerRemoved"
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
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 :disabled="true"
                                                 @select="provinceChanged"
                                                 @remove="provinceRemoved"
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
                                                 @select="cityChanged"
                                                 @remove="cityRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>Outlet<span class="talent_font_red">*</span></label>
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
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 :disabled="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <label>Position<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="listPosition"
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
                                                 :show-no-results="true"
                                                 :disabled="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <h4>File Upload</h4>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Upload Event Picture<span class="talent_font_red">*</span></label>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <div class="custom-file">
                                                        <input type="file" class="custom-file-input" id="customFile" disabled />
                                                        <label class="custom-file-label talent_textshorten " for="customFile">{{detailFileName !== null ? detailFileName : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 font_size_12">
                                                    *Max File Size 5MB
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="h-100">
                        <img :src="detailImageData" for="customFile" class="w-100" />
                    </div>
                </div>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white" @click="backPage">Close</button>
                                    <button v-else class="btn talent_bg_red talent_font_white" @click="closeClicked">Close</button>
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
    import { Sort } from '../../class/Sort';
    import { EventService, EventJoinViewModel, ApprovalStatusService, EventCategoryService, EventJoinModel, ReleaseTrainingService, EventCategoryViewModel, EventCategoryModel, EventFormModel, FileContent, OutletCompleteViewModel, OutletViewModel, CityViewModel, ProvinceViewModel, DealerViewModel, AreaViewModel, PositionViewModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService'
    import { BlobService } from '../../services/BlobService';
    import { isNullOrUndefined } from 'util';
import { PageEnums } from '../../enum/PageEnums';

    @Component({
        created: async function (this: Event) {
            this.isBusy = true;
            await this.initDropdownData();
            await this.fetch();
            await this.getAccess();
            if (this.fromOutside === true) {
                await this.detailClicked(this.eventId);
            }
        },

        props: ['eventId', 'fromOutside']
    })

    export default class Event extends Vue {

        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Event);
            this.crud = data;
        }

         crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        isBusy: boolean = false;
        itemPerPage: number = 10;
        success: boolean = false;
        successMessage: string = '';

        fileSize: boolean = true;
        fileType: boolean = true;

        eventId: number;
        fromOutside: boolean;

        alertClose() {
            this.success = false;
            this.successMessage = '';
        }

        // ---------------------------------------- FETCH -----------------------------------------

        currentPage: number = 1;
        filterDate = {
            start: null,
            end: null
        };
        filterEventName: string = '';
        filterEventCategoryName: string = '';
        filterHostName: string = '';
        filterApprovalStatus: string = '';
        filterFromEventDate: Date = null;
        filterToEventDate: Date = null;
        sortBy: string = '';

        eventMan: EventService = new EventService();
        events: EventJoinViewModel = {};
        approvalMan: ApprovalStatusService = new ApprovalStatusService();
        approvalStatuses = {};
        eventCategoryMan: EventCategoryService = new EventCategoryService();
        eventCategories: EventCategoryModel[] = [];
        releaseTrainingMan: ReleaseTrainingService = new ReleaseTrainingService();

        imageUpload: FileContent = null;

        async fetch() {
            this.isBusy = true;
            var from = this.filterFromEventDate == null ? null : new Date(Date.UTC(this.filterFromEventDate.getFullYear(), this.filterFromEventDate.getMonth(), this.filterFromEventDate.getDate()));
            var to = this.filterToEventDate == null ? null : new Date(Date.UTC(this.filterToEventDate.getFullYear(), this.filterToEventDate.getMonth(), this.filterToEventDate.getDate()));
            this.events = await this.eventMan.getAllJoinEvents(this.filterDate.start, this.filterDate.end, this.filterEventName, this.filterEventCategoryName, this.filterHostName, this.filterApprovalStatus, from, to, this.sortBy, this.currentPage);
            this.isBusy = false;
        }

        async initDropdownData() {
            this.approvalStatuses = await this.approvalMan.getAllApprovalStatuses();
            this.eventCategories = await this.eventCategoryMan.getAllEventCategoriesNoFilter();

            this.listArea = await this.releaseTrainingMan.getAllArea();
            this.listDealer = await this.releaseTrainingMan.getAllDealer();
            this.listProvince = await this.releaseTrainingMan.getAllProvince();
            this.listCity = await this.releaseTrainingMan.getAllCity();
            this.listCompletedOutlet = await this.releaseTrainingMan.getAllOutlet();

            this.listAreaGroup[0].ListOption = this.listArea;
            this.listDealerGroup[0].ListOption = this.listDealer;
            this.listProvinceGroup[0].ListOption = this.listProvince;
            this.listCityGroup[0].ListOption = this.listCity;
            this.listOutletCompletedGroup[0].ListOption = this.listCompletedOutlet;
            this.listPositionGroup[0].ListOption = await this.releaseTrainingMan.getAllPosition();
        }

        addOutletDisabled = true;
        editOutletDisabled = true;

        convertDateTime(stringdate) {
            var date = new Date(stringdate);
            function pad(n) { return n < 10 ? "0" + n : n; }
            return pad(date.getDate()) + "/" + pad(date.getMonth() + 1) + "/" + date.getFullYear();
        }

        backPage() {
            this.imageUpload = null;
            window.history.back();
        }


        reset() {
            this.filterDate = {
                start: null,
                end: null
            };
            this.filterEventName = '';
            this.filterEventCategoryName = '';
            this.filterHostName = '';
            this.filterApprovalStatus = '';
            this.filterFromEventDate = null;
            this.filterToEventDate = null;
            this.ResetSort('');
            this.fetch();
        }

        // ----------------------------------------- CRUD ---------------------------------------------

        //Navigation
        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;

        //Add
        addValidation = false;
        addModel: EventFormModel = {
            eventName: null,
            eventHostName: null,
            location: null
        };
        addEventCategory: EventCategoryModel = {};
        addFileName = null;
        addImageData = '/upload-image2.png';
        addFormData = null;

        addAreas = [];
        //addRegions = [];
        addDealers = [];
        addProvinces = [];
        addCities = [];
        addOutlets = [];
        addStartDate = null;
        addEndDate = null;

        addClicked() {
            this.alertClose();
            this.add = true;
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

        async onAddFileChange(e) {
            var fileInput = e.target.files || e.dataTransfer.files;

            if (!fileInput.length) return;

            //this.imageUpload = null;
            this.fileType = true;
            this.fileSize = true;

            var extension = fileInput[0].name.substring(fileInput[0].name.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpeg' && extension != '.jpg' && extension != '.png') {
                this.fileType = false;
            }
            if (fileInput[0].size > 5048576) {
                this.fileSize = false;
            }

            if (this.fileType && this.fileSize) {

                this.addFileName = fileInput[0].name;
                this.addFormData = new FormData();
                Array
                    .from(Array(e.target.files.length).keys())
                    .map(x => {
                        this.addFormData.append(e.target.files, e.target.files[x], e.target.files[x].name);
                    });

                var temp = this;
                let reader = new FileReader();
                reader.readAsDataURL(fileInput[0]);
                reader.onload = function () {
                    temp.addImageData = reader.result as string;
                };

                let inputFile = (<HTMLInputElement>e.target).files[0];

                let base64String = await this.convertToBase64(inputFile);

                let file = e.target.files[0];

                let array = file.name.split(".");

                this.imageUpload = {
                    base64: base64String,
                    fileName: fileInput[0].name,
                    contentType: array.pop()
                };
            }
        }

        validateAdd() {
            if (this.addFileName === '' || this.addModel.eventName === '' || this.addEventCategory.eventCategoryId == 0 || this.addStartDate === '' || this.addEndDate === '' || this.selectedOutletCompleted === null || this.listPosition === null || this.addModel.eventHostName === '' || this.addModel.location === '') {
                return false;
            }
            else if (this.addEndDate != null && this.addEndDate != null && this.addEndDate < this.addStartDate) {
                return false;
            }
            else {
                return true;
            }
        }

        async addData(approvalId) {
            this.isBusy = true;

            this.addValidation = true;
            if (isNullOrUndefined(this.addFileName)) {
                this.addFileName = '';
            }
            if (isNullOrUndefined(this.addModel.eventName)) {
                this.addModel.eventName = '';
            }
            if (isNullOrUndefined(this.addEventCategory.eventCategoryId)) {
                this.addEventCategory.eventCategoryId = 0;
            }
            if (isNullOrUndefined(this.addStartDate)) {
                this.addStartDate = null;
            }
            if (isNullOrUndefined(this.addEndDate)) {
                this.addEndDate = null;
            }
            if (isNullOrUndefined(this.addModel.eventHostName)) {
                this.addModel.eventHostName = '';
            }
            if (isNullOrUndefined(this.addModel.location)) {
                this.addModel.location = '';
            }
            if (this.selectedOutletCompleted != null && this.selectedOutletCompleted.length == 0) {
                this.selectedOutletCompleted = null;
            }
            if (this.listPosition != null && this.listPosition.length == 0) {
                this.listPosition = null;
            }
            if (this.validateAdd() === false) {
                this.addValidation = false;
                this.isBusy = false;
                return;
            }
            if (this.addValidation === true) {
                this.addModel.approvalId = approvalId;
                this.addModel.eventCategoryId = this.addEventCategory.eventCategoryId;
                this.addModel.startDateTime = this.addStartDate;
                this.addModel.endDateTime = this.addEndDate;
                this.addModel.listEventOutlets = [];
                for (var i = 0; i < this.selectedOutletCompleted.length; i++) {
                    this.addModel.listEventOutlets.push({ eventId: 0, outletId: this.selectedOutletCompleted[i].outletId });
                }
                this.addModel.listEventPositions = [];
                for (var i = 0; i < this.listPosition.length; i++) {
                    this.addModel.listEventPositions.push({ eventId: 0, positionId: this.listPosition[i].positionId });
                }
                if (this.imageUpload != null) {
                    this.addModel.imageUpload = this.imageUpload;
                }
                await this.eventMan.create(this.addModel);
                this.reset();
                this.addModel.eventName = null;
                this.addModel.eventDescription = null;
                this.addEventCategory = {};
                this.addStartDate = null;
                this.addEndDate = null;
                this.addModel.eventHostName = null;
                this.addModel.location = null;
                this.addAreas = [];
                this.addDealers = [];
                this.addProvinces = [];
                this.addCities = [];
                this.addOutlets = [];
                this.listPosition = [];
                this.addOutletDisabled = true;
                this.addFormData = new FormData();
                this.addImageData = '/upload-image2.png';
                this.successMessage = 'Success to Add Data!';
                this.success = true;
                this.closeClicked();
            }
            this.isBusy = false;
        }

        //Edit
        editValidation = false;
        editModel: EventJoinModel = {};
        editEventCategory: EventCategoryModel = {};
        editFileName = null;
        editImageData = '/upload-image2.png';
        editFormData = null;
        editStartDate = null;
        editEndDate = null;

        async editClicked(eventId: number) {

            if(this.crud.update == false){
                return;
            }

            //Taking time to load;
            this.isBusy = true;

            this.alertClose();
            this.editModel = await this.eventMan.getJoinEventById(eventId);
            this.editFileName = this.editModel.fileName;
            this.editImageData = '/upload-image2.png';
            if (this.editModel.mime == 'jpg' || this.editModel.mime == 'jpeg' || this.editModel.mime == 'png') {
                this.editImageData = await BlobService.getImageUrl(this.editModel.blobId, this.editModel.mime);
            }
            var startYear = Number.parseInt(this.editModel.startDateTime.toString().substr(0, 4));
            var startMonth = Number.parseInt(this.editModel.startDateTime.toString().substr(5, 2));
            var startDate = Number.parseInt(this.editModel.startDateTime.toString().substr(8, 2));
            this.editStartDate = new Date(startYear, startMonth - 1, startDate);
            var endYear = Number.parseInt(this.editModel.endDateTime.toString().substr(0, 4));
            var endMonth = Number.parseInt(this.editModel.endDateTime.toString().substr(5, 2));
            var endDate = Number.parseInt(this.editModel.endDateTime.toString().substr(8, 2));
            this.editEndDate = new Date(endYear, endMonth - 1, endDate);
            var eventCategoryId = this.eventCategories.findIndex(Q => Q.eventCategoryId == this.editModel.eventCategoryId);
            if (eventCategoryId > -1) {
                this.editEventCategory = this.eventCategories[eventCategoryId];
            }

            this.selectedArea = this.listAreaGroup[0].ListOption.filter(Q => this.editModel.listEventAreaIds.includes(Q.areaId)).map(x => x);
            this.selectedDealer = this.listDealerGroup[0].ListOption.filter(Q => this.editModel.listEventDealerIds.includes(Q.dealerId)).map(x => x);
            this.selectedProvince = this.listProvinceGroup[0].ListOption.filter(Q => this.editModel.listEventProvinceIds.includes(Q.provinceId)).map(x => x);
            this.selectedCity = this.listCityGroup[0].ListOption.filter(Q => this.editModel.listEventCityIds.includes(Q.cityId)).map(x => x);

            var currentSelectedOutletIds = this.editModel.listEventOutlets.map(Q => Q.outletId);
            this.editListGroup();

            var selected = this.listOutletCompletedGroup[0].ListOption.filter(Q => currentSelectedOutletIds.includes(Q.outletId)).map(x => x);
            this.selectedOutletCompleted = selected.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var currentPositions = this.editModel.listEventPositions.map(Q => Q.positionId);
            this.listPosition = this.listPositionGroup[0].ListOption.filter(Q => currentPositions.includes(Q.positionId)).map(x => x);

            this.edit = true;
            this.isBusy = false;
        }

        async onEditFileChange(e) {
            var fileInput = e.target.files || e.dataTransfer.files;
            if (!fileInput.length) return;

            //this.imageUpload = null;
            this.fileType = true;
            this.fileSize = true;

            var extension = fileInput[0].name.substring(fileInput[0].name.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpeg' && extension != '.jpg' && extension != '.png') {
                this.fileType = false;
            }
            if (fileInput[0].size > 5048576) {
                this.fileSize = false;
            }

            if (this.fileType && this.fileSize) {
                this.editFileName = fileInput[0].name;
                this.editFormData = new FormData();
                Array
                    .from(Array(e.target.files.length).keys())
                    .map(x => {
                        this.editFormData.append(e.target.files, e.target.files[x], e.target.files[x].name);
                    });

                var temp = this;
                let reader = new FileReader();
                reader.readAsDataURL(fileInput[0]);
                reader.onload = function () {
                    temp.editImageData = reader.result as string;
                };

                let inputFile = (<HTMLInputElement>e.target).files[0];

                let base64String = await this.convertToBase64(inputFile);

                let file = e.target.files[0];

                let array = file.name.split(".");

                this.imageUpload = {
                    base64: base64String,
                    fileName: fileInput[0].name,
                    contentType: array.pop()
                };
            }
        }

        validateEdit() {
            if (this.editModel.eventName === '' || this.editStartDate === '' || this.editEndDate === '' || this.editModel.eventHostName === '' || this.editModel.location === '' || this.selectedOutletCompleted == null || this.listPosition == null) {
                return false;
            }
            else if (this.editEndDate != null && this.editEndDate != null && this.editEndDate < this.editStartDate) {
                return false;
            }
            else {
                return true;
            }
        }

        async editData(approvalId) {
            this.isBusy = true;

            this.editValidation = true;
            
            if (isNullOrUndefined(this.editStartDate)) {
                this.editStartDate = '';
            }
            if (isNullOrUndefined(this.editEndDate)) {
                this.editEndDate = '';
            }
            if (this.selectedOutletCompleted != null && this.selectedOutletCompleted.length == 0) {
                this.selectedOutletCompleted = null;
            }
            if (this.listPosition != null && this.listPosition.length == 0) {
                this.listPosition = null;
            }

            if (this.validateEdit() === false) {
                this.editValidation = false;
            }
            if (this.editValidation === true) {
                this.editModel.approvalId = approvalId;
                this.editModel.eventCategoryId = this.editEventCategory.eventCategoryId;
                this.editModel.startDateTime = this.editStartDate;
                this.editModel.endDateTime = this.editEndDate;
                this.editModel.listEventOutlets = [];
                for (var i = 0; i < this.selectedOutletCompleted.length; i++) {
                    this.editModel.listEventOutlets.push({ eventId: 0, outletId: this.selectedOutletCompleted[i].outletId });
                }
                this.editModel.listEventPositions = [];
                for (var i = 0; i < this.listPosition.length; i++) {
                    this.editModel.listEventPositions.push({ eventId: 0, positionId: this.listPosition[i].positionId });
                }
                
                if (this.imageUpload != null) {
                    this.editModel.imageUpload = this.imageUpload;
                }
                await this.eventMan.update(this.editModel);
                this.reset();
                this.successMessage = 'Success to Edit Data!';
                this.success = true;
                this.closeClicked();
            }
            this.isBusy = false;
        }

        //Detail
        detailValidation = false;
        detailModel: EventJoinModel = {};
        detailFileName = null;
        detailImageData = '/upload-image2.png';


        async detailClicked(eventId: number) {
            this.isBusy = true;

            this.alertClose();
            this.detailModel = await this.eventMan.getJoinEventById(eventId);
            this.detailFileName = this.detailModel.fileName;
            this.detailImageData = '/upload-image2.png';
            if (this.detailModel.mime == 'jpg' || this.detailModel.mime == 'jpeg' || this.detailModel.mime == 'png') {
                this.detailImageData = await BlobService.getImageUrl(this.detailModel.blobId, this.detailModel.mime);
            }

            //TO DO DETAIL
            this.selectedArea = this.listAreaGroup[0].ListOption.filter(Q => this.detailModel.listEventAreaIds.includes(Q.areaId)).map(x => x);
            this.selectedDealer = this.listDealerGroup[0].ListOption.filter(Q => this.detailModel.listEventDealerIds.includes(Q.dealerId)).map(x => x);
            this.selectedProvince = this.listProvinceGroup[0].ListOption.filter(Q => this.detailModel.listEventProvinceIds.includes(Q.provinceId)).map(x => x);
            this.selectedCity = this.listCityGroup[0].ListOption.filter(Q => this.detailModel.listEventCityIds.includes(Q.cityId)).map(x => x);

            var currentSelectedOutletIds = this.detailModel.listEventOutlets.map(Q => Q.outletId);
            this.editListGroup();

            var selected = this.listOutletCompletedGroup[0].ListOption.filter(Q => currentSelectedOutletIds.includes(Q.outletId)).map(x => x);
            this.selectedOutletCompleted = selected.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var currentPositions = this.detailModel.listEventPositions.map(Q => Q.positionId);
            this.listPosition = this.listPositionGroup[0].ListOption.filter(Q => currentPositions.includes(Q.positionId)).map(x => x);

            this.detail = true;
            this.isBusy = false;
        }

        //Delete
        deleteEventId;
        deleteIndex;

        async deleteClicked(eventId: number, index: number) {
            this.alertClose();
            this.deleteEventId = eventId;
            this.deleteIndex = index;
        }

        async deleteData() {
            this.isBusy = true;
            await this.eventMan.delete(this.deleteEventId);
            this.fetch();
            this.isBusy = false;
            this.successMessage = 'Success to Remove Data!';
            this.success = true;
        }

        closeClicked() {
            this.clearForm();

            this.add = false;
            this.edit = false;
            this.detail = false;
            this.imageUpload = null;
        }

        clearForm(){
            this.addModel = {
                eventName: null,
                eventHostName: null,
                location: null
            }

            this.addStartDate = null;
            this.addEndDate = null;

            this.listPosition = [];
            this.selectedArea = [];
            this.selectedDealer = [];
            this.selectedProvince = [];
            this.selectedCity = [];
            this.selectedOutletCompleted = [];
            
            this.addEventCategory = {};

            this.addImageData = '/upload-image2.png';
            this.addFileName = null;
        }

        // ---------------------------------------- Sorting ------------------------------------------

        //Variable untuk sorting
        eventTitle = new Sort();
        eventCategory = new Sort();
        startDate = new Sort();
        endDate = new Sort();
        host = new Sort();
        approvalStatus = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortAsssessment
        async ClickSortEventTitle() {
            this.ResetSort('eventTitle');
            //Sorting
            this.eventTitle.sorting();
            //Function Sorting
            if (this.eventTitle.sortup == true) {
                this.sortBy = 'eventName';
            }
            else if (this.eventTitle.sortdown == true) {
                this.sortBy = 'eventNameDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickEmployeeName
        async ClickSortEventCategory() {
            this.ResetSort('eventCategory');
            //Sorting
            this.eventCategory.sorting();
            //Function Sorting
            if (this.eventCategory.sortup == true) {
                this.sortBy = 'eventCategory';
            }
            else if (this.eventCategory.sortdown == true) {
                this.sortBy = 'eventCategoryDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortPosition
        async ClickSortStartDate() {
            this.ResetSort('startDate');
            //Sorting
            this.startDate.sorting();
            //Function Sorting
            if (this.startDate.sortup == true) {
                this.sortBy = 'startDate';
            }
            else if (this.startDate.sortdown == true) {
                this.sortBy = 'startDateDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortDealer
        async ClickSortEndDate() {
            this.ResetSort('endDate');
            //Sorting
            this.endDate.sorting();
            //Function Sorting
            if (this.endDate.sortup == true) {
                this.sortBy = 'endDate';
            }
            else if (this.endDate.sortdown == true) {
                this.sortBy = 'endDateDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortOutlert
        async ClickSortHost() {
            this.ResetSort('host');
            //Sorting
            this.host.sorting();
            //Function Sorting
            if (this.host.sortup == true) {
                this.sortBy = 'host';
            }
            else if (this.host.sortdown == true) {
                this.sortBy = 'hostDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortCreatedDate
        async ClickSortApprovalStatus() {
            this.ResetSort('approvalStatus');
            //Sorting
            this.approvalStatus.sorting();
            //Function Sorting
            if (this.approvalStatus.sortup == true) {
                this.sortBy = 'approvalStatus';
            }
            else if (this.approvalStatus.sortdown == true) {
                this.sortBy = 'approvalStatusDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting
            if (this.createdDate.sortup == true) {
                this.sortBy = 'createdDate';
            }
            else if (this.createdDate.sortdown == true) {
                this.sortBy = 'createdDateDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting
            if (this.updatedDate.sortup == true) {
                this.sortBy = 'updatedDate';
            }
            else if (this.updatedDate.sortdown == true) {
                this.sortBy = 'updatedDateDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'eventTitle':
                    this.eventCategory.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.host.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'eventCategory':
                    this.eventTitle.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.host.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'startDate':
                    this.eventTitle.reset();
                    this.eventCategory.reset();
                    this.endDate.reset();
                    this.host.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'endDate':
                    this.eventTitle.reset();
                    this.eventCategory.reset();
                    this.startDate.reset();
                    this.host.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'host':
                    this.eventTitle.reset();
                    this.eventCategory.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'approvalStatus':
                    this.eventTitle.reset();
                    this.eventCategory.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.host.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.eventTitle.reset();
                    this.eventCategory.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.host.reset();
                    this.approvalStatus.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.eventTitle.reset();
                    this.eventCategory.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.host.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    return;
                default:
                    this.sortBy = '';
                    this.updatedDate.reset();
                    this.eventTitle.reset();
                    this.eventCategory.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.host.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    return;
            }
        }

        // ---------------------------------------- Filter Feature -----------------------------------------

        listArea: AreaViewModel[] = [];
        listDealer: DealerViewModel[] = [];
        listProvince: ProvinceViewModel[] = [];
        listCity: CityViewModel[] = [];
        listOutlet: OutletViewModel[] = [];
        listCompletedOutlet: OutletCompleteViewModel[] = [];

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
        selectedDealer: DealerViewModel[] = [];
        selectedProvince: ProvinceViewModel[] = [];
        selectedCity: CityViewModel[] = [];
        selectedOutletCompleted: OutletCompleteViewModel[] = [];

        listPosition: any[] = [];

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
    }

    interface ListGrouping {
        ListOption: any[],
        GroupName: string
    }
</script>