
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
using Android.Bluetooth;


namespace RishaApp
{
	[Activity (Label = "Bluetooth" ,MainLauncher = false, Icon="@drawable/icon" , Theme="@android:style/Theme.Holo.Light")]		
	//[Activity (Label = "Bluetooth", 
	//	Theme = "@android:style/Theme.Dialog", 
	//	ConfigurationChanges=Android.Content.PM.ConfigChanges.KeyboardHidden | Android.Content.PM.ConfigChanges.Orientation)]	
	public class BTSelect : ListActivity
	{
		List<string> listitems = new List<string> (); 
		BluetoothSocket _socket;
		BluetoothDevice device;
		//string[] data = {"one", "two", "three", "four", "five"} ;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			//SetContentView (Resource.Layout.BTList);

			BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;
			if(adapter == null)
				throw new Exception("No Bluetooth adapter found.");

			if(!adapter.IsEnabled)
				throw new Exception("Bluetooth adapter is not enabled.");


			//BluetoothDevice device
			//List<BluetoothDevice> items = adapter.BondedDevices.ToList();

			device = (from bd in adapter.BondedDevices 
				where bd.Name == "HC-05" select bd).FirstOrDefault();

			if(device == null)
				throw new Exception("Named device not found.");

			listitems = new List<string> (); 
			//foreach (BluetoothDevice d in items) {
				listitems.Add (device.Name);
			//}

			ArrayAdapter adapter2 = new ArrayAdapter (this,Resource.Layout.TextViewItem, listitems.ToArray());        
			ListAdapter = adapter2;

			//if(device == null)
			//	throw new Exception("Named device not found.");
		}
		protected override void OnListItemClick (ListView l, View v,int position, long id)
		{
			base.OnListItemClick (l, v, position, id);
			//Toast.MakeText (this, listitems [position],
			//	ToastLength.Short).Show ();

			//BluetoothDevice dev = l.GetChildAt (position);
			//View v = l.GetChildAt (position);
			//string str = v.ToString();

			//int k = 0 ;
			//_socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
			//await _socket.ConnectAsync();
			StartActivity(typeof(BluetoothChat));
		}
	}
}

