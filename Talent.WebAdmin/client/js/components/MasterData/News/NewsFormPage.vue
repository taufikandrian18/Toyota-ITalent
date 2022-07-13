<template>
    <div>
        <div class="row">
            <loading-overlay :loading="isBusy"></loading-overlay>

            <div class="col col-md-12">
                <!--TITLE-->
                <h3 v-if="isView == true">
                    <fa icon="database"></fa>
                    Master Data > News >
                    <span class="talent_font_red">Detail News</span>
                </h3>
                <h3 v-if="isInsert == true">
                    <fa icon="database"></fa>
                    Master Data > News >
                    <span class="talent_font_red">Add News</span>
                </h3>
                <h3 v-if="isEdit == true">
                    <fa icon="database"></fa>
                    Master Data > News >
                    <span class="talent_font_red">Edit News</span>
                </h3>

                <br />

                <!--Add-->
                <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                    <h4>News Information</h4>
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
                                    <label>
                                        News Title
                                        <span class="talent_font_red" v-if="isView == false">*</span>
                                    </label>
                                    <div class="input-group">
                                        <input class="form-control" name="NewsTitle" type="text" v-model="formModel.newsTitle"
                                               :disabled="isView == true" maxlength="250"/>
                                    </div>
                                    <br />
                                    <label>
                                        News Category
                                        <span class="talent_font_red" v-if="isView == false">*</span>
                                    </label>
                                    <multiselect v-model="selectedNewsCategory"
                                                 :options="newsCategoryList"
                                                 :searchable="true"
                                                 :allow-empty="true"
                                                 label="name"
                                                 track-by="id"
                                                 name="Category"
                                                 @input="setNewsCategory()"
                                                 :disabled="isView == true"
                                                 >
                                    </multiselect>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        News Description
                                    </label>
                                    <textarea class="form-control"
                                              name="description"
                                              style="height:150px;overflow-y:scroll;"
                                              v-model="formModel.description"
                                              :disabled="isView == true"
                                              maxlength="5000"
                                              ></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <div class="row">
                        <div class="col-md-12">
                            <div class="row justify-content-between">
                                <div class="col-md-6">
                                    <label>
                                        News Link
                                    </label>
                                    <br />
                                    <div class="input-group">
                                        <input class="form-control" name="NewsLink" type="text" v-model="formModel.newsLink" :disabled="isView == true" maxlength="1000"/>
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <label>
                                        Author
                                        <span class="talent_font_red" v-if="isView == false">*</span>
                                    </label>
                                    <div class="input-group">
                                        <input class="form-control" name="Author" type="text" v-model="formModel.author"
                                               :disabled="isView == true" maxlength="120"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <div class="row justify-content-between">
                        <div class="col-md-6">
                            <div class="d-flex">
                                <div class="w-50">
                                    <img :src="imageFileModel.fileUrl.length ? imageFileModel.fileUrl : '/upload-image1.png'"
                                    alt="Alternate Text"
                                    class="" 
                                    style="width: 90%; object-fit:cover;"
                                     />
                                </div>
                                <div class="w-50">
                                    <h5>File Upload</h5>
                                    <label>
                                        Upload Image
                                        <span class="talent_font_red" v-if="isView == false">*</span>
                                    </label>
                                    <div class="custom-file overflow-hidden">
                                        <input type="file"
                                                class="custom-file-input"
                                                id="customImage"
                                                @change="onAddImageChange($event)" 
                                                :disabled="isView == true"
                                                name="Upload Image"
                                                />
                                        <label class="custom-file-label"
                                                for="customImage"
                                                :disabled="isView == true">
                                                {{imageFileModel.name == null ? 'Choose File' : imageFileModel.name}}
                                        </label>
                                    </div>
                                    <div class="col-md-4 font_size_12">
                                        *Image Size 300x300
                                        <br />*Max File Size 5MB
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label>Is Downloadable <span class="talent_font_red" v-if="isView == false">*</span></label><br />
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="typeDownload" :value="true" id="yesAnswer" v-model="formModel.isDownloadable" :disabled="isView == true">
                                <label class="form-check-label" for="yesAnswer">Yes</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="typeDownload" :value="false" id="noAnswer" v-model="formModel.isDownloadable" :disabled="isView == true">
                                <label class="form-check-label" for="noAnswer">No</label>
                            </div>
                        </div>
                    </div>
                    <br />

                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
                        <h4>File Upload</h4>

                        <!-- <div v-for="(file,index) in fileUpload"></div> -->
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row justify-content-between">
                                    <div class="col-md-12">
                                        <div class="row align-items-center row justify-content-between">
                                            <div class="col-md-6">
                                                <label>
                                                    Upload News Picture
                                                    <span class="talent_font_red" v-if="isView == false">*</span>
                                                </label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="row justify-content-between">
                                                    <div class="col-md-6 overflow-hidden">
                                                        <div class="custom-file">
                                                            <input type="file"
                                                                   class="custom-file-input"
                                                                   id="customFile"
                                                                   @change="onAddFileChange($event)" 
                                                                   :disabled="isView == true"
                                                                   name="Upload File News"
                                                                   />
                                                            <label class="custom-file-label"
                                                                for="customFile"
                                                                :disabled="isView == true">
                                                                {{fileFileModel.name == null ? 'Choose File' : fileFileModel.name}}
                                                            </label>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-6">
                                                        *Max File Size 5MB
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="py-4">
                            <img :src="fileFileModel.fileUrl.length ? fileFileModel.fileUrl : '/upload-image2.png'"
                                 alt="Alternate Text"
                                 class="img-fluid w-100" />
                        </div>
                    </div>
                    <br />
                    <div class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow" >
                        <div class="row">
                            <div class="col-md-12">
                                <div class="d-flex align-items-end flex-column">
                                    <div>
                                        <button class="btn talent_bg_red talent_font_white" @click.prevent="closePage()">Close</button>
                                        <button class="btn talent_bg_lightgreen talent_font_white" v-if="isView == false"
                                                @click.prevent="saveData('save')" :disabled="sendButtonLoading"
                                                >Save</button>
                                        <button class="btn talent_bg_darkblue talent_font_white" v-if="isView == false"
                                                @click.prevent="saveData('submit')" :disabled="sendButtonLoading"
                                                >Submit</button>
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
    import { Sort } from '../../../class/Sort';
    import { BlobService } from '../../../services/BlobService';
    import { NewsService, MasterNewsFilterModel, DropdownService, MasterNewsViewModel, MasterNewsFormModel, DropDownModel, FileUploadService, FileModel, UserAccessCRUD, UserPrivilegeSettingsService, FileContent } from '../../../services/NSwagService';

    @Component({
        props: ['type', 'theId', 'isShowMessage', 'theShowMessage', 'isBusy'],
        mounted: async function (this: NewsFormPage) {
            await this.getAccess();
            this.setTypeForm()
            await this.fetching();
        }
    })
    export default class NewsFormPage extends Vue {
        props: any = {
            placeholder: '',
            readonly: true
        };

        isEdit: boolean = false;
        isInsert: boolean = false;
        isView: boolean = false;
        isBusy:boolean = true;

        //loading for 
        sendButtonLoading: boolean = false;
        isLoading: boolean = false;

        viewType: string = "view";

        type: string;
        theId: number;

        //nama variable buat form data
        formDataName: string = "file";

        fileUpload: FileContent = null;
        thumbnailUpload: FileContent = null;

        //variable
        formModel: MasterNewsFormModel = {
            newsId: null,
            newsTitle: "",
            newsLink: "",
            author: "",
            description: "",
            newsCategoryId: null,
            isDownloadable: true,
            thumbnailBlobId: "",
            fileBlobId: "",
        }

        fileUploadUrl: string = "";
        imageUploadUrl: string = "";
        imageName: string = "";
        fileName: string = "";
        isImageUpload: boolean = false;
        isFileUpload: boolean = false;

        imageFileModel: FileModel = {
            name: "",
            mime: "",
            fileUrl: ""
        };
        fileFileModel: FileModel = {
            name: "",
            mime: "",
            fileUrl: ""
        };
        crud: UserAccessCRUD = {
            create: false, delete: false, read: false, update: false
        }

        imageFormData: FormData = new FormData();
        fileFormData: FormData = new FormData();

        //service
        newsServiceMan: NewsService = new NewsService();
        dropdownServiceMan: DropdownService = new DropdownService();
        privilegeApi: UserPrivilegeSettingsService = new UserPrivilegeSettingsService();

        //select variable
        selectedNewsCategory: DropDownModel = {
            id: 0,
            name: ""
        };

        //list showed variable
        newsList: MasterNewsViewModel[] = [];
        newsCategoryList: DropDownModel[] = [];

        async fetching() {
            this.isLoading = true;
            this.isBusy = true;

            this.fileUpload = null;
            this.thumbnailUpload = null;

            this.setTypeForm();
            await this.getCategoryListData();
            await this.getDataNews();
        }

        async getAccess() {
            var data = await this.privilegeApi.crudAccessPage("NWS");
            this.crud = data
            if (data.create == false && this.type.toLowerCase().search("add") != -1) {
                window.location.href = `/MasterData/News`;
            }
            else if (data.update == false && this.type.toLowerCase().search("update") != -1) {
                window.location.href = `/MasterData/News`;
            }
            else if (data.read == false && this.type.toLowerCase().search("detail") != -1) {
                window.location.href = `/MasterData/News`;
            }
            else if (data.read == false && data.create == false && data.update == false) {
                window.location.href = `/MasterData/News`;
            }
        }

        // --------------- service / main function --------------------

        setTypeForm() {
            if (this.type != null && this.type != undefined) {
                if (this.type.toLowerCase().search("add") != -1) {
                    this.isInsert = true;
                    this.isEdit = false;
                    this.isView = false;
                }
                else if (this.type.toLowerCase().search("update") != -1) {
                    this.isInsert = false;
                    this.isEdit = true;
                    this.isView = false;
                }
                else if (this.type.toLowerCase().search("detail") != -1) {
                    this.isInsert = false;
                    this.isEdit = false;
                    this.isView = true;
                } else {
                    this.isInsert = false;
                    this.isEdit = false;
                    this.isView = true;
                }
            }
        }

        async getDataNews() {
            if (this.isView == true || this.isEdit == true) {
                try{
                    let model = await this.newsServiceMan.getDetail(this.theId);
                    this.setDataIntoForm(model);
                }catch{
                    this.isBusy = false;
                }
            }
            this.isBusy = false;
            this.isLoading = false;
        }

        async setDataIntoForm(model: MasterNewsFormModel) {

            this.formModel.newsId = this.theId;
            this.formModel.newsTitle = model.newsTitle;
            this.formModel.newsLink = model.newsLink;
            this.formModel.author = model.author;
            this.formModel.description = model.description;
            this.formModel.newsCategoryId = model.newsCategoryId;
            this.formModel.isDownloadable = model.isDownloadable;
            this.formModel.thumbnailBlobId = model.thumbnailBlobId;
            this.formModel.fileBlobId = model.fileBlobId;

            //set into multi select
            let findedModel = this.newsCategoryList.find(category => category.id == model.newsCategoryId);
            this.selectedNewsCategory.id = findedModel.id;
            this.selectedNewsCategory.name = findedModel.name;

            //set file
            if (model.thumbnailBlobId != null) {
                let getDetailImage = await BlobService.getImageDetail(model.thumbnailBlobId);
                this.imageFileModel = {
                    name: getDetailImage.name,
                    mime: getDetailImage.mime,
                    fileUrl: getDetailImage.fileUrl
                };
            }
            if (model.fileBlobId != null) {
                let getDetailFile = await BlobService.getImageDetail(model.fileBlobId);
                this.fileFileModel = {
                    name: getDetailFile.name,
                    mime: getDetailFile.mime,
                    fileUrl: getDetailFile.fileUrl
                };
            }

            this.isLoading = false;
        }

        async getFileDetail(id: string) {
            let fileDetail = BlobService.getImageDetail(id);
        }

        //UPLOADIMAGE
        UploadFile(formData: FormData) {
            return BlobService.uploadService(formData);
        }

        async getCategoryListData() {
            let getList = await this.dropdownServiceMan.getNewsList();
            this.newsCategoryList = getList;
        }

        validateAllInput() {
            let result = true;
            //
            if (this.formModel.newsTitle == null || this.formModel.newsTitle == undefined || this.formModel.newsTitle == "") {
                this.$validator.errors.add({
                    field: 'News Title',
                    msg: 'News Title is empty, please fill title field'
                });
                result = false;
            }

            if (this.formModel.newsTitle.length > 250) {
                this.$validator.errors.add({
                    field: 'News Title length',
                    msg: 'Length of News Title can not more than 250 Characters.'
                });
                result = false;
            }

            //
            if (this.formModel.newsCategoryId <= 0) {
                this.$validator.errors.add({
                    field: 'News Category',
                    msg: 'News Category is empty, please fill Category field'
                });
                result = false;
            }
            //
            if (this.formModel.author == null || this.formModel.author == undefined || this.formModel.author == "") {
                this.$validator.errors.add({
                    field: 'author',
                    msg: 'Author is empty, please fill Author field'
                });
                result = false;
            }

            if (this.formModel.author.length > 120) {
                this.$validator.errors.add({
                    field: 'author length',
                    msg: 'Length of Author can not more than 120 Characters.'
                });
                result = false;
            }

            if (this.formModel.newsLink.length > 1000) {
                this.$validator.errors.add({
                    field: 'link length',
                    msg: 'Length of News Link can not more than 1000 Characters.'
                });
                result = false;
            }

            if (this.formModel.description.length > 5000) {
                this.$validator.errors.add({
                    field: 'description length',
                    msg: 'Length of News Description can not more than 5000 Characters.'
                });
                result = false;
            }

            if (this.imageFileModel.fileUrl == null || this.imageFileModel.fileUrl == undefined || this.imageFileModel.fileUrl == "") {
                this.$validator.errors.add({
                    field: 'Image Empty',
                    msg: 'Image is empty, please upload image'
                });
                result = false;
            }

            if (this.fileFileModel.fileUrl == null || this.fileFileModel.fileUrl == undefined || this.fileFileModel.fileUrl == "") {
                this.$validator.errors.add({
                    field: 'File Empty',
                    msg: 'File is empty, please upload File'
                });
                result = false;
            }
            return result
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

            var tempId1:string = "";
            var tempId2:string = "";
            //upload file
            if (this.isView == false) {
                if (result == true) {
                    if (this.isImageUpload == true) {
                        this.formModel.thumbnailUpload = this.thumbnailUpload;
                    }
                    if (this.isFileUpload == true) {
                        this.formModel.fileUpload = this.fileUpload;

                    }
                }
            }
            
            //insert or delete
            if (this.isView == false) {
                if (result == true) {
                    if (this.isInsert == true && this.isEdit == false) {
                        let responseResult = await this.newsServiceMan.insert(this.formModel,type);
                        this.notif(responseResult);
                    }
                    else if (this.isEdit == true && this.isInsert == false) {
                        let responseResult = await this.newsServiceMan.update(this.formModel,type);
                        this.notif(responseResult);
                    }
                }
                this.$validator.reset();
            }
            this.sendButtonLoading = false; 
            this.isBusy = false;
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
                this.isBusy = false;
            }
        }

        async closePage() {
            if(this.type.toLowerCase() == 'fromoutside'){
                window.history.back();
            }
            else{
                if(this.type)
                this.isBusy = false;
                this.resetAll();
                this.$emit('update:type', "view");
            }
        }

        // ---------------- side function -------------------

        async setNewsCategory() {
            this.formModel.newsCategoryId = this.selectedNewsCategory.id;
        }

        resetAll() {

            this.formModel = {
                newsId: null,
                newsTitle: "",
                newsLink: "",
                author: "",
                description: "",
                newsCategoryId: null,
                isDownloadable: null,
                thumbnailBlobId: "",
                fileBlobId: "",
            }

            this.fileUploadUrl = "";
            this.imageUploadUrl = "";
            this.imageName = "";
            this.fileName = "";

            this.imageFileModel = {
                name: "",
                mime: "",
                fileUrl: ""
            };
            this.fileFileModel = {
                name: "",
                mime: "",
                fileUrl: ""
            };

            this.imageFormData = new FormData();
            this.fileFormData = new FormData();

            this.selectedNewsCategory = {
                id: 0,
                name: ""
            };

            this.fileUpload = null;
            this.thumbnailUpload = null;
        }

        async convertToBase64(file: File): Promise<string> {
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

        async onAddImageChange(e) {
            var fileInput = e.target.files || e.dataTransfer.files;
            if (!fileInput.length) return;

            let fileType = true;
            let imageSize = true;

            let file = e.target.files[0];

            let array = file.name.split(".");

            var extension = array.pop();
            if (extension != 'jpg' && extension != 'png' && extension != 'jpeg') {
                fileType = false;
                this.$validator.errors.add({
                    field: 'Image Type',
                    msg: 'Please input an image file (jpg/png/jpeg)'
                });
            }

            if (fileInput[0].size > 5048576) {
                imageSize = false;
                this.$validator.errors.add({
                    field: 'Image Size',
                    msg: 'Maximum Image File Size is 5 MB'
                });
            }

            //get all this variable
            if (fileType && imageSize) {
                let tempVariable = this;
                let reader = new FileReader();
                reader.readAsDataURL(fileInput[0]);
                reader.onload = function () {
                    var image = new Image();
                    image.src = reader.result as string;
                    image.onload = function () {
                        tempVariable.imageFileModel.fileUrl = reader.result as string;
                        tempVariable.imageFileModel.name = fileInput[0].name;
                        tempVariable.isImageUpload = true;
                        tempVariable.imageFormData.append(e.target.files, e.target.files[0], e.target.files[0].name);
                    };
                };

                let inputFile = (<HTMLInputElement>e.target).files[0];

                let base64String = await this.convertToBase64(inputFile);

                this.thumbnailUpload = {
                    base64: base64String,
                    fileName: fileInput[0].name,
                    contentType: extension
                };
            }
        }

        async onAddFileChange(e) {
            var fileInput = e.target.files || e.dataTransfer.files;
            if (!fileInput.length) return;

            let fileType = true;
            let fileSize = true;
            
            if (fileInput[0].size > 5048576) {
                fileSize = false;
                this.$validator.errors.add({
                    field: 'Image Size',
                    msg: 'Maximum Image File Size is 5 MB'
                });
            }

            //get all this variable
            if (fileType && fileSize) {
                let tempVariable = this
                let reader = new FileReader();
                reader.readAsDataURL(fileInput[0]);
                reader.onload = function () {
                    var image = new Image();
                    image.src = reader.result as string;
                    image.onload = function () {

                        tempVariable.fileFileModel.fileUrl = reader.result as string;
                        tempVariable.fileFileModel.name = fileInput[0].name;
                        tempVariable.isFileUpload = true;
                        tempVariable.fileFormData.append(e.target.files, e.target.files[0], e.target.files[0].name);
                    };
                };

                let inputFile = (<HTMLInputElement>e.target).files[0];

                let base64String = await this.convertToBase64(inputFile);

                let file = e.target.files[0];

                let array = file.name.split(".");

                this.fileUpload = {
                    base64: base64String,
                    fileName: fileInput[0].name,
                    contentType: array.pop()
                };
            }
        }
    }
</script>