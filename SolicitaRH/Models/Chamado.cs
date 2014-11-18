using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolicitaRH.Models
{
    public class Chamado
    {
        [Key]
        public int ChamadoId { get; set; }

        [Required(ErrorMessage = "Preencha a Célula!")]
        [Display(Name = "Célula")]
        public int CelulaId { get; set; }

        [Required(ErrorMessage = "Preencha a Categoria!")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Preencha o Tópico!")]
        [Display(Name = "Tópico")]
        public int TopicoId { get; set; }

        [Required]
        [Display(Name = "Usuário")]
        public int UsuarioId { get; set; }

        [Display(Name = "Atendente")]
        public int? AtendenteId { get; set; }

        [Display(Name = "Etapa")]
        public int EtapaId { get; set; }

        [Required(ErrorMessage = "Preencha o Título!")]
        [Display(Name = "Título")]
        public String Titulo { get; set; }

        [Required]
        [Display(Name = "Data de abertura")]
        public DateTime DataAbertura { get; set; }
    }

    public class ChamadoInteracao
    {
        [Key]
        public int ChamadoInteracaoId { get; set; }
        public int ChamadoId { get; set; }
        public DateTime Data { get; set; }
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Preencha a Descrição!")]
        [Display(Name = "Descrição")]
        public string Interacao { get; set; }
    }

    public class _ChamadoInteracao {
        public ChamadoInteracao ChamadoInteracao { get; set; }
        public Usuario Usuario { get; set; }
    }

    public class ChamadoAnexo {
        [Key]
        public int ChamadoAnexoId { get; set; }

        public int ChamadoId { get; set; }

        public String AnexoNome { get; set; }

        public String AnexoCaminho { get; set; }
    }

    public class _Chamado {
        public Chamado Chamado { get; set; }
        public Celula Celula { get; set; }
        public Categoria Categoria { get; set; }
        public Topico Topico { get; set; }
        public Etapa Etapa { get; set; }
        public Usuario Usuario { get; set; }
        public Usuario Atendente { get; set; }
    }
}

