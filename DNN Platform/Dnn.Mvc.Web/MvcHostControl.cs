#region Copyright
// 
// DotNetNuke® - http://www.dotnetnuke.com
// Copyright (c) 2002-2014
// by DotNetNuke Corporation
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
// documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
// to permit persons to whom the Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or substantial portions 
// of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
// TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.Web;
using System.Web.UI;
using Dnn.Mvc.Web.Framework.Modules;
using DotNetNuke.ComponentModel;
using DotNetNuke.UI.Modules;

namespace Dnn.Mvc.Web
{
    public class MvcHostControl : ModuleControlBase
    {
        private readonly string _mvcSourceClass;
        private ModuleRequestResult _moduleResult;

        public MvcHostControl(string mvcSource)
        {
            _mvcSourceClass = mvcSource.Replace(".mvc", "");
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Wrap the http Context
            HttpContextBase httpContext = new HttpContextWrapper(HttpContext.Current);

            //var moduleExecutionEngine = ComponentFactory.GetComponent<IModuleExecutionEngine>();
            var moduleExecutionEngine = new ModuleExecutionEngine();

            _moduleResult = moduleExecutionEngine.ExecuteModule(httpContext, ModuleContext, String.Empty);
        }

        //This is method that DotNetNuke's ModuleHost control uses to render the module
        public override void RenderControl(HtmlTextWriter writer)
        {
            if (_moduleResult != null)
            {
                //Execute the ActionResult
                _moduleResult.ActionResult.ExecuteResult(_moduleResult.ControllerContext);
            }
        }

    }
}
