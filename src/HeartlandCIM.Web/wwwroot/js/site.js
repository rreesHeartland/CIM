// Heartland CIM - client-side helpers

// Zoom an image inside the shared #imageZoomModal
function zoomImage(src, title) {
    var modalEl = document.getElementById('imageZoomModal');
    if (!modalEl) return;
    modalEl.querySelector('#zoomImg').src = src;
    modalEl.querySelector('.modal-title').textContent = title || 'Photo';
    bootstrap.Modal.getOrCreateInstance(modalEl).show();
}

// Perform a calibration workflow action via AJAX (no full page reload).
function doCalibrationAction(url, token, id, action, alsoVerify) {
    return fetch(url, {
        method: 'POST',
        headers: { 'RequestVerificationToken': token },
        body: new URLSearchParams({
            id: id,
            action: action,
            alsoVerify: alsoVerify ? 'true' : 'false'
        })
    }).then(function (r) { return r.json(); });
}
