using System;
using System.Collections.Generic;
using UIKit;
using icom.globales.ModalViewPicker;
using Foundation;


namespace icom
{
	public partial class ObrasController : UIViewController
	{
		public ObrasController() : base("ObrasController", null)
		{
		}

		private List<clsListadoObra> lstObras;
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			lstObras = new List<clsListadoObra>();

			clsListadoObra obj1 = new clsListadoObra
			{
				idobra = 1,
				nombre = "obra 1",
				porcentajeavance = 25,
				noclasificacioens = 3
			};

			clsListadoObra obj2 = new clsListadoObra
			{
				idobra = 1,
				nombre = "obra 2",
				porcentajeavance = 98,
				noclasificacioens = 3
			};

			clsListadoObra obj3 = new clsListadoObra
			{
				idobra = 1,
				nombre = "obra 3",
				porcentajeavance = 75,
				noclasificacioens = 3
			};

			clsListadoObra obj4 = new clsListadoObra
			{
				idobra = 1,
				nombre = "obra 4",
				porcentajeavance = 50,
				noclasificacioens = 3
			};

			lstObras.Add(obj1);
			lstObras.Add(obj2);
			lstObras.Add(obj3);
			lstObras.Add(obj4);

			tblObras.Source = new FuenteTablaObras(this, lstObras);

			btnModificarObra.TouchUpInside += delegate {
				NSIndexPath indexPath = tblObras.IndexPathForSelectedRow;
				if (indexPath != null){
					ModificarObraController viewmodobra = new ModificarObraController();


					viewmodobra.Title = "Modifica Obra";
					this.NavigationController.PushViewController(viewmodobra, false);
					UIView.BeginAnimations(null);
					UIView.SetAnimationDuration(0.7);
					UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
					UIView.CommitAnimations();
				}else {
					funciones.MessageBox("Aviso", "Ninguna celda seleccionada");
				}

				tblObras.DeselectRow(indexPath, true);
			};

			btnNuevaObra.TouchUpInside += delegate {
				AltaObraController viewaobras = new AltaObraController();


				viewaobras.Title = "Nueva Obra";
				this.NavigationController.PushViewController(viewaobras, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();
			};

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


