using System;

using UIKit;
using Foundation;
using CoreGraphics;
using icom.globales.ModalViewPicker;
using System.Drawing;
namespace icom
{
	public partial class NuevaTareaController : UIViewController
	{
		public NuevaTareaController() : base("NuevaTareaController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.DidShowNotification, TecladoArriba);
			NSNotificationCenter.DefaultCenter.AddObserver(UIKeyboard.WillHideNotification, TecladoAbajo);


			txtNotas.Layer.BorderColor = UIColor.Black.CGColor;
			txtNotas.Layer.BorderWidth = (nfloat)2.0;
			txtNotas.Text = "";

			swTodoDia.On = false;
			swTodoDia.ValueChanged += delegate
			{
				if (swTodoDia.On)
				{					
					if (!txtInicio.Text.Equals(""))
					{
						txtFinal.Text = txtInicio.Text.Substring(0, 10) + " 23:59:59";
						txtInicio.Text = txtInicio.Text.Substring(0, 10) + " 00:00:00";
					}
					else {
						txtFinal.Text = "";
					}
					btnFinal.Enabled = false;
				}
				else {
					
					txtInicio.Text = "";
					txtFinal.Text = "";
					btnFinal.Enabled = true;
				}
			};

			btnInicio.TouchUpInside += DatePickerFechaInicio;
			btnFinal.TouchUpInside += DateTimePikerFechafin;

			bajatecladoinputs();
		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;


			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtNotas.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			txtNotas.InputAccessoryView = toolbar;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtPorcentaje.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			txtPorcentaje.InputAccessoryView = toolbar;


			txtTitulo.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		async void DatePickerFechaInicio(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije una Fecha", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			if (!swTodoDia.On)
			{
				modalPicker.DatePicker.Mode = UIDatePickerMode.DateAndTime;
				modalPicker.OnModalPickerDismissed += (s, ea) =>
				{
					var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };
					var dateFormatterHora = new NSDateFormatter() { DateFormat = "HH:mm:ss" };

					NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
					dateFormatterFecha.Locale = locale;
					dateFormatterHora.Locale = locale;

					txtInicio.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date) + " " + dateFormatterHora.ToString(modalPicker.DatePicker.Date);
				};
			}
			else {
				modalPicker.DatePicker.Mode = UIDatePickerMode.Date;
				modalPicker.OnModalPickerDismissed += (s, ea) =>
				{
					var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };

					NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
					dateFormatterFecha.Locale = locale;

					txtInicio.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date) + " 00:00:00";
					txtFinal.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date) + " 23:59:59";
				};
			}

			await PresentViewControllerAsync(modalPicker, true);
		}

		async void DateTimePikerFechafin(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije Fecha", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.DatePicker.Mode = UIDatePickerMode.DateAndTime;

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };
				var dateFormatterHora = new NSDateFormatter() { DateFormat = "HH:mm:ss" };

				NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
				dateFormatterFecha.Locale = locale;
				dateFormatterHora.Locale = locale;

				txtFinal.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date) + " " + dateFormatterHora.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}

		double ajuste = 7;
		Boolean blntecladoarriba = false;

		private void TecladoArriba(NSNotification notif)
		{

			if (txtNotas.IsFirstResponder || txtPorcentaje.IsFirstResponder)
			{
				if (txtPorcentaje.IsFirstResponder)
				{
					ajuste = 50;
				}
				else {
					ajuste = 7;
				}

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


