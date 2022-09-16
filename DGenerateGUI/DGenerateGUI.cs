using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Dynamic;
using Newtonsoft.Json;

namespace DGenerateGUI
{
    public partial class DGenerateGUI : Form
    {
        public DGenerateGUI()
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            listBoxCalls.SelectedValueChanged += ListBoxCalls_SelectedValueChanged;
        }

        private void buttonCheckDtrace_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO");
        }

        private void buttonStartProcess_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO");
        }

        private List<DTraceCall> _traceCalls = new List<DTraceCall>();

        private void buttonLoadFile_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Trace logs (*.txt)|*.txt|All files (*.*)|*.*",
            };
            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            using (var sr = new StreamReader(ofd.FileName))
            {
                bool stop = false;
                bool started = false; // TODO: can be used to suppress errors
                var parser = new DTraceParser();
                parser.ParseError += (obj, error) =>
                {
                    if(error.Fatal)
                        MessageBox.Show(this, $"Error at line {error.Line}: {error.Error}\nContext:\n{error.PrettyError}", "Parse error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
                parser.Call += (obj, call) =>
                {
                    // Save some memory for the test
                    call.Lines = null;
                    _traceCalls.Add(call);
                };
                while (!stop)
                {
                    var line = sr.ReadLine();
                    if (line == null)
                        break;
                    if (line == "Started up")
                    {
                        started = true;
                        continue;
                    }
                    if(!parser.ProcessLine(line))
                        break;
                }
                listBoxCalls.DataSource = _traceCalls;
            }
        }

        private static TreeNode ConvertNode(string name, MemberValue value)
        {
            var node = new TreeNode($"{name}: {value}");
            if (value.Struct != null)
            {
                foreach (var kv in value.Struct)
                {
                    node.Nodes.Add(ConvertNode(kv.Key, kv.Value));
                }
            }

            node.ExpandAll();
            return node;
        }

        private void ListBoxCalls_SelectedValueChanged(object sender, EventArgs e)
        {
            var selectedCall = listBoxCalls.SelectedValue as DTraceCall;
            if (selectedCall == null)
                return;

            treeViewData.Nodes.Clear();
            var root = selectedCall.Data.Struct;
            foreach (var kv in root)
            {
                treeViewData.Nodes.Add(ConvertNode(kv.Key, kv.Value));
            }

            // Scroll to the top
            treeViewData.Nodes[0].EnsureVisible();
        }
    }
}
