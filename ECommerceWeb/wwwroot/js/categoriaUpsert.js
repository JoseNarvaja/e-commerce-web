function validarImagen() {
    if (document.getElementById("URLImagen").value == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'No se subió ninguna imagen'
        })
        return false;
    }
    return true;
}

$('#trumbowyg-text').trumbowyg({
    btns: [
        ['strong', 'em',],
        ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
        ['unorderedList', 'orderedList'],
        ['removeformat']
    ]
});