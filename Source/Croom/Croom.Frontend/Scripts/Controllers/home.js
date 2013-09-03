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
            $scope.event.startDate = moment(ev.start_date).format('dd/MM/yyyy hh:mm:ss') || "";
            $scope.event.endDate = moment(ev.end_date).format('dd/MM/yyyy hh:mm:ss') || "";
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        };

        $scope.closeLightBox = function () {
            scheduler.endLightbox(false, $element.find('.lightbox-form')[0]);
        };

        $scope.data = {
            date: moment().format("dd/MM/yyyy hh:mm:ss")
        }

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
            startDate: moment().format('dd/MM/yyyy hh:mm:ss'),
            endDate: moment().format('dd/MM/yyyy hh:mm:ss')
        };
    });

}).call(this);