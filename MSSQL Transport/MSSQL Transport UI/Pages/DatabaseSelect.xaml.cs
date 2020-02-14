using MSSQLTransportLibrary;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MSSQLTransportUI.Pages
{
    /// <summary>
    /// Interaction logic for DatabaseSelect.xaml
    /// </summary>
    public partial class DatabaseSelect : Page
    {
        private Guid sessionGuid;

        public DatabaseSelect(Guid sessionGuid)
        {
            InitializeComponent();

            this.sessionGuid = sessionGuid;
            Task.Run(async () =>
            {
                this.Dispatcher.Invoke(() => this.processing(true));
                await this.loadDatabasesAsync();
                this.Dispatcher.Invoke(() => this.processing(false));
            });
        }

        private async Task loadDatabasesAsync()
        {
            string[] databaseNames = await ObjectLists.GetDatabaseListAsync(this.sessionGuid);
            this.Dispatcher.Invoke(() => this.lvDatabaseNames.ItemsSource = databaseNames);
        }

        private void processing(bool processing)
        {
            this.IsEnabled = !processing;
            this.pnlProgress.Visibility = processing ? Visibility.Visible : Visibility.Hidden;
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Task.Run(this.selectDatabase);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(this.selectDatabase);
        }

        private async Task selectDatabase()
        {
            string selectedDatabase = String.Empty;
            this.Dispatcher.Invoke(() =>
            {
                this.processing(true);
                selectedDatabase = this.lvDatabaseNames.SelectedItem as string;
            });
            await SessionManager.GetConnection(this.sessionGuid).ChangeDatabaseAsync(selectedDatabase);
            this.Dispatcher.Invoke(() =>
            {
                this.processing(false);
                this.NavigationService.Navigate(new TaskSelect(this.sessionGuid));
            });
        }
    }
}
