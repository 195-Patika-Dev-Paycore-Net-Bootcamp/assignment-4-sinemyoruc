using Accord.MachineLearning; //KMeans icin kullanilan kutuphane
using CenterSpace.NMath.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SinemYoruc_HW4.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class ClusteringController : ControllerBase
    {
        private readonly IMapperSession session;
        public ClusteringController(IMapperSession session)
        {
            this.session = session;
        }

        [HttpGet]
        public ActionResult<List<Container>> GetByClustering(int id, int n)
        {
              var query = session.Containers.Where(x => x.VehicleId == id).ToList()
                  .Select((x, i) => new { Index = i, value = x })
                  .GroupBy(x => x.Index % n)
                  .Select(x => x.Select(v => v.value).ToList())
                  .ToList();

            /*
                              var x = query.Last().First().Latitude;
                              var y = query.Last().First().Longitude;

                              double[][] observations =
                              {
                                   new double[] {x, y}
                              };

                              KMeans kMeans = new KMeans(n);

                              var cluster = kMeans.Learn(observations);
                              var result = new int[observations.Length];

                                          for (int i = 0; i < result.Length; i++)
                                          {
                                              result[i] = cluster.Decide(observations[i]);
                                          }

                              return Ok(result);*/

            string container = null;

            for (int i = 0; i < query.Count; i++)
            {
                for (int j = 0; j < query[i].Count; j++)
                {
                    container += query[i].ToArray().ElementAtOrDefault(j).Latitude;
                }
            }

             var ml = new MLContext();

             var pipeline = ml.Transforms.Concatenate("Container", container)
                .Append(ml.Clustering.Trainers.KMeans("Container", numberOfClusters: n));
            /*
                        var data = new DoubleMatrix(8, 3, new RandGenUniform());
                        var cl = new KMeansClustering(data);
                        ClusterSet clusters = cl.Cluster(2);
                        return Ok(clusters);
            */

            return Ok(pipeline);
        }
        }
}
