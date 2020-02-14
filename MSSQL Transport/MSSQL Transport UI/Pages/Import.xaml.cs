using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MSSQLTransportUI.Pages
{
    /// <summary>
    /// Interaction logic for Import.xaml
    /// </summary>
    public partial class Import : Page
    {
        private Guid sessionGuid;
        private string importFile;

        public Import(Guid sessionGuid, string importFile)
        {
            InitializeComponent();

            this.sessionGuid = sessionGuid;
            this.importFile = importFile;
        }
    }
}
