/*  CollisionType.cs
 *  The type of collision
 *  
 *  Revision History:
 *      Ryan Beausoleil, 2018.12.01: Created
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomXna.Framework
{
    public enum CollisionType
    {
        CollisionEnter,
        CollisionStay,
        CollisionExit
    }
}