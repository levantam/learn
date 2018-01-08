$(function () {
    // select the target node
    var target = document.querySelector('#izenda-root');

    var observer = new MutationObserver(function (mutations) {
        mutations.forEach(function (mutation) {
            $("#progressLoader").hide();
            $("#progressLoaderText").hide();
            $(mutation.removedNodes).each(function (value, index) {
                if (this.nodeType === 1) {
                    console.log(this);
                }
            });
        });
    });

    // configuration of the observer:
    var config = { attributes: true, childList: true, characterData: true };
    if (target) {
        // pass in the target node, as well as the observer options
        observer.observe(target, config);
    }
});