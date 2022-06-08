(function() {
    var wdm = angular.module('webDashModules');

    wdm.run([
        '$log',
        'moduleService',
        'menuService',
        function ($log, moduleService, menuService) {
            $log.debug('webDashModules.plex.run()');

            menuService.addMenuItem('Plex Settings', '#/settings/plexSettings', 'Settings', '#/settings/');

            var plexModule = new webDash.models.wdModule(
                'plex',
                'Plex Media Server',
                'Plex'
            );

            plexModule.addRoute('/settings/plexSettings', 'settings/home.html', 'plexSettingsController');
            plexModule.addRoute('/settings/plexSettings/api', 'settings/api.html', 'plexApiSettingsController');

            moduleService.registerModule(plexModule);
        }
    ]);
}());