var est = true;
$(document).ready(function () {
    Listar();
    ListarHerramientas();
    $('#precio_compra').numeric();
    $('#precio_venta').numeric();
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
    if ($('#selectFecha').val() == '') {
        Materialize.toast('Debe serleccionar una fecha!', 8000);
        return false;
    }
    if ($('#precio_compra').val() == '') {
        Materialize.toast('Debe establecer un precio de compra!', 8000);
        return false;
    }
    if ($('#precio_venta').val() == '') {
        Materialize.toast('Debe establecer un precio de venta!', 8000);
        return false;
    }
    if ($('#selectHerramienta').val() == null) {
        Materialize.toast('Debe seleccionar una herramienta!', 8000);
        return false;
    }
    else {
        return true;
    }
};

function Guardar() {
    var i = $('#id').val();
    var fecha = $('#selectFecha').val();
    var id_herr = $('#selectHerramienta').val();
    var fecha = $('#selectFecha').val();
    var p_compra = $('#precio_compra').val();
    var p_venta = $('#precio_venta').val();
    $.getJSON("/UnidadHerramienta/Guardar", { id: i, herramienta: id_herr, fecha_ingreso: fecha, precio_compra: p_compra, precio_venta: p_venta, estado: est }, function (e) {
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
    $.getJSON("/UnidadHerramienta/Get", o, function (obj) {
        var codigo = '<p class="red-text text-darken-3 flow-text">EDITAR ' + obj.nombre + '</p>';
        $('#cabeceraModal').html(codigo);
        $('#id').val(id);
        $('#selectHerramienta').val(obj.herramienta);
        $('#selectFecha').val(obj.fecha_ingreso);
        $('#precio_compra').val(obj.precio_compra);
        $('#precio_venta').val(obj.precio_venta);
        $('select').material_select();
        est = obj.estado;
        CargarEstadoEnChck(est);        
        Materialize.updateTextFields();
    });
    $('#modalDatos').modal('open');
};

function LimpiarCampos() {
    $('#id').val(0);
    $('#selectHerramienta').val('');
    $('#selectFecha').val('');
    $('#precio_compra').val('');
    $('#precio_venta').val('');
    $('select').material_select();
};

function Listar() {
    $.getJSON("/UnidadHerramienta/Listar", null, function (cadena) {
        $("#tabla").html(cadena);
        CrearDataTable();
    });
    $('#btnListar').show();
};

function ListarHerramientas() {
    $.getJSON("/Herramienta/ListarSelectHerramientas", function (cadena) {
        $('#campoHerramienta').html(cadena);
        $('select').material_select();
    });
};

function ModalConfirmar(id,nom) {
    $('#idEliminar').val(id);
    $('#nomEliminar').val(nom);
    var codigo = '<p class="red-text text-darken-3 flow-text">Esta seguro que desea Eliminar ' + nom + '?</p>';
    $('#cabeceraModalEliminar').html(codigo);
    $('#modalEliminar').modal('open');
}
$('#aceptarEliminar').click(function () {
    Eliminar($('#idEliminar').val());
    $('#modalEliminar').modal('close');
    Materialize.toast('La Unidad de Herramieta fue eliminado exitosamente!', 8000);
    Listar();
});
$('#cancelarEliminar').click(function () {
    $('#idEliminar').val('');
    $('#nomEliminar').val('');
    $('#modalEliminar').modal('close');
});

function Eliminar(id) {
    var o = { id: id };
    $.getJSON("/UnidadHerramienta/Delete", o, function (e) {
        Listar();
    });
};
