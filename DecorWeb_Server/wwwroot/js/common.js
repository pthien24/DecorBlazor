window.ShowToastr = (type, message) => {
    if(type ==="success"){
        toastr.success(message, 'successfully', {timeOut: 5000})
    }
    if(type ==="error"){
        toastr.error(message, 'error')
    }
}



window.ShowSweet2 = (type, message) => {
    if (type === "success") {
        Swal.fire({
            title: "Good job!",
            text: message,
            icon: "success"
        });
    }
    if (type === "error") {
        Swal.fire({
            icon: "error",
            title: "Oops...",
            text: message,
            footer: '<a href="#">Why do I have this issue?</a>'
        });
    }
}


function ShowDeleteComfirmtionModal() {
    $('#DeleteModal').modal('show');
}

function HideDeleteComfirmtionModal() {
    $('#DeleteModal').modal('hide');
}