using System;

using UIKit;

namespace icom
{
	public partial class Login : UIViewController
	{
		public Login () : base ("Login", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			btnLogin.TouchUpInside += delegate {
				Principal viewprin = new Principal();
				viewprin.Title = "I.C.O.M";


				this.NavigationController.PushViewController(viewprin, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View,true);
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


