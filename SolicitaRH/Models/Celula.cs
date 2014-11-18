using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SolicitaRH.Models
{
    public class Celula
    {
        [Key]
        public int CelulaId { get; set; }

        [Required(ErrorMessage = "O preenchimento da célula é obrigatório!")]
        [Display(Name="Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O preenchimento da prioridade é obrigatório!")]
        [Display(Name="Prioridade")]
        [Range(0,100, ErrorMessage="Prioridade deve ser um valor número de 0 a 100.")]
        public int Prioridade { get; set; }
    }

    public class UsuarioCelula
    {
        [Key]
        public int UsuarioCelulaId { get; set; }
        public int UsuarioId { get; set; }
        public int CelulaId { get; set; }
        public bool Atendente { get; set; }
    }

    public class _UsuarioCelula
    {
        public Usuario Usuario { get; set; }
        public Celula Celula { get; set; }
        public UsuarioCelula UsuarioCelula { get; set; }
    }
}