<template>
    <div>
        <div class="row">
            <div class="col col-md-12">
                <h3>
                    <fa icon="chart-pie"></fa> Dashboard >
                    <span class="talent_font_red">Operation</span>
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
                                        ><b>{{ totalAverageAccessTime }}m</b></a
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
                <!--ROW 3-->
                <div class="row">
                    <div class="col-md-4 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow h-100 talent_roundborder talent_zoombig"
                        >
                            <div class="talent_padding">
                                <h4><b>Total Certification Rate</b></h4>
                                <div
                                    v-if="isLoadingTotalCertificationRate"
                                    class="d-flex justify-content-center mt-5"
                                >
                                    <div
                                        class="spinner-grow text-center talent_bg_blue"
                                        style="width:10rem; height:10rem; border-width:0.75em"
                                    ></div>
                                </div>
                                <div v-if="!isLoadingTotalCertificationRate">
                                    <vueapexcharts
                                        width="100%"
                                        type="radialBar"
                                        :options="donuthollow"
                                        :series="
                                            seriesdonuthollowCeritificationRate
                                        "
                                    ></vueapexcharts>
                                    <div class="talent_legend">
                                        <table class="w-100 text-center">
                                            <tr>
                                                <td>
                                                    <span
                                                        class="dot talent_bg_gradient"
                                                    ></span
                                                    ><a> Completed</a>
                                                </td>
                                                <td>
                                                    <span
                                                        class="dot talent_bg_grey"
                                                    ></span
                                                    ><a> Incomplete</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow h-100 talent_roundborder talent_zoombig"
                        >
                            <div class="talent_padding">
                                <h4><b>Approval</b></h4>
                                <div
                                    v-if="isLoadingApproval"
                                    class="d-flex justify-content-center mt-5"
                                >
                                    <div
                                        class="spinner-grow text-center talent_bg_blue"
                                        style="width:10rem; height:10rem; border-width:0.75em"
                                    ></div>
                                </div>
                                <div
                                    v-if="!isLoadingApproval"
                                    class="h-100 overflow-auto"
                                >
                                    <table
                                        class="talent_nomargin w-100 d-block table table-sm"
                                        style="height:300px"
                                    >
                                        <thead>
                                            <tr>
                                                <th></th>
                                                <th>
                                                    Remaining
                                                </th>
                                                <th>
                                                    Completed
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr
                                                v-for="index in approvalList.listApproval"
                                                :key="index.id"
                                            >
                                                <td>
                                                    {{ index.approvalContent }}
                                                </td>
                                                <td class="text-center">
                                                    {{ index.remaining }}
                                                </td>
                                                <td class="text-center">
                                                    {{ index.completed }}
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig h-100 talent_roundborder"
                        >
                            <div class="talent_padding">
                                <h4><b>Complaint</b></h4>
                                <!--<vueapexcharts width="100%" type="radialBar" :options="donuthollow" :series="seriesdonuthollow"></vueapexcharts>
                                <div class="talent_legend">
                                    <table class="w-100 text-center">
                                        <tr>
                                            <td><span class="dot talent_bg_gradient"></span><a> Urgent</a></td>
                                            <td><span class="dot talent_bg_grey"></span><a> Not Urgent</a></td>
                                        </tr>
                                    </table>

                                </div>-->
                            </div>
                        </div>
                    </div>
                </div>
                <!--<br />-->
                <!--ROW 4-->
                <div class="row">
                    <div class="col-md-6 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <h4><b>Users</b></h4>
                            <div
                                v-if="isLoadingUsers"
                                class="d-flex justify-content-center mt-5"
                            >
                                <div
                                    class="spinner-grow text-center talent_bg_blue"
                                    style="width:10rem; height:10rem; border-width:0.75em"
                                ></div>
                            </div>

                            <div v-if="!isLoadingUsers">
                                <vueapexcharts
                                    width="100%"
                                    type="area"
                                    :options="line"
                                    :series="seriesUsers"
                                ></vueapexcharts>
                                <div class="talent_legend">
                                    <table class="w-100 text-center">
                                        <tr>
                                            <td>
                                                <span
                                                    class="dot talent_bg_darkblue"
                                                ></span
                                                ><a> Mobile App</a>
                                            </td>
                                            <td>
                                                <!--<span class="dot talent_bg_red"></span><a> Web</a>-->
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <div
                                v-if="isLoadingClassSchedule"
                                class="d-flex justify-content-center mt-5"
                            >
                                <div
                                    class="spinner-grow text-center talent_bg_blue"
                                    style="width:10rem; height:10rem; border-width:0.75em"
                                ></div>
                            </div>

                            <div
                                v-if="!isLoadingClassSchedule"
                                class="border-bottom"
                            >
                                <h6>
                                    <b
                                        >This Week's Class ({{
                                            classesList.totalThisWeekClass
                                        }})</b
                                    >
                                </h6>
                                <table
                                    class="talent_nomargin w-100 d-block table table-sm"
                                >
                                    <tbody>
                                        <tr
                                            v-for="index in classesList.thisWeekClassList"
                                            :key="index.id"
                                        >
                                            <td class="w-50">
                                                {{ index.courseName }}
                                            </td>
                                            <td class="w-50">
                                                {{
                                                    index.startDate | dateFormat
                                                }}
                                                -
                                                {{ index.endDate | dateFormat }}
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div
                                class="mb-3"
                                v-if="classesList.totalThisWeekClass > 5"
                            >
                                <a
                                    class="talent_font_red font_size_12"
                                    href="/Setup/ReleaseTraining"
                                    >More...</a
                                >
                            </div>
                            <div v-if="!isLoadingClassSchedule">
                                <h6>
                                    <b
                                        >Next Week's Class ({{
                                            classesList.totalNextWeekClass
                                        }})</b
                                    >
                                </h6>
                                <table
                                    class="talent_nomargin w-100 d-block table table-sm"
                                >
                                    <tbody>
                                        <tr
                                            v-for="index in classesList.nextWeekClassList"
                                            :key="index.id"
                                        >
                                            <td class="w-50">
                                                {{ index.courseName }}
                                            </td>
                                            <td class="w-50">
                                                {{
                                                    index.startDate | dateFormat
                                                }}
                                                -
                                                {{ index.endDate | dateFormat }}
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div v-if="classesList.totalNextWeekClass > 5">
                                <a
                                    class="talent_font_red font_size_12"
                                    href="/Setup/ReleaseTraining"
                                    >More...</a
                                >
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
                <!--<br />-->
                <!--ROW 7-->
                <div class="row">
                    <div class="col-md-3 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-12">
                                    <h4><b>Top 5 Topic</b></h4>
                                </div>
                            </div>
                            <div
                                v-if="isLoadingTop5Topic"
                                class="d-flex justify-content-center"
                            >
                                <div
                                    class="spinner-grow text-center mt-3 talent_bg_blue"
                                    style="width:8rem; height:8rem; border-width:0.75em"
                                ></div>
                            </div>
                            <div v-if="!isLoadingTop5Topic" class="row">
                                <div class="col-md-12">
                                    <table class="h-100 w-100">
                                        <tr
                                            v-for="(topic,
                                            index) in topicList.topicsList"
                                            :key="topic.id"
                                        >
                                            <td class="align-top">
                                                {{ index + 1 }}
                                            </td>
                                            <td>{{ topic.topicName }}</td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-12">
                                    <h4><b>Top 5 Learning</b></h4>
                                </div>
                            </div>
                            <div
                                v-if="isLoadingTop5Learning"
                                class="d-flex justify-content-center"
                            >
                                <div
                                    class="spinner-grow text-center mt-3 talent_bg_blue"
                                    style="width:8rem; height:8rem; border-width:0.75em"
                                ></div>
                            </div>
                            <div v-if="!isLoadingTop5Learning" class="row">
                                <div class="col-md-12">
                                    <table class="h-100 w-100">
                                        <tr
                                            v-for="(learning,
                                            index) in learningList.learningsList"
                                            :key="learning.id"
                                        >
                                            <td class="align-top">
                                                {{ index + 1 }}
                                            </td>
                                            <td>{{ learning.courseName }}</td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <h4><b>Total Learning Library</b></h4>
                            <div
                                v-if="isLoadingTotalLearningLibrary"
                                class="d-flex justify-content-center"
                            >
                                <div
                                    class="spinner-grow text-center mt-3 talent_bg_blue"
                                    style="width:8rem; height:8rem; border-width:0.75em"
                                ></div>
                            </div>

                            <div
                                v-if="!isLoadingTotalLearningLibrary"
                                class="row text-center"
                            >
                                <div class="col-md-12">
                                    <a
                                        class="talent_fontsize80 talent_font_gradientblue"
                                        ><b>{{
                                            learningLibraryModel.total
                                        }}</b></a
                                    >
                                </div>
                            </div>
                            <div
                                v-if="!isLoadingTotalLearningLibrary"
                                class="row text-center"
                            >
                                <div class="col-md-12">
                                    <table class="h-100 w-100 text-left">
                                        <tr>
                                            <td>
                                                <b
                                                    >{{
                                                        learningLibraryModel.totalCourses
                                                    }}
                                                    Courses</b
                                                >
                                            </td>
                                            <td>
                                                <b
                                                    >{{
                                                        learningLibraryModel.totalTask
                                                    }}
                                                    Tasks</b
                                                >
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <b
                                                    >{{
                                                        learningLibraryModel.totalModules
                                                    }}
                                                    Modules</b
                                                >
                                            </td>
                                            <td>
                                                <b
                                                    >{{
                                                        learningLibraryModel.totalPopQuiz
                                                    }}
                                                    Pop Quizzes</b
                                                >
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!--<br />-->
                <!--ROW 8-->
                <div class="row">
                    <div class="col-md-3 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-12">
                                    <h4><b>Top 5 News</b></h4>
                                </div>
                            </div>
                            <div
                                v-if="isLoadingTop5News"
                                class="d-flex justify-content-center"
                            >
                                <div
                                    class="spinner-grow text-center mt-3 talent_bg_blue"
                                    style="width:8rem; height:8rem; border-width:0.75em"
                                ></div>
                            </div>
                            <div v-if="!isLoadingTop5News" class="row">
                                <div class="col-md-12">
                                    <table class="h-100 w-100">
                                        <tr
                                            v-for="(news,
                                            index) in newsList.topNewsList"
                                            :key="news.id"
                                        >
                                            <td class="align-top">
                                                {{ index + 1 }}
                                            </td>
                                            <td>{{ news.title }}</td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-12">
                                    <h4><b>Top 5 Event</b></h4>
                                </div>
                            </div>
                            <div
                                v-if="isLoadingTop5Event"
                                class="d-flex justify-content-center"
                            >
                                <div
                                    class="spinner-grow text-center mt-3 talent_bg_blue"
                                    style="width:8rem; height:8rem; border-width:0.75em"
                                ></div>
                            </div>
                            <div v-if="!isLoadingTop5Event" class="row">
                                <div class="col-md-12">
                                    <table class="h-100 w-100">
                                        <tr
                                            v-for="(event,
                                            index) in eventsList.topEventsList"
                                            :key="event.id"
                                        >
                                            <td class="align-top">
                                                {{ index + 1 }}
                                            </td>
                                            <td>{{ event.title }}</td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-12">
                                    <h4><b>Top 5 Coach</b></h4>
                                </div>
                            </div>
                            <div
                                v-if="isLoadingTop5Coach"
                                class="d-flex justify-content-center"
                            >
                                <div
                                    class="spinner-grow text-center mt-3 talent_bg_blue"
                                    style="width:8rem; height:8rem; border-width:0.75em"
                                ></div>
                            </div>
                            <div v-if="!isLoadingTop5Coach" class="row">
                                <div class="col-md-12">
                                    <table class="h-100 w-100">
                                        <tr
                                            v-for="(coach,
                                            index) in coachList.coachList"
                                            :key="coach.id"
                                        >
                                            <td class="align-top">
                                                {{ index + 1 }}
                                            </td>
                                            <td>{{ coach.coachName }}</td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <div class="row">
                                <div class="col-md-12">
                                    <h4><b>Top 5 Reward Type</b></h4>
                                </div>
                            </div>
                            <div
                                v-if="isLoadingTop5RewardType"
                                class="d-flex justify-content-center"
                            >
                                <div
                                    class="spinner-grow text-center mt-3 talent_bg_blue"
                                    style="width:8rem; height:8rem; border-width:0.75em"
                                ></div>
                            </div>
                            <div v-if="!isLoadingTop5RewardType" class="row">
                                <div class="col-md-12">
                                    <table class="h-100 w-100">
                                        <tr
                                            v-for="(reward,
                                            index) in rewardList.topRewardTypeList"
                                            :key="reward.id"
                                        >
                                            <td class="align-top">
                                                {{ index + 1 }}
                                            </td>
                                            <td>{{ reward.rewardType }}</td>
                                        </tr>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!--<br />-->
                <!--ROW 9-->
                <div class="row">
                    <div class="col-md-6 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <h4><b>NPS Report</b></h4>
                            <div
                                v-if="isLoadingNpsReport"
                                class="d-flex justify-content-center"
                            >
                                <div
                                    class="spinner-grow text-center mt-5 talent_bg_blue"
                                    style="width:10rem; height:10rem; border-width:0.75em"
                                ></div>
                            </div>
                            <div v-if="!isLoadingNpsReport">
                                <vueapexcharts
                                    width="100%"
                                    type="bar"
                                    :options="barNPS"
                                    :series="seriesbarNpsReport"
                                ></vueapexcharts>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 mb-4">
                        <div
                            class="talent_bg_white talent_bg_shadow talent_zoombig talent_padding h-100 talent_roundborder"
                        >
                            <h4><b>My Insight Status</b></h4>
                            <div
                                v-if="isLoadingMyInsightStatus"
                                class="d-flex justify-content-center"
                            >
                                <div
                                    class="spinner-grow text-center mt-5 talent_bg_blue"
                                    style="width:10rem; height:10rem; border-width:0.75em"
                                ></div>
                            </div>
                            <vueapexcharts
                                width="100%"
                                type="bar"
                                :options="barInsight"
                                :series="seriesbarMyInsight"
                            ></vueapexcharts>
                        </div>
                    </div>
                </div>
                <!--<br />-->
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
    DashboardApprovalListViewModel,
    DashboardApprovalData,
    DashboardUsersThisYearListViewModel,
    DashboardClassListViewModel,
    DashboardClassData,
    DashboardTop5TopicViewModel,
    DashboardTop5TopicData,
    DashboardTop5LearningViewModel,
    DashboardTop5LearningData,
    DashboardTotalLearningLibraryViewModel,
    DashboardTop5NewsViewModel,
    DashboardTop5NewsData,
    DashboardTop5EventsViewModel,
    DashboardTop5EventsData,
    DashboardTop5CoachViewModel,
    DashboardTop5CoachData,
    DashboardTop5RewardTypeViewModel,
    DashboardTop5RewardTypeData,
    DashboardNPSReportViewModel,
    DashboardMyInsightViewModel,
    DashboardMyInsightData,
    DashboardCompetencyMappingModel,
    CompetencyMappingModel
} from '../../services/NSwagService';
import { setInterval } from 'timers';

@Component({
    props: ['framework', 'compiler'],
    created: async function(this: Operation) {
        this.initialize();
        await setInterval(this.initialize, 24 * 60 * 60000);
    }
})
export default class Operation extends Vue {
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

    //Total Certification Rate
    totalCertificateRate: number = 0;

    //Approval
    approvalModel: DashboardApprovalData[] = [];
    approvalList: DashboardApprovalListViewModel = {
        listApproval: this.approvalModel
    };

    //Complaint

    //Users (@Year)
    totalUserThisYearList: DashboardUsersThisYearListViewModel = {
        januaryUsersData: 0,
        februaryUsersData: 0,
        marchUsersData: 0,
        aprilUsersData: 0,
        mayUsersData: 0,
        juneUsersData: 0,
        julyUsersData: 0,
        augustUsersData: 0,
        septemberUsersData: 0,
        octoberUsersData: 0,
        novemberUsersData: 0,
        decemberUsersData: 0
    };

    //Classes
    classesModel: DashboardClassData[] = [];
    classesList: DashboardClassListViewModel = {
        thisWeekClassList: this.classesModel,
        totalThisWeekClass: 0,
        nextWeekClassList: this.classesModel,
        totalNextWeekClass: 0
    };

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

    //Top 5 Topic
    topicModel: DashboardTop5TopicData[] = [];
    topicList: DashboardTop5TopicViewModel = {
        topicsList: this.topicModel
    };

    //Top 5 Learning
    learningModel: DashboardTop5LearningData[] = [];
    learningList: DashboardTop5LearningViewModel = {
        learningsList: this.learningModel
    };

    //Total Learning Library
    learningLibraryModel: DashboardTotalLearningLibraryViewModel = {
        total: 0,
        totalCourses: 0,
        totalModules: 0,
        totalPopQuiz: 0,
        totalTask: 0
    };

    //Top 5 News
    newsModel: DashboardTop5NewsData[] = [];
    newsList: DashboardTop5NewsViewModel = {
        topNewsList: this.newsModel
    };

    //Top 5 Events
    eventsModel: DashboardTop5EventsData[] = [];
    eventsList: DashboardTop5EventsViewModel = {
        topEventsList: this.eventsModel
    };

    //Top 5 Coach
    coachModel: DashboardTop5CoachData[] = [];
    coachList: DashboardTop5CoachViewModel = {
        coachList: this.coachModel
    };

    //Top 5 Reward Type
    rewardModel: DashboardTop5RewardTypeData[] = [];
    rewardList: DashboardTop5RewardTypeViewModel = {
        topRewardTypeList: this.rewardModel
    };

    //NPS Report
    npsReportModel: DashboardNPSReportViewModel = {
        detractors: 0,
        passive: 0,
        promotor: 0
    };

    //My Insight
    myInsightModel: DashboardMyInsightData = {
        getMonth: '',
        totalInsight: 0
    };
    myInsightList: DashboardMyInsightViewModel = {
        thisMonth: this.myInsightModel = null,
        lastMonth: this.myInsightModel = null,
        last2Month: this.myInsightModel = null
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
        //events: {
        //    dataPointSelection: function (event) {
        //        console.log("tes");
        //    }
        //}
    };

    //Line Chart
    //Users
    name1: string = 'MobileApp';
    //name2: string = 'MobileApp';
    usersDataList: any = [];
    seriesUsers: any = [];
    line: any = {
        //colors: ["#0085CA", "#EC182E"],
        colors: ['#0085CA'],
        labels: [
            'Jan',
            'Feb',
            'Mar',
            'Apr',
            'Mei',
            'Jun',
            'Jul',
            'Aug',
            'Sep',
            'Oct',
            'Nov',
            'Dec'
        ],
        chart: {
            id: 'Line',
            //Fitur bagus yang gak kepake
            toolbar: {
                show: false
            },
            sparkline: {
                enabled: false
            }
            //shadow
            //dropShadow: {
            //    enabled: true,
            //    top: 0,
            //    left: 0,
            //    bottom: 2,
            //    blur: 3,
            //    opacity: 0.35
            //}
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
            //Markers adalah gambar bulet yang ada di legend bar
            markers: {
                width: 12,
                height: 12,
                radius: 12
            }
        },
        //Untuk memberi warna pada chart
        //fill: {
        //    type: 'gradient',
        //    gradient: {
        //        shade: 'dark',
        //        type: 'vertical',
        //        shadeIntensity: 0.5,
        //        gradientToColors: ['#00AA00', '#0085CA'],
        //        //inverseColors: true,
        //        opacityFrom: [1,1],
        //        opacityTo: [1,1],
        //        stops: [0, 100]
        //    }
        //},
        //Stroke adalah garis pada chart
        stroke: {
            opacity: 1,
            show: true,
            width: 4
        },

        //Marker adalah buletan yang ada di ujung setiap sisi
        markers: {
            show: false,
            size: 0,
            hover: {
                size: 5
            }
        },
        responsive: [
            {
                breakpoint: undefined,
                options: {}
            }
        ],

        dataLabels: {
            enabled: false
        }
    };

    //Bar Chart
    namebarNPS: string = 'NPS Report';
    seriesbarNpsReport: any = [];
    namebarMyInsight: string = 'My Insight';
    seriesbarMyInsight: any = [];

    //My Insight bar options
    barInsight: any = {
        colors: ['#EC182E'],
        chart: {
            id: 'Bar',
            //Fitur bagus yang gak kepake
            toolbar: {
                show: false
            },
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
                radius: 12
            }
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
            showAlways: true
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
                fontSize: '16px',
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

    //NPS Report bar options
    barNPS: any = {
        colors: ['#EC182E'],
        labels: ['Promotor', 'Passive', 'Detractors'],
        chart: {
            id: 'Bar',
            //Fitur bagus yang gak kepake
            toolbar: {
                show: false
            },
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
                radius: 12
            }
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
            showAlways: true
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
                fontSize: '16px',
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
        await this.fetchTotalCertificateRate();
        await this.fetchApproval();
        await this.fetchTotalUsersThisYearModel();
        await this.fetchClassesSchedule();
        await this.fetchTSLReport();
        await this.fetchtop5Topic();
        await this.fetchTop5Learning();
        await this.fetchTotalLearningLibrary();
        await this.fetchTop5News();
        await this.fetchTop5Events();
        await this.fetchTop5Coaches();
        await this.fetchTop5RewardType();
        await this.fetchNpsReport();
        await this.fetchMyInsight();
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

    //fetch Total Certificate Rate
    async fetchTotalCertificateRate() {
        this.isBusy = true;
        this.isLoadingTotalCertificationRate = true;

        try {
            this.totalCertificateRate = await this.serviceOperation.getTotalCertificateRate();
        } catch (e) {
            this.errorMessage = 'Failed to get Total Certificate Rate Data!';
        }

        this.seriesdonuthollowCeritificationRate = [this.totalCertificateRate];

        this.isBusy = false;
        this.isLoadingTotalCertificationRate = false;
    }

    //fetch Approval
    async fetchApproval() {
        this.isBusy = true;
        this.isLoadingApproval = true;

        try {
            this.approvalList = await this.serviceOperation.getAllApprovalContent();
        } catch (e) {
            this.errorMessage = 'Failed to get Apprpoval Data!';
        }

        this.isBusy = false;
        this.isLoadingApproval = false;
    }

    //fetch Complaint

    //fetch Users @Year
    async fetchTotalUsersThisYearModel() {
        this.isBusy = true;
        this.isLoadingUsers = true;

        try {
            this.totalUserThisYearList = await this.serviceOperation.getAllTotalUsersThisYear();
        } catch (e) {
            this.errorMessage = 'Failed to get Users Data!';
        }

        this.usersDataList = [
            this.totalUserThisYearList.januaryUsersData,
            this.totalUserThisYearList.februaryUsersData,
            this.totalUserThisYearList.marchUsersData,
            this.totalUserThisYearList.aprilUsersData,
            this.totalUserThisYearList.mayUsersData,
            this.totalUserThisYearList.juneUsersData,
            this.totalUserThisYearList.julyUsersData,
            this.totalUserThisYearList.augustUsersData,
            this.totalUserThisYearList.septemberUsersData,
            this.totalUserThisYearList.octoberUsersData,
            this.totalUserThisYearList.novemberUsersData,
            this.totalUserThisYearList.decemberUsersData
        ];

        this.seriesUsers = [{ name: this.name1, data: this.usersDataList }];

        //kalau mau dua data contoh:
        //series: any = [{ name: this.name1, data: this.data1 }, { name: this.name2, data: this.data2 }];
        this.isBusy = false;
        this.isLoadingUsers = false;
    }

    //fetch Classes Schedule
    async fetchClassesSchedule() {
        this.isBusy = true;
        this.isLoadingClassSchedule = true;

        try {
            this.classesList = await this.serviceOperation.getTwoWeeksClassSchedule();
        } catch (e) {
            this.errorMessage = 'Failed to get Classes Data!';
        }

        this.isBusy = false;
        this.isLoadingClassSchedule = false;
    }

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

    //fetch Top 5 Topic
    async fetchtop5Topic() {
        this.isBusy = true;
        this.isLoadingTop5Topic = true;

        try {
            this.topicList = await this.serviceOperation.getTop5Topics();
        } catch (e) {
            this.errorMessage = 'Failed to get Top 5 Topic Data!';
        }

        this.isBusy = false;
        this.isLoadingTop5Topic = false;
    }

    //fetch Top 5 Learning
    async fetchTop5Learning() {
        this.isBusy = true;
        this.isLoadingTop5Learning = true;

        try {
            this.learningList = await this.serviceOperation.getTop5Learnings();
        } catch (e) {
            this.errorMessage = 'Failed to get Top 5 Learning Data!';
        }

        this.isBusy = false;
        this.isLoadingTop5Learning = false;
    }

    //fetch Total Learning Library
    async fetchTotalLearningLibrary() {
        this.isBusy = true;
        this.isLoadingTotalLearningLibrary = true;

        try {
            this.learningLibraryModel = await this.serviceOperation.getTotalLearningLibrary();
        } catch (e) {
            this.errorMessage = 'Failed to get Total Learning Library Data!';
        }

        this.isBusy = false;
        this.isLoadingTotalLearningLibrary = false;
    }

    //fetch Top 5 News
    async fetchTop5News() {
        this.isBusy = true;
        this.isLoadingTop5News = true;

        try {
            this.newsList = await this.serviceOperation.getTop5News();
        } catch (e) {
            this.errorMessage = 'Failed to get Top 5 News Data!';
        }

        this.isBusy = false;
        this.isLoadingTop5News = false;
    }

    //fetch Top 5 Events
    async fetchTop5Events() {
        this.isBusy = true;
        this.isLoadingTop5Event = true;

        try {
            this.eventsList = await this.serviceOperation.getTop5Events();
        } catch (e) {
            this.errorMessage = 'Failed to get Top 5 Event Data!';
        }

        this.isBusy = false;
        this.isLoadingTop5Event = false;
    }

    //fetch Top 5 Coaches
    async fetchTop5Coaches() {
        this.isBusy = true;
        this.isLoadingTop5Coach = true;

        try {
            this.coachList = await this.serviceOperation.getTop5Coach();
        } catch (e) {
            this.errorMessage = 'Failed to get Top 5 Coaches Data!';
        }

        this.isBusy = false;
        this.isLoadingTop5Coach = false;
    }

    //fetch Top 5 Reward Type
    async fetchTop5RewardType() {
        this.isBusy = true;
        this.isLoadingTop5RewardType = true;

        try {
            this.rewardList = await this.serviceOperation.getTop5RewardType();
        } catch (e) {
            this.errorMessage = 'Failed to get Top 5 Reward Type Data!';
        }

        this.isBusy = false;
        this.isLoadingTop5RewardType = false;
    }

    //fetch NPS Report
    async fetchNpsReport() {
        this.isBusy = true;
        this.isLoadingNpsReport = true;

        try {
            this.npsReportModel = await this.serviceOperation.getNpsReport();
        } catch (e) {
            this.errorMessage = 'Failed to get NPS Report Data!';
        }

        this.seriesbarNpsReport = [
            {
                name: this.namebarNPS,
                data: [
                    this.npsReportModel.promotor,
                    this.npsReportModel.passive,
                    this.npsReportModel.detractors
                ]
            }
        ];

        this.isBusy = false;
        this.isLoadingNpsReport = false;
    }

    //fetch My Insight
    async fetchMyInsight() {
        this.isBusy = true;
        this.isLoadingMyInsightStatus = true;

        try {
            this.myInsightList = await this.serviceOperation.getMyInsight();
        } catch (e) {
            this.errorMessage = 'Failed to get My Insight Data!';
        }

        this.seriesbarMyInsight = [
            {
                name: this.namebarMyInsight,
                data: [
                    this.myInsightList.last2Month.totalInsight,
                    this.myInsightList.lastMonth.totalInsight,
                    this.myInsightList.thisMonth.totalInsight
                ]
            }
        ];
        this.barInsight = {
            labels: [
                this.myInsightList.last2Month.getMonth,
                this.myInsightList.lastMonth.getMonth,
                this.myInsightList.thisMonth.getMonth
            ]
        };

        this.isBusy = false;
        this.isLoadingMyInsightStatus = false;
    }

    //---------------FUNCTION/METHOD--------------
}
</script>
