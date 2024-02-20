using MathNet.Numerics.LinearAlgebra.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;

namespace TPA.Schemas
{
    public static class PlaneDistanceCalculator
    {

        public static float[] 편차계산(List<Single> 센서값)
        {
            Single[] distances = new Single[센서값.Count];
            for (int lop = 0; lop < 센서값.Count; lop++)
            {
                distances[lop] = 센서값[lop]* 2;
            }

            return distances;
        }

        public static float[] CalculateDistances(Int32 queryPointCnt, float[,] planePoints, float[,] queryPoints)
        {
            var matrixX = DenseMatrix.OfArray(new float[,] {
                { planePoints[0, 0], planePoints[0, 1], 1 },
                { planePoints[1, 0], planePoints[1, 1], 1 },
                { planePoints[2, 0], planePoints[2, 1], 1 },
                { planePoints[3, 0], planePoints[3, 1], 1 }
            });

            var vectorZ = DenseVector.OfArray(new float[] {
                planePoints[0, 2],
                planePoints[1, 2],
                planePoints[2, 2],
                planePoints[3, 2]
            });

            var result = matrixX.Solve(vectorZ);
            Single A = result[0];
            Single B = result[1];
            Single C = -1;
            Single D = result[2];
            Single[] distances = new Single[queryPointCnt];
            for (int i = 0; i < queryPointCnt; i++)
            {
                Single x = queryPoints[i, 0];
                Single y = queryPoints[i, 1];
                Single z = queryPoints[i, 2];
                Single distance = (A * x + B * y + C * z + D) / (Single)Math.Sqrt(A * A + B * B + C * C);
                distances[i] = distance;
            }

            return distances;
        }

        public static Single FindMinMaxDiff(Single[] arr) => Math.Abs(arr.Max() - arr.Min());
        public static Single FindAbsMaxDiff(Single[] arr) => Math.Max(Math.Abs(arr.Max()), Math.Abs(arr.Min())) * 2;
        public static Single FindAbsMaxDiff2(Single[] arr) => Math.Max(Math.Abs(arr.Max()), Math.Abs(arr.Min()));
    }
}
