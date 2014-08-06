using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kfupdater
{
    public class GestorZip
    {
        /// <summary>
        /// Comprime un archivo y lo deja en esa misma carpeta en la que esta
        /// </summary>
        /// <param name="fi">Fileinfo del archivo a comprimir</param>
        /// <returns>String con el nombre del archivo comprimido</returns>
        public static string CompressFile(FileInfo fi)
        {
            string filename = "";
            using (FileStream inFile = fi.OpenRead())
            {
                if ((File.GetAttributes(fi.FullName)
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

        /// <summary>
        /// Descomprime el archivo especificado en el directorio correspondiente
        /// </summary>
        /// <param name="fi"></param>
        public static string decompressFile(FileInfo fi)
        {
            string name = fi.Name.ToString().Replace(".pkg", "");

            FileInfo zipfile = fi.CopyTo(name + ".zip" ,true);
            string dirDestino = fi.Directory.FullName + @"\" + name;
            if(Directory.Exists(dirDestino)==true)
            {
                Directory.Delete(dirDestino,true);
                
            }
            Directory.CreateDirectory(dirDestino);

            fi.Delete();
            Ionic.Zip.ZipFile zf = new Ionic.Zip.ZipFile(zipfile.FullName);
            zf.ExtractAll(dirDestino,Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
            Console.WriteLine("extraido aunque no lo parezca");
            return dirDestino;
            /*
            using (FileStream infile = fi.OpenRead())
            {
                string curfile = fi.FullName;
                string originalName = curfile.Remove(curfile.Length - fi.Extension.Length);

                using (FileStream outfile = File.Create(originalName))
                {

                    using (GZipStream decompress = new GZipStream(infile, CompressionMode.Decompress))
                    {
                        decompress.CopyTo(outfile);
                    }
                }
            }
                 */
        }


        internal static string decompressFile(string pkgDescargado)
        {
            FileInfo f = new FileInfo(pkgDescargado);
            return decompressFile(f);
        }
    }
}
