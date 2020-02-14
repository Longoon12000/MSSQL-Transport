using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace MSSQLTransportUI.Pages
{
    /// <summary>
    /// Interaction logic for ImportSelect.xaml
    /// </summary>
    public partial class ImportSelect : Page
    {
        private OpenFileDialog openFileDialog;
        private Guid sessionGuid;

        public ImportSelect(Guid sessionGuid)
        {
            InitializeComponent();

            this.sessionGuid = sessionGuid;

            this.openFileDialog = new OpenFileDialog()
            {
                Filter = this.FindResource("file_dialog_filter") as string
            };
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            string path = this.txtPath.Text;

            if (String.IsNullOrEmpty(path))
            {
                MessageBox.Show(this.FindResource("error_no_path") as string, this.FindResource("error_title_path") as string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!File.Exists(path))
            {
                MessageBox.Show(this.FindResource("error_no_file") as string, this.FindResource("error_title_path") as string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.NavigationService.Navigate(new Import(this.sessionGuid, path));
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            if (!this.openFileDialog.ShowDialog() ?? false)
                return;

            this.txtPath.Text = this.openFileDialog.FileName;
        }
    }
}
