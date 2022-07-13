<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="row">
            <div class="col-md-12">
                <!--TITLE-->
                <!--<h3><fa icon="cog"></fa> <fa icon="user"></fa> <fa icon="database"></fa> <fa icon="folder"></fa> Menu > <span class="talent_font_red">Page</span></h3>-->
                <h3><fa icon="folder"></fa> Setup > <span class="talent_font_red">Task</span></h3>
                <br />
                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Task</h4>
                    <br />
                    <!--3 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group talent_front">
                                        <v-date-picker v-model="search.DateFilter" class="v-date-style-report" mode="range" :value="null" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                    <span>Type of Question</span>
                                </div>
                                <div class="col-md-9">
                                    <select v-model="search.QuestionTypeId" class="form-control">
                                        <option value="" disabled hidden>-</option>
                                        <option value="1">True/False</option>
                                        <option value="2">Matching</option>
                                        <option value="3">Sequence</option>
                                        <option value="4">Tebak Gambar</option>
                                        <option value="5">Hotspot</option>
                                        <option value="6">Short Answer</option>
                                        <option value="7">Open Question</option>
                                        <option value="8">Check List</option>
                                        <option value="9">Rating</option>
                                        <option value="10">Multiple Choice</option>
                                        <option value="11">File Upload</option>
                                        <option value="12">Matrix</option>
                                    </select>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Created By</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input v-model="search.CreateBy" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <!--3 Column Menu-->
                    <div class="row">
                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Task Code</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input v-model="search.TaskCode" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Module</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input v-model="search.ModuleName" class="form-control" />
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
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="fetchdata()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="resetsearch()">
                                            Reset
                                        </button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <br />

                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Task List</h4>
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a v-if="allData.taskData">Showing {{allData.taskData.length}} of {{allData.totalData}} Entry(s)</a>

                            <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white" data-toggle="modal" data-target=".bd-example-modal-lg">Create Question</button>
                        </div>
                    </div>

                    <div class="talent_overflowx mb-3">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortTaskCode()">Task Code <fa icon="sort" v-if="taskCode.sort == true"></fa><fa icon="sort-up" v-if="taskCode.sortup == true"></fa><fa icon="sort-down" v-if="taskCode.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortTypeofQuestion()">Type of Question <fa icon="sort" v-if="typeofQuestion.sort == true"></fa><fa icon="sort-up" v-if="typeofQuestion.sortup == true"></fa><fa icon="sort-down" v-if="typeofQuestion.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortModule()">Module <fa icon="sort" v-if="module.sort == true"></fa><fa icon="sort-up" v-if="module.sortup == true"></fa><fa icon="sort-down" v-if="module.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedBy()">Created By <fa icon="sort" v-if="createdBy.sort == true"></fa><fa icon="sort-up" v-if="createdBy.sortup == true"></fa><fa icon="sort-down" v-if="createdBy.sortdown == true"></fa></a>
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
                                <tr v-for="data in allData.taskData">
                                    <td>
                                        {{data.taskCode}}
                                    </td>
                                    <td v-if="data.questionTypeId == 1">
                                        True/False
                                    </td>
                                    <td v-if="data.questionTypeId == 2">
                                        Matching
                                    </td>
                                    <td v-if="data.questionTypeId == 3">
                                        Sequence
                                    </td>
                                    <td v-if="data.questionTypeId == 4">
                                        Tebak Gambar
                                    </td>
                                    <td v-if="data.questionTypeId == 5">
                                        Hot Spot
                                    </td>
                                    <td v-if="data.questionTypeId == 6">
                                        Short Answer
                                    </td>
                                    <td v-if="data.questionTypeId == 7">
                                        Essay
                                    </td>
                                    <td v-if="data.questionTypeId == 8">
                                        Checklist
                                    </td>
                                    <td v-if="data.questionTypeId == 9">
                                        Rating
                                    </td>
                                    <td v-if="data.questionTypeId == 10">
                                        Multiple Choice
                                    </td>
                                    <td v-if="data.questionTypeId == 11">
                                        File Upload
                                    </td>
                                    <td v-if="data.questionTypeId == 12">
                                        Matrix
                                    </td>
                                    <td>
                                        {{data.moduleName}}
                                    </td>
                                    <td>
                                        {{data.createdBy}}
                                    </td>
                                    <td>
                                        {{data.createdAt}}
                                    </td>
                                    <td>
                                        {{data.updateAt}}
                                    </td>
                                    <td v-if="crud.read" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click="redirectView(data.taskId,data.questionTypeId)">View Detail</button>
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click="redirectEdit(data.taskId,data.questionTypeId)">Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#deleteConfirmation" @click="setDelete(data.taskId,data.taskName)">
                                            Remove
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!--Pagination-->
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate :currentPage.sync="currentPage" :total-data=allData.totalData :limit-data=10 @update:currentPage="fetchdata()"></paginate>
                    </div>

                </div>

                <br />

                <div class="modal fade bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myExtraLargeModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered modal-lg">
                        <div class="modal-content talent_roundborder">
                            <div class="modal-body">
                                <div class="row">
                                    <div class="col-md-12 d-flex justify-content-center talent_margintop">
                                        <div class="col-md-4 talent_marginbottom2">
                                            <span>Type of Question</span>
                                        </div>
                                        <div class="col-md-8 talent_marginbottom2">
                                            <select class="form-control" v-model="key">
                                                <option value="/Setup/Tasks/TrueFalse/Add">True/False</option>
                                                <option value="/Setup/Tasks/Matching/Add">Matching</option>
                                                <option value="/Setup/Tasks/Sequence/Add">Sequence</option>
                                                <option value="/Setup/Tasks/TebakGambar/Add">Tebak Gambar</option>
                                                <option value="/Setup/Tasks/Hotspot/Add">Hotspot</option>
                                                <option value="/Setup/Tasks/ShortAnswer/Add">Short Answer</option>
                                                <option value="/Setup/Tasks/Essay/Add">Essay</option>
                                                <option value="/Setup/Tasks/Checklist/Add">Check List</option>
                                                <option value="/Setup/Tasks/Rating/Add">Rating</option>
                                                <option value="/Setup/Tasks/MultipleChoice/Add">Multiple Choice</option>
                                                <option value="/Setup/Tasks/FileUpload/Add">File Upload</option>
                                                <option value="/Setup/Tasks/Matrix/Add">Matrix</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div class="col-md-12 d-flex justify-content-center">
                                        <div>
                                            <button type="button" value="Go" @click="goToNewPage()" class="btn talent_bg_cyan talent_roundborder talent_font_white " data-toggle="modal" data-target=".bd-example-modal-lg">Choose Type</button>
                                        </div>
                                    </div>
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
                            <h5>Are you sure you want to delete task?</h5>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="col-md-12 d-flex justify-content-around">
                                <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="deleteTask(toBeDeletedId)">Delete</button>
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
    //import { TaskService, TaskPaginationModel, ITaskPaginationModel } from '../../services/Task';
    import { TaskService, TaskPaginationModel, UserPrivilegeSettingsService, UserAccessCRUD } from '../../services/NSwagService'

    @Component({
        props: [],
        created: async function (this: Task) {
            await this.fetchdata();
            await this.getAccess();
        }
    })
    export default class Task extends Vue {
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("TSK");
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }


        isBusy = true;
        allData: TaskPaginationModel = { taskData: null, totalData: null }
        Service: TaskService = new TaskService();


        currentPage: number = 1;
        key: string = "";



        looping: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        //Variable untuk sorting
        taskCode = new Sort();
        typeofQuestion = new Sort();
        module = new Sort();
        createdBy = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        /*
         * used to store deleted id and name
         * used in the modal
         * */
        toBeDeletedId: number = 0;
        toBeDeletedName: string = '';

        /*
         * used to store data for serching
         * */
        search: ITaskSearchModel = {
            TaskCode: "",
            QuestionTypeId: null,
            ModuleName: "",
            CreateBy: "",
            DateFilter: {
                end: null,
                start: null,
            },
            SortBy: "",
            PageIndex: 1,
            pageSize: 10,
            taskId: null
        }

        /*
         * set delete id and name
         * used for modal pop up
         * */
        setDelete(index: number, name: string) {

            if (!this.crud.delete) {
                return;
            }

            this.toBeDeletedId = index;
            this.toBeDeletedName = name;
        }

        /*
         * used to reset data in serching
         * */
        async resetsearch() {

            this.search.CreateBy = '';
            this.search.DateFilter = {
                end: null,
                start: null
            }
            this.search.ModuleName = '';
            this.search.PageIndex = 1;
            this.search.pageSize = 10;
            this.search.QuestionTypeId = null;
            this.search.SortBy = '';
            this.search.TaskCode = '';

            this.taskCode.reset();
            this.typeofQuestion.reset();
            this.module.reset();
            this.createdBy.reset();
            this.createdDate.reset();
            this.updatedDate.reset();

            this.fetchdata();
        }

        /*
         * delete selected Id
         * */
        async deleteTask(index: number) {
            this.isBusy = false;
            await this.Service.deleteTask(index);

            this.toBeDeletedId = 0;
            this.toBeDeletedName = '';

            this.isBusy = true;

            await this.fetchdata();
        }

        async fetchdata() {
            this.isBusy = true;
            var temp = await this.Service.getViewData(
                this.search.DateFilter.start,
                this.search.DateFilter.end,
                this.search.TaskCode,
                this.search.QuestionTypeId,
                this.search.ModuleName,
                this.search.CreateBy,
                this.search.taskId,
                this.search.SortBy,
                this.currentPage,
                this.search.pageSize);

            this.allData.taskData = temp.taskData;
            this.allData.totalData = temp.totalData;
            this.isBusy = false;
        }

        /*
         * set delete id and name
         * used for modal pop up
         * */

        //CreateNew
        async goToNewPage(event) {

            window.location.href = this.key; // create di tab yang sama
            //window.open(this.key, '_blank'); // create di tab baru
        }

        redirectView(taskId: number, questionTypeId: number) {

            if (!this.crud.read) {
                return;
            }

            let stringUrl;

            switch (questionTypeId) {
                case 1:
                    stringUrl = "/Setup/Tasks/TrueFalse/View?TaskId=" + taskId;
                    break;
                case 2:
                    stringUrl = "/Setup/Tasks/Matching/View?TaskId=" + taskId;
                    break;
                case 3:
                    stringUrl = "/Setup/Tasks/Sequence/View?TaskId=" + taskId;
                    break;
                case 4:
                    stringUrl = "/Setup/Tasks/TebakGambar/View?TaskId=" + taskId;
                    break;
                case 5:
                    stringUrl = "/Setup/Tasks/Hotspot/View?TaskId=" + taskId;
                    break;
                case 6:
                    stringUrl = "/Setup/Tasks/ShortAnswer/View?TaskId=" + taskId;
                    break;
                case 7:
                    stringUrl = "/Setup/Tasks/Essay/View?TaskId=" + taskId;
                    break;
                case 8:
                    stringUrl = "/Setup/Tasks/Checklist/View?TaskId=" + taskId;
                    break;
                case 9:
                    stringUrl = "/Setup/Tasks/Rating/View?TaskId=" + taskId;
                    break;
                case 10:
                    stringUrl = "/Setup/Tasks/MultipleChoice/View?TaskId=" + taskId;
                    break;
                case 11:
                    stringUrl = "/Setup/Tasks/FileUpload/View?TaskId=" + taskId;
                    break;
                case 12:
                    stringUrl = "/Setup/Tasks/Matrix/View?TaskId=" + taskId;
                    break;
            }

            window.location.href = stringUrl;
        }

        redirectEdit(taskId: number, questionTypeId: number) {

            if (!this.crud.update) {
                return;
            }

            let stringUrl;

            switch (questionTypeId) {
                case 1:
                    stringUrl = "/Setup/Tasks/TrueFalse/Edit?TaskId=" + taskId;
                    break;
                case 2:
                    stringUrl = "/Setup/Tasks/Matching/Edit?TaskId=" + taskId;
                    break;
                case 3:
                    stringUrl = "/Setup/Tasks/Sequence/Edit?TaskId=" + taskId;
                    break;
                case 4:
                    stringUrl = "/Setup/Tasks/TebakGambar/Edit?TaskId=" + taskId;
                    break;
                case 5:
                    stringUrl = "/Setup/Tasks/Hotspot/Edit?TaskId=" + taskId;
                    break;
                case 6:
                    stringUrl = "/Setup/Tasks/ShortAnswer/Edit?TaskId=" + taskId;
                    break;
                case 7:
                    stringUrl = "/Setup/Tasks/Essay/Edit?TaskId=" + taskId;
                    break;
                case 8:
                    stringUrl = "/Setup/Tasks/Checklist/Edit?TaskId=" + taskId;
                    break;
                case 9:
                    stringUrl = "/Setup/Tasks/Rating/Edit?TaskId=" + taskId;
                    break;
                case 10:
                    stringUrl = "/Setup/Tasks/MultipleChoice/Edit?TaskId=" + taskId;
                    break;
                case 11:
                    stringUrl = "/Setup/Tasks/FileUpload/Edit?TaskId=" + taskId;
                    break;
                case 12:
                    stringUrl = "/Setup/Tasks/Matrix/Edit?TaskId=" + taskId;
                    break;
            }
            window.location.href = stringUrl;
        }

        async ResetCreate() {
            this.key = "";
        }

        //ClickSortCode
        async ClickSortTaskCode() {
            this.ResetSort('taskCode');
            //Sorting
            this.taskCode.sorting();
            //Function Sorting

            if (this.taskCode.sortup == true) {
                this.search.SortBy = 'TaskAsc';
            }
            else if (this.taskCode.sortdown == true) {
                this.search.SortBy = 'TaskDesc';
            }
            else {
                this.search.SortBy = '';
            }

            this.fetchdata();

            return;
        }

        //ClickSortTypeofQuestion
        async ClickSortTypeofQuestion() {
            this.ResetSort('typeofQuestion');
            //Sorting
            this.typeofQuestion.sorting();
            //Function Sorting

            if (this.typeofQuestion.sortup == true) {
                this.search.SortBy = 'QuestionTypeIdAsc';
            }
            else if (this.typeofQuestion.sortdown == true) {
                this.search.SortBy = 'QuestionTypeIdDesc';
            }
            else {
                this.search.SortBy = '';
            }

            this.fetchdata();

            return;
        }

        //ClickSortModule
        async ClickSortModule() {
            this.ResetSort('module');
            //Sorting
            this.module.sorting();
            //Function Sorting

            if (this.module.sortup == true) {
                this.search.SortBy = 'ModuleNameAsc';
            }
            else if (this.module.sortdown == true) {
                this.search.SortBy = 'ModuleNameDesc';
            }
            else {
                this.search.SortBy = '';
            }

            this.fetchdata();

            return;
        }

        //ClickSortCreatedBy
        async ClickSortCreatedBy() {
            this.ResetSort('createdBy');
            //Sorting
            this.createdBy.sorting();
            //Function Sorting

            if (this.createdBy.sortup == true) {
                this.search.SortBy = 'CreatedByAsc';
            }
            else if (this.createdBy.sortdown == true) {
                this.search.SortBy = 'CreatedByDesc';
            }
            else {
                this.search.SortBy = '';
            }

            this.fetchdata();

            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting

            if (this.createdDate.sortup == true) {
                this.search.SortBy = 'CreatedDateAsc';
            }
            else if (this.createdDate.sortdown == true) {
                this.search.SortBy = 'CreatedDateDesc';
            }
            else {
                this.search.SortBy = '';
            }

            this.fetchdata();

            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting

            if (this.updatedDate.sortup == true) {
                this.search.SortBy = 'UpdatedDateAsc';
            }
            else if (this.updatedDate.sortdown == true) {
                this.search.SortBy = 'UpdatedDateDesc';
            }
            else {
                this.search.SortBy = '';
            }

            this.fetchdata();

            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'taskCode':
                    this.typeofQuestion.reset();
                    this.module.reset();
                    this.createdBy.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'typeofQuestion':
                    this.taskCode.reset();
                    this.module.reset();
                    this.createdBy.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'module':
                    this.taskCode.reset();
                    this.typeofQuestion.reset();
                    this.createdBy.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'CreatedBy':
                    this.taskCode.reset();
                    this.typeofQuestion.reset();
                    this.module.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.taskCode.reset();
                    this.typeofQuestion.reset();
                    this.module.reset();
                    this.createdBy.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.taskCode.reset();
                    this.typeofQuestion.reset();
                    this.module.reset();
                    this.createdBy.reset();
                    this.createdDate.reset();
                    return;
            }

        }
    }
</script>