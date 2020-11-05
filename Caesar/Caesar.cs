/*
   Copyright 2009-2012 Arno Wacker, University of Kassel

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace Cryptool.Caesar
{
    public class Caesar
    {
        #region Private elements

        private readonly CaesarSettings _settings;
        private bool _isRunning;

        #endregion

        #region Public interface

        /// <summary>
        /// Constructor
        /// </summary>
        public Caesar() => _settings = new CaesarSettings();

        /// <summary>
        /// Get or set all settings for this algorithm.
        /// </summary>
        public CaesarSettings Settings => _settings;

        public string InputString { get; set; }

        public string OutputString { get; set; }

        public string AlphabetSymbols
        {
            get => _settings.AlphabetSymbols;
            set
            {
                if (value != null && value != _settings.AlphabetSymbols)
                {
                    _settings.AlphabetSymbols = value;
                    OnPropertyChanged("AlphabetSymbols");
                }
            }
        }

        public int ShiftKey
        {
          get => _settings.ShiftKey;
          set
          {
            if (_isRunning)
            {
                _settings.SetKeyByValue(value);
            }
          }
        }

        #endregion

        #region IPlugin members
        public void Initialize()
        {
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// No algorithm visualization
        /// </summary>
        public UserControl Presentation => null;

        public void Stop() => _isRunning = false;

        public void PostExecution() => _isRunning = false;

        public void PreExecution() => _isRunning = true;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion


        #region IPlugin Members


		public void Execute()
        {
		    var output = new StringBuilder();

		    // If we are working in case-insensitive mode, we will use only
		    // capital letters, hence we must transform the whole alphabet
		    // to uppercase.
            var alphabet = _settings.CaseSensitive ? _settings.AlphabetSymbols : _settings.AlphabetSymbols.ToUpper();

            if (string.IsNullOrEmpty(InputString))
            {
                return;
            }

            foreach (var t in InputString)
            {
                // Get the plaintext char currently being processed.
                var currentchar = t;

                // Store whether it is upper case.
                var uppercase = char.IsUpper(currentchar);

                // Get the position of the plaintext character in the alphabet.
                var ppos = alphabet.IndexOf(_settings.CaseSensitive ? currentchar : char.ToUpper(currentchar));

                if (ppos >= 0)
                {
                    // We found the plaintext character in the alphabet,
                    // hence we will commence shifting.
                    var cpos = 0;
                    switch (_settings.Action)
                    {
                        case CaesarSettings.CaesarMode.Encrypt:
                            cpos = (ppos + _settings.ShiftKey) % alphabet.Length;
                            break;
                        case CaesarSettings.CaesarMode.Decrypt:
                            cpos = (ppos - _settings.ShiftKey + alphabet.Length) % alphabet.Length;
                            break;
                    }

                    // We have the position of the ciphertext character,
                    // hence just output it in the correct case.
                    if (_settings.CaseSensitive)
                    {
                        output.Append(alphabet[cpos]);
                    }
                    else
                    {
                        output.Append(uppercase ? char.ToUpper(alphabet[cpos]) : char.ToLower(alphabet[cpos]));
                    }

                }
                else
                {
                    // The plaintext character was not found in the alphabet,
                    // hence proceed with handling unknown characters.
                    switch (_settings.UnknownSymbolHandling)
                    {
                        case CaesarSettings.UnknownSymbolHandlingMode.Ignore:
                            output.Append(t);
                            break;
                        case CaesarSettings.UnknownSymbolHandlingMode.Replace:
                            output.Append('?');
                            break;
                    }
                }
            }
            OutputString = _settings.CaseSensitive | _settings.MemorizeCase ? output.ToString() : output.ToString().ToUpper();
            OnPropertyChanged("OutputString");
        }

        #endregion

    }
}
