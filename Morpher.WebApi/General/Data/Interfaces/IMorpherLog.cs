namespace Morpher.WebService.V3.General
{
    using Microsoft.Owin;

    public interface IMorpherLog
    {
        void Log(IOwinContext context);

        void Sync();
    }
}