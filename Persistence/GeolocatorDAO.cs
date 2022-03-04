using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EloPedidos.Models;
using EloPedidos.Persistence;

namespace EloPedidos.Persistence
{
	public class GeolocatorDAO
	{
		public GeolocatorDAO()
		{
			Database.GetConnection().CreateTable<Geolocator>();
		}
		public Geolocator GetGeolocator()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Geolocator>()
					.FirstOrDefault();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public bool Save(Geolocator geolocator)
		{
			var conn = Database.GetConnection();

			try
			{
				if (geolocator.Id == null)
				{
					if (conn.Insert(geolocator) > 0)
						return true;
					else
						return false;
				}
				else
				{
					if (this.FindById(geolocator.Id.Value) != null)
						conn.Update(geolocator);

					return true;
				}
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return false;
			}
		}

		public List<Geolocator> FindAll()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Geolocator>().OrderBy(g => g.DTHLOC).ToList();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public bool AtualizarSincronizado(Geolocator g)
		{
			var conn = Database.GetConnection();
			try
			{
				g.INDENV = true;
				return this.Save(g);
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return false;
			}
		}


		/// <summary>
		///  Recupera todos os registros não sincronizados com o host remoto
		/// <returns></returns>
		/// /// </summary>
		public List<Geolocator> FindAllNotSync()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Geolocator>()
					.Where(p => !p.INDENV)
					.OrderBy(g => g.DTHLOC).ToList();
			}
			catch (Exception ex)
			{
				string error = "";
				Log.Error(error, ex.ToString());
				return null;
			}
		}

		public int NotSyncCount()
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Table<Geolocator>().Where(g => !g.INDENV).Count();
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return 0;
			}
		}

		public bool Delete(long id)
		{
			var conn = Database.GetConnection();
			try
			{
				return (conn.Delete<Geolocator>(id) > 0);
			}
			catch (Exception ex)
			{
				Log.Error("error", ex.ToString());
				return false;
			}
		}

		public Geolocator FindById(long Id)
		{
			var conn = Database.GetConnection();
			try
			{
				return conn.Find<Geolocator>(Id);
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
				return null;
			}
		}

		public Geolocator FindByDate(string date)
		{
			try
			{
				var lista = FindAll().Where(g => g.DTHLOC.ToString("dd/MM/yyyy HH:mm:ss").Equals(date)).ToList();
				if (lista.Count > 0)
					return lista.FirstOrDefault();
				else
					return null;
			}
			catch (Exception ex)
			{
				Log.Error("Error", ex.ToString());
				return null;
			}
		}
	}
}