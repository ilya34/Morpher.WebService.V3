﻿namespace Morpher.WebApi.Services
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Dapper;

    using Morpher.WebApi.Models;
    using Morpher.WebApi.Services.Interfaces;

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

        public CacheObject GetUserLimits(Guid guid)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                return connection.QueryFirstOrDefault<CacheObject>(
                    "sp_GetLimit",
                    new { Token = guid },
                    commandType: CommandType.StoredProcedure);
            }
        }

        public bool IsIpBlocked(string ip)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                return connection.QuerySingleOrDefault<bool>(
                    "SELECT Blocked FROM RemoteAddresses WHERE REMOTE_ADDR = @ip",
                    new { ip });
            }
        }
    }
}