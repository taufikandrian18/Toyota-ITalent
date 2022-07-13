import { throwException } from "../common";
import { cleanQueryToString, queryToString } from "../helper";
import { CreateDictionaryModel, DictionaryModel, IAllDictionaries, IParamDictionaries } from "./DictionaryModel";

export class DictionaryService {
  private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

  constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
      this.http = http ? http : <any>window;
      this.baseUrl = baseUrl ? baseUrl : "";
  }
  
  getAllDictionaries(params?: IParamDictionaries): Promise<IAllDictionaries> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/Dictionaries/get-all-dictionaries?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllDictionaries(_response);
      });
  }
  
  protected processGetAllDictionaries(response: Response): Promise<IAllDictionaries> {
    const status = response.status;
    let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
    if (status === 200) {
        return response.text().then((_responseText) => {
        let result200: any = null;
        result200 = _responseText === "" ? null : <IAllDictionaries>JSON.parse(_responseText, this.jsonParseReviver);
        return result200;
        });
    } else if (status !== 200 && status !== 204) {
        return response.text().then((_responseText) => {
        return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        });
    }
    return Promise.resolve<IAllDictionaries>(<any>null);
  }
    
    getDictionary(id?: string): Promise<DictionaryModel> {
    let url_ = this.baseUrl + `/api/v1/Dictionaries/get-dictionary-by-id?dictionaryId=${id}`;
        url_ = url_.replace(/[?&]$/, "");

        let options_ = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetDictionary(_response);
        });
    }
  
    protected processGetDictionary(response: Response): Promise<DictionaryModel> {
        const status = response.status;
        let _headers: any = {}; if (response.headers && response.headers.forEach) { response.headers.forEach((v: any, k: any) => _headers[k] = v); };
        if (status === 200) {
            return response.text().then((_responseText) => {
            let result200: any = null;
            result200 = _responseText === "" ? null : <DictionaryModel>JSON.parse(_responseText, this.jsonParseReviver);
            return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<DictionaryModel>(<any>null);
    }

  createDictionary(body?: CreateDictionaryModel): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/Dictionaries/insert-dictionary`;
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
          return this.processCreateDictionary(_response);
      });
  }
  
  protected processCreateDictionary(response: Response): Promise<any> {
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
    
    updateDictionary(body?: CreateDictionaryModel): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/Dictionaries/update-dictionary-by-id`;
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
          return this.processUpdateDictionary(_response);
      });
  }
  
  protected processUpdateDictionary(response: Response): Promise<any> {
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
  

    /**
     * @param id (optional)
     * @return Success
     */
    deleteDictionary(id?: String): Promise<any> {
        let url_ = this.baseUrl + `/api/v1/Dictionaries/delete-dictionary`;
        url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
            body: JSON.stringify({
              dictionaryId: id,
              isDeleteDictionary: true
            }),
            method: "DELETE",
            headers: {
                "Accept": "application/json",
              "Content-Type": "application/json",
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processDeleteDictionary(_response);
        });
    }

    protected processDeleteDictionary(response: Response): Promise<any> {
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

    /**
     * @param model (optional) 
     * @return Success
     */
    insertAnnouncement(model: any): Promise<any> {
        let url_ = this.baseUrl + "/api/v1/announcement/data";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(model);

        let options_ = <RequestInit>{
            body: content_,
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processInsertAnnouncement(_response);
        });
    }

    protected processInsertAnnouncement(response: Response): Promise<any> {
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

    /**
     * @param model (optional) 
     * @return Success
     */
    updateAnnouncement(model: any): Promise<any> {
        let url_ = this.baseUrl + "/api/v1/announcement/data";
        url_ = url_.replace(/[?&]$/, "");

        const content_ = JSON.stringify(model);

        let options_ = <RequestInit>{
            body: content_,
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            }
        };

        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processUpdateAnnouncement(_response);
        });
    }

    protected processUpdateAnnouncement(response: Response): Promise<any> {
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