using System;
using System.IO;
using System.Threading.Tasks;

// https://github.com/dotnet/command-line-api/wiki/Your-first-app-with-System.CommandLine.DragonFruit

//
// Rename all files in the folder
// 

namespace renameFiles
{
    class Program
    {
        /// <param name="name">Main name of the file</param>
        static void Main(string name)
        {
            var currentPath = Directory.GetCurrentDirectory();
            var imagesPath = Path.Combine(currentPath, "images");

            var files = Directory.GetFiles(imagesPath);

            Console.WriteLine($"Renaming {files.Length} files from {imagesPath} ...");

            Parallel.For(0, files.Length, index =>{
                string extension = Path.GetExtension(files[index]);
                string newName = string.Concat(name, "_", index, extension);
                string newPath = Path.Combine(imagesPath, newName);
                File.Move(files[index], newPath);
            });

            Console.WriteLine("All files has been renamed");
        }
    }
}
