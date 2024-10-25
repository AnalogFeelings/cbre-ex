﻿using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CBRE.QuickForms.Items
{
    /// <summary>
    /// A control that shows a NumericUpDown control.
    /// </summary>
    public class QuickFormNumericUpDown : QuickFormItem
    {
        private readonly int _max;
        private readonly int _min;
        private readonly int _decimals;
        private readonly decimal _defaultValue;

        public QuickFormNumericUpDown(string nudname, int nudmin, int nudmax, int nuddecimals, decimal value)
        {
            Name = nudname;
            _min = nudmin;
            _max = nudmax;
            _decimals = nuddecimals;
            _defaultValue = value;
        }

        public override List<Control> GetControls(QuickForm qf)
        {
            List<Control> controls = new List<Control>();
            Label l = new Label { Text = Name, Font = SystemFonts.MessageBoxFont, FlatStyle = FlatStyle.System };
            Location(l, qf, true);
            Size(l, qf.LabelWidth);
            TextAlign(l);
            controls.Add(l);
            NumericUpDown n = new NumericUpDown
            {
                Maximum = _max,
                Minimum = _min,
                DecimalPlaces = _decimals,
                Name = Name,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Increment = (_decimals > 0) ? (1m / (_decimals * 10m)) : (1),
                Width = 80,
                Value = _defaultValue,
                Font = SystemFonts.MessageBoxFont
            };
            Location(n, qf, false);
            n.Location = new Point(qf.ClientSize.Width - QuickForm.ItemPadding - n.Width, n.Location.Y);
            controls.Add(n);
            return controls;
        }
    }
}
