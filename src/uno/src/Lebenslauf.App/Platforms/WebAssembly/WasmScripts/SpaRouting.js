// SPA Routing support for GitHub Pages
// Fixes URL prefix issue: Uno Navigation sets /Main/Home instead of /Lebenslauf/Main/Home
(function() {
    var basePath = '/Lebenslauf';

    // Only apply fix if we're hosted in a subdirectory
    if (window.location.pathname.startsWith(basePath) ||
        window.location.hostname === 'airoutine.github.io') {

        // Restore path from 404 redirect
        var redirectPath = sessionStorage.getItem('spa-path');
        if (redirectPath) {
            sessionStorage.removeItem('spa-path');
            window.history.replaceState(null, '', basePath + redirectPath);
        }

        // Intercept History API to add base path prefix
        var originalPushState = history.pushState;
        var originalReplaceState = history.replaceState;

        function fixUrl(url) {
            if (typeof url === 'string' && url.startsWith('/') && !url.startsWith(basePath)) {
                return basePath + url;
            }
            return url;
        }

        history.pushState = function(state, title, url) {
            return originalPushState.call(this, state, title, fixUrl(url));
        };

        history.replaceState = function(state, title, url) {
            return originalReplaceState.call(this, state, title, fixUrl(url));
        };
    }
})();
