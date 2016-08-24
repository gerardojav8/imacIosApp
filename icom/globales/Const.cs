using System;
using UIKit;
namespace icom.globales
{
	public static class Consts
	{
		public static readonly string ipserv = "192.168.0.31";
		//public static readonly string ipservchat = "icom-chat-server-jav85861.c9users.io";
		public static readonly string ipservchat = "192.168.0.31";
		public static readonly string puertochat = ":3000";

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


	}
}

