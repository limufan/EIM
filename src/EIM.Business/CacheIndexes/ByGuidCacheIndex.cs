using EIM.Business.CacheManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIM.Business.CacheIndexes
{
    public class ByGuidCacheIndex<T>: DicationaryCacheIndex<T, string>
        where T : class, IGuidProvider
    {
        public ByGuidCacheIndex(CacheManager<T> cacheManager):
            base(cacheManager)
        {

        }

        protected override void OnAdded(T cache)
        {
            base.OnAdded(cache);

            if (cache is ICacheGuidChanged<T>)
            {
                (cache as ICacheGuidChanged<T>).GuidChanged += ByGuidCacheManager_GuidChanged;
            }
        }

        private void ByGuidCacheManager_GuidChanged(T sender, CacheGuidChangedArgs args)
        {
            this.Remove(args.SnapshotGuid);
            this.Add(sender);
        }

        protected override string GetKey(T cache)
        {
            return cache.Guid;
        }

        public override object Get(object key)
        {
            if (key == null || key.GetType() != typeof(string))
            {
                return null;
            }

            string guid = key.ToString();

            if (guid.Length == 36)
            {
                return base.Get(key);
            }

            return null;
        }
    }
}
