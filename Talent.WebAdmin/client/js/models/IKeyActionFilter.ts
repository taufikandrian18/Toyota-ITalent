interface IKeyActionFilter {
    SortBy?: string;
    PageNumber: number;

    DateFilter?: {
        start?: Date;
        end?: Date
    }

    KeyActionCode?: string;
    KeyActionName?: string;
}

interface IDigitalSignatureFilter {
    EmployeeName?: string;
    SortBy?: string;
    PageNumber: number;
}

interface ICoachesFilter {
    SortBy?: string;
    PageNumber: number;

    CoachSchedule?: {
        start?: Date;
        end?: Date;
    }

    DateFilter?: {
        start?: Date;
        end?: Date;
    }

    CoachName?: string;
    TopicName?: string;
    CoachIsActive?: boolean;
    EbadgeName?: string;

}

interface ITaskSearchModel {
    TaskCode: string;
    QuestionTypeId: number;
    ModuleName: string;
    CreateBy: string;
    DateFilter: {
        start?: Date;
        end?: Date;
    }
    SortBy?: string;
    PageIndex: number;
    pageSize: number;
    taskId: number;
}
