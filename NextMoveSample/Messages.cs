using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NextMove.Lib
{

    public class Rootobject
    {

        [JsonProperty("content")]
        public List<StandardBusinessDocument> content { get; set; }
        public Pageable pageable { get; set; }
        public int totalPages { get; set; }
        public bool last { get; set; }
        public int totalElements { get; set; }
        public int number { get; set; }
        public int size { get; set; }
        public Sort1 sort { get; set; }
        public int numberOfElements { get; set; }
        public bool first { get; set; }
        public bool empty { get; set; }
    }

    public class Pageable
    {
        public Sort sort { get; set; }
        public int offset { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public bool unpaged { get; set; }
        public bool paged { get; set; }
    }

    public class Sort
    {
        public bool unsorted { get; set; }
        public bool sorted { get; set; }
        public bool empty { get; set; }
    }

    public class Sort1
    {
        public bool unsorted { get; set; }
        public bool sorted { get; set; }
        public bool empty { get; set; }
    }
}
