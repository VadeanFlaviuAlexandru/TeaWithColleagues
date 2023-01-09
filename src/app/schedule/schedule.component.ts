import { HttpClient } from '@angular/common/http';
import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { CalendarOptions, DateSelectArg, EventApi, EventClickArg } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import listPlugin from '@fullcalendar/list';
import timeGridPlugin from '@fullcalendar/timegrid';
import { environment } from 'src/environments/environment';
import { IdUserService } from '../id-user.service';
import { UserE } from '../models/UserE';
import { createEventId, INITIAL_EVENTS } from './event-utils';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})

export class ScheduleComponent implements OnInit {
  @ViewChild('myDialog5') myDialog5?: ElementRef;
  @ViewChild('myDialog6') myDialog6?: ElementRef;
  idUser: number = 0;
  UserName: string = "default";
  UserSurname: string = "default";
  UserPhoneNumber: string = "default";
  success = false;
  failure = false;
  CreateEvent = false;
  DoneCreateEvent = false;
  title: string = "default";
  currentEventClicked: string = "default";
  deleteEvent = false;
  createEvent = false;

  ngOnInit(): void {
    this.idUser = this.IdUserService.getIdUser();
    console.log("On schedule page with id:")
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
    phoneNumber: new FormControl()
  })
  CreateEventForm = new FormGroup({
    title: new FormControl(),
  })

  FinishEdit() {
    let UserE: UserE = {
      idUser: this.idUser,
      name: this.EditForm.get('name')?.value,
      surname: this.EditForm.get('surname')?.value,
      phoneNumber: this.EditForm.get('phoneNumber')?.value,
    }
    this.EditUser(UserE).subscribe((response) => {
      if (response.statusText == "OK") {
        this.success = true
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
    this.SendUserID().subscribe(response => {
      const responseBody = JSON.parse(response.body!);
      this.UserName = responseBody.name;
      this.UserSurname = responseBody.surname;
      this.UserPhoneNumber = responseBody.phoneNumber;
    });
  }

  SendUserID() {
    return this.http.get(`${environment.BaseUrl}/User/get-user-by-id?id=${this.idUser}`, {
      observe: 'response',
      responseType: 'text',
    });
  }

  calendarVisible = true;
  calendarOptions: CalendarOptions = {
    plugins: [
      dayGridPlugin,
      timeGridPlugin,
      listPlugin,
      interactionPlugin,
    ],
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
    },
    initialView: 'dayGridMonth',
    initialEvents: INITIAL_EVENTS, // alternatively, use the `events` setting to fetch from a feed
    weekends: true,
    editable: true,
    selectable: true,
    selectMirror: true,
    dayMaxEvents: true,
    locale: 'ro',
    select: this.handleDateSelect.bind(this),
    eventClick: this.handleEventClick.bind(this),
    eventsSet: this.handleEvents.bind(this)
    /* you can update a remote database when these fire:
    eventAdd:
    eventChange:
    eventRemove:
    */
  };
  currentEvents: EventApi[] = [];

  constructor(private changeDetector: ChangeDetectorRef, private IdUserService: IdUserService, private http: HttpClient) {
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
      this.title = this.CreateEventForm.get("title")?.value;
      this.SavingTitle(selectInfo);
      dialog.removeEventListener('close', closeListener);
    }
    dialog.addEventListener('close', closeListener);
    this.CreateEventForm.get("title")?.reset();
  }

  SavingTitle(selectInfo: DateSelectArg) {
    const titleGot = this.title
    const calendarApi = selectInfo.view.calendar;
    calendarApi.unselect(); // clear date selection
    if (titleGot) {
      calendarApi.addEvent({
        id: createEventId(),
        title: titleGot,
        start: selectInfo.startStr,
        end: selectInfo.endStr,
        allDay: selectInfo.allDay
      });
    }
  }

  async handleEventClick(clickInfo: EventClickArg) {
    const dialog = (this.myDialog6 as ElementRef).nativeElement;
    this.currentEventClicked = clickInfo.event.title;
    dialog.show();

    while (!this.deleteEvent) {
      await new Promise(resolve => setTimeout(resolve, 500));
    }

    clickInfo.event.remove();
    this.deleteEvent = false;
    console.log(this.deleteEvent)
  }

  handleEvents(events: EventApi[]) {
    this.currentEvents = events;
    this.changeDetector.detectChanges();
  }
}
