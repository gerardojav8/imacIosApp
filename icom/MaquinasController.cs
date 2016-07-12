using System;

using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using icom.globales;
using System.Text;
using Newtonsoft.Json;
using System.Json;
using System.Linq;
using CoreGraphics;

namespace icom
{
	public partial class MaquinasController : UIViewController
	{
		public MaquinasController() : base("MaquinasController", null)
		{
		}

		LoadingOverlay loadPop;
		HttpClient client;
		public static List<clsListadoMaquinas> lstMaqServ;


		public override void ViewDidLoad()
		{

			base.ViewDidLoad();

			lstMaqServ = new List<clsListadoMaquinas>();
			lstMaquinas.Source = new FuenteTablaExpandible(this);

			/*Boolean resp = await getAllMaquinas();

			if (resp)
			{
				loadPop.Hide();
				lstMaquinas.ReloadData();


			}*/

			clsListadoMaquinas obj1 = new clsListadoMaquinas();
			obj1.noserie = "1234568";
			obj1.noeconomico = 1234;
			obj1.marca = "Mercedes venz";
			obj1.modelo = 1234;
			obj1.IdTipoMaquina = 1;
			obj1.tieneReporte = 1;


			clsListadoMaquinas obj2 = new clsListadoMaquinas();
			obj2.noserie = "45678";
			obj2.noeconomico = 6789;
			obj2.marca = "Toyota";
			obj2.modelo = 3654;
			obj2.IdTipoMaquina = 2;
			obj2.tieneReporte = 0;


			clsListadoMaquinas obj3 = new clsListadoMaquinas();
			obj3.noserie = "987654";
			obj3.noeconomico = 9871;
			obj3.marca = "Volvo";
			obj3.modelo = 8798;
			obj3.IdTipoMaquina = 3;
			obj3.tieneReporte = 1;


			lstMaqServ.Add(obj1);
			lstMaqServ.Add(obj2);
			lstMaqServ.Add(obj3);

			btnAgregar.TouchUpInside += delegate {
				solicitudMaquinaController viewsolmaq = new solicitudMaquinaController();
				viewsolmaq.Title = "Solicitud de Maquinaria";
				viewsolmaq.viewmaq = this;

				this.NavigationController.PushViewController(viewsolmaq, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnSearch.TouchUpInside += delegate
			{
				funciones.MessageBox("Aviso", "Boton de buscar");
			};



		}



		public async void recargarListado() {
			
			lstMaqServ = new List<clsListadoMaquinas>();
			Boolean resp = await getAllMaquinas();

			if (resp)
			{
				loadPop.Hide();
				lstMaquinas.ReloadData();
			}
		}


		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public async Task<Boolean> getAllMaquinas()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando Maquinas ...");
			View.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "maquinas/getListadoMaquinas";
			var uri = new Uri(string.Format(url));

			var content = new StringContent("", Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Consts.token);

			HttpResponseMessage response = null;

			try
			{
				response = await client.PostAsync(uri, content);
			}
			catch (Exception e)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI "+ e.HResult );
				return false;
			}

			if (response == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI ");
				return false;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			JArray jrarray;


			try
			{
				var jsonresponse = JArray.Parse(responseString);
				jrarray = jsonresponse;
			}
			catch (Exception e)
			{
				loadPop.Hide();
				var jsonresponse = JObject.Parse(responseString);

				string mensaje = "error al traer maquinas del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}




			foreach(var maquina in jrarray)
			{
				clsListadoMaquinas objm = getobjMaquina(maquina);
				lstMaqServ.Add(objm);
			}


			return true;
		}

		public clsListadoMaquinas getobjMaquina(Object varjson)
		{
			clsListadoMaquinas objmaq = new clsListadoMaquinas();
			JObject json = (JObject)varjson;

			objmaq.noserie = json["noserie"].ToString();
			objmaq.noeconomico = Int32.Parse(json["noeconomico"].ToString());
			objmaq.marca = json["marca"].ToString();
			objmaq.modelo = Int32.Parse(json["modelo"].ToString());
			objmaq.IdTipoMaquina = Int32.Parse(json["idtipomaquina"].ToString());
			objmaq.tieneReporte = Int32.Parse(json["tieneReporte"].ToString());


			return objmaq;
		}
	}


	public class FuenteTablaExpandible : UITableViewSource
	{		
		static readonly string idPersonaje = "Celda";
		protected readonly string ParentCellIdentifier = "ParentCell";
		protected readonly string ChildCellIndentifier = "ChildCell";
		protected int currentExpandedIndex = -1;
		protected UIViewController viewparent;
		protected Boolean sec = false;

		public FuenteTablaExpandible(UIViewController view) {
			viewparent = view;
		}


		public override nint RowsInSection(UITableView tableview, nint section)
		{
			if (currentExpandedIndex > -1) {
				return icom.MaquinasController.lstMaqServ.Count + 3;
			}

			return icom.MaquinasController.lstMaqServ.Count;
		}



		void collapseSubItemsAtIndex(UITableView tableView, int index)
		{
			tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 1, 0) }, UITableViewRowAnimation.Fade);
			tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 2, 0) }, UITableViewRowAnimation.Fade);
			tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 3, 0) }, UITableViewRowAnimation.Fade);
		}

		void expandItemAtIndex(UITableView tableView, int index)
		{
			int insertPos = index + 1;
			tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
			tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
			tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
		}

		protected bool isChild(NSIndexPath indexPath)
		{
			return currentExpandedIndex > -1 &&
				   indexPath.Row > currentExpandedIndex &&
				   indexPath.Row <= currentExpandedIndex + 3;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (isChild(indexPath))
			{
				//Handle selection of child cell


				if (indexPath.Row == currentExpandedIndex + 1)
				{

					clsListadoMaquinas maquina = icom.MaquinasController.lstMaqServ.ElementAt(currentExpandedIndex);

					ReporteOperador viewro = new ReporteOperador();
					viewro.Title = "Reporte Operador";

					viewro.strNoeconomico = maquina.noeconomico.ToString();
					viewro.strModelo = maquina.modelo.ToString();
					viewro.strNoSerie = maquina.noserie;
					viewro.viewmaq = viewparent;

					viewparent.NavigationController.PushViewController(viewro, false);
					UIView.BeginAnimations(null);
					UIView.SetAnimationDuration(0.7);
					UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
					UIView.CommitAnimations();

					tableView.DeselectRow(indexPath, true);
					return;
				}
				else {
					if (indexPath.Row == currentExpandedIndex + 2)
					{

						clsListadoMaquinas maquina = icom.MaquinasController.lstMaqServ.ElementAt(currentExpandedIndex);
						ReporteServicio viewrs = new ReporteServicio();
						viewrs.Title = "Reporte Servicio";
						viewrs.strNoSerie = maquina.noserie;
						viewrs.viewmaq = viewparent;

						viewparent.NavigationController.PushViewController(viewrs, false);
						UIView.BeginAnimations(null);
						UIView.SetAnimationDuration(0.7);
						UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
						UIView.CommitAnimations();

						tableView.DeselectRow(indexPath, true);
						return;
					}
					else {

						clsListadoMaquinas maquina = icom.MaquinasController.lstMaqServ.ElementAt(currentExpandedIndex);

						FichaMaquinaController viewfm = new FichaMaquinaController();
						viewfm.Title = "Ficha Tecnica de la Maquina";
						viewfm.viewmaq = viewparent;
						viewfm.strNoserie = maquina.noserie;


						viewparent.NavigationController.PushViewController(viewfm, false);
						UIView.BeginAnimations(null);
						UIView.SetAnimationDuration(0.7);
						UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
						UIView.CommitAnimations();

						tableView.DeselectRow(indexPath, true);
						return;

					}
				}
			}

			tableView.BeginUpdates();
			if (currentExpandedIndex == indexPath.Row)
			{
				this.collapseSubItemsAtIndex(tableView, currentExpandedIndex);
				currentExpandedIndex = -1;
			}
			else {
				var shouldCollapse = currentExpandedIndex > -1;
				if (shouldCollapse)
				{
					this.collapseSubItemsAtIndex(tableView, currentExpandedIndex);
				}
				currentExpandedIndex = (shouldCollapse && indexPath.Row > currentExpandedIndex) ? indexPath.Row - 3 : indexPath.Row;
				this.expandItemAtIndex(tableView, currentExpandedIndex);
			}
			tableView.EndUpdates();
			tableView.DeselectRow(indexPath, true);
		}

		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			return (nfloat)0.0;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (isChild(indexPath))
			{
				return (nfloat)40.0;
			}
			else {
				return (nfloat)70.0;
			}
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
							

			if (isChild(indexPath))
			{
				var cell = tableView.DequeueReusableCell(idPersonaje);
				if (cell == null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Subtitle, idPersonaje);
				}

				if (indexPath.Row == currentExpandedIndex + 1)
				{
					cell.TextLabel.Text = "Reporte de Operador";
				}
				else {
					if (indexPath.Row == currentExpandedIndex + 2)
					{
						cell.TextLabel.Text = "Reporte de Servicio";
					}else{
						cell.TextLabel.Text = "Ficha de Maquina";
					}
				}
				cell.TextLabel.TextColor = UIColor.FromRGB(54, 74, 97);
				cell.ImageView.Image = null;
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				cell.ContentView.BackgroundColor = UIColor.FromRGB(217, 215, 213);
				return cell;
			}
			else {
				var cell = tableView.DequeueReusableCell(idPersonaje) as CustomVegeCell;

				if (cell == null)
				{
					cell = new CustomVegeCell((NSString)idPersonaje, sec);
				}

				int indicearreglo = indexPath.Row;

				if (currentExpandedIndex > -1 && indexPath.Row > currentExpandedIndex)
				{
					indicearreglo -= 3;
				}
					

				clsListadoMaquinas maquina = icom.MaquinasController.lstMaqServ.ElementAt(indicearreglo);

				UIImage imgmaq = null;
				String strtipomaq = "";

				if (maquina.IdTipoMaquina == Consts.Excabadora)
				{
					imgmaq = UIImage.FromFile("excabadora.png");
					strtipomaq = "Excabadora";
				}
				else if (maquina.IdTipoMaquina == Consts.Revolvedora)
				{
					imgmaq = UIImage.FromFile("revolvedora.png");
					strtipomaq = "Revolvedora";
				}
				else if (maquina.IdTipoMaquina == Consts.Trascabo)
				{
					imgmaq = UIImage.FromFile("trascabo.png");
					strtipomaq = "Trascabo";
				}
				else {
					imgmaq = null;
					strtipomaq = "";
				}

				String leyenda = "Marca: " + maquina.marca + "   Modelo: " + maquina.modelo ;

				UIImage imagesem;

				if (maquina.tieneReporte == 1)
				{
					imagesem = UIImage.FromFile("red.png");
				}
				else { 
					imagesem = UIImage.FromFile("green.png");
				}


				cell.UpdateCell( strtipomaq, leyenda, imgmaq, imagesem);


				cell.Accessory = UITableViewCellAccessory.None;
				sec = !sec;
				return cell;
			}

		}
	}

	public class CustomVegeCell : UITableViewCell
	{
		UILabel headingLabel, subheadingLabel;
		UIImageView imageView;
		UIImageView imageView2;

		public CustomVegeCell(NSString cellId, Boolean sec) : base(UITableViewCellStyle.Default, cellId)
		{
			
			SelectionStyle = UITableViewCellSelectionStyle.Gray;
			if (sec)
			{
				ContentView.BackgroundColor = UIColor.FromRGB(237, 242, 248);
			}
			else { 
				ContentView.BackgroundColor = UIColor.FromRGB(216, 223, 231);
			}
			imageView = new UIImageView();
			imageView2 = new UIImageView();
			headingLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 22f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				BackgroundColor = UIColor.Clear
			};
			subheadingLabel = new UILabel()
			{
				Font = UIFont.FromName("Arial", 15f),
				TextColor = UIColor.FromRGB(54, 74, 97),
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear
			};
			ContentView.AddSubviews(new UIView[] { headingLabel, subheadingLabel, imageView, imageView2 });

		}
		public void UpdateCell(string caption, string subtitle, UIImage image, UIImage image2)
		{
			imageView.Image = image;
			imageView2.Image = image2;
			headingLabel.Text = caption;
			subheadingLabel.Text = subtitle;
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();

			imageView.Frame = new CGRect(5, 5, 55, 55);
			headingLabel.Frame = new CGRect(80, 4, ContentView.Bounds.Width - 63, 25);
			imageView2.Frame = new CGRect(300, 6, 25, 25);
			subheadingLabel.Frame = new CGRect(80, 32, 500, 20);
		}

	}

}


