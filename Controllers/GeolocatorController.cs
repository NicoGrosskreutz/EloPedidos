using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Locations;
using Android.Util;
using EloPedidos.Models;
using EloPedidos.Persistence;
using EloPedidos.Utils;
using EloPedidos.Views;
using Plugin.Geolocator;
using Xamarin.Essentials;

namespace EloPedidos.Controllers
{
	public class GeolocatorController
	{
		public Models.Geolocator geolocator { get { return this.GetGeolocator(); } }
		public int NotSyncCount { get => DAO.NotSyncCount(); }

		private GeolocatorDAO DAO;

		public GeolocatorController()
		{
			DAO = new GeolocatorDAO();
		}
		public Geolocator GetGeolocator()
		{
			return DAO.GetGeolocator();
		}

		public bool Save(Geolocator geolocator) => DAO.Save(geolocator);

		public Geolocator FindById(long Id) => DAO.FindById(Id);

		/// <summary>
		/// Salva a localizacao do pedido
		/// </summary>
		/// <param name="pFT_PEDIDO_ID"></param>
		public async void SaveOrderLocalization(long pFT_PEDIDO_ID)
		{
			try
			{
				Geolocator g = await GetLocalization();
				g.INDENV = true;

				Save(g);

				var pedidoController = new PedidoController();
				Pedido p = pedidoController.FindById(pFT_PEDIDO_ID);

				p.LOCALIZACAO_ID = g.Id;

				pedidoController.Save(p);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
			}
		}

		/// <summary>
		/// Salva a localizaca da devolucao do pedido
		/// </summary>
		/// <param name="pFT_PEDIDO_ITEM_DEVOLUCAO_ID"></param>
		public async void SaveDevolutionLocalization(long pFT_PEDIDO_ITEM_DEVOLUCAO_ID)
		{
			try
			{
				Geolocator g = await GetLocalization();
				g.INDENV = true;

				Save(g);

				var devolucaoController = new DevolucaoItemController();
				DevolucaoItem devolucao = devolucaoController.FindById(pFT_PEDIDO_ITEM_DEVOLUCAO_ID);
				devolucao.LOCALIZACAO_ID = g.Id;

				devolucaoController.SaveItemDevolucao(devolucao);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
			}
		}

		/// <summary>
		/// Salva a localizacao do recebimento do pedido
		/// </summary>
		/// <param name="pFT_PEDIDO_BAIXA_ID"></param>
		public async void SaveReceivementLocalizationAsync(long pFT_PEDIDO_BAIXA_ID)
		{
			try
			{
				Geolocator g = await GetLocalization();
				g.INDENV = true;

				Save(g);

				var baixaController = new BaixasPedidoController();
				BaixasPedido baixa = baixaController.FindById(pFT_PEDIDO_BAIXA_ID);
				baixa.LOCALIZACAO_ID = g.Id;

				baixaController.SaveReceivement(baixa);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
			}
		}

		/// <summary>
		/// Retorna uma instância de um objeto com a localização do mesmo horário chamado (se possível)
		/// </summary>
		/// <returns></returns>
		public async Task<Geolocator> GetLocalization()
		{
			try
			{
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 50;
				if (locator.IsGeolocationEnabled == false)
				{
					return new Geolocator
					{
						Longitude = "",
						Latitude = "",
						NOMMUNIC = "",
						DTHLOC = DateTime.Now
					};
				}
				else
				{
					var position = await locator.GetPositionAsync(timeout: TimeSpan.FromMilliseconds(10000));

					for (int i = 0; i <= 5; i++)
					{
						if (position != null)
							break;

						if (i == 5)
							throw new Exception("Erro ao gravar localização do pedido!");

						Thread.Sleep(100);
					}

					string MUNIC = string.Empty;

					if (new NetworkController().TestConnection())
					{
						try
						{
							var placemarks = await Geocoding.GetPlacemarksAsync(position.Latitude, position.Longitude);

							Placemark placemark = placemarks?.FirstOrDefault();
							MUNIC = placemark.SubAdminArea;
						}
						catch(Exception e)
                        {
							Log.Error("placemarks", e.ToString());
                        }
					}

					var config = new Models.Config();
					string CODMUNIC = new MunicipioController().FindCODMUNIC(MUNIC).ToString();
					ConfigController cController = new ConfigController();
					cController.Update(1, int.Parse(CODMUNIC), MUNIC);

					return new Geolocator
					{
						Longitude = position.Longitude.ToString().Replace(",", "."),
						Latitude = position.Latitude.ToString().Replace(",", "."),
						NOMMUNIC = MUNIC.ToString(),
						DTHLOC = DateTime.Now
					};
				}
			}
			catch (Exception e)
			{
				Log.Error("GetLocalization", e.ToString());
				return new Geolocator
				{
					Longitude = "",
					Latitude = "",
					NOMMUNIC = "",
					DTHLOC = DateTime.Now
				};
			}
		}


		public bool Delete(long id) => DAO.Delete(id);

		public bool AtualizarSincronizado(Geolocator g) => DAO.AtualizarSincronizado(g);


		public List<Geolocator> FindAll() => DAO.FindAll();

		public List<Geolocator> FindAllNotSync() => DAO.FindAllNotSync();

		public Geolocator GetLastLocalization()
		{
			try
			{
				return FindAll().Last();
			}
			catch (ArgumentNullException)
			{
				return null;
			}
		}

		public DateTime? GetLastDate()
		{
			try
			{
				return GetLastLocalization().DTHLOC;
			}
			catch
			{
				return null;
			}
		}
		public Models.Geolocator FindByDate(string data) => DAO.FindByDate(data);
		public DateTime? GetMaxDate()
		{
			try
			{
				return FindAll().Max(g => g.DTHLOC);
			}
			catch
			{
				return null;
			}
		}

		public bool ComSocket(Geolocator location, string host, int port)
		{
			bool result = true;

			TcpClient client = null;
			NetworkStream stream = null;

			Thread t = new Thread(() =>
			{
				try
				{
					var gController = new GeolocatorController();
					if (!string.IsNullOrEmpty(location.Latitude) && !string.IsNullOrEmpty(location.Longitude))
					{
						client = new TcpClient(host, port);
						stream = client.GetStream();

						StringBuilder builder = new StringBuilder();

						Operador o = new OperadorController().GetOperador();

						string codempre = new EmpresaDAO().GetEmpresa().CODEMPRE;
						string idvendedor = string.Format("{0:0000}", new VendedorDAO().GetVendedor(o).CG_VENDEDOR_ID);

						builder.Append("CARGALOCALIZACAO")
								   .Append(codempre)
								   .Append(";")
								   .Append(idvendedor)
								   .Append(";")
								   .Append(string.Format("{0:000000}", location.Id))
								   .Append(";")
								   .Append(location.Latitude.Replace(".", ","))
								   .Append(";")
								   .Append(location.Longitude.Replace(".", ","))
								   .Append(";")
								   .Append(location.DTHLOC.ToString("dd/MM/yyyy HH:mm:ss"))
								   .Append("@@FIM");

						string str = builder.ToString();

						builder.Clear();

						byte[] bytes = str.ToUTF8(true);
						stream.Write(bytes, 0, bytes.Length);

						if (stream.CanRead)
						{
							result = true;

							byte[] received = new byte[client.ReceiveBufferSize];
							stream.Read(received, 0, client.ReceiveBufferSize);
							string receivedStr = received.UTF7ToString();

							if (receivedStr.Contains("\0\0"))
								receivedStr = receivedStr.Split("\0\0")[0];

							if (receivedStr.ToLower().Equals("cargalocalizacao@@ok@@"))
								gController.AtualizarSincronizado(location);
							else if (receivedStr.ToLower().Equals("cargalocalizacao@@movimento ja gravado@@"))
								gController.AtualizarSincronizado(location);
							else if (receivedStr.ToLower().Contains("naoenviarrota"))
								new ConfigController().DisableENVLOC();
							else
							{
								gController.Delete(location.Id.Value);
								result = false;
							}
						}
					}
					else
					{
						gController.Delete(location.Id.Value);
						result = false;
					}
				}
				catch (Exception ex)
				{
					Log.Error("gwork", ex.ToString());
					result = false;
				}
				finally
				{
					if (stream != null) stream.Dispose();
					if (client != null) client.Close();
				}
			});

			t.Start();
			t.Join();

			return result;
		}

		/// <summary>
		/// Retorna latitude,longitude,cidade
		/// </summary>
		/// <returns></returns>
		public string GetLocalizationString(long id)
		{
			Geolocator g = null;
			if ((g = FindById(id)) != null)
			{
				return $"{g.Latitude},{g.Longitude}, {g.NOMMUNIC} ";
			}
			else
				return string.Empty;
		}
		/// <summary>
		/// Retorna latitude,longitude,cidade
		/// </summary>
		/// <returns></returns>
		public string GetLocalizationStringLATLONG(long id)
		{
			Geolocator g = null;
			if ((g = FindById(id)) != null)
			{
				return $"{g.Latitude},{g.Longitude}";
			}
			else
				return string.Empty;
		}

		/// <summary>
		/// Retorna a data e hora da localizacao
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public string GetLocalizationDateString(long id)
		{
			Geolocator g = null;
			if ((g = FindById(id)) != null)
			{
				return g.DTHLOC.ToString("dd/MM/yyyy HH:mm:ss");
			}
			else
				return string.Empty;
		}

		public async Task<Geolocator> getLocationAsync()
		{
			Geolocator geolocator;
			try
			{
				Criteria locationCriteria = new Criteria();
				locationCriteria.Accuracy = Accuracy.Coarse;
				locationCriteria.PowerRequirement = Power.Medium;

				LocationManager locationManager = Android.App.Application.Context.GetSystemService(Android.App.Application.LocationService) as LocationManager;
				string bestProvider = locationManager.GetBestProvider(locationCriteria, true);
				Android.Locations.Location location = locationManager.GetLastKnownLocation(bestProvider);
				if (location != null)
				{
					var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

					Placemark placemark = placemarks?.FirstOrDefault();
					string nome = placemark.SubAdminArea;

					var config = new Models.Config();
					string CODMUNIC = new MunicipioController().FindCODMUNIC(nome).ToString();
					ConfigController cController = new ConfigController();
					cController.Update(1, int.Parse(CODMUNIC), nome);

					geolocator = new Geolocator();
					geolocator.Longitude = location.Longitude.ToString().Replace(",", ".");
					geolocator.Latitude = location.Latitude.ToString().Replace(",", ".");
					geolocator.NOMMUNIC = nome;
					geolocator.DTHLOC = DateTime.Now;
				}
				else
				{
					geolocator = new Geolocator();
					geolocator.Longitude = "";
					geolocator.Latitude = "";
					geolocator.NOMMUNIC = "";
				}
			}
			catch
			{
				geolocator = new Geolocator();
				geolocator.Longitude = "";
				geolocator.Latitude = "";
				geolocator.NOMMUNIC = "";
			}
			return geolocator;
		}
	}
}