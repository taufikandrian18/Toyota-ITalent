<template>
  <!--Table-->
  <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
      <h5>Account</h5>
      <hr />
      <div class="col-md-12 talent_magin_small mt-3">
          <div class="row align-items-center justify-content-between">
              <div class="col d-flex py-0 pl-0 pr-4">
                  <!-- <button class="btn btn-info mr-2" >
                      Filter
                  </button> -->

                  <div class="dropdown">
                    <button class="btn btn-info dropdown-toggle mr-2" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-expanded="false">
                      Filter
                    </button>
                    <div class="dropdown-menu p-3" aria-labelledby="dropdownMenuButton" style="min-width: 500px" @click="handlePreventMenuClose">
                      <b>Filter</b>
                      <div class="row">
                        <div class="col mt-4">
                          <div class="input-group talent_front">
                            <label for="">Registered Date</label>
                              <v-date-picker class="v-date-style-report"
                                              v-model="filter.date"
                                              mode="range"
                                              :firstDayOfWeek="2"
                                              :value="null"
                                              :masks="{ input: 'DD/MM/YYYY' }"></v-date-picker>
                              <div class="input-group-append">
                                  <span class="input-group-text">
                                      <fa icon="calendar-alt"></fa>
                                  </span>
                              </div>
                          </div>

                          <div class="input-group talent_front mt-2" v-if="dealer == ''">
                            <label for="">Dealer</label>
                            <multiselect v-model="selectedDealers"
                                          name="Dealer"
                                          :options="dealers"
                                          :allow-empty="true"
                                          label="dealerName"
                                          track-by="dealerName">
                            </multiselect>
                          </div>
                          <div class="input-group talent_front mt-2">
                            <label for="">Outlet</label>
                            <multiselect v-model="selectedOutlet"
                                          name="Dealer"
                                          :options="outlets"
                                          :allow-empty="true"
                                          label="outletName"
                                          track-by="outletName">
                            </multiselect>
                          </div>
                          <div class="input-group talent_front mt-2">
                            <label for="">Position</label>
                            <multiselect v-model="selectedPosition"
                                          name="Dealer"
                                          :options="positions"
                                          :allow-empty="true"
                                          label="positionName"
                                          track-by="positionName">
                            </multiselect>
                          </div>
                        </div>

                        <div class="col mt-4">
                          <div class="input-group talent_front">
                            <label for="">Status</label>
                            <div class="w-100 d-flex flex-wrap">
                              <div :class="`${data.isSelected ? 'filter-status-active' : 'filter-status'} mx-2 my-2`" v-for="data in filterStatus" :key="data.value" @click="data.isSelected = !data.isSelected">
                                {{ data.label }}
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class="row d-flex flex-wrap">
                        <hr class="w-100">
                        <div class="w-100 mt-1 d-flex justify-content-end px-3">
                          <button data-toggle="dropdown" class="btn btn-info mr-3" @click="handleFilter">Apply</button>
                          <button data-toggle="dropdown" class="btn btn-info mr-3" @click="handleResetFilter">Reset</button>
                          <button data-toggle="dropdown" class="btn btn-danger" @click="hideDropdown">Cancel</button>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="input-group">
                      <input class="form-control" placeholder="Search" @keyup="handleSearch"/>
                  </div>
              </div>
              <div class="col-auto row">
                  <button class="btn btn-info mr-2" data-toggle="modal" data-target="#modalConfirmationVerify" :disabled="selectedDatas.length == 0">
                      Verify Account
                  </button>
                  <button class="btn btn-success mr-2" data-toggle="modal" data-target="#modalConfirmationExtend" :disabled="selectedDatas.length == 0">
                      Extend Account
                  </button>
                  <button class="btn btn-danger mr-2" data-toggle="modal" data-target="#modalConfirmationSuspend" :disabled="selectedDatas.length == 0">
                      Suspend Account
                  </button>
                  <!-- <button class="btn btn-warning mr-2" data-toggle="modal" data-target="#modalConfirmationUpgraded" :disabled="selectedDatas.length == 0">
                      Upgrade Account
                  </button> -->
                  <div class="dropdown mr-2">
                    <button class="btn talent_font_white talent_bg_darkblue dropdown-toggle" type="button" id="dropdownMenuButton1" data-toggle="dropdown" aria-expanded="false">
                      Export to xls/csv
                    </button>
                    <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1" style="min-width: 200px">
                      <li><a class="dropdown-item" href="#" @click="handleDownloadXLS">Export to xls</a></li>
                      <li><a class="dropdown-item" href="#" @click="handleDownloadCSV">Export to csv</a></li>
                    </ul>
                  </div>
              </div>
          </div>
      </div>

      <div class="w-100 talent_overflowx mt-3">
          <table class="table table-bordered table-responsive-md talent_table_padding">
              <thead>
                  <tr>
                      <th scope="col" class="text-center">
                          <input type="checkbox" id="selectAll" @change="handleCheckedAllItem">
                      </th>
                      <th scope="col">
                          <a href="#" class="talent_sort" style="color: white;" @click="handleSorting('Email')">
                            Email 
                            <fa icon="sort" v-if="accountParams.SortBy !== 'Email'"/>
                            <fa icon="sort-up" v-if="accountParams.SortBy === 'Email' && !accountParams.Ascending"/>
                            <fa icon="sort-down" v-if="accountParams.SortBy === 'Email' && accountParams.Ascending"/>
                          </a>
                      </th>
                      <th>
                          <a href="#" class="talent_sort" style="color: white;" @click="handleSorting('Name')">
                            Name 
                            <fa icon="sort" v-if="accountParams.SortBy !== 'Name'"/>
                            <fa icon="sort-up" v-if="accountParams.SortBy === 'Name' && !accountParams.Ascending"/>
                            <fa icon="sort-down" v-if="accountParams.SortBy === 'Name' && accountParams.Ascending"/>
                          </a>
                      </th>
                      <th>
                          <a href="#" class="talent_sort" style="color: white;" @click="handleSorting('Dealer')">
                            Dealer 
                            <fa icon="sort" v-if="accountParams.SortBy !== 'Dealer'"/>
                            <fa icon="sort-up" v-if="accountParams.SortBy === 'Dealer' && !accountParams.Ascending"/>
                            <fa icon="sort-down" v-if="accountParams.SortBy === 'Dealer' && accountParams.Ascending"/>
                          </a>
                      </th>
                      <th>
                          <a href="#" class="talent_sort" style="color: white;" @click="handleSorting('Outlet')">
                            Outlet 
                            <fa icon="sort" v-if="accountParams.SortBy !== 'Outlet'"/>
                            <fa icon="sort-up" v-if="accountParams.SortBy === 'Outlet' && !accountParams.Ascending"/>
                            <fa icon="sort-down" v-if="accountParams.SortBy === 'Outlet' && accountParams.Ascending"/>
                          </a>
                      </th>
                      <th>
                          <a href="#" class="talent_sort" style="color: white;" @click="handleSorting('RegisteredDate')">
                            Registered Date 
                            <fa icon="sort" v-if="accountParams.SortBy !== 'RegisteredDate'"/>
                            <fa icon="sort-up" v-if="accountParams.SortBy === 'RegisteredDate' && !accountParams.Ascending"/>
                            <fa icon="sort-down" v-if="accountParams.SortBy === 'RegisteredDate' && accountParams.Ascending"/>
                          </a>
                      </th>
                      <th>
                          <a href="#" class="talent_sort" style="color: white;">
                            Status
                          </a>
                      </th>
                      <th>
                          <a href="#" class="talent_sort" style="color: white;">
                            Request Upgrade
                          </a>
                      </th>
                      <th>
                          <a href="#" class="talent_sort" style="color: white;">
                            Suspended
                          </a>
                      </th>
                      <th colspan="2" class="text-center">Action</th>
                  </tr>
              </thead>
              <tbody>
                  <tr v-for="g in datas" :key="g.employeeID">
                      <th class="text-center">
                          <input type="checkbox" @change="(e) => handleCheckedItem(g, e)" :checked="g.isSelected">
                      </th>
                      <td>
                          {{ g.email }}
                      </td>
                      <td>
                          {{ g.name }}
                      </td>
                      <td>
                          {{ g.dealerName }}
                      </td>
                      <td>
                          {{ g.outletName }}
                      </td>
                      <td>
                          {{convertDateTime(g.registeredDate)}}
                      </td>
                      <td>
                          {{ g.status }}
                      </td>
                      <td class="text-center">
                          {{ g.isDataValidation ? 'Data Valid' : g.isRequestUpgrade ? 'Requesting' : '-' }}
                      </td>
                      <td class="text-center">
                          {{ g.isSuspend ? 'Suspended' : '-' }}
                      </td>
                      <td class="talent_nopadding_important">
                          <button class="btn btn-block talent_bg_orange talent_font_white" @click="handleDetail(g)">View Detail</button>
                      </td>
                      <td class="talent_nopadding_important">
                          <button class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(g)">Edit</button>
                      </td>
                      <!-- <td v-if="crud.delete" class="talent_nopadding_important">
                          <button class="btn btn-block talent_bg_red talent_font_white" data-toggle="modal" data-target="#exampleModalCenter" @click.prevent="deleteClicked(g.guideId, index)" :disabled="g.approvalStatus == 'Waiting for Approval'">Remove</button>
                      </td> -->
                  </tr>
              </tbody>
          </table>
      </div>

      <!--Pagination-->
      <div class="col-md-12 d-flex justify-content-center">
          <paginate :currentPage.sync="accountParams.Page" :total-data=totalData :limit-data=10 @update:currentPage="fetch()"></paginate>
      </div>

      <modal-confirmation
        name="modalConfirmationSuspend"
        message="Are you sure want to suspend this accounts?"
        @yes="handleConfirmationUpdateStatus('Suspended')"/>

      <modal-confirmation
        name="modalConfirmationVerify"
        message="Are you sure want to verify this accounts?"
        @yes="handleConfirmationUpdateStatus('Verified')"/>

      <modal-confirmation
        name="modalConfirmationExtend"
        message="Are you sure want to extend this accounts?"
        @yes="handleConfirmationUpdateStatus('Extend')"/>

      <modal-confirmation
        name="modalConfirmationUpgraded"
        message="Are you sure want to upgrade this accounts?"
        @yes="handleConfirmationUpdateStatus('Upgraded')"/>

  </div>
</template>

<script>
import { DashboardGuestAccountService, DropdownService } from '../../../services/NSwagService'
import debounce from 'lodash/debounce'
import moment from 'moment'
import ModalConfirmation from '../../shared/ModalConfirmation.vue'
import download from 'downloadjs'
import $ from 'jquery'

export default {
  props: ['dealer'],
  components: { ModalConfirmation },
  data() {
    return {
      isBusy: false,
      service: new DashboardGuestAccountService(),
      dropdownService: new DropdownService(),
      datas: [],
      totalData: 0,
      limit: 5,
      status: '',
      page: 1,
      filter: {
        date: null
      },
      selectedDealers: null,
      dealers: [],
      selectedOutlet: null,
      outlets: [],
      selectedPosition: null,
      positions: [],
      filterStatus: [
        {
          label: 'Unverified',
          value: 'Unverified',
          isSelected: false
        },
        {
          label: 'Verified',
          value: 'Verified',
          isSelected: false
        },
        {
          label: 'Upgraded',
          value: 'Upgraded',
          isSelected: false
        },
        {
          label: 'Suspended',
          value: 'Suspended',
          isSelected: false
        },
        {
          label: 'Requesting',
          value: 'Requesting',
          isSelected: false
        },
        {
          label: 'Data Valid',
          value: 'Valid',
          isSelected: false
        }
      ],
      accountParams: {
        status: [],
        RegisterdDateStart: '',
        RegisterdDateEnd: '',
        DealerID: '',
        OutletID: '',
        PositionID: '',
        Page: 1,
        Limit: 10,
        Ascending: false,
        SortBy: 'RegisteredDate',
        SearchQuery: '',
        isSuspended: false,
        IsRequetUpgrade: false,
        IsDataValidation: false
      }
    }
  },
  watch: {
    isBusy () {
      this.$emit("isBusy", this.isBusy)
    },
    accountParams() {
      this.fetch()
    },
    selectedDealers(data) {
      this.selectedOutlet = null
      if(data != null)
        this.fetchOutlets(data.dealerId)
      else
        this.outlets = []
    },
    selectedOutlet(data) {
      this.selectedPosition = null
      if(data != null)
        this.fetchPositions(data.outletId)
      else
        this.positions = []
    }
  },
  computed: {
    selectedDatas () {
      return this.datas.filter(v => v.isSelected)
    }
  },
  async mounted() {
    this.isBusy = true
    try {
      const res = await this.dropdownService.getDealerDropdown()
      this.dealers = res
      if(this.dealer != '') {
        this.selectedDealers = {
          dealerId: this.dealer,
          dealerName: ''
        }
        this.accountParams = {
          ...this.accountParams,
          status: this.filterStatus.filter(v =>v.isSelected && !['Suspended', 'Valid', 'Requesting'].includes(v.value)).map(v => v.value),
          RegisterdDateStart: this.filter.date ? moment(this.filter.date.start).format('YYYY-MM-DD') : '',
          RegisterdDateEnd: this.filter.date ? moment(this.filter.date.end).format('YYYY-MM-DD') : '',
          DealerID: this.selectedDealers ? this.selectedDealers.dealerId : '',
          OutletID: this.selectedOutlet ? this.selectedOutlet.outletId : '',
          PositionID: this.selectedPosition ? this.selectedPosition.positionId : '',
          Page: 1,
          Ascending: false,
          SortBy: 'RegisteredDate',
          isSuspended: this.filterStatus.filter(v =>v.isSelected && v.value=='Suspended').length > 0,
          IsRequetUpgrade: this.filterStatus.filter(v =>v.isSelected && v.value=='Requesting').length > 0,
          IsDataValidation: this.filterStatus.filter(v =>v.isSelected && v.value=='Valid').length > 0
        }
      } else {
        this.fetch()
      }
    }catch(err) { console.log(err) }
    this.isBusy = false
  },
  methods: {
    clean(params) {
      let status = ''
      if(!params.isSuspended) {
        params.isSuspended = ''
      }
      if(!params.IsRequetUpgrade) {
        params.IsRequetUpgrade = ''
      }
      if(!params.IsDataValidation) {
        params.IsDataValidation = ''
      }
      if(params.IsRequetUpgrade) {
        if(!params.IsDataValidation) {
          params.IsDataValidation = false
        }
      }
      params.status.forEach((v, i) => {
        status += v
        if(i != params.status.length - 1) {
          status+= '&status='
        }
      })
      const obj = {...params, status: status}
      for (var propName in obj) {
        if (obj[propName] === null || obj[propName] === undefined || obj[propName] === '' || obj[propName].length === 0 ) {
          delete obj[propName];
        }
      }
      return obj
    },
    async fetch() {
      this.isBusy = true
      try {
          const res = await this.service.getAccounts(this.clean({...this.accountParams}))
          this.datas = res.data.map(v => ({...v, isSelected: false}))
          this.totalData = res.count
      } catch(err) { console.log(err) }
      this.isBusy = false
    },
    async fetchDealers () {
      this.isBusy = true
      try {
        const res = await this.dropdownService.getDealerDropdown()
        this.dealers = res
      }catch(err) { console.log(err) }
      this.isBusy = false
    },
    async fetchOutlets () {
      this.isBusy = true
      try {
        const res = await this.dropdownService.getOutletDropdown(this.selectedDealers.dealerId)
        this.outlets = res
      }catch(err) { console.log(err) }
      this.isBusy = false
    },
    async fetchPositions () {
      this.isBusy = true
      try {
        const res = await this.dropdownService.getPositionDropdown(this.selectedOutlet.outletId)
        this.positions = res
      }catch(err) { console.log(err) }
      this.isBusy = false
    },
    async doBulkUpdate (entries) {
      this.isBusy = true
      try {
        const promises = entries.map(v => this.service.updateAccountStatus(v))
        await Promise.all(promises)
        this.fetch()
        this.$emit('refresh')
      } catch (e) {}
      this.isBusy = false
    },
    convertDateTime(stringdate) {
        var date = new Date(stringdate);
        function pad(n) { return n < 10 ? "0" + n : n; }
        return pad(date.getDate()) + "/" + pad(date.getMonth() + 1) + "/" + date.getFullYear();
    },
    handlePreventMenuClose(e) {
      e.stopPropagation();
    },
    handleEdit(data) {
      this.$emit('edit', data)
    },
    handleDetail(data) {
      this.$emit('detail', data)
    },
    //Sorting
    handleSorting(key){
      this.accountParams = {
        ...this.accountParams,
        SortBy: key,
        Ascending: !this.accountParams.Ascending
      }
    },
    handleFilter() {
      this.accountParams = {
        ...this.accountParams,
        status: this.filterStatus.filter(v =>v.isSelected && !['Suspended', 'Valid', 'Requesting'].includes(v.value)).map(v => v.value),
        RegisterdDateStart: this.filter.date ? moment(this.filter.date.start).format('YYYY-MM-DD') : '',
        RegisterdDateEnd: this.filter.date ? moment(this.filter.date.end).format('YYYY-MM-DD') : '',
        DealerID: this.selectedDealers ? this.selectedDealers.dealerId : '',
        OutletID: this.selectedOutlet ? this.selectedOutlet.outletId : '',
        PositionID: this.selectedPosition ? this.selectedPosition.positionId : '',
        Page: 1,
        isSuspended: this.filterStatus.filter(v =>v.isSelected && v.value=='Suspended').length > 0,
        IsRequetUpgrade: this.filterStatus.filter(v =>v.isSelected && v.value=='Requesting').length > 0,
        IsDataValidation: this.filterStatus.filter(v =>v.isSelected && v.value=='Valid').length > 0
      }

      this.hideDropdown()
    },
    handleResetFilter() {
      this.accountParams = {
        status: [],
        RegisterdDateStart: '',
        RegisterdDateEnd: '',
        DealerID: this.dealer == '' ? '' : this.dealer,
        OutletID: '',
        PositionID: '',
        Page: 1,
        Limit: 10,
        Ascending: true,
        SortBy: '',
        SearchQuery: ''
      }
      if(this.dealer == '') {
        this.selectedDealers = []
      }
      this.selectedOutlet = []
      this.selectedPosition = []
      this.filter.date = null
      this.filterStatus = this.filterStatus.map(v => ({...v, isSelected: false}))
      this.hideDropdown()
    },
    handleSearch(e) {
      this.search(this, e.target.value)
    },
    search: debounce(async (vm, keyword) => {
      try {
        vm.accountParams = {
          ...vm.accountParams,
          SearchQuery: keyword,
          page: 1
        }
      } catch(err) {
        console.log(err)
      }
    }, 1000),
    handleCheckedItem(item, e) {
      this.datas = this.datas.map(v => ({...v, isSelected: v.employeeID === item.employeeID ? e.target.checked : v.isSelected}))
    },
    handleCheckedAllItem(e) {
      this.datas = this.datas.map(v => ({...v, isSelected: e.target.checked}))
    },
    handleConfirmationUpdateStatus (status) {
      const selectedAccount = this.datas.filter(v => v.isSelected).map(v => ({
        employeeID: v.employeeID, 
        status: (status === 'Suspended' || status === 'Extend') ? v.status : status, 
        extend: status === 'Extend',
        isSuspend: status === 'Suspended',
        isRequestUpgrade: status === 'Upgraded' ?  false : v.isRequestUpgrade
      }))
      this.doBulkUpdate(selectedAccount)
    },
    handleClickSuspend(e) {
      e.preventDefault()
    },
    async handleDownloadCSV() {
      this.isBusy = true
      try {
          const params = this.clean(this.accountParams)
          const res = await this.service.exportAccounts({...params, filetype: 'csv', Limit: this.totalData})
          download(res, 'account.csv')
      } catch(err) { console.log(err) }
      this.isBusy = false
    },
    async handleDownloadXLS(){
      this.isBusy = true
      try {
          const params = this.clean(this.accountParams)
          const res = await this.service.exportAccounts({...params, filetype: 'xlsx', Limit: this.totalData})
          download(res, 'account.xlsx')
      } catch(err) { console.log(err) }
      this.isBusy = false
    },
    hideDropdown () {
      $('#dropdownMenuButton').dropdown('hide')
    }
  }
}
</script>

<style>
.filter-status {
  border-radius: 9999px;
  color: #9e9e9e;
  border: 1px solid #9e9e9e;
  padding: 4px 6px;
  cursor: pointer;
}
.filter-status-active {
  border-radius: 9999px;
  color: white;
  border: 1px solid #007bff;
  background-color: #007bff;
  padding: 4px 6px;
  cursor: pointer;
}
</style>