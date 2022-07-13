<template>
    <div class="modal fade" :id="name" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 justify-content-center d-flex">
                        <h5>{{ message }}</h5>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="col-md-12 d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-around">
                            <button class="btn btn-primary talent_font_white talent_roundborder" type="button" data-dismiss="modal" @click="handleYes">Yes</button>
                            <button class="btn btn-light talent_roundborder" type="button" data-dismiss="modal">No</button>
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
            default: 'modalConfirmation'
        },
        message: {
            type: String,
            default: 'Message'
        }
    },
    methods: {
        handleYes () {
            this.$emit('yes')
        }
    }
}
</script>

<style>

</style>