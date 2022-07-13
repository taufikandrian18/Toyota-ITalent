<template>
    <div>
        <div v-if="successMessage" class="alert alert-success alert-dismissible fade show" role="alert">
            {{successMessage}}
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>

        <loading-overlay :loading="isBusy"></loading-overlay>

        <div v-if="this.errors.items.length != 0 || errorMessage.length > 0" class="alert alert-danger">

            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>

            <div>{{errorMessage}}</div>
            <div>{{errors.first('title')}}</div>
            <div>{{errors.first('description')}}</div>
            <div>{{errors.first('typeOfBanner')}}</div>
            <div>{{errors.first('File')}}</div>
            <div>{{errors.first('content')}}</div>
            <div>{{errors.first('startDate')}}</div>
            <div>{{errors.first('endDate')}}</div>
        </div>

        <div class="row" v-if="add == false && edit == false & viewdetail == false">
            <div class="col col-md-12">
                <h3>
                    <fa icon="database"></fa> Master Data >
                    <span class="talent_font_red">Banner</span>
                </h3>
                <br />

                <div>
                    <!--ADVANCE SEARCH-->
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <h4>Search Banner</h4>

                        <br />

                        <!--3 Column Menu-->
                        <div class="row">
                            <div class="col-md-3">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>Date</span>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="input-group talent_front">
                                            <v-date-picker class="v-date-style-report"
                                                           mode="range"
                                                           v-model="bannerFilter.DateFilter"
                                                           :firstDayOfWeek="2"
                                                           :value="null"
                                                           :input-props="props"
                                                           :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                            <div class="input-group-append">
                                                <span class="input-group-text">
                                                    <fa icon="calendar-alt"></fa>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>Start Date</span>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="input-group talent_front">
                                            <v-date-picker class="v-date-style-report"
                                                           v-model="bannerFilter.StartDate"
                                                           :firstDayOfWeek="2"
                                                           :value="null"
                                                           :input-props="props"
                                                           :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                            <div class="input-group-append">
                                                <span class="input-group-text">
                                                    <fa icon="calendar-alt"></fa>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>End Date</span>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="input-group talent_front">
                                            <v-date-picker class="v-date-style-report"
                                                           v-model="bannerFilter.EndDate"
                                                           :firstDayOfWeek="2"
                                                           :value="null"
                                                           :input-props="props"
                                                           :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                            <div class="input-group-append">
                                                <span class="input-group-text">
                                                    <fa icon="calendar-alt"></fa>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>Action By</span>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <input class="form-control" v-model="bannerFilter.ApprovedBy" />
                                        </div>
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
                                        <span>Banner Title</span>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <input class="form-control" v-model="bannerFilter.BannerTitle" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-3">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>Type of Banner</span>
                                    </div>
                                    <div class="col-md-8">
                                        <select class="form-control" v-model="bannerFilter.TypeofBanner">
                                            <option :value="1">Regular</option>
                                            <option :value="2">Specific</option>
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
                                        <select class="form-control" v-model="bannerFilter.Status">
                                            <option v-for="approval in approvalStatusBanner" :value="approval.approvalName">{{approval.approvalName}}</option>
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
                                            <button class="btn talent_bg_blue talent_font_white" @click.prevent="search()">Search</button>
                                            <button class="btn talent_bg_red talent_font_white" @click.prevent="resetSearch()">Reset</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <!--Table-->
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <h4>Banner List</h4>
                        <div class="col-md-12 talent_magin_small">
                            <div class="row align-items-center row justify-content-between">
                                <a>Showing {{bannerList.listBanner.length}} of {{bannerList.totalData}} Entry(s)</a>
                                <button class="btn talent_bg_cyan talent_roundborder talent_font_white" v-if="crud.create" @click="addClicked">
                                    Add Banner
                                </button>
                            </div>
                        </div>

                        <div class="talent_overflowx">
                            <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                                <thead>
                                    <tr>
                                        <th scope="col">
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortBanner()">
                                                Banner Title
                                                <fa icon="sort" v-if="bannerName.sort == true"></fa>
                                                <fa icon="sort-up" v-if="bannerName.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="bannerName.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortTypeBanner()">
                                                Type of Banner
                                                <fa icon="sort" v-if="bannerType.sort == true"></fa>
                                                <fa icon="sort-up" v-if="bannerType.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="bannerType.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortStartDate()">
                                                Start Date
                                                <fa icon="sort" v-if="startDate.sort == true"></fa>
                                                <fa icon="sort-up" v-if="startDate.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="startDate.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortEndDate()">
                                                End Date
                                                <fa icon="sort" v-if="endDate.sort == true"></fa>
                                                <fa icon="sort-up" v-if="endDate.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="endDate.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortStatus()">
                                                Approval Status
                                                <fa icon="sort" v-if="status.sort == true"></fa>
                                                <fa icon="sort-up" v-if="status.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="status.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortApprovedBy()">
                                                Action By
                                                <fa icon="sort" v-if="approvedBy.sort == true"></fa>
                                                <fa icon="sort-up" v-if="approvedBy.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="approvedBy.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortCreatedDate()">
                                                Created Date
                                                <fa icon="sort" v-if="createdDate.sort == true"></fa>
                                                <fa icon="sort-up" v-if="createdDate.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="createdDate.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortUpdatedDate()">
                                                Updated Date
                                                <fa icon="sort" v-if="updatedDate.sort == true"></fa>
                                                <fa icon="sort-up" v-if="updatedDate.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th v-if="crud.read || crud.update || crud.delete" colspan="3">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="(banner,index) in bannerList.listBanner">
                                        <td>{{banner.name}}</td>
                                        <td>{{banner.bannerTypeName}}</td>
                                        <td>{{getStringDate(banner.startDate)}}</td>
                                        <td>{{getStringDate(banner.endDate)}}</td>
                                        <td>{{banner.approvalStatus}}</td>
                                        <td>{{banner.createdBy}}</td>
                                        <td>{{banner.createdAt | dateFormat}}</td>
                                        <td>{{banner.updatedAt | dateFormat}}</td>
                                        <td v-if="crud.read" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_orange talent_font_white" @click="viewClicked(index)">
                                                View Detail
                                            </button>
                                        </td>
                                        <td v-if="crud.update" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_cyan talent_font_white" @click="editClicked(index)" :disabled="banner.approvalStatusId === waitingStatus">
                                                Edit
                                            </button>
                                        </td>
                                        <td v-if="crud.delete" class="talent_nopadding_important">
                                            <button :disabled="banner.approvalStatusId === waitingStatus" class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click.prevent="deleteClicked(index)">
                                                Remove
                                            </button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <!--Pagination-->
                        <div class="col-md-12 d-flex justify-content-center">
                            <paginate :currentPage.sync="currentPage" :total-data=bannerList.totalData :limit-data=pageSize @update:currentPage="fetch()"></paginate>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Add-->
        <div class="row" v-if="add === true">

            <div class="col-md-12">
                <h3>
                    <fa icon="database"></fa> Master Data > Banner >
                    <span class="talent_font_red">Add Banner</span>
                </h3>

                <br />
                <!--Add-->

                <div class="row">
                    <div class="col-md-12">
                        <div class="talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                            <h4>Banner Information</h4>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>
                                                Banner Title
                                                <span class="talent_font_red">*</span>
                                            </label>
                                            <div class="input-group">
                                                <input class="form-control" v-validate="'required'" name="title" v-model="formModel.name" maxlength="1024" />
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label>
                                                        Period
                                                    </label>
                                                </div>
                                                <div class="col-md-6"></div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <div class="input-group">
                                                        <v-date-picker class="v-date-style-report" mode="single" :masks="{ input: 'DD/MM/YYYY' }" name="startDate" :firstDayOfWeek="2" :value="null" v-validate="{'required' : formModel.endDate != null}" :max-date="formModel.endDate" :input-props='{placeholder: "Start Date"}' v-model="formModel.startDate"></v-date-picker>
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
                                                        <v-date-picker class="v-date-style-report" mode="single" :masks="{ input: 'DD/MM/YYYY' }" name="endDate" :firstDayOfWeek="2" :value="null" v-validate="{'required' : formModel.startDate != null}" :input-props='{placeholder: "End Date"}' v-model="formModel.endDate" disabled :min-date="formModel.startDate"></v-date-picker>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>
                                                Banner Description
                                                <span class="talent_font_red">*</span>
                                            </label>
                                            <textarea class="form-control"
                                                      style="height:150px;overflow-y:scroll;" v-validate="'required'" name="description" v-model="formModel.description" maxlength="1024"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>
                                                Type of Banner
                                                <span class="talent_font_red">*</span>
                                            </label>
                                            <br />
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input"
                                                       type="radio"
                                                       name="typeOfBanner"
                                                       id="regular"
                                                       :value="1"
                                                       v-validate="'required'"
                                                       v-model="formModel.bannerTypeId" />
                                                <label class="form-check-label" for="regular">Regular</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input"
                                                       type="radio"
                                                       name="typeOfBanner"
                                                       id="specific"
                                                       :value="2"
                                                       v-model="formModel.bannerTypeId" />
                                                <label class="form-check-label" for="specific">Specific</label>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>
                                                Content Category
                                                <span class="talent_font_red"></span>
                                            </label>
                                            <div class="input-group">
                                                <select class="form-control" v-model="selectedContentType" @change="resetSelectedContent">
                                                    <option :value="emptyContentType">-------</option>
                                                    <option v-for="content in contentTypeList" :value="content">{{content.name}}</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row jusify-content-between">
                                <div class="col-md-6">
                                    <h4>File Upload</h4>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-12">
                                                    <label>
                                                        Upload Banner Picture
                                                        <span class="talent_font_red">*</span>
                                                    </label>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-6">
                                                                    <div class="custom-file">

                                                                        <input type="file" name="File" class="custom-file-input" id="customFile" v-validate="'image|size:5120|required'" accept="image/*" @change="previewImage($event)">
                                                                        <label class="custom-file-label talent_textshorten " for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    *Image Size 1270x548
                                                                    <br />*Max File Size 5MB
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        Content Name
                                        <span class="talent_font_red"></span>
                                    </label>
                                    <div class="input-group">
                                        <multiselect v-model="selectedContent"
                                                     id="ajax"
                                                     track-by="name"
                                                     placeholder="Select Content Name"
                                                     label="name"
                                                     name="content"
                                                     :options="contentList"
                                                     :searchable="true"
                                                     :close-on-select="true"
                                                     :clear-on-select="true"
                                                     :loading="isLoading"
                                                     :hide-selected="true"
                                                     :showNoResults="true"
                                                     @search-change="AutoComplete"
                                                     @open="reset"
                                                     v-validate="{required:selectedContentType.mobilePageId != null}">

                                            <span slot="noResult"><i>Not Found!</i></span>

                                        </multiselect>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <img class="img-fluid w-100" :src="imageData.length ? imageData : 'upload-image2.png'" for="customFile" />
                            </div>
                        </div>
                        <br />
                        <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="d-flex align-items-end flex-column">
                                        <div>
                                            <button class="btn talent_bg_red talent_font_white" @click="closeClicked">Close</button>
                                            <button class="btn talent_bg_lightgreen talent_font_white" @click="insertBanner(1)">Save</button>
                                            <button class="btn talent_bg_blue talent_font_white" @click="insertBanner(2)">Submit</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Edit-->
        <div class="row" v-if="edit === true">

            <div class="col-md-12">
                <h3>
                    <fa icon="database"></fa> Master Data > Banner >
                    <span class="talent_font_red">Edit Banner</span>
                </h3>

                <br />

                <div class="row">
                    <div class="col-md-12">
                        <div class="talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                            <h4>Banner Information</h4>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>
                                                Banner Title
                                                <span class="talent_font_red">*</span>
                                            </label>
                                            <div class="input-group">
                                                <input class="form-control" v-validate="'required'" name="title" v-model="formModel.name" maxlength="1024" />
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label>
                                                        Period
                                                    </label>
                                                </div>
                                                <div class="col-md-6"></div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-5">
                                                    <div class="input-group">
                                                        <v-date-picker class="v-date-style-report" mode="single" name="startDate" :masks="{ input: 'DD/MM/YYYY' }" :firstDayOfWeek="2" :value="null" :input-props='{placeholder: "Start Date"}' v-model="formModel.startDate" :max-date="formModel.endDate"></v-date-picker>
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
                                                        <v-date-picker class="v-date-style-report" mode="single" :masks="{ input: 'DD/MM/YYYY' }" name="endDate" :firstDayOfWeek="2" :value="null" :min-date="formModel.startDate" :input-props='{placeholder: "End Date"}' v-model="formModel.endDate"></v-date-picker>
                                                        <div class="input-group-append">
                                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>
                                                Banner Description
                                                <span class="talent_font_red">*</span>
                                            </label>
                                            <textarea class="form-control"
                                                      style="height:150px;overflow-y:scroll;" v-validate="'required'" name="description" v-model="formModel.description" maxlength="1024"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>
                                                Type of Banner
                                                <span class="talent_font_red">*</span>
                                            </label>
                                            <br />
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input"
                                                       type="radio"
                                                       name="typeOfBanner"
                                                       id="regular"
                                                       :value="1"
                                                       v-validate="'required'"
                                                       v-model="formModel.bannerTypeId" />
                                                <label class="form-check-label" for="regular">Regular</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input"
                                                       type="radio"
                                                       name="typeOfBanner"
                                                       id="specific"
                                                       :value="2"
                                                       v-model="formModel.bannerTypeId" />
                                                <label class="form-check-label" for="specific">Specific</label>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>
                                                Content Category
                                                <span class="talent_font_red"></span>
                                            </label>
                                            <div class="input-group">
                                                <select class="form-control" v-model="selectedContentType" @change="resetSelectedContent">
                                                    <option :value="emptyContentType">-------</option>
                                                    <option v-for="content in contentTypeList" :value="content">{{content.name}}</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />
                            <div class="row jusify-content-between">
                                <div class="col-md-6">
                                    <h4>File Upload</h4>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-12">
                                                    <label>
                                                        Upload Banner Picture
                                                        <span class="talent_font_red">*</span>
                                                    </label>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-6">
                                                                    <div class="custom-file">

                                                                        <input type="file" name="File" class="custom-file-input" id="customFile" v-validate="'image|size:5120'" accept="image/*" @change="previewImage($event)">
                                                                        <label class="custom-file-label talent_textshorten " for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    *Image Size 1270x548
                                                                    <br />*Max File Size 5MB
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        Content Name
                                        <span class="talent_font_red"></span>
                                    </label>
                                    <div class="input-group">
                                        <multiselect v-model="selectedContent"
                                                     id="ajax"
                                                     track-by="name"
                                                     placeholder="Select Content Name"
                                                     label="name"
                                                     name="content"
                                                     :options="contentList"
                                                     :searchable="true"
                                                     :close-on-select="true"
                                                     :clear-on-select="true"
                                                     :loading="isLoading"
                                                     :hide-selected="true"
                                                     :showNoResults="true"
                                                     @search-change="AutoComplete"
                                                     @open="reset"
                                                     v-validate="{required:selectedContentType.mobilePageId != null}">

                                            <span slot="noResult"><i>Not Found!</i></span>

                                        </multiselect>
                                    </div>
                                </div>
                            </div>
                            <div>
                                <img class="img-fluid w-100" :src="imageData.length ? imageData : 'upload-image2.png'" for="customFile" />
                            </div>
                        </div>
                        <br />
                        <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="d-flex align-items-end flex-column">
                                        <div>
                                            <button class="btn talent_bg_red talent_font_white" @click="closeClicked">Close</button>
                                            <button class="btn talent_bg_lightgreen talent_font_white" @click="editBanner(1)">Save</button>
                                            <button class="btn talent_bg_blue talent_font_white" @click="editBanner(2)">Submit</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--View Detail-->
        <div class="row" v-if="viewdetail == true">
            <div class="col-md-12">
                <h3>
                    <fa icon="database"></fa> Master Data > Banner >
                    <span class="talent_font_red">View Detail Banner</span>
                </h3>
                <br />

                <div class="row">
                    <div class="col-md-12">
                        <div class="talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                            <h4>Banner Information</h4>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>
                                                Banner Title
                                                <span class="talent_font_red">*</span>
                                            </label>
                                            <div class="input-group">
                                                <input class="form-control" v-model="bannerList.listBanner[viewIndex].name" disabled />
                                            </div>
                                            <br />
                                            <div class="row">
                                                <div class="col-md-6">
                                                    <label>
                                                        Period
                                                    </label>
                                                </div>
                                                <div class="col-md-6"></div>
                                            </div>
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="row justify-content-between">
                                                        <div class="col-md-6">
                                                            <div class="input-group talent_front ">
                                                                <input class="form-control" :placeholder="dateToString(bannerList.listBanner[viewIndex].startDate)" disabled />
                                                                <div class="input-group-append">
                                                                    <span class="input-group-text">
                                                                        <fa icon="calendar-alt"></fa>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6">
                                                            <div class="input-group talent_front">
                                                                <input class="form-control" :placeholder="dateToString(bannerList.listBanner[viewIndex].endDate)" disabled />
                                                                <div class="input-group-append">
                                                                    <span class="input-group-text">
                                                                        <fa icon="calendar-alt"></fa>
                                                                    </span>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>
                                                Banner Description
                                                <span class="talent_font_red">*</span>
                                            </label>
                                            <textarea class="form-control" disabled v-model="bannerList.listBanner[viewIndex].description"
                                                      style="height:150px;overflow-y:scroll;"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />

                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>
                                                Type of Banner
                                                <span class="talent_font_red">*</span>
                                            </label>
                                            <br />
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input"
                                                       type="radio"
                                                       name="typeOfBanner"
                                                       v-model="bannerList.listBanner[viewIndex].bannerTypeId"
                                                       :value="1"
                                                       disabled />
                                                <label class="form-check-label" for="inlineRadio1">Regular</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <input class="form-check-input"
                                                       type="radio"
                                                       name="typeOfBanner"
                                                       v-model="bannerList.listBanner[viewIndex].bannerTypeId"
                                                       :value="2"
                                                       disabled />
                                                <label class="form-check-label" for="inlineRadio2">Specific</label>
                                            </div>
                                        </div>
                                        <div class="col-md-6">
                                            <label>
                                                Content Type
                                                <span class="talent_font_red"></span>
                                            </label>
                                            <div class="input-group">
                                                <select class="form-control" v-model="bannerList.listBanner[viewIndex].pageName" disabled>
                                                    <option :value="bannerList.listBanner[viewIndex].pageName">{{bannerList.listBanner[viewIndex].pageName}}</option>
                                                </select>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <br />

                            <div class="row jusify-content-between">
                                <div class="col-md-6">
                                    <h4>File Upload</h4>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-12">
                                                    <label>
                                                        Upload Banner Picture
                                                        <span class="talent_font_red">*</span>
                                                    </label>
                                                    <div class="row">
                                                        <div class="col-md-12">
                                                            <div class="row justify-content-between">
                                                                <div class="col-md-6">
                                                                    <div class="custom-file">
                                                                        <input type="file"
                                                                               class="custom-file-input"
                                                                               id="customFile"
                                                                               disabled />
                                                                        <label class="custom-file-label"
                                                                               for="customFile">{{bannerList.listBanner[viewIndex].blobName}}</label>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    *Image Size 1270x548
                                                                    <br />*Max File Size 5MB
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        Content Name
                                        <span class="talent_font_red"></span>
                                    </label>
                                    <div class="input-group">
                                        <select class="form-control" v-model="selectedContent" disabled>
                                            <option :value="selectedContent">{{selectedContent.name}}</option>
                                        </select>
                                    </div>
                                </div>
                            </div>

                            <div>
                                <img class="img-fluid w-100" :src="imageData.length ? imageData : 'upload-image2.png'" for="customFile" />
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
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="col-md-12 justify-content-center d-flex">
                            <h5>Are you sure want to delete {{deleteName}}?</h5>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="col-md-12 d-flex justify-content-around">
                                <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="deleteConfirmed()">Delete</button>
                                <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" data-dismiss="modal">Close</button>
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
    import { BannerViewModel, BannerFormModel, MobileMappingPage, ContentModel, BannerService, ApprovalStatusForBannerModel, UserPrivilegeSettingsService, UserAccessCRUD, FileContent } from '../../services/NSwagService';
    import { BlobService } from '../../services/BlobService';
    import Message from '../../class/Message';
    import { ApprovalStatusEnum } from '../../enum/ApprovalStatusEnum';
    import { PageEnums } from '../../enum/PageEnums';

    @Component({
        created: async function (this: Banners) {
            this.bannerList = {};
            this.bannerList.listBanner = [];
            this.getAccess();
            await this.initialize();

            if (this.fromOutside === true) {
                await this.initialize();
                await this.viewClickedFromOutside(this.bannerId);
            }
        },

        props: ['bannerId', 'fromOutside']
    })

    export default class Banners extends Vue {

        bannerId: number;
        fromOutside: boolean;

        props: any = {
            class: "disabled",
            placeholder: '',
            readonly: true
        };

        errorMessage: string = '';
        successMessage: string = '';

        currentPage: number = 1;
        pageSize: number = 10;
        framework: string;
        compiler: string;

        imageUpload: FileContent = null;

        bannerFilter: IBannerFilterModel = {
            ApprovedBy: '',
            BannerTitle: '',
            DateFilter: {
                end: null,
                start: null
            },
            EndDate: null,
            SortBy: '',
            StartDate: null,
            Status: '',
            TypeofBanner: null
        }

        approvalStatusBanner: ApprovalStatusForBannerModel[] = [];

        isBusy: boolean = false;

        //Variable untuk sorting
        bannerName = new Sort();
        bannerType = new Sort();
        startDate = new Sort();
        endDate = new Sort();
        status = new Sort();
        approvedBy = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        Service: BannerService = new BannerService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        minDate: Date = new Date('0001-01-01T00:00:00');

        bannerList: BannerViewModel = {};
        contentTypeList: MobileMappingPage[] = [];
        contentList: ContentModel[] = [];

        selectedContentType: MobileMappingPage = {};
        selectedContent: ContentModel = {};

        emptyContentType: MobileMappingPage = {};

        isDisabled: number = 1;

        //add
        add: boolean = false;
        edit: boolean = false;
        viewdetail: boolean = false;

        viewIndex: number;
        deleteIndex: number;
        deleteName: string = '';

        waitingStatus = ApprovalStatusEnum.WAITING;

        //File Upload
        imageName: string = '';
        imageTempName: string = '';
        imageData: string | ArrayBuffer = '/upload-image2.png';
        formData: FormData = new FormData();

        isLoading: boolean = false;
        timeout: any = null;

        formModel: BannerFormModel = {
            bannerId: null,
            bannerTypeId: null,
            blobId: null,
            name: '',
            startDate: null,
            endDate: null,
            mobilePageId: null,
            contentId: null,
            description: null,
            createdAt: null,
            createdBy: null,
            updatedAt: null,
            approvedAt: null,
            bannerTypeName: null,
            approvalStatus: null,
            actionBy: null
        };

        dateToString(date: Date) {

            if (!date) {
                return '';
            }

            if (new Date(date).getUTCFullYear() === this.minDate.getUTCFullYear()) {
                return '';
            }

            let newDate = new Date(date);

            var ddDate = newDate.getDate();
            var mmDate = newDate.getMonth() + 1;
            var yyyyDate = newDate.getFullYear();
            var dd = '' + ddDate;
            var mm = '' + mmDate;
            var yyyy = yyyyDate;
            if (ddDate < 10) {
                dd = '0' + ddDate;
            }
            if (mmDate < 10) {
                mm = '0' + mmDate;
            }
            var today = dd + '/' + mm + '/' + yyyy;

            return today;
        }

        resetSearch() {
            this.bannerFilter = {
                ApprovedBy: '',
                BannerTitle: '',
                DateFilter: {
                    end: null,
                    start: null
                },
                EndDate: null,
                SortBy: '',
                StartDate: null,
                Status: '',
                TypeofBanner: null
            };

            this.ResetSort('');

            this.fetch();
        }


        async search() {
            await this.fetch();
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Banner);
            this.crud = data
        }

        async fetch() {
            this.isBusy = true;
            this.bannerList = await this.Service.getBannerFiltered(this.bannerFilter.StartDate, this.bannerFilter.EndDate, this.bannerFilter.DateFilter.start, this.bannerFilter.DateFilter.end, this.bannerFilter.TypeofBanner, this.bannerFilter.BannerTitle, this.bannerFilter.Status, this.bannerFilter.ApprovedBy, this.bannerFilter.SortBy, this.currentPage);

            this.approvalStatusBanner = await this.Service.getApprovalStatusForBanner();
            this.isBusy = false;
        }

        clearMessage() {
            this.errorMessage = '';
            this.successMessage = '';
        }

        async insertBanner(type: number) {

            if (!this.crud.create) {
                return
            }

            let validate = await this.$validator.validateAll();

            if (validate == false) {
                return;
            }

            //let blobId = await BlobService.uploadService(this.formData);

            //if (blobId == null) {
            //    return;
            //}

            if (this.imageUpload != null) {
                this.formModel.imageUpload = this.imageUpload;
            }

            if (this.selectedContent.id != null) {
                this.formModel.mobilePageId = this.selectedContentType.mobilePageId;
                this.formModel.contentId = this.selectedContent.id;
            }
            this.formModel.insertType = type;
            //this.formModel.blobId = blobId;
            this.isBusy = true;
            try {
                var result = await this.Service.insertBanner(this.formModel);
                if (result === true) {

                    this.successMessage = Message.SuccessInsertMessage;

                    this.isBusy = false;
                    this.resetSearch();
                    this.closeClicked();
                }

            }
            catch{
                this.errorMessage = "Failed to Insert!";
                this.isBusy = false;
            }
        }

        async editBanner(type: number) {

            if (!this.crud.update) {
                return
            }

            let validate = await this.$validator.validateAll();

            if (validate == false) {
                return;
            }

            if (this.imageTempName != this.imageName) {
                if (this.imageUpload != null) {
                    this.formModel.imageUpload = this.imageUpload;
                }
            }

            if (this.selectedContent.id != null) {
                this.formModel.mobilePageId = this.selectedContentType.mobilePageId;
                this.formModel.contentId = this.selectedContent.id;
            }
            else {
                this.formModel.mobilePageId = null;
                this.formModel.contentId = null;
            }
            this.formModel.insertType = type;
            this.isBusy = true;
            try {
                var result = await this.Service.updateBanner(this.formModel);

                if (result === true) {
                    this.successMessage = Message.SuccessEditMessage;
                    this.isBusy = false;

                    this.resetSearch();

                    this.closeClicked();
                }
            }
            catch{
                this.errorMessage = "Failed to Update Data!";
                this.isBusy = false;
            }
            this.isBusy = false;
        }

        async deleteConfirmed() {

            if (!this.crud.delete) {
                return
            }

            this.isBusy = true;
            try {
                let result = await this.Service.deleteBanner(this.bannerList.listBanner[this.deleteIndex].bannerId);
                if (result === true) {
                    this.fetch();
                    this.successMessage = Message.RemoveMessage;
                }
            }
            catch{
                this.errorMessage = "Failed to Delete Data!"
            }
            this.isBusy = false;
        }

        async initialize() {
            await this.fetch();
        }

        async addClicked() {
            this.clearMessage();

            if (!this.crud.create) {
                return
            }

            this.formData = new FormData();
            this.imageData = '/upload-image2.png';
            this.imageName = '';
            this.add = true;
            this.selectedContent = {};
            this.selectedContentType = {};
            this.formModel = {
                bannerId: null,
                bannerTypeId: null,
                blobId: null,
                name: '',
                startDate: null,
                endDate: null,
                mobilePageId: null,
                contentId: null,
                description: null,
                createdAt: null,
                createdBy: null,
                updatedAt: null,
                approvedAt: null,
                bannerTypeName: null,
                approvalStatus: null,
                actionBy: null
            };

            this.contentTypeList = await this.Service.getAllContentType();
        }

        deleteClicked(index: number) {
            this.clearMessage();

            if (!this.crud.delete) {
                return;
            }

            this.deleteIndex = index;
            this.deleteName = this.bannerList.listBanner[index].name;
        }

        async viewClicked(index: number) {

            if (!this.crud.read) {
                return
            }

            this.viewdetail = true;
            this.viewIndex = index;

            let imageUrl = await BlobService.getImageUrl(this.bannerList.listBanner[this.viewIndex].blobId, this.bannerList.listBanner[this.viewIndex].mime);
            this.imageData = imageUrl;

            this.selectedContent = {};

            if (this.bannerList.listBanner[this.viewIndex].mobilePageId != null) {
                this.selectedContent = await this.Service.getContentNameById(this.bannerList.listBanner[this.viewIndex].mobilePageId, this.bannerList.listBanner[this.viewIndex].contentId)
            }
        }

        async viewClickedFromOutside(id: number) {

            if (!this.crud.read) {
                return
            }

            this.viewdetail = true;
            this.viewIndex = 0;

            this.bannerList = await this.Service.getBannerByIdFromOutside(id);

            let imageUrl = await BlobService.getImageUrl(this.bannerList.listBanner[this.viewIndex].blobId, this.bannerList.listBanner[this.viewIndex].mime);
            this.imageData = imageUrl;

            this.selectedContent = {};

            if (this.bannerList.listBanner[this.viewIndex].mobilePageId != null) {
                this.selectedContent = await this.Service.getContentNameById(this.bannerList.listBanner[this.viewIndex].mobilePageId, this.bannerList.listBanner[this.viewIndex].contentId)
            }


        }

        getStringDate(date: Date) {

            if (!date) {
                return '-';
            }

            if (new Date(date) === this.minDate) {
                return '-';
            }

            let newDate = new Date(date);

            var ddDate = newDate.getDate();
            var mmDate = newDate.getMonth() + 1;
            var yyyyDate = newDate.getFullYear();
            var dd = '' + ddDate;
            var mm = '' + mmDate;
            var yyyy = yyyyDate;
            if (ddDate < 10) {
                dd = '0' + ddDate;
            }
            if (mmDate < 10) {
                mm = '0' + mmDate;
            }
            var today = dd + '/' + mm + '/' + yyyy;

            return today;
        }

        closeClicked() {
            this.add = false;
            this.edit = false;
            this.viewdetail = false;
            this.imageUpload = null;

            this.fetch();

        }

        async editClicked(index: number) {
            this.clearMessage();

            if (!this.crud.update) {
                return;
            }

            this.formModel = {
                bannerId: null,
                bannerTypeId: null,
                blobId: null,
                name: '',
                startDate: null,
                endDate: null,
                mobilePageId: null,
                contentId: null,
                description: null,
                createdAt: null,
                createdBy: null,
                updatedAt: null,
                approvedAt: null,
                bannerTypeName: null,
                approvalStatus: null,
                actionBy: null
            };
            this.formModel.bannerId = this.bannerList.listBanner[index].bannerId;
            this.formModel.bannerTypeId = this.bannerList.listBanner[index].bannerTypeId;
            this.formModel.blobId = this.bannerList.listBanner[index].blobId;
            this.formModel.contentId = this.bannerList.listBanner[index].contentId;
            this.formModel.description = this.bannerList.listBanner[index].description;
            this.formModel.mobilePageId = this.bannerList.listBanner[index].mobilePageId;
            this.formModel.endDate = this.bannerList.listBanner[index].endDate == null ? null : new Date(this.bannerList.listBanner[index].endDate);
            this.formModel.startDate = this.bannerList.listBanner[index].startDate == null ? null : new Date(this.bannerList.listBanner[index].startDate);
            this.formModel.name = this.bannerList.listBanner[index].name;

            this.contentTypeList = await this.Service.getAllContentType();

            let urlImage = await BlobService.getImageUrl(this.formModel.blobId, this.bannerList.listBanner[index].mime);

            this.imageData = urlImage;
            this.imageName = this.bannerList.listBanner[index].blobName;
            this.imageTempName = this.bannerList.listBanner[index].blobName;

            if (this.formModel.contentId != null) {
                this.selectedContentType = this.contentTypeList.find(Q => Q.mobilePageId == this.formModel.mobilePageId);
                this.selectedContent = await this.Service.getContentNameById(this.formModel.mobilePageId, this.formModel.contentId);
            }
            else {
                this.selectedContentType = {};
                this.selectedContent = {};
            }

            this.edit = true;
        }

        async AutoComplete(query) {

            if (!this.selectedContentType.mobilePageId) {
                this.errorMessage = 'Content type must be filled';
                return;
            }

            if (query == null || query === '') {
                this.contentList = [];
                return;
            }

            this.isLoading = true;
            if (this.timeout) clearTimeout(this.timeout);
            this.timeout = setTimeout(
                () => {
                    this.GetContentByName(query);
                }, 500
            );
        }


        async GetContentByName(query) {
            if (query === '' || query == null) {
                this.contentList = [];
                return;
            }

            let result = await this.Service.getContentName(this.selectedContentType.mobilePageId, query);
            this.contentList = result;

            this.isLoading = false;
        }

        reset() {
            this.contentList = [];
        }

        resetSelectedContent() {
            this.selectedContent = {};
        }

        backPage() {
            this.imageUpload = null;
            window.history.back();
        }

        //ClickSortBanner
        async ClickSortBanner() {
            this.ResetSort('bannerName');
            //Sorting
            this.bannerName.sorting();
            //Function Sorting
            if (this.bannerName.sortup === true) {
                this.bannerFilter.SortBy = 'bannerName';
            }
            else if (this.bannerName.sortdown === true) {
                this.bannerFilter.SortBy = 'bannerNameDesc';
            }
            else {
                this.bannerFilter.SortBy = '';
            }

            this.fetch();

            return;
        }

        //ClickSortTypeBanner
        async ClickSortTypeBanner() {
            this.ResetSort('bannerType');
            //Sorting
            this.bannerType.sorting();
            //Function Sorting
            if (this.bannerType.sortup === true) {
                this.bannerFilter.SortBy = 'bannerType';
            }
            else if (this.bannerType.sortdown === true) {
                this.bannerFilter.SortBy = 'bannerTypeDesc';
            }
            else {
                this.bannerFilter.SortBy = '';
            }

            this.fetch();

            return;
        }

        //ClickSortStartDate
        async ClickSortStartDate() {
            this.ResetSort('startDate');
            //Sorting
            this.startDate.sorting();
            //Function Sorting
            if (this.startDate.sortup === true) {
                this.bannerFilter.SortBy = 'startDate';
            }
            else if (this.startDate.sortdown === true) {
                this.bannerFilter.SortBy = 'startDateDesc';
            }
            else {
                this.bannerFilter.SortBy = '';
            }
            this.fetch();

            return;
        }

        //ClickSortEndDate
        async ClickSortEndDate() {
            this.ResetSort('endDate');
            //Sorting
            this.endDate.sorting();
            //Function Sorting
            if (this.endDate.sortup === true) {
                this.bannerFilter.SortBy = 'endDate';
            }
            else if (this.endDate.sortdown === true) {
                this.bannerFilter.SortBy = 'endDateDesc';
            }
            else {
                this.bannerFilter.SortBy = '';
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
            if (this.status.sortup === true) {
                this.bannerFilter.SortBy = 'status';
            }
            else if (this.status.sortdown === true) {
                this.bannerFilter.SortBy = 'statusDesc';
            }
            else {
                this.bannerFilter.SortBy = '';
            }
            this.fetch();
            return;
        }

        //ClickSortApprovedBy
        async ClickSortApprovedBy() {
            this.ResetSort('approvedBy');
            //Sorting
            this.approvedBy.sorting();
            //Function Sorting
            if (this.approvedBy.sortup === true) {
                this.bannerFilter.SortBy = 'approvedBy';
            }
            else if (this.approvedBy.sortdown === true) {
                this.bannerFilter.SortBy = 'approvedByDesc';
            }
            else {
                this.bannerFilter.SortBy = '';
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
            if (this.createdDate.sortup === true) {
                this.bannerFilter.SortBy = 'createdDate';
            }
            else if (this.createdDate.sortdown === true) {
                this.bannerFilter.SortBy = 'createdDateDesc';
            }
            else {
                this.bannerFilter.SortBy = '';
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

            if (this.updatedDate.sortup === true) {
                this.bannerFilter.SortBy = 'updatedDate';
            }
            else if (this.updatedDate.sortdown === true) {
                this.bannerFilter.SortBy = 'updatedDateDesc';
            }
            else {
                this.bannerFilter.SortBy = '';
            }
            this.fetch();
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'bannerName':
                    this.bannerType.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.status.reset();
                    this.approvedBy.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'bannerType':
                    this.bannerName.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.status.reset();
                    this.approvedBy.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'startDate':
                    this.bannerName.reset();
                    this.bannerType.reset();
                    this.endDate.reset();
                    this.approvedBy.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'endDate':
                    this.bannerName.reset();
                    this.bannerType.reset();
                    this.startDate.reset();
                    this.status.reset();
                    this.approvedBy.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'status':
                    this.bannerName.reset();
                    this.bannerType.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.approvedBy.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'approvedBy':
                    this.bannerName.reset();
                    this.bannerType.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.bannerName.reset();
                    this.bannerType.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.status.reset();
                    this.approvedBy.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.bannerName.reset();
                    this.bannerType.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.status.reset();
                    this.approvedBy.reset();
                    this.createdDate.reset();
                    return;
                default:
                    this.bannerFilter.SortBy = '';
                    this.bannerName.reset();
                    this.bannerType.reset();
                    this.startDate.reset();
                    this.endDate.reset();
                    this.status.reset();
                    this.approvedBy.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
            }
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

        //Function for preview the Image
        async previewImage(event) {
            var input = event.target;
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.readAsDataURL(event.target.files[0]);
                let name = event.target.files[0].name;
                reader.onload = (e: Event) => {
                    let image = (<FileReader>e.target).result;
                    this.imageData = image;
                }
                this.imageName = name;
            }

            this.formData = new FormData();

            Array.from(Array(event.target.files.length).keys())
                .map(x => {
                    this.formData.append(event.target.files, event.target.files[x], event.target.files[x].name);
                });

            let inputFile = (<HTMLInputElement>event.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = event.target.files[0];

            let array = file.name.split(".");

            this.imageUpload = {
                base64: base64String,
                fileName: file.name,
                contentType: array.pop()
            };
        }
    }
</script>