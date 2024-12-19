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
