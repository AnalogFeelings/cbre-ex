﻿using CBRE.Common.Mediator;
using CBRE.DataStructures.MapObjects;
using CBRE.Editor.Documents;
using CBRE.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CBRE.Editor.UI.Sidebar
{
    public partial class VisgroupSidebarPanel : UserControl, IMediatorListener
    {
        public VisgroupSidebarPanel()
        {
            InitializeComponent();
            Mediator.Subscribe(EditorMediator.DocumentActivated, this);
            Mediator.Subscribe(EditorMediator.DocumentAllClosed, this);
            Mediator.Subscribe(EditorMediator.VisgroupsChanged, this);
            Mediator.Subscribe(EditorMediator.VisgroupVisibilityChanged, this);
        }

        public int? SelectedVisgroup
        {
            get { return VisgroupPanel.GetSelectedVisgroup(); }
        }

        private void DocumentActivated()
        {
            VisgroupPanel.Update(DocumentManager.CurrentDocument);
        }

        private void DocumentAllClosed()
        {
            VisgroupPanel.Update(DocumentManager.CurrentDocument);
        }

        private void VisgroupsChanged()
        {
            VisgroupPanel.Update(DocumentManager.CurrentDocument);
        }

        private void VisgroupVisibilityChanged(int visgroupId)
        {
            Document doc = DocumentManager.CurrentDocument;
            if (doc == null) return;

            // Update the group
            List<MapObject> visItems = GetVisgroupItems(visgroupId, doc);
            SetCheckState(visgroupId, visItems);

            // Update any other visgroups those objects are in
            IEnumerable<int> otherGroups = visItems.SelectMany(x => x.GetVisgroups(true)).Distinct().Where(x => x != visgroupId);
            foreach (int oid in otherGroups)
            {
                SetCheckState(oid, GetVisgroupItems(oid, doc));
            }
        }

        private void SetCheckState(int visgroupId, ICollection<MapObject> visItems)
        {
            if (visItems.Count > 0) // Only override default checkbox behavior if necessary
            {
                int numHidden = visItems.Count(x => x.IsVisgroupHidden);

                CheckState state;
                if (numHidden == visItems.Count && numHidden != 0) state = CheckState.Unchecked; // All hidden
                else if (numHidden > 0) state = CheckState.Indeterminate; // Some hidden
                else state = CheckState.Checked; // None hidden
                VisgroupPanel.SetCheckState(visgroupId, state);
            }
            else
            {
                VisgroupPanel.SetCheckState(visgroupId, CheckState.Indeterminate);
            }
        }

        private static List<MapObject> GetVisgroupItems(int visgroupId, Document doc)
        {
            List<MapObject> visItems = doc.Map.WorldSpawn.Find(x => x.IsInVisgroup(visgroupId, true), true);
            return visItems;
        }

        private void SelectButtonClicked(object sender, EventArgs e)
        {
            int? sv = SelectedVisgroup;
            if (sv.HasValue) Mediator.Publish(EditorMediator.VisgroupSelect, sv.Value);
        }

        private void EditButtonClicked(object sender, EventArgs e)
        {
            Mediator.Publish(EditorMediator.VisgroupShowEditor);
        }

        private void ShowAllButtonClicked(object sender, EventArgs e)
        {
            Mediator.Publish(EditorMediator.VisgroupShowAll);
        }

        private void NewButtonClicked(object sender, EventArgs e)
        {
            Mediator.Publish(HotkeysMediator.VisgroupCreateNew);
        }

        private void VisgroupToggled(object sender, int visgroupId, CheckState state)
        {
            Mediator.Publish(EditorMediator.VisgroupToggled, visgroupId, state);
        }

        private void VisgroupSelected(object sender, int? visgroupId)
        {

        }

        public void Notify(string message, object data)
        {
            Mediator.ExecuteDefault(this, message, data);
        }

        public void Clear()
        {
            VisgroupPanel.Clear();
        }

        private void VisgroupSelected(object sender)
        {

        }
    }
}
