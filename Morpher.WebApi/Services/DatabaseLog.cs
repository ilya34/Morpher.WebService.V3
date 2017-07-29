// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Data;
    using System.Data.SqlClient;

    using Morpher.WebService.V3.Extensions;
    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;

    public class DatabaseLog : IDatabaseLog
    {
        private readonly string connectionString;

        public DatabaseLog(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void Upload(ConcurrentQueue<LogEntity> logs)
        {
            DataTable dataTable = new DataTable("Queries3");

            dataTable.Columns.Add("RemoteAddress", typeof(string));
            dataTable.Columns.Add("QueryString", typeof(string));
            dataTable.Columns.Add("QuerySource", typeof(string));
            dataTable.Columns.Add("DateTimeUTC", typeof(DateTime));
            dataTable.Columns.Add("WebServiceToken", typeof(Guid));
            dataTable.Columns.Add("UserAgent", typeof(string));
            dataTable.Columns.Add("ErrorCode", typeof(int));
            dataTable.Columns.Add("UserId", typeof(Guid));

            LogEntity logEntity;
            while (logs.TryDequeue(out logEntity))
            {
                dataTable.Rows.Add(
                    logEntity.RemoteAddress,
                    logEntity.QueryString.Truncate(150),
                    logEntity.QuerySource.Truncate(150),
                    logEntity.DateTimeUTC,
                    logEntity.WebServiceToken,
                    logEntity.UserAgent,
                    logEntity.ErrorCode,
                    logEntity.UserId);
            }

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                SqlBulkCopyOptions options =
                      SqlBulkCopyOptions.TableLock
                    | SqlBulkCopyOptions.FireTriggers
                    | SqlBulkCopyOptions.UseInternalTransaction;

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(
                    connection,
                    options,
                    null))
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                    }

                    bulkCopy.DestinationTableName = dataTable.TableName;
                    connection.Open();
                    bulkCopy.WriteToServer(dataTable);
                    connection.Close();
                }
            }
        }
    }
}