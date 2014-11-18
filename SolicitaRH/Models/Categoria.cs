using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolicitaRH.Models
{
    public class Categoria {
        [Key]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage="O preenchimento do Nome da Categoria é obrigatório!")]
        [Display(Name="Nome da Categoria")]
        public string Nome { get; set; }
    }
}
