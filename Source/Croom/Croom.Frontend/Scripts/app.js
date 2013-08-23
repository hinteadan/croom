(function () {
    "use strict";
    
    function fetchRandomImage() {
        var url = "http://picasaweb.google.com/data/feed/base/user/111894761797963080366/albumid/5915244362953664289?alt=json&kind=photo&hl=en_US&fields=entry(title,gphoto:numphotos,media:group(media:content,media:thumbnail))";
        //url = url.replace(/:user_id/, user).replace(/:album_id/, album);
        url = url.replace(/:user_id/, '111894761797963080366').replace(/:album_id/, 5915244362953664289);
        var images = new Array();
        var imagesCount = 0;
        $.getJSON(url, function (data) {
            $.each(data.feed.entry, function (i, element) {
                element["media$group"]["media$content"].length;
                images.push(element["media$group"]["media$content"][0]);
            });
            var random = Math.floor(Math.random() * imagesCount);
        });
    }

    var app = angular.module('Croom', ['ngResource'])
        .config(function ($routeProvider) {
          $routeProvider
            .when('/', {
                templateUrl: 'views/home.html',
                controller: 'home'
            })
            .otherwise({
                redirectTo: '/'
            });
        });

        fetchRandomImage();

}).call(this);