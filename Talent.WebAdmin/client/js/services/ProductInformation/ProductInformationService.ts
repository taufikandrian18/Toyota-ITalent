import { throwException } from "../common";
import { cleanQueryToString, queryToString } from "../helper";

export class ProductInformationService {
  private http: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> };
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

  constructor(baseUrl?: string, http?: { fetch(url: RequestInfo, init?: RequestInit): Promise<Response> }) {
      this.http = http ? http : <any>window;
      this.baseUrl = baseUrl ? baseUrl : "";
  }

	// Categories
  getAllCategories(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductCategories/get-all-productCategories?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCategories(_response);
      });
  }
  
  protected processGetAllCategories(response: Response): Promise<any> {
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

	createProductCategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductCategories/insert-product-category`;
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
          return this.processCreateProductCategory(_response);
      });
  }
  
  protected processCreateProductCategory(response: Response): Promise<any> {
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

  updateProductCategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductCategories/update-product-category-by-id`;
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
          return this.processUpdateProductCategory(_response);
      });
  }
  
  protected processUpdateProductCategory(response: Response): Promise<any> {
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
  
  deleteProductCategory(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductCategories/delete-product-category`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productCategoryId: id,
            isDeleteProductCategory: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductCategory(_response);
      });
  }

  protected processDeleteProductCategory(response: Response): Promise<any> {
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
	
	// END Categories
    
	getAllSegments(): Promise<any> {
	let url_ = this.baseUrl + `/api/v1/dropdown/get-product-segment`;
		url_ = url_.replace(/[?&]$/, "");

		let options_ = <RequestInit>{
				method: "GET",
				headers: {
						"Accept": "application/json"
				}
		};

		return this.http.fetch(url_, options_).then((_response: Response) => {
				return this.processGetAllSegments(_response);
		});
	}
    
  getAllProducts(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/Products/get-all-products?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProducts(_response);
      });
  }
  
  protected processGetAllProducts(response: Response): Promise<any> {
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
  
  protected processGetAllSegments(response: Response): Promise<any> {
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
    
  createProduct(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/Products/insert-product`;
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
          return this.processCreateProduct(_response);
      });
  }
  
  protected processCreateProduct(response: Response): Promise<any> {
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

  updateProduct(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/Products/update-product-by-id`;
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
          return this.processUpdateProduct(_response);
      });
  }
  
  protected processUpdateProduct(response: Response): Promise<any> {
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
    
  getProductById(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/Products/get-product-by-id?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetProductById(_response);
      });
  }
  
  protected processGetProductById(response: Response): Promise<any> {
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

  getAllProductTypes(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductTypes/get-all-product-types?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductTypes(_response);
      });
  }
  
  protected processGetAllProductTypes(response: Response): Promise<any> {
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

  createProductType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductTypes/insert-product-type`;
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
          return this.processCreateProductType(_response);
      });
  }
  
  protected processCreateProductType(response: Response): Promise<any> {
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

  updateProductType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductTypes/update-product-type-by-id`;
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
          return this.processUpdateProductType(_response);
      });
  }
  
  protected processUpdateProductType(response: Response): Promise<any> {
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

  deleteProductType(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductTypes/delete-product-type`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productTypeId: id,
            isDeleteProductType: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductType(_response);
      });
  }

  protected processDeleteProductType(response: Response): Promise<any> {
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

  // FAQ Categories
  getAllProductFAQCategories(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductFAQCategories/get-all-product-faq-category?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductFAQCategories(_response);
      });
  }
  
  protected processGetAllProductFAQCategories(response: Response): Promise<any> {
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


  createProductFAQCategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFAQCategories/insert-product-faq-category`;
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
          return this.processCreateProductFAQCategory(_response);
      });
  }
  
  protected processCreateProductFAQCategory(response: Response): Promise<any> {
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

  updateProductFAQCategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFAQCategories/update-product-faq-category-by-id`;
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
          return this.processUpdateProductFAQCategory(_response);
      });
  }
  
  protected processUpdateProductFAQCategory(response: Response): Promise<any> {
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

  deleteProductFAQCategory(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductFAQCategories/delete-product-faq-category`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productFaqCategoryId: id,
            isDeleteProductFAQCategory: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductFAQCategory(_response);
      });
  }

  protected processDeleteProductFAQCategory(response: Response): Promise<any> {
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

  // END FAQ Categories

  // FAQ

  getAllProductFAQUser(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductFAQUsers/get-all-product-faq-users?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductFAQUser(_response);
      });
  }
  
  protected processGetAllProductFAQUser(response: Response): Promise<any> {
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

  getAllProductFAQ(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductFAQs/get-all-product-faqs?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductFAQ(_response);
      });
  }
  
  protected processGetAllProductFAQ(response: Response): Promise<any> {
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

  createProductFAQ(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFAQs/insert-product-faq`;
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
          return this.processCreateProductFAQ(_response);
      });
  }
  
  protected processCreateProductFAQ(response: Response): Promise<any> {
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

  updateProductFAQ(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFAQs/update-product-faq-by-id`;
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
          return this.processUpdateProductFAQ(_response);
      });
  }
  
  protected processUpdateProductFAQ(response: Response): Promise<any> {
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

  deleteProductFAQ(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductFAQs/delete-product-faq`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productFaqId: id,
            isDeleteProductFAQ: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductFAQ(_response);
      });
  }

  protected processDeleteProductFAQ(response: Response): Promise<any> {
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
  // END FAQ
    
  // Photo Gallery
  getAllProductGallery(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductGalleries/get-all-product-galleries?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductGallery(_response);
      });
  }
  
  protected processGetAllProductGallery(response: Response): Promise<any> {
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
    
    
  getAllProductGalleryContribute(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductGalleries/get-all-product-gallery-contributors?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductGalleryContribute(_response);
      });
  }
  
  protected processGetAllProductGalleryContribute(response: Response): Promise<any> {
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

  createProductGallery(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductGalleries/insert-product-gallery`;
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
          return this.processCreateProductGallery(_response);
      });
  }
  
  protected processCreateProductGallery(response: Response): Promise<any> {
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

  updateProductGallery(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductGalleries/update-product-gallery-by-id`;
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
          return this.processUpdateProductGallery(_response);
      });
  }
  
  protected processUpdateProductGallery(response: Response): Promise<any> {
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
  
  deleteProductGallery(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductGalleries/delete-product-gallery`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productGalleryId: id,
            isDeleteProductGallery: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductGallery(_response);
      });
  }

  protected processDeleteProductGallery(response: Response): Promise<any> {
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
  approvalProductGallery(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductGalleries/update-product-gallery-by-approval`;
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
          return this.processApprovalProductGallery(_response);
      });
  }
  
  protected processApprovalProductGallery(response: Response): Promise<any> {
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
  // End Photo gallery

  // Photo Gallery
  getAllProductPhotos(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductPhotos/get-all-product-photos?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductPhotos(_response);
      });
  }
  
  protected processGetAllProductPhotos(response: Response): Promise<any> {
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

  createProductPhoto(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductPhotos/insert-product-photo`;
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
          return this.processCreateProductPhoto(_response);
      });
  }
  
  protected processCreateProductPhoto(response: Response): Promise<any> {
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

  updateProductPhoto(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductPhotos/update-product-photo-by-id`;
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
          return this.processUpdateProductPhoto(_response);
      });
  }
  
  protected processUpdateProductPhoto(response: Response): Promise<any> {
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
  
  deleteProductPhoto(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductPhotos/delete-product-photo`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productPhotoId: id,
            isDeleteProduct: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductPhoto(_response);
      });
  }

  protected processDeleteProductPhoto(response: Response): Promise<any> {
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
  // End Photo gallery

  // Customer Type
  getAllCustomerType(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductCustomerTypes/get-all-product-customer-types?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCustomerType(_response);
      });
  }
  
  protected processGetAllCustomerType(response: Response): Promise<any> {
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

  createCustomerType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductCustomerTypes/insert-product-customer-type`;
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
          return this.processCreateCustomerType(_response);
      });
  }
  
  protected processCreateCustomerType(response: Response): Promise<any> {
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

  updateCustomerType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductCustomerTypes/update-product-customer-type-by-id`;
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
          return this.processUpdateCustomerType(_response);
      });
  }
  
  protected processUpdateCustomerType(response: Response): Promise<any> {
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
  
  deleteCustomerType(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductCustomerTypes/delete-product-customer-type`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productCustomerTypeId: id,
            isDeleteProductCustomerType: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteCustomerType(_response);
      });
  }

  protected processDeleteCustomerType(response: Response): Promise<any> {
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
  // End Customer Type

  // Customer
  getAllCustomer(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductCustomers/get-all-product-customers?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCustomer(_response);
      });
  }
  
  protected processGetAllCustomer(response: Response): Promise<any> {
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

  createCustomer(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductCustomers/insert-product-customer`;
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
          return this.processCreateCustomer(_response);
      });
  }
  
  protected processCreateCustomer(response: Response): Promise<any> {
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

  updateCustomer(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductCustomers/update-product-customer-by-id`;
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
          return this.processUpdateCustomer(_response);
      });
  }
  
  protected processUpdateCustomer(response: Response): Promise<any> {
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
  
  deleteCustomer(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductCustomers/delete-product-customer`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productCustomerId: id,
            isDeleteProductCustomer: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteCustomer(_response);
      });
  }

  protected processDeleteCustomer(response: Response): Promise<any> {
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
  // End Customer

  // Product Competitor
  getAllProductCompetitor(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/Products/get-all-competitor-products?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductCompetitor(_response);
      });
  }
  
  protected processGetAllProductCompetitor(response: Response): Promise<any> {
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
  // End Product Competitor
  
  // Product Competitor Mapping
  getAllProductCompetitorMapping(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/Product-Competitor-Mappings/get-all-product-competitor-mappings?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductCompetitorMapping(_response);
      });
  }
  
  protected processGetAllProductCompetitorMapping(response: Response): Promise<any> {
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

  createProductCompetitorMapping(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/Product-Competitor-Mappings/insert-product-competitor-mapping`;
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
          return this.processCreateProductCompetitorMapping(_response);
      });
  }
  
  protected processCreateProductCompetitorMapping(response: Response): Promise<any> {
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

  updateProductCompetitorMapping(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/Product-Competitor-Mappings/update-product-competitor-mapping`;
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
          return this.processUpdateProductCompetitorMapping(_response);
      });
  }
  
  protected processUpdateProductCompetitorMapping(response: Response): Promise<any> {
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
  
  deleteProductCompetitorMapping(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/Product-Competitor-Mappings/delete-product-competitor-mapping`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productCompetitorMappingId: id,
            isDeleteProductCompetitor: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductCompetitorMapping(_response);
      });
  }

  protected processDeleteProductCompetitorMapping(response: Response): Promise<any> {
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
  // End Product Competitor Mapping

  // Tips Category
  getAllTipCategory(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductTipCategories/get-all-product-tip-category?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllTipCategory(_response);
      });
  }
  
  protected processGetAllTipCategory(response: Response): Promise<any> {
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

  createTipCategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductTipCategories/insert-product-tip-category`;
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
          return this.processCreateTipCategory(_response);
      });
  }
  
  protected processCreateTipCategory(response: Response): Promise<any> {
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

  updateTipCategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductTipCategories/update-product-tip-category-by-id`;
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
          return this.processUpdateTipCategory(_response);
      });
  }
  
  protected processUpdateTipCategory(response: Response): Promise<any> {
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
  
  deleteTipCategory(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductTipCategories/delete-product-tip-category`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productTipCategoryId: id,
            isDeleteProductTipCategory: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteTipCategory(_response);
      });
  }

  protected processDeleteTipCategory(response: Response): Promise<any> {
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
  // End Tips Category

  // Tips
  getAllTip(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductTips/get-all-product-tips?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllTip(_response);
      });
  }
  
  protected processGetAllTip(response: Response): Promise<any> {
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

  createTip(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductTips/insert-product-tip`;
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
          return this.processCreateTip(_response);
      });
  }
  
  protected processCreateTip(response: Response): Promise<any> {
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

  updateTip(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductTips/update-product-tip-by-id`;
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
          return this.processUpdateTip(_response);
      });
  }
  
  protected processUpdateTip(response: Response): Promise<any> {
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
  
  deleteTip(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductTips/delete-product-tip`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productTipId: id,
            isDeleteProductTip: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteTip(_response);
      });
  }

  protected processDeleteTip(response: Response): Promise<any> {
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
    
  
  updateApprovalTip(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductTips/update-product-tip-by-approval`;
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
          return this.processUpdateApprovalTip(_response);
      });
  }
  
  protected processUpdateApprovalTip(response: Response): Promise<any> {
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
  // End Tips
    
    
  // SPWA Category
  getAllSPWACategory(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductSPWACategories/get-all-product-spwa-category?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllSPWACategory(_response);
      });
  }
  
  protected processGetAllSPWACategory(response: Response): Promise<any> {
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

  createSPWACategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductSPWACategories/insert-product-spwa-category`;
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
          return this.processCreateSPWACategory(_response);
      });
  }
  
  protected processCreateSPWACategory(response: Response): Promise<any> {
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

  updateSPWACategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductSPWACategories/update-product-spwa-category-by-id`;
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
          return this.processUpdateSPWACategory(_response);
      });
  }
  
  protected processUpdateSPWACategory(response: Response): Promise<any> {
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
  
  deleteSPWACategory(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductSPWACategories/delete-product-spwa-category`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productSPWACategoryId: id,
            isDeleteProductSPWACategory: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteSPWACategory(_response);
      });
  }

  protected processDeleteSPWACategory(response: Response): Promise<any> {
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
  // End SPWA Category

  // SPWA Mapping
  getAllSPWAMapping(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductSPWAMappings/get-all-product-spwa-mappings?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllSPWAMapping(_response);
      });
  }
  
  protected processGetAllSPWAMapping(response: Response): Promise<any> {
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

  createSPWAMapping(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductSPWAMappings/insert-product-spwa-mappings`;
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
          return this.processCreateSPWAMapping(_response);
      });
  }
  
  protected processCreateSPWAMapping(response: Response): Promise<any> {
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

  updateSPWAMapping(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductSPWAMappings/update-product-spwa-mapping-by-id`;
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
          return this.processUpdateSPWAMapping(_response);
      });
  }
  
  protected processUpdateSPWAMapping(response: Response): Promise<any> {
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
  
  deleteSPWAMapping(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductSPWAMappings/delete-product-spwa-mapping`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productSPWAMappingId: id,
            isDeleteProductSPWAMapping: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteSPWAMapping(_response);
      });
  }

  protected processDeleteSPWAMapping(response: Response): Promise<any> {
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
  // End SPWA Mapping

  // CMS Content
  getAllCMSContent(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/CmsContents/get-all-cms-contents?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCMSContent(_response);
      });
  }
  
  protected processGetAllCMSContent(response: Response): Promise<any> {
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

  createCMSContent(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsContents/insert-cms-content`;
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
          return this.processCreateCMSContent(_response);
      });
  }
  
  protected processCreateCMSContent(response: Response): Promise<any> {
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

  updateCMSContent(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsContents/update-cms-content-by-id`;
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
          return this.processUpdateCMSContent(_response);
      });
  }
  
  protected processUpdateCMSContent(response: Response): Promise<any> {
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
  
  deleteCMSContent(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/CmsContents/delete-cms-content`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            cms_ContentId: id,
            isDeleteCmsContent: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteCMSContent(_response);
      });
  }

  protected processDeleteCMSContent(response: Response): Promise<any> {
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
  // End CMS Content

  // CMS FMB
  getAllCMSFmb(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/CmsFmbs/get-all-cms-fmbs?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCMSFmb(_response);
      });
  }
  
  protected processGetAllCMSFmb(response: Response): Promise<any> {
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

  createCMSFmb(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsFmbs/insert-cms-fmbs`;
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
          return this.processCreateCMSFmb(_response);
      });
  }
  
  protected processCreateCMSFmb(response: Response): Promise<any> {
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

  updateCMSFmb(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsFmbs/update-cms-fmb-by-id`;
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
          return this.processUpdateCMSFmb(_response);
      });
  }
  
  protected processUpdateCMSFmb(response: Response): Promise<any> {
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
  
  deleteCMSFmb(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/CmsFmbs/delete-cms-fmb`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            cms_FmbId: id,
            isDeleteCmsFmb: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteCMSFmb(_response);
      });
  }

  protected processDeleteCMSFmb(response: Response): Promise<any> {
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
  // End CMS FMB

  // CMS Menu
  getAllCMSMenu(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/CmsMenus/get-all-cms-menus?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCMSMenu(_response);
      });
  }
  
  protected processGetAllCMSMenu(response: Response): Promise<any> {
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

  createCMSMenu(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsMenus/insert-cms-menu`;
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
          return this.processCreateCMSMenu(_response);
      });
  }
  
  protected processCreateCMSMenu(response: Response): Promise<any> {
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

  updateCMSMenu(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsMenus/update-cms-menu-by-id`;
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
          return this.processUpdateCMSMenu(_response);
      });
  }
  
  protected processUpdateCMSMenu(response: Response): Promise<any> {
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
  
  deleteCMSMenu(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/CmsMenus/delete-cms-menu`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            cms_MenuId: id,
            isDeleteCmsMenu: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteCMSMenu(_response);
      });
  }

  protected processDeleteCMSMenu(response: Response): Promise<any> {
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
  // End CMS Menu

  // CMS Operation
  getAllCMSOperation(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/CmsOperations/get-all-cms-operations?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCMSOperation(_response);
      });
  }
  
  protected processGetAllCMSOperation(response: Response): Promise<any> {
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

  createCMSOperation(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsOperations/insert-cms-operations`;
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
          return this.processCreateCMSOperation(_response);
      });
  }
  
  protected processCreateCMSOperation(response: Response): Promise<any> {
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

  updateCMSOperation(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsOperations/update-cms-operation-by-id`;
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
          return this.processUpdateCMSOperation(_response);
      });
  }
  
  protected processUpdateCMSOperation(response: Response): Promise<any> {
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
  
  deleteCMSOperation(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/CmsOperations/delete-cms-operation`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            cms_OperationId: id,
            isDeleteCmsOperation: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteCMSOperation(_response);
      });
  }

  protected processDeleteCMSOperation(response: Response): Promise<any> {
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
  // End CMS Operation

	// CMS Setting
  getAllCMSSetting(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/CmsSettings/get-all-cms-settings?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCMSSetting(_response);
      });
  }
  
  protected processGetAllCMSSetting(response: Response): Promise<any> {
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

  createCMSSetting(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsSettings/insert-cms-settings`;
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
          return this.processCreateCMSSetting(_response);
      });
  }
  
  protected processCreateCMSSetting(response: Response): Promise<any> {
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

  updateCMSSetting(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsSettings/update-cms-setting-by-id`;
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
          return this.processUpdateCMSSetting(_response);
      });
  }
  
  protected processUpdateCMSSetting(response: Response): Promise<any> {
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
  
  deleteCMSSetting(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/CmsSettings/delete-cms-settings`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            cms_SettingId: id,
            isDeleteCmsSetting: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteCMSSetting(_response);
      });
  }

  protected processDeleteCMSSetting(response: Response): Promise<any> {
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
  // End CMS Setting

	// CMS Sub Content
  getAllCMSSubContent(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/CmsSubContents/get-all-cms-subcontents?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCMSSubContent(_response);
      });
  }
  
  protected processGetAllCMSSubContent(response: Response): Promise<any> {
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

  createCMSSubContent(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsSubContents/insert-cms-subcontents`;
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
          return this.processCreateCMSSubContent(_response);
      });
  }
  
  protected processCreateCMSSubContent(response: Response): Promise<any> {
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

  updateCMSSubContent(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsSubContents/update-cms-subcontent-by-id`;
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
          return this.processUpdateCMSSubContent(_response);
      });
  }
  
  protected processUpdateCMSSubContent(response: Response): Promise<any> {
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
  
  deleteCMSSubContent(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/CmsSubContents/delete-cms-subcontent`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            cms_SubContentId: id,
            isDeleteCmsSubContent: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteCMSSubContent(_response);
      });
  }

  protected processDeleteCMSSubContent(response: Response): Promise<any> {
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
  // End CMS Sub Content

  // CMS Work Principal
  getAllCMSWorkPrincipal(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/CmsWorkPrincipals/get-all-cms-work-principals?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllCMSWorkPrincipal(_response);
      });
  }
  
  protected processGetAllCMSWorkPrincipal(response: Response): Promise<any> {
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

  createCMSWorkPrincipal(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsWorkPrincipals/insert-cms-work-principals`;
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
          return this.processCreateCMSWorkPrincipal(_response);
      });
  }
  
  protected processCreateCMSWorkPrincipal(response: Response): Promise<any> {
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

  updateCMSWorkPrincipal(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/CmsWorkPrincipals/update-cms-work-principal-by-id`;
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
          return this.processUpdateCMSWorkPrincipal(_response);
      });
  }
  
  protected processUpdateCMSWorkPrincipal(response: Response): Promise<any> {
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
  
  deleteCMSWorkPrincipal(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/CmsWorkPrincipals/delete-cms-work-principal`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            cms_WorkPrincipalId: id,
            isDeleteCmsWorkPrincipal: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteCMSWorkPrincipal(_response);
      });
  }

  protected processDeleteCMSWorkPrincipal(response: Response): Promise<any> {
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
  // End CMS Work Principal

  getAllProductFeatureCategory(): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/dropdown/get-product-feature`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductFeatureCategory(_response);
      });
  }
  
  protected processGetAllProductFeatureCategory(response: Response): Promise<any> {
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
    
  // product feature categories
  getAllProductFeatureCategories(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductFeatureCategories/get-all-product-feature-categories?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductFeatureCategories(_response);
      });
  }
  
  protected processGetAllProductFeatureCategories(response: Response): Promise<any> {
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
    
    createProductFeatureCategories(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFeatureCategories/insert-product-feature-category`;
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
          return this.processCreateProductFeatureCategories(_response);
      });
  }
  
  protected processCreateProductFeatureCategories(response: Response): Promise<any> {
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

  updateProductFeatureCategories(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFeatureCategories/update-product-feature-category-by-id`;
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
          return this.processUpdateProductFeatureCategories(_response);
      });
  }
  
  protected processUpdateProductFeatureCategories(response: Response): Promise<any> {
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
  
  deleteProductFeatureCategories(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductFeatureCategories/delete-product-feature-category`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productFeatureCategoryId: id,
            isDeleteProductFeatureCategory: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductFeatureCategories(_response);
      });
  }

  protected processDeleteProductFeatureCategories(response: Response): Promise<any> {
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
  // end product feature categories

  // Product Feature
  getAllProductFeature(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductFeatures/get-all-product-features?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductFeature(_response);
      });
  }
  
  protected processGetAllProductFeature(response: Response): Promise<any> {
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

  createProductFeature(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFeatures/insert-product-feature`;
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
          return this.processCreateProductFeature(_response);
      });
  }
  
  protected processCreateProductFeature(response: Response): Promise<any> {
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

  updateProductFeature(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFeatures/update-product-feature-by-id`;
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
          return this.processUpdateProductFeature(_response);
      });
  }
  
  protected processUpdateProductFeature(response: Response): Promise<any> {
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
  
  deleteProductFeature(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductFeatures/delete-product-feature`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productFeatureId: id,
            isDeleteProductFeature: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductFeature(_response);
      });
  }

  protected processDeleteProductFeature(response: Response): Promise<any> {
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
  // END Product Feature

  // Product Feature Mapping
  getAllProductFeatureMapping(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductFeatureMappings/get-all-product-feature-mappings?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductFeatureMapping(_response);
      });
  }
  
  protected processGetAllProductFeatureMapping(response: Response): Promise<any> {
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

  createProductFeatureMapping(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFeatureMappings/insert-product-feature-mappings`;
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
          return this.processCreateProductFeatureMapping(_response);
      });
  }
  
  protected processCreateProductFeatureMapping(response: Response): Promise<any> {
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

  updateProductFeatureMapping(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFeatureMappings/update-product-feature-mapping-by-id`;
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
          return this.processUpdateProductFeatureMapping(_response);
      });
  }
  
  protected processUpdateProductFeatureMapping(response: Response): Promise<any> {
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
    
    updateApprovalProductFeatureMapping(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductFeatureMappings/update-approval-product-feature-mapping`;
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
          return this.processUpdateApprovalProductFeatureMapping(_response);
      });
  }
  
  protected processUpdateApprovalProductFeatureMapping(response: Response): Promise<any> {
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
  
  deleteProductFeatureMapping(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductFeatureMappings/delete-product-feature-mapping`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productFeatureMappingId: id,
            isDeleteProductFeatureMapping: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductFeatureMapping(_response);
      });
  }

  protected processDeleteProductFeatureMapping(response: Response): Promise<any> {
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
  // END Product Feature Mapping
    
  // Product Program Category
  getAllProductProgramCategory(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductProgramCategories/get-all-product-program-category?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductProgramCategory(_response);
      });
  }
  
  protected processGetAllProductProgramCategory(response: Response): Promise<any> {
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

  createProductProgramCategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductProgramCategories/insert-product-program-category`;
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
          return this.processCreateProductProgramCategory(_response);
      });
  }
  
  protected processCreateProductProgramCategory(response: Response): Promise<any> {
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

  updateProductProgramCategory(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductProgramCategories/update-product-program-category-by-id`;
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
          return this.processUpdateProductProgramCategory(_response);
      });
  }
  
  protected processUpdateProductProgramCategory(response: Response): Promise<any> {
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
  
  deleteProductProgramCategory(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductProgramCategories/delete-product-program-category`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productProgramCategoryId: id,
            isDeleteProductProgramCategory: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductProgramCategory(_response);
      });
  }

  protected processDeleteProductProgramCategory(response: Response): Promise<any> {
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
  // END Product Program Category

	// Product Program Mapping
  getAllProductProgramMapping(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ProductProgramMappings/get-all-product-program-mappings?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllProductProgramMapping(_response);
      });
  }
  
  protected processGetAllProductProgramMapping(response: Response): Promise<any> {
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

  createProductProgramMapping(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductProgramMappings/insert-product-program-mappings`;
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
          return this.processCreateProductProgramMapping(_response);
      });
  }
  
  protected processCreateProductProgramMapping(response: Response): Promise<any> {
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

  updateProductProgramMapping(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ProductProgramMappings/update-product-program-mapping-by-id`;
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
          return this.processUpdateProductProgramMapping(_response);
      });
  }
  
  protected processUpdateProductProgramMapping(response: Response): Promise<any> {
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
  
  deleteProductProgramMapping(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ProductProgramMappings/delete-product-program-mapping`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            productProgramMappingId: id,
            isDeleteProductProgramMapping: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteProductProgramMapping(_response);
      });
  }

  protected processDeleteProductProgramMapping(response: Response): Promise<any> {
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
  // END Product Program Mapping
    
  // Kodawari
  getAllKodawari(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/Kodawaris/get-all-kodawaris?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllKodawari(_response);
      });
  }
  
  protected processGetAllKodawari(response: Response): Promise<any> {
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
    
    createKodawari(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/Kodawaris/insert-kodawaris`;
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
          return this.processCreateKodawari(_response);
      });
  }
  
  protected processCreateKodawari(response: Response): Promise<any> {
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

  updateKodawari(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/Kodawaris/update-kodawari-by-id`;
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
          return this.processUpdateKodawari(_response);
      });
  }
  
  protected processUpdateKodawari(response: Response): Promise<any> {
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
  
  deleteKodawari(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/Kodawaris/delete-kodawaris`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            kodawariId: id,
            isDeleteKodawari: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteKodawari(_response);
      });
  }

  protected processDeleteKodawari(response: Response): Promise<any> {
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
  // End Kodawari

  // Kodawari Banner
  getAllKodawariBanner(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/KodawariBanners/get-all-kodawari-banners?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllKodawariBanner(_response);
      });
  }
  
  protected processGetAllKodawariBanner(response: Response): Promise<any> {
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

  createKodawariBanner(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KodawariBanners/insert-kodawari-banner`;
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
          return this.processCreateKodawariBanner(_response);
      });
  }
  
  protected processCreateKodawariBanner(response: Response): Promise<any> {
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

  updateKodawariBanner(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KodawariBanners/update-kodawari-banner-by-id`;
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
          return this.processUpdateKodawariBanner(_response);
      });
  }
  
  protected processUpdateKodawariBanner(response: Response): Promise<any> {
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
  
  deleteKodawariBanner(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/KodawariBanners/delete-kodawari-banners`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            kodawariBannerId: id,
            isDeleteKodawariBanners: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteKodawariBanner(_response);
      });
  }

  protected processDeleteKodawariBanner(response: Response): Promise<any> {
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
  // End Kodawari Banner

  // Kodawari Download
  getAllKodawariDownload(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/KodawariDownloads/get-all-kodawari-downloads?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllKodawariDownload(_response);
      });
  }
  
  protected processGetAllKodawariDownload(response: Response): Promise<any> {
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

  createKodawariDownload(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KodawariDownloads/insert-kodawari-download`;
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
          return this.processCreateKodawariDownload(_response);
      });
  }
  
  protected processCreateKodawariDownload(response: Response): Promise<any> {
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

  updateKodawariDownload(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KodawariDownloads/update-kodawari-download-by-id`;
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
          return this.processUpdateKodawariDownload(_response);
      });
  }
  
  protected processUpdateKodawariDownload(response: Response): Promise<any> {
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
  
  deleteKodawariDownload(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/KodawariDownloads/delete-kodawari-downloads`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            kodawariDownloadId: id,
            isDeleteKodawariDownload: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteKodawariDownload(_response);
      });
  }

  protected processDeleteKodawariDownload(response: Response): Promise<any> {
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
    
  // End Kodawari Download

  // Kodawari Menu
  getAllKodawariMenu(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/KodawariMenus/get-all-kodawari-menus?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllKodawariMenu(_response);
      });
  }
  
  protected processGetAllKodawariMenu(response: Response): Promise<any> {
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

  createKodawariMenu(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KodawariMenus/insert-kodawari-menu`;
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
          return this.processCreateKodawariMenu(_response);
      });
  }
  
  protected processCreateKodawariMenu(response: Response): Promise<any> {
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

  updateKodawariMenu(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KodawariMenus/update-kodawari-menu-by-id`;
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
          return this.processUpdateKodawariMenu(_response);
      });
  }
  
  protected processUpdateKodawariMenu(response: Response): Promise<any> {
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
  
  deleteKodawariMenu(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/KodawariMenus/delete-kodawari-menus`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            kodawariMenuId: id,
            isDeleteKodawariMenus: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteKodawariMenu(_response);
      });
  }

  protected processDeleteKodawariMenu(response: Response): Promise<any> {
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
  // End Kodawari Menu

  // Kodawari Type
  getAllKodawariType(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/KodawariTypes/get-all-kodawari-types?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllKodawariType(_response);
      });
  }
  
  protected processGetAllKodawariType(response: Response): Promise<any> {
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

  createKodawariType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KodawariTypes/insert-kodawari-type`;
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
          return this.processCreateKodawariType(_response);
      });
  }
  
  protected processCreateKodawariType(response: Response): Promise<any> {
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

  updateKodawariType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KodawariTypes/update-kodawari-type-by-id`;
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
          return this.processUpdateKodawariType(_response);
      });
  }
  
  protected processUpdateKodawariType(response: Response): Promise<any> {
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
  
  deleteKodawariType(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/KodawariTypes/delete-kodawari-types`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            kodawariTypeId: id,
            isDeleteKodawariType: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteKodawariType(_response);
      });
  }

  protected processDeleteKodawariType(response: Response): Promise<any> {
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
  // End Kodawari Type
    
  // Key Peace Of Mind
  getAllKeyPeaceOfMind(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/KeyPieceOfMinds/get-all-key-piece-of-minds?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllKeyPeaceOfMind(_response);
      });
  }
  
  protected processGetAllKeyPeaceOfMind(response: Response): Promise<any> {
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

  createKeyPeaceOfMind(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KeyPieceOfMinds/insert-key-piece-of-minds`;
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
          return this.processCreateKeyPeaceOfMind(_response);
      });
  }
  
  protected processCreateKeyPeaceOfMind(response: Response): Promise<any> {
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

  updateKeyPeaceOfMind(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KeyPieceOfMinds/update-key-piece-of-mind-by-id`;
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
          return this.processUpdateKeyPeaceOfMind(_response);
      });
  }
  
  protected processUpdateKeyPeaceOfMind(response: Response): Promise<any> {
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
  
  deleteKeyPeaceOfMind(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/KeyPieceOfMinds/delete-key-piece-of-minds`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            keyPieceOfMindId: id,
            isDeleteKeyPieceOfMind: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteKeyPeaceOfMind(_response);
      });
  }

  protected processDeleteKeyPeaceOfMind(response: Response): Promise<any> {
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
  // End Key Peace Of Mind

  // Key Peace Of Mind Type
  getAllKeyPeaceOfMindType(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/KeyPieceOfMindTypes/get-all-key-piece-of-mind-type?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllKeyPeaceOfMindType(_response);
      });
  }
  
  protected processGetAllKeyPeaceOfMindType(response: Response): Promise<any> {
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

  createKeyPeaceOfMindType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KeyPieceOfMindTypes/insert-key-piece-of-mind-type`;
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
          return this.processCreateKeyPeaceOfMindType(_response);
      });
  }
  
  protected processCreateKeyPeaceOfMindType(response: Response): Promise<any> {
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

  updateKeyPeaceOfMindType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KeyPieceOfMindTypes/update-key-piece-of-mind-type-by-id`;
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
          return this.processUpdateKeyPeaceOfMindType(_response);
      });
  }
  
  protected processUpdateKeyPeaceOfMindType(response: Response): Promise<any> {
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
  
  deleteKeyPeaceOfMindType(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/KeyPieceOfMindTypes/delete-key-piece-of-mind-type`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            keyPieceOfMindTypeId: id,
            isDeleteKeyPieceOfMindType: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteKeyPeaceOfMindType(_response);
      });
  }

  protected processDeleteKeyPeaceOfMindType(response: Response): Promise<any> {
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
  // End Key Peace Of Mind Type

    // Service Tips
  getAllServiceTips(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ServiceTips/get-all-service-tips?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllServiceTips(_response);
      });
  }
  
  protected processGetAllServiceTips(response: Response): Promise<any> {
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

  createServiceTips(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ServiceTips/insert-service-tips`;
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
          return this.processCreateServiceTips(_response);
      });
  }
  
  protected processCreateServiceTips(response: Response): Promise<any> {
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

  updateServiceTips(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ServiceTips/update-service-tip-by-id`;
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
          return this.processUpdateServiceTips(_response);
      });
  }
  
  protected processUpdateServiceTips(response: Response): Promise<any> {
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
  
  deleteServiceTips(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ServiceTips/delete-service-tips`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            serviceTipId: id,
            isDeleteServiceTip: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteServiceTips(_response);
      });
  }

  protected processDeleteServiceTips(response: Response): Promise<any> {
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
  // End Service Tips

  // Service Tips Type
  getAllServiceTipsType(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ServiceTipTypes/get-all-service-tip-types?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllServiceTipsType(_response);
      });
  }
  
  protected processGetAllServiceTipsType(response: Response): Promise<any> {
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

  createServiceTipsType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ServiceTipTypes/insert-service-tip-type`;
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
          return this.processCreateServiceTipsType(_response);
      });
  }
  
  protected processCreateServiceTipsType(response: Response): Promise<any> {
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

  updateServiceTipsType(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ServiceTipTypes/update-service-tip-type-by-id`;
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
          return this.processUpdateServiceTipsType(_response);
      });
  }
  
  protected processUpdateServiceTipsType(response: Response): Promise<any> {
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
  
  deleteServiceTipsType(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ServiceTipTypes/delete-service-tip-type`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            keyPieceOfMindTypeId: id,
            isDeleteKeyPieceOfMindType: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteServiceTipsType(_response);
      });
  }

  protected processDeleteServiceTipsType(response: Response): Promise<any> {
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
  // End Service Tips Type

  // Service Tips Menu
  getAllServiceTipsMenu(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/ServiceTipMenus/get-all-service-tip-menus?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllServiceTipsMenu(_response);
      });
  }
  
  protected processGetAllServiceTipsMenu(response: Response): Promise<any> {
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

  createServiceTipsMenu(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ServiceTipMenus/insert-service-tip-menu`;
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
          return this.processCreateServiceTipsMenu(_response);
      });
  }
  
  protected processCreateServiceTipsMenu(response: Response): Promise<any> {
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

  updateServiceTipsMenu(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/ServiceTipMenus/update-service-tip-menu-by-id`;
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
          return this.processUpdateServiceTipsMenu(_response);
      });
  }
  
  protected processUpdateServiceTipsMenu(response: Response): Promise<any> {
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
  
  deleteServiceTipsMenu(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/ServiceTipMenus/delete-service-tip-menu`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            serviceTipMenuId: id,
            isDeleteServiceTipMenu: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteServiceTipsMenu(_response);
      });
  }

  protected processDeleteServiceTipsMenu(response: Response): Promise<any> {
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
  // End Service Tips Menu
    
// KPOM Menu
  getAllKPOMMenu(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/KeyPieceOfMindMenus/get-all-key-piece-of-mind-menus?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllKPOMMenu(_response);
      });
  }
  
  protected processGetAllKPOMMenu(response: Response): Promise<any> {
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

  createKPOMMenu(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KeyPieceOfMindMenus/insert-key-piece-of-mind-menu`;
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
          return this.processCreateKPOMMenu(_response);
      });
  }
  
  protected processCreateKPOMMenu(response: Response): Promise<any> {
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

  updateKPOMMenu(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/KeyPieceOfMindMenus/update-key-piece-of-mind-menu-by-id`;
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
          return this.processUpdateKPOMMenu(_response);
      });
  }
  
  protected processUpdateKPOMMenu(response: Response): Promise<any> {
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
  
  deleteKPOMMenu(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/KeyPieceOfMindMenus/delete-key-piece-of-mind-menu`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            keyPieceOfMindMenuId: id,
            isDeleteKeyPieceOfMindMenu: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteKPOMMenu(_response);
      });
  }

  protected processDeleteKPOMMenu(response: Response): Promise<any> {
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
  // End KPOM Menu
    
  // Service Tips Menu
  getAllHOUploadGuideline(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/HOGuidelines/get-all-ho-guideline-uploads?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllHOUploadGuideline(_response);
      });
  }
  
  protected processGetAllHOUploadGuideline(response: Response): Promise<any> {
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

  getAllHOGuideline(params?: any): Promise<any> {
    const query = cleanQueryToString(params);
    let url_ = this.baseUrl + `/api/v1/HOGuidelines/get-all-ho-guidelines?${query}`;
      url_ = url_.replace(/[?&]$/, "");

      let options_ = <RequestInit>{
          method: "GET",
          headers: {
              "Accept": "application/json"
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processGetAllHOGuideline(_response);
      });
  }
  
  protected processGetAllHOGuideline(response: Response): Promise<any> {
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

  createHOUploadGuideline(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/HOGuidelines/insert-ho-guideline-upload`;
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
          return this.processCreateHOUploadGuideline(_response);
      });
  }
  
  protected processCreateHOUploadGuideline(response: Response): Promise<any> {
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
    
  updateHOUploadGuideline(body?: any): Promise<any> {
  let url_ = this.baseUrl + `/api/v1/HOGuidelines/update-ho-guideline-by-id`;
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
        return this.processUpdateCommentHOUploadGuideline(_response);
    });
  }
  
  protected processUpdateHOUploadGuideline(response: Response): Promise<any> {
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

  updateCommentHOUploadGuideline(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/HOGuidelines/update-ho-guideline-comment-by-id`;
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
          return this.processUpdateCommentHOUploadGuideline(_response);
      });
  }
  
  protected processUpdateCommentHOUploadGuideline(response: Response): Promise<any> {
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

  updateApprovalHOUploadGuideline(body?: any): Promise<any> {
    let url_ = this.baseUrl + `/api/v1/HOGuidelines/update-ho-guideline-approve-by-id`;
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
          return this.processUpdateApprovalHOUploadGuideline(_response);
      });
  }
  
  protected processUpdateApprovalHOUploadGuideline(response: Response): Promise<any> {
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
  
  deleteHOUploadGuideline(id?: String): Promise<any> {
      let url_ = this.baseUrl + `/api/v1/HOGuidelines/delete-ho-guidelines`;
      url_ = url_.replace(/[?&]$/, "");

    let options_ = <RequestInit>{
          body: JSON.stringify({
            hoGuidelineId: id,
            isDeleteHOGuidelines: true
          }),
          method: "DELETE",
          headers: {
              "Accept": "application/json",
            "Content-Type": "application/json",
          }
      };

      return this.http.fetch(url_, options_).then((_response: Response) => {
          return this.processDeleteHOUploadGuideline(_response);
      });
  }

  protected processDeleteHOUploadGuideline(response: Response): Promise<any> {
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
  // End Service Tips Menu
}

