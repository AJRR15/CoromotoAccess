document.addEventListener('DOMContentLoaded', function () {
    var agregarHabitacionBtn = document.getElementById('agregarHabitacionBtn');
    var agregarHabitacionModal = new bootstrap.Modal(document.getElementById('agregarHabitacionModal'));

    agregarHabitacionBtn.addEventListener('click', function () {
        agregarHabitacionModal.show();
    });
});

function eliminarHabitacion(id) {
    if (confirm('¿Estás seguro de que deseas eliminar esta habitación?')) {
        alert('Habitación eliminada');
        window.location.reload();
    }
}

function filtrarHabitacion() {
    const tipoId = document.getElementById("buscarHabitacion").value;
    const tables = document.querySelectorAll(".villa-table");

    tables.forEach(table => {
        const rows = table.querySelectorAll("tbody tr");
        rows.forEach(row => {
            const rowTipoId = row.getAttribute("data-tipo-habitacion-id");
            if (tipoId === "" || rowTipoId === tipoId) {
                row.style.display = "table-row";
            } else {
                row.style.display = "none";
            }
        });
    });
}
