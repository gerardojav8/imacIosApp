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
using System.IO;
using Foundation;
using System.Drawing;


namespace icom
{
	public partial class FichaMaquinaController : UIViewController
	{
		public UIViewController viewmaq { get; set; }
		public String strNoserie { get; set; }
		private int idequipoaux = -1;
		public List<clsEstadosFisicosMaquinas> lstefact = new List<clsEstadosFisicosMaquinas>();
		public List<clsFiltrosMaquinas> lstfmact = new List<clsFiltrosMaquinas>();

		LoadingOverlay loadPop;
		HttpClient client;
		List<clsCmbObras> lstobras;

		UIActionSheet actShObras;

		private int idobra = -1;
		private String strimagenbase64 = "";

		public FichaMaquinaController() : base("FichaMaquinaController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			if (UIScreen.MainScreen.Bounds.Width == 414)
			{
				scrViewFichaMaquina.ContentSize = new CoreGraphics.CGSize(375, 1883);
			}
			else {
				scrViewFichaMaquina.ContentSize = new CoreGraphics.CGSize(316, 1883);
			}

			lstobras = new List<clsCmbObras>();

			/*var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Obteniendo datos de Maquina espera por favor...");
			View.Add(loadPop);

			Boolean respobras = await getObras();
			if (!respobras) { 

				this.NavigationController.PopToViewController(viewmaq, true);
			}

			inicializaCombos();

			clsFichaMaquina objfichamaquina = await getFichaMaquina();

			if (objfichamaquina == null)
			{
				this.NavigationController.PopToViewController(viewmaq, true);
			}

			if (objfichamaquina.imagen != "") {
				byte[] imageBytes = Convert.FromBase64String(objfichamaquina.imagen);
				NSData imageData = NSData.FromArray(imageBytes);
				btnImgMaq.SetImage(UIImage.LoadFromData(imageData), UIControlState.Normal);
			}


			txtNoEconomico.Text = objfichamaquina.noeco;
			txtDescripcion.Text = objfichamaquina.descripcion;
			txtMarcaMaq.Text = objfichamaquina.marca;
			txtModeloMaq.Text = objfichamaquina.modelo;
			txtSerieMaq.Text = objfichamaquina.serie;

			if (objfichamaquina.equipoaux == 1)
			{
				segEquipoAux.SelectedSegment = 0;
				idequipoaux = Int32.Parse(objfichamaquina.idequipoaux);
				txtMarca.Text = objfichamaquina.marcaaux;
				txtModelo.Text = objfichamaquina.modeloaux;
				txtSerie.Text = objfichamaquina.serieaux;
			}
			else {
				segEquipoAux.SelectedSegment = 1;
				txtMarca.Enabled = false;
				txtModelo.Enabled = false;
				txtSerie.Enabled = false;
				txtMarca.Text = "";
				txtModelo.Text = "";
				txtSerie.Text = "";
			}

			lstefact = objfichamaquina.estadosfisicos;
			lstfmact = objfichamaquina.filtros;

			foreach (clsFiltrosMaquinas fm in lstfmact) {
				switch (fm.idfiltro) {
					case 1: txtFiltroMotor.Text = fm.medicion; break;
					case 2: txtFiltroHid.Text = fm.medicion; break;
					case 3: txtFiltroComb.Text = fm.medicion; break;
					case 4: txtFiltroTrans.Text = fm.medicion; break;
					case 5: txtFiltroAgua.Text = fm.medicion; break;
					case 6: txtFiltroOtro.Text = fm.medicion; break;
					default:  break;
				}
			}

			foreach (clsEstadosFisicosMaquinas ef in lstefact)
			{
				switch (ef.idcomponente)
				{
					case 1:
						if (ef.calificacion == "") {
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnEFMotor.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnEFMotor.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else { 
							btnEFMotor.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						 break;
					case 2: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnEFTransmision.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnEFTransmision.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnEFTransmision.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						break;
						
					case 3:  break;
						
					case 4: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnEFEqHid.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnEFEqHid.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnEFEqHid.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						 break;
					case 5: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnEqelec.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnEqelec.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnEqelec.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						 break;
					case 6: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnMandosfin.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnMandosfin.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnMandosfin.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						 break;
					case 7: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnLlantas.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnLlantas.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnLlantas.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						 break;
					case 8: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnmangueras.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnmangueras.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnmangueras.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						 break;
					case 9: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnDireccion.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnDireccion.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnDireccion.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						 break;
					case 10: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnEquipodesgaste.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnEquipodesgaste.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnEquipodesgaste.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						 break;
					case 11: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnPintura.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnPintura.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnPintura.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						break;
					case 12: 
						if (ef.calificacion == "")
						{
							break;
						}
						if (Int32.Parse(ef.calificacion) <= 4)
						{
							btnTapiceria.BackgroundColor = UIColor.FromRGB(157, 28, 22);
						}
						else if (Int32.Parse(ef.calificacion) >= 5 && Int32.Parse(ef.calificacion) <= 7)
						{
							btnTapiceria.BackgroundColor = UIColor.FromRGB(161, 162, 19);
						}
						else {
							btnTapiceria.BackgroundColor = UIColor.FromRGB(59, 100, 52);
						}
						 break;
					default: break;
				}
			}

			txtLocalizacionAct.Text = objfichamaquina.obraactual;
			idobra = objfichamaquina.idobraactual;



			loadPop.Hide();
			*/
			


			btnImgMaq.Layer.BorderColor = UIColor.Black.CGColor;
			btnImgMaq.Layer.BorderWidth = (nfloat)2.0;

			btnImgMaq.TouchUpInside += delegate {


				String pathimagen = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "imagenficha.jpg");
				if (File.Exists(pathimagen))
				{
					File.Delete(pathimagen);
				}

				Camera.TakePicture(this, (obj) =>
				{
					var photo = obj.ValueForKey(new NSString("UIImagePickerControllerOriginalImage")) as UIImage;
					
					NSData imgData = photo.AsJPEG();
					NSError err = null;
					if (imgData.Save(pathimagen, false, out err))
					{
						Console.WriteLine("saved as " + pathimagen);
						btnImgMaq.SetImage(UIImage.FromFile(pathimagen), UIControlState.Normal);

						/*UIImage img = UIImage.FromFile(pathimagen);
						Byte[] imageBytes;
						using (NSData imagenData = img.AsJPEG())
						{
							imageBytes = new Byte[imagenData.Length];
							System.Runtime.InteropServices.Marshal.Copy(imagenData.Bytes, imageBytes, 0, Convert.ToInt32(imagenData.Length));
						}

						strimagenbase64 = Convert.ToBase64String(imageBytes);*/
						strimagenbase64 = funciones.getBase64Image(pathimagen);


					}
					else {
						Console.WriteLine("NOT saved as " + pathimagen + " because" + err.LocalizedDescription);
						funciones.MessageBox("Error", "NOT saved as " + pathimagen + " because" + err.LocalizedDescription);
					}
				});


			};



			btnEFMotor.TouchUpInside += delegate {
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Motor";
				viewef.viewft = this;
				viewef.idcomponente = 1;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 1:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnEFTransmision.TouchUpInside += delegate {
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Transmision";
				viewef.viewft = this;
				viewef.idcomponente = 2;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 2:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnEFEqHid.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Euipo Hidraulico";
				viewef.viewft = this;
				viewef.idcomponente = 4;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 4:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnEqelec.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Euipo Electrico";
				viewef.viewft = this;
				viewef.idcomponente = 5;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 5:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnMandosfin.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Mandos Finales";
				viewef.viewft = this;
				viewef.idcomponente = 6;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 6:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnLlantas.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Llantas";
				viewef.viewft = this;
				viewef.idcomponente = 7;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 7:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnmangueras.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Mangueras y Conexiones";
				viewef.viewft = this;
				viewef.idcomponente = 8;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 8:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnDireccion.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Direccion";
				viewef.viewft = this;
				viewef.idcomponente = 9;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 9:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnEquipodesgaste.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Equipo de Desgaste";
				viewef.viewft = this;
				viewef.idcomponente = 10;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 10:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnPintura.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Pintura";
				viewef.viewft = this;
				viewef.idcomponente = 11;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 11:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnTapiceria.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Tapiceria";
				viewef.viewft = this;
				viewef.idcomponente = 12;
				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";

				foreach (clsEstadosFisicosMaquinas ef in lstefact)
				{
					switch (ef.idcomponente)
					{
						case 12:
							viewef.idcomponente = ef.idcomponente;
							viewef.strmarca = ef.marca;
							viewef.strtipo = ef.tipo;
							viewef.strcapacidad = ef.capacidad;
							viewef.strcalificacion = ef.calificacion;
							viewef.strcomentario = ef.comentario;
							break;
					}
				}

				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			otros.TouchUpInside += delegate
			{
				EstadoFisicoController viewef = new EstadoFisicoController();
				viewef.titulo = "Otros";
				viewef.viewft = this;

				viewef.strmarca = "";
				viewef.strtipo = "";
				viewef.strcapacidad = "";
				viewef.strcalificacion = "";
				viewef.strcomentario = "";


				this.NavigationController.PushViewController(viewef, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.FlipFromRight, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			segEquipoAux.ValueChanged += delegate {
				if (segEquipoAux.SelectedSegment == 0)
				{
					txtMarca.Enabled = true;
					txtModelo.Enabled = true;
					txtSerie.Enabled = true;
				}
				else { 
					txtMarca.Enabled = false;
					txtMarca.Text = "";

					txtModelo.Enabled = false;
					txtModelo.Text = "";

					txtSerie.Enabled = false;
					txtSerie.Text = "";

				}
			};

			btnGuardar.TouchUpInside += guardarFicha;

			bajatecladoinputs();

		}

		private void bajatecladoinputs()
		{
			UIToolbar toolbar;
			UIBarButtonItem doneButton;


			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtFiltroMotor.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			this.txtFiltroMotor.InputAccessoryView = toolbar;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtFiltroHid.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			this.txtFiltroHid.InputAccessoryView = toolbar;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtFiltroComb.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			this.txtFiltroComb.InputAccessoryView = toolbar;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtFiltroTrans.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			this.txtFiltroTrans.InputAccessoryView = toolbar;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtFiltroAgua.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			this.txtFiltroAgua.InputAccessoryView = toolbar;

			toolbar = new UIToolbar(new RectangleF(0.0f, 0.0f, (float)this.View.Frame.Size.Width, 44.0f));
			toolbar.Layer.BackgroundColor = UIColor.Blue.CGColor;
			doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { txtFiltroOtro.EndEditing(true); });
			toolbar.Items = new UIBarButtonItem[] { new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace), doneButton };
			this.txtFiltroOtro.InputAccessoryView = toolbar;


			txtDescripcion.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtMarca.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtModelo.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };
			txtSerie.ShouldReturn += (txtUsuario) => { ((UITextField)txtUsuario).ResignFirstResponder(); return true; };


		}

		async void guardarFicha(object sender, EventArgs e)
		{
			if (segEquipoAux.SelectedSegment == 0)
			{
				if (txtMarca.Text == "")
				{
					funciones.MessageBox("Error", "Debe de ingresar una marca para el equipo auxiliar");
					return;
				}

				if (txtModelo.Text == "")
				{
					funciones.MessageBox("Error", "Debe de ingresar un modelo para el equipo auxiliar");
					return;
				}

				if (txtSerie.Text == "")
				{
					funciones.MessageBox("Error", "Debe de ingresar la serie para el equipo auxiliar");
					return;
				}
			}


			if (txtFiltroMotor.Text == "")
			{
				funciones.MessageBox("Error", "Debe de ingresar la medicion para el filtro de motor");
				return;
			}


			Decimal fmotor;
			if (!Decimal.TryParse(txtFiltroMotor.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), out fmotor))
			{
				funciones.MessageBox("Error", "La cantidad en el filtro de motor debe de ser numerica");
				return;
			}

			if (txtFiltroHid.Text == "")
			{
				funciones.MessageBox("Error", "Debe de ingresar la medicion para el filtro hidraulico");
				return;
			}


			Decimal fhid;
			if (!Decimal.TryParse(txtFiltroHid.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), out fhid))
			{
				funciones.MessageBox("Error", "La cantidad en el filtro hidraulico debe de ser numerica");
				return;
			}

			if (txtFiltroComb.Text == "")
			{
				funciones.MessageBox("Error", "Debe de ingresar la medicion para el filtro de combustible");
				return;
			}

			Decimal fcom;
			if (!Decimal.TryParse(txtFiltroComb.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), out fcom))
			{
				funciones.MessageBox("Error", "La cantidad en el filtro de combustible debe de ser numerica");
				return;
			}

			if (txtFiltroTrans.Text == "")
			{
				funciones.MessageBox("Error", "Debe de ingresar la medicion para el filtro de transmision");
				return;
			}

			Decimal ftrans;
			if (!Decimal.TryParse(txtFiltroTrans.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), out ftrans))
			{
				funciones.MessageBox("Error", "La cantidad en el filtro de transmision debe de ser numerica");
				return;
			}

			if (txtFiltroAgua.Text == "")
			{
				funciones.MessageBox("Error", "Debe de ingresar la medicion para el filtro de Agua");
				return;
			}

			Decimal fagua;
			if (!Decimal.TryParse(txtFiltroAgua.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), out fagua))
			{
				funciones.MessageBox("Error", "La cantidad en el filtro de Agua debe de ser numerica");
				return;
			}

			if (txtFiltroOtro.Text == "")
			{
				funciones.MessageBox("Error", "Debe de ingresar la medicion para el filtro otro");
				return;
			}

			Decimal fotro;
			if (!Decimal.TryParse(txtFiltroOtro.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CreateSpecificCulture("en-US"), out fotro))
			{
				funciones.MessageBox("Error", "La cantidad en el filtro otro debe de ser numerica");
				return;
			}


			String respsave = await saveFichaMaq();

			if (respsave != "")
			{
				((MaquinasController)viewmaq).recargarListado();
				this.NavigationController.PopToViewController(viewmaq, true);
			}
		}

		public async Task<String> saveFichaMaq()
		{
			var bounds = UIScreen.MainScreen.Bounds;
			loadPop = new LoadingOverlay(bounds, "Guardando Ficha de Maquina...");
			View.Add(loadPop);

			client = new HttpClient();
			client.Timeout = new System.TimeSpan(0, 0, 0, 10, 0);

			string url = Consts.ulrserv + "maquinas/ModificaFichaMaquina";
			var uri = new Uri(string.Format(url));

			clsFichaMaquina objfm = new clsFichaMaquina();

			objfm.serie = strNoserie;
			objfm.idobraactual = idobra;
			objfm.imagen = strimagenbase64;


			if (segEquipoAux.SelectedSegment == 1)
			{
				objfm.equipoaux = 0;
			}
			else {
				objfm.equipoaux = 1;
				if (idequipoaux >= 0)
				{
					objfm.idequipoaux = idequipoaux.ToString();
				}
				else {
					objfm.idequipoaux = "";
				}
			}

			objfm.marcaaux = txtMarca.Text;
			objfm.modeloaux = txtModelo.Text;
			objfm.serieaux = txtSerie.Text;
			objfm.estadosfisicos = lstefact;



			for (int i = 0; i <= lstfmact.Count - 1; i++) {
				switch (lstfmact.ElementAt(i).idfiltro) {
					case 1: lstfmact.ElementAt(i).medicion = txtFiltroMotor.Text; break;
					case 2: lstfmact.ElementAt(i).medicion = txtFiltroHid.Text; break;
					case 3: lstfmact.ElementAt(i).medicion = txtFiltroComb.Text; break;
					case 4: lstfmact.ElementAt(i).medicion = txtFiltroTrans.Text; break;
					case 5: lstfmact.ElementAt(i).medicion = txtFiltroAgua.Text; break;
					case 6: lstfmact.ElementAt(i).medicion = txtFiltroOtro.Text; break;
				}
			}


			objfm.filtros = lstfmact;

			var json = JsonConvert.SerializeObject(objfm);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Consts.token);

			HttpResponseMessage response = null;

			try
			{
				response = await client.PostAsync(uri, content);

			}
			catch (Exception e)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI " + e.HResult);
				return "";
			}

			if (response == null)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI");
				return "";
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			var jsonresponse = JObject.Parse(responseString);

			var jtokenerror = jsonresponse["error_description"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return "";
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return "";
			}

			jtokenerror = jsonresponse["Message"];


			if (jtokenerror != null)
			{
				if (jtokenerror.ToString().Contains("Error"))
				{
					loadPop.Hide();
					string error = jtokenerror.ToString();
					funciones.MessageBox("Error", error);
					return "";
				}
			}

			funciones.MessageBox("Aviso", "Se ha guardado la ficha tecnica");
			return "Exito";

		}



		public async Task<clsFichaMaquina> getFichaMaquina()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "maquinas/getFichaMaquina";
			var uri = new Uri(string.Format(url));

			Dictionary<string, string> obj = new Dictionary<string, string>();
			obj.Add("noserie", strNoserie);
			var json = JsonConvert.SerializeObject(obj);

			var content = new StringContent(json, Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Consts.token);

			HttpResponseMessage response = null;

			try
			{
				response = await client.PostAsync(uri, content);
			}
			catch (Exception e)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI " + e.HResult);
				return null;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			var jsonresponse = JObject.Parse(responseString);

			var jtokenerror = jsonresponse["error_description"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return null;
			}

			jtokenerror = jsonresponse["error"];


			if (jtokenerror != null)
			{
				loadPop.Hide();
				string error = jtokenerror.ToString();
				funciones.MessageBox("Error", error);
				return null;
			}

			clsFichaMaquina objfichamaquina = new clsFichaMaquina();

			objfichamaquina.noeco = jsonresponse["noeco"].ToString();
			objfichamaquina.descripcion = jsonresponse["descripcion"].ToString();
			objfichamaquina.marca = jsonresponse["marca"].ToString();
			objfichamaquina.modelo = jsonresponse["modelo"].ToString();
			objfichamaquina.serie = jsonresponse["serie"].ToString();
			objfichamaquina.idobraactual = Int32.Parse(jsonresponse["idobraactual"].ToString());
			objfichamaquina.obraactual = jsonresponse["obraactual"].ToString();
			objfichamaquina.imagen = jsonresponse["imagen"].ToString();
			objfichamaquina.idequipoaux = jsonresponse["idequipoaux"].ToString();
			objfichamaquina.equipoaux = Int32.Parse(jsonresponse["equipoaux"].ToString());
			objfichamaquina.marcaaux = jsonresponse["marcaaux"].ToString();
			objfichamaquina.modeloaux = jsonresponse["modelaux"].ToString();
			objfichamaquina.serieaux = jsonresponse["serieaux"].ToString();


			JArray jarrEstadosFisicos = JArray.Parse(jsonresponse["estadosfisicos"].ToString());
			List<clsEstadosFisicosMaquinas> lstef = new List<clsEstadosFisicosMaquinas>();
			foreach (var jef in jarrEstadosFisicos) {
				clsEstadosFisicosMaquinas objef = getEstadosFisicosdeJson(jef);
				lstef.Add(objef);
			}

			JArray jarrFiltros = JArray.Parse(jsonresponse["filtros"].ToString());
			List<clsFiltrosMaquinas> lstfm = new List<clsFiltrosMaquinas>();
			foreach (var jfm in jarrFiltros) {
				clsFiltrosMaquinas objfm = getFiltrosdeJson(jfm);
				lstfm.Add(objfm);
			}

			objfichamaquina.estadosfisicos = lstef;
			objfichamaquina.filtros = lstfm;


			return objfichamaquina;
		}

		public clsFiltrosMaquinas getFiltrosdeJson(Object varjson) {
			clsFiltrosMaquinas objfm = new clsFiltrosMaquinas();
			JObject json = (JObject)varjson;

			objfm.nombre = json["nombre"].ToString();
			objfm.idfiltro = Int32.Parse(json["idfiltro"].ToString());
			objfm.medicion = json["medicion"].ToString();
			objfm.comentario = json["comentario"].ToString();

			return objfm;
		}

		public clsEstadosFisicosMaquinas getEstadosFisicosdeJson(Object varjson) { 
			clsEstadosFisicosMaquinas objef = new clsEstadosFisicosMaquinas();
			JObject json = (JObject)varjson;

			objef.nombre = json["nombre"].ToString();
			objef.idcomponente = Int32.Parse(json["idcomponente"].ToString());
			objef.marca = json["marca"].ToString();
			objef.tipo = json["tipo"].ToString();
			objef.capacidad = json["capacidad"].ToString();
			objef.calificacion = json["calificacion"].ToString().Replace(",", ".");
			objef.comentario = json["comentario"].ToString();

			return objef;
		}

		public async Task<Boolean> getObras()
		{


			client = new HttpClient();
			string url = Consts.ulrserv + "obras/getObras";
			var uri = new Uri(string.Format(url));

			var content = new StringContent("", Encoding.UTF8, "application/json");
			client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Consts.token);

			HttpResponseMessage response = null;

			try
			{
				response = await client.PostAsync(uri, content);
			}
			catch (Exception e)
			{
				loadPop.Hide();
				funciones.MessageBox("Error", "No se ha podido hacer conexion con el servicio, verfiquelo con su administrador TI " + e.HResult);
				return false;
			}

			string responseString = string.Empty;
			responseString = await response.Content.ReadAsStringAsync();
			JArray jrarray;


			try
			{
				var jsonresponse = JArray.Parse(responseString);
				jrarray = jsonresponse;
			}
			catch (Exception e)
			{
				loadPop.Hide();
				var jsonresponse = JObject.Parse(responseString);

				string mensaje = "error al traer las obras del servidor: " + e.HResult;

				var jtokenerror = jsonresponse["error"];
				if (jtokenerror != null)
				{
					mensaje = jtokenerror.ToString();
				}

				funciones.MessageBox("Error", mensaje);
				return false;
			}

			foreach (var ob in jrarray)
			{
				clsCmbObras objobras = getObrasdeJson(ob);
				lstobras.Add(objobras);
			}

			return true;
		}

		public clsCmbObras getObrasdeJson(Object varjson)
		{
			clsCmbObras objobra = new clsCmbObras();
			JObject json = (JObject)varjson;

			objobra.idobra = Int32.Parse(json["idobra"].ToString());
			objobra.nombre = json["nombre"].ToString();

			return objobra;
		}

		public void inicializaCombos()
		{

			//--------Combo Reporto---------------------
			actShObras = new UIActionSheet("Seleccionar");
			foreach (clsCmbObras ob in lstobras)
			{
				String nombre = ob.nombre;
				actShObras.Add(nombre);
			}
			actShObras.Add("Cancelar");

			actShObras.Style = UIActionSheetStyle.BlackTranslucent;
			actShObras.CancelButtonIndex = lstobras.Count;

			btnLocalizacionAct.TouchUpInside += delegate
			{
				actShObras.ShowInView(this.contentFichaMaquina);
			};

			actShObras.Clicked += delegate (object sender, UIButtonEventArgs e)
			{
				if (e.ButtonIndex != lstobras.Count)
				{
					clsCmbObras ob = lstobras.ElementAt((int)e.ButtonIndex);
					txtLocalizacionAct.Text = ob.nombre;
					idobra = ob.idobra;
				}
				else {
					txtLocalizacionAct.Text = "";
					idobra = -1;
				}
			};
		}

	}
}


