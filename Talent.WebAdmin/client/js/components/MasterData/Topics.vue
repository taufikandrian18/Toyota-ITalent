<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <!--main page topic-->
        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > <span class="talent_font_red">Topic</span></h3>

                <!--alert message-->
                <div v-if="success !== ''" class="my-md-2 mt-md-4">
                    <div class="alert alert-success">
                        {{success}}
                        <button type="button" class="close" aria-label="Close" @click="success = ''">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                </div>

                <div v-if="deleteError !== ''" class="my-md-2 mt-md-4">
                    <div class="alert alert-danger">
                        {{deleteError}}
                        <button type="button" class="close" aria-label="Close" @click="deleteError = ''">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                </div>

                <!--page body-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <h4>Search Topic</h4>

                    <!--2 Column filter-->
                    <div class="row justify-content-between mb-md-4">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker class="v-date-style-report" mode="range" :firstDayOfWeek="2"
                                                       v-model="filter.DateFilter" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>E-Badge</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <select class="form-control" v-model="filter.Ebadge">
                                            <option :value="0"></option>
                                            <option v-for="ebadge in badgeOption" :value="ebadge.ebadgeId">{{ebadge.ebadgeName}}</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--2 Column filter-->
                    <div class="row justify-content-between mb-md-4">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Topic Name</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.TopicName" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Minimum Points</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <vue-autonumeric class="form-control" :emptyInputBehavior="'null'" :options="['integer', {emptyInputBehavior: 'null'}]" v-model.number="filter.MinPoint"></vue-autonumeric>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--Search Button-->
                    <div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="d-flex align-items-end flex-column">
                                    <div>
                                        <button class="btn talent_bg_blue talent_font_white"
                                                @click="filter.pageIndex = 1; fetch()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white"
                                                @click="reset()">
                                            Reset
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Topic List</h4>

                    <!--navbar table-->
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{grid.topics.length}} of {{grid.totalDataFilter}} Entry(s)</a>
                            <button :disabled="!crud.create"
                                    class="btn talent_bg_cyan talent_roundborder talent_font_white" @click="addClicked" v-if="crud.create">
                                Add Topic
                            </button>
                        </div>
                    </div>

                    <!--table topic-->
                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortTopicName()">Topic Name <fa icon="sort" v-if="topicName.sort == true"></fa><fa icon="sort-up" v-if="topicName.sortup == true"></fa><fa icon="sort-down" v-if="topicName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortDescription()">Topic Description <fa icon="sort" v-if="description.sort == true"></fa><fa icon="sort-up" v-if="description.sortup == true"></fa><fa icon="sort-down" v-if="description.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortEBadge()">E-Badge <fa icon="sort" v-if="ebadge.sort == true"></fa><fa icon="sort-up" v-if="ebadge.sortup == true"></fa><fa icon="sort-down" v-if="ebadge.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortMinimumPoints()">Minimum Points <fa icon="sort" v-if="minimumPoints.sort == true"></fa><fa icon="sort-up" v-if="minimumPoints.sortup == true"></fa><fa icon="sort-down" v-if="minimumPoints.sortdown == true"></fa></a>
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
                                <tr v-for="data in grid.topics">
                                    <td>
                                        {{data.topicName}}
                                    </td>
                                    <td>
                                        {{data.topicDesc}}
                                    </td>
                                    <td>
                                        <div v-if="data.eBadge === 1">Bronze</div>
                                        <div v-if="data.eBadge === 2">Silver</div>
                                        <div v-if="data.eBadge === 3">Gold</div>
                                    </td>
                                    <td>
                                        <vue-autonumeric :tag="'p'" :options="[{readOnly:true},'integer']" v-model="data.minPoint"></vue-autonumeric>
                                    </td>
                                    <td>
                                        {{data.createdAt}}
                                    </td>
                                    <td>
                                        {{data.updatedAt}}
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.read">
                                        <button type="button" class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="detailClicked(data.topicId)">
                                            View Detail
                                        </button>
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.update">
                                        <button type="button" class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="editClicked(data.topicId)">
                                            Edit
                                        </button>
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.delete">
                                        <button type="button" class="btn btn-block talent_bg_red talent_font_white"
                                                data-toggle="modal" data-target="#deleteConfirmation"
                                                @click="setDelete(data.topicId, data.topicName)">
                                            Remove
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--pagination-->
                    <div class="row justify-content-center">
                        <paginate :currentPage.sync="filter.pageIndex" :total-data="totalDataFilter" :limit-data="filter.pageSize" @update:currentPage="fetch()"></paginate>
                    </div>

                </div>
            </div>
        </div>

        <!--Add-->
        <div class="row" v-if="add == true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Topic > <span class="talent_font_red">Add Topic</span></h3>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Topic Information</h4>

                    <div v-if="error !== ''">
                        <div class="alert alert-danger">
                            {{error}}
                            <button type="button" class="close" aria-label="Close" @click="error = ''">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <label>Topic Name<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <div class="input-group">
                                                    <input class="form-control" v-model="createForm.topicName" maxlength="255" />
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label>E-Badge<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <select class="form-control" v-model="createForm.eBadge">
                                                            <option v-for="ebadge in badgeOption" :value="ebadge.ebadgeId">{{ebadge.ebadgeName}}</option>
                                                        </select>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <label>Minimum Points<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <div class="input-group">
                                                            <vue-autonumeric class="form-control" :options="'integer'" v-model="createForm.minPoint"></vue-autonumeric>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 ">
                                    <label>Topic Description</label>
                                    <textarea class="form-control talent_textarea" v-model="createForm.topicDesc" maxlength="1024"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <h5>Upload</h5>

                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row justify-content-between">
                                                    <div class="col-md-4 h-100">
                                                        <img class="h-100 w-100" :src="imageData.length ? imageData : '/upload-image2.png'" for="customFile" />
                                                    </div>
                                                    <div class="col-md-8">
                                                        <h5>File Upload</h5>
                                                        <label>Upload Image<span class="talent_font_red">*</span></label>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="row justify-content-between">
                                                                    <div class="col-md-8">
                                                                        <div class="custom-file">
                                                                            <input type="file"
                                                                                   class="custom-file-input"
                                                                                   id="customFile"
                                                                                   @change="handler"
                                                                                   name="File Upload"
                                                                                   accept=".png,.jpg,.jpeg"
                                                                                   v-validate="'required'" />
                                                                            <label class="custom-file-label talent_textshorten " for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4 font_size_12">
                                                                        *Image Size 300x300<br />
                                                                        *Max File Size 5MB
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
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="insert()">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Edit-->
        <div class="row">
            <div v-if="edit == true" class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Topic > <span class="talent_font_red">Edit Topic</span></h3>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Topic Information</h4>

                    <div v-if="error !== ''">
                        <div class="alert alert-danger">
                            {{error}}
                            <button type="button" class="close" aria-label="Close" @click="error = ''">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">

                                        <div class="col-md-12">
                                            <label>Topic Name<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <div class="input-group">
                                                    <input class="form-control" v-model="updateForm.topicName" maxlength="255" />
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label>E-Badge<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <select class="form-control" v-model="updateForm.eBadge">
                                                            <option v-for="ebadge in badgeOption" :value="ebadge.ebadgeId">{{ebadge.ebadgeName}}</option>
                                                        </select>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <label>Minimum Points<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <div class="input-group">
                                                            <vue-autonumeric class="form-control" :options="'integer'" v-model="updateForm.minPoint"></vue-autonumeric>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 ">
                                    <label>Topic Description</label>
                                    <textarea class="form-control talent_textarea" v-model="updateForm.topicDesc" maxlength="1024"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <h5>Upload</h5>

                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row justify-content-between">
                                                    <div class="col-md-4 h-100">
                                                        <img class="h-100 w-100" :src="imageData.length ? imageData : '/upload-image2.png'" for="customFile" />
                                                    </div>
                                                    <div class="col-md-8">
                                                        <h5>File Upload</h5>
                                                        <label>Upload Image<span class="talent_font_red">*</span></label>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="row justify-content-between">
                                                                    <div class="col-md-8">
                                                                        <div class="custom-file">
                                                                            <input type="file"
                                                                                   class="custom-file-input"
                                                                                   id="customFile"
                                                                                   @change="handler"
                                                                                   name="File Upload"
                                                                                   accept=".png,.jpg,.jpeg"
                                                                                   v-validate="'required'" />
                                                                            <label class="custom-file-label talent_textshorten " for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4 font_size_12">
                                                                        *Image Size 300x300<br />
                                                                        *Max File Size 5MB
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
                            </div>
                        </div>
                    </div>
                </div>

                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="update()">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Detail-->
        <div class="row">
            <div v-if="detail == true" class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Topic > <span class="talent_font_red">Detail Topic</span></h3>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Topic Information</h4>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <div class="row">

                                        <div class="col-md-12">
                                            <label>Topic Name<span class="talent_font_red">*</span></label>
                                            <div class="input-group">
                                                <div class="input-group">
                                                    <input class="form-control" v-model="viewOnlyForm.topicName" disabled />
                                                </div>
                                            </div>
                                        </div>
                                        <br />
                                        <div class="col-md-12">
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label>E-Badge<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <select class="form-control" v-model="viewOnlyForm.eBadge" disabled>
                                                            <option v-for="ebadge in badgeOption" :value="ebadge.ebadgeId">{{ebadge.ebadgeName}}</option>
                                                        </select>
                                                    </div>
                                                </div>

                                                <div class="col-md-6">
                                                    <label>Minimum Points<span class="talent_font_red">*</span></label>
                                                    <div class="input-group">
                                                        <div class="input-group">
                                                            <vue-autonumeric class="form-control" :options="'integer'" v-model="viewOnlyForm.minPoint" disabled></vue-autonumeric>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-6 ">
                                    <label>Topic Description</label>
                                    <textarea class="form-control talent_textarea" v-model="viewOnlyForm.topicDesc" disabled></textarea>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <h5>Upload</h5>

                                    <div class="col-md-12">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row justify-content-between">
                                                    <div class="col-md-4 h-100">
                                                        <img class="h-100 w-100" :src="imageData.length ? imageData : '/upload-image2.png'" for="customFile" />
                                                    </div>
                                                    <div class="col-md-8">
                                                        <h5>File Upload</h5>
                                                        <label>Upload Image<span class="talent_font_red">*</span></label>
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="row justify-content-between">
                                                                    <div class="col-md-8">
                                                                        <div class="custom-file">
                                                                            <input type="file"
                                                                                   class="custom-file-input"
                                                                                   id="customFile"
                                                                                   @change="handler"
                                                                                   name="File Upload"
                                                                                   accept=".png,.jpg,.jpeg"
                                                                                   v-validate="'required'"
                                                                                   disabled />
                                                                            <label class="custom-file-label talent_textshorten " for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4 font_size_12">
                                                                        *Image Size 300x300<br />
                                                                        *Max File Size 5MB
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--<upload-file layout="Horizontal"
                                    :mountImage="viewURL"
                                    :mountName="viewOnlyForm.blobName"
                                    :disable="true"></upload-file>-->
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Delete Confirmation only-->
        <div class="modal fade" id="deleteConfirmation" tabindex="-1" role="dialog" aria-labelledby="deleteConfirmLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="col-md-12 justify-content-center d-flex">
                            <h5>Are you sure want to delete?</h5>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="col-md-12 d-flex justify-content-around">
                                <button type="button" class="btn talent_bg_red talent_font_white talent_roundborder" data-dismiss="modal" @click.prevent="deleteTopic(toBeDeletedId)">Delete</button>
                                <button type="button" class="btn talent_bg_blue talent_font_white talent_roundborder" data-dismiss="modal">Close</button>
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
    import { BlobService } from '../../services/BlobService';
    import { TopicService, TopicCreateModel, GridTopicModel, TopicUpdateModel, TopicEbadgeOptionModel, UserPrivilegeSettingsService, UserAccessCRUD, FileContent } from '../../services/NSwagService'
    import { PageEnums } from '../../enum/PageEnums';
    import VueAutonumeric from 'vue-autonumeric';

    @Component({
        created: async function (this: Topics) {
            await this.getAccess();
            await this.fetch();
            await this.setOption();
        },
        data() {
            return {
                money: {
                    decimal: ',',
                    thousands: '.',
                    prefix: '',
                    suffix: '',
                    precision: 0,
                    masked: false
                }
            }
        },

    })

    export default class Topics extends Vue {
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Topic);
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        /*
         * topic service on nswag
         * */
        Service: TopicService = new TopicService();

        props: any = {
            placeholder: "",
            readonly: true
        };

        deleteError: string = '';

        isLoading: boolean = false;
        isImageChange: boolean = false;
        imageData: any = [];
        imageName: string = '';

        /*
         * contain all filter and sorting
         * */
        filter: ITopicFilter = {
            sortBy: '',
            pageIndex: 1,
            pageSize: 10,

            DateFilter: {
                start: null,
                end: null
            },
            Ebadge: 0,
            MinPoint: null,
            TopicName: ''
        }

        /*
         * form to create new topics
         * */
        createForm: TopicCreateModel = {
            topicName: '',
            topicDesc: '',
            eBadge: 0,
            minPoint: 0,
            fileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        /*
         * form to store udpate model before and after the update
         * */
        updateForm: TopicUpdateModel = {
            topicId: 0,
            topicName: '',
            topicDesc: '',
            eBadge: 0,
            minPoint: 0,
            blobId: '',
            blobName: '',
            blobFileType: '',
            fileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        /*
         * form to store data for view detail only
         * */
        viewOnlyForm: TopicUpdateModel = {
            topicId: 0,
            topicName: '',
            topicDesc: '',
            eBadge: 0,
            minPoint: 0,
            blobId: '',
            blobName: '',
            blobFileType: ''
        }

        badgeOption: TopicEbadgeOptionModel[] = [];

        /*
         * store all topic data after filter, total data, and total data after filter
         * */
        grid: GridTopicModel = {
            topics: [],
            totalData: 0,
            totalDataFilter: 0
        }

        /*
         * used to store deleted id and name
         * used in the modal
         * */
        toBeDeletedId: number = 0;
        toBeDeletedName: string = '';

        /*
         * store total data and total data filter
         * used in showing entry
         * */
        totalData: number = 0;
        totalDataFilter: number = 0;

        /*
         * use for pagination
         * calculated page size is count after the filter
         * */
        calculatedPageSize: number = 0;

        /*
         * show error, success message
         * */
        error: string = '';
        success: string = '';

        createFileName: string = null;
        updateURL: string = '';
        viewURL: string = '';

        //Navigation
        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;

        async setOption() {
            this.isLoading = true;
            this.badgeOption = await this.Service.getEbadgeOption();
            this.isLoading = false;
        }

        /*
         * fetch all data before and after filter
         * */
        async fetch() {
            this.isLoading = true;
            try {
                let response = await this.Service.getAllTopic(
                    this.filter.DateFilter.start,
                    this.filter.DateFilter.end,
                    this.filter.TopicName,
                    this.filter.Ebadge,
                    this.filter.MinPoint,
                    this.filter.sortBy,
                    this.filter.pageIndex,
                    this.filter.pageSize);

                this.grid = response;

                this.totalData = this.grid.totalData;
                this.totalDataFilter = this.grid.totalDataFilter;

                this.calculatedPageSize = Math.ceil(this.totalDataFilter / this.filter.pageSize);
            }
            catch (e) {

            }

            this.isLoading = false;
        }

        /*
         * to reset filter search
         * */
        async reset() {
            this.filter.DateFilter = {
                start: null,
                end: null
            };

            this.filter.TopicName = '';
            this.filter.Ebadge = 0;
            this.filter.MinPoint = null;

            this.ResetSort('');
            await this.fetch();
        }

        /*
         * inserting data after all requirement is fulfilled
         * */
        async insert() {
            if (!this.crud.create) {
                return
            }
            this.isLoading = true;

            if (!this.createForm.topicName) {
                this.error = 'Topic name should be filled';
                this.isLoading = false;
                return;
            }

            if (!this.createForm.eBadge) {
                this.error = 'Ebadge should be filled';
                this.isLoading = false;
                return;
            }

            if (!this.createForm.minPoint) {
                this.error = 'Minimum points should be filled';
                this.isLoading = false;
                return;
            }

            if (isNaN(Number(this.createForm.minPoint))) {
                this.error = 'Minimum points should be filled with number';
                this.isLoading = false;
                return;
            }

            if (!this.createFileName) {
                this.error = 'Image should be filled';
                this.isLoading = false;
                return;
            }

            let extension = this.createFileName.substring(this.createFileName.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpg' && extension != '.png' && extension != '.jpeg') {
                this.error = 'File must be image in JPG/PNG/JPEG';
                this.isLoading = false;
                return;
            }

            if (await this.Service.validateTopicName(this.createForm.topicName)) {
                this.error = 'Topic name has been used, please use another name';
                this.isLoading = false;
                return;
            }

            await this.Service.createNewTopic(this.createForm);

            this.createForm.topicName = '';
            this.createForm.topicDesc = '';
            this.createForm.minPoint = 0;
            this.createForm.eBadge = 0;
            this.createForm.fileContent = {
                base64: '',
                contentType: '',
                fileName: ''
            }

            this.error = '';
            this.success = 'Success to Add Data!';
            this.reset();
            await this.fetch();
            this.closeClicked();
        }

        /*
         * update data after all requirement is fulfilled
         * */
        async update() {
            if (!this.crud.update) {
                return
            }
            this.isLoading = true;
            if (!this.updateForm.topicName) {
                this.error = 'Topic name should be filled';
                this.isLoading = false;
                return;
            }

            if (!this.updateForm.eBadge) {
                this.error = 'Ebadge should be filled';
                this.isLoading = false;
                return;
            }

            if (!this.updateForm.minPoint) {
                this.error = 'Minimum points should be filled';
                this.isLoading = false;
                return;
            }

            if (isNaN(Number(this.updateForm.minPoint))) {
                this.error = 'Minimum points should be filled with number';
                this.isLoading = false;
                return;
            }

            if (!this.updateForm.blobName) {
                this.error = 'Image should be filled';
                this.isLoading = false;
                return;
            }

            let extension = this.updateForm.blobName.substring(this.updateForm.blobName.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpg' && extension != '.png' && extension != '.jpeg') {
                this.error = 'File must be image in JPG/PNG/JPEG';
                this.isLoading = false;
                return;
            }

            if (await this.Service.validateUpdateTopicName(this.updateForm.topicName, this.updateForm.topicId)) {
                this.error = 'Topic name has been used, please use another name';
                this.isLoading = false;
                return;
            }

            await this.Service.updateTopic(this.updateForm, this.updateForm.topicId);

            this.updateForm = {
                blobId: '',
                blobName: '',
                blobFileType: '',
                topicId: 0,
                topicName: '',
                topicDesc: '',
                eBadge: 0,
                minPoint: 0,
                fileContent: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                }
            }

            this.error = '';
            this.success = 'Success to Edit Data!';
            this.reset();
            await this.fetch();
            this.closeClicked();
        }

        /*
         * set delete id and name
         * used for modal pop up
         * */
        setDelete(index: number, name: string) {
            this.toBeDeletedId = index;
            this.toBeDeletedName = name;
        }

        /*
         * delete selected Id
         * */
        async deleteTopic(index: number) {
            if (!this.crud.delete) {
                return
            }
            let success = await this.Service.deleteTopic(index);

            if (!success) {
                this.deleteError = 'Cannot delete this topic because there are module(s) or coach(s) with this topic.'
                return;
            }

            this.toBeDeletedId = 0;
            this.toBeDeletedName = '';

            this.success = 'Success to Remove Data!';
            await this.fetch();
        }

        /*
         * change page based on page number
         * */
        async changePage(page: number) {
            this.filter.pageIndex = page;

            await this.fetch();
        }

        /*
         * go to next page from page index
         * */
        async nextPage() {
            this.filter.pageIndex++;

            await this.fetch();
        }

        /*
         * go to prev page from page index
         * */
        async prevPage() {
            this.filter.pageIndex--;

            await this.fetch();
        }

        //Variable untuk sorting
        topicName = new Sort();
        description = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();
        ebadge = new Sort();
        minimumPoints = new Sort();

        //Navigation Function
        addClicked() {
            if (!this.crud.create) {
                return
            }
            this.add = true;
            this.edit = false;
            this.detail = false;
        }

        /*
         * to open view detail topic
         * */
        async editClicked(index: number) {
            if (!this.crud.update) {
                return
            }
            let topic = await this.Service.getTopicById(index);

            this.updateForm.topicId = topic.topicId;
            this.updateForm.topicName = topic.topicName;
            this.updateForm.topicDesc = topic.topicDesc;
            this.updateForm.eBadge = topic.eBadgeId;
            this.updateForm.minPoint = topic.minPoint;
            this.updateForm.blobId = topic.blobId;
            this.updateForm.blobName = topic.blobName;
            this.updateForm.blobFileType = topic.blobFileType;

            this.imageData = await BlobService.getImageUrl(topic.blobId, topic.blobFileType);
            this.imageName = topic.blobName;

            this.edit = true;
            this.add = false;
            this.detail = false;
        }

        async detailClicked(index: number) {
            if (!this.crud.read) {
                return
            }
            let topic = await this.Service.getTopicById(index);

            this.viewOnlyForm.topicId = topic.topicId;
            this.viewOnlyForm.topicName = topic.topicName;
            this.viewOnlyForm.topicDesc = topic.topicDesc;
            this.viewOnlyForm.eBadge = topic.eBadgeId;
            this.viewOnlyForm.minPoint = topic.minPoint;
            this.viewOnlyForm.blobId = topic.blobId;
            this.viewOnlyForm.blobName = topic.blobName;
            this.viewOnlyForm.blobFileType = topic.blobFileType;

            this.imageData = await BlobService.getImageUrl(topic.blobId, topic.blobFileType);
            this.imageName = topic.blobName;

            this.edit = false;
            this.add = false;
            this.detail = true;
        }

        closeClicked() {
            this.error = '';
            this.createFileName = '';
            this.imageData = [];
            this.imageName = '';

            this.edit = false;
            this.add = false;
            this.detail = false;
        }

        changeImage() {
            console.log('testing');
        }

        //ClickSortMinimumPoints
        async ClickSortMinimumPoints() {
            this.ResetSort('minimumPoints');
            //Sorting
            this.minimumPoints.sorting();

            if (this.minimumPoints.sortup === true) {
                this.filter.sortBy = 'MinPoint';
            }

            else if (this.minimumPoints.sortdown === true) {
                this.filter.sortBy = 'MinPointDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortuserRole
        async ClickSortTopicName() {
            this.ResetSort('topicName');
            //Sorting
            this.topicName.sorting();

            if (this.topicName.sortup === true) {
                this.filter.sortBy = 'TopicName';
            }

            else if (this.topicName.sortdown === true) {
                this.filter.sortBy = 'TopicNameDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting

            return;
        }

        async ClickSortEBadge() {
            this.ResetSort('ebadge');
            //Sorting
            this.ebadge.sorting();

            if (this.ebadge.sortup === true) {
                this.filter.sortBy = 'Ebadge';
            }

            else if (this.ebadge.sortdown === true) {
                this.filter.sortBy = 'EbadgeDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //ClickSortdescription
        async ClickSortDescription() {
            this.ResetSort('description');
            //Sorting
            this.description.sorting();

            if (this.description.sortup === true) {
                this.filter.sortBy = 'TopicDescription';
            }

            else if (this.description.sortdown === true) {
                this.filter.sortBy = 'TopicDescriptionDesc';
            }

            else {
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
            }

            else if (this.createdDate.sortdown === true) {
                this.filter.sortBy = 'CreatedAtDesc';
            }

            else {
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
            }

            else if (this.updatedDate.sortdown === true) {
                this.filter.sortBy = 'UpdatedAtDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();
            //Function Sorting
            return;
        }

        //Reset Sorting
        ResetSort(parameter: string) {
            switch (parameter) {
                case 'topicName':
                    this.description.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    this.ebadge.reset();
                    this.minimumPoints.reset();
                    return;
                case 'section':
                    this.topicName.reset();
                    this.description.reset();
                    this.createdDate.reset();
                    this.minimumPoints.reset();
                    this.updatedDate.reset();
                    this.ebadge.reset();
                    return;
                case 'description':
                    this.topicName.reset();
                    this.createdDate.reset();
                    this.minimumPoints.reset();
                    this.updatedDate.reset();
                    this.ebadge.reset();
                    return;
                case 'createdDate':
                    this.topicName.reset();
                    this.minimumPoints.reset();
                    this.description.reset();
                    this.updatedDate.reset();
                    this.ebadge.reset();
                    return;
                case 'updatedDate':
                    this.topicName.reset();
                    this.description.reset();
                    this.minimumPoints.reset();
                    this.createdDate.reset();
                    this.ebadge.reset();
                    return;
                case 'ebadge':
                    this.topicName.reset();
                    this.description.reset();
                    this.minimumPoints.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'minimumPoints':
                    this.filter.sortBy = '';
                    this.topicName.reset();
                    this.description.reset();
                    this.ebadge.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                default:
                    this.filter.sortBy = '';
                    this.topicName.reset();
                    this.description.reset();
                    this.ebadge.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    this.minimumPoints.reset();
                    return;
            }
        }

        async handler($event) {
            if ($event.target.files.length === 0) {
                return;
            }

            this.isImageChange = true;

            let baseFileInput = (<HTMLInputElement>$event.target).files[0];
            let base64String = await this.convertToBase64(baseFileInput);
            let extension = baseFileInput.name.split(".").pop();

            this.createForm.fileContent = {
                base64: base64String,
                contentType: extension,
                fileName: baseFileInput.name
            }

            this.updateForm.fileContent = {
                base64: base64String,
                contentType: extension,
                fileName: baseFileInput.name
            }

            var reader = new FileReader();
            reader.onload = (e: Event) => {
                this.imageData = (<FileReader>e.target).result;
            }
            reader.readAsDataURL($event.target.files[0]);
            this.imageName = $event.target.files[0].name;
            this.createFileName = this.imageName;
        }

        convertToBase64(file: File): Promise<string> {
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
    }
</script>
