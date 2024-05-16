
function resizeImage() {
    const width = 40;
    const height = 40;
    const canvas = document.getElementById('canvas');
    const ctx = canvas.getContext('2d');
    const img = document.createElement('img');

    const file = document.getElementById('upload').files[0];
    if (!file) {
        alert("Please select an image file.");
        return;
    }

    const reader = new FileReader();

    reader.onload = function (e) {
        img.onload = function () {
            canvas.width = width;
            canvas.height = height;
            ctx.drawImage(img, 0, 0, width, height);

            const resizedDataURL = canvas.toDataURL('image/jpeg');
            document.getElementById('output').src = resizedDataURL;
            document.getElementById('output').style.display = 'block';
        };
        img.src = e.target.result;
    };

    reader.readAsDataURL(file);
}