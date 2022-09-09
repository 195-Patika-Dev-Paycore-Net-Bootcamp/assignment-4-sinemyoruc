using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;


//Aglomera.NET, Microsoft.ML, GoogleMaps.Net.Clustering
namespace SinemYoruc_HW4
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

            var query = session.Containers.Where(x => x.VehicleId == id).ToList();

            var ml = new MLContext();

            DatabaseLoader loader = ml.Data.CreateDatabaseLoader<Container>();

            string featuresColumnName = "Container";
            var pipeline = ml.Transforms
                .Concatenate(featuresColumnName, "Latitude", "Longitude")
                .Append(ml.Clustering.Trainers.KMeans(featuresColumnName, numberOfClusters: n));

            return Ok(pipeline);



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
            /*
                        string container = null;
                        string container2 = null;

                        for (int i = 0; i < query.Count; i++)
                        {
                            for (int j = 0; j < query[i].Count; j++)
                            {
                                container += query[i].ElementAtOrDefault(j).Latitude;
                                container2 += query[i].ElementAtOrDefault(j).Longitude;
                            }
                        }*/








            /*IEstimator<ITransformer> dataPrepEstimator =
                         ml.Transforms.Concatenate("Container", "Latitude", "Longitude")
                        .Append(ml.Transforms.NormalizeMinMax("Container"));
                        */







            //var pipeline = ml.Transforms.Concatenate(container, container2)
            //  .Append(ml.Clustering.Trainers.KMeans(numberOfClusters: n));
            /*
                        var data = new DoubleMatrix(8, 3, new RandGenUniform());
                        var cl = new KMeansClustering(data);
                        ClusterSet clusters = cl.Cluster(2);
                        return Ok(clusters);
            */






            /*
                        var query = session.Containers.Where(x => x.VehicleId == id).ToArray();


                        var random = new Random(5555);
                        // Her satırı rastgele bir kümeye ata
                        var sonucKumesi = Enumerable
                                                .Range(0, query.Length)
                                                .Select(index => (AtananKume: random.Next(0, n),
                                                              Degerler: query[index]))
                                                .ToList();

                        var boyutSayisi = query.Length;
                        var limit = 10000;
                        var guncellendiMi = true;
                        while (--limit > 0)
                        {
                            // kümelerin merkez noktalarını hesapla
                            var merkezNoktalar = Enumerable.Range(0, n)
                                                            .AsParallel()
                                                            .Select(kumeNumarasi =>
                                                            (
                                                            kume: kumeNumarasi,
                                                            merkezNokta: Enumerable.Range(0, boyutSayisi)
                                                                                                .Select(eksen => sonucKumesi.Where(s => s.AtananKume == kumeNumarasi)
                                                                                                .Average(s => s.Degerler[eksen]))
                                                                                                .ToArray())
                                                                    ).ToArray();
                            // Sonuç kümesini merkeze en yakın ile güncelle
                            guncellendiMi = false;
                            //for (int i = 0; i < sonucKumesi.Count; i++)
                            Parallel.For(0, sonucKumesi.Count, i =>
                            {
                                var satir = sonucKumesi[i];
                                var eskiAtananKume = satir.AtananKume;

                                var yeniAtananKume = merkezNoktalar.Select(n => (KumeNumarasi: n.kume,
                                                                                Uzaklik: UzaklikHesapla(satir.Degerler, n.merkezNokta)))
                                                     .OrderBy(x => x.Uzaklik)
                                                     .First()
                                                     .KumeNumarasi;

                                if (yeniAtananKume != eskiAtananKume)
                                {
                                    sonucKumesi[i] = (AtananKume: yeniAtananKume, Degerler: satir.Degerler);
                                    guncellendiMi = true;
                                }
                            });

                            if (!guncellendiMi)
                            {
                                break;
                            }
                        } // while

                        return Ok(sonucKumesi.Select(k => k.AtananKume).ToArray());
                    }

                    static double UzaklikHesapla(double[] birinciNokta, double[] ikinciNokta)
                    {
                        var kareliUzaklik = birinciNokta
                                                .Zip(ikinciNokta,
                                                    (n1, n2) => Math.Pow(n1 - n2, 2)).Sum();
                        return Math.Sqrt(kareliUzaklik);
                    }

            */
        }
    }
}
