(function() {
    var app = angular.module('wdCommon');

    app.service('moduleService', [
        '$log',
        function ($log) {
            var modules = {};

            var api = {};

            api.registerModule = function (module) {
                if (module instanceof webDash.models.wdModule === false) {
                    throw new webDash.exceptions.GeneralError(
                        'Provided module is not an instance of "webDash.models.wdModule"!',
                        'moduleService.registerModule()');
                }

                modules[module.name] = module;
            };

            api.getAllModules = function() {
                return modules;
            };

            // private methods
            var initialize = function() {
                $log.debug('moduleService.init()');
            };

            // Initialize and return
            initialize();
            return api;
        }
    ]);

}());