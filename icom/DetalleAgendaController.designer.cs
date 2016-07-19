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
	[Register ("DetalleAgendaController")]
	partial class DetalleAgendaController
	{
		[Outlet]
		UIKit.UIButton btnAgregarCalendario { get; set; }

		[Outlet]
		UIKit.UIButton btnEnviar { get; set; }

		[Outlet]
		UIKit.UIView ContentDetalleAgencia { get; set; }

		[Outlet]
		UIKit.UILabel lblComentario { get; set; }

		[Outlet]
		UIKit.UILabel lblLapso { get; set; }

		[Outlet]
		UIKit.UIScrollView scrDetalleAgencia { get; set; }

		[Outlet]
		UIKit.UITextField txtChatDetalleAgencia { get; set; }

		[Outlet]
		UIKit.UITextView txtchatgeneralDetalleAgencia { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (scrDetalleAgencia != null) {
				scrDetalleAgencia.Dispose ();
				scrDetalleAgencia = null;
			}

			if (ContentDetalleAgencia != null) {
				ContentDetalleAgencia.Dispose ();
				ContentDetalleAgencia = null;
			}

			if (lblComentario != null) {
				lblComentario.Dispose ();
				lblComentario = null;
			}

			if (lblLapso != null) {
				lblLapso.Dispose ();
				lblLapso = null;
			}

			if (btnAgregarCalendario != null) {
				btnAgregarCalendario.Dispose ();
				btnAgregarCalendario = null;
			}

			if (btnEnviar != null) {
				btnEnviar.Dispose ();
				btnEnviar = null;
			}

			if (txtChatDetalleAgencia != null) {
				txtChatDetalleAgencia.Dispose ();
				txtChatDetalleAgencia = null;
			}

			if (txtchatgeneralDetalleAgencia != null) {
				txtchatgeneralDetalleAgencia.Dispose ();
				txtchatgeneralDetalleAgencia = null;
			}
		}
	}
}
