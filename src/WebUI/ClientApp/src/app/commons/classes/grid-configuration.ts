export class GridConfiguration {
    
    constructor(
        public FieldInfo: FieldInfo[],
        public ItemsPerPage: number[] = [0]){ //[0] for not paginate
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