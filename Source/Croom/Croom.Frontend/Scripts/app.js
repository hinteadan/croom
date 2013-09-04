(function () {
    "use strict";

    var app = angular.module('Croom', ['ngResource', 'ngRoute', 'ui.bootstrap.datetimepicker'])
        .config(function ($routeProvider) {
          $routeProvider
            .when('/', {
                templateUrl: 'views/home.html',
                controller: 'home'
            })
            .otherwise({
                redirectTo: '/'
            });
        });

}).call(this);