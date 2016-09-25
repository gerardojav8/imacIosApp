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
	[Register ("PreviewDocsController")]
	partial class PreviewDocsController
	{
		[Outlet]
		UIKit.UIButton btnMail { get; set; }

		[Outlet]
		UIKit.UILabel txttitulo { get; set; }

		[Outlet]
		UIKit.UIWebView webViewDocs { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnMail != null) {
				btnMail.Dispose ();
				btnMail = null;
			}

			if (txttitulo != null) {
				txttitulo.Dispose ();
				txttitulo = null;
			}

			if (webViewDocs != null) {
				webViewDocs.Dispose ();
				webViewDocs = null;
			}
		}
	}
}
