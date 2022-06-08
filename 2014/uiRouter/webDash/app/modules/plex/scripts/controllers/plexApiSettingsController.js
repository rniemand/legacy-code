(function() {
    var wdm = angular.module('webDashModules');

    wdm.controller('plexApiSettingsController', [
        '$scope',
        function($scope) {
            $scope.message = 'Hello from the Plex API settings controller';
        }
    ]);
}());