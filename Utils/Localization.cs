using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Util;
using EloPedidos.Controllers;
using EloPedidos.Models;
using Plugin.Geolocator;

namespace EloPedidos.Utils
{
	public class Localization
	{
		/// <summary>
		///  Método assíncrono responsável por envio de dados para o servidor
		/// </summary>
		/// <returns></returns>
		public async Task<bool> GetLocalizationAsync()
		{
			Log.Debug("gwork", $"SINCRONIZANDO DADOS COM SERVIDOR ===> OK _ {DateTime.Now.ToString("HH:mm:ss")}");
			
			try
			{
				var config = new ConfigController().GetConfig();
				var gController = new GeolocatorController();

				/* Indica se o serviço está desativado */
				if (config.INDDESAT)
					return false;

				var date = gController.GetMaxDate();
				 
				Geolocator geolocator = await gController.GetLocalization();

				if (geolocator.Latitude != null && geolocator.Longitude != null)
				{
					if (geolocator.Latitude != "" && geolocator.Longitude != "")
					{
						Geolocator aux = gController.FindByDate(geolocator.DTHLOC.ToString("dd/MM/yyyy HH:mm:ss"));

						if(aux == null)
							if (gController.Save(geolocator))
								Log.Debug("gservice", "LOCALIZAÇÃO SALVA COM SUCESSO");

						if(config.ENVLOC)
							enviarDados();
						return true;
					}
					else
					{
						if (config.ENVLOC)
							enviarDados();
						return false;
					}
				}
				else
				{
					if (config.ENVLOC)
						enviarDados();
					return false;
				}
			}
			catch (Exception ex)
			{
				Log.Error("gwork", ex.ToString());
				return false;
			}
		}

		private void enviarDados()
		{
			try
			{
				GeolocatorController gController = new GeolocatorController();
				DNS dns = new ConfigController().GetDNS();
				if (new ConfigController().TestServerConnection())
				{
					bool loop = true;

					var lista = gController.FindAllNotSync();

					foreach (var l in lista)
					{
						Thread.Sleep(500);
						if (!l.INDENV)
						{
							if (gController.ComSocket(l, dns.Host, dns.Port))
								Log.Debug("GEOLOCATOR", $"LOCALIZAÇÃO DE {l.DTHLOC} FOI SINCRONIZADA");
						}
					}
					Log.Debug("gservice", "DADOS ENVIADOS");
				}
			}
			catch(Exception ex)
			{
				SisLog.Logger(ex.ToString(), "Geolocation");
			}
		}
	}
}