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
import { environment } from 'src/environments/environment';
import { IdUserService } from '../id-user.service';
import { UserE } from '../models/UserE';
import { createEventId, INITIAL_EVENTS } from './event-utils';
import { format } from 'date-fns'

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

  ngOnInit(): void {
    this.idUser = this.IdUserService.getIdUser();
    console.log('On schedule page with id:');
    console.log(this.idUser);
    this.GetUserInfo();
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
    this.SendUserID().subscribe((response) => {
      const responseBody = JSON.parse(response.body!);
      this.UserName = responseBody.name;
      this.UserSurname = responseBody.surname;
      this.UserPhoneNumber = responseBody.phoneNumber;
    });
  }

  SendUserID() {
    return this.http.get(
      `${environment.BaseUrl}/User/get-user-by-id?id=${this.idUser}`,
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
    const dialog = (this.myDialog5 as ElementRef).nativeElement;
    dialog.show();
    const closeListener = () => {
      this.title = this.CreateEventForm.get('title')?.value;
      this.location = this.CreateEventForm.get('location')?.value;

      this.SavingTitle(selectInfo);
      dialog.removeEventListener('close', closeListener);
    };
    dialog.addEventListener('close', closeListener);
    this.CreateEventForm.get('title')?.reset();
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
    this.currentEventClickedLOCATION = clickInfo.event.extendedProps['location']
    this.currentEventClickedDATEsub = clickInfo.event.start?.toString()!
    this.currentEventClickedDATE= this.currentEventClickedDATEsub.substring(0,15)// Wed Jan 18 2023
    const dateC=new Date(this.currentEventClickedDATE)
    const formatDate = new Date(dateC.toUTCString()).toISOString()
    console.log(formatDate)
    dialog.show();

    while (!this.deleteEvent) {
      await new Promise((resolve) => setTimeout(resolve, 500));
    }

    clickInfo.event.remove();
    this.deleteEvent = false;
    console.log(this.deleteEvent);
  }

  AddAvailability(){
    const dialog = (this.myDialog6 as ElementRef).nativeElement;
    dialog.show();
  }

  POSTAvailability(availability: any) {
    return this.http.post(`${environment.BaseUrl}/Availability/users/${this.idUser}/availability`, availability, {
      observe: 'response',
      responseType: 'text',
    });
  }
  
  handleEvents(events: EventApi[]) {
    this.currentEvents = events;
    this.changeDetector.detectChanges();
  }
}
