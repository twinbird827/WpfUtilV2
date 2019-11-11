using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
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

        /// ****************************************************************************************************
        /// override 定義
        /// ****************************************************************************************************

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

        public override bool CanTimeout
        {
            get { ThrowIfDisposed(); return m_streamBase.CanTimeout; }
        }

        public override void Flush()
        {
            ThrowIfDisposed(); m_streamBase.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed(); return m_streamBase.Read(buffer, offset, count);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            ThrowIfDisposed(); return m_streamBase.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ThrowIfDisposed(); return m_streamBase.WriteAsync(buffer, offset, count, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfDisposed(); return m_streamBase.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            ThrowIfDisposed(); m_streamBase.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed(); m_streamBase.Write(buffer, offset, count);
        }

        public override void Close()
        {
            ThrowIfDisposed(); m_streamBase.Close();
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
        {
            ThrowIfDisposed(); return m_streamBase.CopyToAsync(destination, bufferSize, cancellationToken);
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            ThrowIfDisposed(); return m_streamBase.CreateObjRef(requestedType);
        }

        public override bool Equals(object obj)
        {
            ThrowIfDisposed(); return m_streamBase.Equals(obj);
        }

        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            ThrowIfDisposed(); return m_streamBase.FlushAsync(cancellationToken);
        }

        public override int GetHashCode()
        {
            ThrowIfDisposed(); return m_streamBase.GetHashCode();
        }

        public override object InitializeLifetimeService()
        {
            ThrowIfDisposed(); return m_streamBase.InitializeLifetimeService();
        }

        public override int ReadByte()
        {
            ThrowIfDisposed(); return m_streamBase.ReadByte();
        }

        public override void WriteByte(byte value)
        {
            ThrowIfDisposed(); m_streamBase.WriteByte(value);
        }

        public override string ToString()
        {
            ThrowIfDisposed(); return m_streamBase.ToString();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            ThrowIfDisposed(); return m_streamBase.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            ThrowIfDisposed(); return m_streamBase.BeginWrite(buffer, offset, count, callback, state);
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            ThrowIfDisposed(); return m_streamBase.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            ThrowIfDisposed(); m_streamBase.EndWrite(asyncResult);
        }

        /// ****************************************************************************************************
        /// new 定義 (override不可ﾒｿｯﾄﾞの隠蔽)
        /// ****************************************************************************************************

        public new Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed(); return ReadAsync(buffer, offset, count, CancellationToken.None);
        }

        public new Task WriteAsync(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed(); return WriteAsync(buffer, offset, count, CancellationToken.None);
        }

        public new Task CopyToAsync(Stream destination)
        {
            ThrowIfDisposed(); return CopyToAsync(destination, 81920);
        }

        public new Task CopyToAsync(Stream destination, int bufferSize)
        {
            ThrowIfDisposed(); return CopyToAsync(destination, 81920, CancellationToken.None);
        }

        public new Task FlushAsync()
        {
            ThrowIfDisposed(); return FlushAsync(CancellationToken.None);
        }

        /// ****************************************************************************************************
        /// 内部Stream固有の処理
        /// ****************************************************************************************************

        /// <summary>
        /// ｽﾄﾘｰﾑの内容をﾊﾞｲﾄ配列に書き込みます。
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// WrappingStreamによって使用されている全てのﾘｿｰｽを開放します。
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_streamBase.Dispose();
                m_streamBase = null;  //disposeしたら内部ストリームをnullにして参照を外す
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// 内部Streamがすでに破棄されている場合、例外を発生させます。
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (m_streamBase == null)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

    }
}
