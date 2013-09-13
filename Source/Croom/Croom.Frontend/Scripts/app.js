(function (undefined) {
    'use strict';

    var appModule = angular.module('Croom.Ui', ['ngResource']);
    this.AppModule = appModule;
    /*
    var app = angular.module('Croom', ['ngResource', 'ngRoute', 'ui.bootstrap.datetimepicker'])
        .factory('ReservationService', ['$resource', function ($resource) {
            var baseUrl = '/Croom.Backend/Reservation/:id',
                api = $resource(baseUrl, { id: '' }, { change: { method: 'PUT' } });

            return {
                Add: api.save,
                All: api.query,
                Single: api.get,
                Change: api.change,
                Cancel: api.delete
            };
          }])
        .factory('UserService', ['$resource', function ($resource) {
            var baseUrl = '/Croom.Backend/User/:id',
                api = $resource(baseUrl, { id: '' });

            return {
                Current: api.get
            };
        }])
        */

}).call(this.Croom);