using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;

namespace EloPedidos.Controllers
{
    public class MunicipioController
    {
        private MunicipioDAO DAO;

        public MunicipioController()
        {
            DAO = new MunicipioDAO();
        }

        public bool Save(Municipio municipio)
        {
            return DAO.Save(municipio);
        }

        /// <summary>
        /// Retorna a data da ultima atualizacao
        /// </summary>
        /// <returns></returns>
        /// 
        public DateTime GetLastDateTime()
        {
            DateTime DTHULTAT = DAO.GetLastDateTime().Max(m => m.DTHULTAT);
            return DTHULTAT;
        }


        public Municipio FindById(long pCODMUNIC)
        {
            return DAO.FindById(pCODMUNIC);
        }

        public IList<Municipio> FindAll()
        {
            return DAO.FindAll();
        }

        public IList<Municipio> FindByName(string name)
        {
            return DAO.FindByName(name);
        }
        public long? FindCODMUNIC(string name)
        {
            var MUNIC = DAO.FindByNOMMUNIC(name);
            return MUNIC.CODMUNIC;
        }

        public string FindNameById(long? pCODMUNIC)
        {
            return DAO.FindNameById(pCODMUNIC);
        }

        public Municipio FindByCODMUNGV(long cod)
        {
            var conn = Database.GetConnection();
            try
            {
                return conn.Table<Municipio>().ToList()
                    .Where(m => m.CODMUNGV == cod).FirstOrDefault();
            }
            catch (Exception ex)
            {
                string error = "";
                Log.Error(error, ex.ToString());
                return null;
            }
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

                            if (receiveMsg.Contains("\0\0"))
                                receiveMsg = receiveMsg.Split("\0\0")[0];

                            receiveMsg = receiveMsg.Replace("CARGAMUNICIPIO@@", "");
                            string[] lines = receiveMsg.Split("@@");

                            lines.ToList().ForEach((str) =>
                            {
                                if (str != lines[lines.Length - 1] && !str.StartsWith("FIMMUN"))
                                {
                                    string[] data = str.Split(';');

                                    Municipio m = new Municipio()
                                    {
                                        CODMUNIC = data[0].ToLong(),
                                        NOMMUNIC = data[1],
                                        CODUF = data[2],
                                        CODMUNGV = data[3].ToLong(),
                                        NROCEP = data[4].ToLong(),
                                        VLRLAT = data[5].ToDouble(),
                                        VLRLONG = data[6].ToDouble(),
                                        DTHULTAT = DateTime.Parse(data[7]),
                                        USRULTAT = data[8]
                                    };

                                    this.Save(m);

                                }
                                else
                                {
                                    if (str.StartsWith("FIMMUN"))
                                        loop = false;
                                    else
                                    {
                                        if (str.Contains("\0\0"))
                                            str = str.Split("\0\0")[0];

                                        if (!str.IsEmpty())
                                        {
                                            if (request.EndsWith(""))
                                            {
                                                string msg1 = "CARGAMUNICIPIO" + str;
                                                byte[] msgBytes = msg1.ToUTF8(true);
                                                netStream.Write(msgBytes, 0, msgBytes.Length);
                                            }
                                            else
                                            {
                                                string data = request.Substring(20, 10);
                                                string msg1 = "CARGAMUNICIPIO" + str + data;
                                                byte[] msgBytes = msg1.ToUTF8(true);
                                                netStream.Write(msgBytes, 0, msgBytes.Length);
                                            }
                                        }
                                    }
                                }
                            });
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