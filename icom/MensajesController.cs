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
	public partial class MensajesController : UIViewController
	{
		List<Message> messages;
		ChatSource chatSource;

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

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
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
	}

	public class ChatSource2 : UITableViewSource
	{
		static readonly string idPersonaje = "Celda";
		protected readonly string ParentCellIdentifier = "ParentCell";
		protected readonly string ChildCellIndentifier = "ChildCell";
		protected int currentExpandedIndex = -1;
		protected UIViewController viewparent;
		protected Boolean sec = false;

		public ChatSource2(UIViewController view)
		{
			viewparent = view;
		}


		public override nint RowsInSection(UITableView tableview, nint section)
		{			
			return 3;
		}



		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			funciones.MessageBox("Aviso", "Seleccion");
		}

		public override nfloat GetHeightForHeader(UITableView tableView, nint section)
		{
			return (nfloat)0.0;
		}

		public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
		{
			
			return (nfloat)70.0;

		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{


			var cell = tableView.DequeueReusableCell(idPersonaje) as BubbleCell2;

			if (cell == null)
			{
				cell = new BubbleCell2((NSString)idPersonaje);
			}


			cell.UpdateCell("hola adfadfafdfds adfafadfs");


			cell.Accessory = UITableViewCellAccessory.None;
			sec = !sec;
			return cell;


		}
	}

	public class BubbleCell2 : UITableViewCell
	{
		UILabel lblmensaje;
		UIImageView imageView;


		public BubbleCell2(NSString cellId) : base(UITableViewCellStyle.Default, cellId)
		{


			imageView = new UIImageView();
			lblmensaje = new UILabel()
			{
				Font = UIFont.FromName("Arial", 15f),
				//TextColor = UIColor.FromRGB(54, 74, 97),
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear
			};

			ContentView.AddSubviews(new UIView[] { imageView, lblmensaje});

		}

		public void UpdateCell(string strmensaje)
		{
			
			lblmensaje.Text = strmensaje;
			imageView.Image = CreateBubbleWithBorder(UIColor.FromRGB(82, 121, 174));
		}

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			float ancho = 150;
			imageView.Frame = new CGRect(4, 4,  ancho, 50);
			lblmensaje.Frame = new CGRect(imageView.Bounds.X + 30,4, ancho,50);
		}

		protected static UIImage CreateColoredImage(UIColor color, UIImage mask)
		{
			var rect = new CGRect(CGPoint.Empty, mask.Size);
			UIGraphics.BeginImageContextWithOptions(mask.Size, false, mask.CurrentScale);
			CGContext context = UIGraphics.GetCurrentContext();
			mask.DrawAsPatternInRect(rect);
			context.SetFillColor(color.CGColor);
			context.SetBlendMode(CGBlendMode.SourceAtop);
			context.FillRect(rect);
			UIImage result = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();
			return result;
		}

		protected static UIImage CreateBubbleWithBorder(UIColor bubbleColor)
		{
			UIImage bubbleImg = UIImage.FromBundle("BubbleIncoming");
			bubbleImg = CreateColoredImage(bubbleColor, bubbleImg);
			CGSize size = bubbleImg.Size;

			UIGraphics.BeginImageContextWithOptions(size, false, 0);
			var rect = new CGRect(CGPoint.Empty, size);
			bubbleImg.Draw(rect);

			var result = UIGraphics.GetImageFromCurrentImageContext();
			UIGraphics.EndImageContext();

			return result;
		}

	}
}


