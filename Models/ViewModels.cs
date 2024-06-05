using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace x9web.Models
{
    public class ViewModels
    {

    }
    public class um_man_cplook
    {
        public List<xball> xball { get; set; }
        public List<xxiazu> xiazu { get; set; }
        public List<xqisu> xqisu { get; set; }
        public int qisu { get; set; }
        public string xm_name { get; set; }
    }
    public class um_man_touji
    {
        public List<xusers> xusers { get; set; }
        public List<xxiazu> xxiazu { get; set; }
    }
}