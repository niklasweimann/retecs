﻿/*
   Copyright 2008 Martin Saternus, University of Duisburg-Essen

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

using System.Windows.Navigation;
using System.Windows.Documents;
using System.Diagnostics;

namespace Cryptool.PluginBase.Miscellaneous
{
  public class DescriptionHyperlink : Hyperlink
  {
    public DescriptionHyperlink()
    {
      this.ToolTip = "Click to Open";
      this.RequestNavigate += new RequestNavigateEventHandler(MyHyperlink_RequestNavigate);
    }

    void MyHyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
      try
      {
        Process.Start(e.Uri.AbsoluteUri);
      }
      catch
      {

      }
    }
  }
}
