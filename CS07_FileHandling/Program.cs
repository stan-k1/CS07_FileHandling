using System;
using System.IO;

namespace CS07_FileHandling
{
    class Program
    {
        static void Main(string[] args)
        {
            //Basic File and Directory Operations

            Console.WriteLine("Hello World!");
            string textFile = "C:\\CS07\\Example.txt";
            FileOperations.CopyFile(textFile, "C:\\CS07\\Copy.txt", true);
            FileOperations.MoveFile("C:\\CS07\\Copy.txt", "C:\\CS07\\Inner\\Copy.txt");
            DirectoryOperations.NewDirectory("C:\\CS07\\Backup");
            DirectoryOperations.NewDirectory("C:\\CS07\\DirToMove");
            DirectoryOperations.DeleteDirectory("C:\\CS07\\Backup\\MovedDir");
            DirectoryOperations.MoveDirectory("C:\\CS07\\DirToMove", "C:\\CS07\\Backup\\MovedDir");

            //Enumerating over files and folders in a directory

            Console.WriteLine();
            DirectoryOperations.EnumerateAndPrint("C:\\CS07");
            DirectoryOperations.EnumerateAndPrint("C:\\CS07", ".txt");
            DirectoryOperations.EnumerateAndPrintSubdirectories("C:\\");

            //Operating on the Working Directory

            Console.WriteLine();
            Console.WriteLine("Default App Working Directory: " + Directory.GetCurrentDirectory());
            Directory.SetCurrentDirectory("C:\\CS07");
            Console.WriteLine("New App Working Directory: " + Directory.GetCurrentDirectory());
            DirectoryOperations.NewDirectory("RelativeDir");

            //System.IO.Path Class Operations

            Console.WriteLine();
            Console.WriteLine(Path.Combine("C:", "CS07", "Backup")); // C:\CS07\Backup 
            Console.WriteLine(Path.Combine("C:\\CS07", "Backup")); // C:\CS07\Backup 

            Console.WriteLine(Path.Combine("C:\\CS07", "Backup", "Example.txt")); // C:\CS07\Backup\Example.txt
            string[] pathToCobmine = new[] {"C:", "CS07", "Backup", "Example.txt"};
            Console.WriteLine(Path.Combine(pathToCobmine)); // C:\CS07\Backup\Example.txt

            Console.WriteLine();
            Console.WriteLine(Path.GetExtension("C:\\CS07\\Backup\\Example.txt")); // .txt
            Console.WriteLine(Path.GetExtension("C:\\CS07\\Backup.old\\Example.txt")); // .txt
            Console.WriteLine(Path.GetExtension("C:\\CS07\\Backup.old\\Example.txt\\")); // String.Empty

            Console.WriteLine();
            Console.WriteLine(Path.GetFullPath("RelativeDir")); // C:\CS07\RelativeDir
            Console.WriteLine(Path.GetFullPath("TheoreticalDir/Subdir/image.jpg")); // C:\CS07\TheoreticalDir\Subdir\image.jpg
            Console.WriteLine(Path.GetRelativePath(Directory.GetCurrentDirectory(), "C:\\CS07\\Example.txt")); // Example.txt
            Console.WriteLine(Path.GetRelativePath
                ("C:\\CS07\\Theoretical", 
                "C:\\CS07\\Theoretical\\TheoreticalFile.txt")); // TheoreticalFile.txt

            //Copy File Contents to Another File

            try
            {
                string textFileContent = File.ReadAllText("C:\\CS07\\Example.txt");
                textFileContent = textFileContent.ToUpper();
                File.WriteAllText("C:\\CS07\\Output.txt", textFileContent);
            }
            catch (IOException)
            {
                Console.Error.WriteLine("The source file does not exist or cannot be accessed!");
            }

            //Read all lines of a file into a string array and write a line to another file

            try
            {
                string[] textFileLines = File.ReadAllLines(@"C:\CS07\Example.txt");
                textFileLines[1] = textFileLines[1].ToUpper();
                File.WriteAllLines(@"C:\CS07\LinesOutput.txt", textFileLines);
            }
            catch (IOException) //If the file does not exist, ReadAllLines() will raise an IOException
            {
                Console.Error.WriteLine("The source file does not exist or cannot be accessed!");
            }
            catch (IndexOutOfRangeException) //If the line doesn't exist, accessing textFileLines[1] will raise the exception
            {
                Console.WriteLine("The file does not contain a second line!");
            }

            //Append an entire string to a text file

            try
            {
                string textToAppend = "abc Appended Text xyz";
                File.AppendAllText(@"C:\CS07\Output.txt", textToAppend);
            }
            catch (AccessViolationException)
            {
                Console.WriteLine("The selected file is read only!");
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while trying to append.");
            }

            //Append all lines of a string array to a text file

            try
            {
                string[] linesToAppend = new[] {"Lorem", "Ipsum", "Dolor", "Amat"};
                File.AppendAllLines(@"C:\CS07\Output.txt", linesToAppend);
            }
            catch (AccessViolationException)
            {
                Console.WriteLine("The selected file is read only!");
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while trying to append.");
            }

            //Write to and Read from a Binary File
            
            try
            {
                byte[] numbers = new byte[] { 0, 100, 255 };
                File.WriteAllBytes(@"C:\CS07\BinaryExample.bin", numbers);
                byte[] readNumbers = File.ReadAllBytes(@"C:\CS07\BinaryExample.bin");
                Console.WriteLine(readNumbers[1]); //100
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while trying to process the binary.");
            }

            //Copying a file with a Stream (Manual Definition)
            
            Console.WriteLine();

            try
            {
                using FileStream textInputStream = new(@"C:\CS07\Example.txt", FileMode.Open);
                using StreamReader textInputReader = new(textInputStream);

                using FileStream textOutputStream = new(@"C:\CS07\StreamOutput.txt", FileMode.Append);
                using StreamWriter textOutputWriter = new(textOutputStream);

                while (!textInputReader.EndOfStream)
                {
                    textOutputWriter.WriteLine(textInputReader.ReadLine().ToUpper());
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine("The specified file does not exist.");
            }

            //Copying a file with a Stream (With Shortcut Constructors)

            Console.WriteLine();

            try
            {
                //using StreamReader InputStream = File.OpenText(@"C:\CS07\Example.txt");
                using StreamReader inputStream = new(@"C:\CS07\Example.txt");

                using StreamWriter outputStream = new(@"C:\CS07\StreamOutput2.txt");

                while (!inputStream.EndOfStream)
                {
                    outputStream.WriteLine(inputStream.ReadLine().ToUpper());
                }
            }
            catch (IOException)
            {
                Console.Error.WriteLine("An I/O exception occurred while trying to copy the file.");
            }

            //Random Access With Streams

            Console.WriteLine();

            using FileStream textInputStream1 = new(@"C:\CS07\Example.txt", FileMode.Open);
            using (StreamReader textInputReader1 = new(textInputStream1))
            {
                textInputStream1.Seek(2, SeekOrigin.Begin); //rem ipsum dolor sit amet
                Console.WriteLine(textInputReader1.ReadLine());
            }

            using FileStream textInputStream2 = new(@"C:\CS07\Example.txt", FileMode.Open);
            using (StreamReader textInputReader2 = new(textInputStream2))
            {
                textInputStream2.Seek(-35, SeekOrigin.End);
                Console.WriteLine(textInputReader2.ReadLine());
            }

            using FileStream pStream = new(@"C:\CS07\Example.txt", FileMode.Open);
            using (StreamReader pReader = new(pStream))
            {
                pStream.Position = 6;
                Console.WriteLine(pReader.ReadLine()); //ipsum dolor sit amet
            }
        }
    }
}
