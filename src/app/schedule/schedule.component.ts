import { HttpClient } from '@angular/common/http';
import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import {
  CalendarOptions,
  DateSelectArg,
  EventApi,
  EventClickArg,
} from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import listPlugin from '@fullcalendar/list';
import timeGridPlugin from '@fullcalendar/timegrid';
import { filter } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IdUserService } from '../id-user.service';
import { GETavailabilities } from '../models/GETavailabilities';
import { POSTavailability } from '../models/POSTavailability';
import { UserE } from '../models/UserE';
import { createEventId, INITIAL_EVENTS } from './event-utils';
import { TeaTime } from '../models/TeaTime';
@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent implements OnInit {
  @ViewChild('myDialog5') myDialog5?: ElementRef;
  @ViewChild('myDialog6') myDialog6?: ElementRef;
  @ViewChild('myDialog2') myDialog2?: ElementRef;

  idUser: number = 0;
  UserName: string = 'default';
  location: string = 'default';
  UserSurname: string = 'default';
  UserPhoneNumber: string = 'default';
  success = false;
  failure = false;
  CreateEvent = false;
  DoneCreateEvent = false;
  title: string = 'default';
  currentEventClicked: string = 'default';
  currentEventClickedLOCATION: string = 'default';
  currentEventClickedDATE: string = 'default'!;
  currentEventClickedDATEsub: string = 'default';
  deleteEvent = false;
  createEvent = false;
  Participant = false;
  ParticipantDONE = false;
  names: string[] = [];
  selectedOption: any;
  selectedid: string = 'default';
  // ------
  finaldate: string = 'default';
  finaldate2: string = 'default';
  idp: number = 0;
  FilteredListAvailabilities: Array<{
    idAvailability: number;
    idUser: number;
    idParticipant: number;
    location: string;
    type: number;
    startDate: string;
    apiUser: {
      idUser: number;
      name: string;
      surname: string;
      phoneNumber: string;
      mailAddress: string;
    };
  }> = [];
  saving(id: any, teatime: any) {
    return this.http.post(
      `${environment.BaseUrl}/Availability/users/${id}/TeaTime`,
      teatime,
      {
        observe: 'response',
        responseType: 'text',
      }
    );
  }
  ngOnInit(): void {
    this.idUser = this.IdUserService.getIdUser();
    console.log('On schedule page with id:');
    console.log(this.idUser);
    this.GetUserInfo();
    this.GETavailabilityByUSER(this.idUser).subscribe((response) => {
      this.FilteredListAvailabilities = response;
      console.log(this.FilteredListAvailabilities);
    });
  }

  GETallAvailabilities() {
    return this.http.get<GETavailabilities[]>(
      `${environment.BaseUrl}/Availability/get-all-availabilities`,
      {}
    );
  }
  GETavailabilityByTYPE(type: any) {
    return this.http.get<GETavailabilities[]>(
      `${environment.BaseUrl}/Availability/get-availabilities-by-type?type=${type}`,
      {}
    );
  }
  GETavailabilityByUSER(user: any) {
    return this.http.get<GETavailabilities[]>(
      `${environment.BaseUrl}/Availability/get-all-availabilities-by-user?idUser=${user}`,
      {}
    );
  }
  GETavailabilityByTIME(time: any) {
    return this.http.get<GETavailabilities[]>(
      `${environment.BaseUrl}/Availability/get-availabilities-by-date-and-time?dateTime=${time}`,
      {}
    );
  }
  DELETEavailability(id: any) {
    return this.http.delete(
      `${environment.BaseUrl}/Availability/availability/${id}`,
      {
        observe: 'response',
        responseType: 'text',
      }
    );
  }
  reset() {
    this.success = false;
    this.failure = false;
  }

  EditForm = new FormGroup({
    name: new FormControl(),
    surname: new FormControl(),
    phoneNumber: new FormControl(),
  });
  CreateEventForm = new FormGroup({
    title: new FormControl(),
    location: new FormControl(),
    hourStart: new FormControl(),
    hourEnd: new FormControl(),
  });
  SendAvailability = new FormGroup({
    date: new FormControl(),
    time: new FormControl(),
  });
  FinishEdit() {
    let UserE: UserE = {
      idUser: this.idUser,
      name: this.EditForm.get('name')?.value,
      surname: this.EditForm.get('surname')?.value,
      phoneNumber: this.EditForm.get('phoneNumber')?.value,
    };
    this.EditUser(UserE).subscribe((response) => {
      if (response.statusText == 'OK') {
        this.success = true;
      } else {
        this.failure = false;
      }
    });
  }

  EditUser(UserE: any) {
    return this.http.put(`${environment.BaseUrl}/User/edit-user`, UserE, {
      observe: 'response',
      responseType: 'text',
    });
  }

  GetUserInfo() {
    this.SendUserID(this.idUser).subscribe((response) => {
      const responseBody = JSON.parse(response.body!);
      this.UserName = responseBody.name;
      this.UserSurname = responseBody.surname;
      this.UserPhoneNumber = responseBody.phoneNumber;
    });
  }

  SendUserID(idUser: any) {
    return this.http.get(
      `${environment.BaseUrl}/User/get-user-by-id?id=${idUser}`,
      {
        observe: 'response',
        responseType: 'text',
      }
    );
  }

  GETuserbyNAME(name: any) {
    return this.http.get(
      `${environment.BaseUrl}/User/get-user-by-name?name=${name}`,
      {
        observe: 'response',
        responseType: 'text',
      }
    );
  }

  calendarVisible = true;
  calendarOptions: CalendarOptions = {
    plugins: [dayGridPlugin, timeGridPlugin, listPlugin, interactionPlugin],
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek',
    },
    initialView: 'dayGridMonth',
    initialEvents: INITIAL_EVENTS,
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    locale: 'ro',
    select: this.handleDateSelect.bind(this),
    eventClick: this.handleEventClick.bind(this),
    eventsSet: this.handleEvents.bind(this),
  };
  currentEvents: EventApi[] = [];

  constructor(
    private changeDetector: ChangeDetectorRef,
    private IdUserService: IdUserService,
    private http: HttpClient
  ) {
    this.myDialog5 = new ElementRef(null);
  }

  handleCalendarToggle() {
    this.calendarVisible = !this.calendarVisible;
  }

  handleWeekendsToggle() {
    const { calendarOptions } = this;
    calendarOptions.weekends = !calendarOptions.weekends;
  }

  handleDateSelect(selectInfo: DateSelectArg) {
    this.names.length = 0;
    const dialog = (this.myDialog5 as ElementRef).nativeElement;
    dialog.show();
    const closeListener = () => {
      this.title = this.CreateEventForm.get('title')?.value;
      this.location = this.CreateEventForm.get('location')?.value;

      const selectedDate = new Date(selectInfo.start);
      const hourStart =
        parseInt(this.CreateEventForm.get('hourStart')?.value) % 24;
      const minutes = selectedDate.getMinutes();
      selectedDate.setUTCHours(hourStart);
      selectedDate.setUTCMinutes(minutes);
      const finalDate = selectedDate.toISOString();
      this.finaldate = finalDate;
      console.log(this.finaldate);
      const selectedDate2 = new Date(selectInfo.start);
      const hourEnd = parseInt(this.CreateEventForm.get('hourEnd')?.value) % 24;
      const minutes2 = selectedDate2.getMinutes();
      selectedDate2.setUTCHours(hourEnd);
      selectedDate2.setUTCMinutes(minutes2);
      const finalDate2 = selectedDate2.toISOString();
      this.finaldate2 = finalDate2;
      console.log(this.finaldate2);
      this.GETallAvailabilities().subscribe((response) => {
        let filterLIST = response;
        filterLIST = filterLIST.filter(
          (availability) => availability.type === 0
        );
        filterLIST = filterLIST.filter(
          (availability) =>
            new Date(availability.startDate) > new Date(finalDate)
        );
        filterLIST = filterLIST.filter(
          (availability) =>
            new Date(availability.startDate) < new Date(finalDate2)
        );
        filterLIST.map((item) => {
          this.names.push(item.apiUser.name);
        });
      });

      this.SavingTitle(selectInfo);
      dialog.removeEventListener('close', closeListener);
    };

    dialog.addEventListener('close', closeListener);
    this.CreateEventForm.get('title')?.reset();
    this.CreateEventForm.get('location')?.reset();
    this.CreateEventForm.get('hourStart')?.reset();
    this.CreateEventForm.get('hourEnd')?.reset();
    this.Participant = false;
  }
  savingTeaTime() {
    this.GETuserbyNAME(this.selectedid).subscribe((response) => {
      if (response.statusText == 'OK' && response.body) {
        const responseBody = JSON.parse(response.body);
        this.idp = responseBody.idUser;
      }
      let teatime: TeaTime = {
        idParticipant: this.idp,
        location: this.location,
        startDate: this.finaldate,
      };
      const CurrentID = this.idUser;
      console.log('currentID', CurrentID, 'and teatime:', teatime);
      this.saving(CurrentID, teatime).subscribe((response) => {
        if (response.statusText == 'OK') {
          console.log('OK TEATIME');
        } else {
          console.log('BAD TEATIME');
        }
      });
      this.GETallAvailabilities().subscribe((response) => {
        let filterLIST = response;
        console.log('check: ', filterLIST);
        filterLIST = filterLIST.filter(
          (availability) => availability.idUser === this.idp
        );
        const idfordelete = filterLIST[0].idAvailability;
        console.log(idfordelete);
        console.log(filterLIST);
        this.DELETEavailability(idfordelete);
      });
    });
  }

  SavingTitle(selectInfo: DateSelectArg) {
    const titleINPUT = this.title;
    const calendarApi = selectInfo.view.calendar;
    const locationINPUT = this.location;
    calendarApi.unselect();
    if (titleINPUT) {
      calendarApi.addEvent({
        id: createEventId(),
        title: titleINPUT,
        start: selectInfo.startStr,
        end: selectInfo.endStr,
        allDay: selectInfo.allDay,
        location: locationINPUT,
      });
    }
  }

  async handleEventClick(clickInfo: EventClickArg) {
    const dialog = (this.myDialog6 as ElementRef).nativeElement;
    this.currentEventClicked = clickInfo.event.title;
    this.currentEventClickedLOCATION =
      clickInfo.event.extendedProps['location'];
    this.currentEventClickedDATEsub = clickInfo.event.start?.toString()!;
    this.currentEventClickedDATE = this.currentEventClickedDATEsub.substring(
      0,
      15
    );
    const dateC = new Date(this.currentEventClickedDATE);
    const formatDate = new Date(dateC.toUTCString()).toISOString();
    console.log(formatDate);
    dialog.show();

    while (!this.deleteEvent) {
      await new Promise((resolve) => setTimeout(resolve, 500));
    }

    clickInfo.event.remove();
    this.deleteEvent = false;
    console.log(this.deleteEvent);
  }

  AddAvailability() {
    const dialog = (this.myDialog2 as ElementRef).nativeElement;
    dialog.show();
    const date = this.SendAvailability.get('date')?.value;
    const time = this.SendAvailability.get('time')?.value;

    const [year, month, day] = date.split('-').map(Number);
    const [hours, minutes] = time.split(':').map(Number);
    const datetime = new Date(Date.UTC(year, month - 1, day, hours, minutes));

    const id = this.idUser;
    let POSTavailability: POSTavailability = {
      startDate: datetime.toISOString(),
    };
    console.log('id', POSTavailability, 'POSTavailability', POSTavailability);
    this.POSTAvailability(id, POSTavailability).subscribe((response) => {
      console.log(response);
    });
    dialog.close();
  }

  POSTAvailability(id: any, availability: any) {
    return this.http.post(
      `${environment.BaseUrl}/Availability/users/${id}/availability`,
      availability,
      {
        observe: 'response',
        responseType: 'text',
      }
    );
  }

  POSTteatime(id: any, teatime: any) {
    return this.http.post(
      `${environment.BaseUrl}/Availability/users/${id}/availability`,
      teatime,
      {
        observe: 'response',
        responseType: 'text',
      }
    );
  }
  handleEvents(events: EventApi[]) {
    this.currentEvents = events;
    this.changeDetector.detectChanges();
  }
}
