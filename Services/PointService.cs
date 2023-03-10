using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Write_Wash.Services
{
    internal class PointService
    {
        private readonly DataContext _context;
        public PointService(DataContext context)
        {
            _context = context;
        }
        public async Task<List<Points>> GetPoints()
        {
            List<Points> points = new();

            await Task.Run(async () =>
            {
                try
                {
                    List<PointContext> point = await _context.Point.ToListAsync();
                    foreach (var item in point)
                    {
                        points.Add(new Points
                        {
                            PointId = item.idPoint,
                            PointIndex = item.PointIndex,
                            PointCity = item.PointCity,
                            PointStreet = item.PointStreet,
                            PointHome = item.PointHome
                        });
                    }
                }
                catch { }
            });
            return points;
        }
    }
}
