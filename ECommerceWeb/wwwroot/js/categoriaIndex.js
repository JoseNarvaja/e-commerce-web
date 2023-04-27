function validarBorrado(idProducto) {
    const swalWithBootstrapButtons = Swal.mixin({
        customClass: {
            confirmButton: 'btn btn-success',
            cancelButton: 'btn btn-danger'
        },
        buttonsStyling: false
    })

    swalWithBootstrapButtons.fire({
        title: '¿Estás seguro que quieres borrar esta categoria?',
        text: "Esta acción no se puede revertir",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Confirmar acción',
        cancelButtonText: 'Cancelar ',
        reverseButtons: true
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = `/Admin/Categoria/Delete/${idProducto}`;
        }
        else if (result.dismiss === Swal.DismissReason.cancel) {
            swalWithBootstrapButtons.fire(
                'Acción cancelada',
                '',
                'error'
            );
        }
    });
}