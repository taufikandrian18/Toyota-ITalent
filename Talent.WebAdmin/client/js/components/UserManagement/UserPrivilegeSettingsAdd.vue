<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="row">
            <div class="col col-md-12">
                <!--Add User Privilege-->
                <h3><fa icon="user"></fa> User Management > User Privilege Settings > <span class="talent_font_red">Add Privilege</span></h3>
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
                                        <checkbox class="talent_checkbox talent_nomargin" style="line-height: 50%;" v-model="viewPage" color="#0085CA" :size="16" v-validate:menuLength="'min_value:1'"  name="menu" value="dashboard">Dashboard</checkbox>
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
                                <h5>Privilege</h5>
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
                                <h5>Privilege</h5>
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
                                <h5>Privilege</h5>
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
                                <h5>Privilege</h5>
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
                                <h5>Privilege</h5>
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
                                <h5>Privilege</h5>
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
    import { UserPrivilegeSettingsService, UserPrivilegeSettingPageCRUD, UserPrivilegeSettingsUserRoleModel, UserPrivilegeSettingsInsertModel, } from '../../services/NSwagService';
    import VeeValidate from 'vee-validate';

    Vue.use(VeeValidate); 
    @Component({
        created: async function (this: UserPrivilegeSettingsAdd) {
            this.isBusy = true;
            await this.GetAllPage();
            await this.GetAllUserRole();
            //await this.validationInit();
            this.isBusy = false;
            
        },
        props: ["add", "success-message-show","success-message"]
    })
    export default class UserPrivilegeSettingsAdd extends Vue {
        
        isBusy: boolean = false

        //API
        privilegeSettingsAPI: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        //Variable show hide page by menu
        optionsDashboard: UserPrivilegeSettingPageCRUD[] = [];
        optionsReports: UserPrivilegeSettingPageCRUD[] = [];
        optionsForUserManagement: UserPrivilegeSettingPageCRUD[] = [];
        optionsForSetup: UserPrivilegeSettingPageCRUD[] = [];
        optionsForMasterData: UserPrivilegeSettingPageCRUD[] = []
        optionsForMyTools: UserPrivilegeSettingPageCRUD[] = [];
        viewPage: string[] = [];
        showDashboard: boolean = false;
        showReport: boolean = false;
        showUserManagement: boolean = false;
        showSetup: boolean = false;
        showMasterData: boolean = false;

        //Variable Add
        privilegeSettingsAdd: UserPrivilegeSettingPageCRUD[] = []

        //AllPageWithMenu
        async GetAllPage() {
            this.isBusy = true
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
            this.isBusy = false
            //MyTools
            var my = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsMenuPageCrudById("MYTOOLS")
            this.optionsForMyTools = my
            this.isBusy = false
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
            var data = await this.privilegeSettingsAPI.getAllUserPrivilegeSettingsUserRoleNotYetSet();
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

        mustBeFilledErrorMessage(){
            this.$validator.errors.add({
                    field: "user role",
                    msg: "Please Choose Create/Read/Update/Delete",
                });
                this.isBusy = false;
                return;
        }

 
        async Insert() {
            this.isBusy = true;
            let valid = await this.$validator.validateAll();
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

            //User Management
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
            await this.privilegeSettingsAPI.insertUserPrivilegeSettings(this.savePrivilege)
            this.$emit('update:successMessage', "Success to Add Data");
            this.$emit('update:successMessageShow', true);
            this.$emit('update:add', false);
            //this.isBusy = false
        }
        Close() {
            this.$emit('update:add', false);
        }
    }
</script>
