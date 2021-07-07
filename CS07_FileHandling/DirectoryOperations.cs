using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace CS07_FileHandling
{
    public static class DirectoryOperations
    {
        public static void NewDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Console.Error.WriteLine($"Directory at {path} already exists!");
                return;
            }

            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }

            Console.WriteLine($"Directory at {path} created!");
        }

        public static void MoveDirectory(string inPath, string outPath)
        {
            if (!Directory.Exists(inPath))
            {
                Console.Error.WriteLine($"Directory at {inPath} does not exist!");
                return;
            }

            try
            {
                Directory.Move(inPath, outPath);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
            }
        }

        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.Error.WriteLine($"Directory at {path} does not exist!");
                return;
            }
            Directory.Delete(path);
        }

        public static void EnumerateAndPrint(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.Error.WriteLine($"Directory at {path} does not exist!");
                return;
            }

            Console.WriteLine($"---Contents of directory {path}:---");

            IEnumerable<string> files = Directory.EnumerateFiles(path);

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            Console.WriteLine("----------------------------------");
        }

        public static void EnumerateAndPrint(string path, string extension)
        {
            if (!Directory.Exists(path))
            {
                Console.Error.WriteLine($"Directory at {path} does not exist!");
                return;
            }

            if (extension[0] == '.') extension = extension.Substring(1);
            string searchPattern = "*." + extension;

            Console.WriteLine($"---Contents of directory {path} with extension {extension}:---");

            IEnumerable<string> files = Directory.EnumerateFiles(path, searchPattern);

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            Console.WriteLine("----------------------------------");
        }

        public static void EnumerateAndPrintSubdirectories(string path)
        {
            if (!Directory.Exists(path))
            {
                Console.Error.WriteLine($"Directory at {path} does not exist!");
                return;
            }

            Console.WriteLine($"---Subdirectories of directory {path} :---");

            IEnumerable<string> folders = Directory.EnumerateDirectories(path);

            foreach (var folder in folders)
            {
                Console.WriteLine(folder);
            }

            Console.WriteLine("----------------------------------");
        }
    }
}
