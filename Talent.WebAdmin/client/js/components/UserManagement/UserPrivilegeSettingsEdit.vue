<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="row">
            <div class="col col-md-12">
                <!--Add User Privilege-->
                <h3><fa icon="user"></fa> User Management > User Privilege Settings > <span class="talent_font_red">Edit Privilege</span></h3>
                <br />
                <div>
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <h2>User Privilege Information</h2>

                        <div v-if="errorMessageShow == true && errors.items.length > 0" class="alert alert-danger alert-dismissible fade show" role="alert">

                            <div v-for="error in errors.all()">
                                {{error}}
                            </div>

                            <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="row justify-content-between">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-md-12">
                                        <h5>User Role<span class="talent_font_red">*</span></h5>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="input-group">
                                            <select v-validate="'required'" name="user role" v-model="userRoleId" class="form-control">
                                                <option v-for="u in userRole" :value="u.userRoleId">{{u.userRole}}</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>
                                <br />
                            </div>
                            <div class="col-md-12">
                                <h5>Menu<span class="talent_font_red">*</span></h5>
                            </div>
                            <div class="col-md-12">
                                <div class="d-flex">
                                    <div class="form-check form-check-inline">
                                        <checkbox class="talent_checkbox talent_nomargin" style="line-height: 50%;" v-model="viewPage" color="#0085CA" :size="16" name="menu" v-validate:menuLength="'min_value:1'" value="dashboard">Dashboard</checkbox>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <checkbox class="talent_checkbox talent_nomargin" style="line-height: 50%;" v-model="viewPage" color="#0085CA" :size="16" value="report">Report</checkbox>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <checkbox class="talent_checkbox talent_nomargin" style="line-height: 50%;" v-model="viewPage" color="#0085CA" :size="16" value="userManagement">User Management</checkbox>
                                    </div>
                                    <div class="form-check form-check-inline">

                                        <checkbox class="talent_checkbox talent_nomargin" style="line-height: 50%;" v-model="viewPage" color="#0085CA" :size="16" value="setup">Setup</checkbox>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <checkbox class="talent_checkbox talent_nomargin" style="line-height: 50%;" v-model="viewPage" color="#0085CA" :size="16" value="masterData">Master Data</checkbox>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <checkbox class="talent_checkbox talent_nomargin" style="line-height: 50%;" v-model="viewPage" color="#0085CA" :size="16" value="myTools">My Tools</checkbox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div v-if="viewPage.includes('dashboard')">
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div class="row justify-content-between">
                            <div class="col-md-4">
                                <h5>Dashboard<span class="talent_font_red">*</span></h5>
                            </div>
                            <div class="col-md-8">
                                <h5>Privilege<span class="talent_font_red">*</span></h5>
                            </div>
                        </div>
                        <!--Untuk Validasi-->
                        <input name="dashboard" v-validate:dashboardLength="'min_value:1'" hidden />
                        <div class="row justify-content-between mb-2" v-for="(dashboard,index) in optionsDashboard">
                            <div class="col-md-4">
                                <checkbox v-model="dashboard.isPageCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{dashboard.page}}</checkbox>
                            </div>
                            <div class="col-md-8 justify-content-between d-flex">
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="dashboard.create" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Create</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="dashboard.read" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Read</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="dashboard.update" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Update</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="dashboard.delete" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Delete</checkbox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <div v-if="viewPage.includes('report')">
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div class="row justify-content-between">
                            <div class="col-md-4">
                                <h5>Report<span class="talent_font_red">*</span></h5>
                            </div>
                            <div class="col-md-8">
                                <h5>Privilege<span class="talent_font_red">*</span></h5>
                            </div>
                        </div>
                        <!--Untuk Validasi-->
                        <input name="report" v-validate:reportLength="'min_value:1'" hidden />
                        <div class="row justify-content-between mb-2" v-for="(report,index) in optionsReports">
                            <div class="col-md-4">
                                <checkbox v-model="report.isPageCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{report.page}}</checkbox>
                            </div>
                            <div class="col-md-8 justify-content-between d-flex">
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsReports[index].create" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Create</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsReports[index].read" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Read</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsReports[index].update" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Update</checkbox>

                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsReports[index].delete" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Delete</checkbox>

                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <div v-if="viewPage.includes('userManagement')">
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div class="row justify-content-between">
                            <div class="col-md-4">
                                <h5>User Management<span class="talent_font_red">*</span></h5>
                            </div>
                            <div class="col-md-8">
                                <h5>Privilege<span class="talent_font_red">*</span></h5>
                            </div>
                        </div>
                        <!--Untuk Validasi-->
                        <input name="user management" v-validate:userManagementLength="'min_value:1'" hidden />
                        <div class="row justify-content-between mb-2" v-for="(userManagement,index) in optionsForUserManagement">
                            <div class="col-md-4">
                                <checkbox v-model="userManagement.isPageCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{userManagement.page}}</checkbox>
                            </div>
                            <div class="col-md-8 justify-content-between d-flex">
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForUserManagement[index].create" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Create</checkbox>

                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForUserManagement[index].read" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Read</checkbox>

                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForUserManagement[index].update" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Update</checkbox>

                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForUserManagement[index].delete" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Delete</checkbox>

                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <div v-if="viewPage.includes('setup')">
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div class="row justify-content-between">
                            <div class="col-md-4">
                                <h5>Setup<span class="talent_font_red">*</span></h5>
                            </div>
                            <div class="col-md-8">
                                <h5>Privilege<span class="talent_font_red">*</span></h5>
                            </div>
                        </div>
                        <!--Untuk Validasi-->
                        <input name="setup" v-validate:setupLength="'min_value:1'" hidden />
                        <div class="row justify-content-between mb-2" v-for="(setup,index) in optionsForSetup">
                            <div class="col-md-4">
                                <checkbox v-model="setup.isPageCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{setup.page}}</checkbox>
                            </div>
                            <div class="col-md-8 justify-content-between d-flex">
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForSetup[index].create" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Create</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForSetup[index].read" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Read</checkbox>

                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForSetup[index].update" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Update</checkbox>

                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForSetup[index].delete" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Delete</checkbox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                </div>
                <div v-if="viewPage.includes('masterData')">
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div class="row justify-content-between">
                            <div class="col-md-4">
                                <h5>Master Data<span class="talent_font_red">*</span></h5>
                            </div>
                            <div class="col-md-8">
                                <h5>Privilege<span class="talent_font_red">*</span></h5>
                            </div>
                        </div>
                        <!--Untuk Validasi-->
                        <input name="master data" v-validate:masterDataLength="'min_value:1'" hidden />
                        <div class="row justify-content-between mb-2" v-for="(masterData,index) in optionsForMasterData">
                            <div class="col-md-4">
                                <checkbox v-model="masterData.isPageCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{masterData.page}}</checkbox>
                            </div>
                            <div class="col-md-8 justify-content-between d-flex">
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForMasterData[index].create" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Create</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForMasterData[index].read" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Read</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForMasterData[index].update" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Update</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForMasterData[index].delete" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Delete</checkbox>
                                </div>
                            </div>
                        </div>

                    </div>
                    <br />
                </div>
                <div v-if="viewPage.includes('myTools')">
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <div class="row justify-content-between">
                            <div class="col-md-4">
                                <h5>My Tools<span class="talent_font_red">*</span></h5>
                            </div>
                            <div class="col-md-8">
                                <h5>Privilege<span class="talent_font_red">*</span></h5>
                            </div>
                        </div>
                        <!--Untuk Validasi-->
                        <input name="master data" v-validate:myToolsLength="'min_value:1'" hidden />
                        <div class="row justify-content-between mb-2" v-for="(masterData,index) in optionsForMyTools">
                            <div class="col-md-4">
                                <checkbox v-model="masterData.isPageCheck" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">{{masterData.page}}</checkbox>
                            </div>
                            <div class="col-md-8 justify-content-between d-flex">
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForMyTools[index].create" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Create</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForMyTools[index].read" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Read</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForMyTools[index].update" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Update</checkbox>
                                </div>
                                <div class="form-check form-check-inline">
                                    <checkbox v-model="optionsForMyTools[index].delete" class="talent_checkbox talent_nomargin" style="line-height: 50%;" color="#0085CA" :size="16">Delete</checkbox>
                                </div>
                            </div>
                        </div>

                    </div>
                    <br />
                </div>
                <!--Button-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click="Close()">
                                        Close
                                    </button>
                                    <button class="btn talent_bg_lightgreen talent_font_white" @click="Insert()">
                                        Save
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <br />
        </div>
        <br />
    </div>
</template>
<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { UserPrivilegeSettingsService, UserPrivilegeSettingPageCRUD, UserPrivilegeSettingsUserRoleModel, UserPrivilegeSettingsInsertModel,} from '../../services/NSwagService';
    import VeeValidate from 'vee-validate';

    Vue.use(VeeValidate); 
    @Component({
        created: async function (this: UserPrivilegeSettingsEdit) {
            this.isBusy = true;
            await this.GetAllPage();
            await this.GetAllUserRole();
            await this.GetAllUserRoleAccess();
            this.isBusy = false;
        },
        props: ["edit", "edit-id", "success-message-show", "success-message"]
    })
    export default class UserPrivilegeSettingsEdit extends Vue {
        editId: number;
        isBusy: boolean = true;

        //API
        privilegeSettingsAPI: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        //Variable show hide page by menu
        optionsDashboard: UserPrivilegeSettingPageCRUD[] = [];
        optionsReports: UserPrivilegeSettingPageCRUD[] = [];
        optionsForUserManagement: UserPrivilegeSettingPageCRUD[] = [];
        optionsForSetup: UserPrivilegeSettingPageCRUD[] = [];
        optionsForMasterData: UserPrivilegeSettingPageCRUD[] = []
        optionsForMyTools: UserPrivilegeSettingPageCRUD[] = []
        viewPage: string[] = [];
        showDashboard: boolean = false;
        showReport: boolean = false;
        showUserManagement: boolean = false;
        showSetup: boolean = false;
        showMasterData: boolean = false;

        //Variable Add
        privilegeSettingsUpdate: UserPrivilegeSettingPageCRUD[] = []

        //AllPageWithMenu
        async GetAllPage() {
            //Dashboard
            var dsb = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsMenuPageCrudById("DSB")
            this.optionsDashboard = dsb
            //Master Data
            var md = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsMenuPageCrudById("MD")
            this.optionsForMasterData = md
            //Report
            var rprt = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsMenuPageCrudById("RPRT")
            this.optionsReports = rprt
            //Setup
            var stp = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsMenuPageCrudById("STP")
            this.optionsForSetup = stp
            //UserManagement
            var um = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsMenuPageCrudById("UM")
            this.optionsForUserManagement = um
            //My Tools
            var my = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsMenuPageCrudById("MYTOOLS")
            this.optionsForMyTools = my
        }

        //variable user role
        userRole: UserPrivilegeSettingsUserRoleModel[] = []

        //Variable Save
        userRoleId: number = null
        savePrivilege: UserPrivilegeSettingsInsertModel[] = []

        //Variable Validasi
        errorMessageShow: boolean = false;
        //reset error message
        ResetErrorMessage() {
            this.errorMessageShow = false;
        }

        //GetAllUserRole
        async GetAllUserRole() {
            var data = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsUserRoleNotYetSetExceptSelf(this.editId);
            this.userRole = data;
        }

        //Validasi menu required
        get menuLength() {
            return this.viewPage.length;
        }

        //Validasi Dashboard
        get dashboardLength() {
            var count = 0
            var data = this.optionsDashboard.forEach(function (element) {
                if (element.isPageCheck == true) {
                    count++;
                }
            })
            console.log(count)
            return count
        }

        get reportLength() {
            var count = 0
            var data = this.optionsReports.forEach(function (element) {
                if (element.isPageCheck == true) {
                    count++;
                }
            })
            console.log(count)
            return count
        }

        get userManagementLength() {
            var count = 0
            var data = this.optionsForUserManagement.forEach(function (element) {
                if (element.isPageCheck == true) {
                    count++;
                }
            })
            console.log(count)
            return count
        }

        get setupLength() {
            var count = 0
            var data = this.optionsForSetup.forEach(function (element) {
                if (element.isPageCheck == true) {
                    count++;
                }
            })
            console.log(count)
            return count
        }

        get masterDataLength() {
            var count = 0
            var data = this.optionsForMasterData.forEach(function (element) {
                if (element.isPageCheck == true) {
                    count++;
                }
            })
            console.log(count)
            return count
        }

        get myToolsLength() {
            var count = 0
            var data = this.optionsForMyTools.forEach(function (element) {
                if (element.isPageCheck == true) {
                    count++;
                }
            })
            console.log(count)
            return count
        }

        //GetAllUserData
        getPageId: string[] = []

        async GetAllUserRoleAccess() {
            this.isBusy = true
            var data = await this.privilegeSettingsAPI.getUserPrivilegeSettingsByUserRoleId(this.editId);
            this.userRoleId = data.userRoleId
            for (var mp of data.menuPage) {
                this.getPageId.push(mp.pageId)
                switch (mp.menuId) {
                    case "DSB":
                        if (this.viewPage.find(Q => Q == "dashboard") == undefined) {
                            this.viewPage.push("dashboard")
                        }
                        console.log(this.optionsDashboard.find(Q => Q.pageId == mp.pageId))
                        var dsb = this.optionsDashboard.find(Q => Q.pageId == mp.pageId)
                        dsb.isPageCheck = true
                        dsb.create = mp.create
                        dsb.read = mp.read
                        dsb.update = mp.update
                        dsb.delete = mp.delete
                        break;
                    case "MD":
                        if (this.viewPage.find(Q => Q == "masterData") == undefined) {
                            this.viewPage.push("masterData")
                        }

                        console.log(this.optionsForMasterData.find(Q => Q.pageId == mp.pageId))
                        var md = this.optionsForMasterData.find(Q => Q.pageId == mp.pageId)
                        md.isPageCheck = true
                        md.create = mp.create
                        md.read = mp.read
                        md.update = mp.update
                        md.delete = mp.delete
                        break;
                    case "RPRT":
                        if (this.viewPage.find(Q => Q == "report") == undefined) {
                            this.viewPage.push("report")
                        }

                        console.log(this.optionsReports.find(Q => Q.pageId == mp.pageId))
                        var rprt = this.optionsReports.find(Q => Q.pageId == mp.pageId)
                        rprt.isPageCheck = true
                        rprt.create = mp.create
                        rprt.read = mp.read
                        rprt.update = mp.update
                        rprt.delete = mp.delete
                        break;
                    case "STP":
                        if (this.viewPage.find(Q => Q == "setup") == undefined) {
                            this.viewPage.push("setup")
                        }

                        console.log(this.optionsForSetup.find(Q => Q.pageId == mp.pageId))
                        var stp = this.optionsForSetup.find(Q => Q.pageId == mp.pageId)
                        stp.isPageCheck = true
                        stp.create = mp.create
                        stp.read = mp.read
                        stp.update = mp.update
                        stp.delete = mp.delete
                        break;
                    case "UM":
                        if (this.viewPage.find(Q => Q == "userManagement") == undefined) {
                            this.viewPage.push("userManagement")
                        }

                        console.log(this.optionsForUserManagement.find(Q => Q.pageId == mp.pageId))
                        var um = this.optionsForUserManagement.find(Q => Q.pageId == mp.pageId)
                        um.isPageCheck = true
                        um.create = mp.create
                        um.read = mp.read
                        um.update = mp.update
                        um.delete = mp.delete
                        break
                    case "MYTOOLS":
                        if (this.viewPage.find(Q => Q == "myTools") == undefined) {
                            this.viewPage.push("myTools")
                        }

                        console.log(this.optionsForMyTools.find(Q => Q.pageId == mp.pageId))
                        var my = this.optionsForMyTools.find(Q => Q.pageId == mp.pageId)
                        my.isPageCheck = true
                        my.create = mp.create
                        my.read = mp.read
                        my.update = mp.update
                        my.delete = mp.delete
                        break
                }
            }
            this.isBusy = false
        }

        mustBeFilledErrorMessage(){
            this.$validator.errors.add({
                field: "user role",
                msg: "Please Choose Create/Read/Update/Delete",
            });
            this.isBusy = false;
            return;
        }


        async Insert() {
            this.isBusy = true
            let valid = await this.$validator.validateAll();
            console.log(this.$validator.errors)
            if (!valid) {
                this.errorMessageShow = true;
                this.isBusy = false
                return;
            }
            this.$validator.reset;

            //Dashboard
            for (var psa of this.optionsDashboard) {
                if (this.viewPage.find(Q => Q == "dashboard") == undefined) {
                    break
                }

                if (psa.isPageCheck == true) {
                    var notFilled = psa.create == false && psa.update == false && psa.read == false && psa.delete == false;
                    if(notFilled){
                        this.errorMessageShow = true;
                        this.mustBeFilledErrorMessage();
                        return;
                    }

                    this.savePrivilege.push({
                        userRoleId: this.userRoleId,
                        pageId: psa.pageId,
                        create: psa.create,
                        read: psa.read,
                        update: psa.update,
                        delete: psa.delete
                    })
                }
                //this.savePrivilege.push(insert);
            }

            //Report
            for (var psa of this.optionsReports) {
                if (this.viewPage.find(Q => Q == "report") == undefined) {
                    break
                }

                if (psa.isPageCheck == true) {
                    var notFilled = psa.create == false && psa.update == false && psa.read == false && psa.delete == false;
                    if(notFilled){
                        this.errorMessageShow = true;
                        this.mustBeFilledErrorMessage();
                        return;
                    }

                    this.savePrivilege.push({
                        userRoleId: this.userRoleId,
                        pageId: psa.pageId,
                        create: psa.create,
                        read: psa.read,
                        update: psa.update,
                        delete: psa.delete
                    })
                }
            }

            //User Management
            for (var psa of this.optionsForUserManagement) {
                if (this.viewPage.find(Q => Q == "userManagement") == undefined) {
                    break
                }
                if (psa.isPageCheck == true) {
                    var notFilled = psa.create == false && psa.update == false && psa.read == false && psa.delete == false;
                    if(notFilled){
                        this.errorMessageShow = true;
                        this.mustBeFilledErrorMessage();
                        return;
                    }

                    this.savePrivilege.push({
                        userRoleId: this.userRoleId,
                        pageId: psa.pageId,
                        create: psa.create,
                        read: psa.read,
                        update: psa.update,
                        delete: psa.delete
                    })
                }
            }

            //Setup
            for (var psa of this.optionsForSetup) {
                if (this.viewPage.find(Q => Q == "setup") == undefined) {
                    break
                }
                if (psa.isPageCheck == true) {
                    var notFilled = psa.create == false && psa.update == false && psa.read == false && psa.delete == false;
                    if(notFilled){
                        this.errorMessageShow = true;
                        this.mustBeFilledErrorMessage();
                        return;
                    }

                    this.savePrivilege.push({
                        userRoleId: this.userRoleId,
                        pageId: psa.pageId,
                        create: psa.create,
                        read: psa.read,
                        update: psa.update,
                        delete: psa.delete
                    })
                }
            }

            //Master Data
            for (var psa of this.optionsForMasterData) {
                if (this.viewPage.find(Q => Q == "masterData") == undefined) {
                    break
                }
                if (psa.isPageCheck == true) {
                    var notFilled = psa.create == false && psa.update == false && psa.read == false && psa.delete == false;
                    if(notFilled){
                        this.errorMessageShow = true;
                        this.mustBeFilledErrorMessage();
                        return;
                    }
                    
                    this.savePrivilege.push({
                        userRoleId: this.userRoleId,
                        pageId: psa.pageId,
                        create: psa.create,
                        read: psa.read,
                        update: psa.update,
                        delete: psa.delete
                    })
                }
                //this.savePrivilege.push(insert);
            }

            //My Tools
            for (var psa of this.optionsForMyTools) {
                if (this.viewPage.find(Q => Q == "myTools") == undefined) {
                    break
                }
                if (psa.isPageCheck == true) {
                    var notFilled = psa.create == false && psa.update == false && psa.read == false && psa.delete == false;
                    if(notFilled){
                        this.errorMessageShow = true;
                        this.mustBeFilledErrorMessage();
                        return;
                    }
                    
                    this.savePrivilege.push({
                        userRoleId: this.userRoleId,
                        pageId: psa.pageId,
                        create: psa.create,
                        read: psa.read,
                        update: psa.update,
                        delete: psa.delete
                    })
                }
                //this.savePrivilege.push(insert);
            }

            await this.privilegeSettingsAPI.updateUserPrivilegeSettings(this.editId, this.savePrivilege)
            this.$emit('update:successMessage', "Success to Edit Data");
            this.$emit('update:successMessageShow', true);
            this.$emit('update:edit', false);
        }
        Close() {
            this.$emit('update:edit', false);
        }
    }
</script>
