// services/ helper.ts
export const queryToString = (params) => {
  return Object.keys(params)
    .map(key => `${encodeURIComponent(key)}=${params[key]}`)
    .join('&');
}

export const cleanQueryToString = (params) => {

  const obj = {...params}
  for (var propName in obj) {
    if (obj[propName] == false) {
      
    } else
    if (obj[propName] === null || obj[propName] === undefined || obj[propName] === '' || obj[propName].length === 0 ) {
      delete obj[propName];
    }
  }

  return Object.keys(obj)
    .map(key => `${encodeURIComponent(key)}=${params[key]}`)
    .join('&');
}