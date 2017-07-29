// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services
{
    using System.Collections.Concurrent;
    using System.Net.Http;

    using Morpher.WebService.V3.Models;
    using Morpher.WebService.V3.Services.Interfaces;
    using Morpher.WebService.V3.Shared.Models.Exceptions;

    public class MorpherLogLocal : IMorpherLog
    {
        public ConcurrentQueue<LogEntity> logQueue { get; }

        public void Log(HttpRequestMessage message, MorpherException exception = null)
        {
            return;
        }

        public void Sync()
        {
            return;
        }
    }
}