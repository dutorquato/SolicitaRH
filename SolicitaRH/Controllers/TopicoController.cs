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
    public class TopicoController : Controller
    {
        private SolicitaRHContext db = new SolicitaRHContext();

        public ViewResult Index()
        {
            IEnumerable<_Topico> mTopicos = (from t in db.Topico
                                             join c in db.Categoria on t.CategoriaId equals c.CategoriaId
                                             select new _Topico { Topico = t, Categoria = c });

            return View(mTopicos);
        }

        public ViewResult Details(int id)
        {
            _Topico mTopico = (from t in db.Topico
                               join c in db.Categoria on t.CategoriaId equals c.CategoriaId
                               join ce in db.Celula on t.CelulaId equals ce.CelulaId
                               where t.TopicoId == id
                               select new _Topico { Topico = t, Categoria = c, Celula = ce }).FirstOrDefault() ;
            return View(mTopico);
        }

        public ActionResult Create()
        {
            ViewBag.CategoriaId = db.Categoria.OrderBy(x => x.Nome).ToList();

            ViewBag.CelulaId = db.Celula.OrderBy(x => x.Nome).ToList();

            return View();
        } 

        [HttpPost]
        public ActionResult Create(Topico topico)
        {
            if (ModelState.IsValid)
            {
                db.Topico.Add(topico);
                db.SaveChanges();
                TempData["Mensagem"] = "Registro inserido com sucesso.";
                return RedirectToAction("Index");  
            }

            ViewBag.CategoriaId = db.Categoria.OrderBy(x => x.Nome).ToList();
            ViewBag.CelulaId = db.Celula.OrderBy(x => x.Nome).ToList();
            return View(topico);
        }
        
        public ActionResult Edit(int id)
        {
            ViewBag.CategoriaId = db.Categoria.OrderBy(x => x.Nome).ToList();
            ViewBag.CelulaId = db.Celula.OrderBy(x => x.Nome).ToList();

            Topico topico = db.Topico.Find(id);
            return View(topico);
        }

        [HttpPost]
        public ActionResult Edit(Topico topico)
        {
            if (ModelState.IsValid)
            {
                db.Entry(topico).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Mensagem"] = "Registro editado com sucesso.";
                return RedirectToAction("Index");
            }

            ViewBag.CategoriaId = db.Categoria.OrderBy(x => x.Nome).ToList();
            ViewBag.CelulaId = db.Celula.OrderBy(x => x.Nome).ToList();
            return View(topico);
        }

        public ActionResult Delete(int id)
        {
            _Topico mTopico = (from t in db.Topico
                               join c in db.Categoria on t.CategoriaId equals c.CategoriaId
                               join ce in db.Celula on t.CelulaId equals ce.CelulaId
                               where t.TopicoId == id
                               select new _Topico { Topico = t, Categoria = c, Celula = ce }).FirstOrDefault();
            return View(mTopico);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Topico topico = db.Topico.Find(id);
            db.Topico.Remove(topico);
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