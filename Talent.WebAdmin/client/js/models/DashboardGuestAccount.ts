// models/DashboardGuestAccount.ts
export interface GetAccountsQueryModel {
  status: string,
  RegisterdDate: Date,
  DealerID: string,
  OutletID: string,
  PositionID: string,
  Page: number,
  Limit: number,
  Ascending: boolean,
  SortBy: string,
  SearchQuery: string,
  filetype?: string
}
