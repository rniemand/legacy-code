(function() {
    var app = angular.module('webDash', ['ui.router', 'wdCommon']);

    app.config(function($stateProvider, $urlRouterProvider) {
        $urlRouterProvider.otherwise("/home");

        $stateProvider
            .state('home', {
                url: '/home',
                templateUrl: 'app/templates/home.html',
                controller: 'homeController'
            })
            .state('config', {
                url: '/config',
                templateUrl: 'app/templates/config.html',
                controller: 'configController'
            });
    });
}());
