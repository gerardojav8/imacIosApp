using System;

using UIKit;

namespace icom
{
	public partial class solicitudMaquinaController : UIViewController
	{
		public solicitudMaquinaController() : base("solicitudMaquinaController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();


			scrViewSolicitudMaquina.ContentSize = new CoreGraphics.CGSize(375, 1583);

			lstRequerimientos.Layer.BorderColor = UIColor.Black.CGColor;
			lstRequerimientos.Layer.BorderWidth = (nfloat)2.0;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


