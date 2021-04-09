using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace NBAStats.Models
{
    public class CarouselView : BindableBase
    {
        public CarouselView(string image)
        {
            Image = image;
        }
        public ImageSource Image { get; set; }
    }
}
