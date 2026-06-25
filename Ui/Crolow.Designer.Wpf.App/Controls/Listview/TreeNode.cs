using Crolow.Designer.Common.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Crolow.Designer.Wpf.App.Controls
{
    public class TreeNode : INotifyPropertyChanged
    {
        private bool _isExpanded = false;
        private bool _isVisible = false;
        private bool _isSelected;

        public IDataObject DataObject { get; set; }
        public TreeNode Parent { get; set; }
        public string Text { get; set; } = "";
        public string? Icon { get; set; }

        public ObservableCollection<TreeNode> Children { get; }
            = new ObservableCollection<TreeNode>();

        public bool HasChildren => Children.Count > 0;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                if (_isExpanded == value)
                    return;

                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible == value)
                    return;

                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value)
                    return;

                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}