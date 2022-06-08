(function() {
    var wdm = angular.module('webDashModules');

    wdm.controller('sabApiSettingsController', [
        '$scope',
        'storageService',
        function ($scope, storageService) {
            var keys = {
                config: 'wd.modules.sab.config'
            };

            $scope.config = null;

            $scope.openUrl = function() {
                window.open($scope.config.apiUrl);
            };

            $scope.saveConfig = function() {
                storageService.saveJson(keys.config, $scope.config);
            };

            // private functions
            var loadConfig = function() {
                var config = storageService.loadJson(keys.config);

                if(!config) {
                    config = {
                        apiUrl: '',
                        apiKey: ''
                    };
                }

                $scope.config = config;
            };

            var initialize = function() {
                loadConfig();
            };

            initialize();
        }
    ]);
}());