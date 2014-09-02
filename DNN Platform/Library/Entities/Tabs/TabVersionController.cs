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

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Framework;

namespace DotNetNuke.Entities.Tabs
{
    public class TabVersionController: ServiceLocator<ITabVersionController, TabVersionController>, ITabVersionController
    {
        private static readonly DataProvider Provider = DataProvider.Instance();

        public TabVersion GetTabVersion(int tabVersionId, int tabId, bool ignoreCache = false)
        {
            return GetTabVersions(tabId, ignoreCache).SingleOrDefault(tv => tv.TabVersionId == tabVersionId);
        }
        
        public IEnumerable<TabVersion> GetTabVersions(int tabId, bool ignoreCache = false)
        {
            //if we are not using the cache
            if (ignoreCache || Host.Host.PerformanceSetting == Globals.PerformanceSettings.NoCaching)
            {
                return CBO.FillCollection<TabVersion>(Provider.GetTabVersions(tabId));
            }
            
            string cacheKey = string.Format(DataCache.TabVersionsCacheKey, tabId);
            return CBO.GetCachedObject<List<TabVersion>>(new CacheItemArgs(cacheKey,
                                                                    DataCache.TabVersionsCacheTimeOut,
                                                                    DataCache.TabVersionsCachePriority),
                                                            c =>
                                                            {
                                                                return CBO.FillCollection<TabVersion>(Provider.GetTabVersions(tabId));                                                                    
                                                            });            
        }

        public void SaveTabVersion(TabVersion tabVersion)
        {
            var tabVersionId = Provider.SaveTabVersion(tabVersion.TabVersionId, tabVersion.TabId, tabVersion.TimeStamp,
                tabVersion.Version, tabVersion.CreatedByUserID, tabVersion.LastModifiedByUserID);

            tabVersion.TabVersionId = tabVersionId;
        }

        public void DeleteTabVersion(int tabVersionId)
        {
            Provider.DeleteTabVersion(tabVersionId);
        }

        protected override System.Func<ITabVersionController> GetFactory()
        {
            return () => new TabVersionController();
        }
    }
}
