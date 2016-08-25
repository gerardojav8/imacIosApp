using System;
using System.Collections.Generic;
using UIKit;

namespace icom
{
	public partial class PlanificadorController : UIViewController
	{
		public PlanificadorController() : base("PlanificadorController", null)
		{
		}
		private List<clsEvento> lstEventos; 
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			lstEventos = new List<clsEvento>();

			clsEvento objev1 = new clsEvento
			{
				titulo = "Evento 1",
				clasificacion = "Clasificacion 1",
				totalhoras = "25",
				horainicio = "12:00",
				horafinal = "12:00",
				porcentajeavance = 27
			};

			clsEvento objev2 = new clsEvento
			{
				titulo = "Evento 2",
				clasificacion = "Clasificacion 2",
				totalhoras = "25",
				horainicio = "12:00",
				horafinal = "12:00",
				porcentajeavance = 40
			};

			clsEvento objev3 = new clsEvento
			{
				titulo = "Evento 3",
				clasificacion = "Clasificacion 3",
				totalhoras = "25",
				horainicio = "12:00",
				horafinal = "12:00",
				porcentajeavance = 85
			};

			clsEvento objev4 = new clsEvento
			{
				titulo = "Evento 4",
				clasificacion = "Clasificacion 4",
				totalhoras = "25",
				horainicio = "12:00",
				horafinal = "12:00",
				porcentajeavance = 36
			};

			clsEvento objev5 = new clsEvento
			{
				titulo = "Evento 5",
				clasificacion = "Clasificacion 5",
				totalhoras = "25",
				horainicio = "12:00",
				horafinal = "12:00",
				porcentajeavance = 95
			};

			lstEventos.Add(objev1);
			lstEventos.Add(objev2);
			lstEventos.Add(objev3);
			lstEventos.Add(objev4);
			lstEventos.Add(objev5);

			tblEventos.Source = new FuenteTablaEventos(this, lstEventos);

			btnNuevoEvento.TouchUpInside += delegate {
				NuevaTareaController viewnt = new NuevaTareaController();


				viewnt.Title = "Nueva Tarea";
				this.NavigationController.PushViewController(viewnt, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnEditarEvento.TouchUpInside += delegate {
				ModifciarTareaController viewmt = new ModifciarTareaController();


				viewmt.Title = "Modificar Tarea";
				this.NavigationController.PushViewController(viewmt, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnGraficas.TouchUpInside += delegate {
				GraficasTareasController viewg = new GraficasTareasController();


				viewg.Title = "Graficas";
				this.NavigationController.PushViewController(viewg, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();	
			};

			btnExportaPDF.TouchUpInside += delegate
			{
				ExportarTareasController viewe = new ExportarTareasController();


				viewe.Title = "Exportacion PDF";
				this.NavigationController.PushViewController(viewe, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();
			};

			btnCategorias.TouchUpInside += delegate {
				CategoriasTareasController viewcat = new CategoriasTareasController();


				viewcat.Title = "Exportacion PDF";
				this.NavigationController.PushViewController(viewcat, false);
				UIView.BeginAnimations(null);
				UIView.SetAnimationDuration(0.7);
				UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, NavigationController.View, true);
				UIView.CommitAnimations();
			};


		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


