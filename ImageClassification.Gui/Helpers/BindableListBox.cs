using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using Wkiro.ImageClassification.Core.Models.Dto;

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
            SelectedItemsList = new ObservableCollection<Category>(SelectedItems.Cast<Category>());
        }

        public ObservableCollection<Category> SelectedItemsList
        {
            get { return (ObservableCollection<Category>)GetValue(SelectedItemsListProperty); }
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

        public static readonly DependencyProperty SelectedItemsListProperty = RegisterProperty(x => x.SelectedItemsList, null, x => x.OnSelectedItemsListChanged());

        private void OnSelectedItemsListChanged()
        {
            SetSelectedItems(SelectedItemsList);
        }

        public static readonly DependencyProperty SelectAllOnSourceChangeProperty = RegisterProperty(x => x.SelectAllOnSourceChange, false);

        #region Dependency Property initialization area

        private static string GetPropertyName<TObject1, T>(Expression<Func<TObject1, T>> exp)
        {
            var body = exp.Body;
            var convertExpression = body as UnaryExpression;
            if (convertExpression == null)
                return ((MemberExpression) body).Member.Name;

            if (convertExpression.NodeType != ExpressionType.Convert)
            {
                throw new ArgumentException("Invalid property expression.", nameof(exp));
            }
            body = convertExpression.Operand;
            return ((MemberExpression)body).Member.Name;
        }

        private static DependencyProperty RegisterProperty<T>(
            Expression<Func<BindableListBox, T>> associatedProperty,
            T defaultValue,
            Action<BindableListBox> valueChangedAction = null)
        {
            return DependencyProperty.Register(
                GetPropertyName(associatedProperty),
                typeof(T),
                typeof(BindableListBox),
                new PropertyMetadata(defaultValue, (s, e) =>
                {
                    var sender = s as BindableListBox;
                    if (sender != null)
                    {
                        valueChangedAction?.Invoke(sender);
                    }
                }));
        }

        #endregion
    }
}
