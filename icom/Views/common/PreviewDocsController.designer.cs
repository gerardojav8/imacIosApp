// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace icom
{
    [Register ("PreviewDocsController")]
    partial class PreviewDocsController
    {
        [Outlet]
        UIKit.UILabel txttitulo { get; set; }


        [Outlet]
        UIKit.UIWebView webViewDocs { get; set; }

        void ReleaseDesignerOutlets ()
        {
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