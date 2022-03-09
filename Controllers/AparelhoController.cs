using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using EloPedidos.Persistence;
using EloPedidos.Models;
using System.Threading;
using System.Net.Sockets;
using Android.Util;
using EloPedidos.Utils;

namespace EloPedidos.Controllers
{
	public class AparelhoController
	{
		public AparelhoController()
		{
			Database.GetConnection().CreateTable<Aparelho>();
		}
		public bool Insert(Aparelho a)
		{
			var conn = Database.GetConnection();
			if (conn.Insert(a) > 0)
			{
				return true;
			}
			else
				return false;
		}
		public bool Update(Aparelho a)
		{
			var conn = Database.GetConnection();
			if (conn.Update(a) > 0)
				return true;
			else
				return false;
		}
		public List<Aparelho> FindAll()
		{
			var conn = Database.GetConnection();
			return conn.Table<Aparelho>().ToList();
		}
		public Aparelho GetAparelho()
		{
			return FindAll().FirstOrDefault();
		}
		public bool syncDevice(Aparelho aparelho, out string message)
		{
			bool result = false;
			string outStr = string.Empty;

			Thread t = new Thread(() =>
			{
				TcpClient client = null;
				NetworkStream netStream = null;
				StringBuilder builder = null;

				try
				{
					client = new TcpClient();
					client.Connect("elosoftware.dyndns.org", 8560);
					//client.Connect("192.168.0.78", 8560);

					netStream = client.GetStream();
					netStream.ReadTimeout = 5000;

					builder = new StringBuilder();

					builder.Append("MANUTENIRAPARELHO")
					.Append(aparelho.DSCAPAR)
					.Append(";")
					.Append(aparelho.NOMOPER)
					.Append(";")
					.Append(aparelho.IDTPESS)
					.Append(";")
					.Append(aparelho.NROVERS)
					.Append(";")
					.Append(aparelho.TIPSAPAR)
					.Append(";")
					.Append(aparelho.INDINAT)
					.Append(";")
					.Append(aparelho.DTHULTAT.ToString("dd/MM/yyyy HH:mm:ss"))
					.Append(";")
					.Append(aparelho.USRULTAT);

					byte[] msg = builder.ToString().ToUTF8(true);
					netStream.Write(msg, 0, msg.Length);

					if (netStream.CanRead)
					{
						byte[] bytes = new byte[client.ReceiveBufferSize];
						netStream.Read(bytes, 0, client.ReceiveBufferSize);
						string received = bytes.UTF7ToString();

						if (received.Contains("\0\0"))
							received = received.Split("\0\0")[0];

						/* Caso o pedido retorne Ok */
						if (received.ToUpper().Contains("MANUTENIROK"))
						{
							aparelho.INDSYNC = true;
							this.Update(aparelho);
							result = true;
						}
						else if (received.ToUpper().Contains("APARELHO JÁ CADASTRADO"))
						{
							aparelho.INDSYNC = true;
							this.Update(aparelho);
							result = true;
						}
						else
							result = true;
					}

				}
				catch (Exception e)
				{
					Log.Error("SyncError", e.ToString());

					if (e.Message.ToUpper().StartsWith("UNABLE TO READ"))
						outStr = "UNABLE TO READ";
					else if (e.Message.ToUpper().Contains("NETWORK"))
						outStr = "UNABLE TO READ";

					result = true;
				}
				finally
				{
					if (client != null) client.Close();
					if (netStream != null) netStream.Close();
				}
			});
			t.Start();
			t.Join();

			message = outStr;
			return result;
		}
	}
}