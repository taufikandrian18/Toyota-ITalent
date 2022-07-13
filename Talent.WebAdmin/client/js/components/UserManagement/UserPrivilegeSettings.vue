<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div v-if="add == false && edit == false" class="row">
            <div class="col col-md-12">
                <h3><fa icon="user"></fa> User Management > <span class="talent_font_red">User Privilege Settings</span></h3>
                <br />
                <div v-if="successMessageShow == true" class="alert alert-success alert-dismissible fade show" role="alert">
                    {{successMessage}}
                    <button type="button" class="close" data-dismiss="alert" @click.prevent="ResetSuccessMessage()" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <!--ADVANCE SEARCH-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Search Privilege Settings</h4>
                    <br />
                    <!--2 Column Menu-->
                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Date</span>
                                </div>
                                <div class="col-md-8">
                                    <div class="input-group talent_front">
                                        <v-date-picker v-model="filter.Date" class="v-date-style-report" mode="range" :firstDayOfWeek="2" :value="null" :input-props="props" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                    <span>Menu</span>
                                </div>
                                <div class="col-md-8">
                                    <select v-model="filter.MenuId" class="form-control">
                                        <option v-for="menu in menuSearch" :value="menu.menuId">{{menu.menu}}</option>
                                    </select>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <!--2 Column Menu-->
                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>User Role</span>
                                </div>
                                <div class="col-md-8">
                                    <input v-model="filter.UserRole" class="form-control" />
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="row align-items-center">
                                <div class="col-md-4">
                                    <span>Page</span>
                                </div>
                                <div class="col-md-8">
                                    <select v-model="filter.PageId" class="form-control">
                                        <option v-for="page in pageSearch" :value="page.pageId">{{page.page}}</option>
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
                                        <button class="btn talent_bg_blue talent_font_white" @click.prevent="FetchData()">
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
                <br />
                <!--Table-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Privilege List</h4>

                    <div class="col-md-12 talent_magin_small">
                        <div class="row align-items-center row justify-content-between">
                            <a v-if="(userPrivilegeSettingsContent.totalData - ((currentPage - 1) * pageSize) ) >= pageSize">Showing {{pageSize}} of {{userPrivilegeSettingsContent.totalData}} Entry(s)</a>
                            <a v-else>Showing {{(userPrivilegeSettingsContent.totalData) - ((currentPage - 1) * pageSize)}} of {{userPrivilegeSettingsContent.totalData}} Entry(s)</a>
                            <button class="btn talent_bg_cyan talent_roundborder talent_font_white" @click.prevent="addClicked()" v-if="crud.create">New Privilege</button>
                        </div>
                    </div>

                    <div class="talent_overflowx">
                        <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                            <thead>
                                <tr>
                                    <th scope="col">
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUserRole()">User Role <fa icon="sort" v-if="userRoleSort.sort == true"></fa><fa icon="sort-up" v-if="userRoleSort.sortup == true"></fa><fa icon="sort-down" v-if="userRoleSort.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortMenu()">Menu <fa icon="sort" v-if="menuSort.sort == true"></fa><fa icon="sort-up" v-if="menuSort.sortup == true"></fa><fa icon="sort-down" v-if="menuSort.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortPage()">Page <fa icon="sort" v-if="pageSort.sort == true"></fa><fa icon="sort-up" v-if="pageSort.sortup == true"></fa><fa icon="sort-down" v-if="pageSort.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        Create
                                    </th>
                                    <th>
                                        Read
                                    </th>
                                    <th>
                                        Update
                                    </th>
                                    <th>
                                        Delete
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortCreatedDate()">Created Date <fa icon="sort" v-if="createdDate.sort == true"></fa><fa icon="sort-up" v-if="createdDate.sortup == true"></fa><fa icon="sort-down" v-if="createdDate.sortdown == true"></fa></a>
                                    </th>
                                    <th>
                                        <a href="#" class="talent_sort" @click.prevent="ClickSortUpdatedDate()">Updated Date <fa icon="sort" v-if="updatedDate.sort == true"></fa><fa icon="sort-up" v-if="updatedDate.sortup == true"></fa><fa icon="sort-down" v-if="updatedDate.sortdown == true"></fa></a>
                                    </th>
                                    <th colspan="2" v-if="crud.update || crud.delete">
                                        Action
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr v-for="(privilege, index) in userPrivilegeSettingsContent.data">
                                    <td>
                                        {{privilege.userRole}}
                                    </td>
                                    <td>
                                        {{privilege.menu}}
                                    </td>
                                    <td>
                                        {{privilege.page}}
                                    </td>
                                    <td>
                                        <a v-if="privilege.create == true"><fa icon="check"></fa></a>
                                        <a v-else><fa icon="times"></fa></a>
                                    </td>
                                    <td>
                                        <a v-if="privilege.read == true"><fa icon="check"></fa></a>
                                        <a v-else><fa icon="times"></fa></a>
                                    </td>
                                    <td>
                                        <a v-if="privilege.update == true"><fa icon="check"></fa></a>
                                        <a v-else><fa icon="times"></fa></a>
                                    </td>
                                    <td>
                                        <a v-if="privilege.delete == true"><fa icon="check"></fa></a>
                                        <a v-else><fa icon="times"></fa></a>
                                    </td>
                                    <td>
                                        {{privilege.createdAt | dateFormat}}
                                    </td>
                                    <td>
                                        {{privilege.updatedAt | dateFormat}}
                                    </td>
                                    <td v-if="crud.update" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_cyan talent_font_white" @click.prevent="GetUserRoleId(privilege.userRoleId)" >Edit</button>
                                    </td>
                                    <td v-if="crud.delete" class="talent_nopadding_important">
                                        <button class="btn btn-block talent_bg_red talent_font_white" @click.prevent="GetRemove(privilege.privilegeSettingsId)" data-toggle="modal" data-target="#modalDelete">Remove</button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="col-md-12 d-flex justify-content-center mt-3">
                        <paginate :currentPage.sync="currentPage" :total-data=userPrivilegeSettingsContent.totalData :limit-data=pageSize @update:currentPage="FetchData()"></paginate>
                    </div>
                </div>
                <br />
            </div>
        </div>

        <div v-if="add == true" class="row">
            <div class="col col-md-12">
                <!--Add User Privilege-->
                <UserPrivilegeSettingsAdd :add.sync="add" :success-message-show.sync="successMessageShow" :success-message.sync="successMessage" @update:add="FetchData()"></UserPrivilegeSettingsAdd>
            </div>
            <br />
        </div>

        <div v-if="edit == true" class="row">
            <div class="col col-md-12">
                <!--Add User Privilege-->
                <UserPrivilegeSettingsEdit :edit.sync="edit" :success-message-show.sync="successMessageShow" :success-message.sync="successMessage" :edit-id="editId" @update:edit="FetchData()"></UserPrivilegeSettingsEdit>
            </div>
            <br />
        </div>


        <!--MODAL FOR REMOVE-->
        <div class="modal fade" id="modalDelete" tabindex="-1" role="dialog">
            <loading-overlay :loading="isBusyRemove"></loading-overlay>
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="col-md-12 justify-content-center d-flex">
                            <h5>Are you sure want to delete?</h5>
                        </div>
                    </div>

                    <div class="modal-body">
                        <div class="col-md-12">
                            <h5>Menu</h5>
                        </div>
                        <div class="col-md-12 mb-4">
                            <div class="d-flex">
                                <div class="form-check form-check-inline" v-for="menu in viewPage">
                                    <checkbox v-model="menu.isCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16" name="menu" v-validate:menuLength="'min_value:1'" @change="SelectAllPageByMenuId(menu.menu, menu.isCheck)">{{menu.menu}}</checkbox>
                                </div>
                            </div>
                        </div>

                        <!--DASHBOARD-->
                        <div v-if="menuPageShow.includes('Dashboard')" class="mb-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-12">
                                        <h5>Dashboard</h5>
                                    </div>
                                </div>

                                <div class="row justify-content-between mb-2" v-for="(dashboard,index) in deleteDashboard">
                                    <div class="col-md-12">
                                        <checkbox v-model="dashboard.isCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{dashboard.page}}</checkbox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Report-->
                        <div v-if="menuPageShow.includes('Report')" class="mb-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-12">
                                        <h5>Report</h5>
                                    </div>
                                </div>

                                <div class="row justify-content-between mb-2" v-for="(dashboard,index) in deleteReports">
                                    <div class="col-md-12">
                                        <checkbox v-model="dashboard.isCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{dashboard.page}}</checkbox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--User Management-->
                        <div v-if="menuPageShow.includes('User Management')" class="mb-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-12">
                                        <h5>User Management</h5>
                                    </div>
                                </div>

                                <div class="row justify-content-between mb-2" v-for="(dashboard,index) in deleteForUserManagement">
                                    <div class="col-md-12">
                                        <checkbox v-model="dashboard.isCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{dashboard.page}}</checkbox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Setup-->
                        <div v-if="menuPageShow.includes('Setup')" class="mb-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-12">
                                        <h5>Setup</h5>
                                    </div>
                                </div>

                                <div class="row justify-content-between mb-2" v-for="(dashboard,index) in deleteForSetup">
                                    <div class="col-md-12">
                                        <checkbox v-model="dashboard.isCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{dashboard.page}}</checkbox>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!--Master Data-->
                        <div v-if="menuPageShow.includes('Master Data')" class="mb-4">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-12">
                                        <h5>Master Data</h5>
                                    </div>
                                </div>

                                <div class="row justify-content-between mb-2" v-for="(dashboard,index) in deleteForMasterData">
                                    <div class="col-md-12">
                                        <checkbox v-model="dashboard.isCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{dashboard.page}}</checkbox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="col-md-12 d-flex justify-content-center">
                            <div class="col-md-12 d-flex justify-content-around">
                                <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="Remove()">Delete</button>
                                <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click.prevent="CloseDelete()">Close</button>
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
    import { UserPrivilegeSettingsService, UserPrivilegeSettingsMenuModel, UserPrivilegeSettingsPageModel, UserAccessCRUD, UserPrivilegeSettingsUserRoleModel, UserPrivilegeSettingsPaginateModel } from '../../services/NSwagService';
    import { Sort } from '../../class/Sort';
    import UserPrivilegeSettingsAdd from '../../components/UserManagement/UserPrivilegeSettingsAdd.vue'
    import UserPrivilegeSettingsEdit from '../../components/UserManagement/UserPrivilegeSettingsEdit.vue'

    @Component({
        created: async function (this: UserPrivilegeSettings) {
            this.isBusy = true;
            await this.getAccess()
            //await this.GetAllUserRole()
            await this.GetAllMenu()
            await this.GetAllPage()
            await this.FetchData()
            this.isBusy = false;
        },
        components: {
            UserPrivilegeSettingsAdd,
            UserPrivilegeSettingsEdit
        }
    })
    export default class UserPrivilegeSettings extends Vue {

        //API
        privilegeSettingsAPI: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async getAccess() {
            var data = await this.privilegeSettingsAPI.crudAccessPage("UPS");
            this.crud = data
            console.log(this.crud);
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        isBusy: boolean = false;
        //Control
        add: boolean = false;
        edit: boolean = false;

        //Success Message
        successMessageShow: boolean = false;
        successMessage: string = ""

        ResetSuccessMessage() {
            this.successMessageShow = false;
            this.successMessage = "";
        }

        //DatePicker
        props: any = {
            placeholder: "",
            readonly: true
        };

        //Pagination
        currentPage: number = 1;
        pageSize: number = 10;


        //VARIABLE ADVANCE SEARCH
        filter: IUserPrivilegeSettingsFilter = { Date: { end: null, start: null }, MenuId: '', PageId: '', SortBy: '', PageIndex: this.currentPage, PageSize: this.pageSize, UserRole: '' }
        userRoleSearch: UserPrivilegeSettingsUserRoleModel[] = []
        menuSearch: UserPrivilegeSettingsMenuModel[] = []
        pageSearch: UserPrivilegeSettingsPageModel[] = []

        //Variable Table
        userPrivilegeSettingsContent: UserPrivilegeSettingsPaginateModel = { data: null, totalData: null }

        //Advance Search
        //GetAll Menu
        async GetAllMenu() {
            this.isBusy = true;
            var data = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsMenu()
            this.menuSearch = data;
            this.isBusy = false;
        }
        //GetAll Page
        async GetAllPage() {
            this.isBusy = true;
            var data = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsPage()
            this.pageSearch = data;
            this.isBusy = false;
        }

        //FetchData
        async FetchData() {
            this.isBusy = true;
            var data = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsPaginate(this.filter.Date.start, this.filter.Date.end, this.filter.MenuId, this.filter.UserRole, this.filter.PageId, this.filter.SortBy, this.currentPage, this.filter.PageSize);
            this.userPrivilegeSettingsContent.data = data.data
            this.userPrivilegeSettingsContent.totalData = data.totalData
            this.isBusy = false;
            console.log(this.userPrivilegeSettingsContent.data)
        }

        //Reset
        async Reset() {
            this.filter = { Date: { end: null, start: null }, MenuId: '', PageId: '', SortBy: '', PageIndex: this.currentPage, PageSize: this.pageSize, UserRole: '' }
            this.userRoleSort.reset();
            this.pageSort.reset();
            this.menuSort.reset();
            this.createdDate.reset();
            this.updatedDate.reset();
            await this.FetchData()
        }

        addClicked() {
            this.add = true;
            this.Reset();
        }

        editId: number = null
        GetUserRoleId(id: number) {
            this.Reset()
            this.editId = id
            console.log(id)
            this.edit = true;
        }

        //SORTING
        //Variable untuk sorting
        userRoleSort = new Sort();
        menuSort = new Sort();
        pageSort = new Sort();
        createdDate = new Sort();
        updatedDate = new Sort();

        //ClickSortProgramType
        async ClickSortUserRole() {
            this.ResetSort('userRoleSort');
            //Sorting
            this.userRoleSort.sorting();
            //Function Sorting
            if (this.userRoleSort.sortup == true) {
                this.filter.SortBy = "UserRoleAsc";
            } else if (this.userRoleSort.sortdown == true) {
                this.filter.SortBy = "UserRoleDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortLearningType
        async ClickSortMenu() {
            this.ResetSort('menuSort');
            //Sorting
            this.menuSort.sorting();
            //Function Sorting
            if (this.menuSort.sortup == true) {
                this.filter.SortBy = "MenuAsc";
            } else if (this.menuSort.sortdown == true) {
                this.filter.SortBy = "MenuDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortLevel
        async ClickSortPage() {
            this.ResetSort('pageSort');
            //Sorting
            this.pageSort.sorting();
            //Function Sorting
            if (this.pageSort.sortup == true) {
                this.filter.SortBy = "PageAsc";
            } else if (this.pageSort.sortdown == true) {
                this.filter.SortBy = "PageDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.FetchData()
            return;
        }


        //ClickSortCreatedDate
        async ClickSortCreatedDate() {
            this.ResetSort('createdDate');
            //Sorting
            this.createdDate.sorting();
            //Function Sorting
            if (this.createdDate.sortup == true) {
                this.filter.SortBy = "CreatedDateAsc";
            } else if (this.createdDate.sortdown == true) {
                this.filter.SortBy = "CreatedDateDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.FetchData()
            return;
        }

        //ClickSortUpdatedDate
        async ClickSortUpdatedDate() {
            this.ResetSort('updatedDate');
            //Sorting
            this.updatedDate.sorting();
            //Function Sorting
            if (this.updatedDate.sortup == true) {
                this.filter.SortBy = "UpdatedDateAsc";
            } else if (this.updatedDate.sortdown == true) {
                this.filter.SortBy = "UpdatedDateDesc"
            }
            else {
                this.filter.SortBy = "";
            }
            this.FetchData()
            return;
        }

        async ResetSort(parameter: string) {
            switch (parameter) {
                case 'userRoleSort':
                    this.pageSort.reset();
                    this.menuSort.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'menuSort':
                    this.userRoleSort.reset();
                    this.pageSort.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'pageSort':
                    this.userRoleSort.reset();
                    this.menuSort.reset();
                    this.createdDate.reset();
                    this.updatedDate.reset();
                    return;
                case 'createdDate':
                    this.userRoleSort.reset();
                    this.pageSort.reset();
                    this.menuSort.reset();
                    this.updatedDate.reset();
                    return;
                case 'updatedDate':
                    this.userRoleSort.reset();
                    this.pageSort.reset();
                    this.menuSort.reset();
                    this.createdDate.reset();
                    return;
            }
        }


        //Remove
        isBusyRemove: boolean = true;

        //Page
        deleteDashboard: { pageId: string, menuId: string, page: string, isCheck: boolean }[] = []
        deleteReports: { pageId: string, menuId: string, page: string, isCheck: boolean }[] = []
        deleteForUserManagement: { pageId: string, menuId: string, page: string, isCheck: boolean }[] = []
        deleteForSetup: { pageId: string, menuId: string, page: string, isCheck: boolean }[] = [];
        deleteForMasterData: { pageId: string, menuId: string, page: string, isCheck: boolean }[] = []

        //Menu
        viewPage: { menu: string, isCheck: boolean }[] = []
        menuPageShow: string[] = []

        getPageId: string[] = []
        privilegeSettingIdDelete: number

        //UserRoleId
        userRoleId: number = null

        async GetRemove(id: number) {
            if (!this.crud.delete) {
                return
            }
            this.privilegeSettingIdDelete = id
            this.userRoleId = await this.privilegeSettingsAPI.getUserRoleIdFromPrivilegePageMappingsId(this.privilegeSettingIdDelete)
            await this.GetAllUserRoleAccess(this.userRoleId)
        }
        async GetAllUserRoleAccess(id: number) {
            this.isBusyRemove = true;
            var data = await this.privilegeSettingsAPI.getUserPrivilegeSettingsByUserRoleId(id);
            console.log(data)
            for (var mp of data.menuPage) {
                this.getPageId.push(mp.pageId)
                switch (mp.menuId) {
                    case "DSB":
                        if (this.viewPage.find(Q => Q.menu == "Dashboard") == undefined) {
                            this.viewPage.push({ menu: "Dashboard", isCheck: false })
                            this.menuPageShow.push("Dashboard")
                        }
                        this.deleteDashboard.push({ pageId: mp.pageId, page: mp.page, menuId: mp.menuId, isCheck: false })
                        break;

                    case "MD":
                        if (this.viewPage.find(Q => Q.menu == "Master Data") == undefined) {
                            this.viewPage.push({ menu: "Master Data", isCheck: false })
                            this.menuPageShow.push("Master Data")
                        }
                        this.deleteForMasterData.push({ pageId: mp.pageId, page: mp.page, menuId: mp.menuId, isCheck: false })
                        break;

                    case "RPRT":
                        if (this.viewPage.find(Q => Q.menu == "Report") == undefined) {
                            this.viewPage.push({ menu: "Report", isCheck: false })
                            this.menuPageShow.push("Report")
                        }
                        this.deleteReports.push({ pageId: mp.pageId, page: mp.page, menuId: mp.menuId, isCheck: false })
                        break;

                    case "STP":
                        if (this.viewPage.find(Q => Q.menu == "Setup") == undefined) {
                            this.viewPage.push({ menu: "Setup", isCheck: false })
                            this.menuPageShow.push("Setup")
                        }
                        this.deleteForSetup.push({ pageId: mp.pageId, page: mp.page, menuId: mp.menuId, isCheck: false })
                        break;

                    case "UM":
                        if (this.viewPage.find(Q => Q.menu == "User Management") == undefined) {
                            this.viewPage.push({ menu: "User Management", isCheck: false })
                            this.menuPageShow.push("User Management")
                        }
                        this.deleteForUserManagement.push({ pageId: mp.pageId, page: mp.page, menuId: mp.menuId, isCheck: false })
                        break
                }
            }
            this.isBusyRemove = false;
        }

        async SelectAllPageByMenuId(menu: string, isCheck: boolean) {
            console.log("dorr")
            switch (menu) {
                case "Dashboard":
                    if (isCheck == true) {
                        for (var i of this.deleteDashboard) {
                            i.isCheck = true;
                        }
                    } else {
                        for (var i of this.deleteDashboard) {
                            i.isCheck = false;
                        }
                    }
                    break;
                case "Master Data":
                    if (isCheck == true) {
                        for (var i of this.deleteForMasterData) {
                            i.isCheck = true;
                        }
                    } else {
                        for (var i of this.deleteForMasterData) {
                            i.isCheck = false;
                        }
                    }
                    break;
                case "Report":
                    if (isCheck == true) {
                        for (var i of this.deleteReports) {
                            i.isCheck = true;
                        }
                    } else {
                        for (var i of this.deleteReports) {
                            i.isCheck = false;
                        }
                    }
                    break;
                case "Setup":
                    if (isCheck == true) {
                        for (var i of this.deleteForSetup) {
                            i.isCheck = true;
                        }
                    } else {
                        for (var i of this.deleteForSetup) {
                            i.isCheck = false;
                        }
                    }
                    break;
                case "User Management":
                    if (isCheck == true) {
                        for (var i of this.deleteForUserManagement) {
                            i.isCheck = true;
                        }
                    } else {
                        for (var i of this.deleteForUserManagement) {
                            i.isCheck = false;
                        }
                    }
                    break;
            }
        }

        async Remove() {
            if (!this.crud.delete) {
                return
            }
            this.isBusyRemove = true;
            this.isBusy = true;
            var pageId: string[] = [];
            //Dashboard
            console.log(this.deleteDashboard.length);
            if (this.deleteDashboard.length > 0) {
                for (var dashboard of this.deleteDashboard) {
                    if (dashboard.isCheck == true) {
                        pageId.push(dashboard.pageId)
                    }
                }
            }

            //Master Data
            if (this.deleteForMasterData.length > 0) {
                for (var i of this.deleteForMasterData) {
                    if (i.isCheck == true) {
                        pageId.push(i.pageId)
                    }
                }
            }

            //Master Setup
            if (this.deleteForSetup.length > 0) {
                for (var i of this.deleteForSetup) {
                    if (i.isCheck == true) {
                        pageId.push(i.pageId)
                    }
                }
            }

            //User Management
            if (this.deleteForUserManagement.length > 0) {
                for (var i of this.deleteForUserManagement) {
                    if (i.isCheck == true) {
                        pageId.push(i.pageId)
                    }
                }
            }

            //Report
            if (this.deleteReports.length > 0) {
                for (var i of this.deleteReports) {
                    if (i.isCheck == true) {
                        pageId.push(i.pageId)
                    }
                }
            }
            
            await this.privilegeSettingsAPI.deleteUserPrivilegeSettings(this.userRoleId, pageId);
            
            this.CloseDelete();
            this.successMessage = "Success to Remove Data"
            this.successMessageShow = true
            await this.FetchData();
        }

        CloseDelete() {
            this.viewPage = []
            this.deleteDashboard = []
            this.deleteReports = []
            this.deleteForSetup = []
            this.deleteForUserManagement = []
            this.deleteForMasterData = []
            this.menuPageShow = []
            this.userRoleId = null
        }

    }
</script>
