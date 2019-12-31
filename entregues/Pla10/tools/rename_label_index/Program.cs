using System;
using System.IO;
using System.Threading.Tasks;

// Replace the category index in the annotation files (data/custom/labels/*.txt)
// From: 15 0.027729 0.499120 0.054577 0.995892
// To:    0 0.027729 0.499120 0.054577 0.995892
// The index is based on classes.names files.

namespace rename_label_index
{
    class Program
    {
        /// <param name="annotationsPath">Path of the annotation files</param>
        static void Main(string annotationsPath)
        {
            var annotations = Directory.GetFiles(annotationsPath);

            Parallel.ForEach(annotations, (path)=>{
                Console.WriteLine($"Replacing index in {path} ...");
                ReplaceIndex(path);
            });
        }

        static void ReplaceIndex(string filePath){
            var lines = File.ReadAllLines(filePath);

            for(var x=0;x<lines.Length;x++){
                // Split the line by white space (default char)
                var parts = lines[x].Split();
                // Replace the index by 0
                parts[0] = "0";
                //Join the line again
                lines[x] = string.Join(" ", parts);
            }

            // Overwrite the file with the lines updated
            File.WriteAllLines(filePath, lines);
        }
    }
}