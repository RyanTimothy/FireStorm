/*  TileType.cs
 *  The specific tiles in the game ordered by Texture grid sequence
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment
{
    public enum TileType
    {
        Water1 = 5,
        Water2 = 6,
        Water3 = 7,
        Water4 = 8,

        DeepWater1 = 37,
        DeepWater2 = 38,
        DeepWater3 = 45,
        DeepWater4 = 46,

        DeepWaterHipNW1 = 49,
        DeepWaterHipNE1 = 50,
        DeepWaterHipSE1 = 57,
        DeepWaterHipSW1 = 58,

        DeepWaterValleyNW1 = 55,
        DeepWaterValleyNE1 = 56,
        DeepWaterValleySW1 = 63,
        DeepWaterValleySE1 = 64,

        DeepWaterEdgeS1 = 61,
        DeepWaterEdgeS2 = 62,
        DeepWaterEdgeN1 = 53,
        DeepWaterEdgeN2 = 54,
        DeepWaterEdgeW1 = 52,
        DeepWaterEdgeW2 = 60,
        DeepWaterEdgeE1 = 51,
        DeepWaterEdgeE2 = 59,

        Land1 = 1,
        Land2 = 2,
        Land3 = 3,
        Land4 = 4,

        WaterValleyNE1 = 25,
        WaterValleyNE2 = 33,
        WaterValleyNE3 = 41,
        WaterValleySE1 = 26,
        WaterValleySE2 = 34,
        WaterValleySE3 = 42,
        WaterValleyNW1 = 27,
        WaterValleyNW2 = 35,
        WaterValleyNW3 = 43,
        WaterValleySW1 = 28,
        WaterValleySW2 = 36,
        WaterValleySW3 = 44,

        WaterHipNE1 = 23,
        WaterHipNE2 = 24,
        WaterHipSE1 = 21,
        WaterHipSE2 = 22,
        WaterHipSW1 = 17,
        WaterHipSW2 = 18,
        WaterHipNW1 = 19,
        WaterHipNW2 = 20,

        WaterEdgeS1 = 9,
        WaterEdgeS2 = 10,
        WaterEdgeN1 = 13,
        WaterEdgeN2 = 14,
        WaterEdgeW1 = 11,
        WaterEdgeW2 = 12,
        WaterEdgeE1 = 15,
        WaterEdgeE2 = 16
    }
}
