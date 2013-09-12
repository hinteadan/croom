(function (undefined) {
    'use strict';

    this.config(['$routeProvider', 'AuthServiceProvider', function ($routeProvider, authProvider) {
        authProvider.ConfigureHeaders();
    }]);

}).call(this.Croom.AppModule);