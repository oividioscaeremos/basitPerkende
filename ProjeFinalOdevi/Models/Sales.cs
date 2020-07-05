using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using System.Web;

namespace ProjeFinalOdevi.Models
{
    public class Sales
    {
        public virtual int sales_id { get; set; }
        public virtual User belongsToUser { get; set; }
        public virtual Stock stock_id { get; set; }
        public virtual DateTime date { get; set; }
        public virtual int quantity { get; set; }
        public virtual int sale_sum { get; set; }
    }

    public class SalesMap : ClassMapping<Sales>
    {

        public SalesMap()
        {
            Table("sales");
            Schema("projefinalodevi");
            Id(x => x.sales_id, map => map.Generator(Generators.Identity));
            Property(x => x.date, map => map.NotNullable(true));
            Property(x => x.quantity, map => map.NotNullable(true));
            Property(x => x.sale_sum, map => map.NotNullable(true));

            ManyToOne(x => x.stock_id, map => { map.Column("stock_id"); map.Cascade(Cascade.None); });
            ManyToOne(x => x.belongsToUser, map => { map.Column("belongsToUser"); map.Cascade(Cascade.None); });


        }
    }
}