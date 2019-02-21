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
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if ((charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57)) || charCode == 46)
        return false;
    return true;
}