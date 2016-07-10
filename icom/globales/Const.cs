using System;
namespace icom.globales
{
	public static class Consts
	{
		public static readonly string ipserv = "192.168.0.16";
		public static readonly string urltoken = "http://"+ ipserv + "/icomtoken/oauth2/token";
		public static readonly string ulrserv = "http://"+ ipserv + "/icomApi/";

		public static readonly int Trascabo = 1;
		public static readonly int Revolvedora = 2;
		public static readonly int Excabadora = 3;

		public static string token = "";


	}
}

