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

		HttpClient client;
		public static List<clsMaquinas> lstMaqServ = new List<clsMaquinas>();
		public string token
		{
			get;
			set;
		}


		public override void ViewDidLoad()
		{
			
			base.ViewDidLoad();




			lstMaquinas.Source = new FuenteTablaExpandible(this);


			lstMaquinas.SeparatorColor = UIColor.Blue;
			lstMaquinas.SeparatorStyle = UITableViewCellSeparatorStyle.DoubleLineEtched;

			// blur effect
			//lstMaquinas.SeparatorEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Dark);

			//vibrancy effect
			//var effect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Light);
			//lstMaquinas.SeparatorEffect = UIVibrancyEffect.FromBlurEffect(effect);



			//await getAllMaquinas();

			clsMaquinas obj1 = new clsMaquinas();
			obj1.noserie = "1234568";
			obj1.noeconomico = 1234;
			obj1.marca = "Mercedes venz";
			obj1.modelo = 1234;
			obj1.aniofabricacion = 2015;
			obj1.idequipo = -1;
			obj1.imagen = "";

			clsMaquinas obj2 = new clsMaquinas();
			obj2.noserie = "45678";
			obj2.noeconomico = 6789;
			obj2.marca = "Toyota";
			obj2.modelo = 3654;
			obj2.aniofabricacion = 2012;
			obj2.idequipo = -1;
			obj2.imagen = "";

			clsMaquinas obj3 = new clsMaquinas();
			obj3.noserie = "987654";
			obj3.noeconomico = 9871;
			obj3.marca = "Volvo";
			obj3.modelo = 8798;
			obj3.aniofabricacion = 2017;
			obj3.idequipo = -1;
			obj3.imagen = "";

			lstMaqServ.Add(obj1);
			lstMaqServ.Add(obj2);
			lstMaqServ.Add(obj3);


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public async Task<Boolean> getAllMaquinas()
		{

			client = new HttpClient();
			string url = Consts.ulrserv + "maquinas/getTodasMaquinas";
			var uri = new Uri(string.Format(url));

			var content = new StringContent("", Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

			HttpResponseMessage response = null;

			try
			{
				response = await client.PostAsync(uri, content);
			}
			catch (Exception e)
			{
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI");
				return false;
			}

			if (response == null)
			{
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI");
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
				var jsonresponse = JObject.Parse(responseString);

				string mensaje = "error al traer maquinas del servidor";

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}


			lstMaqServ = new List<clsMaquinas>();

			foreach(var maquina in jrarray)
			{
				clsMaquinas objm = getobjMaquina(maquina);
				lstMaqServ.Add(objm);
			}

			funciones.MessageBox("Aviso", "Maquinas cargadas");
			return true;
		}

		public clsMaquinas getobjMaquina(Object varjson)
		{
			clsMaquinas objmaq = new clsMaquinas();
			JObject json = (JObject)varjson;

			objmaq.noserie = json["noserie"].ToString();
			objmaq.noeconomico = Int32.Parse(json["noeconomico"].ToString());
			objmaq.marca = json["marca"].ToString();
			objmaq.modelo = Int32.Parse(json["modelo"].ToString());
			objmaq.aniofabricacion = Int32.Parse(json["aniofabricacion"].ToString());
			objmaq.idequipo = Int32.Parse(json["idequipo"].ToString());
			objmaq.imagen = json["imagen"].ToString();

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
					ReporteOperador viewro = new ReporteOperador();
					viewro.Title = "Reporte Operador";


					viewparent.NavigationController.PushViewController(viewro, false);
					UIView.BeginAnimations(null);
					UIView.SetAnimationDuration(0.7);
					UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
					UIView.CommitAnimations();
					//funciones.MessageBox("Aviso", "Reporte de Operador de Maquina");
					tableView.DeselectRow(indexPath, true);
					return;
				}
				else {
					if (indexPath.Row == currentExpandedIndex + 2)
					{
						ReporteServicio viewrs = new ReporteServicio();
						viewrs.Title = "Reporte Servicio";


						viewparent.NavigationController.PushViewController(viewrs, false);
						UIView.BeginAnimations(null);
						UIView.SetAnimationDuration(0.7);
						UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, viewparent.NavigationController.View, true);
						UIView.CommitAnimations();
						//funciones.MessageBox("Aviso", "Reporte de Servicio de Maquina");
						tableView.DeselectRow(indexPath, true);
						return;
					}
					else {
						funciones.MessageBox("Aviso", "Ficha tecnica de maquina");
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

				//cell.DetailTextLabel.Text = "Fila Expandida";
				cell.ImageView.Image = null;
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				return cell;
			}
			else {
				var cell = tableView.DequeueReusableCell(idPersonaje) as CustomVegeCell;

				if (cell == null)
				{
					cell = new CustomVegeCell((NSString)idPersonaje);
				}

				var maquina = icom.MaquinasController.lstMaqServ.ElementAt(indexPath.Row);

				cell.UpdateCell( maquina.marca,
			                      "Modelo: " + maquina.modelo,
			 					  null);
				

				/*string uri = personaje.RutaImagen;

				string filename = uri.Replace("https://dl.dropboxusercontent.com/u/106676747/", "");
				string documentspath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				string localpath = Path.Combine(documentspath, filename);

				if (File.Exists(localpath))
				{
					cell.ImageView.Image = UIImage.FromFile(localpath);
				}
				else {
					var url = new NSUrl(uri);
					var data = NSData.FromUrl(url);

					byte[] dataBytes = new byte[data.Length];
					System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));
					File.WriteAllBytes(localpath, dataBytes);

					cell.ImageView.Image = UIImage.LoadFromData(data);
				}*/

				cell.Accessory = UITableViewCellAccessory.None;

				return cell;
			}


		}
	}

	public class CustomVegeCell : UITableViewCell
	{
		UILabel headingLabel, subheadingLabel;
		UIImageView imageView;

		public CustomVegeCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{
			SelectionStyle = UITableViewCellSelectionStyle.Gray;
			ContentView.BackgroundColor = UIColor.FromRGB(234, 217, 136);
			imageView = new UIImageView();
			headingLabel = new UILabel()
			{
				Font = UIFont.FromName("Cochin-Bold", 22f),
				TextColor = UIColor.FromRGB(127, 51, 0),
				BackgroundColor = UIColor.Clear
			};
			subheadingLabel = new UILabel()
			{
				Font = UIFont.FromName("AmericanTypewriter", 12f),
				TextColor = UIColor.FromRGB(136, 117, 28),
				TextAlignment = UITextAlignment.Right,
				BackgroundColor = UIColor.Clear
			};
			ContentView.AddSubviews(new UIView[] { headingLabel, subheadingLabel, imageView });
			//ContentView.AddSubviews(new UIView[] { headingLabel, subheadingLabel });

		}
		public void UpdateCell(string caption, string subtitle, UIImage image)
		{
			imageView.Image = image;
			headingLabel.Text = caption;
			subheadingLabel.Text = subtitle;
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			imageView.Frame = new CGRect(ContentView.Bounds.Width - 63, 5, 0, 33);
			headingLabel.Frame = new CGRect(5, 4, ContentView.Bounds.Width - 63, 25);
			subheadingLabel.Frame = new CGRect(100, 18, 200, 20);
		}
	}

}


