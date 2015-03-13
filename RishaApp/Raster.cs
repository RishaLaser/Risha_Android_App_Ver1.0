
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace RishaApp
{
	[Activity (Label = "Rasterization", MainLauncher = false , Icon="@drawable/icon" , Theme="@android:style/Theme.Holo.Light")]			
	public class Raster : Activity
	{
		List<ColorCode> Colors = new List<ColorCode>();

		public string GenerateRasterCode(ColorCode p_cc = null)
		{
			//float scale = Math.Min(IMAGE_LENGTH_SCALED / IMAGE_LENGTH, IMAGE_WIDTH_SCALED / IMAGE_WIDTH);

			Bitmap b = Picture._picture;

			RasterInterpreter r = new RasterInterpreter(false);
			List<ColorCode> Rasters = new List<ColorCode>();

			//Raster all if inputed ColorCode is null
			if (p_cc == null)
			{
				foreach (ColorCode cc in Colors)
				{
					if (cc.Type == ColorCode.ColorType.Raster)
					{
						Rasters.Add(cc);
					}
				}
			}
			else //Raster only the ColorCode inputed
			{
				Rasters.Add(p_cc);
			}

			return r.Convert(b, Rasters);
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.raster);

			ImageView _img = FindViewById<ImageView> (Resource.Id.rasterImage);
			TextView _txt = FindViewById<TextView> (Resource.Id.textRaster);

			_img.SetImageBitmap (Picture._picture);

			ColorCode cc = new ColorCode();
			cc.Id = 0;
			cc.Ccolor = Color.Black;
			cc.Type = ColorCode.ColorType.Raster;
			cc.Power = 80;
			cc.Speed = 0;
			Colors.Add (cc);

			string rastercode = GenerateRasterCode ();

			_txt.Text = rastercode;

			Button _start = FindViewById<Button> (Resource.Id.buttonStart);
			_start.Click += delegate {
				Picture._rastercode = rastercode;
				StartActivity(typeof(BluetoothChat));
				Finish();
			};
		}
			
	}
}

