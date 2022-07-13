<template>
    <div>
        <div class="row">
            <!-- buat loading -->
            <loading-overlay :loading="isBusy"></loading-overlay>

            <div class="col col-md-12">
                <!--TITLE-->
                <h3 v-if="isDetail == true">
                    <fa icon="user"></fa>User Management > Position Mapping >
                    <span class="talent_font_red">Detail Position Mapping</span>
                </h3>
                <h3 v-if="isInsert == true">
                    <fa icon="user"></fa>User Management > Position Mapping >
                    <span class="talent_font_red">Add Position Mapping</span>
                </h3>
                <h3 v-if="isEdit == true">
                    <fa icon="user"></fa>User Management > Position Mapping >
                    <span class="talent_font_red">Edit Position Mapping</span>
                </h3>
                <br />
                <div class="row" >
                    <div class="col col-md-12">
                        <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                            <h4>Position Mapping Information</h4>
                            <div class="alert alert-danger pb-0" v-show="errors.items.length > 0">
                                <button type="button" class="close" aria-label="Close" @click="$validator.errors.clear()">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                                <ul>
                                    <li v-for="error in errors.all()" :key="error.field">{{error}}</li>
                                </ul>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-6">
                                            <label>Position Name<span class="talent_font_red">*</span></label>
                                            <multiselect v-model="selectedPosition"
                                                    :options="positionList"
                                                    :searchable="true"
                                                    :allow-empty="true"
                                                    :disabled="isDetail == true"
                                                    label="name"
                                                    track-by="id"
                                                    @input="setPosition()"
                                                    >
                                            </multiselect>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Position Description</label>
                                            <textarea class="form-control" style="height:150px;overflow-y:scroll;" 
                                            v-model="formModel.positionDescription" :disabled="isDetail == true" maxlength="2000"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <br />
                            <!-- //repeat able -->
                            <div class="row">
                                <div class="talent_marginbottom col-md-12" v-for="index in totalCompetency" :key="index">
                                    <div class="row justify-content-between">
                                        <div class="col-md-5">
                                            <label>Competency Name <span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-3">
                                            <label>Priority <span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-3">
                                            <label>Proficiency Level <span class="talent_font_red">*</span></label>
                                        </div>
                                        <div class="col-md-1">
                                            <label>Action</label>
                                        </div>
                                    </div>
                                    <div class="row justify-content-between" >
                                        <div class="col-md-5">
                                            <multiselect v-model="selectedCompetencyName[index -1]"
                                                    :options="competencyNameList"
                                                    :searchable="true"
                                                    :allow-empty="true"
                                                    label="name"
                                                    track-by="id"
                                                    :disabled="isDetail == true"
                                                    @input="setCompetencyName(index -1)"
                                                    >
                                            </multiselect>
                                        </div>
                                        <div class="col-md-3">
                                            <multiselect v-model="selectedPriority[index -1]"
                                                    :options="priorityLevelList"
                                                    :searchable="true"
                                                    :allow-empty="true"
                                                    :disabled="isDetail == true"
                                                    @input="setPriority(index -1)"
                                                    >
                                            </multiselect>
                                        </div>
                                        <div class="col-md-3">
                                            <multiselect v-model="selectedProficiencyLevel[index -1]"
                                                    :options="proficiencyLevelList"
                                                    :searchable="true"
                                                    :allow-empty="true"
                                                    :disabled="isDetail == true"
                                                    @input="setProficiencyLevel(index -1)"
                                                    >
                                            </multiselect>
                                        </div>
                                        <div class="col-md-1" >
                                            <div v-if="isInsert == true || isEdit == true">
                                                <button v-if="((index) == totalCompetency)" class="button_text_no_background" @click.prevent="addNewCompetency()">
                                                    <fa icon="plus-circle"></fa>
                                                </button>
                                                <button v-if="(index < totalCompetency)" class="button_text_no_background" @click.prevent="deleteCompetency(index-1)">
                                                    <fa icon="trash-alt"></fa>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                    <br />
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow" >
                        <div class="row">
                            <div class="col-md-12">
                                <div class="d-flex align-items-end flex-column">
                                    <div>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="closePage()">Close</button>
                                        <button class="btn talent_bg_lightgreen talent_font_white" v-if="isDetail == false"
                                                @click.prevent="saveData('save')" 
                                                :disabled="sendButtonLoading || isDetail == true"
                                                >Save</button>
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
    import { DropdownService, DropDownModel, FileUploadService, FileModel, PositionMapViewModel, PositionMapFormModel, PosCompetencyModel, PositionMapService, UserAccessCRUD, UserPrivilegeSettingsService } from '../../../services/NSwagService';

    @Component({
        props: ['type', 'theId', 'isShowMessage', 'theShowMessage'],
        mounted: async function (this: NewsFormPage) {
            await this.getAccess();
            await this.fetching();
        }
    })
    export default class NewsFormPage extends Vue {
        props: any = {
            placeholder: '',
            readonly: true
        };

        isBusy: boolean = true;

        isEdit: boolean = false;
        isInsert: boolean = false;
        isDetail: boolean = true;

        //loading for 
        sendButtonLoading: boolean = false;
        isLoading: boolean = false;

        viewType: string = "view";

        type: string;
        theId: number;

        fileUpload: string[] = ['asd'];

        //variable
        selectedCompetencyName:DropDownModel[] = [{
            id:0,
            name:"",
        }];
        selectedPosition:DropDownModel = {
            id:0,
            name:"",
        };
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }
        selectedPriority:string[] = [""];
        selectedProficiencyLevel:number[] = [0];

        competencyNameList:DropDownModel[] = [];
        positionList:DropDownModel[] = [];
        priorityLevelList:string[] = [];
        proficiencyLevelList:number[] = [];

        inputCompetencyList: PosCompetencyModel[] = []

        formModel:PositionMapFormModel = {
            positionMapId:0,
            positionId:0,
            positionName:"",
            positionDescription:"",
            competencyList: []
        }

        //buat icon di competency
        totalCompetency = 1;

        //service
        positionServiceMan: PositionMapService = new PositionMapService();
        dropdownServiceMan: DropdownService = new DropdownService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        async fetching() {
            this.isLoading = true;
            this.isBusy = true;
            this.setTypeForm();
            await this.getPositionListData();
            await this.getPriorityListData();
            await this.getCompetencyListData();
            await this.getproficiencyListData();
            await this.getDataPositionMap();
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("PM");
            this.crud = data
            if (data.create == false && this.type.toLowerCase().search("add") != -1) {
                window.location.href = `/MasterData/PositionMapping`;
                //window.history.back();
            }
            else if (data.update == false && this.type.toLowerCase().search("update") != -1) {
                window.location.href = `/MasterData/PositionMapping`;
                //window.history.back();
            }
            else if (data.read == false && this.type.toLowerCase().search("detail") != -1) {
                window.location.href = `/MasterData/PositionMapping`;
                //window.history.back();
            }
            else if (data.read == false && data.create == false && data.update == false) {
                window.location.href = `/MasterData/PositionMapping`;
                //window.history.back();
            }
        }
        // --------------- service / main function --------------------

        setTypeForm() {
            this.$validator.reset();
            this.$validator.errors.clear();
            if (this.type != null && this.type != undefined) {
                if (this.type.toLowerCase().search("add") != -1) {
                    this.isInsert = true;
                    this.isEdit = false;
                    this.isDetail = false;
                }
                else if (this.type.toLowerCase().search("update") != -1) {
                    this.isInsert = false;
                    this.isEdit = true;
                    this.isDetail = false;
                }
                else if (this.type.toLowerCase().search("detail") != -1) {
                    this.isInsert = false;
                    this.isEdit = false;
                    this.isDetail = true;
                } else {
                    this.isInsert = false;
                    this.isEdit = false;
                    this.isDetail = true;
                }
            }
            else {
                this.isInsert = false;
                this.isEdit = false;
                this.isDetail = true;
            }
        }

        async getDataPositionMap() {
            this.$validator.errors.clear();
            try{
                if (this.isDetail == true || this.isEdit == true) {
                    let model = await this.positionServiceMan.getDetail(this.theId);
                    this.setDataIntoForm(model);
                }
                else if(this.isInsert == true){
                    this.inputCompetencyList[0] = {
                        competencyId:0,
                        competencyName:"",
                        proficiencyLevel:0,
                        priority:""
                    }
                }
            }catch{
                this.isBusy = false;
            }

            this.isBusy = false;
            this.isLoading = false;
        }

        async setDataIntoForm(model: PositionMapFormModel) {

            this.formModel.positionMapId = this.theId;
            this.formModel.positionId = model.positionId;
            this.formModel.positionName = model.positionName;
            this.formModel.positionDescription = model.positionDescription;

            //set into multi select
            this.inputCompetencyList = model.competencyList;

            //buat view

            this.selectedPosition.id = model.positionId;
            this.selectedPosition.name = model.positionName;

            this.totalCompetency = model.competencyList.length;

            for(var index = 0 ; index < this.totalCompetency ; index++){
                this.selectedCompetencyName[index] = {
                    id: model.competencyList[index].competencyId,
                    name: model.competencyList[index].competencyName
                };
                this.selectedPriority[index] = model.competencyList[index].priority;
                this.selectedProficiencyLevel[index] = model.competencyList[index].proficiencyLevel;
            }

            this.isLoading = false;
        }

        validateAllInput() {
            let result = true;
            //
            if (this.formModel.positionId == null || this.formModel.positionId == undefined || this.formModel.positionId == 0) {
                this.$validator.errors.add({
                    field: 'Position Name',
                    msg: 'Position Name is empty, please fill the field'
                });
                result = false;
            }

            for(let i = 0 ; i < this.inputCompetencyList.length; i++) {
                if (this.inputCompetencyList[i].competencyName == null || this.inputCompetencyList[i].competencyName == undefined || this.inputCompetencyList[i].competencyName == "") {
                    this.$validator.errors.add({
                        field: 'Competency Name ' + (i+1),
                        msg: 'Competency Name row' + (i+1) +' is empty, please fill the field'
                    });
                    result = false;
                }

                if (this.inputCompetencyList[i].priority == null || this.inputCompetencyList[i].priority == undefined || this.inputCompetencyList[i].priority == "") {
                    this.$validator.errors.add({
                        field: 'priority ' + (i+1),
                        msg: 'Priority row' + (i+1) +' is empty, please fill the field'
                    });
                    result = false;
                }

                if (this.inputCompetencyList[i].proficiencyLevel <= 0 ) {
                    this.$validator.errors.add({
                        field: 'Proficiency Level ' + (i+1),
                        msg: 'Proficiency Level row' + (i+1) +' is empty, please fill the field'
                    });
                    result = false;
                }
            }

            if(result == true){
                let hasil = this.checkingCompetency();

                if(hasil == false){
                    this.$validator.errors.add({
                        field: 'Proficiency Level Competency has Same',
                        msg: 'Competency Name on Proficiency Level have more than 1 with the same Name'
                    });
                    result = false;
                }

                return result
            }

            return result
        }

        checkingCompetency(){

            let result = true;
            for(let i = 0; i < this.totalCompetency ; i++){
                let temp = this.inputCompetencyList.filter(x => x.competencyName == this.selectedCompetencyName[i].name);
                if(temp.length > 1){
                    result = false;
                    break;
                }
            }

            return result;
        }

        async saveData(type:string) {
            // this.$validator.reset();
            this.isBusy = true;

            this.$validator.resume();
            this.$validator.errors.clear();
            let result = this.validateAllInput();
            let valid = await this.$validator.validateAll();
            if (valid == false) {
                this.sendButtonLoading = false; 
                this.isBusy = false;

                return;
            }

            this.sendButtonLoading = true;
            
            if (this.isDetail == false) {
                if (result == true) {

                    this.formModel.competencyList = this.inputCompetencyList;

                    if (this.isInsert == true || this.isEdit == false) {
                        let responseResult = await this.positionServiceMan.insert(this.formModel);
                        this.notif(responseResult);
                    }
                    else if (this.isEdit == true || this.isInsert == false) {
                        let responseResult = await this.positionServiceMan.update(this.formModel);
                        this.notif(responseResult);
                    }
                }
                this.$validator.reset();
            }
            this.sendButtonLoading = false; 
            this.isBusy = false;
        }

        async getPriorityListData(){
            let getList = await this.dropdownServiceMan.getCompetencyPriority();
            this.priorityLevelList = getList;
        }

        async getCompetencyListData(){
            let getList = await this.dropdownServiceMan.getCompetency();
            this.competencyNameList = getList;
        }

        async getproficiencyListData(){
            let getList = await this.dropdownServiceMan.getCompetencyProficiency();
            this.proficiencyLevelList = getList;
        }

        async getPositionListData(){
            let getList = await this.dropdownServiceMan.getPosition();
            this.positionList = getList;
        }

        // ---------------- side function -------------------

        resetAll() {
            this.selectedCompetencyName = [{
                id:0,
                name:"",
            }];
            this.selectedPriority = [""];
            this.selectedProficiencyLevel = [0];

            this.inputCompetencyList = [];

            this.selectedPosition = {
                id:0,
                name:"",
            };

            this.formModel = {
                positionMapId:0,
                positionId:0,
                positionName:"",
                positionDescription:"",
                competencyList: []
            }
        }

        async notif(result:string){

            if (result.toLowerCase().search("failed") != -1) {
                this.$validator.errors.add({
                    field: 'Response From Server',
                    msg: result
                });
                this.sendButtonLoading = false;
            }
            else if (result.toLowerCase().search("success") != -1) {
                this.sendButtonLoading = false;
                this.$emit('update:theShowMessage', result);
                this.$emit('update:isShowMessage', true);
                this.closePage();
            }
            else{
                this.sendButtonLoading = false; 
            }
        }

        async closePage() {
            this.isBusy = false;
            this.resetAll();
            this.$emit('update:type', "view");
            //window.location.href = `/MasterData/PositionMapping`;
            // this.$emit('update:isView', true);
            // window.history.back();
            // window.location.href = `/MasterData/PositionMapping?type="${this.viewType}"`;
        }
        
        addNewCompetency(){

            let lastIndex = this.inputCompetencyList.length;

            this.inputCompetencyList[lastIndex] = {
                competencyId:0,
                competencyName:"",
                proficiencyLevel:0,
                priority:""
            };

            this.totalCompetency += 1;

        }

        deleteCompetency(index:number){
            let lastIndex = this.inputCompetencyList.length;
            this.inputCompetencyList.splice(index,1);
            this.selectedCompetencyName.splice(index,1);
            this.selectedPriority.splice(index,1);
            this.selectedProficiencyLevel.splice(index,1);

            this.totalCompetency = lastIndex - 1;
        }

        setPriority(index:number){
            this.inputCompetencyList[index].priority = this.selectedPriority[index];
        }

        setProficiencyLevel(index:number){
            this.inputCompetencyList[index].proficiencyLevel = this.selectedProficiencyLevel[index];
        }

        setCompetencyName(index:number){
            this.inputCompetencyList[index].competencyId = this.selectedCompetencyName[index].id;
            this.inputCompetencyList[index].competencyName = this.selectedCompetencyName[index].name;
        }

        setPosition(){
            this.formModel.positionId = this.selectedPosition.id;
            this.formModel.positionName = this.selectedPosition.name;
        }
        
    }
</script>