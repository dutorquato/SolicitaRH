// Função para adaptar os Optgroups
(function ($) {
    $.alerta = function (options) {

        var defaults = {
            mensagem: "",
            link: ""
        };

        var options = $.extend(defaults, options);


        $("#Modal-btn-continuar").hide();        
        $("#Modal-btn-cancelar").hide();
        $("#Modal-btn-fechar").show();


        $("#Modal-btn-fechar").click(function () {
            $("#AlertaModal").hide();
            if (options.link != "") {
                location.href = options.link;
            }
        });


        $("#Modal-texto").html(options.mensagem);
        $('#AlertaModal').modal('toggle');

    }
    
    $.confirma = function (options) {
        var i = 0;
        var defaults = {
            mensagem: "",
            cancelar: "",
            continuar: $.noop            
        };
        
        var options = $.extend(defaults, options);
        //options = null;
        $("#Modal-btn-continuar").show();        
        $("#Modal-btn-cancelar").show();
        $("#Modal-btn-fechar").hide();

        // Limpa a ação do Click
        $("#Modal-btn-continuar").unbind('click');

        $("#Modal-btn-continuar").click(function () {
            $("#AlertaModal").modal('hide');
            setTimeout(function () {
                defaults.continuar.call();
            }, 500);
        });
              

        $("#Modal-texto").html(defaults.mensagem);
        $('#AlertaModal').modal('toggle');
        
    }
})(jQuery);