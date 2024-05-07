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
                // Do something with the selected files, e.g., upload them
                console.log(input.files);
                const files = input.files;

                // Create a new FormData object
                const formData = new FormData();
                formData.append("productId", document.getElementById("Id").value);
                // Append each file to the FormData object
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
                
                for (const pair of options.body.entries()) {
                    console.log(pair[0], pair[1]); // pair[0] is the key, pair[1] is the value
                }
                // Make the fetch request
                fetch(`https://localhost:7091/admin/Image/Create`, options)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    })
                    .then(data => {
                        console.log('Files uploaded successfully:', data);
                    })
                    .catch(error => {
                        console.error('There was a problem with the fetch operation:', error);
                    });
            });
            

        });

    }

}



