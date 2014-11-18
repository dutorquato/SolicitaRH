using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolicitaRH.Models
{
    public class SolicitaRHContext : DbContext  
    {
        //public SolicitaRhContext() : base("name=SolicitaRHConnection") {}

        public DbSet<Chamado> Chamado { get; set; }
        public DbSet<ChamadoInteracao> ChamadoInteracao { get; set; }

        public DbSet<Celula> Celula { get; set; }
        public DbSet<UsuarioCelula> UsuarioCelula { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Topico> Topico { get; set; }

        public DbSet<Etapa> Etapa { get; set; }
        public DbSet<Acao> Acao { get; set; }
        public DbSet<EtapaAcao> EtapaAcao { get; set; }
    }
}