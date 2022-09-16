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
        }

        private void buttonCheckDtrace_Click(object sender, EventArgs e)
        {

        }

        private void buttonStartProcess_Click(object sender, EventArgs e)
        {

        }

        class ParseError
        {
            public int Line;
            public string Error;
            public string Context;
        }

        public class MemberValue
        {
            [JsonIgnore]
            public string Type;

            [JsonIgnore]
            public string Ptr;

            [JsonProperty(PropertyName = "Type", Order = 1)]
            public string PrettyType => Type + Ptr;

            [JsonProperty(PropertyName = "Bitfield", Order = 2, DefaultValueHandling = DefaultValueHandling.Ignore)]
            public int? BitSize;

            [JsonProperty(Order = 2)]
            public object Value;

            [JsonIgnore]
            public Dictionary<string, MemberValue> Struct => Value as Dictionary<string, MemberValue>;

            [JsonIgnore]
            public string Str => Value as string;

            public MemberValue(string type, string ptr, object value, int? bitsize)
            {
                Type = type;
                Ptr = ptr;
                Value = value;
                BitSize = bitsize;
            }

            public void FlattenStrings()
            {
                var s = Struct;
                if (s == null)
                    return;
                foreach (var kv in s)
                {
                    if (kv.Value.Struct != null)
                    {
                        var child = kv.Value.Struct;
                        if (child.Count == 1)
                        {
                            var v = child.First().Value;
                            if (v.Type.StartsWith("char [") && v.Str.StartsWith("[ "))
                            {
                                // TODO: handle char values ' '
                                var str = v.Str;
                                str = str.Substring(3, str.Length - 6);
                                // Replace the object with a string value
                                kv.Value.Value = str;
                            }
                        }
                        else
                            kv.Value.FlattenStrings();
                    }
                }
            }

            public override string ToString()
            {
                if (Struct != null)
                    return $"{PrettyType} => {{ ... }}";
                else
                    return $"{PrettyType} => {Str}";
            }
        }

        public class DTraceCall
        {
            public string Function;
            public MemberValue Data;
            public string[] Lines;

            public override string ToString()
            {
                return Function;
            }
        }

        class DTraceParser
        {
            public event EventHandler<string> Started;
            public event EventHandler<DTraceCall> Call;
            public event EventHandler<ParseError> ParseError;

            public DTraceParser()
            {
            }

            public bool ProcessLine(string line)
            {
                _lineNumber++;
                if (_started)
                    return processStarted(line);
                else
                    return processPre(line);
            }

            bool processPre(string line)
            {
                _pre += line + "\n";
                if (line == "Started up")
                {
                    _started = true;
                    Started?.Invoke(this, _pre);
                }
                return true;
            }

            bool parseError(string error)
            {
                var lastContext = _context.Skip(Math.Max(0, _context.Count() - 20));
                ParseError?.Invoke(this, new ParseError
                {
                    Line = _lineNumber,
                    Error = error,
                    Context = string.Join("\n", lastContext),
                });
                return false;
            }

            private bool _started = false;
            private string _pre = "";
            private Regex _reNeutralCall = new Regex(@"^struct (.+)CALL {$");
            private Regex _reCloseCall = new Regex(@"^}struct (.+)CALL {$");
            private Regex _reMember = new Regex(@"^ *(.+) (\**)([a-zA-Z0-9_]+)( :\d+)? = (.+)$");
            private List<string> _context = new List<string>();
            private int _lineNumber = 0;

            class StructState
            {
                public StructState(string name)
                {
                    Name = name; ;
                    Root = new MemberValue(name, "", new Dictionary<string, MemberValue>(), null);
                    _stack.Push(Root);
                }

                [JsonIgnore]
                private Stack<MemberValue> _stack = new Stack<MemberValue>();

                public string Name;
                public MemberValue Root;

                [JsonIgnore]
                public int Count => _stack.Count;

                public int Pop()
                {
                    _stack.Pop();
                    return _stack.Count;
                }

                public void AddMemberValue(string type, string ptr, string name, string value, int? bitsize)
                {
                    ((Dictionary<string, MemberValue>)_stack.Peek().Value).Add(name, new MemberValue(type, ptr, value, bitsize));
                }

                public void PushMemberStruct(string type, string ptr, string name)
                {
                    var value = new MemberValue(type, ptr, new Dictionary<string, MemberValue>(), null);
                    ((Dictionary<string, MemberValue>)_stack.Peek().Value).Add(name, value);
                    _stack.Push(value);
                }
            }

            private StructState _struct;

            bool flush(StructState fstruct)
            {
                fstruct.Root.FlattenStrings();

                Call?.Invoke(this, new DTraceCall
                {
                    Lines = _context.ToArray(),
                    Function = fstruct.Name,
                    Data = fstruct.Root,
                });

                // Clear the context
                _context.Clear();

                return true;
            }

            bool processStarted(string line)
            {
                _context.Add(line);

                if (_struct == null)
                {
                    var m = _reNeutralCall.Match(line);
                    if (!m.Success)
                        return parseError("Failed to match struct header");

                    _struct = new StructState(m.Groups[1].Value);
                }
                else
                {
                    var m = _reMember.Match(line);
                    if (m.Success)
                    {
                        if (_lineNumber == 47904)
                            Debugger.Break();
                        var type = m.Groups[1].Value;
                        var ptr = m.Groups[2].Value;
                        var name = m.Groups[3].Value;
                        int? bitsize = null;
                        if (!string.IsNullOrEmpty(m.Groups[4].Value))
                        {
                            if (!int.TryParse(m.Groups[4].Value.Trim(' ', ':'), out int bitfieldValue))
                                return false;
                            bitsize = bitfieldValue;
                        }
                        var value = m.Groups[5].Value.Trim();
                        if (value == "{")
                        {
                            _struct.PushMemberStruct(type, ptr, name);
                        }
                        else if(value == "[")
                        {
                            return parseError("arrays not yet supported");
                            // TODO: support arrays
                        }
                        else
                        {
                            _struct.AddMemberValue(type, ptr, name, value, bitsize);
                        }
                    }
                    else
                    {
                        var trim = line.Trim();
                        if (!trim.StartsWith("}"))
                            return parseError("Failed to match member line");

                        if (_struct.Count == 0)
                            return parseError("Invalid nesting level");

                        if (_struct.Pop() == 0)
                        {
                            var fstruct = _struct;

                            if (trim == "}")
                            {
                                _struct = null;
                            }
                            else
                            {
                                // End of the root object, create a new one
                                m = _reCloseCall.Match(line);
                                if (!m.Success)
                                    return parseError("Failed to match closing call");

                                _struct = new StructState(m.Groups[1].Value);
                            }

                            if (!flush(fstruct))
                                return false;
                        }
                    }
                }
                return true;
            }
        }

        private List<DTraceCall> _traceCalls = new List<DTraceCall>();

        private void buttonTest_Click(object sender, EventArgs e)
        {
            var path = @"d:\CodeBlocks\jonas-temp\notepad.trace";
            using (var sr = new StreamReader(path))
            {
                bool stop = false;
                var parser = new DTraceParser();
                parser.Started += (obj, pre) =>
                {
                    //MessageBox.Show(this, pre, "Started!");
                };
                parser.ParseError += (obj, error) =>
                {
                    MessageBox.Show(this, $"Error at line {error.Line}: {error.Error}\nContext:\n{error.Context}", "Parse error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    stop = true;
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
                    if (!parser.ProcessLine(line))
                        break;
                }
                listBoxCalls.DataSource = _traceCalls;
            }
        }
    }
}
