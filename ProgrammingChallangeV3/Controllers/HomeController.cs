using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProgrammingChallangeV3.Models;

namespace ProgrammingChallangeV3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IHostingEnvironment hostingEnv;

        public HomeController(ILogger<HomeController> logger, IHostingEnvironment hostingEnv)
        {
            _logger = logger;
            this.hostingEnv = hostingEnv;
        }



        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<IEnumerable> Proceso()
        {

            var x = DateTime.Now;

            General general = new General();
            string ruta = Path.Combine(hostingEnv.WebRootPath, "upload\\");


            // Obtiene la lista de archivos por descargar de las últimas 5 horas
            List<String> listaNombreArchivo;
            string strError;

            Boolean bResultado = general.obtenerNombreArchivos(out listaNombreArchivo, out strError);
            if (!bResultado)
            {

                System.Console.WriteLine(strError);
                return null;
            }

            // Recorre la lista para Grabar el archivo en un carpeta local, descomprime y lee
            List<ALL_HOURS> listALLHours = new List<ALL_HOURS>();

            foreach (string cadena in listaNombreArchivo)
            {
                await Task.Run(() =>
                {
                    List<ALL_HOURS> listArchivo = new List<ALL_HOURS>();

                    strError = "";
                    Boolean bResultadoProcesar = general.procesoArchivo(cadena, out listArchivo, ruta, out strError);
                    if (!bResultadoProcesar)
                    {
                        System.Console.WriteLine(strError);
                    }

                    listALLHours.AddRange(listArchivo);
                });

            }


            System.Console.WriteLine("Inicio el proceso " + x);
            System.Console.WriteLine("Termino el proceso " + DateTime.Now);

            //Obtiene el top 100 

            var result =
            from o in listALLHours
            group o by (o.PAGE_TITEL, o.DOMAIN_CODE) into g
            orderby g.Sum(o => o.CNT) descending
            select new
            {
                PAGE_TITEL = g.Key.PAGE_TITEL,
                DOMAIN_CODE = g.Key.DOMAIN_CODE,
                CNT = g.Sum(o => o.CNT)
            }
             ;


            Console.WriteLine(DateTime.Now);
            result = result.Take(100);


            return result;


        }


    }
}
