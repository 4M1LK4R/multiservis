var est = true;
$(document).ready(function () {
    Listar();
    ListarTecnicos();
})

$('#activo').click(function () {
    est = true;
});

$('#inactivo').click(function () {
    est = false;
});

function CargarEstadoEnChck(flag) {
    if (flag) {
        $('#activo').prop('checked', true);
    }
    else {
        $('#inactivo').prop('checked', true);
    }
};

function Nuevo() {
    LimpiarCampos();
    est = true;
    CargarEstadoEnChck(est);
    $('#modalDatos').modal('open');
    var codigo = '<p class="red-text text-darken-3 flow-text">CREAR NUEVO</p>';
    $('#cabeceraModal').html(codigo);
};

$('#aceptar').click(function () {
    if (EvaluarVacios()) {
        Guardar();
    }
});

$('#cancelar').click(function () {
    LimpiarCampos();
    $('#modalDatos').modal('close');
});

function EvaluarVacios() {
    if ($('#selectTecnico').val() == null) {
        Materialize.toast('Debe establecer el tecnico!', 8000);
        return false;
    }
    if ($('#fecha').val() == '') {
        Materialize.toast('Debe establecer la fecha!', 8000);
        return false;
    }
    else {
        return true;
    }
};
function Guardar() {
    var o = {
        id: $('#id').val(),
        progreso: $('#progreso').val(),
        tecnico: $('#selectTecnico').val(),
        estado: est
    };
    $.getJSON("/AdmReserva/Guardar", o, function (e) {
        if (e != "") {
            Materialize.toast(e, 8000);
        }
        else {
            Materialize.toast('Registro exitoso!', 8000);
            LimpiarCampos();
            $('#modalDatos').modal('close');
            Listar();
        }
    });
    Listar();
};
function Editar(id) {
    var o = { id: id };
    $.getJSON("/AdmReserva/Get", o, function (obj) {
        var codigo = '<p class="red-text text-darken-3 flow-text">ASIGNAR TECNICO</p>';
        $('#cabeceraModal').html(codigo);
        $('#id').val(id);
        $('#progreso').val(obj.progreso);
        $('#selectTecnico').val(obj.tecnico_id);

        var codigo = '<p><b>Codigo Reserva</b>:&nbsp;' + obj.reserva + '</p>';
        codigo += '<p><b>Detalle de Servicio</b>:&nbsp;' + obj.detalle_servicio + '</p>';

        if (obj.tecnico=="") {
            codigo += '<p><b>Tecnico</b>:&nbsp;<span class="red-text">Pendiente</span></p>';
        }
        else {
            codigo += '<p><b>Tecnico</b>:&nbsp;' + obj.tecnico + '</p>';
        }
        if (obj.usuario == "") {
            codigo += '<p><b>Usuario</b>:&nbsp;<span class="red-text">Pendiente</span></p>';
        }
        else {
            codigo += '<p><b>Usuario</b>:&nbsp;' + obj.usuario + '</p>';
        }

        codigo += '<p><b>Precio</b>:&nbsp;' + obj.precio + '</p>';
        codigo += '<p><b>Descripcion</b>:&nbsp;' + obj.descripcion + '</p>';
        $('#Datos').html(codigo);
        $('select').material_select();
        est = obj.estado;
        CargarEstadoEnChck(est);
        Materialize.updateTextFields();
    });
    $('#modalDatos').modal('open');
};

function LimpiarCampos() {
    $('#id').val(0);
    $('#selectTecnico').val('');
    $('#selectTipoServicio').val(''),
    $('#progreso').val(''),
    $('#fecha').val(''),
    $('#especialidad').val(''),
    $('#nivel').val(''),
    $('select').material_select();
};

function Listar() {
    $.getJSON("/AdmReserva/Listar", null, function (cadena) {
        $("#tabla").html(cadena);
        CrearDataTable();
    });
    $('#btnListar').show();
};

function ListarTecnicos() {
    $.getJSON("/Tecnico/ListarSelectTecnicos", function (cadena) {
        $('#campoTecnico').html(cadena);
        $('select').material_select();
    });
};

function ModalConfirmar(id, nom) {
    $('#idEliminar').val(id);
    $('#nomEliminar').val(nom);
    var codigo = '<p class="red-text text-darken-3 flow-text">Esta seguro que desea eliminar el registro?</p>';
    $('#cabeceraModalEliminar').html(codigo);
    $('#modalEliminar').modal('open');
}
$('#aceptarEliminar').click(function () {
    Eliminar($('#idEliminar').val());
    $('#modalEliminar').modal('close');
    Materialize.toast('La reserva fue eliminada exitosamente!', 8000);
    Listar();
});
$('#cancelarEliminar').click(function () {
    $('#idEliminar').val('');
    $('#nomEliminar').val('');
    $('#modalEliminar').modal('close');
});

function Eliminar(id) {
    var o = { id: id };
    $.getJSON("/AdmReserva/Delete", o, function (e) {
        Listar();
    });
};
