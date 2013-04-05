using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ParserBuilder
{
    public class BaseCodeEmitter
    {
        private StringBuilder _generationEnvironmentField = new StringBuilder();
        private readonly CompilerErrorCollection _errorsField = new CompilerErrorCollection();
        private readonly List<int> _indentLengthsField = new List<int>();
        private string _currentIndentField = "";
        private bool _endsWithNewline;

        public virtual string TransformText()
        {
            return GenerationEnvironment.ToString();
        }

        public IDisposable GetBuffer()
        {
            return new OutputBuffer(GenerationEnvironment);
        }

        public class OutputBuffer : IDisposable
        {
            private readonly StringBuilder _generationEnvironment;
            private readonly int _startLength;

            public OutputBuffer(StringBuilder generationEnvironment)
            {
                _generationEnvironment = generationEnvironment;
                _startLength = generationEnvironment.Length;
            }

            public override string ToString()
            {
                return _generationEnvironment.ToString(_startLength, _generationEnvironment.Length);
            }

            public void Dispose()
            {
                _generationEnvironment.Length = _startLength;
            }
        }

        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        protected StringBuilder GenerationEnvironment
        {
            get { return _generationEnvironmentField; }
            set { _generationEnvironmentField = value; }
        }

        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public CompilerErrorCollection Errors
        {
            get { return _errorsField; }
        }

        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private List<int> IndentLengths
        {
            get
            {
                return _indentLengthsField;
            }
        }

        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get { return _currentIndentField; }
        }

        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual IDictionary<string, object> Session { get; set; }

        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (GenerationEnvironment.Length == 0 || _endsWithNewline)
            {
                GenerationEnvironment.Append(_currentIndentField);
                _endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(Environment.NewLine, StringComparison.CurrentCulture))
            {
                _endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if (_currentIndentField.Length == 0)
            {
                GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(Environment.NewLine,(Environment.NewLine + _currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (_endsWithNewline)
            {
                GenerationEnvironment.Append(textToAppend, 0,(textToAppend.Length - _currentIndentField.Length));
            }
            else
            {
                GenerationEnvironment.Append(textToAppend);
            }
        }

        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            Write(textToAppend);
            GenerationEnvironment.AppendLine();
            _endsWithNewline = true;
        }


        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            Errors.Add(new CompilerError {ErrorText = message});
        }

        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            Errors.Add(new CompilerError {ErrorText = message, IsWarning = true});
        }

        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if (indent == null)
            {
                throw new ArgumentNullException("indent");
            }
            _currentIndentField = _currentIndentField + indent;
            IndentLengths.Add(indent.Length);
        }

        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            if (IndentLengths.Count <= 0) { return ""; }
            var index = IndentLengths.Count - 1;
            var indentLength = IndentLengths[index];
            IndentLengths.RemoveAt(index);

            if (indentLength <= 0) { return ""; }
            var currentIndent = _currentIndentField;
            var startIndex = currentIndent.Length - indentLength;
            _currentIndentField =currentIndent.Remove(startIndex);
            return currentIndent.Substring(startIndex);
        }

        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            IndentLengths.Clear();
            _currentIndentField = "";
        }

        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private IFormatProvider _formatProviderField = CultureInfo.InvariantCulture;

            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public IFormatProvider FormatProvider
            {
                get { return _formatProviderField; }
                set
                {
                    if (value != null)
                    {
                        _formatProviderField = value;
                        _formatProviderFieldForParam = null;
                    }
                }
            }

            readonly IDictionary<Type, MethodInfo> _toStringMethod = new Dictionary<Type, MethodInfo>();
            private object[] _formatProviderFieldForParam;

            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if (_formatProviderFieldForParam == null)
                {
                    _formatProviderFieldForParam = new object[] {_formatProviderField};
                } 

                if ((objectToConvert == null))
                {
                    throw new ArgumentNullException("objectToConvert");
                }
                var t = objectToConvert.GetType();
                MethodInfo method;
                if (!_toStringMethod.TryGetValue(t, out method))
                {
                    method = t.GetMethod("ToString", new[] { typeof(IFormatProvider) });
                    _toStringMethod.Add(t,method);
                }
                if (method == null)
                {
                    return objectToConvert.ToString();
                }
                return ((string) (method.Invoke(objectToConvert, _formatProviderFieldForParam)));
            }
        }

        private readonly ToStringInstanceHelper _toStringHelperField = new ToStringInstanceHelper();

        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get { return _toStringHelperField; }
        }
    }

}