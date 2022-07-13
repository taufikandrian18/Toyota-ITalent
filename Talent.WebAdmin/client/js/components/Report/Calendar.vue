<template>
    <div>
        <loading-overlay :loading="isLoading"></loading-overlay>

        <div class="row">
            <div class="col-md-12">
                <h3 class="mb-md-4"><fa icon="chart-bar"></fa> Report > <span class="talent_font_red">Calendar</span></h3>

                <div class="talent_bg_white talent_padding talent_bg_shadow talent_roundborder">
                    <!--untuk pindah ke tanggal yang diinginkan-->

                    <div class="d-flex justify-content-between">
                        <div class="input-group">
                            <v-date-picker v-model="datePick" :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                            <div class="input-group-append">
                                <button class="btn btn-primary" type="button" @click="goToDate">Go To Date</button>
                            </div>
                        </div>

                        <div>
                            <button class="btn btn-secondary" type="button" @click="goBackDate">Return</button>
                        </div>
                    </div>


                    <FullCalendar class="mw-100 talent_calendarscheduler mt-md-5"
                                  defaultView="dayGridMonth"
                                  ref="fullCalendar"
                                  :displayEventTime="false"
                                  :plugins="calendarPlugins"
                                  id="calendar"
                                  :header="{
                                    left: 'prev,',
                                    center: 'title',
                                    right: 'next'
                                    }"
                                  :events="eventsDB"
                                  :eventLimit="true"
                                  :height="850"
                                  :firstDay="1"
                                  locale='er' />
                    <!--jngn di pake fitur bikin susah-->
                    <!--:editable="true"-->
                    <!--:header="{
                    left: 'prev, today',
                    center: 'title',
                    right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek, next'
                    }"-->
                </div>
            </div>
        </div>


    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import dayGridPlugin from '@fullcalendar/daygrid';
    import interactionPlugin from '@fullcalendar/interaction';
    import { DateTime } from 'luxon';
    import { CalendarService, CalendarScheduleCourseModel } from '../../services/NSwagService';


    @Component({
        props: [],
        created: async function (this: Calendars) {
            await this.fetch();
        }
    })

    export default class Calendars extends Vue {
        calMan: CalendarService = new CalendarService();

        isLoading: boolean = false;

        calendarPlugins: any = [
            dayGridPlugin,
            interactionPlugin
        ];

        datePick: Date = null;

        //cara bikin jadi format iso!
        tanggal: any = DateTime.local().toISO()

        eventsDB: CalendarScheduleCourseModel[] = [];

        async fetch() {
            this.isLoading = true;

            this.eventsDB = await this.calMan.getAllCourseSchedule();

            this.isLoading = false;
        }

        goToDate() {
            if (this.datePick !== null) {
                let api = (<any>this.$refs.fullCalendar).getApi();
                api.gotoDate(this.datePick);
            }
        }

        goBackDate() {
            let api = (<any>this.$refs.fullCalendar).getApi();
            api.gotoDate(new Date());
        }

    }
</script>