using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace DGenerateGUI
{
    class ParseError
    {
        public int Line;
        public string Error;
        public string PrettyError;
        public bool Fatal;
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
        public List<MemberValue> Array => Value as List<MemberValue>;

        [JsonIgnore]
        public string String => Value as string;

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
                        if (v.Type.StartsWith("char [") && v.String.StartsWith("[ "))
                        {
                            // TODO: handle char values ' '
                            var str = v.String;
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
                return $"{PrettyType} => {String}";
        }

        public bool Visit(string name, Func<string, MemberValue, bool> visitor)
        {
            if (!visitor(name, this))
                return false;
            if(Struct != null)
            {
                foreach(var kv in Struct)
                {
                    if (!kv.Value.Visit(kv.Key, visitor))
                        return false;
                }
            }
            else if(Array != null)
            {
                for(var i = 0; i < Array.Count;i++)
                {
                    if (!Array[i].Visit($"[{i}]", visitor))
                        return false;
                }
            }
            return true;
        }
    }

    public class DTraceCall
    {
        public int Id;
        public string Function;
        public MemberValue Data;
        public int LineNumber;
        public string[] RawLines;

        public override string ToString()
        {
            return $"#{Id}\t{Function}";
        }
    }

    class DTraceParser
    {
        public event EventHandler<DTraceCall> Call;
        public event EventHandler<ParseError> ParseError;

        bool parseError(string error, bool fatal = true)
        {
            // Include the last 20 lines in the pretty error
            var lastContext = _context.Skip(Math.Max(0, _context.Count() - 20)).ToArray();
            var sb = new StringBuilder();
            for (var i = 0; i < lastContext.Length; i++)
            {
                var line = _lineNumber - lastContext.Length + i + 1;
                sb.AppendLine($"{line}:\t{lastContext[i]}");
            }

            // Attempt to format a nice error message
            var lastIndent = "";
            {
                var errorLine = lastContext.Last();
                foreach (var ch in lastContext.Last())
                {
                    if (char.IsWhiteSpace(ch))
                        lastIndent += ch;
                    else
                        break;
                }
            }
            sb.AppendLine($"{_lineNumber}:\t{lastIndent}^");
            sb.AppendLine($"{_lineNumber}:\t{lastIndent}{error}");

            // Raise the error event
            ParseError?.Invoke(this, new ParseError
            {
                Line = _lineNumber,
                Error = error,
                PrettyError = sb.ToString().TrimEnd(),
                Fatal = fatal,
            });

            if (!fatal)
                _context.Clear();

            return false;
        }

        private Regex _reNeutralCall = new Regex(@"^struct (.+)CALL {$");
        private Regex _reCloseCall = new Regex(@"^}struct (.+)CALL {$");
        private Regex _reMember = new Regex(@"^ *(.+) (\**)([a-zA-Z0-9_]+)( :\d+)? = (.+)$");
        private Regex _reArray = new Regex(@"^ *struct (.+) {$");
        private List<string> _context = new List<string>();
        private int _lineNumber = 0;
        private ParseState _state;
        private int _callId = 0;

        class ParseState
        {
            public ParseState(string name, int line)
            {
                Name = name; ;
                Line = line;
                Root = new MemberValue(name, "", new Dictionary<string, MemberValue>(), null);
                _stack.Push(Root);
            }

            [JsonIgnore]
            private Stack<MemberValue> _stack = new Stack<MemberValue>();

            public string Name;
            public int Line;
            public MemberValue Root;

            [JsonIgnore]
            public int Count => _stack.Count;

            public int Pop()
            {
                _stack.Pop();
                return _stack.Count;
            }

            public MemberValue Peek()
            {
                return _stack.Count > 0 ? _stack.Peek() : null;
            }

            public void AddMemberValue(string type, string ptr, string name, string value, int? bitsize)
            {
                Peek().Struct.Add(name, new MemberValue(type, ptr, value, bitsize));
            }

            public void PushStruct(string type, string ptr, string name)
            {
                var value = new MemberValue(type, ptr, new Dictionary<string, MemberValue>(), null);
                var top = Peek();
                if (top.Struct != null)
                {
                    top.Struct.Add(name, value);
                }
                else if (top.Array != null)
                {
                    top.Array.Add(value);
                }
                else
                {
                    throw new InvalidOperationException("Cannot push a member");
                }

                _stack.Push(value);
            }

            public void PushArray(string type, string ptr, string name)
            {
                var value = new MemberValue(type, ptr, new List<MemberValue>(), null);
                var top = Peek();
                if (top.Struct != null)
                {
                    top.Struct.Add(name, value);
                }
                else if (top.Array != null)
                {
                    top.Array.Add(value);
                }
                else
                {
                    throw new InvalidOperationException("Cannot push array");
                }

                _stack.Push(value);
            }
        }

        bool flush(ParseState fstruct)
        {
            fstruct.Root.FlattenStrings();

            Call?.Invoke(this, new DTraceCall
            {
                Id = _callId++,
                RawLines = _context.ToArray(),
                Function = fstruct.Name,
                Data = fstruct.Root,
                LineNumber = fstruct.Line,
            });

            // Clear the context
            _context.Clear();

            return true;
        }

        public bool ProcessLine(string line)
        {
            _lineNumber++;
            _context.Add(line);

            if (_state == null)
            {
                var m = _reNeutralCall.Match(line);
                if (!m.Success)
                {
                    // We can gracefully keep retrying here
                    parseError("Failed to match struct header", false);
                }
                else
                {
                    _state = new ParseState(m.Groups[1].Value, _lineNumber);
                }
            }
            else
            {
                if (_state.Peek().Array != null)
                {
                    var m = _reArray.Match(line);
                    if (m.Success)
                    {
                        var type = m.Groups[1].Value.Trim();
                        _state.PushStruct(type, "", $"[{_state.Peek().Array.Count}]");
                    }
                    else if (line.Trim() == "]")
                    {
                        if (_state.Count == 0)
                            return parseError("Invalid nesting level");

                        if (_state.Pop() == 0)
                            return parseError("Invalid nesting level (array)");
                    }
                    else
                    {
                        return parseError("Failed to match array member line");
                    }
                }
                else
                {
                    var m = _reMember.Match(line);
                    if (m.Success)
                    {
                        var type = m.Groups[1].Value.Trim();
                        var ptr = m.Groups[2].Value.Trim();
                        var name = m.Groups[3].Value.Trim();
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
                            _state.PushStruct(type, ptr, name);
                        }
                        else if (value == "[")
                        {
                            _state.PushArray(type, ptr, name);
                        }
                        else
                        {
                            _state.AddMemberValue(type, ptr, name, value, bitsize);
                        }
                    }
                    else
                    {
                        var trim = line.Trim();
                        if (!trim.StartsWith("}"))
                            return parseError("Failed to match member line");

                        if (_state.Count == 0)
                            return parseError("Invalid nesting level (struct)");

                        if (_state.Pop() == 0)
                        {
                            var fstruct = _state;

                            if (trim == "}")
                            {
                                _state = null;
                            }
                            else
                            {
                                // End of the root object, create a new one
                                m = _reCloseCall.Match(line);
                                if (!m.Success)
                                {
                                    // We found a } so we can gracefully continue
                                    parseError("Failed to match closing call", false);
                                    _state = null;
                                }
                                else
                                {
                                    _state = new ParseState(m.Groups[1].Value, _lineNumber);
                                }
                            }

                            if (!flush(fstruct))
                                return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
