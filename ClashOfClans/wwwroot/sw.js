const version = "v23";

const CACHE = `cache-${version}`;

// The activate handler takes care of cleaning up old caches.
self.addEventListener("activate", event => {
    event.waitUntil(
        caches.keys()
            .then(cacheNames => cacheNames.filter(cacheName => CACHE !== cacheName))
            .then(cachesToDelete => Promise.all(cachesToDelete.map(cacheToDelete => caches.delete(cacheToDelete)))
            .then(() => self.clients.claim()))
    );
});

// The fetch handler serves responses for same-origin resources from a cache.
// If no response is found, it populates the runtime cache with the response
// from the network before returning it to the page.
self.addEventListener('fetch', event => {
    // Skip cross-origin requests, like those for Google Analytics.
    console.log(">> " + event.request.url);
    if ((event.request.url.startsWith(self.location.origin) && !event.request.url.includes("/api/")) || event.request.url.startsWith("https://api-assets.clashofclans.com/")) {
        event.respondWith(
            caches.match(event.request).then(cachedResponse => {
                if (cachedResponse) {
                    console.log("cache: " + event.request.url);
                    return cachedResponse;
                }

                return caches.open(CACHE).then(cache => {
                    console.log("fetch: " + event.request.url);
                    return fetch(event.request).then(response => {
                        // Put a copy of the response in the runtime cache.
                        return cache.put(event.request, response.clone()).then(() => {
                            return response;
                        });
                    });
                });
            })
        );
    }
});