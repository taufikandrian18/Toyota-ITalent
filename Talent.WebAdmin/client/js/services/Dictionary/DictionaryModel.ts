export interface IParamDictionaries {
  StartDate?: string,
  EndData?: string,
  DictionaryName?: string,
  SortBy?: string,
  PageIndex: number,
  PageSize: number,
  ApprovedAt?: boolean
}

export interface DictionaryModel {
  dictionaryId: string,
  blobId: string,
  dictionaryName: string,
  dictionaryStatus: true,
  isFavorite: boolean,
  dictionaryArti: string,
  dictionaryManfaat: string,
  dictionaryFakta: string,
  dictionaryBasicOperation: string,
  createdAt: string,
  createdBy: string,
  updatedAt: string,
  updatedBy: string,
  isDeleted: boolean,
  file?: string,
  imageUrl: string,
  approvedAt?: any,
  blob: any
}

export interface IAllDictionaries {
  dIctionaries: DictionaryModel[],
  totalDictionaries: number
}

export interface CreateDictionaryModel {
  dictionaryId?: string,
  dictionaryName: string,
  dictionaryArti: string,
  dictionaryManfaat: string,
  dictionaryFakta: string,
  dictionaryStatus: boolean,
  productFileContent?: FileModel,
  dictionaryBasicOperation: string,
  approvedAt?: any
}

export interface FileModel {
  base64: string,
  fileName: string,
  contentType: string
}