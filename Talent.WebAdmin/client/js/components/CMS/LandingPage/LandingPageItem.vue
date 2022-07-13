<template>
  <div class="row card-item mt-3 d-flex align-items-center p-3">
    <div class="col-auto">
      <input type="checkbox" @change="handleSelected" :checked="data.isSelected">
    </div>
    <div class="col-auto position-relative">
      <div class="position-absolute rounded bg-primary text-white px-2 m-2">
        {{ data.sectionNumber }}
      </div>
      <img v-if="data.onBoardingFileType === 'mp4'" class="image" :src="data.onBoardingFileURL" alt="" onerror="this.src='/upload-video.jpeg';">
      <img v-else class="image" :src="data.onBoardingFileURL" alt="" onerror="this.src='/upload-image2.png';">
    </div>
    <div class="col d-flex flex-wrap">
      <div class="w-100">
        <b>Description</b>
      </div>
      <div class="w-100 mt-2" v-html="data.description"></div>
    </div>
    <div class="col-auto d-flex flex-wrap">
      <div class="w-100 d-flex justify-content-end">
        <!-- Toggle -->
        <div class="custom-control custom-switch">
          <input type="checkbox" class="custom-control-input" :id="`customSwitch-${data.onBoardingID}`" @change="handleChangeView" :checked="data.statusView">
          <label class="custom-control-label" :for="`customSwitch-${data.onBoardingID}`"></label>
        </div>
      </div>
      <div class="w-100 mt-2 d-flex justify-content-end">
        <!-- Edit -->
        <button type="button" class="btn btn-sm btn-primary mr-2"
                  data-toggle="modal"
                  data-target="#modalCrudLandingPage" @click="handleUpdate">
          Edit
        </button>
        <!-- Delete -->
        <button type="button" class="btn btn-sm btn-danger" 
                  data-toggle="modal"
                  data-target="#modalDeleteLandingPage" @click="handleDelete">
          Delete
        </button>
      </div>
    </div>
    <div class="col-auto action h-100 d-flex flex-wrap">
      <div class="w-100 button cursor-pointer" @click="handleUp">
        <fa icon="sort-up"></fa>
      </div>
      <!-- <div class="w-100">
        D
      </div> -->
      <div class="w-100 button cursor-pointer" @click="handleDown">
        <fa icon="sort-down"></fa>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    data: {
      type: Object,
      default: () => {}
    }
  },
  methods: {
    handleSelected (e) {
      this.$emit('selected', {...this.data, isSelected: e.target.checked})
    },
    handleUp () {
      this.$emit('up', this.data)
    },
    handleDown () {
      this.$emit('down', this.data)
    },
    handleChangeView (e) {
      this.$emit('status', {...this.data, statusView: e.target.checked})
    },
    handleDelete () {
      this.$emit('delete', this.data)
    },
    handleUpdate () {
      this.$emit('update', this.data)
    }
  }
}
</script>

<style>
  .card-item {
    border: 1px solid #DADADA;
    border-radius: 4px;
    /* cursor: grab; */
  }

  .image {
    width: 100px;
  }

  .action {
    border-left: 1px solid #DADADA;
  }
</style>