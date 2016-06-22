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
	[Register ("solicitudMaquinaController")]
	partial class solicitudMaquinaController
	{
		[Outlet]
		UIKit.UIButton btnAgregar { get; set; }

		[Outlet]
		UIKit.UIButton btnAreaObra { get; set; }

		[Outlet]
		UIKit.UIButton btnequiposolicitado { get; set; }

		[Outlet]
		UIKit.UIButton btnMarca { get; set; }

		[Outlet]
		UIKit.UIButton btnModelo { get; set; }

		[Outlet]
		UIKit.UIButton btnResponsable { get; set; }

		[Outlet]
		UIKit.UIButton btnSolicitar { get; set; }

		[Outlet]
		UIKit.UIView contentViewSolicitudMaquina { get; set; }

		[Outlet]
		UIKit.UITableView lstRequerimientos { get; set; }

		[Outlet]
		UIKit.UIScrollView scrViewSolicitudMaquina { get; set; }

		[Outlet]
		UIKit.UITextField txtAreaObra { get; set; }

		[Outlet]
		UIKit.UITextField txtCantidad { get; set; }

		[Outlet]
		UIKit.UITextField txtEquiposolicitado { get; set; }

		[Outlet]
		UIKit.UITextField txtmarca { get; set; }

		[Outlet]
		UIKit.UITextField txtModelo { get; set; }

		[Outlet]
		UIKit.UITextField txtRequeridaPara { get; set; }

		[Outlet]
		UIKit.UITextField txtRequerimiento { get; set; }

		[Outlet]
		UIKit.UITextField txtResponsable { get; set; }

		[Outlet]
		UIKit.UITextField txtSolicitud { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (btnSolicitar != null) {
				btnSolicitar.Dispose ();
				btnSolicitar = null;
			}

			if (lstRequerimientos != null) {
				lstRequerimientos.Dispose ();
				lstRequerimientos = null;
			}

			if (btnAgregar != null) {
				btnAgregar.Dispose ();
				btnAgregar = null;
			}

			if (txtmarca != null) {
				txtmarca.Dispose ();
				txtmarca = null;
			}

			if (btnMarca != null) {
				btnMarca.Dispose ();
				btnMarca = null;
			}

			if (txtModelo != null) {
				txtModelo.Dispose ();
				txtModelo = null;
			}

			if (btnModelo != null) {
				btnModelo.Dispose ();
				btnModelo = null;
			}

			if (txtEquiposolicitado != null) {
				txtEquiposolicitado.Dispose ();
				txtEquiposolicitado = null;
			}

			if (btnequiposolicitado != null) {
				btnequiposolicitado.Dispose ();
				btnequiposolicitado = null;
			}

			if (txtCantidad != null) {
				txtCantidad.Dispose ();
				txtCantidad = null;
			}

			if (txtResponsable != null) {
				txtResponsable.Dispose ();
				txtResponsable = null;
			}

			if (btnResponsable != null) {
				btnResponsable.Dispose ();
				btnResponsable = null;
			}

			if (txtAreaObra != null) {
				txtAreaObra.Dispose ();
				txtAreaObra = null;
			}

			if (btnAreaObra != null) {
				btnAreaObra.Dispose ();
				btnAreaObra = null;
			}

			if (txtRequeridaPara != null) {
				txtRequeridaPara.Dispose ();
				txtRequeridaPara = null;
			}

			if (txtSolicitud != null) {
				txtSolicitud.Dispose ();
				txtSolicitud = null;
			}

			if (txtRequerimiento != null) {
				txtRequerimiento.Dispose ();
				txtRequerimiento = null;
			}

			if (contentViewSolicitudMaquina != null) {
				contentViewSolicitudMaquina.Dispose ();
				contentViewSolicitudMaquina = null;
			}

			if (scrViewSolicitudMaquina != null) {
				scrViewSolicitudMaquina.Dispose ();
				scrViewSolicitudMaquina = null;
			}
		}
	}
}
