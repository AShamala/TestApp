using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestApp
{
    public class NewsAdapter : RecyclerView.Adapter
    {
        private List<News> _newsList;

        public NewsAdapter(List<News> newsList)
        {
            _newsList = newsList;
        }

        public override int ItemCount => _newsList.Count;

        public EventHandler<News> HideNews;
        public EventHandler<News> AddToFavorite;

        private void OnHide(News news)
        {
            if (HideNews != null)
            {
                HideNews(this, news);
            }
        }

        private void OnAddToFavorite(News news)
        {
            if (AddToFavorite != null)
            {
                AddToFavorite(this, news);
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var viewHolder = holder as NewsHolder;
            var title = _newsList[position].Title;
            var content = _newsList[position].Content;
            viewHolder.Title.Text = title;
            viewHolder.Content.Text = content;
            if (!viewHolder.Button.HasOnClickListeners)
            {
                viewHolder.Button.SetOnClickListener(new ButtonAction(viewHolder));
            }
        }

        public void RemoveItem(int position)
        {
            OnHide(_newsList[position]);
            NotifyItemRemoved(position);
        }

        public void SwipeItem(int position)
        {
            OnAddToFavorite(_newsList[position]);  
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.news_item, parent, false);
            var viewHolder = new NewsHolder(itemView);
            return viewHolder;
        }

        public override long GetItemId(int position)
        {
            return _newsList[position].Id;
        }

        public override int GetItemViewType(int position)
        {
            return _newsList[position].Id;
        }

        public class NewsHolder : RecyclerView.ViewHolder
        {
            public TextView Title { get; private set; }
            public TextView Content { get; private set; }
            public TextView Button { get; private set; }

            public NewsHolder(View itemView) : base(itemView)
            {
                Title = itemView.FindViewById<TextView>(Resource.Id.title);
                Content = itemView.FindViewById<TextView>(Resource.Id.content);
                Button = itemView.FindViewById<TextView>(Resource.Id.button);               
            }
        }

        internal class ButtonAction : Java.Lang.Object, View.IOnClickListener
        {
            NewsHolder _holder;

            public ButtonAction(RecyclerView.ViewHolder holder)
            {
                _holder = holder as NewsHolder;
            }

            public void OnClick(View v)
            {
                if (_holder.Content.Visibility == ViewStates.Gone)
                {
                    _holder.Button.Text = "Скрыть";
                    _holder.Content.Visibility = ViewStates.Visible;
                }
                else
                {
                    _holder.Button.Text = "Показать еще";
                    _holder.Content.Visibility = ViewStates.Gone;
                };
            }
        }
    }
}