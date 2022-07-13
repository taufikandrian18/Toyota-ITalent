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

        <div
            class="row"
            v-if="!add && !edit && !detail"
        >
            <div class="col col-md-12">
                <h3>
                    <fa icon="database"></fa> CMS >
                    <span class="talent_font_red">Guide</span>
                </h3>
                <br />

                <!-- Advanced Search -->
                <div class="row mb-4">
                    <!-- Filter -->
                    <div class="col-auto">
                        <div class="dropdown">
                            <button
                                type="button"
                                class="btn btn-block talent_bg_cyan talent_font_white"
                                @click.prevent="showFilter"
                            >
                                Filter
                            </button>
                            <div id="filter" class="dropdown-menu p-4">
                                <div class="row mb-3">
                                    <div class="col">
                                        <h5 class="modal-title">Filter</h5>
                                    </div>
                                    <div
                                        class="col-auto"
                                        @click="closeFilter"
                                        style="cursor: pointer;"
                                    >
                                        <fa icon="times"></fa>
                                    </div>
                                </div>
                                <hr />
                                <div class="row">
                                    <!-- Left -->
                                    <div class="col-md-6">
                                        <!-- Date -->
                                        <div class="mb-1">Date</div>
                                        <div
                                            class="mb-2 input-group talent_front"
                                        >
                                            <v-date-picker
                                                class="v-date-style-report"
                                                :masks="{ input: 'DD/MM/YYYY' }"
                                                mode="range"
                                                :firstDayOfWeek="2"
                                                v-model="filterDate"
                                            ></v-date-picker>
                                            <div class="input-group-append">
                                                <span class="input-group-text"
                                                    ><fa
                                                        icon="calendar-alt"
                                                    ></fa
                                                ></span>
                                            </div>
                                        </div>
                                        <!-- Guide Title -->
                                        <div class="mb-1">Guide Title</div>
                                        <div class="input-group mb-2">
                                            <input
                                                class="form-control"
                                                v-model="filterGuideName"
                                            />
                                        </div>
                                        <!-- Created By -->
                                        <div class="mb-1">Created By</div>
                                        <div class="input-group">
                                            <input
                                                class="form-control"
                                                v-model="filterCreatedBy"
                                            />
                                        </div>
                                    </div>
                                    <!-- Right -->
                                    <div class="col-md-6">
                                        <!-- Type of Guide -->
                                        <div class="mb-1">Type of Guide</div>
                                        <div class="mb-2">
                                            <select
                                                class="form-control"
                                                v-model="filterGuideTypeName"
                                            >
                                                <option
                                                    v-for="t in guideTypes.listGuideTypeModel"
                                                    :key="t.id"
                                                    :value="t.name"
                                                    >{{ t.name }}</option
                                                >
                                            </select>
                                        </div>
                                        <!-- Approval Status -->
                                        <div class="mb-1">Approval Status</div>
                                        <div>
                                            <select
                                                class="form-control"
                                                v-model="filterApprovalStatus"
                                            >
                                                <option
                                                    v-for="a in approvalStatuses.listApprovalStatusModel"
                                                    :key="a.id"
                                                    :value="a.approvalName"
                                                    >{{
                                                        a.approvalName
                                                    }}</option
                                                >
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <hr />
                                <!-- Apply Button -->
                                <div class="row">
                                    <div class="col-md-12">
                                        <div
                                            class="d-flex align-items-end flex-column"
                                        >
                                            <div>
                                                <button
                                                    class="btn talent_bg_red talent_font_white px-4"
                                                    @click.prevent="reset"
                                                >
                                                    Reset
                                                </button>
                                                <button
                                                    class="btn talent_bg_darkblue talent_font_white px-4"
                                                    @click.prevent="fetch"
                                                >
                                                    Apply
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- Search Bar -->
                    <div class="col px-0">
                        <form class="d-flex">
                            <input
                                class="form-control"
                                type="search"
                                placeholder="Search"
                                aria-label="Search"
                            />
                            <button
                                class="btn talent_bg_darkblue talent_font_white"
                                type="submit"
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
                    <!-- Add Guide -->
                    <div class="col d-flex justify-content-end">
                        <button
                            v-if="crud.create"
                            class="btn talent_bg_darkblue talent_font_white"
                            @click.prevent="addClicked"
                        >
                            + Add Guide
                        </button>
                    </div>
                </div>

                <!--Table-->
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <!-- Header -->
                    <div class="row">
                        <div class="col">
                            <h4>Guide List</h4>
                        </div>
                        <div class="col-auto">
                            <button 
                                class="btn talent_bg_red talent_font_white" 
                                data-toggle="modal"
                                data-target="#modalDeleteSelected">
                                Delete
                            </button>
                        </div>
                    </div>
                    <hr />
                    <!-- Content -->
                    <div class="talent_magin_small">
                        <a
                            >Showing
                            {{
                                guides.listGuideJoinModel == null
                                    ? 0
                                    : guides.listGuideJoinModel.length
                            }}
                            of {{ guides.totalItem }} Entry(s)</a
                        >
                    </div>

                    <div class="talent_overflowx">
                        <table
                            class="table table-bordered table-responsive-md talent_table_padding"
                        >
                            <thead>
                                <tr>
                                    <th class="text-center">
                                        <div class="form-check">
                                            <input
                                                class="form-check-input"
                                                type="checkbox"
                                                value=""
                                                id="item"
                                                @change="handleSelectAll"
                                            />
                                        </div>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortGuideName()
                                            "
                                            >Guide Title
                                            <fa
                                                icon="sort"
                                                v-if="guideName.sort == true"
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="guideName.sortup == true"
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    guideName.sortdown == true
                                                "
                                            ></fa
                                        ></a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortGuideType()
                                            "
                                            >Type of Guide
                                            <fa
                                                icon="sort"
                                                v-if="guideType.sort == true"
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="guideType.sortup == true"
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    guideType.sortdown == true
                                                "
                                            ></fa
                                        ></a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortCreatedBy()
                                            "
                                            >Created By
                                            <fa
                                                icon="sort"
                                                v-if="createdBy.sort == true"
                                            ></fa
                                            ><fa
                                                icon="sort-up"
                                                v-if="createdBy.sortup == true"
                                            ></fa
                                            ><fa
                                                icon="sort-down"
                                                v-if="
                                                    createdBy.sortdown == true
                                                "
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
                                            >Status
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
                                    <th class="text-center" colspan="3">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr
                                    v-for="(g,
                                    index) in guides.listGuideJoinModel"
                                    :key="g.id"
                                >
                                    <th class="text-center">
                                        <div class="form-check">
                                            <input
                                                type="checkbox"
                                                @change="
                                                    e => handleCheckedItem(g, e)
                                                "
                                                :checked="g.isSelected"
                                            />
                                        </div>
                                    </th>
                                    <td>
                                        {{ g.name }}
                                    </td>
                                    <td>
                                        {{ g.guideTypeName }}
                                    </td>
                                    <td>
                                        {{ g.createdBy }}
                                    </td>
                                    <td>
                                        {{ g.approvalStatus }}
                                    </td>
                                    <td>
                                        {{ convertDateTime(g.createdAt) }}
                                    </td>
                                    <td>
                                        {{ convertDateTime(g.updatedAt) }}
                                    </td>
                                    <td
                                        v-if="crud.read"
                                        class="talent_nopadding_important"
                                    >
                                        <button
                                            class="btn btn-block talent_bg_orange talent_font_white"
                                            @click.prevent="
                                                detailClicked(g.guideId)
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
                                                editClicked(g.guideId)
                                            "
                                            :disabled="
                                                g.approvalStatus ==
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
                                                deleteClicked(g.guideId, index)
                                            "
                                            :disabled="
                                                g.approvalStatus ==
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

                    <!-- Table Footer -->
                    <div class="row mt-2">
                        <!-- Set Showed Data Per Page -->
                        <div class="col row">
                            <div class="col-auto d-flex align-items-center">
                                Show
                            </div>
                            <div
                                class="col-auto d-flex align-items-center px-0"
                            >
                                <select
                                    class="form-select show-entries"
                                    aria-label="select"
                                >
                                    <option selected>5</option>
                                    <option value="10">10</option>
                                    <option value="15">15</option>
                                    <option value="20">20</option>
                                </select>
                            </div>
                            <div class="col d-flex align-items-center">
                                Entries
                            </div>
                        </div>
                        <!-- Pagination -->
                        <div class="col-auto d-flex justify-content-center">
                            <paginate
                                :currentPage.sync="currentPage"
                                :total-data="guides.totalItem"
                                :limit-data="itemPerPage"
                                @update:currentPage="fetch()"
                            ></paginate>
                        </div>
                    </div>
                </div>

                <!-- Modal -->
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


                <modal-confirmation
                    name="modalDeleteSelected"
                    message="Are you sure want to delete this datas?"
                    @yes="handleDeleteBulk()"
                />
            </div>
        </div>
        <!--Add-->
        <div class="row" v-if="add">
            <div class="col-md-12">
                <h3>
                    <fa icon="database"></fa> CMS > Guide >
                    <span class="talent_font_red">Add Guide</span>
                </h3>
                <br />

                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Add Guide</h4>
                    <hr />
                    <div
                        v-if="fileType === false || fileSize === false"
                        class="alert alert-danger"
                    >
                        <div v-if="fileSize === false">
                            Maximum File Size is 5 MB
                        </div>
                        <div v-if="fileType === false">
                            Please input a pdf/jpg/png/jpeg file
                        </div>
                    </div>

                    <div
                        v-if="validateAddMessage() === false"
                        class="alert alert-danger"
                    >
                        <div v-if="addModel.guideTypeId === 0">
                            Guide Type is Required
                        </div>
                        <div v-if="addModel.name === ''">
                            Guide Title is Required
                        </div>
                        <div v-if="addFileName === ''">File is Required</div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between d-flex">
                                <div class="col-md-6 mb-3">
                                    <label
                                        >Title
                                        <span class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="addModel.name"
                                            maxlength="1024"
                                        />
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label
                                        >Type of Guide
                                        <span class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <select
                                        class="form-control"
                                        v-model="addModel.guideTypeId"
                                    >
                                        <option
                                            v-for="t in guideTypes.listGuideTypeModel"
                                            :key="t.id"
                                            :value="t.guideTypeId"
                                            >{{ t.name }}</option
                                        >
                                    </select>
                                </div>
                                <div class="col-md-12">
                                    <label>Description</label>
                                    <textarea
                                        class="form-control"
                                        style="height:130px; overflow-y:scroll;"
                                        v-model="addModel.description"
                                        maxlength="1024"
                                    ></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>File Upload</h4>
                    <hr />
                    <div class="row">
                        <div class="col-md-12">
                            <label>
                                Upload File
                                <span class="talent_font_red">*</span>
                            </label>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="custom-file">
                                        <input
                                            type="file"
                                            class="custom-file-input"
                                            id="customFile"
                                            @change="onAddFileChange"
                                        />
                                        <label
                                            class="custom-file-label talent_textshorten"
                                            for="customFile"
                                            >{{
                                                addFileName == null
                                                    ? 'Choose File'
                                                    : addFileName
                                            }}</label
                                        >
                                    </div>
                                </div>
                                <div
                                    class="col-md-12 mb-3 mt-2 talent_font_red"
                                >
                                    *Recommended File: PDF, JPEG, JPG, PNG, MP4
                                    <br />*Max File Size: 100MB
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="h-100">
                        <img
                            v-if="fileUploadType == 'image'"
                            class="h-100 w-100"
                            :src="addImageData"
                            for="customFile"
                        />
                        <object
                            v-else
                            class="h-100 w-100 min-heigth-full"
                            :data="addFileData"
                            for="customFile"
                        />
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
            </div>
        </div>

        <!--Edit-->
        <div class="row" v-if="edit">
            <div class="col-md-12">
                <h3>
                    <fa icon="database"></fa>Master Data > Guide >
                    <span class="talent_font_red">Edit Guide</span>
                </h3>
                <br />
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Guide Information</h4>
                    <hr />
                    <div
                        v-if="fileType === false || fileSize === false"
                        class="alert alert-danger"
                    >
                        <div v-if="fileSize === false">
                            Maximum File Size is 5 MB
                        </div>
                        <div v-if="fileType === false">
                            Please input a pdf/jpg/png/jpeg file
                        </div>
                    </div>

                    <div
                        v-if="validateEditMessage() === false"
                        class="alert alert-danger"
                    >
                        <div v-if="editModel.guideTypeId === 0">
                            Guide Type is Required
                        </div>
                        <div v-if="editModel.name === ''">
                            Guide Title is Required
                        </div>
                        <div v-if="editFileName === ''">File is Required</div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between d-flex">
                                <div class="col-md-6 mb-3">
                                    <label
                                        >Title<span class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="editModel.name"
                                            maxlength="1024"
                                        />
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label
                                        >Type of Guide<span
                                            class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <select
                                        class="form-control"
                                        v-model="editModel.guideTypeId"
                                    >
                                        <option
                                            v-for="t in guideTypes.listGuideTypeModel"
                                            :key="t.id"
                                            :value="t.guideTypeId"
                                            >{{ t.name }}</option
                                        >
                                    </select>
                                </div>
                                <div class="col-md-12">
                                    <label>Description</label>
                                    <textarea
                                        class="form-control"
                                        style="height:130px;overflow-y:scroll;"
                                        v-model="editModel.description"
                                        maxlength="1024"
                                    ></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>File Upload</h4>
                    <hr />
                    <div class="row">
                        <div class="col-md-12">
                            <label>
                                Upload File
                                <span class="talent_font_red">*</span>
                            </label>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="custom-file">
                                        <input
                                            type="file"
                                            class="custom-file-input"
                                            id="customFile"
                                            @change="onEditFileChange"
                                        />
                                        <label
                                            class="custom-file-label talent_textshorten"
                                            for="customFile"
                                            >{{
                                                editFileName == null
                                                    ? 'Choose File'
                                                    : editFileName
                                            }}</label
                                        >
                                    </div>
                                </div>
                                <div
                                    class="col-md-12 mb-3 mt-2 talent_font_red"
                                >
                                    *Recommended File: PDF, JPG, JPEG, PNG, MP4
                                    <br />*Max File Size 100MB
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="h-100">
                        <img
                            v-if="fileUploadType == 'image'"
                            class="h-100 w-100"
                            :src="editImageData"
                            for="customFile"
                        />
                        <object
                            v-if="fileUploadType == 'pdf'"
                            class="h-100 w-100 min-heigth-full"
                            :data="editFileData"
                            for="customFile"
                        />
                        <video v-if="fileUploadType == 'mp4' " id="myVideo" class="w-100 video" controls :src="editImageData"></video>
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

        <!--Detail-->
        <div class="row" v-if="detail">
            <div class="col-md-12">
                <h3>
                    <fa icon="database"></fa> CMS > Guide >
                    <span class="talent_font_red">View Detail Guide</span>
                </h3>
                <br />

                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Guide Information</h4>
                    <hr />
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between d-flex">
                                <div class="col-md-6 mb-3">
                                    <label
                                        >Title<span class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="detailModel.name"
                                            disabled
                                        />
                                    </div>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label
                                        >Type of Guide<span
                                            class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="detailModel.guideTypeName"
                                            disabled
                                        />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <label>
                                        Description
                                    </label>
                                    <textarea
                                        class="form-control"
                                        style="height:130px;overflow-y:scroll;"
                                        v-model="detailModel.description"
                                        disabled
                                    ></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>File Upload</h4>
                    <hr />
                    <div class="row">
                        <div class="col-md-12">
                            <label>
                                Upload File
                                <span class="talent_font_red">*</span>
                            </label>
                            <div class="row">
                                <div class="col-md-6">
                                    <div class="custom-file">
                                        <input
                                            type="file"
                                            class="custom-file-input"
                                            id="customFile"
                                            disabled
                                        />
                                        <label
                                            class="custom-file-label talent_textshorten"
                                            for="customFile"
                                            >{{
                                                detailFileName.length
                                                    ? detailFileName
                                                    : 'Choose File'
                                            }}</label
                                        >
                                    </div>
                                </div>
                                <div
                                    class="col-md-12 mb-3 mt-2 talent_font_red"
                                >
                                    *Recommended File: PDF, JPG, JPEG, PNG, MP4
                                    <br />*Max File Size 100MB
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="h-100">
                        <video v-if="detailFileData.includes('mp4')" id="myVideo" class="w-100 video" controls :src="detailFileData"></video>
                        <object
                            v-else
                            class="h-100 w-100 min-heigth-full"
                            :data="detailFileData"
                            for="customFile"
                        />
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
    GuideService,
    GuideTypeService,
    GuideFormModel,
    GuideTypeViewModel,
    GuideJoinViewModel,
    ApprovalStatusService,
    // FileContent,
    UserPrivilegeSettingsService,
    UserAccessCRUD
} from '../../services/NSwagService';
import { BlobService } from '../../services/BlobService';
import { isNullOrUndefined } from 'util';
import { PageEnums } from '../../enum/PageEnums';
import ModalConfirmation from '../../components/shared/ModalConfirmation.vue';

@Component({
    components: { ModalConfirmation },
    created: async function(this: Guide) {
        this.isBusy = true;
        await this.initDropdownData();
        await this.getAccess();
        if (this.fromOutside === true) {
            await this.detailClicked(this.guideId);
        }
        await this.fetch();
    },
    props: ['guideId', 'fromOutside'],

})
export default class Guide extends Vue {
    privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

    async getAccess() {
        var data = await this.privilegeApi.crudAccessPage(PageEnums.Guide);
        this.crud = data;
    }

    crud: UserAccessCRUD = {
        create: false,
        delete: false,
        read: false,
        update: false
    };

    isBusy: boolean = false;
    itemPerPage: number = 10;
    success: boolean = false;
    successMessage: string = '';

    guideId: number;
    fromOutside: boolean;

    fileSize: boolean = true;
    fileType: boolean = true;

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
    filterGuideName: string = '';
    filterGuideTypeName: string = '';
    filterCreatedBy: string = '';
    filterApprovalStatus: string = '';
    sortBy: string = '';

    guideMan: GuideService = new GuideService();
    guideTypeMan: GuideTypeService = new GuideTypeService();
    guides: GuideJoinViewModel = {};
    guideTypes: GuideTypeViewModel = {};
    approvalMan: ApprovalStatusService = new ApprovalStatusService();
    approvalStatuses = {};

    async fetch() {
        this.isBusy = true;
        this.guides = await this.guideMan.getAllJoinGuides(
            this.filterDate.start,
            this.filterDate.end,
            this.filterGuideName,
            this.filterGuideTypeName,
            this.filterCreatedBy,
            this.filterApprovalStatus,
            this.sortBy,
            this.currentPage
        );
        this.guides.listGuideJoinModel = this.guides.listGuideJoinModel.map(v => ({...v, isSelected: false}))
        this.isBusy = false;
        this.closeFilter();
    }

    handleSelectAll (e) {
        this.guides.listGuideJoinModel = this.guides.listGuideJoinModel.map(v => ({ ...v, isSelected: e.target.checked }))
    }

    handleCheckedItem(item, e) {
        this.guides.listGuideJoinModel = this.guides.listGuideJoinModel.map(v => ({
            ...v,
            isSelected: v.guideId === item.guideId ? e.target.checked : v.isSelected
        }));
    }

    async handleDeleteBulk() {
        this.isBusy = true;
        try {
            await Promise.all(
                this.guides.listGuideJoinModel
                    .filter(v => v.isSelected)
                    .map(v => this.guideMan.delete(v.guideId))
            );

            this.fetch();
        } catch (err) {
            console.log(err);
        }
        this.isBusy = false;
    }

    async initDropdownData() {
        this.guideTypes = await this.guideTypeMan.getAllGuideTypes();
        this.approvalStatuses = await this.approvalMan.getAllApprovalStatuses();
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

    reset() {
        this.filterDate = {
            start: null,
            end: null
        };
        this.filterGuideName = '';
        this.filterGuideTypeName = '';
        this.filterCreatedBy = '';
        this.filterApprovalStatus = '';
        this.ResetSort('');
        this.fetch();
    }

    // ----------------------------------------- CRUD ---------------------------------------------

    //Navigation
    add: boolean = false;
    edit: boolean = false;
    detail: boolean = false;

    //Create
    addValidation = false;
    addModel: GuideFormModel = {
        guideId: 0,
        name: null,
        description: '',
        guideTypeId: null,
        blobId: null,
        createdBy: 'Admin',
        fileContent: null
    };
    addFormData: FormData = new FormData();
    addFileData: any = null;
    addImageData: any = '/upload-image2.png';
    fileUploadType: string = 'image';
    addFileName = null;

    addClicked() {
        if (this.crud.create == false) {
            return;
        }

        this.alertClose();
        this.add = true;
    }

    showFilter() {
        const show = document.getElementById('filter');
        const isShowed = show.style.display;
        if (isShowed === 'none') {
            show.style.display = 'block';
        } else {
            this.closeFilter();
        }
    }

    closeFilter() {
        const show = document.getElementById('filter');
        show.style.display = 'none';
    }

    async onAddFileChange(e) {
        var fileInput = e.target.files || e.dataTransfer.files;
        if (!fileInput.length) return;
        this.fileType = true;
        this.fileSize = true;

        let file = e.target.files[0];

        let array = file.name.split('.');

        let extension = array.pop();
        if (
            extension != 'pdf' &&
            extension != 'jpeg' &&
            extension != 'jpg' &&
            extension != 'png' &&
            extension != 'mp4'
        ) {
            this.fileType = false;
        }
        if (fileInput[0].size > 100971520) {
            this.fileSize = false;
        }

        if (!this.fileType || !this.fileSize) return;

        this.addFileName = fileInput[0].name;

        let baseFileInput = (<HTMLInputElement>e.target).files[0];
        let base64String = await this.convertToBase64(baseFileInput);

        this.addModel.fileContent = {
            base64: base64String,
            fileName: this.addFileName,
            contentType: extension
        };

        if (extension == 'jpeg' || extension == 'jpg' || extension == 'png') {
            this.fileUploadType = 'image';
            var temp = this;
            let reader = new FileReader();
            reader.readAsDataURL(fileInput[0]);
            reader.onload = function() {
                temp.addImageData = reader.result as string;
                temp.addFileData = null;
            };
        } else if (extension == 'pdf') {
            this.fileUploadType = 'pdf';
            this.addFileData = URL.createObjectURL(file);
            this.addImageData = '/upload-image2.png';
        } else if (extension == 'mp4') {
            this.fileUploadType = 'mp4';
            this.addFileData = URL.createObjectURL(file);
            this.addImageData = '/upload-image2.png';
        }
     }

    validateAdd() {
        if (
            this.addModel.guideTypeId === 0 ||
            this.addModel.name === '' ||
            this.addFileName == '' ||
            this.addModel.fileContent == null
        ) {
            return false;
        } else {
            return true;
        }
    }

    validateAddMessage() {
        if (
            this.addModel.guideTypeId === 0 ||
            this.addModel.name === '' ||
            this.addFileName == ''
        ) {
            return false;
        } else {
            return true;
        }
    }

    validateEditMessage() {
        if (
            this.editModel.guideTypeId === 0 ||
            this.editModel.name === '' ||
            this.editFileName == ''
        ) {
            return false;
        } else {
            return true;
        }
    }

    async addData(approvalId) {
        this.isBusy = true;

        this.addValidation = true;
        if (isNullOrUndefined(this.addModel.guideTypeId)) {
            this.addModel.guideTypeId = 0;
        }
        if (isNullOrUndefined(this.addModel.name)) {
            this.addModel.name = '';
        }
        if (isNullOrUndefined(this.addFileName)) {
            this.addFileName = '';
        }
        if (this.validateAdd() === false) {
            this.addValidation = false;
        }
        if (this.addValidation === true) {
            this.addModel.approvalId = approvalId;
            await this.guideMan.create(this.addModel);
            this.reset();

            this.addModel.name = null;
            this.addModel.description = '';
            this.addModel.guideTypeId = null;
            this.addModel.fileContent = null;
            this.addFormData = new FormData();
            this.addFileData = null;
            this.addFileName = null;
            this.addImageData = '/upload-image2.png';

            this.successMessage = 'Success to Add Data!';
            this.success = true;
            this.closeClicked();
        }
        this.isBusy = false;
    }

    //Edit
    editValidation = false;
    editModel: GuideFormModel = {
        guideId: 0,
        name: null,
        description: '',
        guideTypeId: null,
        blobId: null,
        createdBy: 'Admin',
        fileContent: null
    };
    editFormData: FormData = new FormData();
    editImageData = '/upload-image2.png';
    editFileData: any = null;
    editFileName = null;

    async editClicked(guideId: number) {
        this.alertClose();

        if (this.crud.update == false) {
            return;
        }
        this.isBusy = true;

        var data = await this.guideMan.getJoinGuideById(guideId);

        this.editFormData = null;
        this.editFileName = data.fileName;
        this.editModel.guideId = data.guideId;
        this.editModel.blobId = data.blobId;
        this.editModel.name = data.name;
        this.editModel.guideTypeId = data.guideTypeId;
        this.editModel.description = data.description;
        this.editImageData = '/upload-image2.png';
        if (data.mime == 'jpg' || data.mime == 'jpeg' || data.mime == 'png') {
            this.fileUploadType = 'image';
            this.editImageData = await BlobService.getImageUrl(
                data.blobId,
                data.mime
            );
        } else if (data.mime == 'pdf') {
            this.fileUploadType = 'pdf';
            this.editFileData = await BlobService.getFilePDF(data.blobId);
        } else if (data.mime = 'mp4') {
            this.fileUploadType = 'mp4';
            this.editImageData = await BlobService.getImageUrl(
                data.blobId,
                data.mime
            );
        }

        this.edit = true;
        this.isBusy = false;
    }

    async onEditFileChange(e) {
        var fileInput = e.target.files || e.dataTransfer.files;
        if (!fileInput.length) return;
        this.fileType = true;
        this.fileSize = true;

        let file = e.target.files[0];

        let array = file.name.split('.');

        let extension = array.pop();

        if (
            extension != 'pdf' &&
            extension != 'jpeg' &&
            extension != 'jpg' &&
            extension != 'png' &&
            extension != 'mp4'
        ) {
            this.fileType = false;
        }
        if (fileInput[0].size > 100971520) {
            this.fileSize = false;
        }

        if (!this.fileType || !this.fileSize) return;

        let baseFileInput = (<HTMLInputElement>e.target).files[0];
        let base64String = await this.convertToBase64(baseFileInput);

        this.editModel.fileContent = {
            base64: base64String,
            contentType: extension,
            fileName: fileInput[0].name
        };

        if (extension == 'jpeg' || extension == 'jpg' || extension == 'png') {
            this.fileUploadType = 'image';
            var temp = this;
            let reader = new FileReader();
            reader.readAsDataURL(fileInput[0]);
            reader.onload = function() {
                temp.editImageData = reader.result as string;
            };
        } else if (extension == 'pdf') {
            this.fileUploadType = 'pdf';
            this.editFileData = URL.createObjectURL(file);
        }else if (extension == 'mp4') {
            this.fileUploadType = 'mp4';
            this.editImageData = URL.createObjectURL(file);
        }
    }

    validateEdit() {
        if (this.editModel.name === '') {
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

            await this.guideMan.update(this.editModel);
            this.reset();
            this.editModel.fileContent = null;
            this.successMessage = 'Success to Edit Data!';
            this.success = true;
            this.closeClicked();
        }
        this.isBusy = false;
    }

    //Detail
    detailValidation = false;
    detailModel = {
        guideId: 0,
        name: null,
        description: '',
        guideTypeId: null,
        blobId: null,
        createdBy: 'Admin',
        guideTypeName: ''
    };
    detailFileName = null;
    detailFileData = '/upload-image2.png';

    async detailClicked(guideId: number) {
        this.isBusy = true;
        this.alertClose();
        var data = await this.guideMan.getJoinGuideById(guideId);

        this.detailFileName = data.fileName;
        this.detailModel.guideId = data.guideId;
        this.detailModel.blobId = data.blobId;
        this.detailModel.name = data.name;
        this.detailModel.guideTypeName = data.guideTypeName;
        this.detailModel.description = data.description;
        this.detailFileData = '/upload-image2.png';
        if (data.mime == 'jpg' || data.mime == 'jpeg' || data.mime == 'png') {
            this.detailFileData = await BlobService.getImageUrl(
                data.blobId,
                data.mime
            );
        } else if (data.mime == 'pdf') {
            this.detailFileData = await BlobService.getFilePDF(data.blobId);
        } else if (data.mime == 'mp4') {
            this.detailFileData = await BlobService.getImageUrl(
                data.blobId,
                data.mime
            );
        }
        this.detail = true;
        this.isBusy = false;
    }

    //Delete
    deleteGuideId;
    deleteIndex;

    async deleteClicked(guideId: number, index: number) {
        this.alertClose();

        if (this.crud.delete == false) {
            return;
        }

        this.deleteGuideId = guideId;
        this.deleteIndex = index;
    }

    async deleteData() {
        this.isBusy = true;
        await this.guideMan.delete(this.deleteGuideId);
        this.fetch();
        this.isBusy = false;
        this.successMessage = 'Success to Remove Data!';
        this.success = true;
    }

    closeClicked() {
        this.add = false;
        this.edit = false;
        this.detail = false;

        this.fileType = true;
        this.fileSize = true;
    }

    backPage() {
        window.history.back();
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

    // ---------------------------------------- Sorting ------------------------------------------

    //Variable untuk sorting
    guideName = new Sort();
    guideType = new Sort();
    createdBy = new Sort();
    approvalStatus = new Sort();
    createdDate = new Sort();
    updatedDate = new Sort();

    //ClickSortGuideName
    async ClickSortGuideName() {
        this.ResetSort('guideName');
        //Sorting
        this.guideName.sorting();
        //Function Sorting
        if (this.guideName.sortup == true) {
            this.sortBy = 'guideName';
        } else if (this.guideName.sortdown == true) {
            this.sortBy = 'guideNameDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortGuideType
    async ClickSortGuideType() {
        this.ResetSort('guideType');
        //Sorting
        this.guideType.sorting();
        //Function Sorting
        if (this.guideType.sortup == true) {
            this.sortBy = 'guideType';
        } else if (this.guideType.sortdown == true) {
            this.sortBy = 'guideTypeDesc';
        } else {
            this.sortBy = '';
        }
        this.fetch();
    }

    //ClickSortCreatedBy
    async ClickSortCreatedBy() {
        this.ResetSort('createdBy');
        //Sorting
        this.createdBy.sorting();
        //Function Sorting
        if (this.createdBy.sortup == true) {
            this.sortBy = 'createdBy';
        } else if (this.createdBy.sortdown == true) {
            this.sortBy = 'createdByDesc';
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
            case 'guideName':
                this.guideType.reset();
                this.createdBy.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'guideType':
                this.guideName.reset();
                this.createdBy.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'createdBy':
                this.guideName.reset();
                this.guideType.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'approvalStatus':
                this.guideName.reset();
                this.guideType.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'createdDate':
                this.guideName.reset();
                this.guideType.reset();
                this.createdBy.reset();
                this.updatedDate.reset();
                return;
            case 'updatedDate':
                this.guideName.reset();
                this.guideType.reset();
                this.createdBy.reset();
                this.createdDate.reset();
                return;
            default:
                this.sortBy = '';
                this.guideName.reset();
                this.guideType.reset();
                this.createdBy.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
        }
    }
}
</script>
