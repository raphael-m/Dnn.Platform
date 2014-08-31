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

using System.Web.Mvc;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.Modules;

namespace Dnn.Mvc.Web.Framework.Modules
{
    public abstract class DnnControllerBase : Controller
    {
        public ModuleInfo ActiveModule 
        {
            get { return (ModuleContext == null) ? null : ModuleContext.Configuration; }
        }

        public TabInfo ActivePage
        {
            get { return (ModuleContext == null) ? null : ModuleContext.PortalSettings.ActiveTab; }
        }

        public PortalInfo ActiveSite
        {
            get
            {
                return (ModuleContext == null) ? null : PortalController.Instance.GetPortal(ModuleContext.PortalSettings.PortalId);
            }
        }

        public PortalAliasInfo ActiveSiteAlias
        {
            get { return (ModuleContext == null) ? null : ModuleContext.PortalSettings.PortalAlias; }
        }

        public ModuleInstanceContext ModuleContext { get; set; }

        public new UserInfo User
        {
            get { return (ModuleContext == null) ? null : ModuleContext.PortalSettings.UserInfo; }
        }

        protected override ViewResult View(IView view, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }

            return new DnnViewResult
            {
                View = view,
                ViewData = ViewData,
                TempData = TempData
            };
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }

            return new DnnViewResult
            {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = ViewData,
                TempData = TempData,
                ViewEngineCollection = ViewEngineCollection
            };
        }
    }
}
