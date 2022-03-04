using System;
using Android.App;
using Android.Widget;
using M = EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Models;
using System.Net.Sockets;
using Xamarin.Essentials;
using System.Threading;
using EloPedidos.Utils;
using Android.Util;
using Plugin.DeviceInfo;
using System.Net;
using System.Collections.Generic;

namespace EloPedidos.Controllers
{
    public class ConfigController
    {
        public Models.Config Config { get { return this.GetConfig(); } }

        private ConfigDAO DAO;

        public ConfigController()
        {
            DAO = new ConfigDAO();
        }

        public bool Save(M.Config config)
        {
            if (DAO.Save(config))
            {
                config = GetConfig();
                return true;
            }
            else
                return false;
        }
        public bool Update(long CONFIG_ID, int COD, string name)
        {
            return DAO.Update(CONFIG_ID, COD, name);
        }

        public M.Config GetConfig()
        {
            return DAO.GetConfig();
        }


        public bool DisableENVLOC()
        {
            M.Config config = this.GetConfig();

            if (config != null)
            {
                config.ENVLOC = false;
                config.GRVLOC = true;
                config.INDDESAT = false;
                return this.Save(config);
            }
            else
                return false;
        }

        /// <summary>
        ///  Retorna o DNS atual baseado na configuração Interno/Externo
        /// </summary>
        /// <returns></returns>
        public DNS GetDNS()
        {
            try
            {
                var config = this.Config;

                DNS dns = new DNS();
                dns.Host = config.INDDNS ? config.DNSEXT.Split(':')[0] : config.DNSINT.Split(':')[0];
                dns.Port = int.Parse(config.INDDNS ? config.DNSEXT.Split(':')[1] : config.DNSINT.Split(':')[1]);
                dns.DNSInfo = config.INDDNS ? DNS.IndDNS.DNSExterno : DNS.IndDNS.DNSInterno;

                return dns;
            }
            catch (Exception ex)
            {
                Toast.MakeText(Application.Context, ex.ToString(), ToastLength.Short).Show();
                return null;
            }
        }

        /// <summary>
        ///  Verifica se é possível conectar ao servidor (usando configurações já salvas no sistema)
        /// </summary>
        /// <returns></returns>
        public bool TestServerConnection()
        {
            try
            {
                //DNS dns = GetDNS();

                //TcpClient client = new TcpClient();
                //var connection = client.BeginConnect(dns.Host, dns.Port, null, null);
                //var isConnected = connection.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                //if (!isConnected)
                //    return false;
                //else
                //    return true;

                //IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                //TcpConnectionInformation[] tcpConnections = iPGlobalProperties.GetActiveTcpConnections();
                //foreach (var connection in tcpConnections)
                //{
                //    var internalAdress = connection.LocalEndPoint.Address;
                //    var internalPort = connection.LocalEndPoint.Port;

                //    var externalAdress = connection.RemoteEndPoint.Address;
                //    var externalPort = connection.RemoteEndPoint.Port;
                //}

                DNS dns = GetDNS();

                bool result = false;

                IPAddress ipa = (IPAddress)Dns.GetHostAddresses(dns.Host)[0];
                TcpClient client = new TcpClient();
                client.ConnectAsync(ipa, dns.Port);

                for (int i = 0; i <= 2; i++)
                {
                    if (client.Connected)
                    {
                        client.Close();
                        result = true;
                        break;
                    }
                    Thread.Sleep(1000);
                }

                return result;

            }
            catch (Exception e)
            {
                Log.Error("", e.ToString());
                return false;
            }
        }

        /// <summary>
        ///  Testa conexão com servidor a partir de parâmetros host e port
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool TestServerConnection(string host, int port)
        {
            try
            {
                //TcpClient client = new TcpClient();
                //var connection = client.BeginConnect(host, port, null, null);
                //var isConnected = connection.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(1));

                //if (!isConnected)
                //    return false;
                //else
                //    return true;

                bool result = false;

                IPAddress ipa = (IPAddress)Dns.GetHostAddresses(host)[0];
                TcpClient client = new TcpClient();
                client.ConnectAsync(ipa, port);

                for (int i = 0; i <= 2; i++)
                {
                    if (client.Connected)
                    {
                        client.Close();
                        result = true;
                        break;
                    }
                    Thread.Sleep(1000);
                }

                return result;

            }
            catch (Exception e)
            {
                Log.Error("", e.ToString());
                return false;
            }
        }

        public bool applicationPermission(out string error)
        {
            bool result = false;
            string strMsg = string.Empty;

            Aparelho aparelho = new AparelhoController().GetAparelho();

            string str = $"CONSULTARAPARELHO{aparelho.DSCAPAR};{aparelho.NOMOPER};{aparelho.IDTPESS}";

            TcpClient client = null;
            NetworkStream netStream = null;

            Thread t = new Thread(() =>
            {
                try
                {
                    client = new TcpClient();
                    client.Connect("elosoftware.dyndns.org", 8560);

                    byte[] msg = str.ToUTF8(true);

                    netStream = client.GetStream();
                    netStream.Write(msg, 0, msg.Length);

                    if (netStream.CanRead)
                    {
                        byte[] bytes = new byte[client.ReceiveBufferSize];
                        netStream.Read(bytes, 0, client.ReceiveBufferSize);
                        string receiveMsg = bytes.UTF7ToString();

                        if (receiveMsg.Contains("\0\0"))
                            receiveMsg = receiveMsg.Split("\0\0")[0];

                        // lógica para receber resposta do sistema
                        receiveMsg = receiveMsg.Replace("CONSULTARAPARELHO@@", "").Replace("@@", "#").Replace(",", "#");

                        while (receiveMsg.Contains("##"))
                            receiveMsg = receiveMsg.Replace("##", "# #");

                        string[] data = receiveMsg.Split("#");

                        if (data[1].ToUpper().StartsWith("APARELHOOK"))
                            result = true;
                        else if (data[0].ToUpper().StartsWith("APARELHO NÃO ENCONTRADO"))
                        {
                            strMsg = "APARELHO NÃO ENCONTRADO";
                            result = false;
                        }
                        else if (data[0].ToUpper().StartsWith("APARELHO ESTÁ INATIVO"))
                        {
                            strMsg = "APARELHO ESTÁ INATIVO";
                            result = false;
                        }
                        else if (data[0].ToUpper().StartsWith("APARELHO INATIVO NO SISTEMA"))
                        {
                            strMsg = "APARELHO INATIVO NO SISTEMA";
                            result = false;
                        }
                        else if (data[0].ToUpper().StartsWith("LICENÇA PARA USO EXPIRADA"))
                        {
                            strMsg = "LICENÇA PARA USO EXPIRADA";
                            result = false;
                        }
                        else
                            result = false;
                    }
                }
                catch (Exception ex)
                {
                    string error = "";
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

            error = strMsg;
            return result;
        }
    }
}