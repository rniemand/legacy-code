(function() {
    var app = angular.module('wdCommon');

    app.run([
        '$log',
        function($log) {
            $log.debug('wdCommon.run()');
        }
    ]);
}());