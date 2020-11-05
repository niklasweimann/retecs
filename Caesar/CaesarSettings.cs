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

namespace Cryptool.Caesar
{
    public class CaesarSettings
    {
        #region Public Caesar specific interface

        public enum CaesarMode { Encrypt = 0, Decrypt = 1 };

        /// <summary>
        /// An enumaration for the different modes of dealing with unknown characters
        /// </summary>
        public enum UnknownSymbolHandlingMode { Ignore = 0, Remove = 1, Replace = 2 };


        #endregion

        #region Private variables and public constructor

        private CaesarMode _selectedAction = CaesarMode.Encrypt;
        private string _upperAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private string _lowerAlphabet = "abcdefghijklmnopqrstuvwxyz";
        private string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private int _shiftValue = 3;
        private string _shiftString;
        private UnknownSymbolHandlingMode _unknownSymbolHandling = UnknownSymbolHandlingMode.Ignore;
        private bool _caseSensitiveSensitive;
        private bool _memorizeCase;

        public CaesarSettings() => SetKeyByValue(_shiftValue);

        #endregion

        #region Private methods

        private string RemoveEqualChars(string value)
        {
            var length = value.Length;

            for (var i = 0; i < length; i++)
            {
                for (var j = i + 1; j < length; j++)
                {
                    if (value[i] != value[j] && !(!CaseSensitive & (char.ToUpper(value[i]) == char.ToUpper(value[j]))))
                    {
                        continue;
                    }

                    value = value.Remove(j,1);
                    j--;
                    length--;
                }
            }

            return value;
        }

        /// <summary>
        /// Set the new shiftValue and the new shiftString to offset % alphabet.Length
        /// </summary>
        public void SetKeyByValue(int offset, bool firePropertyChanges = true)
        {
            // making sure the shift value lies within the alphabet range
            _shiftValue = ((offset % _alphabet.Length) + _alphabet.Length) % _alphabet.Length;
            _shiftString = "A -> " + _alphabet[_shiftValue];

            // Anounnce this to the settings pane
            if (firePropertyChanges)
            {
                OnPropertyChanged("ShiftValue");
                OnPropertyChanged("ShiftString");
            }
        }

        #endregion

        #region Algorithm settings properties (visible in the Settings pane)

        public CaesarMode Action
        {
            get => _selectedAction;
            set
            {
                if (value == _selectedAction)
                {
                    return;
                }

                _selectedAction = value;
                OnPropertyChanged("Action");
            }
        }

        public int ShiftKey
        {
            get => _shiftValue;
            set => SetKeyByValue(value);
        }

        public string ShiftString => _shiftString;

        //[SettingsFormat(0, "Normal", "Normal", "Black", "White", Orientation.Vertical)]
        public string AlphabetSymbols
        {
          get => _alphabet;
          set
          {
            var a = RemoveEqualChars(value);
            if (a.Length == 0) // cannot accept empty alphabets
            {
            }
            else if (!_alphabet.Equals(a))
            {
              _alphabet = a;
              SetKeyByValue(_shiftValue); //re-evaluate if the shiftvalue is still within the range
              OnPropertyChanged("AlphabetSymbols");
            }
          }
        }

        public UnknownSymbolHandlingMode UnknownSymbolHandling
        {
            get => _unknownSymbolHandling;
            set
            {
                if (value == _unknownSymbolHandling)
                {
                    return;
                }

                _unknownSymbolHandling = value;
                OnPropertyChanged("UnknownSymbolHandling");
            }
        }

        public bool CaseSensitive
        {
            get => _caseSensitiveSensitive;
            set
            {
                if (value == _caseSensitiveSensitive)
                {
                    return;
                }

                if (value)
                {
                    _memorizeCase = false;
                    OnPropertyChanged("MemorizeCase");
                }

                _caseSensitiveSensitive = value;
                if (value)
                {
                    if (_alphabet == _upperAlphabet)
                    {
                        _alphabet = _upperAlphabet + _lowerAlphabet;
                        OnPropertyChanged("AlphabetSymbols");
                    }
                }
                else
                {
                    if (_alphabet == _upperAlphabet + _lowerAlphabet)
                    {
                        _alphabet = _upperAlphabet;
                        OnPropertyChanged("AlphabetSymbols");
                        // re-set also the key (shiftvalue/shiftString to be in the range of the new alphabet
                        SetKeyByValue(_shiftValue);
                    }
                }

                // remove equal characters from the current alphabet
                var a = _alphabet;
                _alphabet = RemoveEqualChars(_alphabet);

                if (a != _alphabet)
                {
                    OnPropertyChanged("AlphabetSymbols");
                }

                OnPropertyChanged("CaseSensitive");
            }
        }
        public bool MemorizeCase
        {
            get => _memorizeCase;
            set
            {
                _memorizeCase = !CaseSensitive && value;
                OnPropertyChanged("MemorizeCase");
            }
        }

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public void Initialize()
        {

        }

        private void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        #endregion
    }
}
