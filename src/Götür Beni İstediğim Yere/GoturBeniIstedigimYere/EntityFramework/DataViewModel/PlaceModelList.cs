using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoturBeniIstedigimYere.EntityFramework.DataViewModel
{
    public class PlaceModelList
    {
        public PlaceCategoriesViewModel PlaceCateM { get; set; }
        public PlaceViewModel PlaceM { get; set; }
        public PlaceModelList()
        {
            this.PlaceCateM = new PlaceCategoriesViewModel();
            this.PlaceM = new PlaceViewModel();
        }
    }
}
