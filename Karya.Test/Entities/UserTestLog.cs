using Karya.Core.Abstracts.Entities;
using Karya.Core.Interfaces.Entities;
using Karya.Core.Interfaces.Entities.Tanent;
using System.ComponentModel.DataAnnotations;

namespace Karya.Test.Entities;

public class UserTestLog : BaseTanentEntity
{
    public DateTime LogDate { get; set; }
    public string Description { get; set; }
  
}