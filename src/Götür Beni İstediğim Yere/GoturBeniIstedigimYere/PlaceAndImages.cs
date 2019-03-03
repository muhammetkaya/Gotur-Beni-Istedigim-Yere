using GoturBeniIstedigimYere.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere
{
    public class PlaceAndImages
    {
        public Place Place{ get; set; }
        public string Icon { get; set; }
        public PlaceAndImages()
        {
            Place = new Place();
        }
    }
}
