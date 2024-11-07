using System.Runtime.Serialization;

namespace Inventory.Shared.Dtos.CommonDto
{
    [DataContract]
    public class PagedAndSortedRequest
    {
        [DataMember(Order = 1)] public virtual string Sorting { get; set; }
        [DataMember(Order = 2)] public virtual string Filter { get; set; }
        [DataMember(Order = 3)] public virtual int SkipCount { get; set; }
        [DataMember(Order = 4)] public virtual int MaxResultCount { get; set; } = 10;
    }

    public interface IListResult<T>
    {
        [DataMember(Order = 1)] IReadOnlyList<T> Items { get; set; }
    }

    [Serializable]
    [DataContract]
    public class ListResultDto<T> : IListResult<T>
    {
        [DataMember(Order = 1)] private IReadOnlyList<T> _items;

        [DataMember(Order = 2)]
        public IReadOnlyList<T> Items
        {
            get => this._items ??= (IReadOnlyList<T>)new List<T>();
            set => this._items = value;
        }

        public ListResultDto()
        {
        }

        public ListResultDto(IReadOnlyList<T> items) => this.Items = items;
    }

    [Serializable]
    [DataContract]
    public class PagedResultDto<T> : ListResultDto<T>, IPagedResult<T>, IListResult<T>, IHasTotalCount, IHasSumData<T>
    {
        [DataMember(Order = 1)] public long TotalCount { get; set; }

        public PagedResultDto()
        {
        }

        public PagedResultDto(long totalCount, IReadOnlyList<T> items)
            : base(items)
            => this.TotalCount = totalCount;

        [DataMember(Order = 2)] public T SumData { get; set; }
        [DataMember(Order = 3)] public ResStatus ResponseStatus { get; set; }
    }

    public interface IPagedResult<T> : IListResult<T>, IHasTotalCount, IHasSumData<T>
    {
    }

    public interface IHasTotalCount
    {
        long TotalCount { get; set; }
    }

    public interface IHasSumData<T>
    {
        T SumData { get; set; }
    }
}