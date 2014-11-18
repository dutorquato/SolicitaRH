using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolicitaRH.Models
{
    public class Usuario 
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage="Preencha o Nome do Usuário!")]
        public string Nome { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Preencha o Login do Usuário!")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Preencha o campo Senha!")]
        public string Senha { get; set; }

        public DateTime? UltimoLogin { get; set; }

        public bool Administrador { get; set; }
    }
}
