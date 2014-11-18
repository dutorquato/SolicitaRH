using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SolicitaRH.Models
{
    public class Etapa
    {
        [Key]
        public int EtapaId { get; set; }

        [Display(Name = "É a primeira etapa")]
        public bool EPrimeiraEtapa { get; set; }

        [Required(ErrorMessage = "Preencha o Nome!")]
        [Display(Name = "Nome")]
        public String Nome { get; set; }

        //public int Identificador { get; set; }

        [Display(Name = "Permite interação")]
        public bool PermiteInteracao { get; set; }

        //[Display(Name = "Destaca as ações (Destacas as ações desta etapa na tela de detalhes da solicitação):")]
        //public bool DestacaAcoes { get; set; }

        [Display(Name = "Desativa: ")]
        [Required]
        [System.ComponentModel.DefaultValue(false)]
        public bool Desativado { get; set; }

        /*
         * PermiteAceiteAnalista
         * PermiteCancelar
         * PermiteEncerrar
         * PermiteResposta
         * PermiteEnvioAnexo
         * PermiteEncaminhamento
         * PermiteMudarProcedencia
        */
    }

    public class EtapaAcao
    {
        [Key]
        public int EtapaAcaoId { get; set; }

        //public int EtapasId { get; set; }
        public int EtapaId { get; set; }
        public int AcaoId { get; set; }
    }

    public class Acao 
    { 
        [Key]
        public int AcaoId { get; set; }

        [Required(ErrorMessage = "Preencha o nome!")]
        public String Nome { get; set; }

        [Required(ErrorMessage = "Preencha a etapa seguinte!")]
        [Display(Name = "Etapa Seguinte")]
        public int EtapaSeguinteId { get; set; }

        [Display(Name = "Texto de confirmação opcional (Ex.: Tem certeza que deseja cancelar esta solicitação)")]
        public String TextoConfirmacao { get; set; }

        [Display(Name = "Atendente visualiza")]
        public bool AtendenteVisualiza { get; set; }

        [Display(Name="Marca o Atendente (Quando o atendente realizar esta ação ele será marcado como o responsável pelo chamado)")]
        public bool MarcaAtendente { get; set; }

        [Display(Name = "Permite transferencia (Quando o atendente realizar esta ação ele poderá transferir o chamado para outro atendente)")]
        public bool PermiteTransferencia { get; set; }

        [Display(Name = "Usuário visualiza")]
        public bool UsuarioVisualiza { get; set; }

        // O Que a ação faz alem de mudar de etapa
        [Display(Name = "Exige planejamento")]
        public bool ExigePlanejamento { get; set; }
        //public bool ExigePrePlanejamento { get; set; }
        [Display(Name = "Exige justificativa")]
        public bool ExigeJustificativa { get; set; }
        //public bool MarcaAnalista { get; set; }
    }

    public class _Acao {
        public Acao Acao { get; set; }
        public Etapa EtapaSeguinte { get; set; }
    }

    public class _EtapaAcao {
        public Etapa Etapa { get; set; }
        public EtapaAcao EtapaAcao { get; set; }
        public Acao Acao { get; set; }
    }
}