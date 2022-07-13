<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="row">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-4"><fa icon="database"></fa> Master Data > Survey > <span class="talent_font_red">View Detail Survey</span></h3>


                <!--Add-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>Survey Information</h4>

                    <div v-if="errorMessageShow == true && errors.items.length > 0" class="alert alert-danger alert-dismissible fade show" role="alert">

                        <div v-for="error in errors.all()">
                            {{error}}
                        </div>

                        <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click.prevent="ResetErrorMessage()">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>

                    <h5 class="mb-3">Participant</h5>
                    <div class="row justify-content-center">
                        <div class="col-md-12">
                            <label>Survey Title<span class="talent_font_red">*</span></label>
                            <div class="input-group">
                                <input name="title" v-model="surveyInsert.title" class="form-control" disabled />
                            </div>
                        </div>
                        <br />
                    </div>
                    <br>
                    <div class="row">
                        <div class="col my-2">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>Area<span class="talent_font_red">*</span></label>

                                    <multiselect v-model="selectedArea"
                                                 name="area"
                                                 tag-placeholder="Add Area"
                                                 placeholder="Add Area"
                                                 label="name"
                                                 track-by="areaId"
                                                 :options="listArea"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 @input="findOutlet"
                                                 @remove="checkingOutlet"
                                                 selectLabel="Press enter to select"
                                                 disabled>
                                    </multiselect>
                                    <br />
                                    <label>Dealer<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="selectedDealer"
                                                 name="dealer"
                                                 tag-placeholder="Add Dealer"
                                                 placeholder="Add Dealer"
                                                 label="dealerName"
                                                 track-by="dealerId"
                                                 :options="listDealer"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 @input="findOutlet"
                                                 selectLabel="Press enter to select"
                                                 disabled>
                                    </multiselect>
                                    <br />
                                    <label>Province<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="selectedProvince"
                                                 name="province"
                                                 tag-placeholder="Add Province"
                                                 placeholder="Add Province"
                                                 label="provinceName"
                                                 track-by="provinceId"
                                                 :options="listProvince"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 @input="findOutlet"
                                                 selectLabel="Press enter to select"
                                                 disabled>
                                    </multiselect>
                                    <br />
                                    <label>City<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="selectedCity"
                                                 name="city"
                                                 tag-placeholder="Add City"
                                                 placeholder="Add City"
                                                 label="cityName"
                                                 track-by="cityId"
                                                 :options="listCity"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 @input="findOutlet"
                                                 selectLabel="Press enter to select"
                                                 disabled>
                                    </multiselect>
                                    <br />
                                    <label>Outlet<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="surveyInsert.outlet"
                                                 name="outlet"
                                                 tag-placeholder="Add Outlet"
                                                 placeholder="Add Outlet"
                                                 label="name"
                                                 track-by="outletId"
                                                 :options="listOutlet"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 selectLabel="Press enter to select"
                                                 disabled>
                                    </multiselect>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <label>Position<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="surveyInsert.position"
                                                 name="position"
                                                 tag-placeholder="Add Position"
                                                 placeholder="Add Position"
                                                 label="positionName"
                                                 track-by="positionId"
                                                 :options="listPosition"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 :hide-selected="true"
                                                 :show-no-results="true"
                                                 selectLabel="Press enter to select"
                                                 disabled>
                                    </multiselect>
                                </div>
                            </div>
                        </div>
                        <br>
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-md-12 mb-3">
                                    <label>Period<span class="talent_font_red">*</span></label>
                                    <div class="row justify-content-between">
                                        <div class="col-md-5">
                                            <div class="input-group talent_front">
                                                <input class="form-control"
                                                       name="period start"
                                                       type="text"
                                                       disabled
                                                       :placeholder="getStringDate(surveyInsert.startDate)" />
                                                <div class="input-group-append">
                                                    <span class="input-group-text">
                                                        <fa icon="calendar-alt"></fa>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-2">
                                            <button class="button_text_no_background" disabled>
                                                <fa style="font-size:24px" icon="arrow-right"></fa>
                                            </button>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="input-group talent_front">
                                                <input class="form-control"
                                                       name="period end"
                                                       type="text"
                                                       disabled
                                                       :placeholder="getStringDate(surveyInsert.endDate)" />
                                                <div class="input-group-append">
                                                    <span class="input-group-text">
                                                        <fa icon="calendar-alt"></fa>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h3>Question</h3>
                    <br />
                    <div v-for="(survey,indexQuestion) in surveyInsert.question">
                        <div class="row justify-content-center">
                            <div class="col-md-12">
                                <div class="d-flex justify-content-between">
                                    <h5>Survey Number {{survey.questionNumber}} of {{totalSurveyInsert}}</h5>

                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="d-flex align-items-end flex-column">
                                                <div class="float-left d-inline-block">
                                                    <button class="btn talent_bg_cyan talent_roundborder talent_font_white" v-if="indexQuestion+1 == totalSurveyInsert" disabled>Add Survey</button>
                                                    <button class="btn talent_bg_red talent_roundborder talent_font_white" v-else disabled>Delete Survey</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-4">
                                    <div class="col-md-12 mb-4">
                                        <label>Survey Question<span class="talent_font_red">*</span></label>
                                        <textarea name="question" v-model="survey.question" class="form-control talent_textarea" disabled></textarea>
                                    </div>
                                    <br />
                                    <div class="col-md-12">
                                        <label>Type of Survey<span class="talent_font_red">*</span></label>
                                        <select class="form-control" v-model="survey.surveyQuestionTypeId" @change.prevent="ForceUpdate(indexQuestion, survey.surveyQuestionTypeId)" disabled>
                                            <option v-for="l in listQuestionType" :value="l.surveyQuestionTypeId">{{l.name}}</option>
                                        </select>
                                    </div>
                                </div>

                                <div class="row">
                                    <!--Matrix-->
                                    <div class="col-md-12 mb-4" v-if="survey.surveyQuestionTypeId == 12">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <table class="talent_matrix talent_nopadding">
                                                    <thead>
                                                        <tr>
                                                            <th>

                                                            </th>
                                                            <th v-for="(m, index) in survey.matrixChoice.matrixChoice">
                                                                <textarea class="form-control" disabled :placeholder="'Insert Measurement ' + (index + 1)" v-model="survey.matrixChoice.matrixChoice[index].text" maxlength="2000"></textarea>
                                                            </th>
                                                            <th class="talent_noborder " style="padding: 0px 10px">

                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr v-for="(option, indexMatrix) in  survey.matrixChoice.matrixQuestion">
                                                            <td><textarea class="form-control" disabled :placeholder="'Insert Option ' + (indexMatrix + 1)" v-model="survey.matrixChoice.matrixQuestion[indexMatrix].question"></textarea></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td class="talent_noborder" style="padding: 0px 0px 0px 10px">
                                                                <!--<a class="h-100 w-100 talent_cursorpoint text-center" v-if="indexMatrix != listMatrix[indexQuestion].option.length - 1" @click.prevent="removeMatrix(indexQuestion,indexMatrix)"><fa icon="trash-alt"></fa></a>
                                                                <a class="h-100 w-100 talent_cursorpoint text-center" v-if="indexMatrix == listMatrix[indexQuestion].option.length - 1" @click.prevent="addMatrix(indexQuestion)"><fa icon="plus-circle"></fa></a>-->
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>

                                    <!--File Upload-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 11">
                                    </div>

                                    <!--Multiple Choice-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 10">
                                        <div v-for="(choice, indexMultipleChoice) in survey.choice" class="row justify-content-center">
                                            <div class="col-md-12">
                                                <div class="row align-content-center mb-3">
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        {{++indexMultipleChoice}}
                                                    </div>
                                                    <div class="col-md-11">
                                                        <input name="multiplechoice choice" v-model="choice.choice" class="form-control" disabled />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--Rating-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 9">
                                        <div v-for="(choice, indexRating) in survey.choice" :key="indexRating" class="row justify-content-center">
                                            <div class="col-md-12">
                                                <div class="row align-content-center mb-3">
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        {{++indexRating}}
                                                    </div>
                                                    <div class="col-md-10">
                                                        <input name="rating choice" v-model="choice.choice" class="form-control" disabled />
                                                    </div>
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        <!--Remove-->
                                                        <div v-if="indexRating != surveyInsert.question[indexQuestion].choice.length" class="text-center" disabled><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                                        <!--Add-->
                                                        <div v-if="indexRating == surveyInsert.question[indexQuestion].choice.length" class="text-center" disabled><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--Checklist-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 8">
                                        <div v-for="(choice, indexChecklist) in survey.choice" class="row justify-content-center">
                                            <div class="col-md-12">
                                                <div class="row align-content-center mb-3">
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        {{++indexChecklist}}
                                                    </div>
                                                    <div class="col-md-10">
                                                        <input name="checklist choice" v-model="choice.choice" class="form-control" disabled />
                                                    </div>
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        <!--Remove-->
                                                        <div v-if="indexChecklist != surveyInsert.question[indexQuestion].choice.length" class="text-center" disabled><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                                        <!--Add-->
                                                        <div v-if="indexChecklist == surveyInsert.question[indexQuestion].choice.length" class="text-center" disabled><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--Open Questions/Essay-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 7">
                                    </div>

                                    <!--Short Answer-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 6">
                                    </div>

                                    <!--Hot Spot-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 5">
                                        <div class="justify-content-between mb-3">
                                            <div class="col-md-12">
                                                <img class="h-100 w-100" :src="QuestionImage[indexQuestion] ? QuestionImage[indexQuestion].FileUpload[0].imageData : '/upload-image2.png'" />
                                                <div class="custom-file">
                                                    <input name="Hotspot Image" disabled v-validate="'required'" type="file" class="custom-file-input" id="customFile" @change.prevent="handlerQuestion(indexQuestion, $event)" />
                                                    <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{QuestionImage[indexQuestion] ? QuestionImage[indexQuestion].FileUpload[0].fileName : 'Choose File'}}</label>
                                                </div>
                                            </div>
                                        </div>

                                        <div>
                                            <div class="col-md-12">
                                                <div class="row justify-content-between">
                                                    <div class="col-md-3">
                                                        Hotspot Total Option :
                                                    </div>
                                                    <div class="col-md-3">
                                                        <select class="form-control" v-model="selectedHotspotOption[indexQuestion]" @change="hotspotChange(indexQuestion, $event)" disabled>
                                                            <option v-for="hotspot in hotspotOptions" :value="hotspot.value">{{hotspot.text}}</option>
                                                        </select>
                                                    </div>
                                                    <div class="col-md-6">
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--Tebak Gambar-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 4">
                                        <div v-for="(image, indexTebakGambar) in AnswerImage[indexQuestion].FileUpload">
                                            <div class=" h-100">
                                                <div class="row">
                                                    <div class="col-md-1 align-self-center">
                                                        {{indexTebakGambar + 1}}
                                                    </div>
                                                    <div class="col-md-10 mb-4">
                                                        <div>
                                                            <img class="h-100 w-100" :src="image.imageData ? image.imageData : '/upload-image2.png'" />
                                                            <div class="custom-file">
                                                                <input name="tebak gambar choice" v-validate="'required'" type="file" class="custom-file-input" id="customFile" disabled />
                                                                <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{image.fileName.length ? image.fileName : 'Choose File'}}</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1 align-self-center">
                                                        <div v-if="indexTebakGambar != AnswerImage[indexQuestion].FileUpload.length - 1" class="text-center" disabled><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                                        <div v-if="indexTebakGambar == AnswerImage[indexQuestion].FileUpload.length - 1" class="text-center" disabled><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                                    </div>

                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--Sequence-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 3">
                                        <div v-for="(choice, indexSequence) in survey.choice" :key="indexSequence" class="row justify-content-center">
                                            <div class="col-md-12">
                                                <div class="row align-content-center mb-3">
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        {{++indexSequence}}
                                                    </div>
                                                    <div class="col-md-10">
                                                        <input name="checklist choice" v-model="choice.choice" class="form-control" disabled />
                                                    </div>
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        <!--Remove-->
                                                        <div v-if="indexSequence != surveyInsert.question[indexQuestion].choice.length" class="text-center" disabled><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                                        <!--Add-->
                                                        <div v-if="indexSequence == surveyInsert.question[indexQuestion].choice.length" class="text-center" disabled><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--Matching-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 2">
                                        <div v-for="(leftMatchingChoice, index) in survey.matchingChoices.leftChoices" class="" :key="index">
                                            <div class="row ">
                                                <div class="col-md-12">
                                                    <div class="row">
                                                        <div class="col-md-6 talent_borderright ">
                                                            <div class="talent_marginbottom h-100">
                                                                <div class="input-group mb-3 talent_nomargin">
                                                                    <div class="input-group-prepend ">
                                                                        <span class="input-group-text talent_noroundborder_bottom">{{index+1}}</span>
                                                                    </div>
                                                                    <select class="form-control talent_noroundborder_bottom" v-model="surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].type" :disabled=true>
                                                                        <option value="image">Image</option>
                                                                        <option value="text">Text</option>
                                                                    </select>
                                                                </div>
                                                                <div v-if="surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].type === 'image'">
                                                                    <img class="h-100 w-100" :src="(matchingImageFormData[indexQuestion] && matchingImageFormData[indexQuestion].data[index] && matchingImageFormData[indexQuestion].data[index].leftImage) ? matchingImageFormData[indexQuestion].data[index].leftImage : '/upload-image2.png'" />
                                                                    <div class="custom-file">
                                                                        <input type="file" class="custom-file-input talent_noroundborder_top" v-validate="'required'" :disabled=true name="matching left image choice" accept="image/*">
                                                                        <label class="custom-file-label talent_textshorten talent_noroundborder_top">{{surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].imageUpload? surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].imageUpload.fileName : 'Choose File'}}</label>
                                                                    </div>
                                                                </div>
                                                                <div v-if="surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].type === 'text'" class="h-100">
                                                                    <textarea class="form-control talent_textarea talent_noroundborder_top" :disabled=true v-validate="'required'" name="matching left text choice" v-model="surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].text" style="height: calc(100% - 52px)!important;" maxlength="2000"></textarea>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 ">
                                                            <div class="talent_marginbottom h-100">
                                                                <div class="input-group mb-3 talent_nomargin">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text talent_noroundborder_bottom">{{index+1}}</span>
                                                                    </div>
                                                                    <select class="form-control talent_noroundborder_bottom" v-model="surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].type" :disabled=true>
                                                                        <option value="image">Image</option>
                                                                        <option value="text">Text</option>
                                                                    </select>
                                                                </div>
                                                                <div v-if="surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].type === 'image'">
                                                                    <img class="h-100 w-100" :src="(matchingImageFormData[indexQuestion] && matchingImageFormData[indexQuestion].data[index] && matchingImageFormData[indexQuestion].data[index].rightImage) ? matchingImageFormData[indexQuestion].data[index].rightImage : '/upload-image2.png'" />
                                                                    <div class="custom-file">
                                                                        <input :disabled=true type="file" class="custom-file-input talent_noroundborder_top" v-validate="'required'" name="matching right image choice" accept="image/*">
                                                                        <label class="custom-file-label talent_textshorten talent_noroundborder_top">{{surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].imageUpload? surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].imageUpload.fileName : 'Choose File'}}</label>
                                                                    </div>
                                                                </div>
                                                                <div v-if="surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].type === 'text'" class="h-100">
                                                                    <textarea :disabled=true class="form-control talent_textarea talent_noroundborder_top" v-validate="'required'" name="matching right text choice" v-model="surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].text" style="height: calc(100% - 52px)!important;" maxlength="2000"></textarea>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--True/False-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 1">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="d-flex align-items-end flex-column">
                                <div>
                                    <button class="btn talent_bg_red talent_font_white" @click.prevent="Close()">
                                        Close
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
            </div>
            <br />
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { BlobService } from '../../services/BlobService';
    import { SurveysService, SurveyInsert, SurveyQuestion, SurveyChoice, AreaViewModel, PositionViewModel, RegionViewModel, DealerViewModel, ProvinceViewModel, CityViewModel, OutletViewModel, ReleaseTrainingService, SurveyQuestionType, SurveyInitialize, BlobService as BlobServiceNswag, UserAccessCRUD, UserPrivilegeSettingsService } from '../../services/NSwagService'

    @Component({
        props: ["edit", "type", "success-message-show", "success-message", "survey-id"],
        created: async function (this: SurveyViewDetail) {
            this.isBusy = true;
            await this.getAccess()
            await this.Initialize();
        }
    })
    export default class SurveyViewDetail extends Vue {
        isBusy: boolean = false
        isDisabled: boolean = true;
        type: string;

        //Api
        surveyAPI: SurveysService = new SurveysService();
        blobServiceNswag: BlobServiceNswag = new BlobServiceNswag();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        surveyInsert: SurveyInitialize = { surveyId: null, title: '', startDate: null, endDate: null, outlet: [], position: [], question: [] }

        get totalSurveyInsert() {
            return this.surveyInsert.question.length
        }

        ForceUpdate(index, questionTypeId: number) {
            this.surveyInsert.question[index].choice = []
            if (questionTypeId == 8 || questionTypeId == 3 || questionTypeId == 9 || questionTypeId == 4) {
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })

            } else if (questionTypeId == 10) {
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
            }


            this.$forceUpdate();
        }

        Service: ReleaseTrainingService = new ReleaseTrainingService();
        listQuestionType: SurveyQuestionType[] = [];
        matchingImageFormData: IMatchingImageFormData[] = []

        listArea: AreaViewModel[] = [];
        listPosition: PositionViewModel[] = [];
        listDealer: DealerViewModel[] = [];
        listProvince: ProvinceViewModel[] = [];
        listCity: CityViewModel[] = [];
        listOutlet: OutletViewModel[] = [];

        selectedArea: AreaViewModel[] = [];
        //selectedPosition: PositionViewModel[] = [];
        selectedDealer: DealerViewModel[] = [];
        selectedProvince: ProvinceViewModel[] = [];
        selectedCity: CityViewModel[] = [];
        selectedOutlet: OutletViewModel[] = [];


        hotspotOptions: any = [
            { text: '1', value: 1 },
            { text: '2', value: 2 },
            { text: '3', value: 3 },
            { text: '4', value: 4 },
            { text: '5', value: 5 },
            { text: '6', value: 6 },
            { text: '7', value: 7 },
            { text: '8', value: 8 },
            { text: '9', value: 9 },
            { text: '10', value: 10 }
        ];
        selectedHotspotOption: number[] = [];

        searchableOutlet() {
            if (this.selectedArea.length > 0 && this.selectedProvince.length > 0 && this.selectedDealer.length > 0 && this.selectedCity.length > 0) {
                return false;
            }
            return true;
        }

        async checkingOutlet() {
            if (this.searchableOutlet() === false) {
                //console.log("Boleh Search");
                var filter: OutletFilterModel = {
                    Area: this.selectedArea,
                    Province: this.selectedProvince,
                    City: this.selectedCity,
                    Dealer: this.selectedDealer,
                };

                var jsonString = JSON.stringify(filter);

                this.listOutlet = await this.Service.getOutletFiltered(jsonString);

                //Mencurigakan
                for (let a = 0; a < this.surveyInsert.outlet.length; a++) {
                    let find = this.listOutlet.findIndex(Q => Q.outletId === this.surveyInsert.outlet[a]);
                    if (find === -1) {
                        console.log("Not found");
                        this.selectedOutlet = this.selectedOutlet.splice(a, 1);
                    }
                    else {
                        console.log("Found");
                    }
                }
            }
            else {
                console.log("Ada yang kosong");
                this.surveyInsert.outlet = [];
            }
            return;
        }

        async findOutlet() {
            if (this.searchableOutlet() === false) {
                var filter: OutletFilterModel = {
                    Area: this.selectedArea,
                    Province: this.selectedProvince,
                    City: this.selectedCity,
                    Dealer: this.selectedDealer,
                };

                var jsonString = JSON.stringify(filter);
                //console.log(jsonString);

                this.listOutlet = await this.Service.getOutletFiltered(jsonString);
            }
            return;
        }

        surveyId: number;

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("SVY");
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        async Initialize() {
            if (!this.crud.read) {
                return
            }
            this.isBusy = true
            this.listArea = await this.Service.getAllArea();
            this.listDealer = await this.Service.getAllDealer();
            this.listProvince = await this.Service.getAllProvince();
            this.listCity = await this.Service.getAllCity();
            this.listQuestionType = await this.surveyAPI.getAllSurveyQuestionType();

            this.selectedArea = await this.surveyAPI.getAreaById(this.surveyId);
            this.selectedCity = await this.surveyAPI.getCityById(this.surveyId);
            this.selectedProvince = await this.surveyAPI.getProvinceById(this.surveyId);
            this.selectedDealer = await this.surveyAPI.getDealerById(this.surveyId);
            this.surveyInsert = await this.surveyAPI.getSurveyById(this.surveyId);
            this.surveyInsert.startDate = new Date(this.surveyInsert.startDate.toString());
            this.surveyInsert.endDate = new Date(this.surveyInsert.endDate.toString());

            for (var i = 0; i < this.surveyInsert.question.length; i++) {
                this.QuestionImage.push({ FileUpload: [{ fileName: '', formData: new FormData, imageData: '' }] });
                this.AnswerImage.push({
                    FileUpload: []
                });
                this.selectedHotspotOption.push(1);
                if (this.surveyInsert.question[i].surveyQuestionTypeId == 2) {
                    this.matchingImageFormData.push({ data: [] })

                    let leftImageData: (string | ArrayBuffer)[] = []
                    let rightImageData: (string | ArrayBuffer)[] = []

                    for (const leftChoice of this.surveyInsert.question[i].matchingChoices.leftChoices) {
                        const leftImageBlobId = leftChoice.blobId;
                        if (leftImageBlobId != null) {
                            let imageBlob = await this.blobServiceNswag.getBlobById(leftImageBlobId);
                            let leftImageDatum = await BlobService.getImageUrl(imageBlob.blobId, imageBlob.mime);
                            leftImageData.push(leftImageDatum)
                        }
                        else {
                            leftImageData.push(null)
                        }
                    }

                    for (const rightChoice of this.surveyInsert.question[i].matchingChoices.rightChoices) {
                        const rightImageBlobId = rightChoice.blobId;
                        if (rightImageBlobId != null) {
                            let imageBlob = await this.blobServiceNswag.getBlobById(rightImageBlobId);
                            let rightImageDatum = await BlobService.getImageUrl(imageBlob.blobId, imageBlob.mime);
                            rightImageData.push(rightImageDatum)
                        }
                        else {
                            rightImageData.push(null)
                        }
                    }
                    for (let j = 0; j < leftImageData.length; j++) {
                        this.matchingImageFormData[i].data.push({ leftImage: leftImageData[j], rightImage: rightImageData[j] })
                    }

                } else {
                    this.matchingImageFormData.push({ data: null })
                }
                var questionBlobId = this.surveyInsert.question[i].blobId;

                if (questionBlobId != null) {
                    var imageQuestion = await this.blobServiceNswag.getBlobById(questionBlobId);
                    var imageQuestionUrl = await BlobService.getImageUrl(imageQuestion.blobId, imageQuestion.mime);

                    this.QuestionImage[i].FileUpload[0].imageData = imageQuestionUrl;
                    this.QuestionImage[i].FileUpload[0].fileName = this.surveyInsert.question[i].fileName;
                }

                if (this.surveyInsert.question[i].choice != null) {
                    for (var j = 0; j < this.surveyInsert.question[i].choice.length; j++) {
                        var blobId = this.surveyInsert.question[i].choice[j].blobId;
                        if (blobId != null) {
                            this.AnswerImage[i].FileUpload.push({
                                fileName: '',
                                formData: new FormData(),
                                imageData: ''
                            });
                            var image = await this.blobServiceNswag.getBlobById(blobId);
                            var imageUrl = await BlobService.getImageUrl(image.blobId, image.mime);

                            this.AnswerImage[i].FileUpload[j].imageData = imageUrl;
                            this.AnswerImage[i].FileUpload[j].fileName = this.surveyInsert.question[i].choice[j].fileName;
                        }
                    }
                }

                if (this.surveyInsert.question[i].surveyQuestionTypeId == 5) {
                    this.selectedHotspotOption[i] = this.surveyInsert.question[i].choice.length;
                }
            }

            this.isBusy = false
        }

        //TEBAK GAMBAR
        //Variable untuk File Upload Manual
        //AnswerImage: TebakGambar[] = [{ FileUpload: [{ fileName: '', formData: new FormData, imageData: '' }] }] //Variable yang di pake buat atur UI image
        AnswerImage: TebakGambar[] = []; //Variable yang di pake buat atur UI image
        QuestionImage: TebakGambar[] = []; //Variable yang di pake buat atur UI image

        //VALIDASI
        errorMessageShow: boolean = false

        handler(indexQuestion: number, index: number, $event) {
            if ($event.target.files.length === 0) {
                return;
            }
            this.loadFile($event, indexQuestion, index);
        }
        loadFile($event, indexQuestion: number, index: number) {
            var reader = new FileReader();
            reader.onload = (e: Event) => {
                this.AnswerImage[indexQuestion].FileUpload[index].imageData = (<FileReader>e.target).result;
            }
            reader.readAsDataURL($event.target.files[0]);
            this.AnswerImage[indexQuestion].FileUpload[index].fileName = $event.target.files[0].name;
            this.AnswerImage[indexQuestion].FileUpload[index].formData.set($event.target.files, $event.target.files[0], $event.target.files[0].name);
        }

        Close() {
            if (this.type != undefined) {
                if (this.type.toLowerCase() == 'fromoutside') {
                    window.history.back();
                }
            }

            this.$emit('update:view', false);
        }

        minDate: Date = new Date('0001-01-01T00:00:00');
        getStringDate(date: Date) {

            if (!date) {
                return '-';
            }

            if (new Date(date) === this.minDate) {
                return '-';
            }

            let newDate = new Date(date);

            var ddDate = newDate.getDate();
            var mmDate = newDate.getMonth() + 1;
            var yyyyDate = newDate.getFullYear();
            var dd = '' + ddDate;
            var mm = '' + mmDate;
            var yyyy = yyyyDate;
            if (ddDate < 10) {
                dd = '0' + ddDate;
            }
            if (mmDate < 10) {
                mm = '0' + mmDate;
            }
            var today = dd + '/' + mm + '/' + yyyy;

            return today;
        }

    }
    interface OutletFilterModel {
        Area: AreaViewModel[],
        Province: ProvinceViewModel[],
        City: CityViewModel[],
        Dealer: DealerViewModel[]
    }
    interface FileUploadImage {
        imageData: string | ArrayBuffer;
        formData: FormData;
        fileName: string;
    }
    interface TebakGambar {
        FileUpload: FileUploadImage[]
    }
    interface IMatchingImageFormData {
        data: IMatchingImageFormDatum[]
    }
    interface IMatchingImageFormDatum {
        leftImage: string | ArrayBuffer | undefined,
        rightImage: string | ArrayBuffer | undefined
    }
</script>
