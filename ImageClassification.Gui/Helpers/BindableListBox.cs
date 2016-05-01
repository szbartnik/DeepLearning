using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Wkiro.ImageClassification.Gui.Helpers
{
    public class BindableListBox : ListBox
    {
        public BindableListBox()
        {
            SelectionChanged += ListBoxCustom_SelectionChanged;
        }

        private void ListBoxCustom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsList = SelectedItems;
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public bool SelectAllOnSourceChange
        {
            get { return (bool)GetValue(SelectAllOnSourceChangeProperty); }
            set { SetValue(SelectAllOnSourceChangeProperty, value); }
        }

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if(SelectAllOnSourceChange)
                SelectAll();
        }

        public static readonly DependencyProperty SelectedItemsListProperty = DependencyProperty
            .Register("SelectedItemsList", typeof(IList), typeof(BindableListBox), new PropertyMetadata(null));
        public static readonly DependencyProperty SelectAllOnSourceChangeProperty = DependencyProperty
            .Register("SelectAllOnSourceChange", typeof(bool), typeof(BindableListBox), new PropertyMetadata(defaultValue: false));
    }
}
