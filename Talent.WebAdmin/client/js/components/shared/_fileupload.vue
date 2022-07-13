<template>
    <div>
        <div v-if="isNoImage === true">
            <div class="custom-file">
                <input type="file" class="custom-file-input" id="customFile" @change="handler" :disabled="isDisabled === true" />
                <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
            </div>
        </div>

        <div class="col-md-12" v-if="layout == 'Horizontal'">
            <div class="row">
                <div class="col-md-12">
                    <div class="row justify-content-between">
                        <div class="col-md-4 h-100">
                            <img class="h-100 w-100" :src="imageData.length ? imageData : '/upload-image2.png'" for="customFile" />
                        </div>
                        <div class="col-md-8">
                            <h5>File Upload</h5>
                            <label>Upload Image<span class="talent_font_red">*</span></label>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="row justify-content-between">
                                        <div class="col-md-8">
                                            <div class="custom-file">
                                                <input type="file" class="custom-file-input" id="customFile" @change="handler" accept="" :disabled="isDisabled === true" />
                                                <label class="custom-file-label talent_textshorten " for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
                                            </div>
                                        </div>
                                        <div class="col-md-4 font_size_12">
                                            *Image Size 300x300<br />
                                            *Max File Size 5MB
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div v-if="layout == 'Vertical'">
            <img class="h-100 w-100" :src="imageData.length ? imageData : '/upload-image1.png'" />
            <div class="custom-file">
                <input type="file" class="custom-file-input" id="customFile" @change="handler" />
                <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
            </div>
        </div>

        <div v-if="layout == 'TaskVertical'">
            <img class="h-100 w-100" :src="imageData.length ? imageData : '/upload-image1.png'" />
            <div class="custom-file">
                <input type="file" class="custom-file-input" id="customFile" @change="handler" :disabled="isStoryLineType != true || layoutType != 2 || isDisabled == true" />
                <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
            </div>
        </div>

        <div v-if="layout == 'TaskHorizontal'">
            <img class="h-100 w-100" :src="imageData.length ? imageData : '/upload-image2.png'" />
            <div class="custom-file">
                <input type="file" class="custom-file-input" id="customFile" @change.prevent="handler" :disabled="isStoryLineType != true || layoutType != 1 || isDisabled == true" />
                <label class="custom-file-label talent_textshorten talent_noroundborder_top" for="customFile">{{imageName.length ? imageName : 'Choose File'}}</label>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import IFileContent from '../../models/IFileContent';

    @Component({
        props: ['layout', 'formData', 'fileName', 'isStoryLineType', 'layoutType', 'mountImage', 'mountName', 'updatedImage', 'updatedName', 'disable', 'noImage', 'delete'],
        mounted: function (this: UploadFile) {
            if (this.mountImage != null) {
                this.imageData = this.mountImage;
            }

            if (this.mountName != null) {
                this.imageName = this.mountName;
            }

            if (this.disable != null) {
                this.isDisabled = this.disable;
            }

            if (this.noImage != null) {
                this.isNoImage = this.noImage;
            }
        },

        updated: function (this: UploadFile) {
            if (this.updatedImage != null) {
                this.imageData = this.updatedImage;
            }

            if (this.updatedName != null) {
                this.imageName = this.updatedName;
            }
        },
    })
    export default class UploadFile extends Vue {
        layout: string;
        isStoryLineType: boolean;
        layoutType: number;

        imageData: any = [];
        imageName: string = '';

        mountImage: string;
        mountName: string;

        noImage: boolean;
        isNoImage: boolean = false;

        updatedImage: string;
        updatedName: string;

        disable: boolean;
        isDisabled: boolean = false;

        //Upload File
        //formData: FormData;
        formData: IFileContent = {
            base64: "",
            contentType: "",
            fileName: "",
        };
        fileName: string;

        handler($event) {
            if ($event.target.files.length === 0) {
                //this.imageData = '';
                //this.imageName = '';
                return;
            }
            this.$emit('update:updatedImage', null);
            this.$emit('update:updatedName', null);

            this.loadFile($event);
            this.fileChange($event);
        }

        //fileChange(e) {
        //    this.formData.set(e.target.files, e.target.files[0], e.target.files[0].name);
        //    this.$emit('update:formData', this.formData);
        //    this.$emit('update:fileName', e.target.files[0].name);
        //    //this.$emit('update:delete', false);
        //    console.log("masuk")
        //}

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

        async fileChange(e) {
            //this.formData.set(e.target.files, e.target.files[0], e.target.files[0].name);

            let inputFile = (<HTMLInputElement>e.target).files[0];

            let base64String = await this.convertToBase64(inputFile);

            let file = e.target.files[0];

            let array = file.name.split(".");
            //let lastIndex = array.length - 1;

            this.formData.base64 = base64String;
            this.formData.fileName = e.target.files[0].name;
            this.formData.contentType = array.pop();

            this.$emit('update:formData', this.formData);
            this.$emit('update:fileName', e.target.files[0].name);
            //this.$emit('update:delete', false);
        }

        loadFile($event) {
            var reader = new FileReader();
            reader.onload = (e: Event) => {
                this.imageData = (<FileReader>e.target).result;
            }
            reader.readAsDataURL($event.target.files[0]);
            this.imageName = $event.target.files[0].name;
        }
    }

</script>
