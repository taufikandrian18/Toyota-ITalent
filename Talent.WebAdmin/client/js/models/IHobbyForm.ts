interface IHobbyForm {
    hobbyName: string,
    hobbyDescription: string
}

interface IHobbyFilter {
    sortBy: string,
    pageIndex: number,
    pageSize: number,
    hobbyName: string,
}