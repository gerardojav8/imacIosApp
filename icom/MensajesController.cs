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
using CoreAnimation;
using System.IO;


namespace icom
{
	public partial class MensajesController : UIViewController
	{
		List<Message> messages;
		ChatSource chatSource;
		Boolean blntecladoarriba = false;
		LoadingOverlay loadPop;
		HttpClient client;

		public MensajesController() : base("MensajesController", null)
		{
		}

		public  override void ViewDidLoad()
		{
			base.ViewDidLoad();
			messages = new List<Message>();
			tblChat.Layer.BorderColor = UIColor.Black.CGColor;
			tblChat.Layer.BorderWidth = (nfloat)2.0;

			/*Boolean resp = await getAllMensajes();

			if (resp)
			{
				loadPop.Hide();
				tblChat.ReloadData();
			}*/

			messages = new List<Message>() {
				new Message { Type = MessageType.IncomingFile, Text = "Hola", nombre = "Manuel Gamez", iniciales = "MG", fecha = "2012-01-01",hora = "12:00:00", filename="test.pdf", idmensaje="5" },
				new Message { Type = MessageType.Outgoing, Text = "Que onda", nombre = "", iniciales= "", fecha = "2012-01-01",hora = "12:00:00", filename="", idmensaje="" },
				new Message { Type = MessageType.Incoming, Text = "Mensaje de prueba", nombre = "Manuel Gamez", iniciales = "MG" , fecha = "2012-01-01",hora = "12:00:00", filename="", idmensaje="" },
				new Message { Type = MessageType.OutgoingFile, Text = "si si", nombre = "", iniciales= "", fecha = "2012-01-01",hora = "12:00:00" , filename="test.pdf", idmensaje="5" }
			};

			SetUpTableView();


			txtmensaje.Started += OnTextViewStarted;

			btnenviar.Layer.CornerRadius = 10;
			btnenviar.ClipsToBounds = true;

			btnArchivo.Layer.CornerRadius = 10;
			btnArchivo.ClipsToBounds = true;

			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, TecladoArriba);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, TecladoAbajo);

			txtmensaje.ShouldReturn += (txtUsuario) =>
			{
				((UITextField)txtUsuario).ResignFirstResponder();
				return true;
			};

			btnenviar.TouchUpInside += delegate {
				var text = txtmensaje.Text;
				txtmensaje.Text = string.Empty; // this will not generate change text event


				if (string.IsNullOrWhiteSpace(text))
					return;

				DateTime dtahora = DateTime.Now;

				var msg = new Message
				{
					Type = MessageType.Outgoing,
					Text = text.Trim(),
					nombre = "",
					iniciales = "",
					fecha = dtahora.Year + "-" + dtahora.Month + "-" + dtahora.Day,
					hora = dtahora.TimeOfDay.ToString(),
					filename = "",
					idmensaje = ""
						
				};


				messages.Add(msg);

				tblChat.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(messages.Count - 1, 0) }, UITableViewRowAnimation.None);
				ScrollToBottom(true);

				txtmensaje.EndEditing(true);



			};

			btnArchivo.TouchUpInside += delegate {
				String pathfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "primerplus.pdf");
				var bytes = default(byte[]);
				using (var streamReader = new StreamReader(pathfile)) {					
					using (var memstream = new MemoryStream()) {
						streamReader.BaseStream.CopyTo(memstream);
						bytes = memstream.ToArray();
					}
				}

				String strbase64 = Convert.ToBase64String(bytes);

				Console.WriteLine(strbase64);
			};

			ScrollToBottom(true);

		}

		public async Task<Boolean> getAllMensajes()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando Mensajes ...");
			View.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "controldeobras/getMensajesChat";
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
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI " + e.HResult);
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

				string mensaje = "error al traer los mensajes del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}




			foreach (var mensaje in jrarray)
			{
				Message objm = getobjMensaje(mensaje);
				messages.Add(objm);
			}


			return true;
		}

		private Message getobjMensaje(Object varjson) {

			Message objm = new Message();
			JObject json = (JObject)varjson;

			if (Consts.idusuarioapp.Equals(json["idusuario"].ToString()))
			{
				objm.Type = MessageType.Outgoing;
				objm.nombre = "";
				objm.iniciales = "";
			}
			else { 
				objm.Type = MessageType.Incoming;
				objm.nombre = json["nombre"].ToString();
				objm.iniciales = json["iniciales"].ToString();
			}

			objm.Text = json["mensaje"].ToString();
			objm.fecha = json["fecha"].ToString();
			objm.hora = json["hora"].ToString();

			return objm;
		}


		void OnTextViewStarted(object sender, EventArgs e)
		{
			ScrollToBottom(true);
		}


		void ScrollToBottom(bool animated)
		{
			if (tblChat.NumberOfSections() == 0)
				return;

			int items = (int)tblChat.NumberOfRowsInSection(0);
			if (items == 0)
				return;

			int finalRow = (int)NMath.Max(0, tblChat.NumberOfRowsInSection(0) - 1);
			NSIndexPath finalIndexPath = NSIndexPath.FromRowSection(finalRow, 0);
			tblChat.ScrollToRow(finalIndexPath, UITableViewScrollPosition.Top, animated);
		}

		void UpdateButtonState()
		{
			btnenviar.Enabled = !string.IsNullOrWhiteSpace(txtmensaje.Text);
		}


		void SetUpTableView()
		{
			
			tblChat.TranslatesAutoresizingMaskIntoConstraints = false;
			tblChat.AllowsSelection = false;
			tblChat.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			tblChat.RegisterClassForCellReuse(typeof(IncomingCell), IncomingCell.CellId);
			tblChat.RegisterClassForCellReuse(typeof(OutgoingCell), OutgoingCell.CellId);
			tblChat.RegisterClassForCellReuse(typeof(IncomingFileCell), IncomingFileCell.CellId);
			tblChat.RegisterClassForCellReuse(typeof(OutgoingFileCell), OutgoingFileCell.CellId);

			chatSource = new ChatSource(messages);
			tblChat.Source = chatSource;
		}

		private void TecladoArriba(NSNotification notif) {

			var r = UIKeyboard.FrameBeginFromNotification(notif);

			var keyboardHeight = r.Height;
			if (!blntecladoarriba)
			{
				
				CGRect newrect = new CGRect(View.Frame.X,
											View.Frame.Y - keyboardHeight,
											View.Frame.Width,
											View.Frame.Height);

				View.Frame = newrect;
				blntecladoarriba = true;
			}
			else {
				var rr = UIKeyboard.FrameEndFromNotification(notif);
				var hact = View.Frame.Y * -1;
				var hnew = rr.Height;
				var dif = hact - hnew;
				CGRect newrect = new CGRect(View.Frame.X,
											View.Frame.Y + dif,
											View.Frame.Width,
											View.Frame.Height);

				View.Frame = newrect;


			}

		}

		private void TecladoAbajo(NSNotification notif)
		{

			var r = UIKeyboard.FrameBeginFromNotification(notif);
			var keyboardHeight = r.Height;
			CGRect newrect = new CGRect(View.Frame.X,
										View.Frame.Y + keyboardHeight,
										View.Frame.Width,
										View.Frame.Height);

			View.Frame = newrect;
			blntecladoarriba = false;

		}
	}
}


