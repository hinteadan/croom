(function (svc, undefined) {
    'use strict';

    var priority = {
        Low: 0,
        Normal: 1,
        High: 2
    };

    function Reservation() {
        this.RequestedBy = { FullName: '' };
        this.Title = '';
        this.Priority = priority.Normal;
        this.Description = '';
        this.StartsAt = '2013-09-13 00:00:00';
        this.EndsAt = '2013-09-13 00:00:05';
    }

    function NewMeetingController($scope, $location,reservationApi, userApi) {
        /// <param name="reservationApi" type="svc.ReservationService" />
        /// <param name="userApi" type="svc.UserService" />
        var reservation = new Reservation();

        function createReservation() {
            var userInfo = userApi.Current(function () {
                reservation.RequestedBy = userInfo.User;
                reservationApi.Add(reservation, function () {
                    $location.path('/');
                }, function (response) {
                    if (response.status === 401) {//Not Authorized
                        $location.path('/Authenticate/New');
                    }
                });
            });
        }

        $scope.Reservation = reservation;
        $scope.Create = createReservation;
    }

    this.controller('NewMeetingController', ['$scope', '$location', 'ReservationService', 'UserService', NewMeetingController]);

}).call(this.Croom.AppModule, this.Croom.Services);