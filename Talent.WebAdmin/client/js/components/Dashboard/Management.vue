<template>
    <div>
        <div class="row">
            <div class="col col-md-12">
                <h3>
                    <fa icon="chart-pie"></fa> Dashboard >
                    <span class="talent_font_red">Management</span>
                </h3>
                <br />

                <!--Row 1-->
                <div class=" row">
                    <div class="col-md-4 mb-4">
                        <div
                            class="talent_bg_shadow talent_bg_white  talent_paddingsmall talent_zoombig talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-3 text-center">
                                    <span class="talent_fontsize45"
                                        ><fa icon="users"></fa
                                    ></span>
                                </div>
                                <div class="col-md-9">
                                    <h5 class="talent_nomargin">
                                        <b>Total Users</b>
                                    </h5>
                                    <div
                                        v-if="isLoadingTotalUser"
                                        class="spinner-grow"
                                    ></div>
                                    <a
                                        v-if="!isLoadingTotalUser"
                                        class="talent_fontsize30 talent_font_gradientblue"
                                        ><b>{{ totalUsers }}</b></a
                                    >
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 mb-4">
                        <div
                            class="talent_bg_shadow talent_bg_white  talent_paddingsmall talent_zoombig talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-3 text-center">
                                    <span class="talent_fontsize45"
                                        ><fa icon="percent"></fa
                                    ></span>
                                </div>
                                <div class="col-md-9">
                                    <h5 class="talent_nomargin">
                                        <b>Total Access Rate</b>
                                    </h5>
                                    <div
                                        v-if="isLoadingTotalAccessRate"
                                        class="spinner-grow"
                                    ></div>
                                    <a
                                        v-if="!isLoadingTotalAccessRate"
                                        class="talent_fontsize30 talent_font_gradientblue"
                                        ><b>{{ totalAccessRate }}%</b></a
                                    >
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 mb-4">
                        <div
                            class="talent_bg_shadow talent_bg_white talent_paddingsmall talent_zoombig talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-3 text-center">
                                    <span class="talent_fontsize45"
                                        ><fa icon="file-alt"></fa
                                    ></span>
                                </div>
                                <div class="col-md-9">
                                    <h5 class="talent_nomargin">
                                        <b>Total Learning Enrolment</b>
                                    </h5>
                                    <div
                                        v-if="isLoadingTotalLearningEnrolment"
                                        class="spinner-grow"
                                    ></div>
                                    <a
                                        v-if="!isLoadingTotalLearningEnrolment"
                                        class="talent_fontsize30 talent_font_gradientblue"
                                        ><b>{{ totalLearningEnrollment }}</b></a
                                    >
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!--<br />-->
                <!--ROW 2-->
                <div class=" row">
                    <div class="col-md-4 mb-4">
                        <div
                            class="talent_bg_shadow talent_bg_white  talent_paddingsmall talent_zoombig talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-3 text-center">
                                    <span class="talent_fontsize45"
                                        ><fa icon="user"></fa
                                    ></span>
                                </div>
                                <div class="col-md-9">
                                    <h5 class="talent_nomargin">
                                        <b>Active Users</b>
                                    </h5>
                                    <div
                                        v-if="isLoadingActiveUsers"
                                        class="spinner-grow"
                                    ></div>
                                    <a
                                        v-if="!isLoadingActiveUsers"
                                        class="talent_fontsize30 talent_font_gradientblue"
                                        ><b>{{ totalActiveUsers }}</b></a
                                    >
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 mb-4">
                        <div
                            class="talent_bg_shadow talent_bg_white  talent_paddingsmall talent_zoombig talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-3 text-center">
                                    <span class="talent_fontsize45"
                                        ><fa icon="clock"></fa
                                    ></span>
                                </div>
                                <div class="col-md-9">
                                    <h5 class="talent_nomargin">
                                        <b>Average Access Time</b>
                                    </h5>
                                    <div
                                        v-if="isLoadingAverageAccessTime"
                                        class="spinner-grow"
                                    ></div>
                                    <a
                                        v-if="!isLoadingAverageAccessTime"
                                        class="talent_fontsize30 talent_font_gradientblue"
                                        ><b>{{ totalAverageAccessTime }}h</b></a
                                    >
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 mb-4">
                        <div
                            class="talent_bg_shadow talent_bg_white talent_paddingsmall talent_zoombig talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-3 text-center">
                                    <span class="talent_fontsize45"
                                        ><fa icon="percent"></fa
                                    ></span>
                                </div>
                                <div class="col-md-9">
                                    <h5 class="talent_nomargin">
                                        <b>Total Attendance Rate</b>
                                    </h5>
                                    <!--<div v-if="isLoadingTotalAttendanceRate" class="spinner-border"></div>-->
                                    <a
                                        v-if="!isLoadingTotalAttendanceRate"
                                        class="talent_fontsize30 talent_font_gradientblue"
                                        ><b>{{ totalAttendanceRate }}%</b></a
                                    >
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!--<br />-->
                <!--ROW 5-->
                <div class="row">
                    <div class="col-md-12 mb-4">
                        <div
                            class=" talent_bg_white talent_bg_shadow talent_padding talent_zoomsmall talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row align-items-center">
                                        <div class="col-md-6">
                                            <h4><b>TSL Report</b></h4>
                                        </div>
                                        <div
                                            class="col-md-6 d-flex align-items-end flex-column"
                                        >
                                            <!--<button class="btn talent_bg_blue talent_font_white">View Full Report</button>-->
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="text-center">
                                                <h5><b>Basic Level</b></h5>

                                                <div
                                                    v-if="isLoadingTslReport"
                                                    class="d-flex justify-content-center mt-5"
                                                >
                                                    <div
                                                        class="spinner-grow text-center talent_bg_blue"
                                                        style="width:10rem; height:10rem; border-width:0.75em"
                                                    ></div>
                                                </div>

                                                <div v-if="!isLoadingTslReport">
                                                    <vueapexcharts
                                                        width="100%"
                                                        type="radialBar"
                                                        :options="donuthollow"
                                                        :series="
                                                            seriesdonuthollowBasic
                                                        "
                                                    ></vueapexcharts>
                                                    <div
                                                        class="d-flex justify-content-around"
                                                    >
                                                        <div class="w-50">
                                                            Sales
                                                            <vueapexcharts
                                                                type="bar"
                                                                :options="
                                                                    tslSalesBarOptions
                                                                "
                                                                :series="
                                                                    basicSales
                                                                "
                                                                class="talent_legend"
                                                            ></vueapexcharts>
                                                        </div>

                                                        <div class="w-50">
                                                            After Sales
                                                            <vueapexcharts
                                                                type="bar"
                                                                :options="
                                                                    tslAfterSalesBarOptions
                                                                "
                                                                :series="
                                                                    basicAfterSales
                                                                "
                                                                class="talent_legend"
                                                            ></vueapexcharts>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="text-center">
                                                <h5><b>Fundamental TR</b></h5>

                                                <div
                                                    v-if="isLoadingTslReport"
                                                    class="d-flex justify-content-center mt-5"
                                                >
                                                    <div
                                                        class="spinner-grow text-center talent_bg_blue"
                                                        style="width:10rem; height:10rem; border-width:0.75em"
                                                    ></div>
                                                </div>

                                                <div v-if="!isLoadingTslReport">
                                                    <vueapexcharts
                                                        width="100%"
                                                        type="radialBar"
                                                        :options="donuthollow"
                                                        :series="
                                                            seriesdonuthollowFundamental
                                                        "
                                                    ></vueapexcharts>
                                                    <div
                                                        class="d-flex justify-content-around"
                                                    >
                                                        <div class="w-50">
                                                            Sales
                                                            <vueapexcharts
                                                                type="bar"
                                                                :options="
                                                                    tslSalesBarOptions
                                                                "
                                                                :series="
                                                                    fundamentalSales
                                                                "
                                                                class="talent_legend"
                                                            ></vueapexcharts>
                                                        </div>

                                                        <div class="w-50">
                                                            After Sales
                                                            <vueapexcharts
                                                                type="bar"
                                                                :options="
                                                                    tslAfterSalesBarOptions
                                                                "
                                                                :series="
                                                                    fundamentalAfterSales
                                                                "
                                                                class="talent_legend"
                                                            ></vueapexcharts>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="text-center">
                                                <h5><b>Advance Level</b></h5>
                                                <!--<vueapexcharts width="100%" type="donut" :options="donut2" :series="seriesdonut"></vueapexcharts>-->

                                                <div
                                                    v-if="isLoadingTslReport"
                                                    class="d-flex justify-content-center mt-5"
                                                >
                                                    <div
                                                        class="spinner-grow text-center talent_bg_blue"
                                                        style="width:10rem; height:10rem; border-width:0.75em"
                                                    ></div>
                                                </div>

                                                <div v-if="!isLoadingTslReport">
                                                    <vueapexcharts
                                                        width="100%"
                                                        type="radialBar"
                                                        :options="donuthollow"
                                                        :series="
                                                            seriesdonuthollowAdvance
                                                        "
                                                    ></vueapexcharts>
                                                    <div
                                                        class="d-flex justify-content-around"
                                                    >
                                                        <div class="w-50">
                                                            Sales
                                                            <vueapexcharts
                                                                type="bar"
                                                                :options="
                                                                    tslSalesBarOptions
                                                                "
                                                                :series="
                                                                    advanceSales
                                                                "
                                                                class="talent_legend"
                                                            ></vueapexcharts>
                                                        </div>

                                                        <div class="w-50">
                                                            After Sales
                                                            <vueapexcharts
                                                                type="bar"
                                                                :options="
                                                                    tslAfterSalesBarOptions
                                                                "
                                                                :series="
                                                                    advanceAfterSales
                                                                "
                                                                class="talent_legend"
                                                            ></vueapexcharts>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!--<br />-->
                <!--ROW 6-->
                <div class="row">
                    <div class="col-md-12 mb-4">
                        <div
                            class=" talent_bg_white talent_bg_shadow talent_padding talent_zoomsmall talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-12">
                                    <h4><b>Competency Mapping</b></h4>
                                    <br />
                                    <div class="row align-items-center">
                                        <div class="col-md-4">
                                            <div class="row">
                                                <div class="col-md-3">
                                                    <span>Sort by</span>
                                                </div>
                                                <div class="col-md-9">
                                                    <select
                                                        class="form-control"
                                                        v-model="filterPosition"
                                                        @change="
                                                            fetchPositionCompetencyMappingData()
                                                        "
                                                    >
                                                        <option
                                                            v-for="p in positions"
                                                            :key="p.id"
                                                            :value="
                                                                p.positionName
                                                            "
                                                            >{{
                                                                p.positionName
                                                            }}</option
                                                        >
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div
                                            class="col-md-8 d-flex align-items-end flex-column"
                                        >
                                            <!--<button class="btn talent_bg_blue talent_font_white">View Full Report</button>-->
                                        </div>
                                    </div>
                                    <br />
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="text-center">
                                                <h5><b>Hard Competency</b></h5>
                                                <div
                                                    v-if="
                                                        isLoadingCompetencyMapping
                                                    "
                                                    class="spinner-grow talent_bg_blue"
                                                    style="width:10rem; height:10rem;"
                                                ></div>
                                                <div
                                                    v-if="
                                                        !isLoadingCompetencyMapping
                                                    "
                                                >
                                                    <vueapexcharts
                                                        width="100%"
                                                        type="radar"
                                                        :options="hardRadar"
                                                        :series="
                                                            seriesradarHardCompetency
                                                        "
                                                    ></vueapexcharts>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="text-center">
                                                <h5><b>Soft Competency</b></h5>
                                                <div
                                                    v-if="
                                                        isLoadingCompetencyMapping
                                                    "
                                                    class="spinner-grow talent_bg_blue"
                                                    style="width:10rem; height:10rem;"
                                                ></div>
                                                <div
                                                    v-if="
                                                        !isLoadingCompetencyMapping
                                                    "
                                                >
                                                    <vueapexcharts
                                                        width="100%"
                                                        type="radar"
                                                        :options="softRadar"
                                                        :series="
                                                            seriesradarSoftCompetency
                                                        "
                                                    >
                                                    </vueapexcharts>
                                                </div>
                                            </div>
                                        </div>
                                        <!--<div class="col-md-4">
                                            <div class="text-center">
                                                <h5><b>KSB Mapping</b></h5>
                                                <vueapexcharts width="100%" type="radar" :options="radar" :series="seriesradar"></vueapexcharts>
                                                <div class="talent_legend">
                                                    <table class="w-100 text-center">
                                                        <tr>
                                                            <td><span class="dot talent_bg_darkblue"></span><a> Skill</a></td>
                                                            <td><span class="dot talent_bg_orange"></span><a> Behavior</a></td>
                                                            <td><span class="dot talent_bg_red"></span><a> Knowledge</a></td>
                                                        </tr>
                                                    </table>

                                                </div>
                                            </div>
                                        </div>-->
                                    </div>
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
import {
    DashboardOperationService,
    SetupTSLViewModel,
    Sales,
    AfterSales,
    TotalSales,
    CompetencyMappingModel,
    DashboardCompetencyMappingModel
} from '../../services/NSwagService';
import { setInterval } from 'timers';

@Component({
    props: ['framework', 'compiler'],
    created: async function(this: Management) {
        this.initialize();
        await setInterval(this.initialize, 24 * 60 * 60000);
    }
})
export default class Management extends Vue {
    serviceOperation: DashboardOperationService = new DashboardOperationService();
    isBusy: boolean = false;
    isLoadingTotalUser: boolean = true;
    isLoadingTotalAccessRate: boolean = true;
    isLoadingTotalLearningEnrolment: boolean = true;
    isLoadingActiveUsers: boolean = true;
    isLoadingAverageAccessTime: boolean = true;
    isLoadingTotalAttendanceRate: boolean = true;
    isLoadingTotalCertificationRate: boolean = true;
    isLoadingApproval: boolean = true;
    isLoadingComplaint: boolean = true;
    isLoadingUsers: boolean = true;
    isLoadingClassSchedule: boolean = true;
    isLoadingTslReport: boolean = true;
    isLoadingCompetencyMapping: boolean = true;
    isLoadingTop5Topic: boolean = true;
    isLoadingTop5Learning: boolean = true;
    isLoadingTotalLearningLibrary: boolean = true;
    isLoadingTop5News: boolean = true;
    isLoadingTop5Event: boolean = true;
    isLoadingTop5Coach: boolean = true;
    isLoadingTop5RewardType: boolean = true;
    isLoadingNpsReport: boolean = true;
    isLoadingMyInsightStatus: boolean = true;

    errorMessage: string = '';

    //-------------All Model--------------
    //Total Users
    totalUsers: number = 0;

    //Total Access Rate
    totalAccessRate: number = 0;

    //Total Learning Enrolment
    totalLearningEnrollment: number = 0;

    //Active Users
    totalActiveUsers: number = 0;

    //Average Access Time
    totalAverageAccessTime: number = 0;

    //Total Attendance Rate
    totalAttendanceRate: number = 0;

    //TSL Report
    sales: Sales = {
        basicLevel: 0,
        fundamentalLevel: 0,
        advanceLevel: 0
    };

    afterSales: AfterSales = {
        basicLevel: 0,
        fundamentalLevel: 0,
        advanceLevel: 0
    };

    total: TotalSales = {
        basicLevel: 0,
        fundamentalLevel: 0,
        advanceLevel: 0
    };

    TSLReportViewModel: SetupTSLViewModel = {
        sales: this.sales,
        afterSales: this.afterSales,
        total: this.total
    };

    //Competency Mapping
    filterPosition: string = 'Branch Manager';
    positions = [];
    hardCompetency: CompetencyMappingModel[] = [];
    softCompetency: CompetencyMappingModel[] = [];

    competencyMappingModel: DashboardCompetencyMappingModel = {
        hardCompetency: this.hardCompetency,
        softCompetency: this.softCompetency
    };

    //------------Chart Option-------------
    //----Radial Chart----
    //label name
    namedonuthollow: string = 'Completed';

    //sample
    datadonuthollow: number = 90;
    seriesdonuthollow: any = [this.datadonuthollow];

    //total certification rate
    seriesdonuthollowCeritificationRate: any[] = [];

    //tsl report
    seriesdonuthollowBasic: any[] = [];
    seriesdonuthollowFundamental: any = [];
    seriesdonuthollowAdvance: any = [];

    //option radial Chart
    donuthollow: any = {
        colors: ['#EC182E'],
        chart: {
            type: 'radialBar'
        },
        labels: [this.namedonuthollow],
        //Untuk mensetting legend
        legend: {
            show: false
        },
        dataLabels: {
            enabled: false
        },
        plotOptions: {
            radialBar: {
                startAngle: 0,
                //endAngle: ,
                hollow: {
                    margin: 0,
                    size: '60%',
                    background: '#fff',
                    position: 'front',
                    dropShadow: {
                        enabled: true,
                        top: 0,
                        left: 0,
                        blur: 4,
                        opacity: 0.24
                    }
                },
                track: {
                    show: true,
                    background: '#D5D5D5',
                    strokeWidth: '100%',
                    margin: 0 // margin is in pixels
                    //shadow
                    //dropShadow: {
                    //    enabled: true,
                    //    top: 0,
                    //    left: 0,
                    //    blur: 4,
                    //    opacity: 0.35
                    //}
                },
                dataLabels: {
                    showOn: 'always',
                    name: {
                        offsetY: -10,
                        show: true,
                        color: '#888',
                        fontSize: '17px'
                    },
                    value: {
                        //formatter: function (val) {
                        //    return parseInt(val);
                        //},
                        color: '#111',
                        fontSize: '36px',
                        show: true
                    }
                },
                stroke: {
                    colors: '#0085CA',
                    width: 2
                }
            }
        },
        fill: {
            type: 'gradient',
            gradient: {
                shade: 'dark',
                type: 'horizontal',
                shadeIntensity: 0.5,
                gradientToColors: ['#0085CA'],
                inverseColors: true,
                opacityFrom: 1,
                opacityTo: 1,
                stops: [0, 100]
            }
        },

        stroke: {
            //fill: {
            //    type: 'gradient',
            //    gradient: {
            //        shade: 'dark',
            //        type: 'horizontal',
            //        shadeIntensity: 0.5,
            //        gradientToColors: ['#0085CA'],
            //        inverseColors: true,
            //        opacityFrom: 1,
            //        opacityTo: 1,
            //        stops: [0, 100]
            //    }
            //},
        },
        responsive: [
            {
                breakpoint: undefined,
                options: {}
            }
        ]
    };

    //----------Competency Mapping---------------
    //Radar Chart punya properti kusus
    //nameradar1: string = 'Knowledge';
    //nameradar2: string = 'Skill';
    //nameradar3: string = 'Behavior';
    dataRadarHardCompetency: any = [];
    dataRadarSoftCompetency: any = [];
    //dataradar2: Array<number> = [4.5, 3.5, 2, 2, 2, 4, 4.5, 5];
    //dataradar3: Array<number> = [2, 4, 1.5, 1.5, 5, 4, 3.5, 5];
    //seriesradar: any = [{ name: this.nameradar1, data: this.dataradar1 }, { name: this.nameradar2, data: this.dataradar2 }, { name: this.nameradar3, data: this.dataradar3 }];
    //seriesradar: any = [{ name: this.nameradar1, data: this.dataRadarHardCompetency }];
    seriesradarHardCompetency: any = [];
    seriesradarSoftCompetency: any = [];
    hardRadar: any = {
        colors: ['#FF4C4C', '#38ACEC'],
        //series: [this.dataradar1],
        //labels: ['BTR', 'CIM', 'CFO', 'IAC', 'TW', 'AJU', 'MWO', 'QOR'],
        chart: {
            id: 'Radar',
            //Fitur bagus yang gak kepake
            toolbar: {
                show: false
            }
            //dropShadow: {
            //    enabled: true,
            //    top: 0,
            //    left: 0,
            //    blur: 3,
            //    opacity: 0.5
            //}
        },
        plotOptions: {
            radar: {
                //Untuk warna pada background chart
                polygons: {
                    strokeColor: '#e8e8e8',
                    connectorColors: '#e8e8e8',
                    fill: {
                        colors: ['#f8f8f8', '#fff']
                    }
                }
            }
        },

        //Untuk mensetting legend
        legend: {
            show: false,
            showForSingleSeries: true,
            showForNullSeries: true,
            showForZeroSeries: true,
            fontSize: '16px',
            onItemClick: {
                toggleDataSeries: false
            },
            onItemHover: {
                highlightDataSeries: true
            },
            markers: {
                width: 12,
                height: 12,
                radius: 12,
                strokeWidth: 2,
                strokeColor: '#0085CA'
            }
        },
        //Untuk memberi warna pada chart
        fill: {
            opacity: 0.1
            //colors: ["#EEA944", "#0085CA"]
        },
        //Stroke adalah garis pada chart
        stroke: {
            opacity: 0.1,
            show: true,
            width: 2
            //colors: ["#EEA944", "#0085CA"]
        },
        //Marker adalah buletan yang ada di ujung setiap sisi
        markers: {
            //size: 3,
            //hover: {
            //    size: 5
            //},
            //colors: ["#EEA944", "#0085CA"],
            size: 3
        },
        //Settingan untuk Tulisan
        dataLabels: {
            enabled: true,
            style: {
                fontSize: '12px',
                colors: ['#000000']
                //colors: ["#EEA944", "#0085CA"]
            }
        },
        //Untuk menentukan jumlah polygon
        yaxis: {
            show: true,
            tickAmount: 5,
            min: 5,
            max: 5
        },
        //Agar tulisannya mengikuti ukuran wadah
        responsive: [
            {
                breakpoint: undefined,
                options: {}
            }
        ],
        sparkline: {
            enabled: false
        }
        //events: {
        //    dataPointSelection: function (event) {
        //        console.log("tes");
        //    }
        //}
    };

    softRadar: any = {
        colors: ['#FF4C4C', '#38ACEC'],
        //series: [this.dataradar1],
        //labels: ['BTR', 'CIM', 'CFO', 'IAC', 'TW', 'AJU', 'MWO', 'QOR'],
        chart: {
            id: 'Radar',
            //Fitur bagus yang gak kepake
            toolbar: {
                show: false
            }
            //dropShadow: {
            //    enabled: true,
            //    top: 0,
            //    left: 0,
            //    blur: 3,
            //    opacity: 0.5
            //}
        },
        plotOptions: {
            radar: {
                //Untuk warna pada background chart
                polygons: {
                    strokeColor: '#e8e8e8',
                    connectorColors: '#e8e8e8',
                    fill: {
                        colors: ['#f8f8f8', '#fff']
                    }
                }
            }
        },

        //Untuk mensetting legend
        legend: {
            show: false,
            showForSingleSeries: true,
            showForNullSeries: true,
            showForZeroSeries: true,
            fontSize: '16px',
            onItemClick: {
                toggleDataSeries: false
            },
            onItemHover: {
                highlightDataSeries: true
            },
            markers: {
                width: 12,
                height: 12,
                radius: 12,
                strokeWidth: 2,
                strokeColor: '#0085CA'
            }
        },
        //Untuk memberi warna pada chart
        fill: {
            opacity: 0.1
            //colors: ["#EEA944", "#0085CA"]
        },
        //Stroke adalah garis pada chart
        stroke: {
            opacity: 0.1,
            show: true,
            width: 2
            //colors: ["#EEA944", "#0085CA"]
        },
        //Marker adalah buletan yang ada di ujung setiap sisi
        markers: {
            //size: 3,
            //hover: {
            //    size: 5
            //},
            //colors: ["#EEA944", "#0085CA"],
            size: 3
        },
        //Settingan untuk Tulisan
        dataLabels: {
            enabled: true,
            style: {
                fontSize: '12px',
                colors: ['#000000']
                //colors: ["#EEA944", "#0085CA"]
            }
        },
        //Untuk menentukan jumlah polygon
        yaxis: {
            show: true,
            tickAmount: 5,
            min: 5,
            max: 5
        },
        //Agar tulisannya mengikuti ukuran wadah
        responsive: [
            {
                breakpoint: undefined,
                options: {}
            }
        ],
        sparkline: {
            enabled: false
        }
        //events: {
        //    dataPointSelection: function (event) {
        //        console.log("tes");
        //    }
        //}
    };

    //data sales dan after sales bar chart for tsl report
    basicAfterSales: any = [];
    basicSales: any = [];
    fundamentalSales: any = [];
    fundamentalAfterSales: any = [];
    advanceSales: any = [];
    advanceAfterSales: any = [];
    tslSalesBarOptions: any = {
        colors: ['#EC182E', 'black'],
        labels: ['Sales'],
        chart: {
            id: 'Bar',
            //Fitur bagus yang gak kepake
            toolbar: {
                show: false
            },
            stacked: true,
            stackType: '100%',
            //shadow
            //dropShadow: {
            //    enabled: true,
            //    top: 0,
            //    left: 0,
            //    blur: 4,
            //    opacity: 0.24
            //},
            sparkline: {
                enabled: false
            }
        },
        //Untuk set style bar
        plotOptions: {
            bar: {
                horizontal: true,
                barHeight: '35px'
            }
        },
        //Untuk mensetting legend
        legend: {
            show: false,
            showForSingleSeries: true,
            showForNullSeries: true,
            showForZeroSeries: true,
            //fontSize: '7px',
            onItemClick: {
                toggleDataSeries: false
            },
            onItemHover: {
                highlightDataSeries: true
            }
            //markers: {
            //    width: 12,
            //    height: 12,
            //    radius: 12,
            //},
        },
        //Untuk memberi warna pada chart
        fill: {
            type: 'gradient',
            gradient: {
                shade: 'dark',
                type: 'vertical',
                shadeIntensity: 0.5,
                gradientToColors: ['#0085CA'],
                inverseColors: true,
                opacityFrom: 1,
                opacityTo: 1,
                stops: [0, 100]
            }
        },
        //Stroke adalah garis pada chart
        stroke: {
            show: false
        },
        yaxis: {
            show: false,
            showAlways: false,
            axisTicks: {
                show: false
            },
            axisborder: {
                show: false
            },
            crosshairs: {
                show: false
            }
        },
        xaxis: {
            labels: {
                show: false,
                showAlways: true
            },
            axisborder: {
                show: false
            },
            axisTicks: {
                show: false
            },
            crosshairs: {
                show: false
            }
        },
        grid: {
            show: false
        },
        dataLabels: {
            enabled: true,
            dropShadow: {
                enabled: true
            },
            style: {
                fontSize: '11px',
                colors: ['#fff']
            }
        },
        responsive: [
            {
                breakpoint: undefined,
                options: {}
            }
        ]
    };
    tslAfterSalesBarOptions: any = {
        colors: ['#EC182E', 'black'],
        labels: ['After Sales'],
        chart: {
            id: 'Bar',
            //Fitur bagus yang gak kepake
            toolbar: {
                show: false
            },
            stacked: true,
            stackType: '100%',
            //shadow
            //dropShadow: {
            //    enabled: true,
            //    top: 0,
            //    left: 0,
            //    blur: 4,
            //    opacity: 0.24
            //},
            sparkline: {
                enabled: false
            }
        },
        //Untuk set style bar
        plotOptions: {
            bar: {
                horizontal: true,
                barHeight: '35px'
            }
        },
        //Untuk mensetting legend
        legend: {
            show: false,
            showForSingleSeries: true,
            showForNullSeries: true,
            showForZeroSeries: true,
            //fontSize: '7px',
            onItemClick: {
                toggleDataSeries: false
            },
            onItemHover: {
                highlightDataSeries: true
            }
            //markers: {
            //    width: 12,
            //    height: 12,
            //    radius: 12,
            //},
        },
        //Untuk memberi warna pada chart
        fill: {
            type: 'gradient',
            gradient: {
                shade: 'dark',
                type: 'vertical',
                shadeIntensity: 0.5,
                gradientToColors: ['#0085CA'],
                inverseColors: true,
                opacityFrom: 1,
                opacityTo: 1,
                stops: [0, 100]
            }
        },
        //Stroke adalah garis pada chart
        stroke: {
            show: false
        },
        yaxis: {
            show: false,
            showAlways: false,
            axisTicks: {
                show: false
            },
            axisborder: {
                show: false
            },
            crosshairs: {
                show: false
            }
        },
        xaxis: {
            labels: {
                show: false,
                showAlways: true
            },
            axisborder: {
                show: false
            },
            axisTicks: {
                show: false
            },
            crosshairs: {
                show: false
            }
        },
        grid: {
            show: false
        },
        dataLabels: {
            enabled: true,
            dropShadow: {
                enabled: true
            },
            style: {
                fontSize: '11px',
                colors: ['#fff']
            }
        },
        responsive: [
            {
                breakpoint: undefined,
                options: {}
            }
        ]
    };

    //------------------INITIALIZE-----------------
    async initialize() {
        await this.fetchTotalUsers();
        await this.fetchTotalAccessRate();
        await this.fetchTotalLearningEnrolment();
        await this.fetchActiveUsers();
        await this.fetchAverageAccessTime();
        await this.fetchTSLReport();
        await this.fetchPositionNameDropdownData();
        await this.fetchPositionCompetencyMappingData();
    }

    //------------------FETCH DATA-----------------
    //fetch Total Users
    async fetchTotalUsers() {
        this.isBusy = true;
        this.isLoadingTotalUser = true;

        try {
            this.totalUsers = await this.serviceOperation.getAllTotalUsers();
        } catch (e) {
            this.errorMessage = 'Failed to get Total Users Data!';
        }
        this.isBusy = false;
        this.isLoadingTotalUser = false;
    }

    //fetch Total Access Rate
    async fetchTotalAccessRate() {
        this.isBusy = true;
        this.isLoadingTotalAccessRate = true;

        try {
            this.totalAccessRate = await this.serviceOperation.getAllTotalAccessRate();
        } catch (e) {
            this.errorMessage = 'Failed to get Total Access Rate Data!';
        }

        this.isBusy = false;
        this.isLoadingTotalAccessRate = false;
    }

    //fetch Total Learning Enrolment
    async fetchTotalLearningEnrolment() {
        this.isBusy = true;
        this.isLoadingTotalLearningEnrolment = true;

        try {
            this.totalLearningEnrollment = await this.serviceOperation.getTotalLearningEnrollment();
        } catch (e) {
            this.errorMessage = 'Failed to get Total Learning Enrolment Data!';
        }

        this.isBusy = false;
        this.isLoadingTotalLearningEnrolment = false;
    }

    //fetch Active Users
    async fetchActiveUsers() {
        this.isBusy = true;
        this.isLoadingActiveUsers = true;

        try {
            this.totalActiveUsers = await this.serviceOperation.getAllTotalActiveUsers();
        } catch (e) {
            this.errorMessage = 'Failed to get Data!';
        }

        this.isBusy = false;
        this.isLoadingActiveUsers = false;
    }

    //fetch Average Access Time
    async fetchAverageAccessTime() {
        this.isBusy = true;
        this.isLoadingAverageAccessTime = true;

        try {
            this.totalAverageAccessTime = await this.serviceOperation.getAverageAccessTime();
        } catch (e) {
            this.errorMessage = 'Failed to get Average Access timeData!';
        }

        this.isBusy = false;
        this.isLoadingAverageAccessTime = false;
    }

    //fetch Total Attendance Rate

    //fetch TSL Report
    async fetchTSLReport() {
        this.isBusy = true;
        this.isLoadingTslReport = true;

        try {
            this.TSLReportViewModel = await this.serviceOperation.getAllTslReport();
            console.log(this.TSLReportViewModel);
        } catch (e) {
            this.errorMessage = 'Failed to get Data!';
        }

        this.seriesdonuthollowBasic = [
            this.TSLReportViewModel.total.basicLevel
        ];
        this.seriesdonuthollowFundamental = [
            this.TSLReportViewModel.total.fundamentalLevel
        ];
        this.seriesdonuthollowAdvance = [
            this.TSLReportViewModel.total.advanceLevel
        ];
        this.basicAfterSales = [
            {
                name: ' ',
                data: [this.TSLReportViewModel.afterSales.basicLevel]
            },
            {
                name: ' ',
                data: [100 - this.TSLReportViewModel.afterSales.basicLevel]
            }
        ];
        this.basicSales = [
            {
                name: ' ',
                data: [this.TSLReportViewModel.sales.basicLevel]
            },
            {
                name: ' ',
                data: [100 - this.TSLReportViewModel.sales.basicLevel]
            }
        ];
        this.fundamentalSales = [
            {
                name: ' ',
                data: [this.TSLReportViewModel.sales.fundamentalLevel]
            },
            {
                name: ' ',
                data: [100 - this.TSLReportViewModel.sales.fundamentalLevel]
            }
        ];
        this.fundamentalAfterSales = [
            {
                name: ' ',
                data: [this.TSLReportViewModel.afterSales.fundamentalLevel]
            },
            {
                name: ' ',
                data: [
                    100 - this.TSLReportViewModel.afterSales.fundamentalLevel
                ]
            }
        ];
        this.advanceSales = [
            {
                name: ' ',
                data: [this.TSLReportViewModel.sales.advanceLevel]
            },
            {
                name: ' ',
                data: [100 - this.TSLReportViewModel.sales.advanceLevel]
            }
        ];
        this.advanceAfterSales = [
            {
                name: ' ',
                data: [this.TSLReportViewModel.afterSales.advanceLevel]
            },
            {
                name: ' ',
                data: [100 - this.TSLReportViewModel.afterSales.advanceLevel]
            }
        ];

        this.isBusy = false;
        this.isLoadingTslReport = false;
    }

    //fetch Competency Mapping
    async fetchPositionNameDropdownData() {
        this.isBusy = true;
        this.isLoadingCompetencyMapping = true;

        try {
            this.positions = await this.serviceOperation.getAllPositionName();
        } catch (e) {
            this.errorMessage = 'Failed to get Positions Data!';
        }

        this.isBusy = false;
        this.isLoadingCompetencyMapping = false;
    }

    async fetchPositionCompetencyMappingData() {
        this.isBusy = true;
        this.isLoadingCompetencyMapping = true;

        try {
            this.competencyMappingModel = await this.serviceOperation.getPositionCompetencyMapping(
                this.filterPosition
            );
        } catch (e) {
            this.errorMessage = 'Failed to get CompetencyMappingData Data!';
        }

        //hard competency
        var hardCompetency = this.competencyMappingModel.hardCompetency;
        var listHardCompetencyPrefixCode = [];
        var listHardCompetencyProficiencyLevel = [];
        var listHardCompetencyEvaluatedLevel = [];

        for (var i = 0; i < hardCompetency.length; i++) {
            listHardCompetencyProficiencyLevel.push(
                hardCompetency[i].proficiencyLevel
            );
            listHardCompetencyPrefixCode.push(hardCompetency[i].prefixCode);

            if (
                hardCompetency[i].scoreTotal != 0 &&
                hardCompetency[i].maxScoreTotal != 0
            ) {
                var avg =
                    (hardCompetency[i].scoreTotal /
                        hardCompetency[i].maxScoreTotal) *
                    5;
                listHardCompetencyEvaluatedLevel.push(avg);
            } else {
                listHardCompetencyEvaluatedLevel.push(0);
            }
        }

        this.seriesradarHardCompetency = [
            {
                name: 'Level',
                data: listHardCompetencyProficiencyLevel
            },
            {
                name: 'Evaluated Level',
                data: listHardCompetencyEvaluatedLevel
            }
        ];

        this.hardRadar = {
            colors: ['#FF4C4C', '#38ACEC'],
            labels: listHardCompetencyPrefixCode,
            chart: {
                id: 'Radar',
                //Fitur bagus yang gak kepake
                toolbar: {
                    show: false
                }
            },
            plotOptions: {
                radar: {
                    //Untuk warna pada background chart
                    polygons: {
                        strokeColor: '#e8e8e8',
                        connectorColors: '#e8e8e8',
                        fill: {
                            colors: ['#f8f8f8', '#fff']
                        }
                    }
                }
            },

            //Untuk mensetting legend
            legend: {
                show: false,
                showForSingleSeries: true,
                showForNullSeries: true,
                showForZeroSeries: true,
                fontSize: '16px',
                onItemClick: {
                    toggleDataSeries: false
                },
                onItemHover: {
                    highlightDataSeries: true
                },
                markers: {
                    width: 12,
                    height: 12,
                    radius: 12,
                    strokeWidth: 2,
                    strokeColor: '#0085CA'
                }
            },
            //Untuk memberi warna pada chart
            fill: {
                opacity: 0.1
            },
            //Stroke adalah garis pada chart
            stroke: {
                opacity: 0.1,
                show: true,
                width: 2
            },
            //Marker adalah buletan yang ada di ujung setiap sisi
            markers: {
                size: 3
            },
            //Settingan untuk Tulisan
            dataLabels: {
                enabled: true,
                style: {
                    fontSize: '12px',
                    colors: ['#000000']
                    //colors: ["#EEA944", "#0085CA"]
                }
            },
            //Untuk menentukan jumlah polygon
            yaxis: {
                show: true,
                tickAmount: 5,
                min: 0,
                max: 5
            },
            //Agar tulisannya mengikuti ukuran wadah
            responsive: [
                {
                    breakpoint: undefined,
                    options: {}
                }
            ],
            sparkline: {
                enabled: false
            }
        };

        //SOFT COMPETENCY
        var SoftCompetency = this.competencyMappingModel.softCompetency;
        var listSoftCompetencyPrefixCode = [];
        var listSoftCompetencyProficiencyLevel = [];
        var listSoftCompetencyEvaluatedLevel = [];

        for (var i = 0; i < SoftCompetency.length; i++) {
            listSoftCompetencyProficiencyLevel.push(
                SoftCompetency[i].proficiencyLevel
            );
            listSoftCompetencyPrefixCode.push(SoftCompetency[i].prefixCode);

            if (
                SoftCompetency[i].scoreTotal != 0 &&
                SoftCompetency[i].maxScoreTotal != 0
            ) {
                var avg =
                    (SoftCompetency[i].scoreTotal /
                        SoftCompetency[i].maxScoreTotal) *
                    5;
                listSoftCompetencyEvaluatedLevel.push(avg);
            } else {
                listSoftCompetencyEvaluatedLevel.push(0);
            }
        }

        this.seriesradarSoftCompetency = [
            {
                name: 'Level',
                data: listSoftCompetencyProficiencyLevel
            },
            {
                name: 'Evaluated Level',
                data: listSoftCompetencyEvaluatedLevel
            }
        ];

        this.softRadar = {
            colors: ['#FF4C4C', '#38ACEC'],
            labels: listSoftCompetencyPrefixCode,
            chart: {
                id: 'Radar',
                //Fitur bagus yang gak kepake
                toolbar: {
                    show: false
                }
            },
            plotOptions: {
                radar: {
                    //Untuk warna pada background chart
                    polygons: {
                        strokeColor: '#e8e8e8',
                        connectorColors: '#e8e8e8',
                        fill: {
                            colors: ['#f8f8f8', '#fff']
                        }
                    }
                }
            },

            //Untuk mensetting legend
            legend: {
                show: false,
                showForSingleSeries: true,
                showForNullSeries: true,
                showForZeroSeries: true,
                fontSize: '16px',
                onItemClick: {
                    toggleDataSeries: false
                },
                onItemHover: {
                    highlightDataSeries: true
                },
                markers: {
                    width: 12,
                    height: 12,
                    radius: 12,
                    strokeWidth: 2,
                    strokeColor: '#0085CA'
                }
            },
            //Untuk memberi warna pada chart
            fill: {
                opacity: 0.1
            },
            //Stroke adalah garis pada chart
            stroke: {
                opacity: 0.1,
                show: true,
                width: 2
            },
            //Marker adalah buletan yang ada di ujung setiap sisi
            markers: {
                size: 3
            },
            //Settingan untuk Tulisan
            dataLabels: {
                enabled: true,
                style: {
                    fontSize: '12px',
                    colors: ['#000000']
                    //colors: ["#EEA944", "#0085CA"]
                }
            },
            //Untuk menentukan jumlah polygon
            yaxis: {
                show: true,
                tickAmount: 5,
                min: 0,
                max: 5
            },
            //Agar tulisannya mengikuti ukuran wadah
            responsive: [
                {
                    breakpoint: undefined,
                    options: {}
                }
            ],
            sparkline: {
                enabled: false
            }
        };

        this.isBusy = false;
        this.isLoadingCompetencyMapping = false;
    }

    //---------------FUNCTION/METHOD--------------
}
</script>
