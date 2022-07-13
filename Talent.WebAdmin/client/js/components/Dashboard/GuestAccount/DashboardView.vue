<template>
    <div class="row">
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="col">
            <h3>
                <fa icon="chart-pie"></fa> Dashboard >
                <span class="talent_font_red">Guest Account</span>
            </h3>
        </div>

        <div class="col-md-12 mt-4">
            <div>Account</div>
            <div style="font-size: 18px">
                <b>{{ isDealerHO ? 'Dealer HO' : 'TAM People' }}</b>
            </div>
        </div>

        <!-- First Row -->
        <div class="col-md-12 mt-4">
            <div class="row d-flex align-items-stretch">
                <div class="col-md-4 pr-2 d-flex">
                    <div
                        class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                    >
                        <h5>Total Guest Account</h5>
                        <hr />
                        <div class="col h-75 d-flex align-items-center">
                            <chart-total-guest-account
                                :data="summaries.series"
                                :labels="summaries.labels"
                                @isBusy="status => (isBusySummary = status)"
                            />
                        </div>
                    </div>
                </div>
                <div class="col-md-8 px-2 text-sm" v-if="!isDealerHO">
                    <div class="row m-0 p-0 h-100">
                        <div class="col-md-6 text-sm pr-3 m-0 h-100" v-if="!isDealerHO">
                            <div
                                class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow h-100"
                            >
                                <h5>Dealer</h5>
                                <hr />
                                <dealer-table
                                    :isRefresh="isRefreshDealers"
                                    @isBusy="status => (isBusyDealers = status)"
                                    @refreshDone="isRefreshDealers = false"
                                />
                            </div>
                        </div>
                        <div class="col-md-6 text-sm pl-3 m-0 h-100" v-if="!isDealerHO">
                            <div
                                class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow h-100"
                            >
                                <h5>Others</h5>
                                <hr />
                                <others-table
                                    :isRefresh="isRefreshOthers"
                                    @isBusy="status => (isBusyOthers = status)"
                                    @refreshDone="isRefreshOthers = false"
                                />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-8 pl-2 text-sm" v-else>
                        <div
                            class="w-100 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
                        >
                            <h5>Outlet</h5>
                            <outlet-table
                                :dealer="`1`"
                                :isRefresh="isRefreshOutlet"
                                @isBusy="status => (isBusyOutlet = status)"
                                @refreshDone="isRefreshOutlet = false"
                            />
                        </div>
                </div>
            </div>
        </div>
        <!-- Second Row -->
        <div class="col-md-12 mt-4">
          <div class="col-md-12">
            <account-table
                :dealer="isDealerHO ? `1` : ''"
                @edit="handleEdit"
                @detail="handleDetail"
                @isBusy="status => (isBusyAccounts = status)"
                @refresh="handleRefreshFromAccount"
            />
          </div>
        </div>
    </div>
</template>

<script>
import {
    DashboardGuestAccountService,
    UsersideProfileService
} from '../../../services/NSwagService';
import AccountTable from './AccountTable.vue';
import ChartTotalGuestAccount from './ChartTotalGuestAccount.vue';
import DealerTable from './DealerTable.vue';
import OthersTable from './OthersTable.vue';
import OutletTable from './OutletTable.vue';

export default {
    props: ['data', 'claims'],
    components: {
        DealerTable,
        ChartTotalGuestAccount,
        OthersTable,
        AccountTable,
        OutletTable
    },
    data() {
        return {
            isBusyDashboard: false,
            service: new DashboardGuestAccountService(),
            profileService: new UsersideProfileService(),
            summaries: {
                labels: [],
                series: []
            },
            isBusySummary: false,
            isBusyDealers: false,
            isBusyOthers: false,
            isBusyOutlet: false,
            isBusyAccounts: false,
            selectedAccount: {},

            isRefreshSummary: false,
            isRefreshDealers: false,
            isRefreshOthers: false,
            isRefreshOutlet: false
        };
    },
    mounted() {
        this.fetchSummary();
    },
    computed: {
        isBusy() {
            return (
                this.isBusyDashboard ||
                this.isBusySummary ||
                this.isBusyDealers ||
                this.isBusyOthers ||
                this.isBusyAccounts || 
                this.isBusyOutlet
            );
        },
        isDealerHO() {
            return this.claims.Roles.includes('Dealer HO');
        }
    },
    methods: {
        async fetchSummary() {
            try {
                const res = await this.service.getChartSummary(this.claims.Roles.includes('Dealer HO') ? '1' : '');
                this.mapSummary(res.data);
            } catch (err) {
                console.log(err);
            }
        },
        mapSummary(data) {
            const summaries = {
                labels: ['Upgraded', 'Verified', 'Unverified'],
                series: [ 0, 0, 0]
            };
            data.forEach(v => {
                switch (v.status) {
                    case 'Suspended':
                        // summaries.series[0] = v.sumDone;
                        break;
                    case 'Upgraded':
                        summaries.series[0] = v.sumDone;
                        break;
                    case 'Verified':
                        summaries.series[1] = v.sumDone;
                        break;
                    case 'Unverified':
                        summaries.series[2] = v.sumDone;
                        break;
                    default:
                        summaries.series[2] += v.status ? v.sum : 0;
                        break;
                }
            });
            this.summaries = summaries;
        },
        async handleDetail(data) {
            this.isBusyDashboard = true;
            try {
                const res = await this.profileService.getProfilAccount(
                    data.employeeID
                );
                this.selectedAccount = res;
                this.$emit('navigation', {
                    layout: 'detail',
                    selectedAccount: { ...data, ...res }
                });
            } catch (err) {
                console.log(err);
            }
            this.isBusyDashboard = false;
        },
        async handleEdit(data) {
            this.isBusyDashboard = true;
            try {
                const res = await this.profileService.getProfilAccount(
                    data.employeeID
                );
                this.selectedAccount = res;
                this.$emit('navigation', {
                    layout: 'edit',
                    selectedAccount: { ...data, ...res }
                });
            } catch (err) {
                console.log(err);
            }
            this.isBusyDashboard = false;
        },
        handleRefreshFromAccount () {
            console.log('called')
            this.isRefreshDealers = true
            this.isRefreshOthers = true
            this.isRefreshOutlet = true
        }
    }
};
</script>

<style></style>
