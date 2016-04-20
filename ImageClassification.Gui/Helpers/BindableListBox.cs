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

        void ListBoxCustom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsList = SelectedItems;
        }

        public IList SelectedItemsList
        {
            get { return (IList)GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsListProperty =
           DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(BindableListBox), new PropertyMetadata(null));

    }
}
