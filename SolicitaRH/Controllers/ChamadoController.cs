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
    public class ChamadoController : Controller
    {
        private SolicitaRHContext db = new SolicitaRHContext();

        public ViewResult Index(int EtapaId = 0, String PesquisaTopo = "")
        {
            int UsuarioId = Funcoes.Sessao.UsuarioId;
            List<int> CelulasAtendenteId = (from uc in db.UsuarioCelula
                                            where uc.UsuarioId == UsuarioId
                                            && uc.Atendente == true 
                                            select uc.CelulaId).ToList();

            #region Pega novos chamados (que sou atendente da celula mas que o chamado nao tenha atendentes)

            IEnumerable<_Chamado> mNovosChamados =
                (from c in db.Chamado
                 join ce in db.Celula on c.CelulaId equals ce.CelulaId
                 join ca in db.Categoria on c.CategoriaId equals ca.CategoriaId
                 join t in db.Topico on c.TopicoId equals t.TopicoId
                 join e in db.Etapa on c.EtapaId equals e.EtapaId
                 join u in db.Usuario on c.UsuarioId equals u.UsuarioId into tUsuario
                 join a in db.Usuario on c.AtendenteId equals a.UsuarioId into tAtendente
                 from tu in tUsuario.DefaultIfEmpty()
                 from ta in tAtendente.DefaultIfEmpty()
                 where CelulasAtendenteId.Contains(c.CelulaId) && (c.AtendenteId == 0 || c.AtendenteId == null )
                 && e.Nome != "Fechado"
                 select new _Chamado { Chamado = c, Celula = ce, Categoria = ca, Topico = t, Etapa = e, Usuario = tu, Atendente = ta });

            if (EtapaId != 0) {
                mNovosChamados = mNovosChamados.Where(c => c.Etapa.EtapaId == EtapaId);
            }
            if (PesquisaTopo != "") {
                mNovosChamados = mNovosChamados.Where(c => 
                    c.Chamado.Titulo.StartsWith(PesquisaTopo)
                    || c.Categoria.Nome.StartsWith(PesquisaTopo)
                    || c.Topico.Nome.StartsWith(PesquisaTopo)
                    || c.Celula.Nome.StartsWith(PesquisaTopo)
                    || c.Usuario.Nome.StartsWith(PesquisaTopo)
                    );
            }

            ViewBag.mNovosChamados = mNovosChamados;

            #endregion

            #region Pega meus chamados (que sou atendente ou o usuario que abriu)

            IEnumerable<_Chamado> mMeusChamados =
                (from c in db.Chamado
                 join ce in db.Celula on c.CelulaId equals ce.CelulaId
                 join ca in db.Categoria on c.CategoriaId equals ca.CategoriaId
                 join t in db.Topico on c.TopicoId equals t.TopicoId
                 join e in db.Etapa on c.EtapaId equals e.EtapaId
                 join u in db.Usuario on c.UsuarioId equals u.UsuarioId into tUsuario
                 join a in db.Usuario on c.AtendenteId equals a.UsuarioId into tAtendente
                 from tu in tUsuario.DefaultIfEmpty()
                 from ta in tAtendente.DefaultIfEmpty()
                 where c.AtendenteId == UsuarioId || c.UsuarioId == UsuarioId 
                 select new _Chamado { Chamado = c, Celula = ce, Categoria = ca, Topico = t, Etapa = e, Usuario = tu, Atendente = ta });

            if (PesquisaTopo != "")
            {
                mMeusChamados = mMeusChamados.Where(c =>
                    c.Chamado.Titulo.StartsWith(PesquisaTopo)
                    || c.Categoria.Nome.StartsWith(PesquisaTopo)
                    || c.Topico.Nome.StartsWith(PesquisaTopo)
                    || c.Celula.Nome.StartsWith(PesquisaTopo)
                    || c.Usuario.Nome.StartsWith(PesquisaTopo)
                    );
            }
            if (EtapaId != 0) {
                mMeusChamados = mMeusChamados.Where(c => c.Etapa.EtapaId == EtapaId);
            }

            ViewBag.mMeusChamados = mMeusChamados;

            #endregion

            return View();
        }

        public ViewResult Details(int id)
        {
            _Chamado mChamados =
                (from c in db.Chamado
                 join ce in db.Celula on c.CelulaId equals ce.CelulaId
                 join ca in db.Categoria on c.CategoriaId equals ca.CategoriaId
                 join t in db.Topico on c.TopicoId equals t.TopicoId
                 join e in db.Etapa on c.EtapaId equals e.EtapaId
                 join u in db.Usuario on c.UsuarioId equals u.UsuarioId into tUsuario
                 join a in db.Usuario on c.AtendenteId equals a.UsuarioId into tAtendente
                 from tu in tUsuario.DefaultIfEmpty()
                 from ta in tAtendente.DefaultIfEmpty()
                 where c.ChamadoId == id
                 select new _Chamado { Chamado = c, Celula = ce, Categoria = ca, Topico = t, Etapa = e, Usuario = tu, Atendente = ta }).FirstOrDefault();

            bool Atendente = (from uc in db.UsuarioCelula
                              where uc.UsuarioId == Funcoes.Sessao.UsuarioId
                              && uc.CelulaId == mChamados.Celula.CelulaId
                              select uc.Atendente).FirstOrDefault();
            ViewBag.Atendente = Atendente;

            #region Pega Interações para ViewBag.mChamadoInteracao

            List<_ChamadoInteracao> mChamadoInteracao =
                (from ci in db.ChamadoInteracao
                 join u in db.Usuario on ci.UsuarioId equals u.UsuarioId into tUsuario
                 where ci.ChamadoId == id
                 orderby ci.ChamadoId descending
                 from tu in tUsuario.DefaultIfEmpty()
                 select new _ChamadoInteracao { ChamadoInteracao = ci, Usuario = tu }).ToList();
            ViewBag.mChamadoInteracao = mChamadoInteracao;

            #endregion

            #region Pega lista de ações para ViewBag.mAcoes

            List<Acao> mAcoes = (from a in db.Acao
                                 join ea in db.EtapaAcao on a.AcaoId equals ea.AcaoId
                                 where ea.EtapaId == mChamados.Chamado.EtapaId
                                 select a).ToList();
            ViewBag.mAcoes = mAcoes;

            #endregion

            #region Pega lista de celulas para ViewBag.mCelulas

            List<Celula> mCelulas = (from c in db.Celula 
                                     orderby c.Nome 
                                     select c).ToList();
            ViewBag.mCelulas = mCelulas;

            #endregion

            return View(mChamados);
        }

        public ActionResult Create()
        {
            int UsuarioId = Funcoes.Sessao.UsuarioId;
            List<int> CelulasId = (from uc in db.UsuarioCelula
                                   where uc.UsuarioId == UsuarioId
                                   select uc.CelulaId).ToList();


            ViewBag.CategoriaId = (from c in db.Categoria
                                   join t in db.Topico on c.CategoriaId equals t.CategoriaId 
                                   join ce in db.Celula on t.CelulaId equals ce.CelulaId
                                   where CelulasId.Contains(ce.CelulaId)
                                   orderby c.Nome
                                   select c).Distinct().ToList();


            return View();
        } 

        [HttpPost]
        public ActionResult Create(Chamado chamado)
        {
            chamado.UsuarioId = Funcoes.Sessao.UsuarioId;
            chamado.DataAbertura = DateTime.Now;

            if (ModelState.IsValid)
            {
                chamado.EtapaId = Funcoes.Geral.PegaPrimeiraEtapaId();

                db.Chamado.Add(chamado);
                db.SaveChanges();

                /* Cadastra Interação */
                String Interacao = Request.Form["Descricao"];

                ChamadoInteracao nChamadoInteracao = new ChamadoInteracao() { 
                    ChamadoId = chamado.ChamadoId,
                    UsuarioId = chamado.UsuarioId,
                    Data = DateTime.Now,
                    Interacao = Interacao
                };
                db.ChamadoInteracao.Add(nChamadoInteracao);
                db.SaveChanges();
                /* Cadastra Interação */

                TempData["Mensagem"] = "Chamado inserido com sucesso.";
                return RedirectToAction("Index");  
            }

            return View(chamado);
        }

        [HttpPost]
        public ActionResult SalvaInteracao() {
            int ChamadoId = Funcoes.TrataDados.TrataInt(Request.Form["ChamadoId"]);
            String Interacao = Request.Form["Interacao"];

            ChamadoInteracao nChamadoInteracao = new ChamadoInteracao()
            {
                ChamadoId = ChamadoId,
                Data = DateTime.Now,
                UsuarioId = Funcoes.Sessao.UsuarioId,
                Interacao = Interacao
            };
            db.ChamadoInteracao.Add(nChamadoInteracao);
            db.SaveChanges();

            TempData["Mensagem"] = "Interação inserida com sucesso.";
            return RedirectToAction("Details", "Chamado", new { id = ChamadoId }); 
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public JsonResult PegaTopicos(int CategoriaId)
        {
            String strTopicos = "<option></option>";

            List<Topico> mTopicos = (from t in db.Topico where t.CategoriaId == CategoriaId orderby t.Nome select t).ToList();

            foreach (var item in mTopicos)
            {
                strTopicos += "<option value=" + item.TopicoId + ">" + item.Nome + "</option>";
            }

            return Json(strTopicos);
        }

        public JsonResult PegaCelula(int TopicoId)
        {
            int CelulaId = (from t in db.Topico where t.TopicoId == TopicoId select t.CelulaId).FirstOrDefault();
            Celula mCelula = (from c in db.Celula where c.CelulaId == CelulaId select c).FirstOrDefault();

            return Json(mCelula);
        }

        public JsonResult PegaAtendentesCombo(int CelulaId) { 
            String ret = "<option></option>";

            List<Usuario> Atendentes = (from u in db.Usuario
                                        join uc in db.UsuarioCelula on u.UsuarioId equals uc.UsuarioId
                                        where uc.CelulaId == CelulaId
                                        && uc.Atendente == true
                                        select u).ToList();

            foreach (Usuario item in Atendentes) {
                ret += "<option value='" + item.UsuarioId + "'>" + item.Nome + "</option>";
            }

            return Json(ret);
        }

        public JsonResult ExecutaAcao(int ChamadoId, int AcaoId, String CampoTxt = "", String Campo = "")
        {
            Acao mAcao = db.Acao.Find(AcaoId);
            Chamado mChamado = db.Chamado.Find(ChamadoId);

            mChamado.EtapaId = mAcao.EtapaSeguinteId;
            if (mAcao.MarcaAtendente == true) {
                mChamado.AtendenteId = Funcoes.Sessao.UsuarioId;
            }

            if (CampoTxt != "") {
                if (Campo.IndexOf("Justificativa") > 0) {
                    ChamadoInteracao nchamadointeracao = new ChamadoInteracao()
                    {
                        Data = DateTime.Now,
                        ChamadoId = ChamadoId,
                        UsuarioId = Funcoes.Sessao.UsuarioId,
                        Interacao = "Ação realizada: " + mAcao.Nome + "; Justificativa: " + CampoTxt
                    };
                    db.ChamadoInteracao.Add(nchamadointeracao);
                } else if (Campo.IndexOf("Planejamento") > 0) {
                    ChamadoInteracao nchamadointeracao = new ChamadoInteracao()
                    {
                        Data = DateTime.Now,
                        ChamadoId = ChamadoId,
                        UsuarioId = Funcoes.Sessao.UsuarioId,
                        Interacao = "Ação realizada: " + mAcao.Nome + "; Planejamento de atividade: " + CampoTxt
                    };
                    db.ChamadoInteracao.Add(nchamadointeracao);

                } else if (Campo.IndexOf("Transferencia") > 0) {
                    mChamado.AtendenteId = Funcoes.TrataDados.TrataInt(CampoTxt);
                }
            }

            db.Entry(mChamado).State = EntityState.Modified;
            db.SaveChanges();

            return Json("sucesso");  
        }
    }
}