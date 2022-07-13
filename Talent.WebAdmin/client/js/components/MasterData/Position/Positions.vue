<template>
    <div>
        <div class="col col-md-12">
            <loading-overlay :loading="isBusy"></loading-overlay>

            <!-- // start view -->
            <div v-if="isView == true">
                <!--TITLE-->
                <h3>
                    <fa icon="user"></fa> User Management >
                    <span class="talent_font_red">Position Mapping</span>
                </h3>
                <br />
                <div
                    v-if="isShowMessage == true"
                    class="alert alert-success alert-dismissible fade show"
                    role="alert"
                >
                    {{ theShowMessage }}
                    <button
                        type="button"
                        class="close"
                        data-dismiss="alert"
                        @click.prevent="ResetSuccessMessage()"
                        aria-label="Close"
                    >
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <br />
                    <!-- error message -->
                    <div v-if="isShowErrorMessage == true" class="alert alert-danger alert-dismissible fade show" role="alert">
                        {{theShowErrorMessage}}
                        <button type="button" class="close" data-dismiss="alert" @click.prevent="ResetErrorMessage()" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                <br />
                <div>
                    <!--ADVANCE SEARCH-->
                    <div
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>Search Position Mapping</h4>
                        <br />
                        <!--3 Column Menu-->
                        <div class="row justify-content-between">
                            <div class="col-md-4">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>Date</span>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="input-group talent_front">
                                            <v-date-picker
                                                class="v-date-style-report"
                                                mode="range"
                                                v-model="rangePicker"
                                                :firstDayOfWeek="2"
                                                :value="null"
                                                :input-props="props"
                                                :masks="{ input: 'DD/MM/YYYY' }"
                                            ></v-date-picker>
                                            <div class="input-group-append">
                                                <span class="input-group-text"
                                                    ><fa
                                                        icon="calendar-alt"
                                                    ></fa
                                                ></span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>Competency Name</span>
                                    </div>
                                    <div class="col-md-8">
                                        <multiselect
                                            v-model="selectedCompetencyName"
                                            :options="competencyNameList"
                                            :searchable="true"
                                            :allow-empty="true"
                                            label="name"
                                            track-by="id"
                                            @input="setCompetencyName()"
                                        >
                                        </multiselect>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>Proficiency Level</span>
                                    </div>
                                    <div class="col-md-8">
                                        <multiselect
                                            v-model="selectedProficiencyLevel"
                                            :options="proficiencyLevelList"
                                            :searchable="true"
                                            :allow-empty="true"
                                        >
                                        </multiselect>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <br />

                        <!--3 Column Menu-->
                        <div class="row">
                            <div class="col-md-4">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>Position Name</span>
                                    </div>
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <input
                                                type="text"
                                                class="form-control"
                                                v-model="
                                                    PositionMapFilterModel.positionName
                                                "
                                            />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="col-md-4">
                                <div class="row align-items-center">
                                    <div class="col-md-4">
                                        <span>Priority</span>
                                    </div>
                                    <div class="col-md-8">
                                        <multiselect
                                            v-model="selectedPriority"
                                            :options="priorityLevelList"
                                            :searchable="true"
                                            :allow-empty="true"
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
                                    <div
                                        class="d-flex align-items-end flex-column"
                                    >
                                        <div>
                                            <button
                                                class="btn talent_bg_blue talent_font_white"
                                                @click.prevent="searchByInput()"
                                            >
                                                Search
                                            </button>
                                            <button
                                                class="btn talent_bg_red talent_font_white"
                                                @click.prevent="resetAll()"
                                            >
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
                    <div
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h4>Position Mapping List</h4>
                        <div class="col-md-12 talent_magin_small">
                            <div
                                class="row align-items-center row justify-content-between"
                            >
                                <a
                                    >Showing {{ cureentItem }} of
                                    {{ totalData }} Entry(s)</a
                                >
                                <button v-if="crud.create"
                                    class="btn talent_bg_cyan talent_roundborder talent_font_white"
                                    @click.prevent="goToAddPage()"
                                >
                                    Add Mapping
                                </button>
                            </div>
                        </div>

                        <div class="talent_overflowx">
                            <table
                                class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter"
                            >
                                <thead>
                                    <tr>
                                        <th scope="col">
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="
                                                    ClickSortPositionName()
                                                "
                                                >Position Name
                                                <fa
                                                    icon="sort"
                                                    v-if="
                                                        positionName.sort ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        positionName.sortup ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        positionName.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th>
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="
                                                    ClickSortCompetencyName()
                                                "
                                                >Competency Name
                                                <fa
                                                    icon="sort"
                                                    v-if="
                                                        competencyName.sort ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        competencyName.sortup ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        competencyName.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th>
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="
                                                    ClickSortPriority()
                                                "
                                                >Priority
                                                <fa
                                                    icon="sort"
                                                    v-if="priority.sort == true"
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        priority.sortup == true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        priority.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th>
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="
                                                    ClickSortProficiencyLevel()
                                                "
                                                >Proficiency Level
                                                <fa
                                                    icon="sort"
                                                    v-if="
                                                        proficiencyLevel.sort ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        proficiencyLevel.sortup ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        proficiencyLevel.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th>
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="
                                                    ClickSortCreatedDate()
                                                "
                                                >Created Date
                                                <fa
                                                    icon="sort"
                                                    v-if="
                                                        createdDate.sort == true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        createdDate.sortup ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        createdDate.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th>
                                            <a
                                                href="#"
                                                class="talent_sort"
                                                @click.prevent="
                                                    ClickSortUpdatedDate()
                                                "
                                                >Updated Date
                                                <fa
                                                    icon="sort"
                                                    v-if="
                                                        updatedDate.sort == true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-up"
                                                    v-if="
                                                        updatedDate.sortup ==
                                                            true
                                                    "
                                                ></fa
                                                ><fa
                                                    icon="sort-down"
                                                    v-if="
                                                        updatedDate.sortdown ==
                                                            true
                                                    "
                                                ></fa
                                            ></a>
                                        </th>
                                        <th colspan="3">
                                            Action
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr
                                        v-for="data in PositionMapList"
                                        :key="data.posCompetencyId"
                                    >
                                        <td>
                                            {{ data.positionName }}
                                        </td>
                                        <td>
                                            {{ data.competencyName }}
                                        </td>
                                        <td>
                                            {{ data.priority }}
                                        </td>
                                        <td>
                                            {{ data.proficiencyLevel }}
                                        </td>
                                        <td>
                                            {{ data.createdAt | dateFormat }}
                                        </td>
                                        <td>
                                            {{ data.updateAt | dateFormat }}
                                        </td>
                                        <td v-if="crud.read" class="talent_nopadding_important">
                                            <button
                                                class="btn btn-block talent_bg_orange talent_font_white"
                                                @click.prevent="
                                                    goToDetailPage(
                                                        data.positionId
                                                    )
                                                "
                                            >
                                                View Detail
                                            </button>
                                        </td>
                                        <td v-if="crud.update" class="talent_nopadding_important">
                                            <button
                                                class="btn btn-block talent_bg_cyan talent_font_white"
                                                @click.prevent="
                                                    goToUpdatePage(
                                                        data.positionId
                                                    )"
                                            >
                                                Edit
                                            </button>
                                        </td>
                                        <td v-if="crud.delete" class="talent_nopadding_important">
                                            <button
                                                class="btn btn-block talent_bg_red talent_font_white"
                                                data-toggle="modal"
                                                data-target="#modalDelete"
                                                @click.prevent="
                                                    setDelete(
                                                        data.positionId,
                                                        data.positionName
                                                    )
                                                "
                                            >
                                                Remove
                                            </button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>

                        <div
                            class="col-md-12 d-flex justify-content-center mt-3"
                        >
                            <paginate
                                :currentPage.sync="cureentPage"
                                :total-data="totalData"
                                :limit-data="cureentItemPage"
                                @update:currentPage="getDataPositionMap()"
                            ></paginate>
                        </div>
                    </div>
                </div>
            </div>
            <!-- //end view -->
            <!-- modal buat delete -->
            <div
                class="modal fade"
                id="modalDelete"
                tabindex="-1"
                role="dialog"
            >
                <div
                    class="modal-dialog modal-dialog-centered modal-lg"
                    role="document"
                >
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-header" id="deleteConfirmLabel">
                                Delete Confirmation
                            </h5>
                            <h6>Which one you want to delete?</h6>
                        </div>
                        <div class="modal-body">
                            <span>Position: </span
                            ><checkbox
                                class="talent-checkbox-lineheight"
                                v-model="isDeletePosition"
                                @change="checkAllCompetency()"
                                >{{ toBeDeletedName }}</checkbox
                            >
                            <hr />
                            <div>Competency:</div>
                            <div v-for="data in toBeDeletedList" :key="data.id">
                                <checkbox
                                    class="talent-checkbox-lineheight"
                                    :id="data.id.toString()"
                                    :value="data.id"
                                    v-model="toBeDeletedListIds"
                                    @change="checkUncheck()"
                                    >{{ data.name }}</checkbox
                                >
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div
                                class="col-md-12 d-flex justify-content-center"
                            >
                                <div
                                    class="col-md-12 d-flex justify-content-around"
                                >
                                    <button
                                        class="btn talent_bg_red talent_font_white talent_roundborder"
                                        type="button"
                                        data-dismiss="modal"
                                        @click.prevent="deleteData()"
                                        :disabled="!crud.delete"
                                    >
                                        Delete
                                    </button>
                                    <button
                                        class="btn talent_bg_blue talent_font_white talent_roundborder"
                                        type="button"
                                        data-dismiss="modal"
                                        @click.prevent="emptyDelete()"
                                    >
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <br />
            <!-- form section -->
            <position-mapping-form
                v-if="isView == false"
                :type.sync="type"
                :the-show-message.sync="theShowMessage"
                :is-show-message.sync="isShowMessage"
                :the-id="id"
                @update:type="fetching()"
            ></position-mapping-form>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import { Sort } from '../../../class/Sort';
import {
    DropdownService,
    PositionMapService,
    PositionMapFilterModel,
    PositionMapViewModel,
    DropDownModel,
    PositionMapDeleteModel,
    UserAccessCRUD,
    UserPrivilegeSettingsService

} from '../../../services/NSwagService';

@Component({
    props: ['type', 'id', 'isShowMessage', 'theShowMessage'],
    mounted: async function (this: PositionMapping) {
        await this.getAccess();
        await this.fetching();
    }
})
export default class PositionMapping extends Vue {
    props: any = {
        placeholder: '',
        readonly: true
    };

    type: string;
    id: number;

    isShowMessage: boolean;
    theShowMessage: string;

    isShowErrorMessage:boolean = false;
    theShowErrorMessage:string;

    isBusy: boolean = true;

    //boolean buat view
    isView: boolean = true;
    isInsert: boolean = false;
    isUpdate: boolean = false;
    isDetail: boolean = false;

    updateType: string = 'update';
    addType: string = 'add';
    detailType: string = 'detail';
    viewType: string = 'view';

    //variable
    cureentPage: number = 1;
    cureentItem: number = 0;
    cureentItemPage: number = 10;
    totalData: number = 0;

    formId: number = 0;

    PositionMapFilterModel: PositionMapFilterModel = {
        page: 0,
        itemPage: 0,
        startDate: null,
        endDate: null,
        positionName: '',
        competencyName: '',
        priority: '',
        proficiencyLevel: 0,
        orderBy: ''
    };

    //service
    positionServiceMan: PositionMapService = new PositionMapService();
    dropdownServiceMan: DropdownService = new DropdownService();
    privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

    // date variable
    rangePicker = {
        start: null,
        end: null
    };

    crud: UserAccessCRUD = {
        create: false, delete: false, read: false, update: false
    }

    //select variable
    selectedCompetencyName: DropDownModel = {
        id: 0,
        name: ''
    };
    selectedPriority: string = '';
    selectedProficiencyLevel: number = 0;

    //list showed variable
    PositionMapList: PositionMapViewModel[] = [];
    competencyNameList: DropDownModel[] = [];
    priorityLevelList: string[] = [];
    proficiencyLevelList: number[] = [];

    //Variable untuk sorting
    positionName = new Sort();
    competencyName = new Sort();
    priority = new Sort();
    proficiencyLevel = new Sort();
    createdDate = new Sort();
    updatedDate = new Sort();

    //variable buat delete
    toBeDeletedId: number = 0;
    toBeDeletedName: string = '';
    toBeDeletedList: DropDownModel[] = [];
    isDeletePosition: boolean = false;
    toBeDeletedListIds: number[] = [];

    async fetching() {
        this.setType();
        if (this.isView == true) {
            await this.getDataPositionMap();
            await this.getPriorityListData();
            await this.getproficiencyListData();
            await this.getCompetencyListData();
        }
    }

    async getAccess() {
        var data = await this.privilegeApi.crudAccessPage("PM");
        this.crud = data
    }

    // --------------- service / main function --------------------

    setFilter() {
        this.PositionMapFilterModel.page = this.cureentPage;
        this.PositionMapFilterModel.itemPage = this.cureentItemPage;
        this.PositionMapFilterModel.competencyName = this.selectedCompetencyName.name;
        this.PositionMapFilterModel.priority = this.selectedPriority;
        this.PositionMapFilterModel.proficiencyLevel = this.selectedProficiencyLevel;
        this.PositionMapFilterModel.startDate = this.rangePicker.start;
        this.PositionMapFilterModel.endDate = this.rangePicker.end;

        this.PositionMapFilterModel.endDate? this.PositionMapFilterModel.endDate.setHours(23, 59, 59) : this.PositionMapFilterModel.endDate;

    }

    async getDataPositionMap() {
        this.setFilter();
        try{
            this.isBusy = true;
            let getData = await this.positionServiceMan.getPaginate(
                this.PositionMapFilterModel
            );
            if (getData != null) {
                this.PositionMapList = getData.contentList;
                this.totalData = getData.totalData;
                this.cureentItem = getData.contentList.length;
            }
            this.isBusy = false;
        }catch{
            this.isShowErrorMessage = true;
            this.theShowErrorMessage = "Failed to Get Data."
            this.isBusy = false;
        }

    }

    async showDetailData() {}

    async getPriorityListData() {
        let getList = await this.dropdownServiceMan.getCompetencyPriority();
        this.priorityLevelList = getList;
    }

    async getCompetencyListData() {
        let getList = await this.dropdownServiceMan.getCompetency();
        this.competencyNameList = getList;
    }

    async getproficiencyListData() {
        let getList = await this.dropdownServiceMan.getCompetencyProficiency();
        this.proficiencyLevelList = getList;
    }

    async deleteData() {
        if (this.crud.delete == false) {
            return;
        }
        if (this.toBeDeletedId != null) {
            let deleteModel: PositionMapDeleteModel = {
                positionId: this.toBeDeletedId,
                competencyListId: this.toBeDeletedListIds
            }

            let message = await this.positionServiceMan.deleteModel(deleteModel);
            this.resetPageToDefault();
            this.notif(message);
        }
        // this.getDataPositionMap();
    }

    // ---------------- side function -------------------
    setType() {
        if (this.type != null && this.type != undefined) {
            if (this.type.toLowerCase().search('add') != -1) {
                this.isInsert = true;
                this.isUpdate = false;
                this.isDetail = false;
                this.isView = false;
            } else if (this.type.toLowerCase().search('update') != -1) {
                this.isInsert = false;
                this.isUpdate = true;
                this.isDetail = false;
                this.isView = false;
            } else if (this.type.toLowerCase().search('detail') != -1) {
                this.isInsert = false;
                this.isUpdate = false;
                this.isDetail = true;
                this.isView = false;
            } else if (this.type.toLowerCase().search('view') != -1) {
                this.isInsert = false;
                this.isUpdate = false;
                this.isDetail = false;
                this.isView = true;
            } else {
                this.isInsert = false;
                this.isUpdate = false;
                this.isDetail = false;
                this.isView = true;
            }
        }
    }

    ResetSuccessMessage() {
        this.isShowMessage = false;
        this.theShowMessage = null;
    }

    notif(result: string) {
        if (result.toLowerCase().search('success') != -1) {
            this.theShowMessage = result;
            this.isShowMessage = true;
            this.fetching();
        } else {
            this.fetching();
        }
    }
   
    async goToDetailPage(id: number) {
        if (this.crud.read == false ) {
            return;
        }
        this.type = this.detailType;
        this.id = id;
        await this.fetching();
    }

    async goToUpdatePage(id: number) {
        if (this.crud.update == false) {
            return;
        }
        this.resetAll();
        this.type = this.updateType;
        this.id = id;
        await this.fetching();
    }

    async goToAddPage() {
        if (this.crud.create == false) {
            return;
        }
        this.resetAll();
        this.type = this.addType;
        await this.fetching();
    }

    async goToPage(page) {
        this.cureentPage = page;
        await this.getDataPositionMap();
    }

    get totalPage() {
        return Math.ceil(this.totalData / this.cureentItemPage);
    }

    resetPageToDefault() {
        this.cureentPage = 1;
        this.cureentItemPage = 10;
    }
    ResetErrorMessage(){
        this.isShowErrorMessage = false;
        this.theShowErrorMessage = null;
    }

    async searchByInput() {
        this.resetPageToDefault();
        await this.getDataPositionMap();
    }

    async resetAll() {
        this.PositionMapFilterModel = {
            page: 0,
            itemPage: 0,
            startDate: null,
            endDate: null,
            positionName: '',
            competencyName: '',
            priority: '',
            proficiencyLevel: 0,
            orderBy: ''
        };

        this.rangePicker = {
            start: null,
            end: null
        };

        this.selectedCompetencyName = {
            id: 0,
            name: ''
        };
        this.selectedPriority = '';
        this.selectedProficiencyLevel = 0;

        this.resetPageToDefault();
        await this.getDataPositionMap();
    }

    async setDelete(id: number, positionName: string) {
        this.toBeDeletedId = id;
        this.toBeDeletedName = positionName;
        this.toBeDeletedList = await this.positionServiceMan.getList(id);
    }

    checkAllCompetency() {
        if (this.isDeletePosition === true) {
            for (let data of this.toBeDeletedList) {
                var hasExist = this.toBeDeletedListIds.includes(data.id);

                if (hasExist === false) {
                    this.toBeDeletedListIds.push(data.id);
                }
            }
        }

        else {
            this.toBeDeletedListIds = [];
        }

    }

    checkUncheck() {
        if (this.toBeDeletedListIds.length < this.toBeDeletedList.length) {
            this.isDeletePosition = false;
            return;
        }

        this.isDeletePosition = true;
    }

    emptyDelete() {
        this.toBeDeletedId = 0;
        this.toBeDeletedName = '';
        this.toBeDeletedList = [];
        this.isDeletePosition = false;
        this.toBeDeletedListIds = [];
    }

    async setPriority() {}
    async setCompetencyName() {
        this.PositionMapFilterModel.competencyName = this.selectedCompetencyName.name;
    }
    async setProficiencyLevel() {}

    //ClickSortPositionName
    async ClickSortPositionName() {
        this.ResetSort('positionName');
        //Sorting
        this.positionName.sorting();
        //Function Sorting
        if (
            this.positionName.sortup == true &&
            this.positionName.sortdown == false
        ) {
            this.PositionMapFilterModel.orderBy = 'PositionNameUp';
        } else if (
            this.positionName.sortup == false &&
            this.positionName.sortdown == true
        ) {
            this.PositionMapFilterModel.orderBy = 'PositionNameDown';
        } else {
            this.PositionMapFilterModel.orderBy = '';
        }

        await this.getDataPositionMap();
        return;
    }

    //ClickSortCompetencyName
    async ClickSortCompetencyName() {
        this.ResetSort('competencyName');
        //Sorting
        this.competencyName.sorting();
        //Function Sorting
        if (
            this.competencyName.sortup == true &&
            this.competencyName.sortdown == false
        ) {
            this.PositionMapFilterModel.orderBy = 'CompetencyNameUp';
        } else if (
            this.competencyName.sortup == false &&
            this.competencyName.sortdown == true
        ) {
            this.PositionMapFilterModel.orderBy = 'CompetencyNameDown';
        } else {
            this.PositionMapFilterModel.orderBy = '';
        }

        await this.getDataPositionMap();

        return;
    }

    //ClickSortPriority
    async ClickSortPriority() {
        this.ResetSort('priority');
        //Sorting
        this.priority.sorting();
        //Function Sorting
        if (this.priority.sortup == true && this.priority.sortdown == false) {
            this.PositionMapFilterModel.orderBy = 'PriorityUp';
        } else if (
            this.priority.sortup == false &&
            this.priority.sortdown == true
        ) {
            this.PositionMapFilterModel.orderBy = 'PriorityDown';
        } else {
            this.PositionMapFilterModel.orderBy = '';
        }

        await this.getDataPositionMap();
        return;
    }

    //ClickSortProficiencyLevel
    async ClickSortProficiencyLevel() {
        this.ResetSort('proficiencyLevel');
        //Sorting
        this.proficiencyLevel.sorting();
        //Function Sorting
        if (
            this.proficiencyLevel.sortup == true &&
            this.proficiencyLevel.sortdown == false
        ) {
            this.PositionMapFilterModel.orderBy = 'ProficiencyLevelUp';
        } else if (
            this.proficiencyLevel.sortup == false &&
            this.proficiencyLevel.sortdown == true
        ) {
            this.PositionMapFilterModel.orderBy = 'ProficiencyLevelDown';
        } else {
            this.PositionMapFilterModel.orderBy = '';
        }

        await this.getDataPositionMap();
        return;
    }

    //ClickSortCreatedDate
    async ClickSortCreatedDate() {
        this.ResetSort('createdDate');
        //Sorting
        this.createdDate.sorting();
        //Function Sorting
        if (
            this.createdDate.sortup == true &&
            this.createdDate.sortdown == false
        ) {
            this.PositionMapFilterModel.orderBy = 'CreatedDateUp';
        } else if (
            this.createdDate.sortup == false &&
            this.createdDate.sortdown == true
        ) {
            this.PositionMapFilterModel.orderBy = 'CreatedDateDown';
        } else {
            this.PositionMapFilterModel.orderBy = '';
        }

        await this.getDataPositionMap();
        return;
    }

    //ClickSortUpdatedDate
    async ClickSortUpdatedDate() {
        this.ResetSort('updatedDate');
        //Sorting
        this.updatedDate.sorting();
        //Function Sorting
        if (
            this.updatedDate.sortup == true &&
            this.updatedDate.sortdown == false
        ) {
            this.PositionMapFilterModel.orderBy = 'UpdatedDateUp';
        } else if (
            this.updatedDate.sortup == false &&
            this.updatedDate.sortdown == true
        ) {
            this.PositionMapFilterModel.orderBy = 'UpdatedDateDown';
        } else {
            this.PositionMapFilterModel.orderBy = '';
        }

        await this.getDataPositionMap();
        return;
    }

    //Reset Sorting
    ResetSort(parameter: string) {
        switch (parameter) {
            case 'positionName':
                this.competencyName.reset();
                this.priority.reset();
                this.proficiencyLevel.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'competencyName':
                this.positionName.reset();
                this.priority.reset();
                this.proficiencyLevel.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'priority':
                this.positionName.reset();
                this.competencyName.reset();
                this.proficiencyLevel.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'proficiencyLevel':
                this.positionName.reset();
                this.competencyName.reset();
                this.priority.reset();
                this.createdDate.reset();
                this.updatedDate.reset();
                return;
            case 'createdDate':
                this.positionName.reset();
                this.competencyName.reset();
                this.priority.reset();
                this.proficiencyLevel.reset();
                this.updatedDate.reset();
                return;
            case 'updatedDate':
                this.positionName.reset();
                this.competencyName.reset();
                this.priority.reset();
                this.proficiencyLevel.reset();
                this.createdDate.reset();
                return;
        }
    }
}
</script>
