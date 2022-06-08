(function () {
    var files = [
        'app/scripts/wd/models/wdModule',
        'app/scripts/wd/exceptions/GeneralError',

        'app/scripts/wdCommon/wdCommon',
        'app/scripts/wdCommon/wdCommon.run',
        'app/scripts/wdCommon/services/moduleService',

        'app/modules/sb/scripts/sb.config',

        'app/scripts/wd/wd',
        'app/scripts/wd/wd.run',
        'app/scripts/wd/controllers/homeController',
        'app/scripts/wd/controllers/configController'
    ];

    requirejs(files,
        function () {
            angular.element(document).ready(function () {
                angular.bootstrap(document, ['webDash']);
            });
        });
}());