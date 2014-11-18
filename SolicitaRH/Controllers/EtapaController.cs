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
    public class EtapaController : Controller
    {
        private SolicitaRHContext db = new SolicitaRHContext();

        public ViewResult Index()
        {
            return View(db.Etapa.ToList());
        }

        public ViewResult Details(int id)
        {
            Etapa etapa = db.Etapa.Find(id);
            return View(etapa);
        }

        public ActionResult Create()
        {
            IEnumerable<_EtapaAcao> mEtapaAcao = (from a in db.Acao
                                                  orderby a.Nome
                                                  select new _EtapaAcao { Acao = a });
            ViewBag.mEtapaAcao = mEtapaAcao;

            return View();
        } 

        [HttpPost]
        public ActionResult Create(Etapa etapa)
        {
            if (ModelState.IsValid)
            {
                db.Etapa.Add(etapa);
                db.SaveChanges();

                #region Cadastra ações da etapa

                String Acoes = Request.Form["AcaoId"];

                if (Acoes != null) {
                    String[] acoes = Acoes.Split(',');
                    foreach (String item in acoes) {
                        if (item != null && item != "" && item != "0") {
                            EtapaAcao etapaacao = new EtapaAcao() {
                                AcaoId = Funcoes.TrataDados.TrataInt(item),
                                EtapaId = etapa.EtapaId
                            };
                            db.EtapaAcao.Add(etapaacao);
                            db.SaveChanges();
                        }
                    }
                }

                #endregion

                TempData["Mensagem"] = "Registro inserido com sucesso.";
                return RedirectToAction("Index");  
            }

            return View(etapa);
        }
        
        public ActionResult Edit(int id)
        {
            Etapa etapa = db.Etapa.Find(id);

            IEnumerable<_EtapaAcao> mEtapaAcao = (from a in db.Acao
                                                  join ea in db.EtapaAcao on new { EtapaId = id, a.AcaoId } equals new { ea.EtapaId, ea.AcaoId } into tEtapaAcao
                                                  orderby a.Nome
                                                  from tea in tEtapaAcao.DefaultIfEmpty()
                                                  select new _EtapaAcao { Acao = a, EtapaAcao = tea });
            ViewBag.mEtapaAcao = mEtapaAcao;

            return View(etapa);
        }

        [HttpPost]
        public ActionResult Edit(Etapa etapa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(etapa).State = EntityState.Modified;
                db.SaveChanges();

                #region Limpa acoes ja cadastradas

                List<EtapaAcao> removeEtapaAcao = (from ea in db.EtapaAcao where ea.EtapaId == etapa.EtapaId select ea).ToList();
                foreach (EtapaAcao item in removeEtapaAcao)
                {
                    db.EtapaAcao.Remove(item);
                    db.SaveChanges();

                }

                #endregion

                #region Cadastra acoes

                String Acoes = Request.Form["AcaoId"];

                if (Acoes != null)
                {
                    String[] acoes = Acoes.Split(',');
                    foreach (String item in acoes)
                    {
                        if (item != null && item != "" && item != "0")
                        {
                            EtapaAcao etapaacao = new EtapaAcao()
                            {
                                AcaoId = Funcoes.TrataDados.TrataInt(item),
                                EtapaId = etapa.EtapaId
                            };
                            db.EtapaAcao.Add(etapaacao);
                            db.SaveChanges();
                        }
                    }
                }

                #endregion

                TempData["Mensagem"] = "Registro editado com sucesso.";
                return RedirectToAction("Index");
            }
            return View(etapa);
        }

        public ActionResult Delete(int id)
        {
            Etapa etapa = db.Etapa.Find(id);
            return View(etapa);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Etapa etapa = db.Etapa.Find(id);
            db.Etapa.Remove(etapa);
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