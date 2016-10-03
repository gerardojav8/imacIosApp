using System;
using UIKit;
namespace icom.globales
{
	public static class Consts
	{
		public static readonly string ipserv = "proyextra.hopto.org";
		//public static readonly string ipserv = "192.168.43.189";
		public static readonly string ipservchat = "proyextra.hopto.org";
		//public static readonly string ipservchat = "192.168.43.189";
		public static readonly string puertochat = ":8080";
		//public static readonly string puertochat = ":30000";

		public static readonly string urltoken = "http://" + ipserv + "/icomtoken/oauth2/token";
		public static readonly string ulrserv = "http://" + ipserv + "/icomApi/";
		public static readonly string urlserverchat = "http://" + ipservchat + puertochat;

		public static readonly int Trascabo = 1;
		public static readonly int Revolvedora = 2;
		public static readonly int Excabadora = 3;

		public static string token = "";
		public static string idusuarioapp = "";
		public static string nombreusuarioapp = "";
		public static string inicialesusuarioapp = "";
		public static UIViewController logincontroller;

		public static UIColor[] colores = {
			UIColor.FromRGB(48,88,147),//blue
			UIColor.FromRGB(145,147,63),//Yellow,
			UIColor.FromRGB(147,30,24),//Red,
			UIColor.FromRGB(62,53,147),//Purple,
			UIColor.FromRGB(147,123,117),//Brown,
			UIColor.FromRGB(147,88,17),//Orange,
			UIColor.FromRGB(57,147,104),//Green,
			UIColor.FromRGB(129,134,145),//Gray,
			UIColor.FromRGB(111,145,44),//Cyan,
			UIColor.FromRGB(145,88,123)//Magenta
		};

		public static string[] strcolores = {
			"48,88,147",//blue
			"145,147,63",//Yellow,
			"147,30,24",//Red,
			"62,53,147",//Purple,
			"147,123,117",//Brown,
			"147,88,17",//Orange,
			"57,147,104",//Green,
			"129,134,145",//Gray,
			"111,145,44",//Cyan,
			"145,88,123"//Magenta
		};

	}
}

