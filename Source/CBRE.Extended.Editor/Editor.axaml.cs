using System;
using System.Reflection;
using Avalonia.Controls;
using CBRE.Extended.Editor.Logging;

namespace CBRE.Extended.Editor
{
    public partial class Editor : Window
    {
        public static Editor? Instance { get; private set; }
        
        public static readonly Version Version = Assembly.GetEntryAssembly()!.GetName().Version!;

        private readonly string _TitleStart = $"CBRE-EX v{Version.ToString(3)}";

        public Editor()
        {
            InitializeComponent();
            UpdateTitle();

            Instance = this;
        }

        private void UpdateTitle()
        {
            // TODO: Check if document is opened, blah blah.
            this.Title = _TitleStart + " - No documents opened.";
        }
    }
}