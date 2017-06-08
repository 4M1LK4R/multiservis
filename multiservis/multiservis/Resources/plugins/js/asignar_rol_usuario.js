var est = true;
$(document).ready(function () {
    Listar();
    ListarUsuarios();
    ListarRoles();
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
    if ($('#selectUsuario').val() == null) {
        Materialize.toast('Debe establecer el tecnico!', 8000);
        return false;
    }
    if ($('#selectRol').val() == null) {
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
        usuario: $('#selectUsuario').val(),
        rol: $('#selectRol').val(),
        fecha: $('#fecha').val(),
        estado: est
    };
    $.getJSON("/AsignarRolUsuario/Guardar", o, function (e) {
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
    $.getJSON("/AsignarRolUsuario/Get", o, function (obj) {
        var codigo = '<p class="red-text text-darken-3 flow-text">EDITAR ' + obj.nombre + '</p>';
        $('#cabeceraModal').html(codigo);
        $('#id').val(id);
        $('#selectUsuario').val(obj.usuario);
        $('#selectRol').val(obj.rol);
        $('#fecha').val(obj.fecha);
        $('select').material_select();
        est = obj.estado;
        CargarEstadoEnChck(est);
        Materialize.updateTextFields();
    });
    $('#modalDatos').modal('open');
};

function LimpiarCampos() {
    $('#id').val(0),
    $('#selectUsuario').val(''),
    $('#selectRol').val(''),
    $('#fecha').val(''),    
    $('select').material_select();
};

function Listar() {
    $.getJSON("/AsignarRolUsuario/Listar", null, function (cadena) {
        $("#tabla").html(cadena);
        CrearDataTable();
    });
    $('#btnListar').show();
};

function ListarUsuarios() {
    $.getJSON("/Usuario/ListarSelectUsuarios", function (cadena) {
        $('#campoUsuario').html(cadena);
        $('select').material_select();
    });
};
function ListarRoles() {
    $.getJSON("/Rol/ListarSelectRoles", function (cadena) {
        $('#campoRol').html(cadena);
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
    $.getJSON("/AsignarRolUsuario/Delete", o, function (e) {
        Listar();
    });
};
