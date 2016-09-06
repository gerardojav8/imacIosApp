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
    [Register ("AgendaController")]
    partial class AgendaController
    {
        [Outlet]
        UIKit.UIButton btnNuevoEvento { get; set; }


        [Outlet]
        UIKit.UITableView lstAgenda { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (btnNuevoEvento != null) {
                btnNuevoEvento.Dispose ();
                btnNuevoEvento = null;
            }

            if (lstAgenda != null) {
                lstAgenda.Dispose ();
                lstAgenda = null;
            }
        }
    }
}