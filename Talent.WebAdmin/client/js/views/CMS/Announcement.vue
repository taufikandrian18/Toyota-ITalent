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
                <h3><fa icon="database"></fa> CMS > <span class="talent_font_red">Announcement</span></h3>
                <br />

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Announcement List</h4>
                    <div class="col-md-12 talent_magin_small mt-4">
                        <div class="row align-items-center row justify-content-end">
                            <!-- <a>Showing {{announcements.data.length}} of {{guides.totalItem}} Entry(s)</a> -->
                            <!-- Button Delete -->
                            <!-- <div class="col-auto"> -->
                                <button v-if="announcements.data" class="btn btn talent_bg_red talent_roundborder talent_font_white mr-2" 
                                data-toggle="modal"
                                data-target="#modalDeleteBulk"
                                :disabled="announcements.data.filter(v => v.isSelected).length == 0">
                                Delete Selected
                                </button>
                            <!-- </div> -->
                            <!-- Button Submit -->
                            <!-- <div class="col-auto"> -->
                                <button class="btn btn talent_bg_blue talent_roundborder talent_font_white mr-2" 
                                @click="handleUpdateBulk"
                                v-if="announcements.data" 
                                :disabled="announcements.data.filter(v => v.isSelected).length == 0">
                                Submit Selected
                                </button>
                            <!-- </div> -->
                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="addClicked">
                                Add Announcement
                            </button>
                        </div>
                    </div>

                    <div class="talent_overflowx mt-4">
                        <table class="table table-bordered table-responsive-md talent_table_padding">
                            <thead style="white-space: nowrap;">
                                <tr>
                                    <th scope="col" class="text-center">
                                        <input type="checkbox" @change="handleSelectedAll">
                                    </th>
                                    <th scope="col">
                                        <a href="#" style="color: white; white-space: nowrap;" class="talent_sort">Title</a>
                                    </th>
                                    <th>
                                        <a href="#" style="color: white; white-space: nowrap;" class="talent_sort">Created By</a>
                                    </th>
                                    <th>
                                        <a href="#" style="color: white; white-space: nowrap;" class="talent_sort">Status</a>
                                    </th>
                                    <th>
                                        <a href="#" style="color: white; white-space: nowrap;" class="talent_sort">Created Date</a>
                                    </th>
                                    <th>
                                        <a href="#" style="color: white; white-space: nowrap;" class="talent_sort">Updated Date</a>
                                    </th>
                                    <th colspan="3" class="text-center">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="g in announcements.data" :key="g.announcementID">
                                    <th class="text-center">
                                        <input type="checkbox" :checked="g.isSelected" @change="handleSelected(g)">
                                    </th>
                                    <td>
                                        {{g.title}}
                                    </td>
                                    <td>
                                        {{g.createdBy}}
                                    </td>
                                    <td class="text-center">
                                        <div class="custom-control custom-switch">
                                        <input type="checkbox" class="custom-control-input" :id="`customSwitch-${g.announcementID}`" @change="handleChangeStatusAnnouncement(g, $event)" :checked="g.status">
                                        <label class="custom-control-label" :for="`customSwitch-${g.announcementID}`"></label>
                                        </div>
                                    </td>
                                    <td>
                                        {{convertDateTime(g.createdAt)}}
                                    </td>
                                    <td>
                                        {{convertDateTime(g.updatedAt)}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="handleDetailClicked(g)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="handleEditClicked(g)">Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click.prevent="handleDeleteClicked(g)">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--Pagination-->
                    <!-- <div class="col-md-12 d-flex justify-content-center">
                        <paginate :currentPage.sync="currentPage" :total-data=guides.totalItem :limit-data=itemPerPage @update:currentPage="fetch()"></paginate>
                    </div> -->

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
                                        <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="handleDeleteData()">Delete</button>
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
                <h3><fa icon="database"></fa> CMS > Announcement > <span class="talent_font_red">Add Announcement</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Add Announcement</h4>

                    <div v-if="fileType === false || fileSize === false" class="alert alert-danger">
                        <div v-if="fileSize === false">Maximum File Size is 5 MB</div>
                        <div v-if="fileType === false">Please input a pdf/jpg/png/jpeg/mp4 file</div>
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
                                    <label>Title <span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="formData.title" maxlength="1024" />
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label>Description <span class="talent_font_red">*</span></label>
                                    <wysiwyg 
                                        class="w-100" 
                                        v-model="formData.description" 
                                        :options="{
                                            hideModules: {image: true, table: true, removeFormat: true, code: true }
                                        }"/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
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
                                                        <input type="file" class="custom-file-input" id="customFile" @change="handleChangeAddFile" accept="application/pdf,image/jpeg, image/jpg, image/png, video/mp4"/>
                                                        <label class="custom-file-label talent_textshorten" for="customFile">{{image.base64 == '' ? 'Choose File' : image.fileName}}</label>
                                                    </div>
                                                </div>
                                                <div class="col-md-6">
                                                    *Recommended File: PDF, JPEG, JPG, PNG, MP4
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
                        <img v-if="fileUploadType == 'image'" class="h-100 w-100" :src="image.data ? image.data : '/upload-image2.png'" for="customFile" />
                        <video v-if="fileUploadType == 'video' " id="myVideo" class="w-100 video" controls :src="image.data ? image.data : ''"></video>
                        <object v-if="fileUploadType === 'pdf'" class="h-100 w-100 min-heigth-full" :data="image.data" for="customFile" />
                    </div>
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-center">
                                <div class="w-100">
                                    <ol>
                                        <li v-for="err in addErrors" :key="err.id" class="text-danger">
                                            {{ err.message}}
                                        </li>
                                    </ol>
                                </div>
                                <div class="w-100 d-flex justify-content-end">
                                    <button class="btn talent_bg_red talent_font_white mr-2" @click.prevent="closeClicked">Close</button>
                                    <button class="btn talent_bg_lightgreen talent_font_white mr-2" @click.prevent="handleAdd(false)" :disabled="formData.description == '' || formData.title == '' || image.base64 == '' ? 'disabled' : false">Save</button>
                                    <button class="btn talent_bg_darkblue talent_font_white" @click.prevent="handleAdd(true)" :disabled="formData.description == '' || formData.title == '' || image.base64 == '' ? 'disabled' : false">Submit</button>
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
                <h3><fa icon="database"></fa>CMS > Announcement > <span class="talent_font_red">Edit Announcement</span></h3>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Edit Announcement</h4>

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
                                        <input class="form-control" v-model="selectedData.title" maxlength="1024" />
                                    </div>
                                    <br />
                                    <!-- <label>Type of Guide<span class="talent_font_red">*</span></label>
                                    <select class="form-control" v-model="editModel.guideTypeId">
                                        <option v-for="t in guideTypes.listGuideTypeModel" :value="t.guideTypeId">{{t.name}}</option>
                                    </select> -->
                                </div>
                                <div class="col-md-12">
                                    <label>Description</label>
                                    <!-- <textarea class="form-control" style="height:130px;overflow-y:scroll;" v-model="selectedData.description" maxlength="1024"></textarea> -->
                                    <wysiwyg 
                                        class="w-100" 
                                        v-model="selectedData.description" 
                                        :options="{
                                            hideModules: {image: true, table: true, removeFormat: true, code: true }
                                        }"/>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
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
                                                        <input type="file" class="custom-file-input" id="customFile" @change="onEditFileChange" accept="application/pdf,image/jpeg, image/jpg, image/png, video/mp4"/>
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
                        <img v-if="['jpg', 'png', 'jpeg'].includes(selectedData.announcementFileType)" class="h-100 w-100" :src="editFileData != null ? editFileData : selectedData.announcementFileID" for="customFile" />
                        <video v-if="selectedData.announcementFileType == 'mp4' " id="myVideo" class="w-100 video" controls :src="editFileData != null ? editFileData : selectedData.announcementFileID"></video>
                        <object v-if="selectedData.announcementFileType == 'pdf'" class="h-100 w-100 min-heigth-full" :data="editFileData != null ? editFileData : selectedData.announcementFileID" for="customFile" />
                    </div>
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-center">
                                <div class="w-100">
                                    <ol>
                                        <li v-for="err in editErrors" :key="err.id" class="text-danger">
                                            {{ err.message}}
                                        </li>
                                    </ol>
                                </div>
                                <div class="d-flex justify-content-end">
                                    <button class="btn talent_bg_red talent_font_white mr-2" @click.prevent="closeClicked">Close</button>
                                    <button class="btn talent_bg_lightgreen talent_font_white mr-2" @click.prevent="handleEdit(false)" :disabled="selectedData.description == '' || selectedData.title == '' || (image.base64 == '' && selectedData.announcementFileID == '') ? 'disabled' : false">Save</button>
                                    <button class="btn talent_bg_darkblue talent_font_white" @click.prevent="handleEdit(true)" :disabled="selectedData.description == '' || selectedData.title == '' || (image.base64 == '' && selectedData.announcementFileID == '') ? 'disabled' : false">Submit</button>
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
                <h3><fa icon="database"></fa> CMS > Announcement > <span class="talent_font_red">View Detail Announcement</span></h3>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>View Detail Announcement</h4>
                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-12">
                                    <label>Title<span class="talent_font_red">*</span></label>
                                    <div class="input-group">
                                        <input class="form-control" v-model="selectedData.title" disabled />
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label>
                                        Description <span class="talent_font_red">*</span>
                                    </label>
                                    <!-- <textarea class="form-control" style="height:130px;overflow-y:scroll;" v-model="selectedData.description" disabled></textarea> -->
                                    <div class="mt-2" v-html="selectedData.description">

                                    </div>
                                    <!-- <wysiwyg 
                                        class="w-100" 
                                        v-model="selectedData.description" 
                                        :options="{
                                            hideModules: {image: true, table: true, removeFormat: true, code: true }
                                        }"
                                        :disabled="true"
                                    /> -->
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
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
                                                        <!-- <label class="custom-file-label talent_textshorten" for="customFile">{{detailFileName.length ? detailFileName : 'Choose File'}}</label> -->
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
                        <object v-if="selectedData.announcementFileType == 'pdf'"  :data="selectedData.announcementFileID" class="h-100 w-100 min-heigth-full"  for="customFile" type="application/pdf">
                        </object>
                        <img v-if="['png', 'jpg', 'jpeg'].includes(selectedData.announcementFileType)" class="h-100 w-100" :src="selectedData.announcementFileID ? selectedData.announcementFileID : '/upload-image2.png'" for="customFile" />
                        <video v-if="selectedData.announcementFileType == 'mp4' " id="myVideo" class="w-100 video" controls :src="selectedData.announcementFileID ? selectedData.announcementFileID : ''"></video>
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

        <modal-delete
        name="modalDeleteBulk"
        @delete="handleDeleteBulk"
        />
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { Watch } from 'vue-property-decorator'
    import { Sort } from '../../class/Sort';
    import { GuideService, GuideTypeService, GuideFormModel, GuideTypeViewModel, GuideJoinViewModel, ApprovalStatusService, FileContent, UserPrivilegeSettingsService, UserAccessCRUD, AnnouncementService, BlobFileService } from '../../services/NSwagService'
    import { BlobService } from '../../services/BlobService';
    import { isNullOrUndefined } from 'util';
    import { PageEnums } from '../../enum/PageEnums';
import ModalDelete from '../../components/shared/ModalDelete.vue';
import Axios from 'axios';
import $ from 'jquery'

    @Component({
  components: { ModalDelete },
        created: async function (this: Guide) {
            this.isBusy = true;
            await this.initDropdownData();
            await this.getAccess();
            if (this.fromOutside === true) {
                await this.detailClicked(this.guideId);
            }
            await this.fetch();
        },
        mounted() {
            console.log($('.editr--toolbar .dashboard label'))
            $('.editr--toolbar .dashboard label').click(function (event) {$(event.target).focus()});
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

        announcementService: AnnouncementService = new AnnouncementService();
        FileService: BlobFileService = new BlobFileService();
        announcements: any = []

        
        public get addErrors() : any {
            const errs = []
            if (this.formData.title == '') {
                errs.push({
                    id: 'title',
                    message: 'Please insert title.'
                })
            }
            if (this.formData.description == '') {
                errs.push({
                    id: 'description',
                    message: 'Please insert description.'
                })
            }
            if (this.image.data == null || this.image.data.length == 0) {
                errs.push({
                    id: 'image',
                    message: 'Please insert file.'
                })
            }
            return errs
        }

        public get editErrors() : any {
            const errs = []
            // if(this.selectedData.title) {
                if (this.selectedData.title == '') {
                    errs.push({
                        id: 'title',
                        message: 'Please insert title.'
                    })
                }
                if (this.selectedData.description == '') {
                    errs.push({
                        id: 'description',
                        message: 'Please insert description.'
                    })
                }
                if (this.image.base64 == '' && this.selectedData.announcementFileID == '') {
                    errs.push({
                        id: 'image',
                        message: 'Please insert file.'
                    })
                }
            // }
            
            return errs
        }
        

        @Watch('add')
        watchAdd () {
            if(!this.add) {
                this.formData = {
                    title: '',
                    description: ''
                }
                this.image = {
                    base64: '',
                    fileName: '',
                    contentType: '',
                    data: ''
                }
            }
        }

        async handleChangeStatusAnnouncement(data, e) {
            this.isBusy = true
            try {
                await this.announcementService.updateAnnouncement({
                    announcementID: data.announcementID,
                    status: e.target.checked,
                })
                this.fetch()
            } catch (err) {
                this.isBusy = false
                console.log(err)
            }
        }

        async handleDeleteBulk () {
            this.isBusy = true
            try {
                const promises = this.announcements.data.filter(v => v.isSelected).map(v => this.announcementService.deleteAnnouncement(v.announcementID))
                await Promise.all(promises)
                this.fetch()
            } catch (err) {
                this.isBusy = false
                console.log(err)
            }
        }

        async handleUpdateBulk () {
            this.isBusy = true
            try {
                const promises = this.announcements.data.filter(v => v.isSelected).map(v => this.announcementService.updateAnnouncement({
                    announcementID: v.announcementID,
                    status: true,
                }))
                await Promise.all(promises)
                this.fetch()
            } catch (err) {
                this.isBusy = false
                console.log(err)
            }
        }

        async fetch() {
            this.isBusy = true;
            // this.guides = await this.guideMan.getAllJoinGuides(this.filterDate.start, this.filterDate.end, this.filterGuideName, this.filterGuideTypeName, this.filterCreatedBy, this.filterApprovalStatus, this.sortBy, this.currentPage);
            const res = await this.announcementService.getAnnouncements()
            this.announcements = {...res, data: res.data.map(k => ({...k, isSelected: false}))}
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



        handleSelected(data) {
            this.announcements.data = this.announcements.data.map(v => ({...v, isSelected: data === v ? !v.isSelected: v.isSelected}))
        }

        handleSelectedAll(e) {
            this.announcements.data = this.announcements.data.map(v => ({...v, isSelected: e.target.checked}))
        }

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
                if(extension === 'mp4') {
                    this.fileUploadType = 'video';    
                } else {
                    this.fileUploadType = 'image';
                }
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
            } else if (data.mime == 'mp4') {
                this.fileUploadType = "video";
                this.editImageData = await BlobService.getImageUrl(data.blobId, data.mime);
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

            if (extension != 'pdf' && extension != 'jpeg' && extension != 'jpg' && extension != 'png' && extension != 'mp4') {
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

            this.selectedData.announcementFileType = extension

            if (extension == 'jpeg' || extension == 'jpg' || extension == 'png' || extension == 'mp4') {
                if(extension == 'mp4') {
                    this.fileUploadType = "video";
                } else {
                    this.fileUploadType = "image";
                }
                var temp = this;
                let reader = new FileReader();
                reader.readAsDataURL(fileInput[0]);
                reader.onload = function () {
                    temp.editFileData = reader.result as string;
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

        async handleDeleteData() {
            this.isBusy = true;
            await this.announcementService.deleteAnnouncement(this.selectedData.announcementID);
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

        // New!!!!!
        selectedData: any = {}
        formData: any = {
            title: '',
            description: ''
        }
        image: any = {
            base64: '',
            fileName: '',
            contentType: '',
            data: ''
        }

        // API
        async apiInsertAnnouncement (data, image, status) {
            this.isBusy = true
            try {
                // insert image
                const resImage = await this.FileService.insertBlobFile({
                    base64: image.base64,
                    fileName: image.fileName,
                    contentType: image.contentType
                })

                // insert landing page
                const res = await this.announcementService.insertAnnouncement({
                    title: data.title,
                    announcementFileID: resImage,
                    announcementFileType: image.contentType,
                    description: data.description,
                    status: status
                })

                this.onSuccessInsert()
            } catch (err) { console.log(err)}
            this.isBusy = false
        }
        async apiUpdateAnnouncement (data, image, status) {
            this.isBusy = true
            let body: any = {
                announcementID: data.announcementID,
                title: data.title,
                description: data.description,
                status: status
            }
            try {
                // insert image
                if(image.base64) { 
                    const resImage = await this.FileService.insertBlobFile({
                        base64: image.base64,
                        fileName: image.fileName,
                        contentType: image.contentType
                    })
                    body = {
                        ...body,
                        announcementFileID: resImage,
                        announcementFileType: image.contentType
                    }
                }

                // insert landing page
                const res = await this.announcementService.updateAnnouncement(body)

                this.onSuccessUpdate()
            } catch (err) { 
            this.isBusy = false
            console.log(err)}
        }

        // handling status
        onSuccessInsert () {
            this.fetch()
            this.add = false
            this.addFileData = false
        }
        onSuccessUpdate () {
            this.fetch()
            this.edit = false
            this.editFileData = false
        }

        // handling
        async handleChangeAddFile (e) {
            var fileInput = e.target.files || e.dataTransfer.files;
            if (!fileInput.length) return;
            this.fileType = true;
            this.fileSize = true;

            let file = e.target.files[0];

            let array = file.name.split(".");

            let extension = array.pop();
            if (extension != 'pdf' && extension != 'jpeg' && extension != 'jpg' && extension != 'png' && extension != 'mp4') {
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

            this.image = {
                ...this.image,
                base64: base64String,
                fileName: this.addFileName,
                contentType: extension
            }

            if (extension == 'jpeg' || extension == 'jpg' || extension == 'png' || extension == 'mp4') {
                if(extension === 'mp4') {
                    this.fileUploadType = 'video';    
                } else {
                    this.fileUploadType = 'image';
                }
                var temp = this;
                let reader = new FileReader();
                reader.readAsDataURL(fileInput[0]);
                reader.onload = function () {
                    // if(extension == 'mp4') {
                    //     temp.image.data = URL.createObjectURL(file)
                    // } else {
                        temp.image.data = reader.result as string;
                    // }
                    temp.addImageData = reader.result as string;
                    temp.addFileData = null;
                };
                reader.onerror = error => console.log(error);
            }
            else if(extension == 'pdf'){
                this.fileUploadType = 'pdf';
                this.addFileData = URL.createObjectURL(file);
                this.image.data = URL.createObjectURL(file);
                this.addImageData = '/upload-image2.png';
            }
        }

        async handleDetailClicked(announcement) {
            this.selectedData = announcement
            this.selectedData.announcementFileID
            //  =await Axios({
            // url: announcement.announcementFileID,
            //     method: 'GET',
            //     responseType: 'blob', // important
            // }).then((response) => {

            //     let file = new Blob([response.data], { type: 'application/pdf' });            
            //     var fileURL = URL.createObjectURL(file);

            //     return fileURL;
            // });
            this.detail = true;
        }

        async handleEditClicked(announcement) {
            this.selectedData = announcement
            this.selectedData.announcementFileID
            //  =await Axios({
            // url: announcement.announcementFileID,
            //     method: 'GET',
            //     responseType: 'blob', // important
            // }).then((response) => {

            //     let file = new Blob([response.data], { type: 'application/pdf' });            
            //     var fileURL = URL.createObjectURL(file);

            //     return fileURL;
            // });
            this.editFileData = null
            this.edit = true;
        }

        handleDeleteClicked(announcement) {
            this.selectedData = announcement
        }

        handleAdd (status) {
            this.apiInsertAnnouncement(this.formData, this.image, status)
        }

        handleEdit(status) {
            this.apiUpdateAnnouncement(this.selectedData, this.editModel.fileContent ? this.editModel.fileContent :{}, status)
        }

        _resetData() {
            this.edit = false
            this.editFileData = null
            this.selectedData = {}
        }

    }
</script>