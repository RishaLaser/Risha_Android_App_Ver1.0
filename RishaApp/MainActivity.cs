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
using Android.Provider;
using Android.Database;


namespace RishaApp
{

	[Activity (Label = "Risha", MainLauncher = true, Icon="@drawable/icon" , Theme="@android:style/Theme.Holo.Light")]
	//[Activity (Label = "Risha", MainLauncher = true,
	//	Theme = "@android:style/Theme.Dialog", 
	//	ConfigurationChanges=Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.Orientation)]	
	public class MainActivity : Activity
	{
		public static readonly int PickImageId = 1000;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Create your application here
			SetContentView (Resource.Layout.Main);

			Button b = FindViewById<Button> (Resource.Id.camButton);

			b.Click += delegate {
				var camera = new Intent (this, typeof(Cam));
				StartActivity(camera);
			};

			b = FindViewById<Button> (Resource.Id.selectPicButton);

			b.Click += delegate {
				Intent = new Intent();
				Intent.SetType("image/*");
				Intent.SetAction(Intent.ActionGetContent);
				StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), PickImageId);
			};


		}

		protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
		{
			if ((requestCode == PickImageId) && (resultCode == Result.Ok) && (data != null))
			{
				Android.Net.Uri uri = data.Data;


				string path = GetPathToImage(uri);
				Toast.MakeText(this, path, ToastLength.Long);

				//Open the ViewImage activity:
				var viewImageAct = new Intent (this, typeof(ViewImage));
				viewImageAct.PutExtra("ImageUri",uri);
				//viewImageAct.PutExtra ("ImagePath", GetPathToImage (uri));
				StartActivity (viewImageAct);
			}
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


