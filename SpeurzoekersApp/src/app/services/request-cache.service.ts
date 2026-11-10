import { Injectable } from '@angular/core';
import { HttpRequest, HttpResponse } from '@angular/common/http';

const maxage = 60000*60*24*7;


@Injectable({
  providedIn: 'root'
})
export class RequestCacheService {

  private cache = new Map();

  constructor() { }

  public get(request: HttpRequest<any>): HttpResponse<any> | undefined {
    const url = request.urlWithParams;
    const cached = this.cache.get(url);
    if(!cached) {
      return undefined;
    }
    const isExpired = cached.lastRead < (Date.now() - maxage);
    if(isExpired) {
      this.cache.delete(url);
      return undefined;
    }

    return cached.response;
  }

  public set(request: HttpRequest<any>, response: HttpResponse<any>): void {
    const url = request.url;
    if(url.includes("image") || url.includes("info")) {
      const entry = { url, response, lastRead: Date.now() };
      this.cache.set(url, entry);
    }
  }
}
