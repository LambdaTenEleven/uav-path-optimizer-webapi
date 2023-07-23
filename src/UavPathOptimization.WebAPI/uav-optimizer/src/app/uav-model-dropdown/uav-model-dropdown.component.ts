import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ApiService } from '../api.service';
import { Subject } from "rxjs";

@Component({
  selector: 'app-uav-model-dropdown',
  templateUrl: './uav-model-dropdown.component.html',
  styleUrls: ['./uav-model-dropdown.component.css']
})
export class UavModelDropdownComponent implements OnInit {
  @Output() selectedUavModelEvent: EventEmitter<any> = new EventEmitter<any>();
  @Input() pageSize = 10;

  uavModels: any[] = [];
  pageNumber = 1;
  hasNextPage: boolean = true;
  loading: boolean = false;
  selectedUavModel: any;
  typeahead: Subject<string> = new Subject<string>();
  searchKeyword: string = '';
  firstOpen: boolean = true;

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.typeahead.subscribe((text) => {
      this.searchKeyword = text;
      console.log("Search keyword:", this.searchKeyword);
    });
  }

  loadUavModels(keyword: string = '') {
    this.loading = true;
    this.apiService.getUavModels(this.pageNumber, this.pageSize, keyword, 'Name', 0).subscribe(
      response => {
        console.log("UAV models response:", response)
        this.loading = false;

        if (this.firstOpen) {
          this.uavModels = response.items;
          this.firstOpen = false;
        } else {
          for (const element of response.items) {
            this.uavModels = [...this.uavModels, element];
          }
        }

        this.hasNextPage = response.hasNextPage;
        console.log("UAV Models:", this.uavModels);
        console.log("Has next page:", this.hasNextPage);
      }
    )
  }

  scrollToEnd() {
    if (this.hasNextPage) {
      this.pageNumber++;
      this.loadUavModels(this.searchKeyword);
      console.log("Scrolled to the end, loading page number: ", this.pageNumber);
    } else {
      console.log("No more pages to load");
    }
  }

  open() {
    if (this.firstOpen || this.hasNextPage) {
      this.loadUavModels();
    }
  }

  close() {
    this.selectedUavModelEvent.emit(this.selectedUavModel);
    // Clear the item list and pagination information when the dropdown is closed
    this.uavModels = [];
    this.pageNumber = 1;
    this.hasNextPage = true;
  }

  search() {
    this.pageNumber = 1;
    this.uavModels = [];
    this.loadUavModels(this.searchKeyword);
  }
}
