import { throwException } from "../common";
import { cleanQueryToString, queryToString } from "../helper";
import Axios from 'axios';
import FileSaver from 'file-saver';

export class ReportService {
  private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

  constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
    this.http = http ? http : <any>window;
    this.baseUrl = baseUrl ? baseUrl : "";
  }

  // Skill Check
  getCompetencyTrackingReport(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/report-tracking/competency?${query}`;
    url_ = url_.replace(/[?&]$/, "");

    return Axios({
        url: url_,
        method: 'GET',
        responseType: 'blob', // important
    }).then((response) => {
      FileSaver.saveAs(response.data, 'Learning Progress Report.xlsx');
    });
  }
  // END Skill Check
  // KPI
  getKPITrackingReport(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/report-tracking/kpi?${query}`;
    url_ = url_.replace(/[?&]$/, "");

    return Axios({
        url: url_,
        method: 'GET',
        responseType: 'blob', // important
    }).then((response) => {
      FileSaver.saveAs(response.data, 'Learning KPI Report.xlsx');
    });
  }
  // END KPI

  // Report Tracking
  getProgressTrackingReport(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/tracking-progress-report/export-list-progress-tracking?${query}`;
    url_ = url_.replace(/[?&]$/, "");

    return Axios({
        url: url_,
        method: 'GET',
        responseType: 'blob', // important
    }).then((response) => {
      FileSaver.saveAs(response.data, 'Learning Analysis Report.xlsx');
    });
  }
  getProgressTrackingReportDetail(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/tracking-progress-report/export-list-progress-tracking-detail?${query}`;
    url_ = url_.replace(/[?&]$/, "");

    return Axios({
        url: url_,
        method: 'GET',
        responseType: 'blob', // important
    }).then((response) => {
      FileSaver.saveAs(response.data, 'Detail Learning Analysis Report.xlsx');
    });
  }
  
  getProgressTrackingReportJSON(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/tracking-progress-report/export-list-progress-tracking-json?${query}`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      method: "GET",
      headers: {
        "Accept": "application/json"
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processgetProgressTrackingReportJSON(_response);
    });
  }
  
  protected processgetProgressTrackingReportJSON(response: Response): Promise<any> {
    const status = response.status;
    let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
    if (status === 200) {
      return response.text().then((_responseText) => {
        let result200: any = null;
        result200 = _responseText === "" ? null : <any>JSON.parse(_responseText, this.jsonParseReviver);
        return result200;
      });
    } else if (status !== 200 && status !== 204) {
      return response.text().then((_responseText) => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      });
    }
    return Promise.resolve<any>(<any>null);
  }

  getProgressTrackingReportDetailJSON(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/tracking-progress-report/export-list-progress-tracking-detail-json?${query}`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      method: "GET",
      headers: {
        "Accept": "application/json"
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processgetProgressTrackingReportDetailJSON(_response);
    });
  }
  
  protected processgetProgressTrackingReportDetailJSON(response: Response): Promise<any> {
    const status = response.status;
    let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
    if (status === 200) {
      return response.text().then((_responseText) => {
        let result200: any = null;
        result200 = _responseText === "" ? null : <any>JSON.parse(_responseText, this.jsonParseReviver);
        return result200;
      });
    } else if (status !== 200 && status !== 204) {
      return response.text().then((_responseText) => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      });
    }
    return Promise.resolve<any>(<any>null);
  }
  // END Report Tracking


  
  getLearningProgressReportJson(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/report-tracking/competency-api?${query}`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      method: "GET",
      headers: {
        "Accept": "application/json"
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processgetLearningProgressReportJson(_response);
    });
  }
  
  protected processgetLearningProgressReportJson(response: Response): Promise<any> {
    const status = response.status;
    let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
    if (status === 200) {
      return response.text().then((_responseText) => {
        let result200: any = null;
        result200 = _responseText === "" ? null : <any>JSON.parse(_responseText, this.jsonParseReviver);
        return result200;
      });
    } else if (status !== 200 && status !== 204) {
      return response.text().then((_responseText) => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
      });
    }
    return Promise.resolve<any>(<any>null);
  }
}