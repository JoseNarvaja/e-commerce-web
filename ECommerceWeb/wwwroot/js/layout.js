$(document).ready(function () {
    $('#prodtable').DataTable({
        responsive: true,
        scrollX: true,
        language: {
            lengthMenu: 'Mostrar _MENU_ registros por página',
            zeroRecords: 'No se encontraron registros',
            info: 'Mostrando página _PAGE_ de _PAGES_',
            infoEmpty: 'No hay registros disponibles',
            infoFiltered: '(Filtrado de _MAX_ registros totales)',
            search: 'Buscar',
            paginate: {
                previous: 'Anterior',
                next: 'Siguiente'
                }
        },
    });
});