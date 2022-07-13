<template>
  <div>
    <loading-overlay :loading="isBusy || isBusyFetch"></loading-overlay>

    <div class="row">
      <div class="col-sm-12">
        <h3><fa icon="database"></fa> CMS > Landing Page > Add Landing Page Slide</h3>
      </div>
      <div class="col-sm-12 talent_roundborder talent_bg_white talent_padding talent_bg_shadow mt-3">
        <!-- header -->
        <div>
          <h5>Slide</h5>
          <hr>
        </div>

        <!-- action -->
        <div class="row">
          <div class="col-sm-6 pl-5">
            <input type="checkbox" id="selectAll" @change="handleSelectedAll">
            <label for="selectAll">Selecting {{ selectedDatasLength }} of {{ datasLength }} Entry(s)</label>
          </div>
          <div class="col-sm-6">
            <div class="row d-flex justify-content-end">
              <!-- Button Delete -->
              <div class="col-auto">
                <button type="button" class="btn btn-sm btn-outline-danger" 
                  data-toggle="modal"
                  data-target="#modalDeleteBulk">
                  Delete Selected
                </button>
              </div>
              <!-- Button Submit -->
              <div class="col-auto">
                <button type="button" class="btn btn-sm btn-outline-info" @click="handleUpdateBulk">
                  Submit Selected
                </button>
              </div>
              <!-- Button Add -->
              <div class="col-auto">
                <button
                  type="button"
                  class="btn btn-sm btn-primary"
                  data-toggle="modal"
                  data-target="#modalCrudLandingPage"
                  @click="selectedData = {}"
                >
                  Add New Landing Page Image
                </button>
              </div>
            </div>
          </div>
        </div>

        <div class="px-3">
          <landing-page-item 
            v-for="data in landingPages" 
            :key="data.onBoardingID" 
            :data="data" 
            @selected="handleSelected"
            @status="handleChangeStatus"
            @up="handleUp"
            @down="handleDown"
            @delete="handleModalDelete"
            @update="handleModalUpdate"
          />
        </div>
      </div>
    </div>

    <modal-crud-landing-page 
      @submit="handleSubmit"
      @save="handleSave"
      :data="selectedData"
    />

    <modal-delete 
      name="modalDeleteLandingPage"
      @delete="handleDelete"
    />

    <modal-delete 
      name="modalDeleteBulk"
      @delete="handleDeleteBulk"
    />
  </div>
</template>

<script lang="ts">
    import Vue from 'vue';
    import Component from 'vue-class-component';
    import LandingPageItem from '../../components/CMS/LandingPage/LandingPageItem.vue';
    import ModalCrudLandingPage from '../../components/CMS/LandingPage/ModalCrudLandingPage.vue';
    import ModalDelete from '../../components/shared/ModalDelete.vue';
    import { BlobFileService, LandingPageModel, LandingPageService } from '../../services/NSwagService';

    @Component({
      components: { LandingPageItem, ModalCrudLandingPage, ModalDelete },
      mounted: async function (this: LandingPage) {
          await this.fetchLandingPages();
      }
    })
    export default class LandingPage extends Vue {

        Service: LandingPageService = new LandingPageService();
        FileService: BlobFileService = new BlobFileService();
        $refs!: {
          modalDelete: any
        }

        landingPages: LandingPageModel[] = [];
        isBusy: boolean = false
        isBusyFetch: boolean = false
        selectedData: any = {}
        modalDelete: boolean = false


        get datasLength() {
            return this.landingPages.length
        }

        get selectedDatasLength () {
          return this.landingPages.filter(v => v.isSelected).length
        }

        async fetchLandingPages () {
          this.isBusyFetch = true
          try {
            const res = await this.Service.getLandingPages();
            this.landingPages = res.data
          } catch (err) {
            console.log(err)
          }
          this.isBusyFetch = false
        }

        async insertLandingPage (data, status) {
          this.isBusy = true
          try {
            // insert image
            const resImage = await this.FileService.insertBlobFile({
              base64: data.base64,
              fileName: data.fileName,
              contentType: data.contentType
            })

            // insert landing page
            const res = await this.Service.insertLandingPage({
              sectionNumber: this.landingPages.length+1,
              onBoardingFileURL: resImage,
              onBoardingFileType: data.contentType,
              description: data.description,
              title: "",
              statusView: status
            })

            this.fetchLandingPages()
          } catch (err) { console.log(err)}
          this.isBusy = false
        }

        async updateLandingPage (data, status) {
          this.isBusy = true
          try {
            let resImage = ''
            let resContentType = data.onBoadingFileType
            // insert image
            if(data.base64.length > 0) {
              resImage = await this.FileService.insertBlobFile({
                base64: data.base64,
                fileName: data.fileName,
                contentType: data.contentType
              })
              resContentType = data.contentType
            }

            // insert landing page
            const res = await this.Service.updateLandingPage({
              onBoardingID: this.selectedData.onBoardingID,
              sectionNumber: this.selectedData.sectionNumber,
              onBoardingFileURL: resImage,
              onBoardingFileType: resContentType,
              description: data.description,
              title: this.selectedData.title,
              statusView: status
            })

            this.fetchLandingPages()
          } catch (err) { console.log(err)}
          this.isBusy = false
        }

        async updateStatus (data) {
          this.isBusy = true
          try {
            const res = await this.Service.updateLandingPages({
              onBoardings: [
                {
                  id: data.onBoardingID,
                  order: data.sectionNumber,
                  status: data.statusView
                }
              ]
            })
          } catch (err) { console.log(err) }
          this.isBusy = false
        }

        async delete (data) {
          this.isBusy = true
          try {
            const res = await this.Service.deleteLandingPage(data.onBoardingID)
            await this.updateBulk(this.landingPages.filter(v => v.onBoardingID != data.onBoardingID).map((v, i) => ({...v, sectionNumber: i+1})))
          } catch (err) { console.log(err) }
          this.isBusy = false
        }

        async updateBulk (data) {
          this.isBusy = true
          try {
            const res = await this.Service.updateLandingPages({
              onBoardings: data.map(v => (
                {
                  id: v.onBoardingID,
                  order: v.sectionNumber,
                  status: v.statusView
                }))
            })
            this.fetchLandingPages()
          } catch (err) { console.log(err) }
          this.isBusy = false
        }

        async deleteBulk (data) {
          this.isBusy = true
          const promises = data.map(v => this.Service.deleteLandingPage(v.onBoardingID))
          try {
            const res = await Promise.all(promises)
            await this.updateBulk(this.landingPages.filter(v => !data.map(v => v.onBoardingID).includes(v.onBoardingID)).map((v, i) => ({...v, sectionNumber: i+1})))
            // this.fetchLandingPages()
          } catch (err) { console.log(err) }
          this.isBusy = false
        }

        handleSelectedAll (e) {
          this.landingPages = this.landingPages.map(v => ({ ...v, isSelected: e.target.checked }))
        }

        handleSelected (data) {
          this.landingPages = this.landingPages.map(v => ({...v, isSelected: v.onBoardingID === data.onBoardingID ? data.isSelected : v.isSelected}))
        }

        handleSave (data) {
          if(this.selectedData.onBoardingID) {
            this.updateLandingPage(data, false)
          } else {
            this.insertLandingPage(data, false)
          }
        }

        handleSubmit (data) {
          if(this.selectedData.onBoardingID) {
            this.updateLandingPage(data, true)
          } else {
            this.insertLandingPage(data, true)
          }
        }

        handleChangeStatus (data) {
          this.updateStatus(data)
        }

        handleUpdateBulk () {
          const body = this.landingPages.filter(v => v.isSelected).map(v => ({...v, statusView: true}))

          this.updateBulk(body)
        }

        handleDeleteBulk () {
          const body = this.landingPages.filter(v => v.isSelected)

          this.deleteBulk(body)
        }

        handleUp (data) {
          let foundId = null

          this.landingPages.forEach((v,i) => {
            if(v.onBoardingID === data.onBoardingID) {
              foundId = i
              return
            }
          })

          const body = [
            {
              ...this.landingPages[foundId],
              sectionNumber: this.landingPages[foundId-1].sectionNumber,
            },
            {
              ...this.landingPages[foundId-1],
              sectionNumber: data.sectionNumber,
            }
          ]
          this.updateBulk(body)
        }

        handleDown (data) {
          let foundId = null

          this.landingPages.forEach((v,i) => {
            if(v.onBoardingID === data.onBoardingID) {
              foundId = i
              return
            }
          })

          const body = [
            {
              ...this.landingPages[foundId],
              sectionNumber: this.landingPages[foundId+1].sectionNumber,
            },
            {
              ...this.landingPages[foundId+1],
              sectionNumber: data.sectionNumber,
            }
          ]
          this.updateBulk(body)
        }
        
        handleModalUpdate (selectedData) {
          this.selectedData = selectedData
        }

        handleModalDelete (selectedData) {
          this.selectedData = selectedData
        }

        handleDelete () {
          this.delete(this.selectedData)
        }
    }
</script>

<style scoped>
  .version {
    position: absolute;
    bottom: 0;
    margin-bottom: 1rem;
  }
</style>
