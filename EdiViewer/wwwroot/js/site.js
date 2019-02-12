var modalPop1 = null;
//function afterModalTransition(e) {
//    e.setAttribute("style", "display: none !important;");
//}
function menErrorEdi(MensajeO) {
    $('#divMensajePop1').html(MensajeO);
    modalPop1 = $('#modalPop1').modal();    
    //$('#divMensajePop1').on('hide.bs.modal', function (e) {
    //    setTimeout(() => afterModalTransition(this), 200);
    //})

}