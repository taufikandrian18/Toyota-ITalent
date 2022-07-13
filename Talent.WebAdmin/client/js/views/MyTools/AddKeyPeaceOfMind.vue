<template>
  <div>
    <loading-overlay :loading="isBusy"></loading-overlay>

    <div class="row">
      <div class="col mb-2">
        <h3>
            <fa icon="chart-pie"></fa> My Tools >
            <span>Key Piece Of Mind > </span> 
            <span class="talent_font_red">Add Key Piece Of Mind</span>
        </h3>
      </div>

      <div class="col-md-12">
        <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
          <div class="col-md-12">
            <div class="row">
              <div class="col-md-6 mb-4">
                <div class="input-group talent_front">
                  <label for="" class="w-100">Choose Category</label>
                  <div class="d-flex align-items-center w-100">
                    <multiselect
                      class="w-100 mr-2"
                      name="Dealer"
                      :options="[]"
                      label=""
                      track-by="">
                    </multiselect>
                    <button class="btn_add_rounded">
                      +
                    </button>
                  </div>
                </div>
              </div>
              <div class="col-md-6 mb-4">
                <div class="input-group talent_front">
                  <label for="" class="w-100">Choose Type</label>
                  <div class="d-flex align-items-center w-100">
                    <multiselect
                      class="w-100 mr-2"
                      name="Dealer"
                      :options="[]"
                      label=""
                      track-by="">
                    </multiselect>
                    <button class="btn_add_rounded">
                      +
                    </button>
                  </div>
                </div>
              </div>
              <div class="col-md-6 mb-4">
                <div class="input-group talent_front">
                  <label for="" class="w-100">Choose Content</label>
                  <div class="d-flex align-items-center w-100">
                    <multiselect
                      class="w-100 mr-2"
                      name="Dealer"
                      :options="[]"
                      label=""
                      track-by="">
                    </multiselect>
                    <button class="btn_add_rounded">
                      +
                    </button>
                  </div>
                </div>
              </div>
              <div class="col-md-6 mb-4">
                <label>Sequence</label>
                <div class="input-group">
                    <input name="Sequence" type="numver" class="form-control" />
                </div>
              </div>
              <div class="col-md-6 mb-4">
                <div class="input-group talent_front">
                  <label for="" class="w-100">Choose Sub Content</label>
                  <div class="d-flex align-items-center w-100">
                    <multiselect
                      class="w-100 mr-2"
                      name="Dealer"
                      :options="[]"
                      label=""
                      track-by="">
                    </multiselect>
                    <button class="btn_add_rounded">
                      +
                    </button>
                  </div>
                </div>
              </div>
              <div class="col-md-12 d-flex justify-content-end">
                <button class="btn btn-success mr-2">
                    Save
                </button>
                <button class="btn btn-primary">
                    Submit
                </button>
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
    
  @Component({
    props: ['claims'],
    created: async function(this: AddKeyPeaceOfMind) {

    }
  })
  export default class AddKeyPeaceOfMind extends Vue {

    isBusy: boolean = false

    data: any = {
      datas : []
    }  


    handlePreventMenuClose(e) {
      e.stopPropagation();
    }
  }
</script>