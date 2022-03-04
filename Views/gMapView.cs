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
using Xamarin.Essentials;
using EloPedidos.Models;
using EloPedidos.Controllers;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Mapsui.Geometries;
using Mapsui.Utilities;
using Mapsui.Widgets.Zoom;
using Mapsui;
using Mapsui.Projection;
using Mapsui.Providers;
using Mapsui.Styles;
using Mapsui.Layers;
using Mapsui.UI.Android;
using EloPedidos.Utils;
using Mapsui.Widgets.ScaleBar;
using Mapsui.Widgets;
using Plugin.Geolocator.Abstractions;
using Android.Util;

namespace EloPedidos.Views
{
	[Activity(Label = "MapView")]
	public class gMapView : Activity
	{
		List<Point> point;
		private MapControl mapcontrol;

		List<Point> location = null;

		List<Point> pointsOrders = null;

		Point firstPosition = null;

		private GeolocatorController gController;
		protected override void OnCreate(Bundle savedInstanceState)
		{

			base.OnCreate(savedInstanceState);
			SetContentView(Resource.Layout.activity_map);
			mapcontrol = FindViewById<MapControl>(Resource.Id.mapcontrol);

			if (Intent.HasExtra("location"))
			{
				location = new List<Point>();
				pointsOrders = new List<Point>();

				string loc = Intent.GetStringExtra("location");

				//string rotas = loc.Split("pedidos = ")[0];
				string rotas = loc;
				//string pedidos = loc.Split("pedidos = ")[1];	

				bool firstLoop = true;

				foreach (var l in rotas.Split("#"))
				{
					try
					{
						double lat = double.Parse(l.Split(";")[0]);
						double lon = double.Parse(l.Split(";")[1]);

						location.Add(SphericalMercator.FromLonLat(lon, lat));

						if (firstLoop)
							firstPosition = SphericalMercator.FromLonLat(lon, lat);

						firstLoop = false;
					}
					catch(Exception e)
					{
						Log.Error(Utils.Ext.LOG_APP, e.Message);
					}
				}

				//foreach (var l in pedidos.Split("&"))
				//{
				//	double lat = double.Parse(l.Split(";")[0].Replace(".", ","));
				//	double lon = double.Parse(l.Split(";")[1].Replace(".", ","));

				//	pointsOrders.Add(SphericalMercator.FromLonLat(lon, lat));
				//}
			}

			var map = CreateMap();
			mapcontrol.Map = map;

			if (firstPosition != null)
				mapcontrol.Navigator.NavigateTo(firstPosition, 4);
		}

		private static List<Geolocator> GetList()
		{
			var gController = new GeolocatorController();
			List<Geolocator> list = gController.FindAll();
			return list;
		}

		private ILayer LoadRotes(List<Geolocator> list)
		{
			point = new List<Point>();

			foreach (var g in list)
			{
				point.Add(SphericalMercator.FromLonLat(double.Parse(g.Longitude), double.Parse(g.Latitude)));
			}

			LineString ls = new LineString(point);

			Feature f = new Feature
			{
				Geometry = ls,
				["Name"] = "Line 1",
				Styles = new List<IStyle> { new VectorStyle { Line = new Pen(Color.Blue, 6) } }
			};

			return new MemoryLayer
			{
				Name = "Route",
				DataSource = new MemoryProvider(f),
				Style = null
			};
		}

		private Mapsui.Map CreateMap()
		{
			Mapsui.Map map = new Mapsui.Map();

			//map.CRS = "EPSG:3857";
			map.CRS = "EPSG:4326";
			map.Widgets.Add(new ScaleBarWidget(map) { TextAlignment = Alignment.Center, HorizontalAlignment = HorizontalAlignment.Center, VerticalAlignment = VerticalAlignment.Top });
			map.Layers.Add(OpenStreetMap.CreateTileLayer());
			//map.Layers.Add(CreatePolygonLayer());
			map.Layers.Add(new WritableLayer());
			map.Layers.Add(CreateLineLayer());

			if (pointsOrders.Count > 0)
				pointsOrders.ForEach(l => map.Layers.Add(CreatePointLayer(l)));

			map.Widgets.Add(
				new ZoomInOutWidget
				{
					HorizontalAlignment = Mapsui.Widgets.HorizontalAlignment.Left,
					VerticalAlignment = Mapsui.Widgets.VerticalAlignment.Top,
					Orientation = Mapsui.Widgets.Zoom.Orientation.Horizontal,
				}
			);

			return map;
		}
		private static ILayer CreatePolygonLayer()
		{
			var features = new List<IFeature> { CreatePolygonFeature(), CreateMultiPolygonFeature() };
			var provider = new MemoryProvider(features);

			var layer = new MemoryLayer
			{
				Name = "Polygon Layer",
				DataSource = provider,
				Style = null,
				IsMapInfoLayer = true
			};

			return layer;
		}

		private static Feature CreatePolygonFeature()
		{
			var feature = new Feature
			{
				Geometry = CreatePolygon(),
				["Name"] = "Polygon 1",
			};
			feature.Styles.Add(new VectorStyle());
			return feature;
		}
		private static Feature CreateMultiPolygonFeature()
		{
			var feature = new Feature
			{
				Geometry = CreateMultiPolygon(),
				["Name"] = "Multipolygon 1"
			};
			feature.Styles.Add(new VectorStyle { Fill = new Brush(Color.Gray), Outline = new Pen(Color.Black) });
			return feature;
		}


		private static Polygon CreatePolygon()
		{
			var point = new List<Point>();

			foreach (var g in GetList())
			{
				double lat = double.Parse(g.Latitude.Replace(".", ","));
				double lon = double.Parse(g.Longitude.Replace(".", ","));

				point.Add(SphericalMercator.FromLonLat(lon, lat));
			}

			return new Polygon(new LinearRing(point));

			//return new Polygon(new LinearRing(new[]
			//{
			//	new Point(1000000, 1000000),
			//	new Point(1000000, -1000000),
			//	new Point(-1000000, -1000000),
			//	new Point(-1000000, 1000000),
			//	new Point(1000000, 1000000)
			//}));
		}

		private static MultiPolygon CreateMultiPolygon()
		{
			var point = new List<Point>();

			foreach (var g in GetList())
			{
				var lat = g.Latitude.Replace(".", ",");
				var lon = g.Longitude.Replace(".", ",");

				point.Add(new Point(double.Parse(lat), double.Parse(lon)));
			}

			return new MultiPolygon
			{
				Polygons = new List<Polygon>
				{
					//new Polygon(new LinearRing(point))

					new Polygon(new LinearRing(new[]
					{
						new Point(4000000, 3000000),
						new Point(4000000, 2000000),
						new Point(3000000, 2000000),
						new Point(3000000, 3000000),
						new Point(4000000, 3000000)
					})),

					new Polygon(new LinearRing(new[]
					{
						new Point(4000000, 5000000),
						new Point(4000000, 4000000),
						new Point(3000000, 4000000),
						new Point(3000000, 5000000),
						new Point(4000000, 5000000)
					}))
				}
			};
		}

		private ILayer CreateLineLayer()
		{
			return new MemoryLayer
			{
				Name = "Line Layer",
				DataSource = new MemoryProvider(CreateLineFeature()),
				Style = null,
				IsMapInfoLayer = true
			};
		}
		private Feature CreateLineFeature()
		{
			return new Feature
			{

				Geometry = CreateLine(),
				["Name"] = "Line 1",
				Styles = new List<IStyle> { new VectorStyle { Line = new Pen(Color.Black, 6) } }
				//Styles = new List<IStyle> { new SymbolStyle { Fill = new Brush { Color = Color.Gray }, Outline = new Pen(Color.Black), SymbolType = SymbolType.Ellipse}  }
			};

		}

		private LineString CreateLine()
		{
			var point = new List<Point>();

			foreach (var g in GetList())
			{
				if (!string.IsNullOrEmpty(g.Latitude) && !string.IsNullOrEmpty(g.Longitude))
				{
					double lat = double.Parse(g.Latitude.Replace(".", ","));
					double lon = double.Parse(g.Longitude.Replace(".", ","));

					//point.Add(new Point(lat, lon));
					point.Add(SphericalMercator.FromLonLat(lon, lat));
				}
			}

			return new LineString(this.location);
		}

		private Point getFirstPostion()
		{
			var g = GetList().FirstOrDefault();

			double lat = double.Parse(g.Latitude.Replace(".", ","));
			double lon = double.Parse(g.Longitude.Replace(".", ","));

			return SphericalMercator.FromLonLat(lon, lat);
		}


		private static ILayer CreatePointLayer(Point p)
		{
			return new MemoryLayer
			{
				Name = "Point Layer",
				DataSource = new MemoryProvider(CreatePointFeature(p)),
				Style = null,
				IsMapInfoLayer = true
			};
		}
		private static Feature CreatePointFeature(Point p)
		{
			return new Feature
			{

				Geometry = p,
				["Name"] = "Line 2",
				//Styles = new List<IStyle> { new VectorStyle { Line = new Pen(Color.Black, 8) } }
				Styles = new List<IStyle>
				{
					new SymbolStyle
					{
						Fill = new Brush { Color = Color.Red },
						Outline = new Pen(Color.Red, 1),
						SymbolType = SymbolType.Triangle 
					}
				}
			};

		}

	}


}