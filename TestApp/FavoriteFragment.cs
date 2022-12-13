using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.Widget;
using AndroidX.Fragment.App;
using Android.Support.V7.Widget.Helper;
using Com.Orangegangsters.Github.Swipyrefreshlayout.Library;

namespace TestApp
{
    public class FavoriteFragment : Fragment
    {
        RecyclerView _recyclerView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RetainInstance = true;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.news_layout, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            var nd = new NewsData();
            var adapter = new NewsAdapter(nd.GetFavoriteNews());
            adapter.HideNews += (sender, news) => nd.HideNews(news);
            adapter.AddToFavorite += (sender, news) => nd.AddFavorite(news);
            var layoutManager = new LinearLayoutManager(Context);
            layoutManager.StackFromEnd = true;
            _recyclerView.AddItemDecoration(new DividerItemDecoration(Context, layoutManager.Orientation));
            _recyclerView.SetAdapter(adapter);
            _recyclerView.SetLayoutManager(layoutManager);

            var callback = new SwipeCallback(adapter, Context, 0, ItemTouchHelper.Right);
            var itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(_recyclerView);

            var swipeRefreshLayout = view.FindViewById<SwipyRefreshLayout>(Resource.Id.swipeRefreshLayout);
            swipeRefreshLayout.Direction = SwipyRefreshLayoutDirection.Bottom;
            swipeRefreshLayout.Refresh += (sender, e) =>
            {
                swipeRefreshLayout.Refreshing = false;
            };
        }
    }
}