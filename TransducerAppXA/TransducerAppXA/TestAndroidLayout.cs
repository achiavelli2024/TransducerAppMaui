using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransducerAppXA
{
    [Activity(Label = "Test Layout")]

    internal class TestAndroidLayout : Activity
    {

        TextView test0;
        TextView test1;
        Button btn0;
        Button btn1;



        protected override void OnCreate(Bundle savedInstanceState)

        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_teste_android);


           
            test0 = FindViewById<TextView>(Resource.Id.tx0);
            
            btn0 = FindViewById<Button>(Resource.Id.btn0);
            btn1 = FindViewById<Button>(Resource.Id.btn1);


        }



    }
}