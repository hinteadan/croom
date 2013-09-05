(function () {
    "use strict";

    angular.module('Croom')
    .controller('home',['$scope', '$element', 'ReservationService', 'UserService', 'AuthService', function ($scope, $element, reservationApi, userApi, auth) {
        var reservations = getReservations(),
          currentUser = userApi.Current();

        $scope.saveEvent = function () {

            var event = scheduler.getEvent(scheduler.getState().lightbox_id),
                eventClone = {};
            $.extend(true, eventClone, event);

            event.creator = $scope.currentReservation.creator;
            event.title = $scope.currentReservation.title;
            event.description = $scope.currentReservation.description;
            event.Priority = $scope.currentReservation.Priority;
            event.startDate = $scope.currentReservation.startDate;
            event.endDate = $scope.currentReservation.endDate;

            updateInternalSchedulerFields(event);

            if (!(event.title && event.description)) {
                alertify.error("Title and description fields are required.");
                scheduler.setEvent(event.id, eventClone);
                return false;
            }
            if (validateReservation(event, null, null, eventClone)) {
                scheduler.endLightbox(true, $element.find('.lightbox-form')[0])
            }
            //todo: add/modify server side
            //var id = reservationApi.Add(null, event, function () {
            //    reservations = getReservations();
            //});
        }

        $scope.deleteEvent = function () {
            var event_id = scheduler.getState().lightbox_id;
            scheduler.endLightbox(false, $element.find('.lightbox-form')[0]);
            scheduler.deleteEvent(event_id);
            //todo: delete server side
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

        function updateInternalSchedulerFields(event) {
            event.text = event.title;
            event.start_date = event.startDate;
            event.end_date = event.endDate;
        }

        function getReservations() {
            reservationApi.All(function () {
                _.each(reservations, function (item) {
                    item.Value.ServerId = item.Key;
                    item.Value.start_date = item.Value.StartsAt;
                    item.Value.end_date = item.Value.EndsAt;
                    item.Value.title = item.Value.Title;
                });
                var values = _.map(reservations, function (item) {
                    return item.Value;
                })
                scheduler.parse(values, "json");
                scheduler.updateView(new Date(), "week");
            })
        };

        function validateReservation(ev, e, flag, ev_old) {
            var overlappingEvents = _.filter(scheduler.getEvents(), function (item) {
                return item.id != ev.id &&
                    ((item.start_date < ev.start_date && item.end_date > ev.end_date)
                    || (item.start_date > ev.start_date && item.end_date < ev.end_date)
                    || (item.start_date > ev.start_date && item.start_date < ev.end_date)
                    || (item.end_date > ev.start_date && item.end_date < ev.end_date));
            });

            if (overlappingEvents.length) {
                alertify.error("Only 1 event can be scheduled at a given time range.");
                scheduler.setEvent(ev.id, ev_old);
                return false;
            }

            if (!scheduler.isOneDayEvent(ev)) {
                alertify.error("Multi days schedule is not allowed.");
                scheduler.setEvent(ev.id, ev_old);
                return false;
            }
            if (ev.start_date > ev.end_date) {
                alertify.error("End date must be greater than start date");
                scheduler.setEvent(ev.id, ev_old);
                return false;
            }
            return true;
        };

        function configureSchedulerApi() {
            scheduler.config.details_on_create = true;
            scheduler.config.details_on_dblclick = true;
            scheduler.config.collision_limit = 1;
            scheduler.config.quick_info_detached = true;
            scheduler.config.multi_day = false;
            scheduler.config.drag_create = false;
            scheduler.config.xml_date = "%Y-%m-%d %H:%i";
            scheduler.init('scheduler_here', new Date(), "week");
            scheduler.attachEvent("onBeforeEventChanged", validateReservation);
            scheduler.templates.event_class = function (start, end, event) {
                if (event.Priority) {
                    return "priority-" + event.Priority.name;
                }
            };
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
                $scope.currentReservation.Priority = ev.Priority != undefined ? _.find($scope.priorities, { value: ev.Priority.value }) : $scope.priorities[1];
                $scope.currentReservation.startDate = new Date(ev.start_date) || "";
                $scope.currentReservation.endDate = new Date(ev.end_date) || "";

                if (!$scope.$$phase) {
                    $scope.$apply();
                }
                scheduler.hideQuickInfo();
            };
        }

        configureSchedulerApi();

    }]);

}).call(this);