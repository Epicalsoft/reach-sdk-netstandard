using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Epicalsoft.Reach.Api.Client.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string path = "https://www.youtube.com/asdasd/asdasdas/dasdasreachsos-sql-db-2018-10-12-3-0.bacpac";
            var name = Path.GetFileName(path);
            Console.ReadKey();
        }

        private static void Start()
        {
            var entity1 = new MyEntity { Id = 1, Name = "A" };
            var entity2 = new MyEntity { Id = 2, Name = "B" };

            var entities = new List<MyEntity> { entity1, entity2 };

            var filtered = entities.Where(x => x.Name.StartsWith("Z")).Select(x => x.Id).ToList();
        }
    }

    public class MyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}