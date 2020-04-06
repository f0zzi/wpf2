using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.DataModel
{
    public class Role
    {
        public Role()
        {
            Users = new List<User>();
        }
        public int Id { get; set; }
        public string Type { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
