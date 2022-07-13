<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="row">
            <div class="col-md-12">
                <!--TITLE-->
                <h3 v-if="edit == false && detail == false"><fa icon="database"></fa> Setup > <span class="talent_font_red">Training Process</span></h3>
                <h3 v-if="edit == true || detail == true"><fa icon="database"></fa> Setup > Training Process > <span class="talent_font_red">Manage Training Process</span></h3>
                <br />
            </div>
        </div>
        <!-- View -->
        <div class="row" v-if="edit == false && detail == false">
            <div class="col col-md-12">
                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Training Process</h4>

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
                                        <v-date-picker class="v-date-style-report" mode="range" :firstDayOfWeek="2" v-model="filterDate" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                    <span>Program Type</span>
                                </div>
                                <div class="col-md-9">
                                    <select class="form-control" v-model="filterProgramType">
                                        <option v-for="p in programTypes.listProgramTypeModel" :value="p.programTypeName">{{p.programTypeName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Confirmed</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input type="number" class="form-control" v-model="filterConfirmed" />
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
                                    <input class="form-control" v-model="filterCourseName" />
                                    <!--<select class="form-control" v-model="filterCourseName">
                                        <option>-</option>
                                        <option>1</option>
                                    </select>-->
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
                                        <input class="form-control" type="number" v-model="filterBatch" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Quota</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input class="form-control" type="number" v-model="filterQuota" />
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
                    <h4>Training Process List</h4>
                    <br />
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{trainingProcess.listTrainingProcess == null ? 0 : trainingProcess.listTrainingProcess.length }} of {{trainingProcess.totalItem}} Entry(s)</a>
                        </div>
                    </div>
                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCourseName()">Course Name <fa icon="sort" v-if="courseName.sort == true"></fa><fa icon="sort-up" v-if="courseName.sortup == true"></fa><fa icon="sort-down" v-if="courseName.sortdown"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortProgramType()">Program Type <fa icon="sort" v-if="programType.sort == true"></fa><fa icon="sort-up" v-if="programType.sortup == true"></fa><fa icon="sort-down" v-if="programType.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortBatch()">Batch <fa icon="sort" v-if="batch.sort == true"></fa><fa icon="sort-up" v-if="batch.sortup == true"></fa><fa icon="sort-down" v-if="batch.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortConfirmed()">Confirmed <fa icon="sort" v-if="confirmed.sort == true"></fa><fa icon="sort-up" v-if="confirmed.sortup == true"></fa><fa icon="sort-down" v-if="confirmed.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortQuota()">Quota <fa icon="sort" v-if="quota.sort == true"></fa><fa icon="sort-up" v-if="quota.sortup == true"></fa><fa icon="sort-down" v-if="quota.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortStartDate()">Training Start Date <fa icon="sort" v-if="startDate.sort == true"></fa><fa icon="sort-up" v-if="startDate.sortup == true"></fa><fa icon="sort-down" v-if="startDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortEndDate()">Training End Date <fa icon="sort" v-if="endDate.sort == true"></fa><fa icon="sort-up" v-if="endDate.sortup == true"></fa><fa icon="sort-down" v-if="endDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="2">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="item in trainingProcess.listTrainingProcess">
                                    <td>
                                        {{item.courseName}}
                                    </td>
                                    <td>
                                        {{item.programType}}
                                    </td>
                                    <td>
                                        {{item.batch == null ? '-' : item.batch}}
                                    </td>
                                    <td>
                                        {{item.confirmed == null ? '-' : item.confirmed}}
                                    </td>
                                    <td>
                                        {{item.quota == null ? '-' : item.quota}}
                                    </td>
                                    <td>
                                        {{convertDateTime(item.trainingStartDate)}}
                                    </td>
                                    <td>
                                        {{convertDateTime(item.trainingEndDate)}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="detailClicked(item)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="editClicked(item)">Edit</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="row justify-content-center">
                        <paginate :currentPage.sync="currentPage"
                                  :total-data=trainingProcess.totalItem
                                  :limit-data=10
                                  @update:currentPage="fetch()"></paginate>
                    </div>
                </div>
                <br />

            </div>
        </div>

        <!-- View Detail -->
        <div class="row" v-if="edit == false && detail == true">
            <div class="col col-md-12">
                <!--Training Detail-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <!--Header-->
                    <div class="row font-weight-bold">
                        <div class="col-md-3">
                            Course Name
                        </div>
                        <div class="col-md-3">
                            Program Type
                        </div>
                        <div class="col-md-3">
                            Batch
                        </div>
                        <div class="col-md-3">
                            Training Date
                        </div>
                    </div>
                    <!--Detail Data-->
                    <div class="row">
                        <div class="col-md-3">
                            {{courseById.courseName}}
                        </div>
                        <div class="col-md-3">
                            {{courseById.programType}}
                        </div>
                        <div class="col-md-3">
                            {{courseById.batch}}
                        </div>
                        <div class="col-md-3">
                            {{courseById.trainingStartDate | dateFormat}} - {{courseById.trainingEndDate | dateFormat}}
                        </div>
                    </div>
                </div>

                <br />

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Training List Information</h4>
                    <br />
                    <div class="row d-flex align-items-center mb-4">
                        <!-- Title -->
                        <div class="col-auto">
                            <h6 class="mb-0">Confirmed List</h6>
                        </div>
                        <!-- Search Bar -->
                        <div class="col px-0">
                            <form class="d-flex">
                                <input
                                    id="confirmedList"
                                    class="form-control"
                                    type="search"
                                    placeholder="Search"
                                    aria-label="Search"
                                    v-model="confirmedListDOM"
                                />
                                <button
                                    class="btn talent_bg_darkblue talent_font_white"
                                    type="submit"
                                    @click.prevent="confirmedtrainingListFilter()"
                                >
                                    <span
                                        ><i
                                            class="fa fa-search"
                                            aria-hidden="true"
                                        ></i
                                    ></span>
                                </button>
                            </form>
                        </div>
                        <!-- Download Button -->
                        <div class="col d-flex justify-content-end">
                            <button
                                v-if="crud.create"
                                class="btn talent_bg_darkblue talent_font_white"
                                @click.prevent="confirmedListdownloadExcel"
                            >
                                Download
                            </button>
                        </div>
                    </div>
                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter text-center">
                            <thead>
                                <tr>
                                    <th>
                                        No
                                    </th>
                                    <th>
                                        Employee Name
                                    </th>
                                    <th>
                                        Gender
                                    </th>
                                    <th>
                                        Dealer
                                    </th>
                                    <th>
                                        Outlet
                                    </th>
                                    <th>
                                        Accommodation
                                    </th>
                                    <th>
                                        Price
                                    </th>
                                    <th>
                                        Date of Stay
                                    </th>
                                    <th>
                                        Action <fa icon="check-square"></fa>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in confirmedListfilteredData" :key="item.trainingInvitationId">
                                    <td>
                                        {{index+1}}
                                    </td>
                                    <td>
                                        {{item.employeeName}}
                                    </td>
                                    <td>
                                        <div v-if="item.gender != null">
                                            {{item.gender}}
                                        </div>

                                        <div v-if="item.gender == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        {{item.dealer}}
                                    </td>
                                    <td>
                                        {{item.outlet}}
                                    </td>
                                    <td>
                                        <div v-if="item.accommodation != null">
                                            {{item.accommodation}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.price != null">
                                            {{item.price}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.dateofStayStart != null && item.dateofStayEnd != null">
                                            {{item.dateofStayStart | dateFormat}} - {{item.dateofStayEnd | dateFormat}}
                                        </div>
                                        <div v-if="item.dateofStayStart == null && item.dateofStayEnd == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <input type="checkbox" v-model="item.isConfirmed" disabled>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <br />
                    
                    <div class="talent_overflowx">
                        <div class="row d-flex align-items-center mb-4">
                        <!-- Title -->
                        <div class="col-auto">
                            <h6>Waiting List</h6>
                        </div>
                        <!-- Search Bar -->
                        <div class="col px-0">
                            <form class="d-flex">
                                <input
                                    id="confirmedList"
                                    class="form-control"
                                    type="search"
                                    placeholder="Search"
                                    aria-label="Search"
                                    v-model="waitingListDOM"
                                />
                                <button
                                    class="btn talent_bg_darkblue talent_font_white"
                                    type="submit"
                                    @click.prevent="waitingtrainingListFilter()"
                                >
                                    <span
                                        ><i
                                            class="fa fa-search"
                                            aria-hidden="true"
                                        ></i
                                    ></span>
                                </button>
                            </form>
                        </div>
                        <!-- Download Button -->
                        <div class="col d-flex justify-content-end">
                            <button
                                v-if="crud.create"
                                class="btn talent_bg_darkblue talent_font_white"
                                @click.prevent="waitingListdownloadExcel"
                            >
                                Download
                            </button>
                        </div>
                    </div>
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter text-center">
                            <thead>
                                <tr>
                                    <th>
                                        No
                                    </th>
                                    <th>
                                        Employee Name
                                    </th>
                                    <th>
                                        Gender
                                    </th>
                                    <th>
                                        Dealer
                                    </th>
                                    <th>
                                        Outlet
                                    </th>
                                    <th>
                                        Accommodation
                                    </th>
                                    <th>
                                        Price
                                    </th>
                                    <th>
                                        Date of Stay
                                    </th>
                                    <th>
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in waitingListfilteredData" :key="item.trainingInvitationId">
                                    <td>
                                        {{index+1}}
                                    </td>
                                    <td>
                                        {{item.employeeName}}
                                    </td>
                                    <td>
                                        <div v-if="item.gender != null">
                                            {{item.gender}}
                                        </div>

                                        <div v-if="item.gender == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        {{item.dealer}}
                                    </td>
                                    <td>
                                        {{item.outlet}}
                                    </td>
                                    <td>
                                        <div v-if="item.accommodation != null">
                                            {{item.accommodation}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.price != null">
                                            {{item.price}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.dateofStayStart != null && item.dateofStayEnd != null">
                                            {{item.dateofStayStart | dateFormat}} - {{item.dateofStayEnd | dateFormat}}
                                        </div>
                                        <div v-if="item.dateofStayStart == null && item.dateofStayEnd == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <input type="checkbox" v-model="item.isConfirmed" disabled>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-10">
                            <h4>Summary of Confirmed Training List</h4>
                        </div>
                        <div class="col-md-2">
                            <button class="btn talent_bg_green talent_font_white" @click.prevent="summaryConfirmedExcel">Download</button>
                        </div>
                    </div>
                    <br />

                    <h6>Confirmed List</h6>
                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter text-center">
                            <thead>
                                <tr>
                                    <th>
                                        No
                                    </th>
                                    <th>
                                        Employee Name
                                    </th>
                                    <th>
                                        Gender
                                    </th>
                                    <th>
                                        Dealer
                                    </th>
                                    <th>
                                        Outlet
                                    </th>
                                    <th>
                                        Accommodation
                                    </th>
                                    <th>
                                        Price
                                    </th>
                                    <th>
                                        Date of Stay
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in trainingProcessDetail.trainingProcessDetailList.filter(Q=>Q.isConfirmed == true)" :key="'sum'+item.trainingInvitationId">
                                    <td>
                                        {{index + 1}}
                                    </td>
                                    <td>
                                        {{item.employeeName}}
                                    </td>
                                    <td>
                                        <div v-if="item.gender != null">
                                            {{item.gender}}
                                        </div>

                                        <div v-if="item.gender == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        {{item.dealer}}
                                    </td>
                                    <td>
                                        {{item.outlet}}
                                    </td>
                                    <td>
                                        <div v-if="item.accommodation != null">
                                            {{item.accommodation}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.price != null">
                                            {{item.price}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.dateofStayStart != null && item.dateofStayEnd != null">
                                            {{item.dateofStayStart | dateFormat}} - {{item.dateofStayEnd | dateFormat}}
                                        </div>
                                        <div v-if="item.dateofStayStart == null && item.dateofStayEnd == null">
                                            -
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <br />
                    <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter text-center">
                        <thead>
                            <tr>
                                <th>
                                    Gender
                                </th>
                                <th>
                                    Total Confirmed Participant
                                </th>
                                <th>
                                    Quota
                                </th>
                                <th>
                                    Total Room
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    Male
                                </td>
                                <td>
                                    {{maleTotalParticipant}}
                                </td>
                                <td>
                                    {{quotaTotal}}
                                </td>
                                <td>
                                    {{maleTotalRoom}}
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Female
                                </td>
                                <td>
                                    {{femaleTotalParticipant}}
                                </td>
                                <td>
                                    {{quotaTotal}}
                                </td>
                                <td>
                                    {{femaleTotalRoom}}
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-11"></div>
                        <div class="col-md-1">
                            <button class="btn btn-block talent_bg_red talent_font_white" @click.prevent="closeClicked()">Close</button>
                        </div>
                        <!--<div class="col-md-2">
                            <button class="btn btn-block talent_bg_blue talent_font_white">Send Information</button>
                        </div>-->
                    </div>
                </div>
            </div>
        </div>


        <!-- Edit -->
        <div class="row" v-if="detail == false && edit == true">
            <div class="col col-md-12">
                <!--Training Detail-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <!--Header-->
                    <div class="row font-weight-bold">
                        <div class="col-md-3">
                            Course Name
                        </div>
                        <div class="col-md-3">
                            Program Type
                        </div>
                        <div class="col-md-3">
                            Batch
                        </div>
                        <div class="col-md-3">
                            Training Date
                        </div>
                    </div>
                    <!--Detail Data-->
                    <div class="row">
                        <div class="col-md-3">
                            {{courseById.courseName}}
                        </div>
                        <div class="col-md-3">
                            {{courseById.programType}}
                        </div>
                        <div class="col-md-3">
                            {{courseById.batch}}
                        </div>
                        <div class="col-md-3">
                            {{courseById.trainingStartDate | dateFormat}} - {{courseById.trainingEndDate | dateFormat}}
                        </div>
                    </div>
                </div>

                <br />

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Training List Information</h4>
                    <br />
                    <div class="row d-flex align-items-center mb-4">
                        <!-- Title -->
                        <div class="col-auto">
                            <h6 class="mb-0">Confirmed List</h6>
                        </div>
                        <!-- Search Bar -->
                        <div class="col px-0">
                            <form class="d-flex">
                                <input
                                    id="confirmedList"
                                    class="form-control"
                                    type="search"
                                    placeholder="Search"
                                    aria-label="Search"
                                    v-model="confirmedListDOM"
                                />
                                <button
                                    class="btn talent_bg_darkblue talent_font_white"
                                    type="submit"
                                    @click.prevent="confirmedtrainingListFilter()"
                                >
                                    <span
                                        ><i
                                            class="fa fa-search"
                                            aria-hidden="true"
                                        ></i
                                    ></span>
                                </button>
                            </form>
                        </div>
                        <!-- Download Button -->
                        <div class="col d-flex justify-content-end">
                            <button
                                v-if="crud.create"
                                class="btn talent_bg_darkblue talent_font_white"
                                @click.prevent="confirmedListdownloadExcel"
                            >
                                Download
                            </button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th>
                                        No
                                    </th>
                                    <th>
                                        Employee Name
                                    </th>
                                    <th>
                                        Gender
                                    </th>
                                    <th>
                                        Dealer
                                    </th>
                                    <th>
                                        Outlet
                                    </th>
                                    <th>
                                        Accommodation
                                    </th>
                                    <th>
                                        Price
                                    </th>
                                    <th>
                                        Date of Stay
                                    </th>
                                    <th>
                                        Action <fa icon="check-square"></fa>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in confirmedListfilteredData" :key="item.trainingInvitationId">
                                    <td>
                                        {{index+1}}
                                    </td>
                                    <td>
                                        {{item.employeeName}}
                                    </td>
                                    <td>
                                        <div v-if="item.gender != null">
                                            {{item.gender}}
                                        </div>

                                        <div v-if="item.gender == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        {{item.dealer}}
                                    </td>
                                    <td>
                                        {{item.outlet}}
                                    </td>
                                    <td>
                                        <div v-if="item.accommodation != null">
                                            {{item.accommodation}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.price != null">
                                            {{item.price}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.dateofStayStart != null && item.dateofStayEnd != null">
                                            {{item.dateofStayStart | dateFormat}} - {{item.dateofStayEnd | dateFormat}}
                                        </div>
                                        <div v-if="item.dateofStayStart == null && item.dateofStayEnd == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <input type="checkbox" v-model="item.isConfirmed" @change="updateTable()">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <br />
                    <div class="row d-flex align-items-center mb-4">
                        <!-- Title -->
                        <div class="col-auto">
                            <h6>Waiting List</h6>
                        </div>
                        <!-- Search Bar -->
                        <div class="col px-0">
                            <form class="d-flex">
                                <input
                                    id="confirmedList"
                                    class="form-control"
                                    type="search"
                                    placeholder="Search"
                                    aria-label="Search"
                                    v-model="waitingListDOM"
                                />
                                <button
                                    class="btn talent_bg_darkblue talent_font_white"
                                    type="submit"
                                    @click.prevent="waitingtrainingListFilter()"
                                >
                                    <span
                                        ><i
                                            class="fa fa-search"
                                            aria-hidden="true"
                                        ></i
                                    ></span>
                                </button>
                            </form>
                        </div>
                        <!-- Download Button -->
                        <div class="col d-flex justify-content-end">
                            <button
                                v-if="crud.create"
                                class="btn talent_bg_darkblue talent_font_white"
                                @click.prevent="waitingListdownloadExcel"
                            >
                                Download
                            </button>
                        </div>
                    </div>
                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter ">
                            <thead>
                                <tr>
                                    <th>
                                        No
                                    </th>
                                    <th>
                                        Employee Name
                                    </th>
                                    <th>
                                        Gender
                                    </th>
                                    <th>
                                        Dealer
                                    </th>
                                    <th>
                                        Outlet
                                    </th>
                                    <th>
                                        Accommodation
                                    </th>
                                    <th>
                                        Price
                                    </th>
                                    <th>
                                        Date of Stay
                                    </th>
                                    <th>
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in waitingListfilteredData" :key="item.trainingInvitationId">
                                    <td>
                                        {{index+1}}
                                    </td>
                                    <td>
                                        {{item.employeeName}}
                                    </td>
                                    <td>
                                        <div v-if="item.gender != null">
                                            {{item.gender}}
                                        </div>

                                        <div v-if="item.gender == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        {{item.dealer}}
                                    </td>
                                    <td>
                                        {{item.outlet}}
                                    </td>
                                    <td>
                                        <div v-if="item.accommodation != null">
                                            {{item.accommodation}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.price != null">
                                            {{item.price}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.dateofStayStart != null && item.dateofStayEnd != null">
                                            {{item.dateofStayStart | dateFormat}} - {{item.dateofStayEnd | dateFormat}}
                                        </div>
                                        <div v-if="item.dateofStayStart == null && item.dateofStayEnd == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <input type="checkbox" v-model="item.isConfirmed" @change="updateTable()">
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-10">
                            <h4>Summary of Confirmed Training List</h4>
                        </div>
                       <div class="col-md-2">
                            <button class="btn talent_bg_green talent_font_white" @click.prevent="summaryConfirmedExcel">Download</button>
                        </div>
                    </div>
                    <br />

                    <h6>Confirmed List</h6>
                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter text-center">
                            <thead>
                                <tr>
                                    <th>
                                        No
                                    </th>
                                    <th>
                                        Employee Name
                                    </th>
                                    <th>
                                        Gender
                                    </th>
                                    <th>
                                        Dealer
                                    </th>
                                    <th>
                                        Outlet
                                    </th>
                                    <th>
                                        Accommodation
                                    </th>
                                    <th>
                                        Price
                                    </th>
                                    <th>
                                        Date of Stay
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(item, index) in trainingProcessDetail.trainingProcessDetailList.filter(Q=>Q.isConfirmed == true)" :key="item.trainingInvitationId">
                                    <td>
                                        {{index + 1}}
                                    </td>
                                    <td>
                                        {{item.employeeName}}
                                    </td>
                                    <td>
                                        <div v-if="item.gender != null">
                                            {{item.gender}}
                                        </div>

                                        <div v-if="item.gender == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        {{item.dealer}}
                                    </td>
                                    <td>
                                        {{item.outlet}}
                                    </td>
                                    <td>
                                        <div v-if="item.accommodation != null">
                                            {{item.accommodation}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.price != null">
                                            {{item.price}}
                                        </div>

                                        <div v-if="item.accommodation == null">
                                            -
                                        </div>
                                    </td>
                                    <td>
                                        <div v-if="item.dateofStayStart != null && item.dateofStayEnd != null">
                                            {{item.dateofStayStart | dateFormat}} - {{item.dateofStayEnd | dateFormat}}
                                        </div>
                                        <div v-if="item.dateofStayStart == null && item.dateofStayEnd == null">
                                            -
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <br />
                    <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter text-center">
                        <thead>
                            <tr>
                                <th>
                                    Gender
                                </th>
                                <th>
                                    Total Confirmed Participant
                                </th>
                                <th>
                                    Quota
                                </th>
                                <th>
                                    Total Room
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    Male
                                </td>
                                <td>
                                    {{maleTotalParticipant}}
                                </td>
                                <td>
                                    {{quotaTotal}}
                                </td>
                                <td>
                                    {{maleTotalRoom}}
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Female
                                </td>
                                <td>
                                    {{femaleTotalParticipant}}
                                </td>
                                <td>
                                    {{quotaTotal}}
                                </td>
                                <td>
                                    {{femaleTotalRoom}}
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-8"></div>
                        <div class="col-md-1">
                            <button class="btn btn-block talent_bg_red talent_font_white" @click.prevent="closeClicked()">Close</button>
                        </div>
                        <div class="col-md-1">
                            <button class="btn btn-block talent_bg_green talent_font_white" @click.prevent="saveClicked()">Save</button>
                        </div>
                        <div class="col-md-2">
                            <button class="btn btn-block talent_bg_blue talent_font_white" @click.prevent="sendInformation()">Send Information</button>
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
    import { TrainingProcessService, TrainingProcessModel, TrainingProcessViewModel, ProgramTypeService, ProgramTypeViewModel, TrainingProcessDetailViewModel, TrainingProcessDetailModel, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService';
    import { PageEnums } from '../../enum/PageEnums';
    import Axios from 'axios';
    //import { TrainingProcessExcelService } from '../../services/TrainingProcessExcelService';
    @Component({
        created: async function (this: TrainingProcess) {
            this.isBusy = true;
            await this.fetch();
            await this.getAccess();
            await this.initDropdownData();
        }
    })
    export default class TrainingProcess extends Vue {

        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.TrainingProcess);
            this.crud = data;
        }

         crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }
        isBusy: boolean = false;


        // ---------------------------------------- FETCH -----------------------------------------

        trainingProcessMan: TrainingProcessService = new TrainingProcessService();
        programTypeMan: ProgramTypeService = new ProgramTypeService();

        trainingProcess: TrainingProcessViewModel = {
            listTrainingProcess: null,
            totalItem: null
        };
        programTypes: ProgramTypeViewModel = {};

        currentPage: number = 1;
        filterDate = {
            start: null,
            end: null
        };
        filterCourseName: string = '';
        filterProgramType: string = '';
        filterCourseCategory: string = '';
        filterBatch: number = null;
        filterConfirmed: number = null;
        filterQuota: number = null;
        sortBy: string = '';

        // search, download
        confirmedListfilteredData: any = '';
        confirmedListDOM: any = '';
        waitingListfilteredData: any = '';
        waitingListDOM: any = '';
        summaryData: any = '';
        confirmedObj: any = '';
        waitingObj: any = '';
        summaryObj: any = '';
        
        async fetch() {
            this.isBusy = true;
            this.trainingProcess = await this.trainingProcessMan.getTrainingProcess(this.filterDate.start, this.filterDate.end, this.filterCourseName, this.filterProgramType, this.filterBatch, this.filterConfirmed, this.filterQuota, this.sortBy, this.currentPage);
            console.log(this.trainingProcess);
            this.isBusy = false;
        };

        async initDropdownData() {
            this.programTypes = await this.programTypeMan.getAllProgramTypes();

        }
        convertDateTime(stringdate) {
            if (stringdate === null) {
                return "-"
            }
            var date = new Date(stringdate);
            function pad(n) { return n < 10 ? "0" + n : n; }
            return pad(date.getDate()) + "/" + pad(date.getMonth() + 1) + "/" + date.getFullYear();
        }

        // ---------------------------------------- Sorting ------------------------------------------

        //Variable untuk sorting
        courseName = new Sort();
        programType = new Sort();
        batch = new Sort();
        confirmed = new Sort();
        quota = new Sort();
        startDate = new Sort();
        endDate = new Sort();
        updatedDate = new Sort();

        //Reset
        reset() {
            this.filterDate = {
                start: null,
                end: null
            };
            this.filterCourseName = '';
            this.filterProgramType = '';
            this.filterCourseCategory = '';
            this.filterConfirmed = null;
            this.filterQuota = null;
            this.filterBatch = null;
            this.sortBy = '';
            this.currentPage = 1;

            this.courseName.reset();
            this.programType.reset();
            this.batch.reset();
            this.confirmed.reset();
            this.quota.reset();
            this.startDate.reset();
            this.endDate.reset();
            this.updatedDate.reset();

            this.fetch();
        }

        //ClickSortCourseName
        async ClickSortCourseName() {
            this.ResetSort('courseName');
            //Sorting
            this.courseName.sorting();
            //Function Sorting
            if (this.courseName.sortup == true) {
                this.sortBy = 'courseName';
            }
            else if (this.courseName.sortdown == true) {
                this.sortBy = 'courseNameDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortProgramType
        async ClickSortProgramType() {
            this.ResetSort('programType');
            //Sorting
            this.programType.sorting();
            //Function Sorting

            if (this.programType.sortup == true) {
                this.sortBy = 'programType';
            }
            else if (this.programType.sortdown == true) {
                this.sortBy = 'programTypeDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortLearningType
        async ClickSortBatch() {
            this.ResetSort('Batch');
            //Sorting
            this.batch.sorting();
            //Function Sorting

            if (this.batch.sortup == true) {
                this.sortBy = 'batch';
            }
            else if (this.batch.sortdown == true) {
                this.sortBy = 'batchDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortLevel
        async ClickSortConfirmed() {
            this.ResetSort('confirmed');
            //Sorting
            this.confirmed.sorting();
            //Function Sorting

            if (this.confirmed.sortup == true) {
                this.sortBy = 'confirmed';
            }
            else if (this.confirmed.sortdown == true) {
                this.sortBy = 'confirmedDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortPricing
        async ClickSortQuota() {
            this.ResetSort('quota');
            //Sorting
            this.quota.sorting();
            //Function Sorting

            if (this.quota.sortup == true) {
                this.sortBy = 'quota';
            }
            else if (this.quota.sortdown == true) {
                this.sortBy = 'quotaDesc';
            }
            else {
                this.sortBy = '';
            }
            this.fetch();
        }

        //ClickSortApprovalStatus
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

        //ClickSortCreatedDate
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

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'courseName':
                    this.programType.reset();
                    this.batch.reset();
                    this.confirmed.reset();
                    this.quota.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'programType':
                    this.courseName.reset();
                    this.batch.reset();
                    this.confirmed.reset();
                    this.quota.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'batch':
                    this.courseName.reset();
                    this.programType.reset();
                    this.confirmed.reset();
                    this.quota.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'confirmed':
                    this.courseName.reset();
                    this.programType.reset();
                    this.batch.reset();
                    this.quota.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'quota':
                    this.courseName.reset();
                    this.programType.reset();
                    this.batch.reset();
                    this.confirmed.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    return;
                case 'startDate':
                    this.courseName.reset();
                    this.programType.reset();
                    this.batch.reset();
                    this.quota.reset();
                    this.confirmed.reset();
                    this.endDate.reset();
                    return;
                case 'endDate':
                    this.courseName.reset();
                    this.programType.reset();
                    this.batch.reset();
                    this.quota.reset();
                    this.confirmed.reset();
                    this.startDate.reset();
                    return;
            }
        }

        // ----------------------------------------- CRUD ---------------------------------------------

        //model
        courseById = {
            trainingId: 0,
            courseName: '',
            programType: '',
            batch: 0,
            confirmed: 0,
            quota: 0,
            trainingStartDate: Date,
            trainingEndDate: Date,
        }

        //Navigation
        edit: boolean = false;
        detail: boolean = false;

        trainingProcessDetail: TrainingProcessDetailViewModel = {
            trainingProcessDetailList: []
        }

        quotaTotal: number = 0;
        maleTotalParticipant = null;
        femaleTotalParticipant = null;
        maleTotalRoom = null;
        femaleTotalRoom = null;

        //Detail
        async detailClicked(courseById) {
            if(this.crud.read == false){
                return;
            }

            this.detail = true;
            this.edit = false;
            this.courseById = courseById;
            this.quotaTotal = this.courseById.quota;

            this.isBusy = true;

            let dataUpdate = await this.trainingProcessMan.getTrainingProcessById(courseById.trainingId);
            this.trainingProcessDetail = dataUpdate;
            
            this.confirmedListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter(Q=>Q.isConfirmed == true)
            this.waitingListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter(Q=>Q.isConfirmed == false)            
            this.summaryData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
                return (values.isConfirmed== true)
            })

            if (this.trainingProcessDetail.trainingProcessDetailList == null) {
                this.trainingProcessDetail.trainingProcessDetailList = [];
            }

            this.maleTotalParticipant = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Male').length;
            this.femaleTotalParticipant = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Female').length;

            this.maleTotalRoom = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Male' && Q.accommodation != null).length;
            this.femaleTotalRoom = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Female' && Q.accommodation != null).length;

            this.isBusy = false;
        }

        //Edit
        async editClicked(courseById) {
            if(this.crud.update == false){
                return;
            }
            this.edit = true;
            this.detail = false;
            this.courseById = courseById;
            this.quotaTotal = this.courseById.quota;

            this.isBusy = true;

            let dataUpdate = await this.trainingProcessMan.getTrainingProcessById(courseById.trainingId);
            this.trainingProcessDetail = dataUpdate;

            this.confirmedListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter(Q=>Q.isConfirmed == true)
            this.waitingListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter(Q=>Q.isConfirmed == false)
            this.summaryData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
                return (values.isConfirmed== true)
            })

            if (this.trainingProcessDetail.trainingProcessDetailList == null) {
                this.trainingProcessDetail.trainingProcessDetailList = [];
            }

            this.maleTotalParticipant = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Male').length;
            this.femaleTotalParticipant = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Female').length;

            this.maleTotalRoom = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Male' && Q.accommodation != null).length;
            this.femaleTotalRoom = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Female' && Q.accommodation != null).length;

            this.isBusy = false;
        }

        //Close
        closeClicked() {
            this.edit = false;
            this.detail = false;
        }

        updateTable() {
            this.quotaTotal = this.courseById.quota;
            this.maleTotalParticipant = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Male').length;
            this.femaleTotalParticipant = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Female').length;
            this.maleTotalRoom = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Male' && Q.accommodation != null).length;
            this.femaleTotalRoom = this.trainingProcessDetail.trainingProcessDetailList.filter(Q => Q.isJoined == true && Q.gender == 'Female' && Q.accommodation != null).length;
            this.waitingListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
                return (values.isConfirmed== false)
            })

            this.confirmedListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
                return (values.isConfirmed== true)
            })
        }

        async sendInformation() {
            this.isBusy = true;
            var isValid = await this.trainingProcessMan.updateTrainingProcessById(this.trainingProcessDetail, this.courseById.trainingId);

            if (isValid == false) {
                return;
            }

            this.closeClicked();
            this.isBusy = false;
        }

        async saveClicked() {
            this.isBusy = true;

            var isValid = await this.trainingProcessMan.saveDraftTrainingProcess(this.trainingProcessDetail, this.courseById.trainingId);

            if (isValid == false) {
                return;
            }

            this.closeClicked();
            this.isBusy = false;
        }

        confirmedtrainingListFilter() {
        // console.log(this.confirmedListDOM)
        // console.log(this.approvalTrainingDetail.employees)
        this.isBusy = true
        
        // trainingProcessDetail.trainingProcessDetailList.filter(Q=>Q.isConfirmed == true)
        this.confirmedListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
            values => values.isConfirmed == true
            console.log('PROC'+ values)
            return (values.employeeName.toLowerCase().includes(this.confirmedListDOM)) && (values.isConfirmed== true)
        })
        this.isBusy = false
        console.log('RES'+ this.confirmedListfilteredData)

      }

      waitingtrainingListFilter() {
        // console.log(this.confirmedListDOM)
        // console.log(this.approvalTrainingDetail.employees)
        this.isBusy = true
        
        // trainingProcessDetail.trainingProcessDetailList.filter(Q=>Q.isConfirmed == true)
        this.waitingListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
            values => values.isConfirmed == false
            console.log('PROC wait'+ values)
            return (values.employeeName.toLowerCase().includes(this.waitingListDOM)) && (values.isConfirmed== false)
        })
        this.isBusy = false
        console.log(this.waitingListfilteredData)

      }

      triggerConfirmedListCheckbox() {
        // console.log(this.confirmedListDOM)
        // console.log(this.approvalTrainingDetail.employees)
        this.isBusy = true
        this.confirmedListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
            return (values.isConfirmed== true)
        })
        this.waitingListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
            return (values.isConfirmed== false)
        })
        this.isBusy = false
        console.log(this.confirmedListfilteredData)
      }
      
      triggerWaitingListCheckbox() {
        // console.log(this.confirmedListDOM)
        // console.log(this.approvalTrainingDetail.employees)
        this.isBusy = true
        this.waitingListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
            return (values.isConfirmed== false)
        })

        this.confirmedListfilteredData = this.trainingProcessDetail.trainingProcessDetailList.filter((values,index) => {
            return (values.isConfirmed== true)
        })
        this.isBusy = false
        console.log(this.waitingListfilteredData)
      }

        /*async downloadReport() {
            this.isBusy = true;

            await TrainingProcessExcelService.trainingProcessExcelDownload(this.courseById.trainingId);

            this.isBusy = false;
        }*/

        confirmedListdownloadExcel() {
            // this.isBusy = true;
            //var response = await this.surveyReportMan.generateExcel(surveyId);
            this.confirmedObj = JSON.stringify(this.confirmedListfilteredData)
            return Axios({
                url: 'http://52.175.60.104/api/v1/training-process/export-training-process',
                method: 'POST',
                headers: { 
                    'accept': 'application/json', 
                    'Content-Type': 'application/json-patch+json'
                },
                responseType: 'blob',
                
                data: this.confirmedObj
            }).then((response) => {
                const url = window.URL.createObjectURL(new Blob([response.data]));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', 'TrainingList-ConfirmedList.xlsx');
                document.body.appendChild(link);
                link.click();
            });
            // this.isBusy = false;
        }

        waitingListdownloadExcel() {
            // this.isBusy = true;
            //var response = await this.surveyReportMan.generateExcel(surveyId);
            this.confirmedObj = JSON.stringify(this.waitingListfilteredData)
            return Axios({
                url: 'http://52.175.60.104/api/v1/training-process/export-training-process',
                method: 'POST',
                headers: { 
                    'accept': 'application/json', 
                    'Content-Type': 'application/json-patch+json'
                },
                responseType: 'blob',           
                data: this.confirmedObj
            }).then((response) => {
                const url = window.URL.createObjectURL(new Blob([response.data]));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', 'TrainingList-WaitingList.xlsx');
                document.body.appendChild(link);
                link.click();
            });
            // this.isBusy = false;
        }

        summaryConfirmedExcel() {
            // this.isBusy = true;
            //var response = await this.surveyReportMan.generateExcel(surveyId);
            console.log(this.summaryData);
            this.summaryObj = JSON.stringify(this.summaryData)
            return Axios({
                url: 'http://52.175.60.104/api/v1/training-process/export-training-process',
                method: 'POST',
                headers: { 
                    'accept': 'application/json', 
                    'Content-Type': 'application/json-patch+json'
                },
                responseType: 'blob',           
                data: this.summaryObj
            }).then((response) => {
                const url = window.URL.createObjectURL(new Blob([response.data]));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', 'Summary-WaitingList.xlsx');
                document.body.appendChild(link);
                link.click();
            });
            // this.isBusy = false;
        }

    }
</script>
