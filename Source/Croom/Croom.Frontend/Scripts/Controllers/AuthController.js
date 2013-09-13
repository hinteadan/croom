(function (svc, undefined) {
    'use strict';

    function LoginCommand(){
        this.Username = '';
        this.Password = '';
    }

    function AuthController($scope, $location, $routeParams, authApi, notify) {
        /// <param name="authApi" type="svc.AuthService" />
        /// <param name="notify" type="svc.NotifyService" />
        
        var loginCommand = new LoginCommand();

        $scope.Login = loginCommand;
        $scope.Authenticate = function () {
            authApi.Login(loginCommand.Username, loginCommand.Password, function (result) {
                if (!result.IsSuccessful) {
                    notify.Error('Invalid username and/or password', 'Please try again with correct credentials');
                    return;
                }
                if ($routeParams.returnTo) {
                    $location.path('/' + $routeParams.returnTo);
                }
            });
        };

    }

    this.controller('AuthController', 
        ['$scope', '$location', '$routeParams', 
            'AuthService', 'NotifyService', AuthController]);

}).call(this.Croom.AppModule, this.Croom.Services);