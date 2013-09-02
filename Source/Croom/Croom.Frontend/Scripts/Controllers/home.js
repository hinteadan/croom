(function () {
    "use strict";

    angular.module('Croom')
    .controller('home', function ($scope) {
        //scheduler.config.multi_day = true;

        scheduler.config.details_on_create = true;
        scheduler.config.details_on_dblclick = true;
        scheduler.config.quick_info_detached = true;
        scheduler.config.xml_date = "%Y-%m-%d %H:%i";

        //scheduler.config.lightbox.sections = [
        //    { name: "creator", height: 21, map_to: "creator", type: "label"},
		//	//{ name: "Title", height: 21, map_to: "title", type: "textarea", focus: true },
        //    { name: "Description", height: 200, map_to: "description", type: "textarea" },
		
		//	{ name: "time", height: 72, type: "time", map_to: "auto" }
        //];
      
        scheduler.init('scheduler_here', new Date(), "week");
        scheduler.parse([
          { creator: "vladimir.latis", title: "first title", priority:"Normal", description: "asdasdasdsad\nasdsadsadad\n", start_date: "2013-08-30 09:00", end_date: "2013-08-30 12:00" },
           { creator: "vladimir.latis", title: "second title", priority:"High", description: "asdasdasadsadsadsdsad\nasdsadsdsadadsadad\n", start_date: "2013-08-30 13:00", end_date: "2013-08-30 12:00" }
        ], "json");


        scheduler.showLightbox = function (id) {
            var ev = scheduler.getEvent(id);
            scheduler.startLightbox(id, html("my_form"));
            html("title").focus();
            html("creator").value = ev.creator || "";
            html("description").value = ev.description || "";
            html("title").value = ev.title || "";
            if (ev.priority) {
                $(html("priority")).find("option[value='" + ev.priority + "']").prop('selected', true);
            }
            html("startdate").value = ev.start_date || "";
            html("enddate").value = ev.start_date || "";
        };

        $scope.closeLightBox = function () {
            scheduler.endLightbox(false, html("my_form"));
        }

        var html = function (name) { return document.getElementsByName(name)[0]; }; //just a helper

    });

}).call(this);