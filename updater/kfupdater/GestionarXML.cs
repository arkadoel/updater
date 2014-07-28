using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace kfupdater
{
    /// <summary>
    /// Clase encargada de la gestion de los archivos XML de
    /// forma mas o menos sencilla
    /// </summary>
    public class GestionarXML
    {
        public const string SOURCE_XML = "sources.xml";

        /// <summary>
        /// Carga el archivo source.xml para ser usado
        /// 
        /// </summary>
        /// <returns></returns>
        public XDocument CargarSources()
        {
            XDocument doc = XDocument.Load(SOURCE_XML);
            return doc;
        }

        /// <summary>
        /// Obtiene la lista de repositorios
        /// </summary>
        /// <returns></returns>
        public List<Repositorio> ObtenerRepositorios()
        {
            XDocument doc = CargarSources();
            List<Repositorio> lista = new List<Repositorio>();

            var repos = from u in doc.Elements("conf").Elements("repos").Elements()
                        select u;

            if (repos != null)
            {
                Repositorio rtmp = null;

                foreach (var rxml in repos)
                {
                    rtmp = new Repositorio();
                    rtmp.RepoName = rxml.Attribute("name").Value.ToString();
                    rtmp.URL = rxml.Attribute("url").Value.ToString();
                    lista.Add(rtmp);
                }
            }

            LiberarMemoria(doc);
            return lista;
        }

        /// <summary>
        /// Cierra y libera la memoria usada por el gestor de XML
        /// </summary>
        /// <param name="doc"></param>
        private static void LiberarMemoria(XDocument doc)
        {
            doc = null;
            GC.Collect();
        }

        /// <summary>
        /// Lee packageList.xml y obtiene la lista de paquetes
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public List<Paquete> ListarPaquetes(string path)
        {
            List<Paquete> paquetes = new List<Paquete>();
            XDocument doc = XDocument.Load(path);

            var elementos = from u in doc.Elements("packages").Elements()
                            select u;
            if (elementos != null)
            {
                foreach (var pinfo in elementos)
                {
                    Paquete paquete = new Paquete();
                    paquete.FileName = pinfo.Attribute("fileName").Value.ToString();
                    paquete.PackageName = pinfo.Attribute("name").Value.ToString();
                    paquetes.Add(paquete);
                }
            }

            LiberarMemoria(doc);
            return paquetes;
        }

    }
}
