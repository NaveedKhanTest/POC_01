using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace POC.Service.Models
{
    public class LinkModel
    {
        public const string SELF = "self";

        public LinkModel()
        {
        }

        public LinkModel(string href, string rel)
        {
            Href = href ?? throw new ArgumentNullException(nameof(href));
            Rel = rel ?? throw new ArgumentNullException(nameof(rel));
        }

        /// <summary>
        /// URL of related resource.
        /// </summary>
        [Description("URL")]
        public string Href { get; set; }


        [Description("Relationship with current entity")]
        public string Rel { get; set; }


        public static LinkModel Self(string href)
        {
            var link = new LinkModel(href, SELF);
            return link;
        }

    }
}
