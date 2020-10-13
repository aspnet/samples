using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BinarySubmit
{
    public class ChunkedStream : Stream
    {
        private readonly IJSObjectReference _js;
        private readonly int _maxAllowedLength;
        private int _readBytes;
        private bool _previouslyReadFullBuffer;

        public ChunkedStream(IJSObjectReference jsObject, int maxAllowedLength = 512 * 1024)
        {
            _js = jsObject;
            _maxAllowedLength = maxAllowedLength;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new System.NotSupportedException();

        public override long Position { get => _readBytes; set => throw new System.NotSupportedException(); }

        public override void Flush()
        {
            throw new System.NotSupportedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
            => throw new System.NotSupportedException();


        public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            if (_readBytes > 0 && !_previouslyReadFullBuffer)
            {
                // If we did not read a full buffer in the previous read, we must be done. There is no reason
                // for the JS to send us partial buffers.
                return 0;
            }


            var bytes = await _js.InvokeAsync<byte[]>("readBytes", _readBytes);

            if (_maxAllowedLength - _readBytes <  bytes.Length)
            {
                throw new InvalidDataException("Too many bytes read.");
            }

            // Keep this in sync with JS
            const int SegmentSize = 20 * 1024;
            _previouslyReadFullBuffer = bytes.Length == SegmentSize;

            bytes.AsMemory().CopyTo(buffer);
            _readBytes += bytes.Length;

            return bytes.Length;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new System.NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new System.NotSupportedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotSupportedException();
        }
    }
}