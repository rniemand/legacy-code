/* ****************************************************************************
* RnCore object
**************************************************************************** */
(function (window) {
    var rnCore = {
        Version: { Major: 1, Minor: 0, Build: 0 },
        URL: { base: null },
        apps: {}
    };
    
    // todo: have an add plugin thingy mabobby

    rnCore.Version.ToString = function () {
        return this.Major + '.' + this.Minor + '.' + this.Build;
    };

    rnCore.URL.base = window.location.protocol + '//' + window.location.host + '/';

    rnCore.URL.append = function (append) {
        return !append ? rnCore.URL.base : rnCore.URL.base + append;
    };

    window.Rn = rnCore;
})(window);


/* ****************************************************************************
* RnRss object
**************************************************************************** */
(function (window) {
    var rnRss = {
        Version: { Major: 1, Minor: 0, Build: 0 },
        fn: {}
    };

    window.Rn.apps.RnRss = rnRss;
})(window);

/* ****************************************************************************
* RnFontAwesome
**************************************************************************** */
(function (window) {
    var rnFontAwesome = {
        Version: { Major: 1, Minor: 0, Build: 0 },
        icons: {}
    };

    rnFontAwesome.icons.spinner = function (msg) {
        return $('<i />')
            .addClass('icon-spinner icon-spin')
            .after(' ' + msg);
    };

    window.Rn.apps.FontAwesome = rnFontAwesome;
})(window);