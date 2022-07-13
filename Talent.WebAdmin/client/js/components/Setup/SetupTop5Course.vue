<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="row">
            <div class="col-md-12">
                <h3 class="mb-4"><fa icon="folder"></fa> Setup > <span class="talent_font_red">Setup Top 5 Course</span></h3>

                <!--SEARCH-->
                <div class="d-flex align-items-end flex-column">
                    <div class="col-md-3">
                        <div class="row justify-content-between">
                            <div class="col-md-8 talent_nopadding">
                                <input class="form-control shadow-sm bg-white rounded" v-model="courseName" />
                            </div>
                            <div class="col-md-3 talent_nopadding d-flex justify-content-center">
                                <button class="btn btn-block talent_bg_blue talent_font_white shadow-sm" @click="fetch">Search</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="mb-4"></div>

                <!-- View Top 5 Course List -->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="col-md-12 p-0 mb-md-4">
                        <div class="row align-items-start row justify-content-between m-0">
                            <a>Showing {{listST5C.totalItem}} of 5 Entry(s)</a>
                            <button class="btn talent_bg_cyan talent_roundborder talent_font_white" data-toggle="modal" data-target=".bd-example-modal-lg">Add Course</button>
                        </div>

                        <div class="col-md-12 d-flex mt-2 px-3 py-0 font-weight-bold">
                            <div class="col-md-3 p-0">
                                Course Name
                            </div>
                            <div class="col-md-4 p-0">
                                Program Type
                            </div>
                            <div class="col-md-1 p-0">
                                Batch
                            </div>
                            <div class="col-md-2 p-0">
                                Created By
                            </div>
                            <div class="col-md-2 p-0">
                                Date
                            </div>
                        </div>

                        <draggable :list="listST5C.listSetupTop5Course"
                                   class="list-group"
                                   draggable=".data"
                                   v-bind="dragOptions"
                                   @start="dragging = true"
                                   @end="dragging = false">

                            <transition-group type="transition" name="flip-list">
                                <div class="data"
                                     v-for="element in listST5C.listSetupTop5Course"
                                     :key="element.trainingId">

                                    <div class="col-md-12 d-flex mt-2 px-3 py-2 border rounded-pill shadow-sm talent_cursorpoint align-items-center">
                                        <div class="col-md-3 p-0">
                                            <fa icon="inbox"></fa> {{ element.courseName }}
                                        </div>
                                        <div class="col-md-4 p-0">
                                            {{ element.programTypeName }}
                                        </div>
                                        <div class="col-md-1 p-0">
                                            {{ element.batch }}
                                        </div>
                                        <div class="col-md-2 p-0">
                                            {{ element.createdBy }}
                                        </div>
                                        <div class="col-md-2 p-0">
                                            {{ element.date | dateFormatWithTime}}
                                        </div>
                                    </div>
                                </div>
                            </transition-group>
                        </draggable>
                    </div>

                    <div class="d-flex align-items-end flex-column">
                        <div>
                            <button class="btn talent_bg_lightgreen talent_font_white" @click.prevent="rearrangeTop5CourseList" :disabled="courseName != '' || listST5C.listSetupTop5Course.length == 0 ? true : false || !crud.update">
                                Save
                            </button>
                        </div>
                    </div>
                </div>


                <!-- dropdown data -->
                <div class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myExtraLargeModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered modal-lg">
                        <div class="modal-content talent_roundborder">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12 d-flex justify-content-center talent_margintop">
                                        <div class="col-md-4 talent_marginbottom2">
                                            <span>Course Name</span>
                                        </div>
                                        <div class="col-md-8 talent_marginbottom2">
                                            <select class="form-control" v-model="SetupTop5CourseModel">
                                                <option v-for="data in listDropdown.courseNameDropdown" :key="data.trainingId" :value="data">{{data.courseName}}</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-md-12 d-flex justify-content-center">
                                        <div>
                                            <button type="button" value="Go" :disabled="!crud.create" @click="addCourseIntoTop5Course" class="btn talent_bg_cyan talent_roundborder talent_font_white " data-toggle="modal" data-target=".bd-example-modal-lg">Choose</button>
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
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { SetupTop5CourseJoinedModel, SetupTop5CourseService, SetupTop5CourseViewModel, SetupTop5CourseDropdownList, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService';
    import { PageEnums } from '../../enum/PageEnums';

    @Component({
        created: async function (this: SetupTop5Course) {
            await this.getAccess()
            this.initialize();
        }
    })
    export default class SetupTop5Course extends Vue {

        //Service used
        ServiceST5C: SetupTop5CourseService = new SetupTop5CourseService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        listST5C: SetupTop5CourseViewModel = {
            listSetupTop5Course: [],
            totalItem: null
        };

        dragging: boolean = false;

        listDropdown: SetupTop5CourseDropdownList = {
            courseNameDropdown: [],
        }

        SetupTop5CourseModel: SetupTop5CourseJoinedModel = {
            batch: null,
            courseId: null,
            trainingId: null,
            courseName: '',
            createdBy: '',
            programTypeName: '',
            date: null,
        }

        isBusy: boolean = false;

        courseName: string = '';
        errorMessage: string = '';

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.SetupTop5Course);
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        async initialize() {
            await this.fetch();
            await this.fetchDropdown();
        }

        async fetch() {
            this.isBusy = true;
            try {
                this.listST5C = await this.ServiceST5C.getAllSetupTop5Course(this.courseName);
            }
            catch{
                this.errorMessage = "Failed to get Data!"
            }
            this.isBusy = false;
        }

        async fetchDropdown() {
            this.isBusy = true;
            try {
                this.listDropdown = await this.ServiceST5C.getCourseListDropdown();
            } catch {
                this.errorMessage = "Failed to get Data!"
            }
            this.isBusy = false;
        }

        async addCourseIntoTop5Course() {
            if (!this.crud.create) {
                return
            }
            if (this.SetupTop5CourseModel.trainingId == null) {
                this.errorMessage = 'Course name must be choosed';
                return;
            }
            this.isBusy = true;

            let valid = await this.ServiceST5C.insertIntoTop5Course(this.SetupTop5CourseModel);

            if (valid == false) {
                this.errorMessage = "Failed to insert into Top 5 Course List! Course is already exist";
            }

            this.fetch();
            this.fetchDropdown();

            this.isBusy = false;
        }

        async rearrangeTop5CourseList() {
            if (!this.crud.update) {
                return
            }
            this.isBusy = true;
            let valid = await this.ServiceST5C.rearrangeTop5Course(this.listST5C);

            if (valid == false) {
                this.errorMessage = "Failed to Re arrange Top 5 Course List!"
            }

            this.fetch();
            this.isBusy = false;
        }

        get dragOptions() {
            return {
                ghostClass: "ghost"
            }
        }
    }
</script>

<style>
    .ghost {
        opacity: 0.4;
    }

    .flip-list-move {
        transition: transform 0.5s;
    }
</style>