var est = true;
$(document).ready(function () {
    Listar();
    $('#salario').numeric();
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
    if ($('#nro_seguro').val() == '') {
        Materialize.toast('Debe establecer el nro de seguro!', 8000);
        return false;
    }
    if ($('#salario').val() == '') {
        Materialize.toast('Debe establecer un salario minimo 0!', 8000);
        return false;
    }
    if ($('#fecha_inscripcion').val() == '') {
        Materialize.toast('Debe establecer la fecha de inscripcion!', 8000);
        return false;
    }
    else {
        return true;
    }
};


////Para la Imagen
//var contents;
//function readSingleFile(evt) {
//    //Retrieve the first (and only!) File from the FileList object
//    var f = evt.target.files[0];

//    if (f) {
//        var r = new FileReader();
//        r.onload = function (e) {
//            contents = e.target.result;

//            //alert("Got the file.n"
//            //      + "name: " + f.name + "n"
//            //      + "type: " + f.type + "n"
//            //      + "size: " + f.size + " bytesn"
//            //      + "contents:" + contents
//            //     );
//        }
//        r.readAsText(f);
//    } else {
//        alert("Failed to load file");
//    }
//}

//document.getElementById('img').addEventListener('change', readSingleFile, false);



//Para la Imagen
var reader = new FileReader();

function vistaPrevia(input) {
    if (input.files && input.files[0]) {
        //var reader = new FileReader();
        reader.onload = function (e) {
            $('#img_prev').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

$("#file_img").change(function () {
    vistaPrevia(this);
});

function Guardar() {


    var o = {
        id: $('#id').val(),
        nombres: $('#nombres').val(),
        parterno: $('#paterno').val(),
        materno: $('#materno').val(),
        correo: $('#correo').val(),
        nacionalidad: $('#nacionalidad').val(),
        ci: $('#ci').val(),
        telefono: $('#telefono').val(),
        direccion: $('#direccion').val(),
        nro_seguro: $('#nro_seguro').val(),
        salario: $('#salario').val(),
        fecha_inscripcion: $('#fecha_inscripcion').val(),
        estado: est
    };
    $.getJSON("/Tecnico/Guardar", o, function (r) {

        console.log(r);

        if (r.check) {


            SubirImagen(r.id);

            Materialize.toast('Registro exitoso!', 8000);
            LimpiarCampos();
            $('#modalDatos').modal('close');
            Listar();
        }
        else {
            Materialize.toast(r.msg, 8000);
        }
    });
    Listar();
};
function Editar(id) {
    var o = { id: id };
    $.getJSON("/Tecnico/Get", o, function (obj) {
        var codigo = '<p class="red-text text-darken-3 flow-text">EDITAR ' + obj.nombre + '</p>';
        $('#cabeceraModal').html(codigo);
        $('#id').val(id);
        $('#nombres').val(obj.nombres);
        $('#paterno').val(obj.paterno);
        $('#materno').val(obj.materno);
        $('#correo').val(obj.correo);
        $('#nacionalidad').val(obj.nacionalidad);
        $('#ci').val(obj.ci);
        $('#telefono').val(obj.telefono);
        $('#direccion').val(obj.direccion);
        $('#nro_seguro').val(obj.nro_seguro);
        $('#salario').val(obj.salario);
        $('#fecha_inscripcion').val(obj.fecha_inscripcion);
        $('select').material_select();

        $('#img_prev').attr('src', obj.ruta_img);
        


        est = obj.estado;
        CargarEstadoEnChck(est);
        Materialize.updateTextFields();
    });
    $('#modalDatos').modal('open');
};

function LimpiarCampos() {
    $('#id').val(0),
    $('#nombres').val(''),
    $('#paterno').val(''),
    $('#materno').val(''),
    $('#correo').val(''),
    $('#nacionalidad').val(''),
    $('#ci').val(''),
    $('#telefono').val(''),
    $('#direccion').val(''),
    $('#nro_seguro').val(''),
    $('#salario').val(''),
    $('#fecha_inscripcion').val(''),
    $('select').material_select();
};

function Listar() {
    $.getJSON("/Tecnico/Listar", null, function (cadena) {
        $("#tabla").html(cadena);
        CrearDataTable();
    });
    $('#btnListar').show();
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
    Materialize.toast('El Tecnico fue eliminado exitosamente!', 8000);
    Listar();
});
$('#cancelarEliminar').click(function () {
    $('#idEliminar').val('');
    $('#nomEliminar').val('');
    $('#modalEliminar').modal('close');
});

function Eliminar(id) {
    var o = { id: id };
    $.getJSON("/Tecnico/Delete", o, function (e) {
        Listar();
    });
};


function SubirImagen(id) {
    var formData = new FormData();
    var totalFiles = document.getElementById("file_img").files.length;
    for (var i = 0; i < totalFiles; i++) {
        var file = document.getElementById("file_img").files[i];
        formData.append("file_img", file);
        formData.append('id_img', id);
    }
    $.ajax({
        type: "POST",
        url: '/Tecnico/SubirImagen',
        data: formData,
        dataType: 'json',
        contentType: false,
        processData: false,
    });
};


$(document).ready(function () {
    $("#Upload").click(function () {
    });
});