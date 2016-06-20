using System;

using UIKit;

namespace icom
{
	public partial class ReporteOperador : UIViewController
	{
		UIActionSheet actShReporto;
		String[] arrReporto;
		UIActionSheet actShTipoFalla;
		String[] arrTipoFalla;
		UIActionSheet actShAtiende;
		String[] arrAtiende;

		public String strNoeconomico{ get; set;}

		public String strNoSerie{ get; set; }

		public String strModelo { get; set; }

		public ReporteOperador () : base ("ReporteOperador", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			ScrView.ContentSize = new CoreGraphics.CGSize(350, 1000);
			txtDescripcion.Layer.BorderColor = UIColor.Black.CGColor;
			txtDescripcion.Layer.BorderWidth = (nfloat) 2.0;
			txtDescripcion.Text = "";

			inicializadatos();

			inicializaCombos ();



			btnGuardar.TouchUpInside += delegate {
				MessageBox("Guardar", "El registro ha sido guardado con exito!!");
			};


		}

		public void inicializadatos()
		{
			txtequipo.Text = strNoeconomico;
			txtmodelo.Text = strModelo;
			txtnoserie.Text = strNoSerie;


		}

		public void inicializaCombos(){
			
			arrReporto = new string[] {
				"Reporto 1",
				"Reporto 2",
				"Reporto 3",
				"Cancelar"
			};

			actShReporto = new UIActionSheet ("Seleccionar");
			for (int i =0 ; i < arrReporto.Length; i++) {
				actShReporto.Add (arrReporto[i]);
			}

			actShReporto.Style = UIActionSheetStyle.BlackTranslucent;
			actShReporto.CancelButtonIndex = 4;

			btnReporto.TouchUpInside += delegate {
				actShReporto.ShowInView (this.ContentView);
			};

			actShReporto.Clicked += delegate(object sender, UIButtonEventArgs e) {
				if(e.ButtonIndex != 4)
					txtreporto.Text = arrReporto[e.ButtonIndex];
			};

			arrTipoFalla = new string[] {
				"Falla 1",
				"Falla 2",
				"Falla 3",
				"Cancelar"
			};

			actShTipoFalla = new UIActionSheet ("Seleccionar");
			for (int i =0 ; i < arrTipoFalla.Length; i++) {
				actShTipoFalla.Add (arrTipoFalla[i]);
			}

			actShTipoFalla.Style = UIActionSheetStyle.BlackTranslucent;
			actShTipoFalla.CancelButtonIndex = 4;

			btnTipoFalla.TouchUpInside += delegate {
				actShTipoFalla.ShowInView (this.ContentView);
			};

			actShTipoFalla.Clicked += delegate(object sender, UIButtonEventArgs e) {
				if(e.ButtonIndex != 4)
					txtfipofalla.Text = arrTipoFalla[e.ButtonIndex];
			};
			arrAtiende = new string[] {
				"Atiende 1",
				"Atiende 2",
				"Atiende 3",
				"Cancelar"
			};


			actShAtiende = new UIActionSheet ("Seleccionar");
			for (int i =0 ; i < arrAtiende.Length; i++) {
				actShAtiende.Add (arrAtiende[i]);
			}

			actShAtiende.Style = UIActionSheetStyle.BlackTranslucent;
			actShAtiende.CancelButtonIndex = 4;



			btnAtiende.TouchUpInside += delegate {
				actShAtiende.ShowInView (this.ContentView);
			};

			actShAtiende.Clicked += delegate(object sender, UIButtonEventArgs e) {
				if(e.ButtonIndex != 4)
					txtatiende.Text = arrAtiende[e.ButtonIndex];
			};
		}


		private void MessageBox(string titulo, string mensaje){
			using(UIAlertView Alerta = new UIAlertView()){
				Alerta.Title = titulo;
				Alerta.Message = mensaje;
				Alerta.AddButton ("Enterado");
				Alerta.Show ();
			};
		}
	}
}


