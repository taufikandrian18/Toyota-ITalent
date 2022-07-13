<template>
    <div class="row px-4 py-4 w-100">
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="col-md-12 p-0 mb-4" v-if="errored.length > 0">
            <div
                class="alert alert-danger alert-dismissible fade show"
                role="alert"
                v-if="errored.length > 0"
            >
                <ul>
                    <li v-for="error in errored" :key="error.key">
                        {{ error.value }}
                    </li>
                </ul>
            </div>
        </div>
        <div class="col-md-12 p-0">
            <!-- filter -->
            <div class="row w-100" v-if="!isDetail">
                <div class="col-md-12">
                    <h5>Search Learning Analysis Report</h5>
                </div>

                <div class="col-md-12 mt-4">
                    <div class="row">
                        <div class="col-md-6 mt-4">
                            <div class="row align-items-center">
                                <div class="col-md-12">
                                    <span>Program Type</span
                                    ><span class="talent_font_red">*</span>
                                </div>
                                <div class="col-md-12 mt-2">
                                    <div class="input-group">
                                        <multiselect
                                            id="programType"
                                            v-model="selectedProgramTypes"
                                            :options="programTypes"
                                            label="programTypeName"
                                            v-validate="'required'"
                                            :searchable="true"
                                            @select="handleChangeProgramType"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-4">
                            <div class="row align-items-center">
                                <div class="col-md-12">
                                    <span>Course</span>
                                </div>
                                <div class="col-md-12 mt-2">
                                    <div class="input-group">
                                        <multiselect
                                            v-model="selectedTraining"
                                            :options="trainings"
                                            label="courseName"
                                            v-validate="'required'"
                                            :searchable="true"
                                            @search-change="trainingSearch"
                                            :disabled="!selectedProgramTypes"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-4">
                            <div class="row align-items-center">
                                <div class="col-md-12">
                                    <span>Area</span>
                                </div>
                                <div class="col-md-12 mt-2">
                                    <div class="input-group">
                                        <multiselect
                                            v-model="selectedArea"
                                            :options="areas"
                                            label="name"
                                            v-validate="'required'"
                                            :searchable="true"
                                            @select="handleChangeArea"
                                            @remove="handleRemoveArea"
                                            :disabled="claims.DealerId"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-4">
                            <div class="row align-items-center">
                                <div class="col-md-12">
                                    <span>Area Specialist</span>
                                </div>
                                <div class="col-md-12 mt-2">
                                    <div class="input-group">
                                        <multiselect
                                            v-model="selectedAreaSpecialist"
                                            :options="areaSpecialists"
                                            label="areaSpecialistName"
                                            v-validate="'required'"
                                            :searchable="true"
                                            @select="handleChangeSpecial"
                                            @remove="handleRemoveSpecial"
                                            :disabled="claims.DealerId"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-4">
                            <div class="row align-items-center">
                                <div class="col-md-12">
                                    <span>Dealer</span>
                                </div>
                                <div class="col-md-12 mt-2">
                                    <div class="input-group">
                                        <multiselect
                                            v-model="selectedDealer"
                                            :options="filteredDealers"
                                            label="dealerName"
                                            trackBy="dealerId"
                                            v-validate="'required'"
                                            :searchable="true"
                                            :multiple="true"
                                            @select="handleChangeDealer"
                                            @remove="handleRemoveDealer"
                                            :disabled="claims.DealerId"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-4">
                            <div class="row align-items-center">
                                <div class="col-md-12">
                                    <span>Outlet</span>
                                </div>
                                <div class="col-md-12 mt-2">
                                    <div class="input-group">
                                        <multiselect
                                            v-model="selectedOutlet"
                                            :options="filteredOutlets"
                                            label="name"
                                            v-validate="'required'"
                                            :searchable="true"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-4">
                            <div class="row align-items-center">
                                <div class="col-md-12">
                                    <span>Employee ID</span>
                                </div>
                                <div class="col-md-12 mt-2">
                                    <div class="input-group">
                                        <!-- <multiselect
                                            v-model="selectedEmployee"
                                            :options="employees"
                                            label="name"
                                            v-validate="'required'"
                                            :searchable="true"
                                            @search-change="employeeSearch"
                                        /> -->
                                        <input name="name" type="text" class="form-control" v-model="selectedEmployee"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-4">
                            <div class="row align-items-center">
                                <div class="col-md-12">
                                    <span>Employee Name</span>
                                </div>
                                <div class="col-md-12 mt-2">
                                    <div class="input-group">
                                        <input
                                            name="name"
                                            type="text"
                                            class="form-control"
                                            v-model="name"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 mt-4">
                            <div class="row justify-content-end">
                                <div class="col-auto">
                                    <button
                                        class="btn talent_bg_cyan talent_roundborder talent_font_white"
                                        @click.prevent="_handleSearch"
                                    >
                                        Search
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- table -->
            <div class="row w-100 mt-4" v-if="!isDetail">
                <div class="col-md-12 mt-3">
                    <div class="row justify-content-between">
                        <div class="col">
                            <div class="dropdown">
                                <button
                                    class="btn btn-info dropdown-toggle mr-2"
                                    type="button"
                                    id="dropdownMenuButton"
                                    data-toggle="dropdown"
                                    aria-expanded="false"
                                >
                                    Filter
                                </button>
                                <div
                                    class="dropdown-menu p-3"
                                    aria-labelledby="dropdownMenuButton"
                                    style="min-width: 500px"
                                    @click="handlePreventMenuClose"
                                >
                                    <b>Filter</b>
                                    <div class="row">
                                        <div class="col mt-4">
                                            <div
                                                class="input-group talent_front"
                                            >
                                                <!-- <label for="">Status</label> -->
                                                <div
                                                    class="w-100 d-flex flex-wrap"
                                                >
                                                    <div
                                                        :class="
                                                            `${
                                                                data.isSelected
                                                                    ? 'filter-status-active'
                                                                    : 'filter-status'
                                                            } mx-2 my-2`
                                                        "
                                                        v-for="data in filterStatus"
                                                        :key="data.value"
                                                        @click="
                                                            data.isSelected = !data.isSelected
                                                        "
                                                    >
                                                        {{ data.label }}
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row d-flex flex-wrap">
                                        <hr class="w-100" />
                                        <div
                                            class="w-100 mt-1 d-flex justify-content-end px-3"
                                        >
                                            <button
                                                data-toggle="dropdown"
                                                class="btn btn-info mr-3"
                                                @click="handleFilter"
                                            >
                                                Apply
                                            </button>
                                            <button
                                                data-toggle="dropdown"
                                                class="btn btn-info mr-3"
                                                @click="handleResetFilter"
                                            >
                                                Reset
                                            </button>
                                            <button
                                                data-toggle="dropdown"
                                                class="btn btn-danger"
                                                @click="hideDropdown"
                                            >
                                                Cancel
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-auto">
                            <button
                                class="btn talent_bg_green talent_roundborder talent_font_white"
                                @click.prevent="_downloadReport"
                            >
                                Download
                            </button>
                        </div>
                    </div>
                </div>
                <div class="w-100 talent_overflowx mt-3">
                    <table
                        class="table table-bordered table-responsive-md talent_table_padding"
                    >
                        <thead>
                            <tr>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Program Type
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Course
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Area
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Area Specialist
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Dealer
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Outlet
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Position
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Employee ID
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Employee Name
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Learning Type
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Name
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Batch ke-X
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        First Access
                                    </a>
                                </th>
                                <th
                                    scope="col"
                                    class="text-center"
                                    style="max-width: 150px"
                                    rowspan="2"
                                >
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Last Access
                                    </a>
                                </th>
                                <th scope="col" class="text-center" colspan="3">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Total Study Time
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Average Score
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Highest Score
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Latest Score
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Final Score
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Result
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Minimum Score
                                    </a>
                                </th>
                                <th scope="col" class="text-center" rowspan="2">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                        @click="_handleDetail"
                                    >
                                        Number of Attempts
                                    </a>
                                </th>
                            </tr>
                            <tr>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Day
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Hour
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Minute
                                    </a>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(data, index) in datas" :key="index">
                                <td class="text-center">
                                    {{ data.programTypeName }}
                                </td>
                                <td class="text-center">
                                    {{ data.courseName }}
                                </td>
                                <td class="text-center">
                                    {{ data.areaName }}
                                </td>
                                <td class="text-center">
                                    {{ data.areaSpecialistName }}
                                </td>
                                <td class="text-center">
                                    {{ data.dealerName }}
                                </td>
                                <td class="text-center">
                                    {{ data.outletName }}
                                </td>
                                <td class="text-center">
                                    {{ data.positionName }}
                                </td>
                                <td class="text-center">
                                    {{ data.employeeId }}
                                </td>
                                <td class="text-center">
                                    {{ data.employeeName }}
                                </td>
                                <td class="text-center">
                                    {{ data.learningType }}
                                </td>
                                <td class="text-center">
                                    {{
                                        data.learningType == 'Assesment'
                                            ? data.assesmentName
                                            : data.moduleName
                                    }}
                                </td>
                                <td class="text-center">
                                    {{ data.batch }}
                                </td>
                                <td class="text-center">
                                    {{
                                        moment(
                                            data.firstAccess,
                                            'MMM DD YYYY LT'
                                        ).format('DD/MM/YYYY HH:mm:ss')
                                    }}
                                </td>
                                <td class="text-center">
                                    {{
                                        moment(
                                            data.lastAccess,
                                            'MMM DD YYYY LT'
                                        ).format('DD/MM/YYYY HH:mm:ss')
                                    }}
                                </td>
                                <td class="text-center">
                                    {{ data.day }}
                                </td>
                                <td class="text-center">
                                    {{ data.hour }}
                                </td>
                                <td class="text-center">
                                    {{ data.minute }}
                                </td>
                                <td class="text-center">
                                    {{ data.averageScore }}
                                </td>
                                <td class="text-center">
                                    {{ data.highestScore }}
                                </td>
                                <td class="text-center">
                                    {{ data.latestScore }}
                                </td>
                                <td class="text-center">
                                    {{ data.finalScore }}
                                </td>
                                <td class="text-center">
                                    {{ data.completionStatus }}
                                </td>
                                <td class="text-center">
                                    {{ data.minimumScore }}
                                </td>
                                <td class="text-center">
                                    <a v-if="data.attempts != '0'" href="#" @click="_handleDetail(data)">{{
                                        data.attempts
                                    }}</a>
                                    <span v-else>{{ data.attempts}}</span>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!--Pagination-->
                <div class="col-md-12 d-flex justify-content-center">
                    <paginate
                        :currentPage.sync="table.Page"
                        :total-data="totalData"
                        :limit-data="10"
                        @update:currentPage="_fetchReport()"
                    ></paginate>
                </div>
            </div>

            <!-- table Detail-->
            <div class="row w-100 mt-4" v-if="isDetail">
                <div class="col-md-12">
                    <div class="row justify-content-end">
                        <div class="col-auto">
                            <button
                                class="btn talent_bg_darkgrey talent_roundborder talent_font_black"
                                @click.prevent="_handleBack"
                            >
                                Back
                            </button>
                        </div>
                        <div class="col-auto">
                            <button
                                class="btn talent_bg_green talent_roundborder talent_font_white"
                                @click.prevent="_downloadReportDetail"
                            >
                                Download
                            </button>
                        </div>
                    </div>
                </div>
                <div class="w-100 talent_overflowx mt-4">
                    <table
                        class="table table-bordered table-responsive-md talent_table_padding"
                    >
                        <thead>
                            <tr>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Program Type
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Course
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Area
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Area Specialist
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Dealer
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Outlet
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Position
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Employee ID
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Employee Name
                                    </a>
                                </th>
                                <th
                                    scope="col"
                                    class="text-center"
                                    style="max-width: 150px"
                                >
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Attempted Count
                                    </a>
                                </th>
                                <th
                                    scope="col"
                                    class="text-center"
                                    style="max-width: 150px"
                                >
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Attempted Date
                                    </a>
                                </th>
                                <th
                                    v-if="dataDetail.learningType == 'Module'"
                                    scope="col"
                                    class="text-center"
                                >
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Question Type
                                    </a>
                                </th>
                                <th
                                    v-if="dataDetail.learningType == 'Module'"
                                    scope="col"
                                    class="text-center"
                                >
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Question
                                    </a>
                                </th>
                                <th
                                    v-if="dataDetail.learningType == 'Module'"
                                    scope="col"
                                    class="text-center"
                                >
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Answer
                                    </a>
                                </th>
                                <th
                                    v-if="
                                        dataDetail.learningType == 'Assesment'
                                    "
                                    scope="col"
                                    class="text-center"
                                >
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Skill Check
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Score
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Minimum Score
                                    </a>
                                </th>
                                <th scope="col" class="text-center">
                                    <a
                                        href="#"
                                        class="talent_sort"
                                        style="color: white;"
                                    >
                                        Total Score
                                    </a>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr
                                v-for="(data, index) in datasDetail"
                                :key="index"
                            >
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" class="text-center" >
                                    {{ dataDetail.programTypeName }}
                                </td>
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" class="text-center">
                                    {{ dataDetail.courseName }}
                                </td>
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" class="text-center">
                                    {{ dataDetail.areaName }}
                                </td>
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" class="text-center">
                                    {{ dataDetail.areaSpecialistName }}
                                </td>
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" class="text-center">
                                    {{ dataDetail.dealerName }}
                                </td>
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" class="text-center">
                                    {{ dataDetail.outletName }}
                                </td>
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" class="text-center">
                                    {{ dataDetail.positionName }}
                                </td>
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" >
                                    {{ data.employeeId }}
                                </td>
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" >
                                    {{ data.employeeName }}
                                </td>
                                <td
                                    class="text-center"
                                    v-if="
                                        index > 0
                                            ? datasDetail[index - 1]
                                                  .attemptedCount !=
                                              data.attemptedCount
                                            : true
                                    "
                                    :rowspan="
                                        datasDetail.filter(
                                            v =>
                                                v.attemptedCount ==
                                                data.attemptedCount
                                        ).length
                                    "
                                >
                                    {{ data.attemptedCount }}
                                </td>
                                <td v-if="index > 0 ? datasDetail[index - 1].attemptedCount != data.attemptedCount : true" :rowspan="datasDetail.filter(v => v.attemptedCount == data.attemptedCount).length" class="text-center">
                                    {{
                                        moment(
                                            data.attemptedDate,
                                            'MMM DD YYYY LT'
                                        ).format('DD/MM/YYYY HH:mm:ss')
                                    }}
                                </td>
                                <td v-if="dataDetail.learningType == 'Module'">
                                    {{ data.questionType }}
                                </td>
                                <td v-if="dataDetail.learningType == 'Module'">
                                    {{ data.questions }}
                                </td>
                                <td v-if="dataDetail.learningType == 'Module'">
                                    <a
                                        v-if="data.answers.includes('https://')"
                                        class="btn talent_bg_cyan talent_roundborder talent_font_white"
                                        :href="data.answers"
                                        target="_blank"
                                    >
                                        View Answer
                                    </a>
                                    <span v-else>{{ data.answers }}</span>
                                </td>
                                <td
                                    v-if="
                                        dataDetail.learningType == 'Assesment'
                                    "
                                >
                                    {{ data.skillCheckName }}
                                </td>
                                <td class="text-center">
                                    {{ data.score }}
                                </td>
                                <td
                                    class="text-center"
                                    v-if="
                                        index > 0
                                            ? datasDetail[index - 1]
                                                  .attemptedCount !=
                                              data.attemptedCount
                                            : true
                                    "
                                    :rowspan="
                                        datasDetail.filter(
                                            v =>
                                                v.attemptedCount ==
                                                data.attemptedCount
                                        ).length
                                    "
                                >
                                    {{ data.minimumScore }}
                                </td>
                                <td
                                    class="text-center"
                                    v-if="
                                        index > 0
                                            ? datasDetail[index - 1]
                                                  .attemptedCount !=
                                              data.attemptedCount
                                            : true
                                    "
                                    :rowspan="
                                        datasDetail.filter(
                                            v =>
                                                v.attemptedCount ==
                                                data.attemptedCount
                                        ).length
                                    "
                                >
                                    {{ data.totalScore }}
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <!--Pagination-->
                <div class="col-md-12 d-flex justify-content-center">
                    <paginate
                        :currentPage.sync="tableDetail.Page"
                        :total-data="totalDataDetail"
                        :limit-data="10"
                        @update:currentPage="_fetchReportDetail()"
                    ></paginate>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import moment from 'moment';
import { AssessmentService } from '../../services/Assessment/AssesmentService';
import { ReportService } from '../../services/MyLearning/ReportService';
import {
    DashboardGuestAccountService,
    ProgramTypeService,
    ReleaseTrainingService
} from '../../services/NSwagService';

import $ from 'jquery';

export default {
    props:['claims'],
    async mounted() {
        this.isBusy = true;
        this.trainings = await (
            await this.service.getReleaseTrainingReport(
                '',
                '',
                '',
                '',
                '',
                '',
                '',
                '',
                1,
                '',
                `${this.claims.DealerId ? this.claims.DealerId : ''}`
            )
        ).listTraining.map(v => ({
            ...v,
            courseName: `${v.courseName} (Batch-${v.batch})`
        }));
        this.programTypes = await (
            await this.serviceProgram.getAllProgramTypes()
        ).listProgramTypeModel;
        this.areas = await this.service.getAllArea();
        this.dealers = await this.service.getAllDealer();
        this.filteredDealers = this.dealers.sort((a,b) => {
                if ( a.dealerName < b.dealerName ){
                    return -1;
                }
                if ( a.dealerName > b.dealerName ){
                    return 1;
                }
                return 0;
            })
        this.outlets = await this.service.getAllOutlet();
        this.filteredOutlets = this.outlets
        // this.employees = await (
        //     await this.serviceEmployee.getAccounts({
        //         ...this.paramsEmployee,
        //         SearchQuery: ''
        //     })
        // ).data;
        this.areaSpecialists = await (await this.areaService.getAllAreaSpecialist({
            PageIndex: 1,
            PageSize: 500,
        })).areaSpecialists;
        this.isBusy = false;
          if(this.claims.DealerId) {
              this.selectedDealer = [...this.dealers.filter(v => v.dealerId === this.claims.DealerId)]
              this.handleChangeDealer({dealerId: this.claims.DealerId})
          }
          console.log(this.claims)

        // await this._fetchReport();
    },
    data() {
        return {
            moment: moment,
            isBusy: false,
            errored: [],
            trainings: [],
            dealers: [],
            filteredDealers: [],
            outlets: [],
            filteredOutlets: [],
            employees: [],
            programTypes: [],
            areas: [],
            areaSpecialists: [],
            isTrainingsLoading: false,
            reportService: new ReportService(),
            service: new ReleaseTrainingService(),
            serviceEmployee: new DashboardGuestAccountService(),
            serviceProgram: new ProgramTypeService(),
            areaService: new AssessmentService(),
            params: {
                TrainingId: '',
                DealerId: '',
                OutletID: '',
                EmployeeID: ''
            },
            selectedTraining: null,
            selectedDealer: null,
            selectedOutlet: null,
            selectedEmployee: null,
            selectedProgramTypes: null,
            selectedArea: null,
            selectedAreaSpecialist: null,
            paramsEmployee: {
                Page: 1,
                Limit: 100
            },
            name: '',
            // main table
            table: {
                Page: 1,
                Limit: 10
            },
            totalData: 0,
            datas: [],
            // detail table
            tableDetail: {
                Page: 1,
                Limit: 10
            },
            totalDataDetail: 0,
            datasDetail: [],
            isDetail: false,
            dataDetail: null,
            filterStatus: [
                {
                    label: 'Module',
                    value: 'Module',
                    isSelected: false
                },
                {
                    label: 'Competency Check',
                    value: 'Assesment',
                    isSelected: false
                }
            ]
        };
    },
    methods: {
        async _handleBack() {
            this.errored = [];
            this.isDetail = false;
        },
        async _fetchReport() {
            this.datas = [];
            this.errored = [];
            this.isBusy = true;
            try {
                const res = await this.reportService.getProgressTrackingReportJSON(
                    {
                        ProgramTypeId: this.selectedProgramTypes
                            ? this.selectedProgramTypes.programTypeId
                            : null,
                        TrainingId: this.selectedTraining
                            ? this.selectedTraining.trainingId
                            : null,
                        DealerId: this.selectedDealer && this.selectedDealer.length > 0
                            ? this.selectedDealer.map(v=>v.dealerId).join(',')
                            : '',
                        OutletID: this.selectedOutlet
                            ? this.selectedOutlet.outletId
                            : null,
                        AreaId: this.selectedArea
                            ? this.selectedArea.areaId
                            : null,
                        EmployeeID: this.selectedEmployee
                            ? this.selectedEmployee
                            : null,
                        EmployeeName: this.name,
                        AreaSpecialistId: this.selectedAreaSpecialist ? this.selectedAreaSpecialist.areaSpecialistId : null,
                        Page: this.table.Page,
                        Limit: this.table.Limit,
                        LearningType: this.getFilterLearningType()
                    }
                );
                this.datas = res.data.map(v => {
                    let day = 0;
                    let hour = 0;
                    let minute = 0;
                    const arr = v.totalStudyTime.split(':');
                    day = Math.floor(Number(arr[0]) / 24);
                    hour = Number(arr[0]) % 24;
                    minute = Number(arr[1]) + ':' + Number(arr[2]);
                    return {
                        ...v,
                        day: day,
                        hour: hour,
                        minute: minute
                    };
                });
                this.totalData = res.count;
                console.log(res);
            } catch (err) {
                this.errored.push({
                    key: 'error',
                    value: JSON.parse(err.response).data.en
                });
            }
            this.isBusy = false;
        },
        async _fetchReportDetail() {
            this.isBusy = true;
            this.datasDetail=[];
            this.totalDataDetail = 1;
            try {
                const res = await this.reportService.getProgressTrackingReportDetailJSON(
                    {
                        trainingId: this.dataDetail.trainingId,
                        setupModuleId: this.dataDetail.setupModuleId,
                        employeeId: this.dataDetail.employeeId,
                        flagging: this.dataDetail.learningType,
                        page: this.tableDetail.Page,
                        limit: this.tableDetail.Limit
                    }
                );
                this.datasDetail = res.data;
                this.totalDataDetail = res.count;
                console.log(res);
            } catch (err) {}
            this.isBusy = false;
        },
        async _handleSearch() {
            this.errored = [];
            if (this.selectedProgramTypes == null) {
                this.errored.push({
                    key: 'programtype',
                    value: 'Please fill program type'
                });
            }
            if (this.errored.length > 0) return;

            this.table = {
                Page: 1,
                Limit: 10
            };
            this._fetchReport();
        },
        async _handleDetail(data) {
            this.errored = [];
            this.tableDetail = {
                Page: 1,
                Limit: 10
            };
            this.isDetail = true;
            this.dataDetail = data;
            this._fetchReportDetail();
        },
        async _downloadReport() {
            this.errored = [];

            this.isBusy = true;
            try {
                const res = await this.reportService.getProgressTrackingReport({
                    ProgramTypeId: this.selectedProgramTypes
                        ? this.selectedProgramTypes.programTypeId
                        : null,
                    TrainingId: this.selectedTraining
                        ? this.selectedTraining.trainingId
                        : null,
                    DealerId: this.selectedDealer && this.selectedDealer.length > 0
                            ? this.selectedDealer.map(v=>v.dealerId).join(',')
                            : '',
                    OutletID: this.selectedOutlet
                        ? this.selectedOutlet.outletId
                        : null,
                    AreaId: this.selectedArea
                        ? this.selectedArea.areaId
                        : null,
                    EmployeeID: this.selectedEmployee
                        ? this.selectedEmployee
                        : null,
                    AreaSpecialistId: this.selectedAreaSpecialist ? this.selectedAreaSpecialist.areaSpecialistId : null,
                    EmployeeName: this.name
                });
            } catch (err) {
                this.errored.push({
                    key: 'no data',
                    value: 'No report'
                });
            }
            this.isBusy = false;
        },
        async _downloadReportDetail() {
            this.errored = [];

            this.isBusy = true;
            try {
                const res = await this.reportService.getProgressTrackingReportDetail(
                    {
                        trainingId: this.dataDetail.trainingId,
                        setupModuleId: this.dataDetail.setupModuleId,
                        employeeId: this.dataDetail.employeeId,
                        flagging: this.dataDetail.learningType
                    }
                );
            } catch (err) {
                this.errored.push({
                    key: 'no data',
                    value: 'No report'
                });
            }
            this.isBusy = false;
        },
        async trainingSearch(query) {
            if (query == null || query === '') {
                this.isTrainingsLoading = true;
                this.trainings = await (
                    await this.service.getReleaseTrainingReport(
                        '',
                        '',
                        '',
                        '',
                        '',
                        '',
                        '',
                        '',
                        1,
                        this.selectedProgramTypes.programTypeId,
                        `${this.claims.DealerId ? this.claims.DealerId : ''}`
                    )
                ).listTraining.map(v => ({
                    ...v,
                    courseName: `${v.courseName} (Batch-${v.batch})`
                }));
                this.isTrainingsLoading = false;
                return;
            }

            this.isTrainingsLoading = true;
            this.trainings = await (
                await this.service.getReleaseTrainingReport(
                    query,
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    1,
                    this.selectedProgramTypes.programTypeId,
                    `${this.claims.DealerId ? this.claims.DealerId : ''}`
                )
            ).listTraining.map(v => ({
                ...v,
                courseName: `${v.courseName} (Batch-${v.batch})`
            }));
            this.isTrainingsLoading = false;
        },
        async employeeSearch(query) {
            if (query == null || query === '') {
                this.isTrainingsLoading = true;
                this.employees = await (
                    await this.serviceEmployee.getAccounts({
                        ...this.paramsEmployee,
                        SearchQuery: query
                    })
                ).data;
                this.isTrainingsLoading = false;
                return;
            }

            this.isTrainingsLoading = true;
            this.employees = await (
                await this.serviceEmployee.getAccounts({
                    ...this.paramsEmployee,
                    SearchQuery: query
                })
            ).data;
            this.isTrainingsLoading = false;
        },
        handleFilter() {
            this.table = {
                Page: 1,
                Limit: 10
            };
            this._fetchReport();
            this.hideDropdown();
        },
        handleResetFilter() {
            this.filterStatus = this.filterStatus.map(v => ({
                ...v,
                isSelected: false
            }));
            this.table = {
                Page: 1,
                Limit: 10
            };
            this._fetchReport();
            this.hideDropdown();
        },
        hideDropdown() {
            $('#dropdownMenuButton').dropdown('hide');
        },
        handlePreventMenuClose(e) {
            e.stopPropagation();
        },
        getFilterLearningType() {
            const selected = this.filterStatus.filter(v => v.isSelected);
            if (selected.length == 1) {
                return selected[0].value;
            } else {
                return '';
            }
        },
        async handleChangeProgramType(a) {
            this.trainings = await (
                await this.service.getReleaseTrainingReport(
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    '',
                    1,
                    a.programTypeId,
                    `${this.claims.DealerId ? this.claims.DealerId : ''}`
                )
            ).listTraining.map(v => ({
                ...v,
                courseName: `${v.courseName} (Batch-${v.batch})`
            }));
        },
        handleChangeArea(area) {
            // reset
            this.selectedDealer = null
            this.selectedOutlet = null

            const findedOutlets = this.outlets.filter(v => v.areaId === area.areaId)
            const dealerIds = findedOutlets.map(v => v.dealerId)
            const findedDealers = this.dealers.filter(v => dealerIds.includes(v.dealerId))

            this.filteredOutlets = findedOutlets
            this.filteredDealers = findedDealers.sort((a,b) => {
                if ( a.dealerName < b.dealerName ){
                    return -1;
                }
                if ( a.dealerName > b.dealerName ){
                    return 1;
                }
                return 0;
            })
        },
        handleRemoveArea(area) {
            // reset
            this.selectedDealer = null
            this.selectedOutlet = null

            this.filteredOutlets = this.outlets
            this.filteredDealers = this.dealers.sort((a,b) => {
                if ( a.dealerName < b.dealerName ){
                    return -1;
                }
                if ( a.dealerName > b.dealerName ){
                    return 1;
                }
                return 0;
            })
        },
        handleChangeDealer(dealer) {
            // reset
            this.selectedOutlet = null

            const findedOutlets = this.outlets.filter(v => v.dealerId === dealer.dealerId)

            this.filteredOutlets = findedOutlets
        },
        handleRemoveDealer(dealer) {
            // reset
            this.selectedOutlet = null

            this.filteredOutlets = this.outlets
        },
        handleChangeSpecial(special) {
            // reset
            this.selectedOutlet = null
            const outlets = special.outlets.map(v => v.outletId)

            const findedOutlets = this.outlets.filter(v => outlets.includes(v.outletId))

            this.filteredOutlets = findedOutlets
        },
        handleRemoveSpecial(special) {
            // reset
            this.selectedOutlet = null

            this.filteredOutlets = this.outlets
        }
    },
    watch: {
        isDetail() {
            this.$emit('change', this.isDetail);
        }
    }
};
</script>

<style scoped>
.filter-status {
    border-radius: 9999px;
    color: #9e9e9e;
    border: 1px solid #9e9e9e;
    padding: 4px 6px;
    cursor: pointer;
}
.filter-status-active {
    border-radius: 9999px;
    color: white;
    border: 1px solid #007bff;
    background-color: #007bff;
    padding: 4px 6px;
    cursor: pointer;
}
</style>
