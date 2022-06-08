(function() {
    var wdm = angular.module('webDashModules');

    wdm.controller('plexSettingsController', [
        '$scope',
        function($scope) {
            $scope.test = '1, 2, 3... Plex';
            console.log('Plex home controller');
        }
    ]);
}());