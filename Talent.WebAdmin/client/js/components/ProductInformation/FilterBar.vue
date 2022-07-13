<template>
  <div class="w-100">
    <div class="container-filter-bar">
      <div v-for="filter in filtered" :key="filter.key" :class="`filter-item ${filter.isActive ? 'active' : ''}`" @click="handleFilter(filter)">
        {{filter.key}}
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'

@Component({
  created: async function(this: FilterBar) {
  }
})
export default class FilterBar extends Vue {
  @Prop({default: () => []}) readonly filters: Array<any>
  selectedFilter: string = ''

  get filtered() {
    return this.filters.map(v => ({...v, isActive: v.value === this.selectedFilter}))
  }

  handleFilter(filter) {
    this.$emit('callback', filter)
    this.selectedFilter = filter.value
  }

}
</script>

<style scoped>
.container-filter-bar {
  display: flex;
}
.filter-item {
  color: grey;
  background-color: transparent;
  border: 1px solid grey;
  border-radius: 9999px;
  padding: 4px 12px;
  cursor: pointer;
  margin-right: 6px;
}

.active {
  color: white;
  background-color: #0285CA;
  border: 0px;
}
</style>