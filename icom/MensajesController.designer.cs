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
	[Register ("MensajesController")]
	partial class MensajesController
	{
		[Outlet]
		UIKit.UIButton btnArchivo { get; set; }

		[Outlet]
		UIKit.UIButton btnenviar { get; set; }

		[Outlet]
		UIKit.UITableView tblChat { get; set; }

		[Outlet]
		UIKit.UITextField txtmensaje { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnArchivo != null) {
				btnArchivo.Dispose ();
				btnArchivo = null;
			}

			if (btnenviar != null) {
				btnenviar.Dispose ();
				btnenviar = null;
			}

			if (tblChat != null) {
				tblChat.Dispose ();
				tblChat = null;
			}

			if (txtmensaje != null) {
				txtmensaje.Dispose ();
				txtmensaje = null;
			}
		}
	}
}
