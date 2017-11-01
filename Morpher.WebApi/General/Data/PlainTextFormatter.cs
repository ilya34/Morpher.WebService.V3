namespace Morpher.WebService.V3.General.Data
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    public class PlainTextFormatter : MediaTypeFormatter
    {
        public PlainTextFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));
        }

        public override bool CanReadType(Type type) =>
            type == typeof(string);

        public override bool CanWriteType(Type type) =>
            type == typeof(string);

        public override async Task<object> ReadFromStreamAsync(
            Type type,
            Stream readStream,
            HttpContent content,
            IFormatterLogger formatterLogger)
        {
            var streamReader = new StreamReader(readStream);
            return await streamReader.ReadToEndAsync();
        }

        public override async Task WriteToStreamAsync(
            Type type,
            object value,
            Stream writeStream,
            HttpContent content,
            TransportContext transportContext)
        {
            var streamWriter = new StreamWriter(writeStream);
            await streamWriter.WriteAsync((string)value);
            await streamWriter.FlushAsync();
        }
    }
}