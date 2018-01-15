using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace refactor_me.Models
{
  public abstract class BusinessListBase<T,C>
    where C : BusinessBase<C>
  {
    public virtual void Load()
    {
      
    }

  }
}