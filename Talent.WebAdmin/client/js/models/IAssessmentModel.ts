export interface IAssessmentFormModel {
    dealer: DealerDropdownModel,
    outlet: OutletDropdownModel,
    position: PositionDropdownModel,
    employee: EmployeeDropdownModel,
    assessmentName: string,
    publisher: string,
    description: string,
    fileContent: FileContent
}

export interface IAssessmentFilterModel {
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

    outletName: string;
    assessmentName: string;
    positionName: string;
    employeeName: string;
    publisher: string;
}

import { DealerDropdownModel, OutletDropdownModel, PositionDropdownModel, EmployeeDropdownModel, FileContent } from "../services/NSwagService";