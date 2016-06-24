using System;

using UIKit;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using CoreGraphics;

namespace icom
{
	public partial class solicitudMaquinaController : UIViewController
	{

		public static List<clsSolicitudesMaquinas> lstsolmaq = new List<clsSolicitudesMaquinas>();

		UIActionSheet actEquipoSol;
		List<clsEquipo> lstEquipo = new List<clsEquipo>();
		clsEquipo eqselect;

		UIActionSheet actMarca;
		List<clsMarca> lstmarca = new List<clsMarca>();
		clsMarca marcaselect;

		UIActionSheet actModelo;
		List<clsModelo> lstModelo = new List<clsModelo>();
		clsModelo modeloselect;

		public solicitudMaquinaController() : base("solicitudMaquinaController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			scrViewSolicitudMaquina.ContentSize = new CoreGraphics.CGSize(375, 1583);

			lstRequerimientos.Layer.BorderColor = UIColor.Black.CGColor;
			lstRequerimientos.Layer.BorderWidth = (nfloat)2.0;
			lstRequerimientos.Source = new FuenteTablaRequerimientos();

			clsEquipo eq1 = new clsEquipo();
			eq1.idequipo = 1;
			eq1.nombre = "Equipo test";
			eq1.descripcion = "";


			clsEquipo eq2 = new clsEquipo();
			eq2.idequipo = 2;
			eq2.nombre = "Equipo prueba";
			eq2.descripcion = "";

			lstEquipo.Add(eq1);
			lstEquipo.Add(eq2);

			clsMarca marc1 = new clsMarca();
			marc1.idmarca = 1;
			marc1.nombre = "Marca test";
			marc1.descripcion = "";

			clsMarca marc2 = new clsMarca();
			marc2.idmarca = 2;
			marc2.nombre = "Marca prueba";
			marc2.descripcion = "";

			lstmarca.Add(marc1);
			lstmarca.Add(marc2);

			clsModelo mod1 = new clsModelo();
			mod1.idmodelo = 1;
			mod1.nombre = "Marca test";
			mod1.descripcion = "";

			clsModelo mod2 = new clsModelo();
			mod2.idmodelo = 2;
			mod2.nombre = "Marca prueba";
			mod2.descripcion = "";

			lstModelo.Add(mod1);
			lstModelo.Add(mod2);

			inicializaCombos();


			btnAgregar.TouchUpInside += delegate {
				clsSolicitudesMaquinas objsol = new clsSolicitudesMaquinas();
				objsol.cantidad = Int32.Parse(txtCantidad.Text);
				objsol.equipo = eqselect;
				objsol.marca = marcaselect;
				objsol.modelo = modeloselect;

				icom.solicitudMaquinaController.lstsolmaq.Add(objsol);

				lstRequerimientos.ReloadData();
			};

		}

		public void inicializaCombos()
		{

			//--------Combo Equipo solicitado---------------------
			actEquipoSol = new UIActionSheet("Seleccionar");
			foreach (clsEquipo equipo in lstEquipo)
			{				
				actEquipoSol.Add(equipo.nombre);
			}
			actEquipoSol.Add("Cancelar");

			actEquipoSol.Style = UIActionSheetStyle.BlackTranslucent;
			actEquipoSol.CancelButtonIndex = lstEquipo.Count;

			btnequiposolicitado.TouchUpInside += delegate
			{
				actEquipoSol.ShowInView(this.contentViewSolicitudMaquina);
			};

			actEquipoSol.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstEquipo.Count)
				{
					clsEquipo eq = lstEquipo.ElementAt((int)e.ButtonIndex);
					txtEquiposolicitado.Text = eq.nombre;
					eqselect = eq;
				}
				else {
					txtEquiposolicitado.Text = "";
					eqselect = null;
				}
			};


			//--------Combo marca---------------------
			actMarca = new UIActionSheet("Seleccionar");
			foreach (clsMarca marca in lstmarca)
			{
				actMarca.Add(marca.nombre);
			}
			actMarca.Add("Cancelar");

			actMarca.Style = UIActionSheetStyle.BlackTranslucent;
			actMarca.CancelButtonIndex = lstEquipo.Count;

			btnMarca.TouchUpInside += delegate
			{
				actMarca.ShowInView(this.contentViewSolicitudMaquina);
			};

			actMarca.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstmarca.Count)
				{
					clsMarca marca = lstmarca.ElementAt((int)e.ButtonIndex);
					txtmarca.Text = marca.nombre;
					marcaselect  = marca;
				}
				else {
					txtmarca.Text = "";
					marcaselect = null;
				}
			};

			//--------Combo modelo---------------------
			actModelo = new UIActionSheet("Seleccionar");
			foreach (clsModelo modelo in lstModelo)
			{
				actModelo.Add(modelo.nombre);
			}
			actModelo.Add("Cancelar");

			actModelo.Style = UIActionSheetStyle.BlackTranslucent;
			actModelo.CancelButtonIndex = lstEquipo.Count;

			btnModelo.TouchUpInside += delegate
			{
				actModelo.ShowInView(this.contentViewSolicitudMaquina);
			};

			actModelo.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstmarca.Count)
				{
					clsModelo modelo = lstModelo.ElementAt((int)e.ButtonIndex);
					txtModelo.Text = modelo.nombre;
					modeloselect = modelo;
				}
				else {
					txtModelo.Text = "";
					modeloselect = null;
				}
			};
		}
	}

	public class FuenteTablaRequerimientos : UITableViewSource
	{
		static readonly string idPersonaje = "Celda";

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(idPersonaje) as CustomSolMaqCell;
			if (cell == null)
			{
				cell = new CustomSolMaqCell((NSString)idPersonaje);
			}

			clsSolicitudesMaquinas sol = icom.solicitudMaquinaController.lstsolmaq.ElementAt(indexPath.Row);

			cell.UpdateCell(sol.cantidad.ToString(), sol.equipo.nombre, sol.marca.nombre, sol.modelo.nombre);


			cell.Accessory = UITableViewCellAccessory.None;


			return cell;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return icom.solicitudMaquinaController.lstsolmaq.Count;
		}
	}

	public class CustomSolMaqCell : UITableViewCell
	{
		UILabel cantidadlabel, equipolabel, marcalabel, modelolabel;

		public CustomSolMaqCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{

			SelectionStyle = UITableViewCellSelectionStyle.Gray;

			ContentView.BackgroundColor = UIColor.White;

			cantidadlabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 22f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			equipolabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 22f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};
			marcalabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 22f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			modelolabel = new UILabel()				
			{
				Font = UIFont.FromName("Arial", 22f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};

			ContentView.AddSubviews(new UIView[] { cantidadlabel, equipolabel, marcalabel, modelolabel });

		}
		public void UpdateCell(string cantidad, string equipo, String marca, String modelo)
		{
			cantidadlabel.Text = cantidad;
			equipolabel.Text = equipo;
			marcalabel.Text = marca;
			modelolabel.Text = modelo;
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			cantidadlabel.Frame = new CGRect(5, 5, 100, 100);
			equipolabel.Frame = new CGRect(105,  5,100,100);
			marcalabel.Frame = new CGRect(205, 5, 100, 100);
			modelolabel.Frame = new CGRect(305, 5, 100, 100);
		}

	}
}


