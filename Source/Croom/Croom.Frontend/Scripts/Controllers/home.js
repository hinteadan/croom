(function () {
    "use strict";

    angular.module('Croom')
    .controller('home', function ($scope, $element) {

        scheduler.config.details_on_create = true;
        scheduler.config.details_on_dblclick = true;
        scheduler.config.quick_info_detached = true;
        scheduler.config.xml_date = "%Y-%m-%d %H:%i";
      
        scheduler.init('scheduler_here', new Date(), "week");
        scheduler.parse([
          { creator: "vladimir.latis", title: "first title", priority:"Normal", description: "asdasdasdsad\nasdsadsadad\n", start_date: "2013-08-30 09:00", end_date: "2013-08-30 12:00" },
           { creator: "vladimir.latis", title: "second title", priority:"High", description: "asdasdasadsadsadsdsad\nasdsadsdsadadsadad\n", start_date: "2013-08-30 13:00", end_date: "2013-08-30 12:00" }
        ], "json");

        scheduler.showLightbox = function (id) {
            var ev = scheduler.getEvent(id);
            scheduler.startLightbox(id, $element.find('.lightbox-form')[0]);
            $scope.event.creator = ev.creator || "";
            $scope.event.title = ev.title || "";
            $scope.event.description = ev.description || "";
            $scope.event.priority = ev.priority ? _.find($scope.priorities, { value: ev.priority }) : $scope.priorities[0];
            $scope.event.startDate = ev.start_date || "";
            $scope.event.endDate = ev.end_date || "";
            if (!$scope.$$phase) {
                $scope.$apply();
            }
            scheduler.hideQuickInfo();
        };

        $scope.saveEvent = function () {
            var ev = scheduler.getEvent(scheduler.getState().lightbox_id);
            ev.creator = $scope.event.creator;
            ev.title = $scope.event.title;
            ev.description = $scope.event.description;
            ev.priority = $scope.event.priority;
            ev.startDate = $scope.event.startDate;
            ev.endDate = $scope.event.endDate;

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
           { value: "Low" },
           { value: "Normal" },
           { value: "High" }
        ];

        $scope.event = {
            creator: '',
            title: '',
            description: '',
            priority: $scope.priorities[0],
            startDate: new Date(),//moment().format('dd/MM/yyyy hh:mm:ss'),
            endDate: new Date()//moment().format('dd/MM/yyyy hh:mm:ss')
        };
    });

}).call(this);