using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Android.Content;
using Android.Util;
using EloPedidos.Utils;
using EloPedidos.Models;
using EloPedidos.Persistence;

namespace EloPedidos.Controllers
{
    public class ProdutoController
    {
        private ProdutoDAO DAO;

        public ProdutoController()
        {
            DAO = new ProdutoDAO();
        }

        public bool Save(Produto produto)
        {
            return DAO.Save(produto);
        }
        /// <summary>
        /// Retorna a data da ultima atualização
        /// </summary>
        /// <returns></returns>
        public DateTime GetLastDateTime()
        {
            DateTime DTHULTAT = DAO.GetLastDateTime().Max(p => p.DTHULTAT);
            return DTHULTAT;
        }

        public Produto FindById(long pCG_PRODUTO_ID)
        {
            return DAO.FindById(pCG_PRODUTO_ID);
        }

        public Produto FindByCODPROD(long pCODPROD)
        {
            return DAO.FindByCODPROD(pCODPROD);
        }

        public IList<Produto> FindAll()
        {
            return DAO.FindAll();
        }

        public IList<Produto> FindByDSCPROD(string pDSCPROD)
        {
            return DAO.FindByDSCPROD(pDSCPROD);
        }
        public IList<Produto> FindPROD()
		{
            return DAO.FindPROD();

        }

        public Produto FindProdutoByBC(string barcode)
        {
            var conn = Database.GetConnection();
            return conn.Table<Produto>().Where(p => p.CODEAN == barcode).FirstOrDefault();
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

                    netStream = client.GetStream();

                    byte[] msg = request.ToUTF8(true);
                    netStream.Write(msg, 0, msg.Length);

                    Empresa empresa = new EmpresaController().GetEmpresa();

                    bool loop = true;
                    string dados = string.Empty;
                    if (netStream.CanRead)
                        while (loop)
                        {
                            byte[] bytes = new byte[client.ReceiveBufferSize];
                            netStream.Read(bytes, 0, client.ReceiveBufferSize);
                            string receiveMsg = bytes.UTF7ToString();

                            if (receiveMsg.Contains("\0\0"))
                                receiveMsg = receiveMsg.Split("\0\0")[0];

                            receiveMsg = receiveMsg.Replace("CARGAPRODUTO@@", "");

                            if (receiveMsg.ToUpper().Contains("@ERRO"))
                                throw new Exception(receiveMsg.ToUpper());

                            if (receiveMsg.Contains("FIMPRO"))
                            {
                                dados = dados + receiveMsg;
                                loop = false;
                            }
                            else
                                dados = dados + receiveMsg;
                        }

                    dados = dados.Replace("@@FIMPRO", "");
                    dados = dados.Replace("FIMPRO@@", "");

                    if (!dados.Contains("@ERRO"))
                    {
                        string[] lines = dados.Split("@@");
                        foreach (var str in lines)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                string[] data = str.Split(';');

                                Produto p = new Produto()
                                {
                                    CG_PRODUTO_ID = data[0].ToLong(),
                                    CODEMPRE = data[1],
                                    CODPROD = data[2].ToLong(),
                                    DSCPROD = data[3],
                                    IDTUNID = data[4],
                                    CG_CLASSE_PRODUTO_ID = data[5].ToLong(),
                                    QTDUNID = data[6].ToDouble(),
                                    CODEAN = data[7],
                                    CODNCM = data[8].ToLong(),
                                    PRCCUSTO = data[9].ToDouble(),
                                    PRCVENDA = data[10].ToDouble(),
                                    PERDSESP = data[11].ToDouble(),
                                    PERVISTA = data[12].ToDouble(),
                                    INDINAT = data[13].ToBool(),
                                    DTHULTAT = DateTime.Parse(data[14]),
                                    USRULTAT = data[15]
                                };

                                this.Save(p);
                            }
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