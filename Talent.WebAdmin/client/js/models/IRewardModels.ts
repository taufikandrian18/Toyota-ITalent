import { RewardTypeDropdownModel, ModuleDropdownModel, CoachDropdownModel, EventDropdownModel } from '../services/NSwagService';

export interface IRewardFormModel {
    Name: string;
    RewardType: RewardTypeDropdownModel;
    Module: ModuleDropdownModel;
    Coach: CoachDropdownModel;
    Event: EventDropdownModel;
    DateForm: {
        start: Date;
        end: Date;
    };
    isTeaching: boolean;
    isTotal: boolean;
    isLearning: boolean;
    teachingRequiredPoint: number;
    totalRequiredPoint: number;
    learningRequiredPoint: number;
    Description: string;
    TermsAndConditions: string;
    HowToUse: string;
    isActive: boolean
}

export interface IRewardFilterModel {
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

    RewardType?: number;
    IsActive?: boolean;
    RewardName?: string;
    PointType?: number;
}