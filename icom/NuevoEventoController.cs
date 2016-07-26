using System;

using UIKit;
using Foundation;
using icom.globales.ModalViewPicker;
using System.Drawing;

namespace icom
{
	public partial class NuevoEventoController : UIViewController
	{
		public NuevoEventoController() : base("NuevoEventoController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			btnfecha.TouchUpInside += DatePickerFechaEvento;
			btnhora.TouchUpInside += TimePikerHoraInicio;
			btnhorafin.TouchUpInside += TimePikerHoraFin;

			txtComentario.Layer.BorderColor = UIColor.Black.CGColor;
			txtComentario.Layer.BorderWidth = (nfloat)2.0;
			txtComentario.Text = "";

			txtTitulo.ShouldReturn += (txtUsuario) =>
			{
				((UITextField)txtUsuario).ResignFirstResponder();
				return true;
			};

			UIToolbar toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate
			{
				txtComentario.EndEditing(true);
			});

			toolbar.Items = new UIBarButtonItem[] {
				new UIBarButtonItem (UIBarButtonSystemItem.FlexibleSpace), doneButton
			};

			this.txtComentario.InputAccessoryView = toolbar;

		}






		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		async void DatePickerFechaEvento(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije una Fecha", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.DatePicker.Mode = UIDatePickerMode.Date;

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				var dateFormatter = new NSDateFormatter()
				{
					DateFormat = "yyyy-MM-dd"
				};

				NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
				dateFormatter.Locale = locale;
				txtfechaevento.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}

		async void TimePikerHoraInicio(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije una Hora", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.DatePicker.Mode = UIDatePickerMode.Time;

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				var dateFormatter = new NSDateFormatter()
				{
					DateFormat = "HH:mm:ss"
				};

				NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
				dateFormatter.Locale = locale;
				txthorainicio.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}

		async void TimePikerHoraFin(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije una Hora", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.DatePicker.Mode = UIDatePickerMode.Time;

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				var dateFormatter = new NSDateFormatter()
				{
					DateFormat = "HH:mm:ss"
				};

				NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
				dateFormatter.Locale = locale;
				txthorafin.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}
	}
}


