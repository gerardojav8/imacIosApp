using System;

using UIKit;

namespace icom
{
	public partial class Principal : UIViewController
	{
		public Principal () : base ("Principal", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			btnMaquinaria.TouchUpInside += delegate {
				//Maquinaria viewmaq = new Maquinaria();
				//viewmaq.Title = "Maquinaria";

				MaquinasController viewmaq = new MaquinasController();

				this.NavigationController.PushViewController(viewmaq, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnProduccion.TouchUpInside += delegate {
				Produccion viewprod = new Produccion();
				viewprod.Title = "Produccion";


				this.NavigationController.PushViewController(viewprod, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnCtrlObra.TouchUpInside += delegate {
				CtrlObra viewctrlobra = new CtrlObra();
				viewctrlobra.Title = "Control de Obra";


				this.NavigationController.PushViewController(viewctrlobra, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnInformacion.TouchUpInside += delegate {
				Informacion viewinfo = new Informacion();
				viewinfo.Title = "Informacion";


				this.NavigationController.PushViewController(viewinfo, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnCerrarSesion.TouchUpInside += delegate {
				this.NavigationController.PopToRootViewController(true);
			};

			lblUsuario.Text = "Gerardo Javier Gamez Vazquez";
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


