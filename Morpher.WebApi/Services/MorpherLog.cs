namespace Morpher.WebApi.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Policy;

    using Morpher.WebApi.Extensions;
    using Morpher.WebApi.Models.Exceptions;
    using Morpher.WebApi.Services.Interfaces;

    public class MorpherLog : IMorpherLog
    {
        private readonly string connectionString;

        private readonly DataTable dataTable;

        private readonly int logCapicity;

        private readonly Object lockObject = new object();

        public MorpherLog(string connectionString, int logCapicity)
        {
            this.connectionString = connectionString;
            this.logCapicity = logCapicity;
            this.dataTable = new DataTable("Queries3");

            this.dataTable.Columns.Add("RemoteAddress", typeof(string));
            this.dataTable.Columns.Add("QueryString", typeof(string));
            this.dataTable.Columns.Add("QuerySource", typeof(string));
            this.dataTable.Columns.Add("WebServiceToken", typeof(Guid));
            this.dataTable.Columns.Add("UserAgent", typeof(string));
            this.dataTable.Columns.Add("ErrorCode", typeof(int));
        }


        public void Log(HttpRequestMessage message, MorpherException exception = null)
        {
            string remoteAddress = message.GetClientIp();
            Dictionary<string, string> dictionary = message.GetQueryStrings();
            string queryString = string.Empty;
            string querySource = new Uri(message.RequestUri.ToString()).AbsolutePath;

            int errorCode = exception?.Code ?? 0;
            if (dictionary != null)
            {
                queryString = string.Join(";", dictionary.Select(pair => $"{pair.Key}={pair.Value}"));
            }

            string userAgent = message.Headers.UserAgent?.ToString();
            Guid? token = message.GetToken();

            lock (this.lockObject)
            {
                this.dataTable.Rows.Add(remoteAddress, queryString, querySource, token, userAgent, errorCode);

                if (this.dataTable.Rows.Count >= this.logCapicity)
                {
                    using (SqlConnection connection = new SqlConnection(this.connectionString))
                    {
                        using (SqlBulkCopy bulkCopy = new SqlBulkCopy(
                            connection,
                            SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction,
                            null))
                        {
                            foreach (DataColumn column in this.dataTable.Columns)
                            {
                                bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                            }

                            bulkCopy.DestinationTableName = this.dataTable.TableName;
                            connection.Open();
                            bulkCopy.WriteToServer(this.dataTable);
                            connection.Close();
                        }
                    }

                    this.dataTable.Rows.Clear();
                }
            }
        }
    }
}