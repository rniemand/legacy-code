(function (webDash) {
    webDash.exceptions.GeneralError = function (message, method) {
        this.name = 'GeneralError';
        this.method = method;
        this.message = message;

        this.toString = function () {
            return '(' + this.name + ') ' + this.message;
        };
    };
}(window.webDash));