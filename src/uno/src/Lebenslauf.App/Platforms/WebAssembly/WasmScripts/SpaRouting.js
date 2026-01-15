// SPA Routing support for GitHub Pages
// Restores the path from sessionStorage after 404 redirect
(function() {
    var redirectPath = sessionStorage.getItem('spa-path');
    if (redirectPath) {
        sessionStorage.removeItem('spa-path');

        // Use History API to restore the original path
        var basePath = '/Lebenslauf';
        var fullPath = basePath + redirectPath;

        // Replace current URL with the original path so Uno sees the correct route
        window.history.replaceState(null, '', fullPath);
    }
})();
