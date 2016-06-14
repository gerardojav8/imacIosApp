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

namespace icom
{
	public partial class MaquinasController : UIViewController
	{
		public MaquinasController() : base("MaquinasController", null)
		{
		}

		HttpClient client;
		List<clsMaquinas> lstMaqServ;
		public string token
		{
			get;
			set;
		}

		List<String> lstItems = new List<String>();
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			lstItems.Add("Maquina 1");
			lstItems.Add("Maquina 2");
			lstItems.Add("Maquina 3");

			fuentetabla source = new fuentetabla(lstItems.ToArray());
			lstMaquinas.Source = source;


			btnTest.TouchUpInside += delegate
			{
				WillBeginTableEditing(lstMaquinas);
			};

			clsMaquinas obj1 = new clsMaquinas();
			obj1.noserie = "1234568";
			obj1.noeconomico = 1234;
			obj1.marca = "mercedes venz";
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

			//await getAllMaquinas();

		}

		public void WillBeginTableEditing(UITableView tableView)
		{
			tableView.BeginUpdates();
			// insert the 'ADD NEW' row at the end of table display
			tableView.InsertRows(new NSIndexPath[] {
			NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (0), 0)
			}, UITableViewRowAnimation.Fade);
			// create a new item and add it to our underlying data (it is not intended to be permanent)
			lstItems.Add("(add new)");
			fuentetabla source = new fuentetabla(lstItems.ToArray());
			lstMaquinas.Source = source;
			tableView.EndUpdates(); // applies the changes
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

	public class fuentetabla : TableSource
	{
		private string[] lstItems;
		public fuentetabla(string[] items) : base(items) {
			lstItems = items;
		}
		public override nint NumberOfSections(UITableView tableView)
		{
			return lstItems.Length;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			//return base.RowsInSection(tableview, section);
			return 2;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = base.GetCell(tableView, indexPath);
			cell.Accessory = UITableViewCellAccessory.DetailDisclosureButton;
			return cell;
		}

		public override void AccessoryButtonTapped(UITableView tableView, NSIndexPath indexPath)
		{
			new UIAlertView(lstItems[indexPath.Row].ToString(), "prueba",null, "Aceptar", null ).Show ();

			tableView.DeselectRow(indexPath, true);

		}
	}
	public class ExpandableTableSource<T> : UITableViewSource
	{
		public IReadOnlyList<T> Items;
		protected readonly Action<T> TSelected;
		protected readonly string ParentCellIdentifier = "ParentCell";
		protected readonly string ChildCellIndentifier = "ChildCell";
		protected int currentExpandedIndex = -1;

		public ExpandableTableSource() { }


		public ExpandableTableSource(Action<T> TSelected)
		{
			this.TSelected = TSelected;

		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return Items.Count + ((currentExpandedIndex > -1) ? 1 : 0);
		}

		void collapseSubItemsAtIndex(UITableView tableView, int index)
		{
			tableView.DeleteRows(new[] { NSIndexPath.FromRowSection(index + 1, 0) }, UITableViewRowAnimation.Fade);
		}

		void expandItemAtIndex(UITableView tableView, int index)
		{
			int insertPos = index + 1;
			tableView.InsertRows(new[] { NSIndexPath.FromRowSection(insertPos++, 0) }, UITableViewRowAnimation.Fade);
		}

		protected bool isChild(NSIndexPath indexPath)
		{
			return currentExpandedIndex > -1 &&
				   indexPath.Row > currentExpandedIndex &&
				   indexPath.Row <= currentExpandedIndex + 1;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (isChild(indexPath))
			{
				//Handle selection of child cell
				Console.WriteLine("You touched a child!");
				tableView.DeselectRow(indexPath, true);
				return;
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
				currentExpandedIndex = (shouldCollapse && indexPath.Row > currentExpandedIndex) ? indexPath.Row - 1 : indexPath.Row;
				this.expandItemAtIndex(tableView, currentExpandedIndex);
			}
			tableView.EndUpdates();
			tableView.DeselectRow(indexPath, true);
		}

		//TODO: implement this here?
		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			throw new NotImplementedException();
		}
	}

}


