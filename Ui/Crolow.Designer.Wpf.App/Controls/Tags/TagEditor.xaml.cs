using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Crolow.Designer.Wpf.App.Controls.Tags
{
    public partial class TagEditor : UserControl
    {
        public TagEditor()
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                UpdatePlaceholder();
            };

            MouseDown += (_, _) =>
            {
                PART_Input.Focus();
            };
        }

        #region Dependency Properties

        public ObservableCollection<string> Tags
        {
            get => (ObservableCollection<string>)GetValue(TagsProperty);
            set => SetValue(TagsProperty, value);
        }

        public static readonly DependencyProperty TagsProperty =
            DependencyProperty.Register(
                nameof(Tags),
                typeof(ObservableCollection<string>),
                typeof(TagEditor),
                new FrameworkPropertyMetadata(
                    new ObservableCollection<string>(),
                    //FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnTagsChanged));

        private static void OnTagsChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        {
            var editor = (TagEditor)d;

            if (e.OldValue is ObservableCollection<string> oldTags)
                oldTags.CollectionChanged -= editor.Tags_CollectionChanged;

            if (e.NewValue is ObservableCollection<string> newTags)
                newTags.CollectionChanged += editor.Tags_CollectionChanged;

            editor.UpdatePlaceholder();
        }

        #endregion

        private void Tags_CollectionChanged(object? sender,
            System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdatePlaceholder();
        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdatePlaceholder();
        }

        private void Input_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                case Key.OemComma:

                    AddCurrentTag();

                    e.Handled = true;
                    break;

                case Key.Back:

                    if (string.IsNullOrWhiteSpace(PART_Input.Text) &&
                        Tags.Count > 0)
                    {
                        Tags.RemoveAt(Tags.Count - 1);
                    }

                    break;
            }
        }

        private void RemoveTag_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button button)
                return;

            if (button.DataContext is not string tag)
                return;

            Tags.Remove(tag);
        }

        private void AddCurrentTag()
        {
            string tag = PART_Input.Text.Trim();

            if (string.IsNullOrWhiteSpace(tag))
                return;

            if (!Tags.Contains(tag))
                Tags.Add(tag);

            PART_Input.Clear();

            UpdatePlaceholder();
        }

        private void UpdatePlaceholder()
        {
            Placeholder.Visibility =
                 Tags.Count == 0 &&
                 string.IsNullOrWhiteSpace(PART_Input.Text)
                     ? Visibility.Visible
                     : Visibility.Collapsed;
        }
    }
}