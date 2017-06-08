var est = true;
$(document).ready(function () {
    Listar();
    ListarServicios();
    $('#precio').numeric();
    $('#tiempo').numeric();
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
    if ($('#selectServicio').val() == null) {
        Materialize.toast('Debe serleccionar una servicio!', 8000);
        return false;
    }
    if ($('#nombre').val() == '') {
        Materialize.toast('Debe establecer un nombre!', 8000);
        return false;
    }
    if ($('#precio').val() == '') {
        Materialize.toast('Debe establecer un precio!', 8000);
        return false;
    }
    if ($('#tiempo').val() == '') {
        Materialize.toast('Debe establecer el tiempo de duracion en Hrs!', 8000);
        return false;
    }
    else {
        return true;
    }
};

function Guardar() {
    var o = {
    id : $('#id').val(),
    servicio : $('#selectServicio').val(),
    nombre : $('#nombre').val(),
    precio : $('#precio').val(),
    tiempo : $('#tiempo').val(),
    estado : est
    };

    $.getJSON("/DetalleServicio/Guardar", o, function (e) {
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
    $.getJSON("/DetalleServicio/Get", o, function (obj) {
        var codigo = '<p class="red-text text-darken-3 flow-text">EDITAR ' + obj.nombre + '</p>';
        $('#cabeceraModal').html(codigo);
        $('#id').val(id);
        $('#selectServicio').val(obj.servicio);
        $('#nombre').val(obj.nombre);
        $('#precio').val(obj.precio);
        $('#tiempo').val(obj.tiempo);        
        $('select').material_select();
        est = obj.estado;
        CargarEstadoEnChck(est);
        Materialize.updateTextFields();
    });
    $('#modalDatos').modal('open');
};

function LimpiarCampos() {
    $('#id').val(0);    
    $('#selectServicio').val('');
    $('#nombre').val('');
    $('#precio').val('');
    $('#tiempo').val(''); 
    $('select').material_select();
};

function Listar() {
    $.getJSON("/DetalleServicio/Listar", null, function (cadena) {
        $("#tabla").html(cadena);
        CrearDataTable();
    });
    $('#btnListar').show();
};

function ListarServicios() {
    $.getJSON("/Servicio/ListarSelectServicios", function (cadena) {
        $('#campoServicio').html(cadena);
        $('select').material_select();
    });
};

function ModalConfirmar(id, nom) {
    $('#idEliminar').val(id);
    $('#nomEliminar').val(nom);
    var codigo = '<p class="red-text text-darken-3 flow-text">Esta seguro que desea Eliminar ' + nom + '?</p>';
    $('#cabeceraModalEliminar').html(codigo);
    $('#modalEliminar').modal('open');
}
$('#aceptarEliminar').click(function () {
    Eliminar($('#idEliminar').val());
    $('#modalEliminar').modal('close');
    Materialize.toast('La Detalle de Servicio fue eliminado exitosamente!', 8000);
    Listar();
});
$('#cancelarEliminar').click(function () {
    $('#idEliminar').val('');
    $('#nomEliminar').val('');
    $('#modalEliminar').modal('close');
});

function Eliminar(id) {
    var o = { id: id };
    $.getJSON("/DetalleServicio/Delete", o, function (e) {
        Listar();
    });
};
