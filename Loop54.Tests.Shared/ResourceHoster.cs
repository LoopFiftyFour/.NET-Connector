using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Loop54.Tests
{
    public class ResourceHoster : IDisposable
    {
        // Don't allow the caller to specify a port, because closing and re-opening it does not work on Linux in .NET < 2.1.0 (see comment in SetupListener).

        public ResourceHoster() { Port = FreeTcpPort(); }

        private HttpListener _listener;
        private bool _isRunning;
        private Thread _thread;

        public string CalledPath { get; set; }
        public string CalledMethod { get; set; }
        public Dictionary<string, string> CalledHeaders { get; set; }
        public string ResourceString { get; set; }
        public int StatusCode { get; set; }
        public int Port { get; }

        private static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }

        public void Start()
        {
            if (_isRunning)
                return;

            _isRunning = true;
            _thread = new Thread(SetupListener);
            _thread.Start();
        }

        public void Stop()
        {
            _isRunning = false;
            if (_listener.IsListening)
            {
                _listener.Stop();
            }
        }

        private void SetupListener()
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException("The Http Server cannot run on this operating system.");

            //Set up webserver to host test files
            _listener = new HttpListener();

            try
            {
                var listenUrl = $"http://localhost:{Port}/";
                _listener.Prefixes.Add(listenUrl);

                try
                {
                    _listener.Start();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Failed to start listening on {listenUrl}", ex);
                }

                while (_isRunning)
                {
                    IAsyncResult result = _listener.BeginGetContext(ListenerCallback, _listener);
                    result.AsyncWaitHandle.WaitOne();
                }
            }
            finally
            {
                try
                {
                    _listener.Close();
                }
                catch (HttpListenerException)
                {
                    // This fails on Linux - see https://trello.com/c/uDv7zpAb/906-engine-test-fails-on-linux-because-resourcehosterdispose-does-not-close-port
                    // It's a .NET Core bug [https://github.com/dotnet/corefx/issues/24562] fixed in 2.1.0. This can be removed after upgrading to 2.1.0.
                    // The socket then remains open for a while (until the OS closes it), so trying to use the same port again fails.
                }
            }
        }

        public void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;

            if (listener.IsListening && _isRunning)
            {
                HttpListenerContext context = listener.EndGetContext(result);
                using (HttpListenerResponse response = context.Response)
                using (Stream outputBuffer = new MemoryStream()) // Write to a temporary MemoryStream, so we can get its Length to set ContentLength below
                {
                    try
                    {
                        CalledPath = context.Request.RawUrl;
                        CalledMethod = context.Request.HttpMethod;
                        CalledHeaders = GetHeaderDictionary(context.Request.Headers);

                        WriteToOutput(outputBuffer, ResourceString);
                        response.StatusCode = StatusCode;
                    }
                    catch (FileNotFoundException ex)
                    {
                        WriteToOutput(outputBuffer, ex.ToString());
                        response.StatusCode = 404;
                    }
                    catch (Exception ex)
                    {
                        WriteToOutput(outputBuffer, ex.ToString());
                        response.StatusCode = 500;
                    }
                    finally
                    {
                        // Copy to the real output stream (which does not support reading .Length)
                        using (Stream output = response.OutputStream)
                        {
                            response.ContentLength64 = outputBuffer.Length;
                            outputBuffer.Position = 0;
                            outputBuffer.CopyTo(output);
                        }
                    }
                }
            }
        }

        private Dictionary<string, string> GetHeaderDictionary(NameValueCollection headers)
        {
            var dic = new Dictionary<string, string>();

            foreach(var headerKey in headers.AllKeys)
            {
                dic[headerKey] = headers[headerKey];
            }

            return dic;
        }

        public void Dispose()
        {
            Stop();
        }
        
        private static void WriteToOutput(Stream output, string textToWrite)
        {
            var bytes = Encoding.UTF8.GetBytes(textToWrite);
            output.Write(bytes, 0, bytes.Length);
        }
    }
}
