(function (app, check, undefined) {
    'use strict';

    var privateKey = '7582ca9351de43bb82832b17e2fae0f0';

    function dummy() { }
    function UserServiceInterface() {
        check.condition(arguments[0] === privateKey,
            "UserService must not be instantiated. Use the registered angular service.");

        this.Current = dummy();
    }

    function UserServiceFactory($resource) {
        var baseUrl = '/Croom.Backend/User/:id',
            api = $resource(baseUrl, { id: '' }),
            userService = new UserServiceInterface(privateKey);

        userService.Current = api.get;
        return userService;
    }

    app.factory('UserService', ['$resource', UserServiceFactory]);

    this.UserService = UserServiceInterface;

}).call(this.Croom.Services, this.Croom.AppModule, this.Croom.Check);