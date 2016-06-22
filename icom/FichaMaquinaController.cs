using System;

using UIKit;

namespace icom
{
	public partial class FichaMaquinaController : UIViewController
	{
		public UIViewController viewmaq { get; set; }
		public String noserie { get; set; }

		public FichaMaquinaController() : base("FichaMaquinaController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			scrViewFichaMaquina.ContentSize = new CoreGraphics.CGSize(375, 1883);

			txtEstadoFisicoAct.Layer.BorderColor = UIColor.Black.CGColor;
			txtEstadoFisicoAct.Layer.BorderWidth = (nfloat)2.0;
			txtEstadoFisicoAct.Text = "";

			imgMaq.Layer.BorderColor = UIColor.Black.CGColor;
			imgMaq.Layer.BorderWidth = (nfloat)2.0;
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


