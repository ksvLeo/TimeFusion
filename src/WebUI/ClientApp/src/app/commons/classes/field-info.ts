export class FieldInfo {
    label: string;
    property: string;
    format: string;
    allowSorting: boolean;

    constructor(label: string, property: string, format: string, allowSorting: boolean = false){
        this.label = label;
        this.property = property;
        this.format = format;
        this.allowSorting = allowSorting;
    }
}
