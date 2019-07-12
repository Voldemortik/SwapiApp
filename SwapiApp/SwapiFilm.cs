using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;
namespace SwapiApp
{
  
    class SwapiFilm : SwapiEntity
    {
        
        public short Episode_id
        {
            get;
            set;
        }

       
        public List<string> Vehicles
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

      
        public List<string> Starships
        {
            get;
            set;
        }

       
        public string Title
        {
            get;
            set;
        }

     
        public List<string> Species
        {
            get;
            set;
        }

        
        public string Producer
        {
            get;
            set;
        }

     
        public List<string> Planets
        {
            get;
            set;
        }

        
        public string Director
        {
            get;
            set;
        }

       
        public List<string> Characters
        {
            get;
            set;
        }

       
        public string Opening_crawl
        {
            get;
            set;
        }
    }
}
