using System;
using Android.Graphics;

namespace RishaApp
{
	public class Picture
	{
		public Picture (Bitmap k)
		{
			_picture = k;
		}
			
		public static Bitmap _picture;
		public static string _rastercode;
		public static string[] lines;
		public static int totalsteps = 0;
	}
}

