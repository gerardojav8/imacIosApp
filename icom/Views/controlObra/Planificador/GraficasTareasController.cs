using System;

using UIKit;
using Alliance.Charts;
using System.Collections.Generic;

namespace icom
{
	public partial class GraficasTareasController : UIViewController
	{

		public AllianceChart grafica;

		public GraficasTareasController() : base("GraficasTareasController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			grafica = new AllianceChart(Chart.Pie, vwGrafica);

			creargrafica();
			this.View.SetNeedsDisplay();
		}

		private void creargrafica() {
			List<ChartComponent> Componentes = new List<ChartComponent>();
			grafica.PieChart.TitleFont = UIFont.FromName("Arial", 12f);
			grafica.PieChart.ShowPercentage = true;
			ChartComponent chrtcomp = new ChartComponent
			{
				Name = "",
				value = 50.0f,
				color = UIColor.Blue
			};

			ChartComponent chrtcomp2 = new ChartComponent
			{
				Name = "",
				value = 80.0f,
				color = UIColor.Gray
			};

			ChartComponent chrtcomp3 = new ChartComponent
			{
				Name = "",
				value = 24.0f,
				color = UIColor.Green
			};

			ChartComponent chrtcomp4 = new ChartComponent
			{
				Name = "",
				value = 15.0f,
				color = UIColor.Orange
			};

			Componentes.Add(chrtcomp);
			Componentes.Add(chrtcomp2);
			Componentes.Add(chrtcomp3);
			Componentes.Add(chrtcomp4);



			grafica.LoadChart(Componentes, Chart.Pie, vwGrafica);
		}
	}
}


