interface ICompetencyMappingModels {
    Date?: {
        start? : Date;
        end? : Date;
    }
    CompetencyName?: string,
    CompetencyMappingCode?: string,
    TypeofEvalutaion?: string,
    SortBy?: string,
    PageIndex: number,
    PageSize: number
}
