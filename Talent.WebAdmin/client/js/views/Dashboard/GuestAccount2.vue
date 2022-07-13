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
                <h3><fa icon="chart-pie"></fa> Dashboard > <span class="talent_font_red">Guest Account</span></h3>
                <br />
                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow" style="display: none">
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
                                        <v-date-picker class="v-date-style-report" :masks="{ input: 'DD/MM/YYYY' }" mode="range" :firstDayOfWeek="2" v-model="filterDate"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
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
                                    <select class="form-control" v-model="filterGuideTypeName">
                                        <option v-for="t in guideTypes.listGuideTypeModel" :value="t.name">{{t.name}}</option>
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
                                    <select class="form-control" v-model="filterApprovalStatus">
                                        <option v-for="a in approvalStatuses.listApprovalStatusModel" :value="a.approvalName">{{a.approvalName}}</option>
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
                                        <input class="form-control" v-model="filterGuideName" />
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
                                        <input class="form-control" v-model="filterCreatedBy" />
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
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="fetch">Search</button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="reset">Reset</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- First Row -->
                <div class="col-md-12 p-0">
                    <div class="row d-flex align-items-stretch">
                        <div class="col-md-4 pr-2 h-full">
                            <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                                <h5>Total Guest Account</h5>
                                <div class="col">
                                    <chart-total-guest-account />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 px-2 text-sm">
                            <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                                <h5>Dealer</h5>
                                <!-- Filter Search -->
                                <div class="row mb-3">
                                    <!-- Filter -->
                                    <div class="col-auto">
                                        <button type="button" class="btn btn-info text-white">
                                        Filter
                                        </button>
                                    </div>
                                    <!-- Search Bar -->
                                    <div class="col">
                                        <form class="d-flex">
                                        <input
                                            class="form-control"
                                            type="search"
                                            placeholder="Search"
                                            aria-label="Search"
                                        />
                                        </form>
                                    </div>
                                </div>
                                <!-- Table -->
                                <div class="table-container">
                                <table
                                    class="table table-hover table-bordered table-responsive"
                                >
                                    <thead>
                                    <tr>
                                        <th scope="col">
                                        <div class="row">
                                            <div class="col">Status</div>
                                            <div class="col-auto">
                                            <span
                                                ><i class="fa fa-sort" aria-hidden="true"></i
                                            ></span>
                                            </div>
                                        </div>
                                        </th>
                                        <th scope="col">
                                        <div class="row">
                                            <div class="col">Done</div>
                                            <div class="col-auto">
                                            <span
                                                ><i class="fa fa-sort" aria-hidden="true"></i
                                            ></span>
                                            </div>
                                        </div>
                                        </th>
                                        <th scope="col">
                                        <div class="row">
                                            <div class="col">Pending</div>
                                            <div class="col-auto">
                                            <span
                                                ><i class="fa fa-sort" aria-hidden="true"></i
                                            ></span>
                                            </div>
                                        </div>
                                        </th>
                                        <th scope="col">
                                        <div class="row">
                                            <div class="col">Total MP</div>
                                            <div class="col-auto">
                                            <span
                                                ><i class="fa fa-sort" aria-hidden="true"></i
                                            ></span>
                                            </div>
                                        </div>
                                        </th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    <!-- Item 1 -->
                                    <tr>
                                        <td>Unverified</td>
                                        <td>15</td>
                                        <td>15</td>
                                        <td>15</td>
                                    </tr>
                                    <!-- Item 2 -->
                                    <tr>
                                        <td>Unverified</td>
                                        <td>15</td>
                                        <td>15</td>
                                        <td>15</td>
                                    </tr>
                                    <!-- Item 3 -->
                                    <tr>
                                        <td>Unverified</td>
                                        <td>15</td>
                                        <td>15</td>
                                        <td>15</td>
                                    </tr>
                                    </tbody>
                                </table>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 pl-2 text-sm">
                            <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                                <h5>Others</h5>
                                <!-- Filter Search -->
                                <div class="row mb-3">
                                <!-- Filter -->
                                <div class="col-auto">
                                    <button type="button" class="btn btn-info text-white">
                                    Filter
                                    </button>
                                </div>
                                <!-- Search Bar -->
                                <div class="col">
                                    <form class="d-flex">
                                    <input
                                        class="form-control"
                                        type="search"
                                        placeholder="Search"
                                        aria-label="Search"
                                    />
                                    </form>
                                </div>
                                </div>
                                <!-- Table -->
                                <div class="table-container">
                                <table
                                    class="table table-hover table-bordered table-responsive"
                                >
                                    <thead>
                                    <tr>
                                        <th scope="col">
                                        <div class="row">
                                            <div class="col">Status</div>
                                            <div class="col-auto">
                                            <span
                                                ><i class="fa fa-sort" aria-hidden="true"></i
                                            ></span>
                                            </div>
                                        </div>
                                        </th>
                                        <th scope="col">
                                        <div class="row">
                                            <div class="col">Done</div>
                                            <div class="col-auto">
                                            <span
                                                ><i class="fa fa-sort" aria-hidden="true"></i
                                            ></span>
                                            </div>
                                        </div>
                                        </th>
                                        <th scope="col">
                                        <div class="row">
                                            <div class="col">Pending</div>
                                            <div class="col-auto">
                                            <span
                                                ><i class="fa fa-sort" aria-hidden="true"></i
                                            ></span>
                                            </div>
                                        </div>
                                        </th>
                                        <th scope="col">
                                        <div class="row">
                                            <div class="col">Total MP</div>
                                            <div class="col-auto">
                                            <span
                                                ><i class="fa fa-sort" aria-hidden="true"></i
                                            ></span>
                                            </div>
                                        </div>
                                        </th>
                                    </tr>
                                    </thead>
                                    <tbody>
                                    <!-- Item 1 -->
                                    <tr>
                                        <td>Unverified</td>
                                        <td>15</td>
                                        <td>15</td>
                                        <td>15</td>
                                    </tr>
                                    <!-- Item 2 -->
                                    <tr>
                                        <td>Unverified</td>
                                        <td>15</td>
                                        <td>15</td>
                                        <td>15</td>
                                    </tr>
                                    <!-- Item 3 -->
                                    <tr>
                                        <td>Unverified</td>
                                        <td>15</td>
                                        <td>15</td>
                                        <td>15</td>
                                    </tr>
                                    </tbody>
                                </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <br />

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h5>Account</h5>
                    <div class="col-md-12 talent_magin_small mt-3">
                        <div class="row align-items-center row justify-content-between">
                            <div class="col d-flex p-0">
                                <button class="btn btn-info mr-2" >
                                    Filter
                                </button>
                                <div class="input-group">
                                    <input class="form-control" v-model="filterGuideName" placeholder="Search"/>
                                </div>
                            </div>
                            <div class="col-auto">
                                <button class="btn btn-success mr-2" >
                                    Extend Account
                                </button>
                                <button class="btn btn-danger mr-2" >
                                    Suspend Account
                                </button>
                                <button class="btn btn-warning mr-2" >
                                    Upgrade Account
                                </button>
                                <button class="btn btn-primary mr-2" >
                                    Export To xls/csv
                                </button>
                            </div>
                        </div>
                    </div>

                    <div class="talent_overflowx mt-3">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortGuideName()">Guide Title <fa icon="sort" v-if="guideName.sort == true"></fa><fa icon="sort-up" v-if="guideName.sortup == true"></fa><fa icon="sort-down" v-if="guideName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortGuideType()">Type of Guide <fa icon="sort" v-if="guideType.sort == true"></fa><fa icon="sort-up" v-if="guideType.sortup == true"></fa><fa icon="sort-down" v-if="guideType.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedBy()">Created By <fa icon="sort" v-if="createdBy.sort == true"></fa><fa icon="sort-up" v-if="createdBy.sortup == true"></fa><fa icon="sort-down" v-if="createdBy.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortApprovalStatus()">Status <fa icon="sort" v-if="approvalStatus.sort == true"></fa><fa icon="sort-up" v-if="approvalStatus.sortup == true"></fa><fa icon="sort-down" v-if="approvalStatus.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="3">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(g, index) in guides.listGuideJoinModel">
                                    <td>
                                        {{g.name}}
                                    </td>
                                    <td>
                                        {{g.guideTypeName}}
                                    </td>
                                    <td>
                                        {{g.createdBy}}
                                    </td>
                                    <td>
                                        {{g.approvalStatus}}
                                    </td>
                                    <td>
                                        {{convertDateTime(g.createdAt)}}
                                    </td>
                                    <td>
                                        {{convertDateTime(g.updatedAt)}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="detailClicked(g.guideId)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="editClicked(g.guideId)" :disabled="g.approvalStatus == 'Waiting for Approval'">Edit</button>
                                    </td>
                                    <!-- <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click.prevent="deleteClicked(g.guideId, index)" :disabled="g.approvalStatus == 'Waiting for Approval'">Remove</button>
                                    </td> -->
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--Pagination-->
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate :currentPage.sync="currentPage" :total-data=guides.totalItem :limit-data=itemPerPage @update:currentPage="fetch()"></paginate>
                    </div>

                </div>

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
        <div class="row" v-else-if="add === true">
            <div class="col-md-12">
                <h3><fa icon="chart-pie"></fa> Dashboard > Guest Account > <span class="talent_font_red">Detil Guest Account</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Profile</h4>

                    <div v-if="fileType === false || fileSize === false" class="alert alert-danger">
                        <div v-if="fileSize === false">Maximum File Size is 5 MB</div>
                        <div v-if="fileType === false">Please input a pdf/jpg/png/jpeg file</div>
                    </div>

                    <div v-if="validateAddMessage() === false" class="alert alert-danger">
                        <div v-if="addModel.guideTypeId === 0">Guide Type is Required</div>
                        <div v-if="addModel.name === ''">Guide Title is Required</div>
                        <div v-if="addFileName === ''">File is Required</div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Title</label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="addModel.name" maxlength="1024" />
                                    </div>
                                    <br />
                                    <!-- <label>Type of Guide <span class="talent_font_red">*</span></label>
                                    <select class="form-control" v-model="addModel.guideTypeId">
                                        <option v-for="t in guideTypes.listGuideTypeModel" :value="t.guideTypeId">{{t.name}}</option>
                                    </select> -->
                                </div>
                                <div class="col-md-12">
                                    <label>Description</label>
                                    <textarea class="form-control" style="height:130px; overflow-y:scroll;" v-model="addModel.description" maxlength="1024"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow" style="display: none">
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
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <div class="custom-file">
                                                        <input type="file" class="custom-file-input" id="customFile" @change="onAddFileChange" />
                                                        <label class="custom-file-label talent_textshorten" for="customFile">{{addFileName == null ? 'Choose File' : addFileName}}</label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    *Recommended File: PDF, JPEG, JPG, PNG
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
                        <img v-if="fileUploadType == 'image'" class="h-100 w-100" :src="addImageData" for="customFile" />
                        <object v-else class="h-100 w-100 min-heigth-full" :data="addFileData" for="customFile" />
                    </div>
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">Close</button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="addData(3)">Save</button>
                                    <button class="btn talent_bg_darkblue talent_font_white" @click.prevent="addData(2)">Submit</button>
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
                <h3><fa icon="chart-pie"></fa> Dashboard > Dashboard > <span class="talent_font_red">Edit Guest Account</span></h3>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Profile</h4>

                    <div v-if="fileType === false || fileSize === false" class="alert alert-danger">
                        <div v-if="fileSize === false">Maximum File Size is 5 MB</div>
                        <div v-if="fileType === false">Please input a pdf/jpg/png/jpeg file</div>
                    </div>

                    <div v-if="validateEditMessage() === false" class="alert alert-danger">
                        <div v-if="editModel.guideTypeId === 0">Guide Type is Required</div>
                        <div v-if="editModel.name === ''">Guide Title is Required</div>
                        <div v-if="editFileName === ''">File is Required</div>
                    </div>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Title<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="editModel.name" maxlength="1024" />
                                    </div>
                                    <br />
                                    <!-- <label>Type of Guide<span class="talent_font_red">*</span></label>
                                    <select class="form-control" v-model="editModel.guideTypeId">
                                        <option v-for="t in guideTypes.listGuideTypeModel" :value="t.guideTypeId">{{t.name}}</option>
                                    </select> -->
                                </div>
                                <div class="col-md-12">
                                    <label>Description</label>
                                    <textarea class="form-control" style="height:130px;overflow-y:scroll;" v-model="editModel.description" maxlength="1024"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow" style="display: none">
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
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <div class="custom-file">
                                                        <input type="file" class="custom-file-input" id="customFile" @change="onEditFileChange" />
                                                        <label class="custom-file-label talent_textshorten" for="customFile">{{editFileName == null ? 'Choose File' : editFileName}}</label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    *Recommended File: PDF, JPG, JPEG, PNG
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
                        <img v-if="fileUploadType == 'image'" class="h-100 w-100" :src="editImageData" for="customFile" />
                        <object v-else class="h-100 w-100 min-heigth-full" :data="editFileData" for="customFile" />
                    </div>
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">Close</button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="editData(3)">Save</button>
                                    <button class="btn talent_bg_darkblue talent_font_white" @click.prevent="editData(2)">Submit</button>
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
                <h3><fa icon="database"></fa> Dashboard > Guest Account > <span class="talent_font_red">Detil Guest Account</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Profile</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Title<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="detailModel.name" disabled />
                                    </div>
                                    <br />
                                    <!-- <label>Type of Guide<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="detailModel.guideTypeName" disabled />
                                    </div> -->
                                </div>
                                <div class="col-md-12">
                                    <label>
                                        Description
                                    </label>
                                    <textarea class="form-control" style="height:130px;overflow-y:scroll;" v-model="detailModel.description" disabled></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow" style="display: none">
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
                                            <div class="row justify-content-between">
                                                <div class="col-md-6">
                                                    <div class="custom-file">
                                                        <input type="file" class="custom-file-input" id="customFile" disabled />
                                                        <label class="custom-file-label talent_textshorten" for="customFile">{{detailFileName.length ? detailFileName : 'Choose File'}}</label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    *Recommended File: PDF, JPG, JPEG, PNG
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
                        <object class="h-100 w-100 min-heigth-full" :data="detailFileData" for="customFile" />
                    </div>
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white" @click.prevent="backPage">Close</button>
                                    <button v-else class="btn talent_bg_red talent_font_white" @click.prevent="closeClicked">Close</button>
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
    import { GuideService, GuideTypeService, GuideFormModel, GuideTypeViewModel, GuideJoinViewModel, ApprovalStatusService, FileContent, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService'
    import { BlobService } from '../../services/BlobService';
    import { isNullOrUndefined } from 'util';
    import { PageEnums } from '../../enum/PageEnums';
    import ChartTotalGuestAccount from '../../components/Dashboard/GuestAccount/ChartTotalGuestAccount.vue';

    @Component({
        components: { ChartTotalGuestAccount },
        created: async function (this: Guide) {
            this.isBusy = true;
            await this.initDropdownData();
            await this.getAccess();
            if (this.fromOutside === true) {
                await this.detailClicked(this.guideId);
            }
            await this.fetch();
        },
        props: ['guideId', 'fromOutside']
    })
    export default class Guide extends Vue {

        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Guide);
            this.crud = data;
        }

         crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

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
            this.guides = await this.guideMan.getAllJoinGuides(this.filterDate.start, this.filterDate.end, this.filterGuideName, this.filterGuideTypeName, this.filterCreatedBy, this.filterApprovalStatus, this.sortBy, this.currentPage);
            this.isBusy = false;
        }

        async initDropdownData() {
            this.guideTypes = await this.guideTypeMan.getAllGuideTypes();
            this.approvalStatuses = await this.approvalMan.getAllApprovalStatuses();
        }

        convertDateTime(stringdate) {
            var date = new Date(stringdate);
            function pad(n) { return n < 10 ? "0" + n : n; }
            return pad(date.getDate()) + "/" + pad(date.getMonth() + 1) + "/" + date.getFullYear();
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
            if(this.crud.create == false){
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

            let array = file.name.split(".");

            let extension = array.pop();
            if (extension != 'pdf' && extension != 'jpeg' && extension != 'jpg' && extension != 'png') {
                this.fileType = false;
            }
            if (fileInput[0].size > 5048576) {
                this.fileSize = false;
            }

            if (!this.fileType || !this.fileSize)
                return;

            this.addFileName = fileInput[0].name;

            let baseFileInput = (<HTMLInputElement>e.target).files[0];
            let base64String = await this.convertToBase64(baseFileInput);

            this.addModel.fileContent = {
                base64: base64String,
                fileName: this.addFileName,
                contentType: extension
            }

            if (extension == 'jpeg' || extension == 'jpg' || extension == 'png') {
                this.fileUploadType = 'image';
                var temp = this;
                let reader = new FileReader();
                reader.readAsDataURL(fileInput[0]);
                reader.onload = function () {
                    temp.addImageData = reader.result as string;
                    temp.addFileData = null;
                };
            }
            else if(extension == 'pdf'){
                this.fileUploadType = 'pdf';
                this.addFileData = URL.createObjectURL(file);
                this.addImageData = '/upload-image2.png';
            }
        }

        validateAdd() {
            if (this.addModel.guideTypeId === 0 || this.addModel.name === '' || this.addFileName == '' || this.addModel.fileContent == null) {
                return false;
            } else {
                return true;
            }
        }

        validateAddMessage() {
            if (this.addModel.guideTypeId === 0 || this.addModel.name === '' || this.addFileName == '') {
                return false;
            } else {
                return true;
            }
        }

        validateEditMessage() {
            if (this.editModel.guideTypeId === 0 || this.editModel.name === '' || this.editFileName == '') {
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

            if(this.crud.update == false){
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
                this.fileUploadType = "image";
                this.editImageData = await BlobService.getImageUrl(data.blobId, data.mime);
            }
            else if(data.mime == 'pdf'){
                this.fileUploadType = "pdf";
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

            let array = file.name.split(".");

            let extension = array.pop();

            if (extension != 'pdf' && extension != 'jpeg' && extension != 'jpg' && extension != 'png') {
                this.fileType = false;
            }
            if (fileInput[0].size > 5048576) {
                this.fileSize = false;
            }

            if (!this.fileType || !this.fileSize)
                return;


            let baseFileInput = (<HTMLInputElement>e.target).files[0];
            let base64String = await this.convertToBase64(baseFileInput);

            this.editModel.fileContent = {
                base64: base64String,
                contentType: extension,
                fileName: fileInput[0].name
            }

            if (extension == 'jpeg' || extension == 'jpg' || extension == 'png') {
                this.fileUploadType = "image";
                var temp = this;
                let reader = new FileReader();
                reader.readAsDataURL(fileInput[0]);
                reader.onload = function () {
                    temp.editImageData = reader.result as string;
                };
            }
            else if(extension == 'pdf'){
                this.fileUploadType = "pdf";
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
                this.detailFileData = await BlobService.getImageUrl(data.blobId, data.mime);
            }
            else if(data.mime == 'pdf'){
                this.detailFileData = await BlobService.getFilePDF(data.blobId);
            }
            this.detail = true;
            this.isBusy = false;
        }

        //Delete
        deleteGuideId;
        deleteIndex;

        async deleteClicked(guideId: number, index: number) {
            this.alertClose();

            if(this.crud.delete == false){
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
                }
                reader.onerror = error => {
                    reject(error);
                }
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
            }
            else if (this.guideName.sortdown == true) {
                this.sortBy = 'guideNameDesc';
            }
            else {
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
            }
            else if (this.guideType.sortdown == true) {
                this.sortBy = 'guideTypeDesc';
            }
            else {
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
            }
            else if (this.createdBy.sortdown == true) {
                this.sortBy = 'createdByDesc';
            }
            else {
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