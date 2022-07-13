<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="row">
            <div class="col-md-12">
                <h3 class="mb-4"><fa icon="folder"></fa> Setup > <span class="talent_font_red">Setup TSL</span></h3>

                <!-- View Top 5 Course List -->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="col-md-12 p-0 mb-md-4">
                        <div class="row align-items-start row justify-content-between mb-3 mx-2">
                            <a>Training Service Level Table</a>
                            <div class="d-flex">
                                <button class="btn talent_bg_cyan talent_roundborder talent_font_white px-xl-5" @click.prevent="enableInputField" :disabled="!crud.update">Edit</button>
                                <button class="btn talent_bg_green talent_roundborder talent_font_white px-xl-5 ml-2" @click.prevent="saveDataTSL" :disabled="!crud.update">Save</button>
                            </div>
                        </div>

                        <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow pl-5">
                            <div class="d-flex mb-lg-5 font-weight-bold">
                                <div class="w-25">
                                    <span>TSL</span>
                                </div>
                                <div class="w-25">
                                    <span>Basic Level</span>
                                </div>
                                <div class="w-25">
                                    <span>Fundamental Level</span>
                                </div>
                                <div class="w-25">
                                    <span>Advance Level</span>
                                </div>
                            </div>
                            <div class="d-flex mb-4">
                                <div class="w-25 font-weight-bold">
                                    <span>Sales</span>
                                </div>
                                <div class="w-25">
                                    <input class="text-center" type="number" max="100" min="0" v-model="setupTSLViewModel.sales.basicLevel" @change="checkValue()" :disabled="isDisabled"/> %
                                </div>
                                <div class="w-25">
                                    <input class="text-center" type="number" max="100" min="0" v-model="setupTSLViewModel.sales.fundamentalLevel" @change="checkValue()" :disabled="isDisabled"/> %
                                </div>
                                <div class="w-25">
                                    <input class="text-center" type="number" max="100" min="0" v-model="setupTSLViewModel.sales.advanceLevel" @change="checkValue()" :disabled="isDisabled"/> %
                                </div>
                            </div>
                            <div class="d-flex mb-4">
                                <div class="w-25 font-weight-bold">
                                    <span>After Sales</span>
                                </div>
                                <div class="w-25">
                                    <input class="text-center" type="number" max="100" min="0" v-model="setupTSLViewModel.afterSales.basicLevel" @change="checkValue()" :disabled="isDisabled"/> %
                                </div>
                                <div class="w-25">
                                    <input class="text-center" type="number" max="100" min="0" v-model="setupTSLViewModel.afterSales.fundamentalLevel" @change="checkValue()" :disabled="isDisabled"/> %
                                </div>
                                <div class="w-25">
                                    <input class="text-center" type="number" max="100" min="0" v-model="setupTSLViewModel.afterSales.advanceLevel" @change="checkValue()" :disabled="isDisabled"/> %
                                </div>
                            </div>
                            <div class="d-flex mb-4">
                                <div class="w-25 font-weight-bold">
                                    <span>Total</span>
                                </div>
                                <div class="w-25">
                                    <input class="text-center" type="number" max="100" min="0" v-model="setupTSLViewModel.total.basicLevel" @change="checkValue()" :disabled="isDisabled"/> %
                                </div>
                                <div class="w-25">
                                    <input class="text-center" type="number" max="100" min="0" v-model="setupTSLViewModel.total.fundamentalLevel" @change="checkValue()" :disabled="isDisabled"/> %
                                </div>
                                <div class="w-25">
                                    <input class="text-center" type="number" max="100" min="0" v-model="setupTSLViewModel.total.advanceLevel" @change="checkValue()" :disabled="isDisabled"/> %
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
    import { SetupTSLViewModel, SetupTslService, Sales, AfterSales, TotalSales, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService';
    import { PageEnums } from '../../enum/PageEnums';

    @Component({
        created: async function (this: SetupTSL) {
            await this.getAccess()
            this.initialize();
        }
    })
    export default class SetupTSL extends Vue {

        isBusy: boolean = false;
        serviceTSL: SetupTslService = new SetupTslService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        sales: Sales = {
            basicLevel: 0,
            fundamentalLevel: 0,
            advanceLevel: 0
        }

        afterSales: AfterSales = {
            basicLevel: 0,
            fundamentalLevel: 0,
            advanceLevel: 0
        }

        total: TotalSales = {
            basicLevel: 0,
            fundamentalLevel: 0,
            advanceLevel: 0
        }

        setupTSLViewModel: SetupTSLViewModel = {
            sales: this.sales,
            afterSales: this.afterSales,
            total: this.total
        }

        errorMessage: string = '';
        isDisabled: boolean = true;

        //validate value must not < 0 and >100
        checkValue() {
            this.setupTSLViewModel.sales.basicLevel > 100 ? this.setupTSLViewModel.sales.basicLevel = 100 : this.sales.basicLevel = this.sales.basicLevel;
            this.setupTSLViewModel.sales.basicLevel < 0 ? this.setupTSLViewModel.sales.basicLevel = 0 : this.setupTSLViewModel.sales.basicLevel = this.setupTSLViewModel.sales.basicLevel;
            this.setupTSLViewModel.sales.fundamentalLevel > 100 ? this.setupTSLViewModel.sales.fundamentalLevel = 100 : this.setupTSLViewModel.sales.fundamentalLevel = this.setupTSLViewModel.sales.fundamentalLevel;
            this.setupTSLViewModel.sales.fundamentalLevel < 0 ? this.setupTSLViewModel.sales.fundamentalLevel = 0 : this.setupTSLViewModel.sales.fundamentalLevel = this.setupTSLViewModel.sales.fundamentalLevel;
            this.setupTSLViewModel.sales.advanceLevel > 100 ? this.setupTSLViewModel.sales.advanceLevel = 100 : this.setupTSLViewModel.sales.advanceLevel = this.setupTSLViewModel.sales.advanceLevel;
            this.setupTSLViewModel.sales.advanceLevel < 0 ? this.setupTSLViewModel.sales.advanceLevel = 0 : this.setupTSLViewModel.sales.advanceLevel = this.setupTSLViewModel.sales.advanceLevel;

            this.setupTSLViewModel.afterSales.basicLevel > 100 ? this.setupTSLViewModel.afterSales.basicLevel = 100 : this.setupTSLViewModel.afterSales.basicLevel = this.setupTSLViewModel.afterSales.basicLevel;
            this.setupTSLViewModel.afterSales.basicLevel < 0 ? this.setupTSLViewModel.afterSales.basicLevel = 0 : this.setupTSLViewModel.afterSales.basicLevel = this.setupTSLViewModel.afterSales.basicLevel;
            this.setupTSLViewModel.afterSales.fundamentalLevel > 100 ? this.setupTSLViewModel.afterSales.fundamentalLevel = 100 : this.setupTSLViewModel.afterSales.fundamentalLevel = this.setupTSLViewModel.afterSales.fundamentalLevel;
            this.setupTSLViewModel.afterSales.fundamentalLevel < 0 ? this.setupTSLViewModel.afterSales.fundamentalLevel = 0 : this.setupTSLViewModel.afterSales.fundamentalLevel = this.setupTSLViewModel.afterSales.fundamentalLevel;
            this.setupTSLViewModel.afterSales.advanceLevel > 100 ? this.setupTSLViewModel.afterSales.advanceLevel = 100 : this.setupTSLViewModel.afterSales.advanceLevel = this.setupTSLViewModel.afterSales.advanceLevel;
            this.setupTSLViewModel.afterSales.advanceLevel < 0 ? this.setupTSLViewModel.afterSales.advanceLevel = 0 : this.setupTSLViewModel.afterSales.advanceLevel = this.setupTSLViewModel.afterSales.advanceLevel;

            this.setupTSLViewModel.total.basicLevel > 100 ? this.setupTSLViewModel.total.basicLevel = 100 : this.setupTSLViewModel.total.basicLevel = this.setupTSLViewModel.total.basicLevel;
            this.setupTSLViewModel.total.basicLevel < 0 ? this.setupTSLViewModel.total.basicLevel = 0 : this.setupTSLViewModel.total.basicLevel = this.setupTSLViewModel.total.basicLevel;
            this.setupTSLViewModel.total.fundamentalLevel > 100 ? this.setupTSLViewModel.total.fundamentalLevel = 100 : this.setupTSLViewModel.total.fundamentalLevel = this.setupTSLViewModel.total.fundamentalLevel;
            this.setupTSLViewModel.total.fundamentalLevel < 0 ? this.setupTSLViewModel.total.fundamentalLevel = 0 : this.setupTSLViewModel.total.fundamentalLevel = this.setupTSLViewModel.total.fundamentalLevel;
            this.setupTSLViewModel.total.advanceLevel > 100 ? this.setupTSLViewModel.total.advanceLevel = 100 : this.setupTSLViewModel.total.advanceLevel = this.setupTSLViewModel.total.advanceLevel;
            this.setupTSLViewModel.total.advanceLevel < 0 ? this.setupTSLViewModel.total.advanceLevel = 0 : this.setupTSLViewModel.total.advanceLevel = this.setupTSLViewModel.total.advanceLevel;
        }

        enableInputField() {
            return this.isDisabled = false;
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.SetupTSL);
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        async initialize() {
            await this.fetch();
        }

        async fetch() {
            this.isBusy = true;

            try {
                this.setupTSLViewModel = await this.serviceTSL.getAllTrainingServiceLevelData();
            } catch (e) {
                this.errorMessage = "Failed to get Data!";
            }

            this.isBusy = false;
        }

        async saveDataTSL() {
            if (!this.crud.update) {
                return
            }
            this.isBusy = true;
            let valid = await this.serviceTSL.updateTrainingServiceLevel(this.setupTSLViewModel);

            if (valid == false) {
                this.errorMessage = "Failed to edit TSL data!";
            }

            this.fetch();
            this.isDisabled = true;
            this.isBusy = false;
        }



    }
</script>
