<template>
    <div class="row">
        <loading-overlay :loading="isBusy"></loading-overlay>

        <div class="col-md-12">
            <h3>
                <fa icon="database"></fa> Dashboard > Guest Account >
                <span class="talent_font_red">Detail Guest Account</span>
            </h3>
            <br />

            <div
                class="alert alert-success alert-dismissible fade show"
                role="alert"
                v-if="message !== ''"
            >
                {{ message }}
                <button
                    type="button"
                    class="close"
                    data-dismiss="alert"
                    aria-label="Close"
                    @click="handleMessageClose"
                >
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>

            <div
                class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
            >
                <div class="col-md-12">
                    <div class="d-flex justify-content-between">
                        <h4>Profile</h4>
                        <div class="dropdown mr-2">
                            <button
                                class="btn talent_bg_darkblue talent_font_white dropdown-toggle"
                                type="button"
                                id="dropdownMenuButton1"
                                data-toggle="dropdown"
                                aria-expanded="false"
                            >
                                Export to xls/csv
                            </button>
                            <ul
                                class="dropdown-menu"
                                aria-labelledby="dropdownMenuButton1"
                                style="min-width: 200px"
                            >
                                <li>
                                    <a
                                        class="dropdown-item"
                                        href="#"
                                        @click="handleDownloadXLS"
                                        >Export to xls</a
                                    >
                                </li>
                                <li>
                                    <a
                                        class="dropdown-item"
                                        href="#"
                                        @click="handleDownloadCSV"
                                        >Export to csv</a
                                    >
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <hr />
                <div class="row mt-4">
                    <div class="col-md-12">
                        <div class="row justify-content-between">
                            <!-- first -->
                            <div class="col-md-3">
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Employee ID</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.employee.nip !== null ? model.employee.nip : '-' }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <input
                                            class="form-control"
                                            v-model="model.employee.nip"
                                            :disabled="
                                                data.layout == 'edit'
                                                    ? false
                                                    : 'disabled'
                                            "
                                        />
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Name</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.employee.employeeName !== null ? model.employee.employeeName : '-' }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <input
                                            class="form-control"
                                            v-model="
                                                model.employee.employeeName
                                            "
                                            :disabled="
                                                data.layout == 'edit'
                                                    ? false
                                                    : 'disabled'
                                            "
                                        />
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Gender</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.employee.gender !== null ? model.employee.gender : '-' }}
                                    </div>
                                    <div
                                        class="input-group d-flex justify-content-between"
                                        v-else
                                    >
                                        <div
                                            :class="
                                                `col ${
                                                    model.employee.gender ===
                                                    'female'
                                                        ? 'gender-active'
                                                        : 'gender'
                                                } text-center mr-2`
                                            "
                                            @click="
                                                model.employee.gender = 'female'
                                            "
                                        >
                                            Female
                                        </div>
                                        <div
                                            :class="
                                                `col ${
                                                    model.employee.gender ===
                                                    'male'
                                                        ? 'gender-active'
                                                        : 'gender'
                                                } text-center ml-2`
                                            "
                                            @click="
                                                model.employee.gender = 'male'
                                            "
                                        >
                                            Male
                                        </div>
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Date Of Birth</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.employee.dateOfBirth !== null ? model.employee.dateOfBirth : '-' }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <input
                                            class="form-control"
                                            type="date"
                                            v-model="model.employee.dateOfBirth"
                                            :disabled="
                                                data.layout == 'edit'
                                                    ? false
                                                    : 'disabled'
                                            "
                                        />
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label class="font-weight-bold">ID Card No. (KTP)</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.employee.ktp !== null ? model.employee.ktp : '-' }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <input
                                            class="form-control"
                                            v-model="model.employee.ktp"
                                            :disabled="
                                                data.layout == 'edit'
                                                    ? false
                                                    : 'disabled'
                                            "
                                        />
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Religion</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.employee.religion !== null ? model.employee.religion : '-' }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <select
                                            class="form-control"
                                            name="religion"
                                            id=""
                                            v-model="model.employee.religion"
                                        >
                                            <option value=""
                                                >Select Religion</option
                                            >
                                            <option
                                                v-for="religion in religions"
                                                :value="religion"
                                                :key="religion"
                                                >{{ religion }}</option
                                            >
                                        </select>
                                    </div>
                                    <br />
                                </div>
                            </div>
                            <!-- second -->
                            <div class="col-md-3">
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Email</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{
                                            model.employee.employeeEmail !==
                                            null
                                                ? model.employee.employeeEmail
                                                : '-'
                                        }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <input
                                            class="form-control"
                                            v-model="
                                                model.employee.employeeEmail
                                            "
                                            :disabled="
                                                data.layout == 'edit'
                                                    ? false
                                                    : 'disabled'
                                            "
                                        />
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Phone Number</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.employee.employeePhone !== null ? model.employee.employeePhone : '-' }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <input
                                            class="form-control"
                                            v-model="
                                                model.employee.employeePhone
                                            "
                                            :disabled="
                                                data.layout == 'edit'
                                                    ? false
                                                    : 'disabled'
                                            "
                                        />
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Address</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.employee.address !== null ? model.employee.address : '-' }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <textarea
                                            class="form-control"
                                            rows="5"
                                            v-model="model.employee.address"
                                            :disabled="
                                                data.layout == 'edit'
                                                    ? false
                                                    : 'disabled'
                                            "
                                        />
                                    </div>
                                    <br />
                                </div>
                            </div>
                            <!-- third -->
                            <div class="col-md-3">
                                <div class="col-md-12">
                                    <label class="font-weight-bold">{{ model.dealer.isOthers ? 'Company' : 'Dealer' }}</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.dealer.dealerName !== null ? model.dealer.dealerName : '-' }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <multiselect
                                            v-model="selectedDealers"
                                            name="Dealer"
                                            :options="dealers"
                                            :allow-empty="true"
                                            label="dealerName"
                                            track-by="dealerName"
                                        >
                                        </multiselect>
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Outlet</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{ model.outlet.name !== null ? model.outlet.name : '-' }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <multiselect
                                            v-model="selectedOutlet"
                                            name="Outlet"
                                            :options="outlets"
                                            :allow-empty="true"
                                            label="outletName"
                                            track-by="outletName"
                                        >
                                        </multiselect>
                                    </div>
                                    <br />
                                </div>
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Position</label>
                                    <div
                                        class="input-group"
                                        v-if="data.layout !== 'edit'"
                                    >
                                        {{
                                            model.position.length > 0
                                                ? model.position[0].positionName
                                                : '-'
                                        }}
                                    </div>
                                    <div class="input-group" v-else>
                                        <multiselect
                                            v-model="selectedPosition"
                                            name="Position"
                                            :options="positions"
                                            :allow-empty="true"
                                            label="positionName"
                                            track-by="positionName"
                                        >
                                        </multiselect>
                                    </div>
                                    <br />
                                </div>
                                <div
                                    class="col-md-12"
                                    v-if="data.layout !== 'edit'"
                                >
                                    <label class="font-weight-bold">Validity Periode</label>
                                    <div class="input-group">
                                        {{ model.employee.endDate !== null ? formatDateShow(model.employee.endDate) : '-' }}
                                    </div>
                                    <br />
                                </div>
                                <div
                                    class="col-md-12"
                                    v-if="data.layout !== 'edit'"
                                >
                                    <label class="font-weight-bold">Status</label>
                                    <div class="input-group">
                                        {{ model.status !== null ? model.status : '-' }}
                                    </div>
                                    <br />
                                </div>
                                <div
                                    class="col-md-12"
                                    v-if="data.layout !== 'edit'"
                                >
                                    <label class="font-weight-bold">Request Upgrade</label>
                                    <div class="input-group">
                                        {{ model.employee.isRequestUpgrade ? "Requesting" : '-' }}
                                    </div>
                                    <br />
                                </div>
                                <div
                                    class="col-md-12"
                                    v-if="data.layout !== 'edit'"
                                >
                                    <label class="font-weight-bold">Suspended</label>
                                    <div class="input-group">
                                        {{ model.employee.isSuspended ? "Suspended" : '-' }}
                                    </div>
                                    <br />
                                </div>
                            </div>
                            <!-- fourth -->
                            <div class="col-md-3">
                                <div class="col-md-12">
                                    <label class="font-weight-bold">Profile Picture</label>
                                    <div class="input-group mt-1">
                                        <img
                                            class="w-100 rounded"
                                            :src="
                                                model.employee.imageUrl
                                                    ? model.employee.imageUrl
                                                    : '/upload-image2.png'
                                            "
                                            alt=""
                                        />
                                    </div>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <br />
            </div>
            <br />
            <div
                class="col-md-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow"
            >
                <div class="row">
                    <div class="col-md-12">
                        <div class="d-flex justify-content-between">
                            <div v-if="data.layout !== 'edit'">
                                <!-- data.layout == 'edit' -->
                                <button
                                    class="btn talent_bg_red talent_font_white mr-2"
                                    data-toggle="modal"
                                    data-target="#modalConfirmationSuspend"
                                >
                                    Suspend Account
                                </button>
                                <button
                                    class="btn btn-warning talent_font_white mr-2"
                                    data-toggle="modal"
                                    data-target="#modalConfirmationExtend"
                                >
                                    Extend Account
                                </button>
                                <!-- <button
                                    class="btn btn-success talent_font_white mr-2"
                                    data-toggle="modal"
                                    data-target="#modalConfirmationUpgraded"
                                >
                                    Upgrade Account
                                </button> -->
                                <button
                                    class="btn btn-info mr-2"
                                    data-toggle="modal"
                                    data-target="#modalConfirmationVerify"
                                >
                                    Verify Account
                                </button>
                            </div>
                            <div v-else>
                                <!-- data.layout == 'edit' -->
                                <button
                                    class="btn btn-info mr-2"
                                    @click="handleUpdate"
                                >
                                    Submit
                                </button>
                                <button
                                    class="btn btn-success talent_font_white mr-2"
                                    data-toggle="modal"
                                    data-target="#modalConfirmationUpgraded"
                                >
                                    Upgrade Account
                                </button>
                            </div>
                            <div>
                                <!-- <button v-if="fromOutside === true" class="btn talent_bg_red talent_font_white" @click.prevent="handleBack">Close</button> -->
                                <button
                                    class="btn talent_bg_red talent_font_white"
                                    @click.prevent="handleBack"
                                >
                                    Close
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <modal-confirmation
            name="modalConfirmationSuspend"
            message="Are you sure want to suspend this accounts?"
            @yes="handleConfirmationUpdateStatus('Suspended')"
        />

        <modal-confirmation
            name="modalConfirmationVerify"
            message="Are you sure want to verify this accounts?"
            @yes="handleConfirmationUpdateStatus('Verified')"
        />

        <modal-confirmation
            name="modalConfirmationExtend"
            message="Are you sure want to extend this accounts?"
            @yes="handleConfirmationUpdateStatus('Extend')"
        />

        <modal-confirmation
            name="modalConfirmationUpgraded"
            message="Are you sure want to upgrade this accounts?"
            @yes="handleConfirmationUpdateStatus('Upgraded')"
        />
    </div>
</template>

<script>
import {
    DashboardGuestAccountService,
    DropdownService,
    UsersideProfileService
} from '../../../services/NSwagService';
import ModalConfirmation from '../../shared/ModalConfirmation.vue';
import AccountTable from './AccountTable.vue';
import ChartTotalGuestAccount from './ChartTotalGuestAccount.vue';
import DealerTable from './DealerTable.vue';
import OthersTable from './OthersTable.vue';
import download from 'downloadjs';

export default {
    props: ['data'],
    components: {
        DealerTable,
        ChartTotalGuestAccount,
        OthersTable,
        AccountTable,
        ModalConfirmation
    },
    data() {
        return {
            dropdownService: new DropdownService(),
            service: new DashboardGuestAccountService(),
            profileService: new UsersideProfileService(),
            summaries: {
                labels: [],
                series: []
            },
            model: { employee: {} },
            selectedDealers: null,
            dealers: [],
            selectedOutlet: null,
            outlets: [],
            selectedPosition: null,
            positions: [],
            religion: '',
            religions: [
                'Muslim',
                'Protestant',
                'Catholic',
                'Hindu',
                'Buddhist',
                'Confucian'
            ],
            isBusy: false,
            message: ''
        };
    },
    watch: {
        selectedDealers(data) {
            // this.selectedOutlet = null
            if (data != null) this.fetchOutlets(data.dealerId);
            else this.outlets = [];
        },
        selectedOutlet(data) {
            // this.selectedPosition = null
            if (data != null) this.fetchPositions(data.outletId);
            else this.positions = [];
        }
    },
    mounted() {
        this.fetchDealers();
        this.model = {
            ...this.data.selectedAccount,
            employee: {
                ...this.data.selectedAccount.employee,
                dateOfBirth: this.data.selectedAccount.employee.dateOfBirth
                    ? this.data.selectedAccount.employee.dateOfBirth.substring(
                          0,
                          10
                      )
                    : null
            }
        };

        if (this.data.selectedAccount.dealer.dealerId != null) {
            this.selectedDealers = this.model.dealer;
            if (this.data.selectedAccount.outlet.outletId != null) {
                this.selectedOutlet = {
                    outletId: this.model.outlet.outletId,
                    outletName: this.model.outlet.name
                };
                if(this.data.selectedAccount.position.length > 0) {
                    this.selectedPosition = {
                        positionId: this.model.position[0].positionId,
                        positionName: this.model.position[0].positionName
                    }
                }
            }
        }
    },
    methods: {
        async fetchSummary() {
            try {
                const res = await this.service.getChartSummary();
                this.mapSummary(res.data);
            } catch (err) {
                console.log(err);
            }
        },
        async fetchDealers() {
            this.isBusy = true;
            try {
                const res = await this.dropdownService.getDealerDropdown();
                this.dealers = res;
            } catch (err) {
                console.log(err);
            }
            this.isBusy = false;
        },
        async fetchOutlets() {
            this.isBusy = true;
            try {
                const res = await this.dropdownService.getOutletDropdown(
                    this.selectedDealers.dealerId
                );
                this.outlets = res;
            } catch (err) {
                console.log(err);
            }
            this.isBusy = false;
        },
        async fetchPositions() {
            this.isBusy = true;
            try {
                const res = await this.dropdownService.getPositionDropdown(
                    this.selectedOutlet.outletId
                );
                this.positions = res;
            } catch (err) {
                console.log(err);
            }
            this.isBusy = false;
        },
        async doBulkUpdate(entries, status) {
            this.isBusy = true;
            try {
                const promises = entries.map(v =>
                    this.service.updateAccountStatus(v)
                );
                await Promise.all(promises);
                this.fetchDetailProfile()
                this.message = 'Success update status';
            } catch (e) {
                this.isBusy = false;
            }
        },
        async fetchDetailProfile () {
            this.isBusy = true;
            try {
                const res = await this.profileService.getProfilAccount(
                    this.model.employee.employeeId
                );
                // console.log(res)
                this.model.employee = {...this.model.employee, status: res.employee.status, isRequestUpgrade: res.employee.isRequestUpgrade, isSuspended: res.employee.isSuspended}
            } catch (err) {
                console.log(err);
            }
            this.isBusy = false;
        },
        mapSummary(data) {
            const summaries = {
                labels: ['Suspended', 'Upgraded', 'Verified', 'Unverified'],
                series: [0, 0, 0, 0]
            };
            data.forEach(v => {
                switch (data.status) {
                    case 'Suspended':
                        summaries.series[0] = v.sum;
                        break;
                    case 'Upgraded':
                        summaries.series[1] = v.sum;
                        break;
                    case 'Verified':
                        summaries.series[2] = v.sum;
                        break;
                    case 'Unverified':
                        summaries.series[3] = v.sum;
                        break;
                    default:
                        summaries.series[2] = v.sum;
                        break;
                }
            });
            this.summaries = summaries;
        },
        handleBack() {
            this.$emit('navigation', {
                ...this.data,
                layout: 'view'
            });
        },
        convertDateTime(stringdate) {
            var date = new Date(stringdate);
            function pad(n) {
                return n < 10 ? '0' + n : n;
            }
            return (
                pad(date.getDate()) +
                '/' +
                pad(date.getMonth() + 1) +
                '/' +
                date.getFullYear()
            );
        },
        async handleUpdate() {
            this.isBusy = true;
            try {
                const body = {
                    employeeId: this.model.employee.employeeId,
                    outletId: this.selectedOutlet
                        ? this.selectedOutlet.outletId
                        : '',
                    dealerId: this.selectedDealers
                        ? this.selectedDealers.dealerId
                        : '',
                    employeeName: this.model.employee.employeeName,
                    employeeEmail: this.model.employee.employeeEmail,
                    employeePhone: this.model.employee.employeePhone,
                    gender: this.model.employee.gender,
                    dateOfBirth: this.model.employee.dateOfBirth,
                    positionId: this.selectedPosition
                        ? `${this.selectedPosition.positionId}`
                        : 0,
                    manpowerCode: this.model.manpowerCode,
                    religion: this.model.employee.religion,
                    ktp: this.model.employee.ktp,
                    address: this.model.employee.address,
                    nip: this.model.employee.nip
                };

                const res = await this.service.updateAccount(body);

                this.$emit('navigation', {
                    ...this.data,
                    layout: 'view'
                });
            } catch (err) {
                console.log(err);
            }
            this.isBusy = false;
        },
        handleConfirmationUpdateStatus(status) {
            const selectedAccount = [
                {
                    employeeID: this.model.employee.employeeId,
                    status: (status === 'Suspended' || status === 'Extend' || status === 'Upgraded') ? this.model.employee.status : status, 
                    extend: status === 'Extend',
                    isSuspend: status === 'Suspended',
                    isRequestUpgrade: status === 'Upgraded' ?  true : this.model.employee.isRequestUpgrade,
                    isDataValidation: status === 'Upgraded' ?  true : null
                }
            ];
            this.doBulkUpdate(selectedAccount, status);
        },
        handleMessageClose() {
            this.message = '';
        },
        async handleDownloadCSV() {
            this.isBusy = true;
            try {
                const res = await this.service.exportAccounts({
                    guid: this.model.employee.employeeId,
                    Limit: this.totalPage,
                    filetype: 'csv'
                });
                download(res, 'account.csv');
            } catch (err) {
                console.log(err);
            }
            this.isBusy = false;
        },
        async handleDownloadXLS() {
            this.isBusy = true;
            try {
                const res = await this.service.exportAccounts({
                    guid: this.model.employee.employeeId,
                    Limit: this.totalPage,
                    filetype: 'xlsx'
                });
                download(res, 'account.xlsx');
            } catch (err) {
                console.log(err);
            }
            this.isBusy = false;
        },
        clean(obj) {
            for (var propName in obj) {
                if (
                    obj[propName] === null ||
                    obj[propName] === undefined ||
                    obj[propName] === '' ||
                    obj[propName].length === 0
                ) {
                    delete obj[propName];
                }
            }
            return obj;
        },
        formatDateShow(date) {
            const d = new Date(date.substring(0,10))
            let month = '' + (d.getMonth() + 1)
            const day = '' + d.getDate()
            const year = d.getFullYear()

            switch (month) {
                case '1':
                month = 'Januari'
                break
                case '2':
                month = 'Februari'
                break
                case '3':
                month = 'Maret'
                break
                case '4':
                month = 'April'
                break
                case '5':
                month = 'Mei'
                break
                case '6':
                month = 'Juni'
                break
                case '7':
                month = 'Juli'
                break
                case '8':
                month = 'Agustus'
                break
                case '9':
                month = 'September'
                break
                case '10':
                month = 'Oktober'
                break
                case '12':
                month = 'November'
                break
                default:
                month = 'Desember'
                break
            }

            return [day, month, year].join(' ')
        }
    }
};
</script>

<style>
.gender {
    border-radius: 9999px;
    color: #9e9e9e;
    border: 1px solid #9e9e9e;
    padding: 4px 6px;
    cursor: pointer;
}
.gender-active {
    border-radius: 9999px;
    color: white;
    border: 1px solid #007bff;
    background-color: #007bff;
    padding: 4px 6px;
    cursor: pointer;
}
</style>
