var est = true;
$(document).ready(function () {
    Listar();
    //ListarDetalleReserva();
    ListarUnidadMaterial();
    ListarUnidadHerramienta();
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


var id_detalle_reserva = 0;

function Nuevo(id_) {
    id_detalle_reserva = id_;
    alert(id_detalle_reserva);
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
    //if ($('#selectDetalleReserva').val() == null) {
    //    Materialize.toast('Debe establecer el Detalle Reserva!', 8000);
    //    return false;
    //}
    if ($('#selectUnidadMaterial').val() == null) {
        Materialize.toast('Debe establecer almenos una unidad de material!', 8000);
        return false;
    }
    if ($('#selectUnidadHerramienta').val() == null) {
        Materialize.toast('Debe establecer almenos una unidad de herramienta!', 8000);
        return false;
    }
    if ($('#snro_ficha').val() == '') {
        Materialize.toast('Debe establecer el nro de ficha!', 8000);
        return false;
    }
    else {
        return true;
    }
};

function Guardar() {
    //int id, int detalle_reserva, int usuario_almacen, 
    //int nro_ficha, string descripcion, string herramientas, string materiales
    var o = {
        id: $('#id').val(),
        detalle_reserva: id_detalle_reserva,
        usuario_almacen: '',
        nro_ficha: $('#nro_ficha').val(),
        descripcion: $('#descripcion').val(),
        materiales: ($('#selectUnidadMaterial').val()).toString(),
        herramientas: ($('#selectUnidadHerramienta').val()).toString()
    };
    //console.log(o);
    $.getJSON("/FichaTecnica/Guardar", o, function (e) {
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
    $.getJSON("/FichaTecnica/Get", o, function (obj) {
        var codigo = '<p class="red-text text-darken-3 flow-text">DATOS DE FICHA TECNICA</p>';
        $('#cabeceraModal').html(codigo);
        $('#id').val(id);
        //$('#selectDetalleReserva').val(obj.detalle_reserva);
        $('#nro_ficha').val(obj.nro_ficha);
        $('#descripcion').val(obj.descripcion);


        var array_materiales = obj.materiales.split(",");
        var arrayAux1 = [];
        for (var i = 0; i < array_materiales.length; i++) {
            var elem1 = parseInt(array_materiales[i]);
            arrayAux1.push(elem1);
        }

        var array_herramientas = obj.herramientas.split(",");
        var arrayAux2 = [];
        for (var i = 0; i < array_herramientas.length; i++) {
            var elem2 = parseInt(array_herramientas[i]);
            arrayAux2.push(elem2);
        }

        $('#selectUnidadMaterial').val(arrayAux1);
        $('#selectUnidadHerramienta').val(arrayAux2);
        $('select').material_select();
        Materialize.updateTextFields();
    });
    $('#modalDatos').modal('open');
};

function LimpiarCampos() {
    $('#id').val(0),
    $('#selectUnidadMaterial').val(''),
    $('#selectUnidadHerramienta').val(''),
    $('select').material_select();
    $('#nro_ficha').val('');
    $('#descripcion').val('');
    
};

function Listar() {
    $.getJSON("/FichaTecnica/Listar", null, function (cadena) {
        $("#tabla").html(cadena);
        CrearDataTable();
    });
    $('#btnListar').show();
};

//function ListarDetalleReserva() {
//    $.getJSON("/FichaTecnica/ListarDetalleReserva", function (cadena) {
//        $('#campoDetalleReserva').html(cadena);
//        $('select').material_select();
//    });
//};
function ListarUnidadMaterial() {
    $.getJSON("/FichaTecnica/ListarUnidadMaterial", function (cadena) {
        $('#campoUnidadMaterial').html(cadena);
        $('select').material_select();
    });
};
function ListarUnidadHerramienta() {
    $.getJSON("/FichaTecnica/ListarUnidadHerramienta", function (cadena) {
        $('#campoUnidadHerramienta').html(cadena);
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
    Materialize.toast('El Registro de Ficha Tecnica fue eliminado exitosamente!', 8000);
    Listar();
});
$('#cancelarEliminar').click(function () {
    $('#idEliminar').val('');
    $('#nomEliminar').val('');
    $('#modalEliminar').modal('close');
});

function Eliminar(id) {
    var o = { id: id };
    $.getJSON("/FichaTecnica/Delete", o, function (e) {
        Listar();
    });
};
