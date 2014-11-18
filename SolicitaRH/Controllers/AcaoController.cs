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
    public class AcaoController : Controller
    {
        private SolicitaRHContext db = new SolicitaRHContext();

        public ViewResult Index()
        {
            IEnumerable<_Acao> mAcao = (from a in db.Acao
                                        join es in db.Etapa on a.EtapaSeguinteId equals es.EtapaId
                                        orderby a.AcaoId
                                        select new _Acao { Acao = a, EtapaSeguinte = es });

            return View(mAcao);
        }

        public ViewResult Details(int id)
        {
            _Acao mAcao = (from a in db.Acao
                           join es in db.Etapa on a.EtapaSeguinteId equals es.EtapaId
                           where a.AcaoId == id
                           select new _Acao { Acao = a, EtapaSeguinte = es }).FirstOrDefault();

            return View(mAcao);
        }

        public ActionResult Create()
        {
            ViewBag.EtapaSeguinteId = db.Etapa.OrderBy(x => x.Nome).ToList();


            return View();
        } 

        [HttpPost]
        public ActionResult Create(Acao acao)
        {
            if (ModelState.IsValid)
            {
                db.Acao.Add(acao);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.EtapaSeguinteId = db.Etapa.OrderBy(x => x.Nome).ToList();

            return View(acao);
        }
        
        public ActionResult Edit(int id)
        {
            ViewBag.EtapaSeguinteId = db.Etapa.OrderBy(x => x.Nome).ToList();

            Acao acao = db.Acao.Find(id);
            return View(acao);
        }

        [HttpPost]
        public ActionResult Edit(Acao acao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EtapaSeguinteId = db.Etapa.OrderBy(x => x.Nome).ToList();
            return View(acao);
        }

        public ActionResult Delete(int id)
        {
            _Acao mAcao = (from a in db.Acao
                           join es in db.Etapa on a.EtapaSeguinteId equals es.EtapaId
                           where a.AcaoId == id
                           select new _Acao { Acao = a, EtapaSeguinte = es }).FirstOrDefault();

            return View(mAcao);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Acao acao = db.Acao.Find(id);
            db.Acao.Remove(acao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}