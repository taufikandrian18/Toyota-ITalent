<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <!--MAIN PAGE-->
        <div
            class="row"
            v-if="add === false && edit === false && detail === false"
        >
            <div class="col col-md-12">
                <h3 class="mb-md-4"><fa icon="star"></fa> Reward</h3>

                <div class="alert alert-success" v-if="success !== ''">
                    {{ success }}
                    <button
                        type="button"
                        class="close"
                        aria-label="Close"
                        @click="success = ''"
                    >
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <!-- Advance Search -->
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
                                                mode="range"
                                                :firstDayOfWeek="2"
                                                :input-props="props"
                                                v-model="filter.DateFilter"
                                                :masks="{ input: 'DD/MM/YYYY' }"
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
                                        <div class="mb-1">Reward Name</div>
                                        <div class="input-group mb-2">
                                            <input
                                                class="form-control"
                                                v-model="filter.RewardName"
                                            />
                                        </div>
                                        <!-- Created By -->
                                        <div class="mb-1">Type of Reward</div>
                                        <div class="mb-2">
                                            <select
                                                class="form-control"
                                                v-model="filter.RewardType"
                                            >
                                                <option
                                                    v-for="rewardType in RewardTypeOption"
                                                    :key="rewardType.id"
                                                    :value="
                                                        rewardType.rewardTypeId
                                                    "
                                                    >{{
                                                        rewardType.rewardTypeName
                                                    }}</option
                                                >
                                            </select>
                                        </div>
                                    </div>
                                    <!-- Right -->
                                    <div class="col-md-6">
                                        <!-- Type of Guide -->
                                        <div class="mb-1">Type of Points</div>
                                        <div class="mb-2">
                                            <select
                                                class="form-control"
                                                v-model="filter.PointType"
                                            >
                                                <option
                                                    v-for="point in RewardPointTypeOption"
                                                    :key="point.id"
                                                    :value="
                                                        point.rewardPointTypeId
                                                    "
                                                    >{{
                                                        point.rewardPointTypeName
                                                    }}</option
                                                >
                                            </select>
                                        </div>
                                        <!-- Approval Status -->
                                        <div class="mb-1">Status</div>
                                        <div>
                                            <select
                                                class="form-control"
                                                v-model="filter.IsActive"
                                            >
                                                <option :value="true"
                                                    >Active</option
                                                >
                                                <option :value="false"
                                                    >Inactive</option
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
                                                    @click="resetFilter()"
                                                >
                                                    Reset
                                                </button>
                                                <button
                                                    class="btn talent_bg_darkblue talent_font_white px-4"
                                                    @click="fetch()"
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
                    <!-- Add Reward -->
                    <div class="col d-flex justify-content-end">
                        <button
                            v-if="crud.create"
                            class="btn talent_bg_darkblue talent_font_white"
                            @click="addClicked()"
                        >
                            + Add Reward
                        </button>
                    </div>
                </div>

                <!--Table-->
                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4"
                >
                    <!-- Header -->
                    <div class="row">
                        <div class="col">
                            <h4>Reward List</h4>
                        </div>
                        <div class="col-auto">
                            <button class="btn talent_bg_red talent_font_white">
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
                                >Showing {{ rewards.grid.length }} of
                                {{ rewards.totalFilterData }} Entry(s)</a
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
                                            @click.prevent="
                                                ClickSortRewardName()
                                            "
                                        >
                                            Reward Name
                                            <fa
                                                icon="sort"
                                                v-if="rewardName.sort == true"
                                            ></fa>
                                            <fa
                                                icon="sort-up"
                                                v-if="rewardName.sortup == true"
                                            ></fa>
                                            <fa
                                                icon="sort-down"
                                                v-if="
                                                    rewardName.sortdown == true
                                                "
                                            ></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortTypeOfReward()
                                            "
                                        >
                                            Type of Reward
                                            <fa
                                                icon="sort"
                                                v-if="typeOfReward.sort == true"
                                            ></fa>
                                            <fa
                                                icon="sort-up"
                                                v-if="
                                                    typeOfReward.sortup == true
                                                "
                                            ></fa>
                                            <fa
                                                icon="sort-down"
                                                v-if="
                                                    typeOfReward.sortdown ==
                                                        true
                                                "
                                            ></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortTypeOfPoints()
                                            "
                                        >
                                            Type of Points
                                            <fa
                                                icon="sort"
                                                v-if="points.sort == true"
                                            ></fa>
                                            <fa
                                                icon="sort-up"
                                                v-if="points.sortup == true"
                                            ></fa>
                                            <fa
                                                icon="sort-down"
                                                v-if="points.sortdown == true"
                                            ></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="ClickSortStatus()"
                                        >
                                            Status
                                            <fa
                                                icon="sort"
                                                v-if="status.sort == true"
                                            ></fa>
                                            <fa
                                                icon="sort-up"
                                                v-if="status.sortup == true"
                                            ></fa>
                                            <fa
                                                icon="sort-down"
                                                v-if="status.sortdown == true"
                                            ></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortCreatedDate()
                                            "
                                        >
                                            Created Date
                                            <fa
                                                icon="sort"
                                                v-if="createdDate.sort == true"
                                            ></fa>
                                            <fa
                                                icon="sort-up"
                                                v-if="
                                                    createdDate.sortup == true
                                                "
                                            ></fa>
                                            <fa
                                                icon="sort-down"
                                                v-if="
                                                    createdDate.sortdown == true
                                                "
                                            ></fa>
                                        </a>
                                    </th>
                                    <th>
                                        <a
                                            href="#"
                                            class="talent_sort"
                                            @click.prevent="
                                                ClickSortUpdatedDate()
                                            "
                                        >
                                            Updated Date
                                            <fa
                                                icon="sort"
                                                v-if="updatedDate.sort == true"
                                            ></fa>
                                            <fa
                                                icon="sort-up"
                                                v-if="
                                                    updatedDate.sortup == true
                                                "
                                            ></fa>
                                            <fa
                                                icon="sort-down"
                                                v-if="
                                                    updatedDate.sortdown == true
                                                "
                                            ></fa>
                                        </a>
                                    </th>
                                    <th
                                        colspan="3"
                                        v-if="
                                            crud.read ||
                                                crud.update ||
                                                crud.delete
                                        "
                                        class="text-center"
                                    >
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr
                                    v-for="reward in rewards.grid"
                                    :key="reward.id"
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
                                        {{ reward.rewardName }}
                                    </td>
                                    <td>
                                        {{ reward.typeOfReward }}
                                    </td>
                                    <td>
                                        {{ reward.typeOfPoint }}
                                    </td>
                                    <td>
                                        {{ reward.status }}
                                    </td>
                                    <td>
                                        {{ reward.createdAt | dateFormat }}
                                    </td>
                                    <td>
                                        {{ reward.updateAt | dateFormat }}
                                    </td>
                                    <td
                                        class="talent_nopadding_important"
                                        v-if="crud.read"
                                    >
                                        <button
                                            class="btn btn-block talent_bg_orange talent_font_white"
                                            @click.prevent="
                                                detailClicked(reward.rewardId)
                                            "
                                        >
                                            View Detail
                                        </button>
                                    </td>
                                    <td
                                        class="talent_nopadding_important"
                                        v-if="crud.update"
                                    >
                                        <button
                                            class="btn btn-block talent_bg_cyan talent_font_white"
                                            @click.prevent="
                                                editClicked(reward.rewardId)
                                            "
                                        >
                                            Edit
                                        </button>
                                    </td>
                                    <td
                                        class="talent_nopadding_important"
                                        v-if="crud.delete"
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
                                                    reward.rewardId,
                                                    reward.rewardName
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
                                :total-data="rewards.totalFilterData"
                                :limit-data="filter.pageSize"
                                @update:currentPage="fetch()"
                            ></paginate>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--FORM-->
        <div class="row" v-else>
            <div class="col col-md-12">
                <h3 class="mb-md-4">
                    <fa icon="database"></fa>Master Data > Reward >
                    <span v-if="add === true" class="talent_font_red"
                        >Add Reward</span
                    >
                    <span v-else-if="edit === true" class="talent_font_red"
                        >Edit Reward</span
                    >
                    <span v-else-if="detail === true" class="talent_font_red"
                        >View Detail Reward</span
                    >
                </h3>

                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4"
                >
                    <h4>Reward Information</h4>

                    <div
                        class="alert alert-danger pb-0"
                        v-show="errors.items.length > 0"
                    >
                        <button
                            type="button"
                            class="close"
                            aria-label="Close"
                            @click="$validator.errors.clear()"
                        >
                            <span aria-hidden="true">&times;</span>
                        </button>
                        <ul>
                            <li v-for="error in errors.all()" :key="error.id">
                                {{ error }}
                            </li>
                        </ul>
                    </div>

                    <div class="row mb-md-4">
                        <div class="col-md-12">
                            <div class="row justify-content-between mb-md-4">
                                <div class="col-md-6">
                                    <label>
                                        Reward Name
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <div class="input-group mb-md-4">
                                        <input
                                            class="form-control"
                                            name="Reward Name"
                                            v-model="rewardForm.Name"
                                            v-validate="'required'"
                                            :disabled="detail === true"
                                            maxlength="255"
                                        />
                                    </div>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <label>
                                                        Type of Reward
                                                        <span
                                                            class="talent_font_red"
                                                            >*</span
                                                        >
                                                    </label>
                                                </div>
                                                <div class="col-md-6">
                                                    <div
                                                        v-if="
                                                            rewardForm
                                                                .RewardType
                                                                .rewardTypeId ===
                                                                1
                                                        "
                                                    >
                                                        Module<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        >
                                                    </div>
                                                    <div
                                                        v-else-if="
                                                            rewardForm
                                                                .RewardType
                                                                .rewardTypeId ===
                                                                2
                                                        "
                                                    >
                                                        Coach<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        >
                                                    </div>
                                                    <div
                                                        v-else-if="
                                                            rewardForm
                                                                .RewardType
                                                                .rewardTypeId ===
                                                                3
                                                        "
                                                    >
                                                        Event<span
                                                            class="talent_font_red"
                                                            >*</span
                                                        >
                                                    </div>
                                                </div>
                                            </div>
                                            <div
                                                class="row justify-content-between"
                                            >
                                                <div class="col-md-6">
                                                    <div class="input-group">
                                                        <multiselect
                                                            v-model="
                                                                rewardForm.RewardType
                                                            "
                                                            name="Reward Type"
                                                            :options="
                                                                RewardTypeOption
                                                            "
                                                            :allow-empty="false"
                                                            label="rewardTypeName"
                                                            deselect-label="Selected"
                                                            @input="
                                                                resetOptionForm
                                                            "
                                                            v-validate="
                                                                'required'
                                                            "
                                                            :disabled="
                                                                detail === true
                                                            "
                                                        >
                                                        </multiselect>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    <div class="input-group">
                                                        <multiselect
                                                            v-if="
                                                                rewardForm
                                                                    .RewardType
                                                                    .rewardTypeId ===
                                                                    1
                                                            "
                                                            v-model="
                                                                rewardForm.Module
                                                            "
                                                            name="Module"
                                                            :options="
                                                                ModuleOption
                                                            "
                                                            :allow-empty="false"
                                                            label="moduleName"
                                                            deselect-label="Selected"
                                                            v-validate="
                                                                'required'
                                                            "
                                                            :disabled="
                                                                detail === true
                                                            "
                                                        >
                                                        </multiselect>

                                                        <multiselect
                                                            v-if="
                                                                rewardForm
                                                                    .RewardType
                                                                    .rewardTypeId ===
                                                                    2
                                                            "
                                                            v-model="
                                                                rewardForm.Coach
                                                            "
                                                            name="Coach"
                                                            :options="
                                                                CoachOption
                                                            "
                                                            :allow-empty="false"
                                                            label="coachName"
                                                            deselect-label="Selected"
                                                            v-validate="
                                                                'required'
                                                            "
                                                            :disabled="
                                                                detail === true
                                                            "
                                                        >
                                                        </multiselect>

                                                        <multiselect
                                                            v-if="
                                                                rewardForm
                                                                    .RewardType
                                                                    .rewardTypeId ===
                                                                    3
                                                            "
                                                            v-model="
                                                                rewardForm.Event
                                                            "
                                                            name="Event"
                                                            :options="
                                                                EventOption
                                                            "
                                                            :allow-empty="false"
                                                            label="eventName"
                                                            deselect-label="Selected"
                                                            v-validate="
                                                                'required'
                                                            "
                                                            :disabled="
                                                                detail === true
                                                            "
                                                        >
                                                        </multiselect>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        Reward Description
                                    </label>
                                    <textarea
                                        class="form-control description-height talent_overflowy"
                                        name="Description"
                                        v-model="rewardForm.Description"
                                        :disabled="detail === true"
                                        maxlength="1024"
                                    ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 mb-md-4">
                            <div class="row justify-content-between mb-md-4">
                                <div class="col-md-6">
                                    <label>
                                        Period
                                    </label>

                                    <div class="input-group">
                                        <v-date-picker
                                            class="v-date-style-report"
                                            mode="range"
                                            :firstDayOfWeek="2"
                                            :min-date="checkMinDate()"
                                            :max-date="checkMaxDate()"
                                            v-model="rewardForm.DateForm"
                                            name="Period"
                                            :masks="{ input: 'DD/MM/YYYY' }"
                                        ></v-date-picker>

                                        <div class="input-group-append">
                                            <span class="input-group-text"
                                                ><fa icon="calendar-alt"></fa
                                            ></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        Terms & Conditions
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <textarea
                                        class="form-control description-height talent_overflowy"
                                        name="Terms & Conditions"
                                        v-model="rewardForm.TermsAndConditions"
                                        v-validate="'required'"
                                        :disabled="detail === true"
                                    ></textarea>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="row justify-content-between mb-md-4">
                                <div class="col-md-6">
                                    <div
                                        class="row justify-content-between mb-md-4"
                                    >
                                        <div class="col-md-4">
                                            <label>
                                                Type of Points
                                                <span class="talent_font_red"
                                                    >*</span
                                                >
                                            </label>
                                        </div>
                                        <div class="col-md-8">
                                            <label>
                                                Required Points
                                                <span class="talent_font_red"
                                                    >*</span
                                                >
                                            </label>
                                        </div>
                                    </div>

                                    <div
                                        class="row justify-content-between mb-md-4"
                                    >
                                        <div class="col-md-4">
                                            <div class="form-check">
                                                <input
                                                    class="form-check-input"
                                                    type="checkbox"
                                                    name="Type Of Points"
                                                    v-model="
                                                        rewardForm.isTeaching
                                                    "
                                                    @change="
                                                        addValidate(
                                                            'Teaching Required Point',
                                                            rewardForm.isTeaching
                                                        )
                                                    "
                                                    id="teachingPoints"
                                                    :disabled="detail === true"
                                                />
                                                <label
                                                    class="form-check-label"
                                                    for="teachingPoints"
                                                    >Teaching Points</label
                                                >
                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="input-group">
                                                <input
                                                    type="number"
                                                    min="0"
                                                    name="Teaching Required Point"
                                                    class="form-control"
                                                    v-model.number="
                                                        rewardForm.teachingRequiredPoint
                                                    "
                                                    v-validate="
                                                        'required|min_value:1'
                                                    "
                                                    :disabled="
                                                        rewardForm.isTeaching ===
                                                            false ||
                                                            detail === true
                                                    "
                                                />
                                            </div>
                                        </div>
                                    </div>

                                    <div
                                        class="row justify-content-between mb-md-4"
                                    >
                                        <div class="col-md-4">
                                            <div class="form-check">
                                                <input
                                                    class="form-check-input"
                                                    type="checkbox"
                                                    name="Type Of Points"
                                                    v-model="
                                                        rewardForm.isLearning
                                                    "
                                                    @change="
                                                        addValidate(
                                                            'Learning Required Point',
                                                            rewardForm.isLearning
                                                        )
                                                    "
                                                    id="learningPoints"
                                                    :disabled="detail === true"
                                                />
                                                <label
                                                    class="form-check-label"
                                                    for="learningPoints"
                                                    >Learning Points</label
                                                >
                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="input-group">
                                                <input
                                                    type="number"
                                                    name="Learning Required Point"
                                                    min="0"
                                                    class="form-control"
                                                    v-model.number="
                                                        rewardForm.learningRequiredPoint
                                                    "
                                                    v-validate="
                                                        'required|min_value:1'
                                                    "
                                                    :disabled="
                                                        rewardForm.isLearning ===
                                                            false ||
                                                            detail === true
                                                    "
                                                />
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row justify-content-between">
                                        <div class="col-md-4">
                                            <div class="form-check">
                                                <input
                                                    class="form-check-input"
                                                    type="checkbox"
                                                    name="Type Of Points"
                                                    v-model="rewardForm.isTotal"
                                                    @change="
                                                        addValidate(
                                                            'Total Required Point',
                                                            rewardForm.isTotal
                                                        )
                                                    "
                                                    id="totalPoints"
                                                    :disabled="detail === true"
                                                />
                                                <label
                                                    class="form-check-label"
                                                    for="totalPoints"
                                                    >Total Points</label
                                                >
                                            </div>
                                        </div>
                                        <div class="col-md-8">
                                            <div class="input-group">
                                                <input
                                                    type="number"
                                                    min="0"
                                                    name="Total Required Point"
                                                    class="form-control"
                                                    v-model.number="
                                                        rewardForm.totalRequiredPoint
                                                    "
                                                    v-validate="
                                                        'required|min_value:1'
                                                    "
                                                    :disabled="
                                                        rewardForm.isTotal ===
                                                            false ||
                                                            detail === true
                                                    "
                                                />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        How to Use
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <textarea
                                        class="form-control description-height talent_overflowy"
                                        name="How to Use"
                                        v-model="rewardForm.HowToUse"
                                        v-validate="'required'"
                                        :disabled="detail === true"
                                    ></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>
                                        Status
                                        <span class="talent_font_red">*</span>
                                    </label>
                                    <div>
                                        <div
                                            class="form-check form-check-inline"
                                        >
                                            <input
                                                class="form-check-input"
                                                type="radio"
                                                name="inlineRadioOptions"
                                                id="active"
                                                v-model="rewardForm.isActive"
                                                :value="true"
                                                :disabled="detail === true"
                                            />
                                            <label
                                                class="form-check-label"
                                                for="active"
                                                >Active</label
                                            >
                                        </div>
                                        <div
                                            class="form-check form-check-inline"
                                        >
                                            <input
                                                class="form-check-input"
                                                type="radio"
                                                name="inlineRadioOptions"
                                                id="inactive"
                                                v-model="rewardForm.isActive"
                                                :value="false"
                                                :disabled="detail === true"
                                            />
                                            <label
                                                class="form-check-label"
                                                for="inactive"
                                                >Inactive</label
                                            >
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                >
                    <div class="row">
                        <div class="col-md-12 d-flex justify-content-between">
                            <h5>File Upload</h5>
                            <div>
                                <button 
                                    class="btn btn-sm btn-primary mr-2" 
                                    data-toggle="modal"
                                    data-target="#addFile">
                                    + File Upload
                                </button>
                                <button class="btn btn-sm talent_bg_red talent_font_white">Delete</button>
                            </div>
                        </div>
                        <div class="col-md-12 mt-3">
                            <table class="table table-bordered table-responsive-md talent_table_padding">
                                <thead>
                                    <tr>
                                        <th scope="col" class="text-center">
                                            <input type="checkbox" id="selectAll">
                                        </th>
                                        <th scope="col">
                                            <a href="#" class="talent_sort" style="color: white;">
                                                Type 
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" style="color: white;">
                                                Title 
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" style="color: white;">
                                                File 
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" style="color: white;">
                                                Created At 
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#" class="talent_sort" style="color: white;">
                                                Updated At 
                                            </a>
                                        </th>
                                        <th colspan="2" class="text-center">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="g in datas" :key="g.employeeID">
                                        <th class="text-center">
                                            <input type="checkbox" @change="(e) => handleCheckedItem(g, e)" :checked="g.isSelected">
                                        </th>
                                        <td>
                                            {{ g.email }}
                                        </td>
                                        <td>
                                            {{ g.name }}
                                        </td>
                                        <td>
                                            {{ g.dealerName }}
                                        </td>
                                        <td>
                                            {{ g.outletName }}
                                        </td>
                                        <td>
                                            {{convertDateTime(g.registeredDate)}}
                                        </td>
                                        <td>
                                            {{ g.status }}
                                        </td>
                                        <td class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_orange talent_font_white" @click="handleDetail(g)">View Detail</button>
                                        </td>
                                        <td class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(g)">Edit</button>
                                        </td>
                                        <!-- <td v-if="crud.delete" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click.prevent="deleteClicked(g.guideId, index)" :disabled="g.approvalStatus == 'Waiting for Approval'">Remove</button>
                                        </td> -->
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <div
                    class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mt-4"
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
                                        @click.prevent="insert"
                                        v-if="add === true"
                                    >
                                        Save
                                    </button>
                                    <button
                                        class="btn talent_bg_lightgreen talent_font_white"
                                        @click.prevent="update"
                                        v-if="edit === true"
                                    >
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Add File-->
        <div
            class="modal fade"
            id="addFile"
            tabindex="-1"
            role="dialog"
            aria-labelledby="addFileLabel"
            aria-hidden="true"
        >
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="col-md-12 justify-content-center d-flex flex-wrap">
                            <div class="row">
                                <div class="w-100">
                                    <label for="title">Title</label>
                                    <input type="text" class="form-input w-100" id="title" name="title">
                                </div>
                                <div class="w-100 bg-input-file mt-3 bg_talent_bg_grey d-flex flex-wrap align-items-center justify-content-center py-3">
                                    <div class="w-100 d-flex justify-content-center">
                                        <label for="inputFile">
                                            <button class="btn btn-primary btn-sm">Browse File</button>
                                        </label>
                                        <input type="file" style="display: none" name="inputFile" id="inputFile">
                                    </div>
                                    <div class="recommendation">
                                        *Recommended File PDF, JPEG, JPG, PNG
                                        <br>
                                        *Max File 5MB
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div
                                class="col-md-12 d-flex justify-content-end"
                            >
                                <button
                                    type="button"
                                    class="btn talent_bg_blue talent_font_white talent_roundborder"
                                    data-dismiss="modal"
                                >
                                    Close
                                </button>
                                <button
                                    type="button"
                                    class="btn talent_bg_lightgreen talent_font_white talent_roundborder ml-2"
                                    data-dismiss="modal"
                                >
                                    Save
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--DELETE CONFIRMATION-->
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
                                    @click="deleteReward(toBeDeletedReward.id)"
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
    DropdownService,
    RewardTypeDropdownModel,
    ModuleDropdownModel,
    CoachDropdownModel,
    EventDropdownModel,
    RewardCreateModel,
    RewardService,
    RewardGridModel,
    RewardPointTypeDropdown,
    RewardUpdateModel,
    UserPrivilegeSettingsService,
    UserAccessCRUD
} from '../../services/NSwagService';
import {
    IRewardFormModel,
    IRewardFilterModel
} from '../../models/IRewardModels';
import Message from '../../class/Message';
import { PageEnums } from '../../enum/PageEnums';

@Component({
    created: async function(this: Reward) {
        await this.getAccess();
        await this.fillRewardOption();
        await this.fetch();
    }
})
export default class Reward extends Vue {
    privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
    async getAccess() {
        var data = await this.privilegeApi.crudAccessPage(PageEnums.Hobby);
        this.crud = data;
    }

    crud: UserAccessCRUD = {
        create: false,
        delete: false,
        read: false,
        update: false
    };

    dropMan: DropdownService = new DropdownService();
    rewMan: RewardService = new RewardService();

    props: any = {
        placeholder: '',
        readonly: true
    };

    isLoading: boolean = false;

    filter: IRewardFilterModel = {
        pageIndex: 1,
        pageSize: 10,
        sortBy: '',
        DateFilter: {
            start: null,
            end: null
        },
        RewardType: null,
        IsActive: null,
        RewardName: '',
        PointType: null
    };

    rewards: RewardGridModel = {
        grid: [],
        totalFilterData: 0
    };

    RewardPointTypeOption: RewardPointTypeDropdown[] = [];
    RewardTypeOption: RewardTypeDropdownModel[] = [];
    ModuleOption: ModuleDropdownModel[] = [];
    CoachOption: CoachDropdownModel[] = [];
    EventOption: EventDropdownModel[] = [];

    validateScope: string[] = [
        'Reward Name',
        'Terms & Conditions',
        'How to Use'
    ];

    rewardForm: IRewardFormModel = {
        Name: '',
        RewardType: {},
        Module: {},
        Coach: {},
        Event: {},
        DateForm: {
            start: null,
            end: null
        },
        isTeaching: false,
        isTotal: false,
        isLearning: false,
        teachingRequiredPoint: 0,
        totalRequiredPoint: 0,
        learningRequiredPoint: 0,
        Description: '',
        TermsAndConditions: '',
        HowToUse: '',
        isActive: false
    };

    rewardInsertForm: RewardCreateModel = {
        rewardName: '',
        rewardTypeId: null,
        moduleId: null,
        coachId: null,
        eventId: null,
        startDate: null,
        endDate: null,
        rewardRequiredTeachingPoints: null,
        rewardRequiredTotalPoints: null,
        rewardRequiredLearningPoints: null,
        description: '',
        howToUse: '',
        termsAndConditions: '',
        isActive: null
    };

    rewardUpdateForm: RewardUpdateModel = {
        rewardId: 0,
        rewardName: '',
        rewardTypeId: null,
        moduleId: null,
        coachId: null,
        eventId: null,
        startDate: null,
        endDate: null,
        rewardRequiredTeachingPoints: null,
        rewardRequiredTotalPoints: null,
        rewardRequiredLearningPoints: null,
        description: '',
        howToUse: '',
        termsAndConditions: '',
        isActive: null
    };

    success: string = '';

    toBeDeletedReward: { id: number; name: string } = {
        id: 0,
        name: ''
    };

    async fetch() {
        this.isLoading = true;
        this.rewards = await this.rewMan.getAllReward(
            this.filter.DateFilter.start,
            this.filter.DateFilter.end,
            this.filter.RewardType,
            this.filter.IsActive,
            this.filter.RewardName,
            this.filter.PointType,
            this.filter.sortBy,
            this.filter.pageIndex,
            this.filter.pageSize
        );
        this.isLoading = false;
        this.closeFilter();
    }

    async fillRewardOption() {
        this.isLoading = true;
        this.RewardPointTypeOption = await this.dropMan.getAllRewardPointTypeDropdown();
        this.RewardTypeOption = await this.dropMan.getAllRewardTypeDropdown();
        this.ModuleOption = await this.dropMan.getAllModuleDropdown();
        this.CoachOption = await this.dropMan.getAllCoachDropdown();
        this.EventOption = await this.dropMan.getAllEventDropdown();
        this.isLoading = false;
    }

    resetOptionForm() {
        this.rewardForm.Module = {};
        this.rewardForm.Coach = {};
        this.rewardForm.Event = {};
    }

    resetFilter() {
        this.filter = {
            pageIndex: 1,
            pageSize: 10,
            sortBy: '',
            DateFilter: {
                start: null,
                end: null
            },
            RewardType: null,
            IsActive: null,
            RewardName: '',
            PointType: null
        };

        this.ResetSort('');

        this.fetch();
    }

    resetForm() {
        this.$validator.pause();

        this.rewardForm = {
            Name: '',
            RewardType: {},
            Module: {},
            Coach: {},
            Event: {},
            DateForm: {
                start: null,
                end: null
            },
            isTeaching: false,
            isTotal: false,
            isLearning: false,
            teachingRequiredPoint: 0,
            totalRequiredPoint: 0,
            learningRequiredPoint: 0,
            Description: '',
            TermsAndConditions: '',
            HowToUse: '',
            isActive: false
        };
    }

    checkModule(): boolean {
        if (Object.keys(this.rewardForm.Module).length === 0) {
            this.$validator.errors.add({
                field: 'Module',
                msg: 'The Module field is Required'
            });
            return false;
        }
        return true;
    }

    checkCoach(): boolean {
        if (Object.keys(this.rewardForm.Coach).length === 0) {
            this.$validator.errors.add({
                field: 'Coach',
                msg: 'The Coach field is Required'
            });
            return false;
        }
        return true;
    }

    checkEvent(): boolean {
        if (Object.keys(this.rewardForm.Event).length === 0) {
            this.$validator.errors.add({
                field: 'Event',
                msg: 'The Event field is Required'
            });
            return false;
        }
        return true;
    }

    checkRewardType(): boolean {
        if (Object.keys(this.rewardForm.RewardType).length === 0) {
            this.$validator.errors.add({
                field: 'Reward Type',
                msg: 'The Reward Type field is Required'
            });
            return false;
        }
        return true;
    }

    checkPointType(): boolean {
        if (
            !this.rewardForm.isLearning &&
            !this.rewardForm.isTeaching &&
            !this.rewardForm.isTotal
        ) {
            this.$validator.errors.add({
                field: 'Type of Points',
                msg: 'At least 1 Type of Points should be selected'
            });
            return false;
        }

        return true;
    }

    addValidate(validate: string, isCheck: boolean) {
        if (isCheck) {
            this.validateScope.push(validate);
            return;
        }

        if (validate === 'Teaching Required Point') {
            this.rewardForm.teachingRequiredPoint = 0;
        }
        if (validate === 'Learning Required Point') {
            this.rewardForm.learningRequiredPoint = 0;
        }

        if (validate === 'Total Required Point') {
            this.rewardForm.totalRequiredPoint = 0;
        }

        let index = this.validateScope.findIndex(Q => Q === validate);
        this.validateScope.splice(index, 1);
    }

    checkMinDate() {
        if (this.add || this.edit) {
            return null;
        }

        return new Date(Math.max.apply(null, null));
    }

    checkMaxDate() {
        if (this.add || this.edit) {
            return null;
        }

        return new Date(Math.min.apply(null, null));
    }

    async insert() {
        this.isLoading = true;
        this.$validator.resume();
        this.$validator.errors.clear();

        let rewardTypeValid = this.checkRewardType();
        let nextValidOption: boolean = true;
        switch (this.rewardForm.RewardType.rewardTypeId) {
            case 1:
                nextValidOption = this.checkModule();
                break;
            case 2:
                nextValidOption = this.checkCoach();
                break;
            case 3:
                nextValidOption = this.checkEvent();
                break;
            default:
                break;
        }

        let pointTypeValid = this.checkPointType();

        let valid = await this.$validator.validateAll(this.validateScope);
        if (!valid || !rewardTypeValid || !nextValidOption || !pointTypeValid) {
            this.isLoading = false;
            return;
        }

        this.$validator.reset();

        this.rewardInsertForm = {
            rewardName: this.rewardForm.Name,
            rewardTypeId: this.rewardForm.RewardType.rewardTypeId,
            moduleId:
                typeof this.rewardForm.Module.moduleId === 'undefined'
                    ? null
                    : this.rewardForm.Module.moduleId,
            coachId:
                typeof this.rewardForm.Coach.coachId === 'undefined'
                    ? null
                    : this.rewardForm.Coach.coachId,
            eventId:
                typeof this.rewardForm.Event.eventId === 'undefined'
                    ? null
                    : this.rewardForm.Event.eventId,
            startDate: this.rewardForm.DateForm.start,
            endDate: this.rewardForm.DateForm.end,
            rewardRequiredTeachingPoints: this.rewardForm.teachingRequiredPoint,
            rewardRequiredTotalPoints: this.rewardForm.totalRequiredPoint,
            rewardRequiredLearningPoints: this.rewardForm.learningRequiredPoint,
            description: this.rewardForm.Description,
            howToUse: this.rewardForm.HowToUse,
            termsAndConditions: this.rewardForm.TermsAndConditions,
            isActive: this.rewardForm.isActive
        };

        let success = await this.rewMan.insertReward(this.rewardInsertForm);

        if (!success) {
            this.$validator.errors.add({
                field: 'Reward Name',
                msg: 'Reward Name has existed, please use another name'
            });
            this.isLoading = false;
            return;
        }

        this.success = Message.SuccessInsertMessage;
        this.closeClicked();
        this.resetFilter();
        this.fetch();
    }

    async update() {
        this.isLoading = true;
        this.$validator.resume();
        this.$validator.errors.clear();

        let rewardTypeValid = this.checkRewardType();
        let nextValidOption: boolean = true;
        switch (this.rewardForm.RewardType.rewardTypeId) {
            case 1:
                nextValidOption = this.checkModule();
                break;
            case 2:
                nextValidOption = this.checkCoach();
                break;
            case 3:
                nextValidOption = this.checkEvent();
                break;
            default:
                break;
        }

        let pointTypeValid = this.checkPointType();

        let valid = await this.$validator.validateAll(this.validateScope);
        if (!valid || !rewardTypeValid || !nextValidOption || !pointTypeValid) {
            this.isLoading = false;
            return;
        }

        this.$validator.reset();

        this.rewardUpdateForm.rewardName = this.rewardForm.Name;
        this.rewardUpdateForm.rewardTypeId = this.rewardForm.RewardType.rewardTypeId;
        this.rewardUpdateForm.moduleId =
            this.rewardForm.Module.moduleId === 0
                ? null
                : this.rewardForm.Module.moduleId;
        this.rewardUpdateForm.coachId =
            this.rewardForm.Coach.coachId === 0
                ? null
                : this.rewardForm.Coach.coachId;
        this.rewardUpdateForm.eventId =
            this.rewardForm.Event.eventId === 0
                ? null
                : this.rewardForm.Event.eventId;
        this.rewardUpdateForm.startDate = this.rewardForm.DateForm.start;
        this.rewardUpdateForm.endDate = this.rewardForm.DateForm.end;
        this.rewardUpdateForm.rewardRequiredTeachingPoints = this.rewardForm.teachingRequiredPoint;
        this.rewardUpdateForm.rewardRequiredTotalPoints = this.rewardForm.totalRequiredPoint;
        this.rewardUpdateForm.rewardRequiredLearningPoints = this.rewardForm.learningRequiredPoint;
        this.rewardUpdateForm.description = this.rewardForm.Description;
        this.rewardUpdateForm.howToUse = this.rewardForm.HowToUse;
        this.rewardUpdateForm.termsAndConditions = this.rewardForm.TermsAndConditions;
        this.rewardUpdateForm.isActive = this.rewardForm.isActive;

        let success = await this.rewMan.updateReward(this.rewardUpdateForm);

        if (!success) {
            this.$validator.errors.add({
                field: 'Reward Name',
                msg: 'Reward Name has existed, please use another name'
            });
            this.isLoading = false;
            return;
        }

        this.success = Message.SuccessEditMessage;
        this.closeClicked();
        this.resetFilter();
        this.fetch();
    }

    //Navigation
    add: boolean = false;
    detail: boolean = false;
    edit: boolean = false;

    currentPage: number = 1;

    looping: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

    teachingPointsBool: boolean = false;
    totalPointsBool: boolean = false;
    learningPointsBool: boolean = false;

    //Variable untuk sorting
    rewardName = new Sort();
    typeOfReward = new Sort();
    status = new Sort();
    createdBy = new Sort();
    createdDate = new Sort();
    updatedDate = new Sort();
    points = new Sort();

    //Navigation function
    addClicked() {
        this.add = true;
        this.edit = false;
        this.detail = false;
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

    async editClicked(index: number) {
        this.isLoading = true;
        let reward = await this.rewMan.getRewardById(index);

        this.rewardForm = {
            Name: reward.rewardName,
            RewardType: reward.rewardType,
            Module: reward.module,
            Coach: reward.coach,
            Event: reward.event,
            Description: reward.description,
            DateForm: {
                start:
                    reward.startDate == null
                        ? null
                        : new Date(reward.startDate),
                end: reward.endDate == null ? null : new Date(reward.endDate)
            },
            TermsAndConditions: reward.termsAndConditions,
            isLearning: reward.rewardRequiredLearningPoints > 0 ? true : false,
            learningRequiredPoint: reward.rewardRequiredLearningPoints,
            isTeaching: reward.rewardRequiredTeachingPoints > 0 ? true : false,
            teachingRequiredPoint: reward.rewardRequiredTeachingPoints,
            isTotal: reward.rewardRequiredTotalPoints > 0 ? true : false,
            totalRequiredPoint: reward.rewardRequiredTotalPoints,
            HowToUse: reward.howToUse,
            isActive: reward.isActive
        };

        this.rewardUpdateForm.rewardId = reward.rewardId;

        this.add = false;
        this.edit = true;
        this.detail = false;

        this.isLoading = false;
    }

    async detailClicked(index: number) {
        this.isLoading = true;
        let reward = await this.rewMan.getRewardById(index);
        this.rewardForm = {
            Name: reward.rewardName,
            RewardType: reward.rewardType,
            Module: reward.module,
            Coach: reward.coach,
            Event: reward.event,
            Description: reward.description,
            DateForm: {
                start:
                    reward.startDate == null
                        ? null
                        : new Date(reward.startDate),
                end: reward.endDate == null ? null : new Date(reward.endDate)
            },
            TermsAndConditions: reward.termsAndConditions,
            isLearning: reward.rewardRequiredLearningPoints > 0 ? true : false,
            learningRequiredPoint: reward.rewardRequiredLearningPoints,
            isTeaching: reward.rewardRequiredTeachingPoints > 0 ? true : false,
            teachingRequiredPoint: reward.rewardRequiredTeachingPoints,
            isTotal: reward.rewardRequiredTotalPoints > 0 ? true : false,
            totalRequiredPoint: reward.rewardRequiredTotalPoints,
            HowToUse: reward.howToUse,
            isActive: reward.isActive
        };

        this.add = false;
        this.edit = false;
        this.detail = true;

        this.isLoading = false;
    }

    closeClicked() {
        this.resetForm();

        this.add = false;
        this.edit = false;
        this.detail = false;
    }

    setDelete(rewardId: number, rewardName: string) {
        this.toBeDeletedReward = {
            id: rewardId,
            name: rewardName
        };
    }

    emptyDelete() {
        this.toBeDeletedReward = {
            id: 0,
            name: ''
        };
    }

    async deleteReward(rewardId: number) {
        this.isLoading = true;
        this.success = Message.RemoveMessage;
        await this.rewMan.deleteReward(rewardId);
        this.emptyDelete();
        await this.fetch();
    }

    //ClickSortRewardName
    async ClickSortRewardName() {
        this.ResetSort('rewardName');
        //Sorting
        this.rewardName.sorting();

        if (this.rewardName.sortup === true) {
            this.filter.sortBy = 'name';
        } else if (this.rewardName.sortdown === true) {
            this.filter.sortBy = 'nameDesc';
        } else {
            this.filter.sortBy = '';
        }

        await this.fetch();
        //Function Sorting
        return;
    }

    //ClickSortTypeOfReward
    async ClickSortTypeOfReward() {
        this.ResetSort('typeOfReward');
        //Sorting
        this.typeOfReward.sorting();

        if (this.typeOfReward.sortup === true) {
            this.filter.sortBy = 'typeOfReward';
        } else if (this.typeOfReward.sortdown === true) {
            this.filter.sortBy = 'typeOfRewardDesc';
        } else {
            this.filter.sortBy = '';
        }

        await this.fetch();
        //Function Sorting
        return;
    }

    //ClickSortStatus
    async ClickSortStatus() {
        this.ResetSort('status');
        //Sorting
        this.status.sorting();

        if (this.status.sortup === true) {
            this.filter.sortBy = 'status';
        } else if (this.status.sortdown === true) {
            this.filter.sortBy = 'statusDesc';
        } else {
            this.filter.sortBy = '';
        }

        await this.fetch();
        //Function Sorting
        return;
    }

    //ClickSortStatus
    async ClickSortTypeOfPoints() {
        this.ResetSort('points');
        //Sorting
        this.points.sorting();

        if (this.points.sortup === true) {
            this.filter.sortBy = 'points';
        } else if (this.points.sortdown === true) {
            this.filter.sortBy = 'pointsDesc';
        } else {
            this.filter.sortBy = '';
        }

        await this.fetch();
        //Function Sorting
        return;
    }

    //ClickSortCreatedDate
    async ClickSortCreatedDate() {
        this.ResetSort('createdDate');
        //Sorting
        this.createdDate.sorting();

        if (this.createdDate.sortup === true) {
            this.filter.sortBy = 'CreatedAt';
        } else if (this.createdDate.sortdown === true) {
            this.filter.sortBy = 'CreatedAtDesc';
        } else {
            this.filter.sortBy = '';
        }

        await this.fetch();
        //Function Sorting
        return;
    }

    //ClickSortUpdatedDate
    async ClickSortUpdatedDate() {
        this.ResetSort('updatedDate');
        //Sorting
        this.updatedDate.sorting();

        if (this.updatedDate.sortup === true) {
            this.filter.sortBy = 'UpdatedAt';
        } else if (this.updatedDate.sortdown === true) {
            this.filter.sortBy = 'UpdatedAtDesc';
        } else {
            this.filter.sortBy = '';
        }

        await this.fetch();
        //Function Sorting
        return;
    }

    //Reset Sorting
    async ResetSort(parameter: string) {
        switch (parameter) {
            case 'rewardName':
                this.typeOfReward.reset();
                this.status.reset();
                this.createdBy.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                this.points.reset();
                return;
            case 'typeOfReward':
                this.rewardName.reset();
                this.status.reset();
                this.createdBy.reset();
                this.createdDate.reset();
                this.points.reset();
                this.updatedDate.reset();
                return;
            case 'status':
                this.rewardName.reset();
                this.typeOfReward.reset();
                this.points.reset();
                this.createdBy.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'createdDate':
                this.rewardName.reset();
                this.typeOfReward.reset();
                this.status.reset();
                this.points.reset();
                this.createdBy.reset();
                this.updatedDate.reset();
                return;
            case 'updatedDate':
                this.rewardName.reset();
                this.typeOfReward.reset();
                this.status.reset();
                this.createdBy.reset();
                this.points.reset();
                this.createdDate.reset();
                return;
            case 'points':
                this.rewardName.reset();
                this.typeOfReward.reset();
                this.status.reset();
                this.createdBy.reset();
                this.updatedDate.reset();
                this.createdDate.reset();
                return;
            default:
                this.filter.sortBy = '';
                this.points.reset();
                this.rewardName.reset();
                this.typeOfReward.reset();
                this.status.reset();
                this.createdBy.reset();
                this.updatedDate.reset();
                this.createdDate.reset();
                return;
        }
    }
}
</script>

<style scoped>
.recommendation {
    font-size: 0.75rem;
    color: #6c6c6c;
}
.bg-input-file {
    background-color: #d1d1d1;
}
</style>
