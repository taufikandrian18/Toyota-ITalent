<template>
    <div>
        <nav v-if="currentPage != null">
            <ul class="pagination">
                <li class="page-item arrow-nav" v-bind:class="{disabled: isDisablePrev}">
                    <a class="page-link" href="#" aria-label="First" @click.prevent="first()">
                        <span aria-hidden="true">&laquo;</span>
                    </a>
                </li>
                <li class="page-item arrow-nav" v-bind:class="{disabled: isDisablePrev}" @click.prevent="prev()">
                    <a class="page-link" href="#" aria-label="Previous">
                        <span aria-hidden="true">&lsaquo;</span>
                    </a>
                </li>
                <li v-for="(page, index) in pages" class="page-item" v-bind:class="{active: page.active}" @click.prevent="goto(page.label)">
                    <a class="page-link" href="#">{{page.label}}</a>
                </li>

                <li class="page-item arrow-nav" v-bind:class="{disabled: isDisableNext}" @click.prevent="next()">
                    <a class="page-link" href="#" aria-label="Next">
                        <span aria-hidden="true">&rsaquo;</span>
                    </a>
                </li>
                <li class="page-item arrow-nav" v-bind:class="{disabled: isDisableNext}" @click.prevent="last()">
                    <a class="page-link" href="#" aria-label="Last">
                        <span aria-hidden="true">&raquo;</span>
                    </a>
                </li>
            </ul>
        </nav>
    </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';

    @Component({
        props: ['currentPage', 'totalData', 'limitData', 'getFuntion'],
        created: function (this: Pagination) {
            //this.initial();
        },
        updated: function (this: Pagination) {
            if ((this.currentPage == null || this.currentPage == 0) && this.totalData != 0) {
                this.$emit('update:currentPage', 1);
                return
            }
            if (this.currentPage > this.lastpage && this.totalData != 0) {
                this.$emit('update:currentPage', this.lastpage);
                return
            }
        }
    })
    export default class Pagination extends Vue {
        currentPage: number;
        totalData: number;
        limitData: number;
        getFuntion: any;

        get lastpage() {
            return Math.ceil(this.totalData / this.limitData);
        }

        get pages() {
            if (this.totalData != 0) {
                let prev = this.currentPage > 1 ? (this.currentPage - 1) : 1;
                let next = this.currentPage < this.lastpage ? (this.currentPage + 1) : this.lastpage;

                let doublePrev = this.currentPage > 2 ? (this.currentPage - 2) : 2;
                let doubleNext = this.currentPage < this.lastpage - 1 ? (this.currentPage + 2) : this.lastpage - 1;


                let currentisFirst = this.currentPage == 1 ? ([this.currentPage + 2, this.currentPage + 3, this.currentPage + 4]) : [null, null, null];
                let currentisLast = this.currentPage == this.lastpage ? ([this.lastpage - 2, this.lastpage - 3, this.lastpage - 4]) : [null, null, null];

                let currentisFirstPlusOne = this.currentPage == 2 ? (this.currentPage + 3) : null;
                let currentisLastMinusOne = this.currentPage == this.lastpage - 1 ? (this.currentPage - 3) : null;


                let output: { label: number, active: boolean }[] = [];
                for (let i = 1; i <= this.lastpage; i++) {
                    if ([prev, next, this.currentPage, currentisFirst[0], currentisFirst[1], currentisFirst[2], currentisLast[0], currentisLast[1], currentisLast[2], doublePrev, doubleNext, currentisFirstPlusOne, currentisLastMinusOne].includes(i)) {
                        output.push({
                            label: i,
                            active: this.currentPage === i
                        });
                    }
                }
                return output;
            }
            let notFound: { label: number, active: boolean }[] = [{ label: 1, active: true }];
            return notFound;
        }

        get isDisablePrev() {
            if (this.currentPage <= 1) {
                return true;
            }
            return false;
        }

        get isDisableNext() {
            if (this.currentPage >= Math.ceil(this.totalData / this.limitData)) {
                return true;
            }
            return false;
        }

        first() {
            this.$emit('update:currentPage', 1);
        }

        last() {
            this.$emit('update:currentPage', Math.ceil(this.totalData / this.limitData));
        }

        next() {
            if (this.currentPage < this.lastpage) {
                this.$emit('update:currentPage', this.currentPage + 1);
            }
        }

        prev() {
            if (this.currentPage > 1) {
                this.$emit('update:currentPage', this.currentPage - 1);
            }
        }

        goto(page: number) {
            this.$emit('update:currentPage', page);
        }

        fetch() {
            this.getFuntion();
        }

    }
</script>