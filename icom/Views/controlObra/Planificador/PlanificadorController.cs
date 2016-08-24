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
			tblEventos.Source = new FuenteTablaEventos(this, lstEventos);

		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}


