<template>
    <div>
        <div class="row">
            <div class="col col-md-12">

                <!-- buat loading -->
                <loading-overlay :loading="isBusy"></loading-overlay>

                <!--TITLE-->
                <div v-if="isView == true">
                    <h3>
                        <fa icon="database"></fa>Master Data >
                        <span class="talent_font_red">News</span>
                    </h3>
                    <br />
                    <!-- success message -->
                    <div v-if="isShowMessage == true" class="alert alert-success alert-dismissible fade show" role="alert">
                        {{theShowMessage}}
                        <button type="button" class="close" data-dismiss="alert" @click.prevent="ResetSuccessMessage()" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
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
                        <h4>Search News</h4>
                        <br />
                        <!--2 Column Menu-->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="row align-items-center">
                                    <div class="col-md-2">
                                        <span>Date</span>
                                    </div>
                                    <div class="col-md-10">
                                        <div class="input-group talent_front">
                                            <v-date-picker class="v-date-style-report"
                                                           mode="range"
                                                           v-model="rangePicker.DateFilter"
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

                            <div class="col-md-6">
                                <div class="row align-items-center">
                                    <div class="col-md-2">
                                        <span>News Category</span>
                                    </div>
                                    <div class="col-md-10">
                                        <multiselect v-model="selectedNewsCategory"
                                                     :options="newsCategoryList"
                                                     :searchable="true"
                                                     :allow-empty="true"
                                                     label="name"
                                                     track-by="id"
                                                     @input="setNewsCategory()">
                                        </multiselect>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <br />

                        <!--2 Column Menu-->
                        <div class="row">
                            <div class="col-md-6">
                                <div class="row align-items-center">
                                    <div class="col-md-2">
                                        <span>News Title</span>
                                    </div>
                                    <div class="col-md-10">
                                        <div class="input-group">
                                            <input type="text" class="form-control" v-model="newsFilterModel.newsTitle" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-6">
                                <div class="row align-items-center">
                                    <div class="col-md-2">
                                        <span>Status</span>
                                    </div>
                                    <div class="col-md-10">
                                        <multiselect v-model="selectedNewsStatus"
                                                     :options="newsStatusList"
                                                     :searchable="true"
                                                     :allow-empty="true">
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
                                            <button class="btn talent_bg_blue talent_font_white" @click.prevent="searchByInput()">Search</button>
                                            <button class="btn talent_bg_red talent_font_white" @click.prevent="resetAll()">Reset</button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <br />

                    <!--Table-->
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <h4>News List</h4>
                        <div class="col-md-12 talent_magin_small">
                            <div class="row align-items-center row justify-content-between">
                                <a>Showing {{cureentItem}} of {{totalData}} Entry(s)</a>
                                <button v-if="crud.create" class="btn talent_bg_cyan talent_roundborder talent_font_white"
                                        @click.prevent="goToAddPage()">
                                    Add News
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
                                               @click.prevent="ClickSortNewsTitle()">
                                                News Title
                                                <fa icon="sort" v-if="newsTitle.sort == true"></fa>
                                                <fa icon="sort-up" v-if="newsTitle.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="newsTitle.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortNewsCategory()">
                                                News Category
                                                <fa icon="sort" v-if="newsCategory.sort == true"></fa>
                                                <fa icon="sort-up" v-if="newsCategory.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="newsCategory.sortdown == true"></fa>
                                            </a>
                                        </th>
                                        <th>
                                            <a href="#"
                                               class="talent_sort"
                                               @click.prevent="ClickSortStatus()">
                                                Status
                                                <fa icon="sort" v-if="status.sort == true"></fa>
                                                <fa icon="sort-up" v-if="status.sortup == true"></fa>
                                                <fa icon="sort-down" v-if="status.sortdown == true"></fa>
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
                                        <th colspan="3">Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr v-for="data in newsList" :key="data.id">
                                        <td>{{data.newsTitle}}</td>
                                        <td>{{data.newsCategory}}</td>
                                        <td>{{data.newsStatus}}</td>
                                        <td>{{data.createdDate | dateFormat}}</td>
                                        <td>{{data.updatedDate | dateFormat}}</td>
                                        <td v-if="crud.read" class="talent_nopadding_important">
                                            <button class="btn btn-block talent_bg_orange talent_font_white"
                                                    @click.prevent="goToDetailPage(data.id)">
                                                View Detail
                                            </button>
                                        </td>
                                        <td v-if="crud.update" class="talent_nopadding_important">
                                            <button :disabled="data.newsStatus === 'Waiting for Approval'" class="btn btn-block talent_bg_cyan talent_font_white"
                                                    @click.prevent="goToUpdatePage(data.id)">
                                                Edit
                                            </button>
                                        </td>
                                        <td v-if="crud.delete" class="talent_nopadding_important">
                                            <button :disabled="data.newsStatus === 'Waiting for Approval'" class="btn btn-block talent_bg_red talent_font_white"
                                                    data-toggle="modal" data-target="#modalDelete"
                                                    @click.prevent="setDeleteData(data.id)">
                                                Remove
                                            </button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <div class="col-md-12 d-flex justify-content-center mt-3">
                            <paginate :currentPage.sync="cureentPage" :total-data=totalData :limit-data=cureentItemPage @update:currentPage="getDataNews()"></paginate>
                        </div>
                    </div>
                </div>

                <!-- modal buat delete -->
                <div class="modal fade" id="modalDelete" tabindex="-1" role="dialog">
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
                                        <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="deleteData()" :disabled="!crud.delete">Delete</button>
                                        <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" data-dismiss="modal">Close</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <br />
                <!-- form section -->
                <news-form v-if="isView == false" :type.sync="type" :the-show-message.sync="theShowMessage" :is-show-message.sync="isShowMessage" :the-id="formId" @update:type="fetching()"></news-form>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { Sort } from '../../class/Sort';
    import { NewsService, MasterNewsFilterModel, DropdownService, MasterNewsViewModel, DropDownModel, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService';

    @Component({
        props: ['type', 'id', 'isView', 'isShowMessage', 'theShowMessage'],
        mounted: async function (this: News) {
            await this.getAccess();
            await this.fetching();
        }
    })
    export default class News extends Vue {
        props: any = {
            placeholder: '',
            readonly: true
        };

        type: string;
        id: string;
        deletedId: number = 0;

        isShowMessage: boolean = false;
        theShowMessage: string;

        isShowErrorMessage: boolean = false;
        theShowErrorMessage: string;

        fileUpload: string[] = ['asd'];

        updateType: string = "update";
        addType: string = "add";
        detailType: string = "detail";
        fromOutsideType: string = "fromOutside";
        viewType: string = "view";

        isBusy: boolean = true;

        //boolean buat view
        isView: boolean = null;
        isInsert: boolean = false;
        isUpdate: boolean = false;
        isDetail: boolean = false;

        //variable
        cureentPage: number = 1;
        cureentItem: number = 0;
        cureentItemPage: number = 10;
        totalData: number = 0;

        formId: number = 0;
        formType: string = "";

        newsFilterModel: MasterNewsFilterModel = {
            page: 0,
            itemPage: 0,
            startDate: null,
            endDate: null,
            newsTitle: "",
            newsCategory: "",
            newsStatus: "",
            orderBy: "",
        }

        //service
        newsServiceMan: NewsService = new NewsService();
        dropdownServiceMan: DropdownService = new DropdownService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        // date variable
        rangePicker: RangePickerInterface = {
            DateFilter: {
                end: null,
                start: null
            }
        }

        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        //select variable
        selectedNewsCategory: DropDownModel = {
            id: 0,
            name: ""
        };
        selectedNewsStatus: string = "";

        //list showed variable
        newsList: MasterNewsViewModel[] = [];
        newsCategoryList: DropDownModel[] = [];
        newsStatusList: string[] = [];

        //Variable untuk sorting
        newsTitle = new Sort();
        newsCategory = new Sort();
        status = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //service pertama
        async fetching() {
            this.formType = this.type;
            this.formId = Number(this.id);

            this.setType();
            if (this.isView == true) {
                await this.getDataNews();
                await this.getContentListData();
                await this.getApprovalStatus();
            }
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("NWS");
            this.crud = data
        }

        setType() {
            if (this.formType != null && this.formType != undefined) {
                if (this.formType.toLowerCase().search("add") != -1) {
                    this.isInsert = true;
                    this.isUpdate = false;
                    this.isDetail = false;
                    this.isView = false;
                    this.isBusy = false;
                }
                else if (this.formType.toLowerCase().search("update") != -1) {
                    this.isInsert = false;
                    this.isUpdate = true;
                    this.isDetail = false;
                    this.isView = false;
                    this.isBusy = false;
                }
                else if (this.formType.toLowerCase().search("detail") != -1) {
                    this.isInsert = false;
                    this.isUpdate = false;
                    this.isDetail = true;
                    this.isView = false;
                    this.isBusy = false;
                }
                else if (this.formType.toLowerCase().search("fromoutside") != -1) {
                    this.isInsert = false;
                    this.isUpdate = false;
                    this.isDetail = true;
                    this.isView = false;
                    this.isBusy = false;
                }
                else if (this.formType.toLowerCase().search("view") != -1) {
                    this.isInsert = false;
                    this.isUpdate = false;
                    this.isDetail = false;
                    this.isView = true;
                }
                else {
                    this.isInsert = false;
                    this.isUpdate = false;
                    this.isDetail = false;
                    this.isView = true;
                }
            }
        }

        // --------------- service / main function --------------------

        setFilter() {
            this.newsFilterModel.page = this.cureentPage;
            this.newsFilterModel.itemPage = this.cureentItemPage;
            this.newsFilterModel.newsCategory = this.selectedNewsCategory.name;
            this.newsFilterModel.newsStatus = this.selectedNewsStatus;
            this.newsFilterModel.startDate = this.rangePicker.DateFilter.start;
            this.newsFilterModel.endDate = this.rangePicker.DateFilter.end;
        }

        async getDataNews() {

            this.setFilter();
            try {
                this.isBusy = true;
                let getData = await this.newsServiceMan.getPaginate(this.newsFilterModel);
                if (getData != null) {
                    this.newsList = getData.contentList;
                    this.totalData = getData.totalData;
                    this.cureentItem = getData.contentList.length;
                    this.isBusy = false;
                }
            } catch{
                this.isShowErrorMessage = true;
                this.theShowErrorMessage = "Failed to Get Data."
                this.isBusy = false;
            }
        }

        async deleteData() {
            if (this.crud.delete == false) {
                return;
            }
            if (this.deletedId != null) {
                let message = await this.newsServiceMan.delete(this.deletedId);
                this.resetPageToDefault();
                this.notif(message);
            }
        }

        async getContentListData() {
            let getList = await this.dropdownServiceMan.getNewsList();
            this.newsCategoryList = getList;
        }

        async getApprovalStatus() {
            let getList = await this.dropdownServiceMan.getApprovalStatus();
            this.newsStatusList = getList;
        }

        // ---------------- side function -------------------

        notif(result: string) {
            if (result.toLowerCase().search("success") != -1) {
                this.theShowMessage = result;
                this.isShowMessage = true;
                this.fetching();
            }
            else {
                this.fetching();
            }
        }

        setDeleteData(id: number) {
            this.deletedId = id;
        }

        async goToDetailPage(id: number) {
            if (this.crud.read == false) {
                return;
            }
            this.type = this.detailType;
            this.id = id + "";
            await this.fetching();
        }

        async goToUpdatePage(id: number) {
            if (this.crud.update == false) {
                return;
            }
            this.resetSortingAndAdvanceSearch();
            this.type = this.updateType;
            this.id = id + "";
            await this.fetching();
        }

        async goToAddPage() {
            if (this.crud.create == false) {
                return;
            }
            this.resetSortingAndAdvanceSearch();
            this.type = this.addType;
            await this.fetching();
        }

        async goToPage(page) {
            this.cureentPage = page;
            await this.getDataNews();
        }

        get totalPage() {
            return Math.ceil(this.totalData / this.cureentItemPage);
        }

        resetPageToDefault() {
            this.cureentPage = 1;
            this.cureentItemPage = 10;
        }

        ResetSuccessMessage() {
            this.isShowMessage = false;
            this.theShowMessage = null;
        }

        ResetErrorMessage() {
            this.isShowErrorMessage = false;
            this.theShowErrorMessage = null;
        }

        async searchByInput() {
            this.resetPageToDefault();
            await this.getDataNews();
        }

        async resetAll() {

            this.resetSortingAndAdvanceSearch();

            this.resetPageToDefault();
            await this.getDataNews();
        }

        resetSortingAndAdvanceSearch() {
            this.newsFilterModel = {
                page: 0,
                itemPage: 0,
                startDate: null,
                endDate: null,
                newsTitle: "",
                newsCategory: "",
                newsStatus: "",
                orderBy: "",
            }

            this.rangePicker = {
                DateFilter: {
                    start: null,
                    end: null
                }
            }

            this.selectedNewsStatus = "";
            this.selectedNewsCategory = {
                id: 0,
                name: ""
            }

            this.ResetSort('');

        }

        async setNewsStatus() {

        }
        async setNewsCategory() {
            this.newsFilterModel.newsCategory = this.selectedNewsCategory.name;
        }

        //Add File Upload
        AddFile() {
            this.fileUpload.push('asd');
        }

        RemoveFile(indexToDelete: number) {
            this.fileUpload.splice(indexToDelete, 1);
        }

        //ClickSortNewsTitle
        async ClickSortNewsTitle() {
            this.ResetSort('newsTitle');
            //Sorting
            this.newsTitle.sorting();
            //Function Sorting
            if (this.newsTitle.sortup == true && this.newsTitle.sortdown == false) {
                this.newsFilterModel.orderBy = "NewsTitleUp";
            }
            else if (this.newsTitle.sortup == false && this.newsTitle.sortdown == true) {
                this.newsFilterModel.orderBy = "NewsTitleDown";
            }

            await this.getDataNews();
            return;
        }

        //ClickSortNewsCategory
        async ClickSortNewsCategory() {
            this.ResetSort('newsCategory');
            //Sorting
            this.newsCategory.sorting();
            //Function Sorting
            if (this.newsCategory.sortup == true && this.newsCategory.sortdown == false) {
                this.newsFilterModel.orderBy = "NewsCategoryUp";
            }
            else if (this.newsCategory.sortup == false && this.newsCategory.sortdown == true) {
                this.newsFilterModel.orderBy = "NewsCategoryDown";
            }

            await this.getDataNews();

            return;
        }

        //ClickSortStatus
        async ClickSortStatus() {
            this.ResetSort('status');
            //Sorting
            this.status.sorting();
            //Function Sorting
            if (this.status.sortup == true && this.status.sortdown == false) {
                this.newsFilterModel.orderBy = "NewsStatusUp";
            }
            else if (this.status.sortup == false && this.status.sortdown == true) {
                this.newsFilterModel.orderBy = "NewsStatusDown";
            }

            await this.getDataNews();
            return;
        }

        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting
            if (this.createdDate.sortup == true && this.createdDate.sortdown == false) {
                this.newsFilterModel.orderBy = "CreatedDateUp";
            }
            else if (this.createdDate.sortup == false && this.createdDate.sortdown == true) {
                this.newsFilterModel.orderBy = "CreatedDateDown";
            }

            await this.getDataNews();
            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting
            if (this.updatedDate.sortup == true && this.updatedDate.sortdown == false) {
                this.newsFilterModel.orderBy = "UpdatedDateUp";
            }
            else if (this.updatedDate.sortup == false && this.updatedDate.sortdown == true) {
                this.newsFilterModel.orderBy = "UpdatedDateDown";
            }

            await this.getDataNews();
            return;
        }

        //Reset Sorting
        ResetSort(parameter: string) {
            switch (parameter) {
                case 'newsTitle':
                    this.newsCategory.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'newsCategory':
                    this.newsTitle.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'status':
                    this.newsTitle.reset();
                    this.newsCategory.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.newsTitle.reset();
                    this.newsCategory.reset();
                    this.status.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.newsTitle.reset();
                    this.newsCategory.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    return;
                default:
                    this.newsFilterModel.orderBy = "";
                    this.newsTitle.reset();
                    this.newsCategory.reset();
                    this.status.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
            }
        }
    }

    interface RangePickerInterface {
        DateFilter: {
            start?: Date;
            end?: Date;
        };
    }
</script>