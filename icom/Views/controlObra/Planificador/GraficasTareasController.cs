using System;

using UIKit;
using Alliance.Charts;
using System.Collections.Generic;

namespace icom
{
	public partial class GraficasTareasController : UIViewController
	{

		public AllianceChart grafica;
		private List<clsClasificacion> lstClas;
		public GraficasTareasController() : base("GraficasTareasController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			lstClas = new List<clsClasificacion>();

			clsClasificacion obj1 = new clsClasificacion
			{
				nombre = "Clasificacion 1",
				porcentaje = 50.0f,
				color = UIColor.Blue
			};

			clsClasificacion obj2 = new clsClasificacion
			{
				nombre = "Clasificacion 2",
				porcentaje = 80.0f,
				color = UIColor.Gray
			};

			clsClasificacion obj3 = new clsClasificacion
			{
				nombre = "Clasificacion 3",
				porcentaje = 24.0f,
				color = UIColor.Green
			};

			clsClasificacion obj4 = new clsClasificacion
			{
				nombre = "Clasificacion 4",
				porcentaje = 50.0f,
				color = UIColor.Orange
			};

			clsClasificacion obj5 = new clsClasificacion
			{
				nombre = "Clasificacion 5",
				porcentaje = 15.0f,
				color = UIColor.Red
			};

			lstClas.Add(obj1);
			lstClas.Add(obj2);
			lstClas.Add(obj3);
			lstClas.Add(obj4);
			lstClas.Add(obj5);

			tblClasificaciones.Source = new FuenteTablaGraficas(this, lstClas);

			grafica = new AllianceChart(Chart.Pie, vwGrafica);
			creargrafica();

		}

		private void creargrafica() {


			List<ChartComponent> Componentes = new List<ChartComponent>();
			grafica.PieChart.TitleFont = UIFont.FromName("Arial", 12f);
			grafica.PieChart.ShowPercentage = true;

			foreach (clsClasificacion item in lstClas) { 
				ChartComponent chrtcomp = new ChartComponent
				{
					Name = "",
					value = item.porcentaje,
					color = item.color
				};
				Componentes.Add(chrtcomp);
			}

			grafica.LoadChart(Componentes, Chart.Pie, vwGrafica);
		}
	}
}


