using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Geometry;
using System.Collections.ObjectModel;
using System.Windows;

namespace Crolow.Designer.Wpf.App.Views.Document
{
    /// <summary>
    /// Interaction logic for DesignDocumentLayout.xaml
    /// </summary>
    public partial class DesignDocumentEditorDialog : Window
    {
        public DesignDocumentEditorDialog()
        {
            InitializeComponent();
        }
    }

    public partial class DesignDocumentViewModel : ObservableObject
    {
        private readonly DesignDocument _document;

        public DesignDocumentViewModel(DesignDocument document)
        {
            _document = document;

            Name = document.Name;
            Description = document.Description;
            FilePath = document.FilePath;

            CanvasWidth = document.Size.Width;
            CanvasHeight = document.Size.Height;

            Tags = new ObservableCollection<string>(document.Tags);

            ApplyCommand = new RelayCommand(Apply);
            CancelCommand = new RelayCommand(Cancel);
        }

        #region Properties

        [ObservableProperty]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private string _filePath = string.Empty;

        [ObservableProperty]
        private float _canvasWidth;

        [ObservableProperty]
        private float _canvasHeight;

        public ObservableCollection<string> Tags { get; }

        #endregion

        #region Commands

        public RelayCommand ApplyCommand { get; }

        public RelayCommand CancelCommand { get; }

        #endregion

        #region Events

        public event Action<bool>? RequestClose;

        #endregion

        #region Private Methods

        private void Apply()
        {
            _document.Name = Name;
            _document.Description = Description;
            _document.FilePath = FilePath;

            _document.Size = new Size2D(CanvasWidth, CanvasHeight);

            _document.Tags.Clear();

            foreach (string tag in Tags)
                _document.Tags.Add(tag);

            RequestClose?.Invoke(true);
        }

        private void Cancel()
        {
            RequestClose?.Invoke(false);
        }

        #endregion
    }
}
