interface ISetupTimePointModels {
    Date?: {
        start?: Date;
        end?: Date;
    }
    Time: number
    Points: number,
    PointTypeId: number,
    TypeofPoints?: string,
    Score: number,
    SortBy?: string,
    PageIndex: number,
    PageSize: number
}
