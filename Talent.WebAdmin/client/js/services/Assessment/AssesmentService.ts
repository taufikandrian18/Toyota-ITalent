import { throwException } from "../common";
import { cleanQueryToString, queryToString } from "../helper";

export class AssessmentService {
  private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

  constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
    this.http = http ? http : <any>window;
    this.baseUrl = baseUrl ? baseUrl : "";
  }

  // Skill Check
  getAllSkillCheck(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/assesment/skill-check/get-list?${query}`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      method: "GET",
      headers: {
        "Accept": "application/json"
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processGetAllSkillCheck(_response);
    });
  }
  
  protected processGetAllSkillCheck(response: Response): Promise<any> {
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

  createSkillCheck(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/assesment/skill-check/create`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      body: JSON.stringify(body),
      method: "POST",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json",
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processCreateSkillCheck(_response);
    });
  }
  
  protected processCreateSkillCheck(response: Response): Promise<any> {
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

  updateSkillCheck(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/assesment/skill-check/update`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      body: JSON.stringify(body),
      method: "PUT",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json",
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processUpdateSkillCheck(_response);
    });
  }
  
  protected processUpdateSkillCheck(response: Response): Promise<any> {
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
  
  deleteSkillCheck(id?: String): Promise<any> {
    let url_ = this.baseUrl + `/api/assesment/skill-check/delete?request=${id}`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      body: JSON.stringify({
        request: id
      }),
      method: "DELETE",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json",
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processDeleteSkillCheck(_response);
    });
  }

  protected processDeleteSkillCheck(response: Response): Promise<any> {
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
	
  // END Skill Check

  // Area Specialist
  getAllAreaSpecialist(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/AreaSpecialists/get-all-area-specialists?${query}`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      method: "GET",
      headers: {
        "Accept": "application/json"
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processGetAllAreaSpecialist(_response);
    });
  }
  
  protected processGetAllAreaSpecialist(response: Response): Promise<any> {
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

  createAreaSpecialist(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/AreaSpecialists/insert-area-specialist`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      body: JSON.stringify(body),
      method: "POST",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json",
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processCreateAreaSpecialist(_response);
    });
  }
  
  protected processCreateAreaSpecialist(response: Response): Promise<any> {
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


  updateAreaSpecialist(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/AreaSpecialists/update-area-specialist-by-id`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      body: JSON.stringify(body),
      method: "POST",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json",
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processUpdateAreaSpecialist(_response);
    });
  }
  
  protected processUpdateAreaSpecialist(response: Response): Promise<any> {
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
  
  deleteAreaSpecialist(id?: String): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/AreaSpecialists/delete-area-specialist`;
    url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
      body: JSON.stringify({
        "areaSpecialistId": id,
        "isDeleteAreaSpecialist": true
      }),
      method: "DELETE",
      headers: {
        "Accept": "application/json",
        "Content-Type": "application/json",
      }
    };

    return this.http.fetch(url_, options_).then((_response: Response) => {
      return this.processDeleteAreaSpecialist(_response);
    });
  }

  protected processDeleteAreaSpecialist(response: Response): Promise<any> {
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