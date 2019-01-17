/*  LayerType.cs
 *  The various Layer types of the GameObjects - used for collision
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
    public enum LayerType
    {
        None,
        Player,
        PlayerBullet,
        EnemyBullet,
        EnemyStructure,
        FallingDestruction,
        Tree,
        Wall
    }
}
