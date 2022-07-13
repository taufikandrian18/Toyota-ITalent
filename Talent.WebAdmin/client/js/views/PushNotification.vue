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
            v-if="add === false && edit === false && detail === false"
        >
            <div class="col col-md-12">
                <h3><fa icon="bullhorn"></fa> Push Notification</h3>
                <br />
                <!--ADVANCE SEARCH-->
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    style="display: none"
                >
                    <h4>Search Guide</h4>
                    <br />
                    <!--2 Column Menu-->
                    <div class="row justify-content-between">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker
                                            class="v-date-style-report"
                                            :masks="{ input: 'DD/MM/YYYY' }"
                                            mode="range"
                                            :firstDayOfWeek="2"
                                            v-model="filterDate"
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

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Type of Guide</span>
                                </div>
                                <div class="col-md-8">
                                    <select
                                        class="form-control"
                                        v-model="filterGuideTypeName"
                                    >
                                        <option
                                            v-for="t in guideTypes.listGuideTypeModel"
                                            :value="t.name"
                                            >{{ t.name }}</option
                                        >
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Approval Status</span>
                                </div>
                                <div class="col-md-8">
                                    <select
                                        class="form-control"
                                        v-model="filterApprovalStatus"
                                    >
                                        <option
                                            v-for="a in approvalStatuses.listApprovalStatusModel"
                                            :value="a.approvalName"
                                            >{{ a.approvalName }}</option
                                        >
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--2 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Guide Title</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="filterGuideName"
                                        />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Created By</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="filterCreatedBy"
                                        />
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

                <div class="row mb-4">
                    <div class="col-auto">
                        <button class="btn btn-info" @click="handleFilter">
                            Filter
                        </button>
                    </div>
                    <div class="col-auto">
                        <div class="input-group">
                            <input
                                class="form-control"
                                placeholder="Search"
                                v-model="keyword"
                            />
                        </div>
                    </div>
                </div>

                <!--Table-->
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <div class="col-md-12 talent_magin_small">
                        <div
                            class="row align-items-center row justify-content-between"
                        >
                            <h4>Broadcast List</h4>
                            <button
                                v-if="crud.create"
                                class="btn talent_bg_cyan talent_roundborder talent_font_white"
                                @click.prevent="addClicked"
                            >
                                Add Broadcast
                            </button>
                        </div>
                    </div>
                    <div class="col-md-12 talent_magin_small">
                        <div
                            class="row align-items-center row justify-content-between"
                        >
                            <a
                                >Showing
                                {{
                                    guides.listGuideJoinModel == null
                                        ? 0
                                        : guides.listGuideJoinModel.length
                                }}
                                of {{ guides.totalItem }} Entry(s)</a
                            >
                            <button
                                v-if="crud.create"
                                class="btn btn-secondary talent_roundborder talent_font_white"
                                data-toggle="modal"
                                data-target="#modalDeleteSelected"
                            >
                                <fa icon="trash" /> Delete
                            </button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table
                            class="table table-bordered table-responsive-md talent_table_padding"
                        >
                            <thead style="color: white;">
                                <tr>
                                    <th scope="col" class="text-center">
                                        <input
                                            type="checkbox"
                                            id="selectAll"
                                            @change="handleCheckedAllItem"
                                        />
                                    </th>
                                    <th scope="col">
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            style="color: white;"
                                            @click="handleSorting('Sender')"
                                        >
                                            Sender
                                            <fa
                                                icon="sort"
                                                v-if="
                                                    accountParams.SortField !==
                                                        'Sender'
                                                "
                                            />
                                            <fa
                                                icon="sort-up"
                                                v-if="
                                                    accountParams.SortField ===
                                                        'Sender' &&
                                                        !accountParams.Sort
                                                "
                                            />
                                            <fa
                                                icon="sort-down"
                                                v-if="
                                                    accountParams.SortField ===
                                                        'Sender' &&
                                                        accountParams.Sort
                                                "
                                            />
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            style="color: white;"
                                            @click="handleSorting('Title')"
                                        >
                                            Title
                                            <fa
                                                icon="sort"
                                                v-if="
                                                    accountParams.SortField !==
                                                        'Title'
                                                "
                                            />
                                            <fa
                                                icon="sort-up"
                                                v-if="
                                                    accountParams.SortField ===
                                                        'Title' &&
                                                        !accountParams.Sort
                                                "
                                            />
                                            <fa
                                                icon="sort-down"
                                                v-if="
                                                    accountParams.SortField ===
                                                        'Title' &&
                                                        accountParams.Sort
                                                "
                                            />
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            style="color: white;"
                                            @click="handleSorting('Category')"
                                        >
                                            Category
                                            <fa
                                                icon="sort"
                                                v-if="
                                                    accountParams.SortField !==
                                                        'Category'
                                                "
                                            />
                                            <fa
                                                icon="sort-up"
                                                v-if="
                                                    accountParams.SortField ===
                                                        'Category' &&
                                                        !accountParams.Sort
                                                "
                                            />
                                            <fa
                                                icon="sort-down"
                                                v-if="
                                                    accountParams.SortField ===
                                                        'Category' &&
                                                        accountParams.Sort
                                                "
                                            />
                                        </a>
                                    </th>
                                    <th class="text-center">
                                        Status
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            style="color: white;"
                                            @click="handleSorting('Send Date')"
                                        >
                                            Send Date
                                            <fa
                                                icon="sort"
                                                v-if="
                                                    accountParams.SortField !==
                                                        'Send Date'
                                                "
                                            />
                                            <fa
                                                icon="sort-up"
                                                v-if="
                                                    accountParams.SortField ===
                                                        'Send Date' &&
                                                        !accountParams.Sort
                                                "
                                            />
                                            <fa
                                                icon="sort-down"
                                                v-if="
                                                    accountParams.SortField ===
                                                        'Send Date' &&
                                                        accountParams.Sort
                                                "
                                            />
                                        </a>
                                    </th>
                                    <th colspan="3" class="text-center">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="g in notifications" :key="g.guid">
                                    <th class="text-center">
                                        <input
                                            type="checkbox"
                                            @change="
                                                e => handleCheckedItem(g, e)
                                            "
                                            :checked="g.isSelected"
                                        />
                                    </th>
                                    <td>
                                        {{ g.senderName }}
                                    </td>
                                    <td>
                                        {{ g.title }}
                                    </td>
                                    <td>
                                        {{ g.category }}
                                    </td>
                                    <td>
                                        <div :class="`text-center badge py-2 w-100 ${g.isPublished ? 'badge-success' : 'badge-secondary'}`">
                                            {{ g.isPublished ? 'Published' : 'Draft' }}
                                        </div>
                                    </td>
                                    <td>
                                        {{ convertDateTime(g.sendDate) }}
                                    </td>
                                    <td class="talent_nopadding_important">
                                        <button
                                            class="btn btn-block talent_bg_orange talent_font_white"
                                            @click="handleDetail(g)"
                                        >
                                            View Detail
                                        </button>
                                    </td>
                                    <td class="talent_nopadding_important">
                                        <button
                                            class="btn btn-block talent_bg_cyan talent_font_white"
                                            @click="handleEdit(g)"
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
                                                deleteClicked(g.guid)
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
                            :currentPage.sync="accountParams.Page"
                            :total-data="totalData"
                            :limit-data="10"
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

                <modal-confirmation
                    name="modalDeleteSelected"
                    message="Are you sure want to delete this datas?"
                    @yes="handleDeleteBulk()"
                />
            </div>
        </div>
        <!--Add-->
        <div class="row" v-else-if="add === true">
            <div class="col-md-12">
                <h3>
                    <fa icon="bullhorn"></fa> Push Notification >
                    <span class="talent_font_red">Add Broadcast</span>
                </h3>
                <br />

                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Add Broadcast</h4>

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
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>To</label>
                                    <div class="w-100 d-flex">
                                        <div
                                            :class="
                                                `${
                                                    data.value === selectedTos
                                                        ? 'to-active'
                                                        : 'to'
                                                } mr-2`
                                            "
                                            v-for="data in tos"
                                            :key="data.value"
                                            @click="selectedTos = data.value"
                                        >
                                            {{ data.label }}
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12 mt-3" v-show="selectedTos != 'all'">
                                    <label>Group Position</label>
                                    <div class="w-100 d-flex">
                                        <div
                                            :class="`to-active mr-2`"
                                            v-for="data in tos.filter(v => v.value == 'all')"
                                            :key="data.value"
                                        >
                                            {{ data.label }}
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12 mt-3" v-show="selectedTos != 'all'">
                                    <label>Manpower Position</label>
                                    <div class="w-100 d-flex">
                                        <multiselect v-model="selectedPosition"
                                                        name="position"
                                                        :options="positions"
                                                        :multiple="true"
                                                        :allow-empty="true"
                                                        label="positionName"
                                                        track-by="positionName">
                                        </multiselect>
                                    </div>
                                </div>
                                <div class="col-md-12 mt-3">
                                    <label>Title</label>
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="model.title"
                                            maxlength="1024"
                                        />
                                    </div>
                                    <br />
                                    <!-- <label>Type of Guide <span class="talent_font_red">*</span></label>
                                    <select class="form-control" v-model="addModel.guideTypeId">
                                        <option v-for="t in guideTypes.listGuideTypeModel" :value="t.guideTypeId">{{t.name}}</option>
                                    </select> -->
                                </div>
                                <div class="col-md-12">
                                    <label>Description</label>
                                    <textarea
                                        class="form-control"
                                        style="height:130px; overflow-y:scroll;"
                                        v-model="model.description"
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
                    style="display: none"
                >
                    <h4>File Upload</h4>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>
                                        Upload File
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <div class="custom-file">
                                                        <input
                                                            type="file"
                                                            class="custom-file-input"
                                                            id="customFile"
                                                            @change="
                                                                onAddFileChange
                                                            "
                                                        />
                                                        <label
                                                            class="custom-file-label talent_textshorten"
                                                            for="customFile"
                                                            >{{
                                                                addFileName ==
                                                                null
                                                                    ? 'Choose File'
                                                                    : addFileName
                                                            }}</label
                                                        >
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    *Recommended File: PDF,
                                                    JPEG, JPG, PNG
                                                    <br />*Max File Size 5MB
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
                                        @click.prevent="saveData()"
                                        :disabled="isValid"
                                    >
                                        Save
                                    </button>
                                    <button
                                        class="btn talent_bg_darkblue talent_font_white"
                                        @click.prevent="submitData()"
                                        :disabled="isValid"
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
        <div class="row" v-else-if="edit === true">
            <div class="col-md-12">
                <h3>
                    <fa icon="bullhorn"></fa> Push Notification >
                    <span class="talent_font_red">Edit Broadcast</span>
                </h3>
                <br />
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>Edit Broadcast</h4>

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
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>To</label>
                                    <div class="w-100 d-flex">
                                        <div
                                            :class="
                                                `${
                                                    data.value === selectedTos
                                                        ? 'to-active'
                                                        : 'to'
                                                } mr-2`
                                            "
                                            v-for="data in tos"
                                            :key="data.value"
                                            @click="selectedTos = data.value"
                                        >
                                            {{ data.label }}
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12 mt-3">
                                    <label>Title</label>
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="selectedData.title"
                                            maxlength="1024"
                                        />
                                    </div>
                                    <br />
                                    <!-- <label>Type of Guide <span class="talent_font_red">*</span></label>
                                    <select class="form-control" v-model="addModel.guideTypeId">
                                        <option v-for="t in guideTypes.listGuideTypeModel" :value="t.guideTypeId">{{t.name}}</option>
                                    </select> -->
                                </div>
                                <div class="col-md-12">
                                    <label>Description</label>
                                    <textarea
                                        class="form-control"
                                        style="height:130px; overflow-y:scroll;"
                                        v-model="selectedData.body"
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
                    style="display: none"
                >
                    <h4>File Upload</h4>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>
                                        Upload File
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <div class="custom-file">
                                                        <input
                                                            type="file"
                                                            class="custom-file-input"
                                                            id="customFile"
                                                            @change="
                                                                onEditFileChange
                                                            "
                                                        />
                                                        <label
                                                            class="custom-file-label talent_textshorten"
                                                            for="customFile"
                                                            >{{
                                                                editFileName ==
                                                                null
                                                                    ? 'Choose File'
                                                                    : editFileName
                                                            }}</label
                                                        >
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    *Recommended File: PDF, JPG,
                                                    JPEG, PNG
                                                    <br />*Max File Size 5MB
                                                </div>
                                            </div>
                                        </div>
                                    </div>
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
                            v-else
                            class="h-100 w-100 min-heigth-full"
                            :data="editFileData"
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
                                        @click.prevent="saveUpdate()"
                                        :disabled="
                                            selectedData.body == '' ||
                                                selectedData.title == ''
                                        "
                                    >
                                        Save
                                    </button>
                                    <button
                                        class="btn talent_bg_darkblue talent_font_white"
                                        @click.prevent="submitUpdate()"
                                        :disabled="
                                            selectedData.body == '' ||
                                                selectedData.title == ''
                                        "
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
        <div class="row" v-else-if="detail === true">
            <div class="col-md-12">
                <h3>
                    <fa icon="bullhorn"></fa> Push Notification >
                    <span class="talent_font_red">View Detail Broadcast</span>
                </h3>
                <br />

                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <h4>View Detail Broadcast</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-12">
                                            <label>To</label>
                                            <div class="w-100 d-flex">
                                                <div
                                                    :class="
                                                        `${
                                                            data.value ===
                                                            selectedTos
                                                                ? 'to-active'
                                                                : 'to'
                                                        } mr-2`
                                                    "
                                                    v-for="data in tos"
                                                    :key="data.value"
                                                    @click="
                                                        selectedTos = data.value
                                                    "
                                                >
                                                    {{ data.label }}
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-12 mt-3">
                                            <label>Title</label>
                                            <div class="input-group">
                                                <input
                                                    class="form-control"
                                                    v-model="selectedData.title"
                                                    maxlength="1024"
                                                    disabled
                                                />
                                            </div>
                                            <br />
                                            <!-- <label>Type of Guide <span class="talent_font_red">*</span></label>
                                    <select class="form-control" v-model="addModel.guideTypeId">
                                        <option v-for="t in guideTypes.listGuideTypeModel" :value="t.guideTypeId">{{t.name}}</option>
                                    </select> -->
                                        </div>
                                        <div class="col-md-12">
                                            <label>Description</label>
                                            <textarea
                                                class="form-control"
                                                style="height:130px; overflow-y:scroll;"
                                                v-model="selectedData.body"
                                                maxlength="1024"
                                                disabled
                                            ></textarea>
                                        </div>
                                    </div>
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
import { Watch } from 'vue-property-decorator';
import { Sort } from '../class/Sort';
import {
    GuideService,
    GuideTypeService,
    GuideFormModel,
    GuideTypeViewModel,
    GuideJoinViewModel,
    ApprovalStatusService,
    FileContent,
    UserPrivilegeSettingsService,
    UserAccessCRUD,
    PushNotificationService,
    UserRoleService,
    ReleaseTrainingService
} from '../services/NSwagService';
import { BlobService } from '../services/BlobService';
import { isNullOrUndefined } from 'util';
import { PageEnums } from '../enum/PageEnums';
import ModalConfirmation from '../components/shared/ModalConfirmation.vue';

@Component({
    components: { ModalConfirmation },
    created: async function(this: PushNotification) {
        this.isBusy = true;
        await this.initDropdownData();
        await this.getAccess();
        if (this.fromOutside === true) {
            await this.detailClicked(this.guideId);
        }
        await this.fetchPositionAll()
        await this.fetch();
    },
    props: ['guideId', 'fromOutside', 'claims']
})
export default class PushNotification extends Vue {
    @Watch('accountParams')
    onPropertyChanged(value: string, oldValue: string) {
        this.fetch();
    }

    @Watch('edit')
    watchEdit(value: string, oldValue: string) {
        this.selectedTos = 'all';
    }
    @Watch('detail')
    watchDetail(value: string, oldValue: string) {
        this.selectedTos = 'all';
    }
    @Watch('add')
    watchAdd(value: string, oldValue: string) {
        this.selectedTos = 'all';
    }

    @Watch('selectedTos')
    watchSelectedTos() {
        if(this.selectedTos == 'all') {
            this.positions =[]
            this.selectedPosition = []
            this.fetchPositionAll()
        }
        if(this.selectedTos == 'dealer') {
            this.positions =[]
            this.selectedPosition = []
            this.fetchPositionDealer()
        }
        if(this.selectedTos == 'tam') {
            this.positions =[]
            this.selectedPosition = []
            this.fetchPositionTam()
        }
        if(this.selectedTos == 'other') {
            this.positions =[]
            this.selectedPosition = []
            // this.fetchPositionsAll()
        }
    }

    positionsService: UserRoleService = new UserRoleService()
    trainingService: ReleaseTrainingService = new ReleaseTrainingService()

    async fetchPositionAll() {
        this.isBusy = true
        try {
            const res = await this.trainingService.getAllPosition()
            this.positions = res
        } catch (err) {
            console.log(err)
        }
        this.isBusy = false
    }
    async fetchPositionDealer() {
        this.isBusy = true
        try {
            const res = await this.positionsService.tamPositionDropdown()
            this.positions = res
        } catch (err) {
            console.log(err)
        }
        this.isBusy = false
    }
    async fetchPositionTam() {
        this.isBusy = true
        try {
            const res = await this.positionsService.tamPositionDropdown()
            this.positions = res
        } catch (err) {
            console.log(err)
        }
        this.isBusy = false
    }

    get isValid() {
        return this.model.title === '' || this.model.description === '';
    }

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
    claims: any;

    fileSize: boolean = true;
    fileType: boolean = true;

    positions: any = []
    selectedPosition: any = []

    alertClose() {
        this.success = false;
        this.successMessage = '';
    }

    tos: any = [
        {
            label: 'All',
            value: 'all'
        },
        {
            label: 'Dealer',
            value: 'dealer'
        },
        {
            label: 'TAM',
            value: 'tam'
        },
        {
            label: 'Others',
            value: 'other'
        }
    ];
    selectedTos: any = 'all';

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
    SortField: string = '';

    guideMan: GuideService = new GuideService();
    guideTypeMan: GuideTypeService = new GuideTypeService();
    guides: GuideJoinViewModel = {};
    guideTypes: GuideTypeViewModel = {};
    approvalMan: ApprovalStatusService = new ApprovalStatusService();
    approvalStatuses = {};

    service: PushNotificationService = new PushNotificationService();

    notifications: any = [];
    accountParams: any = {
        id: '',
        SenderId: '',
        CategoryId: '',
        Page: 1,
        Limit: 10,
        Sort: true,
        SortField: '',
        Query: ''
    };
    keyword: any = '';
    totalData: any = 0;
    model: any = {
        title: '',
        description: ''
    };
    selectedData: any = {};

    async insertData(model) {
        this.isBusy = true;
        try {
            const res = await this.service.insertPushNotification(model);
            this.model = {
                title: '',
                description: '',
            };
            this.add = false;
            this.fetch();
        } catch (err) {
            console.log(err);
        }
        this.isBusy = false;
    }
    async updateData(model) {
        this.isBusy = true;
        try {
            const res = await this.service.updatePushNotification(model);
            this.edit = false;
            this.fetch();
        } catch (err) {
            console.log(err);
        }
        this.isBusy = false;
    }

    handleCheckedItem(item, e) {
        this.notifications = this.notifications.map(v => ({
            ...v,
            isSelected: v.guid === item.guid ? e.target.checked : v.isSelected
        }));
    }
    handleCheckedAllItem(e) {
        this.notifications = this.notifications.map(v => ({
            ...v,
            isSelected: e.target.checked
        }));
    }

    saveData() {
        this.insertData({
            title: this.model.title,
            body: this.model.description,
            senderId: this.claims.EmployeeId,
            isPublished: false,
            groupPositions: [],
            manPowerPositions: this.selectedPosition.map(v => v.positionId)
        });
    }

    submitData() {
        this.insertData({
            title: this.model.title,
            body: this.model.description,
            senderId: this.claims.EmployeeId,
            isPublished: true,
            groupPositions: [],
            manPowerPositions: this.selectedPosition.map(v => v.positionId)
        });
    }

    saveUpdate() {
        this.updateData({
            guid: this.selectedData.guid,
            title: this.selectedData.title,
            body: this.selectedData.body,
            isPublished: false,
            senderEmployeeId: this.claims.EmployeeId
        });
    }

    submitUpdate() {
        this.updateData({
            guid: this.selectedData.guid,
            title: this.selectedData.title,
            body: this.selectedData.body,
            isPublished: true,
            senderEmployeeId: this.claims.EmployeeId
        });
    }

    handleFilter() {
        this.accountParams = {
            ...this.accountParams,
            Page: 1,
            Query: this.keyword
        };
    }

    async handleDeleteBulk() {
        this.isBusy = true;
        try {
            await Promise.all(
                this.notifications
                    .filter(v => v.isSelected)
                    .map(v => this.service.deleteAnnouncement(v.guid))
            );

            this.fetch();
        } catch (err) {
            console.log(err);
        }
        this.isBusy = false;
    }

    //Sorting
    handleSorting(key) {
        this.accountParams = {
            ...this.accountParams,
            SortField: key,
            Sort: !this.accountParams.Sort
        };
    }

    handleEdit(edit) {
        this.selectedData = edit;
        this.edit = true;
    }

    handleDetail(data) {
        this.selectedData = data;
        this.detail = true;
    }

    ///////////////////////////////////////////////

    async fetch() {
        this.isBusy = true;
        const res: any = await this.service.getPushNotifications({
            ...this.accountParams,
            Sort: this.accountParams.Sort ? 'ASC' : 'DESC'
        });
        this.notifications = res.data.map(v => ({ ...v, isSelected: false }));
        this.totalData = res.count;
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
            extension != 'png'
        ) {
            this.fileType = false;
        }
        if (fileInput[0].size > 5048576) {
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
            extension != 'png'
        ) {
            this.fileType = false;
        }
        if (fileInput[0].size > 5048576) {
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
        }
        this.detail = true;
        this.isBusy = false;
    }

    //Delete
    deleteGuideId;
    deleteIndex;

    async deleteClicked(guid) {
        this.alertClose();

        if (this.crud.delete == false) {
            return;
        }

        this.deleteGuideId = guid;
    }

    async deleteData() {
        this.isBusy = true;
        await this.service.deleteAnnouncement(this.deleteGuideId);
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
            this.SortField = 'guideName';
        } else if (this.guideName.sortdown == true) {
            this.SortField = 'guideNameDesc';
        } else {
            this.SortField = '';
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
            this.SortField = 'guideType';
        } else if (this.guideType.sortdown == true) {
            this.SortField = 'guideTypeDesc';
        } else {
            this.SortField = '';
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
            this.SortField = 'createdBy';
        } else if (this.createdBy.sortdown == true) {
            this.SortField = 'createdByDesc';
        } else {
            this.SortField = '';
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
            this.SortField = 'approvalStatus';
        } else if (this.approvalStatus.sortdown == true) {
            this.SortField = 'approvalStatusDesc';
        } else {
            this.SortField = '';
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
            this.SortField = 'createdDate';
        } else if (this.createdDate.sortdown == true) {
            this.SortField = 'createdDateDesc';
        } else {
            this.SortField = '';
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
            this.SortField = 'updatedDate';
        } else if (this.updatedDate.sortdown == true) {
            this.SortField = 'updatedDateDesc';
        } else {
            this.SortField = '';
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
                this.SortField = '';
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

<style scoped>
.to {
    border-radius: 9999px;
    padding: 4px 1rem;
    color: #acacac;
    background-color: #dadada;
    cursor: pointer;
}
.to-active {
    border-radius: 9999px;
    padding: 4px 1rem;
    color: white;
    background-color: #43b5d7;
    cursor: pointer;
}
</style>
