navigator.serviceWorker.register("/js/sw.js")
    .then(() => navigator.serviceWorker.ready.then((worker) => {
        worker.sync.register("syncdata");
    }))
    .catch((err) => console.log(err));