using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.Internal;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.Net.Http.Headers;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Newtonsoft.Json;

namespace Microsoft.AspNet.OData.Formatters
{
    public class ODataOutputFormatter : OutputFormatter
    {
        /// <summary>
        /// Returns UTF8 Encoding without BOM and throws on invalid bytes.
        /// </summary>
        public static readonly Encoding UTF8EncodingWithoutBOM
            = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        /// <summary>
        /// Returns UTF16 Encoding which uses littleEndian byte order with BOM and throws on invalid bytes.
        /// </summary>
        public static readonly Encoding UTF16EncodingLittleEndian = new UnicodeEncoding(bigEndian: false,
                                                                                        byteOrderMark: true,
                                                                                        throwOnInvalidBytes: true);

        public ODataOutputFormatter()
        {
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json"));
        }

        public override Task WriteResponseBodyAsync(OutputFormatterContext context)
        {
            var response = context.ActionContext.HttpContext.Response;
            var selectedEncoding = context.SelectedEncoding;
            var model = context.ActionContext.HttpContext.Request.ODataProperties().Model;

            using (var delegatingStream = new NonDisposableStream(response.Body))
            using (var writer = new StreamWriter(delegatingStream, selectedEncoding, 1024, leaveOpen: true))
            {
                WriteObject(writer, context.Object, model);
            }

            return Task.FromResult(true);
        }

        public override void WriteResponseHeaders(OutputFormatterContext context)
        {
            context.SelectedContentType.Parameters.Add(new NameValueHeaderValue("odata.metadata", "minimal"));
            context.ActionContext.HttpContext.Response.Headers.Add("OData-Version", new []{ "4.0" });
            base.WriteResponseHeaders(context);
        }
        
        // In the future, should convert to ODataEntry and use ODL to write out.
        // Or use ODL to build a JObject and use Json.NET to write out.
        public void WriteObject(TextWriter writer, object value, IEdmModel model)
        {
            if (value is IEdmModel)
            {
                WriteMetadata(writer, model);
                return;
            }

            using (var jsonWriter = CreateJsonWriter(writer))
            {
                var jsonSerializer = CreateJsonSerializer(model);
                jsonSerializer.Serialize(jsonWriter, value);
            }
        }
        private JsonSerializer CreateJsonSerializer(IEdmModel model)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new ODataJsonConverter(new Uri("http://localhost:58888/"), model));
            var jsonSerializer = JsonSerializer.Create(serializerSettings);
            return jsonSerializer;
        }
        
        private JsonWriter CreateJsonWriter(TextWriter writer)
        {
            var jsonWriter = new JsonTextWriter(writer);
            jsonWriter.CloseOutput = false;

            return jsonWriter;
        }

        private void WriteMetadata(TextWriter writer, IEdmModel model)
        {
            using (var xmlWriter = XmlWriter.Create(writer))
            {
                IEnumerable<EdmError> errors;
                CsdlWriter.TryWriteCsdl(model, xmlWriter, out errors);
            }
        } 
    }
}