(function (undefined) {
    'use strict';

    function HomeController($scope, $location) {
        function newMeeting() {
            $location.path('/New');
        }

        $scope.newMeeting = newMeeting;
    }

    this.controller('HomeController', ['$scope', '$location', HomeController]);

}).call(this.Croom.AppModule);