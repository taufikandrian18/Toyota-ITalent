interface IReleaseTrainingFilter {
    courseName?: string,
    batch?: number,
    approvalStatusId?: number,
    dateFilter?: {
        start?: Date,
        end?: Date
    }
    enrollmentStartDate?: Date,
    enrollmentEndDate?: Date,
    sortBy?: string,
    pageNumber?: number
}