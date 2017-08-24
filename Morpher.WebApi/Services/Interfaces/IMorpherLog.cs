namespace Morpher.WebService.V3.Services.Interfaces
{
    using Microsoft.Owin;

    public interface IMorpherLog
    {
        void Log(IOwinContext context);

        void Sync();
    }
}