using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SolicitaRH.Funcoes
{
    public class Geral {
        public static String PegaCelulasNome(int UsuarioId) {
            SolicitaRH.Models.SolicitaRHContext db = new SolicitaRH.Models.SolicitaRHContext();
            String ret = "";

            List<SolicitaRH.Models.Celula> celulas = (from c in db.Celula
                                                      join uc in db.UsuarioCelula on c.CelulaId equals uc.CelulaId
                                                      where uc.UsuarioId == UsuarioId
                                                      select c).ToList();
            foreach (var item in celulas)
            {
                if (ret == "")
                {
                    ret = item.Nome;
                }
                else
                {
                    ret += ", " + item.Nome;
                }
            }



            return ret;
        }

        public static String PegaAcoesNome(int EtapaId) {
            SolicitaRH.Models.SolicitaRHContext db = new SolicitaRH.Models.SolicitaRHContext();
            String ret = "";

            List<SolicitaRH.Models.Acao> acoes = (from a in db.Acao
                                                  join ea in db.EtapaAcao on a.AcaoId equals ea.AcaoId
                                                  where ea.EtapaId == EtapaId
                                                  select a).ToList();
            foreach (var item in acoes)
            {
                if (ret == "")
                {
                    ret = item.Nome;
                }
                else
                {
                    ret += ", " + item.Nome;
                }
            }



            return ret;
        }
        
        public static String PegaLimitePrevisto(int ChamadoId)
        {
            SolicitaRH.Models.SolicitaRHContext db = new SolicitaRH.Models.SolicitaRHContext();

            Models._Chamado mChamados =
                (from c in db.Chamado
                 join ce in db.Celula on c.CelulaId equals ce.CelulaId
                 join t in db.Topico on c.TopicoId equals t.TopicoId
                 where c.ChamadoId == ChamadoId
                 select new Models._Chamado { Chamado = c, Celula = ce, Topico = t }).FirstOrDefault();

            int TempoPrevisto = mChamados.Topico.TempoMedioPrevisto;

            DateTime DataLimitePrevista = mChamados.Chamado.DataAbertura.AddHours(TempoPrevisto);

            return TrataDados.ExibeData(DataLimitePrevista) ;
        }

        public static String CalculaPrioridade(int ChamadoId)
        {
            SolicitaRH.Models.SolicitaRHContext db = new SolicitaRH.Models.SolicitaRHContext();

            Models._Chamado mChamados =
                (from c in db.Chamado
                 join ce in db.Celula on c.CelulaId equals ce.CelulaId
                 join t in db.Topico on c.TopicoId equals t.TopicoId
                 where c.ChamadoId == ChamadoId
                 select new Models._Chamado { Chamado = c, Celula = ce, Topico = t }).FirstOrDefault();

            int PrioridadeTopico = mChamados.Topico.Prioridade;
            int PrioridadeCelula = mChamados.Celula.Prioridade;

            String PrioridadeFinal = ((PrioridadeTopico + PrioridadeCelula) / 2).ToString();

            return PrioridadeFinal;
        }

        public static int PegaPrimeiraEtapaId() { 
            SolicitaRH.Models.SolicitaRHContext db = new SolicitaRH.Models.SolicitaRHContext();

            int EtapaId = (from e in db.Etapa where e.EPrimeiraEtapa == true orderby e.EtapaId descending select e.EtapaId).FirstOrDefault();

            return EtapaId;
        }

        public static List<Models.Etapa> PegaEtapas() { 
            SolicitaRH.Models.SolicitaRHContext db = new SolicitaRH.Models.SolicitaRHContext();

            List<Models.Etapa> etapas = (from e in db.Etapa orderby e.EtapaId select e).ToList();
            return etapas;
        }
    }

    public class TrataDados
    {
        public static double CalcularPorcentagem(double n, double total)
        {
            var porcentagem = (n * 100) / total;

            // Multiplica e depois divide por 100 para dar duas casas decimais à porcentagem
            var porcentagemArredondada = Math.Round(porcentagem * 100);
            porcentagemArredondada = porcentagemArredondada / 100;

            return porcentagemArredondada;
        }        

        public static String TransformaEmString(IEnumerable<int> ListaInts)
        {
            String retorno = "";
            foreach (var itemTicketId in ListaInts)
            {
                retorno += itemTicketId + ",";
            }

            if (retorno != "")
                retorno = retorno.Substring(0, retorno.Length - 1);

            return retorno;
        }

        public static String RetiraAcentos(String valor)
        {
            String ret = valor;

            ret = ret.Replace("ã", "a").Replace("ç", "c").Replace("í", "i").Replace("á", "a");

            return ret;
        }

        public static DateTime? TrataData(String valor, bool fim)
        {
            DateTime aux = DateTime.MinValue;

            if (DateTime.TryParse(valor, out aux))
            {
                if (fim)
                {
                    aux = DateTime.Parse(valor + " 23:59:59");
                }
                else
                {
                    aux = DateTime.Parse(valor + " 00:00:00");
                }

                return aux;
            }
            else
            {
                return null;
            }
        }

        public static DateTime? TrataData(String valor)
        {
            return TrataData(valor, false);
        }

        public static int TrataInt(String valor)
        {
            int aux = 0;

            if (int.TryParse(valor, out aux))
            {
                return aux;
            }
            else
            {
                return 0;
            }
        }

        public static double TrataDouble(String valor)
        {
            double aux = 0;

            if (double.TryParse(valor, out aux))
            {
                return aux;
            }
            else
            {
                return 0;
            }
        }

        public static int TrataMatricula(String valor)
        {
            int aux = 0;

            if (int.TryParse(valor, out aux) && (valor.Trim().Length >= 3 || valor.Trim().Length <= 6))
            {
                return aux;
            }
            else
            {
                return 0;
            }
        }

        public static String TrataCpf(String valor)
        {
            if (valor == null)
            {
                valor = "";
            }
            valor = valor.Replace(".", "").Replace("-", "").Trim();
            var cpfRet = "";
            //00227945654 // 002.279.456-54
            //03023254419
            if (valor.Length == 11)
            {
                cpfRet = valor.Substring(0, 3) + "." + valor.Substring(3, 3) + "." + valor.Substring(6, 3) + "-" + valor.Substring(9, 2);
            }

            return cpfRet;
        }

        public static bool TrataBoolF(String valor)
        {
            bool ret = false;
            bool aux;
            if (bool.TryParse(valor, out aux))
            {
                ret = aux;
            }
            return ret;
        }

        public static String TrataBoolStr(bool valor)
        {
            String ret = "Não";
            if (valor)
            {
                ret = "Sim";
            }
            return ret;
        }

        public static String PegaExtensaoArquivo(String NomeArquivo)
        {
            var SNomeArquivo = NomeArquivo.Split('.');
            var Extensao = SNomeArquivo[SNomeArquivo.Length - 1];

            return Extensao;
        }

        public static String TrataCaminhoArquivo(String valor)
        {
            // feito para tratar problema do ie que traz arquivo com caminho completo
            var ultimoNome = valor;
            var ultimoNomeSplit = valor.Split('\\');

            if (ultimoNomeSplit.Count() > 0)
            {
                ultimoNome = ultimoNomeSplit[ultimoNomeSplit.Count() - 1];
            }
            valor = ultimoNome;

            // Coloca data no final do arquivo antes do pto
            String Extensao = PegaExtensaoArquivo(valor);
            valor = valor.Replace("." + Extensao, "");
            valor = valor + "-" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + "." + Extensao;

            return valor;
        }

        public static String RetiraPrimeiroChar(String valor)
        {
            String ret = valor;
            String aux = "";

            aux = valor.Substring(0, 1);
            if (aux == "+" || aux == "@" || aux == "#" || aux == "*" || aux == "?" || aux == "-")
            {
                ret = valor.Replace(aux, "");
            }

            return ret;
        }

        public static String ExibeData(DateTime valor)
        {
            //return valor.ToShortDateString();
            String ret = valor.ToShortDateString();
            DateTime aux = DateTime.Parse(valor.ToString());
            if (aux.Hour != 0)
            {
                if (aux.Minute.ToString().Length == 1)
                {
                    ret = ret + " " + aux.Hour + ":" + "0" + aux.Minute;
                }
                else
                {
                    ret = ret + " " + aux.Hour + ":" + aux.Minute;
                }
            }
            return ret;
        }

        public static String ExibeData(DateTime? valor)
        {
            if (valor == null)
            {
                return "";
            }
            else
            {
                String ret = DateTime.Parse(valor.ToString()).ToShortDateString();
                DateTime aux = DateTime.Parse(valor.ToString());
                if (aux.Hour != 0)
                {
                    String Horas = aux.Hour.ToString();
                    if (Horas.Length == 1)
                    {
                        Horas = "0" + Horas;
                    }

                    String Minutos = aux.Minute.ToString();
                    if (Minutos.Length == 1)
                    {
                        Minutos = "0" + Minutos;
                    }

                    ret = ret + " " + Horas + ":" + Minutos;
                }
                return ret;
            }

        }
    }

    public class ValidaUsuario : AuthorizeAttribute
    {
        public bool Administrador;
        //private string Regra { get; set; }

        private bool Autoriza { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            try
            {
                Autoriza = SolicitaRH.Funcoes.Sessao.UsuarioId == 0 ? false : true;

                if (Administrador == true)
                    Autoriza = SolicitaRH.Funcoes.Sessao.Administrador == true ? true : false;

                return Autoriza;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
    public static class Sessao
    {
        public static void Logout()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();

        }

        public static bool Administrador
        {
            get
            {
                return Convert.ToBoolean(HttpContext.Current.Session["Administrador"]);
            }
            set
            {
                HttpContext.Current.Session["Administrador"] = value;
            }
        }

        public static int UsuarioId
        {
            get
            {
                return Convert.ToInt32(HttpContext.Current.Session["UsuarioId"]);
            }
            set
            {
                HttpContext.Current.Session["UsuarioId"] = value;
            }
        }

        public static string Nome
        {
            get
            {
                return HttpContext.Current.Session["Nome"] as string;
            }
            set
            {
                HttpContext.Current.Session["Nome"] = value;
            }
        }
    }

    public static class Seguranca
    {
        public static string GeraMd5(string Senha)
        {

            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(Senha);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();


        }
    }

    public class Config
    {
        public class RequestInitializer : DropCreateDatabaseIfModelChanges<SolicitaRH.Models.SolicitaRHContext>
        {

            protected override void Seed(SolicitaRH.Models.SolicitaRHContext context)
            {
                //Preenche tabela de Celulas com o admin
                /*var celulas = new List<Models.Celulas>{
                    new Models.Celulas { CelulaId = 1, Nome = "Administrador", Prioridade = 100, Administrador = true }
                };
                celulas.ForEach(s => context.Celulas.Add(s));
                context.SaveChanges();

                //Preenche tabela de Usuarios com o meu usuario admin
                var usuarios = new List<Models.Usuarios>{
                    new Models.Usuarios { UsuarioId = 1, Nome = "Eduardo", Sobrenome = "Torquato", Apelido = "Eduardo Torquato", Email = "eduardo@uaise.com", Usuario = "eduardo", Senha = "25E0D20A6E85547E2EA399B2841BD69B", Matricula = "1", DataCriacao = DateTime.Now, UltimoLogin = DateTime.Now },
                    new Models.Usuarios { UsuarioId = 2, Nome = "Kadu", Sobrenome = "Aguiar", Apelido = "Kadu Aguiar", Email = "carlos.eduardo@uaise.com", Usuario = "Kadu", Senha = "202cb962ac59075b964b07152d234b70", Matricula = "2", DataCriacao = DateTime.Now, UltimoLogin = DateTime.Now }
                };
                usuarios.ForEach(s => context.Usuarios.Add(s));
                context.SaveChanges();

                // Preenche tabela UsuariosCelulas com o vinculo de usuario e celula admin
                var usuarioscelulas = new List<Models.UsuariosCelulas>{
                    new Models.UsuariosCelulas { CelulaId = 1, UsuarioId = 1, UsuariosCelulasId = 1 }
                };
                usuarioscelulas.ForEach(s => context.UsuariosCelulas.Add(s));
                context.SaveChanges();
                */
            }
        }

    }
}