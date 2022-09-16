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
        private List<DTraceCall> _traceCalls = new List<DTraceCall>();
        private Timer _filterTimer = new Timer
        {
            Interval = 500,
        };

        public DGenerateGUI()
        {
            InitializeComponent();

            Icon = Icon.ExtractAssociatedIcon(System.Reflection.Assembly.GetExecutingAssembly().Location);
            listBoxCalls.SelectedValueChanged += ListBoxCalls_SelectedValueChanged;

            // Handle call filtering
            textBoxFilterCalls.TextChanged += (sender, e) =>
            {
                _filterTimer.Stop();
                _filterTimer.Start();
            };
            _filterTimer.Tick += (sender, e) =>
            {
                _filterTimer.Stop();
                listBoxCalls.DataSource = filterCalls(textBoxFilterCalls.Text.Trim());
            };
        }

        private void _filterTimer_Tick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private List<DTraceCall> filterCalls(string filter)
        {
            if (string.IsNullOrEmpty(filter))
            {
                return _traceCalls;
            }
            else
            {
                var result = new List<DTraceCall>();
                if (filter.StartsWith("handle:", StringComparison.InvariantCultureIgnoreCase))
                {
                    filter = filter.Substring(filter.IndexOf(':') + 1);
                    foreach (var call in _traceCalls)
                    {
                        call.Data.Visit(call.Function, (name, value) =>
                        {
                            if (value.Type == "HANDLE")
                            {
                                foreach (var kv in value.Struct)
                                {
                                    if (kv.Value.String != null)
                                    {
                                        if (kv.Value.String.ContainsCase(filter))
                                        {
                                            result.Add(call);
                                            return false;
                                        }
                                    }
                                }
                            }
                            return true;
                        });
                    }
                }
                else if (filter.StartsWith("raw:", StringComparison.InvariantCultureIgnoreCase))
                {
                    filter = filter.Substring(filter.IndexOf(':') + 1);
                    // Filter by raw content (slow)
                    foreach (var call in _traceCalls)
                    {
                        foreach (var line in call.Lines)
                        {
                            if (line.ContainsCase(filter))
                            {
                                result.Add(call);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    // Filter by function name
                    foreach (var call in _traceCalls)
                    {
                        if (call.Function.ContainsCase(filter))
                        {
                            result.Add(call);
                        }
                    }
                }
                return result;
            }
        }

        private void buttonCheckDtrace_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO");
        }

        private void buttonStartProcess_Click(object sender, EventArgs e)
        {
            MessageBox.Show("TODO");
        }

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
                    if (error.Fatal)
                        MessageBox.Show(this, $"Error at line {error.Line}: {error.Error}\nContext:\n{error.PrettyError}", "Parse error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
                parser.Call += (obj, call) =>
                {
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
                    if (!parser.ProcessLine(line))
                        break;
                }
                listBoxCalls.DataSource = _traceCalls;
            }

            textBoxFilterCalls.Text = "";
            textBoxFilterData.Text = "";
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
