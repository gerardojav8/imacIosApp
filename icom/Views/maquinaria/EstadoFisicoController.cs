using System;

using UIKit;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using icom.globales;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace icom
{
	public partial class EstadoFisicoController : UIViewController
	{
		public EstadoFisicoController() : base("EstadoFisicoController", null)
		{
		}
		public UIViewController viewft
		{
			get;
			set;
		}
		public String titulo
		{
			get;
			set;
		}

		public int idcomponente { get; set; }
		public string strmarca { get; set; }
		public string strtipo { get; set; }
		public string strcapacidad { get; set; }
		public string strcalificacion { get; set; }
		public string strcomentario { get; set; }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			txtComentarios.Layer.BorderColor = UIColor.Black.CGColor;
			txtComentarios.Layer.BorderWidth = (nfloat)2.0;
			txtComentarios.Text = "";

			lbltitulo.Text = titulo;

			txtmarca.Text = strmarca;
			txttipo.Text = strtipo;
			txtcapacidad.Text = strcapacidad;
			txtcalificacion.Text = strcalificacion;
			txtComentarios.Text = strcomentario;

			btnGuardarEF.TouchUpInside += delegate {

				Boolean blnenc = false;
				for (int i = 0; i <= ((FichaMaquinaController)viewft).lstefact.Count - 1; i++)
				{
					if(((FichaMaquinaController)viewft).lstefact.ElementAt(i).idcomponente == idcomponente){
						((FichaMaquinaController)viewft).lstefact.ElementAt(i).marca = txtmarca.Text;
						((FichaMaquinaController)viewft).lstefact.ElementAt(i).tipo = txttipo.Text;
						((FichaMaquinaController)viewft).lstefact.ElementAt(i).capacidad = txtcapacidad.Text;
						((FichaMaquinaController)viewft).lstefact.ElementAt(i).calificacion = txtcalificacion.Text;
						((FichaMaquinaController)viewft).lstefact.ElementAt(i).comentario = txtComentarios.Text;
						blnenc = true;
						break;
					}
				}

				if (!blnenc) {
					clsEstadosFisicosMaquinas objefm = new clsEstadosFisicosMaquinas();
					objefm.nombre = titulo;
					objefm.idcomponente = idcomponente;
					objefm.marca = txtmarca.Text;
					objefm.tipo = txttipo.Text;
					objefm.capacidad = txtcapacidad.Text;
					objefm.calificacion = txtcalificacion.Text;
					objefm.comentario = txtComentarios.Text;
					((FichaMaquinaController)viewft).lstefact.Add(objefm);
				}

				this.NavigationController.PopToViewController(viewft, true);
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


