(function() {
    var app = angular.module('wdCommon');

    var sbModule = new webDash.models.wdModule(
        'sb',
        'sickBeard',
        'SickBeard');

    app.config([
        '$stateProvider',
        function($stateProvider) {
            $stateProvider
                .state('config.sb', {
                    url: "/sb",
                    views: {
                        '@': {
                            templateUrl: "app/modules/sb/templates/settings/home.html",
                            controller: 'sbSettingsController'
                        }
                    }
                })
                .state('config.sb.api', {
                    url: "/api",
                    views: {
                        '@': {
                            templateUrl: "app/modules/sb/templates/settings/api.html",
                            controller: 'sbApiSettingsController'
                        }
                    }
                });
        }
    ]);

    app.run([
        '$log',
        'moduleService',
        function($log, moduleSvc) {
            $log.debug('wdCommon.sickbeard.run()');
            moduleSvc.registerModule(sbModule);
        }
    ]);

    
}());