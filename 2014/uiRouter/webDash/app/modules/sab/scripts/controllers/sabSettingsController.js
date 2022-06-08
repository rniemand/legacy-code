(function() {
    var wdm = angular.module('webDashModules');

    wdm.controller('sabSettingsController', [
        '$scope',
        function($scope) {
            $scope.test = '1, 2, 3... SAB';
            console.log('SabNZBD home controller');
        }
    ]);
}());