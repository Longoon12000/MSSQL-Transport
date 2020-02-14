using MSSQLTransportLibrary;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MSSQLTransportUI.Pages
{
    /// <summary>
    /// Interaction logic for ServerConnect.xaml
    /// </summary>
    public partial class ServerConnect : Page
    {
        public ServerConnect()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (this.gbSqlAuthentication == null)
                return;

            this.gbSqlAuthentication.IsEnabled = this.rbSqlAuthentication.IsChecked ?? false;
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationData authenticationData = new AuthenticationData()
            {
                ServerAddress = this.txtServerAddress.Text,
                UseWindowsAuthentication = !this.rbSqlAuthentication.IsChecked ?? true,
                Username = this.txtUsername.Text,
                Password = this.txtPassword.Password
            };

            if (String.IsNullOrEmpty(authenticationData.ServerAddress))
            {
                MessageBox.Show(this.FindResource("error_server_address_required") as string, this.FindResource("error_title_missing_field") as string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!authenticationData.UseWindowsAuthentication && String.IsNullOrEmpty(authenticationData.Username))
            {
                MessageBox.Show(this.FindResource("error_sql_username_required") as string, this.FindResource("error_title_missing_field") as string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!authenticationData.UseWindowsAuthentication && String.IsNullOrEmpty(authenticationData.Password))
            {
                MessageBox.Show(this.FindResource("error_sql_password_required") as string, this.FindResource("error_title_missing_field") as string, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Task.Run(async () =>
            {
                this.Dispatcher.Invoke(() => this.processing(true));
                await this.connectionTestAsync(authenticationData);
                this.Dispatcher.Invoke(() => this.processing(false));
            });
        }

        private void processing(bool processing)
        {
            this.IsEnabled = !processing;
            this.pnlProgress.Visibility = processing ? Visibility.Visible : Visibility.Hidden;
        }

        private async Task connectionTestAsync(AuthenticationData authenticationData)
        {
            Guid testConnection = Guid.Empty;
            try
            {
                testConnection = await SessionManager.CreateConnectionAsync(authenticationData, new CancellationTokenSource(1000 * 5).Token);
                this.Dispatcher.Invoke(() => this.connectionSuccess(testConnection));
            }
            catch (Exception e)
            {
                this.Dispatcher.Invoke(() => this.connectionFailure(e));
                SessionManager.CloseConnection(testConnection);
            }
        }

        private void connectionSuccess(Guid sessionGuid)
        {
            this.NavigationService.Navigate(new DatabaseSelect(sessionGuid));
        }

        private void connectionFailure(Exception cause)
        {
            MessageBox.Show(String.Format(this.FindResource("error_sql_connection") as string, cause.Message), this.FindResource("error_title_sql_connection") as string, MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
