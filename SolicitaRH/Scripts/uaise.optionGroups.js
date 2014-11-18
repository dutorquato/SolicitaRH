// Função para adaptar os Optgroups
(function ($) {
    $.fn.optionGroups = function (options) {

        var defaults = {
            value: "OptionGroup"
        };

        var options = $.extend(defaults, options);

        return this.each(function () {
            var opts = $(this).children('option');
            var selected = $(this).val();
            var totalOptions = opts.length - 1;
            

            // Faz o replace
            var contaOption = 0;
            var contaRegistros = 0;
            var contaFim = false;
            var newDropDown = "";

            opts.each(function () {
                var valorAtual = $(this).val();
                var textoAtual = $(this).text();

                if (valorAtual == options.value) {
                    contaOption++;

                    if (contaOption == 1) {
                        newDropDown += "<optgroup label='" + $(this).text() + "'>\n";
                    } else {
                        if (contaRegistros != totalOptions) {
                            newDropDown += "</optgroup> <optgroup label='" + $(this).text() + "'>";
                        }
                    }
                } else {
                    newDropDown += "<option value='" + valorAtual + "'>" + textoAtual + "</option>\n";
                }

                if (contaOption > 0 && contaFim == false && contaRegistros == totalOptions) {
                    newDropDown += ("</optgroup>");
                }

            });
            $(this).remove("option");
            $(this).html(newDropDown);
            $(this).val(selected);
        });
    }
})(jQuery);