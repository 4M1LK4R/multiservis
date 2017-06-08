var est = true;
$(document).ready(function () {
    Listar();
    ListarTecnicos();
    ListarTipoServicios();
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
    if ($('#selectTipoServicio').val() == null) {
        Materialize.toast('Debe establecer el tipo de servicio!', 8000);
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
        tecnico: $('#selectTecnico').val(),
        tipo_servicio: $('#selectTipoServicio').val(),
        fecha: $('#fecha').val(),
        especialidad: $('#especialidad').val(),
        nivel: $('#nivel').val(),
        estado: est
    };
    $.getJSON("/TecnicoArea/Guardar", o, function (e) {
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
    $.getJSON("/TecnicoArea/Get", o, function (obj) {
        var codigo = '<p class="red-text text-darken-3 flow-text">EDITAR ' + obj.nombre + '</p>';
        $('#cabeceraModal').html(codigo);
        $('#id').val(id);
        $('#selectTecnico').val(obj.tecnico);
        $('#selectTipoServicio').val(obj.tipo_servicio);
        $('#fecha').val(obj.fecha);
        $('#especialidad').val(obj.especialidad);
        $('#nivel').val(obj.nivel);
        $('select').material_select();
        est = obj.estado;
        CargarEstadoEnChck(est);
        Materialize.updateTextFields();
    });
    $('#modalDatos').modal('open');
};

function LimpiarCampos() {
    $('#id').val(0),
    $('#selectTecnico').val(''),
    $('#selectTipoServicio').val(''),
    $('#fecha').val(''),
    $('#especialidad').val(''),
    $('#nivel').val(''),    
    $('select').material_select();
};

function Listar() {
    $.getJSON("/TecnicoArea/Listar", null, function (cadena) {
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
function ListarTipoServicios() {
    $.getJSON("/TipoServicio/ListarSelectTipoServicios", function (cadena) {
        $('#campoTipoServicio').html(cadena);
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
    Materialize.toast('El Registro Tecnico Area fue eliminado exitosamente!', 8000);
    Listar();
});
$('#cancelarEliminar').click(function () {
    $('#idEliminar').val('');
    $('#nomEliminar').val('');
    $('#modalEliminar').modal('close');
});

function Eliminar(id) {
    var o = { id: id };
    $.getJSON("/TecnicoArea/Delete", o, function (e) {
        Listar();
    });
};
