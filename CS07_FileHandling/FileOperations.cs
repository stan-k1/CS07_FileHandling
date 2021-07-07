using System;
using System.IO;

namespace CS07_FileHandling
{
    public static class FileOperations
    {
        public static void CopyFile(string inPath, string outPath, bool overwrite)
        {
            if (!File.Exists(inPath))
            {
                Console.Error.WriteLine($"Could not locate file at {inPath}");
                return;
            }

            Console.WriteLine($"File at {inPath} located!");

            try
            {
                File.Copy(inPath, outPath, overwrite);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine($"File was successfully copied to {outPath}!");
        }

        public static void MoveFile(string inPath, string outPath)
        {
            if (!File.Exists(inPath) || File.Exists(outPath))
            {
                Console.Error.WriteLine("The move operation cannot be performed.");
                return;
            }

            try
            {
                File.Move(inPath, outPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine($"File at \"{inPath}\" successfully moved to: {outPath}!");
        }

        public static void DeleteFile(string path)
        {
            if (!File.Exists(path)) 
                Console.Error.WriteLine("The file to delete could not be located!");

            try
            {
                File.Delete(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.WriteLine($"File at \"{path}\" deleted.");
        }
    }
}
