    

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
    public class CategoriaController : Controller
    {
        private SolicitaRHContext db = new SolicitaRHContext();

        public ViewResult Index()
        {
            return View(db.Categoria.ToList());
        }

        public ViewResult Details(int id)
        {
            Categoria categoria = db.Categoria.Find(id);
            return View(categoria);
        }

        public ActionResult Create()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Categoria.Add(categoria);
                db.SaveChanges();
                TempData["Mensagem"] = "Registro inserido com sucesso.";
                return RedirectToAction("Index");  
            }

            return View(categoria);
        }
        
        public ActionResult Edit(int id)
        {
            Categoria categoria = db.Categoria.Find(id);
            return View(categoria);
        }

        [HttpPost]
        public ActionResult Edit(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoria).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Registro editado com sucesso.";
                return RedirectToAction("Index");
            }
            return View(categoria);
        }

        public ActionResult Delete(int id)
        {
            Categoria categoria = db.Categoria.Find(id);
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Categoria categoria = db.Categoria.Find(id);
            db.Categoria.Remove(categoria);
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