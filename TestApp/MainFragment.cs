using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.SwipeRefreshLayout.Widget;
using Com.Orangegangsters.Github.Swipyrefreshlayout.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AndroidX.SwipeRefreshLayout.Widget.SwipeRefreshLayout;

namespace TestApp
{
    public class MainFragment : Fragment
    {
        RecyclerView _recyclerView;
        SwipyRefreshLayout _swipeRefreshLayout;

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
            var nd = new NewsData();
            
            _recyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            
            var adapter = new NewsAdapter(nd.GetNews());
            LoadDataAsync(nd, adapter);
            adapter.HideNews += (sender, news) => nd.HideNews(news);
            adapter.AddToFavorite += (sender, news) => nd.AddFavorite(news);
            adapter.HasStableIds = true;
            var layoutManager = new LinearLayoutManager(Context);
            layoutManager.StackFromEnd = true;
            _recyclerView.AddItemDecoration(new DividerItemDecoration(Context, layoutManager.Orientation));
            _recyclerView.SetAdapter(adapter);
            _recyclerView.SetLayoutManager(layoutManager);

            var callback = new SwipeCallback(adapter, Context, 0, ItemTouchHelper.Right | ItemTouchHelper.Left);           
            var itemTouchHelper = new ItemTouchHelper(callback);
            itemTouchHelper.AttachToRecyclerView(_recyclerView);
            callback.RevertSwipe += (s, e) =>
            {
                itemTouchHelper.AttachToRecyclerView(null);
                itemTouchHelper.AttachToRecyclerView(_recyclerView);
            };
            
            _swipeRefreshLayout = view.FindViewById<SwipyRefreshLayout>(Resource.Id.swipeRefreshLayout);
            _swipeRefreshLayout.Direction = SwipyRefreshLayoutDirection.Bottom;
            _swipeRefreshLayout.Refresh += async (sender, e) =>
            {
                await RefreshDataAsync(nd, adapter);
            };           
        }


        public async Task LoadDataAsync(NewsData nd, NewsAdapter adapter) 
        {
            await nd.LoadDataAsync();
            adapter.NotifyDataSetChanged();
        }

        public async Task RefreshDataAsync(NewsData nd, NewsAdapter adapter)
        {
            await LoadDataAsync(nd, adapter);
            _swipeRefreshLayout.Refreshing = false;
        }
    }
}