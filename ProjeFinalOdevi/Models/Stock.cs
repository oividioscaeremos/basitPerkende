using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using System.Web;

namespace ProjeFinalOdevi.Models
{
    public class Stock
    {
        public virtual int StockId { get; set; }
        public virtual string Description { get; set; }
        public virtual int Unitprice { get; set; }
        public virtual User Belongstouser { get; set; }
        public virtual int Quantityinstock { get; set; }
        

    }

    public class StockMap : ClassMapping<Stock>
    {
        public StockMap()
        {
            Table("stock");
            Schema("projefinalodevi");
            Id(x => x.StockId, map => { map.Column("stock_id"); map.Generator(Generators.Identity); });
            Property(x => x.Description, map => map.NotNullable(true));
            Property(x => x.Unitprice, map => map.NotNullable(true));
            Property(x => x.Quantityinstock, map => { map.Column("quantityInStock"); map.NotNullable(true); });

            ManyToOne(x => x.Belongstouser, map => { map.Column("belongsToUser"); map.Cascade(Cascade.None); });

        }
    }
}