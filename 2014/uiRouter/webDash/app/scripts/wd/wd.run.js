(function() {
    var app = angular.module('webDash');

    app.run([
        '$log',
        function ($log) {
            $log.debug('webDash.run()');
        }
    ]);
}());