<template>
    <div class="modal fade" id="modalTutorials" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <div class="col-md-12 justify-content-center d-flex">
                        <div class="accordion w-100" id="accordionExample">
                          <div class="card" v-for="guide in guides.listGuideJoinModel" :key="guide.guidId">
                            <div class="card-header" :id="`headingOne-${guide.guideId}`">
                              <h2 class="accordion-heading">
                                <button class="btn btn-link w-100 accordion-toggle" type="button" data-toggle="collapse" :data-target="`#collapse-${guide.guideId}`" aria-expanded="true" :aria-controls="`collapse-${guide.guideId}`"
                                  @click="getDetail(guide)">
                                  {{ guide.name }}
                                </button>
                              </h2>
                            </div>

                            <div :id="`collapse-${guide.guideId}`" class="collapse" :aria-labelledby="`headingOne-${guide.guideId}`" data-parent="#accordionExample">
                              <div class="card-body">
                                <div v-show="guide.detailFileData != null">
                                  <video v-if="guide.mime.includes('mp4')" id="myVideo" class="w-100 video" controls :src="guide.detailFileData"></video>
                                  <object
                                      v-if="guide.mime.includes('pdf')"
                                      class="h-100 w-100 min-heigth-full"
                                      :data="guide.detailFileData"
                                      for="customFile"
                                  />
                                  <img v-if="['jpg', 'png', 'jpeg', 'gif'].includes(guide.mime)" class="w-100" :src="guide.detailFileData" alt="">

                                  <div class="mt-3">
                                    <p>{{guide.description}}</p>
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="col-md-12 d-flex justify-content-center">
                        <div class="col-md-12 d-flex justify-content-around">
                            <button class="btn btn-light talent_roundborder" type="button" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { GuideService } from '../../services/NSwagService'
import { BlobService } from '../../services/BlobService';
import $ from 'jquery'

export default {
    props: {
        name: {
            type: String,
            default: 'modalTutorials'
        }
    },
    data() {
      return {
        guideService: new GuideService(),
        guides: []
      }
    },
    mounted() {
      this.fetchGuides()

      $(document).on('show','.accordion', function (e) {
        $(e.target).prev('.accordion-heading').addClass('accordion-opened');
      });
    
      $(document).on('hide','.accordion', function (e) {
          $(this).find('.accordion-heading').not($(e.target)).removeClass('accordion-opened');
      });
    },
    methods: {
      async fetchGuides() {
        try {
          this.guides = await this.guideService.getAllJoinGuides(
              '',
              '',
              '',
              'Tutorial Web Admin',
              '',
              'Approved',
              '',
              1
          );
          this.guides.listGuideJoinModel = this.guides.listGuideJoinModel.map(v => ({...v, detailFileData: null}))
        } catch(err) {
          console.log(err)
        }
      },
      async getDetail(data) {
        if(data.detailFileData != null) return
        let blob = null
        if (data.mime == 'jpg' || data.mime == 'jpeg' || data.mime == 'png') {
             blob = await BlobService.getImageUrl(
                data.blobId,
                data.mime
            );
        } else if (data.mime == 'pdf') {
            blob = await BlobService.getFilePDF(data.blobId);
        } else if (data.mime == 'mp4') {
            blob = await BlobService.getImageUrl(
                data.blobId,
                data.mime
            );
        }

        this.guides.listGuideJoinModel = this.guides.listGuideJoinModel.map(v => ({...v, detailFileData: v.guideId === data.guideId ? blob : v.detailFileData}))
        // data.detailFileData = blob
      }
    }
}
</script>

<style scoped>
@import url('//netdna.bootstrapcdn.com/font-awesome/4.1.0/css/font-awesome.min.css');
.accordion-toggle:after {
    font-family: 'FontAwesome';
    content: "\f078";    
    float: right;
}
.accordion-opened .accordion-toggle:after {    
    content: "\f054";    
}
</style>