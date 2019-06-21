using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Epicalsoft.Reach.Api.Client.Net;
using Epicalsoft.Reach.Api.Client.Net.Managers;
using Epicalsoft.Reach.Api.Client.Net.Models;
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

            ReachClient.InitWithClientCredentials("[clientId]", "[clientSecret]");

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
                var classifications = await GlobalScopeManager.Instance.GetClassificationsAsync();
                var roadTypes = await GlobalScopeManager.Instance.GetRoadTypesAsync();
                var countries = await GlobalScopeManager.Instance.GetCountriesAsync();

                FindViewById<TextView>(Resource.Id.classificationsTextView).Text = JsonConvert.SerializeObject(classifications, Formatting.Indented);
                FindViewById<TextView>(Resource.Id.roadTypesTextView).Text = JsonConvert.SerializeObject(roadTypes, Formatting.Indented);
                FindViewById<TextView>(Resource.Id.countiresTextView).Text = JsonConvert.SerializeObject(countries, Formatting.Indented);

                var byteArray = new byte[] { };
                var result = await UserScopeManager.Instance.UploadMediaFileAsync(new MediaFileData
                {
                    Code = new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    Target = MediaFileTarget.Evidences,
                    Kind = MediaFileKind.Image,
                    Data = Convert.ToBase64String(byteArray),
                    Filename = "incident_evidence_001.jpg"
                });
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