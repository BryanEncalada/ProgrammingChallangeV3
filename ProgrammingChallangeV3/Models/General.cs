using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;

namespace ProgrammingChallangeV3.Models
{
    public class General
    {
        public Boolean procesoArchivo(string cadenaArchivo, out List<ALL_HOURS> listGeneral, string ruta, out string strError)
        {


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();



            // Declarar variabless
            General general = new General();
            listGeneral = new List<ALL_HOURS>();

            string[] archivoSplit = cadenaArchivo.Split(";");
            // Descargar el link en bytes
            WebClient wc = new WebClient();


            var response = wc.DownloadData(archivoSplit[0]);
            Archivo archivoByte = new Archivo();
            archivoByte.archivo = response;
            archivoByte.ruta = ruta;
            archivoByte.nombre = archivoSplit[1];

            // Grabar el arreglo de byes en un carpeta

            Boolean resGrabar = Grabar(archivoByte.archivo, archivoByte.nombre, archivoByte.ruta, out strError);
            if (!resGrabar)
            {
                System.Console.WriteLine(strError);
                return false;
            }

            // Descomprime el archivo
            var archivoZip = new FileInfo(Path.Combine(ruta, archivoSplit[1]));
            Boolean resDecompress = Decompress(archivoZip, out strError);
            if (!resDecompress)
            {
                System.Console.WriteLine(strError);
                return false;
            }

            //  Lee el archivo según la ruta  y devuelve lista de clase ALL_HOURS 
            var archivoDescompress = archivoZip.ToString().Substring(0, archivoZip.ToString().Length - 3);

            List<ALL_HOURS> listRead;

            Boolean bResultado = ReadFile(archivoDescompress, out listRead, out strError);
            if (!bResultado)
            {
                System.Console.WriteLine(strError);
                return false;
            }
            else
            {
                listGeneral.AddRange(listRead);
            }

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            System.Console.WriteLine(String.Format("Tiempo final -> Time: {0}", ts));

            return true;
        }

        public Boolean ReadFile(String archivo, out List<ALL_HOURS> listGeneral, out string strError)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            listGeneral = new List<ALL_HOURS>();
            strError = "";

            //Valida que el nombre no sea vacío
            if (string.IsNullOrWhiteSpace(archivo))
            {
                strError = "El nombre del archivo no debe estar en blanco";
                return false;
            }

            //Valida si la carpeta existe
            FileInfo archivo2 = new FileInfo(archivo);
            if (archivo2.Exists == false)
            {
                strError = "El  archivo no existe";
                return false;
            }

            string directoryPath = archivo;
            using (FileStream fs = System.IO.File.Open(directoryPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                string linea;

                while ((linea = sr.ReadLine()) != null)
                {

                    ALL_HOURS allhoours = new ALL_HOURS();
                    //System.Console.WriteLine(linea);
                    string[] x = linea.Split(' ');

                    allhoours.DOMAIN_CODE = x[0];
                    allhoours.PAGE_TITEL = x[1];
                    allhoours.CNT = Convert.ToInt32(x[2]);

                    listGeneral.Add(allhoours);

                }
            }

            stopWatch.Stop();

            if (listGeneral == null)
            {
                strError = "El  archivo no existe";
                return false;
            }

            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            System.Console.WriteLine(String.Format("RunTime -> ReadFile: Archivo -> {0} Time: {1}", archivo2.FullName, elapsedTime));

            return true;
        }

        public Boolean Decompress(FileInfo fileToDecompress, out string strError)
        {

            strError = "";

            if (fileToDecompress == null)
            {
                strError = "El archivo no puede ser vacio";
                return false;
            }

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length);

                using (FileStream decompressedFileStream = System.IO.File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);

                    }
                }
            }

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            System.Console.WriteLine(String.Format("RunTime -> Decompress: cadenaArchivo -> {0} Time: {1}", fileToDecompress.Name, ts));


            return true;
        }

        public Boolean Grabar(byte[] data, string nombreArchivo, string rutaFile, out string strError)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            strError = "";

            if (string.IsNullOrWhiteSpace(rutaFile))
            {
                strError = "La ruta del archivo no debe ser vacio";
                return false;
            }

            if (string.IsNullOrWhiteSpace(nombreArchivo))
            {
                strError = "El nombre del archivo no debe ser vacio";
                return false;
            }

            if (data == null)
            {
                strError = "La data es vacia";
                return false;
            }

            String ruta = @rutaFile;
            try
            {
                FileInfo dir = new FileInfo(ruta);
                dir.Directory.Create();

                if (dir.Exists == false)
                {
                    Directory.CreateDirectory(ruta);
                }

                if (data.Length > 0)
                {
                    using (var fileStream = new FileStream(Path.Combine(ruta, nombreArchivo), FileMode.Create, FileAccess.Write))
                    {
                        fileStream.Write(data, 0, data.Length);
                    }
                }

            }
            catch (Exception e)
            {

            }
            finally
            {
                stopWatch.Stop();
            }

            TimeSpan ts = stopWatch.Elapsed;


            System.Console.WriteLine(String.Format("RunTime -> Grabar: nombreArchivo -> {0} Time: {1}", nombreArchivo, ts));

            return true;
        }

        public Boolean obtenerNombreArchivos(out List<String> listaArchivo, out string strError)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            strError = "";
            listaArchivo = new List<string>();

            DateTime fechaActual = DateTime.Now;
            for (DateTime date = DateTime.Now.AddHours(-5); date < fechaActual; date = date.AddHours(1))
            {

                var ano = date.Year;
                var mes = date.Month.ToString("D2");
                var dia = date.Day.ToString("D2");
                var hora = date.Hour.ToString("D2");
                var cadena = string.Format("https://dumps.wikimedia.org/other/pageviews/{0}/{1}/pageviews-{2}-{3}0000.gz", ano, ano + "-" + mes, ano + mes + dia, hora);
                var nombre = "pageviews-" + ano + mes + dia + "-" + hora + "0000.gz";
                listaArchivo.Add(cadena + ";" + nombre);

            }

            if (listaArchivo == null)
            {
                strError = "No existen arhivos";
                stopWatch.Stop();
                return false;
            }

            if (listaArchivo.Count < 5)
            {
                strError = "No obtuvo las 5 últimas horas";
                stopWatch.Stop();
                return false;
            }


            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            System.Console.WriteLine(String.Format("RunTime -> obtenerNombreArchivos: Time: {0}", ts));

            return true;
        }

    }
}
