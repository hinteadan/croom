(function (svc, undefined) {
    'use strict';

    function LoginCommand(){
        this.Username = '';
        this.Password = '';
    }

    function AuthController($scope, $location, $routeParams, authApi) {
        /// <param name="authApi" type="svc.AuthService" />
        
        var loginCommand = new LoginCommand();

        $scope.Login = loginCommand;
        $scope.Authenticate = function () {
            authApi.Login(loginCommand.Username, loginCommand.Password, function () {
                if ($routeParams.returnTo) {
                    $location.path('/' + $routeParams.returnTo);
                }
            });
        };

    }

    this.controller('AuthController', ['$scope', '$location', '$routeParams', 'AuthService', AuthController]);

}).call(this.Croom.AppModule, this.Croom.Services);