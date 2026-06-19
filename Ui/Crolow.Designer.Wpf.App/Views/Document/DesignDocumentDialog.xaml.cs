using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Crolow.Designer.Core.Document;
using Crolow.Designer.Core.Geometry;
using System.Windows;
using System.Windows.Input;

namespace Crolow.Designer.Wpf.App.Views.Document
{
    /// <summary>
    /// Interaction logic for DesignDocumentLayout.xaml
    /// </summary>
    public partial class DesignDocumentDialog : Window
    {
        public DesignDocumentDialog()
        {
            InitializeComponent();
        }
    }

    public class DesignDocumentDialogViewModel : ObservableObject
    {
        private readonly DesignDocument _document;

        public DesignDocumentDialogViewModel(DesignDocument document)
        {
            _document = document;

            Name = document.Name;
            Description = document.Description;
            FilePath = document.FilePath;

            Width = document.Size.Width;
            Height = document.Size.Height;

            TagsText = string.Join(", ", document.Tags);

            OkCommand = new RelayCommand(OnOk);
            CancelCommand = new RelayCommand(OnCancel);
        }


        public string Name { get; set; }

        public string Description { get; set; }

        public string FilePath { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public string TagsText { get; set; }

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public Action<bool>? CloseAction { get; set; }

        private void OnCancel()
        {
            CloseAction?.Invoke(false);
        }

        private void OnOk()
        {
            _document.Name = Name;
            _document.Description = Description;
            _document.FilePath = FilePath;
            _document.Size = new Size2D((float)Width, (float)Height);
            _document.Tags = TagsText
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList();

            CloseAction?.Invoke(true);
        }
    }
}
