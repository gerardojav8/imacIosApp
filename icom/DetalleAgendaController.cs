using System;

using UIKit;

namespace icom
{
	public partial class DetalleAgendaController : UIViewController
	{
		public UIViewController viewagenda { get; set; }
		public int idagenda { get; set; }
		public int idevento { get; set; }


		public DetalleAgendaController() : base("DetalleAgendaController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			if (UIScreen.MainScreen.Bounds.Width == 414)
			{
				scrDetalleAgencia.ContentSize = new CoreGraphics.CGSize(355, 1200);
			}
			else {
				scrDetalleAgencia.ContentSize = new CoreGraphics.CGSize(316, 1200);
			}
			txtchatgeneralDetalleAgencia.Layer.BorderColor = UIColor.Black.CGColor;
			txtchatgeneralDetalleAgencia.Layer.BorderWidth = (nfloat)2.0;
			txtchatgeneralDetalleAgencia.Text = "";
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


