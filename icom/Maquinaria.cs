using System;

using UIKit;

namespace icom
{
	public partial class Maquinaria : UIViewController
	{
		public Maquinaria () : base ("Maquinaria", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();



			btnReporteOperador.TouchUpInside += delegate {
				ReporteOperador viewro = new ReporteOperador();
				viewro.Title = "Reporte Operador";


				this.NavigationController.PushViewController(viewro, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};

			btnReporteServicio.TouchUpInside += delegate {
				ReporteServicio viewrs = new ReporteServicio();
				viewrs.Title = "Reporte Servicio";


				this.NavigationController.PushViewController(viewrs, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View,true);
				UIView.CommitAnimations();
			};


		}
			

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
				
	}
}


