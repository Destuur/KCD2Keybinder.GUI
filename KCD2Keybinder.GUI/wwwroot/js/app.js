window.clipboardCopy = {
    copyText: function (text) {
        navigator.clipboard.writeText(text).then(function () {
        })
            .catch(function (error) {
                alert(error);
            });
    }
};
window.removeInitialLoader = function () {
    const loader = document.getElementById('initial-loader');
    if (loader) {
        loader.remove();
    }
};