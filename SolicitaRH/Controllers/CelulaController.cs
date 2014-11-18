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
    [SolicitaRH.Funcoes.ValidaUsuario]
    public class CelulaController : Controller
    {
        private SolicitaRHContext db = new SolicitaRHContext();

        public ViewResult Index()
        {
            return View(db.Celula.ToList());
        }

        public ViewResult Details(int id)
        {
            Celula celula = db.Celula.Find(id);
            return View(celula);
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(Celula celula)
        {
            if (ModelState.IsValid)
            {
                db.Celula.Add(celula);
                db.SaveChanges();
                TempData["Mensagem"] = "Registro inserido com sucesso.";
                return RedirectToAction("Index");  
            }

            return View(celula);
        }
        
        public ActionResult Edit(int id)
        {
            Celula celula = db.Celula.Find(id);
            return View(celula);
        }

        [HttpPost]
        public ActionResult Edit(Celula celula)
        {
            if (ModelState.IsValid)
            {
                db.Entry(celula).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Registro editado com sucesso.";
                return RedirectToAction("Index");
            }
            return View(celula);
        }

        public ActionResult Delete(int id)
        {
            Celula celula = db.Celula.Find(id);
            return View(celula);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Celula celula = db.Celula.Find(id);
            db.Celula.Remove(celula);
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