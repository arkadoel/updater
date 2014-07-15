using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace kfupdater
{
    public class GestorDescargas
    {
        private string packageListUrl = "";
        private Repositorio Repo;
        public Boolean Procesando = true;
        private ConsoleColor original = Console.ForegroundColor;
        private int linea;



        public string DescargarFuentes(Repositorio repo)
        {
            packageListUrl = repo.URL + "/packageList.xml";
            Repo = repo;
        

            WebClient w = new WebClient();
            string destinyFile= Program.DIR_CACHE + @"\" + Repo.RepoName.Replace(" ", "") + "packageList.xml";
            Console.Write("Descargando de: " + Repo.RepoName + ".......");
            linea = 0;

            w.DownloadFileAsync(new Uri(packageListUrl, UriKind.Absolute), destinyFile );

            w.DownloadProgressChanged += w_DownloadProgressChanged;
            w.DownloadFileCompleted += w_DownloadFileCompleted;

            return destinyFile;
        }

        void w_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Console.ForegroundColor = original;
            Console.WriteLine("\r\nDescarga terminada");
            Procesando = false;
        }

        void w_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            
            EscribirProgresoConsola(e.ProgressPercentage, " Descargando fuentes: " + Repo.RepoName);
            System.Threading.Thread.Sleep(100);
            Procesando = true;
        }        

        public void EscribirProgresoConsola(int porcentaje, string mensaje)
        {
            linea++;
            if (linea >100)
            {
                linea = 0;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\r\n[" + porcentaje.ToString() + "%]");
                Console.ForegroundColor = original;
                Console.Write(mensaje);
                Console.SetCursorPosition(0, Console.CursorTop - 1);
            }
        }

        public void DescargarPaquete(string urlServidor, Paquete pkg)
        {
            WebClient w = new WebClient();
            string destinyFile = Program.DIR_TMP + @"\" + pkg.FileName;
            string fileToDownload = urlServidor + "/" + pkg.FileName;

            w.DownloadFileAsync(new Uri(fileToDownload, UriKind.Absolute), destinyFile);
            w.DownloadProgressChanged += (sender, e) =>
            {
                EscribirProgresoConsola(e.ProgressPercentage, " Descargando paquete: " + pkg.PackageName);
                System.Threading.Thread.Sleep(100);
                Procesando = true;
            };
            w.DownloadFileCompleted += (sender, e) =>
            {
                Console.ForegroundColor = original;
                Console.WriteLine("\r\nDescarga terminada");
                Procesando = false;
            };
        }

        public class ConsoleSpinner
        {
            int counter;
            public ConsoleSpinner()
            {
                counter = 0;
            }
            public void Turn()
            {
                counter++;
                if (counter > 1000) counter = 0;
                
                switch (counter % 4)
                {
                    case 0: Console.Write("/"); break;
                    case 1: Console.Write("-"); break;
                    case 2: Console.Write("\\"); break;
                    case 3: Console.Write("|"); break;
                }
                 
                Console.SetCursorPosition(Console.CursorLeft -1, Console.CursorTop);
            }
        }
        
    }


}
