using System;
using System.Reflection;
using Avalonia.Controls;

namespace CBRE.Extended.Editor
{
    public partial class MainWindow : Window
    {
        private static readonly Version _Version = Assembly.GetEntryAssembly()!.GetName().Version!;
        private readonly string _TitleStart = $"CBRE-EX v{_Version.ToString(3)}";
        
        public MainWindow()
        {
            InitializeComponent();
            UpdateTitle();
        }

        private void UpdateTitle()
        {
            // TODO: Check if document is opened, blah blah.
            this.Title = _TitleStart + " - No documents opened.";
        }
    }
}