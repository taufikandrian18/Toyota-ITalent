interface ISurveyFilter {
    Date?: {
        start?: Date,
        end?: Date,
    },
    Title?: string,
    Position?: string,
    Outlet?: string,
    ApprovalId: number

    SortBy?: string,
    PageIndex: number,
    PageSize: number
}
