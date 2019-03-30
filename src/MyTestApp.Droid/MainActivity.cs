using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Epicalsoft.Reach.Api.Client.Net;
using Epicalsoft.Reach.Api.Client.Net.Managers;
using Newtonsoft.Json;
using System;

namespace MyTestApp.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            ReachClient.Init("[clientId]", "[clientSecret]");

            FindViewById<Button>(Resource.Id.button1).Click += MainActivity_Click;
        }

        private void MainActivity_Click(object sender, EventArgs e)
        {
            ReachClient.ClearLocalCache();
        }

        protected override async void OnResume()
        {
            base.OnResume();

            try
            {
                var classifications = await GlobalScopeManager.Instance.GetClassifications();
                var roadTypes = await GlobalScopeManager.Instance.GetRoadTypes();
                var countries = await GlobalScopeManager.Instance.GetCountries();

                FindViewById<TextView>(Resource.Id.classificationsTextView).Text = JsonConvert.SerializeObject(classifications, Formatting.Indented);
                FindViewById<TextView>(Resource.Id.roadTypesTextView).Text = JsonConvert.SerializeObject(roadTypes, Formatting.Indented);
                FindViewById<TextView>(Resource.Id.countiresTextView).Text = JsonConvert.SerializeObject(countries, Formatting.Indented);
            }
            catch (ReachClientException ex)
            {
                Toast.MakeText(this, ex.ErrorMessage, ToastLength.Long).Show();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
    }
}