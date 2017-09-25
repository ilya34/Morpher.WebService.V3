namespace Morpher.WebService.V3.General.Data
{
    using Microsoft.Owin;

    public interface IMorpherLog
    {
        void Log(IOwinContext context);

        void Sync();
    }
}