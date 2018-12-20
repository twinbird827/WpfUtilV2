using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV2.Common
{
    /// <summary>
    /// ストリームのWrapperクラス
    /// </summary>
    /// <remarks>
    /// Dispose 時に、内部ストリームの参照を外します
    /// </remarks>
    public class WrappingStream : Stream
    {
        Stream m_streamBase;

        public WrappingStream(Stream streamBase)
        {
            if (streamBase == null)
            {
                throw new ArgumentNullException("streamBase");
            }
            m_streamBase = streamBase; //渡したStreamを内部ストリームとして保持
        }

        public override bool CanRead
        {
            get { ThrowIfDisposed(); return m_streamBase.CanRead; }
        }

        public override bool CanSeek
        {
            get { ThrowIfDisposed(); return m_streamBase.CanSeek; }
        }

        public override bool CanWrite
        {
            get { ThrowIfDisposed(); return m_streamBase.CanWrite; }
        }

        public override long Length
        {
            get { ThrowIfDisposed(); return m_streamBase.Length; }
        }

        public override long Position
        {
            get { ThrowIfDisposed(); return m_streamBase.Position; }
            set { ThrowIfDisposed(); m_streamBase.Position = value; }
        }

        public override int ReadTimeout
        {
            get { ThrowIfDisposed(); return m_streamBase.ReadTimeout; }
            set { ThrowIfDisposed(); m_streamBase.ReadTimeout = value; }
        }

        public override int WriteTimeout
        {
            get { ThrowIfDisposed(); return m_streamBase.WriteTimeout; }
            set { ThrowIfDisposed(); m_streamBase.WriteTimeout = value; }
        }

        public override void Flush()
        {
            ThrowIfDisposed();
            m_streamBase.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            return m_streamBase.Read(buffer, offset, count);
        }

        //Streamクラスのメソッドをオーバーライドして、内部ストリームの同じメソッドをそのまま呼ぶだけ
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return m_streamBase.ReadAsync(buffer, offset, count, cancellationToken);
        }
        public new Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            return m_streamBase.ReadAsync(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfDisposed();
            return m_streamBase.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            ThrowIfDisposed();
            m_streamBase.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            m_streamBase.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_streamBase.Dispose();
                m_streamBase = null;  //disposeしたら内部ストリームをnullにして参照を外す
            }
            base.Dispose(disposing);
        }

        private void ThrowIfDisposed()
        {
            if (m_streamBase == null)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        public virtual byte[] ToArray()
        {
            var ms = m_streamBase as MemoryStream;
            if (ms != null)
            {
                return ms.ToArray();
            }
            else
            {
                throw new NotSupportedException("Inner stream is not MemoryStream");
            }
        }
    }
}
