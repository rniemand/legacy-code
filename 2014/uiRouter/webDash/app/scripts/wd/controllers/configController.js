(function() {
    var app = angular.module('webDash');

    app.controller('configController', [
        '$log',
        '$scope',
        '$state',
        'moduleService',
        function ($log, $scope, $state, moduleService) {
            $scope.modules = [];

            var initilize = function() {
                $scope.modules = moduleService.getAllModules();
            }

            initilize();
        }
    ]);
}());