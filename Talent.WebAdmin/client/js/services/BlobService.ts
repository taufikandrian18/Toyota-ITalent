import Axios from 'axios';

export let BlobService = {
    uploadService(file) {
        return Axios.post('/api/v1/blob/upload-file', file)
            .then(x => x.data);
    },

    getImageUrl(fileName: string, fileExtension: string) {
        return Axios.get('/api/v1/blob/get-image-url/' + fileName + '/' + fileExtension).then(x => x.data);
    },

    getImageDetail(id: string) {
        return Axios.get('/api/v1/blob/get-image-detail/' + id).then(x => x.data);
    },

    getBlobById(id: string) {
        return Axios.get('/api/v1/blob/get-blob-by-id/' + id).then(x => x.data);
    },
    async getFilePDF(id: string){

        return Axios({
            url: '/api/v1/blob/get-file-stream-url/' + id,
            method: 'GET',
            responseType: 'blob', // important
        }).then((response) => {

            let file = new Blob([response.data], { type: 'application/pdf' });            
            var fileURL = URL.createObjectURL(file);

            return fileURL;
        });
    },
    async getFileVideo(id: string){

        return Axios({
            url: '/api/v1/blob/get-file-stream-url/' + id,
            method: 'GET',
            responseType: 'blob', // important
        }).then((response) => {

            let file = new Blob([response.data], { type: 'video/mp4' });            
            var fileURL = URL.createObjectURL(file);

            return fileURL;
        });
    },
    /*
     * delete file
     * (filename = nama blob yang ada di minio, diambil dari blobid tabel blob)
     * (fileExtension = nama ekstensi file yang akan menjadi folder/bucket minio)
     * */
    deteleFile(fileName: string, fileExtension: string) {
        return Axios.delete('/api/v1/blob/delete-file/' + fileName + '/' + fileExtension);
    }
}