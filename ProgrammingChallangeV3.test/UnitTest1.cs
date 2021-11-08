using NUnit.Framework;
using ProgrammingChallangeV3.Models;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class Tests
    {

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestAllHours()
        {
            var test = new ALL_HOURS();
            Assert.Pass();
        }

        [Test]
        public void TestNombreArchivoVacio()
        {
            var test = new General();

            List<ALL_HOURS> allhours;
            String strRuta = @"C:\Users\p035800\source\repos\ProgrammingChallengeV1\wwwroot\upload\pageviews-20211106-000000 - copia";
            String strError = "";
            test.ReadFile(strRuta, out allhours, out strError);

            Assert.IsNotNull(allhours);

        }

        [Test]
        public void TestNombreArchivoIncorrecto()
        {
            var test = new General();

            List<ALL_HOURS> allhours;
            String strRuta = @"C:\Users\p035800\source\repos\ProgrammingChallengeV1\wwwroot\upload\pageviews-20211106-000000 - copia";
            String strError = "";
            test.ReadFile(strRuta, out allhours, out strError);

            Assert.IsNotNull(allhours);

        }

        public void TestNombreArchivoDescomprirExite()
        {
            var test = new General();

            List<ALL_HOURS> allhours;
            String strRuta = @"C:\Users\p035800\source\repos\ProgrammingChallengeV1\wwwroot\upload\pageviews-20211106-000000 - copia";
            String strError = "";
            test.ReadFile(strRuta, out allhours, out strError);

            Assert.IsNotNull(allhours);

        }

        [Test]
        public void TestObtenerNombreArchivosError()
        {
            var test = new General();

            List<String> listaNombreArchivo;
            string strError;

            test.obtenerNombreArchivos(out listaNombreArchivo, out strError);
            Assert.AreEqual(strError, "");


        }

        [Test]
        public void TestObtenerNombreArchivosCantidad()
        {
            var test = new General();

            List<String> listaNombreArchivo;
            string strError;

            test.obtenerNombreArchivos(out listaNombreArchivo, out strError);

            Assert.AreEqual(strError, "");
            Assert.IsNotNull(listaNombreArchivo);

        }
    }
}