import Axios from 'axios';

export let FileReportService = {
    surveyReportDownloadExcelService(surveyId: number) {
        return Axios({
            url: '/api/v1/survey-report/generate-excel/' + surveyId,
            method: 'GET',
            responseType: 'blob', // important
        }).then((response) => {
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'Report Inquiry.xlsx');
            document.body.appendChild(link);
            link.click();
        });
    },

    async trainingScoreReportDownload(trainingId: number) {
        return await Axios({
            url: '/api/v1/training-score-report/download-training-score-report/' + trainingId,
            method: 'GET',
            responseType: 'blob', // important
        }).then((response) => {
            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', 'Training Score Report.xlsx');
            document.body.appendChild(link);
            link.click();
        });
    },

}