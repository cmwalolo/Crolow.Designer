using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Geometry;
using Crolow.Designer.Wpf.App.Extensions;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

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

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            Keyboard.FocusedElement?.UpdateBindings();
        }
    }

    public partial class DesignDocumentViewModel : ObservableValidator
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
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "The document name is required.")]
        private string _name = string.Empty;

        [ObservableProperty]
        private string _description = string.Empty;

        [ObservableProperty]
        private string _filePath = string.Empty;

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Size is required.")]
        [Range(1, 1000000, ErrorMessage = "Size is required.")]
        private float _canvasWidth;

        [ObservableProperty]
        [Required(ErrorMessage = "Size is required.")]
        [Range(1, 1000000, ErrorMessage = "Size is required.")]
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
            ValidateAllProperties();
            if (HasErrors)
                return;

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
