using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CbrHackaton.CustomControls
{
    class StackLayoutList : StackLayout
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IList), typeof(StackLayoutList), propertyChanged: OnItemsSourceChanged);

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(StackLayoutList), propertyChanged: OnItemTemplateChanged);

        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        private static void OnItemTemplateChanged(BindableObject pObj, object pOldVal, object pNewVal)
        {
            var layout = pObj as StackLayoutList;

            if (layout != null && layout.ItemsSource != null)
                layout.BuildLayout();
        }

        private static void OnItemsSourceChanged(BindableObject pObj, object pOldVal, object pNewVal)
        {
            var layout = pObj as StackLayoutList;

            if (layout != null && layout.ItemTemplate != null)
                layout.BuildLayout();
        }

        private void BuildLayout()
        {
            Children.Clear();

            foreach (var item in ItemsSource)
            {
                var view = (View)ItemTemplate.CreateContent();
                view.BindingContext = item;
                Children.Add(view);
            }
        }
    }
}
