interface ITopicFilter {
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
     * store topic name filter
     * */
    TopicName: string;

    /*
     * store ebadge filter
     * */
    Ebadge?: number;

    /*
     * store minimum point filter
     * */
    MinPoint?: number;
}