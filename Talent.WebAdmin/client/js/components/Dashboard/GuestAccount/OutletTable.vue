<template>
  <div>
    <!-- Filter Search -->
    <div class="row mb-3">
        <!-- Filter -->
        <div class="col-auto">
            <button type="button" class="btn btn-info text-white">
            Filter
            </button>
        </div>
        <!-- Search Bar -->
        <div class="col">
            <form class="d-flex">
              <multiselect v-model="selectedDealers"
                            name="Dealer"
                            :options="dealers"
                            :multiple="true"
                            :allow-empty="true"
                            label="outletName"
                            track-by="outletName">
              </multiselect>
            </form>
        </div>
    </div>
    <!-- Table -->
    <div class="table-container">
    <table
        class="table table-hover table-bordered"
    >
        <thead style="white-space: nowrap;">
        <tr>
            <th scope="col">
            <div class="row">
                <div class="col">Status</div>
            </div>
            </th>
            <th scope="col">
            <div class="row">
                <div class="col">Done</div>
            </div>
            </th>
            <th scope="col">
            <div class="row">
                <div class="col">Pending</div>
            </div>
            </th>
            <th scope="col">
            <div class="row">
                <div class="col">Suspended</div>
            </div>
            </th>
            <!-- <th scope="col">
            <div class="row">
                <div class="col">Total MP</div>
            </div>
            </th> -->
        </tr>
        </thead>
        <tbody>
        <!-- Item 1 -->
        <tr v-for="(g, i) in data" :key="i">
            <td>{{ g.status}}</td>
            <td>{{g.done}}</td>
            <td>{{g.pending}}</td>
            <td>{{g.suspend}}</td>
            <!-- <td>{{g.sum}}</td> -->
        </tr>
        </tbody>
    </table>
    </div>
  </div>
</template>

<script>
import { DashboardGuestAccountService, DropdownService } from '../../../services/NSwagService'
export default {
  props: ['dealer', 'isRefresh'],
  data() {
    return {
      dropdownService: new DropdownService(),
      accountService: new DashboardGuestAccountService(),
      dealers: [],
      selectedDealers: [],
      data: [],
      emptyData: [
        {
          status: 'Unverified',
          done: 0,
          pending: 0,
          suspend: 0,
          sum: 0
        },
        {
          status: 'Verified',
          done: 0,
          pending: 0,
          suspend: 0,
          sum: 0
        },
        {
          status: 'Upgraded',
          done: 0,
          pending: 0,
          suspend: 0,
          sum: 0
        },
        {
          status: 'Total MP',
          done: 0,
          pending: '-',
          suspend: '-',
          sum: '-'
        }
      ]
    }
  },
  mounted() {
    this.fetchOutlets()
      this.fetchSummary()
  },
  watch: {
    selectedDealers () {
      this.fetchSummary()
    },
    isRefresh () {
      if(this.isRefresh) this.fetchSummary()
    }
  },
  methods: {
    async fetchOutlets () {
      try {
        const res = await this.dropdownService.getOutletDropdown(this.dealer)
        this.dealers = res
      }catch(err) { console.log(err) }
    },
    async fetchSummary () {
      const params = this.selectedDealers.map(v => `id=${v.outletId}&`)
      try {
        const res = await this.accountService.getSummaryOutlet(params.length > 0 ? params : '', this.dealer)
        this.data = this.emptyData.map(v => ({
          ...v,
          done: res.data.find(k => k.status === v.status) ? res.data.find(k => k.status === v.status).sumDone : 0,
          pending: res.data.find(k => k.status === v.status) ? res.data.find(k => k.status === v.status).sumRequestUpgrade : 0, 
          suspend: res.data.find(k => k.status === v.status) ? res.data.find(k => k.status === v.status).sumIsSuspended : 0, 
          sum: res.data.find(k => k.status === v.status) ? res.data.find(k => k.status === v.status).sumDone + res.data.find(k => k.status === v.status).sumPending + res.data.find(k => k.status === v.status).sumIsSuspended : 0
        }))
        this.data[3].done = this.data[0].done + this.data[1].done + this.data[2].done
        this.data[3].pending = this.data[0].pending + this.data[1].pending + this.data[2].pending
        this.data[3].suspend = this.data[0].suspend + this.data[1].suspend + this.data[2].suspend
        this.$emit('refreshDone')
      } catch(err) { this.data = this.emptyData }
    }
  }
}
</script>

<style>

</style>