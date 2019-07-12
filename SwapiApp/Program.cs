using System;

using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SwapiApp
{
    
    class Program
    {
        static void Main(string[] args)
        {
            
            
            SwapiPeople luke = new SwapiPeople();
            List<SwapiFilm> lukesFilms = new List<SwapiFilm>();
            SwapiHelper sh = new SwapiHelper();
            List<SwapiPeople> sp = sh.GetAllPeople().Results;
            string path = Path.GetTempFileName();
            FileInfo resultFile = new FileInfo(path);
            int count = 1;
            luke = sp.FirstOrDefault(x=>x.Name=="Luke Skywalker");

            if (resultFile.Exists&&luke!=null)
            {
                //Create a file to write to.
                using (StreamWriter sw = resultFile.CreateText())
                {
                    sw.WriteLine("Name: "+luke.Name);
                    sw.WriteLine("HomePlanet: " + sh.GetPlanet(luke.Homeworld).Result.Name);
                    sw.WriteLine("Films:");
                    foreach (string film in luke.Films)
                    {

                      sw.WriteLine(string.Format("{0}. {1}",count++,sh.GetFilm(film).Result.Title));

                    }
                    
                }
            }

            using (StreamReader sr = resultFile.OpenText())
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
            
            Console.ReadLine();


        }

       
    }
}
