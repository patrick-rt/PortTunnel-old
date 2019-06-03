/*
MIT License
Copyright (c) 2019 Patrick72762
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.Collections.Generic;
using System.Net;

namespace PortTunnelLib
{
    class StreamLogger
    {
        private static Dictionary<string, string> log = new Dictionary<string, string>();

        public static void Write(EndPoint source, EndPoint dest, byte b)
        {
            string ep = "From " +
                ((IPEndPoint)source).Address + ":" +
                ((IPEndPoint)source).Port + " to " +
                ((IPEndPoint)dest).Address + ":" +
                ((IPEndPoint)dest).Port;

            if (!log.ContainsKey(ep))
            {
                log.Add(ep, "StreamLog " + ep + "\r\n\r\n" +
                    (char)b);
            }
            else
            {
                string tmp = log[ep];
                log[ep] = tmp + (char)b;
            }
        }

        public static void EndLogging(EndPoint source, EndPoint dest)
        {
            string ep = "From " +
                ((IPEndPoint)source).Address + ":" +
                ((IPEndPoint)source).Port + " to " +
                ((IPEndPoint)dest).Address + ":" +
                ((IPEndPoint)dest).Port;

            if (log.ContainsKey(ep))
            {
                try
                {
                    Log.main.NewStreamLogCompleted.Invoke(null, log[ep]);
                }
                catch { }

                log.Remove(ep);
            }
        }
    }
}
