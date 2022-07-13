interface IUserPrivilegeSettingsFilter {
    Date?: {
        start?: Date,
        end?: Date,
    },
    MenuId?: string,
    UserRole?: string,
    PageId?: string,

    SortBy?: string,
    PageIndex: number,
    PageSize: number
}
