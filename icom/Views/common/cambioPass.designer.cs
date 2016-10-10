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
	[Register ("cambioPass")]
	partial class cambioPass
	{
		[Outlet]
		UIKit.UIButton btnAceptar { get; set; }

		[Outlet]
		UIKit.UITextField txtconfirmpass { get; set; }

		[Outlet]
		UIKit.UITextField txtnuevopass { get; set; }

		[Outlet]
		UIKit.UITextField txtpassactual { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtpassactual != null) {
				txtpassactual.Dispose ();
				txtpassactual = null;
			}

			if (txtnuevopass != null) {
				txtnuevopass.Dispose ();
				txtnuevopass = null;
			}

			if (txtconfirmpass != null) {
				txtconfirmpass.Dispose ();
				txtconfirmpass = null;
			}

			if (btnAceptar != null) {
				btnAceptar.Dispose ();
				btnAceptar = null;
			}
		}
	}
}
