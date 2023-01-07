import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class IdUserService {
  private idUser: number = 0;

  constructor() { }

  setIdUser(id: number) {
    this.idUser = id;
  }

  getIdUser(): number {
    return this.idUser;
  }
}