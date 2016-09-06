using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;

namespace icom
{
	public class FuenteTablaClasificaciones : UITableViewSource
	{
		static readonly string celdaCategoriasBlack = "celdaCategoriasBlack";
		static readonly string celdaCategoriasWhite = "celdaCategiruasWhite";
		private List<clsClasificacion> lstCalsificacion;
		protected UIViewController viewparent;

		public FuenteTablaClasificaciones(UIViewController view, List<clsClasificacion> lst)
		{
			lstCalsificacion = lst;
			viewparent = view;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return lstCalsificacion.Count;
		}


		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{

		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{

			return (nfloat)50.0;

		}

		public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
		{
			PlanificadorController viewplan = new PlanificadorController();
			viewplan.Title = "Tareas";

			viewparent.NavigationController.PushViewController(viewplan, false);
			UIView.BeginAnimations(null);
			UIView.SetAnimationDuration(0.7);
			UIView.SetAnimationTransition(UIViewAnimationTransition.CurlDown, viewparent.NavigationController.View, true);
			UIView.CommitAnimations();
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{

			int indicearreglo = indexPath.Row;

			clsClasificacion objclas = lstCalsificacion.ElementAt(indicearreglo);
			UITableViewCell cell;

			if (indicearreglo % 2 == 0)
			{
				cell = tableView.DequeueReusableCell(celdaCategoriasBlack) as CustomCategoriasCellBlack;

				if (cell == null)
				{
					cell = new CustomCategoriasCellBlack((NSString)celdaCategoriasBlack, objclas.porcentaje);
				}
			}
			else {
				cell = tableView.DequeueReusableCell(celdaCategoriasWhite) as CustomCategoriasCellWhite;

				if (cell == null)
				{
					cell = new CustomCategoriasCellWhite((NSString)celdaCategoriasWhite, objclas.porcentaje);
				}
			}

			String strTareas = objclas.notareas + " tareas";
			((CustomCategoriasCell)cell).UpdateCell(objclas.nombre, strTareas);
			cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;

			return cell;
		}
	}

	public class CustomCategoriasCell : UITableViewCell
	{
		UILabel headingLabel, categoriaLabel, porLabel;
		private double porcentajelabel;

		public CustomCategoriasCell(NSString cellId, double por) : base(UITableViewCellStyle.Default, cellId)
		{

			SelectionStyle = UITableViewCellSelectionStyle.Blue;


			headingLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 20f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			categoriaLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 12f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};


			porLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial-BoldMT", 7f),
				TextColor = UIColor.White,
				TextAlignment = UITextAlignment.Center,
				BackgroundColor = UIColor.FromRGB(165, 97, 76)
			};

			porcentajelabel = por;


			ContentView.AddSubviews(new UIView[] { porLabel, headingLabel, categoriaLabel });



		}
		public void UpdateCell(string titulo, string categoria)
		{

			headingLabel.Text = titulo;
			categoriaLabel.Text = categoria;
			porLabel.Text = porcentajelabel.ToString() + " %";
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			headingLabel.Frame = new CGRect(10, 1, ContentView.Bounds.Width - 63, 25);
			categoriaLabel.Frame = new CGRect(10, 19, ContentView.Bounds.Width - 63, 25);

			double tam = (porcentajelabel * ContentView.Bounds.Width) / 100;
			porLabel.Frame = new CGRect(0, 42, tam, 7);



		}

	}

	public class CustomCategoriasCellBlack : CustomCategoriasCell
	{

		public CustomCategoriasCellBlack(NSString cellId, double por) : base(cellId, por)
		{

			ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);

		}

	}

	public class CustomCategoriasCellWhite : CustomCategoriasCell
	{

		public CustomCategoriasCellWhite(NSString cellId, double por) : base(cellId, por)
		{
			ContentView.BackgroundColor = UIColor.White;
		}

	}

}
