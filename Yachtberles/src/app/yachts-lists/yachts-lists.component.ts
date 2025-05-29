import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { NgbCalendar, NgbDate, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { BaseService } from '../base.service';

@Component({
  selector: 'app-yachts-lists',
  templateUrl: './yachts-lists.component.html',
  styleUrl: './yachts-lists.component.css'
})
export class YachtsListsComponent {
  calendar = inject(NgbCalendar);
  date: NgbDate | null = null;
  hoveredDate: NgbDate | null = null;
  fromDate: NgbDate = this.calendar.getNext(this.calendar.getToday(),'d',1);
  toDate: NgbDate |null = this.calendar.getNext(this.fromDate, 'd', 2);
  
  yachts:any
  newRental:any={}
  // rentals:any
  error=false
  success=false
  errorMessage:any
  cart:any=null
  
  
  constructor(private base:BaseService){
    this.base.getYachts().subscribe(
      (j)=>this.yachts=j
    )
  }
  
  addCart(yacht:any){
    this.cart=yacht;  
  }
  
  deleteItem(){
    this.cart=null
  }
  
  
  
  postRental(){
   const body={
      uid:"101",
      startDate: this.ngbDateToDateString(this.fromDate),
      endDate: this.toDate?this.ngbDateToDateString(this.toDate):0,
      yachtId:this.cart.id,
      dailyRate:this.cart.daily_price,
      baseFee:this.cart.base_fee
      }
   
    console.log(body)
    this.base.postYachtRental(body).subscribe(
      {
        next:(a)=>{
          this.error=false
          this.success=true;
      },
      error: (e: HttpErrorResponse) => {
            console.log("hiba", e)
            this.success = false;
            this.error = true;              
            const statusCode = e.status;      
           
            const serverMessage = typeof e.error === 'string'
              ? e.error
              : e.error?.message || 'An unknown error occurred.' 
            this.errorMessage=serverMessage             
      }})  
  }
  
  onDateSelection(date: NgbDate) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
    } else if (this.fromDate && !this.toDate && date.after(this.fromDate)) {
      this.toDate = date;
    } else {
      this.toDate = null;
      this.fromDate = date;
    }
  }
  
  isHovered(date: NgbDate) {
    return (
      this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate)
    );
  }
  
  isInside(date: NgbDate) {
    return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
  }
  
  isRange(date: NgbDate) {
    return (
      date.equals(this.fromDate) ||
      (this.toDate && date.equals(this.toDate)) ||
      this.isInside(date) ||
      this.isHovered(date)
    );
  }
  
  ngbDateToDateString(ngbDate: NgbDateStruct): string {
    const year = ngbDate.year;
    const month = String(ngbDate.month).padStart(2, '0');
    const day = String(ngbDate.day).padStart(2, '0');
    const s= `${year}-${month}-${day}`
    return s; 
  }
}

