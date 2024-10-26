import axios from 'axios'

// create a new axios instance
export const $http = axios.create();

// $http.interceptors.request.use(function (config) {
//   config.metadata = { startTime: new Date() }
//   return config;
// }, function (error) {
//   return Promise.reject(error);
// });
// $http.interceptors.response.use(function (response) { 
//   response.config.metadata.endTime = new Date()
//   response.duration = response.config.metadata.endTime - response.config.metadata.startTime
//   return response;
// }, function (error) {
//   error.config.metadata.endTime = new Date();
//   error.duration = error.config.metadata.endTime - error.config.metadata.startTime;
//   return Promise.reject(error);
// });
