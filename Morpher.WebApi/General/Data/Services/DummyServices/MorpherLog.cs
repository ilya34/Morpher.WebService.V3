using Microsoft.Owin;

namespace Morpher.WebService.V3.General.DummyServices
{
    public class MorpherLog : IMorpherLog
    {
        public void Log(IOwinContext context) { return; }
        public void Sync() { return; }
    }
}