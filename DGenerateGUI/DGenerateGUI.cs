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

        class Pointer
        {
            public string Type;
            public UIntPtr Value;
        }

        enum ProcessorMode
        {
            Kernel,
            User,
        }

        enum TokenType
        {

        }

        enum IntegrityLevel
        {

        }

        class Syscall
        {
            public string Function;
            // TODO: generic way to handle arguments
            public uint Result; // NTSTATUS
        }

        class Call
        {
            public string Function; // Duplicated Syscall.Function
            public bool SyscallEntered;
            public Pointer EProcess;
            public Pointer EThread;
            public Pointer Caller;
            public string ImageFile;
            public ProcessorMode PreviousMode;
            public uint Pid;
            public uint Tid;
            public bool Impersonating;
            public Pointer Token;
            public TokenType TokenType;
            public IntegrityLevel Integrity;
            public Syscall Syscall;
        }

        class ParseError
        {
            public string Line;
            public string Error;
            public string Context;
        }

        class DTraceParser
        {
            public event EventHandler<string> Started;
            public event EventHandler<Call> Call;
            public event EventHandler<ParseError> ParseError;

            public DTraceParser()
            {
            }

            public void ProcessLine(string line)
            {
                if (_started)
                    processStarted(line);
                else
                    processPre(line);
            }

            void processPre(string line)
            {
                _pre += line + "\n";
                if (line == "Started up")
                {
                    _started = true;
                    Started?.Invoke(this, _pre);
                }
            }

            void parseError(string error)
            {
                ParseError?.Invoke(this, new ParseError
                {
                    Error = error,
                    Context = _context.ToString(),
                });
            }

            enum LineState
            {
                Neutral,
                InCall,
                InSyscall,
            }

            private bool _started = false;
            private string _pre = "";
            private LineState _lineState = LineState.Neutral;
            private Regex _reNeutralCall = new Regex(@"^struct (.+)CALL {$");
            private Regex _reInCall = new Regex(@"^}struct (.+)CALL {$");
            private Call _currentEvent;
            private int _nesting = 0;
            private StringBuilder _context = new StringBuilder();

            void processStarted(string line)
            {
                _context.AppendLine(line);
                // TODO: find a way to gracefully handle errors
                switch (_lineState)
                {
                    case LineState.Neutral:
                        var m = _reNeutralCall.Match(line);
                        if (m.Success)
                        {
                            _lineState = LineState.InCall;
                            _currentEvent = new Call
                            {
                                Function = m.Groups[1].Value,
                            };
                        }
                        else
                        {
                            // TODO: fall back to generic brace nesting
                            parseError("Failed to match regex");
                        }
                        break;
                    case LineState.InCall:
                        if(line.EndsWith("{"))
                        {
                            _nesting++;
                        }
                        else if(line.Trim() == "}")
                        {
                            if (_nesting == 0)
                                parseError("Invalid brace nesting");
                            _nesting--;
                        }
                        break;
                }
            }
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            var path = @"d:\CodeBlocks\jonas-temp\notepad.trace";
            using (var sr = new StreamReader(path))
            {
                bool stop = false;
                var parser = new DTraceParser();
                parser.Started += (obj, pre) =>
                {
                    MessageBox.Show(this, pre, "Started!");
                };
                parser.ParseError += (obj, error) =>
                {
                    MessageBox.Show(this, $"Line: {error.Line}\nError: {error.Error}", "Parse error");
                    stop = true;
                };
                parser.Call += (obj, call) =>
                {
                    MessageBox.Show(this, $"Function: {call.Function}", "Event");
                    stop = true;
                };
                while (!stop)
                {
                    var line = sr.ReadLine();
                    if (line == null)
                        break;
                    parser.ProcessLine(line);
                }
            }
        }
    }
}
