<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <div class="row mb-md-4">
            <div class="col-md-12">
                <h3 class="mb-md-4">
                    <fa icon="chart-bar"></fa> Report
                </h3>

                <div class="row">
                    <div v-for="report in allReports"
                         :key="report.italentReportId"
                         class="col-md-3 mb-3">
                        <div class="talent_bg_shadow talent_bg_white talent_paddingsmall talent_zoombig talent_roundborder"
                             @click="openReport(report)">
                            <div class="row d-flex align-items-center">
                                <div class="col-md-3 text-center">
                                    <span class="talent_fontsize45">
                                        <fa icon="file-alt"></fa>
                                    </span>
                                </div>
                                <div class="col-md-9">
                                    <h5 class="talent_nomargin talent_cursorpoint">
                                        <b>{{report.name}}</b>
                                    </h5>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!--<multiselect
                v-model="selectedReport"
                :options="allReports"
                placeholder="Select one"
                label="name"
                track-by="name"
                selectLabel="Press enter to select"
                @input="changingUrl"
            ></multiselect>-->
        </div>

        <div v-if="url.length > 0" class="row" style="height:100vh">
            <iframe width="100%"
                    height="100%"
                    :src="url"
                    frameborder="0"
                    allowfullscreen="true"></iframe>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import {
        ReportService,
        ReportModel,
        UserPrivilegeSettingsService,
        UserAccessCRUD
    } from '../../services/NSwagService';
    import Report from './Report.vue';
    import { PageEnums } from '../../enum/PageEnums';

    @Component({
        props: [],
        created: async function (this: Reports) {
            await this.fetch();
        }
    })
    export default class Reports extends Vue {
        isLoading: boolean = false;

        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage(PageEnums.Report);
            this.crud = data;
        }

        urlEmbed: string = '?rs:embed=true';
        url: string = '';

        crud: UserAccessCRUD = {
            create: false,
            delete: false,
            read: false,
            update: false
        };

        selectedReport: ReportModel = {};
        allReports: ReportModel[] = [];

        reportService: ReportService = new ReportService();


        changingUrl() {
            this.url = this.selectedReport.url + this.urlEmbed;
        }

        openReport(report) {
            this.selectedReport = report;

            if (this.selectedReport.italentReportId == 8) {
                window.location.href = "/Report/SurveyReport";
            }
            if (this.selectedReport.italentReportId == 11) {
                window.location.href = "/Report/ReportNPS";
            }
            else if (this.selectedReport.italentReportId == 12) {
                window.location.href = "/Report/TrainingScoreReport";
            }
            else {
                this.changingUrl();
            }
        }

        async fetch() {
            this.allReports = await this.reportService.getAllReport();
        }
    }
</script>