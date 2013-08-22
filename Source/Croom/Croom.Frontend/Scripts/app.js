﻿(function () {
    "use strict";

    angular.module('Croom', ['ngResource'])
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