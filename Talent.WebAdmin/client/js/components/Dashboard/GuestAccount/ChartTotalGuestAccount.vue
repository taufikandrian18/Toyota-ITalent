<template>
  <vueapexcharts width="100%" type="pie" :options="options" :series="series"></vueapexcharts>
</template>

<script>
export default {
    props: {
        data: {
            type: Array,
            default: []
        },
        labels: {
            type: Array,
            default: []
        }
    },
    watch: {
        data () {
            this.series = this.data
        },
        labels() {
            this.options.labels = this.labels
        }
    },
    data: function() {
        return {
            options: {
                labels: ['Upgraded', 'Verified', 'Unverified'],
                legend: {
                    position: "bottom",
                },
            },
            series: this.data,
        }
    }
}
</script>