using System;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Hardware;
using Java.IO;

namespace RishaApp
{
	[Activity (Label = "Camera" ,MainLauncher = false, Icon="@drawable/icon" , Theme="@android:style/Theme.Holo.Light")]			
	//[Activity (Label = "Camera", 
	//	Theme = "@android:style/Theme.Holo.Light", 
	//	ConfigurationChanges=Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.Orientation)]	
	public class Cam : Activity, Android.Hardware.Camera.IPictureCallback ,Android.Hardware.Camera.IPreviewCallback, Android.Hardware.Camera.IShutterCallback, ISurfaceHolderCallback
	{

		Camera camera;
		String PICTURE_FILENAME = "picture.jpg";
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Cam);

			SurfaceView surface = (SurfaceView)FindViewById (Resource.Id.surface);
			var holder = surface.Holder;
			holder.AddCallback (this);
			holder.SetType (Android.Views.SurfaceType.PushBuffers);

			FindViewById(Resource.Id.myButton).Click += delegate {
				Android.Hardware.Camera.Parameters p = camera.GetParameters();
				p.PictureFormat = Android.Graphics.ImageFormatType.Jpeg;
				//p.SetPreviewSize(320,200);
				p.SetRotation(90);
				camera.SetParameters(p);
				//camera.SetDisplayOrientation(90);
				camera.TakePicture(this,this,this);
				System.Threading.Thread.Sleep(1000);
				camera.SetPreviewCallback(null);


				//StartActivity(typeof(Filter));
				StartActivity(typeof(ViewImage));

				Finish();
			};
		}
		void Camera.IPictureCallback.OnPictureTaken(byte[] data, Android.Hardware.Camera camera)
		{
			FileOutputStream outStream = null;
			File dataDir = Android.OS.Environment.ExternalStorageDirectory;
			if (data != null) {
				try{
					outStream = new FileOutputStream(dataDir + "/" + PICTURE_FILENAME);
					outStream.Write(data);
					outStream.Close();
				}catch(FileNotFoundException e){
					System.Console.Out.WriteLine (e.Message);
				}catch(IOException ie){
					System.Console.Out.WriteLine (ie.Message);
				}
			}
		}

		void Camera.IPreviewCallback.OnPreviewFrame(byte[] b, Android.Hardware.Camera c)
		{

		}

		void Camera.IShutterCallback.OnShutter()
		{

		}


		public void SurfaceCreated(ISurfaceHolder holder)
		{
			try{
				camera = Android.Hardware.Camera.Open();
				Android.Hardware.Camera.Parameters p = camera.GetParameters();
				p.PictureFormat = Android.Graphics.ImageFormatType.Jpeg;
				camera.SetParameters(p);
				camera.SetPreviewCallback(this);
				camera.Lock();
				camera.SetDisplayOrientation(90);
				p.SetPreviewSize(320,360);
				camera.SetPreviewDisplay(holder);
				camera.StartPreview();
			}
			catch(IOException e){
			}
		}

		public void SurfaceDestroyed(ISurfaceHolder holder){

			camera.Unlock ();
			camera.StopPreview ();
			camera.Release ();
		}

		public void SurfaceChanged(ISurfaceHolder holder,Android.Graphics.Format f,int i, int j)
		{
		}
	}
}

