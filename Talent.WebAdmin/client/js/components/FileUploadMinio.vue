<template>
    <div>
        <div v-if="msg != ''" class="bg-warning">{{ this.msg }}</div>
        <div class="form-group row">
            <div class="col-sm-3">
                <label class="control-label">
                    Upload File via MinIO
                </label>
            </div>
            <div class="col-sm-9" title="pilih file(bisa lebih dari 1 file) yang berkaitan dengan aset">
                <div class="col-md-12 row">
                    <input type="file" name ="File" @change="fileChange" />
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 text-right">
                <button type="button" @click.prevent="uploadFile()">Upload</button>
            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { BlobService } from '../services/BlobService';

    @Component({

    })
    export default class FileUploadMinio extends Vue {
        msg: string = "";
        
        formData: FormData = new FormData();

        fileChange(e) {
            if (!e.target.files.length) this.msg = "!e.target.files.length";

            Array
                .from(Array(e.target.files.length).keys())
                .map(x => {
                    this.formData.append(e.target.files, e.target.files[x], e.target.files[x].name);
                });
        } 

        async uploadFile() {
            let response = await BlobService.uploadService(this.formData);
            this.msg = response.statusText;
            this.msg = response.data;
        }
    }
</script>
