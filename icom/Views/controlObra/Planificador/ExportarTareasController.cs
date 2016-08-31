using System;

using UIKit;
using Foundation;
using CoreGraphics;
using icom.globales.ModalViewPicker;
using System.Drawing;

namespace icom
{
	public partial class ExportarTareasController : UIViewController
	{
		public ExportarTareasController() : base("ExportarTareasController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			btnDesde.TouchUpInside += DatePickerFechaInicio;
			btnHasta.TouchUpInside += DatePickerFechafin;
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


				modalPicker.DatePicker.Mode = UIDatePickerMode.Date;
				modalPicker.OnModalPickerDismissed += (s, ea) =>
				{
					var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };					

					NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
					dateFormatterFecha.Locale = locale;

					txtDesde.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date);
				};

			await PresentViewControllerAsync(modalPicker, true);
		}

		async void DatePickerFechafin(object sender, EventArgs e)
		{
			var modalPicker = new ModalPickerViewController(ModalPickerType.Date, "Elije Fecha", this)
			{
				HeaderBackgroundColor = UIColor.Red,
				HeaderTextColor = UIColor.White,
				TransitioningDelegate = new ModalPickerTransitionDelegate(),
				ModalPresentationStyle = UIModalPresentationStyle.Custom
			};

			modalPicker.DatePicker.Mode = UIDatePickerMode.Date;

			modalPicker.OnModalPickerDismissed += (s, ea) =>
			{
				var dateFormatterFecha = new NSDateFormatter() { DateFormat = "yyyy-MM-dd" };


				NSLocale locale = NSLocale.FromLocaleIdentifier("es_MX");
				dateFormatterFecha.Locale = locale;


				txtHasta.Text = dateFormatterFecha.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}
	}
}


