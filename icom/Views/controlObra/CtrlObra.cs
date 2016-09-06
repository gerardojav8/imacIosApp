using System;

using UIKit;

namespace icom
{
	public partial class CtrlObra : UIViewController
	{
		public CtrlObra () : base ("CtrlObra", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			btnMensajes.TouchUpInside += delegate {
				MensajesController viewmen = new MensajesController();
				viewmen.Title = "Mensajes";

				this.NavigationController.PushViewController(viewmen, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnAgenda.TouchUpInside += delegate {
				AgendaController viewagen = new AgendaController();
				viewagen.Title = "Agenda";

				this.NavigationController.PushViewController(viewagen, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnPlanificador.TouchUpInside += delegate {
				ObrasController viewobras = new ObrasController();
				viewobras.Title = "Obras";

				this.NavigationController.PushViewController(viewobras, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlDown, NavigationController.View, true);
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


