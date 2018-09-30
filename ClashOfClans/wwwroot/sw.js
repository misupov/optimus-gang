const CACHE = "network-or-cache";
const timeout = 400;

self.addEventListener("install",
    (event) => {
        console.log("Установлен");
        event.waitUntil(
            caches.open(CACHE).then((cache) => cache.addAll([
                    "/"
                ])
            ));
    });

self.addEventListener("activate",
    (event) => {
        console.log("Активирован");
    });

self.addEventListener("fetch", function (event) {
    console.log(event.request);
    event.respondWith(
        fetch(event.request)
    );
});

function fromNetwork(request, timeout) {
    return new Promise((fulfill, reject) => {
        var timeoutId = setTimeout(reject, timeout);
        fetch(request).then((response) => {
                clearTimeout(timeoutId);
                fulfill(response);
            },
            reject);
    });
}

function fromCache(request) {
    return caches.open(CACHE).then((cache) =>
        cache.match(request).then((matching) =>
            matching || Promise.reject("no-match")
        ));
}