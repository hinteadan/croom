(function () {
    "use strict";

    angular.module('Croom')
    .controller('home',['$scope', '$element', 'ReservationService', 'UserService', 'AuthService', function ($scope, $element, reservationApi, userApi, auth) {

        scheduler.config.details_on_create = true;
        scheduler.config.details_on_dblclick = true;
        scheduler.config.quick_info_detached = true;
        scheduler.config.xml_date = "%Y-%m-%d %H:%i";
      
        scheduler.init('scheduler_here', new Date(), "week");

        var reservations = reservationApi.All(function () {
            _.each(reservations, function (item) {
                item.Value.ServerId = item.Key;
                item.Value.start_date = new Date(item.Value.StartsAt);
                item.Value.end_date = new Date(item.Value.EndsAt);
                item.Value.title = item.Value.Title;
            });
            var values = _.map(reservations, function (item) {
                return item.Value;
            })
            scheduler.parse(values, "json");
        }),
            currentUser = userApi.Current();

        //scheduler.parse([
        //  { creator: "vladimir.latis", title: "first title", priority:"Normal", description: "asdasdasdsad\nasdsadsadad\n", start_date: "2013-08-30 09:00", end_date: "2013-08-30 12:00" },
        //   { creator: "vladimir.latis", title: "second title", priority:"High", description: "asdasdasadsadsadsdsad\nasdsadsdsadadsadad\n", start_date: "2013-08-30 13:00", end_date: "2013-08-30 12:00" }
        //], "json");

        scheduler.showLightbox = function (id) {
            //if (!currentUser.IsUserLoggedIn){
            //    alert("auth");
            //    scheduler.deleteEvent(id);
            //    return;
            //}
            var ev = scheduler.getEvent(id);
            scheduler.startLightbox(id, $element.find('.lightbox-form')[0]);

            $scope.currentReservation.creator = ev.RequestedBy && ev.RequestedBy.FullNames || "";
            $scope.currentReservation.title = ev.title || "";
            $scope.currentReservation.description = ev.description || "";
            $scope.currentReservation.priority = ev.Priority != undefined ? _.find($scope.priorities, { value: ev.Priority }) : $scope.priorities[1];
            $scope.currentReservation.startDate = ev.start_date || "";
            $scope.currentReservation.endDate = ev.end_date || "";
            
            if (!$scope.$$phase) {
                $scope.$apply();
            }
            scheduler.hideQuickInfo();
        };

        $scope.saveEvent = function () {
            var ev = scheduler.getEvent(scheduler.getState().lightbox_id);
            ev.creator = $scope.currentReservation.creator;
            ev.title = $scope.currentReservation.title;
            ev.description = $scope.currentReservation.description;
            ev.priority = $scope.currentReservation.priority;
            ev.startDate = $scope.currentReservation.startDate;
            ev.endDate = $scope.currentReservation.endDate;

            scheduler.endLightbox(true, $element.find('.lightbox-form')[0])
        }

        $scope.deleteEvent = function () {
            var event_id = scheduler.getState().lightbox_id;
            scheduler.endLightbox(false, $element.find('.lightbox-form')[0]);
            scheduler.deleteEvent(event_id);
        };

        $scope.closeLightBox = function () {
            scheduler.endLightbox(false, $element.find('.lightbox-form')[0]);
        };

        $scope.priorities = [
           { name: "Low", value:0 },
           { name: "Normal", value:1},
           { name: "High", value:2 }
        ];

        $scope.currentReservation = {
            creator: currentUser,
            title: '',
            description: '',
            priority: $scope.priorities[0],
            startDate: new Date(),
            endDate: new Date()
        };
    }]);

}).call(this);