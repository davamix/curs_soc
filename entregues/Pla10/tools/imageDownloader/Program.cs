using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

//
// Download images from Wikimedia based on a list of categories
//

namespace imageDownloader
{
    class Program
    {
        static HttpClient httpClient = new HttpClient();
        static HtmlWeb web = new HtmlWeb();
        static readonly string ouputPath = Path.Combine(Directory.GetCurrentDirectory(), "downloads");

        static void Main(string[] args)
        {
            //ouputPath = Directory
            var imageLinks = new List<string>();
            
            // List of categories
            var addresses = new List<string>{
                 @"https://commons.wikimedia.org/wiki/Abies_alba",
                 @"https://commons.wikimedia.org/wiki/Abies_borisii-regis",
                 @"https://commons.wikimedia.org/wiki/Abies_bracteata",
                 @"https://commons.wikimedia.org/wiki/Abies_cephalonica",
                 @"https://commons.wikimedia.org/wiki/Abies_cilicica",
                 @"https://commons.wikimedia.org/wiki/Abies_concolor",
                 @"https://commons.wikimedia.org/wiki/Abies_delavayi",
                 @"https://commons.wikimedia.org/wiki/Abies_fabri",
                 @"https://commons.wikimedia.org/wiki/Abies_densa",
                 @"https://commons.wikimedia.org/wiki/Abies_fargesii",
                 @"https://commons.wikimedia.org/wiki/Abies_grandis",
                 @"https://commons.wikimedia.org/wiki/Adansonia_digitata",
                 @"https://commons.wikimedia.org/wiki/Adansonia_grandidieri",
                 @"https://commons.wikimedia.org/wiki/Alnus_glutinosa",
                 @"https://commons.wikimedia.org/wiki/Araucaria_angustifolia",
                 @"https://commons.wikimedia.org/wiki/Araucaria_araucana",
                 @"https://commons.wikimedia.org/wiki/Araucaria_bidwillii",
                 @"https://commons.wikimedia.org/wiki/Araucaria_cunninghamii",
                 @"https://commons.wikimedia.org/wiki/Araucaria_heterophylla",
                 @"https://commons.wikimedia.org/wiki/Arbutus_andrachne",
                 @"https://commons.wikimedia.org/wiki/Arbutus_menziesii",
                 @"https://commons.wikimedia.org/wiki/Arbutus_unedo",
                 @"https://commons.wikimedia.org/wiki/Arbutus_xalapensis",
                 @"https://commons.wikimedia.org/wiki/Betula_pendula",
                 @"https://commons.wikimedia.org/wiki/Betula_alleghaniensis",
                 @"https://commons.wikimedia.org/wiki/Betula_ermanii",
                 @"https://commons.wikimedia.org/wiki/Betula_medwediewii",
                 @"https://commons.wikimedia.org/wiki/Betula_pubescens",
                 @"https://commons.wikimedia.org/wiki/Betula_raddeana",
                 @"https://commons.wikimedia.org/wiki/Betula_utilis",
                 @"https://commons.wikimedia.org/wiki/Cupressus_arizonica",
                 @"https://commons.wikimedia.org/wiki/Cupressus_atlantica",
                 @"https://commons.wikimedia.org/wiki/Cupressus_bakeri",
                 @"https://commons.wikimedia.org/wiki/Cupressus_cashmeriana",
                 @"https://commons.wikimedia.org/wiki/Cupressus_funebris",
                 @"https://commons.wikimedia.org/wiki/Cupressus_glabra",
                 @"https://commons.wikimedia.org/wiki/Cupressus_guadalupensis",
                 @"https://commons.wikimedia.org/wiki/Cupressus_lusitanica",
                 @"https://commons.wikimedia.org/wiki/Cupressus_macrocarpa",
                 @"https://commons.wikimedia.org/wiki/Cupressus_sargentii",
                 @"https://commons.wikimedia.org/wiki/Cupressus_sempervirens",
                 @"https://commons.wikimedia.org/wiki/Cupressus_torulosa",
                 @"https://commons.wikimedia.org/wiki/Cupressus_×_leylandii",
                 @"https://commons.wikimedia.org/wiki/Castanea_dentata",
                 @"https://commons.wikimedia.org/wiki/Castanea_sativa",
                 @"https://commons.wikimedia.org/wiki/Cedrus_deodara",
                 @"https://commons.wikimedia.org/wiki/Cedrus_libani",
                 @"https://commons.wikimedia.org/wiki/Larix_decidua",
                 @"https://commons.wikimedia.org/wiki/Liriodendron_tulipifera",
                 @"https://commons.wikimedia.org/wiki/Metrosideros_excelsa",
                 @"https://commons.wikimedia.org/wiki/Morus_nigra",
                 @"https://commons.wikimedia.org/wiki/Phoenix_canariensis",
                 @"https://commons.wikimedia.org/wiki/Phoenix_dactylifera",
                 @"https://commons.wikimedia.org/wiki/Phoenix_reclinata",
                 @"https://commons.wikimedia.org/wiki/Phoenix_roebelenii",
                 @"https://commons.wikimedia.org/wiki/Picea_abies",
                 @"https://commons.wikimedia.org/wiki/Picea_engelmannii",
                 @"https://commons.wikimedia.org/wiki/Pinus_nigra",
                 @"https://commons.wikimedia.org/wiki/Pinus_pinaster",
                 @"https://commons.wikimedia.org/wiki/Pinus_pinea",
                 @"https://commons.wikimedia.org/wiki/Pinus_sylvestris",
                 @"https://commons.wikimedia.org/wiki/Pinus_sylvestris",
                 @"https://commons.wikimedia.org/wiki/Populus_trichocarpa",
                 @"https://commons.wikimedia.org/wiki/Populus_tremula",
                 @"https://commons.wikimedia.org/wiki/Sequoia_sempervirens",
                 @"https://commons.wikimedia.org/wiki/Sequoiadendron_giganteum",
                 @"https://commons.wikimedia.org/wiki/Sorbus_aucuparia",
                 @"https://commons.wikimedia.org/wiki/Tsuga_heterophylla",
                 @"https://commons.wikimedia.org/wiki/Yucca_decipiens",
                 @"https://commons.wikimedia.org/wiki/Abies_nordmanniana",
                 @"https://commons.wikimedia.org/wiki/Castanea_sativa",
                 @"https://commons.wikimedia.org/wiki/Olea_europaea",
                 @"https://commons.wikimedia.org/wiki/Salix_×_sepulcralis",
                 @"https://commons.wikimedia.org/wiki/Sorbus_aucuparia",

            };
            //var address = @"https://commons.wikimedia.org/wiki/Picea_abies";
            
            
            Parallel.ForEach(addresses, (address)=>{
                var htmlDoc = web.Load(address);
                var links = htmlDoc.DocumentNode.SelectNodes("//a//img");

                // Filter links
                foreach(var l in links){
                    if(l.GetAttributeValue("src", "NOVALUE").Contains("https://upload.wikimedia.org/wikipedia/commons/thumb")){

                        imageLinks.Add(l.Attributes["src"].Value);
                    }
                }
            });

            Console.WriteLine(imageLinks.Count);

            // Clean the addresses
            Parallel.For(0, imageLinks.Count, index=>{
                imageLinks[index] = RemoveThumb(imageLinks[index], "thumb");
                imageLinks[index] = RemoveLastPart(imageLinks[index]);
            });

            // Download and save the images
            Parallel.ForEach(imageLinks, (link)=>{
                var byteImg = DownloadImage(link);

                var imgName = GetImageName(link);

                var path = Path.Combine(ouputPath, imgName);
                SaveImage(path, byteImg);
            });
        }

        // Remove "thumb" text from address
        static string RemoveThumb(string link, string removable){
            var removed = link.Remove(link.IndexOf(removable), removable.Length + 1);

            return removed;
        }

        // Remove last section of the address
        static string RemoveLastPart(string link){
            var slashIndex = link.LastIndexOf("/");
            var countRemove = link.Length - slashIndex;
            
            return link.Remove(slashIndex, countRemove);
        }

        static byte[] DownloadImage(string link){
            Console.WriteLine($"Downloading {link} ...");
            byte[] imageBytes =  httpClient.GetByteArrayAsync(link).Result;

            return imageBytes;
        }

        static string GetImageName(string link){

            var linkSplit = link.Split("/");
            var name = linkSplit[linkSplit.Length-1];

            return name;
        }
        
        static void SaveImage(string path, byte[] imgBytes){
            Console.WriteLine($"Saving {path} ...");
            File.WriteAllBytes(path, imgBytes);
        }
    }
}
