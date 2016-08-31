using System;

using UIKit;
using Foundation;
using CoreGraphics;
using System.Drawing;

namespace icom
{
	public partial class CategoriasAltaController : UIViewController
	{
		public CategoriasAltaController() : base("CategoriasAltaController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, TecladoArriba);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, TecladoAbajo);

			txtComentarios.Layer.BorderColor = UIColor.Black.CGColor;
			txtComentarios.Layer.BorderWidth = (nfloat)2.0;
			txtComentarios.Text = "";

			bajatecladoinputs();

		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtComentarios.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			txtComentarios.InputAccessoryView = toolbar;


			txtNombreCategoria.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };


		}

		double ajuste = 160;
		Boolean blntecladoarriba = false;

		private void TecladoArriba(NSNotification notif)
		{

			if (txtComentarios.IsFirstResponder)
			{
				
				var r = UIKeyboard.FrameBeginFromNotification(notif);

				var keyboardHeight = r.Height;
				if (!blntecladoarriba)
				{
					var desface = (View.Frame.Y - keyboardHeight) + ajuste;
					CGRect newrect = new CGRect(View.Frame.X,
												desface,
												View.Frame.Width,
												View.Frame.Height);

					View.Frame = newrect;
					blntecladoarriba = true;
				}
				else {
					var rr = UIKeyboard.FrameEndFromNotification(notif);
					var hact = View.Frame.Y * -1;
					var hnew = rr.Height;
					var dif = hact - hnew;
					var desface = (View.Frame.Y + dif) + ajuste;
					CGRect newrect = new CGRect(View.Frame.X,
												desface,
												View.Frame.Width,
												View.Frame.Height);

					View.Frame = newrect;


				}
			}

		}

		private void TecladoAbajo(NSNotification notif)
		{
			if (blntecladoarriba)
			{
				var r = UIKeyboard.FrameBeginFromNotification(notif);
				var keyboardHeight = r.Height;
				var desface = View.Frame.Y + keyboardHeight - ajuste;
				CGRect newrect = new CGRect(View.Frame.X,
											desface,
											View.Frame.Width,
											View.Frame.Height);

				View.Frame = newrect;
				blntecladoarriba = false;
			}

		}
	}
}


