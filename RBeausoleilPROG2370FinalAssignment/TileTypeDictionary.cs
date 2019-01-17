using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RBeausoleilPROG2370FinalAssignment
{
    public class TileTypeBaseDictionary
    {
        private static readonly Dictionary<TileType, BaseTileType> tileTypes = new Dictionary<TileType, BaseTileType>()
        {
            { TileType.Water1, BaseTileType.Water },
            { TileType.Water2, BaseTileType.Water },
            { TileType.Water3, BaseTileType.Water },
            { TileType.Water4, BaseTileType.Water },

            { TileType.WaterValleyNE1, BaseTileType.WaterEdge },
            { TileType.WaterValleyNE2, BaseTileType.WaterEdge },
            { TileType.WaterValleyNE3, BaseTileType.WaterEdge },
            { TileType.WaterValleySE1, BaseTileType.WaterEdge },
            { TileType.WaterValleySE2, BaseTileType.WaterEdge },
            { TileType.WaterValleySE3, BaseTileType.WaterEdge },
            { TileType.WaterValleyNW1, BaseTileType.WaterEdge },
            { TileType.WaterValleyNW2, BaseTileType.WaterEdge },
            { TileType.WaterValleyNW3, BaseTileType.WaterEdge },
            { TileType.WaterValleySW1, BaseTileType.WaterEdge },
            { TileType.WaterValleySW2, BaseTileType.WaterEdge },
            { TileType.WaterValleySW3, BaseTileType.WaterEdge },
            { TileType.WaterHipNE1, BaseTileType.WaterEdge },
            { TileType.WaterHipNE2, BaseTileType.WaterEdge },
            { TileType.WaterHipSE1, BaseTileType.WaterEdge },
            { TileType.WaterHipSE2, BaseTileType.WaterEdge },
            { TileType.WaterHipSW1, BaseTileType.WaterEdge },
            { TileType.WaterHipSW2, BaseTileType.WaterEdge },
            { TileType.WaterHipNW1, BaseTileType.WaterEdge },
            { TileType.WaterHipNW2, BaseTileType.WaterEdge },
            { TileType.WaterEdgeS1, BaseTileType.WaterEdge },
            { TileType.WaterEdgeS2, BaseTileType.WaterEdge },
            { TileType.WaterEdgeN1, BaseTileType.WaterEdge },
            { TileType.WaterEdgeN2, BaseTileType.WaterEdge },
            { TileType.WaterEdgeW1, BaseTileType.WaterEdge },
            { TileType.WaterEdgeW2, BaseTileType.WaterEdge },
            { TileType.WaterEdgeE1, BaseTileType.WaterEdge },
            { TileType.WaterEdgeE2, BaseTileType.WaterEdge },

            { TileType.DeepWater1, BaseTileType.DeepWater },
            { TileType.DeepWater2, BaseTileType.DeepWater },
            { TileType.DeepWater3, BaseTileType.DeepWater },
            { TileType.DeepWater4, BaseTileType.DeepWater },

            { TileType.DeepWaterHipNW1, BaseTileType.Water },
            { TileType.DeepWaterHipNE1, BaseTileType.Water },
            { TileType.DeepWaterHipSE1, BaseTileType.Water },
            { TileType.DeepWaterHipSW1, BaseTileType.Water },
            { TileType.DeepWaterValleyNW1, BaseTileType.Water },
            { TileType.DeepWaterValleyNE1, BaseTileType.Water },
            { TileType.DeepWaterValleySW1, BaseTileType.Water },
            { TileType.DeepWaterValleySE1, BaseTileType.Water },
            { TileType.DeepWaterEdgeS1, BaseTileType.Water },
            { TileType.DeepWaterEdgeS2, BaseTileType.Water },
            { TileType.DeepWaterEdgeN1, BaseTileType.Water },
            { TileType.DeepWaterEdgeN2, BaseTileType.Water },
            { TileType.DeepWaterEdgeW1, BaseTileType.Water },
            { TileType.DeepWaterEdgeW2, BaseTileType.Water },
            { TileType.DeepWaterEdgeE1, BaseTileType.Water },
            { TileType.DeepWaterEdgeE2, BaseTileType.Water },

            { TileType.Land1, BaseTileType.Land },
            { TileType.Land2, BaseTileType.Land },
            { TileType.Land3, BaseTileType.Land },
            { TileType.Land4, BaseTileType.Land }
        };

        public static BaseTileType? GetBaseTileType(TileType tileType)
        {
            if (tileTypes.ContainsKey(tileType))
            {
                return tileTypes[tileType];
            }

            return null;
        }
    }
}