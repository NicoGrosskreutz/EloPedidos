using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Android.Content;
using Android.Util;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;

namespace EloPedidos.Controllers
{
    public class OperadorController
    {
        public Operador Operador { get { return this.GetOperador(); } }
        private OperadorDAO DAO;

        public OperadorController()
        {
            DAO = new OperadorDAO();
        }

        public bool Save(Operador operador)
        {
            return DAO.Save(operador);
        }
        public Operador GetOperador()
        {
            return DAO.GetOperador();
        }

        public bool ComSocket(string request, string host, int port)
        {
            bool aux = false;

            Thread t = new Thread(() => 
            {

                TcpClient client = null;
                NetworkStream netStream = null;

                try
                {
                    client = new TcpClient();
                    client.Connect(host, port);

                    byte[] msg = request.ToUTF8(true);

                    netStream = client.GetStream();
                    netStream.Write(msg, 0, msg.Length);

                    if (netStream.CanRead)
                    {
                        byte[] bytes = new byte[client.ReceiveBufferSize];
                        netStream.Read(bytes, 0, client.ReceiveBufferSize);
                        string receiveMsg = bytes.UTF7ToString();

                        receiveMsg = receiveMsg.Replace("CARGAOPERADOR@@", "").Replace("@@FIMOPE", "");

                        if (receiveMsg.Contains("\0\0"))
                            receiveMsg = receiveMsg.Split("\0\0")[0];

                        if (!receiveMsg.Contains("ERRO"))
                        {
                            string[] data = receiveMsg.Split(';');

                            Operador o = new Operador()
                            {
                                USROPER = data[0],
                                NOMOPER = data[1],
                                DSCFUNC = data[2],
                                SNHOPER = data[3],
                                DSCEMAIL = data[4],
                                DSCSENHA = data[5],
                                DTHULTAT = data[6],
                                USRULTAL = data[7]
                            };

                            aux = this.Save(o);
                        }
                        else
                            aux = false;
                    }
                }
                catch (Exception ex)
                {
                    string error = "";
                    Log.Error(error, ex.ToString());
                    aux = false;
                }
                finally
                {
                    if (client != null) client.Close();
                    if (netStream != null) netStream.Close();
                }

            });

            t.Start();
            t.Join();

            return aux;
        }
    }
}