using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SolicitaRH.Models
{
    public class Topico
    {
        [Key]
        public int TopicoId { get; set; }

        [Required(ErrorMessage = "Preencha a Categoria!")]
        [Display(Name = "Categoria")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "Preencha o Nome!")]
        [Display(Name = "Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Preencha a Célula responsável!")]
        [Display(Name = "Célula responsável")]
        public int CelulaId { get; set; }

        [Required(ErrorMessage = "Preencha a Prioridade!")]
        [Display(Name = "Prioridade")]
        [Range(0, 100, ErrorMessage = "Prioridade deve ser um valor número de 0 a 100.")]
        public int Prioridade { get; set; }

        [Required(ErrorMessage = "Preencha o Tempo médio previsto!")]
        [Display(Name = "Tempo médio previsto (Em horas)")]
        public int TempoMedioPrevisto { get; set; }
    }

    public class _Topico {
        public Topico Topico { get; set; }
        public Categoria Categoria { get; set; }
        public Celula Celula { get; set; }
    }
}