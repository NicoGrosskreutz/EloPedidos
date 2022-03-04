using System;
using System.IO;
using Android.App;
using Android.Util;
using Android.Widget;
using SQLite;
using EloPedidos.Models;
using M = EloPedidos.Models;
using PCLExt.FileStorage.Folders;
using Java.Sql;
using Java.Lang;
using MongoDB.Driver;
using System.Linq.Expressions;
using System.Collections.Generic;
using MongoDB.Bson;

namespace EloPedidos.Persistence
{
	public static class Database
	{
		private static SQLiteConnection conn = null;

		public static void ResetDatabase()
		{
			var conn = GetConnection();

			conn.DropTable<Aparelho>();
			conn.DropTable<Pagamento>();
			conn.DropTable<BaixasPedido>();
			conn.DropTable<Models.Config>();
			conn.DropTable<DevolucaoItem>();
			conn.DropTable<Empresa>();
			conn.DropTable<Geolocator>();
			conn.DropTable<ItemPedido>();
			conn.DropTable<Municipio>();
			conn.DropTable<Operador>();
			conn.DropTable<Pedido>();
			conn.DropTable<Pessoa>();
			conn.DropTable<Produto>();
			conn.DropTable<Sequencia>();
			conn.DropTable<Vencimento>();
			conn.DropTable<Vendedor>();
			conn.DropTable<Romaneio>();
			conn.DropTable<RomaneioItem>();
		}


		public static SQLiteConnection GetConnection()
		{
			try
			{
				if (conn == null)
				{
					string dbPath = Path.Combine(
						System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Dados.db3");

					//var file = Android.OS.Environment.ExternalStorageDirectory + Java.IO.File.Separator + "EloSoftware" + Java.IO.File.Separator + "Dados" + Java.IO.File.Separator + "Dados.db3";

					conn = new SQLiteConnection(dbPath);
					conn.Execute("PRAGMA encoding = 'UTF-8'");
				}
				return conn;
			}
			catch (System.Exception ex)
			{
				Log.Error("error", ex.ToString());
				return GetConnection();
			}
		}

		/// <summary>
		/// Conexão/exemplo Bacno SQL Server
		/// </summary>
		/// <returns></returns>
		//public static IConnection SQlConnection()
		//{
		//	IConnection con = null;
		//	string url = "jdbc:jtds:sqlserver://elosoftware.dyndns.org:8560";
		//	string user = "Nicolas";
		//	string password = "123456";

		//	Class.ForName("net.sourceforge.jtds.jdbc.Driver");
		//	con = DriverManager.GetConnection(url, user, password); // CONEXAO COM O BANCO

		//	/// Consultar dados no banco
		//	IStatement statement = con.CreateStatement();
		//	IResultSet resultSet = statement.ExecuteQuery("SELECT * FROM CG_PRODUTO");
		//	while (resultSet.Next())
		//	{
		//		string result = resultSet.GetString(1);
		//	}
		//	resultSet.Close();
		//	statement.Close();

		//	/// Inserir dados no banco
		//	IPreparedStatement preparedStatement = con.PrepareStatement("INSET INTO CG_PRODUO (DSCPROD) VALUES (NOMEPRODUTO)", Statement.ReturnGeneratedKeys);
		//	preparedStatement.Execute();
		//	resultSet = preparedStatement.GeneratedKeys;
		//	while (resultSet.Next())
		//	{
		//		string result = resultSet.GetString(1);
		//	}

		//	return con;
		//}

		/// <summary>
		/// Conexão/exemplo Bacno MongoDB
		/// </summary>
		/// <returns></returns>
		//public static IMongoDatabase GetMongoDB()
		//{
		//    IMongoClient client = new MongoClient(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)); //Seleciona/Cria o diretório do banco
		//    IMongoDatabase database = client.GetDatabase("Dados.mdb"); //Seleciona/Cria o banco


		//    IMongoCollection<Pessoa> colNews = database.GetCollection<Pessoa>("pessoa"); //Seleciona/Cria a tabela

		//    colNews.InsertOne(new Pessoa()); //Insere um registro

		//    //Exclui um registro
		//    FilterDefinition<Pessoa> filterDefinition = "{ CG_PESSOA_ID: 1 }";
		//    colNews.DeleteOne(filterDefinition);

		//    Expression<Func<Pessoa, bool>> filter = x => x.CG_PESSOA_ID.Value.Equals(10);
		//    Pessoa pessoa = colNews.Find(filter).FirstOrDefault(); //Consulta um registro no banco


		//    return database;
		//}



		public static void RunInTransaction(Action action)
			=> GetConnection().RunInTransaction(() => action());

	}
}