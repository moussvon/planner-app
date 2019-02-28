using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Text;
namespace Projet2Cpi
{
    public class OtherThread
    {

        private static TcpClient c;
        private static NetworkStream stream;
        private static StreamReader r;
        private static StreamWriter w;
        private static bool connected = false;
        private static List<String> pending = new List<string>();
        private static bool started;
        private static Thread other_thread;
        private static bool disconnect_requested = false;
        private static String IP = "127.0.0.1"; //"192.168.43.162"; // IP ADDRESS OR DOMAIN NAME
        private static bool alive()
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(Sec.encrypt("PING", Sec.dh_secret));
                stream.Write(data, 0, data.Length);
                w.Flush();
                if (Sec.decrypt(r.ReadLine(), Sec.dh_secret) == "PONG") return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static void main()
        {
            while (true)
            {
                try
                {
                    c = new TcpClient(IP, 1337);
                    Thread.Sleep(2000);
                    setConnected(c.Connected);
                    setStream(c.GetStream());
                    setReader(new StreamReader(stream));
                    setWriter(new StreamWriter(stream));
                    Sec.dh(stream, r, w); // diffie hellman
                    while (true)
                    {
                        if (!alive() || disconnect_requested)
                        {
                            disconnect_requested = false;
                            throw new SocketException();

                        }
                        if (IsPendingData())
                        {
                            String enc = Sec.encrypt(getPendingData(), Sec.dh_secret);
                            //MessageBox.Show(enc);
                            byte[] data = Encoding.ASCII.GetBytes(enc);
                            stream.Write(data, 0, data.Length);
                            w.Flush();
                            if (Sec.decrypt(r.ReadLine(), Sec.dh_secret) == "ACK")
                            {
                                // MessageBox.Show("ACK"); message recu :D
                                clearPendingData();
                            }
                            else
                            {
                                // MessageBox.Show("NAK"); message non recu, reessayer l'envoi
                            }
                        }
                        Thread.Sleep(2000);
                    }

                }
                catch (SocketException)
                {
                    connected = false;
                    if (r != null) r.Dispose();
                    if (w != null) w.Dispose();
                    if (stream != null) stream.Dispose();
                    if (c != null) c.Dispose();
                    //MessageBox.Show("disconnected : " + e.Message);
                }
            }
        }

        private static bool IsPendingData()
        {
            lock (pending)
            {
                return pending.Count != 0;
            }
        }

        public static void disconnect() // requests a disconnect from the server (but will re-attempt to reconnect after that)
        {
            lock ((object)disconnect_requested)
            {
                disconnect_requested = true;
            }
        }
        private static String getPendingData()
        {
            lock (pending)
            {
                return JsonConvert.SerializeObject(pending);
            }
        }

        private static void clearPendingData()
        {
            FileStream temp = new FileStream(/*DataSupervisor.ds.currentUserDataPath + */"not_sent", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            lock (pending)
            {
                temp.SetLength(0);
                temp.Flush();
                pending.Clear();
            }
            temp.Close();
        }
        private static void setConnected(bool val)
        {
            connected = val;
        }

        private static void setStream(NetworkStream s)
        {
            stream = s;
        }

        private static void setReader(StreamReader sr)
        {
            r = sr;
        }

        private static void setWriter(StreamWriter sw)
        {
            w = sw;
        }

        private static void saveToFile()
        {
            //TODO : encrypt file
            FileStream temp = new FileStream(/*DataSupervisor.ds.currentUserDataPath + */"not_sent", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            StreamWriter temp_write = new StreamWriter(temp);
            lock (pending)
            {
                if (pending.Count != 0)
                {
                    temp_write.Write(JsonConvert.SerializeObject(pending));
                    temp_write.Flush();
                }
            }
            temp_write.Close();
            temp.Close();
        }

        private static void loadFromFile()
        {
            //TODO : encrypt file
            // file in user directory
            FileStream temp = new FileStream(/*DataSupervisor.ds.currentUserDataPath + */"not_sent", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            StreamReader temp_read = new StreamReader(temp);
            temp.Seek(0, SeekOrigin.Begin);
            if (temp.Length != 0)
            {
                lock (pending)
                {

                    pending = JsonConvert.DeserializeObject<List<string>>(temp_read.ReadToEnd());
                }
            }
            temp_read.Close();
            temp.Close();
        }

        public static void sendData(Notification obj)
        {
            lock (pending)
            {
                pending.Add(obj.toString());
            }
        }

        public static void start()
        {
            if (started) return;
            loadFromFile();
            ThreadStart start = new ThreadStart(OtherThread.main);
            other_thread = new Thread(start);
            other_thread.Start();
            started = true;
        }
        public static void stop()
        {
            // Call this on logout or exit
            if (!started) return;
            if (other_thread.IsAlive)
                other_thread.Abort();
            saveToFile();
            started = false;
        }
    }
}
