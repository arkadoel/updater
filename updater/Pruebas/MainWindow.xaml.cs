using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pruebas
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            progreso.Value = 0;
            btnDescargar.Click += btnDescargar_Click;
        }

        #region "Descarga de archivos"
        void btnDescargar_Click(object sender, RoutedEventArgs e)
        {
            string url = txtUrl.Text;
            WebClient w = new WebClient();
            
            w.DownloadFileAsync(new Uri(url, UriKind.Absolute), "jor.txt");

            w.DownloadProgressChanged += w_DownloadProgressChanged;
            w.DownloadFileCompleted += w_DownloadFileCompleted;

        }

        void w_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.Title = e.ProgressPercentage.ToString();
            progreso.Value = e.ProgressPercentage;

        }

        void w_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Descarga finalizada");

        }

        #endregion

        #region "Compresion y descompresion de archivos"

        private void btnComprimir_Click(object sender, RoutedEventArgs e)
        {
            //comprimir el archivo jor.txt
            FileInfo jor = new FileInfo("wpsoffice.exe");
            string destino = compressFile(jor);
            jor.MoveTo("antiguo" + jor.Name);

            MessageBox.Show("comprimido");
            decompressFile(new FileInfo(destino));
            MessageBox.Show("descomprimido");
        }

        public string compressFile(FileInfo fi)
        {
            string filename = "";
            using (FileStream inFile = fi.OpenRead())
            {
                if((File.GetAttributes(fi.FullName) 
                    & FileAttributes.Hidden)
                    != FileAttributes.Hidden & fi.Extension != ".gz")
                {
                    using (FileStream outfile = File.Create(fi.FullName + ".gz"))
                    {
                        using (GZipStream compress = new GZipStream(outfile, CompressionMode.Compress))
                        {
                            inFile.CopyTo(compress);
                        }
                        filename = fi.FullName + ".gz";
                    }
                }
            }
            return filename;
        }

        public void decompressFile(FileInfo fi)
        {
            using (FileStream infile = fi.OpenRead())
            {
                string curfile = fi.FullName;
                string originalName = curfile.Remove(curfile.Length - fi.Extension.Length);
                
                using (FileStream outfile = File.Create( originalName  ))
                {
                    
                    using (GZipStream decompress = new GZipStream(infile, CompressionMode.Decompress))
                    {
                        decompress.CopyTo(outfile);
                    }
                }
            }
        }

        #endregion

        public void EscribirProgresoConsola(int porcentaje)
        {
            Console.WriteLine("hola");
            var spin = new ConsoleSpinner();
            Console.Write("Working....");
            while (true)
            {
                spin.Turn();
            }
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
                if (counter > 100)
                {
                    counter = 0;
                    Console.Write("\r\n");
                    Console.Write("Paquete siguiente........");
                }
                /*
                switch (counter % 4)
                {
                    case 0: Console.Write("/"); break;
                    case 1: Console.Write("-"); break;
                    case 2: Console.Write("\\"); break;
                    case 3: Console.Write("|"); break;
                }
                 * */

                ConsoleColor original = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.SetCursorPosition(Console.BufferWidth - 5, Console.CursorTop);
                Console.Write("    ");
                Console.SetCursorPosition(Console.BufferWidth - 5, Console.CursorTop);
                Console.Write(counter.ToString() + "%");
                Console.ForegroundColor = original;
                System.Threading.Thread.Sleep(100);

                //Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }
        }

        private void btnConsola_Click(object sender, RoutedEventArgs e)
        {
            EscribirProgresoConsola(32);
        }
    }
}
