function CerrarSideNav() {
    $('button-collapse').sideNav('hide');
};
$("#AbriMenu").click(function () {
    $('.button-collapse').sideNav('show');
});
$(document).ready(function () {
    $('.modal').modal();
    $('.parallax').parallax();
    $('.button-collapse').sideNav();
    $('.tooltipped').tooltip({ delay: 50 });
    $('.slider').slider();
    $('.collapsible').collapsible();
    $('.materialboxed').materialbox();
    $('select').material_select();
    $('.scrollspy').scrollSpy();
    $('input#input_text, textarea#descripcionTema').characterCounter();
    $('.datepicker').pickadate({
        selectMonths: true,
        selectYears: 2
    });
});
