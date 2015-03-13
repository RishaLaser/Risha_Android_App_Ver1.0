
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
using Java.IO;
using Android.Database;


namespace RishaApp
{
	[Activity (Label = "Filter", MainLauncher = false , Icon="@drawable/icon" , Theme="@android:style/Theme.Holo.Light")]			
	//[Activity (Label = "Filter", 
	//	Theme = "@android:style/Theme.Dialog", 
	//	ConfigurationChanges=Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.Orientation)]	
	public class Filter : Activity, SeekBar.IOnSeekBarChangeListener
	{
		int avg = 127;
		public Bitmap b;
		public Bitmap _output;

		public Bitmap make_bw_normal(Bitmap original) {

			Bitmap output = Bitmap.CreateBitmap(original.Width, original.Height,Bitmap.Config.Argb8888);

			for (int i = 0; i < original.Width; i++) {

				for (int j = 0; j < original.Height ; j--) {

					int k = original.GetPixel(i, j);

					Color c = Color.Rgb (Color.GetRedComponent (k), Color.GetGreenComponent (k), Color.GetBlueComponent (k));

					int average = ((c.R + c.B + c.G) / 3);

					if (average < avg)
						output.SetPixel(i, j, Color.Black);

					else
						output.SetPixel(i, j, Color.White);

				}
			}

			return output;       

		}
		public Bitmap make_bw(Bitmap original) {

			Bitmap output = Bitmap.CreateBitmap(original.Height, original.Width,Bitmap.Config.Argb8888);

			for (int i = 0; i < original.Width; i++) {

				for (int j = original.Height - 1; j > 0 ; j--) {
				
					int k = original.GetPixel(i, j);

					Color c = Color.Rgb (Color.GetRedComponent (k), Color.GetGreenComponent (k), Color.GetBlueComponent (k));

					int average = ((c.R + c.B + c.G) / 3);

					if (average < avg)
						output.SetPixel(original.Height - j, i, Color.Black);

					else
						output.SetPixel(original.Height - j, i, Color.White);

				}
			}

			return output;       

		}

		public void ApplyFilter(Bitmap b)
		{
			//Black and White
			_output = make_bw(b);

			//Output to ImageView
			ImageView img = FindViewById<ImageView> (Resource.Id.filteredimage);
			//img.SetMaxHeight (340);
			img.SetImageBitmap (_output);

		}

		String PICTURE_FILENAME = "picture.jpg";
		//Bitmap b = null;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Filter);

			SeekBar s = FindViewById<SeekBar> (Resource.Id.seekBar1);
			s.Max = 255;
			s.Progress = avg;
			s.SetOnSeekBarChangeListener(this);

			Button _back = FindViewById<Button> (Resource.Id.buttonBack);
			Button _next = FindViewById<Button> (Resource.Id.buttonNext);

			_back.Click += delegate {
				if((bool)Intent.GetParcelableExtra ("camstate"))
					StartActivity(typeof(Cam));
				else
					StartActivity(typeof(MainActivity));

				Finish();
			};

			_next.Click += delegate {
				Picture._picture = _output;
				StartActivity(typeof(Raster));
				Finish();
			};

			String strPath = null;
			if (Intent.GetStringExtra ("ImagePath") != null && !(bool)Intent.GetParcelableExtra ("camstate")) {
				strPath = Intent.GetStringExtra ("ImagePath");
				b = BitmapFactory.DecodeFile (strPath);
			} else {
				File dataDir = Android.OS.Environment.ExternalStorageDirectory;

				b = BitmapFactory.DecodeFile (dataDir + "/" + PICTURE_FILENAME);
			}
			//ERROR: PATH OF IMAGE COMING FROM ViewImage activity does not work here !!!
			//b = BitmapFactory.DecodeFile (strPath);

			//Get Image From External Storage
			//File dataDir = Android.OS.Environment.ExternalStorageDirectory;

			//b = BitmapFactory.DecodeFile (dataDir + "/" + PICTURE_FILENAME);
			//Bitmap output = b;

			ApplyFilter (b);


			//	avg = s.Progress;
			//	ApplyFilter(b);
			


		}
		public void OnProgressChanged(SeekBar seekBar, int progress, bool fromUser)
		{
			if (fromUser)
			{
				//Get Image From External Storage
				//File dataDir = Android.OS.Environment.ExternalStorageDirectory;

				//Bitmap b = BitmapFactory.DecodeFile (dataDir + "/" + PICTURE_FILENAME);
				//Bitmap output = b;
			    avg = seekBar.Progress;
				ApplyFilter(b);
			}
		}

		public void OnStartTrackingTouch(SeekBar seekBar)
		{
			System.Diagnostics.Debug.WriteLine("Tracking changes.");
		}

		public void OnStopTrackingTouch(SeekBar seekBar)
		{
			System.Diagnostics.Debug.WriteLine("Stopped tracking changes.");
		}
	}
}

