var appConsole = (function ($) {
    return {
        create: function (elementId) {
            var element = $(elementId);
            return {
                writeLine: function (line) {
                    element.append(line);
                    element.append('<br />');
                },
                write: function (line) {
                    element.append(line);
                }
            };
        }
    };
})(jQuery);