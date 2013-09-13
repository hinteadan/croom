(function (app, check, undefined) {
    'use strict';

    var privateKey = '162000d1586546b9988f63c30234fc02';

    function dummy() { }
    function AuthServiceInterface() {
        check.condition(arguments[0] === privateKey,
            "AuthService must not be instantiated. Use the registered angular service.");

        this.Login = dummy();
        this.NTLogin = dummy();
        this.Logout = dummy();
    }

    function AuthServiceProvider($httpProvider) {

        var authToken = null;

        function onLoginSuccess(result, onSuccess) {
            if (result.IsSuccessful) {
                authToken = result.AuthToken;
            }
            configureHeaders();
            if (onSuccess) {
                onSuccess.call(result, result);
            }
        }

        function AuthService($resource) {
            var baseUrl = '/Croom.Backend/Auth',
                api = $resource(baseUrl, {}, { ntAuth: { method: 'PUT' } }),
                authService = new AuthServiceInterface(privateKey);

            function login(username, password, onSuccess) {
                var result = api.save(null, { username: username, password: password }, function () {
                    onLoginSuccess(result, onSuccess);
                });
            }

            function windowsLogin(onSuccess) {
                var result = api.ntAuth(null, {}, function () {
                    onLoginSuccess(result, onSuccess);
                });
            }

            function logout(onSuccess) {
                api.delete(null, null, function () {
                    authToken = null;
                    configureHeaders();
                    if (onSuccess) {
                        onSuccess.call(null);
                    }
                });
            }

            authService.Login = login;
            authService.NTLogin = windowsLogin;
            authService.Logout = logout;
            return authService;
        }

        function configureHeaders() {
            $httpProvider.defaults.headers.common['X-Croom-AuthToken'] = authToken;
        }

        this.ConfigureHeaders = configureHeaders;

        this.$get = ['$resource', AuthService];
    }

    app.provider('AuthService', ['$httpProvider', AuthServiceProvider]);

    this.AuthService = AuthServiceInterface;

}).call(this.Croom.Services, this.Croom.AppModule, this.Croom.Check);