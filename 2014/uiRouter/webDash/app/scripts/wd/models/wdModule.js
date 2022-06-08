(function (webDash) {
    webDash.models.wdModule = function(name, displayName, description) {
        //if (!name) throw new webDash.exceptions.MissingArgument('name', 'wdModule');
        //if (!displayName) throw new webDash.exceptions.MissingArgument('displayName', 'wdModule');
        //if (!description) throw new webDash.exceptions.MissingArgument('description', 'wdModule');

        this.name = name;
        this.description = description;
        this.displayName = displayName;
        this.enabled = false;
        this.routes = [];

        // base paths
        this.templateRoot = 'modules/' + name + '/templates/';
        this.imgRoot = 'app/modules/' + name + '/images/';
        this.moduleLogo = this.imgRoot + 'logo.png';
    };

}(window.webDash));