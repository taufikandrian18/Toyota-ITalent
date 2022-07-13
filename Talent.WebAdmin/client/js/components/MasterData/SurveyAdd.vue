<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>
        <div class="row">
            <div class="col col-md-12">
                <!--TITLE-->
                <h3 class="mb-4"><fa icon="database"></fa> Master Data > Survey > <span class="talent_font_red">Add Survey</span></h3>


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
                                <input maxlength="255" name="Title" v-validate="'required'" v-model="surveyInsert.title" class="form-control" />
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
                                                 v-validate:areaLength="'min_value:1'"
                                                 tag-placeholder="Add Area"
                                                 placeholder="Add Area"
                                                 label="name"
                                                 track-by="areaId"
                                                 :options="listAreaGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="areaChanged"
                                                 @remove="areaRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>Dealer<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="selectedDealer"
                                                 name="dealer"
                                                 v-validate:dealerLength="'min_value:1'"
                                                 tag-placeholder="Add Dealer"
                                                 placeholder="Add Dealer"
                                                 label="dealerName"
                                                 track-by="dealerId"
                                                 :options="listDealerGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="dealerChanged"
                                                 @remove="dealerRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>Province<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="selectedProvince"
                                                 name="province"
                                                 v-validate:provinceLength="'min_value:1'"
                                                 tag-placeholder="Add Province"
                                                 placeholder="Add Province"
                                                 label="provinceName"
                                                 track-by="provinceId"
                                                 :options="listProvinceGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="provinceChanged"
                                                 @remove="provinceRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>City<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="selectedCity"
                                                 name="city"
                                                 v-validate:cityLength="'min_value:1'"
                                                 tag-placeholder="Add City"
                                                 placeholder="Add City"
                                                 label="cityName"
                                                 track-by="cityId"
                                                 :options="listCityGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 @select="cityChanged"
                                                 @remove="cityRemoved"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                    <label>Outlet<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="selectedOutletCompleted"
                                                 name="outlet"
                                                 v-validate:outletLength="'min_value:1'"
                                                 tag-placeholder="Add Outlet"
                                                 placeholder="Add Outlet"
                                                 label="name"
                                                 track-by="outletId"
                                                 :options="listOutletCompletedGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 selectLabel="Press enter to select">
                                    </multiselect>
                                    <br />
                                </div>
                                <div class="col-md-6">
                                    <label>Position<span class="talent_font_red">*</span></label>
                                    <multiselect v-model="surveyInsert.position"
                                                 name="position"
                                                 v-validate:positionLength="'min_value:1'"
                                                 tag-placeholder="Add Position"
                                                 placeholder="Add Position"
                                                 label="positionName"
                                                 track-by="positionId"
                                                 :options="listPositionGroup"
                                                 group-values="ListOption"
                                                 group-label="GroupName"
                                                 :group-select="true"
                                                 :clear-on-select="true"
                                                 :multiple="true"
                                                 class="oneline-scrollable-multiselect"
                                                 :show-no-results="true"
                                                 selectLabel="Press enter to select">
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
                                                <v-date-picker class="v-date-style-report"
                                                               name="period start"
                                                               v-validate="'required'"
                                                               mode="single"
                                                               :firstDayOfWeek="2"
                                                               :max-date="surveyInsert.endDate"
                                                               v-model="surveyInsert.startDate"
                                                               :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                                <v-date-picker class="v-date-style-report"
                                                               name="period end"
                                                               v-validate="'required'"
                                                               mode="single"
                                                               :firstDayOfWeek="2"
                                                               :min-date="surveyInsert.startDate"
                                                               v-model="surveyInsert.endDate"
                                                               :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
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
                                                    <button class="btn talent_bg_cyan talent_roundborder talent_font_white" v-if="indexQuestion+1 == totalSurveyInsert" @click.prevent="AddSurvey()">Add Survey</button>
                                                    <button class="btn talent_bg_red talent_roundborder talent_font_white" v-else @click.prevent="DeleteSurvey(indexQuestion)">Delete Survey</button>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row mb-4">
                                    <div class="col-md-12 mb-4">
                                        <label>Survey Question<span class="talent_font_red">*</span></label>
                                        <textarea maxlength="512" name="question" v-validate="'required'" v-model="survey.question" class="form-control talent_textarea"></textarea>
                                    </div>
                                    <br />
                                    <div class="col-md-12">
                                        <label>Type of Survey<span class="talent_font_red">*</span></label>
                                        <select class="form-control" v-model="survey.surveyQuestionTypeId" @change.prevent="ForceUpdate(indexQuestion, survey.surveyQuestionTypeId)">
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
                                                            <th v-for="(m, index) in listMatrix[indexQuestion].measurement">
                                                                <textarea name="Matix Choice" v-validate="'required|max:64'" class="form-control" :placeholder="'Insert Measurement ' + (index + 1)" v-model="listMatrix[indexQuestion].measurement[index]" maxlength="64"></textarea>
                                                            </th>
                                                            <th class="talent_noborder " style="padding: 0px 10px">

                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr v-for="(option, indexMatrix) in listMatrix[indexQuestion].option">
                                                            <td><textarea name="Matix Question" v-validate="'required|max:64'" class="form-control" :placeholder="'Insert Option ' + (indexMatrix + 1)" v-model="listMatrix[indexQuestion].option[indexMatrix]" maxlength="64"></textarea></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td></td>
                                                            <td class="talent_noborder" style="padding: 0px 0px 0px 10px">
                                                                <a class="h-100 w-100 talent_cursorpoint text-center" v-if="indexMatrix != listMatrix[indexQuestion].option.length - 1" @click.prevent="removeMatrix(indexQuestion,indexMatrix)"><fa icon="trash-alt"></fa></a>
                                                                <a class="h-100 w-100 talent_cursorpoint text-center" v-if="indexMatrix == listMatrix[indexQuestion].option.length - 1" @click.prevent="addMatrix(indexQuestion)"><fa icon="plus-circle"></fa></a>
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
                                                        <input maxlength="512" name="multiplechoice choice" v-validate="'required'" v-model="choice.choice" class="form-control" />
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
                                                        <input maxlength="512" name="rating choice" v-validate="'required'" v-model="choice.choice" class="form-control" />
                                                    </div>
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        <!--Remove-->
                                                        <div v-if="indexRating != surveyInsert.question[indexQuestion].choice.length" class="text-center" @click.prevent="removeChoice(indexQuestion, indexRating)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                                        <!--Add-->
                                                        <div v-if="indexRating == surveyInsert.question[indexQuestion].choice.length" class="text-center" @click.prevent="addChoice(indexQuestion)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
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
                                                        <input maxlength="512" name="checklist choice" v-validate="'required'" v-model="choice.choice" class="form-control" />
                                                    </div>
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        <!--Remove-->
                                                        <div v-if="indexChecklist != surveyInsert.question[indexQuestion].choice.length" class="text-center" @click.prevent="removeChoice(indexQuestion, indexChecklist)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                                        <!--Add-->
                                                        <div v-if="indexChecklist == surveyInsert.question[indexQuestion].choice.length" class="text-center" @click.prevent="addChoice(indexQuestion)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
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
                                                <img class="h-100 w-100" :src="QuestionImage[indexQuestion].FileUpload[0].imageData ? QuestionImage[indexQuestion].FileUpload[0].imageData : '/upload-image2.png'" />
                                                <div class="custom-file">
                                                    <input name="Hotspot Image" v-validate="'required'" type="file" class="custom-file-input" id="customFile" @change.prevent="handlerQuestion(indexQuestion, $event)" />
                                                    <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{QuestionImage[indexQuestion].FileUpload[0].fileName.length ? QuestionImage[indexQuestion].FileUpload[0].fileName : 'Choose File'}}</label>
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
                                                        <select class="form-control" v-model="selectedHotspotOption[indexQuestion]" @change="hotspotChange(indexQuestion, $event)">
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
                                                                <input name="Tebak Gambar" v-validate="'required'" type="file" class="custom-file-input" id="customFile" @change.prevent="handler(indexQuestion, indexTebakGambar, $event)" />
                                                                <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{image.fileName.length ? image.fileName : 'Choose File'}}</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-1 align-self-center">
                                                        <div v-if="indexTebakGambar != AnswerImage[indexQuestion].FileUpload.length - 1" class="text-center" @click.prevent="RemovePicture(indexQuestion, indexTebakGambar)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                                        <div v-if="indexTebakGambar == AnswerImage[indexQuestion].FileUpload.length - 1" class="text-center" @click.prevent="AddPicture(indexQuestion)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
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
                                                        <input maxlength="512" name="sequence choice" v-validate="'required'" v-model="choice.choice" class="form-control" />
                                                    </div>
                                                    <div class="col-md-1 d-flex align-items-center justify-content-center">
                                                        <!--Remove-->
                                                        <div v-if="indexSequence != surveyInsert.question[indexQuestion].choice.length" class="text-center" @click.prevent="removeChoice(indexQuestion, indexSequence)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                                        <!--Add-->
                                                        <div v-if="indexSequence == surveyInsert.question[indexQuestion].choice.length" class="text-center" @click.prevent="addChoice(indexQuestion)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <!--Matching-->
                                    <div class="col-md-12" v-else-if="survey.surveyQuestionTypeId == 2">
                                        <div v-for="(leftMatchingChoice, index) in survey.matchingChoices.leftChoices" class="" :key="index">
                                            <div class="row ">
                                                <div class="col-md-10">
                                                    <div class="row">
                                                        <div class="col-md-6 talent_borderright ">
                                                            <div class="talent_marginbottom h-100">
                                                                <div class="input-group mb-3 talent_nomargin">
                                                                    <div class="input-group-prepend ">
                                                                        <span class="input-group-text talent_noroundborder_bottom">{{index+1}}</span>
                                                                    </div>
                                                                    <select class="form-control talent_noroundborder_bottom" @change="onTypeChange($event)" v-model="surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].type">
                                                                        <option value="image">Image</option>
                                                                        <option value="text">Text</option>
                                                                    </select>
                                                                </div>
                                                                <div v-if="surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].type === 'image'">
                                                                    <img class="h-100 w-100" :src="matchingImageFormData[indexQuestion].data[index].leftImage ? matchingImageFormData[indexQuestion].data[index].leftImage : '/upload-image2.png'" />
                                                                    <div class="custom-file">
                                                                        <input type="file" class="custom-file-input talent_noroundborder_top" v-validate="'required'" name="matching left image choice" accept="image/*" @change.prevent="loadFileChoice($event,indexQuestion, index,surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index],true)">
                                                                        <label class="custom-file-label talent_textshorten talent_noroundborder_top">{{surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].imageUpload? surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].imageUpload.fileName : 'Choose File'}}</label>
                                                                    </div>
                                                                </div>
                                                                <div v-if="surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].type === 'text'" class="h-100">
                                                                    <textarea class="form-control talent_textarea talent_noroundborder_top" v-validate="'required'" name="matching left text choice" v-model="surveyInsert.question[indexQuestion].matchingChoices.leftChoices[index].text" style="height: calc(100% - 52px)!important;" maxlength="2000"></textarea>

                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-6 ">
                                                            <div class="talent_marginbottom h-100">
                                                                <div class="input-group mb-3 talent_nomargin">
                                                                    <div class="input-group-prepend">
                                                                        <span class="input-group-text talent_noroundborder_bottom">{{index+1}}</span>
                                                                    </div>
                                                                    <select class="form-control talent_noroundborder_bottom" @change="onTypeChange($event)" v-model="surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].type">
                                                                        <option value="image">Image</option>
                                                                        <option value="text">Text</option>
                                                                    </select>
                                                                </div>
                                                                <div v-if="surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].type === 'image'">
                                                                    <img class="h-100 w-100" :src="matchingImageFormData[indexQuestion].data[index].rightImage ? matchingImageFormData[indexQuestion].data[index].rightImage : '/upload-image2.png'" />
                                                                    <div class="custom-file">
                                                                        <input type="file" class="custom-file-input talent_noroundborder_top" v-validate="'required'" name="matching right image choice" accept="image/*" @change.prevent="loadFileChoice($event,indexQuestion, index,surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index],false)">
                                                                        <label class="custom-file-label talent_textshorten talent_noroundborder_top">{{surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].imageUpload ? surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].imageUpload.fileName  : 'Choose File'}}</label>
                                                                    </div>
                                                                </div>
                                                                <div v-if="surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].type === 'text'" class="h-100">
                                                                    <textarea class="form-control talent_textarea talent_noroundborder_top" v-validate="'required'" name="matching right text choice" v-model="surveyInsert.question[indexQuestion].matchingChoices.rightChoices[index].text" style="height: calc(100% - 52px)!important;" maxlength="2000"></textarea>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-2 d-flex align-items-center ">
                                                    <div v-if="index != survey.matchingChoices.leftChoices.length - 1" class="text-center" @click.prevent="removeMatchingRow(survey,index)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="trash-alt"></fa></a></div>
                                                    <div v-if="index == survey.matchingChoices.leftChoices.length - 1" class="text-center" @click.prevent="addMatchingRow(indexQuestion,index+1)"><a class="h-100 w-100 talent_cursorpoint"><fa icon="plus-circle"></fa></a></div>
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
                                    <button class="btn talent_bg_lightgreen talent_font_white" :disabled="!crud.create" @click.prevent="SaveSurvey()">
                                        Save
                                    </button>
                                    <button class="btn talent_bg_blue talent_font_white" :disabled="!crud.create" @click.prevent="SubmitSurvey()">
                                        Submit
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
    import { TaskMatchingTypeFormModel, SurveyMatrixModel, FileContent, SurveysService, SurveyInsert, SurveyMatchingChoice, SurveyMatchingChoiceFormModel, SurveyQuestion, SurveyChoice, AreaViewModel, PositionViewModel, DealerViewModel, ProvinceViewModel, CityViewModel, OutletViewModel, ReleaseTrainingService, SurveyQuestionType, UserAccessCRUD, UserPrivilegeSettingsService, OutletCompleteViewModel } from '../../services/NSwagService'
    import IFileContent from '../../models/IFileContent';
    import { isNullOrUndefined } from 'util';

    @Component({
        props: ["add", "success-message-show", "success-message"],
        created: async function (this: SurveyAdd) {
            this.isBusy = true;
            await this.getAccess()
            await this.Initialize();
        }
    })
    export default class SurveyAdd extends Vue {
        isBusy: boolean = false

        //Api
        surveyAPI: SurveysService = new SurveysService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        surveyInsert: SurveyInsert = { title: '', startDate: null, endDate: null, outlet: [], position: [], question: [{ blobId: '', choice: [], question: '', questionNumber: 1, surveyQuestionTypeId: 1 }] }

        matrix: IMatrixContainer = { question: "Question", measurement: [null, null, null, null, null], option: [null] };
        listMatrix: IMatrixContainer[] = [{ question: "Question", measurement: [null, null, null, null, null], option: [null] }];

        addMatrix(indexQuestion: number) {
            this.listMatrix[indexQuestion].option.push(null);
            this.$forceUpdate();
        }

        removeMatrix(indexQuestion: number, index: number) {
            if (this.listMatrix[indexQuestion].option.length <= 1) {
                return
            }
            this.listMatrix[indexQuestion].option.splice(index, 1);
            this.$forceUpdate();
        }

        get totalSurveyInsert() {
            return this.surveyInsert.question.length
        }

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

        selectedHotspotOption: number[] = [1];

        ForceUpdate(index, questionTypeId: number) {
            this.surveyInsert.question[index].choice = []
            if (questionTypeId == 8 || questionTypeId == 3 || questionTypeId == 9 || questionTypeId == 4) {
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
            }
            if (questionTypeId == 10) {
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
            } else if (questionTypeId == 12) {
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
                this.listMatrix[index] = { question: "Question", measurement: [null, null, null, null, null], option: [null] };
            }
            if (questionTypeId == 5) {
                this.surveyInsert.question[index].choice.push({ blobId: '', choice: null })
            }
            if (questionTypeId == 2) {
                let survey = this.surveyInsert.question[index]
                survey.choice = null
                survey.matchingChoices = {
                    leftChoices: [{
                        surveyMatchingChoiceCode: this.getMatchingChoiceCode(true, 1),
                        type: 'text'
                    }],
                    rightChoices: [{
                        surveyMatchingChoiceCode: this.getMatchingChoiceCode(false, 1),
                        type: 'text'
                    }]
                }
                this.matchingImageFormData.splice(index, 1, {
                    data: [{ leftImage: null, rightImage: null }]
                })
                this.addMatchingRow(index, 1)
            } else {
                let survey = this.surveyInsert.question[index]
                survey.matchingChoices = null
            }

            this.$forceUpdate();
        }
        addMatchingRow(questionIndex: number, choiceIndex: number) {
            this.surveyInsert.question[questionIndex].matchingChoices.leftChoices.push({
                surveyMatchingChoiceCode: this.getMatchingChoiceCode(true, choiceIndex + 1),
                type: 'text',
                text: null
            })
            this.surveyInsert.question[questionIndex].matchingChoices.rightChoices.push({
                surveyMatchingChoiceCode: this.getMatchingChoiceCode(false, choiceIndex + 1),
                type: 'text',
                text: null
            })
            this.matchingImageFormData[questionIndex].data.push(
                {
                    leftImage: null,
                    rightImage: null
                }
            )
            this.$forceUpdate()
        }
        //Nambah Survey
        async AddSurvey() {
            this.surveyInsert.question.push({ blobId: '', choice: [], question: '', questionNumber: this.totalSurveyInsert + 1, surveyQuestionTypeId: 1 })
            this.listMatrix.push(this.matrix);
            this.AnswerImage.push({ FileUpload: [{ fileName: '', formData: { base64: '', fileName: '', contentType: '' }, imageData: '' }] })
            this.QuestionImage.push({ FileUpload: [{ fileName: '', formData: { base64: '', fileName: '', contentType: '' }, imageData: '' }] })
            this.selectedHotspotOption.push(1);
            this.matchingImageFormData.push({ data: null })
        }

        async DeleteSurvey(indexToDelete: number) {
            this.surveyInsert.question.splice(indexToDelete, 1);
            this.listMatrix.splice(indexToDelete, 1);
            this.selectedHotspotOption.splice(indexToDelete, 1);
            this.AnswerImage.splice(indexToDelete, 1);
            this.QuestionImage.splice(indexToDelete, 1);
            this.matchingImageFormData.splice(indexToDelete, 1);

            if (indexToDelete < this.totalSurveyInsert) {
                for (var a = indexToDelete; a < this.totalSurveyInsert; a++) {
                    this.surveyInsert.question[a].questionNumber = a + 1;
                }
            }

            if (indexToDelete < this.totalSurveyInsert) {
                for (var a = indexToDelete; a < this.totalSurveyInsert; a++) {
                    this.surveyInsert.question[a].questionNumber = a + 1;
                }
            }
        }
        //Outlet And Position MultiSelect

        Service: ReleaseTrainingService = new ReleaseTrainingService();
        listQuestionType: SurveyQuestionType[] = [];

        listArea: AreaViewModel[] = [];
        listPosition: PositionViewModel[] = [];
        listDealer: DealerViewModel[] = [];
        listProvince: ProvinceViewModel[] = [];
        listCity: CityViewModel[] = [];
        listOutlet: OutletViewModel[] = [];
        listCompletedOutlet: OutletCompleteViewModel[] = [];

        listAreaGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listPositionGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listDealerGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listProvinceGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listCityGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listOutletGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];
        listOutletCompletedGroup: ListGrouping[] = [{
            ListOption: [],
            GroupName: 'Select All'
        }];

        selectedArea: AreaViewModel[] = [];
        selectedPosition: PositionViewModel[] = [];
        selectedDealer: DealerViewModel[] = [];
        selectedProvince: ProvinceViewModel[] = [];
        selectedCity: CityViewModel[] = [];
        selectedOutlet: OutletViewModel[] = [];
        selectedOutletCompleted: OutletCompleteViewModel[] = [];

        //Validasi MultiSelect Pakai Computed Buat Cari Length
        get areaLength() {
            return this.selectedArea.length
        }
        get dealerLength() {
            return this.selectedDealer.length
        }
        get provinceLength() {
            return this.selectedProvince.length
        }
        get cityLength() {
            return this.selectedCity.length
        }
        get outletLength() {
            return this.listCompletedOutlet.length
        }
        get positionLength() {
            return this.surveyInsert.position.length
        }

        searchableOutlet() {
            if (this.selectedArea.length > 0 && this.selectedProvince.length > 0 && this.selectedDealer.length > 0 && this.selectedCity.length > 0) {
                return false;
            }
            return true;
        }

        getSelectedArea() {
            var areaIds = this.selectedArea.map(function (Q) {
                return Q.areaId;
            });

            return areaIds;
        }

        getSelectedDealer() {
            var dealerIds = this.selectedDealer.map(function (Q) {
                return Q.dealerId;
            });

            return dealerIds;
        }

        getSelectedProvince() {
            var provinceIds = this.selectedProvince.map(function (Q) {
                return Q.provinceId;
            });

            return provinceIds;
        }

        getSelectedCity() {
            var cityIds = this.selectedCity.map(function (Q) {
                return Q.cityId
            });

            return cityIds;
        }

        findDealer(findableOutlet) {
            var dealerIds = findableOutlet.map(function (Q) {
                return Q.dealerId;
            });

            return dealerIds
        }

        findProvince(findableOutlet) {
            var provinceIds = findableOutlet.map(function (Q) {
                return Q.provinceId;
            });

            return provinceIds;
        }

        findCities(findableOutlet) {
            var cityIds = findableOutlet.map(function (Q) {
                return Q.cityId;
            });

            return cityIds;
        }


        onlyUnique(value, index, self) {
            return self.indexOf(value) === index;
        }

        async areaChanged(value) {
            var areaIds = this.getSelectedArea();
            var inserted;
            if (value != null) {
                if (value.areaId) {
                    inserted = value.areaId;
                    areaIds.push(inserted);
                }
                else {
                    value.forEach(Q => areaIds.push(Q.areaId));
                }
            }
            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = this.listCompletedOutlet.filter(Q => areaIds.includes(Q.areaId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var dealerIds = this.findDealer(findableOutlet);
            var distinctDealerIds = dealerIds.filter(this.onlyUnique);
            var showDealer = this.listDealer.filter(Q => distinctDealerIds.includes(Q.dealerId));
            this.listDealerGroup[0].ListOption = showDealer;

            var provinceIds = this.findProvince(findableOutlet);
            var distinctProvinceIds = provinceIds.filter(this.onlyUnique);
            var showProvince = this.listProvince.filter(Q => distinctProvinceIds.includes(Q.provinceId));
            this.listProvinceGroup[0].ListOption = showProvince;

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
        }


        hotspotChange(indexQuestion, event) {
            this.surveyInsert.question[indexQuestion].choice = [];

            for (var a = 0; a < event.target.value; a++) {
                this.surveyInsert.question[indexQuestion].choice.push({ blobId: '', choice: null });
            }

            this.selectedHotspotOption[indexQuestion] = this.surveyInsert.question[indexQuestion].choice.length;
        }

        async areaRemoved(value) {
            var areaIds = this.getSelectedArea();

            var index = areaIds.findIndex(Q => Q === value.areaId);
            if (index == -1) {
                areaIds = [];
            }
            else {
                areaIds.splice(index, 1);
            }

            var findableOutlet = this.listCompletedOutlet;

            if (areaIds.length == 0) {
                this.listOutletCompletedGroup[0].ListOption = this.listCompletedOutlet;
                this.selectedOutletCompleted = [];
                this.listDealerGroup[0].ListOption = this.listDealer;
                this.selectedDealer = [];
                this.listProvinceGroup[0].ListOption = this.listProvince;
                this.selectedProvince = [];
                this.listCityGroup[0].ListOption = this.listCity;
                this.selectedCity = [];
            }
            else {
                if (areaIds.length != 0) {
                    findableOutlet = this.listCompletedOutlet.filter(Q => areaIds.includes(Q.areaId));
                }

                this.listOutletCompletedGroup[0].ListOption = findableOutlet;
                this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

                var dealerIds = this.findDealer(findableOutlet);
                var distinctDealerIds = dealerIds.filter(this.onlyUnique);
                var showDealer = this.listDealer.filter(Q => distinctDealerIds.includes(Q.dealerId));
                this.listDealerGroup[0].ListOption = showDealer;
                this.selectedDealer = this.selectedDealer.filter((el) => this.listDealerGroup[0].ListOption.includes(el));

                var provinceIds = this.findProvince(findableOutlet);
                var distinctProvinceIds = provinceIds.filter(this.onlyUnique);
                var showProvince = this.listProvince.filter(Q => distinctProvinceIds.includes(Q.provinceId));
                this.listProvinceGroup[0].ListOption = showProvince;
                this.selectedProvince = this.selectedProvince.filter((el) => this.listProvinceGroup[0].ListOption.includes(el));

                var cityIds = this.findCities(findableOutlet);
                var distinctCityIds = cityIds.filter(this.onlyUnique);
                var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
                this.listCityGroup[0].ListOption = showCity;
                this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
            }
        }

        async dealerChanged(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();

            if (value.dealerId) {
                dealerIds.push(value.dealerId);
            }
            else {
                value.forEach(Q => dealerIds.push(Q.dealerId));
            }
            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var provinceIds = this.findProvince(findableOutlet);
            var distinctProvinceIds = provinceIds.filter(this.onlyUnique);
            var showProvince = this.listProvince.filter(Q => distinctProvinceIds.includes(Q.provinceId));
            this.listProvinceGroup[0].ListOption = showProvince;
            this.selectedProvince = this.selectedProvince.filter((el) => this.listProvinceGroup[0].ListOption.includes(el));

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
            this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
        }

        async dealerRemoved(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();

            var index = dealerIds.findIndex(Q => Q === value.dealerId);
            if (index == -1) {
                dealerIds = [];
            }
            else {
                dealerIds.splice(index, 1);
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var provinceIds = this.findProvince(findableOutlet);
            var distinctProvinceIds = provinceIds.filter(this.onlyUnique);
            var showProvince = this.listProvince.filter(Q => distinctProvinceIds.includes(Q.provinceId));
            this.listProvinceGroup[0].ListOption = showProvince;
            this.selectedProvince = this.selectedProvince.filter((el) => this.listProvinceGroup[0].ListOption.includes(el));

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
            this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
        }

        async provinceChanged(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();
            var provinceIds = this.getSelectedProvince();

            if (value.provinceId) {
                provinceIds.push(value.provinceId);
            }
            else {
                value.forEach(Q => provinceIds.push(Q.provinceId));
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }
            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }
            if (provinceIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => provinceIds.includes(Q.provinceId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
            this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
        }

        async provinceRemoved(value) {
            var areaIds = this.getSelectedArea();

            var dealerIds = this.getSelectedDealer();

            var provinceIds = this.getSelectedProvince();

            var index = provinceIds.findIndex(Q => Q === value.provinceId);
            if (index == -1) {
                provinceIds = [];
            }
            else {
                provinceIds.splice(index, 1);
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }

            if (provinceIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => provinceIds.includes(Q.provinceId));
            }
            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));

            var cityIds = this.findCities(findableOutlet);
            var distinctCityIds = cityIds.filter(this.onlyUnique);
            var showCity = this.listCity.filter(Q => distinctCityIds.includes(Q.cityId));
            this.listCityGroup[0].ListOption = showCity;
            this.selectedCity = this.selectedCity.filter((el) => this.listCityGroup[0].ListOption.includes(el));
        }

        async cityChanged(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();
            var provinceIds = this.getSelectedProvince();
            var cityIds = this.getSelectedCity();

            if (value.cityId) {
                cityIds.push(value.cityId);
            }
            else {
                value.forEach(Q => cityIds.push(Q.cityId));
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }

            if (provinceIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => provinceIds.includes(Q.provinceId));
            }

            if (cityIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => cityIds.includes(Q.cityId));
            }

            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));
        }

        async cityRemoved(value) {
            var areaIds = this.getSelectedArea();
            var dealerIds = this.getSelectedDealer();
            var provinceIds = this.getSelectedProvince();
            var cityIds = this.getSelectedCity();

            var index = cityIds.findIndex(Q => Q === value.cityId);
            if (index == -1) {
                cityIds = [];
            }
            else {
                cityIds.splice(index, 1);
            }

            var findableOutlet = this.listCompletedOutlet;
            if (areaIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => areaIds.includes(Q.areaId));
            }

            if (dealerIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => dealerIds.includes(Q.dealerId));
            }

            if (provinceIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => provinceIds.includes(Q.provinceId));
            }

            if (cityIds.length != 0) {
                findableOutlet = findableOutlet.filter(Q => cityIds.includes(Q.cityId));
            }

            this.listOutletCompletedGroup[0].ListOption = findableOutlet;
            this.selectedOutletCompleted = this.selectedOutletCompleted.filter((el) => this.listOutletCompletedGroup[0].ListOption.includes(el));
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("SVY");
            this.crud = data
        }
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        async Initialize() {
            if (!this.crud.create) {
                return
            }
            this.isBusy = true
            this.listArea = await this.Service.getAllArea();
            //this.listPosition = await this.Service.getAllPositionReleaseTraining();
            this.listDealer = await this.Service.getAllDealer();
            this.listProvince = await this.Service.getAllProvince();
            this.listCity = await this.Service.getAllCity();
            this.listCompletedOutlet = await this.Service.getAllOutlet();

            this.listQuestionType = await this.surveyAPI.getAllSurveyQuestionType();

            let newListOutlet = this.surveyInsert.outlet.map(Q => Q.outletId);
            var outletJson = JSON.stringify(newListOutlet);
            this.listPosition = await this.Service.getAllPosition();

            this.listAreaGroup[0].ListOption = this.listArea;
            this.listDealerGroup[0].ListOption = this.listDealer;
            this.listProvinceGroup[0].ListOption = this.listProvince;
            this.listCityGroup[0].ListOption = this.listCity;
            this.listPositionGroup[0].ListOption = this.listPosition;
            this.listOutletCompletedGroup[0].ListOption = this.listCompletedOutlet;


            this.isBusy = false
        }

        addChoice(questionIndex: number) {
            this.surveyInsert.question[questionIndex].choice.push({ blobId: '', choice: null })
        }

        //deletelistoption
        async removeChoice(questionIndex: number, choiceIndex) {
            if (this.surveyInsert.question[questionIndex].choice.length <= 1) {
                return
            }
            this.surveyInsert.question[questionIndex].choice.splice(choiceIndex - 1, 1)
        }
        //TEBAK GAMBAR
        fileContentImage: IFileContent = {
            base64: "",
            contentType: "",
            fileName: ""
        }
        //Variable untuk File Upload Manual
        AnswerImage: TebakGambar[] = [{
            FileUpload: [{
                fileName: '',
                formData: this.fileContentImage = {
                    base64: "",
                    contentType: "",
                    fileName: ""
                },
                imageData: ''
            }]
        }] //Variable yang di pake buat atur UI image

        QuestionImage: TebakGambar[] = [{
            FileUpload: [{
                fileName: '',
                formData: this.fileContentImage = {
                    base64: "",
                    contentType: "",
                    fileName: ""
                },
                imageData: ''
            }]
        }] //Variable yang di pake buat atur UI image

        //ADD ANSWER ARRAY
        AddPicture(questionIndex: number) {
            this.surveyInsert.question[questionIndex].choice.push({ blobId: '', choice: null })
            this.AnswerImage[questionIndex].FileUpload.push({
                fileName: '',
                formData: this.fileContentImage = {
                    base64: "",
                    contentType: "",
                    fileName: ""
                },
                imageData: ''
            })
        }
        //REMOVE ANSWER ARRAY
        RemovePicture(questionIndex: number, choiceIndex) {
            if (this.AnswerImage.length <= 1 && this.surveyInsert.question[questionIndex].choice.length <= 1) {
                return
            }
            this.AnswerImage[questionIndex].FileUpload.splice(choiceIndex, 1);
            this.surveyInsert.question[questionIndex].choice.splice(choiceIndex, 1)
        }

        //UPLOADIMAGE
        UploadFile(formData: FormData) {
            return BlobService.uploadService(formData);
        }

        //VALIDASI
        errorMessageShow: boolean = false


        //Submit SURVEY
        async SubmitSurvey() {
            if (!this.crud.create) {
                return
            }
            this.isBusy = true
            let valid = await this.$validator.validateAll();
            if (!valid) {
                this.errorMessageShow = true;
                this.isBusy = false
                return;
            }
            //foreach semua question
            for (var i = 0; i < this.surveyInsert.question.length; i++) {

                //Hotspot
                if (this.surveyInsert.question[i].surveyQuestionTypeId == 5) {
                    this.surveyInsert.question[i].fileContent = this.QuestionImage[i].FileUpload[0].formData;
                }
                //Tebak Gambar
                else if (this.surveyInsert.question[i].surveyQuestionTypeId == 4) {
                    //foreach semua image
                    for (var j = 0; j < this.AnswerImage[i].FileUpload.length; j++) {
                        //await this.UploadFile(this.AnswerImage[i].FileUpload[j].formData).then((onResult) => {
                        //    this.surveyInsert.question[i].choice[j].blobId = onResult
                        //})
                        this.surveyInsert.question[i].choice[j].fileContent = this.AnswerImage[i].FileUpload[j].formData
                    }
                }
                else if (this.surveyInsert.question[i].surveyQuestionTypeId == 12) {
                    let insertMatrix: SurveyMatrixModel = {
                        matrixChoice: [],
                        matrixQuestion: []
                    };

                    for (var k = 0; k < this.listMatrix[i].measurement.length; k++) {
                        insertMatrix.matrixChoice.push({ text: this.listMatrix[i].measurement[k], number: k + 1 });
                    }

                    for (var k = 0; k < this.listMatrix[i].option.length; k++) {
                        insertMatrix.matrixQuestion.push({ question: this.listMatrix[i].option[k], number: k + 1 });
                    }
                    this.surveyInsert.question[i].matrixChoice = insertMatrix;
                }
                //Matching
                else if (this.surveyInsert.question[i].surveyQuestionTypeId == 2) {
                    let survey = this.surveyInsert.question[i]
                    let getMatchingChoiceCode = this.getMatchingChoiceCode
                    for (let index = 0; index < survey.matchingChoices.leftChoices.length; index++) {
                        survey.matchingChoices.leftChoices[index].surveyMatchingChoiceCode = getMatchingChoiceCode(true, index + 1)
                        survey.matchingChoices.rightChoices[index].surveyMatchingChoiceCode = getMatchingChoiceCode(false, index + 1)
                    }
                }
            }

            this.setOutlet();

            var isSuccess = await this.surveyAPI.submitSurvey(this.surveyInsert).catch((error) => {
                alert(error)
                this.isBusy = false
                return false;
            });
            if (!isSuccess) return;
            this.$emit('update:successMessage', "Success to Add Data");
            this.$emit('update:successMessageShow', true);
            this.Close();

        }

        //save Survey
        async SaveSurvey() {
            if (!this.crud.create) {
                return
            }
            this.isBusy = true
            let valid = await this.$validator.validateAll();
            if (!valid) {
                this.errorMessageShow = true;
                this.isBusy = false
                return;
            }
            this.$validator.reset;

            //foreach semua question
            for (var i = 0; i < this.surveyInsert.question.length; i++) {

                //Hotspot
                if (this.surveyInsert.question[i].surveyQuestionTypeId == 5) {
                    this.surveyInsert.question[i].fileContent = this.QuestionImage[i].FileUpload[0].formData
                }
                //Tebak gambar
                else if (this.surveyInsert.question[i].surveyQuestionTypeId == 4) {
                    //foreach semua image
                    for (var j = 0; j < this.AnswerImage[i].FileUpload.length; j++) {
                        //await this.UploadFile(this.AnswerImage[i].FileUpload[j].formData).then((onResult) => {
                        //    this.surveyInsert.question[i].choice[j].blobId = onResult
                        //})
                        this.surveyInsert.question[i].choice[j].fileContent = this.AnswerImage[i].FileUpload[j].formData
                    }
                }
                else if (this.surveyInsert.question[i].surveyQuestionTypeId == 12) {
                    let insertMatrix: SurveyMatrixModel = {
                        matrixChoice: [],
                        matrixQuestion: []
                    };

                    for (var k = 0; k < this.listMatrix[i].measurement.length; k++) {
                        insertMatrix.matrixChoice.push({ text: this.listMatrix[i].measurement[k], number: k + 1 });
                    }

                    for (var k = 0; k < this.listMatrix[i].option.length; k++) {
                        insertMatrix.matrixQuestion.push({ question: this.listMatrix[i].option[k], number: k + 1 });
                    }

                    this.surveyInsert.question[i].matrixChoice = insertMatrix;
                }
                //Matching
                else if (this.surveyInsert.question[i].surveyQuestionTypeId == 2) {
                    let survey = this.surveyInsert.question[i]
                    let getMatchingChoiceCode = this.getMatchingChoiceCode
                    survey.matchingChoices.leftChoices.forEach(function (part, index, array) {
                        array[index].surveyMatchingChoiceCode = getMatchingChoiceCode(true, index + 1)
                    })
                    survey.matchingChoices.rightChoices.forEach(function (part, index, array) {
                        array[index].surveyMatchingChoiceCode = getMatchingChoiceCode(false, index + 1)
                    })
                }
            }

            this.setOutlet();

            await this.surveyAPI.saveSurvey(this.surveyInsert);
            this.$emit('update:successMessage', "Success to Add Data!");
            this.$emit('update:successMessageShow', true);
            this.Close();

        }

        setOutlet() {
            this.surveyInsert.outlet = [];
            for (var a = 0; a < this.selectedOutletCompleted.length; a++) {
                var outlet = <OutletViewModel>{
                    outletId: this.selectedOutletCompleted[a].outletId
                };

                this.surveyInsert.outlet.push(outlet);
            }
        }

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
            //this.AnswerImage[indexQuestion].FileUpload[index].formData.set($event.target.files, $event.target.files[0], $event.target.files[0].name);

            this.fileChange($event, indexQuestion, index);
        }

        handlerQuestion(indexQuestion: number, $event) {
            if ($event.target.files.length === 0) {
                return;
            }
            this.loadFileQuestion($event, indexQuestion, 0);
        }

        async loadFileQuestion($event, indexQuestion: number, index: number) {
            var reader = new FileReader();
            reader.onload = (e: Event) => {
                this.QuestionImage[indexQuestion].FileUpload[index].imageData = (<FileReader>e.target).result;
            }
            reader.readAsDataURL($event.target.files[0]);
            this.QuestionImage[indexQuestion].FileUpload[index].fileName = $event.target.files[0].name;

            await this.fileChangeQuestion($event, indexQuestion, index);
        }

        async fileChangeQuestion(e, indexQuestion: number, index: number) {
            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");

            this.QuestionImage[indexQuestion].FileUpload[index].formData.base64 = base64String;
            this.QuestionImage[indexQuestion].FileUpload[index].formData.fileName = file.name;
            this.QuestionImage[indexQuestion].FileUpload[index].formData.contentType = array.pop();
        }



        convertToBase64(file: File): Promise<string> {
            let promise = new Promise<string>((resolve, reject) => {
                let reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => {
                    resolve(reader.result.toString().split(',')[1]);
                }
                reader.onerror = error => {
                    reject(error);
                }
            });

            return promise;
        }

        async fileChange(e, indexQuestion: number, index: number) {
            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");

            this.AnswerImage[indexQuestion].FileUpload[index].formData.base64 = base64String;
            this.AnswerImage[indexQuestion].FileUpload[index].formData.fileName = e.target.files[0].name;
            this.AnswerImage[indexQuestion].FileUpload[index].formData.contentType = array.pop();
        }

        Close() {
            this.$emit('update:add', false);
        }
        // MATCHING
        matchingImageFormData: IMatchingImageFormData[] = []

        fileTypeErrorMsg = '';
        emptyChoiceErrorMsg = '';

        onTypeChange(e) {
            this.$forceUpdate()
        }

        async loadFileChoice(e, questionIndex: number, index: number, choice: SurveyMatchingChoiceFormModel, isLeft: boolean) {
            console.dir(`Question Number: ${questionIndex + 1}, Choice Number:${index}, Choice:\n`)
            console.table(choice)
            var fileInput = e.target.files || e.dataTransfer.files;
            if (!fileInput.length) return;

            this.fileTypeErrorMsg = "";
            var extension = fileInput[0].name.substring(fileInput[0].name.lastIndexOf('.')).toLowerCase();
            if (extension != '.jpg' && extension != '.png' && extension != '.jpeg') {
                this.fileTypeErrorMsg = "Please upload .jpg or .png image";
                return;
            }

            var reader = new FileReader();
            reader.onload = (e) => {
                let result = reader.result
                if (isLeft) {
                    this.matchingImageFormData[questionIndex].data[index].leftImage = result
                } else {
                    this.matchingImageFormData[questionIndex].data[index].rightImage = result
                }
                console.table(this.matchingImageFormData[questionIndex].data[index])
            }
            reader.readAsDataURL(e.target.files[0]);
            let inputFile = (<HTMLInputElement>e.target).files[0];
            let base64String = await this.convertToBase64(inputFile);
            let file = e.target.files[0];
            let fileNames: string[] = file.name.split(".")
            let fileExtension: string = fileNames[fileNames.length - 1];
            let fileContent: FileContent = {
                base64: base64String,
                fileName: file.name,
                contentType: fileExtension
            };
            choice.surveyMatchingChoiceCode = this.getMatchingChoiceCode(isLeft, index + 1);
            choice.imageUpload = fileContent;
            this.$forceUpdate();
        }
        getMatchingChoiceCode(isLeft: boolean, number: number): string {
            var numberFormat = function (number: number, width: number) {
                return new Array(width + 1 - (number + '').length).join('0') + number;
            }
            let codeChar: string = isLeft ? 'A' : 'B';
            let string = numberFormat(number, 2) + codeChar
            return string;
        }
        removeMatchingRow(question: SurveyQuestion, index: number) {
            if (question.matchingChoices.leftChoices.length <= 2) {
                alert('There should be at least 2 matching question pairs')
                return
            }
            question.matchingChoices.leftChoices.splice(index, 1);
            question.matchingChoices.rightChoices.splice(index, 1);
            this.matchingImageFormData[question.questionNumber - 1].data.splice(index, 1)
            this.$forceUpdate()
        }


        backPage() {
            window.history.back();
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
        formData: IFileContent;
        fileName: string;
    }
    interface TebakGambar {
        FileUpload: FileUploadImage[]
    }

    interface ListGrouping {
        ListOption: any[],
        GroupName: string
    }

    interface IMatrixContainer {
        question: string;
        measurement: string[];
        option: string[];
    }
    interface IMatchingImageFormData {
        data: IMatchingImageFormDatum[]
    }
    interface IMatchingImageFormDatum {
        leftImage: string | ArrayBuffer | undefined,
        rightImage: string | ArrayBuffer | undefined
    }
</script>
