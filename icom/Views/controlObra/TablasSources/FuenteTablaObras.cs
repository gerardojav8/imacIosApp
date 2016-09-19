using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using System.Linq;
using CoreGraphics;
namespace icom
{
	public class FuenteTablaObras : UITableViewSource
	{
		static readonly string celdaObrasBlack = "CeldaObrasBlack";
		static readonly string celdaObrasWhite = "CeldaObrasWhite";
		private List<clsListadoObra> lstObras;
		protected UIViewController viewparent;

		public FuenteTablaObras(UIViewController view, List<clsListadoObra> lst)
		{
			viewparent = view;
			lstObras = lst;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return lstObras.Count;
		}


		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{

		}

		public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
		{			
			CategoriasTareasController viewCategorias = new CategoriasTareasController();
			viewCategorias.Title = lstObras.ElementAt(indexPath.Row).nombre;
			viewCategorias.idobra = lstObras.ElementAt(indexPath.Row).idobra;

			viewparent.NavigationController.PushViewController(viewCategorias, false);
			UIView.BeginAnimations(null);
			UIView.SetAnimationDuration(0.7);
			UIView.SetAnimationTransition(UIViewAnimationTransition.CurlDown, viewparent.NavigationController.View, true);
			UIView.CommitAnimations();

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

			return (nfloat)50.0;

		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{


			int indicearreglo = indexPath.Row;

			UITableViewCell cell;
			clsListadoObra objobra = lstObras.ElementAt(indicearreglo);


			if (indicearreglo % 2 == 0)
			{
				string idcell = celdaObrasBlack + objobra.idobra.ToString() + objobra.porcentajeavance.ToString();
				cell = tableView.DequeueReusableCell(idcell) as CustomListadoObrasCellBlack;

				if (cell == null)
				{
					cell = new CustomListadoObrasCellBlack((NSString)idcell, objobra.porcentajeavance);
				}
			}
			else { 
				string idcell = celdaObrasWhite + objobra.idobra.ToString() + objobra.porcentajeavance.ToString();
				cell = tableView.DequeueReusableCell(idcell) as CustomListadoObrasCellWhite;

				if (cell == null)
				{
					cell = new CustomListadoObrasCellWhite((NSString)idcell, objobra.porcentajeavance);
				}
			}



			String strClasificaciones =  objobra.noclasificacioens + " Clasificaciones";
			((CustomListadoObrasCell)cell).UpdateCell(objobra.nombre, strClasificaciones);
			cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;

			return cell;


		}
	}

	public class CustomListadoObrasCell : UITableViewCell
	{
		UILabel headingLabel, categoriaLabel, porLabel;
		private double porcentajelabel;

		public CustomListadoObrasCell(NSString cellId, double por) : base(UITableViewCellStyle.Default, cellId)
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
				BackgroundColor = UIColor.FromRGB(76, 100, 142)
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

	public class CustomListadoObrasCellBlack : CustomListadoObrasCell
	{

		public CustomListadoObrasCellBlack(NSString cellId, double por) : base(cellId, por)
		{

			ContentView.BackgroundColor = UIColor.FromRGB(220, 224, 231);

		}

	}

	public class CustomListadoObrasCellWhite : CustomListadoObrasCell
	{

		public CustomListadoObrasCellWhite(NSString cellId, double por) : base(cellId, por)
		{
			ContentView.BackgroundColor = UIColor.White;
		}

	}
}

