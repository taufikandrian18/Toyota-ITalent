<template>
    <div id="accordion" class="container-sidebar">
        <!-- Home -->
        <div class="row m-0 pt-3">
            <a
                class="talent_font_white talent_font_hover_white w-100"
                :class="
                    currentRouteName === '/' || currentRouteName === '/Index'
                        ? 'active'
                        : ''
                "
                href="/"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="home"></fa>
                    </div>
                    <div class="col-10 pl-0 d-flex align-items-center">
                        <span>Home</span>
                    </div>
                </div>
            </a>
        </div>

        <!-- Dashboard -->
        <div class="row m-0">
            <a
                href="#menuDashboard"
                data-toggle="collapse"
                class="talent_font_white talent_font_hover_white w-100"
                @click="rotateIcon('my-tools')"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="chart-pie"></fa>
                    </div>
                    <div class="col pl-0 d-flex align-items-center">
                        <span class="glyphicon glyphicon-folder-close">
                            Dashboard</span
                        >
                    </div>
                    <div
                        id="collapse-icon-my-tools"
                        class="col-auto px-0 d-flex align-items-center"
                        :class="
                            currentRouteName === '/Dashboard/Management' ||
                            currentRouteName === '/Dashboard/Operation' ||
                            currentRouteName === '/Dashboard/Account' ||
                            currentRouteName === '/Dashboard/GuestAccount'
                                ? 'rotate-icon'
                                : ''
                        "
                    >
                        <fa icon="caret-left"></fa>
                    </div>
                </div>
            </a>
            <div
                id="menuDashboard"
                class="px-0 panel-collapse collapse in w-100"
                :class="
                    currentRouteName === '/Dashboard/Management' ||
                    currentRouteName === '/Dashboard/Operation' ||
                    currentRouteName === '/Dashboard/Account' ||
                    currentRouteName === '/Dashboard/GuestAccount'
                        ? 'show'
                        : ''
                "
            >
                <div class="row m-0" v-if="!roles.includes(role) && page.includes('MNG')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Dashboard/Management'
                                ? 'active'
                                : ''
                        "
                        href="/Dashboard/Management"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Management</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="!roles.includes(role) && page.includes('OPR')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Dashboard/Operation'
                                ? 'active'
                                : ''
                        "
                        href="/Dashboard/Operation"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Operation</span>
                            </div>
                        </div>
                    </a>
                </div>
                <!-- Coming Soon -->
                <!-- <div class="row m-0">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Dashboard/Account'
                                ? 'active'
                                : ''
                        "
                        href="/Dashboard/Account"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Account</span>
                            </div>
                        </div>
                    </a>
                </div> -->
                <div class="row m-0" v-if="page.includes('GUE')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Dashboard/GuestAccount'
                                ? 'active'
                                : ''
                        "
                        href="/Dashboard/GuestAccount"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Guest Account</span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>

        <!-- My Tools -->
        <div class="row m-0" v-if="!roles.includes(role)">
            <a
                href="#menuMyTools"
                data-toggle="collapse"
                class="talent_font_white talent_font_hover_white w-100"
                @click="rotateIcon('dashboard')"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="folder-open"></fa>
                    </div>
                    <div class="col pl-0 d-flex align-items-center">
                        <span class="glyphicon glyphicon-folder-close">
                            My Tools</span
                        >
                    </div>
                    <div
                        id="collapse-icon-dashboard"
                        class="col-auto px-0 d-flex align-items-center"
                        :class="
                            currentRouteName === '/MyTools/ProductInformation' ||
                            currentRouteName === '/MyTools/KeyPeaceOfMind' ||
                            currentRouteName === '/MyTools/Kodawari' ||
                            currentRouteName === '/MyTools/Dictionary' ||
                            currentRouteName === '/MyTools/ServiceTips'
                                ? 'rotate-icon'
                                : ''
                        "
                    >
                        <fa icon="caret-left"></fa>
                    </div>
                </div>
            </a>
            <div
                id="menuMyTools"
                class="px-0 panel-collapse collapse in w-100"
                :class="
                    currentRouteName === '/MyTools/ProductInformation' ||
                    currentRouteName === '/MyTools/KeyPeaceOfMind' ||
                    currentRouteName === '/MyTools/Kodawari' ||
                    currentRouteName === '/MyTools/Dictionary' ||
                    currentRouteName === '/MyTools/ServiceTips'||
                    currentRouteName === '/MyTools/UploadGuideline'
                        ? 'show'
                        : ''
                "
            >
                <div class="row m-0" v-if="!roles.includes(role) && page.includes('PRODINFO')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MyTools/ProductInformation'
                                ? 'active'
                                : ''
                        "
                        href="/MyTools/ProductInformation"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Product Information</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="!roles.includes(role) && page.includes('KPOM')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MyTools/KeyPeaceOfMind'
                                ? 'active'
                                : ''
                        "
                        href="/MyTools/KeyPeaceOfMind"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Key Piece Of Mind</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('DICT')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MyTools/Dictionary'
                                ? 'active'
                                : ''
                        "
                        href="/MyTools/Dictionary"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Dictionary</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('SERVTIPS')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MyTools/ServiceTips'
                                ? 'active'
                                : ''
                        "
                        href="/MyTools/ServiceTips"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Service Tips</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('GUIDE')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName.includes('/MyTools/Kodawari')
                                ? 'active'
                                : ''
                        "
                        href="/MyTools/Kodawari?page=Banner"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Guidance</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('UPGUIDE')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MyTools/UploadGuideline'
                                ? 'active'
                                : ''
                        "
                        href="/MyTools/UploadGuideline"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Upload Guideline</span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>

        <!-- Report -->
        <div class="row m-0" v-if="page.includes('RPT')">
            <a
                class="talent_font_white talent_font_hover_white w-100"
                :class="currentRouteName === '/Report/Reports' ? 'active' : ''"
                href="/Report/Reports"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="chart-bar"></fa>
                    </div>
                    <div class="col-10 pl-0 d-flex align-items-center">
                        <span>Report</span>
                    </div>
                </div>
            </a>
        </div>

        <!-- Access Control -->
        <div class="row m-0" v-if="!roles.includes(role)">
            <a
                href="#menuAccessControl"
                data-toggle="collapse"
                class="talent_font_white talent_font_hover_white w-100"
                @click="rotateIcon('access-control')"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="unlock"></fa>
                    </div>
                    <div class="col pl-0 d-flex align-items-center">
                        <span class="glyphicon glyphicon-th">
                            Access Control</span
                        >
                    </div>
                    <div
                        id="collapse-icon-access-control"
                        class="col-auto px-0 d-flex align-items-center"
                        :class="
                            currentRouteName === '/UserManagement/UserRole' ||
                            currentRouteName ===
                                '/UserManagement/UserPrivilegeSettings' ||
                            currentRouteName === '/UserManagement/Coach' ||
                            currentRouteName === '/MasterData/PositionMapping'
                                ? 'rotate-icon'
                                : ''
                        "
                    >
                        <fa icon="caret-left"></fa>
                    </div>
                </div>
            </a>
            <div
                id="menuAccessControl"
                class="panel-collapse collapse px-0 w-100"
                :class="
                    currentRouteName === '/UserManagement/UserRole' ||
                    currentRouteName ===
                        '/UserManagement/UserPrivilegeSettings' ||
                    currentRouteName === '/UserManagement/Coach' ||
                    currentRouteName === '/MasterData/PositionMapping'||
                    currentRouteName === '/MasterData/AreaSpecialist'
                        ? 'show'
                        : ''
                "
            >
                <div class="row m-0" v-if="page.includes('UR')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/UserManagement/UserRole'
                                ? 'active'
                                : ''
                        "
                        href="/UserManagement/UserRole"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>User Role</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('UPS')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName ===
                            '/UserManagement/UserPrivilegeSettings'
                                ? 'active'
                                : ''
                        "
                        href="/UserManagement/UserPrivilegeSettings"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>User Privilege Settings</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('CCH')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/UserManagement/Coach'
                                ? 'active'
                                : ''
                        "
                        href="/UserManagement/Coach"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Coach</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('PM')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/PositionMapping'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/PositionMapping"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Position Mapping</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('PM')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/AreaSpecialist'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/AreaSpecialist"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Area Specialist</span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>

        <!-- My Learning -->
        <div class="row m-0" v-if="!roles.includes(role)">
            <a
                href="#menuMyLearning"
                data-toggle="collapse"
                class="talent_font_white talent_font_hover_white w-100"
                @click="rotateIcon('my-learning')"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="book"></fa>
                    </div>
                    <div class="col pl-0 d-flex align-items-center">
                        <span class="glyphicon glyphicon-folder-close">
                            My Learning</span
                        >
                    </div>
                    <div
                        id="collapse-icon-my-learning"
                        class="col-auto px-0 d-flex align-items-center"
                        :class="
                            currentRouteName === '/MyLearningReport/CompetencyTracking' ||
                            currentRouteName === '/MyLearningReport/ProgressTracking' ||
                            currentRouteName === '/MyLearningReport/KPITracking' ||
                            currentRouteName === '/MasterData/Course' ||
                            currentRouteName === '/MasterData/Module' ||
                            currentRouteName === '/MasterData/Topic' ||
                            currentRouteName === '/MasterData/Competency' ||
                            currentRouteName === '/MasterData/KeyAction' ||
                            currentRouteName ===
                                '/MasterData/CompetencyMapping' ||
                            currentRouteName === '/Setup/SetupLearning' ||
                            currentRouteName === '/Setup/Tasks' ||
                            currentRouteName === '/Setup/ReleaseTraining' ||
                            currentRouteName === '/Setup/ApprovalContent' ||
                            currentRouteName === '/Setup/ApprovalMapping' ||
                            currentRouteName === '/Setup/SetupTop5Course' ||
                            currentRouteName === '/Setup/SetupTimePoint' ||
                            currentRouteName === '/Setup/SetupTSL' ||
                            currentRouteName === '/Setup/ApprovalTraining' ||
                            currentRouteName === '/Setup/TrainingProcess' ||
                            currentRouteName === '/Setup/SkillCheck'
                                ? 'rotate-icon'
                                : ''
                        "
                    >
                        <fa icon="caret-left"></fa>
                    </div>
                </div>
            </a>
            <div
                id="menuMyLearning"
                class="px-0 panel-collapse collapse in w-100"
                :class="
                    currentRouteName === '/MyLearningReport/CompetencyTracking' ||
                    currentRouteName === '/MyLearningReport/ProgressTracking' ||
                    currentRouteName === '/MyLearningReport/KPITracking' ||
                    currentRouteName === '/MasterData/Course' ||
                    currentRouteName === '/MasterData/Module' ||
                    currentRouteName === '/MasterData/Topic' ||
                    currentRouteName === '/MasterData/Competency' ||
                    currentRouteName === '/MasterData/KeyAction' ||
                    currentRouteName === '/MasterData/CompetencyMapping' ||
                    currentRouteName === '/Setup/SetupLearning' ||
                    currentRouteName === '/Setup/Tasks' ||
                    currentRouteName === '/Setup/ReleaseTraining' ||
                    currentRouteName === '/Setup/ApprovalContent' ||
                    currentRouteName === '/Setup/ApprovalMapping' ||
                    currentRouteName === '/Setup/SetupTop5Course' ||
                    currentRouteName === '/Setup/SetupTimePoint' ||
                    currentRouteName === '/Setup/SetupTSL' ||
                    currentRouteName === '/Setup/ApprovalTraining' ||
                    currentRouteName === '/Setup/SkillCheck' ||
                    currentRouteName === '/Setup/TrainingProcess'
                        ? 'show'
                        : ''
                "
            >
                <!-- Master Data -->
                <div class="row m-0">
                    <a
                        href="#menuMasterData"
                        data-toggle="collapse"
                        class="talent_font_white talent_font_hover_white w-100"
                        @click="rotateIcon('master-data')"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col pl-0 d-flex align-items-center">
                                <span class="glyphicon glyphicon-folder-close">
                                    Master Data</span
                                >
                            </div>
                            <div
                                id="collapse-icon-master-data"
                                class="col-auto px-0 d-flex align-items-center"
                                :class="
                                    currentRouteName === '/MasterData/Course' ||
                                    currentRouteName === '/MasterData/Module' ||
                                    currentRouteName === '/MasterData/Topic' ||
                                    currentRouteName ===
                                        '/MasterData/Competency' ||
                                    currentRouteName ===
                                        '/MasterData/KeyAction' ||
                                    currentRouteName ===
                                        '/Setup/Tasks' ||
                                    currentRouteName ===
                                        '/Setup/SkillCheck' ||
                                    currentRouteName ===
                                        '/MasterData/CompetencyMapping'
                                        ? 'rotate-icon'
                                        : ''
                                "
                            >
                                <fa icon="caret-left"></fa>
                            </div>
                        </div>
                    </a>
                    <div
                        id="menuMasterData"
                        class="px-0 panel-collapse panel-collapse-deep collapse in w-100"
                        :class="
                            currentRouteName === '/MasterData/Course' ||
                            currentRouteName === '/MasterData/Module' ||
                            currentRouteName === '/MasterData/Topic' ||
                            currentRouteName === '/MasterData/Competency' ||
                            currentRouteName === '/MasterData/KeyAction' ||
                            currentRouteName ===
                                '/Setup/Tasks' ||
                            currentRouteName ===
                                '/Setup/SkillCheck' ||
                            currentRouteName === '/MasterData/CompetencyMapping'
                                ? 'show'
                                : ''
                        "
                    >
                        <div class="row m-0" v-if="page.includes('CRS')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName === '/MasterData/Course'
                                        ? 'active'
                                        : ''
                                "
                                href="/MasterData/Course"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Course</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="row m-0" v-if="page.includes('MDL')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName === '/MasterData/Module'
                                        ? 'active'
                                        : ''
                                "
                                href="/MasterData/Module"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Module</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="row m-0" v-if="page.includes('TPC')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName === '/MasterData/Topic'
                                        ? 'active'
                                        : ''
                                "
                                href="/MasterData/Topic"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Topic</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="row m-0" v-if="page.includes('COMP')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName ===
                                    '/MasterData/Competency'
                                        ? 'active'
                                        : ''
                                "
                                href="/MasterData/Competency"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Competency</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="row m-0" v-if="page.includes('KA')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName === '/MasterData/KeyAction'
                                        ? 'active'
                                        : ''
                                "
                                href="/MasterData/KeyAction"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Key Action</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="row m-0" v-if="page.includes('COMPM')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName ===
                                    '/MasterData/CompetencyMapping'
                                        ? 'active'
                                        : ''
                                "
                                href="/MasterData/CompetencyMapping"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Competency Mapping</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="row m-0" v-if="page.includes('COMPM')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName === '/Setup/Tasks' ? 'active' : ''
                                "
                                href="/Setup/Tasks"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Task</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="row m-0" v-if="page.includes('SKILLCHK')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName === '/Setup/SkillCheck'
                                        ? 'active'
                                        : ''
                                "
                                href="/Setup/SkillCheck"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Skill Check</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                    </div>
                </div>
                <!-- Report -->
                <div class="row m-0">
                    <a
                        href="#menuMyLearningReport"
                        data-toggle="collapse"
                        class="talent_font_white talent_font_hover_white w-100"
                        @click="rotateIcon('my-learning-report')"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col pl-0 d-flex align-items-center">
                                <span class="glyphicon glyphicon-folder-close">
                                    Report</span
                                >
                            </div>
                            <div
                                id="collapse-icon-my-learning-report"
                                class="col-auto px-0 d-flex align-items-center"
                                :class="
                                    currentRouteName === '/MyLearningReport/ProgressTracking' ||
                    currentRouteName === '/MyLearningReport/KPITracking' ||
                                    currentRouteName ===
                                        '/ReportMyLearning/CompetencyTracking'
                                        ? 'rotate-icon'
                                        : ''
                                "
                            >
                                <fa icon="caret-left"></fa>
                            </div>
                        </div>
                    </a>
                    <div
                        id="menuMyLearningReport"
                        class="px-0 panel-collapse panel-collapse-deep collapse in w-100"
                        :class="
                            currentRouteName === '/MyLearningReport/ProgressTracking' ||
                    currentRouteName === '/MyLearningReport/KPITracking' ||
                            currentRouteName === '/MyLearningReport/CompetencyTracking'
                                ? 'show'
                                : ''
                        "
                    >
                        <div class="row m-0" v-if="page.includes('LRNPROG')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName === '/MyLearningReport/CompetencyTracking'
                                        ? 'active'
                                        : ''
                                "
                                href="/MyLearningReport/CompetencyTracking"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Learning Progress</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="row m-0" v-if="page.includes('LRNANYS')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName === '/MyLearningReport/ProgressTracking'
                                        ? 'active'
                                        : ''
                                "
                                href="/MyLearningReport/ProgressTracking"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Learning Analysis</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                        <div class="row m-0" v-if="page.includes('LRNKPI')">
                            <a
                                class="talent_font_white talent_font_hover_white w-100"
                                :class="
                                    currentRouteName === '/MyLearningReport/KPITracking'
                                        ? 'active'
                                        : ''
                                "
                                href="/MyLearningReport/KPITracking"
                            >
                                <div class="row py-2 mx-0 px-3">
                                    <div
                                        class="col-2 px-0 d-flex align-items-center"
                                    ></div>
                                    <div
                                        class="col-10 pl-0 d-flex align-items-center"
                                    >
                                        <span>Learning KPI</span>
                                    </div>
                                </div>
                            </a>
                        </div>
                    </div>
                </div>
                <!-- Coming Soon -->
                <!-- <div class="row m-0">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        href="/"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Billing Code</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        href="/"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Dormitory</span>
                            </div>
                        </div>
                    </a>
                </div> -->
                <!-- <div class="row m-0" v-if="page.includes('STL')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/SkillCheck'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/SkillCheck"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Skill Check</span>
                            </div>
                        </div>
                    </a>
                </div> -->
                <div class="row m-0" v-if="page.includes('STL')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/SetupLearning'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/SetupLearning"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Setup Learning</span>
                            </div>
                        </div>
                    </a>
                </div>
                <!-- <div class="row m-0" v-if="page.includes('TSK')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/Tasks' ? 'active' : ''
                        "
                        href="/Setup/Tasks"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Task</span>
                            </div>
                        </div>
                    </a>
                </div> -->
                <div class="row m-0" v-if="page.includes('RTG')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/ReleaseTraining'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/ReleaseTraining"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Release Training</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('AC')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/ApprovalContent'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/ApprovalContent"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Approval Content</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('AM')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/ApprovalMapping'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/ApprovalMapping"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Approval Mapping</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('STC')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/SetupTop5Course'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/SetupTop5Course"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Setup Top 5 Course</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('STPT')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/SetupTimePoint'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/SetupTimePoint"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Setup Time Point</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('STSL')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/SetupTSL'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/SetupTSL"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Setup TSL</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('AT')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/ApprovalTraining'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/ApprovalTraining"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Approval Training</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('TP')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/Setup/TrainingProcess'
                                ? 'active'
                                : ''
                        "
                        href="/Setup/TrainingProcess"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Training Process</span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>

        <!-- My Complaint -->
        <!--<div class="row m-0">
            <a
                data-toggle="collapse"
                data-parent="#accordion"
                href="#menuMyComplaint"
                class="talent_font_white talent_font_hover_white w-100"
                @click="rotateIcon('my-complaint')"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="envelope"></fa>
                    </div>
                    <div class="col pl-0 d-flex align-items-center">
                        <span class="glyphicon glyphicon-th">
                            My Complaint</span
                        >
                    </div>
                    <div
                        id="collapse-icon-my-complaint"
                        class="col-auto px-0 d-flex align-items-center"
                        :class="
                            currentRouteName === '/MyComplaint/Complaint' ||
                            currentRouteName === '/MyComplaint/Category'
                                ? 'rotate-icon'
                                : ''
                        "
                    >
                        <fa icon="caret-left"></fa>
                    </div>
                </div>
            </a>
            <div
                id="menuMyComplaint"
                class="panel-collapse collapse px-0 w-100"
                :class="
                    currentRouteName === '/MyComplaint/Complaint' ||
                    currentRouteName === '/MyComplaint/Category'
                        ? 'show'
                        : ''
                "
            >
                <div class="row m-0">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        href="/"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Complaint</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        href="/"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Category</span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>-->

        <!-- Event -->
        <div class="row m-0" v-if="!roles.includes(role)">
            <a
                data-toggle="collapse"
                data-parent="#accordion"
                href="#menuEvent"
                class="talent_font_white talent_font_hover_white w-100"
                @click="rotateIcon('event')"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="calendar"></fa>
                    </div>
                    <div class="col pl-0 d-flex align-items-center">
                        <span class="glyphicon glyphicon-th"> My Event</span>
                    </div>
                    <div
                        id="collapse-icon-event"
                        class="col-auto px-0 d-flex align-items-center"
                        :class="
                            currentRouteName === '/MasterData/Event' ||
                            currentRouteName === '/MasterData/EventCategory'
                                ? 'rotate-icon'
                                : ''
                        "
                    >
                        <fa icon="caret-left"></fa>
                    </div>
                </div>
            </a>
            <div
                id="menuEvent"
                class="panel-collapse collapse px-0 w-100"
                :class="
                    currentRouteName === '/MasterData/Event' ||
                    currentRouteName === '/MasterData/EventCategory'
                        ? 'show'
                        : ''
                "
            >
                <div class="row m-0" v-if="page.includes('EVN')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/Event'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/Event"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Event</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('EC')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/EventCategory'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/EventCategory"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Category</span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>

        <!-- CMS -->
        <div class="row m-0" v-if="!roles.includes(role)">
            <a
                data-toggle="collapse"
                data-parent="#accordion"
                href="#menuCMS"
                class="talent_font_white talent_font_hover_white w-100"
                @click="rotateIcon('cms')"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="database"></fa>
                    </div>
                    <div class="col pl-0 d-flex align-items-center">
                        <span class="glyphicon glyphicon-th"> CMS</span>
                    </div>
                    <div
                        id="collapse-icon-cms"
                        class="col-auto px-0 d-flex align-items-center"
                        :class="
                            currentRouteName === '/CMS/LandingPage' ||
                            currentRouteName === '/CMS/Announcement' ||
                            currentRouteName === '/CMS/Guide' ||
                            currentRouteName === '/MasterData/Hobby' ||
                            currentRouteName === '/MasterData/Banner' ||
                            currentRouteName ===
                                '/MasterData/DigitalSignature' ||
                            currentRouteName === '/MasterData/Topic'
                                ? 'rotate-icon'
                                : ''
                        "
                    >
                        <fa icon="caret-left"></fa>
                    </div>
                </div>
            </a>
            <div
                id="menuCMS"
                class="panel-collapse collapse px-0 w-100"
                :class="
                    currentRouteName === '/CMS/LandingPage' ||
                    currentRouteName === '/CMS/Grouping' ||
                    currentRouteName === '/CMS/SplashScreen' ||
                    currentRouteName === '/CMS/Announcement' ||
                    currentRouteName === '/CMS/Guide' ||
                    currentRouteName === '/MasterData/Hobby' ||
                    currentRouteName === '/MasterData/Banner' ||
                    currentRouteName === '/MasterData/DigitalSignature' ||
                    currentRouteName === '/MasterData/Topic'
                        ? 'show'
                        : ''
                "
            >
                <div class="row m-0" v-if="page.includes('LAND')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/CMS/LandingPage'
                                ? 'active'
                                : ''
                        "
                        href="/CMS/LandingPage"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Landing Page</span>
                            </div>
                        </div>
                    </a>
                </div>
                <!-- Coming Soon -->
                <!-- <div class="row m-0">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/CMS/Grouping' ? 'active' : ''
                        "
                        href="/CMS/Grouping"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Grouping</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/CMS/SplashScreen'
                                ? 'active'
                                : ''
                        "
                        href="/CMS/SplashScreen"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Splash Screen</span>
                            </div>
                        </div>
                    </a>
                </div> -->
                <div class="row m-0" v-if="page.includes('ANNO')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/CMS/Announcement'
                                ? 'active'
                                : ''
                        "
                        href="/CMS/Announcement"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Announcement</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('GDE')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/CMS/Guide' ? 'active' : ''
                        "
                        href="/CMS/Guide"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Guide</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('HB')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/Hobby'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/Hobby"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Hobby</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('BNR')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/Banner'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/Banner"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Banner</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('DS')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/DigitalSignature'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/DigitalSignature"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Digital Signature</span>
                            </div>
                        </div>
                    </a>
                </div>
                <!-- <div class="row m-0" v-if="page.includes('TPC')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/Topic'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/Topic"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Topic</span>
                            </div>
                        </div>
                    </a>
                </div> -->
            </div>
        </div>

        <!-- Survey and Assessment -->
        <div class="row m-0" v-if="!roles.includes(role)">
            <a
                data-toggle="collapse"
                data-parent="#accordion"
                href="#menuSurveyAndAssessment"
                class="talent_font_white talent_font_hover_white w-100"
                @click="rotateIcon('survey-and-assessment')"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="gift"></fa>
                    </div>
                    <div class="col pl-0 d-flex align-items-center">
                        <span class="glyphicon glyphicon-th">
                            Survey & Assessment</span
                        >
                    </div>
                    <div
                        id="collapse-icon-survey-and-assessment"
                        class="col-auto px-0 d-flex align-items-center"
                        :class="
                            currentRouteName === '/MasterData/Survey' ||
                            currentRouteName === '/MasterData/Assessment'
                                ? 'rotate-icon'
                                : ''
                        "
                    >
                        <fa icon="caret-left"></fa>
                    </div>
                </div>
            </a>
            <div
                id="menuSurveyAndAssessment"
                class="panel-collapse collapse px-0 w-100"
                :class="
                    currentRouteName === '/MasterData/Survey' ||
                    currentRouteName === '/MasterData/Assessment'
                        ? 'show'
                        : ''
                "
            >
                <div class="row m-0" v-if="page.includes('SVY')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/Survey'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/Survey"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Survey</span>
                            </div>
                        </div>
                    </a>
                </div>
                <div class="row m-0" v-if="page.includes('ASM')">
                    <a
                        class="talent_font_white talent_font_hover_white w-100"
                        :class="
                            currentRouteName === '/MasterData/Assessment'
                                ? 'active'
                                : ''
                        "
                        href="/MasterData/Assessment"
                    >
                        <div class="row py-2 mx-0 px-3">
                            <div
                                class="col-2 px-0 d-flex align-items-center"
                            ></div>
                            <div class="col-10 pl-0 d-flex align-items-center">
                                <span>Assessment</span>
                            </div>
                        </div>
                    </a>
                </div>
            </div>
        </div>

        <!-- My News -->
        <div class="row m-0" v-if="!roles.includes(role) && page.includes('NWS')">
            <a
                class="talent_font_white talent_font_hover_white w-100"
                :class="currentRouteName === '/MasterData/News' ? 'active' : ''"
                href="/MasterData/News"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="newspaper"></fa>
                    </div>
                    <div class="col-10 pl-0 d-flex align-items-center">
                        <span>My News</span>
                    </div>
                </div>
            </a>
        </div>

        <!-- Reward -->
        <div class="row m-0" v-if="!roles.includes(role) && page.includes('RWRD')">
            <a
                class="talent_font_white talent_font_hover_white w-100"
                :class="
                    currentRouteName === '/MasterData/Reward' ? 'active' : ''
                "
                href="/MasterData/Reward"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="star"></fa>
                    </div>
                    <div class="col-10 pl-0 d-flex align-items-center">
                        <span>Reward</span>
                    </div>
                </div>
            </a>
        </div>

        <!-- Push Notification -->
        <div class="row m-0 pb-3" v-if="!roles.includes(role) && page.includes('PNOTF')">
            <a
                class="talent_font_white talent_font_hover_white w-100"
                :class="
                    currentRouteName === '/PushNotification' ? 'active' : ''
                "
                href="/PushNotification"
            >
                <div class="row py-2 mx-0 px-3">
                    <div class="col-2 px-0 d-flex align-items-center">
                        <fa icon="bullhorn"></fa>
                    </div>
                    <div class="col-10 pl-0 d-flex align-items-center">
                        <span>Push Notification</span>
                    </div>
                </div>
            </a>
        </div>
    </div>
</template>
<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import {
    UserPrivilegeSettingsService,
    InboxService
} from '../../services/NSwagService';

@Component({
    created: async function(this: Navbar) {
        await this.getpage();
        await this.initialize();
    },
    props: ['role']
})
export default class Navbar extends Vue {
    api: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();
    page: string[] = [];
    async getpage() {
        var data = await this.api.pageAccess();
        this.page = data;
    }

    //12/20/2019 - 10:15 tetep di show navbar menunya walau tidak ada content

    InboxService: InboxService = new InboxService();
    UnreadInbox: number = 0;

    roles: Array<string> = ['Dealer HO']

    // Computed
    get currentRouteName() {
        return window.location.pathname;
    }

    async initialize() {
        this.UnreadInbox = await this.InboxService.getUnreadInboxCount();
    }

    // Function & Method
    rotateIcon(menu: string) {
        const rotate = document.getElementById('collapse-icon-' + menu);
        const isRotate = rotate.style.transform;
        if (rotate.classList.contains('rotate-icon')) {
            rotate.style.transform = 'rotate(0deg)';
            if (isRotate === 'rotate(0deg)') {
                rotate.style.transform = 'rotate(-90deg)';
            }
        } else {
            if (isRotate === 'rotate(-90deg)') {
                rotate.style.transform = 'rotate(0deg)';
            } else {
                rotate.style.transform = 'rotate(-90deg)';
            }
        }
    }
}
</script>
