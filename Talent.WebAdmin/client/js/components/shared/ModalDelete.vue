<template>
    <div class="modal fade" :id="name" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 justify-content-center d-flex">
                        <h5>Are you sure want to delete?</h5>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="col-md-12 d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-around">
                            <button class="btn talent_bg_red talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click="handleDelete">Delete</button>
                            <button class="btn talent_bg_blue talent_font_white talent_roundborder" type="button" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
export default {
    props: {
        name: {
            type: String,
            default: 'modalDelete'
        },
        message: {
            type: String,
            default: 'Are you sure want to delete?'
        }
    },
    methods: {
        handleDelete () {
            this.$emit('delete')
        }
    }
}
</script>

<style>

</style>