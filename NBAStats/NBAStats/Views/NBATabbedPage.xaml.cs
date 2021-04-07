using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NBAStats.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NBATabbedPage : TabbedPage
    {
        public NBATabbedPage()
        {
            InitializeComponent();
        }

        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            var tabPage = (TabbedPage)sender;
            Title = tabPage.CurrentPage.Title;
        }
    }
}