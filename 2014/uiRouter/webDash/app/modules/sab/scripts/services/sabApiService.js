(function() {
    var app = angular.module('webDashModules');

    app.service('sabApiService', [
        '$log',
        function ($log) {
            var api = {};

            // private methods
            var initialize = function() {
                $log.debug('sabApiService.initialize()');
            };

            // Initialize and return
            initialize();
            return api;
        }
    ]);
}());