import { Component, Input, Output, EventEmitter, SimpleChanges, OnInit, OnChanges } from "@angular/core";

export class PaginatorChangeEvent {
  page: number;
  pageSize: number;
}

@Component({
  selector: 'ab-paginator',
  templateUrl: 'paginator.component.html',
  styleUrls: ['paginator.component.scss']
})
export class PaginatorComponent implements OnInit, OnChanges {
  @Input()
  page: number;

  @Input()
  pageSize: number;

  @Input()
  totalCount: number;

  @Input()
  styles: string

  @Output()
  change: EventEmitter<PaginatorChangeEvent> = new EventEmitter<PaginatorChangeEvent>();

  numberOfPages: number = 0;
  offset: number = 0;
  pagesShowing: number = 4;
  pages: number[];

  ngOnInit() {
    this.calculate();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['totalCount'])
      this.calculate();
  }

  calculate() {
    this.numberOfPages = Math.ceil(this.totalCount / this.pageSize);
    this.offset = Math.max(this.page - Math.floor(this.pagesShowing / 2), 0);
    this.pages = range(this.offset, this.offset + this.pagesShowing)
      .filter(page => page < this.numberOfPages)
      .slice(0, this.pagesShowing);
  }

  changePage(page: number) {
    this.page = page;
    this.calculate();
    this.change.emit(<PaginatorChangeEvent>{
      page: this.page,
      pageSize: this.pageSize
    });
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.calculate();
    this.change.emit(<PaginatorChangeEvent>{
      page: this.page,
      pageSize: this.pageSize
    });
  }
}

function range(start: number, end: number, step = 1) {
  let result = [];
  for (let i = start; i < end; i += step)
    result.push(i);
  return result;
}