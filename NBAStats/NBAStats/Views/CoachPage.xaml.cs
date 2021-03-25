using System;
using System.Collections.Generic;
using Prism.Mvvm;
using Xamarin.Forms;

namespace NBAStats.Views
{
    public partial class CoachPage : ContentPage
    {
        public CoachPage()
        {

            InitializeComponent();
            ViewModelLocator.SetAutowireViewModel(this, true);

        }
    }
}
