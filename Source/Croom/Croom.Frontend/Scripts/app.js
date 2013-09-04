(function () {
    "use strict";

    var app = angular.module('Croom', ['ngResource', 'ngRoute', 'ui.bootstrap.datetimepicker'])
        .config(function ($routeProvider) {
            $routeProvider
              .when('/', {
                  templateUrl: 'views/home.html',
                  controller: 'home'
              })
              .otherwise({
                  redirectTo: '/'
              });
        })
        .factory('ReservationService', ['$resource', function ($resource) {
            var baseUrl = '/Croom.Backend/Reservation/:id',
                api = $resource(baseUrl, { id: '' }, { change: { method: 'PUT' } });

            return {
                Add: api.save,
                All: api.query,
                Single: api.get,
                Change: api.change,
                Cancel: api.delete
            };
          }])
        .factory('UserService', ['$resource', function ($resource) {
            var baseUrl = '/Croom.Backend/User/:id',
                api = $resource(baseUrl, { id: '' });

            return {
                Current: api.get
            };
        }])
        .provider('AuthService', ['$httpProvider', function ($httpProvider) {

            var authToken = null;

            function AuthService($resource) {
                var baseUrl = '/Croom.Backend/Auth',
                    api = $resource(baseUrl);

                function login(username, password, onSuccess) {
                    var result = api.save(null, { username: username, password: password }, function () {
                        if (result.IsSuccessful) {
                            authToken = result.AuthToken;
                        }
                        configureHeaders();
                        if (onSuccess) {
                            onSuccess.call(null);
                        }
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

                return {
                    Login: login,
                    Logout: logout
                };
            }

            function configureHeaders() {
                $httpProvider.defaults.headers.common['X-Croom-AuthToken'] = authToken;
            }

            this.ConfigureHeaders = configureHeaders;

            this.$get = ['$resource', AuthService];
        }])
        .config(['AuthServiceProvider', function (authProvider) {
            authProvider.ConfigureHeaders();
        }]);

}).call(this);