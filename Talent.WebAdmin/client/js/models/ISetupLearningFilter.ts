interface ISetupLearningFilter {
    Date?: {
        start?: Date;
        end?: Date;
    }
    LearningName: string,
    ProgramType: string
    LearningCategory: string,
    ApprovalStatus: string,

    SortBy?: string,
    PageIndex: number,
    PageSize: number
}
