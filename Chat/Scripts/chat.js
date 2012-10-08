$(document).ready(function () {
    // Post /Chat/New
    $('#btnEnviar').bind('click', function () {

        var xavatar = null;

        if (document.getElementsByName("txtavatar")[0].checked)
            xavatar = document.getElementsByName("txtavatar")[0].value;

        if (document.getElementsByName("txtavatar")[1].checked)
            xavatar = document.getElementsByName("txtavatar")[1].value;

        if (document.getElementsByName("txtavatar")[2].checked)
            xavatar = document.getElementsByName("txtavatar")[2].value;

        if (document.getElementsByName("txtavatar")[3].checked)
            xavatar = document.getElementsByName("txtavatar")[3].value;

        if (document.getElementsByName("txtavatar")[4].checked)
            xavatar = $('#txtavatar4').val();

        if (document.getElementsByName("txtavatar")[5].checked)
            xavatar = document.getElementsByName("txtavatar")[5].value;

        if (document.getElementsByName("txtavatar")[6].checked)
            xavatar = document.getElementsByName("txtavatar")[6].value;

        if (document.getElementsByName("txtavatar")[7].checked)
            xavatar = document.getElementsByName("txtavatar")[7].value;

        if (document.getElementsByName("txtavatar")[8].checked)
            xavatar = document.getElementsByName("txtavatar")[8].value;

        if (document.getElementsByName("txtavatar")[9].checked)
            xavatar = document.getElementsByName("txtavatar")[9].value;
        

        var msgVal = $('#txtMensagem').val();

        $('#txtMensagem').val('');
        $.post("/Chat/New", {  avatar: xavatar, nome: $('#txtNome').val(), msg: msgVal }, function (data, s) {
            if (data.d) {
                //mensagem adicionada
            }
            else {
                //erro ao adicionar
            }

        });
    });

    //Envia a mensagem com enter
    $('#txtMensagem').keydown(function (e) {
        if (e.keyCode == 13) {
            $('#btnEnviar').click();
        }
    });

    setTimeout(function () {
        getMensagens();
    }, 100)
});

function getMensagens() {
    $.post("/Chat", null, function (data, s) {
        if (data.mensagens) {
            $('#msgTmpl').tmpl(data.mensagens).appendTo('#chatList');
        }
        setTimeout(function () {
            getMensagens();
        }, 500)
    });
}