export class GridConfiguration {
    
    constructor(
        public FieldInfo: FieldInfo[],
        public ItemsPerPage: number[],
        public AllowPagination: boolean = false){
    }
}

export class FieldInfo {

    constructor(
        public label: string,
        public property: string,
        public format: string,
        public allowSorting: boolean = false){
    }
}