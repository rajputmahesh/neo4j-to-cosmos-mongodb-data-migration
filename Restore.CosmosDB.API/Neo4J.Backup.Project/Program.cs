using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Neo4j.Driver;

using Newtonsoft.Json;

namespace Neo4jExport
{
    class Program
    {
        static async Task Main(string[] args)
        {


            var uri = "neo4j+s://********.databases.neo4j.io";
            var username = "your user name ";
            var password = "your password";
            var batchSize = 100000;  // Adjust the batch size as needed

            //Change this path where you want to kept json file 
            var outoutPathTemplate = @"D:\Projects\POC\Neo4J.Backup.Project\Neo4J.Backup.Project\OutPut\output[~~].json";

            var driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
            var session = driver.AsyncSession();

            try
            {
                var queryTemplate = "MATCH (n) RETURN n SKIP {skip} LIMIT {limit}";
                var totalNodes = await GetTotalNodesCount(session);

                for (int skip = 0; skip < totalNodes; skip += batchSize)
                {
                    string outputPath = outoutPathTemplate.Replace("~~", skip.ToString());
                    using (var fileStream = new StreamWriter(outputPath))
                    {
                        var query = queryTemplate.Replace("{skip}", skip.ToString()).Replace("{limit}", batchSize.ToString());
                        var nodes = await FetchNodes(session, query);

                        //foreach (var node in nodes)
                        //{
                        //    var json = JsonConvert.SerializeObject(node, Formatting.None);
                        //    await fileStream.WriteAsync(json);

                        //    if (skip + batchSize < totalNodes || nodes.IndexOf(node) < nodes.Count - 1)
                        //    {
                        //        await fileStream.WriteAsync(",");
                        //    }
                        //}
                        var json = JsonConvert.SerializeObject(nodes, Formatting.None);
                        await fileStream.WriteAsync(json);
                    }
                }
                Console.WriteLine("Data exported to output.json");
            }
            finally
            {
                await session.CloseAsync();
                await driver.CloseAsync();
            }
        }

        private static async Task<int> GetTotalNodesCount(IAsyncSession session)
        {
            var result = await session.RunAsync("MATCH (n) RETURN count(n) AS count");
            var record = await result.SingleAsync();
            return record["count"].As<int>();
        }

        private static async Task<List<Dictionary<string, object>>> FetchNodes(IAsyncSession session, string query)
        {
            var result = await session.RunAsync(query);
            var records = await result.ToListAsync();
            var nodes = new List<Dictionary<string, object>>();

            foreach (var record in records)
            {
                var node = record["n"].As<INode>();
                nodes.Add((Dictionary<string, object>)node.Properties);
            }

            return nodes;
        }
    }
}
