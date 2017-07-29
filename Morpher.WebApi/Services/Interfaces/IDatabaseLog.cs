// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
namespace Morpher.WebService.V3.Services.Interfaces
{
    using System.Collections.Concurrent;

    using Morpher.WebService.V3.Models;

    public interface IDatabaseLog
    {
        void Upload(ConcurrentQueue<LogEntity> logs);
    }
}