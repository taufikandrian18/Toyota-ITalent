<template>
    <div>
        <div class="row">
            <!-- buat loading -->
            <loading-overlay :loading="isBusy"></loading-overlay>

            <div class="col col-md-12">
                <!--TITLE-->
                <!--<h2><fa icon="cog"></fa> <fa icon="user"></fa> <fa icon="database"></fa> <fa icon="folder"></fa> Menu > <span class="talent_font_red">Page</span></h2>-->
                <h3><fa icon="folder"></fa> Setup > <span class="talent_font_red">Approval Content</span></h3>
                
                <br />
                    <!-- error message -->
                    <div v-if="isShowErrorMessage == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                        {{theShowErrorMessage}}
                        <button type="button" class="close" data-dismiss="alert" @click.prevent="ResetErrorMessage()" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                <br />

                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Approval Content</h4>

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
                                        <v-date-picker class="v-date-style-report" mode="range" v-model="rangePicker" :firstDayOfWeek="2" :value="null" :masks="{ input: 'DD/MM/YYYY' }" :input-props="props"></v-date-picker>
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
                                    <span>Content Category</span>
                                </div>
                                <div class="col-md-9">
                                    <!-- <select class="form-control">
                                        <option>Banner</option>
                                        <option>1</option>
                                    </select> -->
                                    <multiselect v-model="selectedContentCategory"
                                             :options="contentCategoryList"
                                             :searchable="true"
                                             :allow-empty="true"
                                             @input="setContentCategory"
                                             >
                                    </multiselect>
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
                                        <input type="text" class="form-control" v-model="approvalContentFilter.createdBy"/>
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
                                    <span>Content Name</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input type="text" class="form-control" v-model="approvalContentFilter.contentName"/>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Position</span>
                                </div>
                                <div class="col-md-9">
                                    <div class="input-group">
                                        <input type="text" class="form-control" v-model="approvalContentFilter.contentPosition"/>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="row align-items-center">
                                <div class="col-md-3">
                                    <span>Approval Status</span>
                                </div>
                                <div class="col-md-9">
                                    <!-- <select class="form-control">
                                        <option>Approved</option>
                                        <option>Rejected</option>
                                    </select> -->
                                    <multiselect v-model="selectedApprovalStatus"
                                             :options="approvalStatusList"
                                             :searchable="true"
                                             :allow-empty="true"
                                             @input="setApprovalStatus"
                                             >
                                    </multiselect>
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
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="searchByInput()">
                                            Search
                                        </button>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="resetAll()">
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
                    <h4>Approval Content List</h4>
                    <br />
                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a>Showing {{currentItem}} of {{totalData}} Entry(s)</a>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortContentName()">Content Name <fa icon="sort" v-if="contentName.sort == true"></fa><fa icon="sort-up" v-if="contentName.sortup == true"></fa><fa icon="sort-down" v-if="contentName.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortEmployeeName()">Content Category <fa icon="sort" v-if="contentCategory.sort == true"></fa><fa icon="sort-up" v-if="contentCategory.sortup == true"></fa><fa icon="sort-down" v-if="contentCategory.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedBy()">Created By <fa icon="sort" v-if="createdBy.sort == true"></fa><fa icon="sort-up" v-if="createdBy.sortup == true"></fa><fa icon="sort-down" v-if="createdBy.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortPosition()">Position <fa icon="sort" v-if="position.sort == true"></fa><fa icon="sort-up" v-if="position.sortup == true"></fa><fa icon="sort-down" v-if="position.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortApprovalStatus()">Approval Status <fa icon="sort" v-if="approvalStatus.sort == true"></fa><fa icon="sort-up" v-if="approvalStatus.sortup == true"></fa><fa icon="sort-down" v-if="approvalStatus.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortContenLevel()">Content Level <fa icon="sort" v-if="contenLevel.sort == true"></fa><fa icon="sort-up" v-if="contenLevel.sortup == true"></fa><fa icon="sort-down" v-if="contenLevel.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortActionBy()">Action By <fa icon="sort" v-if="actionBy.sort == true"></fa><fa icon="sort-up" v-if="actionBy.sortup == true"></fa><fa icon="sort-down" v-if="actionBy.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortLearningDate()">Action Date <fa icon="sort" v-if="learningDate.sort == true"></fa><fa icon="sort-up" v-if="learningDate.sortup == true"></fa><fa icon="sort-down" v-if="learningDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="3">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="data in approvalContentList" :key="data.approvalId">
                                    <td>
                                        {{data.contentName}}
                                    </td>
                                    <td>
                                        {{data.contentCategory}}
                                    </td>
                                    <td>
                                        {{data.createdBy}}
                                    </td>
                                    <td>
                                        {{data.position}}
                                    </td>
                                    <td>
                                        {{data.approvalStatus}}
                                    </td>
                                    <td>
                                        {{data.currentLevel}}
                                    </td>
                                    <td>
                                        {{data.approvalBy}}
                                    </td>
                                    <td>
                                        {{data.approvalDate | dateFormat}}
                                    </td>
                                    <td class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_orange talent_font_white" @click.prevent="gotoDetailPage(data.detailId, data.contentCategory)">View Detail</button>
                                    </td>
                                    <td class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_green talent_font_white" :disabled="isButtonDisabled(data)" @click.prevent="approveData(data.approvalId)">Approve</button>
                                    </td>
                                    <td class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" :disabled="isButtonDisabled(data)" @click.prevent="rejectData(data.approvalId)">Reject</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>

                    <!-- paginate -->
                    <div class="col-md-12 d-flex justify-content-center mt-3">
                        <paginate :currentPage.sync="currentPage" :total-data=totalData :limit-data=currentItemPage @update:currentPage="getDataApprovalContent()"></paginate>
                    </div>

                    <!-- <div>
                        <nav aria-label="Page navigation example">
                            <ul class="pagination">
                                <li class="page-item">
                                    <paginate v-model="currentPage"
                                            :page-count="totalPage"
                                            :page-range="3"
                                            :margin-pages="2"
                                            :click-handler="goToPage"
                                            :prev-text="'Prev'"
                                            :next-text="'Next'"
                                            :container-class="'pagination'"
                                            :page-class="'page-item'">
                                    </paginate>
                                </li>
                            </ul>
                        </nav>
                    </div> -->
                    <!-- end paginate -->
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { Sort } from '../../class/Sort';
    import { ContentService, DropdownService, ResponseApprovalContentModel, ApprovalContentViewModel, ApprovalContentFilterModel, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService';

    @Component({
        props: ['framework', 'compiler'],
        mounted: async function (this: ApprovalContent) {
            await this.fetching();
        }
    })
    export default class ApprovalContent extends Vue {

        props: any = {
            placeholder: "",
            readonly: true
        };
        //flag
        isShowDetail: boolean = false;
        isLoading: boolean = false;
        isBusy: boolean = true;
        //

        isShowErrorMessage: boolean = false;
        theShowErrorMessage: string;

        framework: string;
        compiler: string;
        looping: Array<number> = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

        currentPage: number = 1;
        currentItem: number = 0;
        currentItemPage: number = 10;
        totalData: number = 0;

        rangePicker = {
            start: null,
            end: null
        }

        //service
        contentServiceMan: ContentService = new ContentService();
        dropdownServiceMan: DropdownService = new DropdownService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        //variable list content
        selectedApprovalStatus: string = "";
        selectedContentCategory: string = "";

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        //list
        approvalContentList: ApprovalContentViewModel[] = [];
        contentCategoryList: string[] = [];
        approvalStatusList: string[] = [];
        //filter model
        approvalContentFilter: ApprovalContentFilterModel = {
            page: 0,
            itemPage: 0,
            startDate: null,
            endDate: null,
            contentName: '',
            contentCategory: '',
            createdBy: '',
            contentPosition: '',
            approvalStatus: '',
            orderBy: ''
        };

        //Variable untuk sorting
        contentName = new Sort();
        contentCategory = new Sort();
        createdBy = new Sort();
        position = new Sort();
        contenLevel = new Sort();
        approvalStatus = new Sort();
        learningDate = new Sort();
        actionBy = new Sort();

        // ---------------------- fetch ---------------------------

        async fetching() {
            await this.getDataApprovalContent();
            await this.getContentListData();
            await this.getApprovalStatus();
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("AC");
            this.crud = data
        }

        // --------------- service / main function --------------------

        setFilter() {
            this.approvalContentFilter.page = this.currentPage;
            this.approvalContentFilter.itemPage = this.currentItemPage;
            this.approvalContentFilter.approvalStatus = this.selectedApprovalStatus;
            this.approvalContentFilter.contentCategory = this.selectedContentCategory;
            this.approvalContentFilter.startDate = this.rangePicker.start;
            this.approvalContentFilter.endDate = this.rangePicker.end;

            this.approvalContentFilter.endDate ? this.approvalContentFilter.endDate.setHours(23, 59, 59) : this.approvalContentFilter.endDate;
        }

        async getDataApprovalContent() {

            this.setFilter();
            try {
                this.isBusy = true;
                let getData = await this.contentServiceMan.getPaginate(this.approvalContentFilter);
                if (getData != null) {
                    // this.approvalContentList = getData;
                    this.approvalContentList = getData.contentList;
                    this.totalData = getData.totalData;
                    this.currentItem = getData.contentList.length;
                }
            } catch{
                this.isShowErrorMessage = true;
                this.theShowErrorMessage = "Failed to Get Data."
                this.isBusy = false;
            }
            this.isBusy = false;
        }

        async approveData(id: number) {
            //this.isBusy = true;
            await this.contentServiceMan.changeStatus(id, "Approved");
            //this.isBusy = false;
            this.resetPageToDefault();
            await this.getDataApprovalContent();
        }

        async rejectData(id: number) {
            //this.isBusy = true;
            await this.contentServiceMan.changeStatus(id, "Rejected");
            //this.isBusy = false;
            this.resetPageToDefault();
            await this.getDataApprovalContent();
        }

        async showDetailData() {

        }

        async getContentListData() {
            let getList = await this.dropdownServiceMan.getContentCategory();
            this.contentCategoryList = getList;
        }

        async getApprovalStatus() {
            let getList = await this.dropdownServiceMan.getApprovalStatus();
            this.approvalStatusList = getList;
        }

        // ---------------- side function -------------------
        testinputconsole() {
            console.log();
        }

        isButtonDisabled(data: ApprovalContentViewModel) {
            return !data.isApprovableByUser;
        }

        gotoDetailPage(id: number, type: string) {
            var result = type.toLowerCase().search("setup course");
            console.log(type, result);
            if (type.toLowerCase().search("setup course") != -1) {
                window.location.href = `/Setup/SetupCourse/View?SetupCourseId=${id}&fromoutside=true`;
                return;
            }
            if (type.toLowerCase().search("course") != -1) {
                window.location.href = `/masterdata/course?courseId=${id}&fromOutside=true`;
                return;
            }
            if (type.toLowerCase().search("guide") != -1) {
                window.location.href = `/masterdata/guide?guideId=${id}&fromOutside=true`;
                return;
            }
            if (type.toLowerCase().search("banner") != -1) {
                window.location.href = `/masterdata/banner?bannerId=${id}&fromOutside=true`;
                return;
            }
            if (type.toLowerCase().search("event") != -1) {
                window.location.href = `/masterdata/event?eventId=${id}&fromOutside=true`;
                return;
            }
            if (type.toLowerCase().search("module") != -1) {
                window.location.href = `/masterdata/module?moduleId=${id}&fromOutside=true`;
                return;
            }
            if (type.toLowerCase().search("news") != -1) {
                window.location.href = `/MasterData/News?type=fromOutside&newsId=${id}`;
                return;
            }
            if (type.toLowerCase().search("training") != -1) {
                window.location.href = `/setup/releasetraining?trainingId=${id}&fromOutside=true`;
                return;
            }
            if (type.toLowerCase().search("setup module") != -1) {
                window.location.href = `/setup/setupmodule/View?setupModuleId=${id}&fromOutside=true`;
                return;
            }
            if (type.toLowerCase().search("pop quiz") != -1) {
                window.location.href = `/setup/setuppopupquiz/View?popQuizId=${id}&fromOutside=true`;
                return;
            }
            if (type.toLowerCase().search("task") != -1) {
                let question = type.split(" - ")[1];
                window.location.href = `/setup/tasks/${question}/View?taskId=${id}&fromOutside=true`;
                return;
            }
            if (type.toLowerCase().search("survey") != -1) {
                window.location.href = `/MasterData/Survey?type=fromoutside&surveyId=${id}`;
                return;
            }
        }

        async goToPage(page) {
            this.currentPage = page;
            await this.getDataApprovalContent();
        }

        get totalPage() {
            return Math.ceil(this.totalData / this.currentItemPage);
        }

        resetPageToDefault() {
            this.currentPage = 1;
            this.currentItemPage = 10;
        }

        async searchByInput() {
            this.resetPageToDefault();
            await this.getDataApprovalContent();
        }

        async resetAll() {
            this.approvalContentFilter = {
                page: 0,
                itemPage: 0,
                startDate: null,
                endDate: null,
                contentName: '',
                contentCategory: '',
                createdBy: '',
                contentPosition: '',
                approvalStatus: '',
                orderBy: ''
            };

            this.rangePicker = {
                start: null,
                end: null
            }

            this.selectedApprovalStatus = "";
            this.selectedContentCategory = "";
            this.resetPageToDefault();

            this.contentName.reset();
            this.contentCategory.reset();
            this.createdBy.reset();
            this.position.reset();
            this.contenLevel.reset();
            this.approvalStatus.reset();
            this.learningDate.reset();
            this.actionBy.reset();

            await this.getDataApprovalContent();
        }

        ResetErrorMessage() {
            this.isShowErrorMessage = false;
            this.theShowErrorMessage = null;
        }

        async setContentCategory() {

        }

        async setApprovalStatus() {

        }

        // ----------------- sorting handler dan function -------------------
        //ClickSortContentName
        async ClickSortContentName() {
            this.ResetSort('contentName');
            //Sorting
            this.contentName.sorting();
            //Function Sorting
            if (this.contentName.sortup == true && this.contentName.sortdown == false) {
                this.approvalContentFilter.orderBy = "ContentNameUp";
            }
            else if (this.contentName.sortup == false && this.contentName.sortdown == true) {
                this.approvalContentFilter.orderBy = "ContentNameDown";
            } else {
                this.approvalContentFilter.orderBy = "";
            }

            await this.getDataApprovalContent();
            return;
        }

        //ClickSortEmployeeName
        async ClickSortEmployeeName() {
            this.ResetSort('contentCategory');
            //Sorting
            this.contentCategory.sorting();
            //Function Sorting
            if (this.contentCategory.sortup == true && this.contentCategory.sortdown == false) {
                this.approvalContentFilter.orderBy = "ContentCategoryUp";
            }
            else if (this.contentCategory.sortup == false && this.contentCategory.sortdown == true) {
                this.approvalContentFilter.orderBy = "ContentCategoryDown";
            } else {
                this.approvalContentFilter.orderBy = "";
            }

            await this.getDataApprovalContent();
            return;
        }

        //ClickSortCreatedBy
        async ClickSortCreatedBy() {
            this.ResetSort('createdBy');
            //Sorting
            this.createdBy.sorting();
            //Function Sorting
            if (this.createdBy.sortup == true && this.createdBy.sortdown == false) {
                this.approvalContentFilter.orderBy = "CreatedByUp";
            }
            else if (this.createdBy.sortup == false && this.createdBy.sortdown == true) {
                this.approvalContentFilter.orderBy = "CreatedByDown";
            } else {
                this.approvalContentFilter.orderBy = "";
            }

            await this.getDataApprovalContent();
            return;
        }

        //ClickSortPosition
        async ClickSortPosition() {
            this.ResetSort('position');
            //Sorting
            this.position.sorting();
            //Function Sorting
            if (this.position.sortup == true && this.position.sortdown == false) {
                this.approvalContentFilter.orderBy = "PositionUp";
            }
            else if (this.position.sortup == false && this.position.sortdown == true) {
                this.approvalContentFilter.orderBy = "PositionDown";
            } else {
                this.approvalContentFilter.orderBy = "";
            }

            await this.getDataApprovalContent();
            return;
        }

        //ClickSortApprovalStatus
        async ClickSortApprovalStatus() {
            this.ResetSort('approvalStatus');
            //Sorting
            this.approvalStatus.sorting();
            //Function Sorting
            if (this.approvalStatus.sortup == true && this.approvalStatus.sortdown == false) {
                this.approvalContentFilter.orderBy = "ApprovalStatusUp";
            }
            else if (this.approvalStatus.sortup == false && this.approvalStatus.sortdown == true) {
                this.approvalContentFilter.orderBy = "ApprovalStatusDown";
            }

            await this.getDataApprovalContent();
            return;
        }

        //ClickSortLearningDate
        async ClickSortLearningDate() {
            this.ResetSort('learningDate');
            //Sorting
            this.learningDate.sorting();
            //Function Sorting
            if (this.learningDate.sortup == true && this.learningDate.sortdown == false) {
                this.approvalContentFilter.orderBy = "LearningDateUp";
            }
            else if (this.learningDate.sortup == false && this.learningDate.sortdown == true) {
                this.approvalContentFilter.orderBy = "LearningDateDown";
            } else {
                this.approvalContentFilter.orderBy = "";
            }

            await this.getDataApprovalContent();
            return;
        }

        //ClickSortLearningDate
        async ClickSortContenLevel() {
            this.ResetSort('contenLevel');
            //Sorting
            this.contenLevel.sorting();
            //Function Sorting
            if (this.contenLevel.sortup == true && this.contenLevel.sortdown == false) {
                this.approvalContentFilter.orderBy = "ContenLevelUp";
            }
            else if (this.contenLevel.sortup == false && this.contenLevel.sortdown == true) {
                this.approvalContentFilter.orderBy = "ContenLevelDown";
            } else {
                this.approvalContentFilter.orderBy = "";
            }

            await this.getDataApprovalContent();
            return;
        }

        //ClickSortLearningDate
        async ClickSortActionBy() {
            this.ResetSort('learningDate');
            //Sorting
            this.actionBy.sorting();
            //Function Sorting
            if (this.actionBy.sortup == true && this.actionBy.sortdown == false) {
                this.approvalContentFilter.orderBy = "ActionByUp";
            }
            else if (this.actionBy.sortup == false && this.actionBy.sortdown == true) {
                this.approvalContentFilter.orderBy = "ActionByDown";
            } else {
                this.approvalContentFilter.orderBy = "";
            }

            await this.getDataApprovalContent();
            return;
        }

        //Reset Sorting
        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'contentName':
                    this.contentCategory.reset();
                    this.createdBy.reset();
                    this.position.reset();
                    this.approvalStatus.reset();
                    this.learningDate.reset();
                    this.contenLevel.reset();
                    return;
                case 'contentCategory':
                    this.contentName.reset();
                    this.createdBy.reset();
                    this.position.reset();
                    this.approvalStatus.reset();
                    this.contenLevel.reset();
                    this.learningDate.reset();
                    return;
                case 'createdBy':
                    this.contentName.reset();
                    this.contentCategory.reset();
                    this.position.reset();
                    this.approvalStatus.reset();
                    this.contenLevel.reset();
                    this.learningDate.reset();
                    return;
                case 'position':
                    this.contentName.reset();
                    this.contentCategory.reset();
                    this.createdBy.reset();
                    this.approvalStatus.reset();
                    this.contenLevel.reset();
                    this.learningDate.reset();
                    return;
                case 'contenLevel':
                    this.contentName.reset();
                    this.contentCategory.reset();
                    this.createdBy.reset();
                    this.position.reset();
                    this.approvalStatus.reset();
                    this.learningDate.reset();
                    return;
                case 'approvalStatus':
                    this.contentName.reset();
                    this.contentCategory.reset();
                    this.createdBy.reset();
                    this.position.reset();
                    this.contenLevel.reset();
                    this.learningDate.reset();
                    return;
                case 'learningDate':
                    this.contentName.reset();
                    this.contentCategory.reset();
                    this.createdBy.reset();
                    this.position.reset();
                    this.contenLevel.reset();
                    this.approvalStatus.reset();
                    return;
            }
        }
    }
</script>
