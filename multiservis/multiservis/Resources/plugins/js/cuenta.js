$(document).ready(function () {
    LimpiarCampos();
});
var est = true;

function Nuevo() {
    LimpiarCampos();
    est = true;
    $('#modalRegistro').modal('open');
};

//$('#aceptar').click(function () {
//    if (EvaluarVacios()) {
//        Guardar();
//    }
//});
//$('#cancelar').click(function () {
//    LimpiarCampos();
//    $('#modalRegistro').modal('close');
//});
//function EvaluarVacios() {
//    if ($('#nombre').val() == '') {
//        Materialize.toast('El campo nombre no puede estar vacio!', 8000);
//        return false;
//    }
//    else {
//        return true;
//    }
//};
//function Guardar() {
//    var nom = $('#nombreRe').val();
//    var ape = $('#apellidoRe').val();
//    var cor = $('#correoRe').val();
//    var pas = $('#passRe').val();
//    $.getJSON("/Cuenta/CrearCuenta", { nombre: nom, apellido: ape, correo: cor, pass: pas }, function (e) {
//        if (e != "") {
//            Materialize.toast(e, 8000);
//        }
//        else {
//            Materialize.toast('Registro exitoso!', 8000);
//            LimpiarCampos();
//            $('#modalRegistro').modal('close');
//            ListarBoletines();
//        }
//    });
//    ListarBoletines();
//};

//function LimpiarCampos() {
//    $('#nik').val('');
//    $('#pass').val('');
//    $('#correoRe').val('');
//    $('#nombreRe').val('');
//    $('#apellidoRe').val('');
//    $('#passRe').val('');
//};





$('#aceptar').click(function () {
    if (EvaluarVacios()) {
        Guardar();
    }
});

$('#cancelar').click(function () {
    LimpiarCampos();
    $('#modalRegistro').modal('close');
});

function EvaluarVacios() {
    if ($('#nombres').val() == '') {
        Materialize.toast('Debe establecer un nombre!', 8000);
        return false;
    }
    if ($('#nacionalidad').val() == null) {
        Materialize.toast('Debe establecer una nacionalidad!', 8000);
        return false;
    }
    if ($('#ci').val() == '') {
        Materialize.toast('Debe establecer el C.I.!', 8000);
        return false;
    }
    if ($('#nombre_usuario').val() == '') {
        Materialize.toast('Debe establecer el nombre para el usuario!', 8000);
        return false;
    }
    if ($('#password_usuario').val() == '') {
        Materialize.toast('Debe establecer un password para el usuario!', 8000);
        return false;
    }
    else {
        return true;
    }
};
function Guardar() {
    var o = {
        nombres: $('#nombres').val(),
        parterno: $('#paterno').val(),
        materno: $('#materno').val(),
        correo: $('#correo').val(),
        nacionalidad: $('#nacionalidad').val(),
        ci: $('#ci').val(),
        telefono: $('#telefono').val(),
        direccion: $('#direccion').val(),
        nombre_usuario: $('#nombre_usuario').val(),
        password_usuario: $('#password_usuario').val()
    };
    $.getJSON("/Cuenta/CrearCuenta", o, function (e) {
        if (e != "") {
            Materialize.toast(e, 8000);
        }
        else {
            Materialize.toast('Registro exitoso!', 8000);
            LimpiarCampos();
            $('#modalRegistro').modal('close');
        }
    });
};
function LimpiarCampos() {
    $('#nik').val('');
    $('#pass').val('');
    $('#nombres').val(''),
    $('#paterno').val(''),
    $('#materno').val(''),
    $('#correo').val(''),
    $('#nacionalidad').val(''),
    $('#ci').val(''),
    $('#telefono').val(''),
    $('#direccion').val(''),
    $('#nombre_usuario').val(''),
    $('#password_usuario').val(''),
    $('select').material_select();
};
