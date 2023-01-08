import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CalendarOptions, DateSelectArg, EventApi, EventClickArg } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import interactionPlugin from '@fullcalendar/interaction';
import listPlugin from '@fullcalendar/list';
import timeGridPlugin from '@fullcalendar/timegrid';
import { createEventId, INITIAL_EVENTS } from './event-utils';
import { IdUserService } from '../id-user.service';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { FormControl, FormGroup } from '@angular/forms';
import { UserE } from '../models/UserE';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})

export class ScheduleComponent implements OnInit {
  idUser: number = 0;
  UserName: string = "default";
  UserSurname: string = "default";
  UserPhoneNumber: string = "default";
  success = false;
  failure = false;


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

  FinishEdit() {
    let UserE: UserE = {
      idUser: this.idUser,
      name: this.EditForm.get('name')?.value,
      surname: this.EditForm.get('surname')?.value,
      phoneNumber: this.EditForm.get('phoneNumber')?.value,
    }
    this.EditUser(UserE).subscribe((response) => {
      if (response.statusText == "OK") {
        this.success=true
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
  }

  handleCalendarToggle() {
    this.calendarVisible = !this.calendarVisible;
  }

  handleWeekendsToggle() {
    const { calendarOptions } = this;
    calendarOptions.weekends = !calendarOptions.weekends;
  }

  handleDateSelect(selectInfo: DateSelectArg) {
    const title = prompt('Please enter a new title for your event');
    const calendarApi = selectInfo.view.calendar;

    calendarApi.unselect(); // clear date selection

    if (title) {
      calendarApi.addEvent({
        id: createEventId(),
        title,
        start: selectInfo.startStr,
        end: selectInfo.endStr,
        allDay: selectInfo.allDay
      });
    }
  }

  handleEventClick(clickInfo: EventClickArg) {
    if (confirm(`Are you sure you want to delete the event '${clickInfo.event.title}'`)) {
      clickInfo.event.remove();
    }
  }

  handleEvents(events: EventApi[]) {
    this.currentEvents = events;
    this.changeDetector.detectChanges();
  }
}
