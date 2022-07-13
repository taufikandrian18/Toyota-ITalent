<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="row">
            <div class="col col-md-12">
                <h3 class="mb-md-4"><fa icon="folder"></fa> Setup > <span class="talent_font_red">Setup Learning</span></h3>

                <div v-if="successMessageShow == true" class="alert alert-success alert-dismissible fade show" role="alert">
                    {{successMessage}}
                    <button type="button" class="close" data-dismiss="alert" @click.prevent="ResetSuccessMessage()" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mb-md-4">
                    <h4 class="mb-md-4">Search Learning</h4>

                    <!--3 Column Menu-->
                    <div class="row mb-md-4">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group talent_front">
                                        <v-date-picker class="v-date-style-report" v-model="filter.Date" mode="range" :firstDayOfWeek="2" :value="null" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                                        <div class="input-group-append">
                                            <span class="input-group-text"><fa icon="calendar-alt"></fa></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Program Type</span>
                                </div>
                                <div class="col-md-9">
                                    <select v-model="filter.ProgramType" class="form-control">
                                        <option v-for="p in programTypeData">{{p}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Approval Status</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <select v-model="filter.ApprovalStatus" class="form-control">
                                            <option v-for="a in approvalStatusData">{{a}}</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!--3 Column Menu-->
                    <div class="row mb-md-4">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Learning Name</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input v-model="filter.LearningName" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Learning Category</span>
                                </div>
                                <div class="col-md-9">
                                    <select v-model="filter.LearningCategory" class="form-control">
                                        <option v-for="l in learningCategoryData">{{l}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">

                        </div>
                    </div>

                    <!--Search Button-->
                    <div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="d-flex align-items-end flex-column">
                                    <div>
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="Fetch()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="Reset()">
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
                    <h4>Learning List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a v-if="(setupLearning.totalData - ((currentPage - 1) * pageSize) ) >= pageSize">Showing {{pageSize}} of {{setupLearning.totalData}} Entry(s)</a>
                            <a v-else>Showing {{(setupLearning.totalData) - ((currentPage - 1) * pageSize)}} of {{setupLearning.totalData}} Entry(s)</a>
                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" data-toggle="modal" data-target="#setupLearning">Setup Learning</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClicklearningName()">Learning Name <fa icon="sort" v-if="learningName.sort == true"></fa><fa icon="sort-up" v-if="learningName.sortup == true"></fa><fa icon="sort-down" v-if="learningName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortProgramType()">Program Type <fa icon="sort" v-if="programType.sort == true"></fa><fa icon="sort-up" v-if="programType.sortup == true"></fa><fa icon="sort-down" v-if="programType.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortLearningCategory()">Learning Category <fa icon="sort" v-if="learningCategory.sort == true"></fa><fa icon="sort-up" v-if="learningCategory.sortup == true"></fa><fa icon="sort-down" v-if="learningCategory.sortdown == true"></fa></a>
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
                                    <th colspan="3" v-if="crud.delete || crud.read || crud.update">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(sl, index) in setupLearning.data">
                                    <td>
                                        {{sl.learningName}}
                                    </td>
                                    <td>
                                        {{sl.programTypeName}}
                                    </td>
                                    <td>
                                        {{sl.learningCategoryName}}
                                    </td>
                                    <td>
                                        {{sl.approvalStatus}}
                                    </td>
                                    <td>
                                        {{sl.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{sl.updatedAt | dateFormat}}
                                    </td>
                                    <!--<td class="talent_nopadding_important">
                            <div class="onoffswitch">
                                <input type="checkbox" class="onoffswitch-checkbox" :id="index">
                                <label class="onoffswitch-label" :for="index">
                                    <span class="onoffswitch-inner"></span>
                                    <span class="onoffswitch-switch"></span>
                                </label>
                            </div>
                        </td>-->
                                    <!--VIEW DETAIL-->
                                    <td v-if="sl.learningCategoryName != 'Course' && crud.read " class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="sl.learningCategoryName == 'Module' ? RedirectToModule(sl.setupModuleId,'View') : RedirectToPopQuiz(sl.popQuizId,'View')">View Detail</button>
                                    </td>

                                    <td v-else-if="sl.learningCategoryName == 'Course' && crud.read && sl.approvalStatus == waiting" class="talent_nopadding_important && crud.read">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="GetAllCourseLockUnlock(sl.courseId, sl.approvalStatus)" data-toggle="modal" data-target="#lockUnlock">View Detail</button>
                                    </td>

                                    <td v-else class="talent_nopadding_important && crud.read">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="GetAllCourseLockUnlock(sl.courseId, sl.approvalStatus)" data-toggle="modal" data-target="#lockUnlock" :disabled="DisableWaiting(sl.approvalStatus)">View Detail</button>
                                    </td>

                                    <!--EDIT-->
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="sl.learningCategoryName == 'Course' ? RedirectToCourse(sl.courseId) : sl.learningCategoryName == 'Module' ? RedirectToModule(sl.setupModuleId,'Edit') : RedirectToPopQuiz(sl.popQuizId,'Edit')" :disabled="DisableWaiting(sl.approvalStatus)">Edit</button>
                                    </td>

                                    <!--REMOVE-->
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" :disabled="DisableWaiting(sl.approvalStatus)" @click.prevent="sl.learningCategoryName == 'Course' ? RemoveGet(sl.learningCategoryName,sl.courseId) : sl.learningCategoryName == 'Module' ? RemoveGet(sl.learningCategoryName,sl.setupModuleId) : RemoveGet(sl.learningCategoryName,sl.popQuizId)"
                                                data-toggle="modal" data-target="#modalDelete">
                                            Remove
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <div class="col-md-12 d-flex justify-content-center mt-3">
                        <paginate :currentPage.sync="currentPage" :total-data=setupLearning.totalData :limit-data=pageSize @update:currentPage="Fetch()"></paginate>
                    </div>

                </div>
            </div>
        </div>

        <!--Modal Setup Learning-->
        <div class="modal fade" id="setupLearning" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content talent_roundborder">
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-md-12 d-flex justify-content-center talent_margintop">
                                <div class="col-md-4 talent_marginbottom2">
                                    Learning Category
                                </div>
                                <div class="col-md-8 talent_marginbottom2">
                                    <select class="form-control" v-model="selected">
                                        <option value="course" selected="selected">Course</option>
                                        <option value="module">Module</option>
                                        <option value="popup">Pop Quiz</option>
                                    </select>
                                </div>
                            </div>
                            <div class="col-md-12 d-flex justify-content-center">
                                <div class="col-md-12 d-flex justify-content-center">
                                    <button class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="redirectSetUp()">Choose Learning</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Modal Lock Unlock-->
        <div class="modal fade" id="lockUnlock" tabindex="-1" role="dialog" aria-hidden="true">
            <loading-overlay :loading="isBusyLockUnlock"></loading-overlay>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content talent_roundborder">
                    <div class="modal-body">
                        <div class="row py-3">
                            <div class="col-md-12 d-flex justify-content-center">
                                <div class="col-md-4 talent_marginbottom2">
                                    Course Name
                                </div>
                                <div class="col-md-8 talent_marginbottom2">
                                    <input v-model="setupLearningCourseLockUnlock.courseName" class="form-control" disabled />
                                </div>
                            </div>
                            <div class="col-md-12 d-flex justify-content-center">
                                <div class="col-md-4 talent_marginbottom2">
                                    Program Type
                                </div>
                                <div class="col-md-8 talent_marginbottom2">
                                    <input v-model="setupLearningCourseLockUnlock.programTypeName" class="form-control" disabled />
                                </div>
                            </div>
                            <div v-for="(slclu, index) in setupLearningCourseLockUnlock.module" class="col-md-12 d-flex justify-content-center">
                                <div class="col-md-4 talent_marginbottom2">
                                    {{index == 0 ? 'Module Name': ''}}
                                </div>
                                <div class="col-md-7 talent_marginbottom2">
                                    <input v-model="slclu.moduleName" class="form-control" disabled />
                                </div>
                                <div class="col-md-1 talent_marginbottom2 pl-0">
                                    <div v-if="slclu.isPublishedModule == false" class="d-flex justify-content-center h-100 w-100 align-items-center" @click.prevent="LockUnlock(index)" >
                                        <fa class="w-100" icon="lock"></fa>
                                    </div>
                                    <div v-else class="d-flex justify-content-center h-100 w-100 align-items-center" @click.prevent="LockUnlock(index)" >
                                        <fa class="w-100" icon="unlock"></fa>
                                    </div>
                                </div>
                            </div>
                            <div v-for="(slclu, index) in setupLearningCourseLockUnlock.assesment" class="col-md-12 d-flex justify-content-center">
                                <div class="col-md-4 talent_marginbottom2">
                                    {{index == 0 ? 'Assessment Name': ''}}
                                </div>
                                <div class="col-md-7 talent_marginbottom2">
                                    <input v-model="slclu.moduleName" class="form-control" disabled />
                                </div>
                                <div class="col-md-1 talent_marginbottom2 pl-0">
                                    <div v-if="slclu.isPublishedModule == false" class="d-flex justify-content-center h-100 w-100 align-items-center" @click.prevent="LockUnlockAssessment(index)" >
                                        <fa class="w-100" icon="lock"></fa>
                                    </div>
                                    <div v-else class="d-flex justify-content-center h-100 w-100 align-items-center" @click.prevent="LockUnlockAssessment(index)" >
                                        <fa class="w-100" icon="unlock"></fa>
                                    </div>
                                </div>
                            </div>
                            <div v-if="isApprovalStatusWaiting == false" class="col-md-12 d-flex justify-content-center">
                                <div class="col-md-12 d-flex justify-content-center">
                                    <button class="btn talent_bg_green talent_roundborder px-5 talent_font_white" @click.prevent="UpdateLockUnlock()" data-dismiss="modal">Save</button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!--Modal Delete-->
        <div class="modal fade" id="modalDelete" tabindex="-1" role="dialog">
            <loading-overlay :loading="isBusyRemove"></loading-overlay>
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
                                <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="Remove()">Delete</button>
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
    import { UserAccessCRUD, UserPrivilegeSettingsService, SetupLearningService, SetupLearningPaginate, SetupLearningCourseLockUnlock, SetupModuleService, SetupPopQuizService } from '../../services/NSwagService'
    import { ApprovalStatusEnum } from '../../enum/ApprovalStatusEnum'

    @Component({
        props: ['framework', 'compiler'],
        created: async function (this: SetupLearning) {
            this.isBusy = true;
            await this.getAccess()
            await this.Fetch()
            await this.GetAllProgramType()
            await this.GetAllApprovalStatus()
        }
    })
    export default class SetupLearning extends Vue {
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("STL");
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        //API
        setupLearningApi: SetupLearningService = new SetupLearningService()
        setupModuleApi: SetupModuleService = new SetupModuleService()
        setupPopQuizApi: SetupPopQuizService = new SetupPopQuizService()

        //PAGE
        currentPage: number = 1
        pageSize: number = 10


        //bussiness rule
        //edit remove untuk setup module dan pop quiz hanya bisa dilakukan ketikas status sudah approve dan juga rejected
        //view detail untuk course lock unlock module hanya bisa dilakukan ketikas status sudah approve dan juga rejected
        //view detail untuk setup module dan pop quiz bisa dilihat kapan pun

        //Approval status enum
        waiting: string = ApprovalStatusEnum.WAITINGNAME;
        
        //FILTER
        filter: ISetupLearningFilter = {
            ApprovalStatus: '', Date: { end: null, start: null }, LearningCategory: '', LearningName: '', PageIndex: this.currentPage,
            PageSize: this.pageSize, ProgramType: '', SortBy: ''
        }
        //VARIABLE SETUP LEARNING
        programTypeData: string[] = []
        approvalStatusData: string[] = []
        learningCategoryData: string[] = ['Course', 'Module', 'Pop Quiz']
        setupLearning: SetupLearningPaginate = { data: [], totalData: null }
        setupLearningCourseLockUnlock: SetupLearningCourseLockUnlock =
            { courseId: null, courseName: '', programTypeName: '', module: [], assesment: [] }

        //LOADING OVERLAY
        isBusy: boolean = false;

        //DATE PICKER
        selected: string = "";
        props: any = {
            placeholder: "",
            readonly: true
        };

        //FETCH
        async Fetch() {
            this.isBusy = true;
            this.setupLearning = await this.setupLearningApi.getAllSetupLearning(this.filter.Date.start, this.filter.Date.end, this.filter.ProgramType, this.filter.LearningCategory, this.filter.LearningName, this.filter.ApprovalStatus, this.filter.SortBy, this.currentPage, this.filter.PageSize)
            this.isBusy = false;
        }
        //GET ALL PROGRAM TYPE
        async GetAllProgramType() {
            this.isBusy = true;
            this.programTypeData = await this.setupLearningApi.getAllProgramType()
            this.isBusy = false;
        }
        //GET ALL APPROVAL STATUS
        async GetAllApprovalStatus() {
            this.isBusy = true;
            this.approvalStatusData = await this.setupLearningApi.getAllApprovalStatus()
            this.isBusy = false;
        }

        isBusyLockUnlock: boolean = false;
        isApprovalStatusWaiting: boolean = false;

        //GET ALL SetupLearningCourseLockUnlock
        async GetAllCourseLockUnlock(id: number, approvalStatus: string) {
            this.isBusyLockUnlock = true
            this.setupLearningCourseLockUnlock = await this.setupLearningApi.getAllCourseLockUnlock(id)

            if (approvalStatus == ApprovalStatusEnum.WAITINGNAME) {
                this.isApprovalStatusWaiting = true;
            } else {
                this.isApprovalStatusWaiting = false;
            }

            this.isBusyLockUnlock = false
        }

        //LOCK UNLOCK MODULE
        async LockUnlock(index: number) {
            if (this.isApprovalStatusWaiting == true) {
                return
            }

            if (!this.crud.read) {
                return
            }
            if (this.setupLearningCourseLockUnlock.module[index].isPublishedModule == true) {
                this.setupLearningCourseLockUnlock.module[index].isPublishedModule = false
            } else {
                this.setupLearningCourseLockUnlock.module[index].isPublishedModule = true
            }
        }

        //LOCK UNLOCK ASSESSMENT
        async LockUnlockAssessment(index: number) {
            if (this.isApprovalStatusWaiting == true) {
                return
            }

            if (!this.crud.read) {
                return
            }
            if (this.setupLearningCourseLockUnlock.assesment[index].isPublishedModule == true) {
                this.setupLearningCourseLockUnlock.assesment[index].isPublishedModule = false
            } else {
                this.setupLearningCourseLockUnlock.assesment[index].isPublishedModule = true
            }
        }

        async UpdateLockUnlock(id: number) {
            if (!this.crud.read) {
                return
            }
            this.isBusy = true;
            console.log(this.setupLearningCourseLockUnlock)
            await this.setupLearningApi.courseLockUnlockModule(this.setupLearningCourseLockUnlock);
            this.isBusy = false;
            await this.Fetch()
        }

        //REDIRECT TO SETUP MODULE
        RedirectToModule(id: number, mode: string) {
            console.log("SETUP MODULE TO GO " + id)
            if (mode == 'View') {
                if (!this.crud.read) {
                    return
                }
                window.location.href = "/Setup/SetupModule/View?SetupModuleId=" + id;
            } else {
                if (!this.crud.update) {
                    return
                }
                window.location.href = "/Setup/SetupModule/Edit?SetupModuleId=" + id;
            }

        }

        //REDIRECT TO SETUP POP QUIZ
        RedirectToPopQuiz(id: number, mode: string) {
            console.log("SETUP POP QUIZ TO GO " + id)
            if (mode == 'View') {
                if (!this.crud.read) {
                    return
                }
                window.location.href = "/Setup/SetupPopUpQuiz/View?PopQuizId=" + id;
            } else {
                if (!this.crud.update) {
                    return
                }
                window.location.href = "/Setup/SetupPopUpQuiz/Edit?PopQuizId=" + id;
            }
        }

        //REDIRECT TO COURSE
        RedirectToCourse(id: number, ) {
            console.log("SETUP COURSE TO GO " + id)
            if (!this.crud.update) {
                return
            }
            window.location.href = "/Setup/SetupCourse/Edit?SetupCourseId=" + id;
        }


        async Reset() {
            this.filter = {
                ApprovalStatus: '', Date: { end: null, start: null }, LearningCategory: '', LearningName: '', PageIndex: this.currentPage,
                PageSize: this.pageSize, ProgramType: '', SortBy: ''
            }

            this.learningName.reset();
            this.programType.reset();
            this.learningCategory.reset();
            this.approvalStatus.reset();
            this.createdDate.reset();
            this.updatedDate.reset();

            await this.Fetch();
        }

        //REDIRECT
        redirectSetUp() {
            if (!this.crud.create) {
                return
            }
            if (this.selected === "course") {
                window.location.href = "/Setup/SetupCourse/Add";
            }
            else if (this.selected === "module") {
                window.location.href = "/Setup/SetupModule/Add";
            }
            else {
                window.location.href = "/Setup/SetupPopUpQuiz/Add";
            }
        }

        courseDeleteId: number = null;
        moduleDeleteId: number = null;
        popQuizDeleteId: number = null;
        typeOfRemove: string = '';
        isBusyRemove: boolean = false;

        //REMOVEGET
        async RemoveGet(setupLearning: string, id: number) {
            if (!this.crud.delete) {
                return
            }
            this.isBusyRemove = true
            this.typeOfRemove = setupLearning;
            switch (setupLearning) {
                case "Course":
                    this.courseDeleteId = id
                    break;
                case "Module":
                    this.moduleDeleteId = id
                    break;
                case "Pop Quiz":
                    this.popQuizDeleteId = id
                    break;
            }
            this.isBusyRemove = false
        }

        //REMOVE
        async Remove() {
            if (!this.crud.delete) {
                return
            }
            switch (this.typeOfRemove) {
                case "Course":
                    await this.setupLearningApi.removeCourse(this.courseDeleteId)
                    break;
                case "Module":
                    await this.setupModuleApi.deleteSetupModule(this.moduleDeleteId)
                    break;
                case "Pop Quiz":
                    await this.setupPopQuizApi.removePopQuiz(this.popQuizDeleteId)
                    break;
            }
            this.successMessage = "Success to Remove Data"
            this.successMessageShow = true
            this.isBusy = true;
            await this.Fetch();
        }

        //SORTING
        learningName = new Sort();
        programType = new Sort();
        learningCategory = new Sort();
        approvalStatus = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        async ClickSortApprovalStatus() {
            this.ResetSort('approvalStatus');
            //Sorting
            this.approvalStatus.sorting();
            //Function Sorting
            if (this.approvalStatus.sortup == true) {
                this.filter.SortBy = "ApprovalStatusAsc";
            } else if (this.approvalStatus.sortdown == true) {
                this.filter.SortBy = "ApprovalStatusDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.Fetch()
            return;
        }

        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting
            if (this.createdDate.sortup == true) {
                this.filter.SortBy = "CreatedAtAsc";
            } else if (this.createdDate.sortdown == true) {
                this.filter.SortBy = "CreatedAtDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.Fetch()
            return;
        }

        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting
            if (this.updatedDate.sortup == true) {
                this.filter.SortBy = "UpdatedAtAsc";
            } else if (this.updatedDate.sortdown == true) {
                this.filter.SortBy = "UpdatedAtDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.Fetch()
            return;
        }

        async ClicklearningName() {
            this.ResetSort('learningName');
            //Sorting
            this.learningName.sorting();
            //Function Sorting
            if (this.learningName.sortup == true) {
                this.filter.SortBy = "LearningNameAsc";
            } else if (this.learningName.sortdown == true) {
                this.filter.SortBy = "LearningNameDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.Fetch()
            return;
        }

        //ClickSortProgramType
        async ClickSortProgramType() {
            this.ResetSort('programType');
            //Sorting
            this.programType.sorting();
            //Function Sorting
            if (this.programType.sortup == true) {
                this.filter.SortBy = "ProgramTypeNameAsc";
            } else if (this.programType.sortdown == true) {
                this.filter.SortBy = "ProgramTypeNameDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.Fetch()
            return;
        }

        //ClickSortLearningCategory
        async ClickSortLearningCategory() {
            this.ResetSort('learningCategory');
            //Sorting
            this.learningCategory.sorting();
            //Function Sorting
            if (this.learningCategory.sortup == true) {
                this.filter.SortBy = "LearningCategoryNameAsc";
            } else if (this.learningCategory.sortdown == true) {
                this.filter.SortBy = "LearningCategoryNameDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.Fetch()
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'learningName':
                    this.programType.reset();
                    this.learningCategory.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'programType':
                    this.learningName.reset();
                    this.learningCategory.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'learningCategory':
                    this.programType.reset();
                    this.learningName.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'approvalStatus':
                    this.programType.reset();
                    this.learningCategory.reset();
                    this.learningName.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.programType.reset();
                    this.learningCategory.reset();
                    this.approvalStatus.reset();
                    this.learningName.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.programType.reset();
                    this.learningCategory.reset();
                    this.approvalStatus.reset();
                    this.createdDate.reset();
                    this.learningName.reset();
                    return;
            }
        }

        //SUCCESS MESSAGE
        successMessage: string = ''
        successMessageShow: boolean = false;

        ResetSuccessMessage() {
            this.successMessageShow = false;
            this.successMessage = "";
        }

        DisableWaiting(status: string) {
            return status === ApprovalStatusEnum.WAITINGNAME
        }
    }
</script>
