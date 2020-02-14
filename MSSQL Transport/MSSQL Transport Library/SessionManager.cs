using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MSSQLTransportLibrary
{
    public static class SessionManager
    {
        private static readonly Dictionary<Guid, SqlConnection> sessions = new Dictionary<Guid, SqlConnection>();

        public static SqlConnection GetConnection(Guid guid) => sessions.ContainsKey(guid) ? sessions[guid] : null;

        public static async Task<Guid> CreateConnectionAsync(AuthenticationData authenticationData, CancellationToken? cancellationToken = null)
        {
            SqlConnection connection = new SqlConnection(authenticationData.ConnectionString);
            try
            {
                if (!cancellationToken.HasValue)
                    await connection.OpenAsync();
                else
                    await connection.OpenAsync(cancellationToken.Value);
            }
            catch (TaskCanceledException)
            {
                throw new Exception("The connection attempt has timed out.");
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Guid guid = Guid.NewGuid();
            sessions.Add(guid, connection);
            return guid;
        }

        public static void CloseConnection(Guid guid)
        {
            SqlConnection connection = GetConnection(guid);
            if (connection == null)
                return;

            if (connection.State != System.Data.ConnectionState.Closed)
                connection.Close();

            connection.Dispose();

            sessions.Remove(guid);
        }
    }
}
