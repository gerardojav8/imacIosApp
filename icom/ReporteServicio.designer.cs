// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace icom
{
	[Register ("ReporteServicio")]
	partial class ReporteServicio
	{
		[Outlet]
		UIKit.UIButton btnaddref { get; set; }

		[Outlet]
		UIKit.UIButton btnGuardar { get; set; }

		[Outlet]
		UIKit.UIButton btnLimpiarRefacciones { get; set; }

		[Outlet]
		UIKit.UIView ContentViewRepServicios { get; set; }

		[Outlet]
		UIKit.UITableView lstRefacciones { get; set; }

		[Outlet]
		UIKit.UIScrollView scrViewRepServicios { get; set; }

		[Outlet]
		UIKit.UITextField txtaddref { get; set; }

		[Outlet]
		UIKit.UITextView txtDescFalla { get; set; }

		[Outlet]
		UIKit.UITextView txtObservaciones { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnLimpiarRefacciones != null) {
				btnLimpiarRefacciones.Dispose ();
				btnLimpiarRefacciones = null;
			}

			if (btnaddref != null) {
				btnaddref.Dispose ();
				btnaddref = null;
			}

			if (btnGuardar != null) {
				btnGuardar.Dispose ();
				btnGuardar = null;
			}

			if (ContentViewRepServicios != null) {
				ContentViewRepServicios.Dispose ();
				ContentViewRepServicios = null;
			}

			if (lstRefacciones != null) {
				lstRefacciones.Dispose ();
				lstRefacciones = null;
			}

			if (scrViewRepServicios != null) {
				scrViewRepServicios.Dispose ();
				scrViewRepServicios = null;
			}

			if (txtaddref != null) {
				txtaddref.Dispose ();
				txtaddref = null;
			}

			if (txtDescFalla != null) {
				txtDescFalla.Dispose ();
				txtDescFalla = null;
			}

			if (txtObservaciones != null) {
				txtObservaciones.Dispose ();
				txtObservaciones = null;
			}
		}
	}
}
