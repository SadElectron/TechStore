function DeleteImage(event, imageId = null) {
    fetch(`https://localhost:7091/admin/Image/Delete/${imageId}`).then(response => {
        if (response.ok) {
            event.target.parentElement.parentElement.remove();
        }
    });
}

window.onload = () => {


    if (window.location.href.toLowerCase().indexOf('admin/cpu/update') >= 0) {
        let btn = document.getElementById('AddImage');
        const input = document.createElement('input');
        btn.addEventListener("click", (event) => {

            input.type = 'file';
            input.multiple = true; // Allow multiple file selection
            input.style.display = 'none'; // Hide the input element
            input.click();
            input.addEventListener('change', function () {
                const files = input.files;
                const formData = new FormData();
                formData.append("productId", document.getElementById("Id").value);
                for (let i = 0; i < files.length; i++) {
                    formData.append('Files', files[i]);

                }

                // Fetch request options
                const options = {
                    method: 'POST',
                    body: formData,
                    // Optionally, set headers if required, e.g., for authentication
                    // headers: {
                    //     'Authorization': 'Bearer ' + token
                    // }
                };
                fetch(`https://localhost:7091/admin/Image/Create`, options)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    }).then(data => {
                        let tableBody = document.getElementById('ImageTable').tBodies[0];
                        let index = tableBody.rows.length;
                        data.forEach(i => {
                            index++;
                            let row = tableBody.insertRow();
                            row.outerHTML = `
                                <th scope="row">${index}</th>
                                <td>${i.id}</td>
                                <td><img src="data:image /jpeg;base64,${i.file}" style="max-width:100px; height:auto" alt="Image"></td>
                                <td><button class="btn btn-danger" id="DeleteImageButton" onclick="DeleteImage(event,${i.id})">Delete</button></td>
                                `;
                        })
                    }).catch(error => {
                        console.error('There was a problem with the fetch operation:', error);
                    });
            });


        });

    }

}