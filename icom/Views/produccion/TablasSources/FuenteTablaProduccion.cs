using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;
namespace icom
{
	public class FuenteTablaProduccion : UITableViewSource
	{		
		static readonly string celdaProduccionBlack = "CeldaProduccionBlack";
		static readonly string celdaProduccionWhite = "CeldaProduccionWhite";
		private List<clsProduccion> lstProd;
		protected UIViewController viewparent;

		public FuenteTablaProduccion(UIViewController view, List<clsProduccion> lst)
		{
			viewparent = view;
			lstProd = lst;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return lstProd.Count;
		}


		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{

		}


		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			return 0;
		}

		public override nfloat GetHeightForFooter(UITableView tableView, nint section)
		{
			return 0;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{

			return (nfloat)70.0;

		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{


			int indicearreglo = indexPath.Row;

			UITableViewCell cell;
			clsProduccion objprod = lstProd.ElementAt(indicearreglo);


			if (indicearreglo % 2 == 0)
			{
				string idcell = celdaProduccionBlack;
				cell = tableView.DequeueReusableCell(idcell) as CustomListadoProduccionCellBlack;

				if (cell == null)
				{
					cell = new CustomListadoProduccionCellBlack((NSString)idcell);
				}
			}
			else {
				string idcell = celdaProduccionWhite;
				cell = tableView.DequeueReusableCell(idcell) as CustomListadoProduccionCellWhite;

				if (cell == null)
				{
					cell = new CustomListadoProduccionCellWhite((NSString)idcell);
				}
			}



			String strcantidad = objprod.cantidad + " " + objprod.unidad;
			((CustomListadoProduccionCell)cell).UpdateCell(objprod.material, strcantidad, objprod.cliente, objprod.folio, objprod.fecha);


			return cell;


		}
	}

	public class CustomListadoProduccionCell : UITableViewCell
	{
		UILabel headingLabel, cantidadLabel, clienteLabel, clientelblLabel, folioLabel, fechaLabel;

		public CustomListadoProduccionCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{

			SelectionStyle = UITableViewCellSelectionStyle.Blue;


			headingLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 20f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			cantidadLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 20f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Right
			};

			clienteLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Right
			};

			clientelblLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			folioLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			fechaLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear,
				TextAlignment = UITextAlignment.Right
			};


			ContentView.AddSubviews(new UIView[] { headingLabel, cantidadLabel, clienteLabel, clientelblLabel, folioLabel, fechaLabel });



		}
		public void UpdateCell(string material, string cantidad, string cliente, string folio, string fecha)
		{

			headingLabel.Text = material;
			cantidadLabel.Text = cantidad;
			clienteLabel.Text = cliente;
			clientelblLabel.Text = "Cliente:";
			folioLabel.Text = "Folio: " + folio;
			fechaLabel.Text = fecha;
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			headingLabel.Frame = new CGRect(10, 3, ContentView.Bounds.Width - 63, 25);
			cantidadLabel.Frame = new CGRect(7, 3, ContentView.Bounds.Width - 20, 25);

			clientelblLabel.Frame = new CGRect(10, 22, ContentView.Bounds.Width - 63, 25);
			clienteLabel.Frame = new CGRect(7, 22, ContentView.Bounds.Width - 20, 25);

			folioLabel.Frame = new CGRect(10, 40, ContentView.Bounds.Width - 63, 25);
			fechaLabel.Frame = new CGRect(7, 40, ContentView.Bounds.Width - 20, 25);


		}

	}

	public class CustomListadoProduccionCellBlack : CustomListadoProduccionCell
	{

		public CustomListadoProduccionCellBlack(NSString cellId) : base(cellId)
		{

			ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);

		}

	}

	public class CustomListadoProduccionCellWhite : CustomListadoProduccionCell
	{

		public CustomListadoProduccionCellWhite(NSString cellId) : base(cellId)
		{
			ContentView.BackgroundColor = UIColor.White;
		}

	}
}
