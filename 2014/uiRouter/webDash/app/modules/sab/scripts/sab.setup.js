(function() {
    var wdm = angular.module('webDashModules');

    // https://docs.angularjs.org/api/ng/function/angular.module
    // http://blog.getelementsbyidea.com/load-a-module-on-demand-with-angularjs/

    wdm.run([
        '$log',
        'moduleService',
        'menuService',
        function ($log, moduleService, menuService) {
            $log.debug('webDashModules.sabNZBD.run()');

            menuService.addMenuItem('SabNZBD Settings', '#/settings/sabSettings', 'Settings', '#/settings/');

            var sabModule = new webDash.models.wdModule(
                'sab',
                'SabNZBD',
                'SabNZBD'
            );

            sabModule.addRoute('/settings/sabSettings', 'settings/home.html', 'sabSettingsController');
            sabModule.addRoute('/settings/sabSettings/api', 'settings/api.html', 'sabApiSettingsController');
            sabModule.addRoute('/sab', 'queue.html', 'sabQueueController');

            // Register application...
            moduleService.registerModule(sabModule);
        }
    ]);
}());