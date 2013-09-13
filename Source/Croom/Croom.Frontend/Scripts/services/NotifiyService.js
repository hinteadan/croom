(function (app, check, $, undefined) {
    'use strict';

    var privateKey = '15ca8564-4486-4927-b66b-d16646d075ae';

    function NotifyService() {
        check.condition(arguments[0] === privateKey,
            "NotifyService must not be instantiated. Use the registered angular service.");

        this.Error = function (title, text) {
            $.pnotify({
                title: title,
                text: text,
                type: 'error'
            });
        };

    };

    function NotifyServiceFactory() {

        $.pnotify.defaults.styling = 'bootstrap';
        $.pnotify.defaults.history = false;
        $.pnotify.defaults.delay = 4000;

        return new NotifyService(privateKey);
    }

    app.factory('NotifyService', [NotifyServiceFactory]);

    this.NotifyService = NotifyService;

}).call(this.Croom.Services, this.Croom.AppModule, this.Croom.Check, this.$);