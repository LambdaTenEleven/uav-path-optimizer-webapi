export class Page<T> {
  pageNumber: number;
  pageSize: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
  items: T[];

  constructor(pageNumber: number, pageSize: number, totalPages: number, totalCount: number, hasPreviousPage: boolean, hasNextPage: boolean, items: T[]) {
    this.pageNumber = pageNumber;
    this.pageSize = pageSize;
    this.totalPages = totalPages;
    this.totalCount = totalCount;
    this.hasPreviousPage = hasPreviousPage;
    this.hasNextPage = hasNextPage;
    this.items = items;
  }
}
