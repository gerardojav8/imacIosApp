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


namespace icom
{
	public partial class MensajesController : UIViewController
	{
		List<Message> messages;
		ChatSource chatSource;
		Boolean blntecladoarriba = false;

		public MensajesController() : base("MensajesController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			messages = new List<Message>() {
				new Message { Type = MessageType.Incoming, Text = "Hola" },
				new Message { Type = MessageType.Outgoing, Text = "Que onda" },
				new Message { Type = MessageType.Incoming, Text = "Mensaje de prueba" },
				new Message { Type = MessageType.Outgoing, Text = "si si" },
				new Message { Type = MessageType.Incoming, Text = "Mas pruebas" },
				new Message { Type = MessageType.Outgoing, Text = "Este es otro mensaje de prueba" },
				new Message { Type = MessageType.Incoming, Text = "Excelente" },
				new Message { Type = MessageType.Outgoing, Text = "Prueba de conversacion" },
				new Message { Type = MessageType.Incoming, Text = "Mas pruebas" },
			};

			tblChat.Layer.BorderColor = UIColor.Black.CGColor;
			tblChat.Layer.BorderWidth = (nfloat)2.0;
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

				var msg = new Message
				{
					Type = MessageType.Outgoing,
					Text = text.Trim()
				};

				messages.Add(msg);
				tblChat.InsertRows(new NSIndexPath[] { NSIndexPath.FromRowSection(messages.Count - 1, 0) }, UITableViewRowAnimation.None);
				ScrollToBottom(true);

				txtmensaje.EndEditing(true);



			};

			ScrollToBottom(true);

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
			//View.BringSubviewToFront(viewbarrainf);
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
			//View.BringSubviewToFront(viewbarrainf);
		}
	}
}


