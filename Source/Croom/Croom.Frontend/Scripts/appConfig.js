(function (check, undefined) {
    'use strict';

    function RouteInfo(controller, templateUrl) {
        check.notEmpty(controller, 'controller');
        check.notEmpty(templateUrl, 'templateUrl');

        this.controller = controller;
        this.templateUrl = templateUrl;
    }

    this.config(['$routeProvider', 'AuthServiceProvider', function ($routeProvider, authProvider) {
        authProvider.ConfigureHeaders();
        $routeProvider
            .when('/', new RouteInfo('HomeController', 'Views/Calendar.html'))
            .when('/New', new RouteInfo('NewMeetingController', 'Views/NewMeeting.html'))
            .when('/Authenticate/:returnTo', new RouteInfo('AuthController', 'Views/Login.html'))
            .otherwise({ redirectTo: '/' });
    }]);

}).call(this.Croom.AppModule, this.Croom.Check);