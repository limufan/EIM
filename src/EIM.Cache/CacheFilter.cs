using EIM.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EIM.Cache
{
    public abstract class CacheFilter<T>
    {
        public CacheFilter()
        {
            this.Descending = true;
            this.MaxSortCount = 1000000;
            this.MaxFilteCount = 1000000;
        }

        public KeywordMatcher Keyword { set; get; }

        public string OrderBy { set; get; }

        public bool Descending { set; get; }

        public int Start { set; get; }

        public int Size { set; get; }

        /// <summary>
        /// 最大排序数据量
        /// </summary>
        public int MaxSortCount { set; get; }

        /// <summary>
        /// 最大读取数据量
        /// </summary>
        public int MaxFilteCount { set; get; }

        public bool EnableParallel { set; get; }

        /// <summary>
        /// 筛选结果总数
        /// </summary>
        public int TotalCount { set; get; }

        public virtual List<T> Filtrate(IEnumerable<T> source)
        {
            if (this.Size > this.MaxFilteCount)
            {
                throw new EIMException("读取数据量不能超过" + this.MaxFilteCount);
            }
            List<T> filteList = null;
            if (this.EnableParallel)
            {
                filteList = source.AsParallel()
                    .WithDegreeOfParallelism(4)
                    .Where(d => this.Check(d))
                    .ToList();
            }
            else
            {
                filteList = source.Where(d => this.Check(d)).ToList();
            }

            this.TotalCount = filteList.Count;

            this.OnChecked(filteList);

            return this.Sort(filteList)
                .Skip(this.Start)
                .Take(this.Size)
                .ToList();
        }

        public virtual List<T> Filtrate(IEnumerable<T> source, out int totalCount)
        {
            List<T> list = this.Filtrate(source);
            totalCount = this.TotalCount;

            return list;
        }

        protected virtual void OnChecked(List<T> filteList)
        {

        }

        protected virtual List<T> Sort(List<T> list)
        {
            if (string.IsNullOrEmpty(this.OrderBy))
            {
                return this.DefaultSort(list);
            }

            string orderBy = TextHelper.ToPascal(this.OrderBy);
            if (list.Count > this.MaxSortCount)
            {
                throw new ObjectFilterSoftException(string.Format("排序数据量不能超过{0}", this.MaxSortCount));
            }

            if (this.Descending)
            {
                if (this.EnableParallel)
                {
                    return list.AsParallel()
                        .OrderByDescending(orderBy)
                        .ToList();
                }

                return list.OrderByDescending(orderBy).ToList();
            }
            else
            {
                if (this.EnableParallel)
                {
                    return list.AsParallel()
                        .OrderBy(orderBy)
                        .ToList();
                }

                return list.OrderBy(orderBy).ToList();
            }
        }

        protected virtual List<T> DefaultSort(List<T> list)
        {
            return list;
        }

        protected abstract bool Check(T obj);

        protected bool IsEmptyList(IList list)
        {
            if(list == null || list.Count == 0)
            {
                return true;
            }

            return false;
        }
    }
}
