using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unscriptable.Domain.Entitties;

public class Student
{
    public int Id { get; set; }
    public string Phone { get; set; } = null!;
    public string VkProfileLink { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    
    public List<Group> Groups { get; set; } = new();

    public List<Attempt> Attempts { get; set; } = new();
}