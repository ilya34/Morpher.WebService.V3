// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Dapper;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;

    public class MorpherDatabase : IMorpherDatabase
    {
        private readonly string connectionString;

        public MorpherDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int GetDefaultDailyQueryLimit()
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                return connection.QuerySingleOrDefault<int>("SELECT TOP 1 DailyQueryLimit FROM WebServiceSettings");
            }
        }

        public int GetQueryCountByIp(string ip)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                return connection.QuerySingle<int>(
                    "sp_GetQueryCountByIp",
                    new { Ip = ip },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public int GetQueryCountByToken(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                return connection.QuerySingle<int>(
                    "sp_GetQueryCount",
                    new { Token = guid },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public MorpherCacheObject GetUserLimits(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                return connection.QueryFirstOrDefault<MorpherCacheObject>(
                    "sp_GetLimit",
                    new { Token = guid },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public bool IsIpBlocked(string ip)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                return connection.QueryFirstOrDefault<bool>(
                    "SELECT Blocked FROM RemoteAddresses WHERE REMOTE_ADDR = @ip",
                    new { ip });
            }
        }
    }
}