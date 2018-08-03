using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;

namespace ControllerSpecificConfigSample
{
    /// <summary>
    /// This sample formatter illustrates how to use the BufferedMediaTypeFormatter base class for
    /// writing a MediaTypeFormatter. The BufferedMediaTypeFormatter is useful when you either want
    /// to aggregate many small reads or writes or when you are writing synchronously to the underlying
    /// stream.
    /// </summary>
    public class PlainTextBufferedFormatter : BufferedMediaTypeFormatter
    {
        public PlainTextBufferedFormatter()
        {
            // Set supported media type for this media type formatter
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/plain"));

            // Set default supported character encodings for this media type formatter (UTF-8 and UTF-16)
            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
            SupportedEncodings.Add(new UnicodeEncoding(bigEndian: false, byteOrderMark: true, throwOnInvalidBytes: true));
        }

        public override bool CanReadType(Type type)
        {
            // In this sample we only say we can read strings
            return type == typeof(string);
        }

        public override bool CanWriteType(Type type)
        {
            // In this sample we only say we can write strings
            return type == typeof(string);
        }

        public override object ReadFromStream(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            // Get the preferred character encoding based on information in the request
            Encoding effectiveEncoding = SelectCharacterEncoding(content.Headers);

            // Create a stream reader and read the content synchronously
            using (StreamReader sReader = new StreamReader(readStream, effectiveEncoding))
            {
                return sReader.ReadToEnd();
            }
        }

        public override void WriteToStream(Type type, object value, Stream writeStream, HttpContent content)
        {
            // Get the preferred character encoding based on information in the request and what we support
            Encoding effectiveEncoding = SelectCharacterEncoding(content.Headers);

            // Create a stream writer and write the content synchronously
            using (StreamWriter sWriter = new StreamWriter(writeStream, effectiveEncoding))
            {
                sWriter.Write(value);
            }
        }
    }
}