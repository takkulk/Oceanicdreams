using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Yachtberles.Models;

namespace Yachtberles.Data
{
    public class YachtberlesContext : DbContext
    {
        public YachtberlesContext (DbContextOptions<YachtberlesContext> options)
            : base(options)
        {
        }

        public DbSet<Yachtberles.Models.Berles> Berles { get; set; } = default!;
    }
}
