using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Widget;
using EloPedidos.Utils;
using EloPedidos.Models;
using EloPedidos.Persistence;
using System.Linq;
using System.Collections.Generic;

namespace EloPedidos.Controllers
{
    public class VendedorController
    {
        public Vendedor Vendedor { get { return this.GetVendedor(); } }
        private VendedorDAO DAO;

        public VendedorController()
        {
            DAO = new VendedorDAO();
        }

        public bool Save(Vendedor vendedor)
        {
            return DAO.Save(vendedor);
        }

        public List<Vendedor> FindAll() => DAO.FindAll();

        public Vendedor GetVendedor()
        {
            Operador o = new OperadorController().GetOperador();
            return DAO.GetVendedor(o);
        }

        public bool ComSocket(string request, string host, int port)
        {
            bool aux = SocketConnection(request, host, port);

            return aux;
        }

        public bool SocketConnection(string request, string host, int port)
        {
            TcpClient client = null;
            NetworkStream netStream = null;

            bool result = true;


            try
            {
                client = new TcpClient();
                client.Connect(host, port);
                netStream = client.GetStream();

                byte[] msg = request.ToUTF8(true);
                netStream.Write(msg, 0, msg.Length);

                string dados = string.Empty;

                bool loop = true;
                while (loop)
                    if (netStream.CanRead)
                    {
                        byte[] bytes = new byte[client.ReceiveBufferSize];

                        netStream.Read(bytes, 0, bytes.Length);
                        string receiveMsg = bytes.UTF7ToString();

                        if (receiveMsg.Contains("\0\0"))
                            receiveMsg = receiveMsg.Split("\0\0")[0];

                        if (!receiveMsg.EndsWith("FIMVEN"))
                            dados = dados + receiveMsg.Replace("CARGAVENDEDOR@@", "");
                        else
                        {
                            dados = dados + receiveMsg.Replace("CARGAVENDEDOR@@", "");
                            loop = false;
                        }
                    }

                dados = dados.Replace("@@FIMVEN", "");
                //dados.Replace("@@FIMVEN", "");
                string[] lines = dados.Split("#");
                if (lines.Length > 0)
                {
                    lines.ToList().ForEach((str) =>
                    {
                        if (!str.Contains("FIMVEN"))
                        {
                            string[] data = str.Split(';');

                            Vendedor v = new Vendedor()
                            {
                                CG_VENDEDOR_ID = data[0].ToLong(),
                                CODEMPRE = data[1],
                                CODVEND = data[2].ToLong(),
                                NOMVEND = data[3],
                                USROPER = data[4],
                                ES_ESTOQUE_LOCAL_ID = data[5].ToLong(),
                                NROTLFN = data[6],
                                DTHULTAT = DateTime.Parse(data[7]),
                                USRULTAT = data[8]
                            };

                            this.Save(v);
                        }
                    });
                }

                return result;
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Ext.LOG_APP, ex.ToString()); ;
                return false;
            }
            finally
            {
                if (client != null) client.Close();
                if (netStream != null) netStream.Close();
            }
        }

        private void GetError(string msg)
        {
            string error = "";
            Log.Error(error, msg);
        }

        public string getRotas(string request, string host, int port)
        {
            TcpClient client = null;
            NetworkStream netStream = null;

            string rotas = string.Empty;

            try
            {
                client = new TcpClient();
                client.Connect(host, port);
                netStream = client.GetStream();

                byte[] msg = request.ToUTF8(true);
                netStream.Write(msg, 0, msg.Length);

                string dados = string.Empty;
                bool loop = true;
                while (loop)
                    if (netStream.CanRead)
                    {
                        byte[] bytes = new byte[client.ReceiveBufferSize];

                        netStream.Read(bytes, 0, bytes.Length);
                        string receiveMsg = bytes.UTF8ToString();

                        if (receiveMsg.Contains("\0\0"))
                            receiveMsg = receiveMsg.Split("\0\0")[0];

                        if (!receiveMsg.EndsWith("@@FIMROTAVENDEDOR"))
                        {
                            string receive = receiveMsg.Replace("CONSULTARROTAVENDEDOR", "");

                            string[] aux = receive.Split("#");

                            aux.ToList().ForEach(l =>
                            {
                                dados = dados + l.Split(";")[1] + ";" + l.Split(";")[2] + "#";
                            });

                        }
                        else
                        {
                            string receive = receiveMsg.Replace("CONSULTARROTAVENDEDOR", "");

                            string[] aux = receive.Split("#");

                            aux.ToList().ForEach(l =>
                            {
                                if (!l.Contains("FIMROTAVENDEDOR"))
                                {
                                    if (l != aux[aux.Length - 2])
                                        dados = dados + l.Split(";")[1] + ";" + l.Split(";")[2] + "#";
                                    else
                                        dados = dados + l.Split(";")[1] + ";" + l.Split(";")[2];
                                }

                            });

                            loop = false;
                        }
                    }

                dados.Replace("FIMROTAVENDEDOR", "");

                rotas = dados;

                return rotas;
            }
            catch (Exception ex)
            {
                Log.Error(Utils.Ext.LOG_APP, ex.ToString()); ;
                return rotas;
            }
            finally
            {
                if (client != null) client.Close();
                if (netStream != null) netStream.Close();
            }
        }
    }
}