using System;
using System.Linq;
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
    public class SequenciaController
    {
        public Sequencia Sequencia { get { return DAO.GetSequencia(); } }

        private SequenciaDAO DAO;

        public SequenciaController()
        {
            DAO = new SequenciaDAO();
        }


        /// <summary>
        /// Reotrna a data e hora da ultima atualização
        /// </summary>
        /// <returns></returns>
        public DateTime GetLastDateTime()
        {
            DateTime DTHULTAT = DAO.GetLastDateTime().Max(s => s.DTHULTAT);
            return DTHULTAT;
        }


        public bool Save(Sequencia sequencia)
        {
            var seq = Sequencia;

            return DAO.Save(sequencia);
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

                    if (netStream.CanRead)
                    {
                        byte[] bytesMsg = new byte[client.ReceiveBufferSize];
                        netStream.Read(bytesMsg, 0, client.ReceiveBufferSize);
                        string receiveMsg = bytesMsg.UTF7ToString();

                        if (receiveMsg.Contains("\0\0"))
                            receiveMsg = receiveMsg.Split("\0\0")[0];

                        receiveMsg = receiveMsg.Replace("CARGAVENDESEQ@@", "").Replace("@@FIMVSE", "");

                        if (!receiveMsg.Contains("ERRO"))
                        {

                            string[] data = receiveMsg.Split(';');
                            if (!data[0].Contains("FIMVSE"))
                            {
                                Sequencia s = new Sequencia
                                {
                                    CG_VENDEDOR_SEQ_PEDIDO_ID = data[0].ToLong(),
                                    CG_VENDEDOR_ID = data[1].ToLong(),
                                    NROPEDIN = data[2].ToLong(),
                                    NROPEDFI = data[3].ToLong(),
                                    NROPEDAT = data[4].ToLong(),
                                    DTHULTAT = DateTime.Parse(data[5]),
                                    USRULTAT = data[6]
                                };

                                //if (s.NROPEDAT == 0)

                                //    goto Error1;
                                //if ((seqAux = Sequencia) != null)
                                //    if (seqAux.NROPEDAT != s.NROPEDAT && seqAux.NROPEDIN == s.NROPEDIN)
                                //        goto Error2;

                                aux = this.Save(s);
                                goto Finish;
                            }
                            else
                                aux = true;
                                goto Finish;
                        }
                        else
                            aux = false;

                        //Error1:
                        //{
                        //    string message = "CARGAVENDESEQ@@NAO FOI INFORMADO NUMERO ATUAL";
                        //    byte[] bytes = message.ToUTF8(true);
                        //    netStream.Write(bytes, 0 , bytes.Length);
                        //    aux = false;
                        //    goto Finish;
                        //}
                        Error2:
                        {
                            //string message = $"CARGAVENDESEQ@@O NUMERO DO PEDIDO ATUAL INFORMADO ESTA INCORRETO! PEDIDO ATUAL: {Sequencia.NROPEDAT}";
                            //byte[] bytes = message.ToUTF8(true);
                            //netStream.Write(bytes, 0, bytes.Length);
                            //aux = false;
                            aux = true;
                            goto Finish;
                        }
                    Finish: { } // Somente para saltar o rótulo de erro
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
                    if (client.Connected) client.Close();
                    if (netStream != null) netStream.Close();
                }

            });

            t.Start();
            t.Join();

            return aux;
        }
    }
}