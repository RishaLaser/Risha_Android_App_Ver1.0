
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
using Android.Database;

namespace RishaApp
{
	[Activity (Label = "ViewImage")]			
	public class ViewImage : Activity
	{
		String PICTURE_FILENAME = "picture.jpg";

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.ViewImage);
			ImageView _imageView = FindViewById<ImageView> (Resource.Id.imageView2);

			bool cam_state = false;
			Android.Net.Uri URI = null;
			if (Intent.GetParcelableExtra ("ImageUri") != null) {
				URI = (Android.Net.Uri)Intent.GetParcelableExtra ("ImageUri");
				cam_state = false;
			}
				
			if (URI == null) {
				String text = Android.OS.Environment.ExternalStorageDirectory + "/" + PICTURE_FILENAME;
				URI = Android.Net.Uri.Parse (text);
				_imageView.SetImageURI(URI);
				cam_state = true;
			}
			else
				_imageView.SetImageURI(URI);


			Button _back = FindViewById<Button> (Resource.Id.buttonBack);
			Button _next = FindViewById<Button> (Resource.Id.buttonNext);

			_back.Click += delegate {
				StartActivity(typeof(MainActivity));
				Finish();
			};

			_next.Click += delegate {
				var rasterAct = new Intent (this, typeof(Filter));
				if (Intent.GetParcelableExtra ("ImageUri") != null)
				{
				rasterAct.PutExtra("ImagePath",GetPathToImage(URI));
				}
				rasterAct.PutExtra("camstate",cam_state);
				StartActivity (rasterAct);
			};
		}

		private string GetPathToImage(Android.Net.Uri uri)
		{
			string path = null;
			// The projection contains the columns we want to return in our query.
			string[] projection = new[] { Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data };
			using (ICursor cursor = ManagedQuery(uri, projection, null, null, null))
			{
				if (cursor != null)
				{
					int columnIndex = cursor.GetColumnIndexOrThrow(Android.Provider.MediaStore.Images.Media.InterfaceConsts.Data);
					cursor.MoveToFirst();
					path = cursor.GetString(columnIndex);
				}
			}
			return path;
		}
	}
}

