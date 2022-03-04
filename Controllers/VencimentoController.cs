using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Utils;
using EloPedidos.Models;
using EloPedidos.Persistence;

namespace EloPedidos.Controllers
{
    public class VencimentoController
    {
        private VencimentoDAO DAO;

        public VencimentoController()
        {
            DAO = new VencimentoDAO();
        }

        public bool Save(Vencimento vencimento)
        {
            return DAO.Save(vencimento);
        }
        public DateTime GetLastDateTime()
        {
            DateTime DTHULTAT = DAO.GetLastDatetime().Max(v => v.DTHULTAT);
            return DTHULTAT;
        }
        public List<Vencimento> GetVencimentos()
		{
            return DAO.FindAll();
		}

        public Vencimento FindByCG_PESSOA_ID(long pCG_PESSOA_ID)
        {
            return DAO.FindByCG_PESSOA_ID(pCG_PESSOA_ID);
        }

        public bool ComSocket(string request, string host, int port)
        {
            bool aux = false;

            Thread t = new Thread(() =>
            {
                TcpClient client = null;
                NetworkStream netStream = null;
                bool loop = true;

                try
                {
                    client = new TcpClient();
                    client.Connect(host, port);

                    byte[] msg = request.ToUTF8(true);
                    netStream = client.GetStream();
                    netStream.Write(msg, 0, msg.Length);

                    while (loop)
                        if (netStream.CanRead)
                        {
                            byte[] bytes = new byte[client.ReceiveBufferSize];
                            netStream.Read(bytes, 0, client.ReceiveBufferSize);
                            string receiveMsg = bytes.UTF7ToString();

                            receiveMsg = receiveMsg.Replace("CARGAPESSOAVCTO@@", "");

                            if (receiveMsg.Contains("\0\0"))
                                receiveMsg = receiveMsg.Split("\0\0")[0];

                            string[] lines = receiveMsg.Split("@@");

                            if (lines[0] == "FIMPVC")
                                loop = false;
                            else
                            {
                                lines.ToList().ForEach((str) => {
                                    if (str != lines[lines.Length - 1])
                                    {
                                        string[] data = str.Split(';');

                                        Vencimento v = new Vencimento()
                                        {
                                            CG_PESSOA_DIAS_VCTO_ID = data[0].ToLong(),
                                            CG_PESSOA_ID = data[1].ToLong(),
                                            QTDDVCTO = data[2].ToInt(),
                                            DTHULTAT = DateTime.Parse(data[3]),
                                            USRULTAT = data[4]
                                        };

                                        this.Save(v);
                                    }
                                    else
                                    {
                                        if (str.StartsWith("FIMPVC"))
                                            loop = false;
                                        else
                                        {
                                            string id = string.Format("{0:0000}", new VendedorController().GetVendedor().CG_VENDEDOR_ID);
                                            if (request.EndsWith($"CARGAPESSOAVCTO{new EmpresaController().GetEmpresa().CODEMPRE}{id}"))
                                            {
                                                byte[] bytesMsg = (request + str).ToUTF8(true);
                                                netStream.Write(bytesMsg, 0, bytesMsg.Length);
                                            }
                                            else
                                            {
                                                string data = request.Substring(27, 19);
                                                byte[] bytesMsg = $"CARGAPESSOAVCTO{new EmpresaController().GetEmpresa().CODEMPRE}{id}{str}{data}".ToUTF8(true);
                                                netStream.Write(bytesMsg, 0, bytesMsg.Length);
                                            }
                                        }
                                    }
                                });
                            }
                        }

                    aux = true;
                }
                catch (Exception ex)
                {

                    string error = "";
                    Log.Error(error, ex.ToString());
                    aux = false;
                }
                finally
                {
                    if (client.Connected) client.Close();
                    if (netStream != null) netStream.Close();
                }

            });

            t.Start();
            t.Join();

            return aux;
        }
        public bool SincVencimento(long pCG_PESSOA_ID, string host, int port)
        {
            bool result = false;

            Thread t = new Thread(() => 
            {

                TcpClient client = null;
                NetworkStream netStream = null;
                bool loop = true;

                try
                {
                    client = new TcpClient();
                    client.Connect(host, port);

                    netStream = client.GetStream();

                    Empresa empresa = new EmpresaController().GetEmpresa();

                    string strCG_PESSOA_DIAS_VCTO_ID = DAO.FindByCG_PESSOA_ID(pCG_PESSOA_ID).CG_PESSOA_DIAS_VCTO_ID.Value.ToString("D6");

                    string msg = $"CARGAPESSOAVCTO{empresa.CODEMPRE}{strCG_PESSOA_DIAS_VCTO_ID}";
                    byte[] bytes = msg.ToUTF8(true);
                    netStream.Write(bytes, 0, bytes.Length);

                    while (loop)
                        if (netStream.CanRead)
                        {
                            byte[] bytesMsg = new byte[client.ReceiveBufferSize];
                            netStream.Read(bytes, 0, client.ReceiveBufferSize);
                            string receiveMsg = bytes.UTF7ToString();

                            receiveMsg = receiveMsg.Replace("CARGAPESSOAVCTO@@", "");
                            string[] lines = receiveMsg.Split("@@");

                            lines.ToList().ForEach((str) => {
                                if (str != lines[lines.Length - 1])
                                {
                                    string[] data = str.Split(';');

                                    Vencimento v = new Vencimento()
                                    {
                                        CG_PESSOA_DIAS_VCTO_ID = long.TryParse(data[0], out long aux) ? aux : 0,
                                        CG_PESSOA_ID = long.TryParse(data[1], out long aux1) ? aux1 : 0,
                                        QTDDVCTO = int.TryParse(data[2], out int aux2) ? aux2 : 0,
                                        DTHULTAT = DateTime.Parse(data[3]),
                                        USRULTAT = data[4]
                                    };

                                    this.Save(v);
                                }
                                else
                                {
                                    if (str.StartsWith("FIMPES"))
                                        loop = false;
                                }
                            });
                        }

                    result = true;
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                    Log.Error(error, ex.ToString());
                    result = false;
                }
                finally
                {
                    if (client != null) client.Close();
                    if (netStream != null) netStream.Close();
                }

            });

            t.Start();
            t.Join();

            return result;
        }
    }
}