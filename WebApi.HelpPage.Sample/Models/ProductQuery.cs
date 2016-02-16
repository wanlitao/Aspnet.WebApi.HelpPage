using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.HelpPage.Sample.Models
{
    /// <summary>
    /// 产品查询
    /// </summary>
    public class ProductQuery
    {
        public int? Id { get; set; }
        public string Name { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }
    }
}
