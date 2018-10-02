if ("serviceWorker" in navigator) {
    navigator.serviceWorker.register("/sw.js");
}

window.addEventListener("beforeinstallprompt", function (e) {
    // log the platforms provided as options in an install prompt 
    console.log(e.platforms); // e.g., ["web", "android", "windows"] 
    e.userChoice.then(function (outcome) {
        console.log(outcome); // either "accepted" or "dismissed"
    });
    e.prompt();
});