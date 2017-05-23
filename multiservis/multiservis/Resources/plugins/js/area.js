var est = true;
//Restringir Numeros
$(document).ready(function () {
    Listar();
})

$('#activo').click(function () {
    est = true;
    //alert(est);

});
$('#inactivo').click(function () {
    est = false;
    //alert(est);
});




function CargarEstadoEnChck(flag) {
    if (flag) {
        //alert('Cargando estado true');
        $('#activo').prop('checked', true);
    }
    else {
        //alert('Cargando estado false');
        $('#inactivo').prop('checked', true);
    }

}



function Nuevo() {
    LimpiarCampos();
    est = true;   
    CargarEstadoEnChck(est);
    $('#modalDatos').modal('open');
    var codigo = '<p class="light-blue-text text-darken-4 flow-text">CREAR NUEVO</p>';
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
    if ($('#nombre').val() == '') {
        Materialize.toast('El campo nombre no puede estar vacio!', 8000);
        return false;
    }
    else {
        return true;
    }
};
function Guardar() {
    var i = $('#id').val();
    var nom = $('#nombre').val();
    $.getJSON("/Area/Guardar", { id: i, nombre: nom, estado: est }, function (e) {
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
    $.getJSON("/Area/Get", o, function (obj) {

        var codigo = '<p class="light-blue-text text-darken-4 flow-text">EDITAR ' + obj.nombre + '</p>';
        $('#cabeceraModal').html(codigo);
        $("#id").val(id);
        $("#nombre").val(obj.nombre);
        est = obj.estado;
        CargarEstadoEnChck(est);
        //Activar Campos
        Materialize.updateTextFields();
    });

    $('#campo_estado').show();
    $('#modalDatos').modal('open');
};

function LimpiarCampos() {
    $('#id').val(0);
    $('#nombre').val('');
};
function Listar() {
    $.getJSON("/Area/Listar", null, function (cadena) {
        $("#tabla").html(cadena);
    });
    $('#btnListar').show();
    //$('#datatable').datatable();
};



//Funciones para eliminar
function ModalConfirmar(id,nom) {
    //alert(id + nom);
    $('#idEliminar').val(id);
    $('#nomEliminar').val(nom);
    var codigo = '<p class="light-blue-text text-darken-4 flow-text">Esta seguro que desea Eliminar ' + nom + '?</p>';
    $('#cabeceraModalEliminar').html(codigo);
    $('#modalEliminar').modal('open');
}
$('#aceptarEliminar').click(function () {
    Eliminar($('#idEliminar').val());
    $('#modalEliminar').modal('close');
    Materialize.toast('El tipo de grupo fue eliminado exitosamente!', 8000);
    Listar();
});
$('#cancelarEliminar').click(function () {
    $('#idEliminar').val('');
    $('#nomEliminar').val('');
    $('#modalEliminar').modal('close');
});

function Eliminar(id) {
    var o = { id: id };
    $.getJSON("/Area/Delete", o, function (e) {
        Listar();
    });
};
