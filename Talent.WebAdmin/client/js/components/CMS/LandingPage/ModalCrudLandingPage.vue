<template>
    <div class="modal fade" id="modalCrudLandingPage" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 d-flex">
                        Landing Page Slide
                    </div>
                </div>

                <div class="modal-content px-4 py-4">
                    <div class="row p-0 m-0">
                        <div class="col-md-12 p-0 m-0">
                            <div class="alert alert-danger alert-dismissible fade show" role="alert" v-if="message !== ''">
                                {{ message }}
                                <button type="button" class="close" data-dismiss="alert" aria-label="Close" @click="handleMessageClose">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>
                        </div>
                        <div class="col-md-12 p-0 m-0">
                            <video v-if="image.contentType === 'mp4' || data.onBoardingFileType == 'mp4'" ref="myVideo" id="myVideo" class="w-100 video" controls>
                                <source :src="selectedFile ? selectedFile : data.onBoardingFileURL ? data.onBoardingFileURL : '/upload-image2.png'" type="video/mp4">
                            </video>
                            <img  v-else class="image w-100" :src="selectedFile ? selectedFile : data.onBoardingFileURL ? data.onBoardingFileURL : '/upload-image2.png'" alt="" onerror="this.onerror=null;this.src='/upload-image2.png';">
                        </div>
                        <div class="col-md-12 d-flex justify-content-center p-0 m-0 mt-4">
                            <div class="col-sm-6 d-flex p-0">
                                <div>
                                    <label for="inputImage" class="button btn-primary px-3 py-1 rounded">Browse Image</label>
                                    <input type="file" name="inputImage" id="inputImage" ref="inputImage" hidden @change="handleImage" accept="image/gif,image/jpeg, image/jpg, image/png">
                                </div>
                                <div class="text-secondary text-sm pl-2">
                                    <span>*Recommended File: GIF, JPEG, JPG, PNG</span>
                                    <br>
                                    <span>*Max File Size 5MB</span>
                                </div>
                            </div>
                            <div class="col-sm-6 d-flex justify-content-end">
                                <div>
                                    <label for="inputVideo" class="button btn-primary px-3 py-1 rounded">Browse Video</label>
                                    <input type="file" name="inputVideo" id="inputVideo" ref="inputVideo" hidden @change="handleVideo" accept="video/mp4">
                                </div>
                                <div class="text-secondary text-sm pl-2">
                                    <span>*Recommended File: MP4</span>
                                    <br>
                                    <span>*Max File Size 100MB</span>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 mt-3 p-0">
                            Description <span class="talent_font_red">*</span>
                            <wysiwyg 
                                class="w-100" 
                                v-model="image.description" 
                                :options="{
                                    hideModules: {image: true, table: true, removeFormat: true, code: true }
                                }"
                                @change="handleChangeEditor"/>
                        </div>
                        <div class="col-md-12 mt-2 p-0 text-right">
                            {{ textLength }} / 500
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <div class="w-100 d-flex">
                        <ol>
                            <li class="text-danger" v-for="error in errs" :key="error.id">{{ error.message }}</li>
                        </ol>
                    </div>
                    <div class="w-100 d-flex justify-content-end">
                        <button type="button" class="btn btn-sm btn-danger mr-3" data-dismiss="modal">
                            Close
                        </button>
                        <button type="button" class="btn btn-sm btn-success mr-3" data-dismiss="modal" @click.prevent="handleSave" :disabled="!isValid">
                            Save
                        </button>
                        <button type="button" class="btn btn-sm btn-info" data-dismiss="modal" @click.prevent="handleSubmit" :disabled="!isValid">
                            Submit
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import $ from 'jquery'

export default {
    props: {
        data: {
            type: Object,
            default: () => {}
        }
    },
    data(){
        return {
            image: {
                base64: '',
                fileName: '',
                contentType: '',
                description: '',
            },
            selectedFile: null,
            message: '',
            textLength: 0
        }
    },
    watch: {
        data(newData) {
            if(newData.description) {
                this.image.description = newData.description
                this.image.contentType = newData.onBoardingFileType
                if(this.$refs.myVideo) {
                    this.$refs.myVideo.load()
                }
            } else {
                this.reset()
            }
        },
        selectedFile() {

        }
    },
    mounted() {
        $('.editr--toolbar .dashboard label').click(function (event) {$(event.target).focus()});
    },
    computed: {
        isValid() {
            return this.image.description != '' && (this.image.base64 != '' || this.data.onBoardingFileURL) && this.textLength <= 500
        },
        errs() {
            const errs = []
            if(!(this.image.base64 != '' || this.data.onBoardingFileUR)) {
                errs.push({
                    id: 'file',
                    message: 'Please input file'
                })
            }
            if(this.image.description == '') {
                errs.push({
                    id: 'empty-description',
                    message: 'Please input description'
                })
            }
            if(this.image.description.length > 500) {
                errs.push({
                    id: 'long-description',
                    message: 'Description too long. Max 500 character'
                })
            }
            return errs
        }
    },
    methods: {
        reset() {
            this.image = {
                base64: '',
                fileName: '',
                contentType: '',
                description: '',
            }
            this.selectedFile = null
        },
        handleSave() {
            this.$emit('save', this.image)
            this.image = {
                base64: '',
                fileName: '',
                contentType: '',
                description: '',
            }
            $('#modalCrudLandingPage').modal('hide')
        },
        handleSubmit() {
            this.$emit('submit', this.image)
            this.image = {
                base64: '',
                fileName: '',
                contentType: '',
                description: '',
            }
            $('#modalCrudLandingPage').modal('hide')
        },
        async handleImage(e){
            const selectedImage = e.target.files[0]
            if(selectedImage.size > 5048576) {
                this.message = 'File must lower than 5MB. Please Select another file'
                this.image.base64 = ''
                this.image.contentType = ''
                this.image.fileName = ''
                return
            }
            let array = selectedImage.name.split(".");
            let extension = array.pop();
            const res = await this.getBase64(selectedImage)
            this.image.base64 = res
            this.image.fileName = selectedImage.name
            this.image.contentType = extension
            this.$refs.inputImage.value = null
        },
        async handleVideo(e){
            const selectedImage = e.target.files[0]
            if(selectedImage.size > 104857600) {
                this.message = 'File must lower than 100MB. Please Select another file'
                this.image.base64 = ''
                this.image.contentType = ''
                this.image.fileName = ''
                return
            }
            let array = selectedImage.name.split(".");
            let extension = array.pop();
            const res = await this.getBase64(selectedImage)
            this.image.base64 = res
            this.image.fileName = selectedImage.name
            this.image.contentType = extension
            this.$refs.inputVideo.value = null

                if(this.$refs.myVideo) {
                    this.$refs.myVideo.load()
                }
        },
        getBase64(file) {
            return new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => {
                let encoded = reader.result.toString().replace(/^data:(.*,)?/, '');
                if ((encoded.length % 4) > 0) {
                    encoded += '='.repeat(4 - (encoded.length % 4));
                }
                this.selectedFile = reader.result
                resolve(encoded);
                };
                reader.onerror = error => reject(error);
            });
        },
        handleMessageClose() {
            this.message = '';
        },
        handleChangeEditor () {
            const el = document.getElementsByClassName('editr--content')
            
            this.textLength = el.length > 0 ? el[0].innerText.length : 0
        }
    }
}
</script>

<style>

</style>