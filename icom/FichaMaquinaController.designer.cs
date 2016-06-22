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
	[Register ("FichaMaquinaController")]
	partial class FichaMaquinaController
	{
		[Outlet]
		UIKit.UIButton btnGuardar { get; set; }

		[Outlet]
		UIKit.UIButton btnLocalizacionAct { get; set; }

		[Outlet]
		UIKit.UIView contentFichaMaquina { get; set; }

		[Outlet]
		UIKit.UIImageView imgMaq { get; set; }

		[Outlet]
		UIKit.UIScrollView scrViewFichaMaquina { get; set; }

		[Outlet]
		UIKit.UISegmentedControl segEquipoAux { get; set; }

		[Outlet]
		UIKit.UITextField txtDescripcion { get; set; }

		[Outlet]
		UIKit.UITextView txtEstadoFisicoAct { get; set; }

		[Outlet]
		UIKit.UITextField txtFiltroAgua { get; set; }

		[Outlet]
		UIKit.UITextField txtFiltroComb { get; set; }

		[Outlet]
		UIKit.UITextField txtFiltroHid { get; set; }

		[Outlet]
		UIKit.UITextField txtFiltroMotor { get; set; }

		[Outlet]
		UIKit.UITextField txtFiltroOtro { get; set; }

		[Outlet]
		UIKit.UITextField txtFiltroTrans { get; set; }

		[Outlet]
		UIKit.UITextField txtLocalizacionAct { get; set; }

		[Outlet]
		UIKit.UITextField txtMarca { get; set; }

		[Outlet]
		UIKit.UITextField txtMarcaMaq { get; set; }

		[Outlet]
		UIKit.UITextField txtModelo { get; set; }

		[Outlet]
		UIKit.UITextField txtModeloMaq { get; set; }

		[Outlet]
		UIKit.UITextField txtNoEconomico { get; set; }

		[Outlet]
		UIKit.UITextField txtSerie { get; set; }

		[Outlet]
		UIKit.UITextField txtSerieMaq { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (scrViewFichaMaquina != null) {
				scrViewFichaMaquina.Dispose ();
				scrViewFichaMaquina = null;
			}

			if (contentFichaMaquina != null) {
				contentFichaMaquina.Dispose ();
				contentFichaMaquina = null;
			}

			if (btnGuardar != null) {
				btnGuardar.Dispose ();
				btnGuardar = null;
			}

			if (txtLocalizacionAct != null) {
				txtLocalizacionAct.Dispose ();
				txtLocalizacionAct = null;
			}

			if (btnLocalizacionAct != null) {
				btnLocalizacionAct.Dispose ();
				btnLocalizacionAct = null;
			}

			if (txtFiltroOtro != null) {
				txtFiltroOtro.Dispose ();
				txtFiltroOtro = null;
			}

			if (txtFiltroAgua != null) {
				txtFiltroAgua.Dispose ();
				txtFiltroAgua = null;
			}

			if (txtFiltroTrans != null) {
				txtFiltroTrans.Dispose ();
				txtFiltroTrans = null;
			}

			if (txtFiltroComb != null) {
				txtFiltroComb.Dispose ();
				txtFiltroComb = null;
			}

			if (txtFiltroHid != null) {
				txtFiltroHid.Dispose ();
				txtFiltroHid = null;
			}

			if (txtFiltroMotor != null) {
				txtFiltroMotor.Dispose ();
				txtFiltroMotor = null;
			}

			if (txtEstadoFisicoAct != null) {
				txtEstadoFisicoAct.Dispose ();
				txtEstadoFisicoAct = null;
			}

			if (txtSerie != null) {
				txtSerie.Dispose ();
				txtSerie = null;
			}

			if (txtMarca != null) {
				txtMarca.Dispose ();
				txtMarca = null;
			}

			if (txtModelo != null) {
				txtModelo.Dispose ();
				txtModelo = null;
			}

			if (segEquipoAux != null) {
				segEquipoAux.Dispose ();
				segEquipoAux = null;
			}

			if (txtSerieMaq != null) {
				txtSerieMaq.Dispose ();
				txtSerieMaq = null;
			}

			if (txtMarcaMaq != null) {
				txtMarcaMaq.Dispose ();
				txtMarcaMaq = null;
			}

			if (txtModeloMaq != null) {
				txtModeloMaq.Dispose ();
				txtModeloMaq = null;
			}

			if (txtDescripcion != null) {
				txtDescripcion.Dispose ();
				txtDescripcion = null;
			}

			if (txtNoEconomico != null) {
				txtNoEconomico.Dispose ();
				txtNoEconomico = null;
			}

			if (imgMaq != null) {
				imgMaq.Dispose ();
				imgMaq = null;
			}
		}
	}
}
