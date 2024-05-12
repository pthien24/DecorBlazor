window.ShowToastr = (type, message) => {
    if(type ==="success"){
        toastr.success('We do have the Kapua suite available.', 'Turtle Bay Resort', {timeOut: 5000})
    }
    if(type ==="error"){
        toastr.error('I do not think that word means what you think it means.', 'Inconceivable!')
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