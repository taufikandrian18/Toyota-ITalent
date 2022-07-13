<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <!--main page-->
        <div class="row" v-if="add === false && edit === false && detail === false">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-md-4"><fa icon="database"></fa> Master Data > <span class="talent_font_red">Module</span></h3>

                <div v-if="success !== ''" class="my-md-2 mt-md-4">
                    <div class="alert alert-success">
                        {{success}}
                        <button type="button" class="close" aria-label="Close" @click="success = ''">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                </div>

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4 class="mb-md-4">Search Module</h4>

                    <!--3 Column Menu-->
                    <div class="row mb-md-4">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker class="v-date-style-report" :masks="{ input: 'DD/MM/YYYY' }" mode="range" :firstDayOfWeek="2"
                                                       v-model="filter.DateFilter"></v-date-picker>
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
                                    <span>Type of Material</span>
                                </div>
                                <div class="col-md-8">
                                    <select class="form-control" v-model="filter.MaterialTypeId">
                                        <option v-for="mateType in materialTypeOption" :value="mateType.materialTypeId">{{mateType.materialTypeName}}</option>
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
                                    <select class="form-control" v-model="filter.ApprovalStatus">
                                        <option v-for="status in statusOption" :value="status.statusId">{{status.statusName}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--3 Column Menu-->
                    <div class="row mb-md-4">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Module Name</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.ModuleName" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Topic</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <input class="form-control" v-model="filter.TopicName" />
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
                                        <button class="btn talent_bg_blue talent_font_white" @click="fetch()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click="reset()">
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
                    <h4>Module List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{grid.modules.length}} of {{grid.totalFilterData}} Entry(s)</a>
                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="addClicked">Add Module</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortModuleName()">Module Name <fa icon="sort" v-if="moduleName.sort == true"></fa><fa icon="sort-up" v-if="moduleName.sortup == true"></fa><fa icon="sort-down" v-if="moduleName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortTopic()">Topic<fa icon="sort" v-if="topic.sort == true"></fa><fa icon="sort-up" v-if="topic.sortup == true"></fa><fa icon="sort-down" v-if="topic.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortTypeOfMaterial()">Type of Material <fa icon="sort" v-if="typeMaterial.sort == true"></fa><fa icon="sort-up" v-if="typeMaterial.sortup == true"></fa><fa icon="sort-down" v-if="typeMaterial.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortApprovalStatus()">Approval Status <fa icon="sort" v-if="approvalStatus.sort == true"></fa><fa icon="sort-up" v-if="approvalStatus.sortup == true"></fa><fa icon="sort-down" v-if="approvalStatus.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="3" v-if="crud.delete || crud.update || crud.read">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="module in grid.modules">
                                    <td>
                                        {{module.moduleName}}
                                    </td>
                                    <td>
                                        {{module.topicName}}
                                    </td>
                                    <td>
                                        {{module.typeMaterialName}}
                                    </td>
                                    <td>
                                        {{module.approvalStatus}}
                                    </td>
                                    <td>
                                        {{module.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{module.updatedAt | dateFormat}}
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.read">
                                        <button type="button" class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="detailClicked(module.moduleId)">View Detail</button>
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.update">
                                        <button type="button" class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="editClicked(module.moduleId)" :disabled="module.approvalStatus === approvalStatusEnum.WAITINGNAME || !crud.update">Edit </button>
                                    </td>
                                    <td class="talent_nopadding_important" v-if="crud.delete">
                                        <button type="button" class="btn btn-block talent_bg_red talent_font_white"
                                                data-backdrop="static" data-keyboard="false"
                                                data-toggle="modal" data-target="#deleteConfirmation"
                                                :disabled="module.approvalStatus === approvalStatusEnum.WAITINGNAME || !crud.delete"
                                                @click="setDelete(module.moduleId, module.moduleName)">
                                            Remove
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--Pagination-->
                    <div class="row justify-content-center">
                        <paginate :currentPage.sync="filter.pageIndex" :total-data="grid.totalFilterData" :limit-data="filter.pageSize" @update:currentPage="fetch()"></paginate>
                    </div>

                </div>
            </div>
        </div>

        <!--Add-->
        <div class="row" v-if="add === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Module > <span class="talent_font_red">Add Module</span></h3>

                <div class="alert alert-danger" v-show="errors.items.length > 0">
                    <button type="button" class="close" aria-label="Close" @click="$validator.errors.clear()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <div v-for="error in errors.all()">{{error}}</div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <h4>Module Information</h4>
                    <div class="row mb-md-4">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Module Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group mb-md-4">
                                        <input class="form-control"
                                               v-model="moduleForm.ModuleName"
                                               name="Module Name"
                                               v-validate="'required'" 
                                               maxlength="255"/>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-12">
                                                    <h5>File Upload</h5>

                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="row justify-content-between">
                                                                    <div class="col-md-4 h-100">
                                                                        <img class="h-100 w-100" :src="imageDataModule.length ? imageDataModule : '/upload-image2.png'" for="customFile" />
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
                                                                                                   accept=".png,.jpg,.jpeg,.pdf"
                                                                                                   v-validate="'required|size:5120'" />
                                                                                            <label class="custom-file-label talent_textshorten " for="customFile">{{imageNameModule.length ? imageNameModule : 'Choose File'}}</label>
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
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label>Module Description</label>
                            <!-- <textarea class="form-control talent-module-overflow" v-model="moduleForm.ModuleDescription" maxlength="1024"></textarea> -->
                            <wysiwyg 
                                class="w-100" 
                                v-model="moduleForm.ModuleDescription" 
                                :options="{
                                    hideModules: {image: true, table: true, removeFormat: true, code: true }
                                }"
                                @change="(e) => moduleForm.ModuleDescription = e"/>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <h4>Material Information</h4>
                    <div class="row mb-md-4">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Type of Material<span class="talent_font_red">*</span></label>
                                    <select class="form-control"
                                            v-model="moduleForm.MaterialTypeId"
                                            name="Type of Material"
                                            v-validate="'required-type'"
                                            @change="emptyMaterialForm">
                                        <option v-for="mateType in materialTypeOption"
                                                :value="mateType.materialTypeId">
                                            {{mateType.materialTypeName}}
                                        </option>
                                    </select>
                                </div>
                                <div class="col-md-12 mt-md-4">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label v-if="moduleForm.MaterialTypeId !== materialTypeEnum.GAME">Upload File</label>
                                            <label v-else>Link</label>
                                            <div class="custom-file" v-if="moduleForm.MaterialTypeId !== materialTypeEnum.GAME">
                                                <input type="file"
                                                       class="custom-file-input"
                                                       ref="materialInput"
                                                       id="customFile"
                                                       @change="materialFileChange"
                                                       name="Material File"
                                                       v-validate="'required'"
                                                       :accept="acceptExtension" />
                                                <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{fileNameMaterial.length ? fileNameMaterial : 'Choose File'}}</label>
                                            </div>
                                            <div class="input-group" v-else>
                                                <input class="form-control"
                                                       v-model="moduleForm.MaterialLink"
                                                       name="Game Link"
                                                       v-validate="'required'" 
                                                       maxlength="1024"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label>Topic<span class="talent_font_red">*</span></label>
                            <multiselect v-model="moduleForm.Topics"
                                         name="Topic"
                                         track-by="topicId"
                                         :multiple="true"
                                         :close-on-select="false"
                                         :clear-on-select="false"
                                         label="topicName"
                                         v-validate="'required'"
                                         :searchable="true"
                                         :options="topicOption">

                            </multiselect>
                        </div>
                    </div>

                    <div>
                        <label>Downloadable:</label>
                        <input type="radio" id="yes" :value="true" v-model="moduleForm.isDownloadable" />
                        <label for="yes">Yes</label>
                        <input type="radio" id="no" :value="false" v-model="moduleForm.isDownloadable" />
                        <label for="no">No</label>
                    </div>

                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="saveClicked">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_blue talent_font_white" @click.prevent="submitClicked">
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
        <div class="row" v-if="edit === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Module > <span class="talent_font_red">Edit Module</span></h3>

                <div class="alert alert-danger" v-show="errors.items.length > 0">
                    <button type="button" class="close" aria-label="Close" @click="$validator.errors.clear()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <div v-for="error in errors.all()">{{error}}</div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <h4>Module Information</h4>
                    <div class="row mb-md-4">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Module Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group mb-md-4">
                                        <input class="form-control"
                                               v-model="moduleForm.ModuleName"
                                               name="Module Name"
                                               v-validate="'required'"
                                               maxlength="255"/>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-12">
                                                    <h5>File Upload</h5>

                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="row justify-content-between">
                                                                    <div class="col-md-4 h-100">
                                                                        <img class="h-100 w-100" :src="imageDataModule.length ? imageDataModule : '/upload-image2.png'" for="customFile" />
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
                                                                                                   accept=".png,.jpg,.jpeg,.pdf"
                                                                                                   v-validate="moduleUpdateForm.moduleId == null ? 'required|size:5120' : ''" />
                                                                                            <label class="custom-file-label talent_textshorten " for="customFile">{{imageNameModule.length ? imageNameModule : 'Choose File'}}</label>
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
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label>Module Description</label>
                            <!-- <textarea class="form-control talent-module-overflow" v-model="moduleForm.ModuleDescription" maxlength="1024"></textarea> -->

                            <wysiwyg 
                                class="w-100" 
                                v-model="moduleForm.ModuleDescription" 
                                :options="{
                                    hideModules: {image: true, table: true, removeFormat: true, code: true }
                                }"
                                @change="(e) => moduleForm.ModuleDescription = e"/>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <h4>Material Information</h4>
                    <div class="row mb-md-4">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Type of Material<span class="talent_font_red">*</span></label>
                                    <select class="form-control"
                                            v-model="moduleForm.MaterialTypeId"
                                            name="Type of Material"
                                            v-validate="'required-type'"
                                            @change="emptyMaterialForm">
                                        <option v-for="mateType in materialTypeOption"
                                                :value="mateType.materialTypeId">
                                            {{mateType.materialTypeName}}
                                        </option>
                                    </select>
                                </div>
                                <div class="col-md-12 mt-md-4">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label v-if="moduleForm.MaterialTypeId !== materialTypeEnum.GAME">Upload File</label>
                                            <label v-else>Link</label>
                                            <div class="custom-file" v-if="moduleForm.MaterialTypeId !== materialTypeEnum.GAME">
                                                <input type="file"
                                                       class="custom-file-input"
                                                       ref="materialInput"
                                                       id="customFile"
                                                       @change="materialFileChange"
                                                       name="Material File"
                                                       v-validate="'required'"
                                                       :accept="acceptExtension" />
                                                <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{fileNameMaterial.length ? fileNameMaterial : 'Choose File'}}</label>
                                            </div>
                                            <div class="input-group" v-else>
                                                <input class="form-control"
                                                       v-model="moduleForm.MaterialLink"
                                                       name="Game Link"
                                                       v-validate="'required'" 
                                                       maxlength="1024"/>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label>Topic<span class="talent_font_red">*</span></label>
                            <multiselect v-model="moduleForm.Topics"
                                         name="Topic"
                                         track-by="topicId"
                                         :multiple="true"
                                         :close-on-select="false"
                                         :clear-on-select="false"
                                         label="topicName"
                                         v-validate="'required'"
                                         :searchable="true"
                                         :options="topicOption">

                            </multiselect>
                        </div>
                    </div>

                    <div>
                        <label>Downloadable:</label>
                        <input type="radio" id="yes" :value="true" v-model="moduleForm.isDownloadable" />
                        <label for="yes">Yes</label>
                        <input type="radio" id="no" :value="false" v-model="moduleForm.isDownloadable" />
                        <label for="no">No</label>
                    </div>

                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="closeClicked">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="saveClicked">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_blue talent_font_white" @click.prevent="submitClicked">
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
        <div class="row" v-if="detail === true">
            <div class="col col-md-12">
                <h3><fa icon="database"></fa> Master Data > Module > <span class="talent_font_red">View Detail Module</span></h3>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <h4>Module Information</h4>
                    <div class="row mb-md-4">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Module Name<span class="talent_font_red">*</span></label>
                                    <div class="input-group mb-md-4">
                                        <input class="form-control"
                                               v-model="moduleForm.ModuleName"
                                               disabled />
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="row justify-content-between">
                                                <div class="col-md-12">
                                                    <h5>File Upload</h5>

                                                    <div class="col-md-12">
                                                        <div class="row">
                                                            <div class="col-md-12">
                                                                <div class="row justify-content-between">
                                                                    <div class="col-md-4 h-100">
                                                                        <img class="h-100 w-100" :src="imageDataModule.length ? imageDataModule : '/upload-image2.png'" for="customFile" />
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
                                                                                                   disabled />
                                                                                            <label class="custom-file-label talent_textshorten " for="customFile">{{imageNameModule.length ? imageNameModule : 'Choose File'}}</label>
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
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label>Module Description</label>
                            <!-- <textarea class="form-control talent-module-overflow" v-model="moduleForm.ModuleDescription" disabled></textarea> -->
                            <wysiwyg 
                                class="w-100" 
                                v-model="moduleForm.ModuleDescription" 
                                :options="{
                                    hideModules: {image: true, table: true, removeFormat: true, code: true }
                                }"
                                @change="(e) => moduleForm.ModuleDescription = e"/>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <h4>Material Information</h4>
                    <div class="row mb-md-4">
                        <div class="col-md-6">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Type of Material<span class="talent_font_red">*</span></label>
                                    <select class="form-control"
                                            v-model="moduleForm.MaterialTypeId"
                                            disabled>
                                        <option v-for="mateType in materialTypeOption"
                                                :value="mateType.materialTypeId">
                                            {{mateType.materialTypeName}}
                                        </option>
                                    </select>
                                </div>
                                <div class="col-md-12 mt-md-4">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6" v-if="moduleForm.MaterialTypeId !== materialTypeEnum.GAME">
                                            <label>Upload File</label>
                                            <div class="custom-file">
                                                <input type="file" class="custom-file-input" id="customFile" @change="materialFileChange" disabled />
                                                <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{fileNameMaterial.length ? fileNameMaterial : 'Choose File'}}</label>
                                            </div>
                                        </div>
                                        <div class="col-md-6" v-else>
                                            <label>Link</label>
                                            <div class="input-group">
                                                <input class="form-control"
                                                       v-model="moduleForm.MaterialLink"
                                                       disabled />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label>Topic<span class="talent_font_red">*</span></label>
                            <multiselect v-model="moduleForm.Topics"
                                         name="Topic"
                                         track-by="topicId"
                                         :multiple="true"
                                         :close-on-select="false"
                                         :clear-on-select="false"
                                         label="topicName"
                                         v-validate="'required'"
                                         :searchable="true"
                                         :options="topicOption"
                                         :disabled="true">

                            </multiselect>
                        </div>
                    </div>

                    <div>
                        <label>Downloadable:</label>
                        <input type="radio" id="yes" :value="true" v-model="moduleForm.isDownloadable" disabled />
                        <label for="yes">Yes</label>
                        <input type="radio" id="no" :value="false" v-model="moduleForm.isDownloadable" disabled />
                        <label for="no">No</label>
                    </div>

                </div>

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow my-md-4">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white" @click="backPage">
                                        Close
                                    </button>
                                    <button v-else class="btn talent_bg_red talent_font_white" @click="closeClicked">
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
                        <h5 class="modal-header" id="deleteConfirmLabel">Delete Confirmation</h5>
                        <h6>Which one you want to delete?</h6>
                    </div>
                    <div class="modal-body">
                        <span>Module: </span><checkbox class="talent-checkbox-lineheight" v-model="isDeleteModule" @change="checkAllTopic()">{{toBeDeletedName}}</checkbox>
                        <hr />
                        <div>Topic:</div>
                        <div v-for="topic in toBeDeletedTopic">
                            <checkbox class="talent-checkbox-lineheight"
                                      :id="topic.topicId.toString()"
                                      :value="topic.topicId"
                                      v-model="toBeDeletedTopicIds"
                                      @change="checkUncheck()">{{topic.topicName}}</checkbox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="col-md-12 d-flex justify-content-around">
                                <button type="button" class="btn btn-danger" data-dismiss="modal"
                                        @click.prevent="deleteModule()">
                                    Delete
                                </button>
                                <button type="button" class="btn btn-primary" data-dismiss="modal" @click="emptyDelete()">Close</button>
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
    import { ModulesService, TopicDropdownModel, MaterialTypeDropdownModel, ApprovalStatusDropdownModel, DropdownService, ModuleCreateModel, ModuleGridModel, DeleteModuleModel, ModuleUpdateModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService'
    import { IModuleFilter, IModuleFormModel } from '../../models/IModuleModels';
    import { MaterialTypeEnum } from '../../enum/MaterialTypeEnum';
    import { ApprovalStatusEnum } from '../../enum/ApprovalStatusEnum';
    import { PageEnums } from '../../enum/PageEnums';
    import Message from '../../class/Message';

    @Component({
        created: async function (this: Module) {
            this.isLoading = true;
            await this.getAccess();
            this.validateExtend();
            await this.fillOption();
            await this.fetch()
            if (this.fromOutside === true) {
                this.detailClicked(this.moduleId);
            }
            this.isLoading = false;
        },

        props: ['moduleId', 'fromOutside']
    })

    export default class Module extends Vue {
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Module);
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        modMan: ModulesService = new ModulesService();
        dropMan: DropdownService = new DropdownService();

        moduleId: number;
        fromOutside: boolean;

        isLoading: boolean = false;

        id: number = MaterialTypeEnum.VIDEO

        add: boolean = false;
        edit: boolean = false;
        detail: boolean = false;

        materialTypeEnum = MaterialTypeEnum;
        approvalStatusEnum = ApprovalStatusEnum;

        /*
         * contain filter form to filter data
         * */
        filter: IModuleFilter = {
            sortBy: '',
            pageIndex: 1,
            pageSize: 10,

            DateFilter: {
                start: null,
                end: null
            },
            ModuleName: '',
            TopicName: '',
            ApprovalStatus: 0,
            MaterialTypeId: 0
        }

        topicOption: TopicDropdownModel[] = [];
        materialTypeOption: MaterialTypeDropdownModel[] = [];
        statusOption: ApprovalStatusDropdownModel[] = [];

        moduleForm: IModuleFormModel = {
            ModuleName: '',
            ModuleDescription: '',
            ModuleFile: {
                base64: '',
                contentType: '',
                fileName: ''
            },
            MaterialTypeId: 0,
            MaterialFile: {
                base64: '',
                contentType: '',
                fileName: ''
            },
            MaterialLink: '',
            isDownloadable: false,
            Topics: []
        }

        moduleInsertForm: ModuleCreateModel = {
            moduleName: '',
            moduleDesc: '',
            materialTypeId: 0,
            link: '',
            downloadable: null,
            topicId: [],
            approvalStatusId: null,
            moduleFileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            },
            materialFileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        moduleUpdateForm: ModuleUpdateModel = {
            moduleId: 0,
            moduleName: '',
            moduleDesc: '',
            materialTypeId: 0,
            link: '',
            downloadable: null,
            topicId: [],
            approvalStatusId: null,
            materialFileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            },
            moduleFileContent: {
                base64: '',
                contentType: '',
                fileName: ''
            }
        }

        grid: ModuleGridModel = {
            modules: [],
            totalFilterData: 0
        }

        imageDataModule: any = [];
        imageNameModule: string = '';
        fileNameMaterial: string = '';
        isImageModuleChanged: boolean = false;
        isFileMaterialChanged: boolean = false;
        isMaterialTypeChanged: boolean = false;
        acceptExtension: string = '';

        toBeDeletedId: number = 0;
        toBeDeletedName: string = '';
        toBeDeletedTopic: TopicDropdownModel[] = [];
        isDeleteModule: boolean = false;
        toBeDeletedTopicIds: number[] = [];

        success: string = '';

        async fetch() {
            this.isLoading = true;
            this.grid = await this.modMan.getAllModule(this.filter.ModuleName,
                this.filter.MaterialTypeId,
                this.filter.TopicName,
                this.filter.ApprovalStatus,
                this.filter.DateFilter.start,
                this.filter.DateFilter.end,
                this.filter.sortBy,
                this.filter.pageIndex,
                this.filter.pageSize);

            this.isLoading = false;
        }

        async fillOption() {
            this.isLoading = true;
            this.topicOption = await this.dropMan.getTopicDropdown();
            this.materialTypeOption = await this.dropMan.getMaterialTypeDropdown();
            this.statusOption = await this.dropMan.getApprovalStatusDropdown();
            this.isLoading = false;
        }

        async reset() {
            this.filter.DateFilter = {
                start: null,
                end: null
            };

            this.filter.ModuleName = '';
            this.filter.TopicName = '';
            this.filter.ApprovalStatus = 0;
            this.filter.MaterialTypeId = 0;
            this.ResetSort('');
            await this.fetch();
        }

        async saveClicked() {
            this.isLoading = true;
            this.$validator.resume();
            this.$validator.errors.clear();

            let valid: boolean = null;
            let materialUploadValid: string = 'Material File';
            if (this.moduleForm.MaterialTypeId === this.materialTypeEnum.GAME) {
                materialUploadValid = 'Game Link';
            }

            if (this.add === true) {
                valid = await this.$validator.validateAll(['Module Name', 'Type of Material', 'Topic', 'File Upload', materialUploadValid]);
            }
            else {
                let updateValid: string[] = ['Module Name', 'Type of Material', 'Topic', 'File Upload'];
                if (this.isFileMaterialChanged || this.isMaterialTypeChanged) {
                    updateValid.push(materialUploadValid);
                }
                valid = await this.$validator.validateAll(updateValid);
            }

            if (!valid) {
                this.isLoading = false;
                return;
            }

            this.$validator.reset();
            this.isLoading = false;

            if (this.add === true) {
                this.moduleInsertForm.approvalStatusId = this.approvalStatusEnum.DRAFT;
                await this.insertModule();
            }

            else if (this.edit === true) {
                this.moduleUpdateForm.approvalStatusId = this.approvalStatusEnum.DRAFT;
                await this.updateModule();
            }

            return;
        }

        async submitClicked() {
            await this.$validator.resume();
            await this.$validator.errors.clear();

            this.isLoading = true;

            let valid: boolean = null;
            let materialUploadValid: string = 'Material File';
            if (this.moduleForm.MaterialTypeId === this.materialTypeEnum.GAME) {
                materialUploadValid = 'Game Link';
            }

            if (this.add === true) {
                valid = await this.$validator.validateAll(['Module Name', 'Type of Material', 'Topic', 'File Upload', materialUploadValid]);
            }

            else {
                let updateValid: string[] = ['Module Name', 'Type of Material', 'Topic', 'File Upload'];
                if (this.isFileMaterialChanged || this.isMaterialTypeChanged) {
                    updateValid.push(materialUploadValid);
                }
                valid = await this.$validator.validateAll(updateValid);
            }

            if (!valid) {
                this.isLoading = false;
                return;
            }

            this.$validator.reset();
            this.isLoading = false;

            if (this.add === true) {
                this.moduleInsertForm.approvalStatusId = this.approvalStatusEnum.WAITING;
                await this.insertModule();
            }

            else if (this.edit === true) {
                this.moduleUpdateForm.approvalStatusId = this.approvalStatusEnum.WAITING;
                await this.updateModule();
            }

            return;
        }

        async insertModule() {
            this.isLoading = true;

            this.moduleInsertForm.moduleName = this.moduleForm.ModuleName;
            this.moduleInsertForm.moduleDesc = this.moduleForm.ModuleDescription;
            this.moduleInsertForm.moduleFileContent = this.moduleForm.ModuleFile;
            this.moduleInsertForm.materialTypeId = this.moduleForm.MaterialTypeId;
            this.moduleInsertForm.materialFileContent = this.moduleForm.MaterialFile;
            this.moduleInsertForm.downloadable = this.moduleForm.isDownloadable;
            this.moduleInsertForm.link = this.moduleForm.MaterialLink;

            for (let topic of this.moduleForm.Topics) {
                this.moduleInsertForm.topicId.push(topic.topicId);
            }

            let success = await this.modMan.insertModule(this.moduleInsertForm);
            if (!success) {
                this.isLoading = false;
                this.$validator.errors.add({
                    field: 'Module Name',
                    msg: 'Module Name has existed, please use another name'
                });
                return;
            }

            this.success = Message.SuccessInsertMessage;

            this.isFileMaterialChanged = false;
            this.isImageModuleChanged = false;
            this.isMaterialTypeChanged = false;

            this.reset();
            await this.fetch();
            this.closeClicked();
            this.isLoading = false;
        }

        async updateModule() {
            this.isLoading = true;
            if (this.isImageModuleChanged) {
                this.moduleUpdateForm.moduleFileContent = this.moduleForm.ModuleFile;
            }

            if (this.isFileMaterialChanged) {
                this.moduleUpdateForm.materialFileContent = this.moduleForm.MaterialFile;
            }

            this.moduleUpdateForm.moduleName = this.moduleForm.ModuleName;
            this.moduleUpdateForm.moduleDesc = this.moduleForm.ModuleDescription;
            this.moduleUpdateForm.materialTypeId = this.moduleForm.MaterialTypeId;
            this.moduleUpdateForm.downloadable = this.moduleForm.isDownloadable;
            this.moduleUpdateForm.link = this.moduleForm.MaterialLink;

            for (let topic of this.moduleForm.Topics) {
                this.moduleUpdateForm.topicId.push(topic.topicId);
            }

            let success = await this.modMan.updateModuleById(this.moduleUpdateForm);
            if (!success) {
                this.isLoading = false;
                this.$validator.errors.add({
                    field: 'Module Name',
                    msg: 'Module Name has existed, please use another name'
                });
                return;
            }

            this.isFileMaterialChanged = false;
            this.isImageModuleChanged = false;
            this.isMaterialTypeChanged = false;

            this.success = Message.SuccessEditMessage;
            this.reset();
            await this.fetch();
            this.closeClicked();
            this.isLoading = false;
        }

        resetForm() {
            this.$validator.pause();

            this.moduleForm = {
                ModuleName: '',
                ModuleDescription: '',
                MaterialTypeId: 0,
                MaterialLink: '',
                isDownloadable: false,
                Topics: [],
                ModuleFile: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                },
                MaterialFile: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                }
            }

            this.moduleInsertForm = {
                moduleName: '',
                moduleDesc: '',
                materialTypeId: 0,
                link: '',
                downloadable: null,
                topicId: [],
                approvalStatusId: null,
                moduleFileContent: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                },
                materialFileContent: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                }
            }

            this.moduleUpdateForm = {
                moduleId: 0,
                moduleName: '',
                moduleDesc: '',
                materialTypeId: 0,
                link: '',
                downloadable: null,
                topicId: [],
                approvalStatusId: null,
                moduleFileContent: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                },
                materialFileContent: {
                    base64: '',
                    contentType: '',
                    fileName: ''
                }
            }

            this.imageDataModule = [];
            this.fileNameMaterial = '';
            this.imageNameModule = '';
            this.acceptExtension = '';
        }

        async emptyMaterialForm() {
            switch (this.moduleForm.MaterialTypeId) {
                case this.materialTypeEnum.PODCAST:
                    this.acceptExtension = '.mp3';
                    break;
                case this.materialTypeEnum.VIDEO:
                    this.acceptExtension = '.mp4';
                    break;
                case this.materialTypeEnum.JOURNAL:
                    this.acceptExtension = '.pdf'
                    break;
                default:
                    this.acceptExtension = '';
                    break;
            }

            let input = this.$refs.materialInput;
            (<any>input).type = 'text';
            (<any>input).type = 'file';

            this.fileNameMaterial = '';
            this.moduleForm.MaterialLink = '';
            this.isMaterialTypeChanged = true;
            await this.$validator.reset();
        }

        addClicked() {
            if (!this.crud.create) {
                return
            }

            this.add = true;
            this.edit = false;
            this.detail = false;
        }

        async editClicked(moduleId: number) {
            if (!this.crud.update) {
                return
            }
            this.isLoading = true;
            let module = await this.modMan.getModuleById(moduleId);

            this.moduleForm.ModuleName = module.moduleName;
            this.moduleForm.ModuleDescription = module.moduleDescription;
            this.moduleForm.MaterialTypeId = module.materialType.materialTypeId;
            this.moduleForm.MaterialLink = module.materialLink;
            this.moduleForm.isDownloadable = module.isDownloadable;
            this.moduleForm.Topics = module.topics;

            this.imageNameModule = module.moduleBlobName;
            this.fileNameMaterial = module.materialBlobName === null ? '' : module.materialBlobName;
            this.imageDataModule = await BlobService.getImageUrl(module.moduleBlobId, module.moduleBlobMIME);

            this.moduleUpdateForm.moduleId = module.moduleId;

            switch (this.moduleForm.MaterialTypeId) {
                case this.materialTypeEnum.PODCAST:
                    this.acceptExtension = '.mp3';
                    break;
                case this.materialTypeEnum.VIDEO:
                    this.acceptExtension = '.mp4';
                    break;
                case this.materialTypeEnum.JOURNAL:
                    this.acceptExtension = '.pdf'
                    break;
                default:
                    this.acceptExtension = '';
                    break;
            }

            this.add = false;
            this.edit = true;
            this.detail = false;
            this.isLoading = false;
        }

        async detailClicked(moduleId: number) {
            if (!this.crud.read) {
                return
            }
            this.isLoading = true;
            let module = await this.modMan.getModuleById(moduleId);

            this.moduleForm.ModuleName = module.moduleName;
            this.moduleForm.ModuleDescription = module.moduleDescription;
            this.moduleForm.MaterialTypeId = module.materialType.materialTypeId;
            this.moduleForm.MaterialLink = module.materialLink;
            this.moduleForm.isDownloadable = module.isDownloadable;
            this.moduleForm.Topics = module.topics;

            this.imageNameModule = module.moduleBlobName;
            this.fileNameMaterial = module.materialBlobName === null ? '' : module.materialBlobName;
            this.imageDataModule = await BlobService.getImageUrl(module.moduleBlobId, module.moduleBlobMIME);

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

        async setDelete(moduleId: number, moduleName: string) {

            this.isLoading = true;
            this.toBeDeletedId = moduleId;
            this.toBeDeletedName = moduleName;
            this.toBeDeletedTopic = await this.modMan.getModuleById(moduleId).then(X => X.topics);
            this.isLoading = false;
        }

        checkAllTopic() {
            if (this.isDeleteModule === true) {
                for (let topic of this.toBeDeletedTopic) {
                    var hasExist = this.toBeDeletedTopicIds.includes(topic.topicId);

                    if (hasExist === false) {
                        this.toBeDeletedTopicIds.push(topic.topicId);
                    }
                }
            }

            else {
                this.toBeDeletedTopicIds = [];
            }

        }

        checkUncheck() {
            if (this.toBeDeletedTopicIds.length < this.toBeDeletedTopic.length) {
                this.isDeleteModule = false;
                return;
            }

            this.isDeleteModule = true;
        }

        emptyDelete() {
            this.toBeDeletedId = 0;
            this.toBeDeletedName = '';
            this.toBeDeletedTopic = [];
            this.isDeleteModule = false;
            this.toBeDeletedTopicIds = [];
        }

        async deleteModule() {
            if (!this.crud.delete) {
                return
            }

            let deleteModel: DeleteModuleModel = {
                moduleId: this.toBeDeletedId,
                isDeleteModule: this.isDeleteModule,
                topicIds: this.toBeDeletedTopicIds
            }

            await this.modMan.deleteModule(deleteModel);
            this.emptyDelete();

            this.success = Message.RemoveMessage;
            await this.fetch();
        }

        //Variable untuk sorting
        moduleName = new Sort();
        topic = new Sort();
        typeMaterial = new Sort();
        approvalStatus = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortModuleName
        async ClickSortModuleName() {
            this.ResetSort('moduleName');
            //Sorting
            this.moduleName.sorting();
            //Function Sorting

            if (this.moduleName.sortup === true) {
                this.filter.sortBy = 'ModuleName';
            }

            else if (this.moduleName.sortdown === true) {
                this.filter.sortBy = 'ModuleNameDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();

            return;
        }

        //ClickSortTopic
        async ClickSortTopic() {
            this.ResetSort('topic');
            //Sorting
            this.topic.sorting();
            //Function Sorting

            if (this.topic.sortup === true) {
                this.filter.sortBy = 'TopicName';
            }

            else if (this.topic.sortdown === true) {
                this.filter.sortBy = 'TopicNameDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();

            return;
        }

        //ClickSortTypeOfMaterial
        async ClickSortTypeOfMaterial() {
            this.ResetSort('typeMaterial');
            //Sorting
            this.typeMaterial.sorting();
            //Function Sorting

            if (this.typeMaterial.sortup === true) {
                this.filter.sortBy = 'MaterialType';
            }

            else if (this.typeMaterial.sortdown === true) {
                this.filter.sortBy = 'MaterialTypeDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();

            return;
        }

        //ClickSortApprovalStatus
        async ClickSortApprovalStatus() {
            this.ResetSort('approvalStatus');
            //Sorting
            this.approvalStatus.sorting();
            //Function Sorting

            if (this.approvalStatus.sortup === true) {
                this.filter.sortBy = 'ApprovalStatus';
            }

            else if (this.approvalStatus.sortdown === true) {
                this.filter.sortBy = 'ApprovalStatusDesc';
            }

            else {
                this.filter.sortBy = '';
            }

            await this.fetch();

            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting

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

            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting

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

            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'moduleName':
                    this.topic.reset();
                    this.typeMaterial.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'topic':
                    this.moduleName.reset();
                    this.typeMaterial.reset();
                    this.createdDate.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'typeMaterial':
                    this.moduleName.reset();
                    this.topic.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'approvalStatus':
                    this.moduleName.reset();
                    this.topic.reset();
                    this.typeMaterial.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.moduleName.reset();
                    this.topic.reset();
                    this.typeMaterial.reset();
                    this.approvalStatus.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.moduleName.reset();
                    this.topic.reset();
                    this.typeMaterial.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    return;
                default:
                    this.filter.sortBy = '';
                    this.moduleName.reset();
                    this.topic.reset();
                    this.typeMaterial.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
            }
        }

        async handler($event) {
            if ($event.target.files.length === 0) {
                return;
            }

            this.isImageModuleChanged = true;
            this.loadFile($event);
            await this.fileChange($event);
        }

        async fileChange(e) {
            let baseFileInput = (<HTMLInputElement>e.target).files[0];
            let base64String = await this.convertToBase64(baseFileInput);
            let extension = baseFileInput.name.split(".").pop();

            this.moduleForm.ModuleFile.base64 = base64String;
            this.moduleForm.ModuleFile.contentType = extension;
            this.moduleForm.ModuleFile.fileName = baseFileInput.name;
        }

        loadFile(e) {
            var reader = new FileReader();
            reader.onload = (e: Event) => {
                this.imageDataModule = (<FileReader>e.target).result;
            }
            reader.readAsDataURL(e.target.files[0]);
            this.imageNameModule = e.target.files[0].name;
        }

        async materialFileChange(e) {
            this.isFileMaterialChanged = true;

            let baseFileInput = (<HTMLInputElement>e.target).files[0];
            let base64String = await this.convertToBase64(baseFileInput);
            let extension = baseFileInput.name.split(".").pop();

            this.moduleForm.MaterialFile.base64 = base64String;
            this.moduleForm.MaterialFile.contentType = extension;
            this.moduleForm.MaterialFile.fileName = baseFileInput.name;

            this.fileNameMaterial = e.target.files[0].name;
        }

        validateExtend() {
            this.$validator.extend('required-type', {
                validate(value: number) {
                    if (value === 0) {
                        return false;
                    }

                    return true;
                },
                getMessage(field) {
                    return 'The ' + field + ' field is required'
                }
            });
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

        backPage() {
            window.history.back();
        }
    }
</script>