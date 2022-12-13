using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;
using System.Collections.Generic;

namespace TestApp
{
    [Activity(Label = "@string/main", Theme = "@style/AppTheme", MainLauncher = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content, new MainFragment()).Commit();
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.navigation_main:
                    Title = Resources.GetString(Resource.String.main);
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content, new MainFragment()).Commit();
                    
                    return true;
                case Resource.Id.navigation_favorite:
                    Title = Resources.GetString(Resource.String.favorite);
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.content, new FavoriteFragment()).Commit();
                    return true;
            }
            return false;
        }
    }
}

