using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQLTransportLibrary
{
    public static class ObjectLists
    {
        public static async Task<string[]> GetDatabaseListAsync(Guid sessionGuid)
        {
            SqlConnection connection = SessionManager.GetConnection(sessionGuid);

            using (SqlCommand databaseListCommand = new SqlCommand("SELECT [name] FROM [sys].[databases]", connection))
            {
                using (SqlDataReader databaseListReader = await databaseListCommand.ExecuteReaderAsync())
                {
                    List<string> databaseList = new List<string>();

                    while (await databaseListReader.ReadAsync())
                        databaseList.Add(await databaseListReader.GetFieldValueAsync<string>(0));

                    return databaseList.ToArray();
                }
            }
        }

        public static string[] GetTableList(Guid sessionGuid)
        {
            return SessionManager.GetConnection(sessionGuid).GetSchema("Tables").Rows.OfType<DataRow>().OrderBy(r => r["TABLE_NAME"]).Select(r => $"[{r["TABLE_SCHEMA"]}].[{r["TABLE_NAME"]}]").ToArray();
        }
    }
}
