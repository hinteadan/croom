(function (app, check, undefined) {
    'use strict';

    var privateKey = '5e2f32ff428f4706883fee260a7a5fff';

    function dummy() { }
    function ReservationServiceInterface() {
        check.condition(arguments[0] === privateKey,
            "ReservationService must not be instantiated. Use the registered angular service.");

        this.Add = dummy();
        this.All = dummy();
        this.Single = dummy();
        this.Change = dummy();
        this.Cancel = dummy();
    }

    function ReservationServiceFactory($resource) {
        var baseUrl = '/Croom.Backend/Reservation/:id',
            api = $resource(baseUrl, { id: '' }, { change: { method: 'PUT' } }),
            service = new ReservationServiceInterface(privateKey);

        service.Add = api.save;
        service.All = api.query;
        service.Single = api.get;
        service.Change = api.change;
        service.Cancel = api.delete;
        return service;
    }

    app.factory('ReservationService', ['$resource', ReservationServiceFactory]);

    this.ReservationService = ReservationServiceInterface;

}).call(this.Croom.Services, this.Croom.AppModule, this.Croom.Check);