document.addEventListener('DOMContentLoaded', function () {
    var reservarForm = document.getElementById('reservarForm');

    reservarForm.addEventListener('submit', function (event) {
        event.preventDefault();
        alert('Reservación exitosa');
        $('#reservarModal').modal('hide');
    });
});
