using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace ScreenSlicer
{
    [TemplatePart(Name = PART_ListBox, Type = typeof(ListBox))]
    public class MultiSelectComboBox : ComboBox
    {
        #region Template parts

        internal const string PART_ListBox = "PART_ListBox";

        protected ListBox ListBox { get; private set; }

        #endregion //Template parts

        enum ItemsType
        {
            None = 0,
            Enum,
            IList,
            Array
        }

        #region SelectedItems

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(object), typeof(MultiSelectComboBox), new UIPropertyMetadata(null, OnSelectedItemsChanged));

        public object SelectedItems
        {
            get { return (object)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        private static void OnSelectedItemsChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
        {
            if (source is MultiSelectComboBox s)
                s.OnSelectedItemsChanged(args.OldValue, args.NewValue);
        }

        protected virtual void OnSelectedItemsChanged(object oldValue, object newValue)
        {
            if (newValue == null)
            {
                throw new NotImplementedException();
                _itemsType = ItemsType.None;
            }
            else if (newValue is IList list)
            {
                throw new NotImplementedException();
                _itemsType = ItemsType.IList;
                _listItems = list;
            }
            else if (newValue is Array array)
            {
                throw new NotImplementedException();
                _itemsType = ItemsType.Array;
                _arrayItems = array;
            }
            else if (newValue is Enum)
            {
                _itemsType = ItemsType.Enum;
                _enumItems = Convert.ToInt32(newValue);
                aaa = (Enum)newValue;
                _enumType = newValue.GetType();
                SelectionBoxItems = newValue.ToString();
            }
            else
            {
                throw new NotImplementedException();
                _itemsType = ItemsType.None;
            }
        }

        #endregion //SelectedItems

        #region SelectionBoxItems

        public object SelectionBoxItems
        {
            get { return (object)GetValue(SelectionBoxItemsProperty); }
            private set { SetValue(SelectionBoxItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectionBoxItemsProperty =
            DependencyProperty.Register(nameof(SelectionBoxItems), typeof(object), typeof(MultiSelectComboBox), new PropertyMetadata(null));

        #endregion //SelectionBoxItems

        private ItemsType _itemsType;
        private int _enumItems;
        private Enum aaa;
        private Type _enumType;
        private IList _listItems;
        private Array _arrayItems;

        public MultiSelectComboBox()
        {

        }

        public override void OnApplyTemplate()
        {
            if (ListBox != null)
                ListBox.SelectionChanged -= ListBox_SelectionChanged;

            base.OnApplyTemplate();

            ListBox = GetTemplateChild(PART_ListBox) as ListBox;
            ListBox.SelectedItems.Clear();

            switch (_itemsType)
            {
                case ItemsType.Enum:
                    foreach (Enum item in ItemsSource)
                    {
                        if (aaa.HasFlag(item))
                            ListBox.SelectedItems.Add(item);
                    }
                    break;
                default: throw new NotImplementedException();
            }

            ListBox.SelectionChanged += ListBox_SelectionChanged;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (_itemsType)
            {
                case ItemsType.Enum:
                    Enum result = Enum.ToObject(_enumType, 0) as Enum;
                    foreach (Enum item in ListBox.SelectedItems)
                        result = result.Add(item);
                    SelectedItems = result;
                    SelectionBoxItems = result.ToString();
                    break;
                default: throw new NotImplementedException();
            }
        }
    }
}
