import Vue from 'vue'
import Router from 'vue-router'
import SignalR from '@/components/SignalR'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'SignalR',
      component: SignalR
    }
  ]
})
