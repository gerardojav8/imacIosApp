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
using MobileCoreServices;
using Quobject.SocketIoClientDotNet.Client;
using QuickLook;


namespace icom
{
	public partial class MensajesController : UIViewController
	{
		List<Message> messages;
		ChatSource chatSource;
		Boolean blntecladoarriba = false;
		LoadingOverlay loadPop;
		HttpClient client;
		Socket socket;

		public MensajesController() : base("MensajesController", null)
		{
		}

		private void socketioinit() {
			socket = IO.Socket(Consts.urlserverchat);

			socket.On(Socket.EVENT_CONNECT, () =>
			{
				socket.Emit("hi");
			});

			socket.On("listenMessage", (data) =>
			{
				var json = JObject.Parse(data.ToString());
				UIApplication.SharedApplication.InvokeOnMainThread(delegate {
					agregaMensaje(json);		
				});
									
			});

		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();
			socketioinit();
			messages = new List<Message>();
			tblChat.Layer.BorderColor = UIColor.Black.CGColor;
			tblChat.Layer.BorderWidth = (nfloat)2.0;

			Boolean resp = await getAllMensajes();
			SetUpTableView();
			if (resp)
			{
				loadPop.Hide();
				tblChat.ReloadData();
			}



			/*messages.Add(new Message
			{
				Type = MessageType.IncomingFile,
				Text = "prueba de archivo",
				nombre = "Gerardo Javier Gamez Vazquez",
				iniciales = "GG",
				fecha = "2012-01-01",
				hora = " 12:00:00",
				filename = "primeraplus.pdf",
				idmensaje = "10026" 
			});*/

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
				
				Dictionary<string, string> datos = new Dictionary<string, string>();
				datos.Add("idusuario", Consts.idusuarioapp);
				datos.Add("mensaje", text);
				datos.Add("fecha", "");
				datos.Add("hora", "");
				datos.Add("filename", "");
				datos.Add("idmensaje", "");
				datos.Add("nombre", Consts.nombreusuarioapp);
				datos.Add("iniciales", Consts.inicialesusuarioapp);

				var json = JsonConvert.SerializeObject(datos);

				socket.Emit("newMessage", json);

				txtmensaje.EndEditing(true);

			};

			btnArchivo.TouchUpInside += mandaArchivoaServer;
			//btnArchivo.TouchUpInside += abreDocumento;
		

			ScrollToBottom(true);

		}


		async void mandaArchivoaServer(object sender, EventArgs e)
		{
			String namefile = "primerplus.pdf";
			String pathfile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), namefile);
			var bytes = default(byte[]);
			using (var streamReader = new StreamReader(pathfile))
			{
				using (var memstream = new MemoryStream())
				{
					streamReader.BaseStream.CopyTo(memstream);
					bytes = memstream.ToArray();
				}
			}

			String strbase64 = Convert.ToBase64String(bytes);

			int intidmensaje = await guardaMensajeArchivo(strbase64, namefile, txtmensaje.Text);
			if (intidmensaje > -1)
			{
				loadPop.Hide();
				Dictionary<string, string> datos = new Dictionary<string, string>();
				datos.Add("idusuario", Consts.idusuarioapp);
				datos.Add("mensaje", txtmensaje.Text);
				datos.Add("fecha", "");
				datos.Add("hora", "");
				datos.Add("filename", namefile);
				datos.Add("idmensaje", intidmensaje.ToString());
				datos.Add("nombre", Consts.nombreusuarioapp);
				datos.Add("iniciales", Consts.inicialesusuarioapp);

				var json = JsonConvert.SerializeObject(datos);

				socket.Emit("newMessage", json);

				txtmensaje.Text = string.Empty;
				txtmensaje.EndEditing(true);
			}

		}


		void agregaMensaje(JObject json) {
			
			String mensaje = json["mensaje"].ToString();
			String idusmensaje = json["idusuario"].ToString();
			String strfecha = json["fecha"].ToString();
			String strhora = json["hora"].ToString();
			String strfilename = json["filename"].ToString();
			String stridmensaje = json["idmensaje"].ToString();
			String strnombre = json["nombre"].ToString();
			String striniciales = json["iniciales"].ToString();

			MessageType tipomensaje;

			if (Consts.idusuarioapp.Equals(idusmensaje))
			{
				if (strfilename.Equals(""))
				{
					tipomensaje = MessageType.Outgoing;
				}
				else { 
					tipomensaje = MessageType.OutgoingFile;
				}

				strnombre = "";
				striniciales = "";
			}
			else {
				
				if (strfilename.Equals(""))
				{
					tipomensaje = MessageType.Incoming;
				}
				else {
					tipomensaje = MessageType.IncomingFile;
				}
			}

			var msg = new Message
			{
				Type = tipomensaje,
				Text = mensaje.Trim(),
				nombre = strnombre,
				iniciales = striniciales,
				fecha = strfecha,
				hora = strhora,
				filename = strfilename,
				idmensaje = stridmensaje

			};


			messages.Add(msg);


			tblChat.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(messages.Count - 1, 0) }, UITableViewRowAnimation.None);
			ScrollToBottom(true);
		}

		void abreDocumento(object s, EventArgs e)
		{
			// Allow the Document picker to select a range of document types
			var allowedUTIs = new string[] {
					UTType.UTF8PlainText,
					UTType.PlainText,
					UTType.RTF,
					UTType.PNG,
					UTType.Text,
					UTType.PDF,
					UTType.Image
				};

			// Display the picker
			//var picker = new UIDocumentPickerViewController (allowedUTIs, UIDocumentPickerMode.Open);
			var pickerMenu = new UIDocumentMenuViewController(allowedUTIs, UIDocumentPickerMode.Open);
			pickerMenu.DidPickDocumentPicker += (sender, args) =>
			{

					// Wireup Document Picker
					args.DocumentPicker.DidPickDocument += (sndr, pArgs) =>
				{

						// IMPORTANT! You must lock the security scope before you can
						// access this file
						var securityEnabled = pArgs.Url.StartAccessingSecurityScopedResource();

						// Open the document
						funciones.MessageBox("Aviso", pArgs.Url.ToString());

						// TODO: This should work but doesn't
						// Apple's WWDC 2014 sample project does this but it blows
						// up in Xamarin
						NSFileCoordinator fileCoordinator = new NSFileCoordinator();
					NSError err;
					fileCoordinator.CoordinateRead(pArgs.Url, 0, out err, (NSUrl newUrl) =>
					{
						NSData data = NSData.FromUrl(newUrl);
						Console.WriteLine("Data: {0}", data);
					});

						// IMPORTANT! You must release the security lock established
						// above.
						pArgs.Url.StopAccessingSecurityScopedResource();
				};

					// Display the document picker
					PresentViewController(args.DocumentPicker, true, null);
			};

			pickerMenu.ModalPresentationStyle = UIModalPresentationStyle.Popover;
			PresentViewController(pickerMenu, true, null);
			UIPopoverPresentationController presentationPopover = pickerMenu.PopoverPresentationController;
			if (presentationPopover != null)
			{
				presentationPopover.SourceView = this.View;
				presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Down;
				presentationPopover.SourceRect = ((UIButton)s).Frame;
			}
		}

		public async Task<Boolean> getAllMensajes()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Buscando Mensajes ...");
			View.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "controldeobras/getMensajesChat";
			var uri = new Uri(string.Format(url));

			var json = "";

			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2"))
			{
				funciones.SalirSesion(this);
				return false;
			}

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

		public async Task<int> guardaMensajeArchivo(String strarchivo, String strnombre, String strMensaje)
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Archivo ...");
			View.Add(loadPop);

			client = new HttpClient();
			string url = Consts.ulrserv + "controldeobras/guardaMensajeconArchivo";
			var uri = new Uri(string.Format(url));

			Dictionary<String, String> objpet = new Dictionary<string, string>();
			objpet.Add("archivo", strarchivo);
			objpet.Add("mensaje", strMensaje);
			objpet.Add("idusuario", Consts.idusuarioapp);
			objpet.Add("nombre", strnombre);
			var json = JsonConvert.SerializeObject(objpet);


			string responseString = string.Empty;
			responseString = await funciones.llamadaRest(client, uri, loadPop, json, Consts.token);

			if (responseString.Equals("-1") || responseString.Equals("-2"))
			{
				funciones.SalirSesion(this);
				return -1;
			}

			var jsonresponse = JObject.Parse(responseString);

			var jtokenerror = jsonresponse["error_description"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return -1;
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return -1;
			}

			int idmensaje = Int32.Parse(jsonresponse["idmensaje"].ToString());
			return idmensaje;
		}

		private Message getobjMensaje(Object varjson) {

			Message objm = new Message();
			JObject json = (JObject)varjson;
			objm.filename = json["nombrearchivo"].ToString();
			objm.idmensaje = json["idmensaje"].ToString();

			if (Consts.idusuarioapp.Equals(json["idusuario"].ToString()))
			{
				if (objm.filename.Equals(""))
				{
					objm.Type = MessageType.Outgoing;
				}
				else {
					objm.Type = MessageType.OutgoingFile;
				}

				objm.nombre = "";
				objm.iniciales = "";
			}
			else {
				if (objm.filename.Equals(""))
				{
					objm.Type = MessageType.Incoming;
				}
				else {
					objm.Type = MessageType.IncomingFile;
				}

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

			chatSource = new ChatSource(messages, this);
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


