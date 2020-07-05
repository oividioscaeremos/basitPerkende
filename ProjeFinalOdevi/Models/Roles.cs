﻿using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using System.Web;

namespace ProjeFinalOdevi.Models
{
    public class Roles
    {
        public virtual int id { get; set; }
        public virtual string name { get; set; }
    }

    public class RolesMap : ClassMapping<Roles>
    {

        public RolesMap()
        {
            Table("roles");
            Schema("projefinalodevi");
            Lazy(true);
            Id(x => x.id, map => map.Generator(Generators.Identity));
            Property(x => x.name, map => map.NotNullable(true));
        }
    }
}