using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfupdater
{
    /// <summary>
    /// kfUpdater es un instalador y actualizador de paquetes sencillo
    /// desarrollado por mi fer.d.minguela@gmail.com (kamifer)
    /// </summary>
    class Program
    {
        #region "Constantes"
        public static string DIR_CACHE = "";
        public static string ACTUAL_DIR = "";
        public static string DIR_TMP = "";
        #endregion

        /// <summary>
        /// Metodo de inicio de la aplicacion
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //cogemos y revisamos para ver cual es el primer comando
            inicio();

            if (args.Count() > 0)
            {
                string accion = args.First();

                switch (accion)
                {
                    case "--install":   //instalar paquetes
                        instalarPaquetes(args);
                        break;
                    case "--upgrade":   //actualizar las apps instaladas
                        break;
                    case "--update":    //actualizar la lista de paquetes y si hay actualizaciones
                        break;
                    case "--help":      //Muestra la ayuda
                        break;
                    default:
                        Console.WriteLine("No se reconoce el argumento introducido");
                        break;
                }
            }
            else Console.WriteLine("No se reconoce el argumento introducido");

            Console.Read();
            
        }

        private static void instalarPaquetes(string[] args)
        {
            Console.WriteLine("Procediendo a la instalacion");
            if (args.Count() > 1)
            {
                //poner lista de paquetes a instalar
                List<string> nombres = new List<string>();
                for (int i = 1; i < args.Count(); i++)
                {
                    nombres.Add(args[i].ToString());
                }

                new instalar(nombres);
            }
        }

        static void inicio()
        {
            ACTUAL_DIR = Directory.GetCurrentDirectory();
            DIR_CACHE = ACTUAL_DIR + @"\cache";

            //comprobar si existe el directorio de cache
            if (Directory.Exists(DIR_CACHE) == false)
            {
                Directory.CreateDirectory(DIR_CACHE);
            }

            DIR_TMP = ACTUAL_DIR + @"\tmp";
            if (Directory.Exists(DIR_TMP) == false)
            {
                Directory.CreateDirectory(DIR_TMP);
            }


        }
    }
}
