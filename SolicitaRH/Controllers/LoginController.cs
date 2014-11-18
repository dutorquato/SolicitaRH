using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SolicitaRH.Models;

namespace SolicitaRH.Controllers
{
    public class LoginController : Controller
    {
        private SolicitaRHContext db = new SolicitaRHContext();

        public ActionResult Index()
        {
            if (TempData["Mensagem"] != null && TempData["Mensagem"].ToString() != "")
            {
                ViewBag.Mensagem = TempData["Mensagem"];// = @RequestGlobalization.Request.Bnt_Procurar;
            }

            if (SolicitaRH.Funcoes.Sessao.UsuarioId > 0) {
                return RedirectToAction("Index", "Chamado");
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(Usuario usuario)
        {
            if (usuario.Login == null || usuario.Login == "" || usuario.Senha ==  null || usuario.Senha == "")
            {
                ViewBag.Mensagem = "O preenchimento do login e senha é obrigatório!";
                return View();
            }

            // Pega a senha e a criptografa
            string Senha = "";
            if (usuario.Senha == null) {
                Senha = "";
            } else {
                Senha = SolicitaRH.Funcoes.Seguranca.GeraMd5(usuario.Senha);
            }

            // Acha o registro do usuario pelo login e senha criptografada
            Usuario ExisteUsuario = (from u in db.Usuario 
                                     where u.Login == usuario.Login 
                                     && u.Senha == Senha 
                                     select u).FirstOrDefault();

            if (ExisteUsuario != null) {
                // Cria Sessoes
                SolicitaRH.Funcoes.Sessao.UsuarioId = ExisteUsuario.UsuarioId;
                SolicitaRH.Funcoes.Sessao.Administrador = ExisteUsuario.Administrador;
                SolicitaRH.Funcoes.Sessao.Nome = ExisteUsuario.Nome;

                // Pega as celulas id do usuario
                List<int> Celulas = (from uc in db.UsuarioCelula
                                     join c in db.Celula on uc.CelulaId equals c.CelulaId
                                     where uc.UsuarioId == ExisteUsuario.UsuarioId
                                     select c.CelulaId).ToList();

                ViewBag.Administrador = SolicitaRH.Funcoes.Sessao.Administrador;

                return RedirectToAction("Index", "Chamado");
            } else {
                ViewBag.Mensagem = "Usuário e/ou senha incorretos";
            }

            // Mensagem de erro
            if (usuario.Senha != null && usuario.Nome != null) {
                ViewBag.Mensagem = "Usuário e/ou senha incorretos";
            }

            return View();
        }

        public ActionResult Logout()
        {
            SolicitaRH.Funcoes.Sessao.Logout();
            return RedirectToAction("Index", "Login");
        }
    }
}
