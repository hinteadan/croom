(function (svc, undefined) {
    'use strict';

    var priority = {
        Low: 0,
        Normal: 1,
        High: 2
    };

    function Reservation() {
        this.RequestedBy = {};
        this.Title = '';
        this.Priority = priority.Normal;
        this.Description = '';
        this.StartsAt = '';
        this.EndsAt = '';
    }

    function NewMeetingController($scope, $location,reservationApi) {
        /// <param name="reservationApi" type="svc.ReservationService" />
        var reservation = new Reservation();

        function createReservation() {
            reservationApi.Add(null, reservation, function () { }, function (response) {
                if (response.status === 401) {//Not Authorized
                    $location.path('/Authenticate/New');
                }
            });
        }

        $scope.Reservation = reservation;
        $scope.Create = createReservation;
    }

    this.controller('NewMeetingController', ['$scope', '$location', 'ReservationService', NewMeetingController]);

}).call(this.Croom.AppModule, this.Croom.Services);