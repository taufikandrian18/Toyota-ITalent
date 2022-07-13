<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div
            class="alert alert-success alert-dismissible fade show"
            role="alert"
            v-if="success"
        >
            {{ successMessage }}
            <button
                type="button"
                class="close"
                data-dismiss="alert"
                aria-label="Close"
                @clicked="alertClose()"
            >
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <div class="row">
            <div class="col-md-12">
                <!--TITLE-->
                <h3 v-if="add == false && edit == false && detail == false">
                    <fa icon="database"></fa> Master Data >
                    <span class="talent_font_red">Course</span>
                </h3>
                <h3 v-if="add == true && edit == false && detail == false">
                    <fa icon="database"></fa> Master Data > Course >
                    <span class="talent_font_red">Add Course</span>
                </h3>
                <h3 v-if="add == false && edit == true && detail == false">
                    <fa icon="database"></fa> Master Data > Course >
                    <span class="talent_font_red">Edit Course</span>
                </h3>
                <h3 v-if="add == false && edit == false && detail == true">
                    <fa icon="database"></fa> Master Data > Course >
                    <span class="talent_font_red">Detail Course</span>
                </h3>
                <br />
            </div>
        </div>

        <!--View-->
        <div
            class="row"
            v-if="add == false && edit == false && detail == false"
        >
            <div class="col col-md-12">
                <!--ADVANCE SEARCH-->
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Search Course</h4>

                    <br />
                    <!--4 Column Menu-->
                    <div class="row">
                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker
                                            class="v-date-style-report"
                                            mode="range"
                                            :firstDayOfWeek="2"
                                            v-model="filterDate"
                                            :masks="{ input: 'DD/MM/YYYY' }"
                                        ></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"
                                                ><fa icon="calendar-alt"></fa
                                            ></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Program Type</span>
                                </div>
                                <div class="col-md-8">
                                    <select
                                        class="form-control"
                                        v-model="filterProgramType"
                                    >
                                        <option
                                            v-for="p in programTypes.listProgramTypeModel"
                                            :key="p.id"
                                            :value="p.programTypeName"
                                            >{{ p.programTypeName }}</option
                                        >
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>By Category</span>
                                </div>
                                <div class="col-md-8">
                                    <select
                                        class="form-control"
                                        v-model="filterCourseCategory"
                                    >
                                        <option
                                            v-for="c in courseCategories.listCourseCategoryModel"
                                            :key="c.id"
                                            :value="c.courseCategoryName"
                                            >{{ c.courseCategoryName }}</option
                                        >
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Approval Status</span>
                                </div>
                                <div class="col-md-8">
                                    <select
                                        class="form-control"
                                        v-model="filterApprovalName"
                                    >
                                        <option
                                            v-for="a in approvals.listApprovalStatusModel"
                                            :key="a.id"
                                            :value="a.approvalName"
                                            >{{ a.approvalName }}</option
                                        >
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
                                <div class="col-md-4">
                                    <span>Course Name</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="filterCourseName"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Learning Type</span>
                                </div>
                                <div class="col-md-8">
                                    <select
                                        class="form-control"
                                        v-model="filterLearningType"
                                    >
                                        <option
                                            v-for="l in learningTypes.listLearningTypeModel"
                                            :key="l.id"
                                            :value="l.learningName"
                                            >{{ l.learningName }}</option
                                        >
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-3">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Pricing</span>
                                </div>
                                <div class="col-md-8">
                                    <select
                                        class="form-control"
                                        v-model="filterPricingType"
                                    >
                                        <option
                                            v-for="p in pricingTypes"
                                            :key="p.id"
                                            :value="p"
                                            >{{ p }}</option
                                        >
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
                                        <button
                                            class="btn talent_bg_blue talent_font_white"
                                            @click.prevent="fetch"
                                        >
                                            Search
                                        </button>
                                        <button
                                            class="btn talent_bg_red talent_font_white"
                                            @click.prevent="reset"
                                        >
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
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Course List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div
                            class="row align-items-center row justify-content-between"
                        >
                            <a
                                >Showing
                                {{
                                    courses.listCourseJoinModel == null
                                        ? 0
                                        : courses.listCourseJoinModel.length
                                }}
                                of {{ courses.totalItem }} Entry(s)</a
                            >
                            <button
                                v-if="crud.create"
                                class="btn talent_bg_cyan talent_roundborder talent_font_white"
                                @click.prevent="addClicked()"
                            >
                                Add Course
                            </button>
                        </div>
                    </div>
                    <div class="talent_overflowx">
                        <table
                            class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter"
                        >
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortCourseName()
                                            "
                                            >Course Name
                                            <fa
                                                icon="sort"
                                                v-if="courseName.sort == true"
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="courseName.sortup == true"
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    courseName.sortdown == true
                                                "
                                            ></fa
                                        ></a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortProgramType()
                                            "
                                            >Program Type
                                            <fa
                                                icon="sort"
                                                v-if="programType.sort == true"
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="
                                                    programType.sortup == true
                                                "
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    programType.sortdown == true
                                                "
                                            ></fa
                                        ></a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortLearningType()
                                            "
                                            >Learning Type
                                            <fa
                                                icon="sort"
                                                v-if="learningType.sort == true"
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="
                                                    learningType.sortup == true
                                                "
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    learningType.sortdown ==
                                                        true
                                                "
                                            ></fa
                                        ></a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortCourseCategory()
                                            "
                                            >By Category
                                            <fa
                                                icon="sort"
                                                v-if="
                                                    courseCategory.sort == true
                                                "
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="
                                                    courseCategory.sortup ==
                                                        true
                                                "
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    courseCategory.sortdown ==
                                                        true
                                                "
                                            ></fa
                                        ></a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="ClickSortPricing()"
                                            >Pricing
                                            <fa
                                                icon="sort"
                                                v-if="pricing.sort == true"
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="pricing.sortup == true"
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="pricing.sortdown == true"
                                            ></fa
                                        ></a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortApprovalStatus()
                                            "
                                            >Approval Status
                                            <fa
                                                icon="sort"
                                                v-if="
                                                    approvalStatus.sort == true
                                                "
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="
                                                    approvalStatus.sortup ==
                                                        true
                                                "
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    approvalStatus.sortdown ==
                                                        true
                                                "
                                            ></fa
                                        ></a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortCreatedDate()
                                            "
                                            >Created Date
                                            <fa
                                                icon="sort"
                                                v-if="createdDate.sort == true"
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="
                                                    createdDate.sortup == true
                                                "
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    createdDate.sortdown == true
                                                "
                                            ></fa
                                        ></a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortUpdatedDate()
                                            "
                                            >Updated Date
                                            <fa
                                                icon="sort"
                                                v-if="updatedDate.sort == true"
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="
                                                    updatedDate.sortup == true
                                                "
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    updatedDate.sortdown == true
                                                "
                                            ></fa
                                        ></a>
                                    </th>
                                    <th colspan="3">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr
                                    v-for="(c,
                                    index) in courses.listCourseJoinModel"
                                    :key="c.id"
                                >
                                    <td>
                                        {{ c.courseName }}
                                    </td>
                                    <td>
                                        {{ c.programTypeName }}
                                    </td>
                                    <td>
                                        {{ c.learningName }}
                                    </td>
                                    <td>
                                        {{ c.courseCategoryName }}
                                    </td>
                                    <td>
                                        {{ c.coursePrice > 0 ? 'Pay' : 'Free' }}
                                    </td>
                                    <td>
                                        {{ c.approvalName }}
                                    </td>
                                    <td>
                                        {{ convertDateTime(c.createdAt) }}
                                    </td>
                                    <td>
                                        {{ convertDateTime(c.updatedAt) }}
                                    </td>
                                    <td
                                        v-if="crud.read"
                                        class="talent_nopadding_important"
                                    >
                                        <button
                                            :disabled="!crud.read"
                                            class="btn btn-block talent_bg_orange talent_font_white"
                                            @click.prevent="
                                                detailClicked(c.courseId)
                                            "
                                        >
                                            View Detail
                                        </button>
                                    </td>
                                    <td
                                        v-if="crud.update"
                                        class="talent_nopadding_important"
                                    >
                                        <button
                                            class="btn btn-block talent_bg_cyan talent_font_white"
                                            @click.prevent="
                                                editClicked(c.courseId)
                                            "
                                            :disabled="
                                                c.approvalName ==
                                                    'Waiting for Approval'
                                            "
                                        >
                                            Edit
                                        </button>
                                    </td>
                                    <td
                                        v-if="crud.delete"
                                        class="talent_nopadding_important"
                                    >
                                        <button
                                            class="btn btn-block talent_bg_red talent_font_white"
                                            data-toggle="modal"
                                            data-target="#exampleModalCenter"
                                            @click.prevent="
                                                deleteClicked(c.courseId, index)
                                            "
                                            :disabled="
                                                c.approvalName ==
                                                    'Waiting for Approval'
                                            "
                                        >
                                            Remove
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--Pagination-->
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate
                            :currentPage.sync="currentPage"
                            :total-data="courses.totalItem"
                            :limit-data="itemPerPage"
                            @update:currentPage="fetch()"
                        ></paginate>
                    </div>
                </div>

                <div
                    class="modal fade"
                    id="exampleModalCenter"
                    tabindex="-1"
                    role="dialog"
                    aria-labelledby="exampleModalCenterTitle"
                    aria-hidden="true"
                >
                    <div
                        class="modal-dialog modal-dialog-centered modal-lg"
                        role="document"
                    >
                        <div class="modal-content">
                            <div class="modal-header">
                                <div
                                    class="col-md-12 justify-content-center d-flex"
                                >
                                    <h5>Are you sure want to delete?</h5>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <div
                                    class="col-md-12 d-flex justify-content-center"
                                >
                                    <div
                                        class="col-md-12 d-flex justify-content-around"
                                    >
                                        <button
                                            class="btn talent_bg_red talent_font_white talent_roundborder"
                                            type="button"
                                            data-dismiss="modal"
                                            @click.prevent="deleteData()"
                                        >
                                            Delete
                                        </button>
                                        <button
                                            class="btn talent_bg_blue talent_font_white talent_roundborder"
                                            type="button"
                                            data-dismiss="modal"
                                        >
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

        <!--Add-->
        <div class="row" v-if="add == true && edit == false && detail == false">
            <div class="col-md-12">
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Course Information</h4>

                    <div
                        v-if="
                            fileType === false ||
                                fileSize === false ||
                                imageSize === false
                        "
                        class="alert alert-danger"
                    >
                        <div v-if="imageSize === false">
                            Maximum Image Size is 300x300
                        </div>
                        <div v-if="fileSize === false">
                            Maximum File Size is 5 MB
                        </div>
                        <div v-if="fileType === false">
                            Please input an image file (jpg/png/jpeg)
                        </div>
                    </div>

                    <div
                        v-if="validateAdd() === false"
                        class="alert alert-danger"
                    >
                        <div v-if="addModel.programTypeId === 0">
                            Program Type is Required
                        </div>
                        <div v-if="addModel.levelId === 0">
                            Level is Required
                        </div>
                        <div v-if="addModel.learningTypeId === 0">
                            Learning Type is Required
                        </div>
                        <div v-if="addModel.courseName === ''">
                            Course Name is Required
                        </div>
                        <div
                            v-if="
                                (addModel.coursePrice <= 0 &&
                                    addPricing == 'Pay') ||
                                    addModel.coursePrice.toString() == ''
                            "
                        >
                            Input Amount
                        </div>
                        <div
                            v-if="
                                addModel.coursePrice > 1000000000 &&
                                    addPricing == 'Pay'
                            "
                        >
                            Amount is Invalid
                        </div>
                        <div v-if="addImageName === ''">Image is Required</div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label
                                        >Course Name<span
                                            class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="addModel.courseName"
                                            maxlength="255"
                                        />
                                    </div>
                                    <br />
                                    <div class="row justify-content-between">
                                        <div class="col-md-4">
                                            <img
                                                :src="addImageData"
                                                alt="Alternate Text"
                                                class="img-fluid"
                                            />
                                        </div>
                                        <div class="col-md-8">
                                            <h5>File Upload</h5>
                                            <label
                                                >Upload Image<span
                                                    class="talent_font_red"
                                                    >*</span
                                                ></label
                                            >
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div
                                                        class="row justify-content-between"
                                                    >
                                                        <div class="col-md-8">
                                                            <div
                                                                class="custom-file"
                                                            >
                                                                <input
                                                                    type="file"
                                                                    class="custom-file-input"
                                                                    id="addCustomFile"
                                                                    @change="
                                                                        onAddFileChange
                                                                    "
                                                                />
                                                                <label
                                                                    class="custom-file-label talent_textshorten"
                                                                    for="addCustomFile"
                                                                    >{{
                                                                        addImageName ==
                                                                        null
                                                                            ? 'Choose File'
                                                                            : addImageName
                                                                    }}</label
                                                                >
                                                            </div>
                                                        </div>
                                                        <div
                                                            class="col-md-4 font_size_12"
                                                        >
                                                            *Max File Size 5MB
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Course Description</label>
                                    <textarea
                                        class="form-control"
                                        style="height:150px;overflow-y:scroll;"
                                        v-model="addModel.courseDescription"
                                        maxlength="1024"
                                    ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label
                                                        >Program Type<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        ></label
                                                    >
                                                    <div class="input-group">
                                                        <select
                                                            class="form-control"
                                                            v-model="
                                                                addModel.programTypeId
                                                            "
                                                            @change="
                                                                changeLevelList(
                                                                    'add'
                                                                )
                                                            "
                                                        >
                                                            <option
                                                                v-for="p in programTypes.listProgramTypeModel"
                                                                :key="p.id"
                                                                :value="
                                                                    p.programTypeId
                                                                "
                                                                >{{
                                                                    p.programTypeName
                                                                }}</option
                                                            >
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label
                                                        >Level<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        ></label
                                                    >
                                                    <div class="input-group">
                                                        <select
                                                            class="form-control"
                                                            v-model="
                                                                addModel.levelId
                                                            "
                                                            :disabled="
                                                                disableLevel
                                                            "
                                                        >
                                                            <option
                                                                v-for="l in levels.listLevelModel"
                                                                :key="l.id"
                                                                :value="
                                                                    l.levelId
                                                                "
                                                                >{{
                                                                    l.levelName
                                                                }}</option
                                                            >
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label
                                                >Pricing<span
                                                    class="talent_font_red"
                                                    >*</span
                                                ></label
                                            >
                                            <div class="input-group">
                                                <select
                                                    class="form-control"
                                                    v-model="addPricing"
                                                >
                                                    <option
                                                        v-for="p in pricingTypes"
                                                        :key="p.id"
                                                        :value="p"
                                                        >{{ p }}</option
                                                    >
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label>By Category</label>
                                                    <div class="input-group">
                                                        <select
                                                            class="form-control"
                                                            v-model="
                                                                addModel.courseCategoryId
                                                            "
                                                        >
                                                            <option
                                                                v-for="c in courseCategories.listCourseCategoryModel"
                                                                :key="c.id"
                                                                :value="
                                                                    c.courseCategoryId
                                                                "
                                                                >{{
                                                                    c.courseCategoryName
                                                                }}</option
                                                            >
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label
                                                        >Learning Type<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        ></label
                                                    >
                                                    <div class="input-group">
                                                        <select
                                                            class="form-control"
                                                            v-model="
                                                                addModel.learningTypeId
                                                            "
                                                        >
                                                            <option
                                                                v-for="l in learningTypes.listLearningTypeModel"
                                                                :key="l.id"
                                                                :value="
                                                                    l.learningTypeId
                                                                "
                                                                >{{
                                                                    l.learningName
                                                                }}</option
                                                            >
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label>Amount</label>
                                            <div class="input-group">
                                                <!--<input class="form-control" type="number" min="1" v-model="addModel.coursePrice" :disabled="addPricing == 'Free' ? true : false" />-->
                                                <money
                                                    class="form-control"
                                                    v-model="
                                                        addModel.coursePrice
                                                    "
                                                    v-bind="money"
                                                    :disabled="
                                                        addPricing == 'Free'
                                                            ? true
                                                            : false
                                                    "
                                                ></money>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label>Others</label>
                                                    <div class="input-group">
                                                        <input
                                                            class="form-control"
                                                            v-model="
                                                                addModel.others
                                                            "
                                                            :disabled="
                                                                addModel.courseCategoryId !=
                                                                4
                                                                    ? true
                                                                    : false
                                                            "
                                                            maxlength="255"
                                                        />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button
                                        class="btn talent_bg_red talent_font_white"
                                        @click.prevent="closeClicked()"
                                    >
                                        Close
                                    </button>
                                    <button
                                        class="btn talent_bg_lightgreen talent_font_white"
                                        @click.prevent="addData(3)"
                                    >
                                        Save
                                    </button>
                                    <button
                                        class="btn talent_bg_darkblue talent_font_white"
                                        @click.prevent="addData(2)"
                                    >
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
        <div class="row" v-if="edit == true && add == false && detail == false">
            <div class="col-md-12">
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Course Information</h4>

                    <div
                        v-if="
                            fileType === false ||
                                fileSize === false ||
                                imageSize === false
                        "
                        class="alert alert-danger"
                    >
                        <div v-if="imageSize === false">
                            Maximum Image Size is 300x300
                        </div>
                        <div v-if="fileSize === false">
                            Maximum File Size is 5 MB
                        </div>
                        <div v-if="fileType === false">
                            Please input an image file (jpg/png/jpeg)
                        </div>
                    </div>

                    <div
                        v-if="validateEdit() === false"
                        class="alert alert-danger"
                    >
                        <div v-if="editModel.courseName === ''">
                            Course Name is Required
                        </div>
                        <div v-if="editModel.levelId == undefined">
                            Level is Required
                        </div>
                        <div
                            v-if="
                                (editModel.coursePrice <= 0 &&
                                    editPricing == 'Pay') ||
                                    editModel.coursePrice.toString() == ''
                            "
                        >
                            Input Amount
                        </div>
                        <div
                            v-if="
                                editModel.coursePrice > 1000000000 &&
                                    editPricing == 'Pay'
                            "
                        >
                            Amount is Invalid
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label
                                        >Course Name<span
                                            class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="editModel.courseName"
                                            maxlength="255"
                                        />
                                    </div>
                                    <br />
                                    <div class="row justify-content-between">
                                        <div class="col-md-4">
                                            <img
                                                :src="editImageData"
                                                alt="Alternate Text"
                                                class="img-fluid"
                                            />
                                        </div>
                                        <div class="col-md-8">
                                            <h5>File Upload</h5>
                                            <label
                                                >Upload Image<span
                                                    class="talent_font_red"
                                                    >*</span
                                                ></label
                                            >
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div
                                                        class="row justify-content-between"
                                                    >
                                                        <div class="col-md-8">
                                                            <div
                                                                class="custom-file"
                                                            >
                                                                <input
                                                                    type="file"
                                                                    class="custom-file-input"
                                                                    id="editCustomFile"
                                                                    @change="
                                                                        onEditFileChange
                                                                    "
                                                                />
                                                                <label
                                                                    class="custom-file-label talent_textshorten"
                                                                    for="editCustomFile"
                                                                    >{{
                                                                        editImageName ==
                                                                        null
                                                                            ? 'Choose File'
                                                                            : editImageName
                                                                    }}</label
                                                                >
                                                            </div>
                                                        </div>
                                                        <div
                                                            class="col-md-4 font_size_12"
                                                        >
                                                            *Max File Size 5MB
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Course Description</label>
                                    <textarea
                                        class="form-control"
                                        style="height:150px;overflow-y:scroll;"
                                        v-model="editModel.courseDescription"
                                        maxlength="1024"
                                    ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label
                                                        >Program Type<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        ></label
                                                    >
                                                    <div class="input-group">
                                                        <select
                                                            class="form-control"
                                                            v-model="
                                                                editModel.programTypeId
                                                            "
                                                            @change="
                                                                changeLevelList(
                                                                    'edit'
                                                                )
                                                            "
                                                        >
                                                            <option
                                                                v-for="p in programTypes.listProgramTypeModel"
                                                                :key="p.id"
                                                                :value="
                                                                    p.programTypeId
                                                                "
                                                                >{{
                                                                    p.programTypeName
                                                                }}</option
                                                            >
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label
                                                        >Level<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        ></label
                                                    >
                                                    <div class="input-group">
                                                        <select
                                                            class="form-control"
                                                            v-model="
                                                                editModel.levelId
                                                            "
                                                            :disabled="
                                                                disableLevel
                                                            "
                                                        >
                                                            <option
                                                                v-for="l in levels.listLevelModel"
                                                                :key="l.id"
                                                                :value="
                                                                    l.levelId
                                                                "
                                                                >{{
                                                                    l.levelName
                                                                }}</option
                                                            >
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label
                                                >Pricing<span
                                                    class="talent_font_red"
                                                    >*</span
                                                ></label
                                            >
                                            <select
                                                class="form-control"
                                                v-model="editPricing"
                                            >
                                                <option
                                                    v-for="p in pricingTypes"
                                                    :key="p.id"
                                                    :value="p"
                                                    >{{ p }}</option
                                                >
                                            </select>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label>By Category</label>
                                                    <div class="input-group">
                                                        <select
                                                            class="form-control"
                                                            v-model="
                                                                editModel.courseCategoryId
                                                            "
                                                        >
                                                            <option
                                                                v-for="c in courseCategories.listCourseCategoryModel"
                                                                :key="c.id"
                                                                :value="
                                                                    c.courseCategoryId
                                                                "
                                                                >{{
                                                                    c.courseCategoryName
                                                                }}</option
                                                            >
                                                        </select>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label
                                                        >Learning Type<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        ></label
                                                    >
                                                    <div class="input-group">
                                                        <select
                                                            class="form-control"
                                                            v-model="
                                                                editModel.learningTypeId
                                                            "
                                                        >
                                                            <option
                                                                v-for="l in learningTypes.listLearningTypeModel"
                                                                :key="l.id"
                                                                :value="
                                                                    l.learningTypeId
                                                                "
                                                                >{{
                                                                    l.learningName
                                                                }}</option
                                                            >
                                                        </select>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label>Amount</label>
                                            <div class="input-group">
                                                <!--<input class="form-control" type="number" min="1" v-model="editModel.coursePrice" :disabled="editPricing == 'Free' ? true : false" />-->
                                                <money
                                                    class="form-control"
                                                    v-model="
                                                        editModel.coursePrice
                                                    "
                                                    v-bind="money"
                                                    :disabled="
                                                        editPricing == 'Free'
                                                            ? true
                                                            : false
                                                    "
                                                ></money>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label>Others</label>
                                                    <div class="input-group">
                                                        <input
                                                            class="form-control"
                                                            v-model="
                                                                editModel.others
                                                            "
                                                            :disabled="
                                                                editModel.courseCategoryId !=
                                                                4
                                                                    ? true
                                                                    : false
                                                            "
                                                            maxlength="255"
                                                        />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button
                                        class="btn talent_bg_red talent_font_white"
                                        @click.prevent="closeClicked"
                                    >
                                        Close
                                    </button>
                                    <button
                                        class="btn talent_bg_lightgreen talent_font_white"
                                        @click.prevent="editData(3)"
                                    >
                                        Save
                                    </button>
                                    <button
                                        class="btn talent_bg_darkblue talent_font_white"
                                        @click.prevent="editData(2)"
                                    >
                                        Submit
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--View Detail-->
        <div class="row" v-if="detail == true && edit == false && add == false">
            <div class="col-md-12">
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Course Information</h4>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label
                                        >Course Name<span
                                            class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="detailModel.courseName"
                                            disabled
                                        />
                                    </div>
                                    <br />
                                    <div class="row justify-content-between">
                                        <div class="col-md-4">
                                            <img
                                                :src="detailImageData"
                                                alt="Alternate Text"
                                                class="img-fluid"
                                            />
                                        </div>
                                        <div class="col-md-8">
                                            <h5>File Upload</h5>
                                            <label
                                                >Upload Image<span
                                                    class="talent_font_red"
                                                    >*</span
                                                ></label
                                            >
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div
                                                        class="row justify-content-between"
                                                    >
                                                        <div class="col-md-8">
                                                            <div
                                                                class="custom-file"
                                                            >
                                                                <input
                                                                    type="file"
                                                                    class="custom-file-input"
                                                                    id="detailCustomFile"
                                                                    disabled
                                                                />
                                                                <label
                                                                    class="custom-file-label talent_textshorten"
                                                                    for="detailCustomFile"
                                                                    >{{
                                                                        detailImageName ==
                                                                        null
                                                                            ? 'Choose File'
                                                                            : detailImageName
                                                                    }}</label
                                                                >
                                                            </div>
                                                        </div>
                                                        <div
                                                            class="col-md-4 font_size_12"
                                                        >
                                                            *Max File Size 5MB
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>Course Description</label>
                                    <textarea
                                        class="form-control"
                                        style="height:150px;overflow-y:scroll;"
                                        v-model="detailModel.courseDescription"
                                        disabled
                                    ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label
                                                        >Program Type<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        ></label
                                                    >
                                                    <div class="input-group">
                                                        <input
                                                            class="form-control"
                                                            v-model="
                                                                detailModel.programTypeName
                                                            "
                                                            disabled
                                                        />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label
                                                        >Level<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        ></label
                                                    >
                                                    <div class="input-group">
                                                        <input
                                                            class="form-control"
                                                            v-model="
                                                                detailModel.levelName
                                                            "
                                                            disabled
                                                        />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label
                                                >Pricing<span
                                                    class="talent_font_red"
                                                    >*</span
                                                ></label
                                            >
                                            <div class="input-group">
                                                <input
                                                    class="form-control"
                                                    v-model="detailPricing"
                                                    disabled
                                                />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label>By Category</label>
                                                    <div class="input-group">
                                                        <input
                                                            class="form-control"
                                                            v-model="
                                                                detailModel.courseCategoryName
                                                            "
                                                            disabled
                                                        />
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <label
                                                        >Learning Type<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        ></label
                                                    >
                                                    <div class="input-group">
                                                        <input
                                                            class="form-control"
                                                            v-model="
                                                                detailModel.learningName
                                                            "
                                                            disabled
                                                        />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label>Amount</label>
                                            <div class="input-group">
                                                <!--<input class="form-control" v-model="detailModel.coursePrice" disabled />-->
                                                <money
                                                    class="form-control"
                                                    v-model="
                                                        detailModel.coursePrice
                                                    "
                                                    v-bind="money"
                                                    disabled
                                                ></money>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label>Others</label>
                                                    <div class="input-group">
                                                        <input
                                                            class="form-control"
                                                            v-model="
                                                                detailModel.others
                                                            "
                                                            disabled
                                                        />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button
                                        v-if="fromOutside === true"
                                        class="btn talent_bg_red talent_font_white"
                                        @click.prevent="backPage"
                                    >
                                        Close
                                    </button>
                                    <button
                                        v-else
                                        class="btn talent_bg_red talent_font_white"
                                        @click.prevent="closeClicked"
                                    >
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
import { Sort } from '../../class/Sort';
import {
    CourseService,
    CourseJoinViewModel,
    CourseFormModel,
    CourseJoinModel,
    ProgramTypeService,
    ProgramTypeViewModel,
    CourseCategoryService,
    CourseCategoryViewModel,
    LearningTypeService,
    LearningTypeViewModel,
    LevelService,
    LevelViewModel,
    ApprovalStatusViewModel,
    ApprovalStatusService,
    UserPrivilegeSettingsService,
    UserAccessCRUD,
    FileContent
} from '../../services/NSwagService';
import { BlobService } from '../../services/BlobService';
import { isNullOrUndefined } from 'util';
import { Money } from 'v-money';

@Component({
    created: async function(this: Course) {
        this.isBusy = true;
        await this.getAccess();
        await this.initDropdownData();
        await this.fetch();
        if (this.fromOutside === true) {
            await this.detailClicked(this.courseId);
        }
    },
    data() {
        return {
            money: {
                decimal: ',',
                thousands: '.',
                prefix: 'Rp. ',
                suffix: '',
                precision: 0,
                masked: false
            }
        };
    },

    props: ['courseId', 'fromOutside']
})
export default class Course extends Vue {
    privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
    async getAccess() {
        var data = await this.privilegeApi.crudAccessPage('CRS');
        this.crud = data;
    }
    crud: UserAccessCRUD = {
        create: false,
        delete: false,
        read: false,
        update: false
    };

    imageUpload: FileContent = null;

    isBusy: boolean = false;
    itemPerPage: number = 10;
    success: boolean = false;
    successMessage: string = '';

    imageSize: boolean = true;
    fileSize: boolean = true;
    fileType: boolean = true;

    courseId: number;
    fromOutside: boolean;

    alertClose() {
        this.success = false;
        this.successMessage = '';
    }

    disableLevel: boolean = false;

    // ---------------------------------------- FETCH -----------------------------------------

    currentPage: number = 1;
    filterDate = {
        start: null,
        end: null
    };
    filterCourseName: string = '';
    filterProgramType: string = '';
    filterCourseCategory: string = '';
    filterLearningType: string = '';
    filterPricingType: string = '';
    filterApprovalName: string = '';
    sortBy: string = '';

    courseMan: CourseService = new CourseService();
    programTypeMan: ProgramTypeService = new ProgramTypeService();
    courseCategoryMan: CourseCategoryService = new CourseCategoryService();
    learningTypeMan: LearningTypeService = new LearningTypeService();
    levelMan: LevelService = new LevelService();
    approvalMan: ApprovalStatusService = new ApprovalStatusService();
    courses: CourseJoinViewModel = {
        listCourseJoinModel: null,
        totalItem: null
    };
    programTypes: ProgramTypeViewModel = {};
    learningTypes: LearningTypeViewModel = {};
    courseCategories: CourseCategoryViewModel = {};
    levels: LevelViewModel = {};
    approvals: ApprovalStatusViewModel = {};
    pricingTypes = ['Pay', 'Free'];

    async fetch() {
        this.isBusy = true;
        this.courses = await this.courseMan.getAllJoinCourses(
            this.filterDate.start,
            this.filterDate.end,
            this.filterCourseName,
            this.filterProgramType,
            this.filterLearningType,
            this.filterCourseCategory,
            this.filterPricingType,
            this.filterApprovalName,
            this.sortBy,
            this.currentPage
        );
        this.isBusy = false;
    }

    allLevels: LevelViewModel = {};

    async initDropdownData() {
        this.programTypes = await this.programTypeMan.getAllProgramTypes();
        this.learningTypes = await this.learningTypeMan.getAllLearningTypes();
        this.courseCategories = await this.courseCategoryMan.getAllCourseCategories();
        this.allLevels = await this.levelMan.getAllLevels();
        this.approvals = await this.approvalMan.getAllApprovalStatuses();
    }

    changeLevelList(type) {
        if (type == 'add') {
            this.addModel.levelId = null;
            //Set level option to On Boarding and Development Program
            if (this.addModel.programTypeId == 1) {
                this.levels.listLevelModel = this.allLevels.listLevelModel.slice(
                    0,
                    2
                );
                this.disableLevel = false;
            }
            //Set level option to Level 1 - 5 & N/A
            else if (this.addModel.programTypeId == 2) {
                this.levels.listLevelModel = this.allLevels.listLevelModel.slice(
                    4,
                    10
                );
                this.disableLevel = false;
            }
            //Disabled Level and Select Level N/A
            else if (this.addModel.programTypeId == 3) {
                this.levels.listLevelModel = this.allLevels.listLevelModel.slice(
                    4,
                    10
                );
                this.addModel.levelId = 10;
                this.disableLevel = true;
            }
            //Set level to Instructor Program
            else if (this.addModel.programTypeId == 4) {
                this.levels.listLevelModel = this.allLevels.listLevelModel.slice(
                    2,
                    4
                );
                // add new level
                this.levels.listLevelModel.push(...this.allLevels.listLevelModel.slice(
                    10,
                    11
                ))
                this.disableLevel = false;
            } else {
                this.levels.listLevelModel = [];
                this.disableLevel = false;
            }
        } else {
            this.editModel.levelId = null;
            //Set level option to On Boarding and Development
            if (this.editModel.programTypeId == 1) {
                this.levels.listLevelModel = this.allLevels.listLevelModel.slice(
                    0,
                    2
                );
                this.disableLevel = false;
            }
            //Set level option to Level 1 - 5 & N/A
            else if (this.editModel.programTypeId == 2) {
                this.levels.listLevelModel = this.allLevels.listLevelModel.slice(
                    4,
                    10
                );
                this.disableLevel = false;
            }
            //Disabled Level and Select Level N/A
            else if (this.editModel.programTypeId == 3) {
                this.levels.listLevelModel = this.allLevels.listLevelModel.slice(
                    4,
                    10
                );
                this.editModel.levelId = 10;
                this.disableLevel = true;
            }
            //Set level to Instructor Program
            else if (this.editModel.programTypeId == 4) {
                this.levels.listLevelModel = this.allLevels.listLevelModel.slice(
                    2,
                    4
                );
                // add new level
                this.levels.listLevelModel.push(...this.allLevels.listLevelModel.slice(
                    10,
                    11
                ))
                this.disableLevel = false;
            } else {
                this.levels.listLevelModel = [];
                this.disableLevel = false;
            }
        }
    }

    reset() {
        this.filterDate = {
            start: null,
            end: null
        };
        this.filterCourseName = '';
        this.filterProgramType = '';
        this.filterCourseCategory = '';
        this.filterLearningType = '';
        this.filterPricingType = '';
        this.filterApprovalName = '';
        this.ResetSort('');
        this.fetch();
    }

    convertDateTime(stringdate) {
        var date = new Date(stringdate);
        function pad(n) {
            return n < 10 ? '0' + n : n;
        }
        return (
            pad(date.getDate()) +
            '/' +
            pad(date.getMonth() + 1) +
            '/' +
            date.getFullYear()
        );
    }

    // ----------------------------------------- CRUD ---------------------------------------------

    //Navigation
    add: boolean = false;
    edit: boolean = false;
    detail: boolean = false;

    //Create
    addValidation = false;
    addModel: CourseFormModel = {
        courseCategoryId: 4,
        programTypeId: null,
        levelId: null,
        learningTypeId: null,
        courseName: null,
        courseDescription: '',
        coursePrice: 0,
        createdBy: 'admin'
    };
    addPricing = 'Free';
    addFormData: FormData = new FormData();
    addImageData: string = '/upload-image2.png';
    addImageName = null;

    addClicked() {
        if (!this.crud.create) {
            return;
        }
        this.alertClose();
        this.changeLevelList('add');
        this.add = true;
    }

    convertToBase64(file: File): Promise<string> {
        let promise = new Promise<string>((resolve, reject) => {
            let reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
                resolve(reader.result.toString().split(',')[1]);
            };
            reader.onerror = error => {
                reject(error);
            };
        });

        return promise;
    }

    async onAddFileChange(e) {
        var fileInput = e.target.files || e.dataTransfer.files;
        if (!fileInput.length) return;

        this.fileType = true;
        this.fileSize = true;
        this.imageSize = true;

        var extension = fileInput[0].name
            .substring(fileInput[0].name.lastIndexOf('.'))
            .toLowerCase();
        if (
            extension != '.jpg' &&
            extension != '.png' &&
            extension != '.jpeg'
        ) {
            this.fileType = false;
        }
        if (fileInput[0].size > 5048576) {
            this.fileSize = false;
        }

        var temp = this;
        let reader = new FileReader();
        reader.readAsDataURL(fileInput[0]);
        reader.onload = function() {
            var image = new Image();
            image.src = reader.result as string;
            image.onload = function() {
                //if (image.height > 300 || image.width > 300) {
                //    temp.imageSize = false;
                //}
                if (temp.fileType && temp.fileSize && temp.imageSize) {
                    temp.addImageData = reader.result as string;
                    temp.addImageName = fileInput[0].name;
                    temp.addFormData = new FormData();
                    Array.from(Array(e.target.files.length).keys()).map(x => {
                        temp.addFormData.append(
                            e.target.files,
                            e.target.files[x],
                            e.target.files[x].name
                        );
                    });
                }
            };
        };

        if (this.fileType && this.fileSize && this.imageSize) {
            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split('.');

            this.imageUpload = {
                base64: base64String,
                fileName: fileInput[0].name,
                contentType: array.pop()
            };
        }
    }

    validateAdd() {
        if (
            this.addModel.programTypeId === 0 ||
            this.addModel.levelId === 0 ||
            this.addModel.learningTypeId === 0 ||
            this.addModel.courseName === '' ||
            this.addImageName == '' ||
            (this.addModel.coursePrice <= 0 && this.addPricing == 'Pay') ||
            (this.addModel.coursePrice > 1000000000 &&
                this.addPricing == 'Pay') ||
            this.addModel.coursePrice.toString() == ''
        ) {
            return false;
        } else {
            return true;
        }
    }

    async addData(approvalId) {
        this.isBusy = true;

        this.addValidation = true;
        if (isNullOrUndefined(this.addModel.programTypeId)) {
            this.addModel.programTypeId = 0;
        }
        if (isNullOrUndefined(this.addModel.levelId)) {
            this.addModel.levelId = 0;
        }
        if (isNullOrUndefined(this.addModel.learningTypeId)) {
            this.addModel.learningTypeId = 0;
        }
        if (isNullOrUndefined(this.addModel.courseName)) {
            this.addModel.courseName = '';
        }
        if (isNullOrUndefined(this.addImageName)) {
            this.addImageName = '';
        }
        if (this.validateAdd() === false) {
            this.addValidation = false;
        }
        if (this.addValidation === true) {
            this.addModel.approvalId = approvalId;
            this.addModel.others =
                this.addModel.courseCategoryId == 4
                    ? this.addModel.others
                    : null;
            this.addModel.coursePrice =
                this.addPricing == 'Free' ? 0 : this.addModel.coursePrice;

            //let blobId = await BlobService.uploadService(this.addFormData);
            //this.addModel.blobId = blobId;
            if (this.imageUpload != null) {
                this.addModel.imageUpload = this.imageUpload;
            }

            await this.courseMan.create(this.addModel);
            this.reset();
            this.addModel.courseName = null;
            this.addModel.courseDescription = '';
            this.addModel.coursePrice = 0;
            this.addModel.courseCategoryId = 4;
            this.addModel.programTypeId = null;
            this.addModel.levelId = null;
            this.addModel.learningTypeId = null;
            this.addPricing = 'Free';
            this.addFormData = new FormData();
            this.addImageData = '/upload-image2.png';
            this.addImageName = null;

            this.initDropdownData();
            this.successMessage = 'Success to Add Data!';
            this.success = true;
            this.closeClicked();
        }
        this.isBusy = false;
    }

    //Edit
    editValidation = false;
    editModel: CourseFormModel = {
        courseCategoryId: null,
        programTypeId: null,
        levelId: null,
        learningTypeId: null,
        courseName: null,
        courseDescription: null,
        coursePrice: null,
        createdBy: null
    };
    editPricing = '';
    editFormData: FormData = new FormData();
    editImageData = '/upload-image2.png';
    editImageName = null;

    async editClicked(courseId: number) {
        if (!this.crud.update) {
            return;
        }

        this.alertClose();
        var data = await this.courseMan.getJoinCourseById(courseId);

        this.editFormData = null;
        this.editImageData = await BlobService.getImageUrl(
            data.blobId,
            data.mime
        );
        this.editImageName = data.fileName;
        this.editModel.courseId = data.courseId;
        this.editModel.learningTypeId = data.learningTypeId;
        this.editModel.courseCategoryId = data.courseCategoryId;
        this.editModel.programTypeId = data.programTypeId;
        this.changeLevelList('edit');
        this.editModel.levelId = data.levelId;
        this.editModel.blobId = data.blobId;
        this.editModel.courseName = data.courseName;
        this.editModel.courseDescription = data.courseDescription;
        this.editModel.coursePrice = data.coursePrice;
        this.editModel.others = data.others;
        this.editPricing = this.editModel.coursePrice === 0 ? 'Free' : 'Pay';

        this.edit = true;
    }

    async onEditFileChange(e) {
        var fileInput = e.target.files || e.dataTransfer.files;
        if (!fileInput.length) return;

        this.fileType = true;
        this.fileSize = true;
        this.imageSize = true;

        var extension = fileInput[0].name
            .substring(fileInput[0].name.lastIndexOf('.'))
            .toLowerCase();
        if (
            extension != '.jpg' &&
            extension != '.png' &&
            extension != '.jpeg'
        ) {
            this.fileType = false;
        }
        if (fileInput[0].size > 5048576) {
            this.fileSize = false;
        }

        var temp = this;
        let reader = new FileReader();
        reader.readAsDataURL(fileInput[0]);
        reader.onload = function() {
            var image = new Image();
            image.src = reader.result as string;
            image.onload = function() {
                //if (image.height > 300 || image.width > 300) {
                //    temp.imageSize = false;
                //}
                if (temp.fileType && temp.fileSize && temp.imageSize) {
                    temp.editImageData = reader.result as string;
                    temp.editImageName = fileInput[0].name;
                    temp.editFormData = new FormData();
                    Array.from(Array(e.target.files.length).keys()).map(x => {
                        temp.editFormData.append(
                            e.target.files,
                            e.target.files[x],
                            e.target.files[x].name
                        );
                    });
                }
            };
        };

        if (this.fileType && this.fileSize && this.imageSize) {
            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split('.');

            this.imageUpload = {
                base64: base64String,
                fileName: fileInput[0].name,
                contentType: array.pop()
            };
        }
    }

    validateEdit() {
        if (
            this.editModel.courseName === '' ||
            (this.editModel.coursePrice <= 0 && this.editPricing == 'Pay') ||
            (this.editModel.coursePrice > 1000000000 &&
                this.editPricing == 'Pay') ||
            this.editModel.coursePrice.toString() == '' ||
            this.editModel.levelId == undefined
        ) {
            return false;
        } else {
            return true;
        }
    }

    async editData(approvalId) {
        this.isBusy = true;
        this.editValidation = true;
        if (this.validateEdit() === false) {
            this.editValidation = false;
        }
        if (this.editValidation === true) {
            this.editModel.approvalId = approvalId;
            this.editModel.others =
                this.editModel.courseCategoryId == 4
                    ? this.editModel.others
                    : null;
            this.editModel.coursePrice =
                this.editPricing == 'Free' ? 0 : this.editModel.coursePrice;
            //if (this.editFormData != null) {
            //    this.editModel.blobId = await BlobService.uploadService(this.editFormData);
            //}

            if (this.imageUpload != null) {
                this.editModel.imageUpload = this.imageUpload;
            }

            await this.courseMan.update(this.editModel);
            this.reset();
            this.successMessage = 'Success to Edit Data!';
            this.success = true;
            this.closeClicked();
        }
        this.isBusy = false;
    }

    //Detail
    detailModel: CourseJoinModel = {
        courseCategoryName: null,
        programTypeName: null,
        levelName: null,
        learningName: null,
        courseName: null,
        courseDescription: null,
        coursePrice: null,
        createdBy: null
    };

    detailPricing = '';
    detailImageData = '/upload-image2.png';
    detailImageName = null;

    async detailClicked(courseId: number) {
        if (!this.crud.read) {
            return;
        }
        this.alertClose();
        var data = await this.courseMan.getJoinCourseById(courseId);

        this.detailImageData = await BlobService.getImageUrl(
            data.blobId,
            data.mime
        );
        this.detailImageName = data.fileName;
        this.detailModel.courseId = data.courseId;
        this.detailModel.learningName = data.learningName;
        this.detailModel.courseCategoryName = data.courseCategoryName;
        this.detailModel.levelName = data.levelName;
        this.detailModel.programTypeName = data.programTypeName;
        this.detailModel.courseName = data.courseName;
        this.detailModel.courseDescription = data.courseDescription;
        this.detailModel.coursePrice = data.coursePrice;
        this.detailModel.others = data.others;
        this.detailPricing =
            this.detailModel.coursePrice === 0 ? 'Free' : 'Pay';

        this.detail = true;
    }

    //Delete
    deleteCourseId;
    deleteIndex;

    async deleteClicked(courseId: number, index: number) {
        if (!this.crud.delete) {
            return;
        }
        this.alertClose();
        this.deleteCourseId = courseId;
        this.deleteIndex = index;
    }

    async deleteData() {
        this.isBusy = true;
        await this.courseMan.delete(this.deleteCourseId);
        this.fetch();
        //this.courses.listCourseJoinModel.splice(this.deleteIndex, 1);
        this.successMessage = 'Success to Remove Data!';
        this.success = true;
    }

    closeClicked() {
        this.add = false;
        this.edit = false;
        this.detail = false;

        this.fileType = true;
        this.fileSize = true;
        this.imageSize = true;
        this.imageUpload = null;
    }

    backPage() {
        window.history.back();
    }

    // ---------------------------------------- Sorting ------------------------------------------

    //Variable untuk sorting
    courseName = new Sort();
    programType = new Sort();
    learningType = new Sort();
    courseCategory = new Sort();
    pricing = new Sort();
    approvalStatus = new Sort();
    createdDate = new Sort();
    updatedDate = new Sort();

    //ClickSortCourseName
    async ClickSortCourseName() {
        this.ResetSort('courseName');
        //Sorting
        this.courseName.sorting();
        //Function Sorting
        if (this.courseName.sortup == true) {
            this.sortBy = 'courseName';
        } else if (this.courseName.sortdown == true) {
            this.sortBy = 'courseNameDesc';
        } else {
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
        } else if (this.programType.sortdown == true) {
            this.sortBy = 'programTypeDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortLearningType
    async ClickSortLearningType() {
        this.ResetSort('learningType');
        //Sorting
        this.learningType.sorting();
        //Function Sorting

        if (this.learningType.sortup == true) {
            this.sortBy = 'learningType';
        } else if (this.learningType.sortdown == true) {
            this.sortBy = 'learningTypeDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortLevel
    async ClickSortCourseCategory() {
        this.ResetSort('courseCategory');
        //Sorting
        this.courseCategory.sorting();
        //Function Sorting

        if (this.courseCategory.sortup == true) {
            this.sortBy = 'category';
        } else if (this.courseCategory.sortdown == true) {
            this.sortBy = 'categoryDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortPricing
    async ClickSortPricing() {
        this.ResetSort('pricing');
        //Sorting
        this.pricing.sorting();
        //Function Sorting

        if (this.pricing.sortup == true) {
            this.sortBy = 'pricing';
        } else if (this.pricing.sortdown == true) {
            this.sortBy = 'pricingDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortApprovalStatus
    async ClickSortApprovalStatus() {
        this.ResetSort('approvalStatus');
        //Sorting
        this.approvalStatus.sorting();
        //Function Sorting

        if (this.approvalStatus.sortup == true) {
            this.sortBy = 'approvalStatus';
        } else if (this.approvalStatus.sortdown == true) {
            this.sortBy = 'approvalStatusDesc';
        } else {
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
        } else if (this.createdDate.sortdown == true) {
            this.sortBy = 'createdDateDesc';
        } else {
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
        } else if (this.updatedDate.sortdown == true) {
            this.sortBy = 'updatedDateDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //Reset Sorting
    async ResetSort(parameter: string) {
        switch (parameter) {
            case 'courseName':
                this.programType.reset();
                this.learningType.reset();
                this.courseCategory.reset();
                this.pricing.reset();
                this.approvalStatus.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'programType':
                this.courseName.reset();
                this.learningType.reset();
                this.courseCategory.reset();
                this.pricing.reset();
                this.approvalStatus.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'learningType':
                this.courseName.reset();
                this.programType.reset();
                this.courseCategory.reset();
                this.pricing.reset();
                this.approvalStatus.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'courseCategory':
                this.courseName.reset();
                this.programType.reset();
                this.learningType.reset();
                this.pricing.reset();
                this.approvalStatus.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'pricing':
                this.courseName.reset();
                this.programType.reset();
                this.learningType.reset();
                this.courseCategory.reset();
                this.approvalStatus.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'approvalStatus':
                this.courseName.reset();
                this.programType.reset();
                this.learningType.reset();
                this.courseCategory.reset();
                this.pricing.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'createdDate':
                this.courseName.reset();
                this.programType.reset();
                this.learningType.reset();
                this.courseCategory.reset();
                this.pricing.reset();
                this.approvalStatus.reset();
                this.updatedDate.reset();
                return;
            case 'updatedDate':
                this.courseName.reset();
                this.programType.reset();
                this.learningType.reset();
                this.courseCategory.reset();
                this.pricing.reset();
                this.approvalStatus.reset();
                this.createdDate.reset();
                return;
            default:
                this.programType.reset();
                this.learningType.reset();
                this.courseCategory.reset();
                this.pricing.reset();
                this.approvalStatus.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                this.courseName.reset();
                this.sortBy = '';
                return;
        }
    }
}
</script>
