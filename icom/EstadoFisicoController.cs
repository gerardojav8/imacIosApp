using System;

using UIKit;

namespace icom
{
	public partial class EstadoFisicoController : UIViewController
	{
		public EstadoFisicoController() : base("EstadoFisicoController", null)
		{
		}
		public UIViewController viewft
		{
			get;
			set;
		}
		public String titulo
		{
			get;
			set;
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			txtComentarios.Layer.BorderColor = UIColor.Black.CGColor;
			txtComentarios.Layer.BorderWidth = (nfloat)2.0;
			txtComentarios.Text = "";

			lbltitulo.Text = titulo;

			btnGuardarEF.TouchUpInside += delegate {
				this.NavigationController.PopToViewController(viewft, true);
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


