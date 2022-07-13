interface IUserRoleFilter {
    SortBy?: string;
    pageIndex: number;
    pageSize: number;
    DateFilter?: {
        start?: Date;
        end?: Date
    }
    Position: string;
    UserRole: string;
    TypeOfPeople?: boolean;
}
