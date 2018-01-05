using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMVVMSudoku
{
    public class UserContext:DbContext
    {
        public UserContext() : base("SudokuConnect")
        {
        }
        public DbSet<Point> Points { get; set; }
        public DbSet<Save> Saves { get; set; }
    }
}
