using DotNet.Highcharts;
using DotNet.Highcharts.Enums;
using DotNet.Highcharts.Helpers;
using DotNet.Highcharts.Options;
using SolicitaRH.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SolicitaRH.Controllers
{
    public class GraficoController : Controller
    {
        private SolicitaRH.Models.SolicitaRHContext db = new SolicitaRHContext();

        public ActionResult Index(String inicial = "", String final = "")
        {
            // Definição do periodo
            if (inicial == "" && final == "") {
                inicial = DateTime.Now.AddHours(-48).ToShortDateString();
                final = DateTime.Now.ToShortDateString();
            }

            DateTime inicio = DateTime.Parse(inicial + " 00:00:00");
            DateTime fim = DateTime.Parse(final + " 23:59:59");

            var model = (from e in db.Etapa
                         select e).ToList();

            object[] objNome = new object[5];
            double[] objQtd = new double[5];

            int i = 0;
            var total = 0;
            foreach (var item in model) {
                objNome[i] = item.Nome;
                objQtd[i] = (from c in db.Chamado where c.EtapaId == item.EtapaId select c).Count();

                //objQtd[i] = calcularPorcentagem(objQtd[i], total);

                total += Convert.ToInt32(objQtd[i]);
                i++;
            }

            for (int f = 0; f < model.Count; f++)
            {
                if (model.Count >= f)
                    objQtd[f] = Funcoes.TrataDados.CalcularPorcentagem(objQtd[f], total);
            }

            Highcharts chart = new Highcharts("chart4")
             .InitChart(new Chart { PlotShadow = false })
             .SetTitle(new Title { Text = "Etapas: De " + inicio.ToShortDateString() + " até " + fim.ToShortDateString() })
             .SetTooltip(new Tooltip { Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ Math.round(this.percentage * 100) / 100 +' %'; }" })
             .SetPlotOptions(new PlotOptions
             {
                 Pie = new PlotOptionsPie
                 {
                     AllowPointSelect = true,
                     Cursor = Cursors.Pointer,
                     DataLabels = new PlotOptionsPieDataLabels
                     {
                         Color = ColorTranslator.FromHtml("#000000"),
                         ConnectorColor = ColorTranslator.FromHtml("#000000"),
                         Formatter = "function() { return '<b>'+ this.point.name +'</b>: '+ Math.round(this.percentage * 100) / 100 +' %'; }"
                     }
                 }
             }).SetSeries(new Series
             {
                 Type = ChartTypes.Pie,
                 Name = "Categorias",
                 Data = new Data(new object[]
                                               {    
                                                   new object[]{objNome[0],objQtd[0]},
                                                   new object[]{objNome[1],objQtd[1]},
                                                   new object[]{objNome[2],objQtd[2]},
                                                   new object[]{objNome[3],objQtd[3]},
                                                   new object[]{objNome[4],objQtd[4]}
                                                  
                                               })

             });

            return View(chart);
        }

    }
}
