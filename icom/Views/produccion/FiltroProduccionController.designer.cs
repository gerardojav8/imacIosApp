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
	[Register ("FiltroProduccionController")]
	partial class FiltroProduccionController
	{
		[Outlet]
		UIKit.UIButton btnBuscar { get; set; }

		[Outlet]
		UIKit.UIButton btnFechafin { get; set; }

		[Outlet]
		UIKit.UIButton btnFechaini { get; set; }

		[Outlet]
		UIKit.UITextField txtcantidad { get; set; }

		[Outlet]
		UIKit.UITextField txtcliente { get; set; }

		[Outlet]
		UIKit.UITextField txtfechafinal { get; set; }

		[Outlet]
		UIKit.UITextField txtfechaini { get; set; }

		[Outlet]
		UIKit.UITextField txtfolio { get; set; }

		[Outlet]
		UIKit.UITextField txtmaterial { get; set; }

		[Outlet]
		UIKit.UITextField txtunidad { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtfolio != null) {
				txtfolio.Dispose ();
				txtfolio = null;
			}

			if (txtmaterial != null) {
				txtmaterial.Dispose ();
				txtmaterial = null;
			}

			if (txtcantidad != null) {
				txtcantidad.Dispose ();
				txtcantidad = null;
			}

			if (txtunidad != null) {
				txtunidad.Dispose ();
				txtunidad = null;
			}

			if (txtcliente != null) {
				txtcliente.Dispose ();
				txtcliente = null;
			}

			if (txtfechaini != null) {
				txtfechaini.Dispose ();
				txtfechaini = null;
			}

			if (txtfechafinal != null) {
				txtfechafinal.Dispose ();
				txtfechafinal = null;
			}

			if (btnFechaini != null) {
				btnFechaini.Dispose ();
				btnFechaini = null;
			}

			if (btnFechafin != null) {
				btnFechafin.Dispose ();
				btnFechafin = null;
			}

			if (btnBuscar != null) {
				btnBuscar.Dispose ();
				btnBuscar = null;
			}
		}
	}
}
