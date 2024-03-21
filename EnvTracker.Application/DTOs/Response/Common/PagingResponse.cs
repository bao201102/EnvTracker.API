using Newtonsoft.Json;

namespace EnvTracker.Application.DTOs.Response.Common
{
    public class PagingResponse<T>
    {
        [JsonIgnore]
        public CRUDStatusCodeRes StatusCode { get; set; }

        [JsonIgnore]
        public string ErrorMessage { get; set; }

        public int TotalRecord { get; set; }

        public int? CurrentPageIndex { get; set; }

        public int? PageSize { get; set; }

        public ICollection<T> Records { get; set; }
    }
}
