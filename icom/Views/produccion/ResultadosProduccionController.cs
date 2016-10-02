using System;

using UIKit;
using System.Collections.Generic;
using Foundation;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using icom.globales;
using System.Text;
using Newtonsoft.Json;
using System.Json;
using System.Linq;
using CoreGraphics;

namespace icom
{
	public partial class ResultadosProduccionController : UIViewController
	{
		private List<clsProduccion> lstProd;
		public string folio { get; set; }
		public string material { get; set; }
		public string cantidad { get; set; }
		public string unidad { get; set; }
		public string cliente { get; set; }
		public string fechaini { get; set; }
		public string fechafin { get; set; }
		public ResultadosProduccionController() : base("ResultadosProduccionController", null)
		{
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			lstProd = new List<clsProduccion>();


			tblProduccion.Source = new FuenteTablaProduccion(this, lstProd);

			clsProduccion obj = new clsProduccion
			{
				folio = "3165465",
				material = "Arena",
				cantidad = "24.89",
				unidad = "M3",
				cliente = "Gerardo Javier Gamez Vazquez",
				fecha = "2016-01-01"
			};

			clsProduccion obj1 = new clsProduccion
			{
				folio = "3165465",
				material = "Arena",
				cantidad = "875424.89",
				unidad = "M3",
				cliente = "Gerardo Javier Gamez Vazquez",
				fecha = "2016-01-01"
			};

			clsProduccion obj2 = new clsProduccion
			{
				folio = "3165465",
				material = "Arena",
				cantidad = "724.89",
				unidad = "M3",
				cliente = "Gerardo Javier Gamez Vazquez",
				fecha = "2016-01-01"
			};

			lstProd.Add(obj);
			lstProd.Add(obj1);
			lstProd.Add(obj2);
		}

	}
}

