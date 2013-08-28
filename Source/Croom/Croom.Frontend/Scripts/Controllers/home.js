(function () {
    "use strict";

    angular.module('Croom')
    .controller('home', function ($scope) {
        //scheduler.config.multi_day = true;

        scheduler.config.xml_date = "%Y-%m-%d %H:%i";
        scheduler.init('scheduler_here', new Date(), "week");


        scheduler.locale.labels.section_type = "Type";
        scheduler.config.lightbox.sections = [
			{ name: "description", height: 200, map_to: "text", type: "textarea", focus: true },
			{
			    name: "type", height: 21, map_to: "type", type: "select", options: [
                   { key: 1, label: "Simple" },
                   { key: 2, label: "Complex" },
                   { key: 3, label: "Unknown" }
			    ]
			},
			{ name: "time", height: 72, type: "time", map_to: "auto" }
        ];
        scheduler.attachEvent("onClick", function (id, e) {
            alert("require credential");
            scheduler.showLightbox(id)
        });
        scheduler.attachEvent("onBeforeDrag", function (event_id, mode, native_event_object) {
            alert("require credential");
            return true;
        });
       // scheduler.load('events.xml');
        //scheduler.showLightbox(2);
    });

}).call(this);