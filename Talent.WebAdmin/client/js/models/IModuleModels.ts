import { TopicDropdownModel, FileContent } from '../services/NSwagService';

export interface IModuleFilter {
    /*
     * page filter sorting
     * */
    sortBy?: string;

    /*
     * current page pagination
     * */
    pageIndex: number;

    /*
     * page data size for pagination
     * how many data in 1 page
     * */
    pageSize: number;

    /*
     * store date filter range from start to end
     * */
    DateFilter?: {
        start?: Date;
        end?: Date;
    }

    /*
     * store module name filter
     * */
    ModuleName: string;

    MaterialTypeId: number;

    TopicName: string;

    ApprovalStatus: number;
}

export interface IModuleFormModel {
    ModuleName: string;
    ModuleDescription: string;
    ModuleFile: FileContent;
    MaterialTypeId: number;
    MaterialFile: FileContent;
    MaterialLink: string;
    Topics: TopicDropdownModel[];
    isDownloadable: boolean;
}