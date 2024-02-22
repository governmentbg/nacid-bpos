import { Pipe, PipeTransform } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Pipe({
  name: 'nomenclatureTemplatePipe'
})
export class AbNomenclatureTemplatePipe implements PipeTransform {
  constructor(private sanitized: DomSanitizer) { }

  transform(item: any, textTemplate: string | null, textFilter?: string | null): any {
    let resultText = this.getResultText(item, textTemplate);
    if (textFilter) {
      textFilter = textFilter.replace(/[-[\]{}()*+?.,\\^$|#\s]/g, '\\$&');
      resultText = resultText.replace(new RegExp(textFilter, 'gi'), '<span style="text-decoration: underline">$&</span>');
    }

    return this.sanitized.bypassSecurityTrustHtml(resultText);
  }

  getResultText(item: any, textTemplate: string): string {
    let result: any;
    let resultText: string;
    if (textTemplate && item) {
      resultText = textTemplate;
      const regex = new RegExp('{(.*?)}', 'g');
      result = regex.exec(resultText);
      while (result) {
        const innerProperties = result[1].split('.');
        let innerFilterValue = innerProperties.length ? item : item[result[1]];
        innerProperties.forEach((key: string) => {
          if (innerFilterValue && innerFilterValue[key]) {
            innerFilterValue = innerFilterValue[key];
          } else {
            innerFilterValue = null;
          }
        });
        resultText = resultText.replace(result[0], innerFilterValue ? innerFilterValue : '');
        result = regex.exec(resultText);
      }
    } else if (item) {
      resultText = item.name;
    } else {
      resultText = '';
    }

    return resultText;
  }
}
