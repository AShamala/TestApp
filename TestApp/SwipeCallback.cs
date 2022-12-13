using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static TestApp.NewsAdapter;

namespace TestApp
{
    public class SwipeCallback : ItemTouchHelper.SimpleCallback
    {
        private NewsAdapter _adapter;
        private Context _context;
        private int _dragDirs;
        private int _swipeDirs;
        public EventHandler RevertSwipe;
        public SwipeCallback(NewsAdapter adapter, Context context, int dragDirs, int swipeDirs) : base(0, 0)
        {
            _context = context;
            _adapter = adapter;
            _dragDirs = dragDirs;
            _swipeDirs = swipeDirs;
    }

        public override int GetMovementFlags(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            int dragFlags = _dragDirs;
            int swipeFlags = _swipeDirs;
            return MakeMovementFlags(dragFlags, swipeFlags);
        }

        public override void OnChildDraw(Canvas c, RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, float dX, float dY, int actionState, bool isCurrentlyActive)
        {
            var width = viewHolder.ItemView.Width / 4;
            var dx = Math.Abs(dX) > width ? dX > 0 ? width : - width : dX;
            base.OnChildDraw(c, recyclerView, viewHolder, dx, dY, actionState, isCurrentlyActive);   
        }

        public override float GetSwipeThreshold(RecyclerView.ViewHolder viewHolder)
        {
            return 0.25f;
        }

        public override bool IsItemViewSwipeEnabled => true;

        public override bool OnMove(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder, RecyclerView.ViewHolder target)
        {
            return false;
        }

        public override void OnSwiped(RecyclerView.ViewHolder viewHolder, int direction)
        {
            if (direction == ItemTouchHelper.Right) 
            {
                _adapter.RemoveItem(viewHolder.AdapterPosition);
                var toast = Toast.MakeText(_context, "Скрыто из ленты", ToastLength.Short);
                toast.Show();
            }
            else if (direction == ItemTouchHelper.Left)
            {
                _adapter.SwipeItem(viewHolder.AdapterPosition);
                RevertSwipe?.Invoke(this, EventArgs.Empty);
                var toast = Toast.MakeText(_context, "Добавлено в избранное", ToastLength.Short);
                toast.Show();
            }
        }
    }
}