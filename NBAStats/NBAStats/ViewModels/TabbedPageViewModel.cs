using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace NBAStats.ViewModels
{
    public class TabbedPageViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; } = "Home";
        public TabbedPageViewModel()
        {
            CurrentPageChangedCommand = new Command<string>(OnCurrentPageChanged);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnCurrentPageChanged(string currentPageTitle)
        {
            Title = currentPageTitle;
        }

        public ICommand CurrentPageChangedCommand { get; }


    }
}
