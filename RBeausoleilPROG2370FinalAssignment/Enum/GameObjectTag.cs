/*  GameObjectTag.cs
 *  The tags for the various GameObjects
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
    public enum GameObjectTag
    {
        Camera,
        CameraTarget,
        Player,
        Base,
        TurretTower,
        TurretTowerDamaged,
        TowerMuzzleFlash,
        Bullet,
        Tree,
        CameraController,
        TankTurret,
        ConcreteRubble,
        UIHealthBar,
        WallHorizontal,
        WallVertical,
        WallDamagedEast,
        WallDamagedWest,
        WallDamagedNorth,
        WallDamagedSouth,
        WallDamagedFullHorizontal,
        WallDamagedFullVertical,
        TargetBuilding
    }
}