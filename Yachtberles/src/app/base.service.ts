import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class BaseService {
  yachtsApi="https://p161-7ddfd-default-rtdb.europewest1.firebasedatabase.app/yachts.json"
  constructor(private http:HttpClient) { }
    getYachts(){
    return this.http.get(this.yachtsApi)
  }
  postYachtRental(body:any){
    return this.http.post("https://localhost:7086/api/Berlesek", body)
  }
}