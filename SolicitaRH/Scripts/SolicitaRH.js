var Base = $("base").attr('href');

$(document).ready(function () {
    jQuery.ajaxSetup({
        beforeSend: function () {
            $('#loader').show();
        },
        async: true,
        complete: function () {
            $('#loader').hide();
        },
        success: function () { }
    });

    $('input[type="text"]').setMask();

    $(".progressBar").each(function () {
        var valor = ($(this).text());

        $(this).progressBar(valor, { showText: true });
    });

    // Categorias/Index js para esconder e mostrar as subcats
    $('.btnMostraSub').click(function () {
        var CategoriaId = $(this).attr("lang");

        $('.btnMostraSub-' + CategoriaId).hide();
        $('.btnEscondeSub-' + CategoriaId).fadeIn();

        $('.Subs-' + CategoriaId).fadeIn();
    });
    $('.btnEscondeSub').click(function () {
        var CategoriaId = $(this).attr("lang");

        $('.btnEscondeSub-' + CategoriaId).hide();
        $('.btnMostraSub-' + CategoriaId).fadeIn();

        $('.Subs-' + CategoriaId).hide();
    });


    // Etapas/Index js para buscar ou esconder as acoes de cada etapa - torquato
    $('.btnMostraAcoes').click(function () {
        var Identificador = $(this).attr("lang");

        // Solicita info das acoes
        $.post(Base + "Etapas/PegaAcoesStr/", { 'Identificador': Identificador }, function (resposta) {
            if (resposta != "") {
                $('.Acoes-' + Identificador).html(resposta);
                $('span[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
            } else {
                $('.Acoes-' + Identificador).html(" - " + Msg_NaoExisteAcaoEtapa);
            }
        }, "json");

        $('.btnMostraAcoes-' + Identificador).hide();
        $('.btnEscondeAcoes-' + Identificador).fadeIn();

        $('.Acoes-' + Identificador).fadeIn();
    });
    $('.btnEscondeAcoes').click(function () {
        var Identificador = $(this).attr("lang");

        $('.btnEscondeAcoes-' + Identificador).hide();
        $('.btnMostraAcoes-' + Identificador).fadeIn();

        $('.Acoes-' + Identificador).hide();
    });
});


function MinimizaTicketsFiltros(Minimiza) {
    $.post(Base + "TicketsFiltros/AlteraMinimizaTicketsFiltros/", { 'Minimiza': Minimiza }, function (resposta) {
        if (resposta) {
            $(".TicketsFiltros").empty();
            $.post("TicketsFiltros/Index", {}, function (r) {
                $(".TicketsFiltros").hide();
                $(".TicketsFiltros").html(r);
                $(".TicketsFiltros").fadeIn();
                //location.href=("#11");
            },"html");

        }
    }, "json");
}

function AbreFiltros() {
    //alert('teste');
    window.open(base + "TicketsFiltros/Create", "Filtro", "width=700, height=400, left=400, top=200, scrollbars=yes");
}

function AdicionaAnexo(Indice) {
    NovoIndice = parseInt(Indice) + 1;
    
    var NovoAnexo = '<div class="editor-label">Anexo ' + NovoIndice + '</div> ';
    NovoAnexo = NovoAnexo + '   <div id=""Ticket-Anexo-Div" class="editor-field">';
    NovoAnexo = NovoAnexo + '   <input type="file" name="Anexo-' + NovoIndice + '" />';
    //NovoAnexo = NovoAnexo + '   <input type="button" class="MaisAnexo" lang="' + NovoIndice + '" id="MaisAnexo-' + NovoIndice + '1" value="+" />';
    NovoAnexo = NovoAnexo + '</div> ';

    $('#Div-Anexos').append(NovoAnexo);
}

//Kadu Aguiar 17/06/2013
function carregaResultadoRespostas() {

    var totalPerguntas = $("#Perguntas").length;
    
    if (totalPerguntas == 1) {
        $.post(Base + "Perguntas/Busca/", { 'PerguntaPreId': 1 }, function (resposta) {
            if (resposta[0].Titulo) $("#Tickets_Assunto").val(resposta[0].Titulo);
            if (resposta[0].Conteudo) $("#Interacao_Interacao").val(resposta[0].Conteudo);
            if (resposta[0].Tags) $("#Tags_Tag").importTags(resposta[0].Tags);
        }, "json");
    }

}

function PegaImagem(Topico, CategoriaId) {
    $.post(Base + "Tickets/PegaImagem/", { 'TopicosId': Topico, 'SubCategoriaId': CategoriaId }, function (retorno) {
        if (retorno == null) {
            $("#div-imagem").html('');
        } else {
            $('#div-imagem').html('<img src="'+ base +'Uploads/Imagens/' + retorno.Arquivo + '" />');
        }
    }, "json");
}

function MostraMensagem(Topico, CategoriaId) {
    $.post(Base + "Tickets/MostraMensagem/", { 'TopicosId': Topico, 'SubCategoriaId': CategoriaId }, function (retorno) {
        if (retorno != null) {
            $.alerta({
                mensagem: retorno.Mensagem
            });
        }
    }, "json");
}

function ConfereCamposObrigatorios() {
    var ConfGeral = true;
    $(".requerido").each(function () {
        var id = $(this).attr("id");
        if ($(this).val() == "") {
            /*$('#' + id + '_validationMessage').html('* Campos Obrigatórios');
            $('#' + id + '_validationMessage').show();
            alert($(this).attr("id"));*/
            ConfGeral = false;
        }
    });

    var ConfMultiplasEscolhas = true;
    $('.divMultiplasEscolhasRequerido').each(function () {
        var divId = $(this).attr("id");
        var ConfMultipla = false;

        $('#' + divId + ' input[type="checkbox"]').each(function () {
            var checkId = $(this).attr("id");
            if ($('#' + checkId).is(":checked") == true) {
                ConfMultipla = true;
            }
        });
        if (ConfMultipla == false) {
            ConfMultiplasEscolhas = false;
        }
    });

    //alert("ConfGeral: " + ConfGeral + "\nConfMultiplasEscolhas: " + ConfMultiplasEscolhas);
    return ConfGeral && ConfMultiplasEscolhas;
}

//ConfereCamposTicket Desabilitada pois o ck editor nao pega evento
function ConfereSeTicketPreenchido(ExigeValidacaoMatricula) {
    var msg = "";

    // Valida campos padrao do cadastro de ticket
    var CategoriaPai = $('#CategoriaPai').val();
    var Categoria = $('#Tickets_CategoriaId').val();
    var Topico = $('#Tickets_TopicoId').val();
    var Celula = $('#Celula-Div-Nome').html();
    var Assunto = $('#Tickets_Assunto').val();
    var Interacao = $('#Interacao_Interacao').val();

    var MatriculasVinculadas = "nao vazio";
    if (ExigeValidacaoMatricula) {
        //MatriculasVinculadas = $('#Temp_MatriculasVinculadas').val();
        MatriculasVinculadas = $('#Tickets_MatriculasVinculadas').val();
    }

    //alert('CategoriaPai: ' + CategoriaPai + '\nCategoria: ' + Categoria + '\nTopico: ' + Topico + '\nCelula: ' + Celula + '\nAssunto: ' + Assunto + '\nInteracao: ' + Interacao);

    // Mostra msg de campos obrigatorios preenche a msg
    $('#TopicoCatCatPaiCel_validationMessage').hide();
    $('#Tickets_Assunto_validationMessage').hide();
    $('#Interacao_Interacao_validationMessage').hide();
    $('#Tickets_MatriculasVinculadas_validationMessage').hide();

    if (CategoriaPai == "") {
        $('#TopicoCatCatPaiCel_validationMessage').html('* Campos Obrigatórios');
        $('#TopicoCatCatPaiCel_validationMessage').show();
        msg = msg + "CategoriaPai,";
    }
    if (Categoria == "" || Categoria == null) {
        $('#TopicoCatCatPaiCel_validationMessage').html('* Campos Obrigatórios');
        $('#TopicoCatCatPaiCel_validationMessage').show();
        msg = msg + "Categoria,";
    }
    if (Topico == "" || Topico == null) {
        $('#TopicoCatCatPaiCel_validationMessage').html('* Campos Obrigatórios');
        $('#TopicoCatCatPaiCel_validationMessage').show();
        msg = msg + "Topico,";
    }
    if (Celula == "") {
        $('#TopicoCatCatPaiCel_validationMessage').html('* Campos Obrigatórios');
        $('#TopicoCatCatPaiCel_validationMessage').show();
        msg = msg + "Celula,";
    }
    if (Assunto == "") {
        $('#Tickets_Assunto_validationMessage').html('* Campo Obrigatório');
        $('#Tickets_Assunto_validationMessage').show();
        msg = msg + "Assunto,";
    }
    if (Interacao == "") {
        $('#Interacao_Interacao_validationMessage').html('* Campo Obrigatório');
        $('#Interacao_Interacao_validationMessage').show();
        msg = msg + "Interacao,";
    }

    if (MatriculasVinculadas == "") {
        $('#Tickets_MatriculasVinculadas_validationMessage').html('* Campo Obrigatório');
        $('#Tickets_MatriculasVinculadas_validationMessage').show();
        msg = msg + "MatriculasVinculadas,";
    }

    // Retorno
    if (msg == "") {
        if (ConfereCamposObrigatorios()) {
            return "1";
            //$("#Btn-Abrir-Ticket").attr("disabled", false);
        } else {
            return "-1";
            //$("#Btn-Abrir-Ticket").attr("disabled", true);
        }
    } else {
        return "0";
        //$("#Btn-Abrir-Ticket").attr("disabled", true);
    }
}

function AtualizaPermissaoCatPai(CelulaId, CategoriaPai, Permissao) {
    //alert('CelulaId: ' + CelulaId + '\nCat: ' + CategoriaPai + '\nPermissao: ' + Permissao);

    // Passa o texto do combo desta catpai para Sem Visualização
    $('.cbo-' + CategoriaPai).val('Sem Visualização');

    $.post(Base + "Celulas/AtualizaPermissoes", { 'CelulaId': CelulaId, 'CategoriaId': CategoriaPai, 'Permissao': Permissao }, function (resposta) {
        if (resposta == "1") {
            $('.C1-' + CategoriaPai).fadeIn();
            $('.C2-' + CategoriaPai).fadeIn();
        } else {
            $('.C1-' + CategoriaPai).hide();
            $('.C2-' + CategoriaPai).hide();
        }
    }, "json");
}

function AtualizaPermissao(CelulaId, CategoriaId, Permissao) {
    $.post(Base + "Celulas/AtualizaPermissoes", { 'CelulaId': CelulaId, 'CategoriaId': CategoriaId, 'Permissao': Permissao }, function (resposta) {
        $("#celula-alerta-" + CategoriaId).fadeIn('slow').animate({ opacity: 1.0 }, 3000).fadeOut('slow', function () {
            $().remove();
        });
    }, "json");
}

function AtualizaProcedenciaTicket(TicketId, Procedente) {
    $.post(Base + "Tickets/AtualizaProcedente", { 'TicketId': TicketId, 'Procedente': Procedente }, function (r) {
        $("#procedente-alerta").fadeIn('slow').animate({ opacity: 1.0 }, 3000).fadeOut('slow', function () {
            $().remove();
        });
    }, "json");
}

function AtualizaCelulaUsuario(UsuarioId, CelulaId) {
    $.post(Base + "Usuarios/AtualizaPermissoes", { 'UsuarioId': UsuarioId, 'CelulaId': CelulaId }, function (resposta) {
        $("#celula-alerta-" + CelulaId).fadeIn('slow').animate({ opacity: 1.0 }, 3000).fadeOut('slow', function () {
            $().remove();
        });
    }, "json");
}

function AtualizaTicketsPlanejamento() {
    //alert('Status: ' + Status);
    //return false;

    var base = $('base').attr("href");
    link = base + "Tickets/_PlanejamentoTickets";
    $.post(link, $('#frmFiltroPlanejamento').serialize(), function (r) {

        $("#Div-Tickets-Planejamento").hide();
        $("#Div-Tickets-Planejamento").empty();
        $("#Div-Tickets-Planejamento").fadeIn();
        $("#Div-Tickets-Planejamento").html(r);

        $("#Div-Tickets-Planejamento .datepicker").each(function () {
            $(".datepicker").datepicker("option", "dateFormat", "dd/mm/yy");
            $(".datepicker").datepicker({
                numberOfMonths: 3,
                showButtonPanel: true,
                dateFormat: "dd/mm/yy"
            });
        });

        /*$("#Div-Tickets-Planejamento .progressBar").each(function () {
            var valor = ($(this).text());
            $(this).progressBar(valor, { showText: true });
        });*/
    }, "html");

}

function ExisteFeedback(id) {
    $.post(Base + 'Tickets/buscaFeedbackPorId', { 'id': id }, function (success) {
        if (success=='False') {
                        $('#Feedback').show("slow")
                    } else {
            $('#Feedback').hide("slow")
                    }  
    }, 'html');
}

function TicketAcaoContinua(rAcao, Justificativa) {
    // Confere se existe cobrança de Pre planejamento ou planejamento
    /*if (rAcao.ExigePrePlanejamento || rAcao.ExigePlanejamento) {
        window.location = "Planejamentos/Adicionar/?TicketId=" + $("#TicketId").val() + "&AcaoId=";
    }*/

    // Muda o Status no controller
    var TicketId = $("#TicketId").val();

    //var rrAcao = rAcao.json

    //alert('EtapaEnvia: ' + rAcao.EtapaEnvia);
    //alert('0');
    $.post(Base + "Tickets/AlterarStatus/",
    { // Passa todos os campos do tipo `Acoes`
        'TicketId': TicketId
        , 'rAcao.AcoesId': rAcao.AcoesId
        , 'rAcao.Nome': rAcao.Nome
        , 'rAcao.EtapaEnvia': rAcao.EtapaEnvia
        , 'rAcao.TextoConfirmacao': rAcao.TextoConfirmacao
        , 'rAcao.ModoExibicao': rAcao.ModoExibicao
        , 'rAcao.AnalistaVisualiza': rAcao.AnalistaVisualiza
        , 'rAcao.MarcaAnalista': rAcao.MarcaAnalista
        , 'rAcao.DesmarcaAnalista': rAcao.DesmarcaAnalista
        , 'rAcao.UsuarioVisualiza': rAcao.UsuarioVisualiza
        , 'rAcao.ExigePlanejamento': rAcao.ExigePlanejamento
        , 'rAcao.ExigePrePlanejamento': rAcao.ExigePrePlanejamento
        , 'rAcao.ExigeJustificativa': rAcao.ExigeJustificativa
        , 'Justificativa': Justificativa 
    }, function (retorno) {
        //alert('1');
        if (retorno == "1") {
            //alert('2');
            $.alerta({
                mensagem: "Ação realizada com sucesso.",
                link: Base + 'Tickets/Details/' + TicketId
            });
            //alert('3');
        } else if (retorno == "preplanejamento") {
            $.alerta({
                mensagem: MsgAcao_PrePlanejmento,
                link: Base + 'Planejamentos/Adicionar/?TicketId=' + TicketId
            });
        } else if (retorno == "planejamento") {
            $.alerta({
                mensagem: MsgAcao_Planejmento,
                link: Base + 'Planejamentos/Adicionar/?TicketId=' + TicketId + '&AcaoId=' + rAcao.AcoesId
            });
        }
    }, "json");

    //if(rAcao.Exige)
    
}

function TicketAcao(AcaoId, Justificativa){
//function TicketAcao(Acao, Status) {
    // Pega informações da ação e realiza ações e conferencias no else    
    var rAcao;
    $.post(Base + "Acoes/PegaDadosAcoes/", { 'AcaoId': AcaoId }, function (retorno) {
        rAcao = retorno;
        if (rAcao == null) {
            alert('Ocorreu um erro ao localizar a ação solicitada.');
            return false;
        } else {
            // Confere se existe justifica e cobra seu preenchimento
            if (rAcao.ExigeJustificativa == true && Justificativa.replace(" ", "") == "") {
                // Preenche informações da justificativa
                $('#Ticket-Justificativa-AcaoId').val(AcaoId);
                $('#Ticket-Justificativa-Titulo').html('Informe o motivo de <b>' + rAcao.Nome + "</b>");
                $('#Ticket-Justificativa-Confirmado').html(rAcao.Nome);

                // Exibe e posiciona Justificativa
                $("#Ticket-Justificativa-Div").slideDown("normal");
                $('html, body').stop().animate({
                    scrollTop: $("#Ticket-Justificativa-Div").offset().top
                }, 1500);

                return false;
            }

            // Confere se existe texto de confirmação e o exibe antes de continuar
            if (rAcao.TextoConfirmacao != null && rAcao.TextoConfirmacao.replace(" ", "") != "") {
                $.confirma({
                    mensagem: rAcao.TextoConfirmacao,
                    cancelar: function () {
                        return false;
                    },
                    continuar: function () {
                        //alert('Executar as ações 1');
                        TicketAcaoContinua(rAcao, Justificativa);
                    }
                });

            } else {
                //alert('Executar as ações 2');
                TicketAcaoContinua(rAcao, Justificativa);
            }
        }
    }, "json");
}

function confereSessao() {
    $.post(base + "Login/Status", {}, function (status) {
        if (status == 0) {
            location.href = base + "Login";
            return false;
        }
    });
}

function AtualizaTicketsPainelGeral(Status, PaginaAtual) {
    confereSessao();

    $('#Tickets-Carregando-' + Status).show();

    $('#Pagina-' + Status).val(PaginaAtual);
    
    var base = $('base').attr("href");
    link = base + "Tickets/_TicketPainel";
    if (Status == "usuario") {
        link = base + "Tickets/_TicketPainelUsuario";
    }
    
    // Rola para o topo da seção
    $("#Tbl-Tickets-" + Status).slideUp("slow");

    $.post(link, $('#frmFiltroGeral-' + Status).serialize(), function (r) {
        $("#Tickets-Carregando-" + Status).hide();
        
        $("#Div-Tickets-" + Status).empty();
        $("#Div-Tickets-" + Status).html(r);

        $("#Tbl-Tickets-" + Status).fadeIn();

        $('a[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('img[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('td[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('span[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('button[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });

        /*$("#Div-Tickets-" + Status + " .progressBar").each(function () {
            var valor = ($(this).text());
            $(this).progressBar(valor, { showText: true });
        });*/

    }, "html");
}

function AtualizaTicketsUsuario(Status, PaginaAtual) {
    //alert('Status: ' + Status);
    //return false;
    confereSessao();
    $('#Tickets-Carregando-usuario').show();

    $('#Pagina-usuario').val(PaginaAtual);
    $('#Status-usuario').val(Status);

    var base = $('base').attr("href");
    link = base + "Tickets/_TicketPainelUsuario";


    // Rola para o topo da seção
    /*$('html, body').animate({
        scrollTop: $("#Div-Tickets-" + Status).offset().top
    }, 1000);*/
    $("#Tbl-Tickets-usuario").slideUp("slow");

    $.post(link, $('#frmFiltroUsuario').serialize(), function (r) {
        $("#Tickets-Carregando-usuario").hide();

        $("#Div-Tickets-usuario").empty();
        $("#Div-Tickets-usuario").html(r);

        $("#Tbl-Tickets-usuario").fadeIn();

        $('a[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('img[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('td[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('span[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('button[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });

        /*$("#Div-Tickets-" + Status + " .progressBar").each(function () {
            var valor = ($(this).text());
            $(this).progressBar(valor, { showText: true });
        });*/

    }, "html");
}

function AtualizaTicketsResponsavel(Status, PaginaAtual) {
    //alert('Status: ' + Status);
    //return false;
    confereSessao();
    $('#Tickets-Carregando-responsavel').show();

    $('#Pagina-responsavel').val(PaginaAtual);
    $('#Status-responsavel').val(Status);

    var base = $('base').attr("href");
    link = base + "Tickets/_TicketPainelResponsavel";


    // Rola para o topo da seção
    /*$('html, body').animate({
        scrollTop: $("#Div-Tickets-" + Status).offset().top
    }, 1000);*/
    $("#Tbl-Tickets-responsavel").slideUp("slow");

    $.post(link, $('#frmFiltroResponsavel').serialize(), function (r) {
        $("#Tickets-Carregando-responsavel").hide();

        $("#Div-Tickets-responsavel").empty();
        $("#Div-Tickets-responsavel").html(r);

        $("#Tbl-Tickets-responsavel").fadeIn();

        $('a[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('img[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('td[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('span[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });
        $('button[title]').tipsy({ html: true, title: function () { return "<span style='font-size:13px;'>" + this.getAttribute('original-title') + "</span>" } });

        /*$("#Div-Tickets-" + Status + " .progressBar").each(function () {
            var valor = ($(this).text());
            $(this).progressBar(valor, { showText: true });
        });*/

    }, "html");
}

function AtualizaTicketsPesquisa(PaginaAtual, Pesquisa) {
    var base = $('base').attr("href");
    link = base + "Tickets/Pesquisar/?Termo=" + Pesquisa + "&Pagina=" + PaginaAtual;

    window.location = link;
}

var Contador = 0;

// Tickets/Index - Dependendo de qual opção de filtragem for selecionada mostra um campo diferente || DESATIVADO
function AcrescentaTextoFiltro(OpcaoFiltro, Status, Contador) {
    //alert("Opcao: " + OpcaoFiltro + "\nStatus: " + Status + "\nContador: " + Contador);
    var input = "";

    if (OpcaoFiltro == "") {
        input = '<span id="spanFiltro-' + Status + '-' + Contador + '"> <input type="text" name="Filtro-' + Contador + '" />  </span> ';

        $('#spanFiltro-' + Status + '-' + Contador).html(input);
    } else if (OpcaoFiltro == "Categoria") {
        input = '<select id="Categoria-' + Status + '-' + Contador + '" name="Filtro-' + Contador + '" class="ListOptionGroup">';
        input = input + '<option value="">' + Cbo_CategoriaPai + '</option>';
        input = input + '</select>';

        $('#spanFiltro-' + Status + '-' + Contador).html(input);

        // Busca registros da subcategoria e coloca no select recem criado
        $.post(Base + "Categorias/BuscaCategorias/", {  }, function (retorno) {
            for (i = 0; i < retorno.length; i++) {
                if (retorno[i].CategoriaId == retorno[i].CategoriaPai) {
                    $("#Categoria-" + Status + "-" + Contador).append('<option value="' + retorno[i].Nome + '">' + retorno[i].Nome + '</option>');
                }
            }
        }, "json");
    } else if (OpcaoFiltro == "SubCategoria") {
        input = '<select id="SubCategoria-' + Status + '-' + Contador + '" name="Filtro-'+Contador+'" class="ListOptionGroup">';
        input = input + '<option value="">' + Cbo_SubCategoria + '</option>';
        input = input + '</select>';

        $('#spanFiltro-' + Status + '-' + Contador).html(input);

        // Busca registros da subcategoria e coloca no select recem criado
        $.post(Base + "Categorias/BuscaTodasSubCategorias/", {}, function (retorno) {
            for (i = 0; i < retorno.length; i++) {
                if (retorno[i].Value == "OptionGroup") {
                    $("#SubCategoria-" + Status + "-" + Contador).append('<option disabled=true style="font-weight:bold">' + retorno[i].Text + '</option>');
                }else{
                    $("#SubCategoria-" + Status + "-" + Contador).append('<option style="margin-left:20px;" value="' + retorno[i].Text + '">' + retorno[i].Text + '</option>');
                }
            }
        }, "json");
    } else if (OpcaoFiltro == "Empresa") {
        input = '<select id="Empresa-' + Status + '-' + Contador + '" name="Filtro-' + Contador + '" class="ListOptionGroup">';
        input = input + '<option value="">' + Cbo_Empresa + '</option>';
        input = input + '</select>';

        $('#spanFiltro-' + Status + '-' + Contador).html(input);

        // Busca registros da subcategoria e coloca no select recem criado
        $.post(Base + "Empresas/BuscaTodasEmpresas/", {}, function (retorno) {
            for (i = 0; i < retorno.length; i++) {
                $("#Empresa-" + Status + "-" + Contador).append('<option style="margin-left:20px;" value="' + retorno[i].Text + '">' + retorno[i].Text + '</option>');
            }
        }, "json");
    } else if (OpcaoFiltro == "Unidade") {
        input = '<select id="Unidade-' + Status + '-' + Contador + '" name="Filtro-' + Contador + '" class="ListOptionGroup">';
        input = input + '<option value="">'+ Cbo_Unidade + '</option>';
        input = input + '</select>';

        $('#spanFiltro-' + Status + '-' + Contador).html(input);

        // Busca registros da subcategoria e coloca no select recem criado
        $.post(Base + "Unidades/BuscaUnidadesComEmpresas/", {}, function (retorno) {
            for (i = 0; i < retorno.length; i++) {
                if (retorno[i].Value == "OptionGroup") {
                    $("#Unidade-" + Status + "-" + Contador).append('<option disabled=true style="font-weight:bold">' + retorno[i].Text + '</option>');
                } else {
                    $("#Unidade-" + Status + "-" + Contador).append('<option style="margin-left:20px;" value="' + retorno[i].Text + '">' + retorno[i].Text + '</option>');
                }
            }
        }, "json");
    } else if (OpcaoFiltro == "Prioridade") {
        input = '<input type="text" name="Filtro-' + Contador + '" /> ';
        input = input + ' <img src="' + base + 'Content/themes/Request/imagens/icon-ajuda.png" style="cursor:help" title="<span style=\'font-size:11px;\'> Opções de Prioridade: <br /> Pega os Tickets com prioridade 20 a 50:  20-50 <br /> Pega os Tickets com prioridade 20,60 e 80:  20,60,80</span>"> ';

        $('#spanFiltro-' + Status + '-' + Contador).html(input);

        $("img[title]").tipsy({ html: true });
    } else {
        input = '<input type="text" name="Filtro-' + Contador + '" /> ';
        $('#spanFiltro-' + Status + '-' + Contador).html(input);
    }
}

// Tickets/Index - Filtro: Faz a função do botao de 'Adicionar novo filtro'
function AdicionaCombo(Status) {
    Contador = Contador + 1;
    var Texto = 'Opção do filtro: ';
    Texto = Texto + '<select class="OpcaoFiltro" name="OpcaoFiltro-' + Contador + '"> ';
    Texto = Texto + '   <option onclick="AcrescentaTextoFiltro(\'\', \'' + Status + '\', \'' + Contador + '\')" value="">Selecione</option> ';
    Texto = Texto + '   <option onclick="AcrescentaTextoFiltro(\'Categoria\', \'' + Status + '\', \'' + Contador + '\')" value="Categoria">' + Lbl_CategoriaPai + '</option> ';
    Texto = Texto + '   <option onclick="AcrescentaTextoFiltro(\'SubCategoria\', \'' + Status + '\', \'' + Contador + '\')" value="SubCategoria">' + Lbl_SubCategoria + '</option> ';
    Texto = Texto + '   <option onclick="AcrescentaTextoFiltro(\'Assunto\', \'' + Status + '\', \'' + Contador + '\')" value="Assunto">' + Lbl_Assunto + '</option> ';
    Texto = Texto + '   <option onclick="AcrescentaTextoFiltro(\'Usuário\', \'' + Status + '\', \'' + Contador + '\')" value="Usuário">'+ Lbl_Usuario +'</option> ';
    Texto = Texto + '   <option onclick="AcrescentaTextoFiltro(\'Analista\', \'' + Status + '\', \'' + Contador + '\')" value="Analista">' + Lbl_Responsavel + '</option> ';
    Texto = Texto + '   <option onclick="AcrescentaTextoFiltro(\'Prioridade\', \'' + Status + '\', \'' + Contador + '\')" value="Prioridade">' + Lbl_Prioridade + '</option> ';
    Texto = Texto + '   <option onclick="AcrescentaTextoFiltro(\'Empresa\', \'' + Status + '\', \'' + Contador + '\')" value="Empresa">' + Lbl_Empresa + '</option> ';
    Texto = Texto + '   <option onclick="AcrescentaTextoFiltro(\'Unidade\', \'' + Status + '\', \'' + Contador + '\')" value="Unidade">' + Lbl_Unidade + '</option> ';
    Texto = Texto + '</select> ';

    Texto = Texto + '<span id="spanFiltro-' + Status + '-' + Contador + '"> <input type="text" name="Filtro-' + Contador + '" />  </span> ';

    Texto = Texto + '<br /> ';
    Texto = Texto + ' ';

    $('#divOpcoesCombo-' + Status).append(Texto);
}

function PegarTicket(TicketId, ResponsavelId) {
    $.post(Base + "Tickets/PegaTicket/", { 'TicketId': TicketId, 'ResponsavelId': ResponsavelId }, function (resposta) {
        if (resposta == "true") {
            //alert('Solicitação transferida com sucesso.');
            window.location.reload();
        } else if (resposta == "false") {
            alert('Ocorreu um erro.');
        }
    }, "html");
}

function ExibeAjudaMascara() {
    var msg = "Regras de máscara";
    msg = msg + " - '9' representa um número qualquer. \n";
    msg = msg + " - 'a' representa uma letra qualquer. \n";
    //msg = msg + " - '#' representa qualquer caracter. \n";
    alert(msg);
}

function Redireciona(caminho) {
    alert(caminho);
    var link = Base + caminho;
    window.location = link;
}
function RedirecionaVoltar() {
    location.href = location.href;
}

function RetiraBloqueio(LogUsuarioId, UsuarioId) {
    if (confirm('Tem certeza que deseja liberar o acesso ao sistema para este usuário antes da data prevista?')) {
        $.post(Base + "Usuarios/DesativaBloqueioTemporario/", { 'LogUsuarioId': LogUsuarioId }, function (resposta) {
            if (resposta == "true") {
                $.alerta({
                    mensagem: "Bloqueio temporário retirado.",
                    link: Base + "Usuarios/Details/" + UsuarioId
                });
                
            } else if (resposta == "false") {
                $.alerta({
                    mensagem: "Ocorreu um erro.",
                    link: Base + "Usuarios/Details/" + UsuarioId
                });
            }
        }, "html");
        
    }
}

function ConfereMascaraPreenchida(id) {
    if ($('#' + id).val() != "" && $('#' + id).val().length != $('#' + id).attr("alt").length) {
        $.alerta({
            mensagem: 'Campo com número incorreto de caracteres.'
        });
        $('#' + id).val('');
    }
}

function validaFormulario(formulario) {
    //var erro = false;
    $('.validation-summary-errors ul li').remove();

    // Confere preenchimento dos campos padroes
    var ConfGeral = true;
    $("#" + formulario + " .requerido").each(function () {
        var alt = $(this).attr('alt');
        var valor = $(this).val();
        var lang = "O preenchimento do campo " + $(this).attr('lang') + " é obrigatório.";

        if (valor == "" || valor == null) {
            ConfGeral = false;
            $(this).addClass('input-validation-error');
            $(".validation-summary-errors ul").append("<li>" + lang + "</li>");
        } else {
            $(this).removeClass('input-validation-error');
        }
    });

    // Confere preenchimento dos checkbox / radio
    var ConfMultiplasEscolhas = true;
    $('#' + formulario + ' .divMultiplaEscolhaReq').each(function () {
        var divId = $(this).attr("id");
        var lang = "O preenchimento do campo " + $(this).attr('lang') + " é obrigatório.";
        var ConfMultipla = false;

        $('#' + divId + ' .requeridoMultiplaEscolha').each(function () {

            var checkId = $(this).attr("id");
            if ($('#' + checkId).is(":checked") == true) {
                //alert('1');
                ConfMultipla = true;
            }
        });
        if (ConfMultipla == false) {
            ConfMultiplasEscolhas = false;
            //$(this).addClass('input-validation-error');
            $(".validation-summary-errors ul").append("<li>" + lang + "</li>");
        }
    });

    if (ConfGeral && ConfMultiplasEscolhas) {
        return true;
    } else {
        $(".validation-summary-errors").show();
        return false;
    }
}

function AbrirParetoMvHierarquia(Item, QtdTickets, Data, ListaDatas, Tipo, NomeFiltra, Celulas) {
    //var SQLTickets = $('#TicketsIdsSQL-' + Cont).val();
    var base = $("base").attr('href');
    var QueryString = "?Tipo=" + Tipo;
    QueryString += "&Item=" + Item;
    QueryString += "&QtdTickets=" + QtdTickets;
    QueryString += "&Data=" + Data;
    QueryString += "&ListaDatas=" + ListaDatas;
    QueryString += "&NomeFiltra=" + NomeFiltra;
    QueryString += "&Celulas=" + Celulas;

    //alert("qr: " + QueryString);

    var redirecionamento = base + "Relatorios/ExibeParetoMvHierarquia/" + QueryString;
    if (Tipo == "" || Tipo == "Diretoria") {
      window.open(redirecionamento, "Filtro", "width=" + screen.width + ", height=600, left=400, top=200, scrollbars=yes");
    } else {
        window.location = redirecionamento;
    }
}

function replaceAll(find, replace, str) {
    return str.replace(new RegExp(find, 'g'), replace);
}

/*function AbrirQuantidadesMV(FieldId) {
    //var SQLTickets = $('#TicketsIdsSQL-' + Cont).val();
    var base = $("base").attr('href');
    var redirecionamento = base + "Relatorios/ExibeNumerosMatriculasVinculadas/?FieldId=" + FieldId;

    //alert(Cont + " \n" + Nome)

    window.open(redirecionamento, "Filtro", "width=" + screen.width + ", height=600, left=400, top=200, scrollbars=yes");
    //alert(SQLTickets);
}*/

function ExibeMensagem(Mensagem, Tipo) {
    alert(Mensagem);
}

var AcaoIdPersiste = 0;
function ExecutaAcao(ChamadoId, AcaoId, ExigeJustificativa, ExigePlanejamento, PermiteTransferencia, Preenchido) {
    AcaoIdPersiste = AcaoId;
    var ExecutaAcao = true;

    if (ExigeJustificativa == 'True' && Preenchido == 'False') {
        // Pede justificativa
        $('#DivJustificativa').fadeIn();

        ExecutaAcao = false;
    }

    if (ExigePlanejamento == 'True' && Preenchido == 'False') {
        // Pede planejamento
        $('#DivPlanejamento').fadeIn();

        ExecutaAcao = false;
    }

    if (PermiteTransferencia == 'True' && Preenchido == 'False') {
        // Pede info pra transferir
        $('#DivTransferencia').fadeIn();

        ExecutaAcao = false;
    }

    if (ExecutaAcao == true) {
        $.post(Base + "Chamado/ExecutaAcao/", { 'ChamadoId': ChamadoId, 'AcaoId': AcaoId }, function (resposta) {
            if (resposta != "") {
                ExibeMensagem("Ação executada com sucesso.", "");
                window.location.reload();
            }
        }, "json");
    }
}

function ContinuaExecutaAcao(ChamadoId, Campo) {
    var CampoTxt = $('#' + Campo).val();
    if (CampoTxt == "") {
        alert('O preenchimento do campo é obrigatório.');
        return false;
    } else {
        $.post(Base + "Chamado/ExecutaAcao/", { 'ChamadoId': ChamadoId, 'AcaoId': AcaoIdPersiste, 'CampoTxt': CampoTxt, 'Campo': Campo }, function (resposta) {
            if (resposta != "") {
                ExibeMensagem("Ação executada com sucesso.", "");
                window.location.reload();
            }
        }, "json");
    }
}