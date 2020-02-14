using Microsoft.Win32;
using MSSQLTransportLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for ObjectSelect.xaml
    /// </summary>
    public partial class ObjectSelect : Page
    {
        private SaveFileDialog saveFileDialog;

        private Guid sessionGuid;

        public ObjectSelect(Guid sessionGuid)
        {
            InitializeComponent();

            this.sessionGuid = sessionGuid;
            this.saveFileDialog = new SaveFileDialog()
            {
                Filter = this.FindResource("file_dialog_filter") as string,
                OverwritePrompt = true
            };

            Task.Run(() =>
            {
                this.Dispatcher.Invoke(() => this.progress(true));
                this.loadObjects();
                this.Dispatcher.Invoke(() => this.progress(false));
            });
        }

        private void loadObjects()
        {
            string[] tables = ObjectLists.GetTableList(this.sessionGuid);

            this.Dispatcher.Invoke(() =>
            {
                this.lvTables.ItemsSource = tables;
            });
        }

        private void progress(bool progress)
        {
            this.IsEnabled = !progress;
            this.pnlProgress.Visibility = progress ? Visibility.Visible : Visibility.Hidden;
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (!this.saveFileDialog.ShowDialog() ?? false)
                return;

            this.txtPath.Text = this.saveFileDialog.FileName;
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
