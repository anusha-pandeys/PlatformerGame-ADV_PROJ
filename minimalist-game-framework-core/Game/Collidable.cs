using System;
using System.Collections.Generic;
using System.Text;

public interface ICollidable : IDisposable, Entity
{ 
    Entity entity { get;  }
    
}
internal class Collidable
{

}
