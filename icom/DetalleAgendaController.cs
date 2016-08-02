using System;

using UIKit;
using System.Collections.Generic;
using Foundation;
using CoreGraphics;
using System.Threading.Tasks;
using System.Net.Http;
using icom.globales;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace icom
{
	public partial class DetalleAgendaController : UIViewController
	{
		LoadingOverlay loadPop;
		HttpClient client;

		public UIViewController viewagenda { get; set; }
		public int idevento { get; set; }


		List<Message> messages;
		ChatSource chatSource;

		List<String> lstusuarios;
		UIActionSheet actUsuarios;
		Boolean blnTecladoArriba = false;
		double heightact = 0;



		public DetalleAgendaController() : base("DetalleAgendaController", null)
		{
		}

		public async override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (UIScreen.MainScreen.Bounds.Width == 414)
			{
				scrDetalleAgencia.ContentSize = new CoreGraphics.CGSize(355, 905);
			}
			else {
				scrDetalleAgencia.ContentSize = new CoreGraphics.CGSize(316, 1075);
			}

			btnUsuarios.Layer.CornerRadius = 10;
			btnUsuarios.ClipsToBounds = true;

			messages = new List<Message>() {
				new Message { Type = MessageType.Incoming, Text = "Hola", nombre = "Manuel Gamez", iniciales = "MG", fecha = "2012-01-01",hora = "12:00:00" },
				new Message { Type = MessageType.Outgoing, Text = "Que onda", nombre = "", iniciales= "", fecha = "2012-01-01",hora = "12:00:00" },
				new Message { Type = MessageType.Incoming, Text = "Mensaje de prueba", nombre = "Manuel Gamez", iniciales = "MG" , fecha = "2012-01-01",hora = "12:00:00" },
				new Message { Type = MessageType.Outgoing, Text = "si si", nombre = "", iniciales= "", fecha = "2012-01-01",hora = "12:00:00"  }
			};

			tblChatDetalleAgencia.Layer.BorderColor = UIColor.Black.CGColor;
			tblChatDetalleAgencia.Layer.BorderWidth = (nfloat)2.0;
			SetUpTableView();

			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, TecladoArriba);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, TecladoAbajo);

			txtChatDetalleAgencia.Started += OnTextViewStarted;

			btnEnviar.TouchUpInside += delegate
			{
				var text = txtChatDetalleAgencia.Text;
				txtChatDetalleAgencia.Text = string.Empty; // this will not generate change text event


				if (string.IsNullOrWhiteSpace(text))
					return;

				var msg = new Message
				{
					Type = MessageType.Outgoing,
					Text = text.Trim(),
					nombre = "",
					iniciales = "",
					fecha = "2012-01-01",
					hora = "12:00:00"
				};

				messages.Add(msg);
				tblChatDetalleAgencia.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(messages.Count - 1, 0) }, UITableViewRowAnimation.None);
				ScrollToBottom(true);
				txtChatDetalleAgencia.EndEditing(true);

			};

			ScrollToBottom(true);
			lstusuarios = new List<String>();

			clsDetalleEventoAgenda objde = await getDetalleEventoAgenda();
			if (objde != null) {

				lblComentario.Text = objde.titulo;
				lblLapso.Text = objde.lapso;
				lstusuarios = objde.usuarios;					
				inicializaCombos();
			}

			/*
			lstusuarios.Add("Gerardo Javier Gamez Vazquez");
			lstusuarios.Add("Fermin Mojica Araujo");
			*/



			txtChatDetalleAgencia.ShouldReturn += (txtUsuario) =>
			{
				((UITextField)txtUsuario).ResignFirstResponder();
				return true;
			};
		}

		public async Task<clsDetalleEventoAgenda> getDetalleEventoAgenda()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "controldeobras/getEventoAgenda";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> obj = new Dictionary<string, string>();
			obj.Add("idevento", idevento.ToString());
			var json = JsonConvert.SerializeObject(obj);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
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
				return null;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			var jsonresponse = JObject.Parse(responseString);

			var jtokenerror = jsonresponse["error_description"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return null;
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return null;
			}

			clsDetalleEventoAgenda objde = new clsDetalleEventoAgenda();

			objde.mes = Int32.Parse(jsonresponse["mes"].ToString());
			objde.dia = Int32.Parse(jsonresponse["dia"].ToString());
			objde.titulo = jsonresponse["titulo"].ToString();
			objde.comentario = jsonresponse["comentario"].ToString();
			objde.lapso = jsonresponse["lapso"].ToString();

			JArray jarrAsistentes = JArray.Parse(jsonresponse["usuarios"].ToString());
			List<String> lstAsistentes = new List<String>();
			foreach (var jas in jarrAsistentes)
			{
				JObject jsonasistente = (JObject)jas;
				lstAsistentes.Add(jsonasistente["nombre"].ToString());
			}

			objde.usuarios = lstAsistentes;

			return objde;
		}

		private void TecladoArriba(NSNotification notif)
		{

			var r = UIKeyboard.FrameBeginFromNotification(notif);
			var keyboardHeight = r.Height;


			if (!blnTecladoArriba)
			{
				CGRect newrect = new CGRect(viewBarraChat.Frame.X,
												viewBarraChat.Frame.Y - keyboardHeight,
												viewBarraChat.Frame.Width,
												viewBarraChat.Frame.Height);

				viewBarraChat.Frame = newrect;
				blnTecladoArriba = true;
				heightact = keyboardHeight;
			}else{ 
				var rr = UIKeyboard.FrameEndFromNotification(notif);
				var hact = heightact;
				var hnew = rr.Height;
				var dif = hact - hnew;

				Console.WriteLine("hact " + hact.ToString());
				Console.WriteLine("hnew " + hnew.ToString());
				Console.WriteLine("dif " + dif.ToString());

				CGRect newrect = new CGRect(viewBarraChat.Frame.X,
											viewBarraChat.Frame.Y + dif,
											viewBarraChat.Frame.Width,
											viewBarraChat.Frame.Height);

				viewBarraChat.Frame = newrect;
				heightact = hnew;
			}
		}

		private void TecladoAbajo(NSNotification notif)
		{

			var r = UIKeyboard.FrameBeginFromNotification(notif);
			var keyboardHeight = r.Height;
			CGRect newrect = new CGRect(viewBarraChat.Frame.X,
										viewBarraChat.Frame.Y + keyboardHeight,
										viewBarraChat.Frame.Width,
										viewBarraChat.Frame.Height);

			viewBarraChat.Frame = newrect;
			blnTecladoArriba = false;
			heightact = 0;

		}



		public void inicializaCombos()
		{

			actUsuarios = new UIActionSheet("Usuarios en el evento");
			foreach (String us in lstusuarios)
			{
				String nombre = us;
				actUsuarios.Add(nombre);
			}
			actUsuarios.Add("Cancelar");

			actUsuarios.Style = UIActionSheetStyle.BlackTranslucent;
			actUsuarios.CancelButtonIndex = lstusuarios.Count;

			btnUsuarios.TouchUpInside += delegate
			{
				actUsuarios.ShowInView(ContentDetalleAgencia);
			};

			actUsuarios.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstusuarios.Count)
				{
	
				}
				else {
	
				}
			};
		}

		void SetUpTableView()
		{

			tblChatDetalleAgencia.TranslatesAutoresizingMaskIntoConstraints = false;
			tblChatDetalleAgencia.AllowsSelection = false;
			tblChatDetalleAgencia.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			tblChatDetalleAgencia.RegisterClassForCellReuse(typeof(IncomingCell), IncomingCell.CellId);
			tblChatDetalleAgencia.RegisterClassForCellReuse(typeof(OutgoingCell), OutgoingCell.CellId);

			chatSource = new ChatSource(messages);
			tblChatDetalleAgencia.Source = chatSource;
		}

		void OnTextViewStarted(object sender, EventArgs e)
		{
			ScrollToBottom(true);
		}


		void ScrollToBottom(bool animated)
		{
			if (tblChatDetalleAgencia.NumberOfSections() == 0)
				return;

			int items = (int)tblChatDetalleAgencia.NumberOfRowsInSection(0);
			if (items == 0)
				return;

			int finalRow = (int)NMath.Max(0, tblChatDetalleAgencia.NumberOfRowsInSection(0) - 1);
			NSIndexPath finalIndexPath = NSIndexPath.FromRowSection(finalRow, 0);
			tblChatDetalleAgencia.ScrollToRow(finalIndexPath, UITableViewScrollPosition.Top, animated);
		}

		void UpdateButtonState()
		{
			btnEnviar.Enabled = !string.IsNullOrWhiteSpace(txtChatDetalleAgencia.Text);
		}
	}
}


