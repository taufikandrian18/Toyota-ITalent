<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <div
            v-if="successMessageShow == true"
            class="alert alert-success alert-dismissible fade show"
            role="alert"
        >
            {{ successMessage }}
            <button
                type="button"
                class="close"
                data-dismiss="alert"
                @click.prevent="ResetSuccessMessage()"
                aria-label="Close"
            >
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <div
            v-if="errorMessageShow == true"
            class="alert alert-danger alert-dismissible fade show"
            role="alert"
        >
            {{ errorMessage }}
            <button
                type="button"
                class="close"
                data-dismiss="alert"
                aria-label="Close"
                @click.prevent="ResetErrorMessage()"
            >
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <div
            class="row"
            v-if="add === false && edit === false && viewDetail === false"
        >
            <div class="col col-md-12">
                <!--TITLE-->
                <h3>
                    <fa icon="user"></fa> User Management >
                    <span class="talent_font_red">User Role</span>
                </h3>
                <br />

                <div v-if="!add && !edit">
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
                                                    v-model="filter.DateFilter"
                                                    mode="range"
                                                    :firstDayOfWeek="1"
                                                    :value="null"
                                                    :input-props="props"
                                                    :masks="{
                                                        input: 'DD/MM/YYYY'
                                                    }"
                                                ></v-date-picker>
                                                <div class="input-group-append">
                                                    <span
                                                        class="input-group-text"
                                                        ><fa
                                                            icon="calendar-alt"
                                                        ></fa
                                                    ></span>
                                                </div>
                                            </div>
                                            <!-- User Role -->
                                            <div class="mb-1">User Role</div>
                                            <div class="input-group mb-2">
                                                <input
                                                    class="form-control"
                                                    v-model="filter.UserRole"
                                                />
                                            </div>
                                        </div>
                                        <!-- Right -->
                                        <div class="col-md-6">
                                            <!-- Type of People -->
                                            <div class="mb-1">
                                                Type of People
                                            </div>
                                            <div class="mb-2">
                                                <select
                                                    class="form-control"
                                                    v-model="
                                                        filter.TypeOfPeople
                                                    "
                                                >
                                                    <option value></option>
                                                    <option :value="true"
                                                        >TAM People</option
                                                    >
                                                    <option :value="false"
                                                        >Dealer People</option
                                                    >
                                                </select>
                                            </div>
                                            <!-- Position -->
                                            <div class="mb-1">Position</div>
                                            <div class="input-group mb-2">
                                                <input
                                                    class="form-control"
                                                    v-model="filter.Position"
                                                />
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
                                                        @click.prevent="reset()"
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
                        <!-- Add User Role -->
                        <div class="col d-flex justify-content-end">
                            <button
                                v-if="crud.create"
                                class="btn talent_bg_darkblue talent_font_white"
                                @click="addButtonClick()"
                            >
                                + Add User Role
                            </button>
                        </div>
                    </div>

                    <div
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <!-- Header -->
                        <div class="row">
                            <div class="col">
                                <h4>User Role List</h4>
                            </div>
                            <div class="col-auto">
                                <button
                                    class="btn talent_bg_red talent_font_white"
                                >
                                    Delete
                                </button>
                            </div>
                        </div>
                        <hr />
                        <!-- Content -->
                        <div class="col-md-12 talent_magin_small">
                            <div
                                class="row align-items-center row justify-content-between"
                            >
                                <a
                                    >Showing {{ UserRoleListData.length }} of
                                    {{ userRoleGrid.totalDataFilter }}
                                    Entry(s)</a
                                >
                            </div>
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
                                                />
                                            </div>
                                        </th>
                                        <th>
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="UserRoleSort()"
                                                >User Role
                                                <fa
                                                    icon="sort"
                                                    v-if="
                                                        userRoleName.sort ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        userRoleName.sortup ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        userRoleName.sortdown ==
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
                                                    TypeOfPeopleSort()
                                                "
                                                >Type Of People
                                                <fa
                                                    icon="sort"
                                                    v-if="
                                                        typeOfPeople.sort ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        typeOfPeople.sortup ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        typeOfPeople.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th>
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="PositionSort()"
                                                >Position
                                                <fa
                                                    icon="sort"
                                                    v-if="position.sort == true"
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        position.sortup == true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        position.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th>
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="CreatedAtSort()"
                                                >Created Date
                                                <fa
                                                    icon="sort"
                                                    v-if="
                                                        createdDate.sort == true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        createdDate.sortup ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        createdDate.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th>
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="UpdatedAtSort()"
                                                >Updated Date
                                                <fa
                                                    icon="sort"
                                                    v-if="
                                                        updatedDate.sort == true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        updatedDate.sortup ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        updatedDate.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th colspan="3" class="text-center">
                                            Action
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr
                                        v-for="userRole in UserRoleListData"
                                        :key="userRole.id"
                                    >
                                        <th class="text-center">
                                            <div class="form-check">
                                                <input
                                                    class="form-check-input"
                                                    type="checkbox"
                                                    value=""
                                                    id="item"
                                                />
                                            </div>
                                        </th>
                                        <td>
                                            {{ userRole.userRoleName }}
                                        </td>
                                        <td>
                                            {{ userRole.typeOfPeople }}
                                        </td>
                                        <td>
                                            {{ userRole.position.positionName }}
                                        </td>
                                        <td>
                                            {{
                                                userRole.createdAt | dateFormat
                                            }}
                                        </td>
                                        <td>
                                            {{
                                                userRole.updatedAt | dateFormat
                                            }}
                                        </td>
                                        <td
                                            v-if="crud.read"
                                            class="talent_nopadding_important"
                                        >
                                            <button
                                                class="btn btn-block talent_bg_orange talent_font_white"
                                                @click="
                                                    viewDetailButtonClick(
                                                        userRole.userRoleId
                                                    )
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
                                                @click="
                                                    editButtonClick(
                                                        userRole.userRoleId
                                                    )
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
                                                type="button"
                                                class="btn btn-block talent_bg_red talent_font_white"
                                                data-backdrop="static"
                                                data-keyboard="false"
                                                data-toggle="modal"
                                                data-target="#deleteConfirmation"
                                                @click="
                                                    setDelete(
                                                        userRole.userRoleId,
                                                        userRole.userRoleName
                                                    )
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
                                    :currentPage.sync="filter.pageIndex"
                                    :total-data="userRoleGrid.totalDataFilter"
                                    :limit-data="filter.pageSize"
                                    @update:currentPage="fetch()"
                                ></paginate>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />

        <!--Add-->
        <div class="row" v-if="add">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3>
                    <fa icon="user"></fa> User Management > User Role >
                    <span class="talent_font_red">Add User Role</span>
                </h3>
                <br />
                <form method="post" @submit.prevent="insertForm">
                    <div
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>User Role Information</h4>
                        <div
                            class="alert alert-danger"
                            v-show="errors.items.length > 0"
                        >
                            <div v-for="error in errors.all()" :key="error.id">
                                {{ error }}
                            </div>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-md-6">
                                <!--User Role-->
                                <div>
                                    <label
                                        >User Role<span class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            maxlength="64"
                                            class="form-control"
                                            v-model="userRoleForm.userRoleName"
                                            v-validate="'required'"
                                            name="UserRoleName"
                                        />
                                    </div>
                                </div>

                                <!--Type of people-->
                                <div class="mt-2">
                                    <label
                                        >Type of People<span
                                            class="talent_font_red"
                                            >*</span
                                        ></label
                                    ><br />
                                    <div class="form-check form-check-inline">
                                        <input
                                            class="form-check-input"
                                            type="radio"
                                            name="typeOfPeople"
                                            :value="true"
                                            id="tampeople"
                                            v-model="userRoleForm.typeOfPeople"
                                            v-validate="'required'"
                                            @change="changeTypeOfPeople()"
                                        />
                                        <label
                                            class="form-check-label"
                                            for="tampeople"
                                            >TAM People</label
                                        >
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input
                                            class="form-check-input"
                                            type="radio"
                                            name="typeOfPeople"
                                            :value="false"
                                            id="dealerpeople"
                                            v-model="userRoleForm.typeOfPeople"
                                            @change="changeTypeOfPeople()"
                                            v-validate="'required'"
                                        />
                                        <label
                                            class="form-check-label"
                                            for="dealerpeople"
                                            >Dealer People</label
                                        >
                                    </div>
                                </div>
                            </div>

                            <!--User Role Description-->
                            <div class="col-md-6">
                                <label>User Role Description</label>
                                <textarea
                                    class="form-control talent_textarea"
                                    v-model="userRoleForm.roleDescription"
                                ></textarea>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div
                        v-if="userRoleForm.typeOfPeople == true"
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>TAM People</h4>

                        <div class="row justify-content-between">
                            <div class="col-md-12">
                                <label
                                    >Position<span class="talent_font_red"
                                        >*</span
                                    ></label
                                >
                                <multiselect
                                    v-model="userRoleForm.position"
                                    name="Tam Position"
                                    :options="listTamPosition"
                                    :allow-empty="false"
                                    label="positionName"
                                    deselect-label="Can't remove this value"
                                    v-validate="'required'"
                                >
                                </multiselect>
                            </div>
                        </div>
                    </div>

                    <div
                        v-if="userRoleForm.typeOfPeople == false"
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>Dealer People</h4>

                        <div class="row justify-content-between">
                            <div class="col-md-12">
                                <label
                                    >Category<span class="talent_font_red"
                                        >*</span
                                    ></label
                                >
                                <div class="input-group">
                                    <multiselect
                                        v-model="userRoleForm.dealerCategory"
                                        name="Category"
                                        :options="listCategory"
                                        :allow-empty="false"
                                        label="categoryName"
                                        deselect-label="Can't remove this value"
                                        v-validate="'required'"
                                        @input="fetchDealerPosition()"
                                    >
                                    </multiselect>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <label
                                    >Position<span class="talent_font_red"
                                        >*</span
                                    ></label
                                >
                                <div
                                    class="input-group"
                                    value="userRoleForm.positionId"
                                >
                                    <!--<select class="form-control" v-model="userRoleForm.positionId" v-validate="'required'" name="Dealer Position">
                                        <option v-for="position in listDealerPosition" :value="position.positionId">{{position.positionName}}</option>
                                    </select>-->

                                    <multiselect
                                        v-model="userRoleForm.position"
                                        name="Dealer Position"
                                        :options="listDealerPosition"
                                        :allow-empty="false"
                                        label="positionName"
                                        deselect-label="Can't remove this value"
                                        v-validate="'required'"
                                    >
                                    </multiselect>
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
                                            @click.prevent="closeButtonClick()"
                                            type="button"
                                        >
                                            Close
                                        </button>
                                        <button
                                            class="btn talent_bg_lightgreen talent_font_white"
                                            @click.prevent="insertForm()"
                                            type="submit"
                                        >
                                            Save
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>

        <!--Edit-->
        <div class="row" v-if="edit">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3>
                    <fa icon="user"></fa> User Management > User Role >
                    <span class="talent_font_red">Edit User Role</span>
                </h3>
                <br />
                <form method="post" @submit.prevent="insertForm">
                    <div
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>User Role Information</h4>
                        <div
                            class="alert alert-danger"
                            v-show="errors.items.length > 0"
                        >
                            <div v-for="error in errors.all()" :key="error.id">
                                {{ error }}
                            </div>
                        </div>
                        <br />

                        <div class="row">
                            <div class="col-md-6">
                                <!--User Role-->
                                <div>
                                    <label
                                        >User Role<span class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="userRoleForm.userRoleName"
                                            v-validate="'required'"
                                            name="UserUserRoleName"
                                        />
                                    </div>
                                </div>

                                <!--Type of people-->
                                <div class="mt-2">
                                    <label
                                        >Type of People<span
                                            class="talent_font_red"
                                            >*</span
                                        ></label
                                    ><br />
                                    <div class="form-check form-check-inline">
                                        <input
                                            class="form-check-input"
                                            type="radio"
                                            name="typeOfPeople"
                                            :value="true"
                                            id="tampeople"
                                            v-model="userRoleForm.typeOfPeople"
                                            @change="changeTypeOfPeople()"
                                        />
                                        <label
                                            class="form-check-label"
                                            for="tampeople"
                                            >TAM People</label
                                        >
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input
                                            class="form-check-input"
                                            type="radio"
                                            name="typeOfPeople"
                                            :value="false"
                                            id="dealerpeople"
                                            v-model="userRoleForm.typeOfPeople"
                                            @change="changeTypeOfPeople()"
                                        />
                                        <label
                                            class="form-check-label"
                                            for="dealerpeople"
                                            >Dealer People</label
                                        >
                                    </div>
                                </div>
                            </div>
                            <!--User Role Description-->
                            <div class="col-md-6">
                                <label>User Role Description</label>
                                <textarea
                                    class="form-control talent_textarea"
                                    v-model="userRoleForm.roleDescription"
                                ></textarea>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div
                        v-if="userRoleForm.typeOfPeople"
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>TAM People</h4>

                        <div class="row justify-content-between">
                            <div class="col-md-12">
                                <label
                                    >Position<span class="talent_font_red"
                                        >*</span
                                    ></label
                                >
                                <multiselect
                                    v-model="userRoleForm.position"
                                    name="Tam Position"
                                    :options="listTamPosition"
                                    :allow-empty="false"
                                    label="positionName"
                                    deselect-label="Can't remove this value"
                                    v-validate="'required'"
                                >
                                </multiselect>
                            </div>
                        </div>
                    </div>

                    <div
                        v-if="!userRoleForm.typeOfPeople"
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>Dealer People</h4>

                        <div class="row justify-content-between">
                            <div class="col-md-12">
                                <label
                                    >Category<span class="talent_font_red"
                                        >*</span
                                    ></label
                                >
                                <div class="input-group">
                                    <multiselect
                                        v-model="userRoleForm.dealerCategory"
                                        name="Category"
                                        :options="listCategory"
                                        :allow-empty="false"
                                        label="categoryName"
                                        deselect-label="Can't remove this value"
                                        v-validate="'required'"
                                        @input="fetchDealerPosition()"
                                    >
                                    </multiselect>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <label
                                    >Position<span class="talent_font_red"
                                        >*</span
                                    ></label
                                >
                                <div class="input-group">
                                    <multiselect
                                        v-model="userRoleForm.position"
                                        name="Dealer Position"
                                        :options="listDealerPosition"
                                        :allow-empty="false"
                                        label="positionName"
                                        deselect-label="Can't remove this value"
                                        v-validate="'required'"
                                    >
                                    </multiselect>
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
                                            @click.prevent="closeButtonClick()"
                                            type="button"
                                        >
                                            Close
                                        </button>
                                        <button
                                            class="btn talent_bg_lightgreen talent_font_white"
                                            @click.prevent="editForm()"
                                            type="submit"
                                        >
                                            Save
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>

        <!--View Detail-->
        <div class="row" v-if="viewDetail">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3>
                    <fa icon="user"></fa> User Management > User Role >
                    <span class="talent_font_red">View Detail User Role</span>
                </h3>
                <br />
                <form method="post" @submit.prevent="insertForm">
                    <div
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>User Role Information</h4>

                        <br />

                        <div class="row">
                            <!--User Role-->
                            <div class="col-md-6">
                                <div>
                                    <label
                                        >User Role<span class="talent_font_red"
                                            >*</span
                                        ></label
                                    >
                                    <div class="input-group">
                                        <input
                                            class="form-control"
                                            v-model="userRoleForm.userRoleName"
                                            name="User Role"
                                            disabled
                                        />
                                    </div>
                                </div>

                                <!--Type of people-->
                                <div class="mt-2">
                                    <label
                                        >Type of People<span
                                            class="talent_font_red"
                                            >*</span
                                        ></label
                                    ><br />
                                    <div class="form-check form-check-inline">
                                        <input
                                            class="form-check-input"
                                            type="radio"
                                            name="typeOfPeople"
                                            :value="true"
                                            id="tampeople"
                                            v-model="userRoleForm.typeOfPeople"
                                            disabled
                                        />
                                        <label
                                            class="form-check-label"
                                            for="tampeople"
                                            >TAM People</label
                                        >
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input
                                            class="form-check-input"
                                            type="radio"
                                            name="typeOfPeople"
                                            :value="false"
                                            id="dealerpeople"
                                            v-model="userRoleForm.typeOfPeople"
                                            disabled
                                        />
                                        <label
                                            class="form-check-label"
                                            for="dealerpeople"
                                            >Dealer People</label
                                        >
                                    </div>
                                </div>
                            </div>
                            <br />

                            <!--User Role Description-->
                            <div class="col-md-6">
                                <label>User Role Description</label>
                                <textarea
                                    class="form-control talent_textarea"
                                    v-model="userRoleForm.roleDescription"
                                    disabled
                                ></textarea>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div
                        v-if="userRoleForm.typeOfPeople"
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>TAM People</h4>

                        <div class="row justify-content-between">
                            <div class="col-md-12">
                                <label
                                    >Position<span class="talent_font_red"
                                        >*</span
                                    ></label
                                >
                                <multiselect
                                    v-model="userRoleForm.position"
                                    name="Tam Position"
                                    :options="listTamPosition"
                                    :allow-empty="false"
                                    label="positionName"
                                    deselect-label="Can't remove this value"
                                    disabled
                                >
                                </multiselect>
                            </div>
                        </div>
                    </div>

                    <div
                        v-if="!userRoleForm.typeOfPeople"
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>Dealer People</h4>

                        <div class="row justify-content-between">
                            <div class="col-md-12">
                                <label
                                    >Category<span class="talent_font_red"
                                        >*</span
                                    ></label
                                >
                                <div class="input-group">
                                    <multiselect
                                        v-model="userRoleForm.dealerCategory"
                                        name="Category"
                                        :options="listCategory"
                                        :allow-empty="false"
                                        label="categoryName"
                                        deselect-label="Can't remove this value"
                                        :disabled="true"
                                    >
                                    </multiselect>
                                </div>
                            </div>

                            <div class="col-md-12">
                                <label
                                    >Position<span class="talent_font_red"
                                        >*</span
                                    ></label
                                >
                                <multiselect
                                    v-model="userRoleForm.position"
                                    name="Dealer Position"
                                    :options="listDealerPosition"
                                    label="positionName"
                                    :disabled="true"
                                >
                                </multiselect>
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
                                            @click.prevent="closeButtonClick()"
                                            type="button"
                                        >
                                            Close
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </form>
            </div>
        </div>

        <!--Delete Confirmation only-->
        <div
            class="modal fade"
            id="deleteConfirmation"
            tabindex="-1"
            role="dialog"
            aria-labelledby="deleteConfirmLabel"
            aria-hidden="true"
        >
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="col-md-12 justify-content-center d-flex">
                            <h5>Are you sure want to delete?</h5>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div
                                class="col-md-12 d-flex justify-content-around"
                            >
                                <button
                                    type="button"
                                    class="btn talent_bg_red talent_font_white talent_roundborder"
                                    data-dismiss="modal"
                                    @click="
                                        deleteRole(toBeDeletedRole.userRoleId)
                                    "
                                >
                                    Delete
                                </button>
                                <button
                                    type="button"
                                    class="btn talent_bg_blue talent_font_white talent_roundborder"
                                    data-dismiss="modal"
                                    @click="emptyDelete"
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
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { Sort } from '../../class/Sort';
import {
    UserRoleService,
    UserRoleModelCreate,
    UserRoleModelUpdate,
    UserRoleGridModel,
    UserRoleDropdownModel,
    CategoryDropdownModel,
    PositionDropdownModel,
    UserRoleModelView,
    UserAccessCRUD,
    UserPrivilegeSettingsService
} from '../../services/NSwagService';
import { IUserRoleFormModel } from '../../models/IUserRoleFormModel';
import { UserRoleModelViewDetail } from '../../services/NSwagService';
import Message from '../../class/Message';
import { PageEnums } from '../../enum/PageEnums';

@Component({
    mounted: async function(this: UserRole) {
        await this.getAccess();
        await this.fetch();
    },

    props: ['userRoleId']
})
export default class UserRole extends Vue {
    dealerCategoryId: number = 0;
    Service: UserRoleService = new UserRoleService();
    privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
    mode: string;
    currentPage: number = 1;
    isLoading: boolean = null;
    previouseName: string = '';

    props: any = {
        placeholder: '',
        readonly: true
    };

    filter: IUserRoleFilter = {
        UserRole: '',
        TypeOfPeople: null,
        Position: '',
        DateFilter: {
            start: null,
            end: null
        },
        SortBy: '',
        pageIndex: 1,
        pageSize: 10
    };

    typeOfPeopleDropdown: any = [
        {
            typeOfPeopleId: true,
            typeOfPeopleName: 'TAM People'
        },
        {
            typeOfPeopleId: false,
            typeOfPeopleName: 'Dealer People'
        }
    ];

    UserRoleService: UserRoleService = new UserRoleService();
    listTamPosition: PositionDropdownModel[] = [];
    listDealerPosition: PositionDropdownModel[] = [];
    listUserRole: UserRoleDropdownModel[] = [];
    listCategory: CategoryDropdownModel[] = [];
    UserRoleListData: UserRoleModelView[] = [];

    UserPositions: { id: number; name: string }[] = [];

    userRoleGrid: UserRoleGridModel = {
        userRole: [],
        totalData: 0,
        totalDataFilter: 0
    };

    //show success, success message
    successMessageShow: boolean = false;
    successMessage: string = '';
    errorMessageShow: boolean = false;
    errorMessage: string = '';

    ResetSuccessMessage() {
        this.successMessageShow = false;
        this.successMessage = '';
    }

    ResetErrorMessage() {
        this.errorMessageShow = false;
        this.errorMessage = '';
    }

    toBeDeletedRole: { userRoleId: number; userRoleName: string } = {
        userRoleId: 0,
        userRoleName: ''
    };

    setDelete(userRoleId: number, userRoleName: string) {
        if (!this.crud.delete) {
            return;
        }
        this.toBeDeletedRole = {
            userRoleId: userRoleId,
            userRoleName: userRoleName
        };
    }

    emptyDelete() {
        this.toBeDeletedRole = {
            userRoleId: 0,
            userRoleName: ''
        };
    }

    async deleteRole(userRoleId: number) {
        this.isLoading = true;
        var isSuccess = await this.Service.deleteUserRole(userRoleId);

        if (isSuccess == false) {
            await this.fetch();
            this.errorMessage =
                'This Role is being used in User Privilege Settings.';
            this.errorMessageShow = true;
            return;
        }
        await this.fetch();
        this.successMessage = Message.RemoveMessage;
        this.successMessageShow = true;
    }

    //Fetch position for Tam people dropdown
    async fetchTamPosition() {
        this.listTamPosition = await this.UserRoleService.tamPositionDropdown();
    }

    //Fetch position for Dealer people dropdown by Dealer Category Id
    async fetchDealerPosition() {
        this.listDealerPosition = await this.UserRoleService.dealerPositionDropdown();
    }

    //Fetch user role for dropdown
    async fetchUserRole() {
        this.listUserRole = await this.UserRoleService.userRoleDropdown();
    }

    //Fetch dealer category for dropdown
    async fetchCategory() {
        this.listCategory = await this.UserRoleService.categoryDropdown();
    }

    //user privilege setting
    async getAccess() {
        var data = await this.privilegeApi.crudAccessPage(PageEnums.UserRole);
        this.crud = data;
    }

    crud: UserAccessCRUD = {
        create: false,
        delete: false,
        read: false,
        update: false
    };

    //Fetch data from Database
    async fetch() {
        this.$validator.pause();
        this.isLoading = true;
        await this.fetchUserRole();
        await this.fetchTamPosition();
        await this.fetchCategory();
        await this.UserRoleService.getRole;

        this.filter.DateFilter.end
            ? this.filter.DateFilter.end.setHours(23, 59, 59)
            : this.filter.DateFilter.end;

        this.userRoleGrid = await this.Service.getRole(
            this.filter.UserRole,
            this.filter.TypeOfPeople,
            this.filter.Position,
            this.filter.DateFilter.start,
            this.filter.DateFilter.end,
            this.filter.SortBy,
            this.filter.pageIndex,
            this.filter.pageSize
        );
        this.UserRoleListData = this.userRoleGrid.userRole;

        this.add = false;
        this.edit = false;
        this.viewDetail = false;
        this.isLoading = false;
        this.closeFilter();
    }

    //to check if role name exist in database
    async roleNameIsExistCreate(roleName: string) {
        roleName = this.userRoleForm.userRoleName;
        let nameIsExist = await this.UserRoleService.getUserRoleName(roleName);
        if (nameIsExist == true) {
            this.$validator.errors.add({
                field: 'UserRoleName',
                msg: "User Role's Name already exist, please use another name"
            });
            return false;
        }

        return true;
    }

    //to check if user role id's email same with form email, if not same as the one at database, check if role name is already exist
    async roleNameIsExistUpdate(roleName: string) {
        roleName = this.userRoleForm.userRoleName;
        let nameIsExistById = await this.UserRoleService.getUserRoleNameById(
            this.updateForm.userRoleId,
            roleName
        );
        let nameIsExist = await this.UserRoleService.getUserRoleName(roleName);

        if (this.previouseName == roleName) {
            return true;
        }

        if (nameIsExist == true) {
            this.$validator.errors.add({
                field: 'UserRoleName',
                msg: "User Role's Name already exist, please use another name"
            });
            return false;
        }

        return true;
    }

    //Insert To database
    async insertForm() {
        this.$validator.resume();
        this.isLoading = true;
        if (!this.crud.create) {
            this.$validator.pause();
            this.isLoading = false;
            return;
        }
        let valid = await this.$validator.validateAll([
            'UserRoleName',
            'Category',
            'Dealer Position',
            'Tam Position',
            'typeOfPeople'
        ]);

        if (valid == false) {
            this.$validator.pause();
            this.isLoading = false;
            return;
        }

        this.createForm.userRoleName = this.userRoleForm.userRoleName;
        let anotherValid = await this.roleNameIsExistCreate(
            this.createForm.userRoleName
        );
        if (anotherValid == false) {
            this.$validator.pause();
            this.isLoading = false;
            return;
        }

        this.createForm.roleDescription = this.userRoleForm.roleDescription;
        this.createForm.typeOfPeople = this.userRoleForm.typeOfPeople;
        this.createForm.dealerCategory = this.userRoleForm.dealerCategory;
        this.createForm.position = this.userRoleForm.position;

        try {
            await this.UserRoleService.postUserRole(this.createForm);
            this.resetAdd();
            this.$validator.reset();
            this.successMessage = Message.SuccessInsertMessage;
            this.successMessageShow = true;
        } catch {
            this.$validator.errors.add({
                field: 'UserRoleName',
                msg: 'User Role already exist for this Position'
            });
            this.isLoading = false;
            return;
        }
        this.reset();
        this.isLoading = false;
        this.$validator.pause();
        this.closeButtonClick();
    }

    //Update to database
    async editForm() {
        this.$validator.resume();
        this.isLoading = true;
        let valid = await this.$validator.validateAll([
            'User Role',
            'Category',
            'Dealer Position',
            'Tam Position',
            'typeOfPeople'
        ]);
        if (valid == false) {
            this.isLoading = false;
            this.$validator.pause();
            return;
        }
        this.updateForm.userRoleName = this.userRoleForm.userRoleName;
        var userRoleNameValid = await this.roleNameIsExistUpdate(
            this.updateForm.userRoleName
        );
        if (userRoleNameValid == false) {
            this.isLoading = false;
            this.$validator.pause();
            return;
        }

        this.updateForm.roleDescription = this.userRoleForm.roleDescription;
        this.updateForm.typeOfPeople = this.userRoleForm.typeOfPeople;
        this.updateForm.dealerCategory = this.userRoleForm.dealerCategory;
        this.updateForm.position = this.userRoleForm.position;

        try {
            await this.UserRoleService.updateUserRole(this.updateForm);
            this.resetAdd();
            this.$validator.reset();
            this.successMessage = Message.SuccessEditMessage;
            this.successMessageShow = true;
        } catch {
            this.$validator.errors.add({
                field: 'User Role',
                msg: 'User Role already exist for this Position'
            });
            this.isLoading = false;
            return;
        }
        this.reset();
        this.isLoading = false;
        this.$validator.pause();
        this.closeButtonClick();
    }

    userRoleId: number;

    clearAlert() {
        this.errorMessageShow = false;
        this.errorMessage = '';
        this.successMessage = '';
        this.successMessageShow = false;
    }

    showFilter() {
        const show = document.getElementById('filter');
        var isShowed = show.style.display;
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

    async addButtonClick() {
        this.clearAlert();
        if (!this.crud.create) {
            return;
        }
        //this.$validator.resume();
        this.add = true;
        this.edit = false;
        this.viewDetail = false;
        this.$validator.errors.clear();
    }

    //Get role's data by user role into update form when edit button clicked
    async editButtonClick(userRoleId: number) {
        this.clearAlert();
        if (!this.crud.update) {
            return;
        }
        this.add = false;
        this.edit = true;
        this.viewDetail = false;
        this.isLoading = true;
        let dataUpdate = await this.UserRoleService.getUserRoleData(userRoleId);
        this.$validator.errors.clear();

        this.previouseName = dataUpdate.userRoleName;

        this.updateForm.userRoleId = userRoleId;
        this.userRoleForm = {
            userRoleName: dataUpdate.userRoleName,
            roleDescription: dataUpdate.roleDescription,
            typeOfPeople: dataUpdate.typeOfPeople,
            dealerCategory: {
                categoryId: dataUpdate.dealerCategory.categoryId,
                categoryName: dataUpdate.dealerCategory.categoryName
            },
            position: dataUpdate.position
        };
        this.isLoading = false;
    }

    //Get role's data detail by user role into update form when view detail button clicked
    async viewDetailButtonClick(userRoleId: number) {
        this.clearAlert();
        if (!this.crud.read) {
            return;
        }
        this.add = false;
        this.edit = false;
        this.viewDetail = true;
        this.isLoading = true;
        this.$validator.errors.clear();
        let dataView = await this.UserRoleService.viewUserRoleDetail(
            userRoleId
        );

        this.userRoleForm = {
            userRoleName: dataView.userRoleName,
            roleDescription: dataView.roleDescription,
            typeOfPeople: dataView.typeOfPeople,
            dealerCategory: {
                categoryId: dataView.dealerCategory.categoryId,
                categoryName: dataView.dealerCategory.categoryName
            },
            position: dataView.position
        };

        this.isLoading = false;
    }

    closeButtonClick() {
        this.clearAlert();
        this.reset();
        this.resetAdd();
        this.add = false;
        this.edit = false;
        this.viewDetail = false;
        this.successMessageShow = false;
    }

    //If changing the type of People will be resetting position model
    changeTypeOfPeople() {
        this.userRoleForm.position = null;
        this.userRoleForm.dealerCategory = null;
    }

    //reset form search
    async reset() {
        this.filter.DateFilter = {
            start: null,
            end: null
        };
        this.filter.UserRole = '';
        this.filter.TypeOfPeople = null;
        this.filter.Position = '';
        this.filter.SortBy = '';

        this.userRoleName.reset();
        this.typeOfPeople.reset();
        this.position.reset();
        this.createdDate.reset();
        this.updatedDate.reset();

        await this.fetch();
    }

    grid: UserRoleGridModel = {
        userRole: [],
        totalData: 0
    };

    //reset form add, update
    async resetAdd() {
        this.userRoleForm.userRoleName = '';
        this.userRoleForm.typeOfPeople = true;
        this.userRoleForm.roleDescription = '';
        this.userRoleForm.position = null;
        this.userRoleForm.dealerCategory = null;
    }

    //Temporary Form Model before Inserting to Create and Update method
    userRoleForm: IUserRoleFormModel = {
        userRoleName: '',
        roleDescription: '',
        typeOfPeople: true,
        position: null,
        dealerCategory: null
    };

    //default state
    createForm: UserRoleModelCreate = {
        userRoleName: '',
        roleDescription: '',
        typeOfPeople: true,
        position: null,
        dealerCategory: null
    };

    //default state
    updateForm: UserRoleModelUpdate = {
        userRoleId: 0,
        userRoleName: '',
        roleDescription: '',
        typeOfPeople: true,
        position: null,
        dealerCategory: null
    };

    viewForm: UserRoleModelViewDetail = {
        userRoleId: 0,
        userRoleName: '',
        roleDescription: '',
        typeOfPeople: true,
        position: null,
        dealerCategory: null
    };

    //Add , Edit, or View

    add: boolean = false;
    edit: boolean = false;
    viewDetail: boolean = false;

    totalData: number = 0;

    //Variable untuk sorting
    userRoleName = new Sort();
    typeOfPeople = new Sort();
    position = new Sort();
    createdDate = new Sort();
    updatedDate = new Sort();

    //Reset Sort
    async ResetSort(parameter: string) {
        switch (parameter) {
            case 'userRole':
                this.typeOfPeople.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                this.position.reset();
                return;
            case 'position':
                this.typeOfPeople.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                this.userRoleName.reset();
                return;
            case 'typeOfPeople':
                this.position.reset();
                this.userRoleName.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'createdDate':
                this.position.reset();
                this.userRoleName.reset();
                this.typeOfPeople.reset();
                this.updatedDate.reset();
                return;
            case 'updatedDate':
                this.position.reset();
                this.userRoleName.reset();
                this.typeOfPeople.reset();
                this.createdDate.reset();
                return;
        }
    }

    //User Role Sort
    async UserRoleSort() {
        this.ResetSort('userRole');
        //Sorting
        this.userRoleName.sorting();
        //Function Sorting
        if (this.userRoleName.sortup === true) {
            this.filter.SortBy = 'userRole';
        } else if (this.userRoleName.sortdown === true) {
            this.filter.SortBy = 'userRoleDesc';
        } else {
            this.filter.SortBy = '';
        }

        await this.fetch();
        return;
    }

    //User Position Sort
    async PositionSort() {
        this.ResetSort('position');
        //Sorting
        this.position.sorting();
        //Function Sorting
        if (this.position.sortup === true) {
            this.filter.SortBy = 'position';
        } else if (this.position.sortdown === true) {
            this.filter.SortBy = 'positionDesc';
        } else {
            this.filter.SortBy = '';
        }

        await this.fetch();
        return;
    }

    //Type Of People Sort
    async TypeOfPeopleSort() {
        this.ResetSort('typeOfPeople');
        //Sorting
        this.typeOfPeople.sorting();
        //Function Sorting
        if (this.typeOfPeople.sortup === true) {
            this.filter.SortBy = 'typeofPeople';
        } else if (this.typeOfPeople.sortdown === true) {
            this.filter.SortBy = 'typeofPeopleDesc';
        } else {
            this.filter.SortBy = '';
        }

        await this.fetch();
        return;
    }

    //Created Date Sort
    async CreatedAtSort() {
        this.ResetSort('createdDate');
        //Sorting
        this.createdDate.sorting();
        //Function Sorting
        if (this.createdDate.sortup === true) {
            this.filter.SortBy = 'createdDate';
        } else if (this.createdDate.sortdown === true) {
            this.filter.SortBy = 'createdDateDesc';
        } else {
            this.filter.SortBy = '';
        }

        this.fetch();
        return;
    }

    //Updated Date Sort
    async UpdatedAtSort() {
        this.ResetSort('updatedDate');
        //Sorting
        this.updatedDate.sorting();
        //Function Sorting
        if (this.updatedDate.sortup === true) {
            this.filter.SortBy = 'updatedDate';
        } else if (this.updatedDate.sortdown === true) {
            this.filter.SortBy = 'updatedDateDesc';
        } else {
            this.filter.SortBy = '';
        }

        this.fetch();
        return;
    }
}
</script>
