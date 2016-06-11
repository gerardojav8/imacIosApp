// WARNING
//
// This file has been generated automatically by Xamarin Studio Business to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace icom
{
	[Register ("Login")]
	partial class Login
	{
		[Outlet]
		UIKit.UIButton btnLogin { get; set; }

		[Outlet]
		UIKit.UITextField txtPass { get; set; }

		[Outlet]
		UIKit.UITextField txtUsuario { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (txtUsuario != null) {
				txtUsuario.Dispose ();
				txtUsuario = null;
			}

			if (txtPass != null) {
				txtPass.Dispose ();
				txtPass = null;
			}

			if (btnLogin != null) {
				btnLogin.Dispose ();
				btnLogin = null;
			}
		}
	}
}
