<template>
  <div>
    <loading-overlay :loading="isBusy"></loading-overlay>

    <div class="row">
      <div class="col">
        <h3>
            <fa icon="chart-pie"></fa> My Tools >
            <span class="talent_font_red">Dictionary</span>
        </h3>
      </div>

      <div class="col-md-12">
        <div class="row talent_roundborder talent_bg_white talent_padding talent_bg_shadow">
          <h5>Dictionary</h5>
          <hr />
          <div class="col-md-12 talent_magin_small mt-3">
              <div class="row align-items-center justify-content-between">
                  <div class="col d-flex py-0 pl-0 pr-4">
                      <!-- <button class="btn btn-info mr-2" >
                          Filter
                      </button> -->

                      <div class="dropdown">
                        <button class="btn btn-info mr-2" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-expanded="false">
                          Filter
                        </button>
                        <div class="dropdown-menu p-3" aria-labelledby="dropdownMenuButton" style="min-width: 500px" @click="handlePreventMenuClose">
                          <b>Filter</b>
                          <div class="row">
                            <div class="col">
                              <div class="input-group talent_front">
                                <label for="">Status</label>
                                <div class="w-100 d-flex flex-wrap">
                                  <div :class="`${selectedFilterStatus == data.value ? 'filter-status-active' : 'filter-status'} mx-2 my-2`" v-for="data in filterStatus" :key="data.value" @click="handleSelectedFilterStatus(data)">
                                    {{ data.label }}
                                  </div>
                                </div>
                              </div>
                            </div>
                          </div>
                          <div class="row d-flex flex-wrap">
                            <hr class="w-100">
                            <div class="w-100 mt-1 d-flex justify-content-end px-3">
                              <button data-toggle="dropdown" class="btn btn-info mr-3" @click="handleFilter">Apply</button>
                              <button data-toggle="dropdown" class="btn btn-info mr-3" @click="handleResetFilter">Reset</button>
                              <button data-toggle="dropdown" class="btn btn-danger" @click="hideDropdown">Cancel</button>
                            </div>
                          </div>
                        </div>
                      </div>
                      <div class="input-group">
                          <input class="form-control" placeholder="Search By Name" v-model="params.DictionaryName" @keyup.enter="handleFilter"/>
                      </div>
                  </div>
                  <div class="col-8 row d-flex justify-content-end">
                      <a href="/MyTools/AddDictionary">
                        <button class="btn btn-success mr-2">
                            Add New
                        </button>
                      </a>
                      <div class="dropdown mr-2">
                        <!-- <button class="btn talent_font_white talent_bg_darkblue dropdown-toggle" type="button" id="dropdownMenuButton1" data-toggle="dropdown" aria-expanded="false">
                          Export to xls/csv
                        </button> -->
                        <!-- <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton1" style="min-width: 200px">
                          <li><a class="dropdown-item" href="#" @click="handleDownloadXLS">Export to xls</a></li>
                          <li><a class="dropdown-item" href="#" @click="handleDownloadCSV">Export to csv</a></li>
                        </ul> -->
                      </div>
                  </div>
              </div>
          </div>

          <div class="w-100 talent_overflowx mt-3">
              <table class="table table-bordered table-responsive-md talent_table_padding">
                  <thead>
                      <tr>
                          <th scope="col" class="text-center">
                              <a href="#" class="talent_sort" style="color: white;">
                                Name
                              </a>
                          </th>
                          <th scope="col" class="text-center">
                              <a href="#" class="talent_sort" style="color: white;">
                                Media
                              </a>
                          </th>
                          <th scope="col" class="text-center" style="max-width: 150px">
                              <a href="#" class="talent_sort" style="color: white;">
                                Manfaat
                              </a>
                          </th>
                          <th scope="col" class="text-center" style="max-width: 150px">
                              <a href="#" class="talent_sort" style="color: white;">
                                Arti
                              </a>
                          </th>
                          <th scope="col" class="text-center" style="max-width: 150px">
                              <a href="#" class="talent_sort" style="color: white;">
                                Fakta
                              </a>
                          </th>
                          <th scope="col" class="text-center" style="max-width: 150px">
                              <a href="#" class="talent_sort" style="color: white;">
                                Published
                              </a>
                          </th>
                          <th scope="col" class="text-center" colspan="2">
                              <a href="#" class="talent_sort" style="color: white;">
                                Action
                              </a>
                          </th>
                      </tr>
                  </thead>
                  <tbody>
                      <tr v-for="dictionary in data" :key="dictionary.dictionaryId">
                          <td>
                              {{dictionary.dictionaryName}}
                          </td>
                          <td class="text-center">
                              <img v-if="!['mp4', 'pdf'].includes(dictionary.blob.mime)" :src="dictionary.imageUrl" alt="" style="width: 100px">
                              <fa v-if="['mp4'].includes(dictionary.blob.mime)" icon="video" alt="" style="width: 64px;height:64px"></fa>
                              <fa v-if="['pdf'].includes(dictionary.blob.mime)" icon="file-alt" alt="" style="width: 64px;height:64px"></fa>
                          </td>
                          <td style="max-width: 150px;white-space: normal" v-html="dictionary.dictionaryManfaat">
                          </td>
                          <td style="max-width: 150px; white-space: normal" v-html="dictionary.dictionaryArti">
                          </td>
                          <td style="max-width: 150px;white-space: normal" v-html="dictionary.dictionaryFakta">
                          </td>
                          <td>
                              <button
                                :class="`btn btn-block ${dictionary.approvedAt ? 'talent_bg_green talent_font_white' : 'talent_bg_red talent_font_white'}`" 
                                @click="handleEditStatus(dictionary)">
                                  <fa :icon="dictionary.approvedAt ? 'eye' : 'eye-slash'"></fa>
                              </button>
                          </td>
                          <td class="talent_nopadding_important">
                              <button class="btn btn-block talent_bg_cyan talent_font_white" @click="handleEdit(dictionary.dictionaryId)">Edit</button>
                          </td>
                          <td class="talent_nopadding_important">
                            <button type="button" class="btn btn-block talent_bg_red talent_font_white" 
                                      data-toggle="modal"
                                      data-target="#modalDelete" @click="selectedData = dictionary">
                              Remove
                            </button>
                          </td>
                      </tr>
                  </tbody>
              </table>
          </div>

          <!--Pagination-->
          <div class="col-md-12 d-flex justify-content-center">
              <paginate :currentPage.sync="params.PageIndex" :total-data="totalData" :limit-data="10" @update:currentPage="fetchDatas()"></paginate>
          </div>
      </div>
      </div>
    </div>

    <modal-delete
      name="modalDelete"
      @delete="handleDelete"
    />

  </div>
</template>

<script lang="ts">
  import { DictionaryService } from '../../../services/Dictionary/DictionaryService';
  import { IParamDictionaries, IAllDictionaries, DictionaryModel } from '../../../services/Dictionary/DictionaryModel';
  import Vue from 'vue';
  import Component from 'vue-class-component';
  import { BlobService } from '../../../services/NSwagService';
  import ModalDelete from '../../../components/shared/ModalDelete.vue';
import moment from 'moment';
import $ from 'jquery'
    
  @Component({
  components: { ModalDelete },
    props: ['claims'],
    created: async function(this: DictionaryVue) {
      this.fetchDatas()
    }
  })
  export default class DictionaryVue extends Vue {
    isBusy: boolean = false
    blobService: BlobService = new BlobService()
    dictionaryService: DictionaryService = new DictionaryService()
    params: IParamDictionaries = {
      StartDate: null,
      EndData: null,
      DictionaryName: '',
      PageIndex: 1,
      PageSize: 10,
      SortBy: null,
      ApprovedAt: null
    }

    filterStatus: any = [
      {
        value: true,
        label: 'Published',
        isSelected: false
      },
      {
        value: false,
        label: 'Draft',
        isSelected: false
      }
    ]

    totalData: Number = 0
    data: DictionaryModel[] = []
    selectedData: DictionaryModel = null
    selectedFilterStatus: any = null

    async fetchDatas () {
      this.isBusy = true
      this.params.ApprovedAt = this.selectedFilterStatus
      console.log(this.params)
      try {
        const res : IAllDictionaries = await this.dictionaryService.getAllDictionaries(this.params)
        this.data = res.dIctionaries
        this.totalData = res.totalDictionaries
        this.hideDropdown()
      } catch (err) {
        console.error(err)
      }
      this.isBusy = false
    }

    async getFile(id) {
      const file = await this.blobService.getImageDetail(id)
      return file.fileUrl
    }

    // filter 
    handleFilter() {
      this.params.PageIndex = 1
      this.fetchDatas()
    }
    handleResetFilter() {
      this.selectedFilterStatus = null
      this.params.DictionaryName = ''
      this.fetchDatas()
    }
    hideDropdown () {
      //@ts-ignore
      $('#dropdownMenuButton').dropdown('hide')
    }
    handleSearch() {

    }
    handleSelectedFilterStatus(data) {
      const value = data.value == this.selectedFilterStatus ? null : data.value
      this.selectedFilterStatus = value
    }
    // end filter

    async handleDelete() {
      this.isBusy = true
      try {
        await this.dictionaryService.deleteDictionary(this.selectedData.dictionaryId)
        this.fetchDatas()
      } catch (err) {
        console.error(err)
      }
      this.isBusy = false
    }

    handleEdit(id) {
      window.location.href = `/MyTools/AddDictionary?id=${id}`
    }

    async handleEditStatus(data) {
      this.isBusy = true
      try {
        const body = {
          dictionaryId: data.dictionaryId,
          dictionaryName: data.dictionaryName,
          dictionaryArti: data.dictionaryArti,
          dictionaryManfaat: data.dictionaryManfaat,
          dictionaryFakta: data.dictionaryFakta,
          dictionaryStatus: data.dictionaryStatus,
          dictionaryBasicOperation: data.dictionaryBasicOperation,
          productFileContent: {
            base64: '',
            fileName: '',
            contentType: ''
          },
          approvedAt: data.approvedAt ? null : moment().format('YYYY-MM-DD')
        }
        await this.dictionaryService.updateDictionary(body)
        this.fetchDatas()
      } catch (err) {
        console.error(err)
      }
      this.isBusy = false
      this.fetchDatas()
    }


    handlePreventMenuClose(e) {
      e.stopPropagation();
    }
  }
</script>