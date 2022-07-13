<template>
    <div>
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="row">
            <div class="col-md-12">
                <h3><fa icon="chart-bar"></fa> Report > <span class="talent_font_red">Inbox</span></h3>

                <!--SEARCH-->
                <div class="d-flex align-items-end flex-column">
                    <div class="col-md-3">
                        <div class="row justify-content-between">
                            <div class="col-md-8 talent_nopadding">
                                <input class="form-control" v-model="Search" />
                            </div>
                            <div class="col-md-3 talent_nopadding d-flex justify-content-center">
                                <button class="btn btn-block talent_bg_blue talent_font_white" @click="fetch">Search</button>
                            </div>
                        </div>
                    </div>
                </div>
                <br />

                <div class="col-md-12 talent_bg_white talent_roundborder talent_bg_shadow talent_padding">

                    <div class="col-md-12 mb-3">
                        <div class="row">
                            <div class="col-md-2">
                                <b>Content Name</b>
                            </div>
                            <div class="col-md-6">
                                <b>Message</b>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-md-6">
                                        <b>From</b>
                                    </div>
                                    <div class="col-md-6">
                                        <b>Date</b>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div v-for="(inbox, index) in listInbox.listInbox">
                        <div class="col-md-12 talent_bg_white talent_roundborder talent_bg_shadow talent_padding talent_marginbottom talent_cursorpoint talent_zoomsmall" @click="getInboxApproval(index)" data-toggle="modal" data-target=".bd-example-modal-xl">
                            <div class="row">
                                <div class="col-md-2">
                                    <a :class="{ 'font-weight-bold' : !inbox.isRead}"><fa icon="inbox"></fa> {{inbox.title}}</a>
                                </div>
                                <div class="col-md-6 talent_textshorten">
                                    <a :class="{ 'font-weight-bold' : !inbox.isRead}">{{inbox.message}}</a>
                                </div>
                                <div class="col-md-4">
                                    <div class="row">
                                        <div class="col-md-6 talent_textshorten" :class="{ 'font-weight-bold' : !inbox.isRead}"><a>{{inbox.employeeName}}</a></div>
                                        <div class="col-md-6" :class="{ 'font-weight-bold' : !inbox.isRead}"><a>{{inbox.createdAt | dateFormat}}</a></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />

                    <!--Pagination-->
                    <div class="col-md-12 d-flex justify-content-center">
                        <paginate :currentPage.sync="PageNumber" :total-data=listInbox.totalData :limit-data=10 @update:currentPage="fetch()"></paginate>
                    </div>
                </div>

                <!--Modal-->

                <div class="modal fade bd-example-modal-xl" tabindex="-1" role="dialog" aria-labelledby="myExtraLargeModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-xl">
                        <div class="modal-content talent_roundborder">
                            <div class="modal-header talent_noborder">
                                <h5 class="modal-title" id="exampleModalLabel"><fa icon="inbox"></fa> <b>{{approvalDetail.title}}</b></h5>
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                    <span aria-hidden="true">&times;</span>
                                </button>
                            </div>

                            <div class="modal-body">
                                <div class="col-md-12 talent_borderbottom">
                                    <div class="col-md-12">
                                        <p>Dear {{approvalDetail.userPosition}},</p>
                                    </div>
                                    <div class="col-md-12">
                                        <p>{{approvalDetail.message}}</p>
                                    </div>
                                    <div class="col-md-12">
                                        <p>Best Regards,</p>
                                        <p>{{approvalDetail.userName}}</p>
                                    </div>
                                </div>
                                <br />
                                <div class="col-md-12 mb-4">
                                    <table class="table table-bordered table-responsive-md talent_table_padding talent_aligncenter">
                                        <thead>
                                            <tr>
                                                <th>Content Category</th>
                                                <th>Content Name</th>
                                                <th>Created By</th>
                                                <th>Created At</th>
                                                <th>Detail</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td class="py-2">{{approvalDetail.contentCategory}}</td>
                                                <td class="py-2">{{approvalDetail.contentName}}</td>
                                                <td class="py-2">{{approvalDetail.createdBy}}</td>
                                                <td class="py-2">{{approvalDetail.createdAt | dateFormat}}</td>
                                                <td class="talent_nopadding_important">
                                                    <button class="btn btn-block talent_bg_orange talent_font_white" @click="detailClicked()">
                                                        View Detail
                                                    </button>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-md-12">
                                    <textarea maxlength="255" class="form-control"
                                              style="height:150px;overflow-y:scroll;" name="description" v-model="rejectMessage" placeholder="Write message"></textarea>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <div class="col-md-12 d-flex justify-content-center">
                                    <div>
                                        <button type="button" class="btn talent_bg_red talent_font_white" data-dismiss="modal" @click="reject" :disabled="approvalDetail.isDone === true">Reject</button>
                                        <button type="button" class="btn talent_bg_green talent_font_white" data-dismiss="modal" @click="accept" :disabled="approvalDetail.isDone === true">Accept</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
        </div>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import { InboxService, InboxViewModel, ApprovalInboxModel, InboxRejectedModel, ContentService } from '../../services/NSwagService';

    @Component({
        created: async function (this: Inbox) {
            await this.initialize();
        }
    })
    export default class Inbox extends Vue {
        Service: InboxService = new InboxService();
        ApprovalService: ContentService = new ContentService();

        SortBy: string = '';
        PageNumber: number = 1;
        Search: string = '';

        indexModal: number = 0;

        approvalDetail: ApprovalInboxModel = {
            contentCategory: '',
            contentName: '',
            createdBy: '',
            approvalId: 0,
            linkToPage: '',
            createdAt: new Date(),
            inboxId: 0,
            isDone: false,
            message: '',
            title: ''
        }

        rejectMessage: string = '';

        isBusy: boolean = false;

        listInbox: InboxViewModel = {
            listInbox: [],
            totalData: 0
        }

        async fetch() {
            this.isBusy = true;
            this.rejectMessage = '';
            this.listInbox = await this.Service.getInbox(this.Search, this.SortBy, this.PageNumber);
            this.isBusy = false;
        }

        async initialize() {
            await this.fetch();
        }

        async getInboxApproval(index: number) {
            this.indexModal = index;
            this.listInbox.listInbox[index].isRead = true;
            try {
                this.approvalDetail = await this.Service.getApproval(this.listInbox.listInbox[index].approvalId, this.listInbox.listInbox[index].inboxId);
            }
            catch{
                console.log("Error to get data");
            }
        }

        async reject() {

            let rejectModel: InboxRejectedModel = {
                approvalId: this.listInbox.listInbox[this.indexModal].approvalId,
                message: this.rejectMessage
            }

            try {
                let result = await this.ApprovalService.rejectApprovalInbox(rejectModel);
            }
            catch{
                console.log("Failed to Reject");
            }
            await this.fetch();
        }

        async accept() {
            try {
                let result = await this.ApprovalService.acceptApprovalInbox(this.listInbox.listInbox[this.indexModal].approvalId, this.listInbox.listInbox[this.indexModal].inboxId);
            }
            catch{
                console.log("Failed to Accept");
            }

            await this.fetch();
        }

        detailClicked() {
            window.location.href = '/' + this.approvalDetail.linkToPage;
        }

    }
</script>
