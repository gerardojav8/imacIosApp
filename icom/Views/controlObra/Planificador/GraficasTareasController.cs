﻿using System;

using UIKit;
using Alliance.Charts;
using System.Collections.Generic;
using Foundation;
using System.IO;
using Xfinium.Pdf;
using Xfinium.Pdf.Graphics;

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

			btnExportarGrafica.TouchUpInside += delegate {

				crearPDF();

			};


		}

		private void crearPDF() { 
			String pathimagen = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "grafica.png");
			String pathpdf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "pdfgrafica.pdf");

			UIGraphics.BeginImageContext(vwGrafica.Frame.Size);

			try
			{
				using (var context = UIGraphics.GetCurrentContext())
				{
					vwGrafica.Layer.RenderInContext(context);
					using (var image = UIGraphics.GetImageFromCurrentImageContext())
					{


						NSData imgData = image.AsPNG();

						NSError err = null;

						if (!imgData.Save(pathimagen, false, out err))
						{
							funciones.MessageBox("Error", err.LocalizedDescription);
							return;
						}

					}
				}
			}
			finally
			{
				UIGraphics.EndImageContext();
			}

			if (!File.Exists(pathimagen))
			{
				funciones.MessageBox("Error", "No existe la imagen de la grafica, verifiquelo por favor");
				return;
			}

			String strimagenbase64 = funciones.getBase64Image(pathimagen);
			Console.WriteLine(strimagenbase64);

			PdfFixedDocument document = new PdfFixedDocument();
			PdfStandardFont helveticaBoldTitle = new PdfStandardFont(PdfStandardFontFace.HelveticaBold, 16);
			PdfStandardFont helveticaSection = new PdfStandardFont(PdfStandardFontFace.Helvetica, 10);

			PdfPage page = document.Pages.Add();

			FileStream imageStream = new FileStream(pathimagen, FileMode.Open, FileAccess.Read, FileShare.Read);
			PdfPngImage png = new PdfPngImage(imageStream);
			page.Graphics.DrawImage(png, (page.Width / 2) - 200, 5, 400, 300);
			page.Graphics.CompressAndClose();

			document.Save(pathpdf);

			funciones.MessageBox("Aviso", "Documento guardado");

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

