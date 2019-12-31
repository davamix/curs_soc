using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// https://github.com/dotnet/command-line-api/wiki/Your-first-app-with-System.CommandLine.DragonFruit

// This script create two files, train.txt and valid.txt with the images path
// train.txt -> 100 images
// valid.txt -> 50 images

namespace custom_files_dataset
{
    class Program
    {
        /// <param name="imagesPath">Path where the images are saved</param>
        /// <param name="labelsPath">Path with the annotation label files</param>
        static void Main(string imagesPath, string labelsPath)
        {
            var trainFilePath = Path.Combine(Directory.GetCurrentDirectory(), "train.txt");
            var validFilePath = Path.Combine(Directory.GetCurrentDirectory(), "valid.txt");

            // Get label names from files
            var labelFiles = new List<string>();
            foreach(string path in Directory.GetFiles(labelsPath)){
                labelFiles.Add(Path.GetFileNameWithoutExtension(path));
            }

            // Get images path corresponding with the annotation labels
            var df = new DirectoryInfo(imagesPath);
            var images = new List<string>();
            foreach(string label in labelFiles){
                images.Add(GetImageFileName(df, label));
            }
            
            Console.WriteLine("Writing 100 images into train.txt ...");
            WriteLines(trainFilePath, images.Take(100));

            Console.WriteLine("Writing 50 images into valid.txt ...");
            WriteLines(validFilePath, images.TakeLast(50));
        }

        static string GetImageFileName(DirectoryInfo df, string labelName){
            return df.GetFiles($"{labelName}.*").First().FullName;
        }

        static void WriteLines(string filePath, IEnumerable<string> lines){
            using(StreamWriter writer = new StreamWriter(filePath)){
                foreach(string line in lines){
                    writer.WriteLine(line);
                }
            }
        }
    }
}
