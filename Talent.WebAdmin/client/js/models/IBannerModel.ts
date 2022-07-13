interface IBannerFilterModel {
    ApprovedBy?: string;
    BannerTitle?: string;
    TypeofBanner?: number;
    DateFilter: {
        start?: Date;
        end?: Date;
    };
    StartDate?: Date;
    EndDate?: Date;
    SortBy?: string;
    Status?: string;
}