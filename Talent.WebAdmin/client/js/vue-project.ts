import Vue from 'vue';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
import VeeValidate from 'vee-validate';
import { Settings } from 'luxon'
import { setupCalendar, vCalendar, DatePicker } from 'v-calendar';
import VCalendar from 'v-calendar'
import Draggable from 'vuedraggable';
import VueApexCharts from 'vue-apexcharts';
import Multiselect from 'vue-multiselect';
import money from 'v-money'
import wysiwyg from "vue-wysiwyg";

import Navbar from './components/Layout/Navbar.vue';
import SsoLogout from './components/Layout/SsoLogout.vue';
import Coach from './components/UserManagement/Coach.vue';
import Course from './components/MasterData/Course.vue';
import DigitalSignature from './components/MasterData/DigitalSignature.vue';
import Module from './components/MasterData/Module.vue';
import Pagination from './components/shared/_pagination.vue';
import Checkbox from 'vue-material-checkbox';
import CompetencyMapping2 from './components/MasterData/CompetencyMapping.vue';
import FileUploadMinio from './components/FileUploadMinio.vue';
import UploadFile from './components/shared/_fileupload.vue';
import News from './components/MasterData/News.vue';
import NewsForm from './components/MasterData/News/NewsFormPage.vue';
import positionMap from './components/MasterData/Position/Positions.vue';
import positionMapForm from './components/MasterData/Position/PositionMapForm.vue';
import ManageTrainingList from './components/Setup/ManageTrainingList.vue';
import ManageTrainingProcess from './components/Setup/ManageTrainingProcess.vue';

import Hello from './components/Hello.vue';

import CompetencyMapping from './components/MasterData/CompetencyMapping.vue';
import KeyAction from './components/MasterData/KeyAction.vue';
import Competency from './components/MasterData/Competency.vue';
import Reward from './components/MasterData/Reward.vue';
import Guide from './components/CMS/Guide.vue';
import Event from './components/MasterData/Event.vue';
import EventCategory from './components/MasterData/EventCategory.vue';


import SetupTimePoint from './components/Setup/SetupTimePoint.vue';
import SetupLearning from './components/Setup/SetupLearning.vue';
import ApprovalMapping from './components/Setup/ApprovalMapping.vue';
import ApprovalContent from './components/Setup/ApprovalContent.vue';
import ApprovalTraining from './components/Setup/ApprovalTraining.vue';
import TrainingProcess from './components/Setup/TrainingProcess.vue';

import SetupCourse from './components/Setup/SetupCourse.vue';
import SetupPopUpQuiz from './components/Setup/SetupPopQuiz.vue';
import SetupModule from './components/Setup//SetupModule.vue';
import FileUpload from './components/Setup/Task/FileUpload.vue';
import TrueFalse from './components/Setup/Task/TrueFalse.vue';
import Hotspot from './components/Setup/Task/Hotspot.vue';
import TebakGambar from './components/Setup/Task/TebakGambar.vue';
import Matching from './components/Setup/Task/Matching.vue';
import Sequence from './components/Setup/Task/Sequence.vue';
import ShortAnswer from './components/Setup/Task/ShortAnswer.vue';
//import Essay from './components/Setup/Task/Essay.vue';
import Checklist from './components/Setup/Task/Checklist.vue';
import Rating from './components/Setup/Task/Rating.vue';
import MultipleChoice from './components/Setup/Task/MultipleChoice.vue';
//import FileUpload from './components/Task/Setup/FileUpload.vue';
import VueTimepicker from 'vue2-timepicker'

//import TalentMaster from './components/TalentMaster.vue';
import Task from './components/Setup/Task.vue';
//import Banner from './components/MasterData/Banner.vue';
//import LoadingOverlay from './components/shared/LoadingOverlay.vue';
import { DateTime } from 'luxon'
import Hobby from './components/MasterData/Hobby.vue';
import UserRole from './components/UserManagement/UserRole.vue';
import Assessment from './components/MasterData/Assessment.vue';
import Calendars from './components/Report/Calendar.vue';
import Reports from './components/Report/Reports.vue';
import TrainingScoreReport from './components/Report/TrainingScoreReport.vue';
import ReportNPS from './components/Report/ReportNPS.vue';
import UserPrivilegeSettings from './components/UserManagement/UserPrivilegeSettings.vue';

import SetupTop5Course from './components/Setup/SetupTop5Course.vue';
import SetupTSL from './components/Setup/SetupTSL.vue';
import { faInbox } from '@fortawesome/free-solid-svg-icons';
import Inbox from './components/Report/Inbox.vue';
import Survey from './components/MasterData/Survey.vue';
import FullCalendar from '@fullcalendar/vue';

import Operation from './components/Dashboard/Operation.vue';
import ReleaseTrainings from './components/Setup/ReleaseTrainings.vue';
import Banners from './components/MasterData/Banners.vue';
import LoadingOverlays from './components/shared/LoadingOverlays.vue';
import Topics from './components/MasterData/Topics.vue';
import Matrixs from './components/Setup/Task/Matrixs.vue';
import Management from './components/Dashboard/Management.vue';
import SurveyReport from './components/Report/SurveyReport.vue';
import vueautonumeric from 'vue-autonumeric';

import EssayNew from './components/Setup/Task/EssayNew.vue';

import Home from './components/Home/Home.vue'
import ComingSoon from './components/ComingSoon/ComingSoon.vue'

//Dashboard
import GuestAccount from './views/Dashboard/GuestAccount.vue'
// CMS
import LandingPage from './views/CMS/LandingPage.vue'
import Announcement from './views/CMS/Announcement.vue'
// Push Notif
import PushNotification from './views/PushNotification.vue'
import ModalTutorials from './components/shared/ModalTutorials.vue'
// PAGE MY TOOLS
import KeyPeaceOfMind from './views/MyTools/KeyPeaceOfMind.vue'
import AddKeyPeaceOfMind from './views/MyTools/AddKeyPeaceOfMind.vue'
import ServiceTips from './views/MyTools/ServiceTips.vue'
import Dictionary from './views/MyTools/Dictionary/Dictionary.vue'
import AddDictionary from './views/MyTools/Dictionary/AddDictionary.vue'
import ProductInformation from './views/MyTools/ProductInformation/ProductInformation.vue'
import ProductInformationDetail from './views/MyTools/ProductInformation/ProductInformationDetail.vue'
import Kodawari from './views/MyTools/Kodawari/Kodawari.vue'
import UploadGuideline from './views/MyTools/UploadGuideline.vue'
import AreaSpecialist from './views//AreaSpecialist.vue'
import AreaSpecialistForm from './components/MasterData/AreaSpecialistForm.vue'

import $ from 'jquery'

import Verte from 'verte';
// register component globally
Vue.component('verte', Verte);

// Assesment
import SkillCheck from './views/SkillCheck.vue'

// MyLearning Report
import CompetencyTracking from './views/MyLearningReport/CompetencyTracking.vue'
import ProgressTracking from './views/MyLearningReport/ProgressTracking.vue'
import KpiTracking from './views/MyLearningReport/KpiTracking.vue'


Vue.use(wysiwyg, {
  // limit content height if you wish. If not set, editor size will grow with content.
  maxHeight: "500px",

  // set to 'true' this will insert plain text without styling when you paste something into the editor.
    forcePlainTextOnPaste: true,
  hideModules: {image: true, table: true, removeFormat: true, code: true }
});
Vue.use(money);
Vue.use(VCalendar);
Vue.filter('dateFormat', function (value: string) {
    if (value === "") {
        value = DateTime.local().toISO();
    }
    return DateTime.fromISO(value).toFormat("dd/LL/yyyy");
})

Vue.filter('dateFormatWithTime', function (value: string) {
    if (value === "") {
        value = DateTime.local().toISO();
    }
    return DateTime.fromISO(value).toFormat("dd/LL/yyyy hh:mm");
})

Vue.use(VeeValidate, {
    event: ''
});

VeeValidate.Validator.extend('required-multiselect', {
    validate(value: object) {
        if (Object.keys(value).length === 0) {
            return false;
        }
        return true;
    },
    getMessage(field) {
        return 'The ' + field + ' field is required.'
    }
});

Vue.component('FullCalendar', FullCalendar);
Vue.component('fa', FontAwesomeIcon);

// components must be registered BEFORE the app root declaration

//REUSEABLE
Vue.component('multiselect', Multiselect);
Vue.component('v-date-picker', DatePicker);
Vue.component('checkbox', Checkbox);
Vue.component('paginate', Pagination);
Vue.component('upload-file', UploadFile);
Vue.component('file-upload-minio', FileUploadMinio);
Vue.component('draggable', Draggable);
Vue.component('vueapexcharts', VueApexCharts);
Vue.component('vue-autonumeric', vueautonumeric);

//REUSEABLE
Vue.component('vue-timepicker', VueTimepicker);

// Coming Soon
Vue.component('coming-soon', ComingSoon);

//layout
Vue.component('talentnavbar', Navbar);
Vue.component('sso-logout', SsoLogout);
//Vue.component('talentmaster', TalentMaster);

//DASHBOARD
Vue.component('home', Home);
//Vue.component('chart', Chart);
Vue.component('dashboard-operation', Operation);
Vue.component('dashboard-management', Management);

//REPORT
//Vue.component('evaluationreport', EvaluationReport);
//Vue.component('engagementreport', EngagementReport);
//Vue.component('learningreport', LearningReport);
Vue.component('inbox', Inbox);
//Vue.component('attendancereport', AttendanceReport);
Vue.component('calendar', Calendars);
Vue.component('reports', Reports);
Vue.component('survey-report', SurveyReport);
Vue.component('training-score-report', TrainingScoreReport);
Vue.component('reports',Reports);
Vue.component('report-nps',ReportNPS);

//USER MANAGEMENT
Vue.component('user-privilege-settings', UserPrivilegeSettings);
Vue.component('user-role', UserRole);
Vue.component('coach', Coach);
//Vue.component('positionmapping', PositionMapping);

//SETUP
Vue.component('setup-learning', SetupLearning);
Vue.component('approval-content', ApprovalContent);
Vue.component('approvaltraining', ApprovalTraining);
Vue.component('approvalmapping', ApprovalMapping);
Vue.component('releasetrainings', ReleaseTrainings);
Vue.component('setupmodule', SetupModule);
Vue.component('setup-pop-quiz', SetupPopUpQuiz);
Vue.component('setup-course', SetupCourse);
Vue.component('setup-time-point', SetupTimePoint)
Vue.component('task', Task);
Vue.component('setup-top-5-course', SetupTop5Course);
Vue.component('setup-tsl', SetupTSL);
Vue.component('training-process', TrainingProcess);

//MASTER MODULE
Vue.component('assessment', Assessment);
Vue.component('course', Course);
Vue.component('module', Module);
Vue.component('topic', Topics);
Vue.component('competency', Competency);
Vue.component('keyaction', KeyAction);
Vue.component('competencymapping', CompetencyMapping2);
Vue.component('hobby', Hobby);
Vue.component('event', Event);
Vue.component('eventcategory', EventCategory);
//Vue.component('banner', Banner);
Vue.component('banner', Banners);
Vue.component('guide', Guide);

Vue.component('news', News);
Vue.component('news-form', NewsForm);
Vue.component('position-mapping', positionMap);
Vue.component('position-mapping-form', positionMapForm);
//Vue.component('news', News);
Vue.component('reward', Reward);
Vue.component('survey', Survey);
Vue.component('digital-signature', DigitalSignature);

//Task
Vue.component('true-false', TrueFalse);
Vue.component('matching', Matching);
Vue.component('sequence', Sequence);
Vue.component('tebak-gambar', TebakGambar);
Vue.component('hotspot', Hotspot);
Vue.component('short-answer', ShortAnswer);
//Vue.component('essay', Essay);
Vue.component('checklist', Checklist);
Vue.component('rating', Rating);
Vue.component('multiple-choice', MultipleChoice);
Vue.component('file-upload', FileUpload);
Vue.component('matrix', Matrixs);


Vue.component('essay-new', EssayNew);


// PAGE Dashboard
Vue.component('guest-account', GuestAccount);
// PAGE CMS
Vue.component('landing-page', LandingPage);
Vue.component('announcement', Announcement);
// PAGE Push Notification
Vue.component('push-notification', PushNotification);


//loading overlay
Vue.component('loading-overlay', LoadingOverlays);


Vue.component('modaltutorials', ModalTutorials);

// PAGE : MY TOOLS
Vue.component('key-peace-of-mind', KeyPeaceOfMind);
Vue.component('add-key-peace-of-mind', AddKeyPeaceOfMind);
Vue.component('dictionary', Dictionary);
Vue.component('add-dictionary', AddDictionary);
Vue.component('product-information', ProductInformation);
Vue.component('product-information-detail', ProductInformationDetail);
Vue.component('kodawari', Kodawari);
Vue.component('service-tips', ServiceTips);
Vue.component('upload-guideline', UploadGuideline);

Vue.component('skill-check', SkillCheck);

Vue.component('competency-tracking', CompetencyTracking);

Vue.component('progress-tracking', ProgressTracking);
Vue.component('kpi-tracking', KpiTracking);
Vue.component('area-specialist', AreaSpecialist);
Vue.component('area-specialist-form', AreaSpecialistForm);

// bootstrap the Vue app from the root element <div id="app"></div>
new Vue().$mount('#modaltutorial');
new Vue().$mount('#sidebar');
new Vue().$mount('#app');
new Vue().$mount('#logout');

$('.editr--toolbar .dashboard label').click(function (event) {$(event.target).focus()});