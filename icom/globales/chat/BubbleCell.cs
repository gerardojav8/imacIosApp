using System;
using UIKit;
using CoreGraphics;
using Foundation;
namespace icom
{
	public abstract class BubbleCell : UITableViewCell
	{
		public UIImageView BubbleImageView { get; private set; }
		public UILabel MessageLabel { get; private set; }
		public UILabel UsuarioLabel { get; private set; }
		public UIImage BubbleImage { get; set; }
		public UIImage BubbleHighlightedImage { get; set; }
		public MessageType typebubble { get; set;}

		Message msg;

		public Message Message
		{
			get
			{
				return msg;
			}
			set
			{
				msg = value;
				BubbleImageView.Image = BubbleImage;
				BubbleImageView.HighlightedImage = BubbleHighlightedImage;

				MessageLabel.Font = UIFont.FromName("Arial", 12f);
				MessageLabel.Text = "Gerardo Javier Gamez Vazquez " + "\n" + "12-01-2012 12:00:00 :" + "\n" + msg.Text;
				MessageLabel.UserInteractionEnabled = true;

				UsuarioLabel.Font = UIFont.FromName("Arial-BoldMT", 12f);
				UsuarioLabel.Text = "   GG   ";
				UsuarioLabel.UserInteractionEnabled = true;
				UsuarioLabel.Layer.CornerRadius = 10;
				UsuarioLabel.ClipsToBounds = true;
				UsuarioLabel.BackgroundColor = UIColor.FromRGB(43, 119, 250);
				UsuarioLabel.TextColor = UIColor.White;


				BubbleImageView.UserInteractionEnabled = false;
			}
		}

		public BubbleCell(IntPtr handle)
			: base(handle)
		{
			Initialize();
		}

		public BubbleCell()
		{
			Initialize();
		}

		[Export("initWithStyle:reuseIdentifier:")]
		public BubbleCell(UITableViewCellStyle style, string reuseIdentifier)
			: base(style, reuseIdentifier)
		{			
			if (reuseIdentifier == IncomingCell.CellId)
			{
				typebubble = MessageType.Incoming;
			}
			else { 
				typebubble = MessageType.Outgoing;
			}

			Initialize();
		}

		void Initialize()
		{
			BubbleImageView = new UIImageView
			{
				TranslatesAutoresizingMaskIntoConstraints = false
			};
			MessageLabel = new UILabel				
			{
				TranslatesAutoresizingMaskIntoConstraints = false,
				Lines = 0,
				PreferredMaxLayoutWidth = 220f
			};
			UsuarioLabel = new UILabel
			{
				TranslatesAutoresizingMaskIntoConstraints = false
			};





			if (typebubble == MessageType.Incoming)
			{
				ContentView.AddSubviews(UsuarioLabel, BubbleImageView, MessageLabel);
			}
			else { 
				ContentView.AddSubviews(BubbleImageView, MessageLabel);
			}
		}

		public override void SetSelected(bool selected, bool animated)
		{
			base.SetSelected(selected, animated);
			BubbleImageView.Highlighted = selected;
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

		protected static UIImage CreateBubbleWithBorder(UIImage bubbleImg, UIColor bubbleColor)
		{
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

