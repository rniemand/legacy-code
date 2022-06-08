(function() {
    var wdm = angular.module('webDashModules');

    wdm.controller('sabQueueController', [
        '$log',
        '$scope',
        'sabApiService',
        function ($log, $scope, storageService) {

            // Event listeners
            $scope.$on('$destroy', function() {
                $log.debug('sabQueueController.$destroy');
            });
            
            // Initialize
            var initialize = function () {
                $log.debug('sabQueueController.initialize()');
            };

            initialize();
        }
    ]);
}())