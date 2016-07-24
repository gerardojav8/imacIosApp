using System;

using UIKit;
using System.Collections.Generic;
using Foundation;
using CoreGraphics;

namespace icom
{
	public partial class DetalleAgendaController : UIViewController
	{
		public UIViewController viewagenda { get; set; }
		public int idagenda { get; set; }
		public int idevento { get; set; }

		List<Message> messages;
		ChatSource chatSource;

		List<clsCmbUsuarios> lstusuarios;
		UIActionSheet actUsuarios;
		Boolean blnTecladoArriba = false;
		double heightact = 0;

		public DetalleAgendaController() : base("DetalleAgendaController", null)
		{
		}

		public override void ViewDidLoad()
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
				new Message { Type = MessageType.Incoming, Text = "Hola" },
				new Message { Type = MessageType.Outgoing, Text = "Que onda" },
				new Message { Type = MessageType.Incoming, Text = "Mensaje de prueba" },
				new Message { Type = MessageType.Outgoing, Text = "si si" }
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
					Text = text.Trim()
				};

				messages.Add(msg);
				tblChatDetalleAgencia.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(messages.Count - 1, 0) }, UITableViewRowAnimation.None);
				ScrollToBottom(true);
				txtChatDetalleAgencia.EndEditing(true);

			};

			ScrollToBottom(true);

			clsCmbUsuarios objus1 = new clsCmbUsuarios();

			objus1.nombre = "Gerardo Javier";
			objus1.apepaterno = "Gamez";
			objus1.apematerno = "Vazquez";
			objus1.idusuario = 1;

			clsCmbUsuarios objus2 = new clsCmbUsuarios();

			objus2.nombre = "Fermin";
			objus2.apepaterno = "Mojica";
			objus2.apematerno = "Araujo";
			objus2.idusuario = 2;

			lstusuarios = new List<clsCmbUsuarios>();
			lstusuarios.Add(objus1);
			lstusuarios.Add(objus2);

			inicializaCombos();

			txtChatDetalleAgencia.ShouldReturn += (txtUsuario) =>
			{
				((UITextField)txtUsuario).ResignFirstResponder();
				return true;
			};
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

			//--------Combo Reporto---------------------
			actUsuarios = new UIActionSheet("Usuarios en el evento");
			foreach (clsCmbUsuarios us in lstusuarios)
			{
				String nombre = us.nombre + " " + us.apepaterno + " " + us.apematerno;
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


