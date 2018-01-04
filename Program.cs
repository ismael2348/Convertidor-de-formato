using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
/*Desarrollado por IDEAN*/
/*Autor Ismael Gonzalez  :)*/
/*Programa para formatear archivos de cualquier formato a formato UTF-8 por medio de carpetas IN y OUT. Por tiempo indeterminado.*/

namespace convertidor
{
    class Program
    {
        static void Main(string[] args)
        {
            string prefijo = "UTF-8-";
            string dirDefault, dirDefault2;
            string path1 = "";
            string path2 = "";
            Boolean result;
            string horaInicioP = DateTime.Now.ToString("hh:mm:ss");
            string fechaInicioP = DateTime.Now.ToString("dd/MM/yyyy");
            string directoryIn, directoryOut;

            //Asignacion de argumentos para la creacion de carpetas desde un archivo externo.
            directoryIn = args[0];
            directoryOut = args[1];

            SaludoInicio();
            Version();
            System.Threading.Thread.Sleep(4000);
            SaltoLinea();

            Console.WriteLine("Inicio del programa: " + fechaInicioP + "-" + horaInicioP);
            if (!Directory.Exists(args[0]) == true && !Directory.Exists(args[1]) == true)
            {
                Console.WriteLine("No existen las carpetas de procesamiento....");
                Console.WriteLine("Creando carpetas.....");
                dirDefault = Convert.ToString(Directory.CreateDirectory(args[0]));
                dirDefault2 = Convert.ToString(Directory.CreateDirectory(args[1]));
                Console.Clear();
                Console.WriteLine("Carpetas creadas con exito...!");
                System.Threading.Thread.Sleep(4000);
                Console.Clear();

                Console.WriteLine("Carpeta :" + dirDefault);
                Console.WriteLine("Carpeta :" + dirDefault2);
                System.Threading.Thread.Sleep(4000);
                Console.Clear();
            }
            else
            {

                Console.WriteLine("Ya esxisten las carpetas.");
                System.Threading.Thread.Sleep(4000);
                Console.Clear();
            }

            Info();
            Version();
            SaltoLinea();
            Console.WriteLine("Las rutas actuales son : " + directoryIn);
            Console.WriteLine("Las rutas actuales son : " + directoryOut);
            Console.WriteLine("Quiere cambiar las rutas de las carpetas? (s) = si     (n) = no");
            string Usr1 = Console.ReadLine();
            Console.Clear();
            if (Usr1 == "s" || Usr1 == "S")
            {
                Info();
                Version();
                SaltoLinea();
                System.IO.Directory.Delete(directoryIn, true);
                System.IO.Directory.Delete(directoryOut, true);
                Console.WriteLine("Borrando carpetas agregadas por archivo BAT");
                Console.WriteLine("..........");
                System.Threading.Thread.Sleep(4000);
                Info();
                Version();
                SaltoLinea();
                Console.WriteLine("Escriba la ruta donde quiere crear la carpeta de archivos para procesar.");
                Console.WriteLine("Ejemplo : C:\\ArchivosProcesar\\");
                path1 = Convert.ToString(Console.ReadLine());
                Directory.CreateDirectory(path1);
                path1 = checkPath(path1);
                Console.Clear();

                Info();
                Version();
                SaltoLinea();
                Console.WriteLine("Ruta de carpeta de archivos para procesar: " + " " + path1);
                Console.WriteLine("Escriba la ruta donde quiere crear la carpeta de archivos procesados.");
                Console.WriteLine("Ejemplo : C:\\ArchivosProcesados\\");
                path2 = Convert.ToString(Console.ReadLine());
                Directory.CreateDirectory(path2);
                path1 = checkPath(path1);
                Console.Clear();


            }
            else if (Usr1 == "n" || Usr1 == "N")
            {
                Console.WriteLine("Carpeta de entrada de datos : " + directoryIn);
                Console.WriteLine("Carpeta de salida de datos : " + directoryOut);
                path1 = directoryIn;
                path2 = directoryOut;

            }
            else
            {
                Console.WriteLine("Tecla invalida, corra de nuevo el programa.");
                System.Threading.Thread.Sleep(4000);
                Environment.Exit(0);
            }


            /*Metemos este pedazo de codigo en un blucle infinito con la variable bool r que siempre sea true.*/
            Boolean r = true;
            while (r == true)
            {
                path1 = checkPath(path1);
                path2 = checkPath(path2);
                string fechaActual = DateTime.Now.ToString("dd/MM/yyyy");
                string horaActual = DateTime.Now.ToString("hh:mm:ss");
                Console.Clear();
                Info();
                Version();
                SaltoLinea();
                Console.WriteLine("Inicio del programa: " + fechaInicioP + "-" + horaInicioP);
                Console.WriteLine("Fecha y hora actual: " + fechaActual + "-" + horaActual);
                Console.WriteLine("Carpeta de lectura: " + path1);
                Console.WriteLine("Carpeta de escritura: " + path2);
                System.Threading.Thread.Sleep(10000);
                /*Se guardan en un arreglo los archivos para despues ser procesados.*/
                string[] ubicacion = Directory.GetFiles(path1);
                for (int i = 0; i < ubicacion.Length; i++) { }
                string[] filePaths = Directory.GetFiles(path1, "*.yaml");
                Boolean cantidad = Convert.ToBoolean(filePaths.Length);
                result = Convert.ToBoolean(cantidad);

                /*Se recorre el arreglo que contiene la ruta de los archivos para su evaluacion*/
                foreach (var file in filePaths)
                {
                    String[] fileSegments = file.Split('\\');
                    string fileName = fileSegments[fileSegments.Length - 1];
                    System.Console.Write("Procesando " + " " + fileSegments[fileSegments.Length - 1] + " --> " + fileName + "\n");
                    /*Guarda y crea el archivo , se le agrega un prefijo para diferenciar el archivo ya formateado*/
                    File.WriteAllText(path2 + '\\' + prefijo + fileName, readFileAsUtf8(file));
                }
                //Obtenemos los archivos del directorio 
                string[] archivosDelete = Directory.GetFiles(path1);
                //Borramos los archivos del directorio 
                foreach (string filePath in archivosDelete)
                    File.Delete(filePath);
                ///////////////////////////////////////////////////////
                System.Console.Write("\n\nProceso de conversión finalizado.");

            }
        }

        /*Funcion para codificacion de archivo nuevo UTF-8*/
        /*Recibe como parametro la variable "fileName"*/
        public static String readFileAsUtf8(string fileName)
        {
            Encoding encoding = Encoding.Default;
            String original = String.Empty;

            using (StreamReader sr = new StreamReader(fileName, Encoding.Default))
            {
                original = sr.ReadToEnd();
                encoding = sr.CurrentEncoding;
                sr.Close();
            }

            if (encoding == Encoding.UTF8)
                return original;

            byte[] encBytes = encoding.GetBytes(original);
            byte[] utf8Bytes = Encoding.Convert(encoding, Encoding.UTF8, encBytes);
            return Encoding.UTF8.GetString(utf8Bytes);
        }

        // esta funcion primero checa que al final cierre con / o \, si no las tiene le agrega un /
        // despues reemplaza todas las \ con / y devuelve la cadena ya arreglada con puras / y tambien con una / al final
        public static String checkPath(string path)
        {
            if (path.Substring(path.Length - 1) != "/" && path.Substring(path.Length - 1) != "\\")
                path += "/";

            return path.Replace('/', '\\');
        }

        static void SaludoInicio()
        {
            Console.WriteLine("\t\t    Creado por IDEAN Innovacion de negocios");
            Console.WriteLine("\t\t\tBienvenido a Formating Convert");
            Console.WriteLine("\n\n\t\t\tEstamos preparando para usted!");

        }
        static void Info()
        {
            Console.WriteLine("\t\t    Creado por IDEAN Innovacion de negocios");
            Console.WriteLine("\t\t\tBienvenido a Formating Convert");
        }

        static void Version()
        {
            Console.WriteLine("\t\t\tVersion del programa :  3.0");
            //System.Threading.Thread.Sleep(3000);

        }
        static void SaltoLinea()
        {
            Console.WriteLine('\n');
        }

    }
}

