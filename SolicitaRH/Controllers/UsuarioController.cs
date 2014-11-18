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
    public class UsuarioController : Controller
    {
        private SolicitaRHContext db = new SolicitaRHContext();

        public ViewResult Index()
        {
            return View(db.Usuario.ToList());
        }

        public ViewResult Details(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            return View(usuario);
        }

        public ActionResult Create()
        {
            IEnumerable<_UsuarioCelula> mUsuarioCelula = (from c in db.Celula
                                                          select new _UsuarioCelula { Celula = c });
            ViewBag.mUsuarioCelula = mUsuarioCelula;

            return View();
        } 

        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                usuario.Senha = SolicitaRH.Funcoes.Seguranca.GeraMd5(usuario.Senha);

                db.Usuario.Add(usuario);
                db.SaveChanges();

                #region Cadastra celulas de usuario

                String CelulasUsu = Request.Form["CelulaIdUsuario"];

                if( CelulasUsu != null ){
                    String[] celulausu = CelulasUsu.Split(',');
                    foreach (String item in celulausu) {
                        if (item != null && item != "" && item != "0")
                        {
                            UsuarioCelula usuariocelula = new UsuarioCelula()
                            {
                                CelulaId = Funcoes.TrataDados.TrataInt(item),
                                UsuarioId = usuario.UsuarioId,
                                Atendente = false
                            };
                            db.UsuarioCelula.Add(usuariocelula);
                            db.SaveChanges();
                        }
                    }
                }

                #endregion

                #region Cadastra celulas de atendente

                String CelulasAte = Request.Form["CelulaIdAtendente"];

                if (CelulasAte != null){
                    String[] celulaate = CelulasAte.Split(',');
                    foreach (String item in celulaate)
                    {
                        if (item != null && item != "" && item != "0")
                        {
                            UsuarioCelula usuariocelula = new UsuarioCelula()
                            {
                                CelulaId = Funcoes.TrataDados.TrataInt(item),
                                UsuarioId = usuario.UsuarioId,
                                Atendente = true
                            };
                            db.UsuarioCelula.Add(usuariocelula);
                            db.SaveChanges();
                        }
                    }
                }

                #endregion

                TempData["Mensagem"] = "Registro inserido com sucesso.";
                return RedirectToAction("Index");  
            }

            IEnumerable<_UsuarioCelula> mUsuarioCelula = (from c in db.Celula
                                                          select new _UsuarioCelula { Celula = c });
            ViewBag.mUsuarioCelula = mUsuarioCelula;

            return View(usuario);
        }
        
        public ActionResult Edit(int id)
        {
            IEnumerable<_UsuarioCelula> mUsuarioCelula =
                (from c in db.Celula
                 join uc in db.UsuarioCelula on new { UsuarioId = id, c.CelulaId } equals new { uc.UsuarioId, uc.CelulaId } into tUsuarioCelula
                 from tuc in tUsuarioCelula.DefaultIfEmpty()
                 select new _UsuarioCelula { Celula = c, UsuarioCelula = tuc }).ToList();
            ViewBag.mUsuarioCelula = mUsuarioCelula;

            Usuario usuario = db.Usuario.Find(id);
            return View(usuario);
        }

        [HttpPost]
        public ActionResult Edit(Usuario usuario)
        {
            usuario.Senha = (from u in db.Usuario where u.UsuarioId == usuario.UsuarioId select u.Senha).FirstOrDefault();
            ModelState.Remove("Senha");

            if (ModelState.IsValid)
            {
                db.Entry(usuario).State = EntityState.Modified;
                db.SaveChanges();

                if (Funcoes.Sessao.Administrador)
                {
                    #region Limpa celulas ja cadastradas

                    List<UsuarioCelula> removeUsuariocelula = (from uc in db.UsuarioCelula where uc.UsuarioId == usuario.UsuarioId select uc).ToList();
                    foreach (UsuarioCelula item in removeUsuariocelula)
                    {
                        db.UsuarioCelula.Remove(item);
                        db.SaveChanges();

                    }

                    #endregion

                    #region Cadastra celulas de usuario

                    String CelulasUsu = Request.Form["CelulaIdUsuario"];

                    if (CelulasUsu != null)
                    {
                        String[] celulausu = CelulasUsu.Split(',');
                        foreach (String item in celulausu)
                        {
                            if (item != null && item != "" && item != "0")
                            {
                                UsuarioCelula usuariocelula = new UsuarioCelula()
                                {
                                    CelulaId = Funcoes.TrataDados.TrataInt(item),
                                    UsuarioId = usuario.UsuarioId,
                                    Atendente = false
                                };
                                db.UsuarioCelula.Add(usuariocelula);
                                db.SaveChanges();
                            }
                        }
                    }

                    #endregion

                    #region Cadastra celulas de atendente

                    String CelulasAte = Request.Form["CelulaIdAtendente"];

                    if (CelulasAte != null)
                    {
                        String[] celulaate = CelulasAte.Split(',');
                        foreach (String item in celulaate)
                        {
                            if (item != null && item != "" && item != "0")
                            {
                                UsuarioCelula usuariocelula = new UsuarioCelula()
                                {
                                    CelulaId = Funcoes.TrataDados.TrataInt(item),
                                    UsuarioId = usuario.UsuarioId,
                                    Atendente = true
                                };
                                db.UsuarioCelula.Add(usuariocelula);
                                db.SaveChanges();
                            }
                        }
                    }

                    #endregion
                }

                if (Funcoes.Sessao.Administrador)
                {
                    TempData["Mensagem"] = "Registro editado com sucesso.";
                    return RedirectToAction("Index");
                }
                else {
                    TempData["Mensagem"] = "Registro atualizado com sucesso.";
                    return RedirectToAction("Index", "Chamado");
                }
            }
            return View(usuario);
        }

        public ActionResult TrocaSenha(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            usuario.Senha = "";
            return View(usuario);
        }

        [HttpPost]
        public ActionResult TrocaSenha(Usuario usuario)
        {
            var SenhaAnterior = Request.Form["Senha"];
            var NovaSenha = Request.Form["NovaSenha"];
            var ConfirmaSenha = Request.Form["ConfirmaSenha"];
            String SenhaBd = (from u in db.Usuario where u.UsuarioId == usuario.UsuarioId select u.Senha).FirstOrDefault();

            ModelState.Clear();

            if (SenhaAnterior == "") {
                ModelState.AddModelError("Senha", "Senha anterior é obrigatória");
            }
            if (SolicitaRH.Funcoes.Seguranca.GeraMd5(SenhaAnterior) != SenhaBd) {
                ModelState.AddModelError("Senha", "Senha anterior incorreta");
            }
            if (NovaSenha == "") {
                ModelState.AddModelError("NovaSenha", "Nova senha é obrigatória");
            }
            if (ConfirmaSenha == "") {
                ModelState.AddModelError("ConfirmaSenha", "Confirma senha é obrigatória");
            }
            if (NovaSenha != ConfirmaSenha) {
                ModelState.AddModelError("NovaSenha", "A nova senha deve ser a mesma da confirma senha");
                ModelState.AddModelError("ConfirmaSenha", "A nova senha deve ser a mesma da confirma senha");
            }

            if (ModelState.IsValid) {
                String NovaSenhaMd5 = SolicitaRH.Funcoes.Seguranca.GeraMd5(NovaSenha);

                Usuario edtUsuario = db.Usuario.Find(usuario.UsuarioId);
                edtUsuario.Senha = NovaSenhaMd5;
                db.Entry(edtUsuario).State = EntityState.Modified;
                db.SaveChanges();

                TempData["Mensagem"] = "Senha atualizada com sucesso.";
                return RedirectToAction("Index", "Chamado");
            }
            return View(usuario);
        }

        public ActionResult Delete(int id)
        {
            Usuario usuario = db.Usuario.Find(id);
            return View(usuario);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Usuario usuario = db.Usuario.Find(id);
            db.Usuario.Remove(usuario);
            db.SaveChanges();
            TempData["Mensagem"] = "Registro excluído com sucesso.";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}