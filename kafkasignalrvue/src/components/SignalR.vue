<template>
  <table>
    <tr v-for="(str, index) in Msg" :key="index">
      <td>{{str}}</td>
    </tr>
  </table>
</template>

<script>
import $ from 'jquery'
import 'signalr'

export default {
  name: 'SignalR',
  data () {
    return {
      Msg: []
    }
  },
  methods: {
    get () {
      //  下面對應到網址的部份
      let hub = $.hubConnection('http://localhost:52154/signalr/hubs')
      //  下面對應了.net的DefaultHub
      let proxy = hub.createHubProxy('MessageHub')
      proxy.on('BroadCastMessage', data => this.Msg.push(data.PubMessageId + ':' + data.Content))
      //  一開始就先去呼叫Get，以確保畫面一開始就有預設的資料
      hub.start().done(() => proxy.invoke('BroadCastMessage'))
    }
  },
  created () {
    this.get()
  }
}
</script>
