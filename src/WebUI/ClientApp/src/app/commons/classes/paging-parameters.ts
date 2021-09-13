export class PagingParameters {
    
    constructor(public PageNumber: number,public PageSize: number,public Order: PaginationOrder,public OrderField: string) {
        
    }
}

export enum PaginationOrder {
    ASC = 1,
    DESC = 2,
}