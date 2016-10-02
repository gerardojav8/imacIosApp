using System;

using UIKit;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using CoreGraphics;
using icom.globales.ModalViewPicker;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using icom.globales;
using System.Text;
using System.Drawing;

namespace icom
{
	public partial class FiltroProduccionController : UIViewController
	{
		public FiltroProduccionController() : base("FiltroProduccionController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			btnBuscar.Layer.CornerRadius = 10;
			btnBuscar.ClipsToBounds = true;

			btnFechaini.TouchUpInside += DatePickerFechaIniButtonTapped;
			btnFechafin.TouchUpInside += DatePickerFechaFinButtonTapped;

			btnBuscar.TouchUpInside += delegate {
				ResultadosProduccionController viewres = new ResultadosProduccionController();
				viewres.folio = txtfolio.Text;
				viewres.material = txtmaterial.Text;
				viewres.cantidad = txtcantidad.Text;
				viewres.unidad = txtunidad.Text;
				viewres.cliente = txtcliente.Text;
				viewres.fechaini = txtfechaini.Text;
				viewres.fechafin = txtfechafinal.Text;

				viewres.Title = "Resultados Produccion";
				this.NavigationController.PushViewController(viewres, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			bajatecladoinputs();
		}

		async void DatePickerFechaIniButtonTapped(object sender, EventArgs e)
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
				txtfechaini.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}

		async void DatePickerFechaFinButtonTapped(object sender, EventArgs e)
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
				txtfechafinal.Text = dateFormatter.ToString(modalPicker.DatePicker.Date);
			};

			await PresentViewControllerAsync(modalPicker, true);
		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtcantidad.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			this.txtcantidad.InputAccessoryView = toolbar;
			
			txtfolio.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtmaterial.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtunidad.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtcliente.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };


		}
	}
}

