$(document).ready(function () {
    ListarDetalleServicio();
});

var arrayID = [], CostoTotal = 0, TiempoTotal = 0;

var array_descripciones = [];

function ListarDetalleServicio() {
    $.getJSON('/Reserva/ListarDetalleServicio', function (cadena) {
        $('#servicios').html(cadena);
        $('.collapsible').collapsible();
    });
};

function CambiarPantalla() {
    location.href = "/Reserva/CargarDetalle";
};

function Agregar(id, precio) {
    $('#btnGuardar').removeClass('disabled');;
    console.log(precio.replace(/,/, "."));
    precio = parseFloat(precio.replace(",", "."));
    if (document.getElementById(id).checked) {
        //alert('checado');
        arrayID.push(id);
        CostoTotal += precio;
        $('#costoTotal').html(CostoTotal);
        $('#nro').html(arrayID.length);
    } else {
        //alert('no checado');
        Elimiar(id, arrayID);

        CostoTotal -= precio;

        if (arrayID.length == 0) {
            CostoTotal = 0;
            $('#btnGuardar').addClass('disabled');
        }
        $('#costoTotal').html(CostoTotal);
        $('#nro').html(arrayID.length);
    }
    console.log(arrayID);
    console.log(CostoTotal);
};

function Elimiar(id, array) {
    var i;
    for (var ii = 0; ii < array.length; ii++) {
        if (array[ii] == id) {
            array_descripciones.splice(ii, 1);
            arrayID.splice(ii, 1);
            $('#des_' + ii).val('');
        }
    };
};

$('#btnGuardar').click(function () {
    ct = CostoTotal.toString();
    ct = (ct.replace(".", ","));
    for (var i = 0; i < arrayID.length ; i++) {
        console.log($('#des_' + arrayID[i]).val());
        array_descripciones.push($('#des_' + arrayID[i]).val());
    };
    var o = {
        id: 0,
        persona: "",
        usuario: "",
        monto_total: ct,
        detalles: arrayID.toString(),
        descripciones: array_descripciones.toString(),
        estado: true
    };
    $.getJSON('/Reserva/Guardar', o, function (msg) {
        if (msg != "") {
            Materialize.toast(msg, 8000);
        }
        else {
            Materialize.toast('Reserva Exitosa!', 8000);
            LimpiarCampos();
        }
    });
});

function LimpiarCampos() {
    $('#costoTotal').html('0,0');
    $('#nro').html('0');
    $('#btnGuardar').addClass('disabled');
    arrayID = [];
    array_descripciones = [];
    ListarDetalleServicio();
};








