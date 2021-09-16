import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'filterItems',
    pure: false
})

export class FilterItemsPipe implements PipeTransform{
    transform(items: any [], filter: string, property: string): any {
        if(!items || !filter){
            return items;
        }
         
        return items.filter(c => c[property].toLowerCase().includes(filter.toLowerCase()));
    }
}