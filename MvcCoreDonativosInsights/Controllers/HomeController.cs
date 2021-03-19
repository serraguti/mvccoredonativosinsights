using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Mvc;

namespace MvcCoreAzure.Controllers
{
    public class HomeController : Controller
    {
        private TelemetryClient telemetryClient;

        public HomeController(TelemetryClient telemetryclient)
        {
            this.telemetryClient = telemetryclient;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(String nombre, int cantidad)
        {
            ViewData["MENSAJE"] = "Su donativo de "
              + cantidad
              + "€ ha sido almacenado.  " +
              "Muchas gracias, Sr/Sra " + nombre;
            this.telemetryClient.TrackEvent("DonativosRequested");
            MetricTelemetry metricdonativos = new MetricTelemetry();
            metricdonativos.Name = "Donativos";
            metricdonativos.Sum = cantidad;
            this.telemetryClient.TrackMetric(metricdonativos);
            String mensaje = nombre + " " + cantidad + "€.";
            SeverityLevel level;
            if (cantidad < 5)
            {
                level = SeverityLevel.Warning;
            }else if (cantidad < 2)
            {
                level = SeverityLevel.Critical;
            }else if (cantidad < 1)
            {
                level = SeverityLevel.Error;
            }
            else
            {
                level = SeverityLevel.Information;
            }
            TraceTelemetry trace = new TraceTelemetry(mensaje, level);
            this.telemetryClient.TrackTrace(trace);
            return View();
        }
    }
}
