using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfupdater
{
    /// <summary>
    /// Clase encargada de la instalacion de los paquetes
    /// </summary>
    public class instalar
    {
        /// <summary>
        /// Instala la lista de paquetes pasandole los nombres de los mismos
        /// como parametro
        /// </summary>
        /// <param name="nombres"></param>
        public instalar(List<string> nombres)
        {
            foreach (string nombrePaquete in nombres)
            {
                Console.WriteLine("Instalando " + nombrePaquete);

                /* Buscar y descargar paquete del repositorio */
                buscarPaquete(nombrePaquete);
                /* Realizar instalacion */
                 
            }

            Console.WriteLine("Se finalizo la operacion de instalacion");
        }

        /// <summary>
        /// Busca dentro de los repositorios el paquete a instalar y 
        /// devuelve la url para descargarlo
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public string buscarPaquete(string nombrePkg)
        {
            /* Descargar ficheros con WebClient
             * va a sources.xml y coge un repositorio(url)
             * descarga de nuevo el archivo de la url llamado packageList.xml
             * busca en packageList.xml de ese repositorio un paquete que se llame
             * como el que se quiere instalar.
             * Si existe coge la url de descarga indicada en el archivo packageList.xml
             * descarga el paquete en \cache
             * Se descomprime el paquete en \tmp\<paquete>\...
             * Se procede a instalar
             * */
            GestionarXML ges = new GestionarXML();
            List<Repositorio> repos = ges.ObtenerRepositorios();
            List<string> archivosPackageList = new List<string>();
            Boolean encontrado = false; //dice si el paquete fue encontrado o no


            for (int i = 0; i < repos.Count() && encontrado !=true; i++)
            {
                var repo = repos[i];
                GestorDescargas gestorDes = new GestorDescargas();
                //Console.SetBufferSize(80, 150);
                archivosPackageList.Add(gestorDes.DescargarFuentes(repo));
                while (gestorDes.Procesando != false)
                {
                    //esperar
                }

                nombrePkg = nombrePkg.ToUpper().Replace(" ", "");
                

               for (int j = 0; j < archivosPackageList.Count() && encontrado != true; j++)
                {
                    string archivo = archivosPackageList[j];
                    GestionarXML g = new GestionarXML();
                    List<Paquete> paquetes = g.ListarPaquetes(archivo);

                    var resultado = from u in paquetes
                                    where u.PackageName.ToUpper().Replace(" ", "").Contains(nombrePkg)
                                    select u;

                    if (resultado != null && resultado.Count()>0)
                    {
                        //proceder a descargar
                        Console.Write("Paquete encontrado se procede a descargarlo");

                        string pkgDescargado= gestorDes.DescargarPaquete(repo.URL, resultado.First());
                        while (gestorDes.Procesando != false)
                        {
                            //esperar
                        }
                        encontrado = true;

                        //descomprimir el archivo
                        
                        string directorio = GestorZip.decompressFile(pkgDescargado);

                        List<string> procesos= GestionarXML.getInstallProcess(directorio + @"\info.xml");
                        foreach (var proceso in procesos)
                        {
                            System.Diagnostics.Process pexe = new System.Diagnostics.Process();
                            pexe.StartInfo.WorkingDirectory = directorio + @"\installer";
                            pexe.StartInfo.FileName = (directorio + @"\" + procesos.First());
                            pexe.Start();
                        }

                    }
                }
            }

            
            
            return "";
        }


    }
}
