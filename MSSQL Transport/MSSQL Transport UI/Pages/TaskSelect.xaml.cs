using System;
using System.Windows;
using System.Windows.Controls;

namespace MSSQLTransportUI.Pages
{
    /// <summary>
    /// Interaction logic for TaskSelect.xaml
    /// </summary>
    public partial class TaskSelect : Page
    {
        private Guid sessionGuid;

        public TaskSelect(Guid sessionGuid)
        {
            InitializeComponent();

            this.sessionGuid = sessionGuid;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ObjectSelect(this.sessionGuid));
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ImportSelect(this.sessionGuid));
        }
    }
}
