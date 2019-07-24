using System;
using System.Collections.Generic;
using System.Linq;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using AView = Android.Views.View;

namespace HotUI.Android.Controls
{
    public class ModalManager
    {
        static ViewModal currrentDialog;
        static List<View> currentDialogs = new List<View>();
        public ModalManager()
        {
           
        }
        static FragmentManager FragmentManager => AndroidContext.AppCompatActivity.SupportFragmentManager;
        public static void ShowModal(View view)
        {
            var transaction = FragmentManager.BeginTransaction();
            if (currrentDialog != null)
                transaction.Remove(currrentDialog);
            transaction.AddToBackStack(null);

            var dialog = new ViewModal(view);
            currentDialogs.Add(dialog.HView);
            currrentDialog = dialog;
            dialog.Show(transaction, "dialog");
        }
        public static void DismisModal() => PerformDismiss(true);
        static void PerformDismiss(bool removeCurrent = true)
        {
            if (currrentDialog == null)
                return;
            var transaction = FragmentManager.BeginTransaction();

            if (removeCurrent)
            {
                transaction.Remove(currrentDialog);
                transaction.AddToBackStack(null);
            }

            currentDialogs.Remove(currrentDialog.HView);
            currrentDialog = null;
            var currentView = currentDialogs.LastOrDefault();
            if (currentView == null)
            {
                transaction.CommitAllowingStateLoss();
                return;
            }

            currrentDialog = new ViewModal(currentView);
            currrentDialog.Show(transaction, "dialog");
        }
       

        class ViewModal : DialogFragment
        {
            public ViewModal(View view)
            {
                HView = view;
            }

            public View HView { get; }

            AView currentBuiltView;
            public override AView OnCreateView(LayoutInflater inflater,
                ViewGroup container,
                Bundle savedInstanceState) => currentBuiltView = HView.ToView();

            public override void OnDestroy()
            {
                PerformDismiss(false);
                if (HView != null)
                {
                    HView.ViewHandler = null;
                }
                if (currentBuiltView != null)
                {
                    currentBuiltView?.Dispose();
                    currentBuiltView = null;
                }
                base.OnDestroy();
                this.Dispose();
            }
        }
    }
}
